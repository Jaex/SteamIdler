﻿#region License Information (GPL v3)

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

using System.Drawing;

namespace SteamIdler
{
    public class AppTheme
    {
        public Color BackgroundColor { get; set; }
        public Color LightBackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public Font TextFont { get; set; }
        public Color MenuHighlightColor { get; set; }
        public Color MenuHighlightBorderColor { get; set; }
        public Color MenuBorderColor { get; set; }
        public Color MenuCheckBackgroundColor { get; set; }
        public Color SeparatorLightColor { get; set; }
        public Color SeparatorDarkColor { get; set; }

        public static AppTheme DarkTheme { get; private set; } = new AppTheme()
        {
            BackgroundColor = Color.FromArgb(39, 39, 39),
            LightBackgroundColor = Color.FromArgb(46, 46, 46),
            TextColor = Color.FromArgb(231, 233, 234),
            TextFont = new Font("Segoe UI", 9.75f),
            MenuHighlightColor = Color.FromArgb(46, 46, 46),
            MenuHighlightBorderColor = Color.FromArgb(63, 63, 63),
            MenuBorderColor = Color.FromArgb(63, 63, 63),
            MenuCheckBackgroundColor = Color.FromArgb(51, 51, 51),
            SeparatorLightColor = Color.FromArgb(44, 44, 44),
            SeparatorDarkColor = Color.FromArgb(31, 31, 31)
        };
    }
}