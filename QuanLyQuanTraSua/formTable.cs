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
    public partial class formTable : Form
    {
        public formTable()
        {
            InitializeComponent();
        }

        private void LoadFormSettings()
        {
            
        }
        private void LoadTable()
        {
            LoadFormSettings();
            using (TeaEntities te = new TeaEntities())
            {
                dgvTable.DataSource = (from c in te.TableTea select new { ID = c.id, TableName = c.name}).ToList();
            }
            
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            using (var tb = new TeaEntities())
            {
                try
                {
                    tb.TableTea.Add(new TableTea()
                    {
                        name = txtTableName.Text,
                        status = "Trống",
                    });

                    tb.SaveChanges();
                    MessageBox.Show("Thêm bàn mới thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadTable();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            using (var tb = new TeaEntities())
            {
                try
                {
                    int idban = Convert.ToInt32(dgvTable.SelectedRows[0].Cells[0].Value.ToString());
                    var update = tb.TableTea.Find(idban);
                    if (update != null)
                    {
                        update.name = txtTableName.Text;
                    }
                    tb.SaveChanges();
                    MessageBox.Show("Cập nhật dữ liệu thành công", "thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cập nhật dữ liệu thất bại" + ex, "Quản lý quán trà sữa ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadTable();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            using (var tb = new TeaEntities())
            {
                if (dgvTable.SelectedRows.Count > 0)
                {
                    try
                    {
                        int idban = Convert.ToInt32(dgvTable.SelectedRows[0].Cells[0].Value);
                        tb.TableTea.Remove(tb.TableTea.Where(p => p.id == idban).SingleOrDefault());
                        tb.SaveChanges();
                        MessageBox.Show("Xoá bàn thành thành công", "thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Xoá bàn thất bại do bàn đang có người ngồi" + ex, "Quản lý quán trà sữa ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadTable();
                }
                else { MessageBox.Show("Chưa chọn bàn cần xoá","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Question);}
            }
        }

        private void formTable_Load(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void dgvTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTableID.Text = dgvTable.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtTableName.Text = dgvTable.Rows[e.RowIndex].Cells[1].Value.ToString();
            
        }

        private void TimKiemBan(string name)
        {
            using (TeaEntities entities = new TeaEntities())
            {
                dgvTable.DataSource = (from t in entities.TableTea.Where(t => t.name.Contains(name)) select new {ID = t.id, Name = t.name} ).ToList();
            }
        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {
            TimKiemBan(txtSearch.Text.Trim());
        }
    }
}
