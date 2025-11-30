USE `fashion_sales`;

-- Schema Identity của ASP.NET Core (AspNetUsers, AspNetRoles, ...).
-- Lưu ý: Schema này được tạo tự động bởi Entity Framework Migrations.
-- Để tạo schema, chạy:
--      dotnet ef migrations add InitialCreate
--      dotnet ef database update
--
-- Bảng AspNetUsers đã được mở rộng với các cột:
--   - FullName (VARCHAR/TEXT): Họ và tên người dùng
--   - DefaultAddress (VARCHAR/TEXT): Địa chỉ mặc định
--   - IsActive (BOOLEAN): Trạng thái hoạt động của tài khoản
--
-- Các bảng Identity chuẩn:
--   - AspNetUsers: Thông tin người dùng
--   - AspNetRoles: Vai trò (Admin, Staff, Customer)
--   - AspNetUserRoles: Liên kết người dùng - vai trò
--   - AspNetUserClaims: Claims của người dùng
--   - AspNetUserLogins: Thông tin đăng nhập bên ngoài
--   - AspNetUserTokens: Tokens cho 2FA, password reset, etc.
--   - AspNetRoleClaims: Claims của vai trò


