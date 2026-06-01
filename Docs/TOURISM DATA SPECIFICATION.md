# **TOURISM DATA SPECIFICATION**

# **Smart Tourism 360 Platform**

## **Đặc tả dữ liệu du lịch cho nền tảng bản đồ tương tác và tham quan ảo 360 độ**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | Tourism Data Specification |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v1.0 |
| Mục đích | Đặc tả các nhóm dữ liệu du lịch cần quản lý trong hệ thống |
| Đối tượng đọc | Nhóm phát triển, phân tích nghiệp vụ, thiết kế database, thiết kế API, thiết kế UI/UX |
| Phạm vi | Dữ liệu khu vực, địa điểm, danh mục, media, tour 360, panorama, hotspot, audio, video, mô hình 3D, tuyến tham quan và dữ liệu đa ngôn ngữ |

---

## **2\. Mục đích của tài liệu**

Tài liệu này được xây dựng nhằm mô tả chi tiết các loại dữ liệu cần có trong hệ thống **Smart Tourism 360 Platform**. Đây là tài liệu trung gian giữa tài liệu ý tưởng dự án và các tài liệu kỹ thuật như thiết kế cơ sở dữ liệu, thiết kế API và thiết kế giao diện.

Vì hệ thống hướng đến việc quản lý điểm du lịch, bản đồ, ảnh 360, nội dung thuyết minh và dữ liệu đa phương tiện, việc xác định rõ cấu trúc dữ liệu ngay từ đầu là rất quan trọng. Nếu dữ liệu được thiết kế tốt, hệ thống sẽ dễ mở rộng, dễ bảo trì và dễ thay thế dữ liệu mẫu bằng dữ liệu thực tế sau này.

Tài liệu này trả lời các câu hỏi chính:

1. Hệ thống cần quản lý những loại dữ liệu nào?  
2. Một địa điểm du lịch cần có những thông tin gì?  
3. Ảnh 360, panorama, tour 360 và hotspot được mô tả như thế nào?  
4. Media như ảnh, video, audio và mô hình 3D cần lưu những thuộc tính gì?  
5. Dữ liệu đa ngôn ngữ nên được chuẩn bị ra sao?  
6. Dữ liệu mẫu cho MVP nên có cấu trúc như thế nào?  
7. Cần tuân thủ quy tắc nhập liệu và đặt tên file như thế nào?

---

## **3\. Nguyên tắc thiết kế dữ liệu**

Dữ liệu của hệ thống cần được thiết kế theo các nguyên tắc sau:

### **3.1. Tổng quát và linh hoạt**

Hệ thống không được thiết kế chỉ cho một địa phương duy nhất. Dữ liệu cần đủ tổng quát để có thể triển khai cho nhiều khu vực khác nhau như Cần Thơ, An Giang, Vĩnh Long hoặc các địa phương khác.

Ví dụ, thay vì cố định dữ liệu cho một tỉnh cụ thể, hệ thống nên có khái niệm **region** hoặc **area** để đại diện cho khu vực triển khai.

### **3.2. Phù hợp với MVP nhưng có khả năng mở rộng**

Trong MVP, hệ thống chủ yếu cần quản lý địa điểm, bản đồ, ảnh 360, hotspot, video, audio và nội dung mô tả. Tuy nhiên, dữ liệu cũng cần được thiết kế sẵn để hỗ trợ các tính năng tương lai như mô hình 3D, đa ngôn ngữ, tuyến tham quan, AI chatbot và WebGIS 3D.

### **3.3. Tách biệt metadata và file vật lý**

Database chỉ nên lưu thông tin mô tả về file, còn file thực tế nên được lưu trong object storage như MinIO, Cloudflare R2 hoặc S3.

Ví dụ, database lưu:

file\_name  
file\_url  
file\_type  
mime\_type  
size  
storage\_provider

Còn file ảnh/video/audio thực tế được lưu trong storage.

### **3.4. Hỗ trợ dữ liệu bản đồ**

Mỗi địa điểm cần có tọa độ địa lý để hiển thị trên bản đồ. Hệ thống nên hỗ trợ latitude/longitude trong MVP và có thể mở rộng sang dữ liệu hình học phức tạp hơn như polygon, line hoặc boundary trong tương lai.

### **3.5. Hỗ trợ đa ngôn ngữ từ sớm**

MVP có thể chỉ dùng tiếng Việt, nhưng dữ liệu cần thiết kế sẵn để hỗ trợ tiếng Anh hoặc ngôn ngữ khác trong tương lai.

Ví dụ, tên địa điểm, mô tả ngắn, mô tả chi tiết, nội dung hotspot và nội dung thuyết minh nên có khả năng dịch sang nhiều ngôn ngữ.

### **3.6. Có trạng thái xuất bản**

Không phải dữ liệu nào tạo ra cũng hiển thị ngay cho du khách. Vì vậy các đối tượng chính như địa điểm, tour, panorama, hotspot và media nên có trạng thái như draft, published hoặc archived.

### **3.7. Dễ kiểm duyệt và quản trị**

Dữ liệu cần có thông tin người tạo, người cập nhật, thời gian tạo, thời gian cập nhật để phục vụ quản trị và kiểm soát nội dung.

---

## **4\. Tổng quan các nhóm dữ liệu trong hệ thống**

Hệ thống cần quản lý các nhóm dữ liệu chính sau:

| Nhóm dữ liệu | Vai trò |
| ----- | ----- |
| Region / Area | Đại diện cho khu vực triển khai |
| Destination | Địa điểm du lịch hoặc điểm tham quan |
| Category | Phân loại địa điểm |
| Media File | Quản lý file ảnh, video, audio, ảnh 360, mô hình 3D |
| Virtual Tour | Tour tham quan ảo của một địa điểm |
| Panorama | Một điểm nhìn 360 trong tour |
| Hotspot | Điểm tương tác trong ảnh 360 |
| Audio Guide | Nội dung thuyết minh bằng âm thanh |
| Video | Video giới thiệu hoặc video thuyết minh |
| 3D Model | Mô hình 3D gắn với địa điểm hoặc hotspot |
| Route / Itinerary | Tuyến tham quan gồm nhiều địa điểm |
| Translation | Dữ liệu đa ngôn ngữ |
| Analytics Event | Dữ liệu thống kê hành vi người dùng |
| User / Admin | Người quản trị hoặc người dùng hệ thống |

Trong MVP, các nhóm dữ liệu quan trọng nhất là:

Region / Area  
Destination  
Category  
Media File  
Virtual Tour  
Panorama  
Hotspot  
Audio Guide  
User / Admin

Các nhóm như 3D Model, Route, Translation và Analytics có thể thiết kế sẵn nhưng triển khai ở mức cơ bản hoặc để mở rộng sau.

---

## **5\. Dữ liệu khu vực triển khai**

### **5.1. Khái niệm**

Khu vực triển khai là phạm vi địa lý mà hệ thống đang số hóa hoặc quảng bá. Khu vực này có thể là một tỉnh, thành phố, huyện, xã, khu du lịch, cụm điểm du lịch hoặc tuyến du lịch.

Ví dụ:

Cần Thơ  
Vĩnh Long  
An Giang  
Cù lao An Bình  
Một tuyến du lịch ven sông  
Một cụm điểm du lịch sinh thái

### **5.2. Thuộc tính đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã định danh khu vực |
| name | string | Có | Tên khu vực |
| slug | string | Có | Đường dẫn thân thiện |
| description | text | Không | Mô tả tổng quan |
| province | string | Không | Tỉnh/thành |
| district | string | Không | Quận/huyện |
| ward | string | Không | Xã/phường |
| center\_latitude | decimal | Không | Vĩ độ trung tâm bản đồ |
| center\_longitude | decimal | Không | Kinh độ trung tâm bản đồ |
| default\_zoom | integer | Không | Mức zoom bản đồ mặc định |
| cover\_image\_id | UUID / integer | Không | Ảnh đại diện khu vực |
| status | enum | Có | draft/published/archived |
| created\_at | datetime | Có | Thời gian tạo |
| updated\_at | datetime | Có | Thời gian cập nhật |

### **5.3. Ghi chú thiết kế**

Trong MVP, hệ thống có thể chỉ cần một khu vực mặc định. Tuy nhiên, vẫn nên thiết kế bảng khu vực để sau này mở rộng thành hệ thống nhiều khu vực.

Ví dụ:

Smart Tourism 360 Platform  
  └── Region: Cần Thơ Demo  
        ├── Destination 1  
        ├── Destination 2  
        └── Destination 3

---

## **6\. Dữ liệu danh mục địa điểm**

### **6.1. Khái niệm**

Danh mục dùng để phân loại các địa điểm du lịch. Mỗi địa điểm có thể thuộc một hoặc nhiều danh mục tùy thiết kế.

Ví dụ danh mục:

Văn hóa \- lịch sử  
Tâm linh  
Sinh thái  
Nông nghiệp  
Làng nghề  
Ẩm thực  
Lưu trú  
Check-in  
Giải trí  
Sản phẩm địa phương

### **6.2. Thuộc tính đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã danh mục |
| name | string | Có | Tên danh mục |
| slug | string | Có | Đường dẫn thân thiện |
| description | text | Không | Mô tả danh mục |
| icon | string | Không | Tên icon dùng trên giao diện |
| color | string | Không | Màu đại diện danh mục |
| display\_order | integer | Không | Thứ tự hiển thị |
| status | enum | Có | active/inactive |
| created\_at | datetime | Có | Thời gian tạo |
| updated\_at | datetime | Có | Thời gian cập nhật |

### **6.3. Danh mục đề xuất cho MVP**

| Mã | Tên danh mục | Mô tả |
| ----- | ----- | ----- |
| culture | Văn hóa \- lịch sử | Đình, nhà cổ, di tích, bảo tàng |
| spiritual | Tâm linh | Chùa, miếu, nhà thờ, công trình tín ngưỡng |
| eco | Sinh thái | Khu sinh thái, vườn xanh, điểm ven sông |
| agriculture | Nông nghiệp | Vườn trái cây, farmstay, trải nghiệm nông nghiệp |
| craft | Làng nghề | Cơ sở sản xuất thủ công, nghề truyền thống |
| food | Ẩm thực | Quán ăn, món đặc sản, khu ẩm thực |
| stay | Lưu trú | Homestay, farmstay, nhà vườn |

---

## **7\. Dữ liệu địa điểm du lịch**

### **7.1. Khái niệm**

Địa điểm du lịch là đơn vị dữ liệu trung tâm của hệ thống. Mỗi địa điểm đại diện cho một nơi mà du khách có thể xem thông tin, định vị trên bản đồ và tham quan ảo.

Ví dụ:

Một ngôi chùa  
Một nhà cổ  
Một vườn trái cây  
Một khu sinh thái  
Một làng nghề  
Một homestay  
Một quán ăn đặc sản  
Một bến tàu du lịch

### **7.2. Thuộc tính đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã địa điểm |
| region\_id | UUID / integer | Có | Khu vực chứa địa điểm |
| name | string | Có | Tên địa điểm |
| slug | string | Có | Đường dẫn thân thiện |
| short\_description | text | Có | Mô tả ngắn |
| full\_description | text | Không | Mô tả chi tiết |
| address | string | Không | Địa chỉ |
| latitude | decimal | Có | Vĩ độ |
| longitude | decimal | Có | Kinh độ |
| category\_id | UUID / integer | Có | Danh mục chính |
| cover\_image\_id | UUID / integer | Không | Ảnh đại diện |
| opening\_hours | string / json | Không | Giờ mở cửa |
| ticket\_price | string | Không | Giá vé hoặc ghi chú chi phí |
| contact\_phone | string | Không | Số điện thoại liên hệ |
| contact\_email | string | Không | Email liên hệ |
| website\_url | string | Không | Website nếu có |
| facebook\_url | string | Không | Facebook/Zalo page nếu có |
| has\_virtual\_tour | boolean | Có | Có tour 360 hay không |
| has\_audio\_guide | boolean | Có | Có thuyết minh âm thanh hay không |
| has\_3d\_model | boolean | Có | Có mô hình 3D hay không |
| status | enum | Có | draft/published/archived |
| created\_by | UUID / integer | Không | Người tạo |
| updated\_by | UUID / integer | Không | Người cập nhật |
| created\_at | datetime | Có | Thời gian tạo |
| updated\_at | datetime | Có | Thời gian cập nhật |

### **7.3. Các trường bắt buộc trong MVP**

Đối với MVP, mỗi địa điểm nên có tối thiểu:

name  
short\_description  
address hoặc area  
latitude  
longitude  
category  
cover\_image  
status

Nếu có tour 360, cần thêm:

virtual\_tour  
panorama  
hotspot  
audio/text guide nếu có

### **7.4. Ghi chú về tọa độ**

Admin cần có thể chọn vị trí trực tiếp trên bản đồ. Khi admin click vào bản đồ, hệ thống tự động lấy:

latitude  
longitude

Các tọa độ này dùng để hiển thị marker cho du khách.

Trong tương lai, hệ thống có thể mở rộng thêm:

boundary polygon  
walking route  
tour route line  
nearby destination query  
distance calculation

---

## **8\. Dữ liệu media**

### **8.1. Khái niệm**

Media là tất cả các file được upload và sử dụng trong hệ thống, bao gồm:

Ảnh đại diện  
Ảnh thường  
Ảnh 360  
Video  
Audio  
Mô hình 3D  
Thumbnail  
Tài liệu đính kèm nếu có

### **8.2. Nguyên tắc quản lý media**

Hệ thống nên có một kho media chung. Mỗi file media có thể được gắn với một địa điểm, một panorama, một hotspot, một audio guide hoặc một model 3D.

Database chỉ lưu metadata, file thực tế lưu ở object storage.

### **8.3. Thuộc tính đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã media |
| file\_name | string | Có | Tên file gốc |
| stored\_file\_name | string | Có | Tên file sau khi lưu |
| file\_url | string | Có | URL truy cập file |
| storage\_path | string | Có | Đường dẫn trong storage |
| media\_type | enum | Có | image/panorama/video/audio/model3d/document |
| mime\_type | string | Có | MIME type |
| file\_size | long | Có | Dung lượng file |
| extension | string | Có | Phần mở rộng file |
| width | integer | Không | Chiều rộng ảnh/video |
| height | integer | Không | Chiều cao ảnh/video |
| duration | integer | Không | Thời lượng video/audio tính bằng giây |
| thumbnail\_url | string | Không | URL thumbnail |
| alt\_text | string | Không | Mô tả thay thế |
| caption | string | Không | Chú thích |
| uploaded\_by | UUID / integer | Không | Người upload |
| created\_at | datetime | Có | Thời gian upload |

### **8.4. Các loại media cần hỗ trợ**

| Loại media | Mục đích | MVP |
| ----- | ----- | ----- |
| image | Ảnh đại diện, ảnh gallery | Có |
| panorama | Ảnh 360 | Có |
| video | Video giới thiệu | Có |
| audio | Audio thuyết minh | Có |
| model3d | File .glb/.gltf | Thiết kế sẵn |
| document | Tài liệu đính kèm | Chưa bắt buộc |

### **8.5. Định dạng file đề xuất**

| Loại file | Định dạng đề xuất |
| ----- | ----- |
| Ảnh thường | .jpg, .jpeg, .png, .webp |
| Ảnh 360 | .jpg, .jpeg, .webp |
| Video | .mp4, .webm |
| Audio | .mp3, .wav, .ogg |
| Mô hình 3D | .glb, .gltf |
| Thumbnail | .jpg, .webp |

---

## **9\. Dữ liệu tour 360**

### **9.1. Khái niệm**

Tour 360 là trải nghiệm tham quan ảo của một địa điểm. Một địa điểm có thể có một hoặc nhiều tour 360\. Mỗi tour gồm nhiều panorama, các panorama được liên kết với nhau thông qua hotspot chuyển cảnh.

Ví dụ:

Tour 360: Tham quan Chùa A  
  ├── Panorama 1: Cổng chính  
  ├── Panorama 2: Sân chùa  
  ├── Panorama 3: Chánh điện  
  └── Panorama 4: Khu tháp

### **9.2. Thuộc tính đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã tour |
| destination\_id | UUID / integer | Có | Địa điểm sở hữu tour |
| title | string | Có | Tên tour |
| description | text | Không | Mô tả tour |
| thumbnail\_id | UUID / integer | Không | Ảnh đại diện tour |
| default\_panorama\_id | UUID / integer | Không | Panorama mở đầu |
| status | enum | Có | draft/published/archived |
| created\_at | datetime | Có | Thời gian tạo |
| updated\_at | datetime | Có | Thời gian cập nhật |

### **9.3. Quy tắc dữ liệu**

Một địa điểm có thể chưa có tour 360\.

Một địa điểm có thể có nhiều tour, ví dụ:

Tour ban ngày  
Tour ban đêm  
Tour khu chính  
Tour khu trải nghiệm

Trong MVP, mỗi địa điểm chỉ cần tối đa một tour 360\.

---

## **10\. Dữ liệu panorama**

### **10.1. Khái niệm**

Panorama là một ảnh 360 đại diện cho một vị trí đứng trong không gian tham quan ảo. Người dùng có thể xoay nhìn xung quanh tại vị trí đó.

Ví dụ:

Cổng chính  
Sân trung tâm  
Khu vườn  
Nhà trưng bày  
Bến tàu  
Khu sản xuất làng nghề

### **10.2. Thuộc tính đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã panorama |
| virtual\_tour\_id | UUID / integer | Có | Tour chứa panorama |
| title | string | Có | Tên điểm nhìn |
| description | text | Không | Mô tả điểm nhìn |
| panorama\_image\_id | UUID / integer | Có | File ảnh 360 |
| thumbnail\_id | UUID / integer | Không | Thumbnail |
| display\_order | integer | Không | Thứ tự hiển thị |
| initial\_yaw | decimal | Không | Góc nhìn ngang ban đầu |
| initial\_pitch | decimal | Không | Góc nhìn dọc ban đầu |
| initial\_fov | decimal | Không | Góc nhìn rộng ban đầu |
| status | enum | Có | draft/published/archived |
| created\_at | datetime | Có | Thời gian tạo |
| updated\_at | datetime | Có | Thời gian cập nhật |

### **10.3. Ghi chú về góc nhìn**

Các thông số như yaw, pitch và fov dùng cho trình xem ảnh 360\.

Giải thích đơn giản:

yaw: góc xoay trái/phải  
pitch: góc nhìn lên/xuống  
fov: độ rộng góc nhìn

Trong MVP, có thể để mặc định nếu chưa cần chỉnh chi tiết.

---

## **11\. Dữ liệu hotspot**

### **11.1. Khái niệm**

Hotspot là điểm tương tác được đặt trong một panorama. Khi người dùng bấm vào hotspot, hệ thống có thể hiển thị thông tin, phát audio, mở video, chuyển sang panorama khác hoặc mở mô hình 3D.

### **11.2. Loại hotspot đề xuất**

| Loại hotspot | Mô tả | MVP |
| ----- | ----- | ----- |
| info | Hiển thị thông tin văn bản/hình ảnh | Có |
| navigation | Chuyển sang panorama khác | Có |
| audio | Phát audio thuyết minh | Có |
| video | Mở video | Có thể có |
| image | Mở ảnh chi tiết | Có thể có |
| model3d | Mở mô hình 3D | Thiết kế sẵn |
| external\_link | Mở liên kết ngoài | Chưa bắt buộc |

### **11.3. Thuộc tính đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã hotspot |
| panorama\_id | UUID / integer | Có | Panorama chứa hotspot |
| type | enum | Có | info/navigation/audio/video/image/model3d/link |
| title | string | Có | Tên hotspot |
| description | text | Không | Nội dung mô tả |
| yaw | decimal | Có | Vị trí ngang trong ảnh 360 |
| pitch | decimal | Có | Vị trí dọc trong ảnh 360 |
| target\_panorama\_id | UUID / integer | Không | Panorama đích nếu là navigation |
| media\_id | UUID / integer | Không | File media liên quan |
| icon | string | Không | Icon hotspot |
| display\_order | integer | Không | Thứ tự ưu tiên |
| status | enum | Có | draft/published/archived |
| created\_at | datetime | Có | Thời gian tạo |
| updated\_at | datetime | Có | Thời gian cập nhật |

### **11.4. Quy tắc dữ liệu hotspot**

Nếu hotspot có type là `navigation`, cần có:

target\_panorama\_id

Nếu hotspot có type là `audio`, `video`, `image` hoặc `model3d`, cần có:

media\_id

Nếu hotspot có type là `info`, cần có ít nhất:

title  
description

---

## **12\. Dữ liệu audio guide**

### **12.1. Khái niệm**

Audio guide là dữ liệu thuyết minh bằng âm thanh. Audio có thể gắn với địa điểm, tour, panorama hoặc hotspot.

Ví dụ:

Thuyết minh tổng quan về địa điểm  
Thuyết minh về một khu vực trong ảnh 360  
Thuyết minh về một hiện vật/làng nghề/món ăn

### **12.2. Thuộc tính đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã audio guide |
| title | string | Có | Tiêu đề audio |
| transcript | text | Không | Nội dung lời thuyết minh |
| media\_id | UUID / integer | Có | File audio |
| language\_code | string | Có | vi/en/km... |
| target\_type | enum | Có | destination/tour/panorama/hotspot |
| target\_id | UUID / integer | Có | ID đối tượng được gắn audio |
| duration | integer | Không | Thời lượng |
| status | enum | Có | draft/published/archived |
| created\_at | datetime | Có | Thời gian tạo |
| updated\_at | datetime | Có | Thời gian cập nhật |

### **12.3. Ghi chú MVP**

Trong MVP, có thể chưa cần thu âm thật. Có thể dùng:

audio giả lập  
text-to-speech  
file audio mẫu  
hoặc chỉ lưu transcript trước

---

## **13\. Dữ liệu video**

### **13.1. Khái niệm**

Video dùng để giới thiệu địa điểm, mô tả hoạt động du lịch hoặc minh họa nội dung văn hóa, nông nghiệp, làng nghề.

Video có thể gắn với:

destination  
tour  
panorama  
hotspot

### **13.2. Thuộc tính đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã video record |
| title | string | Có | Tiêu đề video |
| description | text | Không | Mô tả video |
| media\_id | UUID / integer | Có | File video |
| target\_type | enum | Có | destination/tour/panorama/hotspot |
| target\_id | UUID / integer | Có | Đối tượng liên kết |
| thumbnail\_id | UUID / integer | Không | Thumbnail |
| duration | integer | Không | Thời lượng |
| status | enum | Có | draft/published/archived |
| created\_at | datetime | Có | Thời gian tạo |
| updated\_at | datetime | Có | Thời gian cập nhật |

---

## **14\. Dữ liệu mô hình 3D**

### **14.1. Khái niệm**

Mô hình 3D đại diện cho một đối tượng, công trình, hiện vật hoặc không gian được số hóa. Trong MVP, hệ thống chưa bắt buộc có viewer 3D, nhưng database/API nên thiết kế sẵn để hỗ trợ upload và gắn mô hình 3D.

Ví dụ mô hình 3D:

Mô hình cổng chùa  
Mô hình nhà cổ  
Mô hình hiện vật  
Mô hình sản phẩm làng nghề  
Mô hình khu vực tham quan

### **14.2. Thuộc tính đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã model |
| title | string | Có | Tên mô hình |
| description | text | Không | Mô tả |
| media\_id | UUID / integer | Có | File .glb/.gltf |
| thumbnail\_id | UUID / integer | Không | Ảnh đại diện |
| target\_type | enum | Có | destination/panorama/hotspot |
| target\_id | UUID / integer | Có | Đối tượng liên kết |
| format | string | Không | glb/gltf |
| polygon\_count | integer | Không | Số lượng polygon nếu có |
| status | enum | Có | draft/published/archived |
| created\_at | datetime | Có | Thời gian tạo |
| updated\_at | datetime | Có | Thời gian cập nhật |

### **14.3. Ghi chú thiết kế**

Trong MVP, model 3D có thể chỉ xuất hiện ở dạng:

Upload file  
Lưu metadata  
Hiển thị tên file trong admin  
Chưa cần viewer 3D

Giai đoạn sau có thể tích hợp Three.js hoặc CesiumJS để xem mô hình.

---

## **15\. Dữ liệu tuyến tham quan**

### **15.1. Khái niệm**

Tuyến tham quan là tập hợp nhiều địa điểm được sắp xếp theo một thứ tự nhất định. Tuyến tham quan giúp du khách khám phá một khu vực theo chủ đề hoặc thời gian.

Ví dụ:

Tuyến khám phá văn hóa Cần Thơ  
Tuyến du lịch sinh thái ven sông  
Tuyến trải nghiệm làng nghề và ẩm thực  
Tuyến tham quan nửa ngày  
Tuyến tham quan cuối tuần

### **15.2. Thuộc tính route**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã tuyến |
| region\_id | UUID / integer | Có | Khu vực |
| title | string | Có | Tên tuyến |
| description | text | Không | Mô tả |
| estimated\_duration | string | Không | Thời lượng dự kiến |
| distance\_km | decimal | Không | Tổng khoảng cách |
| theme | string | Không | Chủ đề tuyến |
| thumbnail\_id | UUID / integer | Không | Ảnh đại diện |
| status | enum | Có | draft/published/archived |
| created\_at | datetime | Có | Thời gian tạo |
| updated\_at | datetime | Có | Thời gian cập nhật |

### **15.3. Thuộc tính route destination**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã liên kết |
| route\_id | UUID / integer | Có | Tuyến tham quan |
| destination\_id | UUID / integer | Có | Địa điểm |
| display\_order | integer | Có | Thứ tự trong tuyến |
| estimated\_stay\_time | string | Không | Thời gian dừng |
| note | text | Không | Ghi chú |

### **15.4. Ghi chú MVP**

Tuyến tham quan có thể là chức năng mở rộng. Tuy nhiên, vẫn nên đặc tả dữ liệu vì nó liên quan trực tiếp đến du lịch thông minh.

---

## **16\. Dữ liệu đa ngôn ngữ**

### **16.1. Khái niệm**

Hệ thống MVP làm tiếng Việt trước, nhưng cần thiết kế dữ liệu sẵn sàng hỗ trợ nhiều ngôn ngữ trong tương lai.

Các nội dung nên hỗ trợ dịch:

Tên khu vực  
Mô tả khu vực  
Tên địa điểm  
Mô tả ngắn địa điểm  
Mô tả chi tiết địa điểm  
Tên tour  
Tên panorama  
Nội dung hotspot  
Transcript audio  
Tên tuyến tham quan

### **16.2. Cách tiếp cận đề xuất**

Có hai cách thiết kế phổ biến:

#### **Cách 1: Thêm cột ngôn ngữ trực tiếp**

Ví dụ:

name\_vi  
name\_en  
description\_vi  
description\_en

Ưu điểm:

Dễ làm  
Dễ query  
Phù hợp hệ thống nhỏ

Nhược điểm:

Khó mở rộng nhiều ngôn ngữ  
Database nhiều cột

#### **Cách 2: Dùng bảng translations**

Ví dụ:

entity\_type: destination  
entity\_id: 123  
field\_name: description  
language\_code: en  
translated\_value: ...

Ưu điểm:

Linh hoạt  
Dễ mở rộng nhiều ngôn ngữ  
Không phải sửa schema khi thêm ngôn ngữ

Nhược điểm:

Query phức tạp hơn  
Cần xử lý ở backend tốt hơn

### **16.3. Khuyến nghị**

Với dự án này, nên dùng cách 2 cho thiết kế dài hạn. Tuy nhiên, trong MVP có thể nhập tiếng Việt trước và chuẩn bị bảng translations để mở rộng.

### **16.4. Thuộc tính translation đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã bản dịch |
| entity\_type | string | Có | Loại đối tượng |
| entity\_id | UUID / integer | Có | ID đối tượng |
| field\_name | string | Có | Tên trường được dịch |
| language\_code | string | Có | vi/en/km... |
| translated\_value | text | Có | Nội dung dịch |
| created\_at | datetime | Có | Thời gian tạo |
| updated\_at | datetime | Có | Thời gian cập nhật |

---

## **17\. Dữ liệu người dùng và quản trị**

### **17.1. Khái niệm**

Trong MVP, du khách không cần đăng nhập. Chỉ quản trị viên cần tài khoản để quản lý dữ liệu.

Tương lai có thể mở rộng tài khoản du khách để lưu yêu thích, tạo lịch trình cá nhân, đánh giá hoặc nhận đề xuất.

### **17.2. Vai trò đề xuất**

| Vai trò | Mô tả |
| ----- | ----- |
| Super Admin | Quản lý toàn bộ hệ thống |
| Admin | Quản lý dữ liệu địa điểm, media, tour |
| Content Manager | Quản lý nội dung du lịch |
| Viewer | Chỉ xem dữ liệu trong dashboard |
| Tourist | Người dùng du khách, mở rộng sau MVP |

### **17.3. Thuộc tính user đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã người dùng |
| full\_name | string | Có | Họ tên |
| email | string | Có | Email đăng nhập |
| password\_hash | string | Có | Mật khẩu đã hash |
| role | enum/string | Có | Vai trò |
| avatar\_id | UUID / integer | Không | Ảnh đại diện |
| status | enum | Có | active/inactive/locked |
| last\_login\_at | datetime | Không | Lần đăng nhập gần nhất |
| created\_at | datetime | Có | Thời gian tạo |
| updated\_at | datetime | Có | Thời gian cập nhật |

---

## **18\. Dữ liệu phân tích hành vi**

### **18.1. Khái niệm**

Analytics event dùng để ghi nhận hành vi người dùng nhằm đánh giá mức độ quan tâm của du khách đối với địa điểm, tour hoặc hotspot.

### **18.2. Loại sự kiện đề xuất**

| Event | Mô tả |
| ----- | ----- |
| view\_destination | Xem chi tiết địa điểm |
| open\_virtual\_tour | Mở tour 360 |
| click\_hotspot | Bấm hotspot |
| play\_audio | Phát audio |
| play\_video | Phát video |
| search\_destination | Tìm kiếm địa điểm |
| filter\_category | Lọc theo danh mục |
| open\_map\_marker | Bấm marker trên bản đồ |

### **18.3. Thuộc tính analytics event đề xuất**

| Thuộc tính | Kiểu dữ liệu gợi ý | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | UUID / integer | Có | Mã sự kiện |
| event\_name | string | Có | Tên sự kiện |
| target\_type | string | Không | Loại đối tượng |
| target\_id | UUID / integer | Không | ID đối tượng |
| session\_id | string | Không | Phiên truy cập |
| user\_id | UUID / integer | Không | Người dùng nếu có đăng nhập |
| metadata | json | Không | Dữ liệu phụ |
| created\_at | datetime | Có | Thời gian xảy ra sự kiện |

### **18.4. Ghi chú MVP**

Analytics chưa bắt buộc trong MVP, nhưng có thể thiết kế bảng đơn giản để ghi nhận lượt xem cơ bản.

---

## **19\. Bộ dữ liệu mẫu cho MVP**

### **19.1. Quy mô dữ liệu mẫu**

MVP nên có khoảng 5–7 địa điểm mẫu. Dữ liệu có thể giả lập hoặc lấy cảm hứng từ Cần Thơ và khu vực Đồng bằng sông Cửu Long.

### **19.2. Nhóm địa điểm mẫu đề xuất**

| STT | Nhóm | Mô tả dữ liệu mẫu |
| ----- | ----- | ----- |
| 1 | Văn hóa \- lịch sử | Một nhà cổ hoặc di tích địa phương |
| 2 | Tâm linh | Một chùa hoặc công trình tín ngưỡng |
| 3 | Sinh thái | Một khu du lịch sinh thái ven sông |
| 4 | Nông nghiệp | Một vườn trái cây hoặc farmstay |
| 5 | Làng nghề | Một cơ sở sản xuất thủ công hoặc đặc sản |
| 6 | Ẩm thực | Một điểm ẩm thực địa phương |
| 7 | Lưu trú | Một homestay hoặc nhà vườn |

### **19.3. Dữ liệu tối thiểu cho mỗi địa điểm mẫu**

| Nhóm dữ liệu | Số lượng đề xuất |
| ----- | ----- |
| Ảnh đại diện | 1 |
| Ảnh thường | 2–5 |
| Ảnh 360 | 1–3 |
| Hotspot | 3–5 |
| Mô tả ngắn | 1 |
| Mô tả chi tiết | 1 |
| Audio hoặc transcript | 1 |
| Video | 0–1 |
| Tọa độ bản đồ | 1 cặp latitude/longitude |

### **19.4. Ví dụ dữ liệu mẫu dạng mô tả**

Tên địa điểm: Vườn trái cây ven sông Demo  
Danh mục: Nông nghiệp  
Khu vực: Cần Thơ Demo  
Mô tả ngắn: Điểm trải nghiệm du lịch nông nghiệp với không gian vườn cây, kênh rạch và hoạt động tham quan nhà vườn.  
Tọa độ: latitude, longitude  
Media:  
\- 1 ảnh đại diện  
\- 3 ảnh thường  
\- 2 ảnh 360  
\- 1 audio thuyết minh  
Tour 360:  
\- Panorama 1: Cổng vào vườn  
\- Panorama 2: Khu vườn trái cây  
Hotspot:  
\- Giới thiệu vườn  
\- Thông tin loại trái cây  
\- Chuyển sang khu tham quan tiếp theo

---

## **20\. Quy chuẩn đặt tên file**

### **20.1. Mục đích**

Quy chuẩn đặt tên file giúp hệ thống dễ quản lý media, tránh trùng lặp và dễ tìm kiếm khi lưu trữ trong object storage.

### **20.2. Cấu trúc tên file đề xuất**

{region\_slug}/{destination\_slug}/{media\_type}/{yyyyMMddHHmmss}\_{short\_name}.{extension}

Ví dụ:

can-tho-demo/vuon-trai-cay-demo/panorama/20260605103000\_cong-vao.jpg  
can-tho-demo/chua-demo/audio/20260605104500\_thuyet-minh-tong-quan.mp3  
can-tho-demo/lang-nghe-demo/video/20260605110000\_gioi-thieu.mp4

### **20.3. Quy tắc đặt tên**

\- Dùng chữ thường  
\- Không dùng dấu tiếng Việt trong tên file lưu trữ  
\- Thay khoảng trắng bằng dấu gạch ngang  
\- Không dùng ký tự đặc biệt  
\- Có timestamp để tránh trùng file  
\- Phân loại file theo media\_type

---

## **21\. Quy chuẩn nhập liệu**

### **21.1. Tên địa điểm**

Tên địa điểm cần rõ ràng, ngắn gọn, đúng chính tả.

Ví dụ:

Chợ nổi Demo  
Vườn trái cây ven sông Demo  
Nhà cổ Nam Bộ Demo

### **21.2. Slug**

Slug dùng cho URL, nên viết không dấu, chữ thường, phân tách bằng dấu gạch ngang.

Ví dụ:

cho-noi-demo  
vuon-trai-cay-ven-song-demo  
nha-co-nam-bo-demo

### **21.3. Mô tả ngắn**

Mô tả ngắn nên dài khoảng 1–3 câu, dùng để hiển thị trong card địa điểm hoặc popup bản đồ.

### **21.4. Mô tả chi tiết**

Mô tả chi tiết nên có cấu trúc rõ ràng, có thể gồm:

Giới thiệu chung  
Điểm nổi bật  
Hoạt động trải nghiệm  
Giá trị văn hóa/sinh thái/nông nghiệp  
Gợi ý tham quan

### **21.5. Tọa độ**

Tọa độ cần dùng hệ latitude/longitude. Admin có thể click trên bản đồ để lấy tọa độ.

Ví dụ:

latitude: 10.0452  
longitude: 105.7469

### **21.6. Trạng thái dữ liệu**

Các dữ liệu chính nên có trạng thái:

| Trạng thái | Ý nghĩa |
| ----- | ----- |
| draft | Đang soạn, chưa hiển thị |
| published | Đã xuất bản cho du khách |
| archived | Đã lưu trữ, không còn hiển thị |

---

## **22\. Quy chuẩn chất lượng dữ liệu**

### **22.1. Địa điểm**

Mỗi địa điểm trước khi xuất bản cần đảm bảo:

Có tên rõ ràng  
Có danh mục  
Có tọa độ  
Có mô tả ngắn  
Có ảnh đại diện  
Có trạng thái published

### **22.2. Ảnh 360**

Ảnh 360 cần đảm bảo:

Không bị vỡ quá nặng  
Không bị lệch quá mức  
Có kích thước phù hợp để tải trên web  
Được nén trước khi upload nếu cần  
Có thumbnail nếu có thể

### **22.3. Hotspot**

Hotspot cần đảm bảo:

Đặt đúng vị trí trong panorama  
Tên hotspot dễ hiểu  
Không đặt quá nhiều hotspot gây rối mắt  
Hotspot navigation phải có panorama đích  
Hotspot media phải có file liên kết hợp lệ

### **22.4. Audio**

Audio cần đảm bảo:

Âm lượng nghe rõ  
Không quá dài trong MVP  
Có transcript nếu có thể  
Đúng với nội dung địa điểm hoặc hotspot

### **22.5. Video**

Video cần đảm bảo:

Định dạng phù hợp với web  
Dung lượng không quá lớn  
Có thumbnail  
Có tiêu đề và mô tả rõ ràng

---

## **23\. Mối liên hệ giữa dữ liệu và chức năng hệ thống**

| Chức năng | Dữ liệu cần dùng |
| ----- | ----- |
| Xem bản đồ | destination, latitude, longitude, category |
| Click marker | destination, cover\_image, short\_description |
| Xem chi tiết địa điểm | destination, media, audio, video, virtual\_tour |
| Mở tour 360 | virtual\_tour, panorama, media\_file |
| Bấm hotspot | hotspot, media\_file, target\_panorama |
| Admin thêm địa điểm | region, category, destination |
| Admin chọn điểm trên bản đồ | latitude, longitude |
| Admin upload media | media\_file |
| Admin tạo tour | virtual\_tour, panorama |
| Admin tạo hotspot | panorama, hotspot |
| Đa ngôn ngữ | translation |
| Gợi ý tuyến tham quan | route, route\_destination |
| Thống kê lượt xem | analytics\_event |

---

## **24\. Dữ liệu bắt buộc và dữ liệu mở rộng**

### **24.1. Dữ liệu bắt buộc cho MVP**

Region  
Category  
Destination  
Media File  
Virtual Tour  
Panorama  
Hotspot  
Admin User

### **24.2. Dữ liệu nên có trong MVP**

Audio Guide  
Video  
Basic Translation Structure  
Route  
Basic Analytics Event

### **24.3. Dữ liệu mở rộng sau MVP**

3D Model Viewer  
Tourist Account  
Favorite Destination  
Review/Rating  
Booking Request  
AI Chatbot Knowledge Base  
AR Content  
Advanced GIS Layer

---

## **25\. Kết luận**

Dữ liệu là nền tảng quan trọng nhất của **Smart Tourism 360 Platform**. Hệ thống không chỉ cần lưu thông tin địa điểm đơn giản, mà còn cần quản lý nhiều loại dữ liệu đặc thù như tọa độ bản đồ, ảnh 360, tour ảo, panorama, hotspot, audio thuyết minh, video, mô hình 3D và nội dung đa ngôn ngữ.

Trong giai đoạn MVP, hệ thống nên tập trung vào dữ liệu cốt lõi gồm khu vực, danh mục, địa điểm, media, tour 360, panorama và hotspot. Các nhóm dữ liệu nâng cao như mô hình 3D, tuyến tham quan, analytics và đa ngôn ngữ nên được thiết kế sẵn để có thể mở rộng sau này.

Với đặc tả dữ liệu này, nhóm phát triển có thể tiếp tục xây dựng các tài liệu kỹ thuật tiếp theo như **System Architecture Document**, **Database Design Document**, **API Specification Document** và **UI/UX Design Document** một cách rõ ràng, nhất quán và có khả năng triển khai thực tế.

