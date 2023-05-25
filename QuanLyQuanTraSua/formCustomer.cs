using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.Core;

namespace QuanLyQuanTraSua
{
    public partial class formCustomer : Form
    {
        public formCustomer()
        {
            InitializeComponent();
        }

        private void formCustomer_Load(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void LoadCustomer()
        {
            using(TeaEntities et = new TeaEntities())
            {
                dgvCustomers.DataSource = (from c in et.Customer.Where(c => c.isHide == false) select new {ID = c.id, Name = c.name, Address = c.address, Phone = c.phoneNumber, Discount = c.discount}).ToList();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (TeaEntities te = new TeaEntities())
            {
                if (!String.IsNullOrEmpty(txtCustomerName.Text) || !String.IsNullOrEmpty(txtCustomerAddress.Text) || !String.IsNullOrEmpty(txtCustomerNumberPhone.Text))
                {
                    try
                    {
                        te.Customer.Add(new Customer()
                        {
                            name = txtCustomerName.Text,
                            address = txtCustomerAddress.Text,
                            phoneNumber = txtCustomerNumberPhone.Text,
                            discount = !String.IsNullOrEmpty(txtDiscount.Text) ? Convert.ToInt32(txtDiscount.Text) : 0,
                            isHide = false
                        });
                        te.SaveChanges();
                        MessageBox.Show("Thêm thông tin khách hàng mới thành công", "Thông báo thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadCustomer();
                }
                else
                {
                    MessageBox.Show("Không được để trống thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCustomerID.Text = dgvCustomers.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtCustomerName.Text = dgvCustomers.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtCustomerAddress.Text = dgvCustomers.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtCustomerNumberPhone.Text = dgvCustomers.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtDiscount.Text = dgvCustomers.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var tb = new TeaEntities())
            {
                try
                {
                    int idCus = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells[0].Value.ToString());
                    var update = tb.Customer.Find(idCus);
                    if (update != null)
                    {
                        update.name= txtCustomerName.Text;
                        update.address= txtCustomerAddress.Text;
                        update.phoneNumber= txtCustomerNumberPhone.Text;
                        update.discount = Convert.ToInt32(txtDiscount.Text);
                    }
                    tb.SaveChanges();
                    MessageBox.Show("Cập nhật dữ liệu thành công", "thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cập nhật dữ liệu thất bại" + ex, "Quản lý quán trà sữa ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadCustomer();
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
                dgvCustomers.DataSource = (from c in entities.Customer.Where(t => t.name.Contains(name)) select new { ID = c.id, Name = c.name, Address = c.address, Phone = c.phoneNumber }).ToList();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (TeaEntities te = new TeaEntities())
            {
                if (dgvCustomers.SelectedRows.Count > 0)
                {
                    try
                    {
                        int id = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells[0].Value.ToString());
                        var update = te.Customer.Find(id);
                        if (update != null)
                        {
                            update.isHide = true;
                        }
                        te.SaveChanges();
                        MessageBox.Show("Xoá khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Xoá khách hàng thất bại: " + ex, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    LoadCustomer();
                }
                else
                {
                    MessageBox.Show("Chọn khách hàng cần xoá trước !!!: ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
    }
}
