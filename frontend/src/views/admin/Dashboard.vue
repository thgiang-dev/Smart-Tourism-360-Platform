<template>
  <div class="space-y-8">
    <!-- Welcome Header banner -->
    <div class="bg-gradient-to-r from-teal-800 to-slate-900 rounded-3xl p-6 md:p-8 text-white shadow-xl relative overflow-hidden">
      <div class="absolute w-64 h-64 rounded-full bg-teal-600/10 blur-3xl -top-20 -right-20"></div>
      <div class="relative z-10 space-y-2">
        <h2 class="text-xl md:text-3xl font-extrabold tracking-tight">Xin chào, {{ authStore.user?.fullName }}! 👋</h2>
        <p class="text-xs md:text-sm text-teal-200/80 max-w-xl">
          Chào mừng đến với bảng điều khiển Quản trị hệ thống Smart Tourism 360. Dưới đây là tóm tắt số liệu hoạt động của hệ thống hiện tại.
        </p>
      </div>
    </div>

    <!-- Error Alert -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-xl flex items-center space-x-3">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0" />
      <span class="text-sm font-medium">{{ error }}</span>
    </div>

    <!-- Statistics Grid -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
      <!-- Destinations count card -->
      <div class="bg-white p-6 rounded-2xl shadow-sm border border-slate-200/60 flex items-center justify-between hover:shadow-md transition">
        <div class="space-y-1">
          <span class="text-xs font-semibold text-slate-400 uppercase tracking-wider block">Địa điểm du lịch</span>
          <div v-if="loading" class="h-8 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-3xl font-black text-slate-800">{{ stats.destinationsCount }}</p>
        </div>
        <div class="p-4 bg-teal-50 text-teal-600 rounded-2xl border border-teal-100">
          <MapPinIcon class="w-6 h-6" />
        </div>
      </div>

      <!-- Categories count card -->
      <div class="bg-white p-6 rounded-2xl shadow-sm border border-slate-200/60 flex items-center justify-between hover:shadow-md transition">
        <div class="space-y-1">
          <span class="text-xs font-semibold text-slate-400 uppercase tracking-wider block">Danh mục</span>
          <div v-if="loading" class="h-8 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-3xl font-black text-slate-800">{{ stats.categoriesCount }}</p>
        </div>
        <div class="p-4 bg-amber-50 text-amber-600 rounded-2xl border border-amber-100">
          <LayersIcon class="w-6 h-6" />
        </div>
      </div>

      <!-- Regions count card -->
      <div class="bg-white p-6 rounded-2xl shadow-sm border border-slate-200/60 flex items-center justify-between hover:shadow-md transition">
        <div class="space-y-1">
          <span class="text-xs font-semibold text-slate-400 uppercase tracking-wider block">Khu vực</span>
          <div v-if="loading" class="h-8 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-3xl font-black text-slate-800">{{ stats.regionsCount }}</p>
        </div>
        <div class="p-4 bg-indigo-50 text-indigo-600 rounded-2xl border border-indigo-100">
          <GlobeIcon class="w-6 h-6" />
        </div>
      </div>

      <!-- Media mock count card -->
      <div class="bg-white p-6 rounded-2xl shadow-sm border border-slate-200/60 flex items-center justify-between hover:shadow-md transition">
        <div class="space-y-1">
          <span class="text-xs font-semibold text-slate-400 uppercase tracking-wider block">Tập tin Media</span>
          <div v-if="loading" class="h-8 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-3xl font-black text-slate-800">{{ stats.mediaCount }}</p>
        </div>
        <div class="p-4 bg-rose-50 text-rose-600 rounded-2xl border border-rose-100">
          <ImageIcon class="w-6 h-6" />
        </div>
      </div>
    </div>

    <!-- Quick Actions -->
    <div class="bg-white p-6 rounded-2xl border border-slate-200/60 shadow-sm space-y-4">
      <h3 class="text-sm font-bold text-slate-800 uppercase tracking-wider">Lối tắt thao tác nhanh</h3>
      <div class="grid grid-cols-1 sm:grid-cols-3 gap-4">
        <router-link 
          to="/admin/destinations/new" 
          class="flex items-center justify-between p-4 bg-teal-50/50 hover:bg-teal-50 border border-teal-100 rounded-xl font-semibold text-sm text-teal-700 hover:text-teal-800 transition"
        >
          <span>Thêm địa điểm du lịch mới</span>
          <PlusIcon class="w-4 h-4" />
        </router-link>
        <router-link 
          to="/admin/destinations" 
          class="flex items-center justify-between p-4 bg-slate-50 hover:bg-slate-100 border border-slate-200 rounded-xl font-semibold text-sm text-slate-700 hover:text-slate-800 transition"
        >
          <span>Quản lý địa điểm</span>
          <MapIcon class="w-4 h-4" />
        </router-link>
        <a 
          href="/" 
          target="_blank"
          class="flex items-center justify-between p-4 bg-slate-50 hover:bg-slate-100 border border-slate-200 rounded-xl font-semibold text-sm text-slate-700 hover:text-slate-800 transition"
        >
          <span>Xem Website công khai</span>
          <ExternalLinkIcon class="w-4 h-4" />
        </a>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useAuthStore } from '../../stores/auth'
import api from '../../utils/api'
import { 
  MapPin as MapPinIcon, 
  Layers as LayersIcon, 
  Globe as GlobeIcon, 
  Image as ImageIcon, 
  Plus as PlusIcon, 
  Map as MapIcon, 
  ExternalLink as ExternalLinkIcon, 
  AlertCircle as AlertCircleIcon 
} from 'lucide-vue-next'

const authStore = useAuthStore()

const loading = ref(false)
const error = ref(null)
const stats = ref({
  destinationsCount: 0,
  categoriesCount: 0,
  regionsCount: 0,
  mediaCount: 0
})

const fetchStatistics = async () => {
  loading.value = true
  error.value = null
  try {
    // Call counts APIs in parallel
    const [destResponse, regResponse, catResponse, mediaResponse] = await Promise.all([
      api.get('/api/admin/destinations', { params: { pageSize: 1 } }),
      api.get('/api/admin/regions'),
      api.get('/api/admin/categories'),
      api.get('/api/admin/media', { params: { pageSize: 1 } })
    ])

    if (destResponse.success) {
      stats.value.destinationsCount = destResponse.data.totalItems
    }
    if (regResponse.success) {
      stats.value.regionsCount = regResponse.data.length
    }
    if (catResponse.success) {
      stats.value.categoriesCount = catResponse.data.length
    }
    if (mediaResponse.success) {
      stats.value.mediaCount = mediaResponse.data.totalItems
    }
  } catch (err) {
    error.value = 'Không thể tải số liệu thống kê từ hệ thống.'
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchStatistics()
})
</script>
