# **PROJECT CONCEPT DOCUMENT**

# **Smart Tourism 360 Platform**

## **Nền tảng du lịch số tích hợp bản đồ tương tác và tham quan ảo 360 độ**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | Project Concept Document |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v1.0 |
| Mục đích | Mô tả tổng quan ý tưởng, mục tiêu, phạm vi và định hướng phát triển của dự án |
| Đối tượng đọc | Giảng viên, nhóm phát triển, nhà quản lý, đơn vị du lịch, nhà đầu tư, đối tác triển khai |
| Phạm vi tài liệu | Tập trung vào ý tưởng, bối cảnh, giá trị, người dùng, phạm vi MVP và định hướng công nghệ tổng quan |

---

## **2\. Giới thiệu dự án**

**Smart Tourism 360 Platform** là một nền tảng du lịch số được xây dựng nhằm hỗ trợ quảng bá, quản lý và số hóa trải nghiệm tham quan các điểm đến địa phương thông qua **bản đồ tương tác**, **nội dung đa phương tiện** và **công nghệ tham quan ảo 360 độ**.

Hệ thống cho phép quản trị viên tạo và quản lý các điểm tham quan trực tiếp trên bản đồ. Mỗi địa điểm có thể được gắn các dữ liệu số như thông tin mô tả, hình ảnh, ảnh 360, video, âm thanh thuyết minh, mô hình 3D và các điểm tương tác trong không gian tham quan ảo. Du khách khi truy cập vào hệ thống có thể xem bản đồ khu vực, lựa chọn địa điểm, tìm hiểu thông tin và trải nghiệm tham quan 360 độ trước khi đến thực tế.

Dự án được thiết kế theo hướng tổng quát, có thể triển khai cho nhiều địa phương khác nhau như một tỉnh, thành phố, huyện, xã, khu du lịch, cụm điểm du lịch hoặc tuyến du lịch cụ thể. Trong giai đoạn MVP, hệ thống sẽ sử dụng dữ liệu mẫu gồm khoảng 5–7 địa điểm, ưu tiên lấy cảm hứng từ khu vực Cần Thơ và Đồng bằng sông Cửu Long. Sau này, hệ thống có thể thay thế dữ liệu mẫu bằng dữ liệu khảo sát thực tế và mở rộng sang nhiều khu vực khác.

---

## **3\. Bối cảnh hình thành dự án**

Trong những năm gần đây, du lịch địa phương ngày càng có nhu cầu ứng dụng công nghệ số để quảng bá hình ảnh, nâng cao trải nghiệm du khách và hỗ trợ công tác quản lý điểm đến. Nhiều địa phương có tài nguyên du lịch phong phú như di tích lịch sử, chùa, đình, làng nghề, vườn trái cây, khu sinh thái, homestay, điểm ẩm thực và sản phẩm đặc trưng. Tuy nhiên, việc giới thiệu các điểm đến này vẫn còn phụ thuộc nhiều vào hình thức truyền thống như bài viết, hình ảnh tĩnh, video ngắn hoặc thông tin rời rạc trên nhiều nền tảng khác nhau.

Đối với du khách, việc tìm hiểu một địa điểm trước khi đến thực tế thường gặp nhiều khó khăn. Thông tin có thể không đầy đủ, thiếu trực quan, không có bản đồ tổng quan, không có trải nghiệm tham quan thử và không thể hình dung rõ không gian thực tế của điểm đến. Đối với cơ quan quản lý hoặc đơn vị vận hành du lịch, việc cập nhật, tổ chức và quảng bá dữ liệu điểm đến cũng chưa thật sự đồng bộ.

Bên cạnh đó, công nghệ ảnh 360, bản đồ số, WebGIS, lưu trữ đám mây, mô hình 3D và các nền tảng web hiện đại đang mở ra cơ hội xây dựng những hệ thống du lịch số có tính tương tác cao. Thay vì chỉ xem thông tin một chiều, du khách có thể chủ động khám phá không gian, lựa chọn điểm đến trên bản đồ, nghe thuyết minh, xem video và tham quan ảo trước khi quyết định đi thực tế.

Từ nhu cầu đó, dự án **Smart Tourism 360 Platform** được đề xuất như một giải pháp giúp số hóa, quản lý và quảng bá các điểm du lịch địa phương một cách trực quan, hiện đại và dễ mở rộng.

---

## **4\. Vấn đề cần giải quyết**

Dự án tập trung giải quyết các nhóm vấn đề chính sau:

### **4.1. Thông tin du lịch địa phương còn phân tán**

Thông tin về các điểm đến thường nằm rải rác trên nhiều website, mạng xã hội, bài viết hoặc video riêng lẻ. Người dùng khó có được một cái nhìn tổng quan về toàn bộ khu vực du lịch, các điểm nổi bật, vị trí địa lý, khoảng cách và mối liên hệ giữa các địa điểm.

### **4.2. Trải nghiệm giới thiệu điểm đến chưa đủ trực quan**

Ảnh tĩnh và mô tả văn bản chưa đủ để du khách hình dung không gian thực tế. Với những địa điểm như chùa, di tích, vườn trái cây, làng nghề, homestay hoặc khu sinh thái, trải nghiệm không gian đóng vai trò quan trọng trong việc tạo sự hứng thú cho du khách.

### **4.3. Thiếu nền tảng quản trị nội dung du lịch số**

Nhiều hệ thống quảng bá du lịch hiện nay chỉ là website giới thiệu cố định. Khi cần thêm địa điểm mới, cập nhật hình ảnh, thay đổi mô tả, thêm audio hoặc video, đơn vị quản lý thường phụ thuộc vào đội kỹ thuật. Điều này gây khó khăn trong việc duy trì dữ liệu lâu dài.

### **4.4. Chưa khai thác tốt dữ liệu bản đồ và vị trí**

Bản đồ là thành phần rất quan trọng trong du lịch, nhưng nhiều hệ thống chỉ hiển thị danh sách địa điểm mà chưa tích hợp trải nghiệm bản đồ tương tác. Du khách cần biết địa điểm nằm ở đâu, gần những điểm nào, thuộc nhóm du lịch nào và có thể kết hợp thành tuyến tham quan ra sao.

### **4.5. Chưa có nền tảng linh hoạt để mở rộng thành WebGIS/3D/AR**

Nhiều giải pháp tham quan ảo chỉ dừng lại ở một tour 360 đơn lẻ. Dự án này hướng tới thiết kế một nền tảng mở, có thể bắt đầu từ MVP nhẹ với bản đồ và ảnh 360, sau đó mở rộng sang WebGIS 3D, mô hình 3D, AR check-in, AI hướng dẫn viên và gợi ý lịch trình thông minh.

---

## **5\. Mục tiêu dự án**

### **5.1. Mục tiêu tổng quát**

Xây dựng một nền tảng du lịch số cho phép quản lý, số hóa và quảng bá các điểm tham quan địa phương thông qua bản đồ tương tác, dữ liệu đa phương tiện và trải nghiệm tham quan ảo 360 độ.

### **5.2. Mục tiêu cụ thể**

Dự án hướng đến các mục tiêu cụ thể sau:

1. Xây dựng một website cho phép du khách khám phá các điểm đến trên bản đồ tương tác.  
2. Cho phép người quản trị tạo địa điểm trực tiếp trên bản đồ bằng thao tác chọn vị trí.  
3. Cho phép quản trị viên nhập thông tin mô tả, danh mục, hình ảnh, video, audio, ảnh 360 và dữ liệu liên quan cho từng địa điểm.  
4. Cung cấp tính năng tham quan ảo 360 độ cho từng địa điểm.  
5. Hỗ trợ hotspot trong không gian 360 để hiển thị thông tin, chuyển cảnh, phát audio hoặc liên kết đến nội dung khác.  
6. Thiết kế hệ thống dữ liệu có khả năng hỗ trợ đa ngôn ngữ trong tương lai.  
7. Thiết kế kiến trúc mở để có thể mở rộng sang mô hình 3D, WebGIS 3D, AI chatbot, lịch trình du lịch và các tính năng nâng cao.  
8. Xây dựng MVP với khoảng 5–7 địa điểm mẫu để chứng minh tính khả thi của hệ thống.  
9. Tạo nền tảng có thể phát triển thành sản phẩm thực tế phục vụ địa phương, cơ quan quản lý du lịch hoặc doanh nghiệp du lịch.

---

## **6\. Đối tượng sử dụng**

Hệ thống hướng đến nhiều nhóm người dùng khác nhau. Trong giai đoạn MVP, dự án ưu tiên hai nhóm chính là **du khách** và **quản trị viên/cơ quan quản lý**.

### **6.1. Du khách**

Du khách là nhóm người dùng chính của hệ thống. Họ có thể truy cập website để xem bản đồ, tìm kiếm địa điểm, đọc thông tin, xem hình ảnh, mở tour 360 và khám phá trước các điểm đến.

Nhu cầu chính của du khách gồm:

* Tìm kiếm điểm tham quan trong một khu vực.  
* Xem vị trí địa điểm trên bản đồ.  
* Xem thông tin mô tả, hình ảnh, video và nội dung thuyết minh.  
* Tham quan ảo 360 độ.  
* Khám phá không gian trước khi đến thực tế.  
* Xem các nhóm địa điểm như văn hóa, sinh thái, nông nghiệp, làng nghề, ẩm thực hoặc lưu trú.

Trong MVP, du khách không bắt buộc đăng nhập. Hệ thống được thiết kế mở để sau này có thể bổ sung tài khoản du khách, lưu địa điểm yêu thích, tạo lịch trình cá nhân và đánh giá điểm đến.

### **6.2. Quản trị viên hệ thống**

Quản trị viên là người có quyền quản lý nội dung và dữ liệu trong hệ thống. Đây có thể là đội vận hành dự án, cơ quan quản lý địa phương, ban quản lý khu du lịch hoặc đơn vị phụ trách truyền thông du lịch.

Nhiệm vụ chính của quản trị viên gồm:

* Đăng nhập vào hệ thống quản trị.  
* Tạo mới, chỉnh sửa và xóa địa điểm.  
* Chọn vị trí địa điểm trực tiếp trên bản đồ.  
* Quản lý danh mục điểm đến.  
* Upload hình ảnh, ảnh 360, video, audio và mô hình 3D.  
* Tạo tour 360 cho từng địa điểm.  
* Tạo hotspot trong ảnh 360\.  
* Quản lý trạng thái hiển thị của địa điểm.  
* Chuẩn bị dữ liệu để phục vụ du khách.

### **6.3. Cơ quan quản lý du lịch hoặc văn hóa**

Nhóm này có thể sử dụng hệ thống như một công cụ hỗ trợ quảng bá và quản lý dữ liệu điểm đến của địa phương.

Nhu cầu chính gồm:

* Có một nền tảng tập trung để giới thiệu điểm đến.  
* Quản lý dữ liệu du lịch có cấu trúc.  
* Cập nhật thông tin điểm đến nhanh chóng.  
* Quảng bá hình ảnh địa phương bằng trải nghiệm trực quan.  
* Hỗ trợ bảo tồn và truyền thông giá trị văn hóa, lịch sử, sinh thái và nông nghiệp.

### **6.4. Cơ sở du lịch, hộ dân, homestay, làng nghề**

Trong giai đoạn mở rộng, hệ thống có thể hỗ trợ các cơ sở du lịch địa phương giới thiệu sản phẩm và dịch vụ của mình. Ví dụ: homestay, farmstay, vườn trái cây, cơ sở làng nghề, quán ăn địa phương hoặc điểm trải nghiệm cộng đồng.

Các chức năng liên quan đến nhóm này có thể được phát triển sau MVP.

---

## **7\. Phạm vi dự án**

### **7.1. Phạm vi tổng quát**

Dự án được thiết kế như một nền tảng tổng quát có thể triển khai cho nhiều khu vực khác nhau. Hệ thống không bị ràng buộc cố định vào một tỉnh hoặc một địa điểm cụ thể.

Các phạm vi triển khai có thể bao gồm:

* Một tỉnh hoặc thành phố.  
* Một huyện, quận, xã hoặc phường.  
* Một khu du lịch cụ thể.  
* Một cụm điểm du lịch.  
* Một tuyến du lịch.  
* Một vùng văn hóa, sinh thái hoặc nông nghiệp.

Trong trường hợp cần minh họa dữ liệu mẫu, dự án ưu tiên lấy cảm hứng từ **Cần Thơ** và khu vực **Đồng bằng sông Cửu Long**.

### **7.2. Phạm vi MVP**

MVP là phiên bản tối thiểu có thể hoạt động được, nhằm chứng minh ý tưởng và tạo nền tảng cho các giai đoạn phát triển tiếp theo.

Phạm vi MVP gồm:

1. Website cho du khách xem thông tin và khám phá địa điểm.  
2. Bản đồ tương tác hiển thị các địa điểm du lịch.  
3. Chức năng xem danh sách và chi tiết địa điểm.  
4. Chức năng tham quan ảo 360 độ cho từng địa điểm.  
5. Hotspot trong ảnh 360\.  
6. Audio thuyết minh hoặc mô tả nội dung cho địa điểm/hotspot.  
7. Trang đăng nhập quản trị.  
8. Admin dashboard để quản lý địa điểm.  
9. Chức năng chọn vị trí địa điểm trực tiếp trên bản đồ.  
10. Chức năng upload và quản lý media cơ bản.  
11. Dữ liệu mẫu cho khoảng 5–7 địa điểm.  
12. Thiết kế dữ liệu sẵn sàng hỗ trợ đa ngôn ngữ.  
13. Thiết kế dữ liệu sẵn sàng hỗ trợ mô hình 3D, nhưng chưa bắt buộc tích hợp viewer 3D trong MVP.

### **7.3. Ngoài phạm vi MVP**

Các chức năng sau chưa bắt buộc trong MVP, nhưng có thể phát triển ở giai đoạn sau:

* Tài khoản du khách.  
* Lưu địa điểm yêu thích.  
* Tạo lịch trình cá nhân.  
* Đặt lịch tham quan.  
* Đánh giá và bình luận.  
* AI chatbot hướng dẫn viên du lịch.  
* Gợi ý lịch trình thông minh.  
* AR check-in tại địa điểm thật.  
* WebGIS 3D bằng CesiumJS hoặc ArcGIS Maps SDK.  
* Tích hợp mô hình 3D nâng cao.  
* Dashboard phân tích hành vi du khách nâng cao.  
* Ứng dụng mobile riêng.  
* Hệ thống nhiều tenant cho nhiều tỉnh/thành khác nhau.

---

## **8\. Dữ liệu mẫu trong giai đoạn đầu**

Do dự án hiện chưa có dữ liệu khảo sát thực tế, giai đoạn đầu sẽ sử dụng dữ liệu mẫu để phục vụ thiết kế, phát triển và demo hệ thống.

Dữ liệu mẫu có thể đến từ ba nguồn:

1. Dữ liệu giả lập do nhóm phát triển tự tạo.  
2. Dữ liệu công khai trên web nếu có thể sử dụng phù hợp.  
3. Dữ liệu lấy cảm hứng từ các địa điểm du lịch tại Cần Thơ hoặc Đồng bằng sông Cửu Long.

Bộ dữ liệu mẫu trong MVP nên có khoảng 5–7 địa điểm, đại diện cho nhiều nhóm du lịch khác nhau, ví dụ:

| Nhóm địa điểm | Ví dụ minh họa |
| ----- | ----- |
| Văn hóa \- lịch sử | Đình, nhà cổ, di tích |
| Tâm linh | Chùa, miếu, công trình tín ngưỡng |
| Sinh thái | Khu sinh thái, vườn xanh, điểm ven sông |
| Nông nghiệp | Vườn trái cây, farmstay, khu trải nghiệm nông nghiệp |
| Làng nghề | Cơ sở làm bánh, thủ công, sản phẩm địa phương |
| Ẩm thực | Quán ăn đặc sản, khu ẩm thực địa phương |
| Lưu trú | Homestay, farmstay, nhà vườn |

Với mỗi địa điểm mẫu, hệ thống cần có tối thiểu:

* Tên địa điểm.  
* Danh mục.  
* Mô tả ngắn.  
* Mô tả chi tiết.  
* Địa chỉ hoặc khu vực.  
* Tọa độ trên bản đồ.  
* Ảnh đại diện.  
* Một số hình ảnh thường.  
* Tối thiểu một ảnh 360 hoặc dữ liệu panorama mẫu.  
* Một số hotspot mẫu.  
* Audio thuyết minh hoặc nội dung thuyết minh dạng văn bản.  
* Trạng thái hiển thị.

---

## **9\. Mô tả chức năng tổng quan**

### **9.1. Nhóm chức năng cho du khách**

#### **9.1.1. Xem trang chủ**

Trang chủ giới thiệu tổng quan về nền tảng, khu vực du lịch, các điểm đến nổi bật và lời kêu gọi khám phá bản đồ.

#### **9.1.2. Xem bản đồ khám phá**

Du khách có thể xem các điểm tham quan được đánh dấu trên bản đồ. Mỗi điểm có marker, tên, danh mục và thông tin tóm tắt.

#### **9.1.3. Tìm kiếm và lọc địa điểm**

Du khách có thể tìm kiếm địa điểm theo tên hoặc lọc theo danh mục như văn hóa, sinh thái, nông nghiệp, ẩm thực, làng nghề, homestay.

#### **9.1.4. Xem chi tiết địa điểm**

Trang chi tiết cung cấp thông tin đầy đủ hơn về địa điểm, bao gồm mô tả, hình ảnh, video, audio, vị trí bản đồ và nút mở tour 360\.

#### **9.1.5. Tham quan ảo 360 độ**

Du khách có thể mở trình xem 360 để quan sát không gian địa điểm. Họ có thể xoay góc nhìn, phóng to, chuyển giữa các panorama và tương tác với hotspot.

#### **9.1.6. Tương tác với hotspot**

Hotspot là các điểm tương tác được đặt trong không gian 360\. Khi bấm vào hotspot, du khách có thể xem thông tin, nghe thuyết minh, mở hình ảnh/video hoặc chuyển sang vị trí khác trong tour.

### **9.2. Nhóm chức năng cho quản trị viên**

#### **9.2.1. Đăng nhập quản trị**

Quản trị viên cần đăng nhập để truy cập dashboard và thực hiện các thao tác quản lý dữ liệu.

#### **9.2.2. Quản lý danh mục địa điểm**

Admin có thể tạo, sửa, xóa hoặc ẩn/hiện các danh mục như văn hóa, sinh thái, nông nghiệp, làng nghề, ẩm thực, lưu trú.

#### **9.2.3. Quản lý địa điểm**

Admin có thể thêm mới địa điểm, cập nhật thông tin, chọn danh mục, nhập mô tả, gắn tọa độ và thiết lập trạng thái hiển thị.

#### **9.2.4. Chọn địa điểm trực tiếp trên bản đồ**

Khi tạo hoặc chỉnh sửa địa điểm, admin có thể click trực tiếp lên bản đồ để lấy tọa độ latitude/longitude. Đây là chức năng quan trọng giúp việc nhập liệu trực quan và thuận tiện hơn.

#### **9.2.5. Quản lý media**

Admin có thể upload và quản lý hình ảnh, ảnh 360, video, audio và file mô hình 3D. Trong MVP, hệ thống ưu tiên hình ảnh, ảnh 360, audio và video. Mô hình 3D được thiết kế dữ liệu để hỗ trợ mở rộng.

#### **9.2.6. Quản lý tour 360**

Admin có thể tạo tour 360 cho một địa điểm, thêm các panorama, thiết lập thứ tự hiển thị và liên kết giữa các điểm nhìn.

#### **9.2.7. Quản lý hotspot**

Admin có thể tạo hotspot trong từng panorama, thiết lập loại hotspot, vị trí, tiêu đề, nội dung và hành động tương ứng.

---

## **10\. Giá trị mang lại**

### **10.1. Giá trị đối với du khách**

Hệ thống giúp du khách có trải nghiệm trực quan hơn khi tìm hiểu điểm đến. Thay vì chỉ đọc mô tả hoặc xem ảnh tĩnh, du khách có thể tương tác với bản đồ, tham quan ảo không gian 360 độ, nghe thuyết minh và hình dung rõ hơn về địa điểm trước khi đến thực tế.

Lợi ích chính gồm:

* Dễ dàng khám phá các điểm đến trong một khu vực.  
* Có trải nghiệm trực quan, sinh động.  
* Tiết kiệm thời gian tìm kiếm thông tin.  
* Có thể xem trước không gian thực tế.  
* Tăng hứng thú và động lực đi du lịch thực tế.

### **10.2. Giá trị đối với cơ quan quản lý**

Hệ thống cung cấp một công cụ quản lý và quảng bá điểm đến tập trung. Các đơn vị quản lý có thể cập nhật dữ liệu, thêm địa điểm mới, quản lý media và tổ chức thông tin du lịch một cách có hệ thống.

Lợi ích chính gồm:

* Tập trung hóa dữ liệu điểm đến.  
* Hỗ trợ quảng bá hình ảnh địa phương.  
* Nâng cao chất lượng truyền thông du lịch.  
* Tạo nền tảng cho chuyển đổi số du lịch.  
* Dễ dàng mở rộng sang nhiều khu vực và loại hình du lịch.

### **10.3. Giá trị đối với cộng đồng địa phương**

Dự án có thể góp phần hỗ trợ cộng đồng địa phương, hộ dân, làng nghề, homestay và các cơ sở du lịch nhỏ bằng cách tạo thêm kênh giới thiệu sản phẩm, dịch vụ và câu chuyện địa phương.

Lợi ích tiềm năng gồm:

* Tăng khả năng tiếp cận du khách.  
* Giới thiệu sản phẩm địa phương.  
* Quảng bá làng nghề, nông sản, văn hóa bản địa.  
* Hỗ trợ phát triển sinh kế du lịch cộng đồng.  
* Góp phần bảo tồn và truyền thông giá trị văn hóa địa phương.

### **10.4. Giá trị đối với nhóm phát triển và học thuật**

Với góc độ dự án học thuật hoặc đồ án, hệ thống có giá trị cao vì kết hợp nhiều mảng công nghệ:

* Phát triển web full-stack.  
* Bản đồ số và dữ liệu tọa độ.  
* Quản lý media.  
* Ảnh 360 và trải nghiệm tương tác.  
* Thiết kế database có khả năng mở rộng.  
* Quản trị nội dung.  
* Triển khai bằng Docker.  
* Tiền đề cho WebGIS 3D, AR/VR và AI trong tương lai.

---

## **11\. Định hướng công nghệ tổng quan**

Dự án được định hướng sử dụng các công nghệ hiện đại, phổ biến và phù hợp với khả năng phát triển MVP.

### **11.1. Frontend**

Frontend dự kiến sử dụng:

* Vue.js 3\.  
* Pinia cho state management.  
* TailwindCSS hoặc Bootstrap cho giao diện.  
* Leaflet hoặc Mapbox GL JS cho bản đồ.  
* Photo Sphere Viewer hoặc Pannellum cho trình xem ảnh 360\.

Frontend chịu trách nhiệm hiển thị giao diện du khách, bản đồ, danh sách địa điểm, chi tiết địa điểm, trình xem 360 và admin dashboard.

### **11.2. Backend**

Backend dự kiến sử dụng:

* ASP.NET Core Web API.  
* Entity Framework Core.  
* JWT Authentication.  
* Swagger/OpenAPI để tài liệu hóa API.

Backend chịu trách nhiệm xử lý nghiệp vụ, quản lý dữ liệu, xác thực người dùng, phân quyền admin, quản lý media metadata, quản lý tour 360 và cung cấp API cho frontend.

### **11.3. Database**

Database dự kiến sử dụng:

* PostgreSQL.  
* PostGIS để hỗ trợ dữ liệu không gian và tọa độ địa lý.

Database lưu trữ dữ liệu người dùng, địa điểm, danh mục, tọa độ, media, tour 360, panorama, hotspot, nội dung thuyết minh, dữ liệu đa ngôn ngữ và các thông tin liên quan.

### **11.4. File Storage**

Hệ thống cần lưu trữ nhiều loại file như ảnh, ảnh 360, video, audio và mô hình 3D. Do đó, dự án có thể sử dụng:

* MinIO cho môi trường local hoặc self-hosted.  
* Cloudflare R2, AWS S3 hoặc dịch vụ object storage tương đương cho môi trường production.

### **11.5. Deployment**

Dự án định hướng triển khai bằng:

* Docker Compose cho môi trường phát triển và demo.  
* Có thể mở rộng sang VPS, cloud server hoặc container platform trong tương lai.

---

## **12\. Phạm vi mở rộng trong tương lai**

Sau khi MVP hoàn thành, hệ thống có thể được mở rộng theo nhiều hướng:

### **12.1. Mở rộng dữ liệu thực tế**

Thay thế dữ liệu mẫu bằng dữ liệu khảo sát thực địa, bổ sung nhiều điểm du lịch hơn, mở rộng sang nhiều khu vực và nhiều loại hình du lịch.

### **12.2. Đa ngôn ngữ**

MVP làm tiếng Việt trước, nhưng hệ thống được thiết kế để hỗ trợ đa ngôn ngữ. Giai đoạn sau có thể bổ sung tiếng Anh, tiếng Khmer hoặc các ngôn ngữ khác tùy địa phương.

### **12.3. WebGIS 3D**

Hệ thống có thể tích hợp CesiumJS hoặc ArcGIS Maps SDK để hiển thị bản đồ 3D, địa hình, mô hình đô thị, lớp dữ liệu không gian hoặc các đối tượng 3D.

### **12.4. Mô hình 3D**

Bổ sung trình xem mô hình 3D bằng Three.js hoặc các công nghệ liên quan. Mô hình 3D có thể được gắn với địa điểm hoặc hotspot trong tour 360\.

### **12.5. AI hướng dẫn viên**

Tích hợp chatbot AI để trả lời câu hỏi của du khách, giới thiệu địa điểm, đề xuất tuyến tham quan và giải thích nội dung văn hóa/lịch sử.

### **12.6. Gợi ý lịch trình**

Hệ thống có thể đề xuất tuyến tham quan dựa trên sở thích, thời gian, vị trí hiện tại hoặc nhóm địa điểm mà du khách quan tâm.

### **12.7. AR check-in**

Trong tương lai, dự án có thể tích hợp AR để du khách check-in tại địa điểm thực tế, xem nội dung mở rộng hoặc tương tác với mô hình số tại hiện trường.

### **12.8. Dashboard phân tích**

Hệ thống có thể bổ sung dashboard thống kê lượt xem, địa điểm được quan tâm nhiều, hành vi người dùng, lượt tương tác hotspot và hiệu quả quảng bá du lịch.

---

## **13\. Kết quả đầu ra mong muốn**

Sau giai đoạn MVP, dự án cần đạt được các kết quả sau:

1. Một website hoạt động được cho du khách.  
2. Một bản đồ tương tác hiển thị các địa điểm mẫu.  
3. Khoảng 5–7 địa điểm mẫu có thông tin cơ bản.  
4. Ít nhất một số địa điểm có tour 360\.  
5. Hotspot trong không gian 360\.  
6. Trang chi tiết địa điểm có hình ảnh, mô tả, video/audio nếu có.  
7. Admin dashboard cho phép quản lý địa điểm.  
8. Chức năng chọn tọa độ địa điểm trực tiếp trên bản đồ.  
9. Chức năng upload và quản lý media.  
10. Backend API có tài liệu Swagger/OpenAPI.  
11. Database PostgreSQL/PostGIS được thiết kế rõ ràng.  
12. Hệ thống chạy được bằng Docker Compose.  
13. Tài liệu kỹ thuật đủ để tiếp tục phát triển các giai đoạn sau.

---

## **14\. Tiêu chí thành công của MVP**

MVP được xem là thành công nếu đạt các tiêu chí sau:

| Nhóm tiêu chí | Mô tả |
| ----- | ----- |
| Trải nghiệm du khách | Du khách có thể xem bản đồ, chọn địa điểm và mở tour 360 |
| Quản trị dữ liệu | Admin có thể thêm/sửa/xóa địa điểm và chọn vị trí trên bản đồ |
| Quản lý media | Admin có thể upload và gắn media cho địa điểm |
| Tour 360 | Hệ thống hiển thị được ảnh 360 và hotspot |
| Kiến trúc | Hệ thống có frontend, backend, database và storage rõ ràng |
| Mở rộng | Database/API có thiết kế sẵn cho đa ngôn ngữ và mô hình 3D |
| Triển khai | Dự án có thể chạy local hoặc demo bằng Docker Compose |
| Tài liệu | Có đủ tài liệu để tiếp tục phát triển system architecture, database, API và UI/UX |

---

## **15\. Rủi ro và hướng xử lý**

### **15.1. Thiếu dữ liệu thực tế**

Do giai đoạn đầu chưa có dữ liệu khảo sát, dự án có thể gặp khó khăn trong việc tạo demo chân thực.

**Hướng xử lý:**  
Sử dụng dữ liệu mẫu, dữ liệu giả lập hoặc dữ liệu công khai phù hợp. Thiết kế hệ thống theo hướng dễ thay thế dữ liệu mẫu bằng dữ liệu thực tế sau này.

### **15.2. Ảnh 360 có dung lượng lớn**

Ảnh 360 thường có dung lượng cao, có thể làm chậm tốc độ tải trang.

**Hướng xử lý:**  
Tối ưu ảnh, sử dụng lazy loading, nén file, tạo thumbnail và lưu trữ bằng object storage.

### **15.3. Phạm vi dự án dễ bị mở rộng quá mức**

Dự án có nhiều hướng phát triển như WebGIS 3D, AR, AI, lịch trình thông minh và mô hình 3D, dễ khiến MVP bị quá tải.

**Hướng xử lý:**  
Giữ MVP tập trung vào bản đồ, địa điểm, tour 360, hotspot và admin quản lý nội dung. Các tính năng nâng cao đưa vào roadmap sau.

### **15.4. Quản lý media phức tạp**

Hệ thống cần quản lý nhiều loại file khác nhau như ảnh, video, audio, ảnh 360 và mô hình 3D.

**Hướng xử lý:**  
Thiết kế bảng media\_files tổng quát, phân loại file bằng media type và lưu file trên object storage thay vì lưu trực tiếp trong database.

### **15.5. Trải nghiệm 360 trên thiết bị di động**

Tour 360 cần hoạt động tốt trên điện thoại, vì du khách thường dùng mobile.

**Hướng xử lý:**  
Ưu tiên responsive design, kiểm thử trên mobile, tối ưu ảnh và sử dụng thư viện viewer 360 nhẹ.

---

## **16\. Kết luận**

**Smart Tourism 360 Platform** là một dự án có tính ứng dụng thực tế cao, kết hợp giữa du lịch thông minh, bản đồ tương tác, quản lý nội dung số và trải nghiệm tham quan ảo 360 độ. Dự án không chỉ giúp du khách khám phá điểm đến một cách trực quan hơn, mà còn hỗ trợ cơ quan quản lý và cộng đồng địa phương trong việc quảng bá, bảo tồn và phát triển giá trị du lịch.

Trong giai đoạn đầu, hệ thống tập trung xây dựng MVP với bản đồ tương tác, quản lý địa điểm, tour 360, hotspot và admin dashboard. Dữ liệu có thể được giả lập hoặc lấy cảm hứng từ Cần Thơ và khu vực Đồng bằng sông Cửu Long. Sau khi MVP hoàn thiện, dự án có thể mở rộng sang dữ liệu thực tế, đa ngôn ngữ, WebGIS 3D, mô hình 3D, AI hướng dẫn viên, gợi ý lịch trình và các dịch vụ du lịch thông minh khác.

Với định hướng này, Smart Tourism 360 Platform có thể phát triển từ một dự án học thuật hoặc đồ án công nghệ thành một nền tảng sản phẩm thực tế phục vụ chuyển đổi số du lịch địa phương.

