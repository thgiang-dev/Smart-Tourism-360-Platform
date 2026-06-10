# **3D MODEL FEATURE SPECIFICATION**

# **Smart Tourism 360 Platform v2**

## **Đặc tả tính năng quản lý và hiển thị mô hình 3D**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | 3D Model Feature Specification |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v2.0 |
| Giai đoạn | Sau MVP |
| Sprint đề xuất | Sprint 17 |
| Mục đích | Đặc tả tính năng upload, quản lý, gắn liên kết và hiển thị mô hình 3D trong hệ thống |
| Đối tượng đọc | Backend developer, frontend developer, GIS/3D developer, UI/UX designer, AI coding agent, project owner |
| Công nghệ liên quan | Vue.js 3, ASP.NET Core Web API, PostgreSQL, MinIO/S3, `<model-viewer>`, Three.js, CesiumJS |
| Định dạng model ưu tiên | `.glb`, `.gltf` |
| Phạm vi chính | Upload model 3D, quản lý metadata, gắn model với địa điểm/hotspot, xem model trên public website |
| Không thuộc phạm vi | 3D Tiles phức tạp, chỉnh sửa model trực tiếp trên web, animation editor, AR nâng cao, scan 3D tự động |

---

## **2\. Bối cảnh**

Phiên bản MVP của **Smart Tourism 360 Platform** đã hỗ trợ:

\- Quản lý địa điểm du lịch  
\- Upload media  
\- Tour 360  
\- Panorama  
\- Hotspot navigation  
\- Bản đồ 2D  
\- Bản đồ 3D CesiumJS theo roadmap

Sau khi hệ thống có khả năng tham quan 360 và bản đồ 3D, bước mở rộng tự nhiên tiếp theo là hỗ trợ **mô hình 3D**. Tính năng này giúp hệ thống không chỉ hiển thị hình ảnh và không gian 360, mà còn có thể số hóa các đối tượng cụ thể như:

\- Công trình kiến trúc  
\- Hiện vật bảo tàng  
\- Sản phẩm làng nghề  
\- Tượng, cổng, chùa, đình, nhà cổ  
\- Mô hình địa điểm du lịch thu nhỏ

Ví dụ:

Địa điểm: Nhà Cổ Bình Thủy  
→ Model 3D: Mô hình kiến trúc nhà cổ

Địa điểm: Làng nghề truyền thống  
→ Model 3D: Sản phẩm thủ công đặc trưng

Địa điểm: Chùa Khmer  
→ Model 3D: Tượng, cổng chùa, chi tiết trang trí

---

## **3\. Mục tiêu tính năng**

Tính năng 3D Model nhằm đạt các mục tiêu sau:

1\. Cho phép admin upload file mô hình 3D.  
2\. Cho phép admin lưu metadata của model như tên, mô tả, định dạng, thumbnail.  
3\. Cho phép admin gắn model 3D với địa điểm, panorama hoặc hotspot.  
4\. Cho phép du khách xem mô hình 3D trong trang chi tiết địa điểm.  
5\. Cho phép du khách mở model 3D từ hotspot trong tour 360\.  
6\. Chuẩn bị khả năng hiển thị model trên bản đồ 3D CesiumJS trong giai đoạn sau.  
7\. Quản lý model 3D như một loại media mở rộng, không lưu file trực tiếp trong database.  
8\. Giữ tính năng đủ nhẹ, dễ demo và không làm phức tạp hệ thống quá mức.

---

## **4\. Giá trị của tính năng**

### **4.1. Với du khách**

Tính năng model 3D giúp du khách:

\- Xem hiện vật/công trình ở nhiều góc độ.  
\- Phóng to, xoay, quan sát chi tiết.  
\- Hiểu rõ hơn về các đối tượng văn hóa, lịch sử, kiến trúc.  
\- Có trải nghiệm tương tác sâu hơn so với ảnh 2D.

---

### **4.2. Với admin / content manager**

Admin có thể:

\- Upload model 3D vào hệ thống.  
\- Gắn model vào địa điểm phù hợp.  
\- Gắn model vào hotspot cụ thể trong tour 360\.  
\- Quản lý model giống như quản lý ảnh/video/audio.  
\- Tạo thêm nội dung số hóa cho địa phương.

---

### **4.3. Với cơ quan quản lý du lịch**

Tính năng này hỗ trợ:

\- Số hóa di sản và hiện vật.  
\- Quảng bá công trình văn hóa.  
\- Tạo kho tư liệu số 3D.  
\- Kết hợp với tour 360 và bản đồ 3D để xây dựng trải nghiệm du lịch số chuyên sâu.

---

## **5\. Phạm vi tính năng**

### **5.1. Trong Sprint 17**

Sprint 17 tập trung vào bản 3D Model MVP:

\- Hỗ trợ upload file .glb/.gltf.  
\- Lưu metadata model vào database.  
\- Quản lý model trong admin dashboard.  
\- Gắn model với destination hoặc hotspot.  
\- Hiển thị model trong trang chi tiết địa điểm.  
\- Hiển thị model từ hotspot loại model3d trong tour 360\.  
\- Dùng \<model-viewer\> để render model 3D.

---

### **5.2. Ngoài phạm vi Sprint 17**

Không làm trong Sprint 17:

\- Không chỉnh sửa model trực tiếp trên web.  
\- Không tạo model 3D tự động từ ảnh/video.  
\- Không làm AR nâng cao.  
\- Không làm 3D Tiles.  
\- Không tối ưu LOD cho model lớn.  
\- Không làm animation editor.  
\- Không làm material editor.  
\- Không tích hợp Blender pipeline nâng cao.  
\- Không bắt buộc đặt model trực tiếp lên Cesium terrain trong sprint này.

---

## **6\. Định dạng model 3D**

### **6.1. Định dạng ưu tiên**

Hệ thống ưu tiên hỗ trợ:

.glb  
.gltf

Trong đó:

.glb:  
\- Đóng gói model, texture, material trong một file duy nhất.  
\- Dễ upload.  
\- Dễ quản lý.  
\- Phù hợp cho web.

.gltf:  
\- Có thể đi kèm file .bin và texture rời.  
\- Phức tạp hơn khi upload.  
\- Nên hỗ trợ sau hoặc yêu cầu nén thành .zip nếu cần.

Khuyến nghị cho Sprint 17:

Ưu tiên .glb trước.  
.gltf có thể hỗ trợ ở mức cơ bản hoặc để sau.

---

### **6.2. Dung lượng file**

Gợi ý giới hạn:

| Loại file | Dung lượng tối đa đề xuất |
| ----- | ----- |
| `.glb` | 50 MB |
| `.gltf` | 20 MB nếu file đơn |
| Thumbnail | 5 MB |

Với MVP mở rộng, nên khuyến nghị model:

\- Dưới 20 MB để tải nhanh.  
\- Đã được tối ưu texture.  
\- Không quá nhiều polygon.

---

### **6.3. Metadata kỹ thuật nên lưu**

Với model 3D, nên lưu thêm:

\- format: glb/gltf  
\- fileSize  
\- polygonCount nếu có  
\- boundingBox nếu sau này cần  
\- thumbnailUrl  
\- source/credit nếu dùng model mẫu

Sprint 17 không bắt buộc tính tự động polygon count, có thể nhập tay hoặc để null.

---

## **7\. Database Design**

Có 2 hướng thiết kế:

Hướng A: Chỉ dùng media\_files với media\_type \= model3d.  
Hướng B: Dùng media\_files để lưu file, thêm bảng models\_3d để lưu metadata nghiệp vụ.

Khuyến nghị dùng **Hướng B**.

Lý do:

\- media\_files chỉ quản lý file.  
\- models\_3d quản lý ý nghĩa nghiệp vụ của model.  
\- Một model có title, description, targetType, targetId, status.  
\- Dễ mở rộng sau này sang CesiumJS/AR.

---

## **8\. Bảng `models_3d`**

### **8.1. Mục đích**

Bảng `models_3d` lưu metadata nghiệp vụ của mô hình 3D.

Model file thật vẫn lưu trong MinIO/S3 và metadata file vẫn nằm trong `media_files`.

---

### **8.2. Cấu trúc bảng**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| title | varchar(250) | Có | Tên model |
| description | text | Không | Mô tả model |
| media\_id | uuid | Có | File model trong media\_files |
| thumbnail\_id | uuid | Không | Ảnh đại diện model |
| target\_type | varchar(50) | Có | destination, panorama, hotspot |
| target\_id | uuid | Có | ID đối tượng được gắn model |
| format | varchar(20) | Không | glb/gltf |
| polygon\_count | integer | Không | Số polygon nếu có |
| status | varchar(30) | Có | draft/published/archived |
| created\_by | uuid | Không | Người tạo |
| updated\_by | uuid | Không | Người cập nhật |
| created\_at | timestamp | Có | Ngày tạo |
| updated\_at | timestamp | Có | Ngày cập nhật |

---

### **8.3. DBML**

Table models\_3d {  
  id uuid \[primary key\]  
  title varchar(250) \[not null\]  
  description text  
  media\_id uuid \[not null\]  
  thumbnail\_id uuid  
  target\_type varchar(50) \[not null, note: 'destination, panorama, hotspot'\]  
  target\_id uuid \[not null\]  
  format varchar(20) \[note: 'glb, gltf'\]  
  polygon\_count integer  
  status varchar(30) \[not null, note: 'draft, published, archived'\]  
  created\_by uuid  
  updated\_by uuid  
  created\_at timestamp  
  updated\_at timestamp  
}

Ref: models\_3d.media\_id \> media\_files.id  
Ref: models\_3d.thumbnail\_id \> media\_files.id  
Ref: models\_3d.created\_by \> users.id  
Ref: models\_3d.updated\_by \> users.id

---

### **8.4. Quan hệ với media\_files**

media\_files  
  └── models\_3d.media\_id

`media_files.media_type` cần hỗ trợ:

model3d

Ví dụ media file:

file\_name: nha-co-binh-thuy.glb  
media\_type: model3d  
mime\_type: model/gltf-binary  
extension: .glb  
storage\_provider: minio  
file\_url: https://storage.example.com/models/nha-co-binh-thuy.glb

---

## **9\. API Specification**

## **9.1. Public 3D Model API**

### **Lấy model theo destination**

GET /api/destinations/{destinationId}/models-3d

Response:

{  
  "success": true,  
  "data": \[  
    {  
      "id": "model-001",  
      "title": "Mô hình Nhà Cổ Bình Thủy",  
      "description": "Mô hình 3D minh họa kiến trúc nhà cổ.",  
      "modelUrl": "https://storage.example.com/models/nha-co-binh-thuy.glb",  
      "thumbnailUrl": "https://storage.example.com/models/nha-co-thumb.jpg",  
      "format": "glb",  
      "targetType": "destination",  
      "targetId": "destination-001",  
      "status": "published"  
    }  
  \]  
}

---

### **Lấy chi tiết model**

GET /api/models-3d/{id}

Response:

{  
  "success": true,  
  "data": {  
    "id": "model-001",  
    "title": "Mô hình Nhà Cổ Bình Thủy",  
    "description": "Mô hình 3D minh họa kiến trúc nhà cổ.",  
    "modelUrl": "https://storage.example.com/models/nha-co-binh-thuy.glb",  
    "thumbnailUrl": "https://storage.example.com/models/nha-co-thumb.jpg",  
    "format": "glb",  
    "polygonCount": 25000,  
    "targetType": "destination",  
    "targetId": "destination-001"  
  }  
}

---

## **9.2. Admin 3D Model API**

### **Lấy danh sách model**

GET /api/admin/models-3d

Query params:

| Tên | Bắt buộc | Mô tả |
| ----- | ----- | ----- |
| keyword | Không | Tìm theo title |
| targetType | Không | destination, panorama, hotspot |
| targetId | Không | ID đối tượng |
| status | Không | draft, published, archived |
| page | Không | Trang |
| pageSize | Không | Số dòng mỗi trang |

Response:

{  
  "success": true,  
  "data": {  
    "items": \[  
      {  
        "id": "model-001",  
        "title": "Mô hình Nhà Cổ Bình Thủy",  
        "format": "glb",  
        "thumbnailUrl": "https://storage.example.com/models/nha-co-thumb.jpg",  
        "targetType": "destination",  
        "targetName": "Nhà Cổ Bình Thủy",  
        "status": "published",  
        "createdAt": "2026-06-10T10:00:00Z"  
      }  
    \],  
    "page": 1,  
    "pageSize": 10,  
    "totalItems": 1,  
    "totalPages": 1  
  }  
}

---

### **Tạo metadata model 3D**

POST /api/admin/models-3d

Request body:

{  
  "title": "Mô hình Nhà Cổ Bình Thủy",  
  "description": "Mô hình 3D minh họa kiến trúc nhà cổ.",  
  "mediaId": "media-model-001",  
  "thumbnailId": "media-thumb-001",  
  "targetType": "destination",  
  "targetId": "destination-001",  
  "format": "glb",  
  "polygonCount": 25000,  
  "status": "draft"  
}

Response:

{  
  "success": true,  
  "message": "3D model created successfully.",  
  "data": {  
    "id": "model-001",  
    "title": "Mô hình Nhà Cổ Bình Thủy",  
    "status": "draft"  
  }  
}

---

### **Cập nhật model 3D**

PUT /api/admin/models-3d/{id}

Request body tương tự tạo mới.

---

### **Cập nhật trạng thái model**

PATCH /api/admin/models-3d/{id}/status

Request body:

{  
  "status": "published"  
}

---

### **Xóa / archive model**

DELETE /api/admin/models-3d/{id}

Khuyến nghị:

Không xóa cứng ngay.  
Chuyển status \= archived.

---

## **9.3. Media Upload API mở rộng**

API upload media hiện tại cần hỗ trợ thêm:

mediaType \= model3d

Ví dụ:

POST /api/admin/media/upload  
Content-Type: multipart/form-data

Form data:

| Field | Bắt buộc | Mô tả |
| ----- | ----- | ----- |
| file | Có | File `.glb` hoặc `.gltf` |
| mediaType | Có | model3d |
| ownerType | Không | destination/hotspot/model3d |
| ownerId | Không | ID đối tượng liên quan |
| altText | Không | Mô tả |
| caption | Không | Chú thích |

Validation:

\- Chỉ cho phép .glb, .gltf.  
\- MIME type hợp lệ.  
\- File size không vượt giới hạn.  
\- Không upload file rỗng.

---

## **10\. Validation Rules**

### **10.1. Validate upload model**

\- File bắt buộc.  
\- Extension phải là .glb hoặc .gltf.  
\- mediaType phải là model3d.  
\- File size \<= giới hạn cấu hình.  
\- Không cho upload extension nguy hiểm.

MIME type gợi ý:

.glb  → model/gltf-binary hoặc application/octet-stream  
.gltf → model/gltf+json hoặc application/json

Lưu ý:

Một số browser/storage có thể trả MIME type .glb là application/octet-stream.  
Vì vậy không nên chỉ dựa vào MIME type, cần kiểm tra cả extension.

---

### **10.2. Validate model metadata**

\- title bắt buộc.  
\- mediaId bắt buộc.  
\- mediaId phải tồn tại.  
\- mediaId phải có mediaType \= model3d.  
\- targetType phải thuộc destination, panorama, hotspot.  
\- targetId bắt buộc.  
\- status phải thuộc draft, published, archived.  
\- format nếu có phải là glb/gltf.

---

### **10.3. Validate publish model**

Trước khi publish:

\- Model phải có title.  
\- Model phải có mediaId hợp lệ.  
\- File URL phải truy cập được.  
\- targetType/targetId phải hợp lệ.

Sprint 17 có thể chưa cần kiểm tra targetId tồn tại ở mọi loại nếu làm generic, nhưng nên kiểm tra ít nhất với `destination`.

---

## **11\. UI/UX Specification**

## **11.1. Admin Model Management Page**

Route:

/admin/models-3d

Mục tiêu:

Cho admin xem, tìm kiếm, lọc, tạo, sửa và publish model 3D.

Layout:

Page header  
Action button: Upload/Create Model  
Filter bar  
Model grid/table  
Pagination

Hiển thị mỗi model:

\- Thumbnail  
\- Title  
\- Format  
\- Target type  
\- Target name  
\- Status  
\- Created at  
\- Actions

Actions:

\- Preview  
\- Edit  
\- Publish/Archive  
\- Delete/Archive

---

## **11.2. Admin Create/Edit Model Page**

Route:

/admin/models-3d/create  
/admin/models-3d/:id/edit

Form sections:

1\. Model Information  
2\. Model File  
3\. Thumbnail  
4\. Target Binding  
5\. Publishing Status

Fields:

\- title  
\- description  
\- mediaId/model file  
\- thumbnailId  
\- targetType  
\- targetId  
\- format  
\- polygonCount  
\- status

Target binding UI:

targetType dropdown:  
\- destination  
\- panorama  
\- hotspot

targetId selector:  
\- Nếu destination: chọn destination từ dropdown/search.  
\- Nếu panorama: chọn virtual tour rồi chọn panorama.  
\- Nếu hotspot: chọn tour/panorama rồi chọn hotspot.

Sprint 17 có thể ưu tiên:

Chỉ hỗ trợ targetType \= destination trước.  
Hotspot/panorama binding có thể bổ sung sau.

---

## **11.3. Admin Model Preview**

Admin cần preview model sau khi upload.

Dùng:

\<model-viewer\>

Preview hỗ trợ:

\- Xoay model  
\- Zoom model  
\- Reset camera nếu có  
\- Hiển thị loading  
\- Hiển thị lỗi nếu model không load được

---

## **11.4. Public Destination Detail Page**

Nếu địa điểm có model 3D, trang chi tiết địa điểm hiển thị section:

Mô hình 3D

Section gồm:

\- Model viewer  
\- Tên model  
\- Mô tả model  
\- Nút fullscreen nếu có  
\- Ghi chú “Kéo để xoay, cuộn để phóng to”

Nếu có nhiều model:

\- Hiển thị danh sách thumbnail model.  
\- Click thumbnail để đổi model đang xem.

Sprint 17 có thể làm đơn giản:

Hiển thị model đầu tiên của destination.

---

## **11.5. Public Model Detail Page**

Có thể thêm route:

/models-3d/:id

Trang này dùng để xem model toàn màn hình.

Nội dung:

\- Model viewer lớn  
\- Tên model  
\- Mô tả  
\- Địa điểm liên quan  
\- Nút quay lại địa điểm

Không bắt buộc trong Sprint 17 nếu đã hiển thị model trong destination detail.

---

## **11.6. Model từ Hotspot trong Tour 360**

Nếu hotspot có:

type \= model3d  
mediaId hoặc model3dId

Khi du khách click hotspot:

\- Mở modal hoặc side panel.  
\- Hiển thị model viewer.  
\- Cho phép xoay/zoom model.  
\- Có nút đóng quay lại tour 360\.

Gợi ý UI:

Desktop:  
\- Modal lớn giữa màn hình hoặc side panel bên phải.

Mobile:  
\- Bottom sheet full height.

---

## **12\. Frontend Technology Choice**

## **12.1. Lựa chọn khuyến nghị: `<model-viewer>`**

Sprint 17 nên dùng:

@google/model-viewer

Lý do:

\- Dễ tích hợp.  
\- Hỗ trợ .glb/.gltf tốt.  
\- Có sẵn camera controls.  
\- Có sẵn auto-rotate.  
\- Có thể bật AR sau nếu cần.  
\- Không cần viết Three.js custom ngay.

Ví dụ ý tưởng component:

\<model-viewer  
  src="model-url.glb"  
  camera-controls  
  auto-rotate  
  shadow-intensity="1"  
  exposure="1"  
  style="width: 100%; height: 500px;"  
\>  
\</model-viewer\>

---

## **12.2. Khi nào dùng Three.js?**

Three.js phù hợp nếu sau này cần:

\- Tùy chỉnh scene phức tạp.  
\- Nhiều model trong cùng scene.  
\- Tương tác nâng cao.  
\- Custom shader/material.  
\- Animation phức tạp.  
\- Kết hợp model với dữ liệu GIS/3D Tiles.

Trong Sprint 17, chưa cần Three.js custom.

---

## **13\. Component Design**

### **13.1. Public components**

ModelViewer.vue  
ModelSection.vue  
ModelThumbnailList.vue  
ModelDetailPage.vue, optional  
ModelLoadingState.vue  
ModelErrorState.vue

---

### **13.2. Admin components**

AdminModelsPage.vue  
ModelFormPage.vue  
ModelUploadField.vue  
ModelPreview.vue  
ModelTargetSelector.vue  
ModelStatusBadge.vue

---

### **13.3. Tour 360 components**

HotspotModelPanel.vue  
ModelHotspotModal.vue

---

## **14\. ModelViewer Component**

### **14.1. Props**

modelUrl  
posterUrl  
title  
autoRotate  
cameraControls  
height

---

### **14.2. State**

isLoading  
hasError  
isFullscreen

---

### **14.3. Behavior**

\- Hiển thị loading khi model đang tải.  
\- Hiển thị error nếu model load lỗi.  
\- Cho phép camera controls.  
\- Cho phép auto rotate nếu bật.  
\- Có fallback nếu modelUrl null.

---

## **15\. Integration với tính năng hiện có**

### **15.1. Tích hợp với media\_files**

Media upload cần thêm:

media\_type \= model3d

Media Library cần hiển thị:

\- Badge model3d  
\- Icon 3D cube  
\- Preview model nếu có thể  
\- File extension .glb/.gltf

---

### **15.2. Tích hợp với Destination Detail**

Destination detail gọi:

GET /api/destinations/{destinationId}/models-3d

Nếu có model:

Hiển thị ModelSection.

---

### **15.3. Tích hợp với Hotspot**

Hotspot hiện có `type = model3d` trong thiết kế trước.

Cần thống nhất:

Option A:  
hotspots.media\_id trỏ trực tiếp đến media\_files có media\_type \= model3d.

Option B:  
hotspots.metadata hoặc trường mở rộng trỏ đến models\_3d.id.

Option C:  
Tạo model targetType \= hotspot, targetId \= hotspotId.

Khuyến nghị Sprint 17:

Dùng Option A cho đơn giản:  
hotspots.type \= model3d  
hotspots.media\_id \= media\_files.id của file .glb

Sau này nếu cần metadata đầy đủ, dùng thêm bảng `models_3d` với targetType \= hotspot.

---

### **15.4. Tích hợp với CesiumJS**

Trong Sprint 17, chưa bắt buộc đặt model lên Cesium map.

Sau Sprint 17 có thể mở rộng:

\- Hiển thị model 3D tại vị trí destination trên Cesium.  
\- Dùng Cesium ModelGraphics hoặc Cesium 3D Tiles.  
\- Cho phép bật/tắt layer model.

Để chuẩn bị, bảng `models_3d` có thể mở rộng sau với:

longitude  
latitude  
height  
heading  
pitch  
roll  
scale

Chưa cần thêm trong Sprint 17 nếu chưa dùng.

---

## **16\. Storage Design**

### **16.1. Storage path đề xuất**

{regionSlug}/{destinationSlug}/models/{fileName}

Ví dụ:

can-tho-demo/nha-co-binh-thuy/models/nha-co-binh-thuy.glb

Thumbnail:

can-tho-demo/nha-co-binh-thuy/models/thumbnails/nha-co-binh-thuy-thumb.jpg

---

### **16.2. Public URL**

Model viewer cần truy cập được model file từ browser.

Vì vậy:

\- MinIO/S3 phải trả được fileUrl hợp lệ.  
\- CORS phải cho phép frontend load model.  
\- Content-Type nên đúng hoặc ít nhất không bị chặn.

---

### **16.3. CORS lưu ý**

Nếu model load lỗi trên browser, cần kiểm tra:

\- File URL có truy cập trực tiếp được không?  
\- MinIO bucket có public/presigned URL đúng không?  
\- CORS có cho phép origin frontend không?  
\- Content-Type có phù hợp không?

---

## **17\. Seed Data**

Nên seed 1–2 model mẫu.

Ví dụ:

Model 1:  
\- Title: Mô hình Nhà Cổ Bình Thủy  
\- Target: destination Nhà Cổ Bình Thủy  
\- Format: glb  
\- Status: published

Model 2:  
\- Title: Mô hình hiện vật làng nghề  
\- Target: destination Làng nghề Demo  
\- Format: glb  
\- Status: published

Nếu chưa có model thật:

\- Dùng model sample miễn phí có license rõ.  
\- Ghi nguồn trong sample-data/credits.md.  
\- Không dùng model không rõ bản quyền.

Lưu ý:

Không seed link ngoài nếu không ổn định.  
Ưu tiên tải model mẫu về MinIO hoặc thư mục sample-data.

---

## **18\. Analytics Events liên quan**

Nếu Analytics module đã có, thêm event:

view\_3d\_model  
open\_3d\_model\_from\_destination  
open\_3d\_model\_from\_hotspot

Ví dụ:

{  
  "eventName": "view\_3d\_model",  
  "targetType": "model3d",  
  "targetId": "model-001",  
  "metadata": {  
    "source": "destination\_detail",  
    "destinationId": "destination-001"  
  }  
}

---

## **19\. Test Cases**

### **19.1. Backend test cases**

TC-BE-01: Upload file .glb với mediaType model3d thành công.  
TC-BE-02: Không cho upload file .exe dưới mediaType model3d.  
TC-BE-03: Không cho tạo model thiếu title.  
TC-BE-04: Không cho tạo model với mediaId không tồn tại.  
TC-BE-05: Không cho tạo model với mediaId không phải model3d.  
TC-BE-06: Public API chỉ trả model published.  
TC-BE-07: Admin API yêu cầu JWT.  
TC-BE-08: Archive model không làm mất file media.

---

### **19.2. Frontend admin test cases**

TC-ADMIN-01: Admin upload model .glb thành công.  
TC-ADMIN-02: Admin thấy model trong Media Library.  
TC-ADMIN-03: Admin tạo metadata model.  
TC-ADMIN-04: Admin gắn model với destination.  
TC-ADMIN-05: Admin preview model trong dashboard.  
TC-ADMIN-06: Admin publish/archive model.  
TC-ADMIN-07: Upload sai định dạng hiển thị lỗi rõ ràng.

---

### **19.3. Frontend public test cases**

TC-PUBLIC-01: Destination detail hiển thị section Model 3D nếu có model.  
TC-PUBLIC-02: Model viewer load được file .glb.  
TC-PUBLIC-03: User xoay/zoom model được.  
TC-PUBLIC-04: Nếu model lỗi, hiển thị error state.  
TC-PUBLIC-05: Nếu hotspot type model3d, click hotspot mở model modal.  
TC-PUBLIC-06: Đóng modal model quay lại tour 360 bình thường.

---

### **19.4. Regression test cases**

TC-REG-01: Upload image/audio/video cũ vẫn hoạt động.  
TC-REG-02: Tour 360 vẫn hoạt động.  
TC-REG-03: Hotspot navigation vẫn hoạt động.  
TC-REG-04: Destination detail vẫn hoạt động nếu không có model.  
TC-REG-05: Docker build frontend/backend vẫn thành công.

---

## **20\. Sprint 17 Implementation Plan**

### **20.1. Mục tiêu Sprint 17**

Bổ sung khả năng upload, quản lý và hiển thị mô hình 3D cho địa điểm hoặc hotspot trong Smart Tourism 360 Platform.

---

### **20.2. Checklist Database**

\- \[ \] Thêm mediaType model3d nếu dùng enum.  
\- \[ \] Tạo entity Model3D.  
\- \[ \] Tạo bảng models\_3d.  
\- \[ \] Tạo migration.  
\- \[ \] Thêm quan hệ media\_id, thumbnail\_id, created\_by, updated\_by.  
\- \[ \] Seed 1–2 model mẫu nếu có dữ liệu hợp lệ.

---

### **20.3. Checklist Backend API**

\- \[ \] Cập nhật media upload cho .glb/.gltf.  
\- \[ \] Validate extension .glb/.gltf.  
\- \[ \] Tạo DTO Model3DListItem.  
\- \[ \] Tạo DTO Model3DDetail.  
\- \[ \] Tạo DTO CreateModel3D.  
\- \[ \] Tạo DTO UpdateModel3D.  
\- \[ \] Tạo Model3DService.  
\- \[ \] Tạo public GET /api/destinations/{destinationId}/models-3d.  
\- \[ \] Tạo public GET /api/models-3d/{id}.  
\- \[ \] Tạo admin GET /api/admin/models-3d.  
\- \[ \] Tạo admin POST /api/admin/models-3d.  
\- \[ \] Tạo admin PUT /api/admin/models-3d/{id}.  
\- \[ \] Tạo admin PATCH /api/admin/models-3d/{id}/status.  
\- \[ \] Tạo admin DELETE /api/admin/models-3d/{id}.  
\- \[ \] Public API chỉ trả model published.  
\- \[ \] Admin API yêu cầu JWT.

---

### **20.4. Checklist Frontend Public**

\- \[ \] Cài @google/model-viewer.  
\- \[ \] Tạo ModelViewer.vue.  
\- \[ \] Tạo ModelSection.vue.  
\- \[ \] Gọi API lấy model theo destination.  
\- \[ \] Hiển thị model trong destination detail.  
\- \[ \] Loading/error state cho model.  
\- \[ \] Tạo ModelHotspotModal nếu hỗ trợ hotspot model3d.  
\- \[ \] Tracking view\_3d\_model nếu Analytics đã có.

---

### **20.5. Checklist Frontend Admin**

\- \[ \] Cập nhật Media Upload cho model3d.  
\- \[ \] Hiển thị badge model3d trong Media Library.  
\- \[ \] Tạo /admin/models-3d.  
\- \[ \] Tạo Model list/grid.  
\- \[ \] Tạo create/edit model form.  
\- \[ \] Tạo target selector cho destination.  
\- \[ \] Tạo model preview trong admin.  
\- \[ \] Publish/archive model.

---

### **20.6. Checklist UI/UX**

\- \[ \] Model viewer có loading state.  
\- \[ \] Model viewer có error state.  
\- \[ \] Có hướng dẫn “Kéo để xoay, cuộn để phóng to”.  
\- \[ \] Model section không làm vỡ destination detail.  
\- \[ \] Mobile hiển thị model ở chiều cao phù hợp.  
\- \[ \] Admin preview model rõ ràng.

---

### **20.7. Checklist Storage/Config**

\- \[ \] MinIO cho phép upload .glb.  
\- \[ \] FileUrl model load được từ browser.  
\- \[ \] Kiểm tra CORS.  
\- \[ \] Kiểm tra content-type.  
\- \[ \] Không commit model quá nặng vào repo.  
\- \[ \] Nếu dùng sample model, ghi source/license.

---

### **20.8. Checklist Regression**

\- \[ \] Image upload vẫn hoạt động.  
\- \[ \] Panorama upload vẫn hoạt động.  
\- \[ \] Audio upload vẫn hoạt động.  
\- \[ \] Destination detail vẫn hoạt động.  
\- \[ \] Tour 360 vẫn hoạt động.  
\- \[ \] Docker compose build vẫn thành công.

---

## **21\. Acceptance Criteria**

Sprint 17 được xem là hoàn thành nếu:

\- Admin upload được file .glb hoặc .gltf hợp lệ.  
\- File model được lưu vào MinIO/object storage.  
\- Metadata file được lưu vào media\_files.  
\- Admin tạo được bản ghi model 3D trong models\_3d.  
\- Admin gắn model với destination.  
\- Public destination detail hiển thị model 3D.  
\- User có thể xoay/zoom model trên web.  
\- Nếu model load lỗi, UI hiển thị lỗi thân thiện.  
\- Admin có thể publish/archive model.  
\- Không làm hỏng chức năng upload media cũ.  
\- Không làm hỏng tour 360/hotspot navigation.

---

## **22\. Non-goals**

Sprint 17 không bao gồm:

\- Không tạo model 3D tự động.  
\- Không scan 3D.  
\- Không chỉnh sửa geometry/material trên web.  
\- Không làm AR hoàn chỉnh.  
\- Không đặt model trực tiếp lên Cesium terrain.  
\- Không làm 3D Tiles.  
\- Không làm animation editor.  
\- Không tối ưu LOD nâng cao.

---

## **23\. Rủi ro và cách kiểm soát**

### **23.1. Model file quá nặng**

Cách kiểm soát:

\- Giới hạn dung lượng upload.  
\- Khuyến nghị dùng .glb tối ưu.  
\- Hiển thị loading rõ ràng.  
\- Không auto-load quá nhiều model cùng lúc.

---

### **23.2. Model không load do CORS/Content-Type**

Cách kiểm soát:

\- Test URL model trực tiếp trên browser.  
\- Cấu hình MinIO CORS.  
\- Chấp nhận application/octet-stream cho .glb nếu cần.

---

### **23.3. Upload .gltf phức tạp vì có file phụ**

Cách kiểm soát:

\- Sprint 17 ưu tiên .glb.  
\- .gltf nhiều file có thể để sau hoặc yêu cầu upload .zip.

---

### **23.4. UI destination detail bị nặng**

Cách kiểm soát:

\- Lazy load model section.  
\- Chỉ load model khi section xuất hiện hoặc khi user bấm “Xem model 3D”.

---

### **23.5. Model không rõ bản quyền**

Cách kiểm soát:

\- Dùng model sample có license rõ.  
\- Ghi source trong credits.md.  
\- Không dùng model tải ngẫu nhiên không rõ nguồn.

---

## **24\. Roadmap mở rộng sau Sprint 17**

Sau khi 3D Model MVP hoạt động, có thể mở rộng:

1\. Hỗ trợ nhiều model cho một destination.  
2\. Model viewer fullscreen.  
3\. Model từ hotspot trong tour 360 nâng cao hơn.  
4\. Gắn model vào CesiumJS map.  
5\. Thêm thông số transform: longitude, latitude, height, heading, pitch, roll, scale.  
6\. AR preview bằng model-viewer.  
7\. Tối ưu model bằng Draco compression.  
8\. Quản lý phiên bản model.  
9\. Thêm model annotation: điểm chú thích trên model.

---

## **25\. Kết luận**

Tính năng **3D Model** giúp **Smart Tourism 360 Platform** mở rộng từ nền tảng bản đồ và tour 360 sang hướng số hóa đối tượng du lịch ở cấp độ chi tiết hơn. Nếu tour 360 giúp du khách “đứng trong không gian”, thì model 3D giúp du khách “cầm nắm và quan sát đối tượng” theo cách trực quan hơn.

Sprint 17 nên triển khai theo hướng vừa đủ:

Admin upload model .glb  
→ lưu vào MinIO  
→ tạo metadata models\_3d  
→ gắn với destination  
→ public hiển thị model bằng \<model-viewer\>

Cách tiếp cận này giữ hệ thống đơn giản, dễ demo, không phá kiến trúc hiện tại và mở đường cho các nâng cấp sau như CesiumJS model placement, AR preview, model annotation và digital heritage experience.

