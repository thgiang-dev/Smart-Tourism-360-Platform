# **API SPECIFICATION DOCUMENT**

# **Smart Tourism 360 Platform**

## **Đặc tả API cho nền tảng du lịch số tích hợp bản đồ tương tác và tham quan ảo 360 độ**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | API Specification Document |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v1.0 |
| Mục đích | Đặc tả các API phục vụ frontend du khách, admin dashboard, bản đồ tương tác, quản lý media và tour 360 |
| Backend đề xuất | ASP.NET Core Web API |
| Chuẩn API | RESTful API |
| Định dạng dữ liệu | JSON |
| Xác thực | JWT Authentication |
| Tài liệu API | Swagger/OpenAPI |

---

## **2\. Mục tiêu của tài liệu API**

Tài liệu này mô tả các API cần thiết để triển khai hệ thống **Smart Tourism 360 Platform**. API là lớp giao tiếp giữa frontend và backend, chịu trách nhiệm cung cấp dữ liệu cho giao diện du khách, admin dashboard, bản đồ tương tác và trình xem tour 360\.

Tài liệu này giúp nhóm phát triển trả lời các câu hỏi:

1. Frontend cần gọi API nào để hiển thị bản đồ và địa điểm?  
2. Admin cần gọi API nào để tạo địa điểm, chọn vị trí, upload media?  
3. Tour 360 nhiều panorama được trả về cho frontend theo cấu trúc nào?  
4. Hotspot điều hướng giữa các panorama được tạo, sửa và đọc như thế nào?  
5. API nào cần xác thực admin?  
6. Response và error format nên chuẩn hóa ra sao?  
7. API nào thuộc MVP, API nào để mở rộng sau?

---

## **3\. Phạm vi API trong MVP**

Trong MVP, hệ thống cần triển khai các nhóm API chính sau:

Auth API  
Region API  
Category API  
Destination API  
Media API  
Virtual Tour API  
Panorama API  
Hotspot API  
Audio Guide API  
Public Tourist API

Các nhóm API có thể thiết kế sẵn hoặc phát triển sau MVP:

Translation API  
Route API  
Analytics API  
Model 3D API  
Tourist Account API  
Review API  
Booking API

---

## **4\. Quy ước chung**

### **4.1. Base URL**

Trong môi trường local:

http://localhost:5000/api

Trong môi trường production:

https://api.smarttourism360.example.com/api

### **4.2. Định dạng dữ liệu**

Tất cả request và response mặc định sử dụng:

Content-Type: application/json

Riêng upload file sử dụng:

Content-Type: multipart/form-data

### **4.3. Phiên bản API**

Có thể dùng versioning:

/api/v1/...

Trong MVP có thể dùng trực tiếp:

/api/...

Khuyến nghị dài hạn:

/api/v1/destinations  
/api/v1/admin/destinations

### **4.4. Phân loại endpoint**

API nên chia thành hai nhóm:

Public API:  
Dành cho du khách, không cần đăng nhập.

Admin API:  
Dành cho quản trị viên, bắt buộc JWT token.

Ví dụ:

GET  /api/destinations  
GET  /api/destinations/{id}  
GET  /api/destinations/{id}/virtual-tour

POST /api/admin/destinations  
PUT  /api/admin/destinations/{id}  
DELETE /api/admin/destinations/{id}

---

## **5\. Chuẩn response chung**

### **5.1. Response thành công dạng đơn**

{  
  "success": true,  
  "message": "Request completed successfully.",  
  "data": {  
    "id": "uuid",  
    "name": "Destination name"  
  }  
}

### **5.2. Response thành công dạng danh sách**

{  
  "success": true,  
  "message": "Data retrieved successfully.",  
  "data": {  
    "items": \[\],  
    "page": 1,  
    "pageSize": 10,  
    "totalItems": 100,  
    "totalPages": 10  
  }  
}

### **5.3. Response lỗi**

{  
  "success": false,  
  "message": "Validation failed.",  
  "errors": \[  
    {  
      "field": "name",  
      "message": "Name is required."  
    }  
  \]  
}

### **5.4. HTTP status code sử dụng**

| Status code | Ý nghĩa |
| ----- | ----- |
| 200 | Thành công |
| 201 | Tạo mới thành công |
| 204 | Xóa thành công, không trả body |
| 400 | Request không hợp lệ |
| 401 | Chưa xác thực |
| 403 | Không có quyền |
| 404 | Không tìm thấy dữ liệu |
| 409 | Xung đột dữ liệu |
| 422 | Lỗi validation |
| 500 | Lỗi hệ thống |

---

## **6\. Xác thực và phân quyền**

### **6.1. Cơ chế xác thực**

Admin đăng nhập bằng email và mật khẩu. Backend xác thực tài khoản, sau đó cấp JWT access token.

Các API admin cần gửi token trong header:

Authorization: Bearer {access\_token}

### **6.2. Vai trò người dùng**

| Vai trò | Quyền |
| ----- | ----- |
| Super Admin | Quản lý toàn bộ hệ thống |
| Admin | Quản lý địa điểm, media, tour 360, hotspot |
| Content Manager | Quản lý nội dung, có thể bị giới hạn quyền |
| Tourist | Người dùng công khai, chưa cần đăng nhập trong MVP |

---

# **PHẦN A: AUTH API**

---

## **7\. Auth API**

### **7.1. Đăng nhập admin**

POST /api/auth/login

#### **Request body**

{  
  "email": "admin@example.com",  
  "password": "Admin@123"  
}

#### **Response**

{  
  "success": true,  
  "message": "Login successfully.",  
  "data": {  
    "accessToken": "jwt-token",  
    "expiresIn": 3600,  
    "user": {  
      "id": "user-001",  
      "fullName": "System Admin",  
      "email": "admin@example.com",  
      "role": "Admin"  
    }  
  }  
}

#### **Ghi chú**

API này dùng cho trang đăng nhập admin dashboard.

---

### **7.2. Lấy thông tin người dùng hiện tại**

GET /api/auth/me

Yêu cầu JWT token.

#### **Response**

{  
  "success": true,  
  "data": {  
    "id": "user-001",  
    "fullName": "System Admin",  
    "email": "admin@example.com",  
    "role": "Admin"  
  }  
}

---

### **7.3. Đăng xuất**

POST /api/auth/logout

Trong MVP, nếu dùng JWT stateless, logout có thể xử lý ở frontend bằng cách xóa token. API này có thể để mở rộng sau.

---

# **PHẦN B: PUBLIC API CHO DU KHÁCH**

---

## **8\. Public Region API**

### **8.1. Lấy thông tin khu vực đang hiển thị**

GET /api/regions/current

#### **Response**

{  
  "success": true,  
  "data": {  
    "id": "region-001",  
    "name": "Cần Thơ Demo",  
    "slug": "can-tho-demo",  
    "description": "Khu vực demo cho nền tảng du lịch số 360.",  
    "centerLatitude": 10.0452,  
    "centerLongitude": 105.7469,  
    "defaultZoom": 12,  
    "coverImageUrl": "https://storage.example.com/cover.jpg"  
  }  
}

---

## **9\. Public Category API**

### **9.1. Lấy danh sách danh mục**

GET /api/categories

#### **Query params**

| Tên | Bắt buộc | Mô tả |
| ----- | ----- | ----- |
| status | Không | active/inactive |

#### **Response**

{  
  "success": true,  
  "data": \[  
    {  
      "id": "cat-001",  
      "name": "Văn hóa \- lịch sử",  
      "slug": "van-hoa-lich-su",  
      "icon": "landmark",  
      "color": "\#C084FC"  
    },  
    {  
      "id": "cat-002",  
      "name": "Sinh thái",  
      "slug": "sinh-thai",  
      "icon": "leaf",  
      "color": "\#22C55E"  
    }  
  \]  
}

---

## **10\. Public Destination API**

### **10.1. Lấy danh sách địa điểm**

GET /api/destinations

#### **Query params**

| Tên | Bắt buộc | Mô tả |
| ----- | ----- | ----- |
| regionId | Không | Lọc theo khu vực |
| categoryId | Không | Lọc theo danh mục |
| keyword | Không | Tìm theo tên/mô tả |
| hasVirtualTour | Không | true/false |
| page | Không | Trang hiện tại |
| pageSize | Không | Số item mỗi trang |

#### **Response**

{  
  "success": true,  
  "data": {  
    "items": \[  
      {  
        "id": "destination-001",  
        "name": "Chùa ven sông Demo",  
        "slug": "chua-ven-song-demo",  
        "shortDescription": "Điểm tham quan tâm linh ven sông với không gian yên bình.",  
        "category": {  
          "id": "cat-002",  
          "name": "Tâm linh",  
          "icon": "temple",  
          "color": "\#F59E0B"  
        },  
        "latitude": 10.0452,  
        "longitude": 105.7469,  
        "coverImageUrl": "https://storage.example.com/chua-cover.jpg",  
        "hasVirtualTour": true,  
        "hasAudioGuide": true  
      }  
    \],  
    "page": 1,  
    "pageSize": 10,  
    "totalItems": 1,  
    "totalPages": 1  
  }  
}

---

### **10.2. Lấy danh sách địa điểm cho bản đồ**

GET /api/destinations/map

API này trả về dữ liệu gọn để hiển thị marker trên bản đồ.

#### **Query params**

| Tên | Bắt buộc | Mô tả |
| ----- | ----- | ----- |
| regionId | Không | Lọc theo khu vực |
| categoryId | Không | Lọc theo danh mục |
| keyword | Không | Tìm kiếm |
| hasVirtualTour | Không | true/false |

#### **Response**

{  
  "success": true,  
  "data": \[  
    {  
      "id": "destination-001",  
      "name": "Chùa ven sông Demo",  
      "slug": "chua-ven-song-demo",  
      "latitude": 10.0452,  
      "longitude": 105.7469,  
      "category": {  
        "id": "cat-002",  
        "name": "Tâm linh",  
        "icon": "temple",  
        "color": "\#F59E0B"  
      },  
      "coverImageUrl": "https://storage.example.com/chua-cover.jpg",  
      "hasVirtualTour": true  
    }  
  \]  
}

#### **Ghi chú**

API này phục vụ màn hình bản đồ du khách. Dữ liệu nên nhẹ, không trả mô tả dài hoặc toàn bộ media.

---

### **10.3. Lấy chi tiết địa điểm**

GET /api/destinations/{id}

#### **Response**

{  
  "success": true,  
  "data": {  
    "id": "destination-001",  
    "regionId": "region-001",  
    "name": "Chùa ven sông Demo",  
    "slug": "chua-ven-song-demo",  
    "shortDescription": "Điểm tham quan tâm linh ven sông với không gian yên bình.",  
    "fullDescription": "Nội dung mô tả chi tiết về địa điểm...",  
    "address": "Cần Thơ Demo",  
    "latitude": 10.0452,  
    "longitude": 105.7469,  
    "category": {  
      "id": "cat-002",  
      "name": "Tâm linh"  
    },  
    "coverImageUrl": "https://storage.example.com/chua-cover.jpg",  
    "gallery": \[  
      {  
        "id": "media-001",  
        "url": "https://storage.example.com/gallery-1.jpg",  
        "caption": "Ảnh minh họa"  
      }  
    \],  
    "videos": \[\],  
    "audioGuides": \[\],  
    "hasVirtualTour": true,  
    "virtualTourId": "tour-001",  
    "openingHours": {  
      "monday": "07:00-17:00"  
    },  
    "ticketPrice": "Miễn phí",  
    "contactPhone": null  
  }  
}

---

# **PHẦN C: PUBLIC VIRTUAL TOUR API**

---

## **11\. Public Virtual Tour API**

### **11.1. Lấy tour 360 của một địa điểm**

GET /api/destinations/{destinationId}/virtual-tour

#### **Mục đích**

API này trả về toàn bộ dữ liệu cần thiết để frontend hiển thị tour 360 nhiều panorama.

Dữ liệu gồm:

Thông tin tour  
Panorama mặc định  
Danh sách panorama  
Ảnh 360 của từng panorama  
Hotspot của từng panorama  
Target panorama cho hotspot navigation  
Media liên quan nếu hotspot là audio/video/image

#### **Response**

{  
  "success": true,  
  "data": {  
    "id": "tour-001",  
    "destinationId": "destination-001",  
    "title": "Tham quan Chùa ven sông Demo",  
    "description": "Tour 360 mô phỏng không gian tham quan của chùa.",  
    "defaultPanoramaId": "pano-001",  
    "panoramas": \[  
      {  
        "id": "pano-001",  
        "title": "Cổng chính",  
        "description": "Vị trí cổng vào của địa điểm.",  
        "imageUrl": "https://storage.example.com/panoramas/cong-chinh.jpg",  
        "thumbnailUrl": "https://storage.example.com/thumbnails/cong-chinh.webp",  
        "initialYaw": 0,  
        "initialPitch": 0,  
        "initialFov": 75,  
        "hotspots": \[  
          {  
            "id": "hotspot-001",  
            "type": "navigation",  
            "title": "Đi vào sân chính",  
            "description": null,  
            "yaw": 120,  
            "pitch": \-8,  
            "targetPanoramaId": "pano-002",  
            "media": null,  
            "icon": "arrow-forward"  
          },  
          {  
            "id": "hotspot-002",  
            "type": "info",  
            "title": "Cổng chính",  
            "description": "Thông tin giới thiệu về cổng chính.",  
            "yaw": 30,  
            "pitch": 5,  
            "targetPanoramaId": null,  
            "media": null,  
            "icon": "info"  
          }  
        \]  
      },  
      {  
        "id": "pano-002",  
        "title": "Sân chính",  
        "description": "Không gian sân chính của địa điểm.",  
        "imageUrl": "https://storage.example.com/panoramas/san-chinh.jpg",  
        "thumbnailUrl": "https://storage.example.com/thumbnails/san-chinh.webp",  
        "initialYaw": 0,  
        "initialPitch": 0,  
        "initialFov": 75,  
        "hotspots": \[  
          {  
            "id": "hotspot-003",  
            "type": "navigation",  
            "title": "Quay lại cổng chính",  
            "yaw": \-80,  
            "pitch": \-6,  
            "targetPanoramaId": "pano-001",  
            "icon": "arrow-back"  
          }  
        \]  
      }  
    \]  
  }  
}

#### **Ghi chú quan trọng**

Đây là API cốt lõi của trải nghiệm tour 360\.

Frontend sẽ dùng `defaultPanoramaId` để biết panorama nào mở đầu.  
Khi người dùng bấm hotspot có `type = navigation`, frontend đọc `targetPanoramaId` để chuyển sang panorama khác.

---

### **11.2. Lấy một panorama cụ thể**

GET /api/virtual-tours/{tourId}/panoramas/{panoramaId}

#### **Mục đích**

API này dùng nếu frontend muốn tải từng panorama riêng lẻ thay vì tải toàn bộ tour một lần.

#### **Response**

{  
  "success": true,  
  "data": {  
    "id": "pano-001",  
    "virtualTourId": "tour-001",  
    "title": "Cổng chính",  
    "imageUrl": "https://storage.example.com/panoramas/cong-chinh.jpg",  
    "initialYaw": 0,  
    "initialPitch": 0,  
    "initialFov": 75,  
    "hotspots": \[  
      {  
        "id": "hotspot-001",  
        "type": "navigation",  
        "title": "Đi vào sân chính",  
        "yaw": 120,  
        "pitch": \-8,  
        "targetPanoramaId": "pano-002"  
      }  
    \]  
  }  
}

#### **Ghi chú**

Với MVP nhỏ, có thể dùng API lấy toàn bộ tour.  
Với tour lớn nhiều ảnh 360, nên dùng lazy loading từng panorama.

---

# **PHẦN D: ADMIN REGION API**

---

## **12\. Admin Region API**

### **12.1. Lấy danh sách khu vực**

GET /api/admin/regions

Yêu cầu JWT.

#### **Response**

{  
  "success": true,  
  "data": \[  
    {  
      "id": "region-001",  
      "name": "Cần Thơ Demo",  
      "slug": "can-tho-demo",  
      "status": "published"  
    }  
  \]  
}

---

### **12.2. Tạo khu vực**

POST /api/admin/regions

#### **Request body**

{  
  "name": "Cần Thơ Demo",  
  "slug": "can-tho-demo",  
  "description": "Khu vực demo cho dự án.",  
  "province": "Cần Thơ",  
  "centerLatitude": 10.0452,  
  "centerLongitude": 105.7469,  
  "defaultZoom": 12,  
  "status": "published"  
}

---

### **12.3. Cập nhật khu vực**

PUT /api/admin/regions/{id}

---

### **12.4. Xóa hoặc lưu trữ khu vực**

DELETE /api/admin/regions/{id}

Khuyến nghị không xóa cứng nếu khu vực đã có dữ liệu. Nên chuyển trạng thái sang `archived`.

---

# **PHẦN E: ADMIN CATEGORY API**

---

## **13\. Admin Category API**

### **13.1. Lấy danh sách danh mục**

GET /api/admin/categories

---

### **13.2. Tạo danh mục**

POST /api/admin/categories

#### **Request body**

{  
  "name": "Sinh thái",  
  "slug": "sinh-thai",  
  "description": "Các điểm du lịch sinh thái, ven sông, vườn xanh.",  
  "icon": "leaf",  
  "color": "\#22C55E",  
  "displayOrder": 3,  
  "status": "active"  
}

---

### **13.3. Cập nhật danh mục**

PUT /api/admin/categories/{id}

---

### **13.4. Xóa danh mục**

DELETE /api/admin/categories/{id}

#### **Ghi chú**

Nếu danh mục đã được dùng bởi địa điểm, không nên xóa cứng. Có thể chuyển `status = inactive`.

---

# **PHẦN F: ADMIN DESTINATION API**

---

## **14\. Admin Destination API**

### **14.1. Lấy danh sách địa điểm trong admin**

GET /api/admin/destinations

#### **Query params**

| Tên | Bắt buộc | Mô tả |
| ----- | ----- | ----- |
| regionId | Không | Lọc theo khu vực |
| categoryId | Không | Lọc theo danh mục |
| keyword | Không | Tìm kiếm |
| status | Không | draft/published/archived |
| page | Không | Trang |
| pageSize | Không | Số item mỗi trang |

#### **Response**

{  
  "success": true,  
  "data": {  
    "items": \[  
      {  
        "id": "destination-001",  
        "name": "Chùa ven sông Demo",  
        "categoryName": "Tâm linh",  
        "regionName": "Cần Thơ Demo",  
        "latitude": 10.0452,  
        "longitude": 105.7469,  
        "hasVirtualTour": true,  
        "status": "published",  
        "updatedAt": "2026-06-05T10:00:00Z"  
      }  
    \],  
    "page": 1,  
    "pageSize": 10,  
    "totalItems": 1,  
    "totalPages": 1  
  }  
}

---

### **14.2. Lấy chi tiết địa điểm trong admin**

GET /api/admin/destinations/{id}

API này trả dữ liệu đầy đủ hơn public API, bao gồm trạng thái, metadata, thông tin người tạo/cập nhật.

---

### **14.3. Tạo địa điểm**

POST /api/admin/destinations

#### **Request body**

{  
  "regionId": "region-001",  
  "categoryId": "cat-002",  
  "name": "Chùa ven sông Demo",  
  "slug": "chua-ven-song-demo",  
  "shortDescription": "Điểm tham quan tâm linh ven sông với không gian yên bình.",  
  "fullDescription": "Nội dung mô tả chi tiết...",  
  "address": "Cần Thơ Demo",  
  "latitude": 10.0452,  
  "longitude": 105.7469,  
  "coverImageId": "media-001",  
  "openingHours": {  
    "monday": "07:00-17:00",  
    "sunday": "07:00-18:00"  
  },  
  "ticketPrice": "Miễn phí",  
  "contactPhone": null,  
  "contactEmail": null,  
  "websiteUrl": null,  
  "facebookUrl": null,  
  "status": "draft"  
}

#### **Response**

{  
  "success": true,  
  "message": "Destination created successfully.",  
  "data": {  
    "id": "destination-001",  
    "name": "Chùa ven sông Demo",  
    "latitude": 10.0452,  
    "longitude": 105.7469,  
    "status": "draft"  
  }  
}

#### **Ghi chú**

Khi nhận `latitude` và `longitude`, backend nên tự tạo trường PostGIS `location`.

---

### **14.4. Cập nhật địa điểm**

PUT /api/admin/destinations/{id}

Request body tương tự tạo mới.

---

### **14.5. Cập nhật tọa độ địa điểm**

PATCH /api/admin/destinations/{id}/location

API này phục vụ trường hợp admin kéo marker hoặc click lại vị trí mới trên bản đồ.

#### **Request body**

{  
  "latitude": 10.047,  
  "longitude": 105.748  
}

#### **Response**

{  
  "success": true,  
  "message": "Destination location updated successfully.",  
  "data": {  
    "id": "destination-001",  
    "latitude": 10.047,  
    "longitude": 105.748  
  }  
}

---

### **14.6. Cập nhật trạng thái địa điểm**

PATCH /api/admin/destinations/{id}/status

#### **Request body**

{  
  "status": "published"  
}

---

### **14.7. Xóa địa điểm**

DELETE /api/admin/destinations/{id}

Khuyến nghị trong MVP:

Không xóa cứng ngay.  
Chuyển status sang archived hoặc dùng soft delete.

---

# **PHẦN G: MEDIA API**

---

## **15\. Admin Media API**

### **15.1. Upload media**

POST /api/admin/media/upload

Yêu cầu JWT.  
Content-Type: `multipart/form-data`

#### **Form data**

| Field | Bắt buộc | Mô tả |
| ----- | ----- | ----- |
| file | Có | File upload |
| mediaType | Có | image/panorama/video/audio/model3d |
| ownerType | Không | destination/panorama/hotspot/tour |
| ownerId | Không | ID đối tượng sở hữu |
| altText | Không | Mô tả ảnh |
| caption | Không | Chú thích |

#### **Response**

{  
  "success": true,  
  "message": "File uploaded successfully.",  
  "data": {  
    "id": "media-001",  
    "fileName": "cong-chinh.jpg",  
    "fileUrl": "https://storage.example.com/can-tho-demo/chua-demo/panoramas/cong-chinh.jpg",  
    "storagePath": "can-tho-demo/chua-demo/panoramas/cong-chinh.jpg",  
    "mediaType": "panorama",  
    "mimeType": "image/jpeg",  
    "extension": ".jpg",  
    "fileSize": 5242880,  
    "width": 4096,  
    "height": 2048,  
    "duration": null,  
    "thumbnailUrl": "https://storage.example.com/thumbnails/cong-chinh.webp"  
  }  
}

---

### **15.2. Lấy danh sách media**

GET /api/admin/media

#### **Query params**

| Tên | Bắt buộc | Mô tả |
| ----- | ----- | ----- |
| mediaType | Không | image/panorama/video/audio/model3d |
| ownerType | Không | destination/panorama/hotspot |
| ownerId | Không | ID đối tượng |
| page | Không | Trang |
| pageSize | Không | Số item mỗi trang |

---

### **15.3. Lấy chi tiết media**

GET /api/admin/media/{id}

---

### **15.4. Cập nhật metadata media**

PUT /api/admin/media/{id}

#### **Request body**

{  
  "altText": "Ảnh cổng chính",  
  "caption": "Không gian cổng chính của địa điểm",  
  "ownerType": "destination",  
  "ownerId": "destination-001"  
}

---

### **15.5. Xóa media**

DELETE /api/admin/media/{id}

#### **Ghi chú**

Khi xóa media cần kiểm tra:

File có đang được dùng làm ảnh đại diện không?  
File có đang được dùng trong panorama không?  
File có đang được dùng trong hotspot không?  
File có đang được dùng trong audio/video/model không?

Nếu đang được dùng, nên chặn xóa hoặc yêu cầu admin gỡ liên kết trước.

---

# **PHẦN H: ADMIN VIRTUAL TOUR API**

---

## **16\. Admin Virtual Tour API**

### **16.1. Lấy danh sách tour của một địa điểm**

GET /api/admin/destinations/{destinationId}/virtual-tours

---

### **16.2. Tạo tour 360**

POST /api/admin/destinations/{destinationId}/virtual-tours

#### **Request body**

{  
  "title": "Tham quan Chùa ven sông Demo",  
  "description": "Tour 360 mô phỏng không gian tham quan.",  
  "thumbnailId": "media-001",  
  "status": "draft"  
}

#### **Response**

{  
  "success": true,  
  "message": "Virtual tour created successfully.",  
  "data": {  
    "id": "tour-001",  
    "destinationId": "destination-001",  
    "title": "Tham quan Chùa ven sông Demo",  
    "status": "draft"  
  }  
}

---

### **16.3. Lấy chi tiết tour trong admin**

GET /api/admin/virtual-tours/{tourId}

API này nên trả về cả danh sách panorama và hotspot để admin preview hoặc chỉnh sửa.

---

### **16.4. Cập nhật tour**

PUT /api/admin/virtual-tours/{tourId}

#### **Request body**

{  
  "title": "Tham quan Chùa ven sông Demo",  
  "description": "Mô tả đã cập nhật.",  
  "thumbnailId": "media-002",  
  "defaultPanoramaId": "pano-001",  
  "status": "published"  
}

---

### **16.5. Chọn panorama mặc định**

PATCH /api/admin/virtual-tours/{tourId}/default-panorama

#### **Request body**

{  
  "defaultPanoramaId": "pano-001"  
}

---

### **16.6. Xóa tour**

DELETE /api/admin/virtual-tours/{tourId}

#### **Ghi chú**

Nếu xóa tour, cần xác định chính sách:

Xóa cả panoramas và hotspots thuộc tour  
hoặc chuyển tour sang archived

Khuyến nghị MVP: chuyển sang `archived`.

---

# **PHẦN I: ADMIN PANORAMA API**

---

## **17\. Admin Panorama API**

### **17.1. Lấy danh sách panorama của tour**

GET /api/admin/virtual-tours/{tourId}/panoramas

#### **Response**

{  
  "success": true,  
  "data": \[  
    {  
      "id": "pano-001",  
      "title": "Cổng chính",  
      "imageUrl": "https://storage.example.com/panoramas/cong-chinh.jpg",  
      "thumbnailUrl": "https://storage.example.com/thumbnails/cong-chinh.webp",  
      "displayOrder": 1,  
      "status": "published"  
    },  
    {  
      "id": "pano-002",  
      "title": "Sân chính",  
      "imageUrl": "https://storage.example.com/panoramas/san-chinh.jpg",  
      "displayOrder": 2,  
      "status": "published"  
    }  
  \]  
}

---

### **17.2. Tạo panorama**

POST /api/admin/virtual-tours/{tourId}/panoramas

#### **Request body**

{  
  "title": "Cổng chính",  
  "description": "Điểm nhìn tại cổng chính.",  
  "panoramaImageId": "media-010",  
  "thumbnailId": "media-011",  
  "displayOrder": 1,  
  "initialYaw": 0,  
  "initialPitch": 0,  
  "initialFov": 75,  
  "status": "draft"  
}

#### **Response**

{  
  "success": true,  
  "message": "Panorama created successfully.",  
  "data": {  
    "id": "pano-001",  
    "virtualTourId": "tour-001",  
    "title": "Cổng chính",  
    "imageUrl": "https://storage.example.com/panoramas/cong-chinh.jpg"  
  }  
}

---

### **17.3. Tạo nhanh nhiều panorama**

POST /api/admin/virtual-tours/{tourId}/panoramas/bulk

#### **Mục đích**

API này giúp admin upload nhiều ảnh 360 trước, sau đó tạo nhiều panorama cùng lúc.

#### **Request body**

{  
  "items": \[  
    {  
      "title": "Cổng chính",  
      "panoramaImageId": "media-010",  
      "displayOrder": 1  
    },  
    {  
      "title": "Sân chính",  
      "panoramaImageId": "media-011",  
      "displayOrder": 2  
    }  
  \]  
}

---

### **17.4. Lấy chi tiết panorama**

GET /api/admin/panoramas/{panoramaId}

---

### **17.5. Cập nhật panorama**

PUT /api/admin/panoramas/{panoramaId}

---

### **17.6. Cập nhật góc nhìn ban đầu**

PATCH /api/admin/panoramas/{panoramaId}/initial-view

#### **Request body**

{  
  "initialYaw": 45,  
  "initialPitch": 0,  
  "initialFov": 80  
}

---

### **17.7. Sắp xếp panorama**

PATCH /api/admin/virtual-tours/{tourId}/panoramas/reorder

#### **Request body**

{  
  "items": \[  
    {  
      "panoramaId": "pano-001",  
      "displayOrder": 1  
    },  
    {  
      "panoramaId": "pano-002",  
      "displayOrder": 2  
    }  
  \]  
}

---

### **17.8. Xóa panorama**

DELETE /api/admin/panoramas/{panoramaId}

#### **Ghi chú quan trọng**

Trước khi xóa panorama, cần kiểm tra:

Panorama có phải default panorama của tour không?  
Có hotspot nào đang trỏ đến panorama này qua targetPanoramaId không?  
Panorama này có hotspot con không?

Nếu đang được liên kết, nên chặn xóa hoặc yêu cầu admin cập nhật liên kết trước.

---

# **PHẦN J: ADMIN HOTSPOT API**

---

## **18\. Admin Hotspot API**

### **18.1. Lấy danh sách hotspot của panorama**

GET /api/admin/panoramas/{panoramaId}/hotspots

#### **Response**

{  
  "success": true,  
  "data": \[  
    {  
      "id": "hotspot-001",  
      "type": "navigation",  
      "title": "Đi vào sân chính",  
      "yaw": 120,  
      "pitch": \-8,  
      "targetPanoramaId": "pano-002",  
      "status": "published"  
    },  
    {  
      "id": "hotspot-002",  
      "type": "info",  
      "title": "Cổng chính",  
      "description": "Thông tin về cổng chính.",  
      "yaw": 30,  
      "pitch": 5,  
      "status": "published"  
    }  
  \]  
}

---

### **18.2. Tạo hotspot**

POST /api/admin/panoramas/{panoramaId}/hotspots

#### **Request body cho hotspot info**

{  
  "type": "info",  
  "title": "Cổng chính",  
  "description": "Thông tin giới thiệu về cổng chính.",  
  "yaw": 30,  
  "pitch": 5,  
  "icon": "info",  
  "status": "draft"  
}

#### **Request body cho hotspot navigation**

{  
  "type": "navigation",  
  "title": "Đi vào sân chính",  
  "description": null,  
  "yaw": 120,  
  "pitch": \-8,  
  "targetPanoramaId": "pano-002",  
  "icon": "arrow-forward",  
  "status": "draft"  
}

#### **Request body cho hotspot audio**

{  
  "type": "audio",  
  "title": "Nghe thuyết minh",  
  "description": "Thuyết minh về khu vực này.",  
  "yaw": 60,  
  "pitch": 0,  
  "mediaId": "media-audio-001",  
  "icon": "volume",  
  "status": "draft"  
}

#### **Response**

{  
  "success": true,  
  "message": "Hotspot created successfully.",  
  "data": {  
    "id": "hotspot-001",  
    "panoramaId": "pano-001",  
    "type": "navigation",  
    "title": "Đi vào sân chính",  
    "yaw": 120,  
    "pitch": \-8,  
    "targetPanoramaId": "pano-002"  
  }  
}

---

### **18.3. Cập nhật hotspot**

PUT /api/admin/hotspots/{hotspotId}

Request body tương tự tạo hotspot.

---

### **18.4. Cập nhật vị trí hotspot**

PATCH /api/admin/hotspots/{hotspotId}/position

#### **Request body**

{  
  "yaw": 135,  
  "pitch": \-10  
}

#### **Mục đích**

API này dùng khi admin kéo hotspot hoặc đặt lại vị trí trong panorama editor.

---

### **18.5. Cập nhật trạng thái hotspot**

PATCH /api/admin/hotspots/{hotspotId}/status

#### **Request body**

{  
  "status": "published"  
}

---

### **18.6. Xóa hotspot**

DELETE /api/admin/hotspots/{hotspotId}

---

## **19\. Quy tắc validation cho hotspot**

### **19.1. Hotspot navigation**

Nếu `type = navigation`, bắt buộc có:

targetPanoramaId

`targetPanoramaId` phải thuộc cùng một virtual tour với panorama hiện tại.

Không nên cho phép navigation hotspot trỏ sang panorama của tour khác trong MVP.

### **19.2. Hotspot media**

Nếu `type` là:

audio  
video  
image  
model3d

Bắt buộc có:

mediaId

Media phải có `mediaType` tương ứng.

### **19.3. Hotspot external link**

Nếu `type = external_link`, bắt buộc có:

externalUrl

### **19.4. Vị trí hotspot**

`yaw` và `pitch` bắt buộc có.

Giá trị gợi ý:

yaw: \-180 đến 180 hoặc 0 đến 360 tùy thư viện viewer  
pitch: \-90 đến 90

Cần thống nhất theo thư viện 360 viewer được chọn.

---

# **PHẦN K: AUDIO GUIDE API**

---

## **20\. Admin Audio Guide API**

### **20.1. Lấy audio guide theo đối tượng**

GET /api/admin/audio-guides

#### **Query params**

| Tên | Bắt buộc | Mô tả |
| ----- | ----- | ----- |
| targetType | Có thể | destination/tour/panorama/hotspot |
| targetId | Có thể | ID đối tượng |
| languageCode | Không | vi/en |

---

### **20.2. Tạo audio guide**

POST /api/admin/audio-guides

#### **Request body**

{  
  "title": "Thuyết minh tổng quan",  
  "transcript": "Nội dung lời thuyết minh...",  
  "mediaId": "media-audio-001",  
  "languageCode": "vi",  
  "targetType": "destination",  
  "targetId": "destination-001",  
  "duration": 120,  
  "status": "draft"  
}

---

### **20.3. Cập nhật audio guide**

PUT /api/admin/audio-guides/{id}

---

### **20.4. Xóa audio guide**

DELETE /api/admin/audio-guides/{id}

---

# **PHẦN L: TRANSLATION API \- MỞ RỘNG**

---

## **21\. Translation API**

Translation API chưa bắt buộc trong MVP, nhưng nên thiết kế sẵn.

### **21.1. Lấy bản dịch của một đối tượng**

GET /api/admin/translations

#### **Query params**

| Tên | Bắt buộc | Mô tả |
| ----- | ----- | ----- |
| entityType | Có | destination/panorama/hotspot/... |
| entityId | Có | ID đối tượng |
| languageCode | Không | en/km/... |

---

### **21.2. Tạo hoặc cập nhật bản dịch**

PUT /api/admin/translations

#### **Request body**

{  
  "entityType": "destination",  
  "entityId": "destination-001",  
  "fieldName": "fullDescription",  
  "languageCode": "en",  
  "translatedValue": "English description..."  
}

---

# **PHẦN M: ROUTE API \- MỞ RỘNG**

---

## **22\. Public Route API**

### **22.1. Lấy danh sách tuyến tham quan**

GET /api/routes

---

### **22.2. Lấy chi tiết tuyến tham quan**

GET /api/routes/{id}

---

## **23\. Admin Route API**

### **23.1. Tạo tuyến tham quan**

POST /api/admin/routes

#### **Request body**

{  
  "regionId": "region-001",  
  "title": "Tuyến khám phá văn hóa và sinh thái Demo",  
  "slug": "tuyen-van-hoa-sinh-thai-demo",  
  "description": "Tuyến tham quan kết hợp văn hóa và sinh thái.",  
  "estimatedDuration": "Nửa ngày",  
  "distanceKm": 12.5,  
  "theme": "culture-eco",  
  "thumbnailId": "media-001",  
  "status": "draft"  
}

---

### **23.2. Gắn địa điểm vào tuyến**

POST /api/admin/routes/{routeId}/destinations

#### **Request body**

{  
  "destinationId": "destination-001",  
  "displayOrder": 1,  
  "estimatedStayTime": "45 phút",  
  "note": "Điểm bắt đầu tuyến."  
}

---

### **23.3. Sắp xếp địa điểm trong tuyến**

PATCH /api/admin/routes/{routeId}/destinations/reorder

---

# **PHẦN N: ANALYTICS API \- CƠ BẢN**

---

## **24\. Analytics API**

### **24.1. Ghi nhận event**

POST /api/analytics/events

API public, không bắt buộc đăng nhập.

#### **Request body**

{  
  "eventName": "open\_virtual\_tour",  
  "targetType": "destination",  
  "targetId": "destination-001",  
  "sessionId": "session-abc-123",  
  "metadata": {  
    "source": "map",  
    "device": "mobile"  
  }  
}

#### **Ghi chú**

Trong MVP, analytics có thể rất đơn giản hoặc chưa cần triển khai ngay.

---

# **PHẦN O: MODEL 3D API \- MỞ RỘNG**

---

## **25\. Model 3D API**

MVP chưa cần viewer 3D, nhưng API có thể thiết kế sẵn để quản lý metadata.

### **25.1. Tạo model 3D metadata**

POST /api/admin/models-3d

#### **Request body**

{  
  "title": "Mô hình cổng chùa Demo",  
  "description": "Mô hình 3D minh họa.",  
  "mediaId": "media-model-001",  
  "thumbnailId": "media-thumb-001",  
  "targetType": "destination",  
  "targetId": "destination-001",  
  "format": "glb",  
  "status": "draft"  
}

---

# **PHẦN P: API PHỤC VỤ ADMIN EDITOR**

---

## **26\. API hỗ trợ admin chọn vị trí trên bản đồ**

Về bản chất, admin chọn vị trí trên bản đồ là xử lý ở frontend. Khi admin click bản đồ, frontend lấy được:

latitude  
longitude

Sau đó gửi về backend qua API:

POST /api/admin/destinations

hoặc:

PATCH /api/admin/destinations/{id}/location

Vì vậy không cần API riêng để “click bản đồ”. API chỉ cần nhận và lưu tọa độ.

---

## **27\. API hỗ trợ panorama editor**

Panorama editor là màn hình admin dùng để đặt hotspot trong ảnh 360\.

Các API liên quan:

GET   /api/admin/panoramas/{panoramaId}  
GET   /api/admin/panoramas/{panoramaId}/hotspots  
POST  /api/admin/panoramas/{panoramaId}/hotspots  
PATCH /api/admin/hotspots/{hotspotId}/position  
PUT   /api/admin/hotspots/{hotspotId}  
DELETE /api/admin/hotspots/{hotspotId}

Luồng editor:

Admin mở panorama editor  
→ Frontend load panorama image  
→ Frontend load hotspots  
→ Admin xoay đến vị trí mong muốn  
→ Admin click lên ảnh 360  
→ Frontend lấy yaw/pitch  
→ Admin nhập thông tin hotspot  
→ Frontend gọi API tạo hotspot

---

# **PHẦN Q: VALIDATION CHUNG**

---

## **28\. Validation cho destination**

Khi tạo hoặc cập nhật destination:

name bắt buộc  
slug bắt buộc  
regionId bắt buộc  
categoryId bắt buộc  
shortDescription bắt buộc  
latitude bắt buộc  
longitude bắt buộc  
status phải thuộc draft/published/archived  
slug không được trùng trong cùng region

Tọa độ hợp lệ:

latitude: \-90 đến 90  
longitude: \-180 đến 180

---

## **29\. Validation cho media upload**

Backend cần kiểm tra:

File không rỗng  
Dung lượng không vượt quá giới hạn  
Định dạng file hợp lệ  
MIME type hợp lệ  
mediaType hợp lệ  
Người upload có quyền

Gợi ý giới hạn:

| Loại file | Dung lượng tối đa |
| ----- | ----- |
| image | 5 MB |
| panorama | 20 MB |
| video | 100 MB |
| audio | 20 MB |
| model3d | 50 MB |

---

## **30\. Validation cho virtual tour**

destinationId phải tồn tại  
title bắt buộc  
defaultPanoramaId nếu có phải thuộc tour hiện tại  
status hợp lệ

---

## **31\. Validation cho panorama**

virtualTourId phải tồn tại  
title bắt buộc  
panoramaImageId bắt buộc  
panoramaImageId phải là mediaType \= panorama  
displayOrder không âm  
status hợp lệ

---

## **32\. Validation cho hotspot**

panoramaId phải tồn tại  
type bắt buộc  
title bắt buộc  
yaw bắt buộc  
pitch bắt buộc  
status hợp lệ

Theo loại hotspot:

type \= navigation → targetPanoramaId bắt buộc  
type \= audio/video/image/model3d → mediaId bắt buộc  
type \= external\_link → externalUrl bắt buộc

Ngoài ra:

targetPanoramaId phải thuộc cùng virtualTour với panorama hiện tại  
mediaId phải có mediaType phù hợp

---

# **PHẦN R: PHÂN QUYỀN API**

---

## **33\. Public API không cần đăng nhập**

GET /api/regions/current  
GET /api/categories  
GET /api/destinations  
GET /api/destinations/map  
GET /api/destinations/{id}  
GET /api/destinations/{id}/virtual-tour  
GET /api/virtual-tours/{tourId}/panoramas/{panoramaId}  
POST /api/analytics/events

---

## **34\. Admin API cần đăng nhập**

Tất cả endpoint bắt đầu với /api/admin/\*

Ví dụ:

POST /api/admin/destinations  
POST /api/admin/media/upload  
POST /api/admin/virtual-tours/{tourId}/panoramas  
POST /api/admin/panoramas/{panoramaId}/hotspots

---

## **35\. API dành cho Super Admin**

Một số API có thể yêu cầu quyền cao hơn:

Quản lý user  
Quản lý role  
Xóa dữ liệu quan trọng  
Quản lý nhiều region  
Cấu hình hệ thống

MVP có thể chưa cần triển khai đầy đủ.

---

# **PHẦN S: THỨ TỰ TRIỂN KHAI API CHO MVP**

---

## **36\. Sprint API đề xuất**

### **Sprint 1: Auth và dữ liệu nền**

POST /api/auth/login  
GET /api/auth/me  
GET /api/categories  
GET /api/admin/categories  
POST /api/admin/categories  
GET /api/admin/regions  
POST /api/admin/regions

### **Sprint 2: Destination và bản đồ**

GET /api/destinations  
GET /api/destinations/map  
GET /api/destinations/{id}  
GET /api/admin/destinations  
POST /api/admin/destinations  
PUT /api/admin/destinations/{id}  
PATCH /api/admin/destinations/{id}/location  
PATCH /api/admin/destinations/{id}/status

### **Sprint 3: Media**

POST /api/admin/media/upload  
GET /api/admin/media  
GET /api/admin/media/{id}  
PUT /api/admin/media/{id}  
DELETE /api/admin/media/{id}

### **Sprint 4: Virtual Tour và Panorama**

GET /api/destinations/{destinationId}/virtual-tour  
GET /api/admin/destinations/{destinationId}/virtual-tours  
POST /api/admin/destinations/{destinationId}/virtual-tours  
GET /api/admin/virtual-tours/{tourId}  
PUT /api/admin/virtual-tours/{tourId}  
POST /api/admin/virtual-tours/{tourId}/panoramas  
GET /api/admin/virtual-tours/{tourId}/panoramas  
PUT /api/admin/panoramas/{panoramaId}

### **Sprint 5: Hotspot điều hướng**

GET /api/admin/panoramas/{panoramaId}/hotspots  
POST /api/admin/panoramas/{panoramaId}/hotspots  
PUT /api/admin/hotspots/{hotspotId}  
PATCH /api/admin/hotspots/{hotspotId}/position  
DELETE /api/admin/hotspots/{hotspotId}

### **Sprint 6: Audio và hoàn thiện**

POST /api/admin/audio-guides  
GET /api/admin/audio-guides  
PUT /api/admin/audio-guides/{id}  
DELETE /api/admin/audio-guides/{id}  
POST /api/analytics/events

---

## **37\. API tối thiểu bắt buộc để demo MVP**

Nếu cần demo nhanh, chỉ cần các API sau:

POST /api/auth/login

GET /api/categories  
GET /api/destinations/map  
GET /api/destinations/{id}  
GET /api/destinations/{id}/virtual-tour

POST /api/admin/destinations  
PATCH /api/admin/destinations/{id}/location

POST /api/admin/media/upload

POST /api/admin/destinations/{destinationId}/virtual-tours  
POST /api/admin/virtual-tours/{tourId}/panoramas  
POST /api/admin/panoramas/{panoramaId}/hotspots

Với bộ API này, hệ thống đã có thể:

Admin đăng nhập  
Admin tạo địa điểm  
Admin chọn tọa độ trên bản đồ  
Admin upload ảnh 360  
Admin tạo tour  
Admin tạo panorama  
Admin tạo hotspot navigation  
Du khách xem bản đồ  
Du khách mở địa điểm  
Du khách tham quan 360 và di chuyển giữa các panorama

---

## **38\. Kết luận**

API của **Smart Tourism 360 Platform** được thiết kế theo hướng RESTful, rõ ràng và dễ triển khai bằng ASP.NET Core Web API. Hệ thống chia API thành hai nhóm chính: Public API cho du khách và Admin API cho quản trị viên.

Trong MVP, API tập trung vào các chức năng cốt lõi gồm xác thực admin, quản lý khu vực, danh mục, địa điểm, upload media, tạo tour 360, tạo panorama và tạo hotspot. Đặc biệt, API tour 360 được thiết kế để hỗ trợ **multi-scene 360 virtual tour**, trong đó một tour có nhiều panorama và hotspot loại `navigation` dùng `targetPanoramaId` để tạo mũi tên di chuyển giữa các điểm nhìn.

Với thiết kế API này, frontend có thể xây dựng trải nghiệm bản đồ tương tác, trang chi tiết địa điểm, trình xem tour 360 và admin dashboard một cách nhất quán. Đồng thời, hệ thống vẫn có khả năng mở rộng sang đa ngôn ngữ, tuyến tham quan, mô hình 3D, analytics, tài khoản du khách và WebGIS 3D trong các giai đoạn tiếp theo.

