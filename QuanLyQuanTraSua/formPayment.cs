using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.Helpers.Transitions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyQuanTraSua
{
    public partial class formPayment : Form
    {
        private int tempTableID;
        private int tempTeaID;
        private int tempBillID;

        public delegate void sendTea(Tea _tea, int idBan, int idBill);
        public formPayment()
        {
            InitializeComponent();
        }

        private void HienThiBan()
        {
            if (flowLayoutPanelBan.Controls.Count > 0)
            {
                flowLayoutPanelBan.Controls.Clear();
            }
            using (TeaEntities entities = new TeaEntities())
            {
                var ban = entities.TableTea.ToList();
                foreach (var item in ban)
                {
                    System.Windows.Forms.Button b = new System.Windows.Forms.Button();
                    b.Size = new Size(90, 90);
                    b.Text = item.name + Environment.NewLine;
                    b.Tag = item;
                    b.TextAlign = ContentAlignment.MiddleCenter;
                    b.Padding = new Padding(0, 0, 0, 5);
                    b.BackColor = CheckTableColor(item.id);
                    b.Click += new EventHandler(BanClick);
                    flowLayoutPanelBan.Controls.Add(b);
                }
            }
        }

        private Color CheckTableColor(int idTalbe)
        {
            using (TeaEntities t = new TeaEntities())
            {
                var _count = t.Bill.Where(x => x.idTable == idTalbe && x.status == 0).ToList().Count;
                if (_count > 0) //Có người
                {
                    return Color.FromArgb(234, 84, 85);
                }
                else
                {
                    return Color.WhiteSmoke;
                }
            }
        }

        private void BanClick(object sender, EventArgs e)
        {
            var info = ShowAccountInfo();
            string tableName = ((sender as System.Windows.Forms.Button).Tag as TableTea).name;
            int tableID = ((sender as System.Windows.Forms.Button).Tag as TableTea).id;

            //MessageBox.Show(s);
            tempTableID = tableID;
            lblBan.Text = tableName;

            using (TeaEntities entities = new TeaEntities())
            {
                var idBill = (from x in entities.Bill.Where(x => x.idTable == tableID && x.status == 0) select new { ID = x.id }).FirstOrDefault();
                if (idBill != null)
                {
                    //MessageBox.Show("Bàn đã có người ngồi, đang hiển thị hoá đơn hiện tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiHoaDon(idBill.ID);
                }
                else
                {
                    //MessageBox.Show("Bàn không có người ngồi, đã tạo hoá đơn mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    entities.Bill.Add(new Bill()
                    {
                        idTable = tableID,
                        idStaff = info.id,
                        idCustomer = 1,
                        dateCheckIn = DateTime.Now,
                        dateCheckOut = DateTime.Now,
                        status = 0
                    });
                    entities.SaveChanges();
                }
                HienThiBan();
                
            }
        }

        private void HienThiHoaDon(int _id)
        {
            using (TeaEntities entities = new TeaEntities())
            {
                try
                {
                    if (dgvTea.Rows.Count > 0)
                    {
                        dgvTea.Rows.Clear();
                    }
                    var listBillInfo = (from i in entities.BillInfo.Where(i => i.idBill == _id) select new { Name = i.Tea.name, Price = i.Tea.price, Quantity = i.quantity, Total = i.Tea.price * i.quantity }).ToList();
                    if (listBillInfo != null)
                    {
                        foreach (var item in listBillInfo)
                        {
                            var tongTien = item.Price * item.Quantity;
                            dgvTea.Rows.Add(item.Name, item.Price, item.Quantity, tongTien);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bàn này chưa có hoá đơn");
                }
            }
        }

        private void HienThiMenu()
        {
            using (TeaEntities entities = new TeaEntities())
            {
                var menu = entities.Tea.ToList();
                foreach (var item in menu)
                {
                    System.Windows.Forms.Button b = new System.Windows.Forms.Button();
                    b.Size = new Size(90, 90);
                    b.Text = item.name + Environment.NewLine + TeaHelper.ConvertMoney(item.price) + "đ";
                    b.Tag = item;
                    b.TextAlign = ContentAlignment.BottomCenter;
                    b.Padding = new Padding(0, 0, 0, 5);
                    b.Click += new EventHandler(MenuClick);
                    flowLayoutPanelMenu.Controls.Add(b);
                }
            }
        }

        private void TimKiemMenu(string name)
        {
            flowLayoutPanelMenu.Controls.Clear();
            using (TeaEntities entities = new TeaEntities())
            {
                var menu = entities.Tea.Where(t => t.name.Contains(name)).ToList();
                foreach (var item in menu)
                {
                    System.Windows.Forms.Button b = new System.Windows.Forms.Button();
                    b.Size = new Size(90, 90);
                    b.Text = item.name + Environment.NewLine + TeaHelper.ConvertMoney(item.price) + "đ";
                    b.Tag = item;
                    b.TextAlign = ContentAlignment.BottomCenter;
                    b.Padding = new Padding(0, 0, 0, 5);
                    b.Click += new EventHandler(MenuClick);
                    flowLayoutPanelMenu.Controls.Add(b);
                }
            }
        }

        private void MenuClick(object sender, EventArgs e)
        {
            var id = ((sender as System.Windows.Forms.Button).Tag as Tea).id;
            var name = ((sender as System.Windows.Forms.Button).Tag as Tea).name;
            var price = ((sender as System.Windows.Forms.Button).Tag as Tea).price;

            tempTeaID = id;
            txtTeaName.Text = name;
            txtPrice.Text = TeaHelper.ConvertMoney(price);
        }

        private void LoadCustomer()
        {
            using (TeaEntities entities = new TeaEntities())
            {
                var result = (from x in entities.Customer select new {x.id, x.name}).ToList();
                cbbKhachHang.DataSource = result;
                cbbKhachHang.DisplayMember = "name";
                cbbKhachHang.ValueMember = "id";
            }
        }
        private string x;
        private void cbbKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isDisCus.Checked)
            {
                ShowDiscount();
            }
        }

        private void ShowDiscount()
        {
            int id = Convert.ToInt32(cbbKhachHang.SelectedValue);
            using (TeaEntities entities = new TeaEntities())
            {
                var res = (from x in entities.Customer.Where(x => x.id == id) select new { Discount = x.discount }).FirstOrDefault();
                txtDisCus.Text = res.Discount.ToString();
            }
        }

        private void LoadCheckDiscount()
        {
            if (isDisPer.Checked)
            {
                panelPer.Enabled = true;
                panelCus.Enabled = false;
            } else
            {
                panelPer.Enabled = false;
                panelCus.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HienThiMenu();
            HienThiBan();
            LoadCustomer();
            LoadCheckDiscount();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string khach = string.Empty;
            int discount = 0;
            var info = ShowAccountInfo();
            try
            {
                int priceTotal = 0;
                foreach (DataGridViewRow row in dgvTea.Rows)
                {
                    priceTotal += Convert.ToInt32(row.Cells[3].Value);
                }

                khach = (isDisPer.Checked == true) ? txtTempCus.Text : cbbKhachHang.Text;
                discount = (isDisPer.Checked == true) ? Convert.ToInt32(txtDisPer.Text) : Convert.ToInt32(txtDisCus.Text);

                ExportToPDF(dgvTea, "hoadon", lblBan.Text, info.displayName, khach, priceTotal.ToString(), discount);
                MessageBox.Show("Xuất hoá đơn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi" + ex);
            }
        }

        private void ExportToPDF(DataGridView gridView, string _filename, string _tableName, string _staff, string _customer, string _total, int giamgia)
        {
            BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            PdfPTable pdfPTable = new PdfPTable(gridView.Columns.Count);
            pdfPTable.WidthPercentage = 100;
            pdfPTable.DefaultCell.Padding = 5;
            pdfPTable.DefaultCell.BorderWidth = 1;
            pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            iTextSharp.text.Font textHeader = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font normal = new iTextSharp.text.Font(bf, 13, iTextSharp.text.Font.NORMAL);

            foreach (DataGridViewColumn column in gridView.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, textHeader));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                pdfPTable.AddCell(cell);
            }

            foreach (DataGridViewRow row in gridView.Rows)
            {
                foreach (DataGridViewCell _cell in row.Cells)
                {
                    pdfPTable.AddCell(new Phrase(_cell.Value.ToString(), normal));
                }
            }

            var saveDialog = new SaveFileDialog();
            saveDialog.FileName = _filename;
            saveDialog.DefaultExt = ".pdf";
            saveDialog.Title = "Chọn nơi lưu hoá đơn";
            saveDialog.Filter = "PDF(*.pdf)|*.pdf";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveDialog.FileName, FileMode.Create))
                {
                    Document document = new Document(PageSize.A4);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    iTextSharp.text.Image teaLogo = iTextSharp.text.Image.GetInstance("logoTea.png");
                    teaLogo.ScalePercent(50f);
                    teaLogo.Alignment = Element.ALIGN_CENTER;
                    document.Add(teaLogo);


                    PdfPTable titleTable = new PdfPTable(1);
                    titleTable.AddCell(new PdfPCell(new Paragraph("Địa chỉ: Ngõ 36, Phú Diễn, Phú Diễn, Bắc Từ Liêm, Hà Nội", normal))
                    {
                        BorderWidth = 0,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    titleTable.AddCell(new PdfPCell(new Paragraph("Số điện thoại: 0987412111", normal))
                    {
                        BorderWidth = 0,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    titleTable.AddCell(new PdfPCell(new Paragraph("----------------", normal))
                    {
                        BorderWidth = 0,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    titleTable.AddCell(new PdfPCell(new Paragraph("HOÁ ĐƠN THANH TOÁN", textHeader))
                    {
                        BorderWidth = 0,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });

                    titleTable.AddCell(new PdfPCell(new Phrase($"Số: HD{TeaHelper.RandomString(7)}", normal))
                    {
                        BorderWidth = 0,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });

                    document.Add(titleTable); 
                    document.Add(new Paragraph($"Ngày tạo: {DateTime.Now}", normal));
                    document.Add(new Paragraph($"Tên bàn: {_tableName}", normal));
                    document.Add(new Paragraph($"Nhân viên thu ngân: {_staff}", normal));
                    document.Add(new Paragraph($"Khách hàng: {_customer}", normal));
                    document.Add(new Paragraph(" ", normal));
                    document.Add(pdfPTable);
                    document.Add(new Paragraph(" ", normal));

                    int tien = Convert.ToInt32(_total);
                    int tienduocgiam = tien - (tien * giamgia) /100;
                    int thue = (int)(tienduocgiam * 0.01);
                    int tongcong = ((int)(tienduocgiam + thue));

                    document.Add(new Paragraph($"Tổng thành tiền: { TeaHelper.ConvertMoney(tien)}đ", normal));
                    document.Add(new Paragraph($"Giảm giá {giamgia}%: {TeaHelper.ConvertMoney(tienduocgiam)}đ", normal));
                    document.Add(new Paragraph($"Phí VAT (1%): {TeaHelper.ConvertMoney(Math.Abs(thue))}đ", normal));
                    document.Add(new Paragraph($"Tổng cộng: {TeaHelper.ConvertMoney(Math.Abs(tongcong))}đ", normal));

                    document.Close();
                    stream.Close();
                }
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
            
        }
        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {

            int count = Convert.ToInt32(numQuantity.Value);
            int price = Convert.ToInt32(txtPrice.Text.Replace(".",""));

            int total = price * count;
            txtTongTien.Text = $"{TeaHelper.ConvertMoney(total)}đ";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            using (TeaEntities te = new TeaEntities())
            {
                var idBill = (from x in te.Bill.Where(x => x.idTable == tempTableID && x.status == 0) select new {ID = x.id}).FirstOrDefault();
                te.BillInfo.Add(new BillInfo()
                {
                    idBill = idBill.ID,
                    idTea = tempTeaID,
                    quantity = Convert.ToInt32(numQuantity.Value)
                });
                te.SaveChanges();
                MessageBox.Show("Thêm thành công","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                HienThiHoaDon(idBill.ID);
            }
        }

        private void picAvatar_Click(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {
            string teaName = txtTimkiem.Text.Trim();
            TimKiemMenu(teaName);
        }

        private void menuQLBan_Click(object sender, EventArgs e)
        {
            formTable ft = new formTable();
            ft.ShowDialog();
        }

        private void menuQLTea_Click(object sender, EventArgs e)
        {
            new formTea().ShowDialog();

        }

        private void menuQLNhanVien_Click(object sender, EventArgs e)
        {
            new formAccount().ShowDialog();
        }

        private void menuQLKhachHang_Click(object sender, EventArgs e)
        {
            formCustomer fc = new formCustomer();
            fc.ShowDialog();
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            using (TeaEntities teaEntities = new TeaEntities())
            {
                try
                {
                    var idBill = (from x in teaEntities.Bill.Where(x => x.idTable == tempTableID && x.status == 0) select new { ID = x.id }).FirstOrDefault();
                    int idTable = idBill.ID;
                    var update = teaEntities.Bill.Find(idTable);
                    if (update != null)
                    {
                        update.status = 1;
                    }


                    var updateTable = teaEntities.TableTea.Find(idTable);
                    if (updateTable != null)
                    {
                        updateTable.status = "Trống";
                    }
                    teaEntities.SaveChanges();
                    MessageBox.Show("Thanh toán thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (dgvTea.Rows.Count > 0)
                    {
                        dgvTea.Rows.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Thanh toán thất bại: " + ex, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                HienThiBan();
            }
        }

        private void isDisPer_CheckedChanged(object sender, EventArgs e)
        {
            LoadCheckDiscount();
        }

        private void isDisCus_CheckedChanged(object sender, EventArgs e)
        {
            LoadCheckDiscount();
        }
    }
}
