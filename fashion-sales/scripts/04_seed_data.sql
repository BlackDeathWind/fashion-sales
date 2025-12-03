USE `fashion_sales`;

-- ============================================
-- SEED DANH MỤC & SẢN PHẨM MẪU
-- ============================================

-- Seed danh mục mẫu
INSERT INTO `Categories` (`Name`, `Slug`, `ParentId`, `IsActive`, `DisplayOrder`)
VALUES
('Nam', 'nam', NULL, 1, 1),
('Nữ', 'nu', NULL, 1, 2),
('Trẻ em', 'tre-em', NULL, 1, 3),
('Phụ kiện', 'phu-kien', NULL, 1, 4);

-- Ví dụ seed sản phẩm (sau khi có dữ liệu thật có thể xoá/sửa)
INSERT INTO `Products`
(`Name`, `Slug`, `CategoryId`, `Description`, `Price`, `DiscountPrice`, `StockQuantity`, `IsActive`, `CreatedAt`, `MainImageUrl`)
VALUES
('Áo thun nam basic', 'ao-thun-nam-basic', 1, 'Áo thun nam form basic, chất cotton.', 199000, NULL, 100, 1, NOW(), 'https://images.unsplash.com/photo-1521572267360-ee0c2909d518?auto=format&fit=crop&q=80&w=800'),
('Đầm nữ dự tiệc', 'dam-nu-du-tiec', 2, 'Đầm nữ dự tiệc sang trọng.', 599000, 499000, 50, 1, NOW(), 'https://images.unsplash.com/photo-1543076447-215ad9ba6923?auto=format&fit=crop&q=80&w=800');

-- ============================================
-- THÔNG TIN THAM KHẢO TÀI KHOẢN ADMIN / STAFF
-- (Credentials thực tế được cấu hình trong appsettings / secrets,
--  không lưu trực tiếp password trong script SQL để tránh lộ bí mật.)
-- ============================================
--
-- Admin:
--   Email    : admin@fashionsales.local
--   Mật khẩu : Admin@12345
--
-- Staff:
--   Email    : staff@fashionsales.local
--   Mật khẩu : Staff@12345
--
-- Hai tài khoản này được tạo tự động bởi DbInitializer thông qua Identity,
-- dựa trên cấu hình trong phần SeedUsers của appsettings.json.
