<template>
  <div class="h-screen flex flex-col overflow-hidden bg-slate-50">
    <!-- Header -->
    <PublicHeader />

    <!-- Split Workspace -->
    <div class="flex-grow flex flex-col md:flex-row overflow-hidden relative">
      
      <!-- Sidebar Panel Left (30%) -->
      <aside class="w-full md:w-80 lg:w-96 bg-white border-r border-slate-200 flex flex-col z-20 flex-shrink-0 shadow-sm">
        <!-- Sidebar Search Form -->
        <div class="p-4 border-b border-slate-100 space-y-3">
          <div class="relative">
            <input 
              v-model="searchKeyword"
              type="text" 
              placeholder="Tìm kiếm địa điểm..." 
              class="w-full pl-9 pr-4 py-2 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white rounded-xl text-xs focus:outline-none transition"
              @input="onFilterChange"
            />
            <span class="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400">
              <SearchIcon class="w-4 h-4" />
            </span>
          </div>

          <!-- Feature filter toggles -->
          <div class="flex items-center justify-between text-xs px-1">
            <span class="text-slate-500 font-bold">Chỉ có Tour ảo 360°</span>
            <button 
              @click="toggleTourFilter"
              :class="[
                'w-9 h-5 rounded-full p-0.5 transition duration-200 focus:outline-none',
                filterHasTour ? 'bg-teal-500' : 'bg-slate-200'
              ]"
            >
              <div 
                :class="[
                  'w-4 h-4 bg-white rounded-full shadow-sm transform duration-200',
                  filterHasTour ? 'translate-x-4' : 'translate-x-0'
                ]"
              ></div>
            </button>
          </div>
        </div>

        <!-- Scrollable Sidebar lists -->
        <div class="flex-grow overflow-y-auto p-4 space-y-4">
          <!-- Categories Filter Chips list -->
          <div class="space-y-2">
            <p class="text-[10px] font-bold text-slate-400 uppercase tracking-wider">Danh mục</p>
            <div class="flex flex-wrap gap-1.5">
              <button 
                @click="selectCategoryChip(null)"
                :class="[
                  'px-2.5 py-1 rounded-lg text-[10px] font-bold transition border',
                  selectedCategory === null 
                    ? 'bg-teal-600 border-teal-600 text-white shadow-sm' 
                    : 'bg-slate-50 border-slate-200 text-slate-600 hover:border-slate-350'
                ]"
              >
                Tất cả
              </button>
              <button 
                v-for="cat in categories" 
                :key="cat.id"
                @click="selectCategoryChip(cat.id)"
                :class="[
                  'px-2.5 py-1 rounded-lg text-[10px] font-bold transition border',
                  selectedCategory === cat.id 
                    ? 'bg-teal-600 border-teal-600 text-white shadow-sm' 
                    : 'bg-slate-50 border-slate-200 text-slate-600 hover:border-slate-350'
                ]"
              >
                {{ cat.name }}
              </button>
            </div>
          </div>

          <!-- Destination lists scroll -->
          <div class="space-y-3 pt-2">
            <p class="text-[10px] font-bold text-slate-400 uppercase tracking-wider">
              Kết quả tìm kiếm ({{ destinations.length }})
            </p>

            <!-- Loading Skeleton -->
            <div v-if="loading" class="space-y-3">
              <div v-for="i in 3" :key="i" class="flex items-center space-x-3 p-2 bg-slate-50 rounded-xl animate-pulse">
                <div class="w-16 h-12 bg-slate-100 rounded-lg flex-shrink-0"></div>
                <div class="space-y-1.5 flex-1">
                  <div class="h-3.5 bg-slate-100 rounded w-3/4"></div>
                  <div class="h-2.5 bg-slate-100 rounded w-1/2"></div>
                </div>
              </div>
            </div>

            <!-- Empty Results -->
            <div v-else-if="destinations.length === 0" class="text-center py-12 space-y-2">
              <InboxIcon class="w-8 h-8 mx-auto text-slate-350" />
              <p class="text-xs font-bold text-slate-500">Không tìm thấy địa điểm nào</p>
              <p class="text-[10px] text-slate-400">Thử đặt lại bộ lọc hoặc điều chỉnh từ khóa tìm kiếm</p>
            </div>

            <!-- List Results Cards -->
            <div v-else class="space-y-2.5">
              <div 
                v-for="dest in destinations" 
                :key="dest.id"
                class="flex space-x-3 p-2.5 bg-slate-50 hover:bg-teal-50/40 border border-slate-200/50 hover:border-teal-500/20 rounded-xl cursor-pointer transition group"
                @click="focusDestination(dest)"
              >
                <!-- Thumbnail -->
                <div class="w-20 h-14 bg-slate-100 border border-slate-200/50 rounded-lg overflow-hidden flex-shrink-0 relative">
                  <img v-if="dest.coverImageUrl" :src="dest.coverImageUrl" class="w-full h-full object-cover" />
                  <ImageIcon v-else class="w-5 h-5 text-slate-400 absolute inset-0 m-auto" />
                </div>
                
                <!-- Info text -->
                <div class="min-w-0 flex-1 space-y-1 flex flex-col justify-between">
                  <div>
                    <h4 class="text-xs font-bold text-slate-800 truncate group-hover:text-teal-700 transition">
                      {{ dest.name }}
                    </h4>
                    <span class="text-[9px] text-slate-400 block font-semibold uppercase tracking-wider">
                      {{ dest.category?.name }} | {{ dest.regionName }}
                    </span>
                  </div>
                  
                  <!-- Has indicators -->
                  <div class="flex items-center space-x-2">
                    <span 
                      v-if="dest.hasVirtualTour" 
                      class="px-1.5 py-0.5 bg-teal-50 text-teal-600 border border-teal-100 rounded text-[8px] font-bold uppercase tracking-wider"
                    >
                      360° Pano
                    </span>
                    <span 
                      v-if="dest.hasAudioGuide" 
                      class="px-1.5 py-0.5 bg-amber-50 text-amber-600 border border-amber-100 rounded text-[8px] font-bold uppercase tracking-wider"
                    >
                      Audio
                    </span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </aside>

      <!-- Leaflet Map Panel Right (70%) -->
      <main class="flex-grow h-full w-full z-10 relative">
        <div ref="mapContainer" class="w-full h-full bg-slate-100"></div>
        
        <!-- Center/Reset button overlay on map -->
        <button 
          @click="resetMapZoom" 
          class="absolute bottom-5 right-5 z-20 p-2.5 bg-white border border-slate-200 rounded-xl text-slate-700 shadow-md hover:bg-slate-50 transition active:scale-95 flex items-center justify-center"
          title="Đặt lại bản đồ"
        >
          <CompassIcon class="w-5 h-5 text-teal-600" />
        </button>
      </main>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, nextTick } from 'vue'
import { useRouter } from 'vue-router'
import api from '../utils/api'
import PublicHeader from '../components/public/PublicHeader.vue'
import L from 'leaflet'
import { 
  Search as SearchIcon, 
  Image as ImageIcon, 
  Inbox as InboxIcon, 
  Compass as CompassIcon 
} from 'lucide-vue-next'

// Redefine Leaflet default icon paths to prevent broken marker images under Vite/bundler
delete L.Icon.Default.prototype._getIconUrl
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png',
  iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
  shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png'
})

const router = useRouter()

const loading = ref(false)
const error = ref(null)

const destinations = ref([])
const categories = ref([])

const searchKeyword = ref('')
const selectedCategory = ref(null)
const filterHasTour = ref(false)

const mapContainer = ref(null)
let map = null
let markersGroup = null
const markersMap = new Map() // ID to marker link

const fetchCategories = async () => {
  try {
    const response = await api.get('/api/categories')
    if (response.success) {
      categories.value = response.data
    }
  } catch (err) {
    console.error('Không thể tải danh mục.', err)
  }
}

const fetchDestinations = async () => {
  loading.value = true
  try {
    const response = await api.get('/api/destinations/map', {
      params: {
        keyword: searchKeyword.value || undefined,
        categoryId: selectedCategory.value || undefined,
        hasVirtualTour: filterHasTour.value ? true : undefined
      }
    })

    if (response.success) {
      destinations.value = response.data
      updateMapMarkers()
    }
  } catch (err) {
    error.value = 'Không thể tải danh sách bản đồ.'
  } finally {
    loading.value = false
  }
}

// Leaflet map initialization
const initMap = () => {
  if (!mapContainer.value) return

  // Default coordinate center Cần Thơ city center
  map = L.map(mapContainer.value).setView([10.03711, 105.78825], 13)

  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
  }).addTo(map)

  markersGroup = L.layerGroup().addTo(map)

  // Intercept click on popup routing link to prevent full reloads
  map.on('popupopen', (e) => {
    const popupEl = e.popup.getElement()
    if (!popupEl) return
    const links = popupEl.querySelectorAll('a')
    links.forEach(link => {
      link.addEventListener('click', (event) => {
        event.preventDefault()
        const url = link.getAttribute('href')
        if (url) {
          router.push(url)
        }
      })
    })
  })

  setTimeout(() => {
    if (map) map.invalidateSize()
  }, 200)
}

// Helper to render customized marker divicon matching category color
const createMarkerIcon = (color) => {
  const iconColor = color || '#0d9488' // Default teal
  return L.divIcon({
    className: 'custom-div-icon',
    html: `
      <div class="relative flex items-center justify-center w-8 h-8 rounded-full shadow-lg border-2 border-white text-white transition hover:scale-110 active:scale-95 duration-150" style="background-color: ${iconColor}">
        <svg class="w-4 h-4" fill="none" stroke="currentColor" stroke-width="2.5" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" d="M15 10.5a3 3 0 11-6 0 3 3 0 016 0z" />
          <path stroke-linecap="round" stroke-linejoin="round" d="M19.5 10.5c0 7.142-7.5 11.25-7.5 11.25S4.5 17.642 4.5 10.5a7.5 7.5 0 1115 0z" />
        </svg>
      </div>
    `,
    iconSize: [32, 32],
    iconAnchor: [16, 32],
    popupAnchor: [0, -32]
  })
}

// Re-draw markers on map based on search results
const updateMapMarkers = () => {
  if (!map || !markersGroup) return

  // Clear existing markers
  markersGroup.clearLayers()
  markersMap.clear()

  const bounds = L.latLngBounds()
  let hasValidCoords = false

  destinations.value.forEach(dest => {
    const lat = parseFloat(dest.latitude)
    const lng = parseFloat(dest.longitude)

    if (isNaN(lat) || isNaN(lng)) return

    const markerColor = dest.category?.color || '#0d9488'
    const customIcon = createMarkerIcon(markerColor)
    
    // Popup HTML layout
    const popupContent = `
      <div class="p-1 max-w-[200px] space-y-2 font-sans">
        ${dest.coverImageUrl ? `<img src="${dest.coverImageUrl}" class="w-full aspect-[16/10] object-cover rounded-lg shadow-sm border border-slate-100" />` : ''}
        <div class="space-y-0.5">
          <span class="text-[9px] font-bold text-teal-600 block uppercase tracking-wider">${dest.category?.name || 'Địa danh'}</span>
          <h4 class="font-black text-xs text-slate-800 line-clamp-1 leading-tight">${dest.name}</h4>
        </div>
        <div class="flex gap-2.5 pt-1.5 border-t border-slate-100">
          <a href="/destinations/${dest.slug || dest.id}" class="text-[10px] font-extrabold text-teal-600 hover:text-teal-700 underline">Chi tiết</a>
          ${dest.hasVirtualTour ? `<a href="/destinations/${dest.id}/tour" class="text-[10px] font-extrabold text-amber-500 hover:text-amber-600 underline flex items-center space-x-0.5"><span>360° Tour</span></a>` : ''}
        </div>
      </div>
    `

    const marker = L.marker([lat, lng], { icon: customIcon })
      .bindPopup(popupContent)
      .addTo(markersGroup)

    markersMap.set(dest.id, marker)
    bounds.extend([lat, lng])
    hasValidCoords = true
  })

  // Fit bounds if we have points
  if (hasValidCoords && destinations.value.length > 0) {
    map.fitBounds(bounds, { padding: [40, 40] })
  }
}

// Handler actions
const onFilterChange = () => {
  fetchDestinations()
}

const toggleTourFilter = () => {
  filterHasTour.value = !filterHasTour.value
  fetchDestinations()
}

const selectCategoryChip = (catId) => {
  selectedCategory.value = catId
  fetchDestinations()
}

const focusDestination = (dest) => {
  const lat = parseFloat(dest.latitude)
  const lng = parseFloat(dest.longitude)

  if (isNaN(lat) || isNaN(lng) || !map) return

  map.setView([lat, lng], 16)
  
  const marker = markersMap.get(dest.id)
  if (marker) {
    marker.openPopup()
  }
}

const resetMapZoom = () => {
  if (destinations.value.length === 0 || !map) {
    map.setView([10.03711, 105.78825], 13)
    return
  }

  const bounds = L.latLngBounds()
  let hasValid = false

  destinations.value.forEach(dest => {
    const lat = parseFloat(dest.latitude)
    const lng = parseFloat(dest.longitude)
    if (!isNaN(lat) && !isNaN(lng)) {
      bounds.extend([lat, lng])
      hasValid = true
    }
  })

  if (hasValid) {
    map.fitBounds(bounds, { padding: [40, 40] })
  }
}

onBeforeUnmount(() => {
  if (map) {
    map.remove()
    map = null
  }
})

onMounted(() => {
  fetchCategories()
  nextTick(() => {
    initMap()
    fetchDestinations()
  })
})
</script>

<style scoped>
:deep(.leaflet-popup-content-wrapper) {
  border-radius: 16px;
  padding: 4px;
  box-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.1), 0 8px 10px -6px rgba(0, 0, 0, 0.1);
  border: 1px border-slate-100;
}
:deep(.leaflet-popup-close-button) {
  top: 8px !important;
  right: 8px !important;
  color: #64748b !important;
}
:deep(.leaflet-tile-container) {
  filter: contrast(1.02) saturate(1.02);
}
</style>
