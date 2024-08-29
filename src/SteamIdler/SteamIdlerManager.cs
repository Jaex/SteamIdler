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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SteamIdler
{
    public class SteamIdlerManager : IDisposable
    {
        public List<long> AppIDs { get; private set; }
        public List<Process> Processes { get; private set; } = new List<Process>();

        public SteamIdlerManager()
        {
        }

        public bool RunApps()
        {
            CloseApps();

            if (AppIDs != null && AppIDs.Count > 0)
            {
                if (!SteamAPI.IsSteamRunning())
                {
                    // Wait for Steam to launch
                    for (int i = 0; i < 60 && !SteamAPI.IsSteamRunning(); i++)
                    {
                        Thread.Sleep(1000);
                    }

                    if (SteamAPI.IsSteamRunning())
                    {
                        // Even "IsSteamRunning" is true still Steam API init can fail, therefore need to give more time for Steam to launch
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        return false;
                    }
                }

                for (int i = 0; i < AppIDs.Count; i++)
                {
                    long appID = AppIDs[i];
                    Process process = Process.Start(Application.ExecutablePath, "-AppID " + appID);
                    Processes.Add(process);
                }

                return true;
            }

            return false;
        }

        public void CloseApps()
        {
            if (Processes.Count > 0)
            {
                foreach (Process process in Processes)
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }

                    process.Dispose();
                }

                Processes.Clear();
            }
        }

        public void LoadAppIDs(string filePath)
        {
            List<long> appIDs = new List<long>();

            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (!string.IsNullOrEmpty(line))
                {
                    int index = line.IndexOf("//");

                    if (index > -1)
                    {
                        line = line.Remove(index);
                    }

                    line = line.Trim();

                    if (long.TryParse(line, out long appID))
                    {
                        appIDs.Add(appID);
                    }
                }
            }

            AppIDs = appIDs;
        }

        public void Dispose()
        {
            CloseApps();
        }
    }
}