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

using System.Drawing;
using System.Windows.Forms;

namespace SteamIdler
{
    public class DarkColorTable : ProfessionalColorTable
    {
        public override Color ButtonSelectedHighlight => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color ButtonSelectedHighlightBorder => AppTheme.DarkTheme.MenuHighlightBorderColor;
        public override Color ButtonPressedHighlight => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color ButtonPressedHighlightBorder => AppTheme.DarkTheme.MenuHighlightBorderColor;
        public override Color ButtonCheckedHighlight => AppTheme.DarkTheme.MenuCheckBackgroundColor;
        public override Color ButtonCheckedHighlightBorder => AppTheme.DarkTheme.MenuHighlightBorderColor;
        public override Color ButtonPressedBorder => AppTheme.DarkTheme.MenuHighlightBorderColor;
        public override Color ButtonSelectedBorder => AppTheme.DarkTheme.MenuHighlightBorderColor;
        public override Color ButtonCheckedGradientBegin => AppTheme.DarkTheme.MenuCheckBackgroundColor;
        public override Color ButtonCheckedGradientMiddle => AppTheme.DarkTheme.MenuCheckBackgroundColor;
        public override Color ButtonCheckedGradientEnd => AppTheme.DarkTheme.MenuCheckBackgroundColor;
        public override Color ButtonSelectedGradientBegin => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color ButtonSelectedGradientMiddle => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color ButtonSelectedGradientEnd => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color ButtonPressedGradientBegin => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color ButtonPressedGradientMiddle => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color ButtonPressedGradientEnd => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color CheckBackground => AppTheme.DarkTheme.MenuCheckBackgroundColor;
        public override Color CheckSelectedBackground => AppTheme.DarkTheme.MenuCheckBackgroundColor;
        public override Color CheckPressedBackground => AppTheme.DarkTheme.MenuCheckBackgroundColor;
        public override Color GripDark => AppTheme.DarkTheme.SeparatorDarkColor;
        public override Color GripLight => AppTheme.DarkTheme.SeparatorLightColor;
        public override Color ImageMarginGradientBegin => AppTheme.DarkTheme.BackgroundColor;
        public override Color ImageMarginGradientMiddle => AppTheme.DarkTheme.BackgroundColor;
        public override Color ImageMarginGradientEnd => AppTheme.DarkTheme.BackgroundColor;
        public override Color ImageMarginRevealedGradientBegin => AppTheme.DarkTheme.BackgroundColor;
        public override Color ImageMarginRevealedGradientMiddle => AppTheme.DarkTheme.BackgroundColor;
        public override Color ImageMarginRevealedGradientEnd => AppTheme.DarkTheme.BackgroundColor;
        public override Color MenuStripGradientBegin => AppTheme.DarkTheme.BackgroundColor;
        public override Color MenuStripGradientEnd => AppTheme.DarkTheme.BackgroundColor;
        public override Color MenuItemSelected => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color MenuItemBorder => AppTheme.DarkTheme.BackgroundColor;
        public override Color MenuBorder => AppTheme.DarkTheme.BackgroundColor;
        public override Color MenuItemSelectedGradientBegin => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color MenuItemSelectedGradientEnd => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color MenuItemPressedGradientBegin => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color MenuItemPressedGradientMiddle => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color MenuItemPressedGradientEnd => AppTheme.DarkTheme.MenuHighlightColor;
        public override Color RaftingContainerGradientBegin => AppTheme.DarkTheme.BackgroundColor;
        public override Color RaftingContainerGradientEnd => AppTheme.DarkTheme.BackgroundColor;
        public override Color SeparatorDark => AppTheme.DarkTheme.SeparatorDarkColor;
        public override Color SeparatorLight => AppTheme.DarkTheme.SeparatorLightColor;
        public override Color StatusStripGradientBegin => AppTheme.DarkTheme.BackgroundColor;
        public override Color StatusStripGradientEnd => AppTheme.DarkTheme.BackgroundColor;
        public override Color ToolStripBorder => AppTheme.DarkTheme.BackgroundColor;
        public override Color ToolStripDropDownBackground => AppTheme.DarkTheme.BackgroundColor;
        public override Color ToolStripGradientBegin => AppTheme.DarkTheme.BackgroundColor;
        public override Color ToolStripGradientMiddle => AppTheme.DarkTheme.BackgroundColor;
        public override Color ToolStripGradientEnd => AppTheme.DarkTheme.BackgroundColor;
        public override Color ToolStripContentPanelGradientBegin => AppTheme.DarkTheme.BackgroundColor;
        public override Color ToolStripContentPanelGradientEnd => AppTheme.DarkTheme.BackgroundColor;
        public override Color ToolStripPanelGradientBegin => AppTheme.DarkTheme.BackgroundColor;
        public override Color ToolStripPanelGradientEnd => AppTheme.DarkTheme.BackgroundColor;
        public override Color OverflowButtonGradientBegin => AppTheme.DarkTheme.BackgroundColor;
        public override Color OverflowButtonGradientMiddle => AppTheme.DarkTheme.BackgroundColor;
        public override Color OverflowButtonGradientEnd => AppTheme.DarkTheme.BackgroundColor;
    }
}