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
using System.Windows.Forms;

namespace SteamIdler
{
    internal static class Program
    {
        /// <summary>
        /// The time to wait for Steam to launch in seconds.
        /// </summary>
        public static int WaitSteam { get; private set; }

        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length >= 2 && args[0].Equals("-AppID", StringComparison.OrdinalIgnoreCase) && int.TryParse(args[1], out int appID))
            {
                if (SteamAPI.Init(appID))
                {
                    Application.Run();
                    SteamAPI.Shutdown();
                }
            }
            else
            {
                using (MutexManager mutex = new MutexManager("d0172a1a-5a5f-4fb3-bf46-a7e3138654f9", 0))
                {
                    if (mutex.HasHandle)
                    {
                        if (args.Length >= 2 && args[0].Equals("-WaitSteam", StringComparison.OrdinalIgnoreCase) && int.TryParse(args[1], out int waitSteam))
                        {
                            WaitSteam = waitSteam;
                        }

                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new SteamIdlerApplicationContext());
                    }
                }
            }
        }
    }
}