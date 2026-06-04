<template>
  <footer class="bg-slate-900 text-slate-400 border-t border-slate-800">
    <div class="max-w-7xl mx-auto px-4 py-12 sm:px-6 lg:px-8">
      <div class="grid grid-cols-1 md:grid-cols-4 gap-8">
        <!-- Logo and brand info -->
        <div class="space-y-4 md:col-span-2">
          <div class="flex items-center space-x-2">
            <div class="w-8 h-8 bg-teal-600 rounded-lg flex items-center justify-center text-white">
              <CompassIcon class="w-4 h-4" />
            </div>
            <span class="text-base font-black tracking-wider text-white">
              SMART TOURISM 360
            </span>
          </div>
          <p class="text-xs text-slate-500 max-w-sm leading-relaxed">
            Nền tảng du lịch thông minh, tích hợp bản đồ số khám phá và không gian tham quan thực tế ảo 360 độ tương tác cao, giúp quảng bá văn hóa và nâng cao trải nghiệm của du khách.
          </p>
        </div>

        <!-- Quick Links -->
        <div class="space-y-4">
          <h4 class="text-xs font-bold text-white uppercase tracking-wider">Khám phá</h4>
          <ul class="space-y-2 text-xs">
            <li>
              <router-link to="/" class="hover:text-teal-400 transition">Trang chủ</router-link>
            </li>
            <li>
              <router-link to="/explore" class="hover:text-teal-400 transition">Bản đồ khám phá</router-link>
            </li>
            <li>
              <router-link to="/destinations" class="hover:text-teal-400 transition">Địa điểm du lịch</router-link>
            </li>
          </ul>
        </div>

        <!-- Contact details -->
        <div class="space-y-4">
          <h4 class="text-xs font-bold text-white uppercase tracking-wider">Thông tin liên hệ</h4>
          <ul class="space-y-2 text-xs text-slate-500">
            <li class="flex items-center space-x-2">
              <MapPinIcon class="w-4 h-4 text-teal-500 flex-shrink-0" />
              <span>Thành phố Cần Thơ, Việt Nam</span>
            </li>
            <li class="flex items-center space-x-2">
              <PhoneIcon class="w-4 h-4 text-teal-500 flex-shrink-0" />
              <span>+84 (0) 292 3838 383</span>
            </li>
            <li class="flex items-center space-x-2">
              <MailIcon class="w-4 h-4 text-teal-500 flex-shrink-0" />
              <span>info@smarttourism360.vn</span>
            </li>
          </ul>
        </div>
      </div>

      <!-- Bottom bar copyright -->
      <div class="border-t border-slate-800 mt-12 pt-6 flex flex-col sm:flex-row justify-between items-center text-xs text-slate-600 gap-4">
        <p>&copy; 2026 Smart Tourism 360 Platform. Bản quyền thuộc về Đề án Phát triển Du lịch Số.</p>
        <div class="flex space-x-6">
          <a href="#" class="hover:text-teal-500 transition">Điều khoản sử dụng</a>
          <a href="#" class="hover:text-teal-500 transition">Chính sách bảo mật</a>
        </div>
      </div>
    </div>
  </footer>
</template>

<script setup>
import { Compass as CompassIcon, MapPin as MapPinIcon, Phone as PhoneIcon, Mail as MailIcon } from 'lucide-vue-next'
</script>
