# **PRODUCT ROADMAP DOCUMENT**

# **Smart Tourism 360 Platform v2**

## **Lộ trình phát triển sản phẩm sau phiên bản MVP**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | Product Roadmap Document |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản tài liệu | v1.0 |
| Giai đoạn | Sau MVP |
| Mục đích | Xác định định hướng phát triển sản phẩm từ MVP sang phiên bản v2 và các giai đoạn mở rộng tiếp theo |
| Đối tượng đọc | Project owner, developer, UI/UX designer, giảng viên hướng dẫn, nhóm phát triển, người đánh giá dự án |
| Phạm vi | UI/UX upgrade, tuyến tham quan, analytics, đa ngôn ngữ, CesiumJS/WebGIS 3D, model 3D, AI Tour Guide |

---

## **2\. Bối cảnh hiện tại**

Dự án **Smart Tourism 360 Platform** đã hoàn thành phiên bản MVP với các chức năng cốt lõi:

\- Public website cho du khách  
\- Admin dashboard cho quản trị viên  
\- Quản lý khu vực, danh mục, địa điểm  
\- Chọn tọa độ địa điểm trực tiếp trên bản đồ  
\- Upload và quản lý media  
\- Tạo virtual tour 360  
\- Tạo nhiều panorama trong tour  
\- Tạo hotspot thông tin và hotspot điều hướng  
\- Du khách xem bản đồ địa điểm  
\- Du khách xem chi tiết địa điểm  
\- Du khách tham quan 360 và chuyển cảnh giữa các panorama  
\- PostgreSQL/PostGIS cho dữ liệu địa lý  
\- MinIO cho object storage  
\- Docker Compose cho dev mode và full demo mode

MVP hiện tại đã chứng minh được tính khả thi về mặt kỹ thuật và nghiệp vụ. Tuy nhiên, để dự án trở thành một sản phẩm demo mạnh hơn, có chiều sâu hơn và có khả năng mở rộng thành nền tảng thực tế, cần tiếp tục phát triển theo hướng có chiến lược.

---

## **3\. Mục tiêu của phiên bản v2**

Phiên bản v2 không chỉ nhằm “thêm tính năng”, mà hướng đến việc nâng cấp dự án từ một MVP kỹ thuật thành một nền tảng du lịch số có trải nghiệm tốt hơn, dữ liệu phong phú hơn và giá trị sử dụng rõ ràng hơn.

Mục tiêu chính của v2 gồm:

1\. Nâng cấp UI/UX để giao diện chuyên nghiệp, hiện đại và hấp dẫn hơn.  
2\. Mở rộng tính năng du lịch thực tế như tuyến tham quan, gợi ý lịch trình, analytics.  
3\. Cải thiện trải nghiệm tour 360, đặc biệt trên mobile.  
4\. Bổ sung khả năng đa ngôn ngữ để phục vụ du khách ngoài địa phương.  
5\. Chuẩn bị nền tảng cho WebGIS 3D bằng CesiumJS.  
6\. Hỗ trợ mô hình 3D cho hiện vật/công trình/địa điểm.  
7\. Thiết kế AI Tour Guide dựa trên dữ liệu nội bộ của hệ thống.  
8\. Giữ hệ thống ổn định, dễ bảo trì và không phá vỡ kiến trúc MVP hiện tại.

---

## **4\. Nguyên tắc phát triển sau MVP**

### **4.1. Không phá lõi MVP**

Các tính năng hiện có như destination, media, virtual tour, panorama và hotspot navigation là lõi của hệ thống. Mọi nâng cấp sau MVP phải giữ ổn định các chức năng này.

Không được làm ảnh hưởng đến các luồng chính:

Admin tạo địa điểm  
→ Admin upload media  
→ Admin tạo tour 360  
→ Admin tạo panorama  
→ Admin tạo hotspot navigation  
→ Du khách mở tour 360  
→ Du khách chuyển cảnh giữa các panorama

### **4.2. Ưu tiên trải nghiệm trước công nghệ cao**

Dù CesiumJS, AI và model 3D là các hướng nâng cấp hấp dẫn, dự án không nên nhảy ngay vào công nghệ cao nếu giao diện và trải nghiệm người dùng chưa đủ tốt.

Thứ tự ưu tiên nên là:

UI/UX tốt hơn  
→ nghiệp vụ du lịch thực tế hơn  
→ công nghệ cao tạo điểm nhấn

### **4.3. Mỗi phase phải có giá trị demo riêng**

Mỗi giai đoạn phát triển phải tạo ra một kết quả có thể demo được. Không nên triển khai các phần quá lớn nhưng chưa hoàn thiện.

Ví dụ:

Phase UI/UX Upgrade  
→ demo được giao diện mới.

Phase Route/Itinerary  
→ demo được tuyến tham quan.

Phase CesiumJS  
→ demo được bản đồ 3D hiển thị địa điểm.

Phase AI Tour Guide  
→ demo được chatbot hỏi đáp dựa trên dữ liệu du lịch.

### **4.4. Tách rõ tính năng MVP, v2 và tương lai**

Dự án cần tránh việc gom quá nhiều tính năng vào một phiên bản. Các tính năng nên được phân cấp:

MVP:  
\- Map 2D  
\- Destination  
\- Media  
\- Virtual Tour  
\- Panorama  
\- Hotspot

v2:  
\- UI/UX upgrade  
\- Route/Itinerary  
\- Analytics  
\- Multi-language

Advanced:  
\- CesiumJS/WebGIS 3D  
\- 3D Model Viewer  
\- AI Tour Guide  
\- AR Check-in

---

## **5\. Định hướng sản phẩm v2**

### **5.1. Từ nền tảng tham quan 360 sang nền tảng du lịch thông minh**

MVP hiện tại chủ yếu trả lời câu hỏi:

Địa điểm này ở đâu?  
Địa điểm này có gì?  
Có thể tham quan ảo 360 không?

Phiên bản v2 cần trả lời thêm:

Tôi nên đi những đâu?  
Tuyến tham quan nào phù hợp?  
Địa điểm nào đang được quan tâm nhiều?  
Du khách tương tác với nội dung nào nhiều nhất?  
Có thể xem khu vực này trên bản đồ 3D không?  
Có thể hỏi AI về địa điểm không?

### **5.2. Từ quản lý địa điểm sang quản lý trải nghiệm du lịch**

MVP quản lý các đơn vị dữ liệu riêng lẻ như địa điểm, media, tour và hotspot. v2 cần quản lý trải nghiệm hoàn chỉnh hơn:

Địa điểm  
→ Tour 360  
→ Hotspot  
→ Audio guide  
→ Tuyến tham quan  
→ Gợi ý lịch trình  
→ Phân tích hành vi  
→ Hỗ trợ AI

---

## **6\. Roadmap tổng quan**

Roadmap sau MVP được chia thành 5 phase chính:

| Phase | Tên phase | Trọng tâm | Kết quả chính |
| ----- | ----- | ----- | ----- |
| Phase 2A | UI/UX Upgrade | Làm đẹp và tối ưu trải nghiệm | Giao diện public/admin chuyên nghiệp hơn |
| Phase 2B | Smart Tourism Features | Tuyến tham quan, analytics, đa ngôn ngữ | Hệ thống có nghiệp vụ du lịch thực tế hơn |
| Phase 3A | WebGIS 3D Integration | CesiumJS, bản đồ 3D | Có chế độ khám phá 3D |
| Phase 3B | 3D Model Support | Upload và xem model 3D | Hỗ trợ số hóa hiện vật/công trình |
| Phase 4 | AI Tour Guide | Chatbot/RAG dữ liệu du lịch | Có trợ lý du lịch thông minh |

---

# **PHASE 2A: UI/UX UPGRADE**

---

## **7\. Mục tiêu Phase 2A**

Phase 2A tập trung nâng cấp giao diện và trải nghiệm người dùng, giúp dự án trở nên chuyên nghiệp, hiện đại và dễ demo hơn.

Mục tiêu:

\- Làm public website hấp dẫn hơn.  
\- Làm trang chi tiết địa điểm giống một landing page du lịch.  
\- Làm tour 360 viewer mượt và trực quan hơn.  
\- Tối ưu trải nghiệm mobile.  
\- Cải thiện admin dashboard để dễ thao tác hơn.

---

## **8\. Phạm vi nâng cấp UI/UX**

### **8.1. Public Home Page**

Cần nâng cấp:

\- Hero section đẹp hơn với ảnh/video nền.  
\- CTA rõ ràng: Khám phá bản đồ, Xem tour 360\.  
\- Section địa điểm nổi bật.  
\- Section trải nghiệm 360\.  
\- Section tuyến tham quan gợi ý, có thể để placeholder nếu route chưa triển khai.  
\- Animation fade/slide nhẹ.

### **8.2. Explore Map Page**

Cần nâng cấp:

\- Marker đẹp hơn theo danh mục.  
\- Popup marker chuyên nghiệp hơn.  
\- Filter category dạng chip.  
\- Search bar nổi bật.  
\- Responsive mobile dạng bottom sheet.  
\- Loading skeleton khi tải marker.  
\- Empty state khi không có kết quả.

### **8.3. Destination Detail Page**

Cần nâng cấp:

\- Hero image lớn.  
\- Category badge.  
\- CTA “Tham quan 360” nổi bật.  
\- Gallery ảnh.  
\- Audio guide player.  
\- Bản đồ vị trí nhỏ.  
\- Section thông tin tham quan.  
\- Section địa điểm gần đó.  
\- Layout đẹp trên mobile.

### **8.4. Tour 360 Viewer**

Cần nâng cấp:

\- Scene list có thumbnail.  
\- First-time guide overlay.  
\- Hotspot icon đẹp hơn.  
\- Hotspot label khi hover/tap.  
\- Transition mượt khi chuyển panorama.  
\- Fullscreen mode.  
\- Auto-rotate mode.  
\- Mobile bottom sheet cho hotspot info/audio.

### **8.5. Admin Dashboard**

Cần nâng cấp:

\- Dashboard có thống kê trực quan hơn.  
\- Form tạo địa điểm chia section rõ hơn.  
\- Media Library có preview tốt hơn.  
\- Panorama Management có thumbnail card đẹp hơn.  
\- Hotspot Editor có side panel dễ dùng hơn.  
\- Badge trạng thái draft/published/archived rõ ràng hơn.

---

## **9\. Kết quả mong đợi Phase 2A**

Sau Phase 2A, hệ thống cần đạt:

\- Public website nhìn chuyên nghiệp hơn.  
\- Du khách dễ hiểu cách khám phá bản đồ và mở tour 360\.  
\- Tour 360 có trải nghiệm tốt hơn trên desktop và mobile.  
\- Admin thao tác dễ hơn khi quản lý dữ liệu.  
\- Dự án phù hợp hơn để demo, quay video và đưa vào portfolio.

---

# **PHASE 2B: SMART TOURISM FEATURES**

---

## **10\. Mục tiêu Phase 2B**

Phase 2B bổ sung các tính năng du lịch thông minh, giúp hệ thống không chỉ hiển thị địa điểm mà còn hỗ trợ du khách khám phá một khu vực theo hành trình.

Mục tiêu:

\- Thêm tuyến tham quan.  
\- Thêm analytics cơ bản.  
\- Chuẩn bị đa ngôn ngữ.  
\- Tăng giá trị cho du khách và cơ quan quản lý.

---

## **11\. Route / Itinerary Module**

### **11.1. Mục tiêu**

Tuyến tham quan giúp du khách khám phá nhiều địa điểm theo một thứ tự hợp lý.

Ví dụ:

Tuyến khám phá văn hóa Cần Thơ nửa ngày:  
1\. Nhà Cổ Bình Thủy  
2\. Bến Ninh Kiều  
3\. Chùa Ông  
4\. Điểm ẩm thực địa phương

### **11.2. Chức năng cho admin**

\- Tạo tuyến tham quan.  
\- Chọn khu vực.  
\- Chọn nhiều địa điểm.  
\- Sắp xếp thứ tự địa điểm.  
\- Nhập thời lượng dự kiến.  
\- Nhập khoảng cách dự kiến.  
\- Nhập mô tả tuyến.  
\- Gắn ảnh đại diện.  
\- Xuất bản hoặc lưu nháp tuyến.

### **11.3. Chức năng cho du khách**

\- Xem danh sách tuyến tham quan.  
\- Xem chi tiết tuyến.  
\- Xem các địa điểm trong tuyến.  
\- Xem tuyến trên bản đồ.  
\- Mở từng địa điểm từ tuyến.  
\- Mở tour 360 của từng địa điểm nếu có.

### **11.4. Giá trị**

\- Tăng tính ứng dụng thực tế.  
\- Giúp du khách dễ lập kế hoạch.  
\- Phù hợp với định hướng du lịch thông minh.  
\- Tạo dữ liệu đầu vào tốt cho AI Tour Guide sau này.

---

## **12\. Analytics Module**

### **12.1. Mục tiêu**

Analytics giúp hệ thống ghi nhận hành vi người dùng để đánh giá mức độ quan tâm đến địa điểm, tour và hotspot.

### **12.2. Event cần ghi nhận**

\- view\_destination  
\- open\_virtual\_tour  
\- click\_hotspot  
\- navigate\_panorama  
\- play\_audio  
\- play\_video  
\- click\_map\_marker  
\- search\_destination  
\- filter\_category

### **12.3. Dashboard admin**

Admin có thể xem:

\- Tổng lượt xem địa điểm.  
\- Tổng lượt mở tour 360\.  
\- Địa điểm được xem nhiều nhất.  
\- Tour 360 được mở nhiều nhất.  
\- Hotspot được click nhiều nhất.  
\- Biểu đồ lượt xem theo ngày.

### **12.4. Giá trị**

\- Hữu ích cho cơ quan quản lý du lịch.  
\- Giúp biết nội dung nào hấp dẫn.  
\- Hỗ trợ cải thiện trải nghiệm người dùng.  
\- Tạo cơ sở dữ liệu cho báo cáo thống kê.

---

## **13\. Multi-language Support**

### **13.1. Mục tiêu**

Hỗ trợ đa ngôn ngữ giúp hệ thống phục vụ du khách ngoài địa phương.

Ngôn ngữ ưu tiên:

\- Tiếng Việt  
\- Tiếng Anh

Có thể mở rộng sau:

\- Tiếng Khmer  
\- Tiếng Hàn  
\- Tiếng Nhật

### **13.2. Nội dung cần dịch**

\- Tên địa điểm  
\- Mô tả ngắn  
\- Mô tả chi tiết  
\- Tên tour  
\- Tên panorama  
\- Tiêu đề hotspot  
\- Nội dung hotspot  
\- Transcript audio  
\- Tên tuyến tham quan  
\- Mô tả tuyến tham quan

### **13.3. Cách triển khai**

Có thể sử dụng bảng `translations` đã được thiết kế trong tài liệu database mở rộng.

Luồng cơ bản:

Frontend gửi languageCode  
→ Backend lấy dữ liệu gốc  
→ Backend tìm bản dịch tương ứng  
→ Nếu có bản dịch, trả về nội dung dịch  
→ Nếu không có, fallback về tiếng Việt

---

# **PHASE 3A: WEBGIS 3D INTEGRATION**

---

## **14\. Mục tiêu Phase 3A**

Phase 3A tích hợp CesiumJS để nâng cấp bản đồ từ 2D sang 3D, tạo điểm nhấn công nghệ cho dự án.

Mục tiêu:

\- Thêm trang khám phá bản đồ 3D.  
\- Hiển thị địa điểm trên bản đồ 3D.  
\- Cho phép click marker để xem thông tin.  
\- Cho phép fly to địa điểm.  
\- Hiển thị tuyến tham quan dạng đường 3D.  
\- Kết nối bản đồ 3D với tour 360\.

---

## **15\. Cách tích hợp CesiumJS**

Không thay thế Leaflet ngay. Leaflet và CesiumJS nên cùng tồn tại:

/explore  
→ Bản đồ 2D bằng Leaflet

/explore-3d  
→ Bản đồ 3D bằng CesiumJS

Lý do:

\- Leaflet nhẹ, phù hợp public map cơ bản.  
\- CesiumJS mạnh cho 3D/WebGIS.  
\- Tách riêng giúp giảm rủi ro.  
\- Không phá chức năng đã ổn định của MVP.

---

## **16\. Chức năng CesiumJS giai đoạn đầu**

\- Load danh sách destinations từ API.  
\- Hiển thị marker/billboard trên globe.  
\- Click marker mở popup địa điểm.  
\- Popup có nút xem chi tiết.  
\- Popup có nút mở tour 360\.  
\- FlyTo khi chọn địa điểm.  
\- Hiển thị route bằng polyline.

Chưa nên làm ngay:

\- 3D Tiles phức tạp.  
\- Digital Twin đầy đủ.  
\- Layer quy hoạch.  
\- Terrain nâng cao.  
\- Đồng bộ GIS dữ liệu lớn.

---

# **PHASE 3B: 3D MODEL SUPPORT**

---

## **17\. Mục tiêu Phase 3B**

Phase 3B bổ sung khả năng quản lý và hiển thị mô hình 3D cho các địa điểm, hiện vật hoặc hotspot.

Mục tiêu:

\- Admin upload file .glb/.gltf.  
\- Admin gắn model 3D với địa điểm hoặc hotspot.  
\- Du khách xem model 3D trong trang chi tiết hoặc trong tour 360\.

---

## **18\. Phạm vi tính năng 3D Model**

### **18.1. Admin**

\- Upload model 3D.  
\- Nhập tên model.  
\- Nhập mô tả model.  
\- Gắn model với destination hoặc hotspot.  
\- Chọn thumbnail.  
\- Preview model trong admin.

### **18.2. Du khách**

\- Xem model 3D trong trang chi tiết địa điểm.  
\- Mở model 3D từ hotspot.  
\- Xoay, zoom, pan model.

### **18.3. Công nghệ đề xuất**

Giai đoạn đầu nên dùng:

\<model-viewer\>

Lý do:

\- Dễ tích hợp.  
\- Hỗ trợ glb/gltf tốt.  
\- Có sẵn camera controls.  
\- Phù hợp MVP mở rộng.

Sau này nếu cần tùy chỉnh nâng cao, có thể dùng Three.js.

---

# **PHASE 4: AI TOUR GUIDE**

---

## **19\. Mục tiêu Phase 4**

Phase 4 xây dựng trợ lý AI hướng dẫn du lịch dựa trên dữ liệu nội bộ của hệ thống.

AI Tour Guide không nên trả lời tự do hoàn toàn, mà cần dựa trên dữ liệu có kiểm soát:

\- Destination  
\- Virtual Tour  
\- Panorama  
\- Hotspot  
\- Audio transcript  
\- Route/Itinerary

---

## **20\. Chức năng AI Tour Guide**

### **20.1. Cho du khách**

\- Hỏi đáp về địa điểm.  
\- Gợi ý điểm đến theo sở thích.  
\- Gợi ý tuyến tham quan.  
\- Tóm tắt thông tin địa điểm.  
\- Giải thích hotspot hoặc hiện vật.

Ví dụ câu hỏi:

“Tôi có nửa ngày ở Cần Thơ thì nên đi đâu?”  
“Địa điểm này có gì nổi bật?”  
“Tuyến nào phù hợp cho gia đình?”  
“Nhà Cổ Bình Thủy có giá trị văn hóa gì?”

### **20.2. Cho admin**

\- Gợi ý mô tả địa điểm.  
\- Gợi ý nội dung hotspot.  
\- Tạo bản nháp audio guide script.  
\- Dịch nội dung sang tiếng Anh.

---

## **21\. Hướng kỹ thuật AI**

### **21.1. Giai đoạn đầu**

Backend lấy dữ liệu từ database  
→ tạo context ngắn  
→ gửi đến LLM API  
→ trả câu trả lời cho frontend

### **21.2. Giai đoạn nâng cao**

Tạo embeddings cho destination/hotspot/route/audio transcript  
→ lưu vào vector database hoặc PostgreSQL pgvector  
→ truy vấn dữ liệu liên quan  
→ dùng RAG để trả lời

### **21.3. Nguyên tắc an toàn**

\- AI chỉ trả lời dựa trên dữ liệu hệ thống nếu có thể.  
\- Nếu không đủ dữ liệu, AI phải nói không chắc.  
\- Không bịa thông tin về địa điểm.  
\- Không đưa thông tin giá vé/giờ mở cửa nếu dữ liệu chưa có.

---

# **22\. Roadmap theo Sprint**

## **Sprint 12: UI/UX Upgrade**

Mục tiêu:

Nâng cấp giao diện public website, tour 360 viewer và admin dashboard.

Checklist chính:

\- Redesign Home Page.  
\- Redesign Destination Card.  
\- Redesign Destination Detail Page.  
\- Improve Explore Map UI.  
\- Improve Map Marker Popup.  
\- Add Tour 360 first-time guide overlay.  
\- Add Scene Thumbnail List.  
\- Improve Hotspot UI.  
\- Improve Mobile Tour 360\.  
\- Add smooth transition between panoramas.  
\- Improve loading/error/empty states.

---

## **Sprint 13: Route / Itinerary Module**

Mục tiêu:

Bổ sung chức năng tuyến tham quan cho admin và du khách.

Checklist chính:

\- Thiết kế database routes và route\_destinations.  
\- Tạo Route API.  
\- Tạo admin route management.  
\- Tạo public route list page.  
\- Tạo public route detail page.  
\- Hiển thị route trên bản đồ.  
\- Liên kết route với destination detail.

---

## **Sprint 14: Analytics Module**

Mục tiêu:

Ghi nhận hành vi người dùng và hiển thị thống kê cơ bản cho admin.

Checklist chính:

\- Tạo analytics\_events API.  
\- Ghi nhận view\_destination.  
\- Ghi nhận open\_virtual\_tour.  
\- Ghi nhận click\_hotspot.  
\- Ghi nhận navigate\_panorama.  
\- Tạo dashboard thống kê cơ bản.

---

## **Sprint 15: Multi-language Support**

Mục tiêu:

Hỗ trợ tiếng Việt và tiếng Anh cho dữ liệu public.

Checklist chính:

\- Tạo bảng translations nếu chưa có.  
\- Tạo Translation API.  
\- Tạo admin translation UI.  
\- Frontend hỗ trợ language switcher.  
\- Public API trả dữ liệu theo languageCode.

---

## **Sprint 16: CesiumJS 3D Map**

Mục tiêu:

Tạo chế độ khám phá bản đồ 3D.

Checklist chính:

\- Cài CesiumJS.  
\- Tạo trang /explore-3d.  
\- Load destination markers lên Cesium globe.  
\- Click marker hiển thị popup.  
\- FlyTo địa điểm.  
\- Mở destination detail/tour 360 từ marker 3D.  
\- Hiển thị route bằng polyline 3D.

---

## **Sprint 17: 3D Model Viewer**

Mục tiêu:

Hỗ trợ upload và xem mô hình 3D.

Checklist chính:

\- Bổ sung model3d media type nếu cần.  
\- Tạo model metadata API.  
\- Admin upload .glb/.gltf.  
\- Admin gắn model vào destination/hotspot.  
\- Tạo public model viewer.  
\- Mở model từ destination detail hoặc hotspot.

---

## **Sprint 18: AI Tour Guide**

Mục tiêu:

Tạo trợ lý AI hướng dẫn du lịch dựa trên dữ liệu nội bộ.

Checklist chính:

\- Tạo chat UI.  
\- Tạo backend AI endpoint.  
\- Tạo context từ destination/tour/hotspot/route.  
\- Gợi ý tuyến tham quan.  
\- Hỏi đáp về địa điểm.  
\- Tạo bản nháp nội dung thuyết minh.

---

# **23\. Ma trận ưu tiên tính năng**

| Tính năng | Giá trị người dùng | Độ khó | Ưu tiên |
| ----- | ----- | ----- | ----- |
| UI/UX Upgrade | Cao | Trung bình | Rất cao |
| Route/Itinerary | Cao | Trung bình | Rất cao |
| Analytics | Trung bình \- cao | Trung bình | Cao |
| Multi-language | Cao | Trung bình | Cao |
| CesiumJS 3D Map | Cao | Cao | Cao |
| 3D Model Viewer | Trung bình \- cao | Trung bình | Trung bình |
| AI Tour Guide | Rất cao | Cao | Trung bình \- cao |
| AR Check-in | Trung bình | Rất cao | Thấp / sau cùng |

---

# **24\. Rủi ro và cách kiểm soát**

## **24.1. Rủi ro mở rộng quá nhanh**

Mô tả:

Dự án có nhiều hướng hấp dẫn như UI/UX, CesiumJS, AI, 3D model, AR. Nếu làm cùng lúc, hệ thống dễ bị rối và khó hoàn thiện.

Cách kiểm soát:

Chỉ làm từng sprint rõ ràng.  
Không bắt đầu CesiumJS khi UI/UX public chưa ổn.  
Không bắt đầu AI khi route/destination data chưa tốt.

---

## **24.2. Rủi ro công nghệ cao làm tăng độ phức tạp**

Mô tả:

CesiumJS, 3D model và AI có thể khiến dự án phức tạp hơn nhiều.

Cách kiểm soát:

Làm bản tối thiểu trước:  
\- CesiumJS chỉ hiển thị marker.  
\- 3D model chỉ xem .glb/.gltf.  
\- AI chỉ hỏi đáp dựa trên dữ liệu ngắn.

---

## **24.3. Rủi ro dữ liệu chưa đủ tốt**

Mô tả:

Nếu dữ liệu địa điểm, ảnh 360, audio và mô tả chưa tốt, các tính năng nâng cao sẽ kém thuyết phục.

Cách kiểm soát:

Ưu tiên chuẩn hóa dữ liệu demo.  
Tạo ít nhất 3 tour 360 chất lượng.  
Mỗi tour nên có 3–5 panorama.  
Mỗi panorama có hotspot rõ ràng.

---

## **24.4. Rủi ro AI trả lời sai**

Mô tả:

AI có thể bịa thông tin nếu không giới hạn nguồn dữ liệu.

Cách kiểm soát:

Dùng dữ liệu nội bộ làm context.  
Nếu thiếu dữ liệu, AI phải nói không có thông tin.  
Không cho AI tự bịa giá vé, giờ mở cửa hoặc thông tin lịch sử chưa có trong database.

---

# **25\. Tiêu chí thành công của v2**

Phiên bản v2 được xem là thành công nếu đạt các tiêu chí:

\- UI public đẹp và dễ dùng hơn MVP.  
\- Tour 360 có trải nghiệm tốt trên desktop và mobile.  
\- Admin có thể tạo tuyến tham quan.  
\- Du khách có thể xem tuyến tham quan trên bản đồ.  
\- Hệ thống ghi nhận được các analytics event cơ bản.  
\- Có hỗ trợ song ngữ Việt \- Anh ở mức dữ liệu chính.  
\- Có ít nhất một chế độ bản đồ 3D bằng CesiumJS.  
\- Có thể demo rõ ràng sự khác biệt giữa MVP và v2.

---

# **26\. Kết luận**

Sau khi hoàn thành MVP, **Smart Tourism 360 Platform** đã có nền tảng kỹ thuật vững chắc để phát triển tiếp. Roadmap v2 nên tập trung trước vào việc nâng cấp trải nghiệm người dùng, sau đó mở rộng tính năng du lịch thông minh và cuối cùng tích hợp các công nghệ cao như WebGIS 3D, mô hình 3D và AI Tour Guide.

Thứ tự phát triển được khuyến nghị là:

Sprint 12: UI/UX Upgrade  
Sprint 13: Route / Itinerary  
Sprint 14: Analytics  
Sprint 15: Multi-language  
Sprint 16: CesiumJS 3D Map  
Sprint 17: 3D Model Viewer  
Sprint 18: AI Tour Guide

Cách đi này giúp dự án phát triển có kiểm soát, không phá vỡ MVP hiện tại và vẫn tạo được các điểm nhấn đủ mạnh để phục vụ demo, báo cáo, portfolio hoặc phát triển thành sản phẩm thực tế.

