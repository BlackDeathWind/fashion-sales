## Use case và luồng xử lý hệ thống bán hàng thời trang

### 1. Actor

- Guest (Khách vãng lai)
- Customer (Khách hàng đã đăng ký)
- Staff (Nhân viên)
- Admin (Quản trị viên)

### 2. Use case theo vai trò

#### 2.1. Guest

- UC-G1: Xem trang chủ
  - Mô tả: Guest truy cập trang chủ để xem banner, danh mục và sản phẩm nổi bật.
  - Luồng chính:
    1. Guest truy cập `/Home/Index`.
    2. Hệ thống hiển thị banner, danh mục và danh sách sản phẩm nổi bật.

- UC-G2: Xem danh mục sản phẩm
  - Mô tả: Guest duyệt danh sách sản phẩm theo danh mục, lọc theo thuộc tính.
  - Tiền điều kiện: Danh mục đã được tạo.
  - Luồng chính:
    1. Guest chọn một danh mục từ menu.
    2. Hệ thống gọi `CategoryController.Index(categoryId, filter)` và hiển thị danh sách sản phẩm.

- UC-G3: Tìm kiếm sản phẩm
  - Mô tả: Guest tìm sản phẩm theo từ khóa.
  - Luồng chính:
    1. Guest nhập từ khóa vào ô tìm kiếm.
    2. Hệ thống tìm trong `Products` theo tên/slug và trả về danh sách kết quả.

- UC-G4: Xem chi tiết sản phẩm
  - Mô tả: Guest xem thông tin chi tiết một sản phẩm.
  - Luồng chính:
    1. Guest chọn một sản phẩm từ danh sách.
    2. Hệ thống gọi `ProductController.Details(id)` và hiển thị thông tin chi tiết, ảnh, giá, tồn kho.

- UC-G5: Quản lý giỏ hàng
  - Mô tả: Guest có thể thêm/xóa/cập nhật sản phẩm trong giỏ (lưu session).
  - Luồng chính:
    1. Tại trang chi tiết/danh mục, Guest nhấn “Thêm vào giỏ”.
    2. Hệ thống cập nhật giỏ hàng trong session.
    3. Guest truy cập `/Cart/Index` để xem, cập nhật số lượng, xóa sản phẩm.

#### 2.2. Customer

- UC-C1: Đăng ký tài khoản
  - Luồng chính:
    1. Customer vào trang `/Identity/Account/Register`.
    2. Nhập email, mật khẩu, họ tên, SĐT.
    3. Hệ thống tạo `AspNetUser` + role `Customer`.

- UC-C2: Đăng nhập/đăng xuất, quên mật khẩu
  - Sử dụng giao diện Identity mặc định (`/Identity/Account/Login`, `Logout`, `ForgotPassword`).

- UC-C3: Quản lý hồ sơ cá nhân
  - Luồng chính:
    1. Customer đăng nhập.
    2. Truy cập `/Account/Profile`.
    3. Cập nhật thông tin (FullName, PhoneNumber, DefaultAddress).

- UC-C4: Đặt đơn hàng
  - Luồng chính:
    1. Customer kiểm tra giỏ hàng tại `/Cart/Index`.
    2. Nhấn “Tiến hành đặt hàng” → `/Order/Checkout` (yêu cầu đăng nhập).
    3. Nhập/Chọn địa chỉ nhận hàng, xác nhận thanh toán giả lập.
    4. Hệ thống tạo bản ghi `Order` + `OrderItems`, trạng thái `Pending`.

- UC-C5: Theo dõi đơn hàng
  - Luồng chính:
    1. Customer truy cập `/Account/Orders`.
    2. Hệ thống hiển thị danh sách đơn thuộc UserId.
    3. Customer xem chi tiết từng đơn.

#### 2.3. Staff

- UC-S1: Xem danh sách đơn hàng
  - Truy cập `Staff/Orders/Index`.
  - Lọc theo trạng thái, ngày đặt.

- UC-S2: Cập nhật trạng thái đơn hàng
  - Mở chi tiết đơn `Staff/Orders/Details(id)`.
  - Chọn trạng thái mới (Processing/Shipping/Completed/Cancelled) và lưu.

- UC-S3: Quản lý danh mục sản phẩm
  - CRUD danh mục tại `Staff/Categories`.

- UC-S4: Quản lý sản phẩm
  - CRUD sản phẩm tại `Staff/Products`, upload ảnh.

- UC-S5: Xem thống kê
  - Xem doanh thu, số đơn, top sản phẩm tại `Staff/Statistics`.

#### 2.4. Admin

- UC-A1: Thực hiện toàn bộ chức năng Staff.
- UC-A2: Quản lý tài khoản người dùng
  - Xem danh sách user tại `Admin/Users`.
  - Gán role, khóa/mở tài khoản.
- UC-A3: Cấu hình thông tin cửa hàng
  - Cập nhật tên cửa hàng, logo, thông tin liên hệ (có thể lưu vào bảng cấu hình riêng hoặc appsettings).

### 3. Luồng hoạt động chính (tóm tắt)

- Flow duyệt sản phẩm & giỏ hàng (Guest/Customer).
- Flow đăng ký/đăng nhập (Customer).
- Flow đặt hàng & thanh toán giả lập (Customer).
- Flow xử lý đơn hàng (Staff/Admin).
- Flow quản lý danh mục & sản phẩm (Staff/Admin).
- Flow quản lý tài khoản (Admin).


