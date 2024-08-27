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

using SteamIdler.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SteamIdler
{
    public partial class EditForm : Form
    {
        public string FilePath { get; private set; }

        public EditForm(string filePath)
        {
            FilePath = filePath;

            InitializeComponent();
            Icon = Resources.SteamIdler_Icon;

            ApplyTheme(AppTheme.DarkTheme);

            if (File.Exists(FilePath))
            {
                rtbAppIDs.Text = File.ReadAllText(FilePath, Encoding.UTF8);
                rtbAppIDs.SelectionStart = rtbAppIDs.TextLength;
                SyntaxHighlight(rtbAppIDs);
            }
        }

        private void ApplyTheme(AppTheme theme)
        {
            BackColor = theme.BackgroundColor;

            pAppIDs.BackColor = theme.LightBackgroundColor;

            rtbAppIDs.BackColor = theme.LightBackgroundColor;
            rtbAppIDs.ForeColor = theme.TextColor;

            btnOK.BackColor = theme.LightBackgroundColor;
            btnOK.ForeColor = theme.TextColor;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.FlatAppearance.BorderSize = 0;

            btnCancel.BackColor = theme.LightBackgroundColor;
            btnCancel.ForeColor = theme.TextColor;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
        }

        private void SyntaxHighlight(RichTextBox rtb)
        {
            if (rtb.TextLength > 0)
            {
                Color colorComment = Color.FromArgb(87, 166, 74);

                rtb.BeginUpdate();
                int originalSelectionStart = rtb.SelectionStart;
                int originalSelectionLength = rtb.SelectionLength;

                rtb.SelectAll();
                rtb.SelectionColor = rtb.ForeColor;

                Regex regex = new Regex("//.*", RegexOptions.Compiled);

                foreach (Match match in regex.Matches(rtb.Text))
                {
                    rtb.Select(match.Index, match.Length);
                    rtb.SelectionColor = colorComment;
                }

                rtb.Select(originalSelectionStart, originalSelectionLength);
                rtb.EndUpdate();
            }
        }

        private void rtbAppIDs_TextChanged(object sender, EventArgs e)
        {
            SyntaxHighlight(rtbAppIDs);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(FilePath, rtbAppIDs.Text, Encoding.UTF8);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                DialogResult = DialogResult.Cancel;

                MessageBox.Show(ex.Message, "Steam Idler - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }
    }
}