<template>
  <div class="space-y-8 font-sans pb-12">
    <!-- Welcome Header banner -->
    <div class="bg-gradient-to-r from-teal-900 via-teal-800 to-slate-900 rounded-3xl p-8 text-white shadow-xl relative overflow-hidden">
      <div class="absolute inset-0 opacity-10 bg-[radial-gradient(#e5e7eb_1px,transparent_1px)] [background-size:16px_16px]"></div>
      <div class="absolute w-64 h-64 rounded-full bg-teal-500/10 blur-3xl -top-20 -right-20 pointer-events-none"></div>
      <div class="relative z-10 flex flex-col md:flex-row md:items-center md:justify-between gap-6">
        <div class="space-y-2">
          <h2 class="text-xl md:text-3xl font-black tracking-tight">Thống kê & Phân tích hành vi 📊</h2>
          <p class="text-xs md:text-sm text-teal-200/80 max-w-xl font-medium">
            Theo dõi và đánh giá hành vi tương tác ẩn danh của du khách để cải thiện chất lượng nội dung và các dịch vụ trải nghiệm số.
          </p>
        </div>
      </div>
    </div>

    <!-- Filters Bar -->
    <div class="bg-white p-6 rounded-2xl shadow-sm border border-slate-200/80 flex flex-col md:flex-row gap-6 items-end justify-between">
      <!-- Left Filters -->
      <div class="flex flex-wrap items-end gap-4 w-full md:w-auto">
        <!-- Date Preset Select -->
        <div class="space-y-1.5 w-full sm:w-44">
          <label class="text-[10px] font-black text-slate-400 uppercase tracking-wider block">Khoảng thời gian</label>
          <select 
            v-model="datePreset" 
            @change="onPresetChange"
            class="w-full px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-xs focus:outline-none transition font-semibold"
          >
            <option value="7days">7 ngày qua</option>
            <option value="30days">30 ngày qua</option>
            <option value="custom">Tùy chọn</option>
          </select>
        </div>

        <!-- Custom Date Pickers -->
        <Transition name="fade">
          <div v-if="datePreset === 'custom'" class="flex items-center gap-2 w-full sm:w-auto">
            <div class="space-y-1.5 w-full sm:w-36">
              <label class="text-[10px] font-black text-slate-400 uppercase tracking-wider block">Từ ngày</label>
              <input 
                type="date" 
                v-model="customFromDate"
                @change="fetchData"
                class="w-full px-3 py-2 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-xs focus:outline-none transition font-semibold"
              />
            </div>
            <span class="text-slate-400 text-xs mt-6">đến</span>
            <div class="space-y-1.5 w-full sm:w-36">
              <label class="text-[10px] font-black text-slate-400 uppercase tracking-wider block">Đến ngày</label>
              <input 
                type="date" 
                v-model="customToDate"
                @change="fetchData"
                class="w-full px-3 py-2 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-xs focus:outline-none transition font-semibold"
              />
            </div>
          </div>
        </Transition>

        <!-- Region Filter -->
        <div class="space-y-1.5 w-full sm:w-48">
          <label class="text-[10px] font-black text-slate-400 uppercase tracking-wider block">Khu vực (Chỉ lọc summary)</label>
          <select 
            v-model="selectedRegionId"
            @change="fetchData"
            class="w-full px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-xs focus:outline-none transition font-semibold text-slate-700"
          >
            <option :value="null">Tất cả khu vực</option>
            <option v-for="reg in regions" :key="reg.id" :value="reg.id">{{ reg.name }}</option>
          </select>
        </div>
      </div>

      <!-- Reset / Refresh Button -->
      <button 
        @click="fetchData"
        :disabled="loading"
        class="premium-btn w-full md:w-auto px-4 py-2.5 bg-slate-100 hover:bg-slate-200 text-slate-700 text-xs font-black rounded-xl border border-slate-200 flex items-center justify-center space-x-1.5 transition active:scale-95 disabled:opacity-50"
      >
        <RefreshIcon class="w-4 h-4 text-slate-500" :class="{ 'animate-spin': loading }" />
        <span>Làm mới số liệu</span>
      </button>
    </div>

    <!-- Error Alert -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-6 py-4 rounded-2xl flex items-center space-x-3 shadow-sm">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0 text-red-500" />
      <span class="text-sm font-bold">{{ error }}</span>
    </div>

    <!-- KPI Summary Grid -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-5">
      <!-- Total Views -->
      <div class="premium-card bg-white p-5 rounded-2xl shadow-sm border border-slate-200/80 flex items-center justify-between transition duration-200">
        <div class="space-y-1">
          <span class="text-[9px] font-black text-slate-400 uppercase tracking-wider block">Lượt xem Địa điểm</span>
          <div v-if="loading" class="h-6 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-2xl font-black text-slate-800">{{ summary.totalDestinationViews }}</p>
        </div>
        <div class="p-3 bg-teal-50 text-teal-600 rounded-xl border border-teal-100/50 shadow-inner flex-shrink-0">
          <MapPinIcon class="w-5 h-5" />
        </div>
      </div>

      <!-- Tour Opens -->
      <div class="premium-card bg-white p-5 rounded-2xl shadow-sm border border-slate-200/80 flex items-center justify-between transition duration-200">
        <div class="space-y-1">
          <span class="text-[9px] font-black text-slate-400 uppercase tracking-wider block">Lượt mở Tour ảo</span>
          <div v-if="loading" class="h-6 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-2xl font-black text-slate-800">{{ summary.totalVirtualTourOpens }}</p>
        </div>
        <div class="p-3 bg-amber-50 text-amber-600 rounded-xl border border-amber-100/50 shadow-inner flex-shrink-0">
          <CompassIcon class="w-5 h-5" />
        </div>
      </div>

      <!-- Panorama Navigations -->
      <div class="premium-card bg-white p-5 rounded-2xl shadow-sm border border-slate-200/80 flex items-center justify-between transition duration-200">
        <div class="space-y-1">
          <span class="text-[9px] font-black text-slate-400 uppercase tracking-wider block">Lượt đổi Panorama</span>
          <div v-if="loading" class="h-6 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-2xl font-black text-slate-800">{{ summary.totalPanoramaNavigations }}</p>
        </div>
        <div class="p-3 bg-indigo-50 text-indigo-600 rounded-xl border border-indigo-100/50 shadow-inner flex-shrink-0">
          <GlobeIcon class="w-5 h-5" />
        </div>
      </div>

      <!-- Hotspot Clicks -->
      <div class="premium-card bg-white p-5 rounded-2xl shadow-sm border border-slate-200/80 flex items-center justify-between transition duration-200">
        <div class="space-y-1">
          <span class="text-[9px] font-black text-slate-400 uppercase tracking-wider block">Lượt click Hotspot</span>
          <div v-if="loading" class="h-6 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-2xl font-black text-slate-800">{{ summary.totalHotspotClicks }}</p>
        </div>
        <div class="p-3 bg-rose-50 text-rose-600 rounded-xl border border-rose-100/50 shadow-inner flex-shrink-0">
          <MouseIcon class="w-5 h-5" />
        </div>
      </div>

      <!-- Route Views -->
      <div class="premium-card bg-white p-5 rounded-2xl shadow-sm border border-slate-200/80 flex items-center justify-between transition duration-200">
        <div class="space-y-1">
          <span class="text-[9px] font-black text-slate-400 uppercase tracking-wider block">Lượt xem Tuyến</span>
          <div v-if="loading" class="h-6 w-12 bg-slate-100 animate-pulse rounded"></div>
          <p v-else class="text-2xl font-black text-slate-800">{{ summary.totalRouteViews }}</p>
        </div>
        <div class="p-3 bg-emerald-50 text-emerald-600 rounded-xl border border-emerald-100/50 shadow-inner flex-shrink-0">
          <RouteIcon class="w-5 h-5" />
        </div>
      </div>
    </div>

    <!-- Chart: Events By Day -->
    <div class="bg-white p-6 md:p-8 rounded-3xl border border-slate-200/80 shadow-sm space-y-4">
      <div class="flex items-center justify-between border-b border-slate-100 pb-4">
        <div>
          <h3 class="text-sm font-black text-slate-800 uppercase tracking-wide">Tần suất hoạt động theo ngày</h3>
          <p class="text-[10px] text-slate-400 font-semibold uppercase">Tổng hợp tất cả các sự kiện tương tác của du khách</p>
        </div>
      </div>

      <div class="relative h-80 w-full">
        <!-- Loading overlay for chart -->
        <div v-if="loading" class="absolute inset-0 bg-white/70 flex items-center justify-center z-10 backdrop-blur-sm">
          <div class="flex flex-col items-center space-y-2">
            <RefreshIcon class="w-8 h-8 text-teal-600 animate-spin" />
            <span class="text-xs text-slate-400 font-bold">Đang tải biểu đồ...</span>
          </div>
        </div>

        <canvas ref="chartCanvas"></canvas>
      </div>
    </div>

    <!-- Top Lists Columns Layout Grid -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
      
      <!-- Top 1. Popular Destinations -->
      <div class="bg-white p-6 rounded-3xl border border-slate-200/80 shadow-sm flex flex-col min-h-[350px]">
        <div class="border-b border-slate-100 pb-4 mb-4 flex justify-between items-center">
          <div>
            <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider">Top 5 Địa điểm được xem nhiều</h3>
            <p class="text-[10px] text-slate-400">Xem trang chi tiết & bấm tour tương tác</p>
          </div>
          <MapPinIcon class="w-4 h-4 text-slate-400" />
        </div>

        <!-- Skeleton -->
        <div v-if="loading" class="space-y-4 flex-1">
          <div v-for="i in 5" :key="i" class="h-10 bg-slate-100 rounded-xl animate-pulse"></div>
        </div>

        <!-- Empty state -->
        <div v-else-if="topDestinations.length === 0" class="flex-grow flex flex-col items-center justify-center text-center py-12 space-y-2">
          <InboxIcon class="w-8 h-8 text-slate-350" />
          <p class="text-xs text-slate-400 font-semibold">Chưa có lượt tương tác nào</p>
        </div>

        <!-- Table list -->
        <div v-else class="flex-grow overflow-x-auto">
          <table class="w-full text-left border-collapse text-xs">
            <thead>
              <tr class="text-slate-400 font-black uppercase text-[9px] border-b border-slate-100">
                <th class="py-2.5">Địa điểm</th>
                <th class="py-2.5 text-right">Lượt xem</th>
                <th class="py-2.5 text-right">Lượt mở Tour</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-slate-50 font-semibold text-slate-700">
              <tr v-for="dest in topDestinations" :key="dest.destinationId" class="hover:bg-slate-50/50">
                <td class="py-3 font-extrabold text-slate-800">{{ dest.destinationName }}</td>
                <td class="py-3 text-right text-teal-600 font-black">{{ dest.viewCount }}</td>
                <td class="py-3 text-right text-amber-600 font-black">{{ dest.tourOpenCount }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Top 2. Popular Virtual Tours -->
      <div class="bg-white p-6 rounded-3xl border border-slate-200/80 shadow-sm flex flex-col min-h-[350px]">
        <div class="border-b border-slate-100 pb-4 mb-4 flex justify-between items-center">
          <div>
            <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider">Top 5 Tour ảo 360°</h3>
            <p class="text-[10px] text-slate-400">Xem Tour 360° & Chuyển scene panorama</p>
          </div>
          <CompassIcon class="w-4 h-4 text-slate-400" />
        </div>

        <!-- Skeleton -->
        <div v-if="loading" class="space-y-4 flex-1">
          <div v-for="i in 5" :key="i" class="h-10 bg-slate-100 rounded-xl animate-pulse"></div>
        </div>

        <!-- Empty state -->
        <div v-else-if="topTours.length === 0" class="flex-grow flex flex-col items-center justify-center text-center py-12 space-y-2">
          <InboxIcon class="w-8 h-8 text-slate-350" />
          <p class="text-xs text-slate-400 font-semibold">Chưa có lượt tương tác nào</p>
        </div>

        <!-- Table list -->
        <div v-else class="flex-grow overflow-x-auto">
          <table class="w-full text-left border-collapse text-xs">
            <thead>
              <tr class="text-slate-400 font-black uppercase text-[9px] border-b border-slate-100">
                <th class="py-2.5">Tên Tour</th>
                <th class="py-2.5 text-right">Lượt mở</th>
                <th class="py-2.5 text-right">Lượt chuyển Panorama</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-slate-50 font-semibold text-slate-700">
              <tr v-for="t in topTours" :key="t.tourId" class="hover:bg-slate-50/50">
                <td class="py-3 font-extrabold text-slate-800">{{ t.tourTitle }}</td>
                <td class="py-3 text-right text-amber-600 font-black">{{ t.openCount }}</td>
                <td class="py-3 text-right text-indigo-600 font-black">{{ t.panoramaNavigationCount }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Top 3. Popular Hotspots -->
      <div class="bg-white p-6 rounded-3xl border border-slate-200/80 shadow-sm flex flex-col min-h-[350px]">
        <div class="border-b border-slate-100 pb-4 mb-4 flex justify-between items-center">
          <div>
            <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider">Top 5 Điểm tương tác (Hotspot)</h3>
            <p class="text-[10px] text-slate-400">Click vào hotspot thông tin/audio/video</p>
          </div>
          <MouseIcon class="w-4 h-4 text-slate-400" />
        </div>

        <!-- Skeleton -->
        <div v-if="loading" class="space-y-4 flex-1">
          <div v-for="i in 5" :key="i" class="h-10 bg-slate-100 rounded-xl animate-pulse"></div>
        </div>

        <!-- Empty state -->
        <div v-else-if="topHotspots.length === 0" class="flex-grow flex flex-col items-center justify-center text-center py-12 space-y-2">
          <InboxIcon class="w-8 h-8 text-slate-350" />
          <p class="text-xs text-slate-400 font-semibold">Chưa có lượt tương tác nào</p>
        </div>

        <!-- Table list -->
        <div v-else class="flex-grow overflow-x-auto">
          <table class="w-full text-left border-collapse text-xs">
            <thead>
              <tr class="text-slate-400 font-black uppercase text-[9px] border-b border-slate-100">
                <th class="py-2.5">Hotspot</th>
                <th class="py-2.5">Phân loại</th>
                <th class="py-2.5 text-right">Lượt click</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-slate-50 font-semibold text-slate-700">
              <tr v-for="h in topHotspots" :key="h.hotspotId" class="hover:bg-slate-50/50">
                <td class="py-3 font-extrabold text-slate-800">{{ h.hotspotTitle || 'Chuyển cảnh' }}</td>
                <td class="py-3 text-slate-500 uppercase text-[10px] tracking-wider">{{ h.hotspotType }}</td>
                <td class="py-3 text-right text-rose-600 font-black">{{ h.clickCount }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Top 4. Popular Routes -->
      <div class="bg-white p-6 rounded-3xl border border-slate-200/80 shadow-sm flex flex-col min-h-[350px]">
        <div class="border-b border-slate-100 pb-4 mb-4 flex justify-between items-center">
          <div>
            <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider">Top 5 Tuyến tham quan nổi bật</h3>
            <p class="text-[10px] text-slate-400">Xem chi tiết & click xem địa điểm trong tuyến</p>
          </div>
          <RouteIcon class="w-4 h-4 text-slate-400" />
        </div>

        <!-- Skeleton -->
        <div v-if="loading" class="space-y-4 flex-1">
          <div v-for="i in 5" :key="i" class="h-10 bg-slate-100 rounded-xl animate-pulse"></div>
        </div>

        <!-- Empty state -->
        <div v-else-if="topRoutes.length === 0" class="flex-grow flex flex-col items-center justify-center text-center py-12 space-y-2">
          <InboxIcon class="w-8 h-8 text-slate-350" />
          <p class="text-xs text-slate-400 font-semibold">Chưa có lượt tương tác nào</p>
        </div>

        <!-- Table list -->
        <div v-else class="flex-grow overflow-x-auto">
          <table class="w-full text-left border-collapse text-xs">
            <thead>
              <tr class="text-slate-400 font-black uppercase text-[9px] border-b border-slate-100">
                <th class="py-2.5">Tên tuyến</th>
                <th class="py-2.5 text-right">Lượt xem tuyến</th>
                <th class="py-2.5 text-right">Click điểm dừng</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-slate-50 font-semibold text-slate-700">
              <tr v-for="r in topRoutes" :key="r.routeId" class="hover:bg-slate-50/50">
                <td class="py-3 font-extrabold text-slate-800">{{ r.routeTitle }}</td>
                <td class="py-3 text-right text-emerald-600 font-black">{{ r.viewCount }}</td>
                <td class="py-3 text-right text-teal-600 font-black">{{ r.destinationClickCount }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import api from '../../utils/api'
import { Chart, registerables } from 'chart.js'
import {
  MapPin as MapPinIcon,
  Compass as CompassIcon,
  Globe as GlobeIcon,
  MousePointerClick as MouseIcon,
  Route as RouteIcon,
  AlertCircle as AlertCircleIcon,
  RefreshCw as RefreshIcon,
  Inbox as InboxIcon
} from 'lucide-vue-next'

// Register all Chart.js modules
Chart.register(...registerables)

const loading = ref(false)
const error = ref(null)

// Filters State
const datePreset = ref('7days')
const customFromDate = ref('')
const customToDate = ref('')
const selectedRegionId = ref(null)
const regions = ref([])

// Analytics Data State
const summary = ref({
  totalEvents: 0,
  totalDestinationViews: 0,
  totalVirtualTourOpens: 0,
  totalHotspotClicks: 0,
  totalPanoramaNavigations: 0,
  totalRouteViews: 0
})

const topDestinations = ref([])
const topTours = ref([])
const topHotspots = ref([])
const topRoutes = ref([])
const eventsByDay = ref([])

// Chart.js references
const chartCanvas = ref(null)
let chartInstance = null

// Compute dates based on preset
const getQueryDates = () => {
  const dates = { fromDate: null, toDate: null }
  const now = new Date()

  if (datePreset.value === '7days') {
    const from = new Date()
    from.setDate(now.getDate() - 7)
    dates.fromDate = from.toISOString().split('T')[0]
    dates.toDate = now.toISOString().split('T')[0]
  } else if (datePreset.value === '30days') {
    const from = new Date()
    from.setDate(now.getDate() - 30)
    dates.fromDate = from.toISOString().split('T')[0]
    dates.toDate = now.toISOString().split('T')[0]
  } else if (datePreset.value === 'custom') {
    dates.fromDate = customFromDate.value || null
    dates.toDate = customToDate.value || null
  }
  return dates
}

const onPresetChange = () => {
  if (datePreset.value === 'custom') {
    // Default to last 7 days custom
    const now = new Date()
    const from = new Date()
    from.setDate(now.getDate() - 7)
    customFromDate.value = from.toISOString().split('T')[0]
    customToDate.value = now.toISOString().split('T')[0]
  }
  fetchData()
}

// Fetch region lists
const fetchRegions = async () => {
  try {
    const res = await api.get('/api/admin/regions')
    if (res.success) {
      regions.value = res.data
    }
  } catch (err) {
    console.error('Không thể tải danh sách khu vực.', err)
  }
}

// Main fetch function executing API calls in parallel
const fetchData = async () => {
  loading.value = true
  error.value = null
  const { fromDate, toDate } = getQueryDates()

  const params = {
    fromDate: fromDate || undefined,
    toDate: toDate || undefined,
    regionId: selectedRegionId.value || undefined
  }

  // Top lists params do not take regionId (as region filter only applies to global summary)
  const topParams = {
    fromDate: fromDate || undefined,
    toDate: toDate || undefined,
    limit: 5
  }

  try {
    const [summaryRes, dailyRes, destRes, toursRes, hotspotsRes, routesRes] = await Promise.all([
      api.get('/api/admin/analytics/summary', { params }),
      api.get('/api/admin/analytics/events-by-day', { params: { fromDate, toDate } }),
      api.get('/api/admin/analytics/top-destinations', { params: topParams }),
      api.get('/api/admin/analytics/top-tours', { params: topParams }),
      api.get('/api/admin/analytics/top-hotspots', { params: topParams }),
      api.get('/api/admin/analytics/top-routes', { params: topParams })
    ])

    if (summaryRes.success) summary.value = summaryRes.data
    if (dailyRes.success) {
      eventsByDay.value = dailyRes.data
      renderChart(dailyRes.data)
    }
    if (destRes.success) topDestinations.value = destRes.data
    if (toursRes.success) topTours.value = toursRes.data
    if (hotspotsRes.success) topHotspots.value = hotspotsRes.data
    if (routesRes.success) topRoutes.value = routesRes.data
  } catch (err) {
    error.value = err.message || 'Có lỗi hệ thống xảy ra khi lấy số liệu thống kê.'
  } finally {
    loading.value = false
  }
}

// Render chart using Chart.js
const renderChart = (dailyData) => {
  if (chartInstance) {
    chartInstance.destroy()
  }

  if (!chartCanvas.value) return

  const ctx = chartCanvas.value.getContext('2d')
  
  // Create beautiful gradient background for lines
  const gradient = ctx.createLinearGradient(0, 0, 0, 300)
  gradient.addColorStop(0, 'rgba(20, 184, 166, 0.4)')
  gradient.addColorStop(1, 'rgba(20, 184, 166, 0.0)')

  // If dailyData is empty, generate last 7 days placeholder dates with count 0
  let chartLabels = dailyData.map(d => {
    // Format YYYY-MM-DD to DD/MM
    const parts = d.date.split('-')
    return parts.length === 3 ? `${parts[2]}/${parts[1]}` : d.date
  })
  let chartValues = dailyData.map(d => d.count)

  if (dailyData.length === 0) {
    const { fromDate, toDate } = getQueryDates()
    const start = fromDate ? new Date(fromDate) : new Date()
    const end = toDate ? new Date(toDate) : new Date()
    
    // Fill values with 0
    const diffDays = Math.ceil((end - start) / (1000 * 60 * 60 * 24)) || 7
    chartLabels = []
    chartValues = []
    for (let i = 0; i <= diffDays; i++) {
      const temp = new Date(start)
      temp.setDate(start.getDate() + i)
      chartLabels.push(`${temp.getDate()}/${temp.getMonth() + 1}`)
      chartValues.push(0)
    }
  }

  chartInstance = new Chart(ctx, {
    type: 'line',
    data: {
      labels: chartLabels,
      datasets: [
        {
          label: 'Tổng lượt tương tác',
          data: chartValues,
          borderColor: '#14b8a6', // teal-500
          borderWidth: 3,
          pointBackgroundColor: '#14b8a6',
          pointBorderColor: '#ffffff',
          pointBorderWidth: 2,
          pointRadius: 4,
          pointHoverRadius: 6,
          backgroundColor: gradient,
          fill: true,
          tension: 0.3
        }
      ]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: false
        },
        tooltip: {
          backgroundColor: '#0f172a',
          titleFont: { size: 11, weight: 'bold', family: 'font-sans' },
          bodyFont: { size: 12, family: 'font-sans' },
          padding: 10,
          cornerRadius: 12,
          displayColors: false
        }
      },
      scales: {
        x: {
          grid: {
            display: false
          },
          ticks: {
            font: { size: 10, weight: 'bold', family: 'font-sans' },
            color: '#64748b'
          }
        },
        y: {
          grid: {
            color: '#f1f5f9'
          },
          ticks: {
            font: { size: 10, weight: 'bold', family: 'font-sans' },
            color: '#64748b',
            stepSize: 1
          },
          beginAtZero: true
        }
      }
    }
  })
}

onBeforeUnmount(() => {
  if (chartInstance) {
    chartInstance.destroy()
  }
})

onMounted(() => {
  fetchRegions()
  fetchData()
})
</script>

<style scoped>
.premium-card {
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}
.premium-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 20px -5px rgba(15, 23, 42, 0.08);
}
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
