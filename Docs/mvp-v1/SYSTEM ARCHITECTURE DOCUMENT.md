# **SYSTEM ARCHITECTURE DOCUMENT**

# **Smart Tourism 360 Platform**

## **Kiến trúc hệ thống nền tảng du lịch số tích hợp bản đồ tương tác và tham quan ảo 360 độ**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | System Architecture Document |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v1.0 |
| Mục đích | Mô tả kiến trúc tổng thể, thành phần kỹ thuật, luồng xử lý và định hướng mở rộng hệ thống |
| Đối tượng đọc | Nhóm phát triển, kiến trúc sư phần mềm, backend developer, frontend developer, database designer, DevOps |
| Phạm vi | Kiến trúc frontend, backend, database, object storage, bản đồ, tour 360, xác thực, media, deployment và khả năng mở rộng |

---

## **2\. Mục tiêu kiến trúc**

Kiến trúc của **Smart Tourism 360 Platform** được thiết kế nhằm đáp ứng các mục tiêu sau:

1. Cho phép du khách khám phá địa điểm du lịch thông qua bản đồ tương tác và tour 360\.  
2. Cho phép quản trị viên tạo địa điểm trực tiếp trên bản đồ bằng thao tác chọn vị trí.  
3. Cho phép quản trị viên quản lý thông tin địa điểm, ảnh, video, audio, ảnh 360, panorama và hotspot.  
4. Hỗ trợ tour 360 nhiều điểm nhìn, trong đó người dùng có thể bấm mũi tên để di chuyển giữa các panorama.  
5. Tách biệt rõ frontend, backend, database và storage để dễ phát triển, bảo trì và mở rộng.  
6. Thiết kế sẵn khả năng hỗ trợ đa ngôn ngữ, mô hình 3D, tuyến tham quan và WebGIS 3D trong tương lai.  
7. Phù hợp để phát triển MVP trước, sau đó mở rộng thành sản phẩm thực tế.

---

## **3\. Tổng quan kiến trúc hệ thống**

Hệ thống được thiết kế theo mô hình web application nhiều lớp, gồm các thành phần chính:

Client Browser  
   |  
   v  
Frontend Web Application  
   |  
   v  
Backend REST API  
   |  
   |----------------------|  
   v                      v  
PostgreSQL/PostGIS     Object Storage  
Database               MinIO / S3 / Cloudflare R2

Trong đó:

| Thành phần | Vai trò |
| ----- | ----- |
| Frontend Web Application | Hiển thị giao diện du khách, bản đồ, tour 360 và admin dashboard |
| Backend REST API | Xử lý nghiệp vụ, xác thực, quản lý dữ liệu, cung cấp API |
| PostgreSQL/PostGIS | Lưu dữ liệu hệ thống, địa điểm, tọa độ, tour, panorama, hotspot |
| Object Storage | Lưu file media như ảnh, ảnh 360, video, audio, model 3D |
| Map Viewer | Hiển thị bản đồ và marker địa điểm |
| 360 Viewer | Hiển thị không gian tham quan 360 và hotspot |
| Admin Dashboard | Công cụ quản lý dữ liệu du lịch |
| Authentication Service | Xác thực và phân quyền quản trị viên |

---

## **4\. Kiến trúc tổng thể mức cao**

\+------------------------------------------------------+  
|                    User Browser                      |  
|------------------------------------------------------|  
| Tourist UI                                           |  
| \- Home Page                                          |  
| \- Interactive Map                                    |  
| \- Destination Detail                                 |  
| \- 360 Virtual Tour Viewer                            |  
|                                                      |  
| Admin UI                                             |  
| \- Login                                              |  
| \- Dashboard                                          |  
| \- Destination Management                             |  
| \- Media Management                                   |  
| \- Virtual Tour Management                            |  
| \- Panorama & Hotspot Management                      |  
\+----------------------------|-------------------------+  
                             |  
                             | HTTPS / REST API  
                             v  
\+------------------------------------------------------+  
|                 ASP.NET Core Web API                 |  
|------------------------------------------------------|  
| Auth Module                                          |  
| Region Module                                        |  
| Category Module                                      |  
| Destination Module                                   |  
| Media Module                                         |  
| Virtual Tour Module                                  |  
| Panorama Module                                      |  
| Hotspot Module                                       |  
| Audio Guide Module                                   |  
| Translation Module                                   |  
| Analytics Module                                     |  
\+----------------------------|-------------------------+  
                             |  
          \+------------------+------------------+  
          |                                     |  
          v                                     v  
\+----------------------+              \+-------------------------+  
| PostgreSQL \+ PostGIS |              | Object Storage          |  
|----------------------|              |-------------------------|  
| Users                |              | Images                  |  
| Regions              |              | Panoramas               |  
| Categories           |              | Videos                  |  
| Destinations         |              | Audio                   |  
| Tours                |              | 3D Models               |  
| Panoramas            |              | Thumbnails              |  
| Hotspots             |              | Documents               |  
| Translations         |              \+-------------------------+  
| Analytics            |  
\+----------------------+

---

## **5\. Công nghệ đề xuất**

### **5.1. Frontend**

Frontend được đề xuất sử dụng:

Vue.js 3  
Pinia  
Vue Router  
TailwindCSS hoặc Bootstrap  
Leaflet hoặc Mapbox GL JS  
Photo Sphere Viewer hoặc Pannellum  
Axios hoặc Fetch API

Vai trò của frontend:

* Hiển thị giao diện du khách.  
* Hiển thị bản đồ tương tác.  
* Hiển thị danh sách và chi tiết địa điểm.  
* Hiển thị tour 360\.  
* Xử lý tương tác hotspot.  
* Hiển thị admin dashboard.  
* Gọi API từ backend.  
* Quản lý trạng thái đăng nhập admin.

### **5.2. Backend**

Backend được đề xuất sử dụng:

ASP.NET Core Web API  
Entity Framework Core  
JWT Authentication  
Swagger/OpenAPI  
FluentValidation nếu cần  
AutoMapper nếu cần

Vai trò của backend:

* Xác thực và phân quyền.  
* Cung cấp API cho frontend.  
* Quản lý địa điểm, danh mục, khu vực.  
* Quản lý tour 360, panorama, hotspot.  
* Quản lý metadata của media.  
* Tạo URL truy cập file từ object storage.  
* Ghi nhận dữ liệu analytics cơ bản.  
* Chuẩn bị cấu trúc mở rộng cho đa ngôn ngữ và model 3D.

### **5.3. Database**

Database được đề xuất sử dụng:

PostgreSQL  
PostGIS extension

Vai trò của database:

* Lưu thông tin người dùng/admin.  
* Lưu dữ liệu khu vực, địa điểm, danh mục.  
* Lưu tọa độ địa lý.  
* Lưu metadata media.  
* Lưu cấu trúc tour 360\.  
* Lưu panorama và hotspot.  
* Lưu liên kết điều hướng giữa các panorama.  
* Lưu dữ liệu đa ngôn ngữ nếu có.  
* Lưu analytics event cơ bản.

### **5.4. Object Storage**

Object storage được đề xuất sử dụng:

MinIO cho môi trường local/dev  
Cloudflare R2 hoặc AWS S3 cho production

Vai trò của object storage:

* Lưu ảnh đại diện.  
* Lưu ảnh gallery.  
* Lưu ảnh panorama 360\.  
* Lưu video.  
* Lưu audio.  
* Lưu thumbnail.  
* Lưu file mô hình 3D như .glb/.gltf trong tương lai.

### **5.5. Deployment**

Môi trường triển khai đề xuất:

Docker Compose cho local/dev/demo  
VPS hoặc cloud server cho production  
Nginx reverse proxy nếu cần  
HTTPS qua domain thật trong production

---

## **6\. Kiến trúc frontend**

Frontend gồm hai phần chính:

Tourist Frontend  
Admin Dashboard

### **6.1. Tourist Frontend**

Đây là giao diện dành cho du khách.

Các màn hình chính:

Home Page  
Explore Map Page  
Destination List Page  
Destination Detail Page  
Virtual Tour Viewer Page  
Route / Itinerary Page

Chức năng chính:

* Xem thông tin giới thiệu.  
* Xem bản đồ khu vực.  
* Lọc địa điểm theo danh mục.  
* Bấm marker để xem thông tin nhanh.  
* Xem chi tiết địa điểm.  
* Mở tour 360\.  
* Di chuyển giữa các panorama trong tour.  
* Tương tác với hotspot thông tin, audio, video.

### **6.2. Admin Dashboard**

Đây là giao diện dành cho quản trị viên.

Các màn hình chính:

Login Page  
Admin Overview Dashboard  
Region Management  
Category Management  
Destination Management  
Media Library  
Virtual Tour Management  
Panorama Management  
Hotspot Management  
Translation Management

Chức năng chính:

* Đăng nhập.  
* Quản lý khu vực.  
* Quản lý danh mục.  
* Tạo địa điểm mới.  
* Click bản đồ để chọn tọa độ.  
* Upload media.  
* Tạo tour 360\.  
* Upload nhiều panorama cho một tour.  
* Tạo hotspot trong panorama.  
* Cấu hình hotspot điều hướng sang panorama khác.  
* Xuất bản hoặc ẩn địa điểm.

---

## **7\. Kiến trúc backend**

Backend được tổ chức theo mô hình module/service để dễ bảo trì.

### **7.1. Các module chính**

Auth Module  
User Module  
Region Module  
Category Module  
Destination Module  
Media Module  
Virtual Tour Module  
Panorama Module  
Hotspot Module  
Audio Guide Module  
Video Module  
Model 3D Module  
Translation Module  
Analytics Module

### **7.2. Vai trò từng module**

| Module | Vai trò |
| ----- | ----- |
| Auth Module | Đăng nhập, cấp JWT, kiểm tra quyền |
| User Module | Quản lý tài khoản admin |
| Region Module | Quản lý khu vực triển khai |
| Category Module | Quản lý danh mục địa điểm |
| Destination Module | Quản lý địa điểm du lịch |
| Media Module | Upload file, lưu metadata, tạo URL |
| Virtual Tour Module | Quản lý tour 360 của địa điểm |
| Panorama Module | Quản lý từng điểm nhìn 360 trong tour |
| Hotspot Module | Quản lý hotspot thông tin, audio, video, navigation |
| Audio Guide Module | Quản lý audio thuyết minh |
| Video Module | Quản lý video giới thiệu |
| Model 3D Module | Thiết kế sẵn để hỗ trợ file .glb/.gltf |
| Translation Module | Quản lý bản dịch đa ngôn ngữ |
| Analytics Module | Ghi nhận sự kiện người dùng |

### **7.3. Gợi ý cấu trúc project backend**

SmartTourism360.Api  
  ├── Controllers  
  ├── Middlewares  
  ├── Filters  
  └── Program.cs

SmartTourism360.Application  
  ├── Services  
  ├── DTOs  
  ├── Interfaces  
  ├── Validators  
  └── UseCases

SmartTourism360.Domain  
  ├── Entities  
  ├── Enums  
  └── ValueObjects

SmartTourism360.Infrastructure  
  ├── Database  
  ├── Repositories  
  ├── Storage  
  ├── Authentication  
  └── ExternalServices

Nếu muốn MVP đơn giản hơn, có thể dùng cấu trúc 3 lớp:

API Layer  
Service Layer  
Data Access Layer

Tuy nhiên, nếu định hướng dự án chuyên nghiệp và dễ mở rộng, nên dùng cấu trúc gần với Clean Architecture.

---

## **8\. Kiến trúc database**

Database sử dụng PostgreSQL kết hợp PostGIS.

### **8.1. Nhóm bảng chính**

users  
roles

regions  
categories  
destinations  
destination\_categories

media\_files

virtual\_tours  
panoramas  
hotspots

audio\_guides  
videos  
models\_3d

routes  
route\_destinations

translations  
analytics\_events

### **8.2. Lý do dùng PostGIS**

PostGIS giúp hệ thống xử lý dữ liệu không gian tốt hơn. Trong MVP, có thể chỉ dùng latitude/longitude, nhưng về lâu dài PostGIS hỗ trợ:

Tìm địa điểm gần vị trí hiện tại  
Tính khoảng cách giữa các địa điểm  
Lưu vùng ranh giới khu vực  
Lưu tuyến đường tham quan  
Truy vấn địa điểm trong một vùng  
Tích hợp WebGIS nâng cao

### **8.3. Dữ liệu tọa độ địa điểm**

Mỗi địa điểm tối thiểu cần có:

latitude  
longitude

Có thể mở rộng thêm trường geometry:

location geometry(Point, 4326\)

Trong đó SRID 4326 là hệ tọa độ phổ biến dùng cho GPS.

---

## **9\. Kiến trúc object storage**

### **9.1. Lý do cần object storage**

Dự án có nhiều file media dung lượng lớn. Nếu lưu trực tiếp file trong database sẽ làm database nặng, khó backup và khó mở rộng.

Vì vậy, database chỉ lưu metadata, file thật lưu trong object storage.

### **9.2. Cấu trúc lưu trữ đề xuất**

smart-tourism-360/  
  ├── regions/  
  │   └── can-tho-demo/  
  │       └── destinations/  
  │           └── vuon-trai-cay-demo/  
  │               ├── images/  
  │               ├── panoramas/  
  │               ├── videos/  
  │               ├── audio/  
  │               ├── models/  
  │               └── thumbnails/

### **9.3. Luồng upload file**

Admin chọn file  
→ Frontend gửi file đến Backend  
→ Backend kiểm tra định dạng và dung lượng  
→ Backend upload file lên Object Storage  
→ Object Storage trả về path/url  
→ Backend lưu metadata vào database  
→ Frontend hiển thị media vừa upload

### **9.4. Các loại file cần hỗ trợ**

| Loại | Định dạng |
| ----- | ----- |
| Ảnh thường | jpg, jpeg, png, webp |
| Ảnh 360 | jpg, jpeg, webp |
| Video | mp4, webm |
| Audio | mp3, wav, ogg |
| Mô hình 3D | glb, gltf |
| Thumbnail | jpg, webp |

---

## **10\. Kiến trúc bản đồ tương tác**

### **10.1. Vai trò bản đồ**

Bản đồ là thành phần trung tâm giúp du khách hiểu được vị trí của các địa điểm trong khu vực.

Bản đồ được dùng ở hai nơi:

Giao diện du khách:  
\- Xem marker địa điểm  
\- Lọc địa điểm  
\- Bấm marker để xem thông tin nhanh  
\- Mở chi tiết địa điểm

Giao diện admin:  
\- Click bản đồ để chọn tọa độ địa điểm  
\- Chỉnh sửa vị trí địa điểm  
\- Kiểm tra vị trí hiển thị

### **10.2. Công nghệ bản đồ**

MVP có thể dùng:

Leaflet  
hoặc Mapbox GL JS

Khuyến nghị:

Leaflet nếu muốn nhẹ, dễ học, miễn phí, phù hợp MVP.  
Mapbox GL JS nếu muốn giao diện hiện đại hơn, vector map đẹp hơn.

### **10.3. Luồng hiển thị bản đồ cho du khách**

Du khách mở trang bản đồ  
→ Frontend gọi API lấy danh sách địa điểm published  
→ Backend trả về danh sách địa điểm gồm tên, tọa độ, danh mục, ảnh đại diện  
→ Frontend render marker trên bản đồ  
→ Du khách bấm marker  
→ Popup hiển thị thông tin tóm tắt  
→ Du khách bấm "Xem chi tiết" hoặc "Tham quan 360"

### **10.4. Luồng chọn vị trí địa điểm cho admin**

Admin mở form tạo địa điểm  
→ Bản đồ hiển thị ở chế độ chọn vị trí  
→ Admin click vào bản đồ  
→ Frontend lấy latitude/longitude  
→ Marker tạm thời xuất hiện  
→ Tọa độ được điền vào form  
→ Admin lưu địa điểm  
→ Backend lưu tọa độ vào database

---

## **11\. Kiến trúc tour 360**

### **11.1. Khái niệm tổng quan**

Tour 360 là một trải nghiệm tham quan ảo của một địa điểm. Một tour gồm nhiều panorama. Mỗi panorama là một điểm đứng trong không gian 360\. Người dùng có thể di chuyển giữa các panorama bằng các hotspot điều hướng dạng mũi tên.

Cấu trúc:

Destination  
  └── Virtual Tour  
        ├── Panorama 1  
        │     ├── Hotspot A: Info  
        │     └── Hotspot B: Navigation → Panorama 2  
        ├── Panorama 2  
        │     ├── Hotspot C: Navigation → Panorama 1  
        │     ├── Hotspot D: Navigation → Panorama 3  
        │     └── Hotspot E: Audio  
        └── Panorama 3  
              └── Hotspot F: Navigation → Panorama 2

### **11.2. Thành phần của 360 Viewer**

360 Viewer trên frontend cần có:

Panorama canvas/viewer  
Hotspot renderer  
Navigation hotspot handler  
Info hotspot popup  
Audio/video handler  
Scene transition handler  
Mini scene list nếu cần  
Back to destination button  
Fullscreen mode nếu cần

### **11.3. Navigation hotspot**

Navigation hotspot là loại hotspot dùng để di chuyển giữa các panorama.

Ví dụ:

Current Panorama: Cổng chính  
Hotspot Type: navigation  
Label: Đi vào sân chùa  
Target Panorama: Sân chùa

Khi người dùng bấm hotspot:

Frontend kiểm tra type \= navigation  
→ Lấy target\_panorama\_id  
→ Tìm panorama đích trong dữ liệu tour  
→ Load ảnh 360 của panorama đích  
→ Render lại hotspot của panorama mới  
→ Cập nhật tiêu đề scene hiện tại

### **11.4. Scene Navigation Graph**

Các panorama trong một tour tạo thành một mạng lưới di chuyển, gọi là **Scene Navigation Graph**.

Ví dụ:

Cổng chính ↔ Sân trung tâm ↔ Chánh điện  
                  ↘ Khu tháp  
                  ↘ Khu vườn

Trong database, mạng lưới này được biểu diễn bằng các hotspot loại `navigation` có trường `target_panorama_id`.

### **11.5. Luồng tải tour 360**

Du khách bấm "Tham quan 360"  
→ Frontend gọi API lấy tour của địa điểm  
→ Backend trả về virtual tour, danh sách panorama và hotspot  
→ Frontend mở panorama mặc định  
→ Frontend render các hotspot trên panorama  
→ Du khách xoay nhìn 360  
→ Du khách bấm mũi tên navigation  
→ Frontend chuyển sang panorama đích

### **11.6. Dữ liệu tour trả về cho frontend**

Backend nên trả về dữ liệu tour theo cấu trúc gồm:

Tour information  
Default panorama  
List of panoramas  
List of hotspots for each panorama  
Media URL for panorama image  
Target panorama for navigation hotspot

Ví dụ:

{  
  "id": "tour-001",  
  "destinationId": "destination-001",  
  "title": "Tham quan Chùa Demo",  
  "defaultPanoramaId": "pano-001",  
  "panoramas": \[  
    {  
      "id": "pano-001",  
      "title": "Cổng chính",  
      "imageUrl": "/storage/panoramas/cong-chinh.jpg",  
      "hotspots": \[  
        {  
          "id": "hotspot-001",  
          "type": "navigation",  
          "title": "Đi vào sân chùa",  
          "yaw": 120,  
          "pitch": \-5,  
          "targetPanoramaId": "pano-002"  
        },  
        {  
          "id": "hotspot-002",  
          "type": "info",  
          "title": "Cổng chùa",  
          "description": "Thông tin giới thiệu về cổng chùa.",  
          "yaw": 40,  
          "pitch": 0  
        }  
      \]  
    },  
    {  
      "id": "pano-002",  
      "title": "Sân chùa",  
      "imageUrl": "/storage/panoramas/san-chua.jpg",  
      "hotspots": \[  
        {  
          "id": "hotspot-003",  
          "type": "navigation",  
          "title": "Quay lại cổng chính",  
          "yaw": \-80,  
          "pitch": \-3,  
          "targetPanoramaId": "pano-001"  
        }  
      \]  
    }  
  \]  
}

---

## **12\. Kiến trúc admin tạo tour 360**

### **12.1. Luồng tạo tour**

Admin chọn một địa điểm  
→ Tạo virtual tour  
→ Upload nhiều ảnh panorama  
→ Mỗi ảnh trở thành một panorama  
→ Chọn panorama mặc định  
→ Mở từng panorama để đặt hotspot  
→ Với hotspot navigation, chọn panorama đích  
→ Lưu tour  
→ Xuất bản tour

### **12.2. Các thao tác admin cần có**

Tạo tour  
Sửa tên tour  
Upload panorama  
Sắp xếp panorama  
Chọn panorama mở đầu  
Xem preview panorama  
Đặt hotspot bằng cách click vào ảnh 360  
Chọn loại hotspot  
Nhập nội dung hotspot  
Chọn media liên quan  
Chọn target panorama nếu là navigation hotspot  
Ẩn/hiện hotspot  
Preview toàn bộ tour  
Xuất bản tour

### **12.3. Cách đặt hotspot**

Trong giao diện admin, khi mở một panorama ở chế độ chỉnh sửa:

Admin xoay đến vị trí mong muốn  
→ Click vào vị trí trên ảnh 360  
→ Hệ thống lấy yaw/pitch  
→ Mở form tạo hotspot  
→ Admin chọn loại hotspot  
→ Admin nhập thông tin  
→ Admin lưu hotspot

Hệ thống lưu:

panorama\_id  
type  
title  
description  
yaw  
pitch  
target\_panorama\_id nếu có  
media\_id nếu có

---

## **13\. Kiến trúc xác thực và phân quyền**

### **13.1. Nguyên tắc**

Trong MVP, chỉ admin cần đăng nhập. Du khách có thể truy cập công khai mà không cần tài khoản.

### **13.2. Công nghệ xác thực**

Backend sử dụng:

JWT Authentication  
Password hashing  
Role-based authorization

### **13.3. Vai trò người dùng**

| Vai trò | Quyền |
| ----- | ----- |
| Super Admin | Quản lý toàn bộ hệ thống |
| Admin | Quản lý địa điểm, media, tour, hotspot |
| Content Manager | Quản lý nội dung nhưng có thể hạn chế quyền hệ thống |
| Tourist | Người dùng công khai, chưa cần đăng nhập trong MVP |

### **13.4. Luồng đăng nhập admin**

Admin nhập email/password  
→ Frontend gửi request login  
→ Backend kiểm tra tài khoản  
→ Backend tạo JWT token  
→ Frontend lưu token  
→ Các request admin gửi kèm token  
→ Backend kiểm tra token và quyền

---

## **14\. Kiến trúc đa ngôn ngữ**

### **14.1. Định hướng**

MVP làm tiếng Việt trước, nhưng hệ thống thiết kế sẵn để hỗ trợ đa ngôn ngữ.

Các nội dung có thể dịch:

Tên địa điểm  
Mô tả ngắn  
Mô tả chi tiết  
Tên tour  
Tên panorama  
Nội dung hotspot  
Transcript audio  
Tên tuyến tham quan

### **14.2. Cách thiết kế**

Dùng bảng translations tổng quát:

entity\_type  
entity\_id  
field\_name  
language\_code  
translated\_value

Ví dụ:

entity\_type: destination  
entity\_id: destination-001  
field\_name: full\_description  
language\_code: en  
translated\_value: English description...

### **14.3. Luồng lấy dữ liệu đa ngôn ngữ**

Frontend gửi request kèm language\_code  
→ Backend lấy dữ liệu gốc  
→ Backend kiểm tra bản dịch nếu có  
→ Backend trả về nội dung theo ngôn ngữ yêu cầu  
→ Nếu chưa có bản dịch, trả về tiếng Việt mặc định

---

## **15\. Kiến trúc media handling**

### **15.1. Quy trình upload media**

Admin upload file  
→ Backend validate file  
→ Backend tạo tên file chuẩn  
→ Backend upload lên object storage  
→ Backend lưu metadata vào media\_files  
→ Backend trả về media\_id và file\_url

### **15.2. Validate file**

Backend cần kiểm tra:

File extension  
MIME type  
File size  
Media type  
User permission

### **15.3. Gợi ý giới hạn dung lượng MVP**

| Loại file | Giới hạn đề xuất |
| ----- | ----- |
| Ảnh thường | 5 MB |
| Ảnh 360 | 20 MB |
| Video | 100 MB |
| Audio | 20 MB |
| Model 3D | 50 MB |

Các giới hạn này có thể thay đổi tùy môi trường triển khai.

---

## **16\. Kiến trúc analytics cơ bản**

Analytics không phải phần bắt buộc của MVP, nhưng nên thiết kế sẵn để ghi nhận các sự kiện đơn giản.

### **16.1. Sự kiện có thể ghi nhận**

view\_destination  
open\_virtual\_tour  
click\_hotspot  
navigate\_panorama  
play\_audio  
play\_video  
click\_map\_marker  
search\_destination  
filter\_category

### **16.2. Luồng ghi nhận analytics**

User thực hiện hành động  
→ Frontend gửi analytics event  
→ Backend lưu vào analytics\_events  
→ Admin có thể xem thống kê cơ bản trong tương lai

---

## **17\. Kiến trúc triển khai**

### **17.1. Môi trường development**

Môi trường development có thể chạy bằng Docker Compose:

frontend  
backend  
postgres  
minio

Ví dụ:

Frontend: http://localhost:5173  
Backend API: http://localhost:5000  
PostgreSQL: localhost:5432  
MinIO Console: http://localhost:9001

### **17.2. Môi trường production**

Môi trường production có thể gồm:

Frontend deploy trên Vercel/Netlify hoặc Nginx  
Backend deploy trên VPS/container  
PostgreSQL deploy trên VPS/cloud database  
Object Storage dùng Cloudflare R2/S3/MinIO  
Domain riêng  
HTTPS

### **17.3. Kiến trúc production đề xuất**

User Browser  
   |  
   v  
Domain \+ HTTPS  
   |  
   v  
Nginx / Reverse Proxy  
   |  
   |--------------------|  
   v                    v  
Frontend Static      Backend API  
                          |  
           \+--------------+--------------+  
           v                             v  
     PostgreSQL                    Object Storage

---

## **18\. Luồng nghiệp vụ chính**

### **18.1. Luồng du khách khám phá địa điểm**

Du khách mở website  
→ Xem trang chủ  
→ Mở bản đồ khám phá  
→ Lọc địa điểm theo danh mục  
→ Bấm vào marker  
→ Xem popup thông tin nhanh  
→ Mở trang chi tiết địa điểm  
→ Bấm "Tham quan 360"  
→ Xem panorama mặc định  
→ Bấm hotspot navigation để di chuyển  
→ Bấm hotspot info/audio/video để xem nội dung

### **18.2. Luồng admin tạo địa điểm**

Admin đăng nhập  
→ Mở dashboard  
→ Chọn quản lý địa điểm  
→ Chọn thêm địa điểm mới  
→ Click bản đồ để lấy tọa độ  
→ Nhập thông tin địa điểm  
→ Chọn danh mục  
→ Upload ảnh đại diện  
→ Lưu bản nháp  
→ Xuất bản khi hoàn tất

### **18.3. Luồng admin tạo tour 360**

Admin chọn địa điểm  
→ Tạo virtual tour  
→ Upload các ảnh 360  
→ Tạo panorama từ từng ảnh  
→ Chọn panorama mặc định  
→ Mở panorama editor  
→ Đặt hotspot thông tin  
→ Đặt hotspot điều hướng sang panorama khác  
→ Preview tour  
→ Xuất bản tour

### **18.4. Luồng người dùng di chuyển trong tour 360**

User đang đứng ở Panorama A  
→ User bấm mũi tên "Đi tới khu vực B"  
→ Frontend đọc target\_panorama\_id  
→ Frontend load Panorama B  
→ Viewer cập nhật ảnh 360  
→ Hotspot của Panorama B được hiển thị  
→ User tiếp tục khám phá

---

## **19\. Yêu cầu phi chức năng cho kiến trúc**

### **19.1. Hiệu năng**

Hệ thống cần đảm bảo:

Tải bản đồ nhanh  
Tải danh sách địa điểm nhanh  
Ảnh 360 không làm đứng trình duyệt  
API phản hồi ổn định  
Media được lazy load khi cần

### **19.2. Khả năng mở rộng**

Kiến trúc cần hỗ trợ:

Thêm nhiều địa điểm  
Thêm nhiều khu vực  
Thêm nhiều tour 360  
Thêm nhiều loại media  
Thêm đa ngôn ngữ  
Thêm WebGIS 3D  
Thêm model 3D

### **19.3. Bảo mật**

Hệ thống cần:

JWT authentication cho admin  
Hash mật khẩu  
Phân quyền API admin  
Validate file upload  
Giới hạn dung lượng file  
Không cho upload file nguy hiểm  
CORS cấu hình đúng  
HTTPS trong production

### **19.4. Khả dụng trên mobile**

Vì du khách thường dùng điện thoại, frontend cần:

Responsive layout  
Map dễ thao tác bằng cảm ứng  
Tour 360 xoay mượt  
Hotspot đủ lớn để bấm  
Ảnh 360 được tối ưu dung lượng

---

## **20\. Khả năng mở rộng tương lai**

### **20.1. WebGIS 3D**

Sau MVP, có thể tích hợp:

CesiumJS  
ArcGIS Maps SDK for JavaScript  
3D Tiles  
Scene Layer  
Terrain  
Building models  
GIS layers

Ứng dụng:

Hiển thị bản đồ 3D  
Hiển thị mô hình công trình  
Hiển thị lớp quy hoạch, giao thông, du lịch  
Hiển thị tuyến tham quan 3D

### **20.2. Mô hình 3D**

Có thể tích hợp Three.js hoặc viewer tương ứng để xem:

.glb  
.gltf  
3D object  
digital artifact  
building model

### **20.3. AI chatbot hướng dẫn viên**

Có thể bổ sung chatbot trả lời câu hỏi dựa trên dữ liệu địa điểm.

Ví dụ:

Địa điểm này có gì nổi bật?  
Nên đi tuyến nào trong nửa ngày?  
Có điểm nào gần đây không?  
Lịch sử địa điểm này là gì?

### **20.4. Tài khoản du khách**

Sau MVP có thể thêm:

Đăng ký/đăng nhập  
Lưu địa điểm yêu thích  
Lịch trình cá nhân  
Đánh giá địa điểm  
Lịch sử tham quan

### **20.5. Multi-region / Multi-tenant**

Hệ thống có thể mở rộng để nhiều địa phương cùng sử dụng.

Ví dụ:

Region: Cần Thơ  
Region: An Giang  
Region: Vĩnh Long  
Region: Đồng Tháp

Mỗi region có bộ địa điểm, tour, media và admin riêng.

---

## **21\. Kiến trúc MVP đề xuất**

Đối với giai đoạn MVP, nên tập trung vào các thành phần sau:

Frontend:  
\- Trang chủ  
\- Bản đồ khám phá  
\- Chi tiết địa điểm  
\- 360 tour viewer  
\- Admin dashboard

Backend:  
\- Auth  
\- Category  
\- Destination  
\- Media  
\- Virtual Tour  
\- Panorama  
\- Hotspot

Database:  
\- users  
\- regions  
\- categories  
\- destinations  
\- media\_files  
\- virtual\_tours  
\- panoramas  
\- hotspots

Storage:  
\- images  
\- panoramas  
\- audio  
\- video

Các thành phần chưa bắt buộc trong MVP nhưng nên thiết kế sẵn:

translations  
models\_3d  
routes  
analytics\_events  
tourist accounts

---

## **22\. Kiến trúc dữ liệu tour 360 trong MVP**

Đây là phần cốt lõi cần đảm bảo không thiếu.

Destination  
  id  
  name  
  latitude  
  longitude

VirtualTour  
  id  
  destination\_id  
  title  
  default\_panorama\_id

Panorama  
  id  
  virtual\_tour\_id  
  title  
  panorama\_image\_id  
  initial\_yaw  
  initial\_pitch  
  initial\_fov

Hotspot  
  id  
  panorama\_id  
  type  
  title  
  yaw  
  pitch  
  description  
  media\_id  
  target\_panorama\_id

Trong đó:

Hotspot type \= navigation  
→ dùng target\_panorama\_id  
→ tạo mũi tên đi đến điểm nhìn khác

Hotspot type \= info  
→ dùng title \+ description

Hotspot type \= audio/video/image/model3d  
→ dùng media\_id

Cấu trúc này đảm bảo hệ thống có thể tạo trải nghiệm giống các website tham quan 360 chuyên nghiệp.

---

## **23\. Kết luận**

Kiến trúc của **Smart Tourism 360 Platform** được thiết kế theo hướng rõ ràng, dễ triển khai MVP và có khả năng mở rộng lâu dài. Hệ thống tách biệt frontend, backend, database và object storage, giúp việc phát triển, bảo trì và triển khai trở nên thuận lợi.

Trong MVP, hệ thống tập trung vào các chức năng cốt lõi gồm bản đồ tương tác, quản lý địa điểm, upload media, tạo tour 360 nhiều panorama, tạo hotspot và điều hướng giữa các panorama bằng mũi tên. Đây là nền tảng quan trọng để tạo ra trải nghiệm tham quan ảo tương tự các hệ thống du lịch 360 chuyên nghiệp.

Sau khi MVP hoàn thiện, kiến trúc có thể mở rộng sang WebGIS 3D, mô hình 3D, đa ngôn ngữ, AI hướng dẫn viên, gợi ý lịch trình, tài khoản du khách và hệ thống nhiều khu vực. Điều này giúp dự án có thể phát triển từ một demo học thuật thành một nền tảng du lịch số thực tế phục vụ địa phương.

