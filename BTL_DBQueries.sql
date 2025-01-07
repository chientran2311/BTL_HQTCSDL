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

--------------------------------------------------------------View-------------------------------------------------------------------------
CREATE VIEW view_Top5SanPhamBanChay AS
SELECT 
    sp.maSanPham,
    sp.tenSanPham,
    SUM(spd.soLuong) AS TongSoLuongBan
FROM 
    SanPham sp
JOIN 
    SP_DonHang spd ON sp.maSanPham = spd.maSanPham
JOIN 
    DonHang dh ON spd.maDonHang = dh.maDonHang
WHERE 
    YEAR(dh.NgayTaoDon) = YEAR(GETDATE())
GROUP BY 
    sp.maSanPham, sp.tenSanPham
ORDER BY 
    TongSoLuongBan DESC
OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY;

CREATE VIEW view_Top5KhachHangMuaHangNhieuNhat AS
SELECT TOP 5
    kh.maKhachhang,
    kh.tenKhachhang,
    kh.soDienThoai,
    kh.email,
    SUM(dh.thanhTien) AS TongTienMuaHang
FROM 
    DonHang dh
JOIN 
    KhachHang kh ON dh.maKhachHang = kh.maKhachhang
WHERE 
    dh.Ngaytaodon >= DATEADD(YEAR, -1, GETDATE())
GROUP BY 
    kh.maKhachhang, kh.tenKhachhang, kh.soDienThoai, kh.email
ORDER BY 
    TongTienMuaHang DESC;

select * from view_Top5KhachHangMuaHangNhieuNhat



select * from view_KhachHangDonHang

--------------------------------------------------------------Trigger-------------------------------------------------------------------------

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

CREATE TRIGGER trig_UpdateSLSanPham	
ON SP_DonHang
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        UPDATE sp
        SET 
            sp.slSanPhamDaBan = sp.slSanPhamDaBan + i.soLuong,
            sp.slSanPhamTonKho = sp.slSanPhamTonKho - i.soLuong
        FROM SanPham sp
        JOIN inserted i ON sp.maSanPham = i.maSanPham;

        IF EXISTS (
            SELECT 1
            FROM SanPham sp
            JOIN inserted i ON sp.maSanPham = i.maSanPham
            WHERE sp.slSanPhamTonKho < 0
        )
        BEGIN
            RAISERROR ('Số lượng tồn kho không đủ.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
    END

    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        UPDATE sp
        SET 
            sp.slSanPhamDaBan = sp.slSanPhamDaBan - d.soLuong,
            sp.slSanPhamTonKho = sp.slSanPhamTonKho + d.soLuong
        FROM SanPham sp
        JOIN deleted d ON sp.maSanPham = d.maSanPham;

        IF EXISTS (
            SELECT 1
            FROM SanPham sp
            JOIN deleted d ON sp.maSanPham = d.maSanPham
            WHERE sp.slSanPhamDaBan < 0
        )
        BEGIN
            RAISERROR ('Số lượng đã bán không hợp lệ.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
    END
END;


--------------------------------------------------------------Function-------------------------------------------------------------------------

CREATE FUNCTION func_DonHangKhachHang(@soDienThoai VARCHAR(15))
RETURNS TABLE
AS
RETURN
(
    SELECT 
        dh.maDonHang,
        dh.Ngaytaodon,
        dh.thanhTien,
        dh.hinhThucThanhToan,
        dh.ghiChu,
        kh.tenKhachhang,
        kh.soDienThoai
    FROM 
        DonHang dh
    JOIN 
        KhachHang kh ON dh.maKhachHang = kh.maKhachhang
    WHERE 
        kh.soDienThoai = @soDienThoai
);

select * from dbo.fn_DonHangKhachHang('1')

CREATE FUNCTION func_TinhTienSauKhuyenMai
(
    @maDonHang INT,
    @maKhuyenMai VARCHAR(50)
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @tongTien DECIMAL(18, 2);
    DECLARE @phanTramGiamGia DECIMAL(5, 2);

    SELECT @phanTramGiamGia = phanTramGiamGia
    FROM KhuyenMai
    WHERE maKhuyenMai = @maKhuyenMai;

    IF @phanTramGiamGia IS NULL
    BEGIN
        RETURN 0;
    END

    SELECT @tongTien = SUM(spd.soLuong * sp.giaBanSanPham)
    FROM SP_DonHang spd
    JOIN SanPham sp ON spd.maSanPham = sp.maSanPham
    WHERE spd.maDonHang = @maDonHang;

    RETURN (@tongTien - (@tongTien * @phanTramGiamGia / 100));
END;


CREATE FUNCTION func_Top5SanPhamBanChayNhat()
RETURNS TABLE
AS
RETURN
(
    SELECT TOP 5 sp.maSanPham, sp.tenSanPham, SUM(spd.soLuong) AS tongSoLuong
    FROM SP_DonHang spd
    JOIN SanPham sp ON spd.maSanPham = sp.maSanPham
    GROUP BY sp.maSanPham, sp.tenSanPham
    ORDER BY tongSoLuong DESC
);

select * from dbo.func_SanPhamBanChayNhat()

CREATE FUNCTION func_SanPhamHetHang()
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

select * from dbo.func_SanPhamConTrongKho()
--------------------------------------------------------------Procedure-------------------------------------------------------------------------

CREATE PROCEDURE sp_KhachHangThanThiet
    @nam INT 
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        kh.maKhachHang,
        kh.tenKhachHang,
        kh.soDienThoai,
        kh.email,
        COUNT(dh.maDonHang) AS TongSoDonHang,
        SUM(spd.soLuong * sp.giaBanSanPham) AS TongDoanhThu
    FROM 
        KhachHang kh
    JOIN 
        DonHang dh ON kh.maKhachHang = dh.maKhachHang
    JOIN 
        SP_DonHang spd ON dh.maDonHang = spd.maDonHang
    JOIN 
        SanPham sp ON spd.maSanPham = sp.maSanPham
    WHERE 
        YEAR(dh.NgayTaoDon) = @nam 
    GROUP BY 
        kh.maKhachHang, kh.tenKhachHang, kh.soDienThoai, kh.email
    HAVING 
        SUM(spd.soLuong * sp.giaBanSanPham) > 10000000 
    ORDER BY 
        TongDoanhThu DESC; 
END;


exec sp_KhachHangThanThiet 2023

CREATE PROCEDURE sp_TinhTongDoanhThu
    @NgayBatDau DATE, 
    @NgayKetThuc DATE 
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        SUM(thanhTien) AS TongDoanhThu
    FROM 
        DonHang
    WHERE 
        Ngaytaodon BETWEEN @NgayBatDau AND @NgayKetThuc;
END

--------------------------------------------------------------Cursor-------------------------------------------------------------------------

----Cur1----

DECLARE @maKhachHang INT
DECLARE @maDonHang INT
DECLARE @doanhThu DECIMAL(18, 2)

DECLARE cur_DoanhThuKhachHang CURSOR FOR
SELECT DISTINCT dh.maKhachHang
FROM DonHang dh
WHERE dh.Ngaytaodon BETWEEN '2024-01-01' AND '2024-12-31'

OPEN cur_DoanhThuKhachHang

FETCH NEXT FROM cur_DoanhThuKhachHang INTO @maKhachHang

WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT @doanhThu = ISNULL(SUM(dh.thanhTien), 0)
    FROM DonHang dh
    WHERE dh.maKhachHang = @maKhachHang
    AND dh.Ngaytaodon BETWEEN '2024-01-01' AND '2024-12-31'

    PRINT N'Khách hàng ID: ' + CAST(@maKhachHang AS NVARCHAR(10)) 
	+ N' có tổng tiền đã mua trong năm 2024 là: ' + CAST(@doanhThu AS NVARCHAR(18))

    FETCH NEXT FROM cur_DoanhThuKhachHang INTO @maKhachHang
END

CLOSE cur_DoanhThuKhachHang
DEALLOCATE cur_DoanhThuKhachHang



----Cur2----
DECLARE @maSanPham INT
DECLARE @slTonKho INT
DECLARE @trangThaiSanPham BIT

DECLARE @MaspInput TABLE (maSanPham INT)

INSERT INTO @MaspInput (maSanPham)
VALUES (1), (2)

DECLARE cur_CheckSanPham CURSOR FOR
SELECT maSanPham
FROM @MaspInput

OPEN cur_CheckSanPham

FETCH NEXT FROM cur_CheckSanPham INTO @maSanPham

WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT @slTonKho = slSanPhamTonKho, @trangThaiSanPham = trangThaiSanPham
    FROM SanPham
    WHERE maSanPham = @maSanPham

    IF @slTonKho > 0 AND @trangThaiSanPham = 0
    BEGIN
        PRINT N'Sản phẩm ID: ' + CAST(@maSanPham AS NVARCHAR(10)) + N' vẫn còn hàng nhưng đã ngừng kinh doanh.'
    END

    ELSE IF @slTonKho = 0
    BEGIN
        UPDATE SanPham
        SET trangThaiSanPham = 0
        WHERE maSanPham = @maSanPham

        PRINT N'Sản phẩm ID: ' + CAST(@maSanPham AS NVARCHAR(10)) + N' đã ngừng bán do hết hàng.'
    END

    ELSE
		BEGIN
			UPDATE SanPham
			SET trangThaiSanPham = 1
			WHERE maSanPham = @maSanPham

			PRINT N'Sản phẩm ID: ' + CAST(@maSanPham AS NVARCHAR(10)) + N' vẫn đang bán.'
		END

    FETCH NEXT FROM cur_CheckSanPham INTO @maSanPham
END

CLOSE cur_CheckSanPham
DEALLOCATE cur_CheckSanPham




--------------------------------------------------------------DB Insert-------------------------------------------------------------------------

-- Khách hàng 1: Nguyễn Văn A (50 đơn hàng, mỗi tháng ít nhất 1 đơn)
INSERT INTO DonHang (maKhachHang, thanhTien, hinhThucThanhToan, ghiChu, maNhanVien, maKhuyenMai, Ngaytaodon)
SELECT 
    1, 1000000, N'Tiền mặt', N'Mua hàng', 1, NULL, DATEADD(MONTH, (number % 12), '2023-01-01')
FROM 
    master.dbo.spt_values
WHERE 
    type = 'P' AND number BETWEEN 1 AND 50;

-- Khách hàng 2: Trần Thị B (35 đơn hàng, mỗi tháng ít nhất 1 đơn)
INSERT INTO DonHang (maKhachHang, thanhTien, hinhThucThanhToan, ghiChu, maNhanVien, maKhuyenMai, Ngaytaodon)
SELECT 
    2, 800000, N'Chuyển khoản', N'Khách VIP', 2, NULL, DATEADD(MONTH, (number % 12), '2023-01-01')
FROM 
    master.dbo.spt_values
WHERE 
    type = 'P' AND number BETWEEN 1 AND 35;

select * from DonHang



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