-- =============================================
-- KỊCH BẢN TẠO CƠ SỞ DỮ LIỆU BÀI TẬP LÝ THUYẾT 7 (BTLT7)
-- =============================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'BTLT7_DB')
BEGIN
    CREATE DATABASE BTLT7_DB;
END
GO

USE BTLT7_DB;
GO

-- -------------------------------------------------------------
-- CÂU 1: Nhà sách mini "Tri Thức Books"
-- -------------------------------------------------------------
IF OBJECT_ID('dbo.Sach', 'U') IS NOT NULL DROP TABLE dbo.Sach;
IF OBJECT_ID('dbo.TheLoaiSach', 'U') IS NOT NULL DROP TABLE dbo.TheLoaiSach;
GO

CREATE TABLE dbo.TheLoaiSach (
    MaTL INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TenTheLoai NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(255) NULL,
    SoLuongSach INT NOT NULL CONSTRAINT DF_TheLoaiSach_SoLuong DEFAULT (0),
    NgayTao DATETIME NOT NULL CONSTRAINT DF_TheLoaiSach_NgayTao DEFAULT (GETDATE())
);
GO

-- Bảng Sách để kiểm thử ràng buộc khóa ngoại khi xóa thể loại
CREATE TABLE dbo.Sach (
    MaSach INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TenSach NVARCHAR(100) NOT NULL,
    MaTL INT NOT NULL CONSTRAINT FK_Sach_TheLoaiSach FOREIGN KEY REFERENCES dbo.TheLoaiSach(MaTL)
);
GO

-- Dữ liệu mẫu Câu 1 (giống hình đề bài)
SET IDENTITY_INSERT dbo.TheLoaiSach ON;
INSERT INTO dbo.TheLoaiSach (MaTL, TenTheLoai, MoTa, SoLuongSach, NgayTao) VALUES
(1, N'Tiểu thuyết', N'Thân tiểu thuyết', 1, '2024-02-20 09:57:07'),
(2, N'Kỹ năng sống', N'Kỹ năng sống nâng sống', 15, '2024-02-20 09:57:03'),
(3, N'Thiếu nhi', N'Thiếu nhi củ có', 2, '2024-02-20 10:52:03'),
(4, N'Sách giáo khoa', N'Sách giáo khoa chỉ đường pháp hơi vào tạo', 2, '2024-02-20 10:33:07'),
(5, N'Truyện tranh', N'Truyện tranh', 1, '2024-02-20 20:42:04');
SET IDENTITY_INSERT dbo.TheLoaiSach OFF;

-- Thêm 1 cuốn sách tham chiếu đến Thể loại 1 để test ràng buộc khóa ngoại khi xóa
INSERT INTO dbo.Sach (TenSach, MaTL) VALUES (N'Những người khốn khổ', 1);
GO


-- -------------------------------------------------------------
-- CÂU 2: Quản lý Hội viên — Phòng tập Gym "FitZone"
-- -------------------------------------------------------------
IF OBJECT_ID('dbo.HoiVien', 'U') IS NOT NULL DROP TABLE dbo.HoiVien;
GO

CREATE TABLE dbo.HoiVien (
    MaHV INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh BIT NOT NULL, -- 1 = Nam, 0 = Nữ
    NgaySinh DATE NULL,
    SDT VARCHAR(15) NULL,
    Email VARCHAR(100) NULL,
    HangThanhVien NVARCHAR(20) NOT NULL, -- Basic / VIP / Premium
    NgayDangKy DATETIME NOT NULL CONSTRAINT DF_HoiVien_NgayDangKy DEFAULT (GETDATE()),
    TrangThai BIT NOT NULL CONSTRAINT DF_HoiVien_TrangThai DEFAULT (1) -- 1 = Đang hoạt động, 0 = Tạm ngưng
);
GO

-- Dữ liệu mẫu Câu 2 (giống hình đề bài)
SET IDENTITY_INSERT dbo.HoiVien ON;
INSERT INTO dbo.HoiVien (MaHV, HoTen, GioiTinh, NgaySinh, SDT, Email, HangThanhVien, NgayDangKy, TrangThai) VALUES
(1, N'Họ tên Văn', 1, '1999-11-07', '07382735879', 'van@gmail.com', N'Basic', '2024-01-12 10:00:00', 1),
(2, N'Nguyễn Ninh', 1, '1999-11-10', '07382731034', 'ninh@gmail.com', N'Premium', '2024-01-12 10:00:00', 1),
(3, N'Nguyễn Bộ Đồn', 1, '1999-12-29', '07882773727', 'don@gmail.com', N'Basic', '2024-01-12 10:00:00', 1),
(4, N'Nguyễn Xim', 1, '1999-11-17', '07375856597', 'xim@gmail.com', N'VIP', '2024-01-12 10:00:00', 1),
(5, N'Nguyễn Tương', 0, '1999-01-22', '07887775931', 'tuong@gmail.com', N'Premium', '2024-01-12 10:00:00', 1);
SET IDENTITY_INSERT dbo.HoiVien OFF;
GO


-- -------------------------------------------------------------
-- CÂU 3: Quản lý Loại phòng — Phòng ở Homestay "Sunrise Homestay"
-- -------------------------------------------------------------
IF OBJECT_ID('dbo.Phong', 'U') IS NOT NULL DROP TABLE dbo.Phong;
IF OBJECT_ID('dbo.LoaiPhong', 'U') IS NOT NULL DROP TABLE dbo.LoaiPhong;
GO

CREATE TABLE dbo.LoaiPhong (
    MaLoai INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TenLoai NVARCHAR(100) NOT NULL,
    GiaMoiDem DECIMAL(18,2) NOT NULL,
    MoTa NVARCHAR(255) NULL
);
GO

CREATE TABLE dbo.Phong (
    MaPhong INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    SoPhong VARCHAR(10) NOT NULL,
    TangSo INT NOT NULL,
    TinhTrang NVARCHAR(20) NOT NULL, -- Trống / Đang ở / Đang dọn
    HinhAnh NVARCHAR(255) NULL,
    MaLoai INT NOT NULL CONSTRAINT FK_Phong_LoaiPhong FOREIGN KEY REFERENCES dbo.LoaiPhong(MaLoai)
);
GO

-- Dữ liệu mẫu Câu 3 (giống hình đề bài)
SET IDENTITY_INSERT dbo.LoaiPhong ON;
INSERT INTO dbo.LoaiPhong (MaLoai, TenLoai, GiaMoiDem, MoTa) VALUES
(1, N'Phòng Đơn', 350000, N'Phòng đơn 1 giường đôi, ban công thoáng mát'),
(2, N'Phòng Đôi', 500000, N'Phòng đôi 2 giường rộng, view thành phố'),
(3, N'Phòng VIP', 800000, N'Phòng VIP cao cấp, tiện nghi đầy đủ');
SET IDENTITY_INSERT dbo.LoaiPhong OFF;

SET IDENTITY_INSERT dbo.Phong ON;
INSERT INTO dbo.Phong (MaPhong, SoPhong, TangSo, TinhTrang, HinhAnh, MaLoai) VALUES
(1, '101', 1, N'Trống', 'room1.jpg', 1),
(2, '102', 1, N'Đang ở', 'room2.jpg', 2),
(3, '201', 2, N'Đang dọn', 'room3.jpg', 1),
(4, '202', 2, N'Đang dọn', 'room4.jpg', 2),
(5, '301', 3, N'Đang dọn', 'room5.jpg', 3);
SET IDENTITY_INSERT dbo.Phong OFF;
GO


-- -------------------------------------------------------------
-- CÂU 4: Quản lý Lịch khám bệnh — Phòng khám tư "An Khang Clinic"
-- -------------------------------------------------------------
IF OBJECT_ID('dbo.LichKham', 'U') IS NOT NULL DROP TABLE dbo.LichKham;
IF OBJECT_ID('dbo.BacSi', 'U') IS NOT NULL DROP TABLE dbo.BacSi;
GO

CREATE TABLE dbo.BacSi (
    MaBS INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    ChuyenKhoa NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15) NULL
);
GO

CREATE TABLE dbo.LichKham (
    MaLich INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TenBenhNhan NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15) NOT NULL,
    NgayKham DATE NOT NULL,
    GioKham NVARCHAR(10) NOT NULL, -- e.g. '09:00', '13:00'
    MaBS INT NOT NULL CONSTRAINT FK_LichKham_BacSi FOREIGN KEY REFERENCES dbo.BacSi(MaBS),
    TrangThai NVARCHAR(20) NOT NULL -- Chờ khám / Đã khám / Đã hủy
);
GO

-- Dữ liệu mẫu Câu 4 (giống hình đề bài)
SET IDENTITY_INSERT dbo.BacSi ON;
INSERT INTO dbo.BacSi (MaBS, HoTen, ChuyenKhoa, SDT) VALUES
(1, N'BS. Nguyễn Văn A', N'Nội tổng quát', '0901234567'),
(2, N'BS. Nguyễn Văn B', N'Nội tổng quát', '0901234568'),
(3, N'BS. Nguyễn Văn C', N'Nội tổng quát', '0901234569'),
(4, N'BS. Nguyễn Văn D', N'Nội tổng quát', '0901234570');
SET IDENTITY_INSERT dbo.BacSi OFF;

SET IDENTITY_INSERT dbo.LichKham ON;
INSERT INTO dbo.LichKham (MaLich, TenBenhNhan, SDT, NgayKham, GioKham, MaBS, TrangThai) VALUES
(1, N'Tên bệnh nhân', '09725667894', '2026-10-10', '09:00', 1, N'Chờ khám'),
(2, N'Nguyễn Xinh', '09725667897', '2026-10-11', '13:00', 1, N'Đã khám'),
(3, N'Nguyễn Hạm', '09725667899', '2026-10-12', '16:00', 1, N'Đã hủy'),
(4, N'Nguyễn Tinh', '09725667830', '2026-10-12', '16:00', 1, N'Đã hủy'),
(5, N'Nguyễn Hoan anh', '09725667899', '2026-10-15', '18:00', 1, N'Đã hủy');
SET IDENTITY_INSERT dbo.LichKham OFF;
GO
