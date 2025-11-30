## Kế hoạch triển khai hệ thống bán hàng thời trang (fashion-sales)

### 1. Yêu cầu môi trường

- .NET SDK 8.0 trở lên.
- MySQL Server 8.x, tài khoản `root` mật khẩu `21050043` (hoặc chỉnh lại trong `appsettings.json`).
- IDE khuyến nghị: Visual Studio 2022 / VS Code + C# extension.

### 2. Chuẩn bị cơ sở dữ liệu

1. Mở MySQL client (MySQL Workbench / CLI).
2. Chạy script:
   - `scripts/01_create_database.sql` – tạo database `fashion_sales`.
3. Cập nhật chuỗi kết nối trong `appsettings.json` nếu cần:
   - `ConnectionStrings:DefaultConnection`.
4. Từ thư mục project `fashion-sales`, chạy lệnh migrations (sau khi cài dotnet-ef nếu cần):
   - `dotnet ef migrations add InitialCreate`
   - `dotnet ef database update`
5. Chạy tiếp:
   - `scripts/03_schema_business.sql` (nếu chưa được tạo bởi migrations).
   - `scripts/04_seed_data.sql` để thêm danh mục và sản phẩm mẫu.

### 3. Chạy ứng dụng

1. Từ thư mục `fashion-sales`:
   - `dotnet run`
2. Mở trình duyệt tại `https://localhost:xxxxx` (port do ASP.NET Core cung cấp).

### 4. Tài khoản và phân quyền

- Khi ứng dụng khởi động, `DbInitializer.SeedAsync` sẽ:
  - Tạo các role: `Admin`, `Staff`, `Customer`.
  - Tạo tài khoản admin mặc định:
    - Email: `admin@fashionsales.local`
    - Mật khẩu: `Admin@12345`
    - Được gán role `Admin`.
- Người dùng đăng ký mới qua giao diện Identity sẽ được gán role `Customer` (cần cấu hình thêm nếu chưa).
- Tài khoản Staff có thể được tạo bởi Admin trong module quản lý tài khoản (phần Admin).

### 5. Chức năng chính đã triển khai

- **Guest/Customer**:
  - Trang chủ (`Home/Index`) với banner, danh mục nổi bật, khu vực sản phẩm nổi bật.
  - Duyệt danh mục (`Category/Index`) với grid sản phẩm, nút thêm vào giỏ.
  - Chi tiết sản phẩm (`Product/Details`) với gallery ảnh, thông tin, nút thêm giỏ.
  - Tìm kiếm sản phẩm (`Product/Search`).
  - Giỏ hàng (`Cart/Index`) lưu bằng session, cho phép chỉnh số lượng, xóa.
  - Đặt hàng (`Order/Checkout`), thanh toán giả lập, trang thành công, lịch sử đơn (`Order/MyOrders`).
- **Admin**:
  - Area Admin với Dashboard cơ bản.
  - Màn hình danh sách sản phẩm/danh mục (xem nhanh, có thể mở rộng CRUD).

### 6. Lộ trình mở rộng (theo đồ án)

1. Bổ sung CRUD đầy đủ cho:
   - `Admin/Categories` (thêm/sửa/xóa danh mục).
   - `Admin/Products` (thêm/sửa/xóa sản phẩm, upload ảnh).
2. Tạo `Admin/Orders` cho phép xem chi tiết và cập nhật trạng thái đơn hàng.
3. Tạo `Admin/Statistics` với các biểu đồ doanh thu, top sản phẩm.
4. Tạo `Admin/Users` để quản lý tài khoản (gán role Staff/Customer, khóa/mở tài khoản).
5. Tạo Area `Staff` (giống Admin nhưng giới hạn chức năng, không có Users).
6. Hoàn thiện tài liệu đồ án:
   - Biểu đồ use case, activity.
   - ERD.
   - Hướng dẫn cài đặt, demo màn hình.


