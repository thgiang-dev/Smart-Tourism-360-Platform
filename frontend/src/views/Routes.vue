<template>
  <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-10 space-y-10 font-sans">
    <!-- Hero Banner Header -->
    <div class="bg-gradient-to-r from-teal-900 via-teal-800 to-slate-900 rounded-3xl p-8 md:p-12 text-white shadow-xl relative overflow-hidden">
      <div class="absolute inset-0 opacity-10 bg-[radial-gradient(#e5e7eb_1px,transparent_1px)] [background-size:16px_16px]"></div>
      <div class="absolute w-72 h-72 rounded-full bg-teal-500/10 blur-3xl -top-20 -right-20 pointer-events-none"></div>
      <div class="relative z-10 max-w-2xl space-y-4">
        <span class="text-xs font-black tracking-widest text-teal-300 uppercase bg-teal-800/40 px-3 py-1 rounded-full border border-teal-700/50">Hành Trình Khám Phá</span>
        <h1 class="text-3xl md:text-5xl font-black tracking-tight">Tuyến Tham Quan & Lịch Trình</h1>
        <p class="text-sm md:text-base text-teal-100/80 leading-relaxed font-medium">
          Khám phá những lịch trình du lịch được thiết kế sẵn theo chủ đề văn hóa, lịch sử, sinh thái sông nước để tối ưu hóa thời gian và trải nghiệm của bạn tại Cần Thơ.
        </p>
      </div>
    </div>

    <!-- Filter Bar -->
    <div class="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm flex flex-col md:flex-row gap-4 items-center justify-between">
      <!-- Search input -->
      <div class="relative w-full md:w-96">
        <SearchIcon class="absolute left-4 top-3.5 h-5 w-5 text-slate-400" />
        <input 
          v-model="filters.keyword"
          type="text" 
          placeholder="Tìm kiếm tuyến du lịch..." 
          class="w-full pl-11 pr-4 py-3 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm font-semibold transition focus:outline-none placeholder:text-slate-400"
          @input="handleFilterChange"
        />
      </div>

      <!-- Filters selectors -->
      <div class="flex flex-wrap gap-3 w-full md:w-auto items-center justify-end">
        <!-- Region Filter -->
        <select 
          v-model="filters.regionId" 
          class="px-4 py-3 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm font-bold transition focus:outline-none text-slate-700"
          @change="handleFilterChange"
        >
          <option :value="null">Tất cả khu vực</option>
          <option v-for="r in regions" :key="r.id" :value="r.id">{{ r.name }}</option>
        </select>

        <!-- Theme Filter -->
        <select 
          v-model="filters.theme" 
          class="px-4 py-3 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm font-bold transition focus:outline-none text-slate-700"
          @change="handleFilterChange"
        >
          <option :value="null">Tất cả chủ đề</option>
          <option value="culture">Văn hóa - Lịch sử</option>
          <option value="eco">Sinh thái sông nước</option>
          <option value="food">Ẩm thực địa phương</option>
          <option value="craft">Làng nghề truyền thống</option>
        </select>
      </div>
    </div>

    <!-- Routes Grid / Content States -->
    <div v-if="loading" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
      <div v-for="i in 3" :key="i" class="bg-white rounded-3xl border border-slate-100 shadow-sm overflow-hidden animate-pulse h-96">
        <div class="bg-slate-200 h-48"></div>
        <div class="p-6 space-y-4">
          <div class="h-4 bg-slate-200 rounded w-1/4"></div>
          <div class="h-6 bg-slate-200 rounded w-3/4"></div>
          <div class="h-4 bg-slate-200 rounded w-full"></div>
          <div class="h-10 bg-slate-200 rounded w-full mt-4"></div>
        </div>
      </div>
    </div>

    <div v-else-if="routes.length === 0" class="bg-white rounded-3xl border border-slate-200/80 shadow-sm p-12 text-center max-w-md mx-auto">
      <div class="w-16 h-16 bg-slate-50 rounded-2xl flex items-center justify-center mx-auto text-slate-400 border border-slate-100 mb-4">
        <RouteIcon class="w-8 h-8" />
      </div>
      <h3 class="text-lg font-bold text-slate-800">Không tìm thấy tuyến nào</h3>
      <p class="text-sm text-slate-500 mt-1">Hãy thử thay đổi từ khóa hoặc bộ lọc để khám phá các hành trình khác.</p>
    </div>

    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
      <!-- Route card -->
      <div 
        v-for="route in routes" 
        :key="route.id"
        class="group bg-white rounded-3xl border border-slate-200/60 hover:border-teal-500/30 hover:shadow-xl hover:shadow-teal-900/[0.03] overflow-hidden flex flex-col justify-between transition-all duration-300 transform hover:-translate-y-1"
      >
        <div>
          <!-- Thumbnail section -->
          <div class="relative h-52 w-full overflow-hidden bg-slate-100">
            <img 
              :src="route.thumbnailUrl || 'https://images.unsplash.com/photo-1507525428034-b723cf961d3e?auto=format&fit=crop&w=600&q=80'" 
              :alt="route.title" 
              class="w-full h-full object-cover group-hover:scale-105 transition duration-500"
            />
            <div class="absolute inset-0 bg-gradient-to-t from-slate-950/40 via-transparent to-transparent"></div>
            
            <!-- Category/Theme Badge -->
            <span class="absolute top-4 left-4 text-[10px] font-black tracking-wider uppercase bg-teal-500 text-white border border-teal-400 px-3 py-1 rounded-full shadow">
              {{ getThemeLabel(route.theme) }}
            </span>
          </div>

          <!-- Description content -->
          <div class="p-6 space-y-3">
            <div class="flex items-center space-x-2 text-xs font-bold text-slate-400">
              <span>{{ route.regionName }}</span>
              <span>•</span>
              <span>{{ route.destinationCount }} địa điểm</span>
            </div>
            <h3 class="text-lg font-black text-slate-800 leading-snug group-hover:text-teal-600 transition">
              {{ route.title }}
            </h3>
            <p class="text-xs text-slate-500 font-medium line-clamp-3 leading-relaxed">
              {{ route.description }}
            </p>
          </div>
        </div>

        <!-- Meta Footer -->
        <div class="px-6 pb-6 pt-4 border-t border-slate-50 flex items-center justify-between">
          <div class="flex items-center space-x-4">
            <div class="flex items-center space-x-1 text-slate-500">
              <ClockIcon class="w-4 h-4 text-slate-400" />
              <span class="text-xs font-bold">{{ route.estimatedDuration || 'N/A' }}</span>
            </div>
            <div class="flex items-center space-x-1 text-slate-500" v-if="route.distanceKm">
              <CompassIcon class="w-4 h-4 text-slate-400" />
              <span class="text-xs font-bold">{{ route.distanceKm }} km</span>
            </div>
          </div>
          <router-link 
            :to="`/routes/${route.slug}`"
            class="px-4 py-2 bg-teal-50 hover:bg-teal-100/80 text-teal-700 text-xs font-bold rounded-xl transition-all"
          >
            Xem tuyến
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import api from '../utils/api'
import { 
  Search as SearchIcon, 
  Map as RouteIcon, 
  Clock as ClockIcon, 
  Compass as CompassIcon 
} from 'lucide-vue-next'

const loading = ref(true)
const routes = ref([])
const regions = ref([])

const filters = reactive({
  keyword: '',
  regionId: null,
  theme: null
})

// Fetch all regions for filtering
const fetchRegions = async () => {
  try {
    const res = await api.get('/api/regions')
    if (res.success) {
      regions.value = res.data
    }
  } catch (err) {
    console.error('Error fetching regions:', err)
  }
}

// Fetch published routes list
const fetchRoutes = async () => {
  loading.value = true
  try {
    const params = {
      page: 1,
      pageSize: 50
    }
    if (filters.regionId) params.regionId = filters.regionId
    if (filters.theme) params.theme = filters.theme
    if (filters.keyword.trim()) params.keyword = filters.keyword.trim()

    const res = await api.get('/api/routes', { params })
    if (res.success) {
      routes.value = res.data.items
    }
  } catch (err) {
    console.error('Error fetching routes:', err)
  } finally {
    loading.value = false
  }
}

let timeout = null
const handleFilterChange = () => {
  clearTimeout(timeout)
  timeout = setTimeout(() => {
    fetchRoutes()
  }, 300)
}

const getThemeLabel = (theme) => {
  const themes = {
    culture: 'Văn hóa - Lịch sử',
    eco: 'Sinh thái sông nước',
    food: 'Ẩm thực địa phương',
    craft: 'Làng nghề'
  }
  return themes[theme] || 'Khám phá'
}

onMounted(() => {
  fetchRegions()
  fetchRoutes()
})
</script>
