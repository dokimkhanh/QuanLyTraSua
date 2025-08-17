# Quản lý quán Trà Sữa (QuanLyTraSua) 🍵

Simple milk tea shop management software — ứng dụng WinForms (C#) để quản lý đồ uống, bàn, khách hàng, hoá đơn và báo cáo.

---

## 🔥 Tóm tắt
- Ứng dụng quản lý quán trà sữa (POS-lite) viết bằng **C# (WinForms)**, sử dụng **Entity Framework** và **SQL Server**.  
- Có sẵn script tạo database `QuanLyTea.sql` trong repo.  
- License: [MIT](https://choosealicense.com/licenses/mit/)


---

## ⚙️ Tính năng chính
- Quản lý món (Milk Tea)
- Quản lý bàn (Table)
- Quản lý nhân viên / khách hàng
- Quản lý hoá đơn & xuất PDF hoá đơn
- Thống kê doanh thu
- Xuất báo cáo / In hoá đơn

---

## 🧰 Tech stack
- Client: C# (WinForms) + Entity Framework + Bunifu UI (giao diện)
- Database: SQL Server (T-SQL)
- File SQL: `QuanLyTea.sql`

---

## ✅ Yêu cầu
- Windows (ứng dụng WinForms)
- Visual Studio (recommended) hoặc VS Community
- SQL Server (Express / LocalDB / full)
- **Bunifu UI license** (project hiện dùng Bunifu — cần license để build/run giao diện đầy đủ)

---

## 🚀 Cách cài đặt & chạy (developer)
1. Clone repo:
```bash
git clone https://github.com/dokimkhanh/QuanLyTraSua.git
cd QuanLyTraSua
````

2. Tạo database:

* Mở `QuanLyTea.sql` bằng SSMS (SQL Server Management Studio) hoặc chạy script trên SQL Server instance của bạn.
* Tên database mặc định trong README cũ: `QuanLyMeoTea` — có thể đổi trong connection string nếu cần.

3. Mở solution:

* Mở `QuanLyQuanTraSua.sln` bằng Visual Studio.
* Restore NuGet packages (nếu có).
* Thiết lập connection string (app.config / Settings) trỏ tới SQL Server instance của bạn.

Ví dụ connection string (chỉ ví dụ — chỉnh lại theo môi trường):

```xml
<connectionStrings>
  <add name="QuanLyTeaConnection" connectionString="Server=.;Database=QuanLyMeoTea;Trusted_Connection=True;" providerName="System.Data.SqlClient"/>
</connectionStrings>
```

4. Build & Run:

* Build solution → Run project chính (WinForms exe).
* Nếu gặp lỗi thiếu Bunifu, cần cài đặt/đăng ký license hoặc remove/replace component đó để build.
