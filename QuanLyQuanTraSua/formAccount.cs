using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyQuanTraSua
{
    public partial class formAccount : Form
    {
        public formAccount()
        {
            InitializeComponent();
        }

        private void LoadAccount()
        {
            using (TeaEntities et = new TeaEntities())
            {
                dgvNV.DataSource = (from c in et.Account.Where(x => x.isHide == false) select new { ID = c.id, Username = c.userName, Password = c.passWord, Name = c.displayName, Address = c.address, Phone = c.phone, Sex = c.sex, Role = (c.accountType == 1) ? "Quản lý" : "Nhân viên", Avatar = c.avatar }).ToList();
            }
        }

        private void formAccount_Load(object sender, EventArgs e)
        {
            LoadAccount();
            LoadSettings();
        }

        private void LoadSettings()
        {
            dgvNV.Columns["Password"].Visible = false;
            dgvNV.Columns["Avatar"].Visible = false;
        }

        private void dgvNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtStaffID.Text = dgvNV.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtUsername.Text = dgvNV.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtPassword.Text = dgvNV.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtStaffName.Text = dgvNV.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtStaffAddress.Text = dgvNV.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtStaffPhone.Text = dgvNV.Rows[e.RowIndex].Cells[5].Value.ToString();
            cbSex.SelectedIndex = (dgvNV.Rows[e.RowIndex].Cells[6].Value.ToString() == "Nam") ? 0 : 1;
            cbRole.SelectedIndex = (dgvNV.Rows[e.RowIndex].Cells[7].Value.ToString() == "Nhân viên") ? 0 : 1;

            var isAvatar = dgvNV.Rows[e.RowIndex].Cells["Avatar"].Value;
            if (!(isAvatar == null))
            {
                pictureBox1.Image = byteArrayToImage((byte[])isAvatar);
            }
            else
            {
                pictureBox1.Image = null;
            }

        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (TeaEntities teaE = new TeaEntities())
            {
                try
                {
                    teaE.Account.Add(new Account()
                    {
                        userName = txtUsername.Text,
                        passWord = txtPassword.Text,
                        displayName = txtStaffName.Text,
                        address = txtStaffAddress.Text,
                        phone = txtStaffPhone.Text,
                        sex = cbSex.Text,
                        accountType = cbRole.SelectedIndex,
                        avatar = (pictureBox1.Image != null) ? imageToByteArray(pictureBox1.Image) : null,
                        isHide = false
                    });
                    teaE.SaveChanges();
                    MessageBox.Show("Thêm nhân viên thành công","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi:" + ex,"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadAccount();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box  
                pictureBox1.Image = new Bitmap(open.FileName);
                // image file path  
                btnUploadAvatar.Text = open.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (TeaEntities te = new TeaEntities())
            {
                if (dgvNV.SelectedRows.Count > 0)
                {
                    try
                    {
                        int idAccount = Convert.ToInt32(dgvNV.SelectedRows[0].Cells[0].Value.ToString());
                        var update = te.Account.Find(idAccount);
                        if (update != null)
                        {
                            update.isHide = true;
                        }
                        te.SaveChanges();
                        MessageBox.Show("Xoá nhân viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Xoá nhân viên thất bại: " + ex, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    LoadAccount();
                }
                else
                {
                    MessageBox.Show("Chọn nhân viên cần cập nhật thông tin !!!: ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (TeaEntities te = new TeaEntities())
            {
                if (dgvNV.SelectedRows.Count > 0)
                {
                    try
                    {
                        int idAccount = Convert.ToInt32(dgvNV.SelectedRows[0].Cells[0].Value.ToString());
                        var update = te.Account.Find(idAccount);
                        if (update != null)
                        {
                            update.userName = txtUsername.Text;
                            update.passWord = txtPassword.Text;
                            update.displayName = txtStaffName.Text;
                            update.address = txtStaffAddress.Text;
                            update.phone = txtStaffPhone.Text;
                            update.sex = cbSex.Text;
                            update.accountType = (cbRole.Text == "Nhân viên") ? 0 : 1;
                            update.avatar = (pictureBox1.Image != null) ? imageToByteArray(pictureBox1.Image) : null;
                        }
                        te.SaveChanges();
                        MessageBox.Show("Cập nhật thông tin nhân viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Cập nhật thông tin nhân viên thất bại: " + ex, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    LoadAccount();
                }
                else
                {
                    MessageBox.Show("Chọn nhân viên cần cập nhật thông tin !!!: ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            TimKiem(txtSearch.Text.Trim());
        }

        private void TimKiem(string name)
        {
            using (TeaEntities entities = new TeaEntities())
            {
                dgvNV.DataSource = (from c in entities.Account.Where(c => c.isHide == false) select new { ID = c.id, Username = c.userName, Password = c.passWord, Name = c.displayName, Address = c.address, Phone = c.phone, Sex = c.sex, Role = (c.accountType == 1) ? "Quản lý" : "Nhân viên", Avatar = c.avatar }).ToList();
            }
        }
    }
}
