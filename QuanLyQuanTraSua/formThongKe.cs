using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanTraSua
{
    public partial class formThongKe : Form
    {
        public formThongKe()
        {
            InitializeComponent();
        }

        private void LoadThongKe()
        {
            using(TeaEntities te = new TeaEntities())
            {
                lbTongSanPham.Text = te.Tea.Count().ToString();
                lbTongBan.Text = te.TableTea.Count().ToString();
                lbTongKhach.Text = te.Customer.Where(x => x.isHide == false).Count().ToString();

                var re = (from _b in te.BillInfo
                                  join _t in te.Tea
                                  on _b.idTea equals _t.id
                                  select new 
                                  {
                                      Price = _t.price, 
                                      Quantity = _b.quantity
                                  }).ToList();
                int tongtien = 0;
                foreach (var item in re)
                {
                    tongtien += item.Price * item.Quantity;
                }
                lbTongTien.Text = tongtien.ToString();

            }
        }

        private void formThongKe_Load(object sender, EventArgs e)
        {
            LoadThongKe();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (TeaEntities te = new TeaEntities())
            {
                var res = (from _a in te.Bill 
                          join _b in te.TableTea on _a.idTable equals _b.id
                          join _c in te.Account on _a.idStaff equals _c.id
                          join _d in te.Customer on _a.idCustomer equals _d.id 
                          where EntityFunctions.TruncateTime(_a.dateCheckIn) >= dateIN.Value && EntityFunctions.TruncateTime(_a.dateCheckOut) <= dateOUT.Value 
                          select new {ID = _a.id, Table = _b.name, Staff = _c.displayName, Khach = _d.name, Vao = _a.dateCheckIn, Ra = _a.dateCheckOut, Status = _a.status});
                foreach (var item in res)
                {
                    dataGridView1.Rows.Add(item.ID, item.Table, item.Staff, item.Khach, item.Vao, item.Ra, item.Status);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Rows.Clear();
            }
            int id = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            using (TeaEntities te = new TeaEntities())
            {
                var res = (from a in te.BillInfo
                           join b in te.Tea on a.idTea equals b.id
                           where a.idBill == id
                           select new { _Tea = b.name, _Price = b.price }).ToList();

                foreach (var item in res)
                {
                    dataGridView2.Rows.Add(item._Tea, item._Price);
                }
            }

            int _tt = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                _tt += Convert.ToInt32(row.Cells[1].Value);
            }

            txtDoanhThu.Text = TeaHelper.ConvertMoney(_tt);

        }
    }
}
