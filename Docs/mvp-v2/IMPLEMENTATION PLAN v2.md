# **IMPLEMENTATION PLAN v2**

# **Smart Tourism 360 Platform**

## **Kế hoạch triển khai phiên bản mở rộng sau MVP**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | Implementation Plan v2 |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v2.0 |
| Giai đoạn | Sau MVP |
| Mục đích | Chuyển các tài liệu roadmap, UI/UX, route, analytics, CesiumJS, 3D model và AI thành kế hoạch triển khai theo sprint |
| Đối tượng đọc | Project owner, developer, AI coding agent, giảng viên hướng dẫn, nhóm phát triển |
| Phạm vi | Sprint 12 đến Sprint 18 |
| Hình thức | Checklist theo sprint, có mục tiêu, phạm vi, task, test và tiêu chí hoàn thành |

---

## **2\. Bối cảnh hiện tại**

Dự án **Smart Tourism 360 Platform** đã hoàn thành phiên bản MVP với các chức năng cốt lõi:

\- Public website cho du khách  
\- Admin dashboard cho quản trị viên  
\- Quản lý khu vực, danh mục, địa điểm  
\- Chọn tọa độ địa điểm trên bản đồ  
\- Upload và quản lý media  
\- Tạo virtual tour 360  
\- Tạo panorama  
\- Tạo hotspot thông tin và hotspot điều hướng  
\- Du khách xem bản đồ địa điểm  
\- Du khách xem chi tiết địa điểm  
\- Du khách tham quan 360 và chuyển cảnh giữa panorama  
\- PostgreSQL/PostGIS  
\- MinIO object storage  
\- Docker Compose dev mode và full demo mode

Phiên bản MVP đã chứng minh được hệ thống hoạt động end-to-end. Giai đoạn v2 tập trung vào việc nâng cấp dự án từ một MVP kỹ thuật thành một nền tảng du lịch số thông minh, có trải nghiệm tốt hơn và có các điểm nhấn công nghệ cao.

---

## **3\. Mục tiêu của Implementation Plan v2**

Mục tiêu của kế hoạch triển khai v2 gồm:

1\. Nâng cấp UI/UX để sản phẩm chuyên nghiệp và dễ demo hơn.  
2\. Bổ sung tính năng tuyến tham quan / itinerary.  
3\. Bổ sung analytics để đo lường hành vi người dùng.  
4\. Bổ sung bản đồ 3D/WebGIS bằng CesiumJS.  
5\. Bổ sung khả năng upload và hiển thị mô hình 3D.  
6\. Bổ sung AI Tour Guide dựa trên dữ liệu nội bộ.  
7\. Giữ ổn định toàn bộ chức năng MVP hiện có.  
8\. Mỗi sprint phải tạo ra kết quả có thể demo được.

---

## **4\. Nguyên tắc triển khai v2**

### **4.1. Không phá MVP**

Các chức năng MVP sau phải luôn được giữ ổn định:

\- Admin login  
\- Destination management  
\- Map location picker  
\- Media upload  
\- Virtual tour  
\- Panorama management  
\- Hotspot navigation  
\- Public map  
\- Public destination detail  
\- Public 360 tour viewer  
\- Docker dev/full mode

Mỗi sprint v2 đều phải có bước regression test để đảm bảo các chức năng trên không bị hỏng.

---

### **4.2. Không làm quá nhiều công nghệ cao cùng lúc**

v2 có nhiều hướng hấp dẫn như CesiumJS, model 3D, AI, analytics. Nếu làm đồng thời, dự án sẽ khó kiểm soát.

Thứ tự triển khai được khuyến nghị:

UI/UX Upgrade  
→ Route / Itinerary  
→ Analytics  
→ CesiumJS 3D Map  
→ 3D Model Viewer  
→ AI Tour Guide

---

### **4.3. Mỗi sprint phải có đầu ra rõ ràng**

Mỗi sprint phải trả lời được:

\- Sprint này thêm giá trị gì?  
\- Người dùng/admin có thể demo gì sau sprint?  
\- Có làm hỏng chức năng cũ không?  
\- Có cần migration/database mới không?  
\- Có cần cập nhật README/docs không?

---

### **4.4. Dùng AI coding agent theo phạm vi nhỏ**

Không yêu cầu AI agent làm toàn bộ v2 trong một lần. Mỗi lần chỉ nên làm một sprint hoặc một phần của sprint.

Ví dụ đúng:

Hãy triển khai Sprint 13 \- Route / Itinerary Module.  
Chỉ làm các task trong Sprint 13\.  
Không làm Analytics, CesiumJS, AI hoặc 3D Model trong sprint này.

Ví dụ không nên dùng:

Hãy nâng cấp toàn bộ dự án lên v2.

---

## **5\. Roadmap tổng quan v2**

| Sprint | Tên sprint | Trọng tâm | Kết quả đầu ra | Trạng thái |
| ----- | ----- | ----- | ----- | ----- |
| Sprint 12 | UI/UX Upgrade | Nâng cấp giao diện public, tour 360, admin | Giao diện đẹp hơn, dễ dùng hơn | [x] Đã hoàn thành |
| Sprint 13 | Route / Itinerary | Tuyến tham quan | Admin tạo tuyến, du khách xem tuyến trên bản đồ | [ ] Chưa thực hiện |
| Sprint 14 | Analytics & Tracking | Ghi nhận hành vi người dùng | Dashboard thống kê cơ bản | [ ] Chưa thực hiện |
| Sprint 15 | Multi-language Support | Song ngữ Việt \- Anh | Public data hỗ trợ languageCode | [ ] Chưa thực hiện |
| Sprint 16 | CesiumJS 3D Map | Bản đồ 3D/WebGIS | Trang `/explore-3d` với marker 3D | [ ] Chưa thực hiện |
| Sprint 17 | 3D Model Viewer | Upload/xem model 3D | Admin upload `.glb`, du khách xem model | [ ] Chưa thực hiện |
| Sprint 18 | AI Tour Guide | Trợ lý AI du lịch | Chatbot hỏi đáp dựa trên dữ liệu nội bộ | [ ] Chưa thực hiện |

Ghi chú: Sprint 15 được thêm vào roadmap để lấp khoảng trống giữa Analytics và CesiumJS. Nếu bạn muốn ưu tiên công nghệ cao, Sprint 15 có thể tạm hoãn và chuyển CesiumJS lên trước.

---

# **SPRINT 12: UI/UX UPGRADE**

---

## **6\. Sprint 12 \- UI/UX Upgrade & Public Experience Polish**

### **6.1. Mục tiêu**

Nâng cấp giao diện và trải nghiệm người dùng cho public website, tour 360 viewer và admin dashboard mà không thay đổi lớn về database/API.

Sau sprint này, hệ thống phải tạo cảm giác chuyên nghiệp hơn khi demo.

---

## **6.2. Phạm vi Sprint 12**

Trong phạm vi:

\- Redesign Home Page  
\- Redesign Explore Map Page  
\- Redesign Destination Card  
\- Redesign Destination Detail Page  
\- Improve Tour 360 Viewer  
\- Add Scene Thumbnail List  
\- Add First-time Guide Overlay  
\- Improve Hotspot UI  
\- Improve Admin Dashboard visual style  
\- Add loading/empty/error states  
\- Improve responsive layout

Ngoài phạm vi:

\- Không làm Route/Itinerary  
\- Không làm Analytics  
\- Không làm CesiumJS  
\- Không làm 3D Model  
\- Không làm AI  
\- Không thay Leaflet  
\- Không thay Photo Sphere Viewer  
\- Không thay đổi database lớn

---

## **6.3. Checklist Public Home Page**

\- \[ \] Redesign hero section.  
\- \[ \] Thêm ảnh/video nền hoặc ảnh du lịch nổi bật.  
\- \[ \] Thêm CTA chính “Khám phá bản đồ”.  
\- \[ \] Thêm CTA phụ “Trải nghiệm tour 360”.  
\- \[ \] Thêm section Featured Categories.  
\- \[ \] Thêm section Featured Destinations.  
\- \[ \] Thêm section 360 Experience.  
\- \[ \] Thêm animation fade/slide nhẹ.  
\- \[ \] Kiểm tra responsive mobile.

---

## **6.4. Checklist Explore Map Page**

\- \[ \] Cải thiện layout desktop.  
\- \[ \] Cải thiện layout mobile.  
\- \[ \] Tạo search/filter bar trực quan hơn.  
\- \[ \] Category filter dạng chip.  
\- \[ \] Marker popup đẹp hơn.  
\- \[ \] Hiển thị badge “Có tour 360”.  
\- \[ \] Thêm empty state khi không có kết quả.  
\- \[ \] Thêm loading state khi tải marker.  
\- \[ \] Thêm nút chuyển sang 3D Map placeholder nếu muốn chuẩn bị Sprint 16\.

---

## **6.5. Checklist Destination List & Card**

\- \[ \] Tạo DestinationCard v2.  
\- \[ \] Card có cover image.  
\- \[ \] Card có category badge.  
\- \[ \] Card có tour 360 badge.  
\- \[ \] Card có mô tả ngắn.  
\- \[ \] Card có CTA “Xem chi tiết”.  
\- \[ \] Hover effect cho desktop.  
\- \[ \] Grid responsive: desktop 3 cột, tablet 2 cột, mobile 1 cột.

---

## **6.6. Checklist Destination Detail Page**

\- \[ \] Hero image lớn.  
\- \[ \] Overlay gradient.  
\- \[ \] Category badge.  
\- \[ \] CTA “Tham quan 360” nổi bật.  
\- \[ \] Destination info panel.  
\- \[ \] Gallery ảnh.  
\- \[ \] Audio/video section nếu có.  
\- \[ \] Bản đồ vị trí nhỏ.  
\- \[ \] Nearby destinations nếu có thể.  
\- \[ \] Responsive mobile.

---

## **6.7. Checklist Tour 360 Viewer**

\- \[ \] Tạo Tour360TopBar.  
\- \[ \] Tạo Tour360ControlBar.  
\- \[ \] Tạo SceneListPanel.  
\- \[ \] Scene list có thumbnail/title.  
\- \[ \] Thêm first-time guide overlay.  
\- \[ \] Thêm hướng dẫn: kéo để xoay, bấm mũi tên để di chuyển.  
\- \[ \] Cải thiện hotspot icon.  
\- \[ \] Hotspot navigation có hiệu ứng pulse nhẹ.  
\- \[ \] Hotspot label khi hover/tap.  
\- \[ \] Transition mượt khi chuyển panorama.  
\- \[ \] Mobile bottom sheet cho hotspot info/audio.  
\- \[ \] Loading overlay khi đổi panorama.

---

## **6.8. Checklist Admin Dashboard**

\- \[ \] Cải thiện admin stat cards.  
\- \[ \] Cải thiện destination table.  
\- \[ \] Cải thiện status/category/media badges.  
\- \[ \] Form tạo/sửa địa điểm chia section rõ ràng.  
\- \[ \] Cải thiện Media Library card.  
\- \[ \] Cải thiện Panorama card.  
\- \[ \] Cải thiện Hotspot Editor side panel.  
\- \[ \] Giữ nguyên logic CRUD hiện có.

---

## **6.9. Checklist Test Sprint 12**

\- \[ \] Public home page không lỗi.  
\- \[ \] Explore map load marker đúng.  
\- \[ \] Destination detail mở đúng.  
\- \[ \] Tour 360 load đúng.  
\- \[ \] Hotspot navigation vẫn chuyển panorama.  
\- \[ \] Admin login vẫn hoạt động.  
\- \[ \] Admin tạo/sửa destination vẫn hoạt động.  
\- \[ \] Upload media vẫn hoạt động.  
\- \[ \] npm run build thành công.  
\- \[ \] docker compose up \-d \--build thành công.

---

## **6.10. Tiêu chí hoàn thành Sprint 12**

Sprint 12 hoàn thành khi:

\- Public website đẹp và nhất quán hơn MVP.  
\- Tour 360 có scene list và hướng dẫn lần đầu.  
\- Hotspot UI dễ hiểu hơn.  
\- Admin dashboard dễ nhìn hơn.  
\- Mobile public không vỡ layout.  
\- Chức năng MVP không bị hỏng.

---

# **SPRINT 13: ROUTE / ITINERARY MODULE**

---

## **7\. Sprint 13 \- Route / Itinerary**

### **7.1. Mục tiêu**

Bổ sung tính năng tuyến tham quan để admin có thể nhóm nhiều địa điểm thành một hành trình, còn du khách có thể xem tuyến trên bản đồ và mở từng địa điểm/tour 360\.

---

## **7.2. Phạm vi Sprint 13**

Trong phạm vi:

\- Bảng routes  
\- Bảng route\_destinations  
\- Admin route management  
\- Admin route destination manager  
\- Public route list page  
\- Public route detail page  
\- Route map bằng Leaflet  
\- Polyline nối các địa điểm theo thứ tự

Ngoài phạm vi:

\- Không tính đường đi thật.  
\- Không GPS navigation.  
\- Không booking.  
\- Không AI itinerary.  
\- Không CesiumJS route 3D trong sprint này.

---

## **7.3. Checklist Database**

\- \[ \] Tạo entity Route.  
\- \[ \] Tạo entity RouteDestination.  
\- \[ \] Tạo bảng routes.  
\- \[ \] Tạo bảng route\_destinations.  
\- \[ \] Thêm quan hệ routes.region\_id.  
\- \[ \] Thêm quan hệ route\_destinations.route\_id.  
\- \[ \] Thêm quan hệ route\_destinations.destination\_id.  
\- \[ \] Thêm index region\_id.  
\- \[ \] Thêm unique index region\_id \+ slug.  
\- \[ \] Thêm unique index route\_id \+ destination\_id.  
\- \[ \] Tạo migration.  
\- \[ \] Seed 2-3 route mẫu.

---

## **7.4. Checklist Backend API**

\- \[ \] Tạo Route DTOs.  
\- \[ \] Tạo RouteDestination DTOs.  
\- \[ \] Tạo RouteService.  
\- \[ \] Tạo public RoutesController.  
\- \[ \] Tạo AdminRoutesController.  
\- \[ \] GET /api/routes.  
\- \[ \] GET /api/routes/{id}.  
\- \[ \] GET /api/admin/routes.  
\- \[ \] POST /api/admin/routes.  
\- \[ \] PUT /api/admin/routes/{id}.  
\- \[ \] PATCH /api/admin/routes/{id}/status.  
\- \[ \] DELETE /api/admin/routes/{id}.  
\- \[ \] POST /api/admin/routes/{routeId}/destinations.  
\- \[ \] POST /api/admin/routes/{routeId}/destinations/bulk.  
\- \[ \] PATCH /api/admin/routes/{routeId}/destinations/reorder.  
\- \[ \] DELETE /api/admin/routes/{routeId}/destinations/{routeDestinationId}.  
\- \[ \] Validate slug không trùng trong cùng region.  
\- \[ \] Validate route phải có ít nhất 2 destinations trước khi publish.

---

## **7.5. Checklist Admin Frontend**

\- \[ \] Tạo /admin/routes.  
\- \[ \] Tạo route table.  
\- \[ \] Tạo filter theo status/theme/keyword.  
\- \[ \] Tạo /admin/routes/create.  
\- \[ \] Tạo /admin/routes/:id/edit.  
\- \[ \] Tạo route form.  
\- \[ \] Tạo /admin/routes/:id/destinations.  
\- \[ \] Tạo destination picker.  
\- \[ \] Tạo route destination list.  
\- \[ \] Cho phép thêm destination vào route.  
\- \[ \] Cho phép remove destination khỏi route.  
\- \[ \] Cho phép reorder bằng nút up/down.  
\- \[ \] Cho phép sửa estimatedStayTime/note.  
\- \[ \] Tạo preview route map.

---

## **7.6. Checklist Public Frontend**

\- \[ \] Tạo /routes.  
\- \[ \] Tạo route list page.  
\- \[ \] Tạo RouteCard.  
\- \[ \] Tạo /routes/:id hoặc /routes/:slug.  
\- \[ \] Tạo route detail page.  
\- \[ \] Tạo RouteMap bằng Leaflet.  
\- \[ \] Hiển thị marker đánh số.  
\- \[ \] Vẽ polyline nối điểm theo displayOrder.  
\- \[ \] Tạo itinerary timeline.  
\- \[ \] Mỗi item có nút xem destination detail.  
\- \[ \] Mỗi item có nút tour 360 nếu hasVirtualTour.  
\- \[ \] Empty/loading/error states.

---

## **7.7. Checklist Test Sprint 13**

\- \[ \] Admin tạo route mới.  
\- \[ \] Admin thêm nhiều địa điểm vào route.  
\- \[ \] Admin reorder địa điểm.  
\- \[ \] Admin publish route.  
\- \[ \] Public route list chỉ hiển thị route published.  
\- \[ \] Public route detail hiển thị đúng thứ tự.  
\- \[ \] Route map hiển thị marker đúng tọa độ.  
\- \[ \] Polyline nối đúng thứ tự.  
\- \[ \] Click destination mở trang chi tiết.  
\- \[ \] Click tour 360 mở tour.  
\- \[ \] Không làm hỏng destination/tour cũ.

---

## **7.8. Tiêu chí hoàn thành Sprint 13**

Sprint 13 hoàn thành khi:

\- Admin quản lý được tuyến tham quan.  
\- Admin thêm/sắp xếp địa điểm trong tuyến.  
\- Du khách xem được danh sách tuyến.  
\- Du khách xem chi tiết tuyến trên bản đồ.  
\- Du khách mở được địa điểm/tour từ tuyến.  
\- Có ít nhất 2-3 tuyến mẫu.

---

# **SPRINT 14: ANALYTICS & TRACKING**

---

## **8\. Sprint 14 \- Analytics & Tracking**

### **8.1. Mục tiêu**

Bổ sung hệ thống ghi nhận hành vi người dùng và dashboard thống kê cơ bản cho admin.

---

## **8.2. Phạm vi Sprint 14**

Trong phạm vi:

\- Bảng analytics\_events  
\- Public tracking API  
\- Admin analytics API  
\- Frontend tracking service  
\- SessionId ẩn danh  
\- Admin analytics dashboard

Ngoài phạm vi:

\- Không realtime dashboard.  
\- Không Google Analytics.  
\- Không AI recommendation.  
\- Không tracking dữ liệu cá nhân nhạy cảm.

---

## **8.3. Checklist Database**

\- \[ \] Tạo entity AnalyticsEvent.  
\- \[ \] Tạo bảng analytics\_events.  
\- \[ \] Thêm event\_name.  
\- \[ \] Thêm target\_type.  
\- \[ \] Thêm target\_id.  
\- \[ \] Thêm session\_id.  
\- \[ \] Thêm metadata jsonb.  
\- \[ \] Thêm created\_at.  
\- \[ \] Index event\_name.  
\- \[ \] Index target\_type \+ target\_id.  
\- \[ \] Index created\_at.  
\- \[ \] Index session\_id.  
\- \[ \] Tạo migration.

---

## **8.4. Checklist Backend API**

\- \[ \] Tạo TrackEventRequest.  
\- \[ \] Tạo AnalyticsService.  
\- \[ \] POST /api/analytics/events.  
\- \[ \] Validate eventName.  
\- \[ \] Validate metadata size.  
\- \[ \] GET /api/admin/analytics/summary.  
\- \[ \] GET /api/admin/analytics/top-destinations.  
\- \[ \] GET /api/admin/analytics/top-tours.  
\- \[ \] GET /api/admin/analytics/top-hotspots.  
\- \[ \] GET /api/admin/analytics/top-routes nếu Route module đã có.  
\- \[ \] GET /api/admin/analytics/events-by-day.  
\- \[ \] Admin analytics API yêu cầu JWT.

---

## **8.5. Checklist Frontend Tracking**

\- \[ \] Tạo analytics.service.js.  
\- \[ \] Tạo session.js.  
\- \[ \] Tự tạo sessionId nếu chưa có.  
\- \[ \] Track view\_destination.  
\- \[ \] Track click\_map\_marker.  
\- \[ \] Track open\_virtual\_tour.  
\- \[ \] Track view\_panorama.  
\- \[ \] Track click\_hotspot.  
\- \[ \] Track navigate\_panorama.  
\- \[ \] Track view\_route.  
\- \[ \] Tracking lỗi không làm crash UI.

---

## **8.6. Checklist Admin Analytics UI**

\- \[ \] Tạo /admin/analytics.  
\- \[ \] Thêm menu Analytics vào sidebar.  
\- \[ \] Tạo date filter 7 ngày/30 ngày.  
\- \[ \] Tạo summary cards.  
\- \[ \] Tạo chart events by day hoặc table.  
\- \[ \] Tạo top destinations list.  
\- \[ \] Tạo top tours list.  
\- \[ \] Tạo top hotspots list.  
\- \[ \] Tạo top routes list nếu có.  
\- \[ \] Empty/loading/error states.

---

## **8.7. Checklist Test Sprint 14**

\- \[ \] Mở destination detail tạo event.  
\- \[ \] Mở tour 360 tạo event.  
\- \[ \] Click hotspot tạo event.  
\- \[ \] Chuyển panorama tạo event.  
\- \[ \] Mở route tạo event.  
\- \[ \] Event được lưu vào database.  
\- \[ \] Admin dashboard hiển thị số liệu.  
\- \[ \] API admin không token trả 401\.  
\- \[ \] Tracking API lỗi không ảnh hưởng UI.

---

## **8.8. Tiêu chí hoàn thành Sprint 14**

Sprint 14 hoàn thành khi:

\- Hệ thống ghi nhận được event public.  
\- Admin xem được dashboard analytics.  
\- Có summary cards và top lists.  
\- Không thu thập dữ liệu cá nhân nhạy cảm.  
\- Chức năng cũ không bị ảnh hưởng.

---

# **SPRINT 15: MULTI-LANGUAGE SUPPORT**

---

## **9\. Sprint 15 \- Multi-language Support**

### **9.1. Mục tiêu**

Bổ sung hỗ trợ song ngữ Việt \- Anh cho dữ liệu public, chuẩn bị nền tảng phục vụ du khách ngoài địa phương.

---

## **9.2. Phạm vi Sprint 15**

Trong phạm vi:

\- Language switcher frontend  
\- Bảng translations  
\- Admin translation UI cơ bản  
\- Public API hỗ trợ languageCode  
\- Fallback về tiếng Việt nếu chưa có bản dịch

Ngoài phạm vi:

\- Không dịch tự động bằng AI trong sprint này.  
\- Không hỗ trợ quá nhiều ngôn ngữ.  
\- Không dịch toàn bộ admin UI nếu chưa cần.  
\- Không voice/audio đa ngôn ngữ nâng cao.

---

## **9.3. Checklist Database**

\- \[ \] Tạo bảng translations nếu chưa có.  
\- \[ \] entity\_type.  
\- \[ \] entity\_id.  
\- \[ \] field\_name.  
\- \[ \] language\_code.  
\- \[ \] translated\_value.  
\- \[ \] created\_at.  
\- \[ \] updated\_at.  
\- \[ \] Unique index entity\_type \+ entity\_id \+ field\_name \+ language\_code.  
\- \[ \] Tạo migration.

---

## **9.4. Checklist Backend API**

\- \[ \] Tạo Translation entity.  
\- \[ \] Tạo Translation DTOs.  
\- \[ \] Tạo TranslationService.  
\- \[ \] GET /api/admin/translations.  
\- \[ \] POST /api/admin/translations.  
\- \[ \] PUT /api/admin/translations/{id}.  
\- \[ \] DELETE /api/admin/translations/{id}.  
\- \[ \] Public destination API nhận languageCode.  
\- \[ \] Public route API nhận languageCode.  
\- \[ \] Public tour API nhận languageCode nếu cần.  
\- \[ \] Fallback tiếng Việt nếu không có translation.

---

## **9.5. Checklist Frontend Public**

\- \[ \] Tạo language switcher VI/EN.  
\- \[ \] Lưu language trong localStorage.  
\- \[ \] Gửi languageCode khi gọi API.  
\- \[ \] Cập nhật home/destination/routes/tour để dùng translated data.  
\- \[ \] Fallback nếu dữ liệu dịch thiếu.

---

## **9.6. Checklist Admin Translation UI**

\- \[ \] Tạo /admin/translations hoặc tab translation trong form.  
\- \[ \] Admin chọn entity type.  
\- \[ \] Admin chọn entity.  
\- \[ \] Admin chọn language.  
\- \[ \] Admin nhập bản dịch cho name/title/description.  
\- \[ \] Lưu bản dịch.  
\- \[ \] Hiển thị trạng thái đã dịch/chưa dịch.

---

## **9.7. Checklist Test Sprint 15**

\- \[ \] Public chuyển VI/EN được.  
\- \[ \] Destination detail hiển thị bản dịch nếu có.  
\- \[ \] Route detail hiển thị bản dịch nếu có.  
\- \[ \] Nếu thiếu bản dịch, fallback tiếng Việt.  
\- \[ \] Admin tạo/cập nhật translation được.  
\- \[ \] Không làm hỏng public API cũ.

---

## **9.8. Tiêu chí hoàn thành Sprint 15**

Sprint 15 hoàn thành khi:

\- Frontend có language switcher.  
\- Public API hỗ trợ languageCode.  
\- Admin có thể nhập bản dịch cơ bản.  
\- Hệ thống fallback tiếng Việt khi thiếu bản dịch.

---

# **SPRINT 16: CESIUMJS 3D MAP**

---

## **10\. Sprint 16 \- CesiumJS / WebGIS 3D Integration**

### **10.1. Mục tiêu**

Bổ sung chế độ bản đồ 3D bằng CesiumJS, cho phép du khách khám phá các địa điểm trên không gian 3D và mở chi tiết/tour 360 từ marker 3D.

---

## **10.2. Phạm vi Sprint 16**

Trong phạm vi:

\- Cài đặt CesiumJS  
\- Tạo /explore-3d  
\- Render destination markers  
\- Click marker mở popup  
\- FlyTo destination  
\- Toggle 2D/3D  
\- Route polyline 3D nếu Route module đã có

Ngoài phạm vi:

\- Không thay Leaflet.  
\- Không 3D Tiles.  
\- Không Digital Twin đầy đủ.  
\- Không ArcGIS.  
\- Không routing đường bộ thật.

---

## **10.3. Checklist Frontend Setup**

\- \[ \] Cài CesiumJS.  
\- \[ \] Cấu hình Vite load Cesium assets.  
\- \[ \] Thêm VITE\_CESIUM\_ION\_TOKEN nếu cần.  
\- \[ \] Cập nhật .env.example.  
\- \[ \] Tạo route /explore-3d.  
\- \[ \] Lazy load CesiumMapPage.

---

## **10.4. Checklist Cesium Components**

\- \[ \] Tạo CesiumMapPage.vue.  
\- \[ \] Tạo CesiumViewer.vue.  
\- \[ \] Tạo CesiumDestinationPopup.vue.  
\- \[ \] Tạo CesiumMapToolbar.vue.  
\- \[ \] Tạo CesiumRouteLayer.vue.  
\- \[ \] Tạo CesiumLoadingOverlay.vue.  
\- \[ \] Tạo CesiumErrorState.vue.

---

## **10.5. Checklist Destination Marker**

\- \[ \] Gọi /api/destinations/map.  
\- \[ \] Render marker bằng Cartesian3.fromDegrees(longitude, latitude).  
\- \[ \] Marker có màu theo category.  
\- \[ \] Label tên địa điểm.  
\- \[ \] Click marker mở popup.  
\- \[ \] Popup có cover image.  
\- \[ \] Popup có category badge.  
\- \[ \] Popup có nút Xem chi tiết.  
\- \[ \] Popup có nút Tour 360 nếu có.

---

## **10.6. Checklist Camera & Route**

\- \[ \] Camera mặc định bay đến region center.  
\- \[ \] FlyTo destination hoạt động.  
\- \[ \] Reset camera hoạt động.  
\- \[ \] Nếu query destinationId, tự flyTo.  
\- \[ \] Nếu query routeId, vẽ route polyline.  
\- \[ \] Route marker có số thứ tự.  
\- \[ \] Có ghi chú polyline chỉ minh họa thứ tự tham quan.

---

## **10.7. Checklist Security/Config**

\- \[ \] Không commit Cesium token thật.  
\- \[ \] Dùng .env.local.  
\- \[ \] .env.example có VITE\_CESIUM\_ION\_TOKEN placeholder.  
\- \[ \] README có hướng dẫn cấu hình token.

---

## **10.8. Checklist Test Sprint 16**

\- \[ \] /explore-3d mở được.  
\- \[ \] Cesium globe hiển thị.  
\- \[ \] Marker đúng tọa độ.  
\- \[ \] Click marker mở popup đúng địa điểm.  
\- \[ \] FlyTo hoạt động.  
\- \[ \] Xem chi tiết hoạt động.  
\- \[ \] Tour 360 từ popup hoạt động.  
\- \[ \] /explore 2D vẫn hoạt động.  
\- \[ \] npm run build thành công.

---

## **10.9. Tiêu chí hoàn thành Sprint 16**

Sprint 16 hoàn thành khi:

\- Có bản đồ 3D CesiumJS độc lập.  
\- Destination markers hiển thị đúng.  
\- Popup marker có hành động rõ.  
\- 2D map không bị ảnh hưởng.  
\- Không lộ token thật.

---

# **SPRINT 17: 3D MODEL VIEWER**

---

## **11\. Sprint 17 \- 3D Model Feature**

### **11.1. Mục tiêu**

Bổ sung khả năng upload, quản lý và hiển thị mô hình 3D cho địa điểm hoặc hotspot.

---

## **11.2. Phạm vi Sprint 17**

Trong phạm vi:

\- Hỗ trợ mediaType model3d  
\- Upload .glb/.gltf  
\- Bảng models\_3d  
\- Admin model management  
\- Public ModelViewer bằng \<model-viewer\>  
\- Hiển thị model trong destination detail  
\- Optional: model hotspot trong tour 360

Ngoài phạm vi:

\- Không scan 3D.  
\- Không chỉnh sửa model trên web.  
\- Không AR nâng cao.  
\- Không 3D Tiles.  
\- Không đặt model lên Cesium terrain trong sprint này.

---

## **11.3. Checklist Database**

\- \[ \] Thêm mediaType model3d nếu dùng enum.  
\- \[ \] Tạo entity Model3D.  
\- \[ \] Tạo bảng models\_3d.  
\- \[ \] media\_id.  
\- \[ \] thumbnail\_id.  
\- \[ \] target\_type.  
\- \[ \] target\_id.  
\- \[ \] format.  
\- \[ \] polygon\_count.  
\- \[ \] status.  
\- \[ \] created\_by/updated\_by.  
\- \[ \] Tạo migration.

---

## **11.4. Checklist Backend API**

\- \[ \] Cập nhật Media Upload hỗ trợ .glb.  
\- \[ \] Validate .glb/.gltf extension.  
\- \[ \] Validate file size.  
\- \[ \] Tạo Model3D DTOs.  
\- \[ \] Tạo Model3DService.  
\- \[ \] GET /api/destinations/{destinationId}/models-3d.  
\- \[ \] GET /api/models-3d/{id}.  
\- \[ \] GET /api/admin/models-3d.  
\- \[ \] POST /api/admin/models-3d.  
\- \[ \] PUT /api/admin/models-3d/{id}.  
\- \[ \] PATCH /api/admin/models-3d/{id}/status.  
\- \[ \] DELETE /api/admin/models-3d/{id}.  
\- \[ \] Public API chỉ trả published.  
\- \[ \] Admin API yêu cầu JWT.

---

## **11.5. Checklist Frontend Public**

\- \[ \] Cài @google/model-viewer.  
\- \[ \] Tạo ModelViewer.vue.  
\- \[ \] Tạo ModelSection.vue.  
\- \[ \] Gọi API model theo destination.  
\- \[ \] Hiển thị model trong destination detail.  
\- \[ \] Loading state.  
\- \[ \] Error state.  
\- \[ \] Hướng dẫn “Kéo để xoay, cuộn để phóng to”.  
\- \[ \] Optional: ModelHotspotModal trong tour 360\.

---

## **11.6. Checklist Frontend Admin**

\- \[ \] Media upload hỗ trợ model3d.  
\- \[ \] Media Library hiển thị badge model3d.  
\- \[ \] Tạo /admin/models-3d.  
\- \[ \] Tạo model list/grid.  
\- \[ \] Tạo model create/edit form.  
\- \[ \] Tạo target selector cho destination.  
\- \[ \] Tạo model preview trong admin.  
\- \[ \] Publish/archive model.

---

## **11.7. Checklist Storage**

\- \[ \] MinIO upload được .glb.  
\- \[ \] Browser load được fileUrl.  
\- \[ \] CORS đúng.  
\- \[ \] Content-Type không bị chặn.  
\- \[ \] Không commit model nặng vào repo.  
\- \[ \] Nếu dùng sample model, ghi credits/license.

---

## **11.8. Checklist Test Sprint 17**

\- \[ \] Upload .glb thành công.  
\- \[ \] Upload file sai định dạng bị chặn.  
\- \[ \] Admin tạo metadata model.  
\- \[ \] Admin gắn model với destination.  
\- \[ \] Public destination detail hiển thị model.  
\- \[ \] User xoay/zoom model được.  
\- \[ \] Model lỗi hiển thị error state.  
\- \[ \] Upload ảnh/audio/panorama cũ vẫn hoạt động.  
\- \[ \] Tour 360 vẫn hoạt động.

---

## **11.9. Tiêu chí hoàn thành Sprint 17**

Sprint 17 hoàn thành khi:

\- Admin upload được model .glb.  
\- Model được lưu vào MinIO.  
\- Metadata lưu trong database.  
\- Public hiển thị model trong destination detail.  
\- Không làm hỏng media upload cũ và tour 360\.

---

# **SPRINT 18: AI TOUR GUIDE**

---

## **12\. Sprint 18 \- AI Tour Guide**

### **12.1. Mục tiêu**

Bổ sung AI Tour Guide để du khách hỏi đáp về địa điểm, tour 360, route và hỗ trợ admin tạo bản nháp nội dung.

---

## **12.2. Phạm vi Sprint 18**

Trong phạm vi:

\- Public AI chat widget  
\- Backend AI chat API  
\- Context từ dữ liệu nội bộ  
\- Admin AI generate content  
\- AI trong Tour 360 với context panorama  
\- Safety fallback khi thiếu dữ liệu

Ngoài phạm vi:

\- Không fine-tune model.  
\- Không training model riêng.  
\- Không voice realtime.  
\- Không AI booking.  
\- Không AI tự crawl Internet.  
\- Không AI tự cập nhật database.  
\- Không bắt buộc RAG/pgvector nâng cao.

---

## **12.3. Checklist Backend**

\- \[ \] Thêm cấu hình AI provider.  
\- \[ \] Không commit AI\_API\_KEY.  
\- \[ \] Cập nhật .env.example.  
\- \[ \] Tạo LLMProviderService.  
\- \[ \] Tạo ContextBuilderService.  
\- \[ \] Tạo AIChatService.  
\- \[ \] Tạo AIContentService.  
\- \[ \] POST /api/ai/chat.  
\- \[ \] POST /api/admin/ai/generate-content.  
\- \[ \] Validate message length.  
\- \[ \] Fallback khi thiếu context.  
\- \[ \] Fallback khi AI provider lỗi.  
\- \[ \] Optional: tạo bảng ai\_chat\_logs.

---

## **12.4. Checklist Context Builder**

\- \[ \] Lấy context destination.  
\- \[ \] Lấy context category/region.  
\- \[ \] Lấy context route.  
\- \[ \] Lấy context tour.  
\- \[ \] Lấy context panorama.  
\- \[ \] Lấy context hotspot.  
\- \[ \] Lấy context audio transcript nếu có.  
\- \[ \] Lấy context model 3D nếu có.  
\- \[ \] Giới hạn context không quá dài.  
\- \[ \] Nếu không có dữ liệu, trả fallback.

---

## **12.5. Checklist Frontend Public**

\- \[ \] Tạo AIChatWidget.  
\- \[ \] Tạo floating AI button.  
\- \[ \] Tạo message list.  
\- \[ \] Tạo suggestion chips.  
\- \[ \] Tạo typing indicator.  
\- \[ \] Gửi context theo page hiện tại.  
\- \[ \] Trang home có câu hỏi gợi ý.  
\- \[ \] Destination detail gửi destinationId.  
\- \[ \] Route detail gửi routeId.  
\- \[ \] AI answer hiển thị suggestions nếu có.  
\- \[ \] Error state thân thiện.

---

## **12.6. Checklist Tour 360 AI**

\- \[ \] Thêm nút “Hỏi AI” trong Tour 360\.  
\- \[ \] Gửi destinationId.  
\- \[ \] Gửi tourId.  
\- \[ \] Gửi panoramaId hiện tại.  
\- \[ \] Suggested questions theo panorama.  
\- \[ \] Chat panel không che toàn bộ viewer.  
\- \[ \] AI trả lời dựa trên panorama/hotspot hiện tại.

---

## **12.7. Checklist Admin AI**

\- \[ \] Thêm AI Generate trong Destination Form.  
\- \[ \] Thêm AI Generate cho hotspot description nếu có.  
\- \[ \] Thêm AI Generate audio script nếu có form audio guide.  
\- \[ \] Tạo AI draft modal.  
\- \[ \] Admin có thể copy draft.  
\- \[ \] Admin có thể insert draft vào field.  
\- \[ \] Hiển thị warning cần kiểm duyệt.  
\- \[ \] Admin AI API yêu cầu JWT.

---

## **12.8. Checklist Safety**

\- \[ \] AI không bịa giá vé nếu database không có.  
\- \[ \] AI không bịa giờ mở cửa nếu database không có.  
\- \[ \] AI không bịa địa điểm không tồn tại.  
\- \[ \] AI nói “hệ thống chưa có đủ dữ liệu” khi thiếu context.  
\- \[ \] Admin draft có cảnh báo cần kiểm duyệt.  
\- \[ \] Public AI không thu thập dữ liệu cá nhân.

---

## **12.9. Checklist Test Sprint 18**

\- \[ \] Public chat mở/đóng được.  
\- \[ \] User hỏi về destination và AI trả lời được.  
\- \[ \] User hỏi route và AI gợi ý route có sẵn.  
\- \[ \] User hỏi thông tin không có dữ liệu và AI không bịa.  
\- \[ \] Tour 360 gửi đúng panorama context.  
\- \[ \] Admin tạo bản nháp mô tả bằng AI.  
\- \[ \] AI provider lỗi hiển thị lỗi thân thiện.  
\- \[ \] Nếu chưa cấu hình AI key, app vẫn chạy được.  
\- \[ \] Docker build vẫn thành công.

---

## **12.10. Tiêu chí hoàn thành Sprint 18**

Sprint 18 hoàn thành khi:

\- Public AI chat widget hoạt động.  
\- AI trả lời dựa trên dữ liệu nội bộ.  
\- AI có fallback khi thiếu dữ liệu.  
\- Admin tạo được bản nháp nội dung bằng AI.  
\- Tour 360 có nút hỏi AI theo context hiện tại.  
\- API key không bị commit.  
\- Chức năng cũ không bị ảnh hưởng.

---

# **13\. Quy trình làm việc với AI Coding Agent cho v2**

## **13.1. Master prompt cho v2**

Bạn là Senior Full-stack Developer đang hỗ trợ phát triển Smart Tourism 360 Platform v2.

Dự án đã hoàn thành MVP. Nhiệm vụ của bạn là phát triển các sprint mở rộng sau MVP dựa trên bộ tài liệu v2 trong thư mục docs.

Quy tắc quan trọng:  
1\. Trước khi code, hãy đọc tài liệu liên quan đến sprint hiện tại.  
2\. Chỉ triển khai đúng phạm vi sprint được yêu cầu.  
3\. Không tự ý làm sang sprint khác.  
4\. Không phá các chức năng MVP hiện có.  
5\. Không thay đổi database/API lớn nếu tài liệu không yêu cầu.  
6\. Sau mỗi sprint, phải báo cáo file đã tạo/sửa, API đã thêm, migration đã tạo, component đã thêm và cách test.  
7\. Không commit secret, API key, token thật.  
8\. Nếu gặp điểm chưa rõ, hãy nêu giả định trước khi triển khai.

---

## **13.2. Prompt mẫu cho từng sprint**

Hãy triển khai Sprint \[số\] \- \[tên sprint\] cho dự án Smart Tourism 360 Platform v2.

Trước khi code:  
1\. Đọc tài liệu \[tên tài liệu\].  
2\. Đọc Implementation Plan v2.  
3\. Tóm tắt lại bạn hiểu sprint này cần làm gì.  
4\. Chỉ triển khai đúng phạm vi sprint này.

Không được làm:  
\- Không làm sang sprint khác.  
\- Không phá MVP.  
\- Không tự thêm công nghệ ngoài tài liệu.  
\- Không commit secret/token/API key.

Sau khi code xong, hãy báo cáo:  
1\. File đã tạo/sửa.  
2\. Migration đã tạo nếu có.  
3\. API đã thêm.  
4\. Component frontend đã thêm.  
5\. Cách chạy/test.  
6\. Checklist hoàn thành.  
7\. Phần chưa hoàn thành hoặc rủi ro còn lại.

---

## **13.3. Prompt review sau mỗi sprint**

Hãy review toàn bộ code vừa triển khai cho Sprint \[số\].

Kiểm tra:  
1\. Có đúng phạm vi sprint không?  
2\. Có làm sang sprint khác không?  
3\. Có phá chức năng MVP không?  
4\. Có lỗi build backend/frontend không?  
5\. Có thiếu validation không?  
6\. Có thiếu loading/error/empty state không?  
7\. Có secret/token nào bị commit không?  
8\. Có cần cập nhật README/docs không?

Sau đó hãy báo cáo:  
\- Vấn đề phát hiện.  
\- Mức độ nghiêm trọng.  
\- Cách sửa.  
\- Nếu lỗi rõ ràng, hãy sửa trực tiếp.

---

# **14\. Quy trình test sau mỗi sprint**

Sau mỗi sprint v2, cần chạy tối thiểu:

## **14.1. Backend**

dotnet build

Nếu có test project:

dotnet test

---

## **14.2. Frontend**

npm install  
npm run build

Nếu có lint:

npm run lint

---

## **14.3. Dev mode**

docker compose \-f docker-compose.dev.yml up \-d

Sau đó chạy backend local:

cd backend/SmartTourism360.Api  
dotnet run

Chạy frontend local:

cd frontend  
npm run dev

---

## **14.4. Full Docker mode**

docker compose up \-d \--build

Kiểm tra:

Frontend: http://localhost:5173  
Backend: http://localhost:5074  
Scalar/Swagger: http://localhost:5074/scalar/v1  
MinIO: http://localhost:9001

---

## **14.5. Regression checklist chung**

\- \[ \] Admin login  
\- \[ \] Public home  
\- \[ \] Public map 2D  
\- \[ \] Destination detail  
\- \[ \] Tour 360  
\- \[ \] Hotspot navigation  
\- \[ \] Admin destination CRUD  
\- \[ \] Media upload  
\- \[ \] Virtual tour management  
\- \[ \] Panorama management  
\- \[ \] Docker full mode

---

# **15\. Quản lý Git cho v2**

## **15.1. Branching đề xuất**

main  
develop  
feature/sprint-12-ui-ux  
feature/sprint-13-routes  
feature/sprint-14-analytics  
feature/sprint-15-multilanguage  
feature/sprint-16-cesium  
feature/sprint-17-3d-model  
feature/sprint-18-ai-guide

---

## **15.2. Commit message gợi ý**

feat: improve public UI and tour viewer  
feat: add route itinerary module  
feat: add analytics tracking  
feat: add multi-language support  
feat: add cesium 3d map  
feat: add 3d model viewer  
feat: add ai tour guide  
fix: resolve route reorder issue  
docs: update v2 implementation plan  
chore: update environment examples

---

## **15.3. Trước khi push**

\- \[ \] Không có .env thật.  
\- \[ \] Không có AI API key.  
\- \[ \] Không có Cesium token thật.  
\- \[ \] Không có model 3D nặng không cần thiết.  
\- \[ \] Không có node\_modules.  
\- \[ \] Không có bin/obj.  
\- \[ \] Không có Docker volume.  
\- \[ \] README/.env.example đã cập nhật.

---

# **16\. Tiêu chí hoàn thành v2**

v2 được xem là hoàn thành khi đạt:

\- Sprint 12 UI/UX hoàn thành.  
\- Sprint 13 Route/Itinerary hoàn thành.  
\- Sprint 14 Analytics hoàn thành.  
\- Sprint 15 Multi-language hoàn thành hoặc có thể tạm hoãn có lý do.  
\- Sprint 16 CesiumJS 3D Map hoàn thành.  
\- Sprint 17 3D Model Viewer hoàn thành.  
\- Sprint 18 AI Tour Guide hoàn thành.  
\- Chức năng MVP vẫn ổn định.  
\- Docker full mode chạy được.  
\- README/docs cập nhật.  
\- Có dữ liệu demo đủ để trình bày v2.

---

# **17\. Kết quả mong đợi sau v2**

Sau v2, dự án sẽ có thể được mô tả như sau:

Smart Tourism 360 Platform là nền tảng du lịch số hỗ trợ bản đồ tương tác, tour 360 nhiều panorama, hotspot điều hướng, tuyến tham quan, analytics, bản đồ 3D WebGIS, mô hình 3D và trợ lý AI hướng dẫn du lịch dựa trên dữ liệu nội bộ.

Dự án khi đó không chỉ là một MVP, mà trở thành một sản phẩm demo có chiều sâu:

\- Có giá trị cho du khách  
\- Có công cụ cho admin  
\- Có dữ liệu phân tích cho quản lý  
\- Có công nghệ 3D/WebGIS  
\- Có AI hỗ trợ trải nghiệm du lịch

---

# **18\. Rủi ro tổng thể của v2**

## **18.1. Rủi ro quá tải phạm vi**

Mô tả:

v2 có nhiều tính năng lớn, nếu làm cùng lúc sẽ dễ mất kiểm soát.

Cách xử lý:

Làm từng sprint.  
Mỗi sprint phải demo được.  
Không bắt đầu sprint mới khi sprint trước còn lỗi nghiêm trọng.

---

## **18.2. Rủi ro công nghệ cao gây lỗi hệ thống**

Mô tả:

CesiumJS, 3D model và AI đều có thể gây lỗi build, lỗi performance hoặc lỗi cấu hình.

Cách xử lý:

Tách route riêng.  
Lazy load.  
Không ảnh hưởng MVP.  
Có fallback nếu token/API key chưa cấu hình.

---

## **18.3. Rủi ro dữ liệu demo không đủ thuyết phục**

Mô tả:

Nếu dữ liệu tour, route, model, AI context ít hoặc giả lập quá nhiều, demo sẽ kém hấp dẫn.

Cách xử lý:

Chuẩn hóa dữ liệu demo.  
Tạo ít nhất:  
\- 7 địa điểm  
\- 3 route  
\- 3 tour 360  
\- 1-2 model 3D  
\- Nội dung mô tả đủ tốt cho AI

---

## **18.4. Rủi ro lộ secret/token**

Mô tả:

Cesium token và AI API key dễ bị commit nhầm.

Cách xử lý:

Dùng .env.local.  
Dùng .env.example.  
Review git diff trước khi push.  
Không commit token thật.

---

# **19\. Thứ tự ưu tiên nếu không đủ thời gian**

Nếu không đủ thời gian làm toàn bộ Sprint 12–18, nên ưu tiên:

1\. Sprint 12 \- UI/UX Upgrade  
2\. Sprint 13 \- Route/Itinerary  
3\. Sprint 16 \- CesiumJS 3D Map  
4\. Sprint 18 \- AI Tour Guide  
5\. Sprint 14 \- Analytics  
6\. Sprint 17 \- 3D Model  
7\. Sprint 15 \- Multi-language

Lý do:

\- UI/UX giúp demo đẹp hơn ngay.  
\- Route tăng giá trị nghiệp vụ du lịch.  
\- CesiumJS tạo điểm nhấn công nghệ.  
\- AI tạo điểm nhấn thông minh.  
\- Analytics, 3D model, multi-language có thể bổ sung sau tùy thời gian.

Tuy nhiên, nếu định hướng sản phẩm thực tế cho cơ quan quản lý, nên ưu tiên Analytics cao hơn AI.

---

# **20\. Kết luận**

**Implementation Plan v2** định hướng phát triển **Smart Tourism 360 Platform** từ một MVP hoàn chỉnh thành một nền tảng du lịch số thông minh và giàu trải nghiệm hơn.

Lộ trình đề xuất:

Sprint 12: UI/UX Upgrade  
Sprint 13: Route / Itinerary  
Sprint 14: Analytics & Tracking  
Sprint 15: Multi-language Support  
Sprint 16: CesiumJS 3D Map  
Sprint 17: 3D Model Viewer  
Sprint 18: AI Tour Guide

Cách triển khai này giúp dự án phát triển có kiểm soát:

Đẹp hơn  
→ thông minh hơn  
→ giàu dữ liệu hơn  
→ có công nghệ 3D/WebGIS  
→ có AI hỗ trợ du lịch

Nếu hoàn thành v2, dự án sẽ có giá trị rất tốt để đưa vào portfolio, báo cáo chuyên đề, luận văn ứng dụng hoặc phát triển tiếp thành sản phẩm demo cho du lịch địa phương.

