# **AI TOUR GUIDE SPECIFICATION**

# **Smart Tourism 360 Platform v2**

## **Đặc tả tính năng trợ lý AI hướng dẫn du lịch dựa trên dữ liệu nội bộ**

---

## **1\. Thông tin tài liệu**

| Mục | Nội dung |
| ----- | ----- |
| Tên tài liệu | AI Tour Guide Specification |
| Tên dự án | Smart Tourism 360 Platform |
| Phiên bản | v2.0 |
| Giai đoạn | Sau MVP |
| Sprint đề xuất | Sprint 18 |
| Mục đích | Đặc tả tính năng trợ lý AI hướng dẫn du lịch cho du khách và công cụ hỗ trợ tạo nội dung cho admin |
| Đối tượng đọc | Backend developer, frontend developer, AI engineer, UI/UX designer, AI coding agent, project owner |
| Công nghệ liên quan | ASP.NET Core Web API, Vue.js 3, PostgreSQL/PostGIS, optional pgvector/vector database, LLM API |
| Phạm vi chính | Chat hỏi đáp du lịch, gợi ý địa điểm/tuyến tham quan, tóm tắt nội dung, hỗ trợ admin tạo nội dung |
| Không thuộc phạm vi | Fine-tune model riêng, voice assistant realtime, AI đặt vé/thanh toán, AI tự quyết định thông tin ngoài dữ liệu hệ thống |

---

## **2\. Bối cảnh**

Sau MVP và các sprint mở rộng, hệ thống **Smart Tourism 360 Platform** đã có nền tảng dữ liệu phong phú:

\- Địa điểm du lịch  
\- Danh mục địa điểm  
\- Khu vực triển khai  
\- Ảnh, video, audio  
\- Tour 360  
\- Panorama  
\- Hotspot  
\- Tuyến tham quan / itinerary  
\- Analytics hành vi người dùng  
\- Bản đồ 2D/3D  
\- Model 3D

Tuy nhiên, người dùng vẫn phải tự tìm kiếm và đọc nội dung. Với tính năng **AI Tour Guide**, hệ thống có thể hỗ trợ du khách theo cách tự nhiên hơn thông qua hội thoại.

Ví dụ người dùng có thể hỏi:

“Tôi có nửa ngày ở Cần Thơ thì nên đi đâu?”  
“Địa điểm nào phù hợp cho gia đình?”  
“Nhà Cổ Bình Thủy có gì nổi bật?”  
“Tour 360 này nên bắt đầu từ đâu?”  
“Có tuyến nào kết hợp văn hóa và ẩm thực không?”

AI Tour Guide giúp biến nền tảng từ một hệ thống hiển thị nội dung thành một hệ thống hỗ trợ khám phá và tư vấn du lịch thông minh.

---

## **3\. Mục tiêu tính năng**

Tính năng AI Tour Guide nhằm đạt các mục tiêu:

1\. Cho phép du khách hỏi đáp về địa điểm, tour 360, tuyến tham quan và nội dung du lịch.  
2\. Gợi ý địa điểm hoặc tuyến tham quan dựa trên nhu cầu cơ bản của người dùng.  
3\. Tóm tắt thông tin địa điểm một cách dễ hiểu.  
4\. Giải thích nội dung hotspot, panorama hoặc model 3D.  
5\. Hỗ trợ admin tạo bản nháp mô tả địa điểm, hotspot, audio guide script.  
6\. Hỗ trợ dịch nội dung cơ bản Việt \- Anh nếu Multi-language module đã có.  
7\. Giảm rủi ro AI bịa thông tin bằng cách giới hạn AI dựa trên dữ liệu nội bộ.  
8\. Chuẩn bị nền tảng cho RAG/vector search trong giai đoạn nâng cao.

---

## **4\. Nguyên tắc thiết kế AI**

### **4.1. AI phải dựa trên dữ liệu nội bộ**

AI không nên trả lời tự do dựa hoàn toàn vào kiến thức ngoài. Câu trả lời cần ưu tiên dựa trên dữ liệu có trong hệ thống:

\- destinations  
\- categories  
\- regions  
\- virtual\_tours  
\- panoramas  
\- hotspots  
\- audio\_guides  
\- routes  
\- route\_destinations  
\- models\_3d

Nếu không có đủ dữ liệu, AI phải nói rõ:

“Hiện hệ thống chưa có đủ thông tin để trả lời chính xác câu hỏi này.”

---

### **4.2. Không bịa thông tin nhạy cảm hoặc thông tin vận hành**

AI không được tự bịa:

\- Giá vé  
\- Giờ mở cửa  
\- Số điện thoại  
\- Địa chỉ chính xác  
\- Lịch sử địa điểm nếu database không có  
\- Chính sách tham quan  
\- Thông tin an toàn/đường đi thực tế

Nếu dữ liệu chưa có:

“Thông tin này hiện chưa được cập nhật trong hệ thống.”

---

### **4.3. AI là trợ lý gợi ý, không phải nguồn xác thực tuyệt đối**

AI có thể gợi ý lịch trình dựa trên dữ liệu hiện có, nhưng không nên trình bày như một quyết định chính thức.

Ví dụ nên nói:

“Dựa trên các địa điểm hiện có trong hệ thống, bạn có thể tham khảo tuyến sau...”

Không nên nói:

“Đây chắc chắn là tuyến tốt nhất.”

---

### **4.4. Bắt đầu đơn giản trước, RAG nâng cao sau**

Sprint 18 có thể triển khai theo 2 mức:

Mức 1 \- Context-based AI:  
Backend lấy dữ liệu liên quan từ database, tạo context ngắn, gửi đến LLM.

Mức 2 \- RAG:  
Tạo embedding cho destination, route, hotspot, transcript, sau đó truy xuất bằng vector search.

Khuyến nghị Sprint 18:

Làm Mức 1 trước để có tính năng hoạt động.  
Thiết kế kiến trúc mở để sau này nâng lên RAG.

---

## **5\. Phạm vi tính năng**

### **5.1. Trong Sprint 18**

Sprint 18 nên triển khai AI Tour Guide ở mức vừa đủ:

\- Public AI chat widget.  
\- AI hỏi đáp về địa điểm.  
\- AI gợi ý địa điểm theo nhu cầu đơn giản.  
\- AI gợi ý tuyến tham quan dựa trên route có sẵn.  
\- AI tóm tắt địa điểm.  
\- AI giải thích hotspot/tour nếu có context.  
\- Admin AI assistant tạo bản nháp mô tả địa điểm/hotspot/audio script.  
\- Backend AI endpoint.  
\- Logging hội thoại cơ bản nếu cần.

---

### **5.2. Ngoài phạm vi Sprint 18**

Không làm trong Sprint 18:

\- Không fine-tune model riêng.  
\- Không voice chat realtime.  
\- Không speech-to-text/text-to-speech realtime.  
\- Không AI đặt vé, thanh toán, booking.  
\- Không AI tự crawl dữ liệu Internet.  
\- Không AI tự sửa database.  
\- Không recommendation cá nhân hóa phức tạp.  
\- Không vector search bắt buộc nếu chưa cần.

---

## **6\. Đối tượng sử dụng**

### **6.1. Du khách**

Du khách dùng AI để:

\- Hỏi nhanh về địa điểm.  
\- Tìm địa điểm phù hợp.  
\- Hỏi tuyến tham quan.  
\- Hỏi cách khám phá tour 360\.  
\- Tóm tắt nội dung dài.

Ví dụ:

“Tôi thích không gian văn hóa, nên tham quan địa điểm nào?”  
“Tôi muốn đi tuyến ngắn trong nửa ngày.”  
“Chùa Ông Cần Thơ có gì nổi bật?”  
“Địa điểm nào có tour 360?”

---

### **6.2. Admin / Content Manager**

Admin dùng AI để:

\- Viết bản nháp mô tả địa điểm.  
\- Viết mô tả ngắn cho destination card.  
\- Tạo kịch bản audio guide.  
\- Gợi ý nội dung hotspot.  
\- Dịch nội dung sang tiếng Anh.  
\- Tóm tắt nội dung dài.

Lưu ý:

AI chỉ tạo bản nháp. Admin cần kiểm duyệt trước khi xuất bản.

---

### **6.3. Cơ quan quản lý**

Cơ quan quản lý có thể dùng AI để:

\- Tìm nhanh thông tin trong hệ thống.  
\- Hỏi địa điểm/tuyến nào đang có dữ liệu.  
\- Yêu cầu tóm tắt nội dung du lịch.  
\- Kết hợp với analytics để hiểu hành vi người dùng trong tương lai.

---

## **7\. Use Cases chính**

### **7.1. UC-01: Du khách hỏi về một địa điểm**

Input:

“Nhà Cổ Bình Thủy có gì nổi bật?”

AI cần:

\- Tìm destination liên quan.  
\- Lấy mô tả, category, địa chỉ, tour 360 nếu có.  
\- Trả lời ngắn gọn, dễ hiểu.  
\- Gợi ý mở trang chi tiết hoặc tour 360 nếu có.

Output mẫu:

Nhà Cổ Bình Thủy là một địa điểm thuộc nhóm Văn hóa \- lịch sử. Theo dữ liệu hiện có, địa điểm này nổi bật với kiến trúc nhà cổ và giá trị văn hóa Nam Bộ. Bạn có thể xem chi tiết địa điểm hoặc mở tour 360 nếu muốn tham quan không gian trực quan hơn.

---

### **7.2. UC-02: Du khách hỏi gợi ý địa điểm**

Input:

“Tôi muốn tham quan nơi có yếu tố văn hóa và có tour 360.”

AI cần:

\- Tìm destinations category văn hóa/tâm linh.  
\- Ưu tiên hasVirtualTour \= true.  
\- Trả danh sách gợi ý.

Output mẫu:

Dựa trên dữ liệu hiện có, bạn có thể tham khảo:  
1\. Nhà Cổ Bình Thủy \- thuộc nhóm Văn hóa \- lịch sử, có tour 360\.  
2\. Chùa Ông Cần Thơ \- thuộc nhóm Tâm linh, có tour 360\.  
Bạn có thể mở tour 360 để xem trước không gian trước khi tham quan thực tế.

---

### **7.3. UC-03: Du khách hỏi tuyến tham quan**

Input:

“Tôi có nửa ngày ở Cần Thơ thì nên đi tuyến nào?”

AI cần:

\- Tìm route có estimatedDuration \= nửa ngày hoặc theme phù hợp.  
\- Trả route có danh sách điểm.  
\- Không tự tạo route nếu chưa có dữ liệu, trừ khi nói rõ là gợi ý tham khảo.

Output mẫu:

Dựa trên các tuyến tham quan hiện có trong hệ thống, bạn có thể tham khảo tuyến “Khám phá văn hóa Cần Thơ”. Tuyến này gồm các điểm như Nhà Cổ Bình Thủy, Chùa Ông Cần Thơ và Bến Ninh Kiều. Đây là tuyến phù hợp nếu bạn muốn kết hợp văn hóa, lịch sử và không gian ven sông.

---

### **7.4. UC-04: Du khách hỏi trong tour 360**

Input:

“Tôi đang ở panorama Sân Trước Nhà Cổ, có gì cần chú ý?”

AI cần:

\- Lấy current panorama context.  
\- Lấy hotspots trong panorama.  
\- Lấy destination/tour liên quan.  
\- Giải thích ngắn gọn.

Output mẫu:

Bạn đang ở khu vực Sân Trước Nhà Cổ. Trong panorama này, bạn có thể chú ý hotspot giới thiệu Nhà Cổ để xem thêm thông tin, hoặc bấm mũi tên điều hướng để đi vào không gian bên trong.

---

### **7.5. UC-05: Admin tạo mô tả địa điểm**

Input:

“Tạo mô tả ngắn cho Nhà Cổ Bình Thủy dựa trên thông tin hiện có.”

AI cần:

\- Lấy dữ liệu destination.  
\- Tạo mô tả ngắn 2-3 câu.  
\- Không tự thêm thông tin không có trong dữ liệu.

Output phải là bản nháp, ví dụ:

Bản nháp mô tả:  
Nhà Cổ Bình Thủy là điểm đến văn hóa \- lịch sử nổi bật tại Cần Thơ, mang đậm dấu ấn kiến trúc và đời sống Nam Bộ. Đây là địa điểm phù hợp cho du khách muốn tìm hiểu không gian văn hóa địa phương và kết hợp tham quan ảo qua tour 360\.

---

## **8\. Kiến trúc tổng quan**

### **8.1. Kiến trúc mức cao**

Frontend Vue.js  
├── Public AI Chat Widget  
├── Tour 360 AI Assistant Panel  
└── Admin AI Assistant Panel

Backend ASP.NET Core API  
├── AIController  
├── AIService  
├── ContextBuilderService  
├── TourismDataService  
└── LLMProviderService

Database PostgreSQL/PostGIS  
├── destinations  
├── categories  
├── regions  
├── virtual\_tours  
├── panoramas  
├── hotspots  
├── routes  
├── audio\_guides  
├── models\_3d  
└── ai\_chat\_logs, optional

---

### **8.2. Luồng xử lý cơ bản**

User gửi câu hỏi  
→ Frontend gọi /api/ai/chat  
→ Backend xác định intent cơ bản  
→ Backend lấy dữ liệu liên quan từ database  
→ Backend tạo context  
→ Backend gửi prompt \+ context đến LLM  
→ LLM trả câu trả lời  
→ Backend kiểm tra/format response  
→ Frontend hiển thị câu trả lời

---

### **8.3. Luồng xử lý không có dữ liệu**

User hỏi thông tin không có trong database  
→ Backend không tìm thấy context phù hợp  
→ AI trả lời theo template an toàn:  
   “Hiện hệ thống chưa có đủ dữ liệu để trả lời chính xác câu hỏi này.”

---

## **9\. Data Sources cho AI**

AI có thể sử dụng các nguồn dữ liệu nội bộ:

### **9.1. Destination data**

\- name  
\- category  
\- shortDescription  
\- fullDescription  
\- address  
\- openingHours  
\- ticketPrice  
\- hasVirtualTour  
\- hasAudioGuide

---

### **9.2. Route / Itinerary data**

\- route title  
\- description  
\- estimatedDuration  
\- distanceKm  
\- theme  
\- list of destinations  
\- displayOrder  
\- estimatedStayTime  
\- note

---

### **9.3. Virtual Tour data**

\- tour title  
\- description  
\- default panorama  
\- panoramas  
\- hotspots

---

### **9.4. Panorama / Hotspot data**

\- panorama title  
\- panorama description  
\- hotspot title  
\- hotspot description  
\- hotspot type  
\- target panorama  
\- linked media

---

### **9.5. Audio transcript**

\- audio title  
\- transcript  
\- languageCode  
\- targetType  
\- targetId

---

### **9.6. Model 3D data**

\- model title  
\- model description  
\- targetType  
\- targetId

---

### **9.7. Analytics data, sau này**

AI có thể dùng analytics để gợi ý:

\- địa điểm được xem nhiều  
\- tour được mở nhiều  
\- route được quan tâm

Sprint 18 không bắt buộc dùng analytics trong câu trả lời, nhưng có thể chuẩn bị.

---

## **10\. AI Modes**

Tính năng AI nên chia thành 2 mode:

---

### **10.1. Public Tour Guide Mode**

Dành cho du khách.

Chức năng:

\- Hỏi đáp về địa điểm.  
\- Gợi ý địa điểm.  
\- Gợi ý tuyến tham quan.  
\- Hỏi đáp trong tour 360\.  
\- Tóm tắt nội dung.

Giọng điệu:

\- Thân thiện  
\- Ngắn gọn  
\- Dễ hiểu  
\- Không quá kỹ thuật  
\- Có gợi ý hành động tiếp theo

---

### **10.2. Admin Content Assistant Mode**

Dành cho admin.

Chức năng:

\- Tạo mô tả ngắn.  
\- Tạo mô tả chi tiết.  
\- Tạo nội dung hotspot.  
\- Tạo audio guide script.  
\- Gợi ý bản dịch.  
\- Tóm tắt nội dung.

Giọng điệu:

\- Chuyên nghiệp  
\- Rõ ràng  
\- Có cấu trúc  
\- Nhấn mạnh đây là bản nháp cần kiểm duyệt

---

## **11\. Intent Design**

Sprint 18 có thể nhận diện intent đơn giản bằng rule hoặc để LLM xử lý có hướng dẫn.

Intent gợi ý:

ask\_destination  
recommend\_destination  
ask\_route  
recommend\_route  
ask\_tour  
ask\_panorama  
ask\_hotspot  
summarize\_destination  
generate\_description  
generate\_audio\_script  
translate\_content  
fallback

---

### **11.1. Intent mapping ví dụ**

| Câu hỏi | Intent |
| ----- | ----- |
| “Nhà Cổ Bình Thủy có gì?” | ask\_destination |
| “Tôi nên đi đâu nếu thích sinh thái?” | recommend\_destination |
| “Có tuyến nào đi nửa ngày không?” | recommend\_route |
| “Tour này bắt đầu từ đâu?” | ask\_tour |
| “Viết mô tả ngắn cho địa điểm này” | generate\_description |
| “Tạo kịch bản audio guide” | generate\_audio\_script |
| “Dịch sang tiếng Anh” | translate\_content |

---

## **12\. Context Building Strategy**

### **12.1. Context theo destinationId**

Nếu frontend gửi `destinationId`, backend lấy:

\- destination detail  
\- category  
\- region  
\- virtual tour nếu có  
\- panoramas  
\- hotspots chính  
\- audio guide transcript nếu có  
\- models\_3d nếu có  
\- routes chứa destination nếu có

Dùng cho:

\- Chat trong trang chi tiết địa điểm  
\- Chat trong tour 360

---

### **12.2. Context theo routeId**

Nếu frontend gửi `routeId`, backend lấy:

\- route detail  
\- list destinations  
\- estimated duration  
\- theme  
\- notes

Dùng cho:

\- Hỏi đáp trong trang route detail  
\- Gợi ý tuyến tương tự

---

### **12.3. Context tìm kiếm theo keyword**

Nếu không có destinationId/routeId, backend có thể tìm keyword trong:

\- destination name  
\- destination description  
\- category name  
\- route title  
\- route description

Sprint 18 làm đơn giản bằng SQL search:

ILIKE keyword

Sau này nâng cấp bằng vector search.

---

### **12.4. Giới hạn context**

Không gửi quá nhiều dữ liệu vào LLM.

Gợi ý:

\- Tối đa 5 destinations liên quan.  
\- Tối đa 3 routes liên quan.  
\- Tối đa 10 hotspots/panoramas liên quan.  
\- Tối đa 6000-10000 tokens context, tùy model.

---

## **13\. Prompt Design**

### **13.1. System instruction cho Public AI**

Bạn là trợ lý hướng dẫn du lịch của hệ thống Smart Tourism 360 Platform.  
Bạn chỉ được trả lời dựa trên dữ liệu hệ thống cung cấp trong phần CONTEXT.  
Nếu thông tin không có trong CONTEXT, hãy nói rõ rằng hệ thống chưa có dữ liệu.  
Không tự bịa giá vé, giờ mở cửa, lịch sử, địa chỉ hoặc thông tin vận hành.  
Trả lời bằng tiếng Việt, thân thiện, ngắn gọn và dễ hiểu.  
Khi phù hợp, hãy gợi ý người dùng mở trang chi tiết, tour 360 hoặc tuyến tham quan.

---

### **13.2. System instruction cho Admin AI**

Bạn là trợ lý tạo nội dung cho admin của Smart Tourism 360 Platform.  
Bạn chỉ tạo bản nháp dựa trên dữ liệu được cung cấp.  
Không tự thêm thông tin chưa có trong dữ liệu.  
Kết quả cần rõ ràng, chuyên nghiệp, phù hợp nội dung du lịch.  
Luôn nhắc rằng nội dung cần được admin kiểm duyệt trước khi xuất bản nếu phù hợp.

---

### **13.3. Prompt structure**

SYSTEM:  
\[system instruction\]

CONTEXT:  
\[dữ liệu nội bộ đã lấy từ database\]

USER QUESTION:  
\[câu hỏi người dùng\]

RESPONSE REQUIREMENTS:  
\- Trả lời bằng tiếng Việt.  
\- Không bịa thông tin ngoài context.  
\- Nếu thiếu dữ liệu, nói rõ.  
\- Nếu có gợi ý địa điểm/tour, liệt kê ngắn gọn.

---

## **14\. API Specification**

## **14.1. Public AI Chat API**

POST /api/ai/chat

Request:

{  
  "message": "Tôi có nửa ngày ở Cần Thơ thì nên đi đâu?",  
  "sessionId": "st360-session-abc123",  
  "context": {  
    "destinationId": null,  
    "routeId": null,  
    "tourId": null,  
    "panoramaId": null,  
    "source": "public\_chat"  
  }  
}

Response:

{  
  "success": true,  
  "data": {  
    "answer": "Dựa trên các tuyến tham quan hiện có trong hệ thống, bạn có thể tham khảo tuyến khám phá văn hóa Cần Thơ...",  
    "intent": "recommend\_route",  
    "suggestions": \[  
      {  
        "type": "route",  
        "id": "route-001",  
        "title": "Tuyến khám phá văn hóa Cần Thơ"  
      }  
    \],  
    "usedContext": {  
      "destinations": 3,  
      "routes": 1,  
      "hotspots": 0  
    }  
  }  
}

---

## **14.2. Admin AI Content API**

POST /api/admin/ai/generate-content

Request:

{  
  "taskType": "generate\_destination\_short\_description",  
  "targetType": "destination",  
  "targetId": "destination-001",  
  "instruction": "Viết mô tả ngắn 2 câu, phong cách hấp dẫn cho du khách."  
}

Response:

{  
  "success": true,  
  "data": {  
    "draftContent": "Nhà Cổ Bình Thủy là điểm đến văn hóa \- lịch sử nổi bật tại Cần Thơ...",  
    "taskType": "generate\_destination\_short\_description",  
    "warning": "Nội dung này là bản nháp do AI tạo và cần được admin kiểm duyệt trước khi xuất bản."  
  }  
}

---

## **14.3. AI Suggestions API, optional**

Có thể thêm API gợi ý câu hỏi:

GET /api/ai/suggestions?destinationId=...

Response:

{  
  "success": true,  
  "data": \[  
    "Địa điểm này có gì nổi bật?",  
    "Có tour 360 không?",  
    "Tôi nên tham quan khu vực nào trước?",  
    "Có tuyến nào liên quan đến địa điểm này không?"  
  \]  
}

---

## **15\. Database Design**

Sprint 18 có thể chưa cần lưu toàn bộ hội thoại. Nhưng để debug và cải thiện, có thể thêm bảng `ai_chat_logs`.

---

## **15.1. Bảng `ai_chat_logs`, optional**

| Cột | Kiểu dữ liệu | Bắt buộc | Mô tả |
| ----- | ----- | ----- | ----- |
| id | uuid | Có | Khóa chính |
| session\_id | varchar(150) | Không | Phiên chat ẩn danh |
| user\_id | uuid | Không | Nếu là admin/user đăng nhập |
| mode | varchar(50) | Có | public/admin |
| message | text | Có | Câu hỏi |
| answer | text | Có | Câu trả lời |
| intent | varchar(100) | Không | Intent được nhận diện |
| context\_summary | jsonb | Không | Tóm tắt context đã dùng |
| created\_at | timestamp | Có | Thời gian tạo |

DBML:

Table ai\_chat\_logs {  
  id uuid \[primary key\]  
  session\_id varchar(150)  
  user\_id uuid  
  mode varchar(50) \[not null, note: 'public, admin'\]  
  message text \[not null\]  
  answer text \[not null\]  
  intent varchar(100)  
  context\_summary jsonb  
  created\_at timestamp  
}

Ref: ai\_chat\_logs.user\_id \> users.id

Lưu ý:

Không lưu thông tin cá nhân nhạy cảm trong log.  
Có thể tắt logging nếu không cần.

---

## **16\. Backend Service Design**

### **16.1. Services đề xuất**

IAIChatService  
AIChatService

IContextBuilderService  
ContextBuilderService

ILLMProviderService  
LLMProviderService

IAIContentService  
AIContentService

---

### **16.2. AIChatService**

Chịu trách nhiệm:

\- Nhận message.  
\- Xác định mode public/admin.  
\- Gọi ContextBuilderService.  
\- Gọi LLMProviderService.  
\- Format response.  
\- Lưu log nếu bật.

---

### **16.3. ContextBuilderService**

Chịu trách nhiệm:

\- Lấy dữ liệu destination.  
\- Lấy dữ liệu route.  
\- Lấy dữ liệu tour/panorama/hotspot.  
\- Tìm kiếm dữ liệu liên quan theo keyword.  
\- Tạo context ngắn gọn.

---

### **16.4. LLMProviderService**

Chịu trách nhiệm:

\- Đóng gói request đến LLM provider.  
\- Quản lý API key.  
\- Timeout/retry.  
\- Xử lý lỗi provider.  
\- Trả text response về service.

LLM provider có thể cấu hình:

OpenAI  
Gemini  
Claude  
Local model, optional

---

## **17\. Environment Configuration**

Backend cần biến môi trường:

AI\_PROVIDER=openai  
AI\_API\_KEY=your\_api\_key\_here  
AI\_MODEL=gpt-4o-mini  
AI\_MAX\_TOKENS=1000  
AI\_TEMPERATURE=0.3  
AI\_ENABLE\_LOGGING=true

Nguyên tắc:

\- Không commit AI\_API\_KEY lên GitHub.  
\- Cung cấp .env.example.  
\- Cho phép tắt AI nếu chưa cấu hình key.

Nếu chưa có key:

Backend trả lỗi thân thiện:  
“AI assistant is not configured.”

---

## **18\. UI/UX Specification**

## **18.1. Public AI Chat Widget**

Vị trí:

Góc phải dưới màn hình

Trạng thái:

Collapsed:  
\- Nút tròn icon chat/compass

Expanded:  
\- Chat panel  
\- Header “AI Tour Guide”  
\- Message list  
\- Suggestion chips  
\- Input box  
\- Send button

---

### **18.2. Chat Widget behavior**

\- User bấm icon chat để mở.  
\- Hiển thị lời chào.  
\- Hiển thị câu hỏi gợi ý.  
\- User nhập câu hỏi.  
\- AI trả lời.  
\- Nếu có suggestions, hiển thị link mở destination/route/tour.

Lời chào mẫu:

Xin chào\! Tôi là trợ lý du lịch ảo. Bạn có thể hỏi tôi về địa điểm, tour 360 hoặc tuyến tham quan trong hệ thống.

---

### **18.3. Suggested Questions**

Trên trang chủ:

\- Tôi nên khám phá địa điểm nào trước?  
\- Có địa điểm nào có tour 360 không?  
\- Gợi ý tuyến tham quan nửa ngày.

Trên destination detail:

\- Địa điểm này có gì nổi bật?  
\- Có tour 360 không?  
\- Địa điểm này thuộc tuyến nào?

Trong tour 360:

\- Tôi đang ở khu vực nào?  
\- Có hotspot nào cần chú ý?  
\- Tôi nên đi scene nào tiếp theo?

---

### **18.4. AI trong Tour 360**

Trong Tour 360 Viewer có thể có nút:

Hỏi AI

Khi mở, AI nhận context:

destinationId  
tourId  
panoramaId

AI có thể trả lời dựa trên panorama hiện tại.

---

### **18.5. Admin AI Assistant Panel**

Trong admin form, thêm nút:

AI Generate

Áp dụng ở:

\- Destination short description  
\- Destination full description  
\- Hotspot description  
\- Audio guide transcript  
\- Route description

Ví dụ trong Destination Form:

\[AI tạo mô tả ngắn\]  
\[AI tạo mô tả chi tiết\]

Sau khi AI tạo:

\- Hiển thị bản nháp trong modal.  
\- Admin có thể copy.  
\- Admin có thể chèn vào field.  
\- Admin có thể chỉnh sửa trước khi lưu.

---

## **19\. Frontend Components**

### **19.1. Public components**

AITourGuideButton.vue  
AIChatWidget.vue  
AIChatMessage.vue  
AIQuestionSuggestions.vue  
AIAnswerSuggestions.vue  
AITypingIndicator.vue

---

### **19.2. Tour 360 components**

TourAIButton.vue  
TourAIContextPanel.vue

---

### **19.3. Admin components**

AdminAIAssistantButton.vue  
AIGenerateContentModal.vue  
AIDraftPreview.vue

---

## **20\. Safety & Hallucination Control**

### **20.1. Không có context thì không trả lời bịa**

Rule:

Nếu không tìm thấy dữ liệu liên quan trong database, câu trả lời phải nói rõ hệ thống chưa có thông tin.

---

### **20.2. Không tự bịa thông tin vận hành**

AI không được tự bịa:

\- Giờ mở cửa  
\- Giá vé  
\- Chính sách tham quan  
\- Số điện thoại  
\- Địa chỉ  
\- Lịch sử cụ thể

---

### **20.3. Câu trả lời phải nêu nguồn nội bộ**

Không cần citation phức tạp, nhưng nên nói:

“Dựa trên dữ liệu hiện có trong hệ thống...”

---

### **20.4. Admin phải kiểm duyệt nội dung AI**

Mọi nội dung AI tạo cho admin cần kèm cảnh báo:

“Nội dung này là bản nháp do AI tạo và cần được kiểm duyệt trước khi xuất bản.”

---

## **21\. Privacy & Logging**

### **21.1. Không yêu cầu đăng nhập với public chat**

Public chat dùng sessionId ẩn danh.

Không lưu:

\- Họ tên  
\- Email  
\- Số điện thoại  
\- Vị trí GPS chính xác

---

### **21.2. Chat logs**

Nếu bật logging, chỉ lưu:

\- sessionId  
\- message  
\- answer  
\- intent  
\- context summary  
\- createdAt

Không nên lưu API key, prompt đầy đủ nếu chứa dữ liệu nhạy cảm.

---

### **21.3. Admin logs**

Admin AI logs có thể gắn userId để audit, nhưng vẫn không nên lưu thông tin nhạy cảm.

---

## **22\. Analytics Integration**

Nếu Analytics module đã có, tracking các event:

open\_ai\_chat  
send\_ai\_message  
receive\_ai\_answer  
click\_ai\_suggestion  
admin\_generate\_ai\_content  
admin\_insert\_ai\_draft

Ví dụ:

{  
  "eventName": "send\_ai\_message",  
  "targetType": "ai\_chat",  
  "targetId": null,  
  "metadata": {  
    "source": "destination\_detail",  
    "destinationId": "destination-001"  
  }  
}

---

## **23\. Error Handling**

### **23.1. AI provider lỗi**

Message cho user:

Trợ lý AI hiện chưa phản hồi được. Vui lòng thử lại sau.

---

### **23.2. AI chưa được cấu hình**

Message:

Tính năng AI chưa được cấu hình trong môi trường hiện tại.

---

### **23.3. Không có dữ liệu liên quan**

Message:

Hiện hệ thống chưa có đủ dữ liệu để trả lời chính xác câu hỏi này.

---

### **23.4. Timeout**

Message:

Yêu cầu mất quá nhiều thời gian xử lý. Vui lòng thử lại với câu hỏi ngắn hơn.

---

## **24\. Performance & Cost Control**

### **24.1. Giới hạn độ dài câu hỏi**

Gợi ý:

Tối đa 1000 ký tự cho public chat.  
Tối đa 2000 ký tự cho admin content generation.

---

### **24.2. Giới hạn response**

Public answer: 150-300 từ.  
Admin draft: tùy task, nhưng nên giới hạn.

---

### **24.3. Rate limiting**

Public AI cần giới hạn để tránh abuse:

\- 10 câu hỏi / session / 10 phút, giai đoạn đầu.  
\- Admin có thể cao hơn.

Sprint 18 có thể ghi TODO nếu chưa triển khai rate limiting.

---

### **24.4. Cache câu hỏi phổ biến**

Sau này có thể cache:

\- Gợi ý địa điểm phổ biến  
\- Tóm tắt destination  
\- Tóm tắt route

Chưa bắt buộc trong Sprint 18\.

---

## **25\. Future RAG Architecture**

### **25.1. Khi nào cần RAG?**

RAG cần thiết khi:

\- Dữ liệu địa điểm nhiều.  
\- Audio transcript dài.  
\- Nhiều route/hotspot.  
\- Người dùng hỏi câu rộng.  
\- SQL keyword search không đủ tốt.

---

### **25.2. Dữ liệu cần embedding**

\- Destination full description  
\- Hotspot description  
\- Audio transcript  
\- Route description  
\- Model 3D description

---

### **25.3. Vector storage**

Có thể dùng:

\- PostgreSQL \+ pgvector  
\- Qdrant  
\- Weaviate  
\- Chroma

Khuyến nghị cho dự án hiện tại:

PostgreSQL \+ pgvector

Lý do:

\- Hệ thống đã dùng PostgreSQL.  
\- Dễ triển khai bằng Docker.  
\- Không cần thêm service phức tạp ở giai đoạn đầu.

---

## **26\. Test Cases**

### **26.1. Backend test cases**

TC-BE-01: Gửi câu hỏi public hợp lệ và nhận câu trả lời.  
TC-BE-02: Gửi câu hỏi liên quan destinationId và AI dùng đúng context.  
TC-BE-03: Gửi câu hỏi không có dữ liệu và AI trả fallback.  
TC-BE-04: Admin gọi generate content có JWT thành công.  
TC-BE-05: Gọi admin AI API không có JWT nhận 401\.  
TC-BE-06: Khi AI provider lỗi, API trả message thân thiện.  
TC-BE-07: Không cho message vượt quá độ dài giới hạn.

---

### **26.2. Frontend public test cases**

TC-FE-01: Chat widget mở/đóng được.  
TC-FE-02: Gửi câu hỏi và hiển thị câu trả lời.  
TC-FE-03: Suggested questions hiển thị đúng theo page.  
TC-FE-04: AI suggestion link mở được destination/route/tour.  
TC-FE-05: AI lỗi không làm crash UI.  
TC-FE-06: Trong tour 360, AI nhận đúng destinationId/tourId/panoramaId.

---

### **26.3. Frontend admin test cases**

TC-ADMIN-01: Admin bấm AI Generate trong destination form.  
TC-ADMIN-02: Modal hiển thị bản nháp.  
TC-ADMIN-03: Admin insert draft vào field.  
TC-ADMIN-04: Admin chỉnh sửa và lưu như bình thường.  
TC-ADMIN-05: API lỗi hiển thị toast rõ ràng.

---

### **26.4. Safety test cases**

TC-SAFE-01: Hỏi giá vé khi database chưa có, AI không bịa.  
TC-SAFE-02: Hỏi giờ mở cửa khi chưa có, AI nói chưa cập nhật.  
TC-SAFE-03: Hỏi địa điểm không tồn tại, AI nói không tìm thấy dữ liệu.  
TC-SAFE-04: Yêu cầu tạo nội dung admin, AI ghi rõ là bản nháp.

---

## **27\. Sprint 18 Implementation Plan**

### **27.1. Mục tiêu Sprint 18**

Bổ sung AI Tour Guide giúp du khách hỏi đáp về địa điểm, tour 360, route và hỗ trợ admin tạo bản nháp nội dung du lịch.

---

### **27.2. Checklist Backend**

\- \[ \] Thêm cấu hình AI provider trong appsettings/env.  
\- \[ \] Tạo LLMProviderService.  
\- \[ \] Tạo ContextBuilderService.  
\- \[ \] Tạo AIChatService.  
\- \[ \] Tạo AIContentService.  
\- \[ \] Tạo POST /api/ai/chat.  
\- \[ \] Tạo POST /api/admin/ai/generate-content.  
\- \[ \] Validate message length.  
\- \[ \] Fallback khi thiếu context.  
\- \[ \] Fallback khi provider lỗi.  
\- \[ \] Optional: tạo ai\_chat\_logs.

---

### **27.3. Checklist Frontend Public**

\- \[ \] Tạo AIChatWidget.  
\- \[ \] Tạo AI Tour Guide floating button.  
\- \[ \] Tạo message list.  
\- \[ \] Tạo suggestion chips.  
\- \[ \] Tạo typing indicator.  
\- \[ \] Gửi context theo page hiện tại.  
\- \[ \] Hiển thị AI suggestions có link.  
\- \[ \] Error state thân thiện.

---

### **27.4. Checklist Tour 360**

\- \[ \] Thêm nút Hỏi AI trong Tour 360\.  
\- \[ \] Gửi destinationId/tourId/panoramaId.  
\- \[ \] Suggested questions theo panorama.  
\- \[ \] AI trả lời không che mất viewer quá nhiều.

---

### **27.5. Checklist Admin**

\- \[ \] Thêm AI Generate button trong destination form.  
\- \[ \] Thêm AI Generate cho hotspot description nếu có.  
\- \[ \] Thêm AI Generate audio script nếu có audio guide form.  
\- \[ \] Tạo modal preview draft.  
\- \[ \] Cho phép insert draft vào field.  
\- \[ \] Hiển thị warning cần kiểm duyệt.

---

### **27.6. Checklist Security/Config**

\- \[ \] Không commit AI API key.  
\- \[ \] Cập nhật .env.example.  
\- \[ \] Cập nhật README hướng dẫn cấu hình AI.  
\- \[ \] Admin AI API yêu cầu JWT.  
\- \[ \] Public AI không thu thập thông tin cá nhân.

---

### **27.7. Checklist Regression**

\- \[ \] Public website vẫn hoạt động khi AI chưa cấu hình.  
\- \[ \] Destination detail vẫn hoạt động.  
\- \[ \] Tour 360 vẫn hoạt động.  
\- \[ \] Admin form vẫn hoạt động nếu không dùng AI.  
\- \[ \] Docker build vẫn thành công.

---

## **28\. Acceptance Criteria**

Sprint 18 hoàn thành khi:

\- Public chat widget hoạt động.  
\- User hỏi về destination và nhận câu trả lời dựa trên dữ liệu hệ thống.  
\- User hỏi thông tin không có dữ liệu và AI không bịa.  
\- AI có thể gợi ý route/destination dựa trên dữ liệu có sẵn.  
\- Tour 360 có thể mở AI với context hiện tại.  
\- Admin có thể tạo bản nháp mô tả bằng AI.  
\- Admin có thể chèn bản nháp vào form.  
\- API key không bị commit lên GitHub.  
\- Nếu AI provider lỗi, UI hiển thị lỗi thân thiện.  
\- Chức năng cũ không bị ảnh hưởng.

---

## **29\. Non-goals**

Sprint 18 không bao gồm:

\- Không fine-tune model.  
\- Không training model riêng.  
\- Không voice assistant realtime.  
\- Không AI booking.  
\- Không AI tự crawl Internet.  
\- Không AI tự cập nhật database.  
\- Không bắt buộc pgvector/RAG nâng cao.  
\- Không cá nhân hóa sâu theo tài khoản người dùng.

---

## **30\. Rủi ro và cách kiểm soát**

### **30.1. AI bịa thông tin**

Cách kiểm soát:

\- Prompt bắt buộc chỉ dựa trên context.  
\- Nếu thiếu dữ liệu, trả fallback.  
\- Safety test cases.  
\- Admin kiểm duyệt nội dung trước khi publish.

---

### **30.2. Chi phí API tăng**

Cách kiểm soát:

\- Giới hạn độ dài message.  
\- Giới hạn response.  
\- Rate limiting.  
\- Không gửi context quá lớn.  
\- Cache câu trả lời phổ biến sau này.

---

### **30.3. Provider lỗi hoặc timeout**

Cách kiểm soát:

\- Timeout rõ ràng.  
\- Error fallback thân thiện.  
\- Không làm crash UI.

---

### **30.4. Prompt/context quá dài**

Cách kiểm soát:

\- Giới hạn số destination/route/hotspot.  
\- Tóm tắt context trước khi gửi.  
\- Chỉ lấy dữ liệu liên quan.

---

### **30.5. Nội dung AI chưa phù hợp để xuất bản**

Cách kiểm soát:

\- Ghi rõ nội dung là bản nháp.  
\- Admin phải chỉnh sửa/duyệt.  
\- Không tự publish nội dung AI.

---

## **31\. Roadmap mở rộng sau Sprint 18**

Sau khi AI Tour Guide cơ bản hoạt động, có thể nâng cấp:

1\. RAG bằng pgvector.  
2\. Embedding destination/hotspot/audio transcript.  
3\. AI gợi ý itinerary cá nhân hóa.  
4\. AI trả lời đa ngôn ngữ.  
5\. AI tạo audio guide script nâng cao.  
6\. Text-to-speech cho audio guide.  
7\. Voice input cho du khách.  
8\. AI kết hợp analytics để gợi ý địa điểm phổ biến.  
9\. AI trong bản đồ 3D: hỏi “địa điểm gần đây có gì?”  
10\. AI hỗ trợ admin kiểm tra thiếu dữ liệu địa điểm.

---

## **32\. Kết luận**

Tính năng **AI Tour Guide** là bước nâng cấp quan trọng giúp **Smart Tourism 360 Platform** trở thành một nền tảng du lịch thông minh hơn. Thay vì chỉ để du khách tự đọc nội dung, hệ thống có thể hỗ trợ hỏi đáp, tóm tắt, gợi ý địa điểm, gợi ý tuyến tham quan và giải thích nội dung trong tour 360\.

Tuy nhiên, AI cần được triển khai có kiểm soát. Nguyên tắc quan trọng nhất là:

AI trả lời dựa trên dữ liệu nội bộ.  
Nếu không có dữ liệu, AI phải nói không chắc hoặc chưa có thông tin.

Sprint 18 nên bắt đầu bằng phiên bản AI vừa đủ:

Public AI Chat Widget  
→ Context từ destination/route/tour/hotspot  
→ Admin AI Draft Generator  
→ Safety fallback

Sau đó, hệ thống có thể mở rộng sang RAG, pgvector, AI itinerary, đa ngôn ngữ, text-to-speech và AI hướng dẫn viên trong không gian WebGIS 3D.

