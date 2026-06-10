<template>
  <div class="space-y-8 font-sans">
    <!-- Welcome Header banner -->
    <div class="bg-gradient-to-r from-teal-900 via-teal-800 to-slate-900 rounded-3xl p-8 text-white shadow-xl relative overflow-hidden">
      <div class="absolute inset-0 opacity-10 bg-[radial-gradient(#e5e7eb_1px,transparent_1px)] [background-size:16px_16px]"></div>
      <div class="absolute w-64 h-64 rounded-full bg-teal-500/10 blur-3xl -top-20 -right-20 pointer-events-none"></div>
      <div class="relative z-10 space-y-2">
        <h2 class="text-xl md:text-3xl font-black tracking-tight">Xin chào, {{ authStore.user?.fullName }}! 👋</h2>
        <p class="text-xs md:text-sm text-teal-200/80 max-w-xl font-medium">
          Chào mừng đến với bảng điều khiển Quản trị hệ thống Smart Tourism 360. Dưới đây là tóm tắt số liệu hoạt động của hệ thống hiện tại.
        </p>
      </div>
    </div>

    <!-- Error Alert -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-xl flex items-center space-x-3 shadow-sm">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0 text-red-500" />
      <span class="text-sm font-bold">{{ error }}</span>
    </div>

    <!-- Statistics Grid -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
      <!-- Destinations count card -->
      <div class="premium-card bg-white p-6 rounded-2xl shadow-sm border border-slate-200/80 flex items-center justify-between transition cursor-pointer">
        <div class="space-y-1">
          <span class="text-[10px] font-black text-slate-400 uppercase tracking-wider block">Địa điểm du lịch</span>
          <div v-if="loading" class="h-8 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-3xl font-black text-slate-800">{{ stats.destinationsCount }}</p>
        </div>
        <div class="p-4 bg-teal-50 text-teal-600 rounded-2xl border border-teal-100/50 shadow-inner">
          <MapPinIcon class="w-6 h-6 text-teal-600" />
        </div>
      </div>

      <!-- Categories count card -->
      <div class="premium-card bg-white p-6 rounded-2xl shadow-sm border border-slate-200/80 flex items-center justify-between transition cursor-pointer">
        <div class="space-y-1">
          <span class="text-[10px] font-black text-slate-400 uppercase tracking-wider block">Danh mục</span>
          <div v-if="loading" class="h-8 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-3xl font-black text-slate-800">{{ stats.categoriesCount }}</p>
        </div>
        <div class="p-4 bg-amber-50 text-amber-600 rounded-2xl border border-amber-100/50 shadow-inner">
          <LayersIcon class="w-6 h-6 text-amber-600" />
        </div>
      </div>

      <!-- Regions count card -->
      <div class="premium-card bg-white p-6 rounded-2xl shadow-sm border border-slate-200/80 flex items-center justify-between transition cursor-pointer">
        <div class="space-y-1">
          <span class="text-[10px] font-black text-slate-400 uppercase tracking-wider block">Khu vực</span>
          <div v-if="loading" class="h-8 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-3xl font-black text-slate-800">{{ stats.regionsCount }}</p>
        </div>
        <div class="p-4 bg-indigo-50 text-indigo-600 rounded-2xl border border-indigo-100/50 shadow-inner">
          <GlobeIcon class="w-6 h-6 text-indigo-600" />
        </div>
      </div>

      <!-- Media mock count card -->
      <div class="premium-card bg-white p-6 rounded-2xl shadow-sm border border-slate-200/80 flex items-center justify-between transition cursor-pointer">
        <div class="space-y-1">
          <span class="text-[10px] font-black text-slate-400 uppercase tracking-wider block">Tập tin Media</span>
          <div v-if="loading" class="h-8 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-3xl font-black text-slate-800">{{ stats.mediaCount }}</p>
        </div>
        <div class="p-4 bg-rose-50 text-rose-600 rounded-2xl border border-rose-100/50 shadow-inner">
          <ImageIcon class="w-6 h-6 text-rose-600" />
        </div>
      </div>
    </div>

    <!-- Quick Actions -->
    <div class="bg-white p-6 md:p-8 rounded-3xl border border-slate-200/80 shadow-sm space-y-5">
      <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-3">Lối tắt thao tác nhanh</h3>
      <div class="grid grid-cols-1 sm:grid-cols-3 gap-4">
        <router-link 
          to="/admin/destinations/new" 
          class="premium-btn flex items-center justify-between p-5 bg-teal-50/40 hover:bg-teal-50 border border-teal-100/60 rounded-2xl font-bold text-sm text-teal-700 hover:text-teal-800 transition"
        >
          <span>Thêm địa điểm du lịch mới</span>
          <PlusIcon class="w-5 h-5 text-teal-600" />
        </router-link>
        <router-link 
          to="/admin/destinations" 
          class="premium-btn flex items-center justify-between p-5 bg-slate-50 hover:bg-slate-100/60 border border-slate-200/80 rounded-2xl font-bold text-sm text-slate-700 hover:text-slate-800 transition"
        >
          <span>Quản lý địa điểm</span>
          <MapIcon class="w-5 h-5 text-slate-500" />
        </router-link>
        <a 
          href="/" 
          target="_blank"
          class="premium-btn flex items-center justify-between p-5 bg-slate-50 hover:bg-slate-100/60 border border-slate-200/80 rounded-2xl font-bold text-sm text-slate-700 hover:text-slate-800 transition"
        >
          <span>Xem Website công khai</span>
          <ExternalLinkIcon class="w-5 h-5 text-slate-500" />
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
