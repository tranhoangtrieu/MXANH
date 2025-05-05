1. Cấu trúc thư mục
Thư mục						| Nội dung
/Controllers				| Chứa các API Controller
/Models				        | Các class model đại diện dữ liệu tương ứng trong Database
/Data                       | DbContext và cấu hình database
/Repositories               | Giao tiếp với database (CRUD, Query)
/Services                   | Xử lý logic nghiệp vụ (business logic)
/Migrations                 | Các file Migration của Entity Framework Core
/DTO/Response               | Các class DTO dùng để truyền dữ liệu giữa client và server
/DTO/Request                | Các class DTO dùng để nhận dữ liệu từ client
/Helpers                    | Chứa các hàm hỗ trợ cho các chức năng khác nhau

2. Quy tắc coding

Tên file        | PascalCase (VD: UserController.cs, User.cs)
Tên class       | PascalCase (VD: UserService, AuthRepository)
Tên method      | PascalCase (VD: RegisterUser(), GetUserById())
Tên biến        | camelCase (VD: phoneNumber, userList)
Comment			| Rõ ràng, ngắn gọn, cần thiết mới viết

3. Quy định gọi API và đặt tên API
HTTP Method theo đúng chức năng:

GET: Lấy dữ liệu

POST: Tạo mới

PUT/PATCH: Cập nhật

DELETE: Xoá

Không dùng động từ trong URL (vd: /getUser, /deleteUser là sai).

Quy tắc đặt tên Endpoint:
Hành động           | HTTP | Đường dẫn API     | Ví dụ
Lấy danh sách       | GET  | /api/users        | Lấy toàn bộ người dùng
Lấy chi tiết        | GET  | /api/users/{id}   | Lấy 1 người dùng
Tạo mới             | POST | /api/users        | Tạo người dùng
Cập nhật            | PUT  | /api/users/{id}   | Cập nhật người dùng
Xoá                 | DELETE | /api/users/{id} | Xoá người dùng