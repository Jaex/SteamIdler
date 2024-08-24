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
using System.IO;
using System.Windows.Forms;

namespace SteamIdler
{
    public class SteamIdlerApplicationContext : ApplicationContext
    {
        public string AppIDsFilePath { get; private set; }

        private SteamIdlerManager steamIdlerManager;
        private NotifyIcon trayIcon;
        private ContextMenuStrip cmsMain;
        private ToolStripMenuItem tsmiEdit, tsmiStartup, tsmiExit;

        public SteamIdlerApplicationContext()
        {
            cmsMain = new ContextMenuStrip();
            cmsMain.Renderer = new ToolStripDarkRenderer();
            cmsMain.Font = AppTheme.DarkTheme.TextFont;

            tsmiEdit = new ToolStripMenuItem("Edit App IDs...", null, tsmiEdit_Click);
            cmsMain.Items.Add(tsmiEdit);

            tsmiStartup = new ToolStripMenuItem("Start with Windows", null, tsmiStartup_Click);
            tsmiStartup.Checked = CheckStartup();
            cmsMain.Items.Add(tsmiStartup);

            tsmiExit = new ToolStripMenuItem("Exit", null, tsmiExit_Click);
            cmsMain.Items.Add(tsmiExit);

            trayIcon = new NotifyIcon();
            trayIcon.Text = Application.ProductName + " " + Application.ProductVersion;
            trayIcon.Icon = Resources.SteamIdler_Icon;
            trayIcon.ContextMenuStrip = cmsMain;
            trayIcon.MouseDoubleClick += trayIcon_MouseDoubleClick;
            trayIcon.Visible = true;

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

            RunApps();
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

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

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