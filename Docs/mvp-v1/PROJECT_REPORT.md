# BÁO CÁO KẾT QUẢ DỰ ÁN: SMART TOURISM 360 PLATFORM

## Nền tảng du lịch số tích hợp bản đồ tương tác và tham quan ảo 360 độ

---

## 1. Giới thiệu tổng quan dự án

**Smart Tourism 360 Platform** là nền tảng số hóa du lịch địa phương, hỗ trợ các đơn vị quản lý, doanh nghiệp và cộng đồng quảng bá hình ảnh điểm đến trực quan, sinh động. Dự án giải quyết các hạn chế của phương thức quảng bá truyền thống bằng cách tích hợp **bản đồ tương tác (Interactive Map)**, **nội dung đa phương tiện** và **công nghệ tham quan ảo 360 độ (360 Virtual Tour)**.

### Mục tiêu dự án:
- **Số hóa điểm đến**: Hỗ trợ lưu trữ thông tin có cấu trúc về các di tích, thắng cảnh, làng nghề, homestay, điểm ẩm thực.
- **Trải nghiệm thực tế ảo**: Cho phép du khách "tham quan trước" các địa điểm thông qua các bức ảnh panorama 360 độ liên kết với nhau bằng các điểm tương tác (hotspots) thông minh.
- **Bản đồ trực quan**: Bản đồ số hóa hỗ trợ tìm kiếm, định vị và lọc địa điểm theo nhóm chuyên đề một cách trực quan.
- **Nền tảng quản trị mở**: Cung cấp giao diện quản trị đầy đủ để thêm mới địa điểm bằng cách ghim trực tiếp lên bản đồ, upload ảnh 360, tạo và cấu hình liên kết tour trực tiếp mà không cần can thiệp code.

---

## 2. Công nghệ sử dụng

Dự án được xây dựng với kiến trúc tách biệt rõ ràng giữa Frontend, Backend, Database và Object Storage:

### 2.1. Backend API
- **Framework**: **ASP.NET Core Web API (.NET 10.0)** sử dụng mô hình Clean Architecture giúp mã nguồn dễ mở rộng và bảo trì.
- **ORM**: **Entity Framework Core** để giao tiếp và quản lý cơ sở dữ liệu.
- **Xác thực**: **JWT (JSON Web Token)** cấp phát token bảo mật cho các yêu cầu chỉnh sửa dữ liệu từ phía quản trị viên.
- **Tài liệu hóa API**: Tích hợp công cụ **Swagger / Scalar** tự động tạo tài liệu kiểm thử API tại địa chỉ `http://localhost:5074/scalar/v1`.

### 2.2. Frontend Web Application
- **Framework**: **Vue.js 3** kết hợp công cụ build **Vite** mang lại tốc độ phản hồi nhanh và tối ưu hóa dung lượng build.
- **State Management**: **Pinia** quản lý trạng thái ứng dụng tập trung (trạng thái đăng nhập admin, danh sách địa điểm).
- **Styling**: **TailwindCSS** hỗ trợ tùy biến giao diện linh hoạt, hiện đại và tương thích hoàn hảo trên thiết bị di động (Responsive Design).
- **Bản đồ tương tác**: Sử dụng thư viện bản đồ mã nguồn mở **Leaflet** (hoặc mở rộng với Mapbox GL JS) giúp tải bản đồ nhẹ nhàng và chính xác.
- **Trình xem ảo 360**: Sử dụng thư viện **Photo Sphere Viewer** (hoặc Pannellum) để xử lý ảnh panorama 360 độ, dựng không gian ảo, hỗ trợ xoay góc nhìn, thu phóng và lắng nghe sự kiện bấm vào các hotspot.

### 2.3. Object Storage
- **Local Dev / Self-hosted**: Sử dụng **MinIO** giả lập môi trường Amazon S3 lưu trữ các tệp dung lượng lớn (ảnh 360, video, audio thuyết minh) nhằm tránh làm nặng cơ sở dữ liệu.
- **Production**: Thiết kế tương thích hoàn toàn để chuyển đổi sang Cloudflare R2 hoặc AWS S3.

### 2.4. Triển khai (Deployment)
- Sử dụng **Docker** và **Docker Compose** đóng gói toàn bộ hệ thống (PostgreSQL, MinIO, Backend, Frontend) chạy đồng nhất trên các môi trường chỉ với một câu lệnh.

---

## 3. Cơ sở dữ liệu (PostgreSQL + PostGIS)

Hệ thống sử dụng cơ sở dữ liệu quan hệ **PostgreSQL** kết hợp tiện ích mở rộng không gian **PostGIS** giúp xử lý và truy vấn tọa độ địa lý thông minh.

### Sơ đồ liên kết thực thể (ERD) và danh sách các bảng cốt lõi:

| STT | Tên Bảng | Khóa chính | Khóa ngoại | Vai trò & Ý nghĩa |
| :--- | :--- | :--- | :--- | :--- |
| 1 | **roles** | `id` (UUID) | - | Lưu các vai trò người dùng (Super Admin, Admin, Content Manager). |
| 2 | **users** | `id` (UUID) | `role_id` | Lưu thông tin đăng nhập của Admin (email, mật khẩu băm, trạng thái hoạt động). |
| 3 | **regions** | `id` (UUID) | `cover_image_id` | Khu vực triển khai (ví dụ: Cần Thơ), chứa tọa độ trung tâm và mức zoom mặc định. |
| 4 | **categories** | `id` (UUID) | - | Danh mục địa điểm (Văn hóa - lịch sử, Tâm linh, Sinh thái, Làng nghề, Ẩm thực...). |
| 5 | **destinations**| `id` (UUID) | `region_id`, `category_id`, `cover_image_id` | Bảng trung tâm lưu thông tin địa điểm (tên, mô tả, địa chỉ, vĩ độ/kinh độ dạng số và kiểu `location` PostGIS). |
| 6 | **media_files** | `id` (UUID) | `uploaded_by` | Metadata của tất cả tệp đa phương tiện, lưu trữ đường dẫn thực tế trên MinIO/S3. |
| 7 | **virtual_tours**| `id` (UUID)| `destination_id`, `default_panorama_id` | Thông tin tour ảo 360 độ tương ứng với địa điểm. |
| 8 | **panoramas** | `id` (UUID) | `virtual_tour_id`, `panorama_image_id` | Lưu các điểm đứng (scenes) trong tour ảo (ví dụ: Cổng chùa, Sân chính, Chánh điện). |
| 9 | **hotspots** | `id` (UUID) | `panorama_id`, `target_panorama_id`, `media_id` | Điểm tương tác đặt trên ảnh 360. Loại `navigation` bắt buộc chứa `target_panorama_id` để chuyển cảnh. |
| 10| **audio_guides**| `id` (UUID) | `media_id` | Lưu các bài thuyết minh âm thanh đính kèm địa điểm hoặc hotspot cụ thể. |

---

## 4. Các chức năng chính

### 4.1. Nhóm chức năng dành cho du khách (Public Client)

#### 1. Khám phá bản đồ tương tác
- **Mô tả ngắn**: Hiển thị các điểm du lịch dưới dạng marker trên bản đồ Leaflet. Cho phép người dùng di chuyển bản đồ, click vào marker để xem thông tin nhanh (popup gồm tên, danh mục, ảnh bìa) và chuyển hướng nhanh vào tour 360.
- **Endpoint API**: 
  - `GET /api/destinations/map` (Lấy dữ liệu rút gọn để render marker bản đồ)
  - `GET /api/regions/current` (Lấy cấu hình bản đồ trung tâm của khu vực hiện tại)

#### 2. Lọc và Tìm kiếm địa điểm
- **Mô tả ngắn**: Cho phép du khách lọc danh sách địa điểm theo danh mục chuyên đề (Sinh thái, Tâm linh, Làng nghề...) hoặc tìm kiếm theo từ khóa tên địa điểm.
- **Endpoint API**:
  - `GET /api/categories` (Lấy danh sách các danh mục đang hoạt động)
  - `GET /api/destinations` (Tìm kiếm và phân trang danh sách điểm đến)

#### 3. Xem chi tiết địa điểm du lịch
- **Mô tả ngắn**: Hiển thị thông tin đầy đủ về địa điểm bao gồm mô tả chi tiết, hình ảnh thư viện, giờ mở cửa, giá vé tham quan, số điện thoại liên hệ và tích hợp bài viết thuyết minh.
- **Endpoint API**:
  - `GET /api/destinations/{id}` (Lấy chi tiết và danh sách media đi kèm địa điểm)

#### 4. Trải nghiệm tham quan ảo 360 độ (Virtual Tour)
- **Mô tả ngắn**: Khởi động trình xem 360 độ từ một điểm đứng (panorama) mặc định. Du khách có thể kéo thả chuột để xoay nhìn không gian toàn cảnh.
- **Endpoint API**:
  - `GET /api/destinations/{destinationId}/virtual-tour` (Lấy toàn bộ dữ liệu cấu trúc tour, danh sách panorama và toàn bộ hotspots tương ứng)

#### 5. Tương tác với Hotspots thông minh
- **Mô tả ngắn**: 
  - Khi bấm vào hotspot **Navigation** (mũi tên hướng đi), hệ thống tự động chuyển đổi sang panorama tiếp theo mà không cần tải lại trang.
  - Khi bấm vào hotspot **Info** hoặc **Audio**, giao diện hiển thị hộp thông tin mở rộng chứa ảnh/video giới thiệu chi tiết hoặc phát tệp âm thanh thuyết minh tại chỗ.
- **Endpoint API**:
  - Tải động dữ liệu panorama tiếp theo qua: `GET /api/virtual-tours/{tourId}/panoramas/{panoramaId}`

---

### 4.2. Nhóm chức năng dành cho quản trị viên (Admin Dashboard)

#### 1. Đăng nhập hệ thống quản trị
- **Mô tả ngắn**: Xác thực thông tin đăng nhập của admin để truy cập trang quản lý. Sau khi đăng nhập thành công, hệ thống cấp mã JWT lưu ở bộ nhớ trình duyệt để gửi kèm các request sau đó.
- **Endpoint API**:
  - `POST /api/auth/login` (Gửi email/mật khẩu và nhận token)
  - `GET /api/auth/me` (Kiểm tra token hợp lệ và phân quyền)

#### 2. Quản lý địa điểm trực quan
- **Mô tả ngắn**: Cho phép admin thêm mới, cập nhật hoặc ẩn/hiện địa điểm. Đặc biệt, admin có thể ghim vị trí địa lý trực tiếp bằng cách click chọn tọa độ trên bản đồ hiển thị trong form nhập liệu.
- **Endpoint API**:
  - `GET /api/admin/destinations` (Lấy danh sách địa điểm quản trị)
  - `POST /api/admin/destinations` (Tạo mới địa điểm và tọa độ địa lý)
  - `PUT /api/admin/destinations/{id}` (Cập nhật thông tin điểm đến)
  - `DELETE /api/admin/destinations/{id}` (Ẩn/xóa địa điểm)

#### 3. Thư viện quản lý tệp tin (Media Library)
- **Mô tả ngắn**: Upload hình ảnh, ảnh panorama 360 độ, file âm thanh thuyết minh lên máy chủ MinIO, phân loại tệp tin và lưu trữ metadata vào database.
- **Endpoint API**:
  - `POST /api/admin/media/upload` (Upload file dạng multipart/form-data)
  - `GET /api/admin/media` (Quản lý danh sách file đã upload)

#### 4. Thiết lập Tour ảo & ghim điểm tương tác (Tour Builder)
- **Mô tả ngắn**: 
  - Admin tạo tour 360 cho địa điểm, upload nhiều ảnh panorama tương ứng với các khu vực thực tế.
  - Admin mở trình chỉnh sửa ảnh 360, xoay chuột đến điểm mong muốn, click chuột để lấy góc quay `yaw`/`pitch` và tiến hành tạo hotspot liên kết (Ví dụ trỏ hotspot "Đi vào chánh điện" tới panorama "Chánh điện").
- **Endpoint API**:
  - `POST /api/admin/virtual-tours` (Tạo tour mới)
  - `POST /api/admin/panoramas` (Thêm ảnh 360 vào tour)
  - `POST /api/admin/hotspots` (Ghim hotspot vào tọa độ góc trên ảnh 360)

---

## 5. Demo hệ thống

### 5.1. Các dịch vụ hệ thống (Chạy cục bộ hoặc Docker)

| Dịch vụ | Địa chỉ truy cập (URL) | Thông tin tài khoản mặc định |
| :--- | :--- | :--- |
| **Giao diện du khách** | `http://localhost:5173` | Không cần đăng nhập |
| **Trang quản trị (Admin)**| `http://localhost:5173/admin/login`| Email: `admin@smarttourism360.vn` <br> Mật khẩu: `Admin@123` |
| **Backend Web API** | `http://localhost:5074` | - |
| **Đặc tả API (Scalar)** | `http://localhost:5074/scalar/v1` | Xem danh sách & thử nghiệm các API |
| **MinIO Console** | `http://localhost:9001` | Tài khoản: `minioadmin` <br> Mật khẩu: `minioadmin_secure_pass` |

### 5.2. Hướng dẫn khởi động nhanh

#### Chạy toàn bộ hệ thống bằng Docker Compose:
```bash
# Khởi động toàn bộ PostgreSQL (PostGIS), MinIO, Backend API, Frontend App
docker compose up -d --build
```
*(Tham số `--build` giúp biên dịch lại Backend và xây dựng các tệp tĩnh Vue.js trên container Docker mới khi có sự thay đổi về mã nguồn).*

#### Dọn dẹp dữ liệu (Reset hệ thống):
```bash
# Xóa container và dọn dẹp các ổ đĩa dữ liệu (Volumes)
docker compose down -v
```

---

## 6. Kết luận và hướng phát triển

### 6.1. Kết luận
Dự án **Smart Tourism 360 Platform** đã hoàn thành phiên bản MVP đạt đầy đủ các tiêu chuẩn kỹ thuật:
- **Kiến trúc bền vững**: Tách rời các phần giúp dễ dàng phân chia công việc cho nhóm phát triển.
- **Trải nghiệm mượt mà**: Bản đồ tương tác và trình xem 360 hoạt động ổn định, khả năng chuyển tiếp cảnh thông qua hotspots nhanh, trực quan.
- **Tính thực tế cao**: Cung cấp công cụ quản trị giúp cán bộ vận hành du lịch địa phương hoàn toàn tự chủ động cập nhật dữ liệu điểm đến và xây dựng tour ảo mà không cần biết viết code.

### 6.2. Hướng phát triển trong tương lai
1. **WebGIS 3D**: Tích hợp thư viện CesiumJS hoặc ArcGIS Maps SDK để hiển thị bản đồ địa hình 3D của khu vực du lịch, giúp tăng tính trực quan của bản đồ.
2. **AI Hướng dẫn viên ảo (AI Tour Guide)**: Tích hợp Chatbot AI hỗ trợ giải đáp nhanh các câu hỏi lịch sử, văn hóa về địa điểm và tự động gợi ý lịch trình di chuyển tối ưu cho du khách.
3. **Mô hình 3D tương tác (Three.js)**: Hỗ trợ nạp và tương tác với các cổ vật hoặc công trình kiến trúc phục dựng 3D (định dạng `.gltf` / `.glb`) ngay trên giao diện web.
4. **Hỗ trợ đa ngôn ngữ (Multilingual)**: Triển khai bảng dịch thuật `translations` đã thiết kế sẵn để hỗ trợ du khách quốc tế (Tiếng Anh, Tiếng Nhật, Tiếng Hàn...).
5. **AR Check-in**: Phát triển tính năng thực tế tăng cường (AR) giúp du khách quét camera tại điểm đến ngoài đời thực để nhận thông tin thuyết minh kỹ thuật số.
