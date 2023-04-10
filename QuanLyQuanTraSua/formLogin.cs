using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanTraSua
{
    public partial class formLogin : Form
    {
        public formLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (TeaEntities teaEntities = new TeaEntities())
            {
                try
                {
                    var result = (from u in teaEntities.Account where (u.userName == txtUsername.Text && u.passWord == txtPassword.Text) select new { data = u }).FirstOrDefault();
                    if (result != null)
                    {
                        MessageBox.Show($"Chào mừng, {result.data.displayName}","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        SaveDataAccount(result.data.id, result.data.userName, result.data.passWord);
                        this.Hide();
                        new formMain().Show();
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không tồn tại", "Thông báo đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi đăng nhập");
                }
            }
        }

        private void SaveDataAccount(int id, string user, string pass)
        {
            Properties.Settings.Default.id = id;
            Properties.Settings.Default.user = user;
            Properties.Settings.Default.pass = pass;
        }
    }
}
