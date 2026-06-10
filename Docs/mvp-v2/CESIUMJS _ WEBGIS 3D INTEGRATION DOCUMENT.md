# **CESIUMJS / WEBGIS 3D INTEGRATION DOCUMENT**

# **Smart Tourism 360 Platform v2**

## **Tài liệu tích hợp bản đồ 3D và WebGIS 3D bằng CesiumJS**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | CesiumJS / WebGIS 3D Integration Document |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v2.0 |
| Giai đoạn | Sau MVP |
| Sprint đề xuất | Sprint 16 |
| Mục đích | Đặc tả phương án tích hợp CesiumJS để bổ sung chế độ bản đồ 3D/WebGIS 3D cho hệ thống |
| Đối tượng đọc | Frontend developer, backend developer, GIS developer, AI coding agent, project owner |
| Công nghệ liên quan | Vue.js 3, CesiumJS, Leaflet, PostgreSQL/PostGIS, ASP.NET Core Web API |
| Phạm vi | 3D map viewer, destination markers, route polyline, fly-to interaction, popup, liên kết với tour 360 |
| Không thuộc phạm vi | Digital Twin đầy đủ, 3D Tiles phức tạp, đồng bộ GIS quy hoạch lớn, phân tích không gian nâng cao |

---

## **2\. Bối cảnh hiện tại**

Phiên bản MVP của **Smart Tourism 360 Platform** hiện đã có:

\- Public website cho du khách  
\- Bản đồ 2D bằng Leaflet  
\- Marker địa điểm du lịch  
\- Trang chi tiết địa điểm  
\- Tour 360 bằng Photo Sphere Viewer  
\- Admin dashboard  
\- Quản lý địa điểm, media, panorama, hotspot  
\- PostgreSQL/PostGIS lưu tọa độ địa điểm

Bản đồ 2D bằng Leaflet phù hợp cho MVP vì nhẹ, dễ triển khai và đáp ứng tốt nhu cầu hiển thị địa điểm trên bản đồ. Tuy nhiên, để nâng cấp dự án theo hướng công nghệ cao, WebGIS 3D và Digital Tourism, cần bổ sung chế độ bản đồ 3D bằng **CesiumJS**.

Mục tiêu của việc tích hợp CesiumJS không phải là thay thế toàn bộ Leaflet, mà là mở thêm một trải nghiệm khám phá 3D cho du khách.

---

## **3\. Lý do tích hợp CesiumJS**

CesiumJS phù hợp với dự án vì các lý do sau:

1\. Hỗ trợ bản đồ 3D trên nền web.  
2\. Có thể hiển thị địa điểm trên globe hoặc terrain 3D.  
3\. Có thể fly-to đến một địa điểm theo tọa độ.  
4\. Có thể hiển thị marker/billboard, label, polyline.  
5\. Có thể mở rộng sang 3D Tiles, mô hình 3D, terrain, city model trong tương lai.  
6\. Phù hợp với định hướng Smart Tourism, WebGIS 3D và Digital Twin.

Với hệ thống hiện tại, CesiumJS giúp dự án nâng cấp từ:

Smart Tourism 360 Platform

thành:

Smart Tourism 360 \+ WebGIS 3D Platform

---

## **4\. Mục tiêu tích hợp**

Mục tiêu chính của Sprint 16 là tạo một phiên bản 3D Map tối thiểu nhưng hoạt động ổn định.

Các mục tiêu:

1\. Thêm trang bản đồ 3D riêng bằng CesiumJS.  
2\. Load danh sách địa điểm từ API hiện có.  
3\. Hiển thị các địa điểm trên Cesium globe bằng marker/billboard.  
4\. Click marker để xem thông tin nhanh của địa điểm.  
5\. Cho phép fly-to đến địa điểm.  
6\. Cho phép mở trang chi tiết địa điểm từ popup.  
7\. Cho phép mở tour 360 từ popup nếu địa điểm có tour.  
8\. Nếu Route/Itinerary đã có, hiển thị tuyến tham quan bằng polyline 3D.  
9\. Không phá vỡ bản đồ Leaflet 2D hiện tại.

---

## **5\. Nguyên tắc tích hợp**

### **5.1. Không thay thế Leaflet trong giai đoạn đầu**

Leaflet vẫn là bản đồ 2D chính trong hệ thống.

CesiumJS được thêm như một chế độ mới:

/explore      → Bản đồ 2D bằng Leaflet  
/explore-3d   → Bản đồ 3D bằng CesiumJS

Lý do:

\- Leaflet nhẹ và ổn định.  
\- CesiumJS nặng hơn và phức tạp hơn.  
\- Tách riêng giúp giảm rủi ro.  
\- Người dùng có thể chọn chế độ phù hợp.  
\- Không làm hỏng chức năng bản đồ 2D đã ổn định.

---

### **5.2. Dùng lại API hiện có càng nhiều càng tốt**

CesiumJS không cần API hoàn toàn mới ở giai đoạn đầu.

Có thể dùng lại:

GET /api/destinations/map  
GET /api/destinations/{id}  
GET /api/routes/{id}

Nếu cần, có thể bổ sung API nhẹ:

GET /api/destinations/geo  
GET /api/routes/{id}/map

Nhưng chỉ thêm nếu dữ liệu hiện tại chưa đủ.

---

### **5.3. Bắt đầu từ 3D markers, chưa làm Digital Twin phức tạp**

Sprint 16 chỉ cần:

\- Globe/terrain  
\- Marker địa điểm  
\- Label  
\- Popup  
\- FlyTo  
\- Polyline route

Chưa làm:

\- 3D Tiles city model  
\- Layer GIS quy hoạch  
\- Mô hình công trình chi tiết  
\- Phân tích không gian  
\- Đồng bộ dữ liệu GIS lớn

---

### **5.4. Ưu tiên ổn định và demo được**

Mục tiêu của Sprint 16 là tạo được một demo 3D rõ ràng:

Du khách mở bản đồ 3D  
→ thấy các điểm du lịch trên globe  
→ click một điểm  
→ xem thông tin  
→ fly-to điểm đó  
→ mở chi tiết hoặc tour 360

---

## **6\. Phạm vi chức năng Sprint 16**

### **6.1. Trong phạm vi**

\- Cài đặt CesiumJS vào frontend.  
\- Tạo route /explore-3d.  
\- Tạo CesiumMapPage.  
\- Tạo CesiumViewer component.  
\- Load destination markers từ API.  
\- Hiển thị marker/billboard.  
\- Hiển thị label tên địa điểm.  
\- Click marker mở popup.  
\- FlyTo destination.  
\- Nút chuyển về bản đồ 2D.  
\- Nút mở destination detail.  
\- Nút mở tour 360 nếu có.  
\- Hiển thị route polyline nếu Route module đã có.

---

### **6.2. Ngoài phạm vi**

\- Không thay thế Leaflet.  
\- Không tích hợp ArcGIS trong sprint này.  
\- Không load 3D Tiles đô thị.  
\- Không upload model 3D.  
\- Không làm digital twin đầy đủ.  
\- Không làm routing đường bộ thực tế.  
\- Không phân tích khoảng cách 3D.  
\- Không làm offline map.

---

## **7\. Kiến trúc tích hợp**

### **7.1. Kiến trúc tổng quan**

Frontend Vue.js  
├── Leaflet 2D Map  
│   └── /explore  
│  
├── CesiumJS 3D Map  
│   └── /explore-3d  
│  
├── Destination Detail  
│   └── /destinations/:id  
│  
└── Tour 360 Viewer  
    └── /destinations/:id/tour

Backend ASP.NET Core API  
├── Destination API  
├── Route API  
└── Media API

Database PostgreSQL/PostGIS  
├── destinations.latitude  
├── destinations.longitude  
└── destinations.location

---

### **7.2. Luồng dữ liệu destination marker**

CesiumMapPage mounted  
→ gọi GET /api/destinations/map  
→ nhận danh sách destinations  
→ chuyển latitude/longitude sang Cesium Cartesian3  
→ tạo billboard/point entity  
→ render marker trên globe

---

### **7.3. Luồng click marker**

User click marker trên Cesium  
→ xác định destination entity  
→ mở popup/side panel  
→ hiển thị thông tin ngắn  
→ user chọn:  
   \- Xem chi tiết  
   \- Tham quan 360  
   \- FlyTo

---

### **7.4. Luồng hiển thị route 3D**

User mở route trong 3D mode  
→ gọi GET /api/routes/{id}  
→ lấy danh sách destinations theo displayOrder  
→ chuyển tọa độ thành Cartesian3  
→ vẽ polyline nối các điểm  
→ hiển thị marker đánh số

---

## **8\. Frontend Route Design**

### **8.1. Route mới**

Thêm route frontend:

/explore-3d

Nếu muốn mở route cụ thể trên bản đồ 3D:

/explore-3d?routeId={routeId}

Nếu muốn focus vào một địa điểm:

/explore-3d?destinationId={destinationId}

---

### **8.2. Navigation giữa 2D và 3D**

Ở trang `/explore`, thêm nút:

Chế độ 3D

Ở trang `/explore-3d`, thêm nút:

Chế độ 2D

Gợi ý UI:

Toggle:  
\[ 2D Map \] \[ 3D Map \]

---

## **9\. Frontend Component Design**

### **9.1. Components cần tạo**

CesiumMapPage.vue  
CesiumViewer.vue  
CesiumDestinationLayer.vue  
CesiumRouteLayer.vue  
CesiumMapToolbar.vue  
CesiumDestinationPopup.vue  
CesiumLoadingOverlay.vue  
CesiumErrorState.vue

---

### **9.2. `CesiumMapPage.vue`**

Vai trò:

\- Trang chính của bản đồ 3D.  
\- Gọi API lấy destinations.  
\- Quản lý state loading/error.  
\- Truyền data vào CesiumViewer.  
\- Xử lý query params destinationId/routeId.

State chính:

destinations  
selectedDestination  
routes  
selectedRoute  
isLoading  
error

---

### **9.3. `CesiumViewer.vue`**

Vai trò:

\- Khởi tạo Cesium Viewer.  
\- Tạo globe/scene.  
\- Nhận danh sách destination.  
\- Render entities.  
\- Xử lý click entity.  
\- Expose flyTo method.

Props gợi ý:

destinations  
selectedDestinationId  
routeData

Events:

destination-click  
viewer-ready  
error

---

### **9.4. `CesiumDestinationLayer.vue`**

Vai trò:

\- Render destination markers.  
\- Mỗi destination là một Cesium entity.  
\- Dùng billboard hoặc point \+ label.

Mỗi marker cần có:

destinationId  
name  
category  
latitude  
longitude  
hasVirtualTour  
coverImageUrl

---

### **9.5. `CesiumDestinationPopup.vue`**

Hiển thị:

\- Cover image  
\- Tên địa điểm  
\- Category badge  
\- Mô tả ngắn  
\- Button FlyTo  
\- Button Xem chi tiết  
\- Button Tour 360 nếu có

---

### **9.6. `CesiumRouteLayer.vue`**

Vai trò:

\- Render route polyline.  
\- Render marker đánh số theo thứ tự route.  
\- Hỗ trợ focus vào route.

---

## **10\. CesiumJS Entity Design**

### **10.1. Destination marker entity**

Mỗi destination có thể render như:

Entity:  
\- id: destination.id  
\- position: Cesium.Cartesian3.fromDegrees(longitude, latitude, height)  
\- billboard hoặc point  
\- label: destination.name  
\- properties: destination data

Chiều cao marker:

height \= 50 hoặc 100 mét

Lý do: giúp marker nổi trên mặt đất, dễ nhìn hơn.

---

### **10.2. Marker style**

Marker có thể phân biệt theo category:

Tâm linh → vàng cam  
Sinh thái → xanh lá  
Văn hóa → tím  
Ẩm thực → đỏ/cam  
Lưu trú → xanh dương

Nếu dùng billboard icon, nên dùng icon đơn giản.

Nếu chưa có icon custom, có thể dùng point entity:

point:  
\- pixelSize: 12  
\- outlineWidth: 2  
\- color theo category

---

### **10.3. Label style**

Label nên:

\- Hiển thị khi zoom gần.  
\- Hoặc luôn hiển thị với font nhỏ.  
\- Có outline để đọc được trên nền map.

Gợi ý:

font: 14px sans-serif  
fillColor: white  
outlineColor: black  
outlineWidth: 2  
style: fill and outline

---

### **10.4. Route polyline**

Polyline nối các destination:

positions \= destinations sorted by displayOrder  
material \= màu primary hoặc theme route  
width \= 3 đến 5  
clampToGround \= true nếu phù hợp

Ghi chú:

Polyline chỉ minh họa thứ tự tham quan, không phải chỉ đường giao thông thực tế.

---

## **11\. Dữ liệu cần từ API**

### **11.1. Destination map item**

API hiện tại `/api/destinations/map` nên trả tối thiểu:

{  
  "id": "destination-001",  
  "name": "Nhà Cổ Bình Thủy",  
  "slug": "nha-co-binh-thuy",  
  "shortDescription": "Di sản kiến trúc cổ tại Cần Thơ.",  
  "latitude": 10.068,  
  "longitude": 105.732,  
  "coverImageUrl": "https://storage.example.com/nha-co.jpg",  
  "hasVirtualTour": true,  
  "category": {  
    "id": "cat-001",  
    "name": "Văn hóa \- lịch sử",  
    "icon": "landmark",  
    "color": "\#7C3AED"  
  }  
}

---

### **11.2. Route map data**

Nếu dùng route:

{  
  "id": "route-001",  
  "title": "Tuyến khám phá văn hóa Cần Thơ",  
  "destinations": \[  
    {  
      "id": "destination-001",  
      "name": "Nhà Cổ Bình Thủy",  
      "latitude": 10.068,  
      "longitude": 105.732,  
      "displayOrder": 1,  
      "hasVirtualTour": true  
    },  
    {  
      "id": "destination-002",  
      "name": "Bến Ninh Kiều",  
      "latitude": 10.0328,  
      "longitude": 105.7889,  
      "displayOrder": 2,  
      "hasVirtualTour": false  
    }  
  \]  
}

---

## **12\. Backend API Impact**

### **12.1. Có thể dùng lại API hiện tại**

Nếu `/api/destinations/map` đã đủ dữ liệu, không cần thêm API mới.

---

### **12.2. API gợi ý nếu cần tối ưu**

#### **API lấy dữ liệu địa điểm cho WebGIS**

GET /api/destinations/geo

Response giống `/api/destinations/map`, nhưng có thể tối ưu cho GIS.

---

#### **API lấy route map data**

GET /api/routes/{id}/map

Response chỉ trả dữ liệu cần cho bản đồ:

{  
  "id": "route-001",  
  "title": "Tuyến khám phá văn hóa Cần Thơ",  
  "theme": "culture",  
  "destinations": \[  
    {  
      "id": "destination-001",  
      "name": "Nhà Cổ Bình Thủy",  
      "latitude": 10.068,  
      "longitude": 105.732,  
      "displayOrder": 1  
    }  
  \]  
}

---

## **13\. UI/UX Specification cho 3D Map**

### **13.1. Layout desktop**

Top header  
Left floating panel:  
\- Search destination  
\- Category filter  
\- Route selector nếu có

Main:  
\- Cesium 3D map full screen

Right/bottom popup:  
\- Destination info khi click marker

---

### **13.2. Layout mobile**

Mobile cần đơn giản:

\- Cesium viewer full screen  
\- Search/filter dạng bottom sheet  
\- Popup địa điểm dạng bottom card  
\- Nút chuyển 2D/3D rõ ràng

Lưu ý:

CesiumJS trên mobile có thể nặng.  
Cần giữ UI tối giản và không render quá nhiều entity.

---

### **13.3. Toolbar**

Toolbar nên có:

\- Nút quay về vị trí khu vực  
\- Toggle 2D/3D map  
\- Nút bật/tắt label  
\- Nút bật/tắt route layer  
\- Nút reset camera

---

### **13.4. Destination popup**

Popup hiển thị:

\- Ảnh đại diện  
\- Tên địa điểm  
\- Danh mục  
\- Mô tả ngắn  
\- Có tour 360 hay không  
\- Nút Xem chi tiết  
\- Nút Tham quan 360  
\- Nút FlyTo

---

## **14\. Camera & FlyTo Behavior**

### **14.1. Camera mặc định**

Khi mở `/explore-3d`, camera nên bay đến region mặc định.

Ví dụ với Cần Thơ Demo:

longitude: 105.7469  
latitude: 10.0452  
height: 30000 \- 60000 meters

---

### **14.2. FlyTo destination**

Khi user chọn địa điểm:

Camera flyTo:  
\- longitude  
\- latitude  
\- height: 1500 \- 3000 meters  
\- pitch: \-45 degrees

Mục tiêu:

Nhìn rõ khu vực xung quanh địa điểm.

---

### **14.3. FlyTo route**

Khi user chọn route:

\- Tính bounding region của tất cả destinations trong route.  
\- Camera bay đến vị trí nhìn bao quát toàn tuyến.

Sprint 16 có thể làm đơn giản:

FlyTo điểm đầu tiên của route  
hoặc dùng viewer.zoomTo(routeEntities)

---

## **15\. Performance Considerations**

### **15.1. Không render quá nhiều entity**

Sprint 16 dữ liệu nhỏ nên không vấn đề. Nhưng về sau:

\- Nếu có hàng nghìn điểm, cần clustering hoặc lazy loading theo vùng nhìn.  
\- Nếu có nhiều layer 3D, cần bật/tắt layer.

---

### **15.2. Lazy load Cesium page**

Không nên import CesiumJS vào bundle chính nếu có thể.

Nên lazy load route:

/explore-3d

Lý do:

CesiumJS khá nặng.  
Người dùng không mở 3D map thì không nên tải Cesium.

---

### **15.3. Tắt bớt widget không cần thiết**

Cesium Viewer có nhiều widget mặc định. Nên tắt bớt để UI sạch:

\- animation: false  
\- timeline: false  
\- baseLayerPicker: có thể false nếu chưa cần  
\- geocoder: false nếu chưa cần  
\- homeButton: false nếu dùng toolbar riêng  
\- sceneModePicker: false nếu chỉ dùng 3D

---

### **15.4. Mobile performance**

Trên mobile:

\- Giảm số label luôn hiển thị.  
\- Chỉ hiển thị label khi marker được chọn.  
\- Không dùng hiệu ứng phức tạp.  
\- Không load route polyline quá nhiều.

---

## **16\. Security & Token Considerations**

### **16.1. Cesium Ion Token**

Nếu dùng Cesium Ion asset hoặc basemap cần token, token frontend có thể bị lộ vì chạy trên browser.

Nguyên tắc:

\- Không dùng token có quyền quản trị.  
\- Chỉ dùng public token giới hạn quyền.  
\- Không commit token production thật vào GitHub.  
\- Dùng biến môi trường VITE\_CESIUM\_ION\_TOKEN.  
\- Cung cấp .env.example.

Ví dụ:

VITE\_CESIUM\_ION\_TOKEN=your\_cesium\_ion\_token\_here

---

### **16.2. Không lộ API secret**

Frontend chỉ nên chứa public token cần thiết. Các secret backend hoặc storage key không được đưa vào frontend.

---

## **17\. Environment Configuration**

### **17.1. Frontend `.env`**

VITE\_API\_BASE\_URL=http://localhost:5074  
VITE\_CESIUM\_ION\_TOKEN=your\_cesium\_ion\_token\_here

---

### **17.2. Docker build args**

Nếu build Docker full mode, cần truyền biến môi trường tương ứng:

VITE\_API\_BASE\_URL  
VITE\_CESIUM\_ION\_TOKEN

Ghi chú:

Vite env được inject tại build time.  
Nếu đổi token/API URL, cần build lại frontend.

---

## **18\. Analytics Integration**

Nếu Analytics module đã có, nên tracking thêm:

view\_3d\_map  
click\_3d\_marker  
fly\_to\_destination  
open\_destination\_from\_3d\_map  
open\_virtual\_tour\_from\_3d\_map  
view\_3d\_route

Ví dụ:

{  
  "eventName": "click\_3d\_marker",  
  "targetType": "destination",  
  "targetId": "destination-001",  
  "metadata": {  
    "source": "explore\_3d"  
  }  
}

Sprint 16 có thể thêm nếu Analytics đã hoàn thành. Nếu chưa, để sau.

---

## **19\. Test Cases**

### **19.1. Frontend test cases**

TC-FE-01: Mở /explore-3d thành công.  
TC-FE-02: Cesium Viewer hiển thị globe.  
TC-FE-03: Load destinations từ API.  
TC-FE-04: Marker hiển thị đúng vị trí latitude/longitude.  
TC-FE-05: Click marker mở popup đúng destination.  
TC-FE-06: Click “Xem chi tiết” chuyển đến trang destination detail.  
TC-FE-07: Click “Tham quan 360” mở tour nếu destination có tour.  
TC-FE-08: Nút FlyTo đưa camera đến đúng địa điểm.  
TC-FE-09: Toggle từ 3D sang 2D hoạt động.  
TC-FE-10: Nếu routeId có trong query, route polyline hiển thị.

---

### **19.2. Backend test cases**

TC-BE-01: API /api/destinations/map trả đủ latitude/longitude.  
TC-BE-02: Destination không có tọa độ không làm crash frontend.  
TC-BE-03: API route detail trả destinations đúng displayOrder.  
TC-BE-04: Route map data trả đúng danh sách điểm.

---

### **19.3. Regression test cases**

TC-REG-01: /explore Leaflet 2D vẫn hoạt động.  
TC-REG-02: Public destination detail vẫn hoạt động.  
TC-REG-03: Tour 360 vẫn hoạt động.  
TC-REG-04: Admin dashboard không bị ảnh hưởng.  
TC-REG-05: Docker build frontend vẫn thành công.

---

## **20\. Sprint 16 Implementation Plan**

### **20.1. Mục tiêu Sprint 16**

Bổ sung chế độ bản đồ 3D bằng CesiumJS cho Smart Tourism 360 Platform, cho phép du khách khám phá các địa điểm du lịch trên không gian 3D và mở chi tiết/tour 360 từ marker 3D.

---

### **20.2. Checklist Frontend Setup**

\- \[ \] Cài CesiumJS.  
\- \[ \] Cấu hình Vite để load Cesium assets nếu cần.  
\- \[ \] Thêm biến môi trường VITE\_CESIUM\_ION\_TOKEN.  
\- \[ \] Cập nhật .env.example.  
\- \[ \] Thêm route /explore-3d.  
\- \[ \] Lazy load CesiumMapPage nếu có thể.

---

### **20.3. Checklist Cesium Components**

\- \[ \] Tạo CesiumMapPage.vue.  
\- \[ \] Tạo CesiumViewer.vue.  
\- \[ \] Tạo CesiumDestinationPopup.vue.  
\- \[ \] Tạo CesiumMapToolbar.vue.  
\- \[ \] Tạo CesiumRouteLayer.vue nếu Route module đã có.  
\- \[ \] Tạo loading/error state cho 3D map.

---

### **20.4. Checklist Destination Markers**

\- \[ \] Gọi API /api/destinations/map.  
\- \[ \] Render destination markers.  
\- \[ \] Marker có màu theo category.  
\- \[ \] Hiển thị label tên địa điểm.  
\- \[ \] Click marker mở popup.  
\- \[ \] Popup hiển thị thông tin địa điểm.  
\- \[ \] Nút Xem chi tiết hoạt động.  
\- \[ \] Nút Tour 360 hoạt động nếu có tour.

---

### **20.5. Checklist Camera**

\- \[ \] Camera mặc định bay đến region center.  
\- \[ \] FlyTo destination hoạt động.  
\- \[ \] Reset camera hoạt động.  
\- \[ \] Nếu có destinationId query, tự flyTo destination đó.

---

### **20.6. Checklist Route 3D**

\- \[ \] Nếu có routeId query, gọi API route detail/map.  
\- \[ \] Sắp xếp destinations theo displayOrder.  
\- \[ \] Vẽ polyline nối các điểm.  
\- \[ \] Marker route có số thứ tự.  
\- \[ \] Có ghi chú polyline chỉ là minh họa thứ tự, không phải đường đi thật.

---

### **20.7. Checklist UI/UX**

\- \[ \] Có nút chuyển 2D/3D.  
\- \[ \] Có toolbar gọn.  
\- \[ \] Có popup destination đẹp.  
\- \[ \] Có loading overlay.  
\- \[ \] Có error state nếu Cesium load lỗi.  
\- \[ \] Giao diện không vỡ trên desktop.  
\- \[ \] Mobile dùng được ở mức cơ bản.

---

### **20.8. Checklist Security/Config**

\- \[ \] Không commit Cesium token thật.  
\- \[ \] Thêm .env.example.  
\- \[ \] README hướng dẫn cấu hình token nếu cần.  
\- \[ \] Docker frontend nhận VITE\_CESIUM\_ION\_TOKEN nếu build full mode.

---

### **20.9. Checklist Regression**

\- \[ \] /explore 2D vẫn hoạt động.  
\- \[ \] /destinations/:id vẫn hoạt động.  
\- \[ \] /destinations/:id/tour vẫn hoạt động.  
\- \[ \] Admin dashboard vẫn hoạt động.  
\- \[ \] npm run build thành công.  
\- \[ \] docker compose up \-d \--build thành công.

---

## **21\. Acceptance Criteria**

Sprint 16 được xem là hoàn thành nếu:

\- Có trang /explore-3d.  
\- Cesium globe hiển thị thành công.  
\- Destination markers hiển thị đúng tọa độ.  
\- Click marker mở popup địa điểm.  
\- Popup có nút xem chi tiết và tour 360\.  
\- FlyTo destination hoạt động.  
\- Có thể chuyển giữa 2D map và 3D map.  
\- Nếu route module đã có, có thể hiển thị route polyline 3D.  
\- Không làm hỏng bản đồ Leaflet 2D.  
\- Không commit token thật vào GitHub.  
\- Frontend build thành công.

---

## **22\. Non-goals**

Sprint 16 không bao gồm:

\- Không thay Leaflet bằng CesiumJS.  
\- Không làm Digital Twin đầy đủ.  
\- Không load 3D Tiles đô thị.  
\- Không đồng bộ ArcGIS services.  
\- Không làm routing đường bộ thật.  
\- Không upload model 3D.  
\- Không phân tích không gian nâng cao.  
\- Không bắt buộc mobile performance hoàn hảo.

---

## **23\. Rủi ro và cách kiểm soát**

### **23.1. CesiumJS làm bundle frontend nặng**

Cách kiểm soát:

\- Lazy load /explore-3d.  
\- Không import Cesium vào App root.  
\- Chỉ load Cesium khi user mở trang 3D.

---

### **23.2. Token Cesium bị lộ**

Cách kiểm soát:

\- Không commit token thật.  
\- Dùng .env.local.  
\- Dùng token giới hạn quyền.  
\- Ghi hướng dẫn trong README.

---

### **23.3. Marker không đúng vị trí**

Cách kiểm soát:

\- Kiểm tra thứ tự longitude/latitude.  
\- Cesium dùng fromDegrees(longitude, latitude), không phải latitude trước.

---

### **23.4. 3D map làm vỡ UX mobile**

Cách kiểm soát:

\- Giữ UI mobile tối giản.  
\- Popup dùng bottom sheet.  
\- Không render quá nhiều label trên mobile.

---

### **23.5. Tích hợp 3D làm ảnh hưởng bản đồ 2D**

Cách kiểm soát:

\- Tách route /explore-3d.  
\- Không sửa logic Leaflet trừ khi cần thêm link chuyển 2D/3D.

---

## **24\. Roadmap mở rộng sau Sprint 16**

Sau khi bản đồ 3D cơ bản hoạt động, có thể mở rộng:

1\. Hiển thị route 3D đẹp hơn.  
2\. Thêm layer địa hình/terrain.  
3\. Thêm 3D model .glb/.gltf cho địa điểm.  
4\. Thêm 3D Tiles cho khu vực/công trình.  
5\. Tích hợp dữ liệu ArcGIS/GeoJSON.  
6\. Thêm layer du lịch: lưu trú, ẩm thực, giao thông.  
7\. Thêm phân tích khoảng cách và vùng ảnh hưởng.  
8\. Kết nối AI Tour Guide với bản đồ 3D.

---

## **25\. Kết luận**

Tích hợp **CesiumJS / WebGIS 3D** là bước nâng cấp công nghệ quan trọng của **Smart Tourism 360 Platform v2**. Trong giai đoạn đầu, CesiumJS không thay thế Leaflet mà hoạt động như một chế độ khám phá 3D riêng, giúp du khách xem các địa điểm du lịch trên không gian 3D và kết nối trực tiếp đến trang chi tiết hoặc tour 360\.

Cách tiếp cận này giúp dự án phát triển an toàn:

Leaflet 2D giữ vai trò bản đồ nhẹ và ổn định.  
CesiumJS 3D tạo điểm nhấn công nghệ và mở đường cho WebGIS 3D/Digital Twin.

Sau Sprint 16, dự án sẽ có nền tảng để mở rộng sang 3D Tiles, model 3D, terrain, dữ liệu GIS nâng cao và các trải nghiệm du lịch số thông minh hơn.

