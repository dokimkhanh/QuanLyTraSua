using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanTraSua
{
    public partial class formTea : Form
    {
        public formTea()
        {
            InitializeComponent();
        }

        private void LoadTea()
        {
            using (TeaEntities et = new TeaEntities())
            {
                dgvTea.DataSource = (from c in et.Tea select new { ID = c.id, Name = c.name, Price = c.price, Image = c.img }).ToList();
            }
        }

        private void LoadSettings()
        {
            dgvTea.Columns["Image"].Visible = false;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            using (TeaEntities teaE = new TeaEntities())
            {
                try
                {
                    teaE.Tea.Add(new Tea()
                    {
                        name = txtName.Text,
                        price = Convert.ToInt32(txtPrice.Text),
                        img = TeaHelper.imageToByteArray(pictureBox1.Image),
                    });
                    teaE.SaveChanges();
                    MessageBox.Show("Thêm thông tin trà sữa mới thành công","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi:" + ex, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadTea();
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box  
                pictureBox1.Image = new Bitmap(open.FileName);
                // image file path  
                btnUpload.Text = open.SafeFileName;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (TeaEntities tea = new TeaEntities())
            {
                try
                {
                    int idTea = Convert.ToInt32(dgvTea.SelectedRows[0].Cells[0].Value.ToString());
                    tea.Tea.Remove(tea.Tea.Where(p => p.id == idTea).SingleOrDefault());
                    tea.SaveChanges();
                    MessageBox.Show("Xoá dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xoá dữ liệu thất bại: " + ex, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadTea();
            }
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            using (TeaEntities te = new TeaEntities())
            {
                if (dgvTea.SelectedRows.Count > 0)
                {
                    try
                    {
                        int idTea = Convert.ToInt32(dgvTea.SelectedRows[0].Cells[0].Value.ToString());
                        var update = te.Tea.Find(idTea);
                        if (update != null)
                        {
                            update.name = txtName.Text;
                            update.price = Convert.ToInt32(txtPrice.Text);
                            update.img = (pictureBox1.Image != null) ? TeaHelper.imageToByteArray(pictureBox1.Image) : null;
                        }
                        te.SaveChanges();
                        MessageBox.Show("Cập nhật thông tin trà sữa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Cập nhật thông tin trà sữa thất bại: " + ex, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    LoadTea();
                }
                else
                {
                    MessageBox.Show("Chọn thông tin loại trà sữa cập nhật thông tin !!!: ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void formTea_Load(object sender, EventArgs e)
        {
            LoadTea();
            LoadSettings();
        }

        private void dgvTea_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTeaID.Text = dgvTea.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvTea.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtPrice.Text = dgvTea.Rows[e.RowIndex].Cells[2].Value.ToString();

            var isImg = dgvTea.Rows[e.RowIndex].Cells["Image"].Value;
            if (!(isImg == null))
            {
                pictureBox1.Image = TeaHelper.byteArrayToImage((byte[])isImg);
            }
            else
            {
                pictureBox1.Image = null;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            TimKiem(txtSearch.Text.Trim());
        }

        private void TimKiem(string name)
        {
            using (TeaEntities entities = new TeaEntities())
            {
                dgvTea.DataSource = (from c in entities.Tea.Where(t => t.name.Contains(name)) select new { ID = c.id, Name = c.name, Price = c.price, Image = c.img }).ToList();
            }
        }
    }


}
