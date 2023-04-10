CREATE DATABASE QuanLyMeoTea --TẠO DB
GO

USE QuanLyMeoTea --Dùng DB
GO

-- Tea
-- Table
-- Account
-- Staff
-- Customer
-- Bill
-- BillInfo

CREATE TABLE Tea (
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL,
	price INT NOT NULL DEFAULT 20,
	img IMAGE
)
GO

CREATE TABLE TableTea (
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL,
	status NVARCHAR(100) NOT NULL -- Trống || Có người
)
GO

CREATE TABLE Account (
	id INT IDENTITY PRIMARY KEY,
	userName NVARCHAR(50) NOT NULL,
	passWord NVARCHAR(50) NOT NULL,
	displayName NVARCHAR(100) NOT NULL,
	address NVARCHAR(100) NOT NULL,
	phone NVARCHAR(10) NOT NULL,
	sex NVARCHAR(3) NOT NULL,
	accountType INT NOT NULL DEFAULT 0, -- 0: Nhân viên, 1: Admin
	avatar IMAGE,
	isHide BIT NOT NULL DEFAULT 0 -- 0: F, 1: T
)
GO

CREATE TABLE Customer (
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL,
	address NVARCHAR(100) NOT NULL,
	phoneNumber NVARCHAR(10) NOT NULL,
)
GO


CREATE TABLE Bill (
	id INT IDENTITY PRIMARY KEY,
	idTable INT NOT NULL,
	idStaff INT NOT NULL,
	idCustomer INT NOT NULL,
	dateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	dateCheckOut DATE NOT NULL,
	status INT NOT NULL DEFAULT 0 -- 0: Chưa thanh toán, 1: Đã thanh toán
	
	FOREIGN KEY (idTable) REFERENCES dbo.TableTea(id),
	FOREIGN KEY (idStaff) REFERENCES dbo.Account(id),
	FOREIGN KEY (idCustomer) REFERENCES dbo.Customer(id)
)
GO

CREATE TABLE BillInfo (
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idTea INT NOT NULL,
	quantity INT NOT NULL DEFAULT 0

	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idTea) REFERENCES dbo.Tea(id)
)

-- INSERT DATA Tea
INSERT INTO dbo.Tea(name, price) VALUES(N'Trà sữa vị dâu', 8000);
INSERT INTO dbo.Tea(name, price) VALUES(N'Trà sữa đào', 9000);
INSERT INTO dbo.Tea(name, price) VALUES(N'Trà sữa Cacao', 8500);
INSERT INTO dbo.Tea(name, price) VALUES(N'Trà sữa Socola', 6500);
INSERT INTO dbo.Tea(name, price) VALUES(N'Trà sữa Mix 7 Vị', 10000);
INSERT INTO dbo.Tea(name, price) VALUES(N'Trà sữa Cầu vồng', 12000);
INSERT INTO dbo.Tea(name, price) VALUES(N'Thạch ngũ vị', 6000);
INSERT INTO dbo.Tea(name, price) VALUES(N'Kem', 4000);
INSERT INTO dbo.Tea(name, price) VALUES(N'Hồng đào', 7500);
INSERT INTO dbo.Tea(name, price) VALUES(N'Trà sữa nướng', 8000);
INSERT INTO dbo.Tea(name, price) VALUES(N'Trà sữa trân châu', 8000);
INSERT INTO dbo.Tea(name, price) VALUES(N'Trà xanh', 20000);

INSERT INTO dbo.TableTea(name, status) VALUES(N'Bàn 01',N'Trống');
INSERT INTO dbo.TableTea(name, status) VALUES(N'Bàn 02',N'Trống');
INSERT INTO dbo.TableTea(name, status) VALUES(N'Bàn 03',N'Trống');
INSERT INTO dbo.TableTea(name, status) VALUES(N'Bàn 04',N'Trống');
INSERT INTO dbo.TableTea(name, status) VALUES(N'Bàn 05',N'Trống');
INSERT INTO dbo.TableTea(name, status) VALUES(N'Bàn 06',N'Trống');
INSERT INTO dbo.TableTea(name, status) VALUES(N'Bàn 07',N'Trống');
INSERT INTO dbo.TableTea(name, status) VALUES(N'Bàn 08',N'Trống');
INSERT INTO dbo.TableTea(name, status) VALUES(N'Bàn 09',N'Trống');

INSERT INTO dbo.Account(userName, passWord, displayName, address, phone, sex ,accountType, isHide) VALUES(N'admin',N'admin' ,N'Đỗ Kim Khánh' ,N'Thường Tín, Hà Nội', N'0999888777',N'Nam', 1, 0);
INSERT INTO dbo.Account(userName, passWord, displayName, address, phone, sex ,accountType, isHide) VALUES(N'levantruong',N'123' ,N'Lê Văn Trường', N'Hà Nội', N'0985472154',N'Nam' ,0, 0);
INSERT INTO dbo.Account(userName, passWord, displayName, address, phone, sex ,accountType, isHide) VALUES(N'thang',N'123' ,N'Thắng Đỗ Hoành' ,N'Hà Nội', N'0985472154',N'Nam',0, 0);
INSERT INTO dbo.Account(userName, passWord, displayName, address, phone, sex ,accountType, isHide) VALUES(N'phuong',N'123' ,N'Hà Phương',N'Hà Nội', N'0985472154',N'Nam' ,0, 0);
INSERT INTO dbo.Account(userName, passWord, displayName, address, phone, sex ,accountType, isHide) VALUES(N'minhan',N'123' ,N'Minh An',N'Hà Nội', N'0985472154',N'Nam' ,0, 0);
INSERT INTO dbo.Account(userName, passWord, displayName, address, phone, sex ,accountType, isHide) VALUES(N'mixue',N'123' ,N'Mixu E',N'Hà Nội', N'0985472154',N'Nam' ,0, 0);

INSERT INTO dbo.Customer(name, address, phoneNumber) VALUES(N'Thủy Ánh',N'65 Phường Hàng Mã, Quận Hoàn Kiếm, Hà Nội' ,N'0987514125' );
INSERT INTO dbo.Customer(name, address, phoneNumber) VALUES(N'Trai Quốc',N'8157 Xã Minh Cường, Huyện Thường Tín, Hà Nội' ,N'0246417269' );
INSERT INTO dbo.Customer(name, address, phoneNumber) VALUES(N'Lê Phương',N'3 Xã Nghĩa Hương, Huyện Quốc Oai, Hà Nội' ,N'0985608245' );
INSERT INTO dbo.Customer(name, address, phoneNumber) VALUES(N'Nguyễn Tú',N'62 Thị trấn Quốc Oai, Huyện Quốc Oai, Hà Nội' ,N'0980561151' );
INSERT INTO dbo.Customer(name, address, phoneNumber) VALUES(N'Lê Dương',N'47, Phường Thụy Khuê, Quận Tây Hồ, Hà Nội' ,N'0988008602' );
INSERT INTO dbo.Customer(name, address, phoneNumber) VALUES(N'Phạm Tú',N'925 Xã Hòa Chính, Huyện Chương Mỹ, Hà Nội' ,N'0989582897' );
INSERT INTO dbo.Customer(name, address, phoneNumber) VALUES(N'Nguyễn Vương Hải',N'4 Phường Nhân Chính, Quận Thanh Xuân, Hà Nội' ,N'0987539096' );
INSERT INTO dbo.Customer(name, address, phoneNumber) VALUES(N'Nguyễn Thị Vành Khuyên',N'65 Phường Nghĩa Tân, Quận Cầu Giấy, Hà Nội' ,N'0980776793' );

INSERT INTO dbo.Bill(idTable, idStaff, idCustomer, dateCheckIn, dateCheckOut, status) VALUES(1, 1, 1, GETDATE(), GETDATE(), 0)
INSERT INTO dbo.Bill(idTable, idStaff, idCustomer, dateCheckIn, dateCheckOut, status) VALUES(2, 1, 1, GETDATE(), GETDATE(), 1)
INSERT INTO dbo.Bill(idTable, idStaff, idCustomer, dateCheckIn, dateCheckOut, status) VALUES(3, 1, 1, GETDATE(), GETDATE(), 0)
INSERT INTO dbo.Bill(idTable, idStaff, idCustomer, dateCheckIn, dateCheckOut, status) VALUES(2, 2, 1, GETDATE(), GETDATE(), 1)
INSERT INTO dbo.Bill(idTable, idStaff, idCustomer, dateCheckIn, dateCheckOut, status) VALUES(2, 2, 1, GETDATE(), GETDATE(), 0)
INSERT INTO dbo.Bill(idTable, idStaff, idCustomer, dateCheckIn, dateCheckOut, status) VALUES(4, 3, 1, GETDATE(), GETDATE(), 0)

INSERT INTO dbo.BillInfo(idBill, idTea, quantity) VALUES(1, 1 , 4)
INSERT INTO dbo.BillInfo(idBill, idTea, quantity) VALUES(1, 2 , 4)
INSERT INTO dbo.BillInfo(idBill, idTea, quantity) VALUES(1, 2 , 2)
INSERT INTO dbo.BillInfo(idBill, idTea, quantity) VALUES(1, 8 , 1)
INSERT INTO dbo.BillInfo(idBill, idTea, quantity) VALUES(2, 5 , 1)
INSERT INTO dbo.BillInfo(idBill, idTea, quantity) VALUES(2, 6 , 2)
INSERT INTO dbo.BillInfo(idBill, idTea, quantity) VALUES(2, 11 , 1)
INSERT INTO dbo.BillInfo(idBill, idTea, quantity) VALUES(2, 12 , 2)
INSERT INTO dbo.BillInfo(idBill, idTea, quantity) VALUES(3, 11 , 1)
INSERT INTO dbo.BillInfo(idBill, idTea, quantity) VALUES(3, 12 , 1)
INSERT INTO dbo.BillInfo(idBill, idTea, quantity) VALUES(4, 1 , 3)
INSERT INTO dbo.BillInfo(idBill, idTea, quantity) VALUES(4, 2 , 3)

SELECT * FROM dbo.Account
SELECT * FROM dbo.Bill -- 0: Chưa thanh toán, 1: Đã thanh toán
SELECT * FROM dbo.BillInfo
SELECT * FROM dbo.Customer
SELECT * FROM  dbo.Tea
SELECT * FROM dbo.TableTea
SELECT TOP 3 * FROM dbo.Bill
DELETE FROM dbo.BillInfo