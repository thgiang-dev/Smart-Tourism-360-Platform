# **ANALYTICS & TRACKING SPECIFICATION**

# **Smart Tourism 360 Platform v2**

## **Đặc tả hệ thống ghi nhận hành vi người dùng và dashboard thống kê**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | Analytics & Tracking Specification |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v2.0 |
| Giai đoạn | Sau MVP |
| Sprint đề xuất | Sprint 14 |
| Mục đích | Đặc tả hệ thống ghi nhận hành vi người dùng, lưu sự kiện tương tác và hiển thị thống kê cho admin |
| Đối tượng đọc | Backend developer, frontend developer, UI/UX designer, AI coding agent, project owner |
| Phạm vi | Tracking event, database, API, frontend integration, admin analytics dashboard |
| Không thuộc phạm vi | Big Data analytics, realtime streaming, machine learning recommendation, Google Analytics integration nâng cao |

---

## **2\. Bối cảnh**

Sau MVP, hệ thống đã có các chức năng cốt lõi:

\- Public website  
\- Bản đồ địa điểm  
\- Chi tiết địa điểm  
\- Tour 360  
\- Panorama  
\- Hotspot  
\- Route / Itinerary, theo roadmap Sprint 13  
\- Admin dashboard

Tuy nhiên, hệ thống hiện mới tập trung vào việc hiển thị và quản lý nội dung. Để phục vụ tốt hơn cho quản trị viên và cơ quan quản lý du lịch, cần bổ sung khả năng ghi nhận hành vi người dùng.

Các câu hỏi cần trả lời sau khi có analytics:

\- Địa điểm nào được xem nhiều nhất?  
\- Tour 360 nào được mở nhiều nhất?  
\- Hotspot nào được người dùng tương tác nhiều nhất?  
\- Người dùng có thường xuyên chuyển panorama không?  
\- Tuyến tham quan nào được quan tâm nhiều?  
\- Người dùng hay tìm kiếm/lọc theo danh mục nào?  
\- Nội dung nào cần cải thiện vì ít tương tác?

Analytics giúp dự án chuyển từ “nền tảng hiển thị du lịch số” sang “nền tảng có khả năng đo lường và hỗ trợ ra quyết định”.

---

## **3\. Mục tiêu tính năng**

Tính năng Analytics & Tracking nhằm đạt các mục tiêu:

1\. Ghi nhận các hành vi quan trọng của du khách trên public website.  
2\. Lưu các event như xem địa điểm, mở tour 360, click hotspot, chuyển panorama, xem route.  
3\. Tổng hợp số liệu cơ bản cho admin dashboard.  
4\. Giúp admin biết địa điểm, tour, route và hotspot nào được quan tâm nhiều.  
5\. Tạo nền tảng dữ liệu cho AI recommendation và báo cáo du lịch trong tương lai.  
6\. Triển khai đơn giản, an toàn, không ảnh hưởng hiệu năng MVP.

---

## **4\. Nguyên tắc thiết kế analytics**

### **4.1. Tracking vừa đủ, không quá phức tạp**

Sprint 14 chỉ cần ghi nhận các event cốt lõi. Không cần làm hệ thống phân tích phức tạp như Google Analytics, Mixpanel hoặc realtime dashboard.

Ưu tiên:

\- Dễ triển khai  
\- Dễ kiểm tra  
\- Dễ mở rộng  
\- Không ảnh hưởng trải nghiệm người dùng

---

### **4.2. Không thu thập dữ liệu nhạy cảm**

Trong MVP/v2, hệ thống không cần thu thập thông tin cá nhân của du khách.

Không tracking:

\- Họ tên  
\- Email  
\- Số điện thoại  
\- Địa chỉ cụ thể  
\- GPS realtime chính xác  
\- Dữ liệu định danh cá nhân

Chỉ tracking hành vi ẩn danh thông qua:

\- sessionId  
\- eventName  
\- targetType  
\- targetId  
\- metadata kỹ thuật cơ bản

---

### **4.3. Tracking không được làm hỏng UX**

Nếu API analytics lỗi, frontend không được làm gián đoạn trải nghiệm người dùng.

Ví dụ:

User mở tour 360  
→ gọi tracking open\_virtual\_tour  
→ nếu tracking lỗi thì vẫn phải mở tour bình thường

Analytics request nên chạy dạng “fire-and-forget” ở frontend.

---

### **4.4. Dữ liệu analytics phải có khả năng tổng hợp**

Mỗi event cần có đủ thông tin để sau này tổng hợp:

\- Event gì?  
\- Xảy ra lúc nào?  
\- Xảy ra trên đối tượng nào?  
\- Thuộc session nào?  
\- Có metadata bổ sung gì?

---

## **5\. Danh sách event cần tracking**

### **5.1. Event cho địa điểm**

| Event name | Khi nào ghi nhận | targetType | targetId |
| ----- | ----- | ----- | ----- |
| `view_destination` | Người dùng mở trang chi tiết địa điểm | `destination` | destinationId |
| `click_map_marker` | Người dùng click marker trên bản đồ | `destination` | destinationId |
| `open_destination_from_map` | Người dùng bấm xem chi tiết từ marker popup | `destination` | destinationId |
| `open_virtual_tour` | Người dùng mở tour 360 của địa điểm | `destination` hoặc `tour` | destinationId/tourId |

---

### **5.2. Event cho tour 360**

| Event name | Khi nào ghi nhận | targetType | targetId |
| ----- | ----- | ----- | ----- |
| `open_virtual_tour` | Mở tour 360 | `tour` | tourId |
| `view_panorama` | Panorama được load | `panorama` | panoramaId |
| `navigate_panorama` | Người dùng click hotspot navigation để chuyển scene | `hotspot` | hotspotId |
| `click_hotspot` | Người dùng click bất kỳ hotspot nào | `hotspot` | hotspotId |
| `play_audio` | Người dùng phát audio từ hotspot/audio guide | `audio` | audioGuideId hoặc mediaId |
| `play_video` | Người dùng mở video từ hotspot | `video` | mediaId |

---

### **5.3. Event cho route / itinerary**

| Event name | Khi nào ghi nhận | targetType | targetId |
| ----- | ----- | ----- | ----- |
| `view_route` | Người dùng mở chi tiết tuyến tham quan | `route` | routeId |
| `click_route_destination` | Người dùng click một địa điểm trong route timeline | `destination` | destinationId |
| `open_destination_from_route` | Người dùng mở chi tiết địa điểm từ route | `destination` | destinationId |
| `open_virtual_tour_from_route` | Người dùng mở tour 360 từ route | `tour` hoặc `destination` | tourId/destinationId |

---

### **5.4. Event cho tìm kiếm và lọc**

| Event name | Khi nào ghi nhận | targetType | targetId |
| ----- | ----- | ----- | ----- |
| `search_destination` | Người dùng tìm kiếm địa điểm | `search` | null |
| `filter_category` | Người dùng lọc theo danh mục | `category` | categoryId |
| `search_route` | Người dùng tìm kiếm route | `search` | null |
| `filter_route_theme` | Người dùng lọc route theo theme | `route_theme` | null |

---

## **6\. Database Design**

### **6.1. Bảng `analytics_events`**

Bảng `analytics_events` lưu toàn bộ event hành vi của người dùng.

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| event\_name | varchar(100) | Có | Tên sự kiện |
| target\_type | varchar(50) | Không | Loại đối tượng: destination, tour, panorama, hotspot, route |
| target\_id | uuid | Không | ID đối tượng liên quan |
| session\_id | varchar(150) | Không | Mã phiên ẩn danh của người dùng |
| user\_id | uuid | Không | Nếu sau này có tài khoản user |
| metadata | jsonb | Không | Dữ liệu bổ sung |
| created\_at | timestamp | Có | Thời gian xảy ra event |

---

### **6.2. Metadata gợi ý**

Trường `metadata` có thể chứa thông tin phụ tùy event.

Ví dụ event `open_virtual_tour`:

{  
  "destinationId": "destination-001",  
  "source": "destination\_detail",  
  "deviceType": "desktop"  
}

Ví dụ event `navigate_panorama`:

{  
  "tourId": "tour-001",  
  "fromPanoramaId": "pano-001",  
  "toPanoramaId": "pano-002",  
  "hotspotType": "navigation"  
}

Ví dụ event `search_destination`:

{  
  "keyword": "nhà cổ",  
  "resultCount": 3  
}

Ví dụ event `filter_category`:

{  
  "categoryName": "Sinh thái",  
  "resultCount": 5  
}

---

### **6.3. DBML**

Table analytics\_events {  
  id uuid \[primary key\]  
  event\_name varchar(100) \[not null, note: 'view\_destination, open\_virtual\_tour, click\_hotspot, navigate\_panorama, view\_route'\]  
  target\_type varchar(50)  
  target\_id uuid  
  session\_id varchar(150)  
  user\_id uuid  
  metadata jsonb  
  created\_at timestamp  
}

Ref: analytics\_events.user\_id \> users.id

---

### **6.4. Index đề xuất**

analytics\_events.event\_name  
analytics\_events.target\_type, target\_id  
analytics\_events.created\_at  
analytics\_events.session\_id  
analytics\_events.user\_id

SQL minh họa:

CREATE INDEX idx\_analytics\_events\_event\_name   
ON analytics\_events(event\_name);

CREATE INDEX idx\_analytics\_events\_target   
ON analytics\_events(target\_type, target\_id);

CREATE INDEX idx\_analytics\_events\_created\_at   
ON analytics\_events(created\_at);

CREATE INDEX idx\_analytics\_events\_session\_id   
ON analytics\_events(session\_id);

---

## **7\. API Specification**

## **7.1. Public tracking API**

### **Ghi nhận event**

POST /api/analytics/events

Request body:

{  
  "eventName": "open\_virtual\_tour",  
  "targetType": "tour",  
  "targetId": "60000000-0000-0000-0000-000000000001",  
  "sessionId": "st360-session-abc123",  
  "metadata": {  
    "destinationId": "40000000-0000-0000-0000-000000000002",  
    "source": "destination\_detail",  
    "deviceType": "desktop"  
  }  
}

Response:

{  
  "success": true,  
  "message": "Event tracked successfully."  
}

Ghi chú:

\- API này public, không bắt buộc đăng nhập.  
\- Cần validate eventName.  
\- Không nên trả quá nhiều dữ liệu.  
\- Nếu event không hợp lệ, trả 400\.

---

## **7.2. Admin analytics summary API**

### **Lấy tổng quan analytics**

GET /api/admin/analytics/summary

Query params:

| Tên | Bắt buộc | Mô tả |
| ----- | ----- | ----- |
| fromDate | Không | Ngày bắt đầu |
| toDate | Không | Ngày kết thúc |
| regionId | Không | Lọc theo region nếu có thể |

Response:

{  
  "success": true,  
  "data": {  
    "totalEvents": 1250,  
    "totalDestinationViews": 420,  
    "totalVirtualTourOpens": 180,  
    "totalHotspotClicks": 360,  
    "totalPanoramaNavigations": 240,  
    "totalRouteViews": 50  
  }  
}

---

## **7.3. Top destinations API**

GET /api/admin/analytics/top-destinations

Query params:

fromDate  
toDate  
limit

Response:

{  
  "success": true,  
  "data": \[  
    {  
      "destinationId": "40000000-0000-0000-0000-000000000002",  
      "destinationName": "Nhà Cổ Bình Thủy",  
      "viewCount": 120,  
      "tourOpenCount": 60  
    }  
  \]  
}

---

## **7.4. Top virtual tours API**

GET /api/admin/analytics/top-tours

Response:

{  
  "success": true,  
  "data": \[  
    {  
      "tourId": "60000000-0000-0000-0000-000000000001",  
      "tourTitle": "Tour 360 Nhà Cổ Bình Thủy",  
      "openCount": 80,  
      "panoramaNavigationCount": 150  
    }  
  \]  
}

---

## **7.5. Top hotspots API**

GET /api/admin/analytics/top-hotspots

Response:

{  
  "success": true,  
  "data": \[  
    {  
      "hotspotId": "80000000-0000-0000-0000-000000000001",  
      "hotspotTitle": "Đi vào trong nhà cổ",  
      "hotspotType": "navigation",  
      "clickCount": 95  
    }  
  \]  
}

---

## **7.6. Events by day API**

GET /api/admin/analytics/events-by-day

Query params:

fromDate  
toDate  
eventName

Response:

{  
  "success": true,  
  "data": \[  
    {  
      "date": "2026-06-01",  
      "count": 40  
    },  
    {  
      "date": "2026-06-02",  
      "count": 65  
    }  
  \]  
}

---

## **7.7. Route analytics API**

GET /api/admin/analytics/top-routes

Response:

{  
  "success": true,  
  "data": \[  
    {  
      "routeId": "90000000-0000-0000-0000-000000000001",  
      "routeTitle": "Tuyến khám phá văn hóa Cần Thơ",  
      "viewCount": 40,  
      "destinationClickCount": 25  
    }  
  \]  
}

---

## **8\. Event Validation Rules**

### **8.1. Danh sách event hợp lệ**

Backend nên định nghĩa danh sách event hợp lệ:

view\_destination  
click\_map\_marker  
open\_destination\_from\_map  
open\_virtual\_tour  
view\_panorama  
click\_hotspot  
navigate\_panorama  
play\_audio  
play\_video  
view\_route  
click\_route\_destination  
open\_destination\_from\_route  
open\_virtual\_tour\_from\_route  
search\_destination  
filter\_category  
search\_route  
filter\_route\_theme

Nếu eventName không thuộc danh sách này, backend trả:

400 Bad Request

---

### **8.2. targetType hợp lệ**

Danh sách `targetType` hợp lệ:

destination  
tour  
panorama  
hotspot  
route  
category  
audio  
video  
search  
route\_theme

---

### **8.3. targetId**

Quy tắc:

\- Với event liên quan object cụ thể, targetId nên có.  
\- Với search/filter text, targetId có thể null.  
\- Backend không bắt buộc phải kiểm tra tồn tại target ở Sprint 14 để tránh phức tạp.  
\- Có thể bổ sung kiểm tra target tồn tại ở sprint sau.

---

### **8.4. sessionId**

Frontend cần tạo một `sessionId` ẩn danh.

Quy tắc:

\- Nếu localStorage chưa có sessionId, tạo mới.  
\- Lưu sessionId trong localStorage.  
\- Gửi sessionId trong mỗi tracking event.

Ví dụ:

st360-session-1723456789-random

---

## **9\. Frontend Tracking Design**

### **9.1. Analytics service**

Tạo service frontend:

src/services/analytics.service.js

Hàm chính:

trackEvent({  
  eventName,  
  targetType,  
  targetId,  
  metadata  
})

Service tự động:

\- Lấy hoặc tạo sessionId.  
\- Gửi request POST /api/analytics/events.  
\- Bắt lỗi silently.  
\- Không block UI.

---

### **9.2. Session utility**

Tạo utility:

src/utils/session.js

Chức năng:

\- getOrCreateSessionId()  
\- getDeviceType()

Device type có thể đơn giản:

mobile nếu window.innerWidth \< 768  
tablet nếu \< 1024  
desktop nếu \>= 1024

---

### **9.3. Tracking trên Public Map**

Khi user click marker:

eventName: click\_map\_marker  
targetType: destination  
targetId: destinationId  
metadata:  
  source: explore\_map  
  categoryId

Khi user bấm “Xem chi tiết” từ popup:

eventName: open\_destination\_from\_map  
targetType: destination  
targetId: destinationId

---

### **9.4. Tracking trên Destination Detail**

Khi page detail mounted:

eventName: view\_destination  
targetType: destination  
targetId: destinationId

Khi user bấm tour 360:

eventName: open\_virtual\_tour  
targetType: destination  
targetId: destinationId  
metadata:  
  source: destination\_detail  
  tourId

---

### **9.5. Tracking trên Tour 360**

Khi tour page mở:

eventName: open\_virtual\_tour  
targetType: tour  
targetId: tourId

Khi panorama được load:

eventName: view\_panorama  
targetType: panorama  
targetId: panoramaId  
metadata:  
  tourId

Khi click hotspot:

eventName: click\_hotspot  
targetType: hotspot  
targetId: hotspotId  
metadata:  
  hotspotType  
  panoramaId  
  tourId

Khi click navigation hotspot:

eventName: navigate\_panorama  
targetType: hotspot  
targetId: hotspotId  
metadata:  
  tourId  
  fromPanoramaId  
  toPanoramaId

---

### **9.6. Tracking trên Route**

Khi mở route detail:

eventName: view\_route  
targetType: route  
targetId: routeId

Khi click destination trong route:

eventName: click\_route\_destination  
targetType: destination  
targetId: destinationId  
metadata:  
  routeId  
  displayOrder

Khi mở destination từ route:

eventName: open\_destination\_from\_route  
targetType: destination  
targetId: destinationId  
metadata:  
  routeId

Khi mở tour từ route:

eventName: open\_virtual\_tour\_from\_route  
targetType: destination  
targetId: destinationId  
metadata:  
  routeId  
  tourId

---

## **10\. Admin Analytics Dashboard**

### **10.1. Mục tiêu**

Dashboard analytics giúp admin hiểu hành vi người dùng và mức độ quan tâm đến nội dung du lịch.

Route:

/admin/analytics

---

### **10.2. Bộ lọc thời gian**

Dashboard nên có filter:

\- Hôm nay  
\- 7 ngày gần nhất  
\- 30 ngày gần nhất  
\- Tùy chọn fromDate/toDate

Sprint 14 có thể làm đơn giản:

7 ngày gần nhất  
30 ngày gần nhất

---

### **10.3. Summary cards**

Các card chính:

\- Tổng lượt xem địa điểm  
\- Tổng lượt mở tour 360  
\- Tổng lượt click hotspot  
\- Tổng lượt chuyển panorama  
\- Tổng lượt xem route

Mỗi card gồm:

\- Icon  
\- Label  
\- Number  
\- Mô tả ngắn

---

### **10.4. Charts**

Sprint 14 nên có ít nhất 1 biểu đồ:

\- Events by day

Biểu đồ gợi ý:

Line chart hoặc bar chart

Nếu chưa muốn thêm chart library, có thể dùng bảng hoặc card list trước. Nhưng nếu dùng chart, có thể chọn:

Chart.js  
hoặc ECharts

Khuyến nghị:

Chart.js cho đơn giản.

---

### **10.5. Top lists**

Dashboard nên hiển thị:

\- Top destinations  
\- Top virtual tours  
\- Top hotspots  
\- Top routes, nếu Sprint 13 đã có route

Mỗi list item gồm:

\- Tên đối tượng  
\- Loại đối tượng  
\- Số lượt tương tác  
\- Nút xem chi tiết nếu cần

---

## **11\. Backend Analytics Dashboard Design**

### **11.1. Service**

Tạo service:

IAnalyticsService  
AnalyticsService

Methods:

TrackEventAsync  
GetSummaryAsync  
GetTopDestinationsAsync  
GetTopToursAsync  
GetTopHotspotsAsync  
GetTopRoutesAsync  
GetEventsByDayAsync

---

### **11.2. Controller**

Public:

AnalyticsController  
POST /api/analytics/events

Admin:

AdminAnalyticsController  
GET /api/admin/analytics/summary  
GET /api/admin/analytics/top-destinations  
GET /api/admin/analytics/top-tours  
GET /api/admin/analytics/top-hotspots  
GET /api/admin/analytics/top-routes  
GET /api/admin/analytics/events-by-day

---

### **11.3. Query logic**

Summary:

Đếm số event theo event\_name trong khoảng thời gian.

Top destinations:

Lọc event\_name \= view\_destination hoặc open\_virtual\_tour  
Group by target\_id  
Join destinations để lấy tên  
Order by count desc

Top tours:

Lọc event\_name \= open\_virtual\_tour  
target\_type \= tour  
Group by target\_id  
Join virtual\_tours

Top hotspots:

Lọc event\_name \= click\_hotspot hoặc navigate\_panorama  
Group by target\_id  
Join hotspots

Events by day:

Group by date(created\_at)  
Order by date

---

## **12\. UI/UX cho Analytics Dashboard**

### **12.1. Layout**

Page header  
Date filter  
Summary cards  
Events by day chart  
Top destinations  
Top tours  
Top hotspots  
Top routes

---

### **12.2. Empty state**

Nếu chưa có analytics data:

Chưa có dữ liệu tương tác.  
Dữ liệu sẽ xuất hiện sau khi du khách truy cập địa điểm, mở tour 360 hoặc tương tác với hotspot.

---

### **12.3. Loading state**

Cần có skeleton/loading cho:

\- Summary cards  
\- Chart  
\- Top list

---

### **12.4. Error state**

Nếu không tải được analytics:

Không thể tải dữ liệu thống kê. Vui lòng thử lại sau.

---

## **13\. Privacy & Data Protection**

### **13.1. Nguyên tắc riêng tư**

Hệ thống chỉ tracking hành vi ẩn danh.

Không thu thập:

\- Email du khách  
\- Tên du khách  
\- Số điện thoại  
\- Địa chỉ nhà  
\- GPS realtime chính xác

---

### **13.2. Session ID**

Session ID chỉ dùng để phân biệt phiên truy cập ẩn danh.

Ví dụ:

st360-session-abc123

Không dùng session ID để định danh người thật.

---

### **13.3. Metadata**

Metadata không nên chứa dữ liệu nhạy cảm. Chỉ lưu thông tin kỹ thuật và ngữ cảnh:

\- source  
\- deviceType  
\- categoryId  
\- tourId  
\- fromPanoramaId  
\- toPanoramaId  
\- resultCount

---

## **14\. Performance Considerations**

### **14.1. Tracking không block UI**

Frontend không chờ analytics API hoàn thành mới điều hướng hoặc mở tour.

Ví dụ:

trackEvent(...)  
router.push(...)

hoặc:

trackEvent(...).catch(() \=\> {})

---

### **14.2. Giới hạn metadata size**

Backend nên giới hạn metadata không quá lớn.

Gợi ý:

metadata JSON không quá 5KB.

---

### **14.3. Index database**

Cần index `event_name`, `target_type + target_id`, `created_at` để dashboard không chậm khi dữ liệu tăng.

---

### **14.4. Dọn dữ liệu cũ**

Chưa cần trong Sprint 14\. Sau này có thể thêm chính sách:

\- Lưu raw events 6-12 tháng  
\- Tổng hợp dữ liệu theo ngày  
\- Xóa event quá cũ nếu cần

---

## **15\. Security Considerations**

### **15.1. Public tracking API**

Vì `/api/analytics/events` là public, cần tránh spam quá mức.

Sprint 14 có thể làm đơn giản:

\- Validate eventName  
\- Validate metadata size  
\- Không xử lý event thiếu eventName

Sau này có thể thêm:

\- Rate limiting  
\- Captcha nếu bị abuse  
\- Chặn event quá nhiều từ cùng session

---

### **15.2. Admin analytics API**

Tất cả API admin analytics cần JWT:

/api/admin/analytics/\*

Không cho public xem dashboard analytics.

---

## **16\. Test Cases**

### **16.1. Backend test cases**

TC-BE-01: Gửi event hợp lệ và lưu thành công.  
TC-BE-02: Gửi eventName không hợp lệ và nhận 400\.  
TC-BE-03: Gửi metadata quá lớn và nhận lỗi validation.  
TC-BE-04: Gọi summary API có token admin và nhận dữ liệu.  
TC-BE-05: Gọi admin analytics API không có token và nhận 401\.  
TC-BE-06: Top destinations trả đúng dữ liệu theo view\_destination.  
TC-BE-07: Events by day nhóm đúng theo ngày.

---

### **16.2. Frontend tracking test cases**

TC-FE-01: Mở destination detail tạo event view\_destination.  
TC-FE-02: Click marker tạo event click\_map\_marker.  
TC-FE-03: Mở tour 360 tạo event open\_virtual\_tour.  
TC-FE-04: Chuyển panorama tạo event navigate\_panorama.  
TC-FE-05: Click hotspot info tạo event click\_hotspot.  
TC-FE-06: Mở route detail tạo event view\_route.  
TC-FE-07: Analytics API lỗi nhưng UI vẫn hoạt động bình thường.

---

### **16.3. Admin dashboard test cases**

TC-ADMIN-01: Admin mở analytics dashboard.  
TC-ADMIN-02: Summary cards hiển thị số liệu.  
TC-ADMIN-03: Top destinations hiển thị đúng thứ tự.  
TC-ADMIN-04: Top tours hiển thị đúng số lượt mở.  
TC-ADMIN-05: Top hotspots hiển thị đúng số lượt click.  
TC-ADMIN-06: Filter 7 ngày/30 ngày hoạt động.  
TC-ADMIN-07: Empty state hiển thị khi chưa có dữ liệu.

---

## **17\. Sprint 14 Implementation Plan**

### **17.1. Mục tiêu Sprint 14**

Bổ sung hệ thống analytics cơ bản để ghi nhận hành vi người dùng trên public website và hiển thị thống kê cho admin.

---

### **17.2. Checklist Database**

\- \[ \] Tạo entity AnalyticsEvent.  
\- \[ \] Tạo migration bảng analytics\_events nếu chưa có.  
\- \[ \] Thêm index event\_name.  
\- \[ \] Thêm index target\_type \+ target\_id.  
\- \[ \] Thêm index created\_at.  
\- \[ \] Thêm index session\_id.

---

### **17.3. Checklist Backend API**

\- \[ \] Tạo DTO TrackEventRequest.  
\- \[ \] Tạo DTO AnalyticsSummaryResponse.  
\- \[ \] Tạo DTO TopDestinationAnalyticsItem.  
\- \[ \] Tạo DTO TopTourAnalyticsItem.  
\- \[ \] Tạo DTO TopHotspotAnalyticsItem.  
\- \[ \] Tạo DTO EventsByDayItem.  
\- \[ \] Tạo AnalyticsService.  
\- \[ \] Tạo POST /api/analytics/events.  
\- \[ \] Tạo GET /api/admin/analytics/summary.  
\- \[ \] Tạo GET /api/admin/analytics/top-destinations.  
\- \[ \] Tạo GET /api/admin/analytics/top-tours.  
\- \[ \] Tạo GET /api/admin/analytics/top-hotspots.  
\- \[ \] Tạo GET /api/admin/analytics/events-by-day.  
\- \[ \] Tạo GET /api/admin/analytics/top-routes nếu Route module đã có.  
\- \[ \] Validate eventName.  
\- \[ \] Validate metadata size.

---

### **17.4. Checklist Frontend Tracking**

\- \[ \] Tạo analytics.service.js.  
\- \[ \] Tạo session.js utility.  
\- \[ \] Tự tạo sessionId nếu chưa có.  
\- \[ \] Track view\_destination.  
\- \[ \] Track click\_map\_marker.  
\- \[ \] Track open\_virtual\_tour.  
\- \[ \] Track view\_panorama.  
\- \[ \] Track click\_hotspot.  
\- \[ \] Track navigate\_panorama.  
\- \[ \] Track view\_route nếu Route module đã có.  
\- \[ \] Đảm bảo tracking lỗi không làm hỏng UI.

---

### **17.5. Checklist Admin Analytics UI**

\- \[ \] Tạo /admin/analytics.  
\- \[ \] Thêm menu Analytics vào sidebar.  
\- \[ \] Tạo summary cards.  
\- \[ \] Tạo date range filter.  
\- \[ \] Tạo events by day chart hoặc table.  
\- \[ \] Tạo top destinations list.  
\- \[ \] Tạo top tours list.  
\- \[ \] Tạo top hotspots list.  
\- \[ \] Tạo empty state.  
\- \[ \] Tạo loading/error states.

---

### **17.6. Checklist Regression**

\- \[ \] Public map vẫn hoạt động.  
\- \[ \] Destination detail vẫn hoạt động.  
\- \[ \] Tour 360 vẫn hoạt động.  
\- \[ \] Hotspot navigation vẫn hoạt động.  
\- \[ \] Route detail vẫn hoạt động nếu đã có Route module.  
\- \[ \] Admin login vẫn hoạt động.  
\- \[ \] Docker build vẫn hoạt động.

---

## **18\. Acceptance Criteria**

Sprint 14 hoàn thành khi:

\- Hệ thống lưu được analytics event vào database.  
\- Frontend tự tạo sessionId ẩn danh.  
\- Mở destination detail ghi nhận view\_destination.  
\- Mở tour 360 ghi nhận open\_virtual\_tour.  
\- Click hotspot ghi nhận click\_hotspot hoặc navigate\_panorama.  
\- Admin xem được analytics dashboard.  
\- Dashboard hiển thị summary cards.  
\- Dashboard hiển thị top destinations, top tours, top hotspots.  
\- Tracking API lỗi không làm gián đoạn trải nghiệm người dùng.  
\- Admin analytics API yêu cầu JWT.  
\- Không thu thập dữ liệu cá nhân nhạy cảm.

---

## **19\. Non-goals**

Sprint 14 không bao gồm:

\- Không tích hợp Google Analytics.  
\- Không làm realtime dashboard.  
\- Không làm recommendation bằng AI.  
\- Không tracking GPS realtime.  
\- Không làm báo cáo PDF.  
\- Không làm data warehouse.  
\- Không làm biểu đồ phức tạp.  
\- Không làm A/B testing.

---

## **20\. Rủi ro và cách kiểm soát**

### **20.1. Rủi ro event quá nhiều làm database phình to**

Cách kiểm soát:

\- Chỉ tracking event quan trọng.  
\- Không tracking mouse move, scroll liên tục.  
\- Có thể thêm retention policy sau.

---

### **20.2. Rủi ro tracking làm chậm UI**

Cách kiểm soát:

\- Frontend gọi tracking dạng fire-and-forget.  
\- Không chờ tracking mới chuyển trang.

---

### **20.3. Rủi ro dữ liệu analytics không chính xác**

Cách kiểm soát:

\- Quy định rõ eventName.  
\- Ghi rõ targetType/targetId.  
\- Test từng luồng public.

---

### **20.4. Rủi ro public API bị spam**

Cách kiểm soát:

\- Validate eventName.  
\- Giới hạn metadata size.  
\- Sau này thêm rate limiting nếu cần.

---

## **21\. Kết luận**

Tính năng **Analytics & Tracking** giúp **Smart Tourism 360 Platform** có khả năng đo lường hành vi người dùng và đánh giá hiệu quả nội dung du lịch. Đây là bước mở rộng quan trọng sau Route / Itinerary vì hệ thống không chỉ hiển thị địa điểm và tuyến tham quan, mà còn biết được nội dung nào đang được quan tâm nhiều.

Sau Sprint 14, admin có thể trả lời các câu hỏi như:

\- Địa điểm nào được xem nhiều?  
\- Tour 360 nào được mở nhiều?  
\- Hotspot nào được click nhiều?  
\- Route nào được quan tâm?  
\- Người dùng tương tác với nội dung nào nhiều nhất?

Dữ liệu analytics này cũng là nền tảng cho các tính năng nâng cao sau này như AI Tour Guide, recommendation, báo cáo du lịch và quản lý nội dung thông minh.

