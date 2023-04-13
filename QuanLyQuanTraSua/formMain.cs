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
    public partial class formMain : Form
    {
        private Form currenFormChid;
        public formMain()
        {
            InitializeComponent();
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }


        private void OpenChildForm(Form childForm)
        {
            if (currenFormChid != null)
            {
                currenFormChid.Close();
            }
            currenFormChid = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelForm.Controls.Add(childForm);
            panelForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

        }

        private void LoadInfo()
        {
            var info = ShowAccountInfo();
            lbName.Text = info.displayName;
            lbChucVu.Text = (info.accountType == 1) ? "Quản lý" : "Nhân viên";

            var isAvatar = info.avatar;
            if (!(isAvatar == null))
            {
                picAvatar.Image = TeaHelper.byteArrayToImage((byte[])isAvatar);
            }
            else
            {
                picAvatar.Image = null;
            }

            if (info.accountType == 1)
            {
                menuQLBan.Enabled = true;
                menuQLKhachHang.Enabled = true;
                menuQLNhanVien.Enabled = true;
                menuQLTea.Enabled = true;
            }

        }

        private Account ShowAccountInfo()
        {
            int id = Properties.Settings.Default.id;
            using (TeaEntities te = new TeaEntities())
            {
                var data = (from c in te.Account where (c.id == id) select new { res = c }).FirstOrDefault();
                Account resAcc = new Account()
                {
                    id = data.res.id,
                    displayName = data.res.displayName,
                    accountType = data.res.accountType,
                    avatar = data.res.avatar,
                };
                return resAcc;
            }

        }

        private void formMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void menuQLBan_Click(object sender, EventArgs e)
        {
            OpenChildForm(new formTable());
        }

        private void menuQLTea_Click(object sender, EventArgs e)
        {
            OpenChildForm(new formTea());
        }

        private void menuQLNhanVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(new formAccount());
        }

        private void menuQLKhachHang_Click(object sender, EventArgs e)
        {
            OpenChildForm(new formCustomer());
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new formPayment());
        }

        private void menuQL_Payment_Click(object sender, EventArgs e)
        {
            OpenChildForm(new formPayment());
        }

        private void formMain_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
