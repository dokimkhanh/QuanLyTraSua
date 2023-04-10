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
                dataGridView1.DataSource = (from c in et.Customer select new {ID = c.id, Name = c.name, Address = c.address, Phone = c.phoneNumber}).ToList();
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
                            phoneNumber = txtCustomerNumberPhone.Text
                        });
                        te.SaveChanges();
                        MessageBox.Show("Thêm thông tin khách hàng mới thành công", "Thông báo thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadCustomer();
                }
                else
                {
                    MessageBox.Show("Không được để trống thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCustomerID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtCustomerName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtCustomerAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtCustomerNumberPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            TimKiem(txtSearch.Text.Trim());
        }

        private void TimKiem(string name)
        {
            using (TeaEntities entities = new TeaEntities())
            {
                dataGridView1.DataSource = (from c in entities.Customer.Where(t => t.name.Contains(name)) select new { ID = c.id, Name = c.name, Address = c.address, Phone = c.phoneNumber }).ToList();
            }
        }
    }
}
