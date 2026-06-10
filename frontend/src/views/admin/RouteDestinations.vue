<template>
  <div class="max-w-7xl mx-auto space-y-8 font-sans">
    <!-- Back Header -->
    <div class="flex items-center">
      <router-link to="/admin/routes" class="inline-flex items-center space-x-2 text-slate-500 hover:text-teal-600 font-bold text-xs transition">
        <ArrowLeftIcon class="w-4 h-4" />
        <span>Quay lại danh sách tuyến</span>
      </router-link>
    </div>

    <!-- Header info -->
    <div v-if="route" class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 border-b border-slate-100 pb-5">
      <div class="space-y-1">
        <span class="text-[10px] font-black tracking-widest text-teal-600 uppercase bg-teal-50 border border-teal-100 px-3 py-1 rounded-full">{{ route.regionName }}</span>
        <h2 class="text-xl md:text-2xl font-black text-slate-800 tracking-tight">Cấu hình địa điểm: {{ route.title }}</h2>
        <p class="text-xs md:text-sm text-slate-400 font-medium">Thêm, xóa và sắp xếp thứ tự tham quan các địa điểm trong tuyến.</p>
      </div>
      <div class="flex items-center space-x-3 text-xs font-bold text-slate-500 bg-slate-100 px-4 py-2.5 rounded-xl border border-slate-200/50">
        <span>Tình trạng:</span>
        <span :class="route.status === 'published' ? 'text-emerald-600' : 'text-slate-500'">
          {{ route.status === 'published' ? 'Đã xuất bản' : 'Bản nháp' }}
        </span>
      </div>
    </div>

    <!-- Error Alert -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-xl flex items-center space-x-3 shadow-sm">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0 text-red-500" />
      <span class="text-sm font-bold">{{ error }}</span>
    </div>

    <div v-if="loading" class="text-center py-12 text-slate-400">
      <div class="w-6 h-6 border-2 border-teal-500 border-t-transparent rounded-full animate-spin mx-auto mb-2"></div>
      <span class="text-sm font-semibold">Đang tải dữ liệu tuyến...</span>
    </div>

    <div v-else-if="route" class="grid grid-cols-1 lg:grid-cols-12 gap-8">
      <!-- Left Panel: Available Destinations (5/12 columns) -->
      <div class="lg:col-span-5 bg-white p-6 rounded-3xl border border-slate-200/80 shadow-sm flex flex-col h-[650px]">
        <div class="space-y-4 pb-4 border-b border-slate-100 flex-shrink-0">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider">Danh sách địa điểm trong khu vực</h3>
          <div class="relative">
            <SearchIcon class="absolute left-3.5 top-3 h-4 w-4 text-slate-400" />
            <input 
              v-model="destKeyword"
              type="text" 
              placeholder="Tìm kiếm địa điểm du lịch..." 
              class="w-full pl-9 pr-4 py-2 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-xs font-semibold transition focus:outline-none placeholder:text-slate-400"
            />
          </div>
        </div>

        <!-- Scrollable Destinations List -->
        <div class="flex-grow overflow-y-auto pt-4 space-y-3 pr-2">
          <div v-if="filteredDestinations.length === 0" class="text-center py-12 text-slate-400 text-xs">
            Không tìm thấy địa điểm nào.
          </div>
          <div 
            v-else 
            v-for="d in filteredDestinations" 
            :key="d.id"
            class="flex items-center justify-between p-3 rounded-xl border border-slate-100 bg-slate-50/50 hover:bg-slate-50 transition"
          >
            <div class="flex items-center space-x-3 min-w-0">
              <div class="w-12 h-8 rounded overflow-hidden bg-slate-200 flex-shrink-0">
                <img :src="d.coverImageUrl" class="w-full h-full object-cover" v-if="d.coverImageUrl" />
              </div>
              <div class="min-w-0">
                <p class="text-xs font-bold text-slate-700 truncate leading-snug">{{ d.name }}</p>
                <p class="text-[9px] text-slate-400 font-bold uppercase tracking-wider">{{ d.category?.name || 'Chưa phân loại' }}</p>
              </div>
            </div>

            <button 
              type="button"
              @click="addDestination(d.id)"
              class="p-1.5 bg-teal-50 hover:bg-teal-100 text-teal-700 border border-teal-200/50 rounded-lg transition"
              title="Thêm vào tuyến"
            >
              <PlusIcon class="w-4 h-4" />
            </button>
          </div>
        </div>
      </div>

      <!-- Right Panel: Configured Itinerary (7/12 columns) -->
      <div class="lg:col-span-7 bg-white p-6 rounded-3xl border border-slate-200/80 shadow-sm flex flex-col h-[650px]">
        <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider pb-4 border-b border-slate-100 flex-shrink-0">
          Địa điểm thuộc tuyến (Tổng số: {{ route.destinations.length }})
        </h3>

        <!-- Scrollable Ordered List -->
        <div class="flex-grow overflow-y-auto pt-4 space-y-4 pr-2">
          <div v-if="route.destinations.length === 0" class="text-center py-12 text-slate-400 text-xs">
            Tuyến tham quan này chưa có địa điểm nào. Hãy thêm từ danh mục bên trái.
          </div>
          <div 
            v-else 
            v-for="(dest, index) in route.destinations" 
            :key="dest.id"
            class="p-4 bg-slate-50/50 rounded-2xl border border-slate-200/40 space-y-3"
          >
            <div class="flex items-center justify-between">
              <div class="flex items-center space-x-3 min-w-0">
                <div class="w-6 h-6 rounded-full bg-teal-600 text-white font-extrabold flex items-center justify-center text-[10px]">
                  {{ index + 1 }}
                </div>
                <div class="min-w-0">
                  <h4 class="text-xs font-bold text-slate-800 truncate">{{ dest.name }}</h4>
                  <span class="text-[9px] text-slate-400 font-bold uppercase">{{ dest.categoryName }}</span>
                </div>
              </div>

              <!-- Ordering and Delete actions -->
              <div class="flex items-center space-x-1">
                <button 
                  type="button"
                  :disabled="index === 0"
                  class="p-1 text-slate-500 hover:bg-slate-200/60 rounded disabled:opacity-30"
                  @click="moveStep(index, -1)"
                >
                  <ArrowUpIcon class="w-4 h-4" />
                </button>
                <button 
                  type="button"
                  :disabled="index === route.destinations.length - 1"
                  class="p-1 text-slate-500 hover:bg-slate-200/60 rounded disabled:opacity-30"
                  @click="moveStep(index, 1)"
                >
                  <ArrowDownIcon class="w-4 h-4" />
                </button>
                <button 
                  type="button"
                  class="p-1 text-rose-600 hover:bg-rose-50 rounded"
                  @click="removeDestination(dest.id)"
                >
                  <TrashIcon class="w-4 h-4" />
                </button>
              </div>
            </div>

            <!-- Stay duration & Note inputs -->
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-3 pt-1">
              <div>
                <label class="block text-[9px] font-bold text-slate-400 uppercase tracking-wider mb-1">Thời gian lưu trú</label>
                <input 
                  v-model="dest.estimatedStayTime"
                  type="text" 
                  placeholder="Ví dụ: 45 phút, 1 giờ"
                  class="w-full px-2.5 py-1.5 bg-white border border-slate-200 focus:border-teal-500 rounded-lg text-xs focus:outline-none focus:ring-2 focus:ring-teal-500/5 transition font-semibold"
                  @blur="saveDestinationConfig(dest)"
                />
              </div>
              <div>
                <label class="block text-[9px] font-bold text-slate-400 uppercase tracking-wider mb-1">Ghi chú hành trình</label>
                <div class="relative">
                  <input 
                    v-model="dest.note"
                    type="text" 
                    placeholder="Lưu ý riêng tại điểm dừng..."
                    class="w-full pl-2.5 pr-8 py-1.5 bg-white border border-slate-200 focus:border-teal-500 rounded-lg text-xs focus:outline-none focus:ring-2 focus:ring-teal-500/5 transition font-semibold"
                    @blur="saveDestinationConfig(dest)"
                  />
                  <!-- Disk save icon showing status -->
                  <button 
                    type="button"
                    @click="saveDestinationConfig(dest)"
                    class="absolute right-2 top-2 text-slate-400 hover:text-teal-600 transition"
                    title="Lưu thông tin"
                  >
                    <SaveIcon class="w-3.5 h-3.5" />
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Live Preview map container -->
    <div v-if="route && route.destinations.length > 0" class="bg-white p-6 rounded-3xl border border-slate-200/80 shadow-sm space-y-4">
      <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-3">Xem trước bản đồ tuyến</h3>
      <div ref="mapContainer" class="h-80 rounded-2xl overflow-hidden border border-slate-200 z-10"></div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, nextTick } from 'vue'
import { useRoute } from 'vue-router'
import api from '../../utils/api'
import L from 'leaflet'
import { 
  ArrowLeft as ArrowLeftIcon, 
  AlertCircle as AlertCircleIcon,
  Search as SearchIcon, 
  Plus as PlusIcon, 
  Trash as TrashIcon,
  ArrowUp as ArrowUpIcon,
  ArrowDown as ArrowDownIcon,
  Save as SaveIcon
} from 'lucide-vue-next'

const routeQuery = useRoute()
const routeId = routeQuery.params.id

const loading = ref(true)
const error = ref(null)
const route = ref(null)
const allDestinations = ref([])
const destKeyword = ref('')

const mapContainer = ref(null)
let map = null
let polyline = null
const markers = []

// Redefine Leaflet default icon paths to prevent broken marker images under Vite/bundler
delete L.Icon.Default.prototype._getIconUrl
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png',
  iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
  shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png'
})

// Filter out available destinations in the region that are not yet in the route
const filteredDestinations = computed(() => {
  if (!route.value) return []
  const routeDestIds = route.value.destinations.map(rd => rd.destinationId)
  
  let list = allDestinations.value.filter(d => d.regionId === route.value.regionId && !routeDestIds.includes(d.id))
  
  if (destKeyword.value.trim()) {
    const kw = destKeyword.value.toLowerCase().trim()
    list = list.filter(d => d.name.toLowerCase().includes(kw))
  }
  return list
})

// Fetch all routes and all destinations
const fetchRouteDetail = async () => {
  loading.value = true
  error.value = null
  try {
    const res = await api.get(`/api/admin/routes/${routeId}`)
    if (res.success) {
      route.value = res.data
      
      // Fetch destinations for selection
      const destRes = await api.get('/api/destinations', {
        params: {
          regionId: route.value.regionId,
          pageSize: 100
        }
      })
      if (destRes.success) {
        allDestinations.value = destRes.data.items
      }
    }
  } catch (err) {
    error.value = err.message || 'Lỗi tải dữ liệu tuyến tham quan.'
  } finally {
    loading.value = false
  }

  if (route.value) {
    nextTick(() => {
      updatePreviewMap()
    })
  }
}

// Add destination to route
const addDestination = async (destId) => {
  error.value = null
  try {
    const nextOrder = route.value.destinations.length + 1
    const res = await api.post(`/api/admin/routes/${routeId}/destinations/bulk`, {
      items: [
        {
          destinationId: destId,
          displayOrder: nextOrder,
          estimatedStayTime: '45 phút',
          note: ''
        }
      ]
    })
    if (res.success) {
      route.value = res.data
      nextTick(() => {
        updatePreviewMap()
      })
    }
  } catch (err) {
    error.value = err.message || 'Không thể thêm địa điểm vào tuyến.'
  }
}

// Remove destination from route
const removeDestination = async (rdId) => {
  error.value = null
  try {
    const res = await api.delete(`/api/admin/routes/${routeId}/destinations/${rdId}`)
    if (res.success) {
      route.value = res.data
      nextTick(() => {
        updatePreviewMap()
      })
    }
  } catch (err) {
    error.value = err.message || 'Không thể xóa địa điểm khỏi tuyến.'
  }
}

// Update note and duration
const saveDestinationConfig = async (dest) => {
  try {
    const res = await api.put(`/api/admin/routes/${routeId}/destinations/${dest.id}`, {
      displayOrder: dest.displayOrder,
      estimatedStayTime: dest.estimatedStayTime,
      note: dest.note
    })
    if (res.success) {
      route.value = res.data
    }
  } catch (err) {
    error.value = err.message || 'Không thể lưu ghi chú địa điểm.'
  }
}

// Reorder destinations using move step
const moveStep = async (index, direction) => {
  error.value = null
  const items = JSON.parse(JSON.stringify(route.value.destinations))
  const temp = items[index]
  items[index] = items[index + direction]
  items[index + direction] = temp

  // Map displayOrder
  const reorderPayload = items.map((item, idx) => ({
    routeDestinationId: item.id,
    displayOrder: idx + 1
  }))

  try {
    const res = await api.patch(`/api/admin/routes/${routeId}/destinations/reorder`, {
      items: reorderPayload
    })
    if (res.success) {
      route.value = res.data
      nextTick(() => {
        updatePreviewMap()
      })
    }
  } catch (err) {
    error.value = err.message || 'Không thể sắp xếp lại vị trí địa điểm.'
  }
}

// Update preview map
const updatePreviewMap = () => {
  if (!mapContainer.value || !route.value) return

  // Cleanup old layers
  if (map) {
    markers.forEach(m => map.removeLayer(m))
    markers.length = 0
    if (polyline) {
      map.removeLayer(polyline)
      polyline = null
    }
  } else {
    // Instantiate map
    map = L.map(mapContainer.value).setView([10.033, 105.78], 13)
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(map)
  }

  const latlngs = []

  route.value.destinations.forEach((dest, index) => {
    const lat = parseFloat(dest.latitude)
    const lng = parseFloat(dest.longitude)
    if (isNaN(lat) || isNaN(lng)) return

    latlngs.push([lat, lng])

    const icon = L.divIcon({
      html: `<div class="w-8 h-8 rounded-full bg-teal-600 text-white font-extrabold flex items-center justify-center border-2 border-white shadow-md">${index + 1}</div>`,
      className: 'custom-numbered-marker-wrapper',
      iconSize: [32, 32],
      iconAnchor: [16, 16]
    })

    const marker = L.marker([lat, lng], { icon }).addTo(map)
      .bindPopup(`<div class="font-sans font-bold text-xs">${dest.name}</div>`)
    
    markers.push(marker)
  })

  if (latlngs.length > 0) {
    const bounds = L.latLngBounds(latlngs)
    map.fitBounds(bounds, { padding: [30, 30] })

    polyline = L.polyline(latlngs, {
      color: '#0d9488',
      weight: 3.5,
      dashArray: '6, 6',
      opacity: 0.8
    }).addTo(map)
  }
}

onMounted(() => {
  fetchRouteDetail()
})
</script>

<style>
.custom-numbered-marker-wrapper {
  overflow: visible !important;
}
</style>
