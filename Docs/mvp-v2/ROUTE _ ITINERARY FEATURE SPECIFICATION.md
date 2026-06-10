# **ROUTE / ITINERARY FEATURE SPECIFICATION**

# **Smart Tourism 360 Platform v2**

## **Đặc tả tính năng tuyến tham quan và lịch trình du lịch**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | Route / Itinerary Feature Specification |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v2.0 |
| Giai đoạn | Sau MVP |
| Sprint đề xuất | Sprint 13 |
| Mục đích | Đặc tả tính năng tuyến tham quan cho admin và du khách |
| Đối tượng đọc | Backend developer, frontend developer, UI/UX designer, AI coding agent, project owner |
| Phạm vi | Database, API, admin UI, public UI, bản đồ, route detail, validation, test cases |
| Không thuộc phạm vi | Tối ưu đường đi tự động, dẫn đường GPS real-time, booking, thanh toán, AI itinerary nâng cao |

---

## **2\. Bối cảnh**

Phiên bản MVP của hệ thống đã có các chức năng:

\- Quản lý địa điểm du lịch  
\- Hiển thị địa điểm trên bản đồ  
\- Xem chi tiết địa điểm  
\- Tạo tour 360 cho từng địa điểm  
\- Tạo panorama và hotspot điều hướng

Tuy nhiên, MVP mới trả lời được câu hỏi:

Một địa điểm cụ thể ở đâu?  
Địa điểm đó có gì?  
Có thể tham quan 360 không?

Phiên bản v2 cần mở rộng để trả lời câu hỏi thực tế hơn của du khách:

Nếu tôi có nửa ngày hoặc một ngày, tôi nên đi những đâu?  
Các địa điểm nào nên đi cùng nhau?  
Tuyến tham quan nào phù hợp với chủ đề văn hóa, sinh thái, ẩm thực?  
Tôi có thể xem thứ tự các điểm trên bản đồ không?

Vì vậy, tính năng **Route / Itinerary** được thêm vào để nhóm nhiều địa điểm thành một tuyến tham quan có chủ đề, thứ tự, thời lượng và bản đồ minh họa.

---

## **3\. Mục tiêu tính năng**

Tính năng Route / Itinerary nhằm đạt các mục tiêu sau:

1\. Cho phép admin tạo tuyến tham quan gồm nhiều địa điểm.  
2\. Cho phép admin sắp xếp thứ tự các địa điểm trong tuyến.  
3\. Cho phép admin nhập thông tin tuyến như tên, mô tả, thời lượng, khoảng cách, chủ đề.  
4\. Cho phép du khách xem danh sách tuyến tham quan.  
5\. Cho phép du khách xem chi tiết một tuyến.  
6\. Cho phép du khách xem các địa điểm trong tuyến trên bản đồ.  
7\. Cho phép du khách mở chi tiết từng địa điểm hoặc tour 360 từ tuyến.  
8\. Tạo nền tảng dữ liệu cho AI Tour Guide gợi ý lịch trình trong tương lai.

---

## **4\. Định nghĩa thuật ngữ**

| Thuật ngữ | Ý nghĩa |
| ----- | ----- |
| Route | Tuyến tham quan gồm nhiều địa điểm theo một thứ tự cụ thể |
| Itinerary | Lịch trình gợi ý cho du khách, có thể hiểu gần giống Route |
| Route Destination | Một địa điểm thuộc tuyến tham quan |
| Display Order | Thứ tự địa điểm trong tuyến |
| Estimated Duration | Thời lượng dự kiến của toàn tuyến |
| Estimated Stay Time | Thời gian dừng dự kiến tại từng địa điểm |
| Route Theme | Chủ đề tuyến, ví dụ: văn hóa, sinh thái, ẩm thực |
| Route Map | Bản đồ hiển thị các điểm trong tuyến và đường nối giữa chúng |

Trong tài liệu này, `Route` và `Itinerary` được dùng gần như tương đương. Về mặt database/API, dùng tên chính là `Route`.

---

## **5\. User Persona và nhu cầu**

### **5.1. Du khách**

Nhu cầu:

\- Muốn biết nên đi đâu trong một khoảng thời gian nhất định.  
\- Muốn xem các địa điểm trong cùng một tuyến.  
\- Muốn biết tuyến này phù hợp với chủ đề nào.  
\- Muốn xem bản đồ tuyến trước khi đi.  
\- Muốn mở tour 360 của từng địa điểm để xem trước.

Ví dụ:

“Tôi có nửa ngày ở Cần Thơ thì nên đi đâu?”  
“Tôi muốn đi tuyến văn hóa \- lịch sử.”  
“Tôi muốn xem trước các điểm trong tuyến bằng tour 360.”

---

### **5.2. Admin / Content Manager**

Nhu cầu:

\- Tạo tuyến tham quan cho từng khu vực.  
\- Chọn các địa điểm có sẵn trong hệ thống.  
\- Sắp xếp thứ tự địa điểm.  
\- Nhập mô tả tuyến.  
\- Nhập thời lượng và khoảng cách dự kiến.  
\- Xuất bản tuyến cho du khách.

---

### **5.3. Cơ quan quản lý du lịch**

Nhu cầu:

\- Tổ chức điểm đến thành các tuyến có chủ đề.  
\- Quảng bá tuyến du lịch địa phương.  
\- Định hướng du khách đến các điểm ít được biết đến.  
\- Kết hợp địa điểm văn hóa, sinh thái, ẩm thực và làng nghề.

---

## **6\. Phạm vi chức năng**

### **6.1. Trong Sprint 13**

Sprint 13 nên tập trung vào bản MVP của Route / Itinerary:

\- Tạo bảng routes.  
\- Tạo bảng route\_destinations.  
\- Tạo API quản lý route.  
\- Tạo admin UI quản lý route.  
\- Cho phép admin thêm/xóa/sắp xếp địa điểm trong route.  
\- Tạo public route list page.  
\- Tạo public route detail page.  
\- Hiển thị route trên Leaflet map.  
\- Cho phép mở destination detail/tour 360 từ route.

---

### **6.2. Không làm trong Sprint 13**

Các chức năng chưa cần làm ngay:

\- Tối ưu tuyến tự động theo khoảng cách.  
\- Tính đường đi thật bằng routing engine.  
\- GPS navigation real-time.  
\- Booking tour.  
\- Đặt vé.  
\- Đánh giá tuyến.  
\- AI tự sinh lịch trình.  
\- Gợi ý tuyến cá nhân hóa.  
\- Offline map.

Những phần này có thể để sau.

---

## **7\. User Stories**

### **7.1. Public user stories**

US-01: Là du khách, tôi muốn xem danh sách tuyến tham quan để chọn hành trình phù hợp.

US-02: Là du khách, tôi muốn lọc tuyến theo chủ đề để tìm nhanh tuyến văn hóa, sinh thái hoặc ẩm thực.

US-03: Là du khách, tôi muốn xem chi tiết một tuyến để biết tuyến gồm những địa điểm nào.

US-04: Là du khách, tôi muốn xem các địa điểm trong tuyến trên bản đồ để hiểu vị trí và thứ tự tham quan.

US-05: Là du khách, tôi muốn bấm vào từng địa điểm trong tuyến để xem chi tiết địa điểm.

US-06: Là du khách, tôi muốn mở tour 360 của địa điểm trong tuyến nếu địa điểm đó có tour.

---

### **7.2. Admin user stories**

US-07: Là admin, tôi muốn tạo tuyến tham quan mới cho một khu vực.

US-08: Là admin, tôi muốn chọn nhiều địa điểm vào tuyến.

US-09: Là admin, tôi muốn sắp xếp thứ tự các địa điểm trong tuyến.

US-10: Là admin, tôi muốn nhập thời lượng dự kiến, khoảng cách và chủ đề tuyến.

US-11: Là admin, tôi muốn lưu nháp tuyến trước khi xuất bản.

US-12: Là admin, tôi muốn xem trước tuyến trước khi công khai.

US-13: Là admin, tôi muốn gỡ một địa điểm khỏi tuyến nếu không còn phù hợp.

---

## **8\. Nghiệp vụ chính**

### **8.1. Route là gì?**

Một `Route` là một tuyến tham quan gồm nhiều địa điểm được sắp xếp theo thứ tự.

Ví dụ:

Route: Tuyến khám phá văn hóa Cần Thơ nửa ngày

1\. Nhà Cổ Bình Thủy  
2\. Chùa Ông Cần Thơ  
3\. Bến Ninh Kiều  
4\. Điểm ẩm thực địa phương

---

### **8.2. Một route thuộc một region**

Mỗi route thuộc về một khu vực.

Ví dụ:

Region: Cần Thơ Demo  
  ├── Route: Tuyến văn hóa \- lịch sử  
  ├── Route: Tuyến sinh thái miệt vườn  
  └── Route: Tuyến ẩm thực ven sông

---

### **8.3. Một route có nhiều destinations**

Quan hệ chính:

routes 1 \--- n route\_destinations  
destinations 1 \--- n route\_destinations

Bảng `route_destinations` là bảng trung gian giữa `routes` và `destinations`.

Lý do cần bảng trung gian:

\- Một route có nhiều destination.  
\- Một destination có thể nằm trong nhiều route.  
\- Cần lưu display\_order.  
\- Cần lưu estimated\_stay\_time.  
\- Cần lưu note riêng cho địa điểm trong tuyến.

---

### **8.4. Route có trạng thái xuất bản**

Route cần có trạng thái:

draft  
published  
archived

Ý nghĩa:

draft: admin đang soạn, chưa hiển thị public  
published: hiển thị cho du khách  
archived: ngừng hiển thị, giữ lại dữ liệu

---

## **9\. Database Design**

### **9.1. Bảng `routes`**

Mục đích: lưu thông tin chung của tuyến tham quan.

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| region\_id | uuid | Có | Khu vực chứa tuyến |
| title | varchar(250) | Có | Tên tuyến |
| slug | varchar(300) | Có | Slug URL |
| description | text | Không | Mô tả tuyến |
| estimated\_duration | varchar(100) | Không | Thời lượng dự kiến, ví dụ: Nửa ngày |
| distance\_km | decimal(8,2) | Không | Khoảng cách ước lượng |
| theme | varchar(100) | Không | Chủ đề tuyến |
| thumbnail\_id | uuid | Không | Ảnh đại diện tuyến |
| status | varchar(30) | Có | draft/published/archived |
| created\_by | uuid | Không | Người tạo |
| updated\_by | uuid | Không | Người cập nhật |
| created\_at | timestamp | Có | Thời gian tạo |
| updated\_at | timestamp | Có | Thời gian cập nhật |

Index đề xuất:

region\_id  
slug  
status  
theme  
(region\_id, slug) unique

---

### **9.2. Bảng `route_destinations`**

Mục đích: lưu danh sách địa điểm thuộc tuyến và thứ tự tham quan.

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| route\_id | uuid | Có | Tuyến tham quan |
| destination\_id | uuid | Có | Địa điểm trong tuyến |
| display\_order | integer | Có | Thứ tự địa điểm trong tuyến |
| estimated\_stay\_time | varchar(100) | Không | Thời gian dừng dự kiến |
| note | text | Không | Ghi chú riêng cho điểm này |
| created\_at | timestamp | Có | Thời gian tạo |

Index đề xuất:

route\_id  
destination\_id  
(route\_id, destination\_id) unique  
(route\_id, display\_order)

---

### **9.3. Quan hệ database**

regions 1 \--- n routes

routes 1 \--- n route\_destinations

destinations 1 \--- n route\_destinations

media\_files 1 \--- n routes qua thumbnail\_id

users 1 \--- n routes qua created\_by / updated\_by

---

### **9.4. DBML bổ sung**

Table routes {  
  id uuid \[primary key\]  
  region\_id uuid \[not null\]  
  title varchar(250) \[not null\]  
  slug varchar(300) \[not null\]  
  description text  
  estimated\_duration varchar(100)  
  distance\_km decimal(8,2)  
  theme varchar(100)  
  thumbnail\_id uuid  
  status varchar(30) \[not null, note: 'draft, published, archived'\]  
  created\_by uuid  
  updated\_by uuid  
  created\_at timestamp  
  updated\_at timestamp

  indexes {  
    (region\_id, slug) \[unique\]  
  }  
}

Table route\_destinations {  
  id uuid \[primary key\]  
  route\_id uuid \[not null\]  
  destination\_id uuid \[not null\]  
  display\_order integer \[not null\]  
  estimated\_stay\_time varchar(100)  
  note text  
  created\_at timestamp

  indexes {  
    (route\_id, destination\_id) \[unique\]  
    (route\_id, display\_order)  
  }  
}

Ref: routes.region\_id \> regions.id  
Ref: routes.thumbnail\_id \> media\_files.id  
Ref: routes.created\_by \> users.id  
Ref: routes.updated\_by \> users.id

Ref: route\_destinations.route\_id \> routes.id  
Ref: route\_destinations.destination\_id \> destinations.id

---

## **10\. API Specification**

### **10.1. Public Route API**

#### **Lấy danh sách tuyến tham quan**

GET /api/routes

Query params:

| Tên | Bắt buộc | Mô tả |
| ----- | ----- | ----- |
| regionId | Không | Lọc theo khu vực |
| theme | Không | Lọc theo chủ đề |
| keyword | Không | Tìm theo tên/mô tả |
| page | Không | Trang hiện tại |
| pageSize | Không | Số item mỗi trang |

Response:

{  
  "success": true,  
  "data": {  
    "items": \[  
      {  
        "id": "route-001",  
        "title": "Tuyến khám phá văn hóa Cần Thơ",  
        "slug": "tuyen-kham-pha-van-hoa-can-tho",  
        "description": "Tuyến tham quan kết hợp các điểm văn hóa và biểu tượng địa phương.",  
        "estimatedDuration": "Nửa ngày",  
        "distanceKm": 12.5,  
        "theme": "culture",  
        "thumbnailUrl": "https://storage.example.com/routes/culture-route.jpg",  
        "destinationCount": 4,  
        "status": "published"  
      }  
    \],  
    "page": 1,  
    "pageSize": 10,  
    "totalItems": 1,  
    "totalPages": 1  
  }  
}

---

#### **Lấy chi tiết tuyến tham quan**

GET /api/routes/{id}

Response:

{  
  "success": true,  
  "data": {  
    "id": "route-001",  
    "regionId": "region-001",  
    "title": "Tuyến khám phá văn hóa Cần Thơ",  
    "slug": "tuyen-kham-pha-van-hoa-can-tho",  
    "description": "Tuyến tham quan kết hợp các điểm văn hóa và biểu tượng địa phương.",  
    "estimatedDuration": "Nửa ngày",  
    "distanceKm": 12.5,  
    "theme": "culture",  
    "thumbnailUrl": "https://storage.example.com/routes/culture-route.jpg",  
    "destinations": \[  
      {  
        "id": "destination-001",  
        "name": "Nhà Cổ Bình Thủy",  
        "slug": "nha-co-binh-thuy",  
        "shortDescription": "Di sản kiến trúc cổ tại Cần Thơ.",  
        "coverImageUrl": "https://storage.example.com/nha-co.jpg",  
        "categoryName": "Văn hóa \- lịch sử",  
        "latitude": 10.068,  
        "longitude": 105.732,  
        "displayOrder": 1,  
        "estimatedStayTime": "45 phút",  
        "note": "Điểm bắt đầu tuyến.",  
        "hasVirtualTour": true  
      }  
    \]  
  }  
}

---

### **10.2. Admin Route API**

#### **Lấy danh sách route trong admin**

GET /api/admin/routes

Query params:

regionId  
theme  
status  
keyword  
page  
pageSize

---

#### **Tạo route**

POST /api/admin/routes

Request body:

{  
  "regionId": "region-001",  
  "title": "Tuyến khám phá văn hóa Cần Thơ",  
  "slug": "tuyen-kham-pha-van-hoa-can-tho",  
  "description": "Tuyến tham quan kết hợp các điểm văn hóa và biểu tượng địa phương.",  
  "estimatedDuration": "Nửa ngày",  
  "distanceKm": 12.5,  
  "theme": "culture",  
  "thumbnailId": "media-001",  
  "status": "draft"  
}

---

#### **Cập nhật route**

PUT /api/admin/routes/{id}

---

#### **Cập nhật trạng thái route**

PATCH /api/admin/routes/{id}/status

Request body:

{  
  "status": "published"  
}

---

#### **Xóa hoặc archive route**

DELETE /api/admin/routes/{id}

Khuyến nghị:

Không xóa cứng ngay.  
Chuyển status \= archived.

---

#### **Thêm địa điểm vào route**

POST /api/admin/routes/{routeId}/destinations

Request body:

{  
  "destinationId": "destination-001",  
  "displayOrder": 1,  
  "estimatedStayTime": "45 phút",  
  "note": "Điểm bắt đầu tuyến."  
}

---

#### **Thêm nhiều địa điểm vào route**

POST /api/admin/routes/{routeId}/destinations/bulk

Request body:

{  
  "items": \[  
    {  
      "destinationId": "destination-001",  
      "displayOrder": 1,  
      "estimatedStayTime": "45 phút",  
      "note": "Điểm bắt đầu tuyến."  
    },  
    {  
      "destinationId": "destination-002",  
      "displayOrder": 2,  
      "estimatedStayTime": "30 phút",  
      "note": "Điểm tham quan tiếp theo."  
    }  
  \]  
}

---

#### **Sắp xếp lại địa điểm trong route**

PATCH /api/admin/routes/{routeId}/destinations/reorder

Request body:

{  
  "items": \[  
    {  
      "routeDestinationId": "route-destination-001",  
      "displayOrder": 1  
    },  
    {  
      "routeDestinationId": "route-destination-002",  
      "displayOrder": 2  
    }  
  \]  
}

---

#### **Cập nhật thông tin một destination trong route**

PUT /api/admin/routes/{routeId}/destinations/{routeDestinationId}

Request body:

{  
  "displayOrder": 2,  
  "estimatedStayTime": "30 phút",  
  "note": "Nên ghé vào buổi chiều."  
}

---

#### **Gỡ địa điểm khỏi route**

DELETE /api/admin/routes/{routeId}/destinations/{routeDestinationId}

---

## **11\. Validation Rules**

### **11.1. Route validation**

Khi tạo/cập nhật route:

\- regionId bắt buộc.  
\- title bắt buộc.  
\- slug bắt buộc.  
\- slug không được trùng trong cùng region.  
\- status phải thuộc draft/published/archived.  
\- distanceKm nếu có phải \>= 0\.  
\- thumbnailId nếu có phải tồn tại trong media\_files.

---

### **11.2. Route destination validation**

Khi thêm địa điểm vào route:

\- routeId phải tồn tại.  
\- destinationId phải tồn tại.  
\- destination nên thuộc cùng region với route.  
\- Không được thêm trùng destination vào cùng một route.  
\- displayOrder phải \>= 1\.  
\- estimatedStayTime không bắt buộc.

---

### **11.3. Publish validation**

Trước khi publish route:

\- Route phải có title.  
\- Route phải có ít nhất 2 destinations.  
\- Các destination trong route nên ở trạng thái published.  
\- displayOrder không được trùng.

Nếu chưa đủ điều kiện, backend nên trả lỗi rõ ràng.

---

## **12\. UI/UX Specification**

## **12.1. Public Route List Page**

Route:

/routes

Mục tiêu:

Cho du khách xem danh sách các tuyến tham quan có sẵn.

Layout:

Header  
Page hero  
Search/filter bar  
Route cards grid  
Pagination/load more  
Footer

Route card gồm:

\- Thumbnail  
\- Theme badge  
\- Title  
\- Description ngắn  
\- Estimated duration  
\- Distance  
\- Destination count  
\- CTA “Xem tuyến”

Filter:

\- Keyword  
\- Theme  
\- Region  
\- Duration, có thể để sau

Theme gợi ý:

culture  
eco  
food  
craft  
spiritual  
family  
half-day  
one-day

---

## **12.2. Public Route Detail Page**

Route:

/routes/:slug

Mục tiêu:

Hiển thị chi tiết tuyến tham quan, danh sách địa điểm theo thứ tự và bản đồ tuyến.

Layout đề xuất:

Hero route  
Overview info  
Route map  
Itinerary timeline  
Destination cards  
CTA section

Nội dung:

\- Tên tuyến  
\- Mô tả tuyến  
\- Thời lượng dự kiến  
\- Khoảng cách  
\- Chủ đề  
\- Số địa điểm  
\- Danh sách địa điểm theo thứ tự  
\- Bản đồ tuyến

---

## **12.3. Route Map**

Bản đồ route dùng Leaflet trong Sprint 13\.

Cần hiển thị:

\- Marker cho từng destination.  
\- Số thứ tự trên marker.  
\- Đường nối giữa các destination theo displayOrder.  
\- Popup khi click marker.

Marker popup gồm:

\- Tên địa điểm  
\- Ảnh nhỏ  
\- Thời gian dừng dự kiến  
\- Nút “Xem chi tiết”  
\- Nút “Tour 360” nếu có

Polyline:

\- Nối các destination theo displayOrder.  
\- Chỉ là đường minh họa trực tiếp giữa các điểm.  
\- Không phải route giao thông thực tế.

Ghi chú quan trọng:

Sprint 13 chưa cần tính đường đi thật theo đường bộ.  
Đường nối trên bản đồ chỉ thể hiện thứ tự tham quan.

---

## **12.4. Itinerary Timeline**

Timeline hiển thị các điểm trong tuyến theo thứ tự.

Mỗi item gồm:

\- Số thứ tự  
\- Tên địa điểm  
\- Category badge  
\- Estimated stay time  
\- Note  
\- Has virtual tour badge  
\- Button xem chi tiết  
\- Button tour 360 nếu có

---

## **12.5. Admin Route Management Page**

Route:

/admin/routes

Mục tiêu:

Cho admin xem, tạo, sửa, publish/archive route.

Table gồm:

\- Thumbnail  
\- Title  
\- Region  
\- Theme  
\- Destination count  
\- Estimated duration  
\- Status  
\- Updated at  
\- Actions

Actions:

\- Edit  
\- Manage destinations  
\- Preview  
\- Publish/Archive  
\- Delete/Archive

---

## **12.6. Admin Route Form**

Route:

/admin/routes/create  
/admin/routes/:id/edit

Form sections:

1\. Basic information  
2\. Route metadata  
3\. Thumbnail  
4\. Publishing status

Fields:

\- regionId  
\- title  
\- slug  
\- description  
\- estimatedDuration  
\- distanceKm  
\- theme  
\- thumbnailId  
\- status

---

## **12.7. Admin Manage Route Destinations**

Route:

/admin/routes/:id/destinations

Mục tiêu:

Cho admin chọn và sắp xếp các địa điểm trong tuyến.

Layout đề xuất:

Left panel:  
\- Danh sách destination có thể thêm  
\- Search/filter destination

Right panel:  
\- Danh sách destination đã thêm vào route  
\- Drag/drop reorder hoặc nút lên/xuống  
\- Form chỉnh estimatedStayTime/note

Tính năng:

\- Search destination.  
\- Add destination to route.  
\- Remove destination from route.  
\- Reorder destinations.  
\- Edit note.  
\- Edit estimated stay time.  
\- Preview route map.

Nếu chưa làm drag/drop:

Có thể dùng nút Move Up / Move Down trong Sprint 13\.  
Drag/drop để sprint sau.

---

## **13\. Frontend Components**

### **13.1. Public components**

RouteCard.vue  
RouteListPage.vue  
RouteDetailPage.vue  
RouteMap.vue  
RouteTimeline.vue  
RouteDestinationCard.vue  
RouteThemeBadge.vue

---

### **13.2. Admin components**

AdminRoutesPage.vue  
RouteFormPage.vue  
RouteDestinationManager.vue  
RouteDestinationPicker.vue  
RouteDestinationList.vue  
RoutePreviewMap.vue  
RouteStatusBadge.vue

---

### **13.3. Reusable components**

DestinationMiniCard.vue  
MapPolyline.vue  
NumberedMapMarker.vue  
EmptyState.vue  
LoadingSkeleton.vue  
ConfirmModal.vue  
ToastContainer.vue

---

## **14\. Backend Implementation Design**

### **14.1. Domain Entities**

Cần thêm entity:

Route  
RouteDestination

---

### **14.2. Route Entity**

Route  
\- Id  
\- RegionId  
\- Title  
\- Slug  
\- Description  
\- EstimatedDuration  
\- DistanceKm  
\- Theme  
\- ThumbnailId  
\- Status  
\- CreatedBy  
\- UpdatedBy  
\- CreatedAt  
\- UpdatedAt  
\- Region  
\- Thumbnail  
\- RouteDestinations

---

### **14.3. RouteDestination Entity**

RouteDestination  
\- Id  
\- RouteId  
\- DestinationId  
\- DisplayOrder  
\- EstimatedStayTime  
\- Note  
\- CreatedAt  
\- Route  
\- Destination

---

### **14.4. Service Layer**

Service đề xuất:

IRouteService  
RouteService

Methods:

GetPublicRoutesAsync  
GetPublicRouteDetailAsync  
GetAdminRoutesAsync  
GetAdminRouteDetailAsync  
CreateRouteAsync  
UpdateRouteAsync  
UpdateRouteStatusAsync  
ArchiveRouteAsync  
AddDestinationToRouteAsync  
AddDestinationsToRouteAsync  
UpdateRouteDestinationAsync  
ReorderRouteDestinationsAsync  
RemoveDestinationFromRouteAsync

---

### **14.5. Controllers**

Public:

RoutesController

Admin:

AdminRoutesController

---

## **15\. Seed Data**

Nên seed ít nhất 3 route mẫu:

1\. Tuyến khám phá văn hóa Cần Thơ  
2\. Tuyến sinh thái miệt vườn  
3\. Tuyến ẩm thực và sông nước

Ví dụ:

Tuyến khám phá văn hóa Cần Thơ:  
1\. Nhà Cổ Bình Thủy  
2\. Chùa Ông Cần Thơ  
3\. Bến Ninh Kiều

Tuyến sinh thái miệt vườn:  
1\. Làng du lịch sinh thái Mỹ Khánh  
2\. Vườn trái cây Demo  
3\. Homestay nhà vườn Demo

Tuyến ẩm thực và sông nước:  
1\. Bến Ninh Kiều  
2\. Điểm ẩm thực địa phương  
3\. Chợ đêm Demo

Seed phải idempotent:

\- Không tạo trùng route nếu đã tồn tại slug.  
\- Không tạo trùng destination trong route.

---

## **16\. Analytics liên quan đến Route**

Nếu Sprint 14 làm analytics, Route nên ghi nhận các event:

view\_route  
click\_route\_destination  
open\_destination\_from\_route  
open\_virtual\_tour\_from\_route

Sprint 13 chỉ cần chuẩn bị tên event, chưa bắt buộc triển khai.

---

## **17\. Integration với tính năng hiện có**

### **17.1. Destination Detail**

Trang chi tiết địa điểm có thể hiển thị:

“Địa điểm này nằm trong các tuyến tham quan”

Ví dụ:

\- Tuyến khám phá văn hóa Cần Thơ  
\- Tuyến nửa ngày ven sông

Nếu chưa muốn thêm API mới, để sau.

---

### **17.2. Public Map**

Có thể thêm layer route:

\- Khi xem route detail, hiển thị các marker và polyline.  
\- Trang explore map chưa cần hiển thị tất cả route.

---

### **17.3. Tour 360**

Trong route detail, nếu destination có tour:

Hiển thị button “Tham quan 360”

Click sẽ mở:

/destinations/:id/tour

---

## **18\. Test Cases**

### **18.1. Backend test cases**

TC-BE-01: Tạo route với dữ liệu hợp lệ.  
TC-BE-02: Không cho tạo route thiếu title.  
TC-BE-03: Không cho tạo route trùng slug trong cùng region.  
TC-BE-04: Thêm destination vào route.  
TC-BE-05: Không cho thêm trùng destination vào cùng route.  
TC-BE-06: Reorder destinations trong route.  
TC-BE-07: Không cho publish route có ít hơn 2 destinations.  
TC-BE-08: Public API chỉ trả route published.  
TC-BE-09: Route detail trả destinations đúng displayOrder.

---

### **18.2. Frontend public test cases**

TC-FE-01: Route list page hiển thị danh sách route.  
TC-FE-02: Filter theme hoạt động.  
TC-FE-03: Route detail hiển thị đúng title/description.  
TC-FE-04: Timeline hiển thị destinations đúng thứ tự.  
TC-FE-05: Route map hiển thị marker đúng tọa độ.  
TC-FE-06: Polyline nối đúng thứ tự địa điểm.  
TC-FE-07: Click destination mở trang chi tiết.  
TC-FE-08: Click Tour 360 mở tour nếu có.

---

### **18.3. Frontend admin test cases**

TC-ADMIN-01: Admin tạo route mới.  
TC-ADMIN-02: Admin sửa route.  
TC-ADMIN-03: Admin publish route.  
TC-ADMIN-04: Admin thêm destination vào route.  
TC-ADMIN-05: Admin xóa destination khỏi route.  
TC-ADMIN-06: Admin reorder destination.  
TC-ADMIN-07: Admin preview route map.  
TC-ADMIN-08: Admin archive route.

---

## **19\. Sprint 13 Implementation Plan**

### **19.1. Mục tiêu Sprint 13**

Bổ sung module Route / Itinerary để admin có thể tạo tuyến tham quan và du khách có thể xem tuyến trên public website.

---

### **19.2. Checklist Database**

\- \[ \] Tạo entity Route.  
\- \[ \] Tạo entity RouteDestination.  
\- \[ \] Tạo migration thêm bảng routes.  
\- \[ \] Tạo migration thêm bảng route\_destinations.  
\- \[ \] Thêm index cần thiết.  
\- \[ \] Seed route mẫu.

---

### **19.3. Checklist Backend API**

\- \[ \] Tạo DTO RouteListItem.  
\- \[ \] Tạo DTO RouteDetail.  
\- \[ \] Tạo DTO CreateRoute.  
\- \[ \] Tạo DTO UpdateRoute.  
\- \[ \] Tạo DTO AddRouteDestination.  
\- \[ \] Tạo DTO ReorderRouteDestinations.  
\- \[ \] Tạo RouteService.  
\- \[ \] Tạo Public RoutesController.  
\- \[ \] Tạo AdminRoutesController.  
\- \[ \] Tạo public GET /api/routes.  
\- \[ \] Tạo public GET /api/routes/{id}.  
\- \[ \] Tạo admin CRUD route.  
\- \[ \] Tạo API thêm destination vào route.  
\- \[ \] Tạo API reorder destination.  
\- \[ \] Tạo API remove destination khỏi route.  
\- \[ \] Validation route.  
\- \[ \] Validation publish route.

---

### **19.4. Checklist Public Frontend**

\- \[ \] Tạo /routes.  
\- \[ \] Tạo /routes/:slug hoặc /routes/:id.  
\- \[ \] Tạo RouteCard.  
\- \[ \] Tạo RouteDetailPage.  
\- \[ \] Tạo RouteMap.  
\- \[ \] Tạo RouteTimeline.  
\- \[ \] Hiển thị marker đánh số.  
\- \[ \] Hiển thị polyline nối điểm.  
\- \[ \] Button xem destination detail.  
\- \[ \] Button mở tour 360 nếu có.  
\- \[ \] Loading/empty/error states.

---

### **19.5. Checklist Admin Frontend**

\- \[ \] Tạo /admin/routes.  
\- \[ \] Tạo route table.  
\- \[ \] Tạo route form create/edit.  
\- \[ \] Tạo route destination manager.  
\- \[ \] Tạo destination picker.  
\- \[ \] Tạo route destination list.  
\- \[ \] Tạo reorder bằng nút up/down.  
\- \[ \] Tạo route preview map.  
\- \[ \] Tạo publish/archive actions.

---

### **19.6. Checklist Regression**

\- \[ \] Destination APIs vẫn hoạt động.  
\- \[ \] Public map vẫn hoạt động.  
\- \[ \] Destination detail vẫn hoạt động.  
\- \[ \] Tour 360 vẫn hoạt động.  
\- \[ \] Admin login vẫn hoạt động.  
\- \[ \] Docker build vẫn hoạt động.

---

## **20\. Acceptance Criteria**

Sprint 13 hoàn thành khi:

\- Admin tạo được route mới.  
\- Admin thêm được nhiều destination vào route.  
\- Admin sắp xếp thứ tự destinations trong route.  
\- Admin publish/archive route.  
\- Public route list hiển thị route published.  
\- Public route detail hiển thị danh sách destination đúng thứ tự.  
\- Route map hiển thị marker và polyline.  
\- Du khách mở được destination detail từ route.  
\- Du khách mở được tour 360 từ route nếu destination có tour.  
\- Seed data có ít nhất 2-3 route mẫu.  
\- Không làm hỏng các chức năng MVP.

---

## **21\. Non-goals**

Sprint 13 không bao gồm:

\- Không tính đường đi thật theo Google Maps/OpenRouteService.  
\- Không tối ưu tuyến tự động.  
\- Không GPS navigation.  
\- Không booking tour.  
\- Không đánh giá route.  
\- Không AI gợi ý route.  
\- Không CesiumJS 3D route.

Các chức năng trên có thể phát triển sau.

---

## **22\. Rủi ro và cách kiểm soát**

### **22.1. Rủi ro route map bị hiểu nhầm là đường đi thật**

Cách kiểm soát:

Ghi chú rõ:  
“Đường nối trên bản đồ chỉ thể hiện thứ tự tham quan, không phải chỉ dẫn giao thông thực tế.”

---

### **22.2. Rủi ro admin UI quá phức tạp**

Cách kiểm soát:

Sprint 13 dùng nút up/down để reorder.  
Drag/drop để sau.

---

### **22.3. Rủi ro dữ liệu route trùng hoặc sai thứ tự**

Cách kiểm soát:

\- Unique route\_id \+ destination\_id.  
\- Validate displayOrder.  
\- Khi reorder, backend cập nhật theo transaction.

---

### **22.4. Rủi ro public API trả quá nhiều dữ liệu**

Cách kiểm soát:

Route list chỉ trả dữ liệu tóm tắt.  
Route detail mới trả destinations đầy đủ.

---

## **23\. Kết luận**

Tính năng **Route / Itinerary** là bước mở rộng nghiệp vụ quan trọng sau MVP. Nó giúp hệ thống chuyển từ việc chỉ hiển thị từng địa điểm riêng lẻ sang hỗ trợ du khách khám phá một khu vực theo tuyến tham quan có chủ đề và thứ tự rõ ràng.

Sau Sprint 13, hệ thống sẽ có thêm giá trị thực tế:

Du khách không chỉ hỏi “địa điểm này ở đâu?”  
mà còn có thể biết “nên đi những địa điểm nào cùng nhau?”

Tính năng này cũng là nền tảng rất tốt cho các bước phát triển tiếp theo như Analytics, Multi-language, CesiumJS 3D Map và AI Tour Guide.

