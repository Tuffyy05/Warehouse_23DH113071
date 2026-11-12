# Warehouse_23DH113071 - Hệ Thống Quản Lý & Kiểm Kê Kho Hàng

Bài thi Giữa kỳ môn **Lập trình Web (Ca 4)**
- **Sinh viên:** Lê Ngọc Tấn Tài
- **Mã số sinh viên (MSSV):** 23DH113071
- **Công nghệ áp dụng:** ASP.NET MVC 5, Entity Framework 6 (Database First), Microsoft SQL Server, .NET Framework 4.8.

---

## 📋 Cấu trúc bài làm & Chức năng

1. **Câu 1: Thiết kế Cơ sở Dữ liệu & Khởi tạo Dự án**
   - Script SQL [`Warehouse_23DH113071.sql`](Warehouse_23DH113071.sql) tạo CSDL `Warehouse_23DH113071` và bảng `dbo.Products`.
   - Kết nối Entity Framework 6 Database First sinh Entity Model `Product`.

2. **Câu 2: Hiển thị & Chỉnh sửa Sản phẩm**
   - **Câu 2a (`/Products`):** Hiển thị danh sách sản phẩm, tự động tính trạng thái tồn kho (*Hết hàng* khi SL = 0, *Sắp hết* khi SL < 5, *Đủ hàng* cho các trường hợp còn lại) kèm badge phân biệt màu sắc.
   - **Câu 2b (`/Products/Edit/{id}`):** Chỉ cho phép chỉnh sửa Tên sản phẩm, Danh mục, Đơn giá (> 0). Khóa không cho sửa Mã sản phẩm và Số lượng tồn kho.

3. **Câu 3: Tra cứu & Bộ lọc Nâng cao (`/Products/Search`)**
   - Tìm kiếm theo tên sản phẩm (không phân biệt chữ hoa/thường).
   - Lọc theo mức trạng thái tồn kho (*Hết hàng*, *Sắp hết*, *Đủ hàng*).
   - Sắp xếp đơn giá tăng dần hoặc giảm dần bằng LINQ.

4. **Câu 4: Kiểm kê 1 Sản phẩm (`/Products/InventoryCheck`)**
   - Chọn sản phẩm từ danh sách.
   - Nhập số lượng thực tế kiểm kê (kiểm tra số nguyên không âm).
   - Tự động tính chênh lệch (*Tăng*, *Giảm*, hoặc *Không chênh lệch*) và cập nhật CSDL.

5. **Câu 5: Kiểm kê đồng thời 2 Sản phẩm - Quản lý Giao dịch (`/Products/InventoryCheckTwo`)**
   - Kiểm kê 2 sản phẩm khác nhau trong 1 Transaction (`db.Database.BeginTransaction()`).
   - Tự động Rollback toàn bộ nếu có bất kỳ lỗi nào xảy ra (số lượng âm, trùng mã sản phẩm, lỗi CSDL) để bảo toàn tính toàn vẹn dữ liệu.

6. **Câu 6: Thêm Sản phẩm Mới (`/Products/Create`)**
   - Validate bắt buộc: Tên không để trống, Giá > 0, Số lượng >= 0.
   - Kiểm tra trùng tên sản phẩm (không phân biệt hoa/thường và khoảng trắng thừa bằng `.Trim().ToLower()`).

7. **Câu 7: Báo cáo Thống kê Tồn kho (`/Products/Statistics`)**
   - Thống kê tổng số mặt hàng bằng LINQ `Count()`.
   - Thống kê số lượng mặt hàng hết kho bằng `Count(p => p.Quantity == 0)`.
   - Thống kê tổng giá trị hàng tồn bằng `Sum(p => p.Price * p.Quantity)`.

---

## 🚀 Hướng dẫn Chạy Dự án

1. Mở file [`Warehouse_23DH113071.sql`](Warehouse_23DH113071.sql) trong SQL Server Management Studio (SSMS) và thực thi để tạo CSDL.
2. Mở file [`Warehouse_23DH113071.sln`](Warehouse_23DH113071.sln) bằng Visual Studio 2022.
3. Nhấn **F5** (hoặc chọn **IIS Express**) để khởi chạy ứng dụng web.
