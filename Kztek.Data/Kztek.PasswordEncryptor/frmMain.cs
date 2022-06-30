using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kztek.Security;

namespace Kztek.PasswordEncryptor
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDecrypted.Text) && !string.IsNullOrEmpty(txtKeypass.Text))
            {
                if(txtKeypass.Text.Length < 12)
                {
                    MessageBox.Show("Độ dài keypass tối thiểu là 12 kí tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var keypass = chkDLL.Checked ? SecurityModel.Keypass : txtKeypass.Text;
                txtEncrypted.Text = CryptoProvider.SimpleEncryptWithPassword(txtDecrypted.Text, keypass);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                var keypass = chkDLL.Checked ? SecurityModel.Keypass : txtKeypass.Text;
                txtDecrypted.Text = CryptoProvider.SimpleDecryptWithPassword(txtEncrypted.Text, keypass);
            }
            catch
            {
                MessageBox.Show("Chuỗi mã hóa không hợp lệ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void chkDLL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDLL.Checked)
                txtKeypass.Enabled = false;
            else
                txtKeypass.Enabled = true;
        }
    }
}
