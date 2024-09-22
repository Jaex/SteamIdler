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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamIdler
{
    public class SteamIdlerApplicationContext : ApplicationContext
    {
        public string AppIDsFilePath { get; private set; }

        private SteamIdlerManager steamIdlerManager;
        private NotifyIcon niMain;
        private ContextMenuStrip cmsMain;
        private ToolStripMenuItem tsmiStart, tsmiStop, tsmiEdit, tsmiStartup, tsmiGitHub, tsmiExit;
        private bool exiting;

        public SteamIdlerApplicationContext()
        {
            cmsMain = new ContextMenuStrip();
            cmsMain.Renderer = new ToolStripDarkRenderer();
            cmsMain.Font = AppTheme.DarkTheme.TextFont;

            tsmiStart = new ToolStripMenuItem("Start Idling");
            tsmiStart.Click += tsmiStart_Click;
            cmsMain.Items.Add(tsmiStart);

            tsmiStop = new ToolStripMenuItem("Stop Idling");
            tsmiStop.Click += tsmiStop_Click;
            cmsMain.Items.Add(tsmiStop);

            tsmiEdit = new ToolStripMenuItem("Edit App IDs...");
            tsmiEdit.Click += tsmiEdit_Click;
            cmsMain.Items.Add(tsmiEdit);

            tsmiStartup = new ToolStripMenuItem("Start with Windows");
            tsmiStartup.Click += tsmiStartup_Click;
            cmsMain.Items.Add(tsmiStartup);

            tsmiGitHub = new ToolStripMenuItem("Open GitHub Page...");
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

            tsmiStartup.Checked = CheckStartup();

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

            UpdateControls();

            InitAsync();
        }

        private async void InitAsync()
        {
            if (Program.WaitSteam > 0)
            {
                await StartApps(Program.WaitSteam, true);
            }
            else if (File.Exists(AppIDsFilePath))
            {
                await StartApps();
            }
            else
            {
                niMain.ShowBalloonTip(5000, null, "Steam Idler app is running in the system tray.\r\n\r\nMake sure to configure App IDs from tray icon.", ToolTipIcon.None);
            }
        }

        private void UpdateControls()
        {
            if (!exiting)
            {
                tsmiStart.Visible = !steamIdlerManager.IsRunning;
                tsmiStop.Visible = steamIdlerManager.IsRunning;
                tsmiStart.Enabled = tsmiStop.Enabled = tsmiEdit.Enabled = true;
            }
        }

        private void ShowError(Exception e)
        {
            if (!exiting)
            {
                niMain.ShowBalloonTip(5000, "Error", e.Message, ToolTipIcon.Error);
            }
        }

        private async Task StartApps(int waitSteam = 0, bool silent = false)
        {
            try
            {
                steamIdlerManager.LoadAppIDs(AppIDsFilePath);

                if (steamIdlerManager.AppIDs != null && steamIdlerManager.AppIDs.Count > 0)
                {
                    tsmiStart.Enabled = tsmiStop.Enabled = tsmiEdit.Enabled = false;

                    bool isSteamRunning = await steamIdlerManager.WaitSteam(waitSteam);

                    if (!exiting)
                    {
                        if (!isSteamRunning)
                        {
                            throw new Exception("Steam is not running.");
                        }

                        steamIdlerManager.RunApps();
                    }
                }
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    ShowError(e);
                }
            }
            finally
            {
                UpdateControls();
            }
        }

        private void StopApps()
        {
            try
            {
                steamIdlerManager.CloseApps();
            }
            catch (Exception e)
            {
                ShowError(e);
            }
            finally
            {
                UpdateControls();
            }
        }

        private async Task OpenEditForm()
        {
            using (EditForm editForm = new EditForm(AppIDsFilePath))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    await StartApps();
                }
            }
        }

        private void SetStartup(bool enable)
        {
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (enable)
                {
                    string applicationPath = $"\"{Application.ExecutablePath}\" -WaitSteam 60";
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
                ShowError(e);
            }

            return false;
        }

        private async void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            await OpenEditForm();
        }

        private async void tsmiStart_Click(object sender, EventArgs e)
        {
            await StartApps();
        }

        private void tsmiStop_Click(object sender, EventArgs e)
        {
            StopApps();
        }

        private async void tsmiEdit_Click(object sender, EventArgs e)
        {
            await OpenEditForm();
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
                ShowError(ex);
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
                ShowError(ex);
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            exiting = true;

            // Hide tray icon, otherwise it will remain shown until user mouses over it.
            niMain.Visible = false;

            try
            {
                steamIdlerManager?.Dispose();
            }
            catch
            {
            }

            Application.Exit();
        }
    }
}