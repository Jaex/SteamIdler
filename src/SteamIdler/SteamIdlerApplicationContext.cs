#region License Information (GPL v3)

/*
    Copyright (c) Jaex

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using Microsoft.Win32;
using SteamIdler.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SteamIdler
{
    public class SteamIdlerApplicationContext : ApplicationContext
    {
        public string AppIDsFilePath { get; private set; }

        private SteamIdlerManager steamIdlerManager;
        private NotifyIcon niMain;
        private ContextMenuStrip cmsMain;
        private ToolStripMenuItem tsmiEdit, tsmiStartup, tsmiGitHub, tsmiExit;

        public SteamIdlerApplicationContext()
        {
            cmsMain = new ContextMenuStrip();
            cmsMain.Renderer = new ToolStripDarkRenderer();
            cmsMain.Font = AppTheme.DarkTheme.TextFont;

            tsmiEdit = new ToolStripMenuItem("Edit App IDs...");
            tsmiEdit.Click += tsmiEdit_Click;
            cmsMain.Items.Add(tsmiEdit);

            tsmiStartup = new ToolStripMenuItem("Start with Windows");
            tsmiStartup.Checked = CheckStartup();
            tsmiStartup.Click += tsmiStartup_Click;
            cmsMain.Items.Add(tsmiStartup);

            tsmiGitHub = new ToolStripMenuItem("Open GitHub page...");
            tsmiGitHub.Click += tsmiGitHub_Click;
            cmsMain.Items.Add(tsmiGitHub);

            cmsMain.Items.Add(new ToolStripSeparator());

            tsmiExit = new ToolStripMenuItem("Exit");
            tsmiExit.Click += tsmiExit_Click;
            cmsMain.Items.Add(tsmiExit);

            niMain = new NotifyIcon();
            niMain.Text = Application.ProductName + " " + Application.ProductVersion;
            niMain.Icon = Resources.SteamIdler_Icon;
            niMain.ContextMenuStrip = cmsMain;
            niMain.MouseDoubleClick += trayIcon_MouseDoubleClick;
            niMain.Visible = true;

            /*
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SteamIdler");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            */

            string folderPath = AppDomain.CurrentDomain.BaseDirectory;
            AppIDsFilePath = Path.Combine(folderPath, "AppIDs.txt");

            steamIdlerManager = new SteamIdlerManager();

            if (File.Exists(AppIDsFilePath))
            {
                RunApps();
            }
            else
            {
                niMain.ShowBalloonTip(5000, null, "Steam Idler app is running in the system tray.\r\n\r\nMake sure to configure App IDs from tray icon.", ToolTipIcon.None);
            }
        }

        private void RunApps()
        {
            try
            {
                if (File.Exists(AppIDsFilePath))
                {
                    steamIdlerManager.LoadAppIDs(AppIDsFilePath);
                    steamIdlerManager.RunApps();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Steam Idler - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenEditForm()
        {
            using (EditForm editForm = new EditForm(AppIDsFilePath))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    RunApps();
                }
            }
        }

        private void SetStartup(bool enable)
        {
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (enable)
                {
                    string applicationPath = $"\"{Application.ExecutablePath}\"";
                    rk.SetValue("SteamIdler", applicationPath);
                }
                else
                {
                    rk.DeleteValue("SteamIdler", false);
                }
            }
        }

        private bool CheckStartup()
        {
            try
            {
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run"))
                {
                    return rk.GetValue("SteamIdler") != null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Steam Idler - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenEditForm();
        }

        private void tsmiEdit_Click(object sender, EventArgs e)
        {
            OpenEditForm();
        }

        private void tsmiStartup_Click(object sender, EventArgs e)
        {
            bool enable = !tsmiStartup.Checked;

            try
            {
                SetStartup(enable);

                tsmiStartup.Checked = enable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Steam Idler - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsmiGitHub_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://github.com/Jaex/SteamIdler");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Steam Idler - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            niMain.Visible = false;

            try
            {
                steamIdlerManager?.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Steam Idler - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Application.Exit();
        }
    }
}