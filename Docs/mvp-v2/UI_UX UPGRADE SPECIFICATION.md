# **UI/UX UPGRADE SPECIFICATION**

# **Smart Tourism 360 Platform v2**

## **Đặc tả nâng cấp giao diện và trải nghiệm người dùng sau MVP**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | UI/UX Upgrade Specification |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v2.0 |
| Giai đoạn | Sau MVP |
| Sprint đề xuất | Sprint 12 |
| Mục đích | Đặc tả các nâng cấp UI/UX cho public website, tour 360 viewer và admin dashboard |
| Đối tượng đọc | Frontend developer, UI/UX designer, AI coding agent, project owner |
| Công nghệ frontend hiện tại | Vue.js 3, Vue Router, Pinia, TailwindCSS, Leaflet, Photo Sphere Viewer, Lucide Icons |
| Phạm vi | Public UI, map page, destination detail, tour 360 viewer, admin dashboard, responsive, animation, loading/error states |

---

## **2\. Bối cảnh hiện tại**

Phiên bản MVP của **Smart Tourism 360 Platform** đã hoàn thành các chức năng cốt lõi:

\- Du khách xem bản đồ địa điểm.  
\- Du khách xem chi tiết địa điểm.  
\- Du khách mở tour 360\.  
\- Du khách bấm hotspot để chuyển panorama.  
\- Admin đăng nhập dashboard.  
\- Admin quản lý địa điểm.  
\- Admin upload media.  
\- Admin tạo virtual tour.  
\- Admin tạo panorama.  
\- Admin tạo hotspot.  
\- Hệ thống chạy được bằng Docker Compose.

Tuy nhiên, giao diện hiện tại chủ yếu phục vụ mục tiêu “chạy được chức năng”. Giai đoạn tiếp theo cần nâng cấp để hệ thống có cảm giác giống một sản phẩm thật hơn, đẹp hơn, dễ dùng hơn và thuyết phục hơn khi demo.

---

## **3\. Mục tiêu nâng cấp UI/UX**

Mục tiêu của tài liệu này là định nghĩa rõ các phần cần nâng cấp giao diện và trải nghiệm, bao gồm:

1\. Làm public website đẹp hơn, hiện đại hơn và mang cảm giác du lịch số.  
2\. Cải thiện trải nghiệm khám phá bản đồ.  
3\. Cải thiện trang chi tiết địa điểm để giống một landing page du lịch.  
4\. Nâng cấp tour 360 viewer để trực quan, mượt và dễ dùng hơn.  
5\. Tối ưu trải nghiệm mobile cho du khách.  
6\. Làm admin dashboard rõ ràng, chuyên nghiệp và dễ thao tác hơn.  
7\. Chuẩn hóa design system: màu sắc, font, card, badge, button, icon, spacing.  
8\. Bổ sung loading state, empty state, error state và micro-interactions.

Sau khi hoàn thành Sprint 12, hệ thống không nhất thiết phải có thêm nghiệp vụ mới, nhưng phải tạo cảm giác:

MVP cũ: chức năng chạy được.  
Phiên bản nâng cấp: trải nghiệm mượt hơn, đẹp hơn, đáng demo hơn.

---

## **4\. Nguyên tắc thiết kế**

### **4.1. Du lịch trước, công nghệ sau**

Giao diện public phải tạo cảm giác đang khám phá một điểm đến du lịch, không chỉ là một dashboard kỹ thuật.

Ưu tiên:

\- Hình ảnh lớn.  
\- Bản đồ trực quan.  
\- CTA rõ ràng.  
\- Trải nghiệm tour 360 nổi bật.  
\- Nội dung địa điểm dễ đọc.

Không nên để public website giống trang quản trị hoặc trang CRUD.

---

### **4.2. 360 tour là điểm nhấn chính**

Tour 360 là tính năng khác biệt nhất của dự án. Vì vậy, UI/UX phải làm rõ cho người dùng:

\- Đây là tour ảo 360 độ.  
\- Có thể kéo để xoay nhìn xung quanh.  
\- Có thể bấm mũi tên để di chuyển.  
\- Có thể bấm hotspot để xem thông tin/audio/video.

Người dùng lần đầu mở tour không được cảm thấy bối rối.

---

### **4.3. Mobile-first cho public website**

Du khách thường dùng điện thoại. Vì vậy:

\- Bản đồ phải dễ thao tác trên mobile.  
\- Tour 360 phải xem được tốt trên mobile.  
\- Hotspot phải đủ lớn để bấm.  
\- Popup nên chuyển thành bottom sheet trên mobile.  
\- Các CTA phải rõ và dễ chạm.

---

### **4.4. Desktop-first cho admin dashboard**

Admin thường thao tác trên laptop/desktop. Vì vậy:

\- Table phải dễ đọc.  
\- Form phải chia section rõ ràng.  
\- Media library cần dễ preview.  
\- Hotspot editor cần không gian rộng.  
\- Sidebar/topbar phải rõ.

---

### **4.5. Không phá kiến trúc hiện tại**

Sprint 12 là UI/UX upgrade, không phải rewrite toàn bộ hệ thống.

Nguyên tắc:

\- Không đổi API nếu không cần.  
\- Không đổi database schema nếu không cần.  
\- Không thay đổi logic tour 360 đang hoạt động.  
\- Ưu tiên refactor component frontend.  
\- Chỉ bổ sung field/API nhỏ nếu thật sự cần cho UI.

---

## **5\. Định hướng visual style**

### **5.1. Phong cách tổng thể**

Phong cách đề xuất cho v2:

Modern Local Tourism  
Digital Heritage  
Immersive 360 Experience  
Clean Smart Dashboard

Cảm giác mong muốn:

\- Hiện đại  
\- Sạch sẽ  
\- Có chiều sâu hình ảnh  
\- Có cảm giác du lịch địa phương  
\- Có điểm nhấn công nghệ  
\- Không quá màu mè  
\- Không quá hành chính

---

### **5.2. Theme màu đề xuất**

Giữ tinh thần màu đã định nghĩa trong MVP nhưng dùng nhất quán hơn.

| Vai trò | Màu | Hex |
| ----- | ----- | ----- |
| Primary | Teal / Xanh ngọc | `#0F766E` |
| Primary Dark | Teal đậm | `#115E59` |
| Primary Light | Teal nhạt | `#CCFBF1` |
| Secondary | Vàng nắng | `#F59E0B` |
| Accent | Cam đất | `#EA580C` |
| Background | Xám rất nhạt | `#F8FAFC` |
| Surface | Trắng | `#FFFFFF` |
| Text Primary | Slate đậm | `#0F172A` |
| Text Secondary | Slate xám | `#64748B` |
| Border | Xám nhạt | `#E2E8F0` |
| Success | Xanh lá | `#16A34A` |
| Warning | Vàng | `#FACC15` |
| Danger | Đỏ | `#DC2626` |

### **5.3. Màu cho category**

| Category | Màu | Icon gợi ý |
| ----- | ----- | ----- |
| Văn hóa \- lịch sử | `#7C3AED` | `landmark` |
| Tâm linh | `#F59E0B` | `church`, `landmark`, `sparkles` |
| Sinh thái | `#22C55E` | `leaf` |
| Nông nghiệp | `#65A30D` | `sprout` |
| Làng nghề | `#C2410C` | `hammer` |
| Ẩm thực | `#EF4444` | `utensils` |
| Lưu trú | `#2563EB` | `hotel` |

---

### **5.4. Font chữ**

Khuyến nghị giữ một font chính toàn hệ thống:

Be Vietnam Pro

Lý do:

\- Hỗ trợ tiếng Việt tốt.  
\- Hiện đại.  
\- Phù hợp cả public website và admin dashboard.  
\- Dễ đọc trên mobile.

Font weight:

Hero title: 700/800  
Section title: 700  
Card title: 600  
Body text: 400/500  
Button text: 600  
Caption: 400

---

### **5.5. Bo góc và shadow**

Quy chuẩn:

Card: rounded-2xl hoặc rounded-3xl  
Button: rounded-xl hoặc rounded-2xl  
Modal: rounded-2xl  
Input: rounded-xl  
Map popup: rounded-2xl  
Tour control: rounded-full hoặc rounded-2xl

Shadow:

Card thường: shadow-sm  
Card hover: shadow-lg  
Floating panel: shadow-xl  
Tour control overlay: shadow-2xl

---

## **6\. Component Design System cần chuẩn hóa**

### **6.1. Button**

Cần chuẩn hóa các loại button:

Primary Button  
Secondary Button  
Ghost Button  
Danger Button  
Icon Button  
CTA Button

Ví dụ:

Primary:  
\- nền teal  
\- chữ trắng  
\- hover teal đậm  
\- shadow nhẹ

Secondary:  
\- nền trắng  
\- border slate-200  
\- chữ slate-700  
\- hover nền slate-50

Ghost:  
\- nền trong suốt  
\- hover nền slate-100

Button quan trọng nhất trong public:

Tham quan 360

Button này nên có icon và visual nổi bật hơn button thường.

---

### **6.2. Card**

Các loại card cần chuẩn hóa:

DestinationCard  
CategoryCard  
RouteCard, chuẩn bị cho Sprint 13  
StatCard  
MediaCard  
PanoramaCard

DestinationCard cần có:

\- Ảnh đại diện  
\- Category badge  
\- Tên địa điểm  
\- Mô tả ngắn  
\- Địa chỉ/khu vực  
\- Badge “Có tour 360”  
\- CTA nhỏ

Hover effect:

\- ảnh zoom nhẹ  
\- card nâng lên nhẹ  
\- shadow tăng  
\- CTA hiện rõ hơn

---

### **6.3. Badge**

Các badge chính:

CategoryBadge  
StatusBadge  
TourBadge  
MediaTypeBadge  
FeaturedBadge

Status badge:

draft: vàng/xám  
published: xanh lá  
archived: xám

Tour badge:

Có tour 360  
360°  
Virtual Tour

---

### **6.4. Toast và Confirm Modal**

MVP đã có Toast/Confirm. Sprint 12 cần đảm bảo style đồng bộ:

Toast types:

success  
error  
warning  
info

Confirm modal dùng cho:

xóa địa điểm  
xóa media  
xóa tour  
xóa panorama  
xóa hotspot  
archive dữ liệu

---

## **7\. Nâng cấp Public Home Page**

### **7.1. Mục tiêu**

Trang chủ phải giới thiệu rõ sản phẩm và dẫn người dùng vào hai hành động chính:

1\. Khám phá bản đồ  
2\. Trải nghiệm tour 360

---

### **7.2. Layout đề xuất**

Header  
Hero Section  
Quick Search / Explore CTA  
Featured Categories  
Featured Destinations  
360 Experience Section  
Map Preview Section  
Future Route Section Placeholder  
Footer

---

### **7.3. Hero Section**

Hero nên có:

\- Ảnh nền du lịch địa phương hoặc ảnh sông nước/văn hóa.  
\- Overlay gradient tối nhẹ.  
\- Tiêu đề lớn.  
\- Mô tả ngắn.  
\- CTA chính: Khám phá bản đồ.  
\- CTA phụ: Xem tour 360\.

Nội dung gợi ý:

Title:  
Khám phá du lịch địa phương qua bản đồ và tour 360°

Subtitle:  
Trải nghiệm các điểm đến văn hóa, sinh thái và biểu tượng địa phương bằng bản đồ tương tác, ảnh 360° và nội dung thuyết minh số.

CTA:  
Khám phá bản đồ  
Trải nghiệm tour 360

---

### **7.4. Featured Categories**

Hiển thị các category dưới dạng card/chip.

Mỗi item gồm:

icon  
name  
short label  
color

Khi click category:

Đi tới /explore?categoryId=...

---

### **7.5. Featured Destinations**

Hiển thị 4–6 địa điểm nổi bật.

Card nên có:

cover image  
category badge  
name  
short description  
has tour 360 badge  
CTA “Xem chi tiết”

Nếu địa điểm có tour:

hiển thị thêm nút “Tour 360”

---

### **7.6. 360 Experience Section**

Mục tiêu: làm nổi bật điểm khác biệt.

Nội dung:

\- Ảnh mockup hoặc screenshot tour 360\.  
\- 3 điểm nổi bật:  
  1\. Xoay nhìn toàn cảnh  
  2\. Di chuyển bằng mũi tên  
  3\. Nghe/xem nội dung thuyết minh

---

## **8\. Nâng cấp Explore Map Page**

### **8.1. Mục tiêu**

Trang bản đồ phải dễ tìm kiếm, dễ lọc và dễ mở địa điểm/tour.

---

### **8.2. Desktop layout**

Đề xuất:

Header  
Main area full height  
Left panel: search/filter/list  
Right area: map

Left panel:

\- Search input  
\- Category chips  
\- Toggle “Có tour 360”  
\- Destination result list

Map area:

\- Leaflet map  
\- Custom marker  
\- Marker popup  
\- Zoom controls  
\- Reset view button

---

### **8.3. Mobile layout**

Trên mobile:

\- Map full screen.  
\- Search bar nổi phía trên.  
\- Filter chips scroll ngang.  
\- Bottom sheet danh sách địa điểm.  
\- Marker popup dạng bottom card.

Yêu cầu:

\- Không để sidebar chiếm quá nhiều màn hình.  
\- Nút bấm phải tối thiểu 44px.  
\- Popup không được che hết map.

---

### **8.4. Marker**

Marker nên thể hiện category bằng màu/icon.

Có thể dùng:

\- Pin icon màu theo category.  
\- Icon trắng bên trong marker.  
\- Marker có pulse nhẹ nếu địa điểm có tour 360\.

---

### **8.5. Marker Popup**

Popup cần hiển thị:

\- Ảnh nhỏ  
\- Tên địa điểm  
\- Category badge  
\- Mô tả ngắn  
\- Badge tour 360 nếu có  
\- Button “Xem chi tiết”  
\- Button “Tour 360” nếu có

Không nên hiển thị quá nhiều chữ.

---

### **8.6. Empty state**

Khi không tìm thấy địa điểm:

Không tìm thấy địa điểm phù hợp.  
Hãy thử thay đổi từ khóa hoặc bộ lọc danh mục.

---

## **9\. Nâng cấp Destination List Page**

### **9.1. Mục tiêu**

Trang danh sách địa điểm cần giống trang khám phá điểm đến, không chỉ là danh sách card đơn giản.

---

### **9.2. Layout đề xuất**

Page header  
Search/filter bar  
Category chips  
Destination grid  
Pagination/load more

Destination grid:

Desktop: 3 columns  
Tablet: 2 columns  
Mobile: 1 column

---

### **9.3. Destination Card v2**

Card gồm:

\- Cover image  
\- Category badge overlay  
\- Tour 360 badge  
\- Tên địa điểm  
\- Địa chỉ/khu vực  
\- Mô tả ngắn  
\- CTA “Xem chi tiết”

Hover desktop:

\- Image scale 1.05  
\- Card shadow-lg  
\- CTA đổi màu primary

---

## **10\. Nâng cấp Destination Detail Page**

### **10.1. Mục tiêu**

Trang chi tiết địa điểm cần trở thành một trang giới thiệu điểm đến đầy đủ.

---

### **10.2. Layout đề xuất**

Hero image  
Destination overview  
CTA section  
Description sections  
Gallery  
Audio/video section  
Map location  
Nearby destinations

---

### **10.3. Hero image**

Hero gồm:

\- Cover image lớn  
\- Overlay gradient  
\- Category badge  
\- Tên địa điểm  
\- Địa chỉ  
\- CTA “Tham quan 360”

Nếu địa điểm có tour:

CTA “Tham quan 360” phải nổi bật nhất.

Nếu không có tour:

CTA chuyển thành “Xem trên bản đồ”.

---

### **10.4. Destination Info Panel**

Hiển thị các thông tin nhanh:

\- Giờ mở cửa  
\- Giá vé  
\- Số điện thoại  
\- Website/Facebook  
\- Có audio guide  
\- Có tour 360

Nếu dữ liệu null:

Không hiển thị trường đó, hoặc hiển thị “Đang cập nhật”.

---

### **10.5. Gallery**

Yêu cầu:

\- Grid ảnh đẹp.  
\- Click mở lightbox nếu có.  
\- Ảnh 360 có badge riêng.  
\- Video có icon play.

---

### **10.6. Nearby Destinations**

Có thể làm đơn giản:

\- Hiển thị 3 địa điểm khác cùng region/category.  
\- Không cần tính khoảng cách phức tạp ở Sprint 12\.

Nếu backend chưa có API nearby, frontend có thể dùng danh sách destinations hiện có để chọn tạm.

---

## **11\. Nâng cấp Tour 360 Viewer**

### **11.1. Mục tiêu**

Tour 360 Viewer cần trở thành trải nghiệm nhập vai, dễ hiểu và mượt hơn MVP.

---

### **11.2. Layout viewer**

Tour page nên gần full-screen:

Full screen 360 viewer  
Top overlay bar  
Bottom control bar  
Scene list panel  
Hotspot popup/bottom sheet  
First-time guide overlay

---

### **11.3. Top Overlay Bar**

Hiển thị:

\- Nút quay lại  
\- Tên địa điểm  
\- Tên panorama hiện tại  
\- Nút mở scene list  
\- Nút fullscreen

Style:

background: rgba(15, 23, 42, 0.6)  
backdrop blur  
text white  
rounded hoặc full-width bar

---

### **11.4. Scene List with Thumbnail**

Cần thêm danh sách scene/panorama.

Desktop:

Panel bên phải hoặc dưới cùng.

Mobile:

Bottom sheet.

Mỗi scene item gồm:

thumbnail  
title  
active state  
display order

Click scene:

Chuyển sang panorama tương ứng.

---

### **11.5. First-time Guide Overlay**

Khi user mở tour lần đầu trong session:

Hiển thị overlay hướng dẫn 3 bước:  
1\. Kéo để xoay nhìn xung quanh.  
2\. Bấm mũi tên để di chuyển.  
3\. Bấm biểu tượng để xem thông tin/thuyết minh.

Có nút:

Đã hiểu

Có thể lưu trạng thái trong `localStorage`:

tourGuideSeen \= true

---

### **11.6. Hotspot UI**

Hotspot cần phân biệt rõ loại:

| Type | Icon | Style |
| ----- | ----- | ----- |
| navigation | arrow / chevron | pulse, nổi bật |
| info | info | xanh/teal |
| audio | volume | vàng/cam |
| video | play | đỏ/cam |
| image | image | xanh dương |
| model3d | box/3d | tím |

Yêu cầu:

\- Hotspot tối thiểu 44px trên mobile.  
\- Có label khi hover desktop.  
\- Có label khi tap mobile.  
\- Navigation hotspot có hiệu ứng pulse nhẹ.

---

### **11.7. Transition khi chuyển panorama**

Khi chuyển scene:

\- Hiển thị loading overlay ngắn.  
\- Fade out panorama cũ.  
\- Load panorama mới.  
\- Fade in panorama mới.  
\- Cập nhật scene title.  
\- Cập nhật active scene.

Không nên reload toàn trang.

---

### **11.8. Info/Audio Popup**

Desktop:

Side panel hoặc floating modal.

Mobile:

Bottom sheet.

Info hotspot hiển thị:

title  
description  
image nếu có

Audio hotspot hiển thị:

title  
audio player  
transcript nếu có

---

## **12\. Nâng cấp Admin Dashboard**

### **12.1. Mục tiêu**

Admin dashboard cần dễ thao tác, rõ ràng, giảm cảm giác “tool thô”.

---

### **12.2. Dashboard Overview**

Nâng cấp các stat card:

\- Tổng địa điểm  
\- Địa điểm đã xuất bản  
\- Tour 360  
\- Panorama  
\- Hotspot  
\- Media files

Nếu analytics chưa có:

Dùng thống kê từ dữ liệu hiện có.

---

### **12.3. Destination Management**

Nâng cấp:

\- Table rõ hơn.  
\- Status badge đẹp.  
\- Category badge.  
\- Thumbnail địa điểm.  
\- Quick action buttons.  
\- Filter/search đẹp hơn.

---

### **12.4. Destination Form**

Form nên chia section:

1\. Thông tin cơ bản  
2\. Vị trí bản đồ  
3\. Media đại diện  
4\. Thông tin liên hệ  
5\. Trạng thái xuất bản

MapLocationPicker:

\- Có instruction rõ.  
\- Marker đẹp hơn.  
\- Có nút reset vị trí.  
\- Hiển thị lat/lng dạng readonly hoặc editable.

---

### **12.5. Media Library**

Nâng cấp:

\- Grid card đẹp hơn.  
\- Preview modal.  
\- Badge media type.  
\- Filter media type.  
\- Search theo tên file.  
\- Empty state.  
\- Upload progress rõ hơn.

---

### **12.6. Panorama Management**

Nâng cấp:

\- Panorama card có thumbnail.  
\- Badge default panorama.  
\- Badge status.  
\- Button “Edit hotspots”.  
\- Button “Preview”.  
\- Reorder nếu có thời gian.

---

### **12.7. Hotspot Editor**

Nâng cấp:

\- Side panel rõ ràng hơn.  
\- Hotspot type selector dạng card.  
\- Dropdown target panorama có thumbnail.  
\- Preview hotspot ngay khi tạo.  
\- Validation message rõ.  
\- Button preview as tourist.

---

## **13\. Loading, Empty, Error States**

### **13.1. Loading States**

Cần có:

\- Skeleton cho destination card.  
\- Spinner/loading overlay cho map.  
\- Spinner cho tour 360\.  
\- Upload progress cho media.  
\- Button loading khi submit form.

---

### **13.2. Empty States**

Cần có empty state cho:

\- Không có địa điểm.  
\- Không có kết quả tìm kiếm.  
\- Chưa có tour 360\.  
\- Chưa có panorama.  
\- Chưa có hotspot.  
\- Chưa có media.

Ví dụ:

Chưa có panorama nào.  
Hãy upload ảnh 360 đầu tiên để bắt đầu tạo tour.

---

### **13.3. Error States**

Cần xử lý:

\- Không tải được dữ liệu địa điểm.  
\- Không tải được bản đồ.  
\- Không tải được tour 360\.  
\- Ảnh panorama lỗi.  
\- Upload thất bại.  
\- Token hết hạn.  
\- Không có quyền thao tác.

Thông báo cần dễ hiểu, không show raw error dài cho user.

---

## **14\. Animation & Micro-interaction**

### **14.1. Public Website**

Animation đề xuất:

\- Fade in section.  
\- Slide up card.  
\- Image hover zoom.  
\- Button hover translate nhẹ.  
\- Marker pulse.  
\- Popup fade/scale.

Không nên dùng animation quá nhiều gây nặng.

---

### **14.2. Tour 360**

Animation đề xuất:

\- Hotspot pulse.  
\- Hotspot hover label.  
\- Scene transition fade.  
\- Control bar fade.  
\- Bottom sheet slide.

---

### **14.3. Admin**

Animation đơn giản:

\- Table row hover.  
\- Modal fade.  
\- Toast slide in.  
\- Form section expand/collapse nếu có.

---

## **15\. Responsive Requirements**

### **15.1. Breakpoints**

Theo Tailwind:

sm: 640px  
md: 768px  
lg: 1024px  
xl: 1280px  
2xl: 1536px

---

### **15.2. Public Mobile**

Yêu cầu:

\- Header mobile có menu gọn.  
\- Card 1 cột.  
\- Map full-screen.  
\- Filter chip scroll ngang.  
\- Popup map thành bottom card.  
\- Tour 360 full-screen.  
\- Scene list thành bottom sheet.  
\- Hotspot tối thiểu 44px.

---

### **15.3. Admin Mobile**

Admin không ưu tiên mobile hoàn hảo nhưng cần không vỡ layout:

\- Sidebar thành drawer.  
\- Table có horizontal scroll hoặc card list.  
\- Form full width.  
\- Hotspot editor có cảnh báo nên dùng desktop nếu màn hình quá nhỏ.

---

## **16\. Accessibility Requirements**

Cần đảm bảo:

\- Text đủ tương phản.  
\- Button có label rõ.  
\- Icon quan trọng có tooltip hoặc text.  
\- Form field có label.  
\- Lỗi form hiển thị gần field.  
\- Không chỉ dùng màu để thể hiện trạng thái.  
\- Tap target mobile tối thiểu 44px.

Tour 360 cần có hướng dẫn rõ vì không phải ai cũng quen thao tác.

---

## **17\. Component cần tạo mới hoặc refactor**

### **17.1. Public Components**

\- HeroSection.vue  
\- CategoryChip.vue  
\- FeaturedDestinationCard.vue  
\- DestinationCardV2.vue  
\- MapMarkerPopup.vue  
\- DestinationInfoPanel.vue  
\- DestinationGallery.vue  
\- TourCtaButton.vue  
\- EmptyState.vue  
\- LoadingSkeleton.vue

---

### **17.2. Tour 360 Components**

\- Tour360TopBar.vue  
\- Tour360ControlBar.vue  
\- SceneListPanel.vue  
\- SceneThumbnailCard.vue  
\- HotspotButton.vue  
\- HotspotInfoPanel.vue  
\- TourGuideOverlay.vue  
\- TourLoadingOverlay.vue

---

### **17.3. Admin Components**

\- AdminStatCard.vue  
\- StatusBadge.vue  
\- CategoryBadge.vue  
\- MediaTypeBadge.vue  
\- FormSection.vue  
\- MediaPreviewModal.vue  
\- PanoramaCard.vue  
\- HotspotTypeSelector.vue  
\- TargetPanoramaSelect.vue

---

## **18\. Sprint 12 Implementation Plan**

### **18.1. Mục tiêu Sprint 12**

Nâng cấp trải nghiệm public website, tour 360 viewer và admin dashboard mà không phá vỡ chức năng MVP hiện tại.

---

### **18.2. Checklist Public UI**

\- \[ \] Redesign Home Page.  
\- \[ \] Redesign Destination Card.  
\- \[ \] Redesign Destination List Page.  
\- \[ \] Redesign Explore Map Page.  
\- \[ \] Redesign Map Marker Popup.  
\- \[ \] Redesign Destination Detail Page.  
\- \[ \] Add Destination Info Panel.  
\- \[ \] Add Gallery layout.  
\- \[ \] Improve CTA “Tham quan 360”.  
\- \[ \] Add loading skeleton.  
\- \[ \] Add empty/error states.

---

### **18.3. Checklist Tour 360**

\- \[ \] Add Tour360TopBar.  
\- \[ \] Add Tour360ControlBar.  
\- \[ \] Add SceneListPanel.  
\- \[ \] Add scene thumbnails.  
\- \[ \] Add first-time guide overlay.  
\- \[ \] Improve hotspot icons.  
\- \[ \] Add hotspot hover/tap labels.  
\- \[ \] Add smooth transition between panoramas.  
\- \[ \] Add fullscreen button if feasible.  
\- \[ \] Improve mobile bottom sheet.

---

### **18.4. Checklist Admin UI**

\- \[ \] Improve Admin Dashboard stat cards.  
\- \[ \] Improve Destination table.  
\- \[ \] Improve Destination form sections.  
\- \[ \] Improve MapLocationPicker UI.  
\- \[ \] Improve Media Library cards.  
\- \[ \] Improve Panorama cards.  
\- \[ \] Improve Hotspot Editor side panel.  
\- \[ \] Improve status/category/media badges.

---

### **18.5. Checklist Responsive**

\- \[ \] Test Home Page on mobile.  
\- \[ \] Test Explore Map on mobile.  
\- \[ \] Test Destination Detail on mobile.  
\- \[ \] Test Tour 360 on mobile.  
\- \[ \] Test Admin layout on desktop.  
\- \[ \] Ensure no horizontal overflow on public pages.

---

### **18.6. Checklist Regression Test**

\- \[ \] Admin login still works.  
\- \[ \] Destination list still loads.  
\- \[ \] Public map still shows markers.  
\- \[ \] Destination detail still opens.  
\- \[ \] Tour 360 still loads.  
\- \[ \] Navigation hotspot still changes panorama.  
\- \[ \] Admin can create/edit destination.  
\- \[ \] Admin can upload media.  
\- \[ \] Admin can create/edit hotspot.  
\- \[ \] Docker build still works.

---

## **19\. Acceptance Criteria**

Sprint 12 được xem là hoàn thành nếu:

\- Public website có giao diện hiện đại và nhất quán hơn MVP.  
\- Trang chủ có hero, CTA, featured destinations và section 360 experience.  
\- Trang bản đồ dễ tìm kiếm/lọc hơn.  
\- Popup marker đẹp và rõ hành động hơn.  
\- Trang chi tiết địa điểm có layout tốt, CTA tour 360 nổi bật.  
\- Tour 360 có scene list, hướng dẫn lần đầu và hotspot UI tốt hơn.  
\- Mobile public pages không vỡ layout.  
\- Admin dashboard dễ nhìn và dễ thao tác hơn.  
\- Các chức năng cũ vẫn hoạt động.  
\- Không làm thay đổi database/API lớn nếu không cần.

---

## **20\. Non-goals của Sprint 12**

Sprint 12 không bao gồm:

\- Không làm Route/Itinerary Module.  
\- Không làm CesiumJS 3D Map.  
\- Không làm AI Tour Guide.  
\- Không làm 3D Model Viewer.  
\- Không thay đổi database schema lớn.  
\- Không rewrite toàn bộ frontend.  
\- Không thay thế Photo Sphere Viewer nếu đang hoạt động ổn.  
\- Không thay Leaflet bằng CesiumJS ở sprint này.

Các tính năng trên sẽ được triển khai ở sprint sau theo roadmap.

---

## **21\. Rủi ro và cách kiểm soát**

### **21.1. Rủi ro làm đẹp quá nhiều nhưng phá chức năng**

Cách kiểm soát:

\- Refactor từng component nhỏ.  
\- Sau mỗi thay đổi lớn, test lại flow tour 360\.  
\- Không đổi logic API nếu chỉ nâng UI.

---

### **21.2. Rủi ro mobile tour 360 khó tối ưu**

Cách kiểm soát:

\- Ưu tiên tap target lớn.  
\- Dùng bottom sheet thay vì popup nhỏ.  
\- Giữ control tối giản trên mobile.

---

### **21.3. Rủi ro animation làm nặng web**

Cách kiểm soát:

\- Dùng CSS transition trước.  
\- Không lạm dụng GSAP.  
\- Tránh animation liên tục trên nhiều card.

---

### **21.4. Rủi ro quá nhiều component mới**

Cách kiểm soát:

\- Tạo component dùng lại thật sự.  
\- Không tách component quá nhỏ nếu không cần.  
\- Ưu tiên component: DestinationCard, EmptyState, SceneList, HotspotButton.

--- 

## **22\. Kết luận**

Tài liệu **UI/UX Upgrade Specification** định hướng nâng cấp giao diện và trải nghiệm người dùng cho **Smart Tourism 360 Platform v2**. Sprint 12 không tập trung thêm nghiệp vụ mới, mà tập trung làm cho hệ thống hiện tại đẹp hơn, dễ dùng hơn, mượt hơn và chuyên nghiệp hơn.

Sau Sprint 12, dự án sẽ phù hợp hơn để:

\- Demo trước giảng viên hoặc người hướng dẫn.  
\- Quay video giới thiệu.  
\- Đưa vào portfolio.  
\- Làm nền tảng cho các tính năng lớn hơn như Route/Itinerary, CesiumJS 3D Map và AI Tour Guide.

