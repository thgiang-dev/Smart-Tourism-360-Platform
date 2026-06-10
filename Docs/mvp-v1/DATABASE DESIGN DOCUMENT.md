# **DATABASE DESIGN DOCUMENT**

# **Smart Tourism 360 Platform**

## **Thiết kế cơ sở dữ liệu cho nền tảng du lịch số tích hợp bản đồ tương tác và tham quan ảo 360 độ**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | Database Design Document |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v1.0 |
| Mục đích | Mô tả thiết kế cơ sở dữ liệu phục vụ quản lý địa điểm du lịch, bản đồ, media, tour 360, panorama và hotspot |
| Database đề xuất | PostgreSQL \+ PostGIS |
| Đối tượng đọc | Backend developer, database designer, system architect, frontend developer, nhóm triển khai dự án |

---

## **2\. Mục tiêu thiết kế database**

Cơ sở dữ liệu của **Smart Tourism 360 Platform** được thiết kế để phục vụ các mục tiêu chính sau:

1. Quản lý dữ liệu khu vực triển khai.  
2. Quản lý danh mục địa điểm du lịch.  
3. Quản lý địa điểm du lịch và tọa độ bản đồ.  
4. Quản lý file media như ảnh, ảnh 360, video, audio và mô hình 3D.  
5. Quản lý tour 360 của từng địa điểm.  
6. Quản lý nhiều panorama trong một tour 360\.  
7. Quản lý hotspot trong từng panorama.  
8. Hỗ trợ hotspot điều hướng dạng mũi tên để di chuyển giữa các panorama.  
9. Hỗ trợ dữ liệu đa ngôn ngữ trong tương lai.  
10. Hỗ trợ mở rộng sang tuyến tham quan, analytics, tài khoản du khách, mô hình 3D và WebGIS 3D.

Trong giai đoạn MVP, database tập trung vào các nhóm dữ liệu cốt lõi:

users  
roles  
regions  
categories  
destinations  
media\_files  
virtual\_tours  
panoramas  
hotspots  
audio\_guides

Các nhóm dữ liệu mở rộng được thiết kế sẵn hoặc mô tả ở mức định hướng:

translations  
videos  
models\_3d  
routes  
route\_destinations  
analytics\_events

---

## **3\. Công nghệ cơ sở dữ liệu**

### **3.1. Database chính**

Hệ thống sử dụng:

PostgreSQL

PostgreSQL phù hợp vì:

* Mạnh, ổn định và phổ biến.  
* Hỗ trợ dữ liệu quan hệ tốt.  
* Hỗ trợ JSONB cho metadata linh hoạt.  
* Kết hợp tốt với ASP.NET Core và Entity Framework Core.  
* Có thể mở rộng với PostGIS cho dữ liệu không gian.

### **3.2. PostGIS**

Hệ thống sử dụng thêm:

PostGIS extension

PostGIS giúp xử lý dữ liệu địa lý, ví dụ:

* Lưu vị trí địa điểm bằng kiểu geometry/geography.  
* Tìm địa điểm gần một vị trí.  
* Tính khoảng cách giữa các địa điểm.  
* Mở rộng sang route, polygon, boundary, GIS layer trong tương lai.

Trong MVP, có thể lưu song song:

latitude  
longitude  
location geometry(Point, 4326\)

Trong đó:

* `latitude`: vĩ độ.  
* `longitude`: kinh độ.  
* `location`: điểm không gian dùng cho truy vấn PostGIS.  
* `4326`: hệ tọa độ WGS84, phổ biến cho GPS và web map.

---

## **4\. Nguyên tắc thiết kế database**

### **4.1. Ưu tiên rõ ràng và dễ mở rộng**

Database cần đủ rõ để triển khai MVP nhanh, nhưng vẫn mở để phát triển thêm các tính năng nâng cao.

### **4.2. Tách metadata và file vật lý**

File thật không lưu trong database. Database chỉ lưu metadata và URL/path.

Ví dụ:

Ảnh 360 thật nằm trong MinIO/S3/R2  
Database chỉ lưu file\_url, storage\_path, media\_type, mime\_type, size

### **4.3. Dùng trạng thái xuất bản**

Các bảng nội dung chính nên có trạng thái:

draft  
published  
archived

Điều này giúp admin có thể tạo dữ liệu nháp trước khi hiển thị cho du khách.

### **4.4. Hỗ trợ audit fields**

Các bảng chính nên có:

created\_at  
updated\_at  
created\_by  
updated\_by

Điều này giúp kiểm soát ai tạo, ai cập nhật và cập nhật khi nào.

### **4.5. Không hard-code địa phương**

Database không khóa cứng vào Cần Thơ hay một tỉnh cụ thể. Thay vào đó, dùng bảng `regions` để có thể triển khai cho nhiều khu vực.

### **4.6. Thiết kế tour 360 theo mô hình nhiều scene**

Tour 360 không chỉ là một ảnh 360 đơn lẻ. Một tour gồm nhiều panorama, mỗi panorama có nhiều hotspot. Hotspot loại `navigation` sẽ trỏ đến panorama khác thông qua `target_panorama_id`.

---

## **5\. Tổng quan ERD dạng mô tả**

Quan hệ chính trong hệ thống:

roles 1 \--- n users

regions 1 \--- n destinations

categories 1 \--- n destinations

destinations 1 \--- n media\_files  
destinations 1 \--- n virtual\_tours  
destinations 1 \--- n audio\_guides

virtual\_tours 1 \--- n panoramas

panoramas 1 \--- n hotspots

media\_files 1 \--- n panoramas  
media\_files 1 \--- n hotspots  
media\_files 1 \--- n audio\_guides

hotspots n \--- 1 panoramas  
hotspots n \--- 1 target\_panorama

Mô hình tour 360:

Destination  
  └── VirtualTour  
        ├── Panorama A  
        │     ├── Hotspot info  
        │     └── Hotspot navigation → Panorama B  
        ├── Panorama B  
        │     ├── Hotspot navigation → Panorama A  
        │     └── Hotspot navigation → Panorama C  
        └── Panorama C  
              └── Hotspot audio

---

## **6\. Danh sách bảng chính**

| STT | Bảng | Mục đích |
| ----- | ----- | ----- |
| 1 | roles | Lưu vai trò người dùng |
| 2 | users | Lưu tài khoản admin/người dùng |
| 3 | regions | Lưu khu vực triển khai |
| 4 | categories | Lưu danh mục địa điểm |
| 5 | destinations | Lưu địa điểm du lịch |
| 6 | media\_files | Lưu metadata file media |
| 7 | virtual\_tours | Lưu tour 360 của địa điểm |
| 8 | panoramas | Lưu các điểm nhìn 360 trong tour |
| 9 | hotspots | Lưu điểm tương tác trong panorama |
| 10 | audio\_guides | Lưu audio thuyết minh |
| 11 | translations | Lưu bản dịch đa ngôn ngữ |
| 12 | videos | Lưu metadata video gắn với đối tượng |
| 13 | models\_3d | Lưu metadata mô hình 3D |
| 14 | routes | Lưu tuyến tham quan |
| 15 | route\_destinations | Lưu địa điểm thuộc tuyến |
| 16 | analytics\_events | Lưu sự kiện hành vi người dùng |

Trong MVP, có thể triển khai trước bảng 1 đến 10\. Các bảng còn lại có thể triển khai sau hoặc tạo sẵn ở mức đơn giản.

---

# **PHẦN A: THIẾT KẾ BẢNG CỐT LÕI CHO MVP**

---

## **7\. Bảng `roles`**

### **7.1. Mục đích**

Bảng `roles` lưu danh sách vai trò trong hệ thống.

Ví dụ:

Super Admin  
Admin  
Content Manager  
Viewer  
Tourist

Trong MVP, tối thiểu cần:

Super Admin  
Admin

### **7.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| name | varchar(100) | Có | Tên vai trò |
| code | varchar(50) | Có | Mã vai trò |
| description | text | Không | Mô tả |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

### **7.3. Ràng buộc**

Primary key: id  
Unique: code

---

## **8\. Bảng `users`**

### **8.1. Mục đích**

Bảng `users` lưu tài khoản người dùng hệ thống. Trong MVP, chủ yếu dùng cho admin.

### **8.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| role\_id | uuid | Có | Khóa ngoại đến roles |
| full\_name | varchar(150) | Có | Họ tên |
| email | varchar(255) | Có | Email đăng nhập |
| password\_hash | text | Có | Mật khẩu đã hash |
| avatar\_id | uuid | Không | Ảnh đại diện |
| status | varchar(30) | Có | active/inactive/locked |
| last\_login\_at | timestamp | Không | Lần đăng nhập gần nhất |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

### **8.3. Ràng buộc**

Primary key: id  
Foreign key: role\_id references roles(id)  
Unique: email

### **8.4. Ghi chú**

MVP chưa bắt buộc du khách đăng nhập. Tuy nhiên, bảng `users` vẫn có thể mở rộng cho tài khoản du khách sau này.

---

## **9\. Bảng `regions`**

### **9.1. Mục đích**

Bảng `regions` lưu khu vực triển khai. Một region có thể là một tỉnh, thành phố, huyện, xã, cụm điểm du lịch hoặc tuyến du lịch.

Ví dụ:

Cần Thơ Demo  
Cù lao An Bình Demo  
Vùng du lịch sinh thái ven sông Demo

### **9.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| name | varchar(200) | Có | Tên khu vực |
| slug | varchar(250) | Có | Slug URL |
| description | text | Không | Mô tả |
| province | varchar(150) | Không | Tỉnh/thành |
| district | varchar(150) | Không | Quận/huyện |
| ward | varchar(150) | Không | Xã/phường |
| center\_latitude | decimal(10,7) | Không | Vĩ độ trung tâm |
| center\_longitude | decimal(10,7) | Không | Kinh độ trung tâm |
| default\_zoom | integer | Không | Mức zoom mặc định |
| cover\_image\_id | uuid | Không | Ảnh đại diện |
| status | varchar(30) | Có | draft/published/archived |
| created\_by | uuid | Không | Người tạo |
| updated\_by | uuid | Không | Người cập nhật |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

### **9.3. Ràng buộc**

Primary key: id  
Unique: slug  
Foreign key: cover\_image\_id references media\_files(id), nullable

### **9.4. Ghi chú**

Trong MVP, có thể tạo sẵn một region:

name: Cần Thơ Demo  
slug: can-tho-demo

---

## **10\. Bảng `categories`**

### **10.1. Mục đích**

Bảng `categories` lưu danh mục phân loại địa điểm.

Ví dụ:

Văn hóa \- lịch sử  
Tâm linh  
Sinh thái  
Nông nghiệp  
Làng nghề  
Ẩm thực  
Lưu trú

### **10.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| name | varchar(150) | Có | Tên danh mục |
| slug | varchar(180) | Có | Slug |
| description | text | Không | Mô tả |
| icon | varchar(100) | Không | Tên icon |
| color | varchar(30) | Không | Màu đại diện |
| display\_order | integer | Không | Thứ tự hiển thị |
| status | varchar(30) | Có | active/inactive |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

### **10.3. Ràng buộc**

Primary key: id  
Unique: slug

---

## **11\. Bảng `destinations`**

### **11.1. Mục đích**

Bảng `destinations` là bảng trung tâm, lưu thông tin các địa điểm du lịch.

Một địa điểm có thể là:

Chùa  
Nhà cổ  
Vườn trái cây  
Khu sinh thái  
Làng nghề  
Homestay  
Quán ăn đặc sản  
Bến tàu  
Điểm check-in

### **11.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| region\_id | uuid | Có | Khu vực chứa địa điểm |
| category\_id | uuid | Có | Danh mục chính |
| name | varchar(250) | Có | Tên địa điểm |
| slug | varchar(300) | Có | Slug |
| short\_description | text | Có | Mô tả ngắn |
| full\_description | text | Không | Mô tả chi tiết |
| address | varchar(500) | Không | Địa chỉ |
| latitude | decimal(10,7) | Có | Vĩ độ |
| longitude | decimal(10,7) | Có | Kinh độ |
| location | geometry(Point, 4326\) | Không | Tọa độ PostGIS |
| cover\_image\_id | uuid | Không | Ảnh đại diện |
| opening\_hours | jsonb | Không | Giờ mở cửa |
| ticket\_price | varchar(150) | Không | Giá vé hoặc ghi chú |
| contact\_phone | varchar(50) | Không | Số điện thoại |
| contact\_email | varchar(255) | Không | Email |
| website\_url | text | Không | Website |
| facebook\_url | text | Không | Facebook/Zalo page |
| has\_virtual\_tour | boolean | Có | Có tour 360 hay không |
| has\_audio\_guide | boolean | Có | Có audio guide hay không |
| has\_3d\_model | boolean | Có | Có model 3D hay không |
| status | varchar(30) | Có | draft/published/archived |
| created\_by | uuid | Không | Người tạo |
| updated\_by | uuid | Không | Người cập nhật |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

### **11.3. Ràng buộc**

Primary key: id  
Foreign key: region\_id references regions(id)  
Foreign key: category\_id references categories(id)  
Foreign key: cover\_image\_id references media\_files(id), nullable  
Unique: slug within region

### **11.4. Ghi chú về `location`**

Trường `latitude` và `longitude` giúp frontend dễ xử lý.

Trường `location` phục vụ PostGIS. Khi tạo địa điểm, backend có thể tự tạo:

ST\_SetSRID(ST\_MakePoint(longitude, latitude), 4326\)

Lưu ý: PostGIS dùng thứ tự:

longitude trước  
latitude sau

### **11.5. Ví dụ `opening_hours`**

{  
  "monday": "07:00-17:00",  
  "tuesday": "07:00-17:00",  
  "wednesday": "07:00-17:00",  
  "thursday": "07:00-17:00",  
  "friday": "07:00-17:00",  
  "saturday": "07:00-18:00",  
  "sunday": "07:00-18:00"  
}

---

## **12\. Bảng `media_files`**

### **12.1. Mục đích**

Bảng `media_files` lưu metadata của tất cả file media trong hệ thống.

File thật được lưu ở object storage, ví dụ MinIO, Cloudflare R2 hoặc S3.

### **12.2. Loại media**

image  
panorama  
video  
audio  
model3d  
document  
thumbnail

### **12.3. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| owner\_type | varchar(50) | Không | Loại đối tượng sở hữu |
| owner\_id | uuid | Không | ID đối tượng sở hữu |
| file\_name | varchar(255) | Có | Tên file gốc |
| stored\_file\_name | varchar(255) | Có | Tên file trong storage |
| file\_url | text | Có | URL truy cập file |
| storage\_path | text | Có | Đường dẫn trong storage |
| storage\_provider | varchar(50) | Có | minio/s3/r2/local |
| media\_type | varchar(50) | Có | image/panorama/video/audio/model3d/... |
| mime\_type | varchar(100) | Có | MIME type |
| extension | varchar(20) | Có | Phần mở rộng |
| file\_size | bigint | Có | Dung lượng file |
| width | integer | Không | Chiều rộng ảnh/video |
| height | integer | Không | Chiều cao ảnh/video |
| duration | integer | Không | Thời lượng audio/video theo giây |
| thumbnail\_url | text | Không | URL thumbnail |
| alt\_text | varchar(255) | Không | Mô tả thay thế |
| caption | text | Không | Chú thích |
| metadata | jsonb | Không | Metadata mở rộng |
| uploaded\_by | uuid | Không | Người upload |
| created\_at | timestamp | Có | Thời gian upload |

### **12.4. Ràng buộc**

Primary key: id  
Foreign key: uploaded\_by references users(id), nullable

### **12.5. Ghi chú về `owner_type` và `owner_id`**

Hai trường này giúp media có thể gắn linh hoạt với nhiều đối tượng.

Ví dụ:

owner\_type \= destination  
owner\_id \= id của địa điểm

owner\_type \= panorama  
owner\_id \= id của panorama

owner\_type \= hotspot  
owner\_id \= id của hotspot

Cách này linh hoạt nhưng cần backend kiểm soát logic.

---

## **13\. Bảng `virtual_tours`**

### **13.1. Mục đích**

Bảng `virtual_tours` lưu tour 360 của một địa điểm.

Một địa điểm có thể có một hoặc nhiều tour 360\. Trong MVP, mỗi địa điểm thường chỉ cần một tour.

### **13.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| destination\_id | uuid | Có | Địa điểm sở hữu tour |
| title | varchar(250) | Có | Tên tour |
| description | text | Không | Mô tả tour |
| thumbnail\_id | uuid | Không | Ảnh đại diện tour |
| default\_panorama\_id | uuid | Không | Panorama mở đầu |
| status | varchar(30) | Có | draft/published/archived |
| created\_by | uuid | Không | Người tạo |
| updated\_by | uuid | Không | Người cập nhật |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

### **13.3. Ràng buộc**

Primary key: id  
Foreign key: destination\_id references destinations(id)  
Foreign key: thumbnail\_id references media\_files(id), nullable  
Foreign key: default\_panorama\_id references panoramas(id), nullable

### **13.4. Ghi chú**

Có vòng tham chiếu logic giữa `virtual_tours` và `panoramas` vì `virtual_tours.default_panorama_id` trỏ đến một panorama thuộc tour đó.

Khi triển khai migration, có thể:

Tạo virtual\_tours trước, chưa thêm foreign key default\_panorama\_id  
Tạo panoramas  
Sau đó thêm foreign key default\_panorama\_id

Hoặc backend kiểm tra logic mà không cần FK cứng cho `default_panorama_id`.

---

## **14\. Bảng `panoramas`**

### **14.1. Mục đích**

Bảng `panoramas` lưu từng điểm nhìn 360 trong một tour.

Ví dụ trong một ngôi chùa:

Cổng chính  
Sân chùa  
Chánh điện  
Khu tháp  
Khu vườn

Mỗi panorama tương ứng với một ảnh 360\.

### **14.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| virtual\_tour\_id | uuid | Có | Tour chứa panorama |
| title | varchar(250) | Có | Tên panorama |
| description | text | Không | Mô tả điểm nhìn |
| panorama\_image\_id | uuid | Có | File ảnh 360 |
| thumbnail\_id | uuid | Không | Thumbnail |
| display\_order | integer | Không | Thứ tự hiển thị |
| initial\_yaw | decimal(8,3) | Không | Góc nhìn ngang ban đầu |
| initial\_pitch | decimal(8,3) | Không | Góc nhìn dọc ban đầu |
| initial\_fov | decimal(8,3) | Không | Góc nhìn rộng ban đầu |
| status | varchar(30) | Có | draft/published/archived |
| created\_by | uuid | Không | Người tạo |
| updated\_by | uuid | Không | Người cập nhật |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

### **14.3. Ràng buộc**

Primary key: id  
Foreign key: virtual\_tour\_id references virtual\_tours(id)  
Foreign key: panorama\_image\_id references media\_files(id)  
Foreign key: thumbnail\_id references media\_files(id), nullable

### **14.4. Ghi chú**

`initial_yaw`, `initial_pitch`, `initial_fov` giúp xác định góc nhìn mặc định khi mở panorama.

Nếu chưa cấu hình, frontend có thể dùng giá trị mặc định.

---

## **15\. Bảng `hotspots`**

### **15.1. Mục đích**

Bảng `hotspots` lưu các điểm tương tác trên panorama.

Hotspot có thể là:

info  
navigation  
audio  
video  
image  
model3d  
external\_link

Hotspot loại `navigation` là thành phần quan trọng để tạo mũi tên di chuyển giữa các panorama.

### **15.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| panorama\_id | uuid | Có | Panorama chứa hotspot |
| type | varchar(50) | Có | Loại hotspot |
| title | varchar(250) | Có | Tiêu đề hotspot |
| description | text | Không | Nội dung mô tả |
| yaw | decimal(8,3) | Có | Vị trí ngang trong ảnh 360 |
| pitch | decimal(8,3) | Có | Vị trí dọc trong ảnh 360 |
| target\_panorama\_id | uuid | Không | Panorama đích nếu là navigation |
| media\_id | uuid | Không | File media nếu hotspot dùng media |
| external\_url | text | Không | Link ngoài nếu có |
| icon | varchar(100) | Không | Icon hiển thị |
| display\_order | integer | Không | Thứ tự hiển thị |
| status | varchar(30) | Có | draft/published/archived |
| created\_by | uuid | Không | Người tạo |
| updated\_by | uuid | Không | Người cập nhật |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

### **15.3. Ràng buộc**

Primary key: id  
Foreign key: panorama\_id references panoramas(id)  
Foreign key: target\_panorama\_id references panoramas(id), nullable  
Foreign key: media\_id references media\_files(id), nullable

### **15.4. Quy tắc nghiệp vụ**

Nếu `type = navigation`:

target\_panorama\_id bắt buộc có  
media\_id có thể null

Nếu `type = info`:

title bắt buộc  
description nên có

Nếu `type = audio`, `video`, `image`, `model3d`:

media\_id bắt buộc có

Nếu `type = external_link`:

external\_url bắt buộc có

### **15.5. Ví dụ navigation hotspot**

panorama\_id: P1 \- Cổng chính  
type: navigation  
title: Đi vào sân chùa  
yaw: 120  
pitch: \-5  
target\_panorama\_id: P2 \- Sân chùa

Khi người dùng bấm hotspot này, frontend chuyển từ panorama P1 sang panorama P2.

---

## **16\. Bảng `audio_guides`**

### **16.1. Mục đích**

Bảng `audio_guides` lưu thông tin audio thuyết minh.

Audio có thể gắn với:

destination  
virtual\_tour  
panorama  
hotspot

### **16.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| title | varchar(250) | Có | Tiêu đề audio |
| transcript | text | Không | Nội dung lời thuyết minh |
| media\_id | uuid | Có | File audio |
| language\_code | varchar(10) | Có | vi/en/km... |
| target\_type | varchar(50) | Có | destination/tour/panorama/hotspot |
| target\_id | uuid | Có | ID đối tượng được gắn audio |
| duration | integer | Không | Thời lượng giây |
| status | varchar(30) | Có | draft/published/archived |
| created\_by | uuid | Không | Người tạo |
| updated\_by | uuid | Không | Người cập nhật |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

### **16.3. Ràng buộc**

Primary key: id  
Foreign key: media\_id references media\_files(id)

### **16.4. Ghi chú**

`target_type` và `target_id` là liên kết đa hình. Backend cần kiểm tra target có tồn tại hay không.

---

# **PHẦN B: THIẾT KẾ BẢNG MỞ RỘNG**

---

## **17\. Bảng `translations`**

### **17.1. Mục đích**

Bảng `translations` hỗ trợ đa ngôn ngữ.

MVP dùng tiếng Việt trước, nhưng có thể chuẩn bị bảng này để mở rộng tiếng Anh, tiếng Khmer hoặc ngôn ngữ khác.

### **17.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| entity\_type | varchar(100) | Có | Loại đối tượng |
| entity\_id | uuid | Có | ID đối tượng |
| field\_name | varchar(100) | Có | Tên trường được dịch |
| language\_code | varchar(10) | Có | vi/en/km... |
| translated\_value | text | Có | Nội dung dịch |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

### **17.3. Ràng buộc**

Primary key: id  
Unique: entity\_type, entity\_id, field\_name, language\_code

### **17.4. Ví dụ**

entity\_type: destination  
entity\_id: destination-001  
field\_name: full\_description  
language\_code: en  
translated\_value: English description...

---

## **18\. Bảng `videos`**

### **18.1. Mục đích**

Bảng `videos` quản lý video giới thiệu hoặc video thuyết minh.

Video cũng có thể được quản lý trực tiếp bằng `media_files`. Tuy nhiên, bảng `videos` hữu ích nếu cần metadata riêng như title, description, thumbnail và target.

### **18.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| title | varchar(250) | Có | Tiêu đề video |
| description | text | Không | Mô tả |
| media\_id | uuid | Có | File video |
| thumbnail\_id | uuid | Không | Thumbnail |
| target\_type | varchar(50) | Có | destination/tour/panorama/hotspot |
| target\_id | uuid | Có | ID đối tượng |
| duration | integer | Không | Thời lượng |
| status | varchar(30) | Có | draft/published/archived |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

---

## **19\. Bảng `models_3d`**

### **19.1. Mục đích**

Bảng `models_3d` lưu metadata mô hình 3D. Trong MVP, chưa cần viewer 3D hoàn chỉnh nhưng nên thiết kế sẵn.

### **19.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| title | varchar(250) | Có | Tên model |
| description | text | Không | Mô tả |
| media\_id | uuid | Có | File .glb/.gltf |
| thumbnail\_id | uuid | Không | Ảnh đại diện |
| target\_type | varchar(50) | Có | destination/panorama/hotspot |
| target\_id | uuid | Có | ID đối tượng |
| format | varchar(20) | Không | glb/gltf |
| polygon\_count | integer | Không | Số polygon nếu có |
| status | varchar(30) | Có | draft/published/archived |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

---

## **20\. Bảng `routes`**

### **20.1. Mục đích**

Bảng `routes` lưu tuyến tham quan gợi ý.

Ví dụ:

Tuyến khám phá văn hóa  
Tuyến du lịch sinh thái ven sông  
Tuyến làng nghề và ẩm thực

### **20.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| region\_id | uuid | Có | Khu vực |
| title | varchar(250) | Có | Tên tuyến |
| slug | varchar(300) | Có | Slug |
| description | text | Không | Mô tả |
| estimated\_duration | varchar(100) | Không | Thời lượng dự kiến |
| distance\_km | decimal(8,2) | Không | Khoảng cách |
| theme | varchar(100) | Không | Chủ đề |
| thumbnail\_id | uuid | Không | Ảnh đại diện |
| status | varchar(30) | Có | draft/published/archived |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

---

## **21\. Bảng `route_destinations`**

### **21.1. Mục đích**

Bảng `route_destinations` lưu danh sách địa điểm thuộc một tuyến tham quan.

### **21.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| route\_id | uuid | Có | Tuyến tham quan |
| destination\_id | uuid | Có | Địa điểm |
| display\_order | integer | Có | Thứ tự trong tuyến |
| estimated\_stay\_time | varchar(100) | Không | Thời gian dừng |
| note | text | Không | Ghi chú |

### **21.3. Ràng buộc**

Foreign key: route\_id references routes(id)  
Foreign key: destination\_id references destinations(id)  
Unique: route\_id, destination\_id

---

## **22\. Bảng `analytics_events`**

### **22.1. Mục đích**

Bảng `analytics_events` lưu hành vi người dùng để phân tích sau này.

### **22.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| event\_name | varchar(100) | Có | Tên sự kiện |
| target\_type | varchar(50) | Không | Loại đối tượng |
| target\_id | uuid | Không | ID đối tượng |
| session\_id | varchar(150) | Không | Phiên truy cập |
| user\_id | uuid | Không | Người dùng nếu có |
| metadata | jsonb | Không | Dữ liệu phụ |
| created\_at | timestamp | Có | Thời gian xảy ra sự kiện |

### **22.3. Ví dụ event**

view\_destination  
open\_virtual\_tour  
click\_hotspot  
navigate\_panorama  
play\_audio  
play\_video  
click\_map\_marker

---

# **PHẦN C: ENUM VÀ QUY ƯỚC DỮ LIỆU**

---

## **23\. Enum đề xuất**

### **23.1. `content_status`**

draft  
published  
archived

Áp dụng cho:

regions  
destinations  
virtual\_tours  
panoramas  
hotspots  
audio\_guides  
videos  
models\_3d  
routes

### **23.2. `user_status`**

active  
inactive  
locked

### **23.3. `category_status`**

active  
inactive

### **23.4. `media_type`**

image  
panorama  
video  
audio  
model3d  
document  
thumbnail

### **23.5. `storage_provider`**

local  
minio  
s3  
r2

### **23.6. `hotspot_type`**

info  
navigation  
audio  
video  
image  
model3d  
external\_link

### **23.7. `audio_target_type`**

destination  
tour  
panorama  
hotspot

---

## **24\. Index đề xuất**

### **24.1. Index cho users**

users.email unique index  
users.role\_id index  
users.status index

### **24.2. Index cho regions**

regions.slug unique index  
regions.status index

### **24.3. Index cho categories**

categories.slug unique index  
categories.status index  
categories.display\_order index

### **24.4. Index cho destinations**

destinations.region\_id index  
destinations.category\_id index  
destinations.slug index  
destinations.status index  
destinations.location GiST index  
destinations.latitude, destinations.longitude index

PostGIS index:

CREATE INDEX idx\_destinations\_location  
ON destinations  
USING GIST (location);

### **24.5. Index cho media\_files**

media\_files.owner\_type, owner\_id index  
media\_files.media\_type index  
media\_files.uploaded\_by index

### **24.6. Index cho tour 360**

virtual\_tours.destination\_id index  
virtual\_tours.status index

panoramas.virtual\_tour\_id index  
panoramas.display\_order index  
panoramas.status index

hotspots.panorama\_id index  
hotspots.target\_panorama\_id index  
hotspots.type index  
hotspots.status index

### **24.7. Index cho translations**

translations.entity\_type, entity\_id index  
translations.language\_code index  
translations.entity\_type, entity\_id, field\_name, language\_code unique index

### **24.8. Index cho analytics**

analytics\_events.event\_name index  
analytics\_events.target\_type, target\_id index  
analytics\_events.created\_at index

---

# **PHẦN D: LUỒNG DỮ LIỆU QUAN TRỌNG**

---

## **25\. Luồng admin tạo địa điểm trên bản đồ**

Khi admin tạo một địa điểm mới:

Admin mở form tạo địa điểm  
→ Click lên bản đồ  
→ Frontend lấy latitude/longitude  
→ Admin nhập thông tin địa điểm  
→ Admin chọn category  
→ Admin upload cover image  
→ Backend lưu destination  
→ Backend tạo location geometry từ longitude/latitude  
→ Địa điểm được lưu ở trạng thái draft hoặc published

Dữ liệu liên quan:

regions  
categories  
destinations  
media\_files

---

## **26\. Luồng admin tạo tour 360 nhiều panorama**

Admin chọn một destination  
→ Tạo virtual\_tour  
→ Upload nhiều ảnh 360  
→ Mỗi ảnh 360 được lưu vào media\_files với media\_type \= panorama  
→ Tạo record panorama cho từng ảnh  
→ Chọn default\_panorama  
→ Mở panorama editor  
→ Đặt hotspot info/navigation/audio/video  
→ Lưu hotspot  
→ Xuất bản tour

Dữ liệu liên quan:

destinations  
virtual\_tours  
media\_files  
panoramas  
hotspots  
audio\_guides

---

## **27\. Luồng người dùng di chuyển giữa các panorama**

Ví dụ:

User đang ở Panorama A  
→ Bấm hotspot type \= navigation  
→ Hotspot có target\_panorama\_id \= Panorama B  
→ Frontend tìm Panorama B trong dữ liệu tour  
→ Frontend load ảnh 360 của Panorama B  
→ Frontend render hotspot của Panorama B

Database cần đảm bảo:

hotspots.panorama\_id \= panorama hiện tại  
hotspots.target\_panorama\_id \= panorama đích

Đây là phần giúp hệ thống có trải nghiệm giống tour 360 chuyên nghiệp.

---

## **28\. Luồng lấy dữ liệu tour 360 cho frontend**

Backend nên trả về dữ liệu theo cấu trúc:

virtual\_tour  
  panoramas\[\]  
    hotspots\[\]

Query logic:

Lấy virtual\_tour theo destination\_id  
→ Lấy danh sách panoramas thuộc tour  
→ Lấy media file của từng panorama  
→ Lấy hotspots của từng panorama  
→ Lấy media file của hotspot nếu có  
→ Trả về dữ liệu dạng cây cho frontend

Ví dụ dữ liệu:

{  
  "id": "tour-001",  
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
        }  
      \]  
    }  
  \]  
}

---

# **PHẦN E: DỮ LIỆU MẪU MVP**

---

## **29\. Dữ liệu seed đề xuất**

### **29.1. Roles**

Super Admin  
Admin  
Content Manager

### **29.2. Region**

Cần Thơ Demo

### **29.3. Categories**

Văn hóa \- lịch sử  
Tâm linh  
Sinh thái  
Nông nghiệp  
Làng nghề  
Ẩm thực  
Lưu trú

### **29.4. Destinations mẫu**

Nhà cổ Nam Bộ Demo  
Chùa ven sông Demo  
Khu sinh thái Demo  
Vườn trái cây Demo  
Làng nghề đặc sản Demo  
Điểm ẩm thực địa phương Demo  
Homestay nhà vườn Demo

### **29.5. Tour mẫu cho một địa điểm**

Ví dụ destination:

Chùa ven sông Demo

Virtual tour:

Tham quan Chùa ven sông Demo

Panoramas:

P1: Cổng chính  
P2: Sân chùa  
P3: Chánh điện  
P4: Khu vườn

Hotspots:

P1 → P2: Đi vào sân chùa  
P2 → P1: Quay lại cổng chính  
P2 → P3: Vào chánh điện  
P2 → P4: Đi ra khu vườn  
P3 → P2: Quay lại sân chùa  
P4 → P2: Quay lại sân chùa

---

## **30\. Gợi ý DDL minh họa cho bảng cốt lõi**

Đây là DDL minh họa, không phải migration cuối cùng. Khi triển khai với Entity Framework Core, có thể tạo entity class và migration tương ứng.

CREATE EXTENSION IF NOT EXISTS postgis;

CREATE TABLE roles (  
    id UUID PRIMARY KEY,  
    name VARCHAR(100) NOT NULL,  
    code VARCHAR(50) NOT NULL UNIQUE,  
    description TEXT,  
    created\_at TIMESTAMP NOT NULL DEFAULT NOW(),  
    updated\_at TIMESTAMP NOT NULL DEFAULT NOW()  
);

CREATE TABLE users (  
    id UUID PRIMARY KEY,  
    role\_id UUID NOT NULL REFERENCES roles(id),  
    full\_name VARCHAR(150) NOT NULL,  
    email VARCHAR(255) NOT NULL UNIQUE,  
    password\_hash TEXT NOT NULL,  
    avatar\_id UUID,  
    status VARCHAR(30) NOT NULL DEFAULT 'active',  
    last\_login\_at TIMESTAMP,  
    created\_at TIMESTAMP NOT NULL DEFAULT NOW(),  
    updated\_at TIMESTAMP NOT NULL DEFAULT NOW()  
);

CREATE TABLE regions (  
    id UUID PRIMARY KEY,  
    name VARCHAR(200) NOT NULL,  
    slug VARCHAR(250) NOT NULL UNIQUE,  
    description TEXT,  
    province VARCHAR(150),  
    district VARCHAR(150),  
    ward VARCHAR(150),  
    center\_latitude DECIMAL(10,7),  
    center\_longitude DECIMAL(10,7),  
    default\_zoom INTEGER,  
    cover\_image\_id UUID,  
    status VARCHAR(30) NOT NULL DEFAULT 'draft',  
    created\_by UUID REFERENCES users(id),  
    updated\_by UUID REFERENCES users(id),  
    created\_at TIMESTAMP NOT NULL DEFAULT NOW(),  
    updated\_at TIMESTAMP NOT NULL DEFAULT NOW()  
);

CREATE TABLE categories (  
    id UUID PRIMARY KEY,  
    name VARCHAR(150) NOT NULL,  
    slug VARCHAR(180) NOT NULL UNIQUE,  
    description TEXT,  
    icon VARCHAR(100),  
    color VARCHAR(30),  
    display\_order INTEGER,  
    status VARCHAR(30) NOT NULL DEFAULT 'active',  
    created\_at TIMESTAMP NOT NULL DEFAULT NOW(),  
    updated\_at TIMESTAMP NOT NULL DEFAULT NOW()  
);

CREATE TABLE media\_files (  
    id UUID PRIMARY KEY,  
    owner\_type VARCHAR(50),  
    owner\_id UUID,  
    file\_name VARCHAR(255) NOT NULL,  
    stored\_file\_name VARCHAR(255) NOT NULL,  
    file\_url TEXT NOT NULL,  
    storage\_path TEXT NOT NULL,  
    storage\_provider VARCHAR(50) NOT NULL,  
    media\_type VARCHAR(50) NOT NULL,  
    mime\_type VARCHAR(100) NOT NULL,  
    extension VARCHAR(20) NOT NULL,  
    file\_size BIGINT NOT NULL,  
    width INTEGER,  
    height INTEGER,  
    duration INTEGER,  
    thumbnail\_url TEXT,  
    alt\_text VARCHAR(255),  
    caption TEXT,  
    metadata JSONB,  
    uploaded\_by UUID REFERENCES users(id),  
    created\_at TIMESTAMP NOT NULL DEFAULT NOW()  
);

CREATE TABLE destinations (  
    id UUID PRIMARY KEY,  
    region\_id UUID NOT NULL REFERENCES regions(id),  
    category\_id UUID NOT NULL REFERENCES categories(id),  
    name VARCHAR(250) NOT NULL,  
    slug VARCHAR(300) NOT NULL,  
    short\_description TEXT NOT NULL,  
    full\_description TEXT,  
    address VARCHAR(500),  
    latitude DECIMAL(10,7) NOT NULL,  
    longitude DECIMAL(10,7) NOT NULL,  
    location GEOMETRY(Point, 4326),  
    cover\_image\_id UUID REFERENCES media\_files(id),  
    opening\_hours JSONB,  
    ticket\_price VARCHAR(150),  
    contact\_phone VARCHAR(50),  
    contact\_email VARCHAR(255),  
    website\_url TEXT,  
    facebook\_url TEXT,  
    has\_virtual\_tour BOOLEAN NOT NULL DEFAULT FALSE,  
    has\_audio\_guide BOOLEAN NOT NULL DEFAULT FALSE,  
    has\_3d\_model BOOLEAN NOT NULL DEFAULT FALSE,  
    status VARCHAR(30) NOT NULL DEFAULT 'draft',  
    created\_by UUID REFERENCES users(id),  
    updated\_by UUID REFERENCES users(id),  
    created\_at TIMESTAMP NOT NULL DEFAULT NOW(),  
    updated\_at TIMESTAMP NOT NULL DEFAULT NOW(),  
    UNIQUE(region\_id, slug)  
);

CREATE TABLE virtual\_tours (  
    id UUID PRIMARY KEY,  
    destination\_id UUID NOT NULL REFERENCES destinations(id) ON DELETE CASCADE,  
    title VARCHAR(250) NOT NULL,  
    description TEXT,  
    thumbnail\_id UUID REFERENCES media\_files(id),  
    default\_panorama\_id UUID,  
    status VARCHAR(30) NOT NULL DEFAULT 'draft',  
    created\_by UUID REFERENCES users(id),  
    updated\_by UUID REFERENCES users(id),  
    created\_at TIMESTAMP NOT NULL DEFAULT NOW(),  
    updated\_at TIMESTAMP NOT NULL DEFAULT NOW()  
);

CREATE TABLE panoramas (  
    id UUID PRIMARY KEY,  
    virtual\_tour\_id UUID NOT NULL REFERENCES virtual\_tours(id) ON DELETE CASCADE,  
    title VARCHAR(250) NOT NULL,  
    description TEXT,  
    panorama\_image\_id UUID NOT NULL REFERENCES media\_files(id),  
    thumbnail\_id UUID REFERENCES media\_files(id),  
    display\_order INTEGER,  
    initial\_yaw DECIMAL(8,3),  
    initial\_pitch DECIMAL(8,3),  
    initial\_fov DECIMAL(8,3),  
    status VARCHAR(30) NOT NULL DEFAULT 'draft',  
    created\_by UUID REFERENCES users(id),  
    updated\_by UUID REFERENCES users(id),  
    created\_at TIMESTAMP NOT NULL DEFAULT NOW(),  
    updated\_at TIMESTAMP NOT NULL DEFAULT NOW()  
);

ALTER TABLE virtual\_tours  
ADD CONSTRAINT fk\_virtual\_tours\_default\_panorama  
FOREIGN KEY (default\_panorama\_id)  
REFERENCES panoramas(id);

CREATE TABLE hotspots (  
    id UUID PRIMARY KEY,  
    panorama\_id UUID NOT NULL REFERENCES panoramas(id) ON DELETE CASCADE,  
    type VARCHAR(50) NOT NULL,  
    title VARCHAR(250) NOT NULL,  
    description TEXT,  
    yaw DECIMAL(8,3) NOT NULL,  
    pitch DECIMAL(8,3) NOT NULL,  
    target\_panorama\_id UUID REFERENCES panoramas(id),  
    media\_id UUID REFERENCES media\_files(id),  
    external\_url TEXT,  
    icon VARCHAR(100),  
    display\_order INTEGER,  
    status VARCHAR(30) NOT NULL DEFAULT 'draft',  
    created\_by UUID REFERENCES users(id),  
    updated\_by UUID REFERENCES users(id),  
    created\_at TIMESTAMP NOT NULL DEFAULT NOW(),  
    updated\_at TIMESTAMP NOT NULL DEFAULT NOW()  
);

CREATE TABLE audio\_guides (  
    id UUID PRIMARY KEY,  
    title VARCHAR(250) NOT NULL,  
    transcript TEXT,  
    media\_id UUID NOT NULL REFERENCES media\_files(id),  
    language\_code VARCHAR(10) NOT NULL DEFAULT 'vi',  
    target\_type VARCHAR(50) NOT NULL,  
    target\_id UUID NOT NULL,  
    duration INTEGER,  
    status VARCHAR(30) NOT NULL DEFAULT 'draft',  
    created\_by UUID REFERENCES users(id),  
    updated\_by UUID REFERENCES users(id),  
    created\_at TIMESTAMP NOT NULL DEFAULT NOW(),  
    updated\_at TIMESTAMP NOT NULL DEFAULT NOW()  
);

---

## **31\. Gợi ý index DDL**

CREATE INDEX idx\_destinations\_region\_id ON destinations(region\_id);  
CREATE INDEX idx\_destinations\_category\_id ON destinations(category\_id);  
CREATE INDEX idx\_destinations\_status ON destinations(status);  
CREATE INDEX idx\_destinations\_location ON destinations USING GIST(location);

CREATE INDEX idx\_media\_files\_owner ON media\_files(owner\_type, owner\_id);  
CREATE INDEX idx\_media\_files\_media\_type ON media\_files(media\_type);

CREATE INDEX idx\_virtual\_tours\_destination\_id ON virtual\_tours(destination\_id);

CREATE INDEX idx\_panoramas\_virtual\_tour\_id ON panoramas(virtual\_tour\_id);

CREATE INDEX idx\_hotspots\_panorama\_id ON hotspots(panorama\_id);  
CREATE INDEX idx\_hotspots\_target\_panorama\_id ON hotspots(target\_panorama\_id);  
CREATE INDEX idx\_hotspots\_type ON hotspots(type);

---

## **32\. Lưu ý khi triển khai bằng Entity Framework Core**

Nếu dùng ASP.NET Core \+ EF Core, cần lưu ý:

1. Cài package hỗ trợ PostgreSQL:

Npgsql.EntityFrameworkCore.PostgreSQL

2. Nếu dùng PostGIS, cần thêm:

NetTopologySuite

3. Cấu hình DbContext:

UseNpgsql(connectionString, o \=\> o.UseNetTopologySuite())

4. Entity `Destination` có thể có property:

Point Location

5. Khi lưu location:

new Point(longitude, latitude) { SRID \= 4326 }

6. Nếu thấy PostGIS phức tạp ở MVP, có thể bắt đầu bằng `latitude` và `longitude`, sau đó thêm `location` ở giai đoạn sau.

---

## **33\. Những điểm cần tránh**

### **33.1. Không lưu file trực tiếp trong database**

Không nên lưu ảnh/video/audio dưới dạng binary trong PostgreSQL.

Nên lưu file trong object storage, database chỉ lưu metadata.

### **33.2. Không thiết kế tour 360 chỉ có một ảnh**

Nếu chỉ lưu một ảnh 360 cho mỗi địa điểm, sau này rất khó mở rộng thành tour nhiều điểm nhìn. Vì vậy cần có:

virtual\_tours  
panoramas  
hotspots  
target\_panorama\_id

### **33.3. Không hard-code danh mục**

Danh mục nên lưu trong bảng `categories`, không hard-code trong frontend.

### **33.4. Không hard-code địa phương**

Khu vực triển khai nên lưu trong bảng `regions`.

### **33.5. Không bỏ qua trạng thái dữ liệu**

Các bảng nội dung nên có `status` để phân biệt dữ liệu nháp và dữ liệu đã xuất bản.

---

## **34\. Kết luận**

Thiết kế database của **Smart Tourism 360 Platform** tập trung vào việc quản lý dữ liệu du lịch số theo cách có cấu trúc, linh hoạt và dễ mở rộng. Các bảng cốt lõi như `regions`, `categories`, `destinations`, `media_files`, `virtual_tours`, `panoramas` và `hotspots` là nền tảng để triển khai MVP.

Điểm quan trọng nhất trong thiết kế này là mô hình **multi-scene 360 virtual tour**. Một địa điểm có thể có một tour 360, một tour có nhiều panorama, mỗi panorama có nhiều hotspot. Hotspot loại `navigation` dùng `target_panorama_id` để tạo các nút mũi tên di chuyển giữa các điểm nhìn, giúp người dùng có trải nghiệm tham quan ảo giống các nền tảng 360 chuyên nghiệp.

Database cũng được thiết kế sẵn để mở rộng sang đa ngôn ngữ, mô hình 3D, tuyến tham quan, analytics và WebGIS 3D trong tương lai. Với thiết kế này, nhóm phát triển có thể tiếp tục xây dựng API, frontend, admin dashboard và hệ thống lưu trữ media một cách nhất quán.

