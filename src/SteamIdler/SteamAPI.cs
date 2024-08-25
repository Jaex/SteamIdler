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
using System.Runtime.InteropServices;

namespace SteamIdler
{
    public static class SteamAPI
    {
        private const string LibraryName = "steam_api";

        /// <summary>
        /// Initialize the Steamworks SDK.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = "SteamAPI_Init", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool Init();

        public static bool Init(long appID)
        {
            using (MutexManager mutex = new MutexManager("SteamAPI_Init_Mutex"))
            {
                Environment.SetEnvironmentVariable("SteamAppId", appID.ToString());

                return Init();
            }
        }

        /// <summary>
        /// SteamAPI_Shutdown should be called during process shutdown if possible.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = "SteamAPI_Shutdown", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Shutdown();

        /// <summary>
        /// SteamAPI_IsSteamRunning() returns true if Steam is currently running.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = "SteamAPI_IsSteamRunning", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool IsSteamRunning();
    }
}