using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kztek.Access.LoaderApp
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void ReadSetting()
        {
            chkSave.Checked = Properties.Settings.Default.Remember;
            if (chkSave.Checked)
            {
                txtID.Text = Properties.Settings.Default.ID;
            }
        }

        private void SaveSetting()
        {
            Properties.Settings.Default.Remember = chkSave.Checked;
            if (chkSave.Checked)
            {
                Properties.Settings.Default.ID = txtID.Text;
            }
            else
            {
                Properties.Settings.Default.ID = "";
            }
            Properties.Settings.Default.Save();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var user = new User() { Username = txtID.Text, Password = txtPW.Text };

            var _user = new DataAccess().CheckLogin(user);

            if (_user != null)
            {
                FunctionHelper.CurrentUser = _user;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Đăng nhập không hợp lệ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            ReadSetting();
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSetting();
        }
    }
}
