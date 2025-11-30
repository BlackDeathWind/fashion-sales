USE `fashion_sales`;

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
('Áo thun nam basic', 'ao-thun-nam-basic', 1, 'Áo thun nam form basic, chất cotton.', 199000, NULL, 100, 1, NOW(), 'https://unsplash.com/photos/two-black-and-white-shirts-sitting-next-to-each-other-pZfRXQhi1eg'),
('Đầm nữ dự tiệc', 'dam-nu-du-tiec', 2, 'Đầm nữ dự tiệc sang trọng.', 599000, 499000, 50, 1, NOW(), 'https://unsplash.com/photos/woman-in-dark-green-dress-with-belt-and-keyhole-back-euqyDvjNNqA');