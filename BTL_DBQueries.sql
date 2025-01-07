create Database QLBH;
use QLBH;

drop database QLBH

-- Bảng Khách Hàng
CREATE TABLE KhachHang (
    maKhachhang INT IDENTITY(1,1) PRIMARY KEY, 
    tenKhachhang NVARCHAR(100) NOT NULL,
    soDienThoai VARCHAR(15),
    ghiChu NVARCHAR(225),
    email VARCHAR(50),
    gioiTinh NVARCHAR(10) CHECK (gioiTinh = N'Nam' OR gioiTinh = N'Nữ'),
    diaChi VARCHAR(100)
);

-- Bảng Nhân Viên
CREATE TABLE NhanVien (
    maNhanvien INT IDENTITY(1,1) PRIMARY KEY, 
    tenNhanvien NVARCHAR(100) NOT NULL,
    soDienThoai VARCHAR(15),
    email VARCHAR(50),
    gioiTinh NVARCHAR(10) CHECK (gioiTinh = N'Nam' OR gioiTinh = N'Nữ'),
    Luong FLOAT CHECK (Luong >= 0), -- Ensure salary is non-negative
    diaChi VARCHAR(100)
);

-- Bảng Tài Khoản
CREATE TABLE TaiKhoan (
    maNhanvien INT, 
    tenTaiKhoan VARCHAR(30) UNIQUE NOT NULL, 
    matKhau VARCHAR(50) NOT NULL,
    quyenQuanTri BIT NOT NULL DEFAULT 0,
    PRIMARY KEY (maNhanvien, tenTaiKhoan), 
    FOREIGN KEY (maNhanvien) REFERENCES NhanVien(maNhanvien) ON DELETE CASCADE
);

-- Bảng Công Ty Giao Hàng
CREATE TABLE CongTyGiaoHang (
    maCongTy INT IDENTITY(1,1) PRIMARY KEY, 
    tenCongTy NVARCHAR(100) NOT NULL, 
    diaChi NVARCHAR(200),
    tongDai NVARCHAR(15),
    email NVARCHAR(100),
    moTa TEXT
);

-- Bảng Nhà Cung Cấp
CREATE TABLE NhaCungCap (
    maNhaCungCap INT IDENTITY(1,1) PRIMARY KEY,
    tenNhaCungCap NVARCHAR(255) NOT NULL,
    diaChi NVARCHAR(200),
    soDienThoai VARCHAR(15),
    email NVARCHAR(100)
);

-- Bảng Phân Phối (Quan hệ Nhiều-Nhiều giữa Nhà Cung Cấp và Công Ty Giao Hàng)
CREATE TABLE PhanPhoi (
    maNhaCungCap INT,
    maCongTy INT,
    PRIMARY KEY (maNhaCungCap, maCongTy),
    FOREIGN KEY (maNhaCungCap) REFERENCES NhaCungCap(maNhaCungCap) ON DELETE CASCADE,
    FOREIGN KEY (maCongTy) REFERENCES CongTyGiaoHang(maCongTy) ON DELETE CASCADE
);

-- Bảng Khuyến Mãi
CREATE TABLE KhuyenMai (
    id INT IDENTITY(1,1) PRIMARY KEY,
    tenKhuyenMai NVARCHAR(255) NOT NULL,
    maKhuyenMai VARCHAR(50) NOT NULL UNIQUE,
    phanTramGiamGia DECIMAL(5, 2) NOT NULL CHECK (phanTramGiamGia >= 0 AND phanTramGiamGia <= 100)
);

-- Bảng Đơn Hàng
CREATE TABLE DonHang (
    maDonHang INT IDENTITY(1,1) PRIMARY KEY, 
    maKhachHang INT NOT NULL,
    thanhTien BIGINT NOT NULL CHECK (thanhTien >= 0),
	hinhThucThanhToan nvarchar(50),
    ghiChu NVARCHAR(225),
    maNhanVien INT NOT NULL,
    maKhuyenMai INT NULL,
    Ngaytaodon DATE NOT NULL,
    FOREIGN KEY (maNhanVien) REFERENCES NhanVien(maNhanvien) ON DELETE CASCADE,
    FOREIGN KEY (maKhachHang) REFERENCES KhachHang(maKhachhang) ON DELETE CASCADE,
    FOREIGN KEY (maKhuyenMai) REFERENCES KhuyenMai(id) ON DELETE SET NULL
);

-- Bảng Sản Phẩm
CREATE TABLE SanPham (
    maSanPham INT IDENTITY(1,1) PRIMARY KEY,                     
    tenSanPham NVARCHAR(255) NOT NULL,  
    giaNhapSanPham DECIMAL(10, 2) NOT NULL CHECK (giaNhapSanPham > 0),
    giaBanSanPham DECIMAL(10, 2) NOT NULL CHECK (giaBanSanPham > 0),
    slSanPhamTonKho INT NOT NULL DEFAULT 0,
	slSanPhamDaBan INT DEFAULT 0 CHECK (slSanPhamDaBan >= 0),                     
    trangThaiSanPham BIT DEFAULT 1
);

-- Bảng Sản Phẩm - Đơn Hàng (Quan hệ Nhiều-Nhiều giữa Sản Phẩm và Đơn Hàng)
CREATE TABLE SP_DonHang (
    maDonHang INT,
    maSanPham INT,
    soLuong INT NOT NULL CHECK (soLuong > 0),
    PRIMARY KEY (maDonHang, maSanPham),
    FOREIGN KEY (maDonHang) REFERENCES DonHang(maDonHang) ON DELETE CASCADE,
    FOREIGN KEY (maSanPham) REFERENCES SanPham(maSanPham) ON DELETE CASCADE
);

CREATE TABLE KhoHang (
    maKhoHang INT IDENTITY(1,1) PRIMARY KEY,  
    ngay DATE NOT NULL, 
    tenKhoHang NVARCHAR(100) NOT NULL, 
    nguoiTao NVARCHAR(50) NOT NULL, 
    moTa NVARCHAR(225),
    maCongTy INT NOT NULL, -- Link to the company that owns the warehouse
    FOREIGN KEY (maCongTy) REFERENCES CongTyGiaoHang(maCongTy) ON DELETE CASCADE
);


CREATE TABLE ChiTietKhoHang (
    maChiTietKhoHang INT IDENTITY(1,1) PRIMARY KEY, -- Unique detail ID
    maKhoHang INT NOT NULL,                         -- References `KhoHang`
    maSanPham INT NOT NULL,                         -- References `SanPham`
    ngayNhap DATE NOT NULL,                         -- Date of product entry
    soLuong INT NOT NULL CHECK (soLuong >= 0),      -- Quantity of the product
    tongTien DECIMAL(18, 2)  CHECK (tongTien >= 0), -- Total value
    FOREIGN KEY (maKhoHang) REFERENCES KhoHang(maKhoHang) ON DELETE CASCADE,
    FOREIGN KEY (maSanPham) REFERENCES SanPham(maSanPham) ON DELETE CASCADE
);

/*
trigger tự động tính điểm tích lũy cho khách hàng, ngày sinh nhật khách hàng tự động trừ tiền hóa đơn
*/

select * from KhachHang

delete from DonHang where maDonHang = 6


CREATE VIEW vw_SanPham_DonHang AS
SELECT 
    sp.maSanPham AS [Mã SP],
    sp.tenSanPham AS [Tên SP],
    spd.soLuong AS [Số lượng],
    sp.giaBanSanPham AS [Giá bán sản phẩm],
    (spd.soLuong * sp.giaBanSanPham) AS [Thành tiền]
FROM 
    SP_DonHang spd
JOIN 
    SanPham sp ON spd.maSanPham = sp.maSanPham;


CREATE TRIGGER trig_UpdateThanhTien
ON SP_DonHang
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @maDonHang INT

    IF EXISTS(SELECT * FROM inserted)
        SELECT @maDonHang = maDonHang FROM inserted
    ELSE
        SELECT @maDonHang = maDonHang FROM deleted

    UPDATE DonHang
    SET thanhTien = (
        SELECT SUM(SP_DonHang.soLuong * SanPham.giaBanSanPham)
        FROM SP_DonHang
        JOIN SanPham ON SP_DonHang.maSanPham = SanPham.maSanPham
        WHERE SP_DonHang.maDonHang = @maDonHang
    )
    WHERE maDonHang = @maDonHang
END

CREATE TRIGGER trg_CapNhatSanPhamDaBan
ON SP_DonHang
AFTER INSERT, update
AS
BEGIN
    UPDATE sp
    SET sp.slSanPhamDaBan = sp.slSanPhamDaBan + i.soLuong
    FROM SanPham sp
    JOIN inserted i ON sp.maSanPham = i.maSanPham;
END;


select * from DonHang

CREATE FUNCTION fn_DemSanPhamTonKho()
RETURNS INT
AS
BEGIN
    DECLARE @soLuongTonKho INT;
    SELECT @soLuongTonKho = SUM(slSanPhamTonKho)
    FROM SanPham;
    RETURN @soLuongTonKho;
END;

CREATE FUNCTION fn_SanPhamHetHang()
RETURNS @SanPhamHetHang TABLE (
    maSanPham INT,
    tenSanPham NVARCHAR(255)
)
AS
BEGIN
    DECLARE @maSanPham INT;
    DECLARE @tenSanPham NVARCHAR(255);

    DECLARE sp_cursor CURSOR FOR
    SELECT maSanPham, tenSanPham
    FROM SanPham
    WHERE slSanPhamTonKho = 0;

    OPEN sp_cursor;
    FETCH NEXT FROM sp_cursor INTO @maSanPham, @tenSanPham;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO @SanPhamHetHang(maSanPham, tenSanPham)
        VALUES (@maSanPham, @tenSanPham);

        FETCH NEXT FROM sp_cursor INTO @maSanPham, @tenSanPham;
    END;

    CLOSE sp_cursor;
    DEALLOCATE sp_cursor;

    RETURN;
END;

CREATE PROCEDURE sp_CapNhatSoLuongSP
    @maSanPham INT, 
    @soLuongMoi INT
AS
BEGIN
    UPDATE SanPham
    SET slSanPhamTonKho = @soLuongMoi
    WHERE maSanPham = @maSanPham;
END;

CREATE PROCEDURE sp_CapNhatLuongNhanVien
    @maNhanvien INT,
    @luongMoi FLOAT
AS
BEGIN
    UPDATE NhanVien
    SET Luong = @luongMoi
    WHERE maNhanvien = @maNhanvien;
END;

CREATE FUNCTION func_TongDoanhThu()
RETURNS BIGINT
AS
BEGIN
    DECLARE @doanhThu BIGINT;
    SELECT @doanhThu = SUM(thanhTien)
    FROM DonHang;
    RETURN @doanhThu;
END;

CREATE VIEW view_KhachHangDonHang AS
SELECT 
    kh.maKhachhang,
    kh.tenKhachhang,
    dh.maDonHang,
    dh.thanhTien,
    dh.Ngaytaodon
FROM KhachHang kh
LEFT JOIN DonHang dh ON kh.maKhachhang = dh.maKhachHang;


use QLBH;




-- test db
-- Insert data into KhachHang
INSERT INTO KhachHang (tenKhachhang, soDienThoai, ghiChu, email, gioiTinh, diaChi)
VALUES
(N'Nguyen Van A', '0987654321', N'Khách VIP', 'nguyenvana@email.com', N'Nam', N'Hà Nội'),
(N'Tran Thi B', '0912345678', N'Thường xuyên mua hàng', 'tranthib@email.com', N'Nữ', N'Hồ Chí Minh');

-- Insert data into NhanVien
INSERT INTO NhanVien (tenNhanvien, soDienThoai, email, gioiTinh, Luong, diaChi)
VALUES
(N'Le Van C', '0981112233', 'levanc@email.com', N'Nam', 15000000, N'Đà Nẵng'),
(N'Pham Thi D', '0909988776', 'phamthid@email.com', N'Nữ', 17000000, N'Hải Phòng');

-- Insert data into TaiKhoan
INSERT INTO TaiKhoan (maNhanvien, tenTaiKhoan, matKhau, quyenQuanTri)
VALUES
(1, 'levanc', 'password123', 1),
(2, 'phamthid', 'secure456', 0);

-- Insert data into CongTyGiaoHang
INSERT INTO CongTyGiaoHang (tenCongTy, diaChi, tongDai, email, moTa)
VALUES
(N'CTGH A', N'123 Đường A', '0909090909', 'contact@ctgha.com', N'Công ty vận chuyển nhanh'),
(N'CTGH B', N'456 Đường B', '0919191919', 'contact@ctghb.com', N'Dịch vụ giao hàng tận nơi');

-- Insert data into NhaCungCap
INSERT INTO NhaCungCap (tenNhaCungCap, diaChi, soDienThoai, email)
VALUES
(N'NCC A', N'Địa chỉ A', '0922334455', 'ncca@email.com'),
(N'NCC B', N'Địa chỉ B', '0933445566', 'nccb@email.com');

-- Insert data into PhanPhoi
INSERT INTO PhanPhoi (maNhaCungCap, maCongTy)
VALUES
(1, 1),
(2, 2);

-- Insert data into KhuyenMai
INSERT INTO KhuyenMai (tenKhuyenMai, maKhuyenMai, phanTramGiamGia)
VALUES
(N'Khuyến mãi Tết', 'TET2024', 10.00),
(N'Khuyến mãi Hè', 'SUM2024', 15.00);

-- Insert data into DonHang
INSERT INTO DonHang (maKhachHang, thanhTien, hinhThucThanhToan, ghiChu, maNhanVien, maKhuyenMai, Ngaytaodon)
VALUES
(1, 500000, N'Tiền mặt', N'Giao nhanh', 1, 1, '2024-01-01'),
(2, 300000, N'Chuyển khoản', N'Đặt trước', 2, 2, '2024-02-01');

-- Insert data into SanPham
INSERT INTO SanPham (tenSanPham, giaNhapSanPham, giaBanSanPham, slSanPhamTonKho, slSanPhamDaBan, trangThaiSanPham)
VALUES
(N'Sản phẩm A', 100000, 150000, 50, 10, 1),
(N'Sản phẩm B', 200000, 250000, 30, 5, 1);

-- Insert data into SP_DonHang
INSERT INTO SP_DonHang (maDonHang, maSanPham, soLuong)
VALUES
(1, 1, 2),
(2, 2, 1);

-- Insert data into KhoHang
INSERT INTO KhoHang (ngay, tenKhoHang, nguoiTao, moTa, maCongTy)
VALUES
('2024-01-01', N'Kho A', N'Admin A', N'Kho chính', 1),
('2024-02-01', N'Kho B', N'Admin B', N'Kho dự trữ', 2);

-- Insert data into ChiTietKhoHang
INSERT INTO ChiTietKhoHang (maKhoHang, maSanPham, ngayNhap, soLuong, tongTien)
VALUES
(1, 1, '2024-01-02', 20, 2000000),
(2, 2, '2024-02-02', 15, 3000000);

select * from NhanVien
select * from DonHang