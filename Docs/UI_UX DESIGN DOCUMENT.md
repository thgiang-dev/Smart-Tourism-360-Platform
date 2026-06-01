# **UI/UX DESIGN DOCUMENT**

# **Smart Tourism 360 Platform**

## **Thiết kế trải nghiệm người dùng và giao diện cho nền tảng du lịch số 360**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | UI/UX Design Document |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v1.0 |
| Mục đích | Mô tả định hướng UI/UX, phong cách giao diện, theme màu, typography, icon, layout, animation và trải nghiệm người dùng |
| Đối tượng đọc | UI/UX designer, frontend developer, backend developer, project owner, nhóm phát triển |
| Phạm vi | Giao diện du khách, bản đồ khám phá, chi tiết địa điểm, tour 360, admin dashboard, quản lý địa điểm, media, panorama và hotspot |
| Ghi chú | Tài liệu không vẽ ảnh giao diện, chỉ mô tả chi tiết để triển khai |

---

## **2\. Mục tiêu UI/UX**

Giao diện của **Smart Tourism 360 Platform** cần đạt các mục tiêu sau:

1. Tạo cảm giác hiện đại, trực quan và hấp dẫn cho du khách.  
2. Giúp người dùng dễ dàng khám phá các địa điểm trên bản đồ.  
3. Làm nổi bật trải nghiệm tham quan ảo 360 độ.  
4. Giúp admin dễ dàng tạo địa điểm, chọn tọa độ, upload media và cấu hình tour 360\.  
5. Đảm bảo giao diện hoạt động tốt trên cả desktop, tablet và mobile.  
6. Có phong cách phù hợp với lĩnh vực du lịch, văn hóa, bản đồ và công nghệ số.  
7. Thiết kế đủ chuyên nghiệp để có thể phát triển thành sản phẩm thật.  
8. Không làm giao diện quá nặng trong MVP, nhưng vẫn có cảm giác “wow” nhờ animation, bản đồ, ảnh lớn và tour 360\.

---

## **3\. Định hướng trải nghiệm tổng thể**

### **3.1. Tinh thần thiết kế**

Giao diện nên đi theo phong cách:

Modern Tourism  
Digital Heritage  
Immersive Map Experience  
Smart Local Discovery  
Clean Admin Dashboard

Nghĩa là hệ thống cần tạo cảm giác:

hiện đại  
sạch sẽ  
dễ dùng  
có chiều sâu khám phá  
gợi cảm giác du lịch địa phương  
trực quan bằng bản đồ  
sinh động bằng tour 360

### **3.2. Cảm xúc mong muốn khi dùng hệ thống**

Khi du khách vào website, họ nên có cảm giác:

“Tôi có thể khám phá địa phương này ngay trên web.”  
“Tôi biết các điểm đến nằm ở đâu.”  
“Tôi có thể xem trước không gian thật trước khi đi.”  
“Website này hiện đại và đáng tin cậy.”

Khi admin dùng dashboard, họ nên có cảm giác:

“Tôi có thể tự tạo và quản lý địa điểm mà không cần sửa code.”  
“Tôi có thể click trên bản đồ để chọn vị trí.”  
“Tôi có thể upload ảnh 360 và tạo tour dễ dàng.”  
“Tôi kiểm soát được nội dung nào đang hiển thị cho du khách.”

---

## **4\. Đối tượng người dùng chính**

### **4.1. Du khách**

Du khách là người dùng công khai, không cần đăng nhập trong MVP.

Nhu cầu chính:

Xem bản đồ du lịch  
Tìm kiếm địa điểm  
Lọc theo loại hình du lịch  
Xem chi tiết địa điểm  
Mở tour 360  
Di chuyển giữa các điểm nhìn trong tour  
Xem thông tin/audio/video tại hotspot

### **4.2. Quản trị viên**

Admin là người dùng có đăng nhập.

Nhu cầu chính:

Tạo địa điểm mới  
Click bản đồ để chọn tọa độ  
Nhập thông tin địa điểm  
Upload ảnh/video/audio/ảnh 360  
Tạo tour 360  
Tạo panorama  
Đặt hotspot  
Thiết lập hotspot điều hướng  
Xuất bản nội dung

### **4.3. Cơ quan quản lý / Content Manager**

Có thể là người dùng mở rộng sau MVP.

Nhu cầu chính:

Kiểm tra nội dung đã đăng  
Quản lý dữ liệu du lịch  
Theo dõi địa điểm nổi bật  
Cập nhật thông tin truyền thông

---

## **5\. Nguyên tắc thiết kế giao diện**

### **5.1. Trực quan trước, chi tiết sau**

Trang du khách nên ưu tiên hiển thị hình ảnh, bản đồ và điểm đến nổi bật trước. Nội dung dài nên được đưa vào trang chi tiết hoặc các tab thông tin.

### **5.2. Bản đồ là trung tâm trải nghiệm**

Bản đồ không chỉ là thành phần phụ, mà là cách chính để người dùng khám phá khu vực.

Do đó màn hình bản đồ cần:

Marker rõ ràng  
Popup dễ hiểu  
Bộ lọc nổi bật  
Tìm kiếm nhanh  
Nút mở tour 360  
Thông tin địa điểm ngắn gọn

### **5.3. Tour 360 phải tạo cảm giác nhập vai**

Màn hình tour 360 nên chiếm toàn màn hình hoặc gần toàn màn hình. Các nút điều hướng, hotspot, audio và thông tin nên nổi trên ảnh 360 nhưng không gây rối.

### **5.4. Admin dashboard phải ưu tiên hiệu quả thao tác**

Admin không cần quá nhiều hiệu ứng gây phân tâm. Giao diện admin cần rõ ràng, nhiều bảng/form, trạng thái dễ nhìn, thao tác upload và chọn vị trí đơn giản.

### **5.5. Mobile-first cho du khách**

Du khách thường truy cập bằng điện thoại. Giao diện public cần ưu tiên mobile tốt, đặc biệt là:

Bản đồ  
Danh sách địa điểm  
Chi tiết địa điểm  
Tour 360  
Hotspot  
Nút điều hướng

### **5.6. Desktop-first cho admin**

Admin dashboard thường dùng trên laptop/desktop. Giao diện admin nên tối ưu cho màn hình rộng, bảng dữ liệu, form và editor.

---

## **6\. Design System tổng quan**

### **6.1. Phong cách hình ảnh**

Phong cách đề xuất:

Clean  
Modern  
Warm  
Tourism-focused  
Map-based  
Immersive  
Semi-premium

Không nên quá “game”, cũng không nên quá “hành chính”. Giao diện cần cân bằng giữa cảm giác du lịch, công nghệ và độ tin cậy.

### **6.2. Bo góc**

Gợi ý:

Card: 16px \- 24px  
Button: 12px \- 16px  
Popup: 16px  
Modal: 20px \- 24px  
Input: 10px \- 12px

### **6.3. Đổ bóng**

Sử dụng shadow mềm:

Card nổi nhẹ  
Popup marker nổi rõ trên bản đồ  
Control panel trong tour 360 có nền glassmorphism  
Admin dashboard dùng shadow nhẹ, không quá màu mè

### **6.4. Khoảng cách**

Sử dụng spacing rộng rãi, dễ đọc:

Section padding desktop: 64px \- 96px  
Section padding mobile: 32px \- 48px  
Card gap: 16px \- 24px  
Form field gap: 16px

---

## **7\. Theme màu**

### **7.1. Định hướng màu**

Theme nên gợi cảm giác:

du lịch  
thiên nhiên  
sông nước  
văn hóa địa phương  
hiện đại  
đáng tin cậy

### **7.2. Bảng màu chính đề xuất**

| Vai trò | Màu | Hex gợi ý | Ý nghĩa |
| ----- | ----- | ----- | ----- |
| Primary | Xanh ngọc / Teal | `#0F766E` | Du lịch, bản đồ, sông nước, hiện đại |
| Primary Light | Teal nhạt | `#CCFBF1` | Nền nhẹ, badge, hover |
| Secondary | Vàng nắng | `#F59E0B` | Năng lượng, du lịch, điểm nhấn |
| Accent | Cam đất | `#EA580C` | Văn hóa, địa phương, CTA phụ |
| Success | Xanh lá | `#16A34A` | Trạng thái tốt, published |
| Warning | Vàng | `#FACC15` | Draft/cảnh báo |
| Danger | Đỏ | `#DC2626` | Xóa/lỗi |
| Background | Xám rất nhạt | `#F8FAFC` | Nền trang |
| Surface | Trắng | `#FFFFFF` | Card, panel |
| Text Primary | Xám đậm | `#0F172A` | Nội dung chính |
| Text Secondary | Xám trung tính | `#64748B` | Mô tả phụ |
| Border | Xám nhạt | `#E2E8F0` | Đường viền |

### **7.3. Màu cho danh mục địa điểm**

| Danh mục | Màu gợi ý |
| ----- | ----- |
| Văn hóa \- lịch sử | Tím `#7C3AED` |
| Tâm linh | Vàng cam `#F59E0B` |
| Sinh thái | Xanh lá `#22C55E` |
| Nông nghiệp | Xanh olive `#65A30D` |
| Làng nghề | Nâu/cam đất `#C2410C` |
| Ẩm thực | Đỏ/cam `#EF4444` |
| Lưu trú | Xanh dương `#2563EB` |

### **7.4. Dark overlay trong tour 360**

Màn hình tour 360 nên dùng overlay tối nhẹ để chữ và control nổi bật:

Overlay background: rgba(15, 23, 42, 0.55)  
Control background: rgba(15, 23, 42, 0.72)  
Border: rgba(255, 255, 255, 0.16)  
Text: \#FFFFFF  
Text muted: \#CBD5E1

---

## **8\. Typography**

### **8.1. Font chữ đề xuất**

Có thể chọn một trong các font sau:

Be Vietnam Pro  
Inter  
Plus Jakarta Sans

Khuyến nghị:

Public website: Be Vietnam Pro hoặc Plus Jakarta Sans  
Admin dashboard: Inter hoặc Be Vietnam Pro

Nếu muốn đồng bộ toàn hệ thống:

Be Vietnam Pro

Lý do:

Hỗ trợ tiếng Việt tốt  
Hiện đại  
Dễ đọc  
Phù hợp cả giao diện du lịch và admin

### **8.2. Cỡ chữ đề xuất**

| Thành phần | Desktop | Mobile |
| ----- | ----- | ----- |
| Hero title | 48–64px | 32–40px |
| Page title | 32–40px | 28–32px |
| Section title | 28–36px | 24–28px |
| Card title | 18–22px | 16–18px |
| Body text | 16px | 15–16px |
| Caption | 13–14px | 12–13px |
| Button | 14–16px | 14–15px |
| Admin table | 14px | 13–14px |

### **8.3. Font weight**

Title lớn: 700 hoặc 800  
Section title: 700  
Card title: 600  
Body: 400 hoặc 500  
Button: 600  
Caption: 400

---

## **9\. Icon system**

### **9.1. Font icon đề xuất**

Có thể dùng:

Lucide Icons  
Heroicons  
Material Symbols  
Font Awesome

Khuyến nghị:

Lucide Icons

Lý do:

Hiện đại  
Đường nét mảnh đẹp  
Dễ dùng với Vue  
Phù hợp dashboard và website  
Nhiều icon bản đồ, media, upload, audio, video

### **9.2. Icon cần dùng**

| Chức năng | Icon gợi ý |
| ----- | ----- |
| Bản đồ | map, map-pin |
| Địa điểm | landmark, building, tree-pine |
| Tour 360 | rotate-3d, scan, compass |
| Audio | volume-2 |
| Video | play-circle |
| Ảnh | image |
| Upload | upload-cloud |
| Hotspot info | info |
| Điều hướng | arrow-right, chevron-right |
| Quay lại | arrow-left |
| Admin | layout-dashboard |
| Danh mục | tags |
| Cài đặt | settings |
| Xuất bản | badge-check |
| Draft | file-edit |
| Xóa | trash-2 |

---

## **10\. Sitemap tổng quan**

### **10.1. Public website**

/  
├── Trang chủ  
├── /explore  
│   └── Bản đồ khám phá  
├── /destinations  
│   └── Danh sách địa điểm  
├── /destinations/:slug  
│   └── Chi tiết địa điểm  
├── /destinations/:slug/tour  
│   └── Tour 360  
└── /routes  
    └── Tuyến tham quan, mở rộng sau MVP

### **10.2. Admin dashboard**

/admin/login  
/admin  
├── Dashboard tổng quan  
├── Regions  
├── Categories  
├── Destinations  
│   ├── Danh sách địa điểm  
│   ├── Tạo/sửa địa điểm  
│   └── Chọn tọa độ trên bản đồ  
├── Media Library  
├── Virtual Tours  
│   ├── Danh sách tour  
│   ├── Panorama management  
│   └── Hotspot editor  
├── Translations, mở rộng  
├── Routes, mở rộng  
└── Settings

---

# **PHẦN A: GIAO DIỆN PUBLIC CHO DU KHÁCH**

---

## **11\. Trang chủ**

### **11.1. Mục tiêu**

Trang chủ cần giới thiệu nhanh nền tảng và dẫn người dùng vào trải nghiệm chính: khám phá bản đồ hoặc mở địa điểm nổi bật.

### **11.2. Nội dung chính**

Trang chủ gồm các phần:

Header  
Hero section  
Search bar hoặc CTA khám phá  
Danh mục du lịch  
Địa điểm nổi bật  
Giới thiệu trải nghiệm 360  
Bản đồ preview nhỏ  
Tuyến tham quan đề xuất, mở rộng sau  
Footer

### **11.3. Hero section**

Hero nên có ảnh nền lớn hoặc video ngắn về du lịch địa phương.

Nội dung hero:

Tiêu đề lớn:  
Khám phá du lịch địa phương qua bản đồ và tour 360

Mô tả:  
Trải nghiệm các điểm đến văn hóa, sinh thái, nông nghiệp và làng nghề bằng bản đồ tương tác, ảnh 360 và thuyết minh số.

CTA chính:  
Khám phá bản đồ

CTA phụ:  
Xem địa điểm nổi bật

### **11.4. Cảm giác thị giác**

Hero nên tạo cảm giác rộng, thoáng, gợi khám phá:

Ảnh nền phong cảnh/sông nước/địa phương  
Overlay tối nhẹ  
Text trắng hoặc gần trắng  
Button primary nổi bật  
Animation fade-in khi vào trang

---

## **12\. Trang bản đồ khám phá**

### **12.1. Mục tiêu**

Đây là màn hình quan trọng nhất của public website. Người dùng có thể xem toàn bộ địa điểm trên bản đồ, lọc theo danh mục và mở chi tiết/tour 360\.

### **12.2. Layout desktop**

Gợi ý layout:

Header cố định phía trên  
Bên trái: panel tìm kiếm/lọc/danh sách địa điểm  
Bên phải: bản đồ chiếm phần lớn màn hình  
Popup marker hiển thị thông tin nhanh

Tỷ lệ:

Left panel: 360px \- 420px  
Map area: phần còn lại

### **12.3. Layout mobile**

Trên mobile:

Bản đồ chiếm toàn màn hình  
Search/filter nằm dạng bottom sheet  
Danh sách địa điểm có thể kéo lên  
Marker popup dạng card phía dưới

### **12.4. Thành phần giao diện**

Search input  
Category filter chips  
Toggle: chỉ hiển thị địa điểm có tour 360  
Destination list cards  
Map markers  
Marker popup  
Button “Xem chi tiết”  
Button “Tham quan 360”  
Current location button, mở rộng sau  
Reset map button

### **12.5. Marker trên bản đồ**

Marker nên có màu theo category.

Ví dụ:

Tâm linh: vàng cam  
Sinh thái: xanh lá  
Ẩm thực: đỏ cam  
Lưu trú: xanh dương  
Văn hóa: tím

Marker có thể có hiệu ứng pulse nhẹ cho địa điểm nổi bật hoặc có tour 360\.

### **12.6. Popup marker**

Khi bấm marker, popup hiển thị:

Ảnh đại diện nhỏ  
Tên địa điểm  
Danh mục  
Mô tả ngắn  
Badge “Có tour 360” nếu có  
Button “Xem chi tiết”  
Button “Tour 360”

---

## **13\. Trang danh sách địa điểm**

### **13.1. Mục tiêu**

Trang này dành cho người dùng muốn duyệt địa điểm dạng danh sách thay vì bản đồ.

### **13.2. Layout**

Header  
Page title  
Search/filter bar  
Grid card địa điểm  
Pagination hoặc load more

### **13.3. Card địa điểm**

Mỗi card gồm:

Ảnh đại diện  
Badge danh mục  
Tên địa điểm  
Mô tả ngắn  
Địa chỉ/khu vực  
Icon “Có tour 360”  
Button “Xem chi tiết”

### **13.4. Hiệu ứng card**

Hover nâng card lên nhẹ  
Ảnh zoom nhẹ khi hover  
Shadow mềm  
Badge category đổi màu theo nhóm

---

## **14\. Trang chi tiết địa điểm**

### **14.1. Mục tiêu**

Trang chi tiết giúp du khách hiểu rõ địa điểm và mở tour 360\.

### **14.2. Cấu trúc trang**

Hero image / cover  
Tên địa điểm  
Danh mục  
Địa chỉ  
Button “Tham quan 360”  
Button “Xem trên bản đồ”  
Gallery ảnh  
Mô tả chi tiết  
Video giới thiệu nếu có  
Audio thuyết minh nếu có  
Thông tin giờ mở cửa / giá vé / liên hệ  
Bản đồ vị trí nhỏ  
Địa điểm liên quan

### **14.3. CTA tour 360**

Nút “Tham quan 360” cần nổi bật.

Gợi ý:

Nền primary teal  
Icon rotate-3d hoặc scan  
Text: “Tham quan 360”  
Có animation pulse nhẹ nếu địa điểm có tour

### **14.4. Gallery**

Gallery nên hỗ trợ:

Grid ảnh  
Click mở lightbox  
Ảnh 360 có badge “360”  
Video có icon play

---

## **15\. Màn hình tour 360**

### **15.1. Mục tiêu**

Màn hình tour 360 là điểm “wow” của hệ thống. Người dùng cần có cảm giác đang đứng trong không gian thật và có thể di chuyển sang các vị trí khác.

### **15.2. Layout tổng quan**

Tour 360 nên dùng layout gần toàn màn hình:

360 viewer chiếm toàn màn hình  
Top bar trong suốt/tối nhẹ  
Bottom control bar  
Hotspot nổi trên ảnh  
Scene list hoặc mini map dạng panel  
Info/audio/video popup  
Button quay lại  
Fullscreen button

### **15.3. Top bar**

Top bar gồm:

Tên địa điểm  
Tên panorama hiện tại  
Nút quay lại chi tiết địa điểm  
Nút fullscreen  
Nút bật/tắt danh sách điểm nhìn

Nền:

rgba(15, 23, 42, 0.55)  
blur nhẹ  
text trắng

### **15.4. Bottom control bar**

Bottom bar có thể gồm:

Nút zoom in/out  
Nút reset view  
Nút bật/tắt auto rotate  
Nút bật/tắt âm thanh  
Nút mở scene list

### **15.5. Hotspot điều hướng**

Hotspot điều hướng là các nút mũi tên để di chuyển giữa các panorama.

Giao diện:

Icon mũi tên / chevron  
Nền tròn hoặc capsule  
Có label khi hover: “Đi vào sân chính”  
Có hiệu ứng pulse nhẹ  
Nằm đúng vị trí yaw/pitch trên ảnh 360

Khi click:

Hiệu ứng fade/blur ngắn  
Load panorama mới  
Cập nhật tên scene  
Render hotspot mới  
Có thể hiện loading spinner nhỏ

### **15.6. Hotspot thông tin**

Hotspot thông tin dùng icon `info`.

Khi click:

Mở popup hoặc side panel  
Hiển thị tiêu đề  
Hiển thị mô tả  
Có thể có ảnh/video/audio liên quan

### **15.7. Hotspot audio**

Hotspot audio dùng icon `volume`.

Khi click:

Mở mini audio player  
Hiển thị tiêu đề audio  
Nút play/pause  
Thanh tiến trình  
Transcript nếu có

### **15.8. Hotspot video**

Hotspot video dùng icon `play-circle`.

Khi click:

Mở modal video  
Nền tối  
Video player ở giữa  
Nút đóng rõ ràng

### **15.9. Scene list**

Scene list là danh sách các panorama trong tour.

Có thể hiển thị dạng:

Panel bên trái/phải trên desktop  
Bottom sheet trên mobile  
Thumbnail nhỏ của panorama  
Tên panorama  
Badge “đang xem”  
Click để chuyển scene

Ví dụ scene:

Cổng chính  
Sân chính  
Chánh điện  
Khu vườn

### **15.10. Trải nghiệm giống tour 360 chuyên nghiệp**

Hệ thống phải hỗ trợ luồng:

Người dùng đứng ở Panorama A  
→ xoay nhìn 360  
→ thấy mũi tên đi đến Panorama B  
→ bấm mũi tên  
→ chuyển sang Panorama B  
→ tiếp tục nhìn 360  
→ bấm mũi tên sang Panorama C hoặc quay lại Panorama A

Đây là yêu cầu cốt lõi của UI/UX tour 360\.

---

# **PHẦN B: GIAO DIỆN ADMIN DASHBOARD**

---

## **16\. Admin login page**

### **16.1. Mục tiêu**

Trang đăng nhập cần đơn giản, chuyên nghiệp và đáng tin cậy.

### **16.2. Layout**

Nửa trái: hình ảnh/gradient du lịch  
Nửa phải: form đăng nhập  
Logo dự án  
Email input  
Password input  
Button đăng nhập  
Thông báo lỗi nếu sai tài khoản

### **16.3. Style**

Background sáng hoặc gradient nhẹ  
Form card bo góc 24px  
Shadow mềm  
Button primary teal

---

## **17\. Admin dashboard overview**

### **17.1. Mục tiêu**

Dashboard tổng quan giúp admin biết nhanh tình trạng dữ liệu.

### **17.2. Thành phần**

Sidebar navigation  
Top bar  
Statistic cards  
Recent destinations  
Recent media uploads  
Quick actions

### **17.3. Statistic cards**

Các card thống kê:

Tổng số địa điểm  
Địa điểm đã xuất bản  
Tour 360  
Media files  
Hotspot  
Địa điểm draft

### **17.4. Quick actions**

Thêm địa điểm mới  
Upload media  
Tạo tour 360  
Quản lý danh mục

---

## **18\. Admin layout tổng thể**

### **18.1. Desktop layout**

Sidebar cố định bên trái  
Top bar phía trên  
Main content bên phải  
Card/table/form trong content area

### **18.2. Sidebar**

Sidebar gồm:

Dashboard  
Regions  
Categories  
Destinations  
Media Library  
Virtual Tours  
Panoramas  
Hotspots  
Translations  
Routes  
Settings

### **18.3. Top bar**

Top bar gồm:

Tên trang hiện tại  
Search nhanh nếu cần  
Nút tạo mới  
Avatar admin  
Menu tài khoản

### **18.4. Mobile admin**

Admin mobile không phải ưu tiên chính, nhưng vẫn nên responsive ở mức cơ bản:

Sidebar thu gọn thành drawer  
Table chuyển thành card list  
Form full width  
Map chọn địa điểm vẫn dùng được

---

## **19\. Trang quản lý địa điểm**

### **19.1. Mục tiêu**

Giúp admin xem, tìm kiếm, lọc, tạo và cập nhật địa điểm.

### **19.2. Danh sách địa điểm**

Hiển thị dạng table trên desktop.

Cột đề xuất:

Ảnh  
Tên địa điểm  
Danh mục  
Khu vực  
Có tour 360  
Trạng thái  
Ngày cập nhật  
Hành động

Hành động:

Xem  
Sửa  
Quản lý tour  
Ẩn/Xuất bản  
Xóa/Lưu trữ

### **19.3. Bộ lọc**

Tìm kiếm theo tên  
Lọc theo region  
Lọc theo category  
Lọc theo status  
Lọc địa điểm có tour 360

---

## **20\. Form tạo/sửa địa điểm**

### **20.1. Mục tiêu**

Cho phép admin nhập đầy đủ thông tin địa điểm và chọn vị trí trên bản đồ.

### **20.2. Layout form**

Form nên chia thành nhiều section:

Thông tin cơ bản  
Vị trí bản đồ  
Media đại diện  
Thông tin liên hệ  
Cấu hình hiển thị  
SEO/slug nếu cần

### **20.3. Thông tin cơ bản**

Các field:

Tên địa điểm  
Slug  
Danh mục  
Khu vực  
Mô tả ngắn  
Mô tả chi tiết  
Trạng thái

### **20.4. Vị trí bản đồ**

Section bản đồ gồm:

Map viewer Leaflet/Mapbox  
Marker vị trí hiện tại  
Instruction: “Click trên bản đồ để chọn vị trí”  
Input latitude  
Input longitude  
Button reset location

Luồng:

Admin click bản đồ  
→ marker xuất hiện  
→ latitude/longitude tự điền  
→ admin có thể kéo marker để chỉnh  
→ lưu form

### **20.5. Media đại diện**

Upload cover image  
Preview ảnh  
Chọn từ media library  
Alt text  
Caption

---

## **21\. Media Library**

### **21.1. Mục tiêu**

Quản lý toàn bộ file upload.

### **21.2. Layout**

Upload zone  
Filter theo media type  
Grid media cards  
Preview modal  
Thông tin metadata

### **21.3. Upload zone**

Upload zone nên hỗ trợ:

Drag and drop  
Click để chọn file  
Hiển thị tiến trình upload  
Thông báo lỗi định dạng/dung lượng

### **21.4. Media card**

Mỗi media card gồm:

Thumbnail  
Tên file  
Media type  
Dung lượng  
Ngày upload  
Menu hành động

Media type badge:

image  
panorama  
video  
audio  
model3d

---

## **22\. Trang quản lý Virtual Tour**

### **22.1. Mục tiêu**

Cho phép admin tạo và quản lý tour 360 của một địa điểm.

### **22.2. Layout**

Thông tin địa điểm  
Danh sách tour  
Button tạo tour mới  
Trạng thái tour  
Button quản lý panorama  
Button preview tour

### **22.3. Form tạo tour**

Field:

Tên tour  
Mô tả  
Thumbnail  
Panorama mặc định  
Trạng thái

---

## **23\. Trang quản lý Panorama**

### **23.1. Mục tiêu**

Cho phép admin upload và quản lý các điểm nhìn 360 trong một tour.

### **23.2. Layout**

Header: tên tour  
Button upload panorama  
Grid/list panorama  
Button set as default  
Button edit hotspots  
Button preview

### **23.3. Panorama card**

Mỗi card gồm:

Thumbnail panorama  
Tên panorama  
Thứ tự hiển thị  
Badge default nếu là scene mặc định  
Trạng thái  
Button sửa  
Button đặt hotspot

### **23.4. Upload panorama**

Admin có thể:

Upload một ảnh 360  
Upload nhiều ảnh 360  
Nhập tên từng panorama  
Sắp xếp thứ tự  
Chọn panorama mặc định

---

## **24\. Panorama Hotspot Editor**

### **24.1. Mục tiêu**

Đây là màn hình quan trọng nhất trong admin tour 360\. Nó cho phép admin đặt các hotspot trực tiếp trong ảnh 360\.

### **24.2. Layout desktop**

Bên trái hoặc trung tâm: 360 viewer editor  
Bên phải: panel danh sách hotspot và form chỉnh sửa  
Top bar: tên panorama, nút preview, nút lưu

### **24.3. Luồng tạo hotspot**

Admin mở panorama editor  
→ Viewer tải ảnh 360  
→ Admin xoay đến vị trí cần đặt hotspot  
→ Admin click vào điểm trên ảnh  
→ Hệ thống lấy yaw/pitch  
→ Form tạo hotspot mở ở side panel  
→ Admin chọn loại hotspot  
→ Admin nhập thông tin  
→ Admin lưu  
→ Hotspot xuất hiện trên viewer

### **24.4. Form hotspot**

Field chung:

Loại hotspot  
Tiêu đề  
Mô tả  
Yaw  
Pitch  
Icon  
Trạng thái

Field theo loại:

navigation:  
\- Target panorama

audio/video/image/model3d:  
\- Media file

external\_link:  
\- External URL

### **24.5. Hotspot type selector**

Nên hiển thị dạng card/radio:

Info  
Navigation  
Audio  
Video  
Image  
3D Model  
External Link

Mỗi loại có icon riêng để admin dễ hiểu.

### **24.6. Navigation hotspot trong editor**

Nếu admin chọn `Navigation`, form phải hiện:

Dropdown chọn panorama đích  
Preview thumbnail panorama đích  
Label mũi tên  
Icon mũi tên

Validation UI:

Không cho lưu nếu chưa chọn target panorama  
Không cho chọn chính panorama hiện tại nếu không cần thiết  
Cảnh báo nếu target panorama đang draft

### **24.7. Preview tour**

Admin cần có nút:

Preview as Tourist

Khi bấm, admin xem tour giống du khách để kiểm tra:

Mũi tên có đúng hướng không  
Click có chuyển panorama không  
Hotspot có đúng vị trí không  
Audio/video có mở được không

---

# **PHẦN C: ANIMATION VÀ MICRO-INTERACTION**

---

## **25\. Animation tổng thể**

Animation nên nhẹ, mượt, không gây rối.

### **25.1. Public website**

Sử dụng:

Fade in khi section xuất hiện  
Slide up nhẹ cho card  
Image zoom khi hover  
Marker pulse nhẹ  
Button hover scale nhẹ  
Popup map fade/scale

### **25.2. Admin dashboard**

Admin animation nên đơn giản:

Table row hover  
Button hover  
Modal fade  
Toast slide-in  
Upload progress  
Skeleton loading

### **25.3. Tour 360**

Tour 360 nên có animation:

Hotspot pulse  
Hotspot hover label  
Fade transition khi chuyển panorama  
Loading spinner khi tải ảnh 360  
Control bar fade in/out  
Scene thumbnail hover

---

## **26\. Micro-interactions quan trọng**

### **26.1. Khi admin click bản đồ**

Marker rơi xuống nhẹ  
Tọa độ được cập nhật ngay  
Toast: “Đã chọn vị trí”

### **26.2. Khi upload file**

Progress bar  
Preview thumbnail sau khi upload  
Toast thành công  
Thông báo lỗi nếu sai định dạng

### **26.3. Khi tạo hotspot**

Hotspot xuất hiện với animation scale-in  
Panel form tự động mở  
Toast: “Đã thêm hotspot”

### **26.4. Khi chuyển panorama**

Màn hình fade out rất ngắn  
Hiển thị loading  
Ảnh mới fade in  
Tên panorama cập nhật

---

# **PHẦN D: RESPONSIVE DESIGN**

---

## **27\. Breakpoints đề xuất**

Nếu dùng TailwindCSS:

sm: 640px  
md: 768px  
lg: 1024px  
xl: 1280px  
2xl: 1536px

### **27.1. Public website**

Ưu tiên:

Mobile-first  
Map dễ thao tác  
Tour 360 fullscreen tốt  
Card địa điểm dễ bấm  
Button đủ lớn

### **27.2. Admin dashboard**

Ưu tiên:

Desktop-first  
Table rõ ràng  
Form nhiều section  
Map chọn vị trí rộng  
Hotspot editor dễ thao tác

---

## **28\. Mobile UX cho tour 360**

Trên mobile:

Viewer toàn màn hình  
Top bar thu gọn  
Bottom controls dạng icon  
Scene list dạng bottom sheet  
Hotspot đủ lớn để chạm  
Popup thông tin dạng bottom sheet  
Audio player dạng mini bar

Kích thước hotspot tối thiểu:

44px x 44px

---

## **29\. Mobile UX cho bản đồ**

Trên mobile:

Map full screen  
Search bar nổi ở top  
Filter chips cuộn ngang  
Bottom sheet danh sách địa điểm  
Marker popup dạng card dưới màn hình

---

# **PHẦN E: STATES, FEEDBACK VÀ ERROR UX**

---

## **30\. Loading states**

Cần có loading cho:

Trang bản đồ  
Danh sách địa điểm  
Chi tiết địa điểm  
Tour 360  
Media upload  
Admin table  
Form submit

Gợi ý:

Skeleton card cho danh sách  
Spinner cho tour 360  
Progress bar cho upload  
Loading overlay cho form submit

---

## **31\. Empty states**

Các empty state cần có:

Không có địa điểm  
Không có kết quả tìm kiếm  
Chưa có tour 360  
Chưa có panorama  
Chưa có hotspot  
Chưa upload media

Ví dụ:

Chưa có panorama nào trong tour này.  
Hãy upload ảnh 360 đầu tiên để bắt đầu tạo trải nghiệm tham quan.

---

## **32\. Error states**

Các lỗi cần hiển thị rõ:

Không tải được bản đồ  
Không tải được tour 360  
Ảnh 360 không tồn tại  
Upload file thất bại  
Token admin hết hạn  
Không có quyền thao tác  
Validation form sai

Ví dụ thông báo:

Không thể tải tour 360\. Vui lòng thử lại sau.  
File không hợp lệ. Chỉ hỗ trợ JPG, PNG, WEBP cho hình ảnh.  
Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.

---

## **33\. Toast notification**

Toast dùng cho các thao tác:

Tạo thành công  
Cập nhật thành công  
Xóa thành công  
Upload thành công  
Lỗi validation  
Lỗi hệ thống

Vị trí:

Admin: góc phải trên  
Public: dưới cùng hoặc góc phải dưới

---

# **PHẦN F: ACCESSIBILITY VÀ USABILITY**

---

## **34\. Accessibility cơ bản**

Hệ thống nên đảm bảo:

Text đủ tương phản  
Button có label rõ  
Icon quan trọng có tooltip hoặc text  
Form field có label  
Lỗi form hiển thị gần field  
Có thể dùng bàn phím ở các form admin  
Không chỉ dùng màu để thể hiện trạng thái

### **34.1. Kích thước click/tap**

Button mobile tối thiểu 44px chiều cao  
Hotspot mobile tối thiểu 44px  
Marker popup button dễ bấm

### **34.2. Tour 360 usability**

Tour 360 cần có hướng dẫn ngắn khi lần đầu mở:

Kéo để xoay nhìn xung quanh  
Bấm mũi tên để di chuyển  
Bấm biểu tượng thông tin để xem nội dung

Có thể hiển thị overlay hướng dẫn trong 3–5 giây đầu.

---

# **PHẦN G: COMPONENT DESIGN**

---

## **35\. Component chính cho public website**

AppHeader  
HeroSection  
SearchBar  
CategoryChip  
DestinationCard  
MapView  
MapMarker  
MapPopup  
DestinationGallery  
AudioPlayer  
VideoModal  
Tour360Viewer  
HotspotButton  
SceneListPanel  
Footer

---

## **36\. Component chính cho admin**

AdminLayout  
Sidebar  
Topbar  
DataTable  
StatusBadge  
CategoryBadge  
FormInput  
RichTextEditor  
MapLocationPicker  
MediaUploader  
MediaLibrary  
TourForm  
PanoramaCard  
PanoramaViewerEditor  
HotspotForm  
Toast  
ConfirmDialog  
LoadingOverlay

---

## **37\. Component Tour360Viewer**

Component này là trung tâm của trải nghiệm 360\.

Props gợi ý:

tour  
defaultPanoramaId  
readonly  
onHotspotClick  
onPanoramaChange

State nội bộ:

currentPanoramaId  
currentPanorama  
isLoadingPanorama  
isSceneListOpen  
activeHotspot  
isFullscreen

Hành vi:

Load panorama mặc định  
Render hotspot theo panorama hiện tại  
Click navigation hotspot thì đổi currentPanoramaId  
Click info hotspot thì mở popup  
Click audio hotspot thì mở audio player  
Click video hotspot thì mở video modal

---

## **38\. Component MapLocationPicker**

Dùng cho admin chọn tọa độ.

Props gợi ý:

initialLatitude  
initialLongitude  
centerLatitude  
centerLongitude  
zoom  
onLocationChange

Hành vi:

Click bản đồ để chọn vị trí  
Kéo marker để chỉnh vị trí  
Cập nhật latitude/longitude  
Hiển thị marker preview

---

## **39\. Component PanoramaViewerEditor**

Dùng cho admin đặt hotspot.

Props gợi ý:

panorama  
hotspots  
mode: edit/preview  
onCreateHotspot  
onUpdateHotspot  
onDeleteHotspot

Hành vi:

Load ảnh 360  
Render hotspot hiện có  
Admin click vào ảnh để tạo hotspot  
Lấy yaw/pitch  
Mở form tạo hotspot  
Cho phép preview navigation

---

# **PHẦN H: GỢI Ý TRIỂN KHAI FRONTEND**

---

## **40\. Công nghệ frontend đề xuất**

Vue.js 3  
Vue Router  
Pinia  
TailwindCSS  
Lucide Vue Icons  
Leaflet hoặc Mapbox GL JS  
Photo Sphere Viewer hoặc Pannellum  
Axios  
VueUse nếu cần

### **40.1. Gợi ý thư viện animation**

Vue transition  
CSS transition  
GSAP nếu cần hiệu ứng phức tạp  
Framer Motion không áp dụng trực tiếp cho Vue, có thể thay bằng Motion One

MVP chỉ cần CSS transition là đủ.

---

## **41\. Cấu trúc thư mục frontend gợi ý**

src/  
  assets/  
  components/  
    common/  
    public/  
    admin/  
    map/  
    tour360/  
    media/  
  layouts/  
    PublicLayout.vue  
    AdminLayout.vue  
  pages/  
    public/  
      HomePage.vue  
      ExploreMapPage.vue  
      DestinationListPage.vue  
      DestinationDetailPage.vue  
      Tour360Page.vue  
    admin/  
      LoginPage.vue  
      DashboardPage.vue  
      DestinationManagementPage.vue  
      DestinationFormPage.vue  
      MediaLibraryPage.vue  
      TourManagementPage.vue  
      PanoramaManagementPage.vue  
      HotspotEditorPage.vue  
  stores/  
    auth.store.ts  
    destination.store.ts  
    tour.store.ts  
  services/  
    api.ts  
    auth.service.ts  
    destination.service.ts  
    media.service.ts  
    tour.service.ts  
  router/  
  utils/  
  styles/

---

# **PHẦN I: MVP UI SCOPE**

---

## **42\. Màn hình bắt buộc trong MVP**

Public:

Trang chủ  
Trang bản đồ khám phá  
Trang danh sách địa điểm  
Trang chi tiết địa điểm  
Trang tour 360

Admin:

Đăng nhập  
Dashboard  
Quản lý danh mục  
Quản lý địa điểm  
Form tạo/sửa địa điểm có map picker  
Media Library  
Quản lý tour  
Quản lý panorama  
Hotspot editor

---

## **43\. Màn hình có thể để sau MVP**

Tuyến tham quan  
Tài khoản du khách  
Yêu thích  
Đánh giá  
Đặt lịch  
Dashboard analytics nâng cao  
Quản lý bản dịch chuyên sâu  
Model 3D viewer  
AR check-in

---

## **44\. Tiêu chí UI/UX thành công cho MVP**

MVP được xem là đạt về UI/UX nếu:

Du khách hiểu ngay cách mở bản đồ  
Du khách bấm marker và mở được chi tiết địa điểm  
Du khách mở được tour 360 trong tối đa 2 bước từ trang chi tiết  
Du khách biết bấm mũi tên để di chuyển giữa panorama  
Hotspot dễ nhìn, dễ bấm  
Admin tạo được địa điểm mà không cần nhập tọa độ thủ công  
Admin upload được ảnh 360 và tạo panorama  
Admin đặt được hotspot navigation sang panorama khác  
Giao diện mobile của public website dùng tốt  
Admin dashboard rõ ràng trên desktop

---

## **45\. Kết luận**

UI/UX của **Smart Tourism 360 Platform** cần kết hợp ba yếu tố: du lịch, bản đồ và trải nghiệm nhập vai 360 độ. Giao diện public phải tạo cảm giác khám phá, hiện đại và trực quan cho du khách. Giao diện admin phải rõ ràng, hiệu quả và hỗ trợ tốt các thao tác quản lý dữ liệu, chọn vị trí trên bản đồ, upload media, tạo tour 360 và đặt hotspot.

Điểm quan trọng nhất của trải nghiệm người dùng là màn hình **Tour 360**. Hệ thống cần hỗ trợ tour nhiều panorama, trong đó người dùng có thể đứng tại một điểm nhìn 360, xoay quan sát, bấm mũi tên để di chuyển sang điểm khác và tiếp tục khám phá. Đây là trải nghiệm cốt lõi giúp dự án khác với một website giới thiệu du lịch thông thường.

Với định hướng UI/UX này, nhóm phát triển có thể triển khai MVP có giao diện chuyên nghiệp, dễ dùng và đủ hấp dẫn để demo cho giảng viên, địa phương hoặc nhà đầu tư. Sau MVP, hệ thống có thể tiếp tục mở rộng sang WebGIS 3D, mô hình 3D, AI hướng dẫn viên, gợi ý lịch trình và các trải nghiệm du lịch thông minh nâng cao.

