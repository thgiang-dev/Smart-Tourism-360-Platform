# Smart Tourism 360 Platform

Nền tảng du lịch số tích hợp bản đồ tương tác và tham quan ảo 360 độ.

---

## 1. Cấu trúc thư mục dự án

Dự án được tổ chức theo cấu trúc sau:
*   `backend/`: Mã nguồn Backend xây dựng bằng ASP.NET Core Web API (Clean Architecture).
*   `frontend/`: Giao diện Frontend xây dựng bằng Vue.js 3, Pinia và Vite.
*   `Docs/`: Tài liệu đặc tả hệ thống, thiết kế cơ sở dữ liệu và kế hoạch triển khai.
*   `docker-compose.dev.yml`: File chạy môi trường lưu trữ (PostgreSQL + MinIO) cho quá trình phát triển hàng ngày.
*   `docker-compose.yml`: File chạy toàn bộ hệ thống gồm PostgreSQL, MinIO, Backend và Frontend trong môi trường Docker.

---

## 2. Hướng dẫn chạy các chế độ

Dự án hỗ trợ 2 chế độ chạy phù hợp với các nhu cầu phát triển và demo:

### Chế độ A: Development Mode (Khuyên dùng khi code hàng ngày)
Chỉ khởi động cơ sở dữ liệu PostgreSQL (PostGIS) và MinIO Object Storage bằng Docker. Backend và Frontend được chạy trực tiếp bằng dòng lệnh trên máy cục bộ để tối ưu tốc độ reload và debug.

1.  **Khởi động các dịch vụ lưu trữ (Docker):**
    ```bash
    docker compose -f docker-compose.dev.yml up -d
    ```
2.  **Chạy Backend (local):**
    *   Yêu cầu cài đặt .NET 10.0 SDK.
    *   Mở một terminal mới, di chuyển vào thư mục backend và chạy:
        ```bash
        dotnet run --project backend/SmartTourism360.Api/SmartTourism360.Api.csproj
        ```
3.  **Chạy Frontend (local):**
    *   Yêu cầu cài đặt Node.js v18+.
    *   Mở một terminal khác, di chuyển vào thư mục frontend và chạy:
        ```bash
        cd frontend
        npm install
        npm run dev
        ```

### Chế độ B: Full Docker Mode (Demo nhanh và kiểm thử triển khai)
Khởi động toàn bộ 4 dịch vụ (PostgreSQL, MinIO, Backend, Frontend) trong container Docker. Không cần cài đặt sẵn môi trường .NET SDK hay Node.js trên máy chủ.

1.  **Dừng chế độ Development cũ (nếu có):**
    ```bash
    docker compose -f docker-compose.dev.yml down
    ```
2.  **Khởi động Full Docker:**
    ```bash
    docker compose up -d --build
    ```
    *(Tùy chọn `--build` bắt buộc sử dụng khi có bất kỳ thay đổi nào trong mã nguồn Backend hoặc Frontend để Docker build lại image mới)*

---

## 3. Các địa chỉ truy cập dịch vụ (Cả 2 chế độ)

| Dịch vụ | URL | Thông tin đăng nhập mặc định |
| :--- | :--- | :--- |
| **Frontend App** | `http://localhost:5173` | Du khách truy cập xem bản đồ và tour 360 |
| **Admin Dashboard** | `http://localhost:5173/admin/login` | Email: `admin@smarttourism360.vn` <br> Mật khẩu: `Admin@123` |
| **Backend API** | `http://localhost:5074` | API Gateway chính cho cả hệ thống |
| **API Reference (Scalar)** | `http://localhost:5074/scalar/v1` | Tài liệu đặc tả Swagger/Scalar API trực quan |
| **MinIO Console** | `http://localhost:9001` | Tài khoản: `minioadmin` <br> Mật khẩu: `minioadmin_secure_pass` |

---

## 4. Lưu ý quan trọng khi quản lý Docker

*   **Khi nào cần thêm `--build`:** Mỗi khi bạn chỉnh sửa code ở backend hoặc frontend, bạn phải chạy lệnh `docker compose up -d --build` để Docker tiến hành build lại các artifact/file tĩnh và nạp vào container.
*   **Cấu hình API Base URL qua Docker build args:** Trong `docker-compose.yml`, dịch vụ `frontend` truyền tham số `VITE_API_BASE_URL` qua `args` khi build (mặc định là `http://localhost:5074`). Nếu bạn muốn thay đổi địa chỉ Backend API mà trình duyệt gọi (ví dụ: khi deploy lên server thật có IP/Domain khác), bạn hãy điều chỉnh giá trị này trong phần `args` của `frontend` trong file `docker-compose.yml` rồi chạy `docker compose up -d --build` để build lại.
*   **Xóa toàn bộ dữ liệu (Reset hệ thống):**
    Dữ liệu database PostgreSQL và các tập tin media trong MinIO được lưu trữ lâu dài thông qua Volume của Docker. Nếu bạn muốn xóa sạch toàn bộ dữ liệu mẫu đã seed để bắt đầu lại một database trống hoàn toàn, hãy chạy lệnh:
    ```bash
    # Reset cho Development mode
    docker compose -f docker-compose.dev.yml down -v
    
    # Reset cho Full Docker mode
    docker compose down -v
    ```
    > [!WARNING]
    > Lệnh `down -v` sẽ xóa vĩnh viễn dữ liệu trong volume. Hãy cân nhắc trước khi thực hiện.
