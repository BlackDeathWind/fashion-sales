## Kế hoạch tổng quan xây dựng website bán thời trang (ASP.NET Core MVC + MySQL)

### 1. Kiến trúc tổng thể & công nghệ

- **Stack chính**
- ASP.NET Core **8** (MVC pattern), Razor Views.
- ORM: **Entity Framework Core** MySQL provider (Pomelo.EntityFrameworkCore.MySql).
- CSDL: **MySQL 8+**, user `root`, password `21050043`.
- Auth: **ASP.NET Core Identity** tích hợp với MySQL, mở rộng bảng user để chứa thông tin khách hàng/nhân viên.
- **Tổ chức solution**
- Project web duy nhất: `FashionSales.Web` (MVC).
- Các layer logic tách qua folder:
- `Models/Entities` (thực thể EF).
- `Models/ViewModels` (model cho View, DTO).
- `Data` (DbContext, cấu hình Identity, seed data).
- `Services` (dịch vụ domain: đặt hàng, thống kê).
- `Areas/Admin` và `Areas/Staff` (khu vực quản trị).

### 2. Phân tích vai trò & phân quyền

- **Vai trò hệ thống**
- `Guest` (không đăng nhập):
- Tìm kiếm sản phẩm, xem danh mục, chi tiết sản phẩm.
- Quản lý giỏ hàng (lưu bằng session/cookie).
- `Customer` (đã đăng ký):
- Tất cả chức năng của Guest.
- Đăng nhập/đăng ký, quên mật khẩu.
- Đặt hàng (thanh toán giả lập), theo dõi đơn hàng.
- Quản lý thông tin cá nhân, địa chỉ giao hàng.
- `Staff`:
- Quản lý đơn hàng (xem, cập nhật trạng thái).
- Thống kê bán hàng (theo ngày/tháng/sản phẩm).
- Quản lý danh mục, sản phẩm (CRUD) trong phạm vi cho phép.
- `Admin`:
- Tất cả chức năng của Staff.
- Quản lý tài khoản (tạo/sửa/xoá/tạm khoá user, phân quyền role).
- Cấu hình hệ thống cơ bản (logo, tên cửa hàng, tham số).

- **Chiến lược phân quyền**
- Sử dụng **ASP.NET Core Identity Roles** (`Admin`, `Staff`, `Customer`).
- Áp dụng `[Authorize(Roles = "Admin,Staff")]` cho controller/area quản trị.
- Dùng **Policy** thêm nếu cần (ví dụ chỉ Admin được quản lý tài khoản).

### 3. Chức năng & use case theo vai trò

- **Guest**
- UC-G1: Xem trang chủ (banner, sản phẩm nổi bật, khuyến mãi).
- UC-G2: Xem danh mục sản phẩm, lọc theo giá, size, màu, giới tính.
- UC-G3: Tìm kiếm sản phẩm theo từ khoá.
- UC-G4: Xem chi tiết sản phẩm (ảnh, mô tả, giá, tồn kho, đánh giá giả lập).
- UC-G5: Thêm sản phẩm vào giỏ, cập nhật số lượng, xoá khỏi giỏ.

- **Customer**
- UC-C1: Đăng ký tài khoản (email, mật khẩu, họ tên, SĐT).
- UC-C2: Đăng nhập/đăng xuất, quên mật khẩu (qua email giả lập hoặc mô phỏng).
- UC-C3: Quản lý hồ sơ cá nhân (họ tên, SĐT, địa chỉ mặc định).
- UC-C4: Đặt đơn hàng từ giỏ hàng (chọn địa chỉ giao, phương thức thanh toán giả lập, xem tóm tắt đơn).
- UC-C5: Theo dõi danh sách đơn hàng, xem chi tiết, trạng thái đơn.

- **Staff**
- UC-S1: Xem danh sách đơn hàng, lọc theo trạng thái.
- UC-S2: Cập nhật trạng thái đơn hàng (Đang xử lý, Đang giao, Hoàn tất, Huỷ).
- UC-S3: Quản lý danh mục sản phẩm (thêm/sửa/xoá, bật/tắt hiển thị).
- UC-S4: Quản lý sản phẩm (CRUD, upload ảnh, cấu hình size/màu, tồn kho).
- UC-S5: Xem thống kê cơ bản (doanh thu theo ngày/tháng, top sản phẩm bán chạy).

- **Admin**
- UC-A1: Tất cả UC-S1..S5.
- UC-A2: Quản lý tài khoản user: xem danh sách, khoá/mở, gán role (Customer/Staff/Admin).
- UC-A3: Cấu hình thông tin cửa hàng, logo, thông tin liên hệ.

### 4. Luồng xử lý chính (flow)

- **Flow 1 – Duyệt sản phẩm & giỏ hàng (Guest/Customer)**

1. Người dùng vào trang chủ → hệ thống tải banner, danh mục, sản phẩm nổi bật.
2. Người dùng chọn danh mục → gọi action `CategoryController.Index` với tham số categoryId, filter.
3. Người dùng xem chi tiết sản phẩm → `ProductController.Details(id)`.
4. Người dùng thêm vào giỏ → `CartController.Add(productId, quantity)` (giỏ trong session).
5. Người dùng vào `CartController.Index` để xem, cập nhật số lượng, xoá sản phẩm.

- **Flow 2 – Đăng ký, đăng nhập (Customer)**

1. Vào trang `Account/Register` → nhập thông tin.
2. Hệ thống dùng **UserManager** tạo user, gán role `Customer`.
3. Đăng nhập qua `Account/Login`, Identity phát sinh cookie auth.

- **Flow 3 – Đặt hàng & thanh toán giả lập**

1. Customer ở trang giỏ hàng nhấn "Đặt hàng" → điều hướng `OrderController/Checkout` (yêu cầu đăng nhập).
2. Chọn địa chỉ nhận hàng (nhập mới hoặc dùng mặc định), chọn hình thức thanh toán **Giả lập**.
3. Xác nhận → hệ thống tạo bản ghi `Order` và `OrderItem`, tính tổng tiền, lưu trạng thái ban đầu `Pending`.
4. Hiển thị trang "Đặt hàng thành công" với mã đơn, gửi email giả lập (log ra console) hoặc ghi log.
5. Đơn hàng xuất hiện trong danh sách đơn của Customer và trong module Quản lý đơn của Staff/Admin.

- **Flow 4 – Xử lý đơn hàng (Staff/Admin)**

1. Nhân viên đăng nhập vào khu vực `Staff`/`Admin`.
2. Vào trang danh sách đơn → lọc theo trạng thái.
3. Mở chi tiết đơn → cập nhật trạng thái (Đang xử lý → Đang giao → Hoàn tất / Huỷ).
4. Hệ thống ghi log thay đổi trạng thái, cập nhật thời gian.

- **Flow 5 – Quản lý sản phẩm & danh mục (Staff/Admin)**

1. Vào `Categories` management: tạo cây danh mục (Nam, Nữ, Trẻ em, Phụ kiện, v.v.).
2. Trong `Products` management: thêm sản phẩm gắn với danh mục, cấu hình thuộc tính (size, màu, giá, giảm giá, tồn kho).
3. Upload ảnh sản phẩm (lưu file lên thư mục `wwwroot/images/products` và lưu path vào DB).

- **Flow 6 – Quản lý tài khoản (Admin)**

1. Vào module User Management.
2. Xem danh sách user, filter theo role.
3. Chọn user → chỉnh sửa role, khoá/mở tài khoản.

### 5. Thiết kế CSDL (MySQL 8+)

- **Bảng Identity chính (sử dụng schema ASP.NET Core Identity)**
- `AspNetUsers`: mở rộng với các cột:
- `FullName`, `PhoneNumber`, `DefaultAddress`, `IsActive`.
- `AspNetRoles`, `AspNetUserRoles`, `AspNetUserClaims`, `AspNetRoleClaims`, `AspNetUserLogins`, `AspNetUserTokens`.

- **Bảng nghiệp vụ**
- `Categories`:
- `Id` (PK), `Name`, `Slug`, `ParentId` (FK self), `IsActive`, `DisplayOrder`.
- `Products`:
- `Id` (PK), `Name`, `Slug`, `CategoryId` (FK), `Description`, `Price`, `DiscountPrice` (nullable), `StockQuantity`, `IsActive`, `CreatedAt`.
- `ProductImages`:
- `Id` (PK), `ProductId` (FK), `ImageUrl`, `IsDefault`.
- `Orders`:
- `Id` (PK), `OrderCode` (unique), `UserId` (FK AspNetUsers), `OrderDate`, `TotalAmount`, `Status` (Pending/Processing/Shipping/Completed/Cancelled), `ShippingAddress`, `Note`.
- `OrderItems`:
- `Id` (PK), `OrderId` (FK), `ProductId` (FK), `Quantity`, `UnitPrice`, `TotalPrice`.
- `StatisticsDaily` (tuỳ chọn, hoặc dùng truy vấn động):
- `StatDate`, `TotalRevenue`, `OrderCount`.

- **Kế hoạch file SQL**
- `scripts/01_create_database.sql`: tạo database `fashion_sales`, user quyền.
- `scripts/02_schema_identity.sql`: script tạo bảng Identity (hoặc để EF Migrations tạo, sau đó export script).
- `scripts/03_schema_business.sql`: tạo bảng Categories, Products, ProductImages, Orders, OrderItems, StatisticsDaily.
- `scripts/04_seed_data.sql`: seed dữ liệu mẫu (admin, staff, danh mục, sản phẩm demo).

### 6. Thiết kế UI/UX chi tiết cho các trang chính

- **Nguyên tắc chung**
- Thiết kế hiện đại, responsive (Bootstrap 5), tông màu thời trang (ví dụ: trắng + đen + accent màu be hoặc pastel).
- Navigation rõ ràng, ít nhất: Logo, Menu (Nam/Nữ/Trẻ em/Phụ kiện), Tìm kiếm, Icon Giỏ hàng (hiển thị số lượng), Tài khoản.

- **Trang chủ (`Home/Index`)**
- Hero banner lớn (slider hoặc static) với CTA "Mua ngay".
- Section danh mục nổi bật (thẻ card với ảnh và tên danh mục).
- Section sản phẩm mới, sản phẩm bán chạy (grid 4 cột desktop, 2 cột tablet, 1 cột mobile).
- Footer: thông tin liên hệ, mạng xã hội, email subscribe.

- **Trang danh mục (`Category/Index`)**
- Breadcrumb (Trang chủ > Nam > Áo thun).
- Sidebar filter: theo giá (range slider), size, màu, sắp xếp (mới nhất, giá tăng/giảm, phổ biến).
- Danh sách sản phẩm dạng grid, mỗi card hiển thị: ảnh, tên, giá, giá khuyến mãi, nút "Thêm vào giỏ".

- **Trang chi tiết sản phẩm (`Product/Details`)**
- Gallery ảnh (ảnh chính lớn + thumbnail).
- Tên sản phẩm, mã sản phẩm, đánh giá (giả lập), giá, giá khuyến mãi.
- Chọn size, màu, số lượng.
- Nút "Thêm vào giỏ" rõ ràng.
- Tab mô tả chi tiết, thông tin chất liệu, bảo quản, sản phẩm liên quan.

- **Giỏ hàng (`Cart/Index`)**
- Bảng sản phẩm trong giỏ: ảnh nhỏ, tên, đơn giá, số lượng (input +/−), thành tiền, nút xoá.
- Tóm tắt đơn hàng bên phải: tổng tiền, nút "Tiếp tục mua sắm", nút "Tiến hành đặt hàng".

- **Thanh toán giả lập (`Order/Checkout`)**
- Form địa chỉ giao hàng, ghi chú.
- Tóm tắt giỏ hàng.
- Chọn phương thức thanh toán: hiển thị rõ "Thanh toán giả lập cho đồ án (không thực hiện thanh toán thật)".
- Nút "Xác nhận đặt hàng".

- **Tài khoản khách hàng**
- `Account/Profile`: form cập nhật thông tin cá nhân.
- `Account/Orders`: danh sách đơn hàng, link xem chi tiết.

- **Khu vực Admin/Staff (Areas)**
- Layout riêng với sidebar trái (Dashboard, Đơn hàng, Sản phẩm, Danh mục, Thống kê, Tài khoản).
- Bảng dữ liệu có phân trang, tìm kiếm.
- Màn hình thống kê: biểu đồ đơn giản (có thể dùng Chart.js) doanh thu theo ngày/tháng, top sản phẩm.

### 7. Định nghĩa layer code & controller chính

- **Controllers phía khách**
- `HomeController`: trang chủ, static pages.
- `CategoryController`: danh mục.
- `ProductController`: chi tiết sản phẩm, tìm kiếm.
- `CartController`: giỏ hàng (Add/Update/Remove/Index).
- `OrderController`: Checkout, lịch sử đơn (cần `[Authorize(Roles="Customer")]`).
- `AccountController` (hoặc dùng `Identity` area với scaffold): đăng ký/đăng nhập, hồ sơ.

- **Areas/Admin Controllers**
- `DashboardController`.
- `UsersController` (quản lý tài khoản).
- `CategoriesController`.
- `ProductsController`.
- `OrdersController`.
- `StatisticsController`.

- **Areas/Staff Controllers** (có thể share lại controller với filter role hoặc tách riêng):
- `DashboardController`.
- `CategoriesController`, `ProductsController`.
- `OrdersController`.
- `StatisticsController` (phiên bản giới hạn hơn Admin).

### 8. Quy ước coding & cấu hình ban đầu

- **Cấu hình MySQL & Identity**
- `appsettings.json`: chuỗi kết nối MySQL (user root, password 21050043).
- `Program.cs`: đăng ký `DbContext` với MySQL, đăng ký Identity, cấu hình cookie.
- **Migrations & DB**
- Dùng `dotnet ef migrations add InitialCreate` → `dotnet ef database update` để sinh bảng.
- Sau khi ổn định schema, export script để tạo file SQL cho đồ án.

- **Chuẩn hoá mã nguồn**
- Dùng ViewModel cho form, không bind trực tiếp Entity vào View.
- Áp dụng DataAnnotations để validate (Required, StringLength, Range...).

### 9. Kế hoạch triển khai & tài liệu đồ án

- **Tài liệu phân tích & thiết kế**
- Biểu đồ use case cho từng nhóm vai trò.
- Biểu đồ hoạt động (activity) cho các flow chính: Đặt hàng, Quản lý đơn, Quản lý sản phẩm.
- ERD cho CSDL MySQL.
- **Tài liệu kỹ thuật**
- Mô tả kiến trúc hệ thống, các layer.
- Hướng dẫn cài đặt: yêu cầu môi trường, cấu hình connection string, chạy migrations hoặc scripts SQL.

- **Lộ trình thực hiện**

1. Thiết kế CSDL & ERD, thống nhất bảng và quan hệ.
2. Khởi tạo project ASP.NET Core 8 MVC, tích hợp Identity + MySQL.
3. Cài đặt Entities, DbContext, chạy migrations.
4. Xây dựng các trang khách (Home, Category, Product, Cart, Checkout, Account).
5. Xây dựng Areas Admin & Staff.
6. Bổ sung thống kê, tối ưu UI/UX, responsive.
7. Hoàn thiện tài liệu (use case, flow, SQL scripts, hướng dẫn cài đặt).