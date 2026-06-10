# **IMPLEMENTATION PLAN**

# **Smart Tourism 360 Platform**

## **Kế hoạch triển khai MVP cho nền tảng du lịch số tích hợp bản đồ tương tác và tham quan ảo 360 độ**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | Implementation Plan |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v1.0 |
| Mục đích | Xác định kế hoạch triển khai MVP theo từng giai đoạn, sprint và checklist công việc |
| Đối tượng đọc | Project owner, developer, team lead, giảng viên hướng dẫn, nhóm triển khai |
| Phạm vi | Backend, frontend, database, storage, bản đồ, admin dashboard, tour 360, hotspot, test và deployment |
| Hình thức | Checklist theo sprint để dễ theo dõi tiến độ |

---

## **2\. Mục tiêu triển khai**

Mục tiêu của kế hoạch triển khai là xây dựng phiên bản MVP của **Smart Tourism 360 Platform** với các chức năng cốt lõi:

Du khách có thể:  
\- Xem bản đồ các địa điểm du lịch.  
\- Lọc và tìm kiếm địa điểm.  
\- Xem chi tiết địa điểm.  
\- Mở tour 360 của địa điểm.  
\- Di chuyển giữa nhiều panorama bằng hotspot mũi tên.  
\- Xem hotspot thông tin/audio/video nếu có.

Admin có thể:  
\- Đăng nhập dashboard.  
\- Quản lý khu vực và danh mục.  
\- Tạo địa điểm mới.  
\- Click trên bản đồ để chọn tọa độ địa điểm.  
\- Upload media: ảnh, ảnh 360, video, audio.  
\- Tạo virtual tour cho địa điểm.  
\- Tạo panorama từ ảnh 360\.  
\- Đặt hotspot trong panorama.  
\- Tạo navigation hotspot để liên kết các panorama.  
\- Xuất bản nội dung cho du khách.

MVP chưa bắt buộc triển khai các chức năng nâng cao như:

CesiumJS/WebGIS 3D  
AI chatbot hướng dẫn viên  
AR check-in  
Tài khoản du khách  
Đánh giá/review  
Booking/đặt lịch tham quan  
Model 3D viewer hoàn chỉnh  
Analytics dashboard nâng cao

---

## **3\. Chiến lược phát triển MVP**

### **3.1. Nguyên tắc triển khai**

Dự án nên triển khai theo hướng:

Làm lõi dữ liệu trước  
→ làm backend API  
→ làm admin nhập dữ liệu  
→ làm public hiển thị dữ liệu  
→ làm tour 360  
→ làm hotspot điều hướng  
→ test và deploy

Không nên bắt đầu bằng giao diện đẹp trước khi có dữ liệu và API. Với dự án này, dữ liệu và cấu trúc tour 360 là phần quan trọng nhất.

### **3.2. Thứ tự ưu tiên**

Thứ tự ưu tiên của MVP:

1\. Database và backend nền tảng  
2\. Auth admin  
3\. Quản lý địa điểm  
4\. Chọn tọa độ trên bản đồ  
5\. Upload media  
6\. Quản lý tour 360  
7\. Quản lý panorama  
8\. Quản lý hotspot  
9\. Public map  
10\. Public destination detail  
11\. Public 360 tour viewer  
12\. Test và deploy

### **3.3. Công nghệ sử dụng**

Frontend:  
\- Vue.js 3  
\- Vue Router  
\- Pinia  
\- TailwindCSS hoặc Bootstrap  
\- Leaflet hoặc Mapbox GL JS  
\- Photo Sphere Viewer hoặc Pannellum  
\- Lucide Icons

Backend:  
\- ASP.NET Core Web API  
\- Entity Framework Core  
\- JWT Authentication  
\- Swagger/OpenAPI

Database:  
\- PostgreSQL  
\- PostGIS

Storage:  
\- MinIO cho local/dev  
\- Có thể thay bằng Cloudflare R2/S3 khi production

DevOps:  
\- Docker Compose  
\- Git/GitHub

---

## **4\. Quy ước quản lý task**

Tài liệu này dùng checkbox để theo dõi tiến độ:

\- \[ \] Chưa làm  
\- \[x\] Đã hoàn thành

Mỗi sprint gồm:

Mục tiêu  
Checklist Backend  
Checklist Frontend  
Checklist Database  
Checklist UI/UX  
Checklist Test  
Tiêu chí hoàn thành

Bạn có thể copy từng sprint vào GitHub Issues, Notion, Trello hoặc README project để quản lý tiến độ.

---

## **5\. Tổng quan roadmap MVP**

| Sprint | Nội dung chính | Kết quả đầu ra | Trạng thái |
| ----- | ----- | ----- | ----- |
| Sprint 0 | Chuẩn bị dự án | Repo, cấu trúc thư mục, Docker Compose | [x] Đã hoàn thành |
| Sprint 1 | Database nền tảng | PostgreSQL/PostGIS, migration, seed data | [x] Đã hoàn thành |
| Sprint 2 | Backend core \+ Auth | API project, JWT, Swagger, auth admin | [x] Đã hoàn thành |
| Sprint 3 | Region, Category, Destination API | API quản lý khu vực, danh mục, địa điểm | [x] Đã hoàn thành |
| Sprint 4 | Admin layout \+ quản lý địa điểm | Admin dashboard, form địa điểm, map picker | [x] Đã hoàn thành |
| Sprint 5 | Media upload \+ storage | Upload ảnh/video/audio/panorama vào MinIO | [x] Đã hoàn thành |
| Sprint 6 | Public website \+ bản đồ | Trang chủ, bản đồ, danh sách, chi tiết địa điểm | [x] Đã hoàn thành |
| Sprint 7 | Virtual Tour + Panorama | Tạo tour, upload panorama, quản lý scene | [x] Đã hoàn thành |
| Sprint 8 | Hotspot editor | Đặt hotspot, navigation hotspot, preview | [x] Đã hoàn thành |
| Sprint 9 | Public 360 Tour Viewer | Du khách xem tour 360 và di chuyển scene | [x] Đã hoàn thành |
| Sprint 10 | Hoàn thiện, test, deploy | Tối ưu UI, seed data demo, Docker, deploy | [ ] Chưa thực hiện |

---

# **SPRINT 0: CHUẨN BỊ DỰ ÁN**

---

## **6\. Sprint 0 \- Project Setup**

### **6.1. Mục tiêu**

Thiết lập môi trường phát triển ban đầu, repository, cấu trúc project và Docker Compose.

### **6.2. Checklist chung**

* Tạo GitHub repository cho dự án.  
* Tạo cấu trúc thư mục tổng thể.  
* Tạo thư mục `backend`.  
* Tạo thư mục `frontend`.  
* Tạo thư mục `docs`.  
* Tạo thư mục `docker`.  
* Tạo file `.gitignore`.  
* Tạo file `README.md`.  
* Ghi mô tả ngắn dự án trong README.  
* Thêm danh sách công nghệ sử dụng vào README.

### **6.3. Checklist Docker**

* Tạo `docker-compose.yml`.  
* Thêm service PostgreSQL.  
* Thêm service MinIO.  
* Thêm network dùng chung.  
* Thêm volume cho PostgreSQL.  
* Thêm volume cho MinIO.  
* Cấu hình biến môi trường cho database.  
* Cấu hình biến môi trường cho MinIO.  
* Kiểm tra chạy được `docker compose up -d`.

### **6.4. Checklist Backend setup**

* Tạo solution ASP.NET Core.  
* Tạo project Web API.  
* Tạo project Application nếu dùng Clean Architecture.  
* Tạo project Domain nếu dùng Clean Architecture.  
* Tạo project Infrastructure nếu dùng Clean Architecture.  
* Cài package Entity Framework Core.  
* Cài package Npgsql.  
* Cài package hỗ trợ JWT.  
* Cài Swagger/OpenAPI.  
* Chạy thử API mặc định.

### **6.5. Checklist Frontend setup**

* Tạo project Vue.js 3\.  
* Cài Vue Router.  
* Cài Pinia.  
* Cài TailwindCSS hoặc Bootstrap.  
* Cài Axios.  
* Cài Lucide Icons.  
* Cài Leaflet hoặc Mapbox GL JS.  
* Cài Photo Sphere Viewer hoặc Pannellum.  
* Tạo layout public.  
* Tạo layout admin.  
* Chạy thử frontend.

### **6.6. Tiêu chí hoàn thành**

* Có repository rõ ràng.  
* Backend chạy được.  
* Frontend chạy được.  
* PostgreSQL chạy được bằng Docker.  
* MinIO chạy được bằng Docker.  
* README có mô tả dự án và cách chạy cơ bản.

---

# **SPRINT 1: DATABASE NỀN TẢNG**

---

## **7\. Sprint 1 \- Database & Initial Migration**

### **7.1. Mục tiêu**

Thiết kế và tạo các bảng cốt lõi cho MVP.

### **7.2. Checklist Database**

* Bật PostGIS extension.  
* Tạo bảng `roles`.  
* Tạo bảng `users`.  
* Tạo bảng `regions`.  
* Tạo bảng `categories`.  
* Tạo bảng `media_files`.  
* Tạo bảng `destinations`.  
* Tạo bảng `virtual_tours`.  
* Tạo bảng `panoramas`.  
* Tạo bảng `hotspots`.  
* Tạo bảng `audio_guides`.  
* Tạo index cho `destinations.location`.  
* Tạo index cho `destinations.region_id`.  
* Tạo index cho `destinations.category_id`.  
* Tạo index cho `media_files.media_type`.  
* Tạo index cho `panoramas.virtual_tour_id`.  
* Tạo index cho `hotspots.panorama_id`.  
* Tạo index cho `hotspots.target_panorama_id`.

### **7.3. Checklist EF Core**

* Tạo entity `Role`.  
* Tạo entity `User`.  
* Tạo entity `Region`.  
* Tạo entity `Category`.  
* Tạo entity `Destination`.  
* Tạo entity `MediaFile`.  
* Tạo entity `VirtualTour`.  
* Tạo entity `Panorama`.  
* Tạo entity `Hotspot`.  
* Tạo entity `AudioGuide`.  
* Cấu hình quan hệ trong `OnModelCreating`.  
* Cấu hình unique index cho slug.  
* Cấu hình enum/status nếu cần.  
* Tạo migration đầu tiên.  
* Chạy migration thành công.

### **7.4. Checklist Seed Data**

* Seed role `Super Admin`.  
* Seed role `Admin`.  
* Seed tài khoản admin mặc định.  
* Seed region `Cần Thơ Demo`.  
* Seed categories: văn hóa, tâm linh, sinh thái, nông nghiệp, làng nghề, ẩm thực, lưu trú.  
* Seed một vài destination demo nếu cần.  
* Kiểm tra dữ liệu seed trong database.

### **7.5. Tiêu chí hoàn thành**

* Database có đủ bảng MVP.  
* Migration chạy thành công.  
* Có tài khoản admin mặc định.  
* Có region và category mẫu.  
* Backend kết nối được PostgreSQL.

---

# **SPRINT 2: BACKEND CORE VÀ AUTH**

---

## **8\. Sprint 2 \- Backend Core & Authentication**

### **8.1. Mục tiêu**

Xây dựng nền tảng backend, chuẩn response, middleware lỗi, Swagger và đăng nhập admin bằng JWT.

### **8.2. Checklist Backend Core**

* Cấu hình connection string.  
* Cấu hình DbContext.  
* Tạo chuẩn response chung.  
* Tạo exception middleware.  
* Tạo validation error response.  
* Cấu hình Swagger.  
* Cấu hình CORS cho frontend.  
* Cấu hình logging cơ bản.  
* Tạo base controller nếu cần.  
* Tạo pagination model.

### **8.3. Checklist Auth**

* Tạo DTO `LoginRequest`.  
* Tạo DTO `LoginResponse`.  
* Tạo service xác thực admin.  
* Hash password bằng BCrypt hoặc thư viện tương đương.  
* Tạo JWT token.  
* Cấu hình JWT Authentication.  
* Tạo endpoint `POST /api/auth/login`.  
* Tạo endpoint `GET /api/auth/me`.  
* Bảo vệ các endpoint `/api/admin/*`.  
* Test login bằng Swagger/Postman.

### **8.4. Checklist Test**

* Test đăng nhập đúng tài khoản.  
* Test đăng nhập sai mật khẩu.  
* Test gọi admin API không có token.  
* Test gọi admin API có token hợp lệ.  
* Test token hết hạn nếu có cấu hình.

### **8.5. Tiêu chí hoàn thành**

* Admin đăng nhập được.  
* Backend trả JWT token.  
* API admin được bảo vệ.  
* Swagger hiển thị rõ API.  
* Error response thống nhất.

---

# **SPRINT 3: REGION, CATEGORY, DESTINATION API**

---

## **9\. Sprint 3 \- Core Tourism Data API**

### **9.1. Mục tiêu**

Xây dựng API quản lý khu vực, danh mục và địa điểm du lịch.

### **9.2. Checklist Region API**

* Tạo DTO region.  
* Tạo service region.  
* Tạo endpoint `GET /api/regions/current`.  
* Tạo endpoint `GET /api/admin/regions`.  
* Tạo endpoint `POST /api/admin/regions`.  
* Tạo endpoint `PUT /api/admin/regions/{id}`.  
* Tạo endpoint `DELETE /api/admin/regions/{id}` hoặc archive.  
* Validate slug không trùng.  
* Test CRUD region.

### **9.3. Checklist Category API**

* Tạo DTO category.  
* Tạo service category.  
* Tạo endpoint `GET /api/categories`.  
* Tạo endpoint `GET /api/admin/categories`.  
* Tạo endpoint `POST /api/admin/categories`.  
* Tạo endpoint `PUT /api/admin/categories/{id}`.  
* Tạo endpoint `DELETE /api/admin/categories/{id}` hoặc inactive.  
* Validate slug không trùng.  
* Test CRUD category.

### **9.4. Checklist Destination API**

* Tạo DTO destination list.  
* Tạo DTO destination detail.  
* Tạo DTO create destination.  
* Tạo DTO update destination.  
* Tạo service destination.  
* Tạo endpoint `GET /api/destinations`.  
* Tạo endpoint `GET /api/destinations/map`.  
* Tạo endpoint `GET /api/destinations/{id}`.  
* Tạo endpoint `GET /api/admin/destinations`.  
* Tạo endpoint `GET /api/admin/destinations/{id}`.  
* Tạo endpoint `POST /api/admin/destinations`.  
* Tạo endpoint `PUT /api/admin/destinations/{id}`.  
* Tạo endpoint `PATCH /api/admin/destinations/{id}/location`.  
* Tạo endpoint `PATCH /api/admin/destinations/{id}/status`.  
* Tạo endpoint `DELETE /api/admin/destinations/{id}` hoặc archive.  
* Validate latitude từ \-90 đến 90\.  
* Validate longitude từ \-180 đến 180\.  
* Tạo `location` PostGIS từ longitude/latitude.  
* Test API danh sách địa điểm.  
* Test API map markers.  
* Test tạo địa điểm.

### **9.5. Tiêu chí hoàn thành**

* Public API lấy được danh sách địa điểm.  
* Public API lấy được dữ liệu marker bản đồ.  
* Admin API tạo/sửa/xóa địa điểm được.  
* Tọa độ địa điểm được lưu đúng.  
* Địa điểm có trạng thái draft/published/archived.

---

# **SPRINT 4: ADMIN LAYOUT VÀ QUẢN LÝ ĐỊA ĐIỂM**

---

## **10\. Sprint 4 \- Admin Dashboard & Destination Management**

### **10.1. Mục tiêu**

Xây dựng giao diện admin cơ bản, đăng nhập, dashboard và form quản lý địa điểm có bản đồ chọn vị trí.

### **10.2. Checklist Frontend Auth**

* Tạo trang `/admin/login`.  
* Tạo form đăng nhập.  
* Gọi API login.  
* Lưu access token.  
* Tạo auth store bằng Pinia.  
* Tạo route guard cho admin.  
* Hiển thị lỗi đăng nhập.  
* Tạo nút logout.

### **10.3. Checklist Admin Layout**

* Tạo `AdminLayout`.  
* Tạo sidebar.  
* Tạo topbar.  
* Tạo menu dashboard.  
* Tạo menu categories.  
* Tạo menu destinations.  
* Tạo menu media.  
* Tạo menu virtual tours.  
* Responsive sidebar cơ bản.

### **10.4. Checklist Destination Management UI**

* Tạo trang danh sách địa điểm admin.  
* Hiển thị table địa điểm.  
* Thêm filter theo category.  
* Thêm filter theo status.  
* Thêm tìm kiếm theo keyword.  
* Thêm nút tạo địa điểm mới.  
* Thêm nút sửa địa điểm.  
* Thêm nút publish/archive.  
* Thêm trạng thái loading.  
* Thêm empty state.

### **10.5. Checklist Destination Form**

* Tạo form tạo địa điểm.  
* Tạo form sửa địa điểm.  
* Field tên địa điểm.  
* Field slug.  
* Field danh mục.  
* Field khu vực.  
* Field mô tả ngắn.  
* Field mô tả chi tiết.  
* Field địa chỉ.  
* Field trạng thái.  
* Field latitude.  
* Field longitude.  
* Nút lưu nháp.  
* Nút xuất bản.

### **10.6. Checklist Map Location Picker**

* Tạo component `MapLocationPicker`.  
* Hiển thị bản đồ Leaflet/Mapbox.  
* Hiển thị marker nếu đã có tọa độ.  
* Cho phép admin click bản đồ để chọn vị trí.  
* Cập nhật latitude/longitude vào form.  
* Cho phép kéo marker để chỉnh vị trí.  
* Hiển thị toast khi chọn vị trí.  
* Test lưu tọa độ vào backend.  
* Test địa điểm vừa tạo hiển thị trên public map.

### **10.7. Tiêu chí hoàn thành**

* Admin đăng nhập được.  
* Admin xem được dashboard.  
* Admin tạo được địa điểm.  
* Admin chọn vị trí bằng cách click trên bản đồ.  
* Địa điểm lưu được vào database.  
* Địa điểm published hiển thị trên public API.

---

# **SPRINT 5: MEDIA UPLOAD VÀ OBJECT STORAGE**

---

## **11\. Sprint 5 \- Media Upload & Storage**

### **11.1. Mục tiêu**

Triển khai upload file media vào MinIO/object storage và quản lý metadata trong database.

### **11.2. Checklist Backend Storage**

* Cấu hình MinIO trong Docker.  
* Tạo bucket cho dự án.  
* Cấu hình access key/secret key.  
* Tạo storage service.  
* Tạo hàm upload file.  
* Tạo hàm xóa file nếu cần.  
* Tạo hàm tạo public URL hoặc presigned URL.  
* Tạo quy chuẩn storage path.  
* Validate file extension.  
* Validate MIME type.  
* Validate file size.

### **11.3. Checklist Media API**

* Tạo endpoint `POST /api/admin/media/upload`.  
* Tạo endpoint `GET /api/admin/media`.  
* Tạo endpoint `GET /api/admin/media/{id}`.  
* Tạo endpoint `PUT /api/admin/media/{id}`.  
* Tạo endpoint `DELETE /api/admin/media/{id}`.  
* Lưu metadata vào bảng `media_files`.  
* Phân loại media type: image, panorama, video, audio, model3d.  
* Test upload ảnh thường.  
* Test upload ảnh panorama.  
* Test upload audio.  
* Test upload video.

### **11.4. Checklist Frontend Media Library**

* Tạo trang Media Library.  
* Tạo component upload file.  
* Hỗ trợ drag & drop.  
* Hiển thị progress upload.  
* Hiển thị danh sách media.  
* Lọc theo media type.  
* Hiển thị thumbnail ảnh.  
* Hiển thị icon cho audio/video/model3d.  
* Tạo modal preview media.  
* Cho phép chọn media từ library.  
* Hiển thị lỗi upload.

### **11.5. Checklist tích hợp với Destination**

* Cho phép chọn cover image cho địa điểm.  
* Cho phép upload cover image từ form địa điểm.  
* Cho phép hiển thị ảnh đại diện trong danh sách địa điểm.  
* Cho phép hiển thị ảnh đại diện trên public destination card.

### **11.6. Tiêu chí hoàn thành**

* Admin upload được file.  
* File được lưu vào MinIO.  
* Metadata được lưu vào database.  
* Frontend hiển thị được file đã upload.  
* Địa điểm có thể gắn ảnh đại diện.

---

# **SPRINT 6: PUBLIC WEBSITE VÀ BẢN ĐỒ DU KHÁCH**

---

## **12\. Sprint 6 \- Public Website & Interactive Map**

### **12.1. Mục tiêu**

Xây dựng giao diện public cho du khách gồm trang chủ, bản đồ khám phá, danh sách địa điểm và chi tiết địa điểm.

### **12.2. Checklist Public Layout**

* Tạo `PublicLayout`.  
* Tạo header.  
* Tạo footer.  
* Tạo navigation.  
* Tạo responsive menu mobile.  
* Thiết lập theme màu.  
* Thiết lập font chữ.  
* Thiết lập icon system.

### **12.3. Checklist Home Page**

* Tạo hero section.  
* Tạo CTA “Khám phá bản đồ”.  
* Tạo section danh mục du lịch.  
* Tạo section địa điểm nổi bật.  
* Tạo section giới thiệu trải nghiệm 360\.  
* Tạo footer thông tin dự án.  
* Thêm animation fade/slide nhẹ.

### **12.4. Checklist Explore Map Page**

* Tạo trang `/explore`.  
* Hiển thị bản đồ Leaflet/Mapbox.  
* Gọi API `GET /api/destinations/map`.  
* Hiển thị marker địa điểm.  
* Marker có màu/icon theo category.  
* Bấm marker mở popup.  
* Popup hiển thị ảnh, tên, mô tả ngắn.  
* Popup có nút “Xem chi tiết”.  
* Popup có nút “Tour 360” nếu có.  
* Thêm search input.  
* Thêm category filter chips.  
* Thêm filter “có tour 360”.  
* Responsive mobile bằng bottom sheet nếu có thời gian.

### **12.5. Checklist Destination List Page**

* Tạo trang `/destinations`.  
* Gọi API `GET /api/destinations`.  
* Hiển thị card địa điểm.  
* Thêm filter category.  
* Thêm search keyword.  
* Thêm pagination hoặc load more.  
* Thêm empty state.  
* Thêm loading skeleton.

### **12.6. Checklist Destination Detail Page**

* Tạo trang `/destinations/:slug` hoặc `/destinations/:id`.  
* Gọi API chi tiết địa điểm.  
* Hiển thị cover image.  
* Hiển thị tên địa điểm.  
* Hiển thị category badge.  
* Hiển thị mô tả ngắn.  
* Hiển thị mô tả chi tiết.  
* Hiển thị địa chỉ.  
* Hiển thị thông tin giờ mở cửa nếu có.  
* Hiển thị gallery ảnh.  
* Hiển thị audio/video nếu có.  
* Hiển thị bản đồ vị trí nhỏ.  
* Hiển thị nút “Tham quan 360” nếu có tour.  
* Xử lý trường hợp địa điểm chưa có tour 360\.

### **12.7. Tiêu chí hoàn thành**

* Du khách xem được trang chủ.  
* Du khách xem được bản đồ.  
* Marker địa điểm hiển thị đúng tọa độ.  
* Du khách xem được chi tiết địa điểm.  
* Nút “Tham quan 360” xuất hiện nếu địa điểm có tour.  
* Giao diện public responsive cơ bản.

---

# **SPRINT 7: VIRTUAL TOUR VÀ PANORAMA MANAGEMENT**

---

## **13\. Sprint 7 \- Virtual Tour & Panorama Management**

### **13.1. Mục tiêu**

Cho phép admin tạo tour 360 cho địa điểm và quản lý nhiều panorama trong tour.

### **13.2. Checklist Backend Virtual Tour API**

* Tạo DTO virtual tour.  
* Tạo service virtual tour.  
* Tạo endpoint `GET /api/admin/destinations/{destinationId}/virtual-tours`.  
* Tạo endpoint `POST /api/admin/destinations/{destinationId}/virtual-tours`.  
* Tạo endpoint `GET /api/admin/virtual-tours/{tourId}`.  
* Tạo endpoint `PUT /api/admin/virtual-tours/{tourId}`.  
* Tạo endpoint `PATCH /api/admin/virtual-tours/{tourId}/default-panorama`.  
* Tạo endpoint `DELETE /api/admin/virtual-tours/{tourId}` hoặc archive.  
* Cập nhật `destinations.has_virtual_tour` khi tour published.

### **13.3. Checklist Backend Panorama API**

* Tạo DTO panorama.  
* Tạo service panorama.  
* Tạo endpoint `GET /api/admin/virtual-tours/{tourId}/panoramas`.  
* Tạo endpoint `POST /api/admin/virtual-tours/{tourId}/panoramas`.  
* Tạo endpoint `POST /api/admin/virtual-tours/{tourId}/panoramas/bulk`.  
* Tạo endpoint `GET /api/admin/panoramas/{panoramaId}`.  
* Tạo endpoint `PUT /api/admin/panoramas/{panoramaId}`.  
* Tạo endpoint `PATCH /api/admin/panoramas/{panoramaId}/initial-view`.  
* Tạo endpoint `PATCH /api/admin/virtual-tours/{tourId}/panoramas/reorder`.  
* Tạo endpoint `DELETE /api/admin/panoramas/{panoramaId}`.  
* Validate `panoramaImageId` có `mediaType = panorama`.  
* Không cho xóa panorama nếu đang là default panorama.  
* Không cho xóa panorama nếu có hotspot khác đang trỏ đến.

### **13.4. Checklist Admin Virtual Tour UI**

* Tạo trang quản lý tour của địa điểm.  
* Hiển thị thông tin địa điểm.  
* Tạo form tạo tour.  
* Tạo form sửa tour.  
* Cho phép chọn thumbnail tour.  
* Cho phép đổi trạng thái tour.  
* Cho phép vào màn hình quản lý panorama.

### **13.5. Checklist Admin Panorama UI**

* Tạo trang danh sách panorama.  
* Hiển thị panorama card.  
* Hiển thị thumbnail panorama.  
* Hiển thị tên panorama.  
* Hiển thị trạng thái.  
* Hiển thị badge default panorama.  
* Cho phép tạo panorama từ ảnh 360 đã upload.  
* Cho phép upload ảnh panorama mới.  
* Cho phép chọn panorama mặc định.  
* Cho phép sắp xếp thứ tự panorama.  
* Cho phép sửa tên/mô tả panorama.  
* Cho phép xóa/archive panorama.  
* Cho phép mở hotspot editor.

### **13.6. Tiêu chí hoàn thành**

* Admin tạo được tour 360\.  
* Admin thêm được nhiều panorama vào tour.  
* Admin chọn được panorama mặc định.  
* Mỗi panorama gắn được ảnh 360\.  
* Dữ liệu tour và panorama lưu đúng database.

---

# **SPRINT 8: HOTSPOT EDITOR VÀ NAVIGATION HOTSPOT**

---

## **14\. Sprint 8 \- Hotspot Editor & Scene Navigation**

### **14.1. Mục tiêu**

Cho phép admin đặt hotspot trong panorama, đặc biệt là hotspot dạng navigation để tạo mũi tên di chuyển giữa các điểm nhìn.

### **14.2. Checklist Backend Hotspot API**

* Tạo DTO hotspot.  
* Tạo service hotspot.  
* Tạo endpoint `GET /api/admin/panoramas/{panoramaId}/hotspots`.  
* Tạo endpoint `POST /api/admin/panoramas/{panoramaId}/hotspots`.  
* Tạo endpoint `PUT /api/admin/hotspots/{hotspotId}`.  
* Tạo endpoint `PATCH /api/admin/hotspots/{hotspotId}/position`.  
* Tạo endpoint `PATCH /api/admin/hotspots/{hotspotId}/status`.  
* Tạo endpoint `DELETE /api/admin/hotspots/{hotspotId}`.  
* Validate `yaw`.  
* Validate `pitch`.  
* Validate hotspot type.  
* Nếu type \= navigation, bắt buộc có `targetPanoramaId`.  
* Nếu type \= navigation, kiểm tra target panorama thuộc cùng tour.  
* Nếu type \= audio/video/image/model3d, bắt buộc có `mediaId`.  
* Nếu type \= external\_link, bắt buộc có `externalUrl`.

### **14.3. Checklist Hotspot Editor UI**

* Tạo trang `HotspotEditorPage`.  
* Tải panorama image.  
* Tải danh sách hotspot.  
* Hiển thị 360 viewer ở chế độ editor.  
* Render hotspot hiện có.  
* Admin click vào ảnh 360 để lấy yaw/pitch.  
* Mở form tạo hotspot.  
* Cho phép chọn hotspot type.  
* Field title.  
* Field description.  
* Field yaw.  
* Field pitch.  
* Field icon.  
* Field status.  
* Nếu chọn navigation, hiển thị dropdown target panorama.  
* Nếu chọn media hotspot, hiển thị media picker.  
* Lưu hotspot.  
* Cập nhật vị trí hotspot.  
* Xóa hotspot.  
* Preview hotspot.

### **14.4. Checklist Navigation Hotspot**

* Hiển thị icon mũi tên cho hotspot navigation.  
* Cho phép chọn panorama đích.  
* Hiển thị thumbnail panorama đích.  
* Không cho chọn target panorama không cùng tour.  
* Lưu `targetPanoramaId`.  
* Preview click navigation trong editor.  
* Test chuyển từ panorama A sang panorama B.  
* Test quay lại từ panorama B sang panorama A.  
* Test mạng lưới nhiều panorama.

### **14.5. Checklist Info/Media Hotspot**

* Tạo hotspot info.  
* Tạo hotspot audio.  
* Tạo hotspot video nếu có.  
* Tạo hotspot image nếu có.  
* Hiển thị popup preview cho info.  
* Hiển thị audio player preview.  
* Hiển thị video modal preview.

### **14.6. Tiêu chí hoàn thành**

* Admin đặt được hotspot trong ảnh 360\.  
* Hotspot lưu đúng yaw/pitch.  
* Admin tạo được navigation hotspot.  
* Navigation hotspot trỏ đúng panorama đích.  
* Có thể tạo tour gồm nhiều panorama liên kết với nhau.  
* Preview tour trong admin hoạt động.

---

# **SPRINT 9: PUBLIC TOUR 360 VIEWER**

---

## **15\. Sprint 9 \- Public 360 Virtual Tour Viewer**

### **15.1. Mục tiêu**

Cho phép du khách mở tour 360 của địa điểm, xoay nhìn không gian và di chuyển giữa các panorama bằng mũi tên.

### **15.2. Checklist Backend Public Tour API**

* [x] Tạo endpoint `GET /api/destinations/{destinationId}/virtual-tour`.  
* [x] API trả về tour published.  
* [x] API trả về default panorama.  
* [x] API trả về danh sách panoramas published.  
* [x] API trả về image URL của từng panorama.  
* [x] API trả về hotspots published.  
* [x] API trả về `targetPanoramaId` cho navigation hotspot.  
* [x] API trả về media URL cho hotspot audio/video/image.  
* [x] Tối ưu response để frontend dễ render.

### **15.3. Checklist Tour360Viewer Component**

* [x] Tạo component `Tour360Viewer`.  
* [x] Nhận dữ liệu tour từ API.  
* [x] Xác định panorama mặc định.  
* [x] Hiển thị ảnh 360 bằng Photo Sphere Viewer/Pannellum.  
* [x] Render hotspot theo panorama hiện tại.  
* [x] Click hotspot navigation để đổi panorama.  
* [x] Click hotspot info để mở popup.  
* [x] Click hotspot audio để mở audio player.  
* [x] Click hotspot video để mở video modal.  
* [x] Hiển thị loading khi chuyển panorama.  
* [x] Hiển thị tên panorama hiện tại.  
* [x] Hiển thị scene list.  
* [x] Cho phép chọn scene từ danh sách.  
* [x] Thêm nút quay lại trang chi tiết.  
* [x] Thêm fullscreen nếu có thời gian.

### **15.4. Checklist Tour 360 Page**

* [x] Tạo route `/destinations/:id/tour`.  
* [x] Gọi API lấy tour.  
* [x] Hiển thị loading state.  
* [x] Hiển thị error state nếu tour không tồn tại.  
* [x] Hiển thị tour viewer.  
* [x] Responsive mobile.  
* [x] Hiển thị hướng dẫn ngắn lần đầu mở.  
* [x] Test trên desktop.  
* [x] Test trên mobile.

### **15.5. Checklist Scene Navigation UX**

* [x] Hotspot navigation có icon mũi tên.  
* [x] Hotspot navigation có label khi hover/tap.  
* [x] Chuyển panorama có fade transition.  
* [x] Khi chuyển scene, hotspot cũ biến mất và hotspot mới xuất hiện.  
* [x] Scene list cập nhật active scene.  
* [x] Không bị reload toàn trang khi đổi panorama.

### **15.6. Tiêu chí hoàn thành**

* [x] Du khách mở được tour 360\.  
* [x] Du khách xoay nhìn 360 độ.  
* [x] Du khách bấm mũi tên để đi sang panorama khác.  
* [x] Du khách có thể quay lại panorama trước.  
* [x] Hotspot info/audio/video hoạt động cơ bản.  
* [x] Trải nghiệm giống một tour 360 nhiều điểm nhìn.

---

# **SPRINT 10: HOÀN THIỆN, TEST VÀ DEPLOY**

---

## **16\. Sprint 10 \- Polish, Testing & Deployment**

### **16.1. Mục tiêu**

Hoàn thiện UI, test toàn bộ luồng MVP, nhập dữ liệu mẫu và triển khai demo.

### **16.2. Checklist UI Polish**

* Thống nhất theme màu.  
* Thống nhất font chữ.  
* Thống nhất icon.  
* Tối ưu spacing.  
* Tối ưu card địa điểm.  
* Tối ưu marker popup.  
* Tối ưu tour 360 controls.  
* Tối ưu admin table.  
* Tối ưu form admin.  
* Thêm loading skeleton.  
* Thêm empty state.  
* Thêm toast notification.  
* Thêm confirm dialog khi xóa/archive.  
* Kiểm tra responsive public website.  
* Kiểm tra responsive tour 360\.

### **16.3. Checklist Dữ liệu Demo**

* Tạo region demo.  
* Tạo 7 category.  
* Tạo 5–7 địa điểm mẫu.  
* Mỗi địa điểm có ảnh đại diện.  
* Ít nhất 2–3 địa điểm có tour 360\.  
* Mỗi tour có ít nhất 2–4 panorama.  
* Mỗi panorama có 2–5 hotspot.  
* Có ít nhất một tour có navigation hotspot hai chiều.  
* Có ít nhất một hotspot info.  
* Có ít nhất một hotspot audio hoặc transcript.  
* Có dữ liệu mô tả đủ đẹp để demo.

### **16.4. Checklist Functional Test**

* Test admin login.  
* Test tạo category.  
* Test tạo destination.  
* Test chọn tọa độ trên bản đồ.  
* Test upload cover image.  
* Test upload panorama.  
* Test tạo virtual tour.  
* Test tạo panorama.  
* Test chọn default panorama.  
* Test tạo navigation hotspot.  
* Test tạo info hotspot.  
* Test public map marker.  
* Test destination detail.  
* Test mở tour 360\.  
* Test chuyển panorama bằng hotspot.  
* Test tour 360 trên mobile.

### **16.5. Checklist Performance**

* Nén ảnh đại diện.  
* Nén ảnh panorama nếu quá nặng.  
* Dùng thumbnail cho card và scene list.  
* Lazy load ảnh gallery.  
* Kiểm tra API response time.  
* Kiểm tra kích thước bundle frontend.  
* Kiểm tra tour 360 không bị lag quá mức.

### **16.6. Checklist Security**

* Admin API bắt buộc JWT.  
* Không lộ password hash.  
* Validate file upload.  
* Chặn upload file không hợp lệ.  
* Cấu hình CORS đúng.  
* Không commit secret vào Git.  
* Dùng biến môi trường.  
* Kiểm tra quyền xóa/sửa dữ liệu.

### **16.7. Checklist Deployment**

* Viết Dockerfile backend.  
* Viết Dockerfile frontend.  
* Hoàn thiện docker-compose.  
* Cấu hình PostgreSQL container.  
* Cấu hình MinIO container.  
* Cấu hình biến môi trường.  
* Chạy migration khi deploy.  
* Seed dữ liệu demo.  
* Test chạy toàn bộ hệ thống bằng Docker.  
* Viết hướng dẫn cài đặt trong README.  
* Quay video demo nếu cần.  
* Chuẩn bị slide/báo cáo nếu cần.

### **16.8. Tiêu chí hoàn thành**

* Hệ thống chạy được end-to-end.  
* Admin tạo được địa điểm từ đầu đến cuối.  
* Admin tạo được tour 360 nhiều panorama.  
* Du khách xem được bản đồ, địa điểm và tour 360\.  
* Có dữ liệu demo đủ thuyết phục.  
* Dự án chạy được bằng Docker Compose.  
* Có README hướng dẫn chạy.  
* Có thể demo trước giảng viên/đơn vị hướng dẫn.

---

# **17\. Definition of Done**

Một task được xem là hoàn thành khi đạt các tiêu chí sau:

\- Code đã chạy được.  
\- Không còn lỗi nghiêm trọng.  
\- Đã test ít nhất một case thành công.  
\- Đã test một số case lỗi cơ bản.  
\- UI hiển thị ổn trên màn hình mục tiêu.  
\- API có response đúng format.  
\- Dữ liệu lưu đúng database.  
\- Không làm hỏng chức năng đã có.  
\- Có commit rõ ràng.

Với task backend:

\- Có controller/endpoint.  
\- Có service xử lý nghiệp vụ.  
\- Có validation.  
\- Có error handling.  
\- Swagger hiển thị được.  
\- Test bằng Swagger/Postman thành công.

Với task frontend:

\- Giao diện hiển thị đúng.  
\- Gọi API thành công.  
\- Có loading state.  
\- Có error state.  
\- Có empty state nếu cần.  
\- Responsive cơ bản.

Với task database:

\- Có migration.  
\- Migration chạy được.  
\- Quan hệ đúng.  
\- Index cần thiết đã có.  
\- Seed data nếu cần.

---

# **18\. Checklist MVP tổng kết**

## **18.1. Backend**

* ASP.NET Core Web API chạy được.  
* Swagger hoạt động.  
* PostgreSQL kết nối được.  
* JWT Auth hoạt động.  
* API category hoạt động.  
* API region hoạt động.  
* API destination hoạt động.  
* API media upload hoạt động.  
* API virtual tour hoạt động.  
* API panorama hoạt động.  
* API hotspot hoạt động.  
* API public tour trả đúng cấu trúc.

## **18.2. Frontend Public**

* Trang chủ.  
* Trang bản đồ.  
* Marker địa điểm.  
* Filter danh mục.  
* Trang danh sách địa điểm.  
* Trang chi tiết địa điểm.  
* Trang tour 360\.  
* Hotspot navigation.  
* Hotspot info/audio/video cơ bản.  
* Responsive mobile cơ bản.

## **18.3. Frontend Admin**

* Trang đăng nhập.  
* Admin layout.  
* Dashboard.  
* Quản lý danh mục.  
* Quản lý địa điểm.  
* Map picker chọn tọa độ.  
* Media library.  
* Upload media.  
* Quản lý tour.  
* Quản lý panorama.  
* Hotspot editor.  
* Preview tour.

## **18.4. Database**

* roles.  
* users.  
* regions.  
* categories.  
* destinations.  
* media\_files.  
* virtual\_tours.  
* panoramas.  
* hotspots.  
* audio\_guides.  
* Index cơ bản.  
* Seed data demo.

## **18.5. Storage**

* MinIO chạy được.  
* Bucket được tạo.  
* Upload file thành công.  
* Lưu URL/path vào database.  
* Frontend hiển thị được file.

## **18.6. Demo Data**

* 5–7 địa điểm mẫu.  
* 7 danh mục.  
* 2–3 tour 360\.  
* Mỗi tour có nhiều panorama.  
* Có navigation hotspot.  
* Có info hotspot.  
* Có audio hoặc transcript.  
* Có ảnh đẹp đủ demo.

---

# **19\. Rủi ro và cách xử lý**

## **19.1. Dự án quá rộng**

Rủi ro:

Dự án có nhiều hướng như WebGIS 3D, AI, AR, model 3D, booking, analytics.  
Nếu triển khai hết ngay, MVP sẽ quá tải.

Cách xử lý:

Chỉ tập trung MVP:  
\- bản đồ  
\- địa điểm  
\- media  
\- tour 360  
\- panorama  
\- hotspot navigation  
\- admin dashboard

---

## **19.2. Ảnh panorama quá nặng**

Rủi ro:

Ảnh 360 dung lượng lớn làm web tải chậm.

Cách xử lý:

Nén ảnh trước khi upload.  
Tạo thumbnail.  
Lazy load panorama.  
Chỉ load scene hiện tại.  
Dùng định dạng webp nếu phù hợp.

---

## **19.3. Hotspot editor khó làm**

Rủi ro:

Đặt hotspot bằng yaw/pitch trong ảnh 360 có thể khó nếu chưa quen thư viện.

Cách xử lý:

Làm bản đơn giản trước:  
\- click lấy vị trí  
\- lưu yaw/pitch  
\- render hotspot  
Sau đó mới thêm kéo thả, preview nâng cao.

---

## **19.4. Upload media và storage lỗi**

Rủi ro:

Kết nối MinIO/S3, CORS file, public URL có thể gây lỗi.

Cách xử lý:

Dùng MinIO local trước.  
Test upload từng loại file.  
Chuẩn hóa storage path.  
Kiểm tra quyền public/presigned URL.

---

## **19.5. Dữ liệu demo chưa đủ đẹp**

Rủi ro:

Không có dữ liệu thực tế làm demo kém thuyết phục.

Cách xử lý:

Tạo dữ liệu giả lập lấy cảm hứng từ Cần Thơ/ĐBSCL.  
Dùng ảnh mẫu có giấy phép phù hợp.  
Dùng panorama mẫu.  
Viết mô tả địa điểm tốt.  
Ưu tiên demo luồng chức năng hơn dữ liệu thật.

---

# **20\. Gợi ý cách chia công việc nếu làm nhóm 2 người**

## **20.1. Người 1 \- Backend/Database**

Phụ trách:

Database schema  
EF Core migration  
Auth API  
Destination API  
Media API  
Virtual Tour API  
Panorama API  
Hotspot API  
Storage service  
Swagger  
Docker backend

## **20.2. Người 2 \- Frontend/UI**

Phụ trách:

Public website  
Admin dashboard  
Map picker  
Media library UI  
Destination form  
Tour management UI  
Panorama management UI  
Hotspot editor UI  
Tour 360 viewer  
Responsive UI

## **20.3. Cùng làm**

Thiết kế dữ liệu demo  
Test end-to-end  
Fix bug  
Deploy  
Viết báo cáo  
Quay video demo  
Làm slide thuyết trình

---

# **21\. Kế hoạch thời gian tham khảo 10 tuần**

| Tuần | Công việc chính |
| ----- | ----- |
| 1 | Setup project, Docker, database, tài liệu kỹ thuật |
| 2 | Backend core, auth, migration, seed data |
| 3 | API region/category/destination |
| 4 | Admin layout, destination management, map picker |
| 5 | Media upload, MinIO, media library |
| 6 | Public website, map, destination detail |
| 7 | Virtual tour, panorama management |
| 8 | Hotspot editor, navigation hotspot |
| 9 | Public tour 360 viewer, mobile responsive |
| 10 | Test, nhập dữ liệu demo, deploy, báo cáo, slide |

---

# **22\. Checklist cuối cùng trước khi demo**

Trước khi demo, cần đảm bảo:

* Docker Compose chạy được toàn bộ hệ thống.  
* Backend API không lỗi khi mở Swagger.  
* Frontend public mở được.  
* Admin login được.  
* Admin tạo được địa điểm.  
* Admin chọn được tọa độ trên bản đồ.  
* Admin upload được ảnh 360\.  
* Admin tạo được tour 360\.  
* Admin tạo được nhiều panorama.  
* Admin tạo được mũi tên điều hướng giữa panorama.  
* Du khách mở được bản đồ.  
* Du khách xem được địa điểm.  
* Du khách mở được tour 360\.  
* Du khách bấm mũi tên và chuyển được scene.  
* Giao diện không vỡ trên mobile.  
* Có dữ liệu demo đẹp.  
* README có hướng dẫn chạy.  
* Có video demo nếu cần.  
* Có slide/báo cáo nếu cần.

---

## **23\. Kết luận**

Kế hoạch triển khai **Smart Tourism 360 Platform** được chia thành các sprint rõ ràng để đảm bảo dự án có thể phát triển từng bước, tránh mở rộng quá mức và vẫn đạt được MVP có giá trị thực tế.

Trọng tâm của MVP là xây dựng được một hệ thống hoàn chỉnh từ admin đến du khách:

Admin tạo địa điểm trên bản đồ  
→ upload media và ảnh 360  
→ tạo tour 360  
→ tạo nhiều panorama  
→ đặt hotspot điều hướng  
→ du khách xem bản đồ  
→ mở địa điểm  
→ tham quan 360  
→ bấm mũi tên để di chuyển giữa các điểm nhìn

Nếu hoàn thành đầy đủ checklist trong tài liệu này, dự án sẽ đủ mạnh để demo như một nền tảng du lịch số 360 có khả năng phát triển thành sản phẩm thực tế. Sau MVP, hệ thống có thể mở rộng sang CesiumJS/WebGIS 3D, mô hình 3D, AI hướng dẫn viên, đa ngôn ngữ, gợi ý lịch trình và các dịch vụ du lịch thông minh khác.