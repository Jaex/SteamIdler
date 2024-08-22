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
using System.IO;
using System.Text;
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

            if (File.Exists(FilePath))
            {
                txtAppIDs.Text = File.ReadAllText(FilePath, Encoding.UTF8);
                txtAppIDs.SelectionStart = 0;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(FilePath, txtAppIDs.Text, Encoding.UTF8);

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