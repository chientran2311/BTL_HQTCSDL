create Database QLBH;
use QLBH;
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


--Đồ trong kho






INSERT INTO SanPham (tenSanPham, giaNhapSanPham, giaBanSanPham, slSanPhamTonKho)
VALUES 
    (N'Điện thoại iPhone 14', 20000000, 24000000, 100),
    (N'Laptop Dell XPS 15', 30000000, 35000000, 50),
    (N'Tai nghe AirPods Pro', 4000000, 5000000, 200),
    (N'Máy ảnh Sony A7 III', 35000000, 40000000, 30),
    (N'Tivi Samsung 55 Inch', 10000000, 12000000, 70),
    (N'Tủ lạnh LG 300L', 8000000, 10000000, 40),
    (N'Máy giặt Electrolux 9kg', 7000000, 8500000, 60),
    (N'Điều hòa Daikin Inverter', 12000000, 15000000, 35),
    (N'Loa JBL Charge 5', 3000000, 4000000, 100),
    (N'Máy tính bảng iPad Air 2022', 18000000, 20000000, 25);




	INSERT INTO CongTyGiaoHang (tenCongTy, diaChi, tongDai, email, moTa)
VALUES 
(N'Giao Hàng Nhanh', N'123 Đường Lý Thường Kiệt, Quận Tân Bình, TP. Hồ Chí Minh', N'0908123456', N'contact@ghn.vn', N'Dịch vụ giao hàng nhanh nội thành.'),
(N'Giao Hàng Tiết Kiệm', N'456 Đường Cộng Hòa, Quận Tân Bình, TP. Hồ Chí Minh', N'0908234567', N'info@ghtk.vn', N'Dịch vụ giao hàng tiết kiệm.'),
(N'Viettel Post', N'789 Đường Hoàng Hoa Thám, Quận Tân Bình, TP. Hồ Chí Minh', N'0908345678', N'contact@viettelpost.vn', N'Dịch vụ chuyển phát nhanh của Viettel.');

INSERT INTO NhaCungCap (tenNhaCungCap, diaChi, soDienThoai, email)
VALUES 
(N'Công Ty Sản Xuất Đồ Gia Dụng A', N'12 Đường Trần Hưng Đạo, Quận 1, TP. Hồ Chí Minh', N'0909123456', N'dogiadungA@gmail.com'),
(N'Công Ty Sản Xuất Đồ Điện B', N'34 Đường Nguyễn Văn Cừ, Quận 5, TP. Hồ Chí Minh', N'0909234567', N'dodienB@gmail.com'),
(N'Công Ty Thực Phẩm Sạch C', N'56 Đường Điện Biên Phủ, Quận Bình Thạnh, TP. Hồ Chí Minh', N'0909345678', N'thucphamsachC@gmail.com');


INSERT INTO PhanPhoi (maNhaCungCap, maCongTy)
VALUES 
(1, 1), -- Nhà cung cấp A phân phối qua Giao Hàng Nhanh
(1, 2), -- Nhà cung cấp A phân phối qua Giao Hàng Tiết Kiệm
(2, 1), -- Nhà cung cấp B phân phối qua Giao Hàng Nhanh
(2, 3), -- Nhà cung cấp B phân phối qua Viettel Post
(3, 2), -- Nhà cung cấp C phân phối qua Giao Hàng Tiết Kiệm
(3, 3); -- Nhà cung cấp C phân phối qua Viettel Post





------------------------Function 1
CREATE FUNCTION dbo.TinhTongTien (
    @SoLuong INT,          -- Số lượng sản phẩm
    @DonGia DECIMAL(10, 2) -- Giá bán sản phẩm
)
RETURNS DECIMAL(18, 2) -- Kiểu dữ liệu trả về là số thập phân
AS
BEGIN
    RETURN @SoLuong * @DonGia; -- Công thức tính tổng tiền
END;



---------------------2.
create function dbo.TinhSoLuongHangTonKho(@MaSanPham int)
returns int
as
begin
declare @SoLuongTonKho int;

select @SoLuongTonKho = sum(soLuong)
from ChiTietKhoHang
where maSanPham = @MaSanPham

return @SoLuongTonKho
end


select  dbo.TinhSoLuongHangTonkho(2) as So_Luong_Hang_Trong_Kho





   
              ---------------------------------Trigger1          
CREATE TRIGGER TinhTongTienKhoHang
ON ChiTietKhoHang
FOR INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;  -- To prevent extra result sets from being returned

    -- Update the `tongTien` column for newly inserted or updated rows
    UPDATE ChiTietKhoHang
    SET tongTien = dbo.TinhTongTien(i.soLuong, sp.giaNhapSanPham)
    FROM ChiTietKhoHang ctkh
    INNER JOIN inserted i ON ctkh.maChiTietKhoHang = i.maChiTietKhoHang
    INNER JOIN SanPham sp ON i.maSanPham = sp.maSanPham;

    PRINT N'Tong tien đã được cập nhật';
END;




------------------trigg2
CREATE TRIGGER CheckStockLimit
ON ChiTietKhoHang
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @maKhoHang INT, @tongSoLuong INT, @tenKhoHang NVARCHAR(50);
    DECLARE cursor_checkStock CURSOR FOR
    SELECT 
        ChiTietKhoHang.maKhoHang,
        KhoHang.tenKhoHang,
        SUM(soLuong) AS tongSoLuong
    FROM 
        ChiTietKhoHang
    JOIN 
        KhoHang ON ChiTietKhoHang.maKhoHang = KhoHang.maKhoHang
    GROUP BY 
        ChiTietKhoHang.maKhoHang, KhoHang.tenKhoHang;
    OPEN cursor_checkStock;
    FETCH NEXT FROM cursor_checkStock INTO @maKhoHang, @tenKhoHang, @tongSoLuong;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @tongSoLuong > 100
        BEGIN
            RAISERROR(
                N'Tổng số lượng trong kho %s vượt qua 100 sản phẩm. Tổng hiện tại: %d.',
                16, 1, @tenKhoHang, @tongSoLuong
            );
            ROLLBACK TRANSACTION;
            BREAK;
        END;
        FETCH NEXT FROM cursor_checkStock INTO @maKhoHang, @tenKhoHang, @tongSoLuong;
    END;
    CLOSE cursor_checkStock;
    DEALLOCATE cursor_checkStock;
END;








----------------------------------------------------------VIew 1,2
CREATE VIEW view_ChiTietKhoHang AS
SELECT 
maChiTietKhoHang,
    KhoHang.maKhoHang, 
	
    ChiTietKhoHang.maSanPham, 
	
   soLuong,
	ngayNhap,
	tongTien
FROM 
    ChiTietKhoHang
Join KhoHang on KhoHang.maKhoHang = ChiTietKhoHang.maKhoHang
Join SanPham on SanPham.maSanPham = ChiTietKhoHang.maSanPham
GROUP BY 
    maChiTietKhoHang,KhoHang.maKhoHang, ChiTietKhoHang.maSanPham ,soLuong,ngayNhap,tongTien




	


SELECT * 
FROM view_ChiTietKhoHang
ORDER BY maSanPham;



-----------View 2

CREATE VIEW view_TongHangTonKhoCuaTungKho as
 SELECT 
    maKhoHang, 
    SanPham.maSanPham, 
	SanPham.tenSanPham,
    SUM(soLuong) AS tongSoLuong
	
FROM 
    ChiTietKhoHang
	join SanPham on SanPham.maSanPham = ChiTietKhoHang.maSanPham
GROUP BY 
    maKhoHang, SanPham.maSanPham , SanPham.tenSanPham 


  ----------------------------------------------------------------------proc1
CREATE PROCEDURE sp_InsertKhoHang
    @ngayNhap DATE,
    @maSanPham INT,
    @soLuong INT,
    @maKhoHang INT
AS
BEGIN
SET NOCOUNT ON
    BEGIN TRANSACTION;

    BEGIN TRY
    
        UPDATE SanPham
        SET slSanPhamTonKho = slSanPhamTonKho + @soLuong
        WHERE maSanPham = @maSanPham;

     
        INSERT INTO ChiTietKhoHang (maKhoHang, maSanPham, ngayNhap, soLuong, tongTien)
        VALUES (@maKhoHang, @maSanPham, @ngayNhap, @soLuong, NULL); 

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
----------------------------------

ALTER PROCEDURE sp_InsertKhoHang
    @ngayNhap DATE,
    @tenSanPham NVARCHAR(50), -- Thay vì maSanPham, sử dụng tên sản phẩm
    @soLuong INT,
    @maKhoHang INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @maSanPham INT;

	    SELECT @maSanPham = maSanPham
		FROM SanPham
		WHERE tenSanPham = @tenSanPham;


        -- Cập nhật số lượng tồn kho trong bảng SanPham
        UPDATE SanPham
        SET slSanPhamTonKho = slSanPhamTonKho + @soLuong
        WHERE maSanPham = @maSanPham;

        -- Chèn thông tin vào bảng ChiTietKhoHang
        INSERT INTO ChiTietKhoHang (maKhoHang, maSanPham, ngayNhap, soLuong, tongTien)
        VALUES (@maKhoHang, @maSanPham, @ngayNhap, @soLuong, NULL); 

        -- Xác nhận giao dịch thành công
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Hủy giao dịch nếu có lỗi xảy ra
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;


EXEC sp_InsertKhoHang 
    @ngayNhap = '2024-01-01',  -- Replace with the actual date of entry
    @tenSanPham = N'Điện thoại iPhone 14',    -- Replace with the actual Product ID
    @soLuong = 3,       -- Replace with the quantity being added
    @maKhoHang = 5;  -- Replace with the actual Warehouse ID




CREATE PROCEDURE sp_TraCuuKhoHang
    @tenKhoHang NVARCHAR(50) = NULL,
    @tenSanPham NVARCHAR(50) = NULL,
    @maKhoHang INT = NULL,
    @maSanPham INT = NULL
AS
BEGIN
    SELECT 
        ChiTietKhoHang.maChiTietKhoHang,
        ChiTietKhoHang.maKhoHang,
        SanPham.maSanPham,
        SanPham.tenSanPham,
        ChiTietKhoHang.soLuong,
        ChiTietKhoHang.ngayNhap,
        ChiTietKhoHang.tongTien
    FROM ChiTietKhoHang
    JOIN SanPham ON ChiTietKhoHang.maSanPham = SanPham.maSanPham
    JOIN KhoHang ON KhoHang.maKhoHang = ChiTietKhoHang.maKhoHang
    WHERE 
        (@tenKhoHang IS NULL OR KhoHang.tenKhoHang = @tenKhoHang) AND
        (@tenSanPham IS NULL OR SanPham.tenSanPham = @tenSanPham) 
        
END;



		--Testing 
EXEC InsertKhoHang
    @maKhoHang = 7,
    @maSanPham = 3,
    @ngayNhap = '2024-10-12',
    @soLuong = 6;





--proc 3


CREATE PROCEDURE sp_DeleteKhoHang
    @ngayNhap DATE,
    @maSanPham INT,
    @soLuong INT,
    @maKhoHang INT,
	@maChiTietKhoHang INT
AS
BEGIN
BEGIN TRANSACTION;

BEGIN TRY

    UPDATE SanPham
    SET slSanPhamTonKho = slSanPhamTonKho - @soLuong
    WHERE maSanPham = @maSanPham;

    IF (SELECT slSanPhamTonKho FROM SanPham WHERE maSanPham = @maSanPham) < 0
    BEGIN
        RAISERROR ('Số lượng tồn kho không thể âm.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;
    -- Xóa chi tiết sản phẩm trong kho
    DELETE FROM ChiTietKhoHang
    WHERE maSanPham = @maSanPham and maChiTietKhoHang = @maChiTietKhoHang and ngayNhap=@ngayNhap and soLuong = @soLuong ;
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH;
END;

EXEC DeleteKhoHang
    @ngayNhap = '2024-1-1',  -- Thay bằng ngày nhập thực tế
    @maSanPham = 1,          -- Thay bằng mã sản phẩm thực tế
    @soLuong = 3,             -- Thay bằng số lượng thực tế
    @maKhoHang = 5,            -- Thay bằng mã kho hàng thực tế
    @maChiTietKhoHang = 2;  -- Thay bằng mã chi tiết kho hàng thực tế









--cursor
DECLARE @maChiTietKhoHang INT, @maKhoHang INT, @maSanPham INT, @ngayNhap DATE;

DECLARE cursor_CheckKhoHang CURSOR FOR
SELECT maChiTietKhoHang, maKhoHang, maSanPham, ngayNhap
FROM ChiTietKhoHang
WHERE DATEDIFF(MONTH, ngayNhap, GETDATE()) > 6;
OPEN cursor_CheckKhoHang;
FETCH NEXT FROM cursor_CheckKhoHang INTO @maChiTietKhoHang, @maKhoHang, @maSanPham, @ngayNhap;
WHILE @@FETCH_STATUS = 0
BEGIN
    PRINT N'Chi tiết hàng tồn kho: Mã ' + CAST(@maChiTietKhoHang AS NVARCHAR) +
          N', Kho ' + CAST(@maKhoHang AS NVARCHAR) +
          N', Sản phẩm ' + CAST(@maSanPham AS NVARCHAR) +
          N', Ngày nhập: ' + CONVERT(NVARCHAR, @ngayNhap, 103) +
          N' đã tồn tại trên 6 tháng.';

    FETCH NEXT FROM cursor_CheckKhoHang INTO @maChiTietKhoHang, @maKhoHang, @maSanPham, @ngayNhap;
END;
CLOSE cursor_CheckKhoHang;
DEALLOCATE cursor_CheckKhoHang;

select * from ChiTietKhoHang










	-- 1. Tạo câu lệnh để xóa tất cả các ràng buộc khóa ngoại
DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += 'ALTER TABLE ' + t.name + ' DROP CONSTRAINT ' + fk.name + ';' + CHAR(13)
FROM sys.foreign_keys AS fk
INNER JOIN sys.tables AS t
    ON fk.parent_object_id = t.object_id;

-- 2. Thực thi câu lệnh xóa khóa ngoại
EXEC sp_executesql @sql;

-- 3. Tạo câu lệnh để xóa tất cả các bảng
DECLARE @sqlDropTables NVARCHAR(MAX) = N'';

SELECT @sqlDropTables += 'DROP TABLE ' + t.name + ';' + CHAR(13)
FROM sys.tables AS t;

-- 4. Thực thi câu lệnh xóa bảng
EXEC sp_executesql @sqlDropTables;
