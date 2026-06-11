<template>
  <div class="min-h-screen bg-slate-50 flex font-sans">
    <!-- Sidebar for Desktop -->
    <aside 
      class="bg-slate-950 text-slate-200 w-64 flex-shrink-0 flex flex-col justify-between transition-all duration-300 border-r border-slate-800/50 hidden md:flex"
    >
      <div class="flex flex-col">
        <!-- Logo Section -->
        <div class="p-6 border-b border-slate-900 flex items-center justify-between">
          <span class="text-xl font-black tracking-wider text-teal-400">SMART TOURISM 360</span>
        </div>
        
        <!-- Navigation Links -->
        <nav class="p-4 space-y-1 flex-grow">
          <router-link 
            to="/admin" 
            exact-active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <LayoutDashboardIcon class="w-5 h-5" />
            <span>Tổng quan</span>
          </router-link>
          
          <router-link 
            to="/admin/destinations" 
            active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <MapPinIcon class="w-5 h-5" />
            <span>Địa điểm du lịch</span>
          </router-link>

          <a 
            href="#" 
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-500 cursor-not-allowed transition duration-150"
            title="Sẽ phát triển ở Sprint tiếp theo"
          >
            <LayersIcon class="w-5 h-5" />
            <span>Danh mục (Sprint 4+)</span>
          </a>

          <router-link 
            to="/admin/media"
            active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <ImageIcon class="w-5 h-5" />
            <span>Thư viện Media</span>
          </router-link>

          <router-link 
            to="/admin/virtual-tours"
            active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <NavigationIcon class="w-5 h-5" />
            <span>Tour ảo 360°</span>
          </router-link>

          <router-link 
            to="/admin/routes"
            active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <RouteIcon class="w-5 h-5" />
            <span>Tuyến tham quan</span>
          </router-link>

          <router-link 
            to="/admin/analytics"
            active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <BarChart3Icon class="w-5 h-5" />
            <span>Thống kê</span>
          </router-link>

          <router-link 
            to="/admin/models-3d"
            active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <BoxIcon class="w-5 h-5" />
            <span>Mô hình 3D</span>
          </router-link>
        </nav>
      </div>

      <!-- User footer -->
      <div class="p-4 border-t border-slate-900 flex flex-col space-y-3">
        <div class="flex items-center space-x-3 px-2">
          <div class="w-10 h-10 rounded-full bg-teal-600/25 border border-teal-500/35 flex items-center justify-center text-teal-400 font-bold uppercase">
            {{ userInitial }}
          </div>
          <div class="overflow-hidden">
            <h4 class="text-sm font-semibold text-slate-200 truncate">{{ authStore.user?.fullName }}</h4>
            <span class="text-xs text-slate-500 truncate block">{{ authStore.user?.role }}</span>
          </div>
        </div>
        <button 
          @click="handleLogout"
          class="w-full flex items-center justify-center space-x-2 py-2.5 bg-red-950/20 hover:bg-red-900/30 text-red-400 hover:text-red-300 border border-red-900/30 rounded-lg text-sm font-medium transition duration-200"
        >
          <LogOutIcon class="w-4 h-4" />
          <span>Đăng xuất</span>
        </button>
      </div>
    </aside>

    <!-- Mobile Sidebar overlay -->
    <div 
      v-if="isMobileMenuOpen" 
      @click="isMobileMenuOpen = false" 
      class="fixed inset-0 bg-slate-950/40 backdrop-blur-sm z-40 md:hidden"
    ></div>

    <!-- Mobile Sidebar -->
    <aside 
      class="fixed top-0 bottom-0 left-0 bg-slate-950 text-slate-200 w-64 z-50 flex flex-col justify-between transition-transform duration-300 md:hidden border-r border-slate-800/50"
      :class="isMobileMenuOpen ? 'translate-x-0' : '-translate-x-full'"
    >
      <div class="flex flex-col">
        <!-- Mobile Logo Header -->
        <div class="p-6 border-b border-slate-900 flex items-center justify-between">
          <span class="text-xl font-black tracking-wider text-teal-400">SMART TOURISM 360</span>
          <button @click="isMobileMenuOpen = false" class="text-slate-400 hover:text-slate-100 focus:outline-none">
            <XIcon class="w-6 h-6" />
          </button>
        </div>
        
        <!-- Navigation Links -->
        <nav class="p-4 space-y-1 flex-grow">
          <router-link 
            to="/admin" 
            exact-active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            @click="isMobileMenuOpen = false"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <LayoutDashboardIcon class="w-5 h-5" />
            <span>Tổng quan</span>
          </router-link>
          
          <router-link 
            to="/admin/destinations" 
            active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            @click="isMobileMenuOpen = false"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <MapPinIcon class="w-5 h-5" />
            <span>Địa điểm du lịch</span>
          </router-link>

          <a 
            href="#" 
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-500 cursor-not-allowed transition duration-150"
          >
            <LayersIcon class="w-5 h-5" />
            <span>Danh mục (Sprint 4+)</span>
          </a>

          <router-link 
            to="/admin/media"
            active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            @click="isMobileMenuOpen = false"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <ImageIcon class="w-5 h-5" />
            <span>Thư viện Media</span>
          </router-link>

          <router-link 
            to="/admin/virtual-tours"
            active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            @click="isMobileMenuOpen = false"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <NavigationIcon class="w-5 h-5" />
            <span>Tour ảo 360°</span>
          </router-link>

          <router-link 
            to="/admin/routes"
            active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            @click="isMobileMenuOpen = false"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <RouteIcon class="w-5 h-5" />
            <span>Tuyến tham quan</span>
          </router-link>

          <router-link 
            to="/admin/analytics"
            active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            @click="isMobileMenuOpen = false"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <BarChart3Icon class="w-5 h-5" />
            <span>Thống kê</span>
          </router-link>

          <router-link 
            to="/admin/models-3d"
            active-class="bg-teal-700/20 text-teal-400 font-semibold border-l-4 border-teal-500"
            @click="isMobileMenuOpen = false"
            class="flex items-center space-x-3 px-4 py-3 rounded-lg text-slate-400 hover:bg-slate-900 hover:text-slate-100 transition duration-150"
          >
            <BoxIcon class="w-5 h-5" />
            <span>Mô hình 3D</span>
          </router-link>
        </nav>
      </div>

      <!-- User footer -->
      <div class="p-4 border-t border-slate-900 flex flex-col space-y-3">
        <div class="flex items-center space-x-3 px-2">
          <div class="w-10 h-10 rounded-full bg-teal-600/25 border border-teal-500/35 flex items-center justify-center text-teal-400 font-bold uppercase">
            {{ userInitial }}
          </div>
          <div>
            <h4 class="text-sm font-semibold text-slate-200 truncate w-36">{{ authStore.user?.fullName }}</h4>
            <span class="text-xs text-slate-500 truncate block">{{ authStore.user?.role }}</span>
          </div>
        </div>
        <button 
          @click="handleLogout"
          class="w-full flex items-center justify-center space-x-2 py-2.5 bg-red-950/20 hover:bg-red-900/30 text-red-400 hover:text-red-300 border border-red-900/30 rounded-lg text-sm font-medium transition duration-200"
        >
          <LogOutIcon class="w-4 h-4" />
          <span>Đăng xuất</span>
        </button>
      </div>
    </aside>

    <!-- Main View Area -->
    <div class="flex-grow flex flex-col min-w-0">
      <!-- Topbar Header -->
      <header class="bg-white border-b border-slate-200/80 px-4 md:px-8 py-4 flex justify-between items-center z-30">
        <div class="flex items-center space-x-3">
          <!-- Mobile Menu Toggle Button -->
          <button 
            @click="isMobileMenuOpen = true" 
            class="md:hidden text-slate-600 hover:text-slate-900 focus:outline-none p-1 rounded hover:bg-slate-100"
          >
            <MenuIcon class="w-6 h-6" />
          </button>
          <h1 class="text-lg md:text-xl font-bold text-slate-800 tracking-tight">{{ pageTitle }}</h1>
        </div>
        
        <div class="flex items-center space-x-4">
          <!-- Connection status indicator -->
          <div class="flex items-center space-x-2 bg-emerald-50 border border-emerald-100 rounded-full px-3 py-1">
            <span class="w-2 h-2 bg-emerald-500 rounded-full animate-pulse"></span>
            <span class="text-xs font-semibold text-emerald-700 hidden sm:inline">API Connected</span>
          </div>
        </div>
      </header>

      <!-- Main Page Content Section -->
      <main class="flex-grow p-4 md:p-8 overflow-y-auto max-w-7xl w-full mx-auto">
        <router-view />
      </main>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '../../stores/auth'
import { 
  LayoutDashboard as LayoutDashboardIcon, 
  MapPin as MapPinIcon, 
  Layers as LayersIcon, 
  Image as ImageIcon, 
  Navigation as NavigationIcon, 
  Route as RouteIcon,
  BarChart3 as BarChart3Icon,
  LogOut as LogOutIcon, 
  Menu as MenuIcon, 
  X as XIcon,
  Box as BoxIcon
} from 'lucide-vue-next'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const isMobileMenuOpen = ref(false)

// Computes initial of active user for circular badge
const userInitial = computed(() => {
  const name = authStore.user?.fullName || 'A'
  return name.charAt(0)
})

// Computes dynamic page title based on active route path
const pageTitle = computed(() => {
  if (route.name === 'AdminDashboard') return 'Tổng quan hệ thống'
  if (route.name === 'AdminDestinations') return 'Quản lý địa điểm du lịch'
  if (route.name === 'AdminCreateDestination') return 'Thêm địa điểm du lịch'
  if (route.name === 'AdminEditDestination') return 'Chỉnh sửa địa điểm du lịch'
  if (route.name === 'AdminMedia') return 'Thư viện Media'
  if (route.name === 'AdminVirtualTours') return 'Quản lý Tour ảo 360°'
  if (route.name === 'AdminPanoramas') return 'Quản lý Cảnh Panorama'
  if (route.name === 'AdminHotspots') return 'Thiết lập Điểm tương tác (Hotspot)'
  if (route.name === 'AdminRoutes') return 'Quản lý Tuyến tham quan'
  if (route.name === 'AdminCreateRoute') return 'Tạo Tuyến tham quan'
  if (route.name === 'AdminEditRoute') return 'Chỉnh sửa Tuyến tham quan'
  if (route.name === 'AdminRouteDestinations') return 'Thiết lập Địa điểm cho Tuyến'
  if (route.name === 'AdminAnalytics') return 'Thống kê & Phân tích hành vi'
  if (route.name === 'AdminModels3D') return 'Quản lý Mô hình 3D'
  if (route.name === 'AdminCreateModel3D') return 'Tạo Mô hình 3D mới'
  if (route.name === 'AdminEditModel3D') return 'Chỉnh sửa Mô hình 3D'
  return 'Hệ thống Quản trị'
})

const handleLogout = () => {
  authStore.logout()
  router.push('/admin/login')
}
</script>

<style scoped>
/* Ensure smooth mobile menu side sliding drawer transition */
aside {
  will-change: transform;
}
</style>
