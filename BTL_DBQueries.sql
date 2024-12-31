create Database QLBH;
use QLBH;

drop database QLBH

CREATE TABLE KhachHang (
    maKhachhang INT IDENTITY(1,1) PRIMARY KEY, 
    tenKhachhang NVARCHAR(100),
    soDienThoai VARCHAR(15),
    ghiChu NVARCHAR(225),
    email VARCHAR(50),
    gioiTinh NVARCHAR(10) CHECK (gioiTinh = N'Nam' OR gioiTinh = N'Nữ'),
    diaChi VARCHAR(100)
);

-- Bảng Nhân Viên
CREATE TABLE NhanVien (
    maNhanvien INT IDENTITY(1,1) PRIMARY KEY, 
    tenNhanvien NVARCHAR(100),
    soDienThoai VARCHAR(15),
    email VARCHAR(50),
    gioiTinh NVARCHAR(10) CHECK (gioiTinh = N'Nam' OR gioiTinh = N'Nữ'),
    Luong FLOAT,
    diaChi VARCHAR(100)
);

-- Bảng Tài Khoản
CREATE TABLE TaiKhoan (
    maNhanvien INT, 
    tenTaiKhoan VARCHAR(30) UNIQUE, 
    matKhau VARCHAR(50),
    quyenQuanTri BIT,
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
    phanTramGiamGia DECIMAL(5, 2) NOT NULL
);

-- Bảng Đơn Hàng (Bỏ tham chiếu đến công ty giao hàng, chỉ tham chiếu đến nhân viên)
CREATE TABLE DonHang (
    maDonHang INT IDENTITY(1,1) PRIMARY KEY, 
    maKhachHang INT,
    thanhTien BIGINT,
    ghiChu NVARCHAR(225),
    maNhanVien INT,
    maKhuyenMai INT NULL,
    Ngaytaodon DATE,
    FOREIGN KEY (maNhanVien) REFERENCES NhanVien(maNhanvien) ON DELETE CASCADE,
    FOREIGN KEY (maKhachHang) REFERENCES KhachHang(maKhachhang) ON DELETE CASCADE,
    FOREIGN KEY (maKhuyenMai) REFERENCES KhuyenMai(id) ON DELETE SET NULL
);

-- Bảng Sản Phẩm
CREATE TABLE SanPham (
    maSanPham INT IDENTITY(1,1) PRIMARY KEY,                     
    tenSanPham NVARCHAR(255) NOT NULL,  
    giaNhapSanPham DECIMAL(10, 2) NOT NULL,
    giaBanSanPham DECIMAL(10, 2) NOT NULL,         
    slSanPhamTonKho INT NOT NULL,    
    slSanPhamDaBan INT DEFAULT 0,                     
    trangThaiSanPham BIT DEFAULT 1
);

-- Bảng Sản Phẩm - Đơn Hàng
CREATE TABLE SP_DonHang (
    maDonHang INT,
    maSanPham INT,
    soLuong INT,
	PRIMARY KEY (maDonHang, maSanPham),
    FOREIGN KEY (maDonHang) REFERENCES DonHang(maDonHang) ON DELETE CASCADE,
    FOREIGN KEY (maSanPham) REFERENCES SanPham(maSanPham) ON DELETE CASCADE
);

-- Bảng Kho Hàng (Tham chiếu đến công ty giao hàng)
CREATE TABLE KhoHang (
    maKhoHang INT IDENTITY(1,1) PRIMARY KEY, 
    ngay DATE, 
    TenKhohang NVARCHAR(100), 
    maSanPham INT,
    soLuong INT, 
    tongTien INT,
    nguoiTao NVARCHAR(50), 
    moTa NVARCHAR(225),
    maCongTy INT, 
    FOREIGN KEY (maSanPham) REFERENCES SanPham(maSanPham) ON DELETE CASCADE,
    FOREIGN KEY (maCongTy) REFERENCES CongTyGiaoHang(maCongTy) ON DELETE CASCADE
);

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

/*
trigger tự động tính điểm tích lũy cho khách hàng, ngày sinh nhật khách hàng tự động trừ tiền hóa đơn
*/

INSERT INTO KhachHang (tenKhachhang, soDienThoai, ghiChu, email, gioiTinh, diaChi)
VALUES 
(N'Nguyễn Văn A', '0987654321', N'Khách hàng thường xuyên', 'vana@gmail.com', N'Nam', N'Hà Nội'),
(N'Trần Thị B', '0912345678', N'Khách hàng tiềm năng', 'thib@gmail.com', N'Nữ', N'Đà Nẵng'),
(N'Lê Văn C', '0978123456', N'Khách hàng mới', 'levanc@gmail.com', N'Nam', N'Hồ Chí Minh');

INSERT INTO NhanVien (tenNhanvien, soDienThoai, email, gioiTinh, Luong, diaChi)
VALUES 
(N'Nguyễn Văn D', '0901234567', 'vandung@gmail.com', N'Nam', 10000000, N'Hà Nội'),
(N'Trần Thị E', '0912987654', 'thie@gmail.com', N'Nữ', 12000000, N'Đà Nẵng');


INSERT INTO CongTyGiaoHang (tenCongTy, diaChi, tongDai, email, moTa)
VALUES 
(N'Giao Nhanh Express', N'Hà Nội', '18001111', 'info@gnexpress.com', N'Vận chuyển nhanh chóng'),
(N'Chuyển Phát Nhanh', N'Đà Nẵng', '18002222', 'info@cpnhanh.com', N'Vận chuyển an toàn');

INSERT INTO NhaCungCap (tenNhaCungCap, diaChi, soDienThoai, email)
VALUES 
(N'Công ty ABC', N'Hồ Chí Minh', '0281234567', 'contact@abc.com'),
(N'Công ty XYZ', N'Hà Nội', '0249876543', 'contact@xyz.com');

INSERT INTO KhuyenMai (tenKhuyenMai, maKhuyenMai, phanTramGiamGia)
VALUES 
(N'Giảm 10%', 'KM10', 10.00),
(N'Giảm 20%', 'KM20', 20.00);

INSERT INTO SanPham (tenSanPham, giaNhapSanPham, giaBanSanPham, slSanPhamTonKho, slSanPhamDaBan, trangThaiSanPham)
VALUES 
(N'Điện thoại iPhone 13', 15000000, 20000000, 50, 10, 1),
(N'Laptop Dell XPS 13', 25000000, 30000000, 30, 5, 1),
(N'Tai nghe Bluetooth Sony', 500000, 800000, 100, 20, 1);

INSERT INTO DonHang (maKhachHang, thanhTien, ghiChu, maNhanVien, maKhuyenMai, Ngaytaodon)
VALUES 
(1, 0, N'Đơn hàng mẫu 1', 1, 1, '2024-12-30'),
(2, 0, N'Đơn hàng mẫu 2', 2, 2, '2024-12-30');

INSERT INTO SP_DonHang (maDonHang, maSanPham, soLuong)
VALUES 
(1, 1, 2), -- iPhone 13, 2 cái
(1, 2, 1), -- Laptop Dell XPS 13, 1 cái
(2, 3, 3); -- Tai nghe Bluetooth Sony, 3 cái

INSERT INTO KhoHang (ngay, TenKhohang, maSanPham, soLuong, tongTien, nguoiTao, moTa, maCongTy)
VALUES 
('2024-12-01', N'Kho Hà Nội', 1, 20, 400000000, N'Nguyễn Văn D', N'Hàng điện tử', 1),
('2024-12-02', N'Kho Đà Nẵng', 2, 10, 300000000, N'Trần Thị E', N'Hàng laptop', 2);

select * from DonHang

delete from DonHang where maDonHang = 6