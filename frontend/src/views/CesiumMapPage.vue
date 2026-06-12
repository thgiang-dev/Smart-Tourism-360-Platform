<template>
  <div class="h-screen flex flex-col overflow-hidden bg-slate-900 font-sans text-slate-100">
    <!-- Header -->
    <PublicHeader />

    <!-- Split Workspace -->
    <div class="flex-grow flex flex-col md:flex-row overflow-hidden relative">
      
      <!-- Sidebar Panel Left (30%) -->
      <aside 
        :class="[
          'bg-slate-950 border-r border-slate-800 flex flex-col z-20 flex-shrink-0 shadow-lg text-slate-300 relative transition-all duration-300 ease-in-out',
          isSidebarOpen ? 'w-full md:w-80 lg:w-96 translate-x-0' : 'w-0 -translate-x-full md:w-0 md:border-r-0 md:pointer-events-none'
        ]"
      >
        <!-- Toggle Button -->
        <button 
          @click="isSidebarOpen = !isSidebarOpen"
          :class="[
            'absolute top-1/2 -translate-y-1/2 z-30 w-6 h-12 flex items-center justify-center border rounded-r-xl shadow-lg transition-all duration-200 pointer-events-auto focus:outline-none',
            isSidebarOpen 
              ? 'left-full -translate-x-1/2 bg-slate-900 hover:bg-slate-850 border-slate-800 text-slate-400 hover:text-teal-400' 
              : 'left-0 translate-x-0 bg-teal-600 hover:bg-teal-500 border-teal-600 text-white shadow-lg shadow-teal-600/30'
          ]"
          title="Bật/Tắt Sidebar"
        >
          <ChevronLeftIcon v-if="isSidebarOpen" class="w-4 h-4" />
          <ChevronRightIcon v-else class="w-4 h-4" />
        </button>

        <!-- Sidebar Content Wrapper (to avoid layout break when sidebar is closed) -->
        <div v-show="isSidebarOpen" class="flex flex-col flex-grow h-full w-full overflow-hidden">
          <!-- Sidebar Search Form -->
          <div class="p-5 border-b border-slate-800 space-y-4">
            <div class="relative">
              <input 
                v-model="searchKeyword"
                type="text" 
                placeholder="Tìm kiếm địa điểm 3D..." 
                class="w-full pl-10 pr-4 py-3 bg-slate-900 border border-slate-800 focus:border-teal-500 focus:bg-slate-850 rounded-2xl text-xs text-white focus:outline-none transition duration-200"
                @input="onFilterChange"
              />
              <span class="absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-500">
                <SearchIcon class="w-4 h-4" />
              </span>
            </div>

            <!-- Feature filter toggles -->
            <div class="flex items-center justify-between text-xs px-1">
              <span class="text-slate-400 font-extrabold">Chỉ có Tour ảo 360°</span>
              <button 
                @click="toggleTourFilter"
                class="w-10 h-6 rounded-full p-0.5 transition duration-200 focus:outline-none"
                :class="filterHasTour ? 'bg-teal-600' : 'bg-slate-800'"
              >
                <div 
                  class="w-5 h-5 bg-white rounded-full shadow-md transform duration-200"
                  :class="filterHasTour ? 'translate-x-4' : 'translate-x-0'"
                ></div>
              </button>
            </div>
          </div>

          <!-- Scrollable Sidebar lists -->
          <div class="flex-grow overflow-y-auto p-5 space-y-6 custom-scrollbar">
            <!-- Categories Filter Chips list -->
            <div class="space-y-3">
              <p class="text-[10px] font-black text-slate-500 uppercase tracking-wider">Danh mục</p>
              <div class="flex flex-wrap gap-2">
                <button 
                  @click="selectCategoryChip(null)"
                  class="px-3 py-1.5 rounded-xl text-[10px] font-black transition-all border duration-200 shadow-sm"
                  :class="[
                    selectedCategory === null 
                      ? 'bg-teal-600 border-teal-600 text-white shadow-teal-600/10' 
                      : 'bg-slate-900 border-slate-800 text-slate-400 hover:border-slate-700 hover:text-white'
                  ]"
                >
                  Tất cả
                </button>
                 <button 
                  v-for="cat in categories" 
                  :key="cat.id"
                  @click="selectCategoryChip(cat.id)"
                  class="px-3 py-1.5 rounded-xl text-[10px] font-black transition-all border duration-200 shadow-sm"
                  :class="[
                    selectedCategory === cat.id 
                      ? 'text-white border-transparent' 
                      : 'bg-slate-900 border-slate-800 text-slate-400 hover:border-slate-700 hover:text-white'
                  ]"
                  :style="selectedCategory === cat.id ? { backgroundColor: cat.color || '#0d9488', boxShadow: `0 4px 12px ${cat.color}33` } : {}"
                >
                  {{ cat.name }}
                </button>
              </div>
            </div>

            <!-- Active Route Info Panel if query param routeId is active -->
            <div v-if="activeRouteData" class="p-4 bg-teal-950/40 border border-teal-800/40 rounded-2xl space-y-3">
              <div class="flex items-center justify-between border-b border-teal-900 pb-2">
                <span class="text-[9px] font-black text-teal-400 uppercase tracking-wider">Tuyến đang xem</span>
                <button @click="clearActiveRoute" class="text-slate-400 hover:text-white transition">
                  <svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" stroke-width="2.5" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
                  </svg>
                </button>
              </div>
              <h5 class="text-xs font-black text-white leading-tight">{{ activeRouteData.title }}</h5>
              <p class="text-[10px] text-slate-400 leading-normal">{{ activeRouteData.description }}</p>
              <div class="text-[9px] font-bold text-teal-400 flex items-center space-x-1">
                <CompassIcon class="w-3.5 h-3.5 animate-spin" style="animation-duration: 6s" />
                <span>Đường nối 3D chỉ minh họa thứ tự tham quan.</span>
              </div>
            </div>

            <!-- Destination lists scroll -->
            <div class="space-y-4 pt-2">
              <p class="text-[10px] font-black text-slate-500 uppercase tracking-wider">
                Kết quả tìm kiếm ({{ destinations.length }})
              </p>

              <!-- Loading Skeleton -->
              <div v-if="loading" class="space-y-3">
                <div v-for="i in 4" :key="i" class="flex items-center space-x-3 p-3 bg-slate-900/60 rounded-2xl animate-pulse border border-slate-850">
                  <div class="w-16 h-12 bg-slate-800 rounded-xl flex-shrink-0"></div>
                  <div class="space-y-2 flex-1">
                    <div class="h-3.5 bg-slate-800 rounded w-3/4"></div>
                    <div class="h-2.5 bg-slate-900/60 rounded w-1/2"></div>
                  </div>
                </div>
              </div>

              <!-- Empty Results -->
              <div v-else-if="destinations.length === 0" class="text-center py-16 space-y-4 border border-slate-850 rounded-2xl bg-slate-900/20">
                <InboxIcon class="w-10 h-10 mx-auto text-slate-600" />
                <div class="space-y-1">
                  <p class="text-xs font-black text-slate-400">Không tìm thấy địa điểm nào</p>
                  <p class="text-[10px] text-slate-600">Thử đặt lại bộ lọc hoặc điều chỉnh từ khóa</p>
                </div>
              </div>

              <!-- List Results Cards -->
              <div v-else class="space-y-3">
                <div 
                  v-for="dest in destinations" 
                  :key="dest.id"
                  class="flex space-x-3 p-3 bg-slate-900 border border-slate-800/80 hover:bg-teal-950/20 hover:border-teal-500/20 rounded-2xl cursor-pointer transition-all duration-350 group"
                  :class="{ 'border-teal-500 bg-teal-950/20': selectedDestination?.id === dest.id }"
                  @click="focusDestination(dest)"
                >
                  <!-- Thumbnail -->
                  <div class="w-20 h-14 bg-slate-800 border border-slate-800 rounded-xl overflow-hidden flex-shrink-0 relative">
                    <img v-if="dest.coverImageUrl" :src="dest.coverImageUrl" class="w-full h-full object-cover group-hover:scale-105 transition duration-300" />
                    <ImageIcon v-else class="w-5 h-5 text-slate-600 absolute inset-0 m-auto" />
                  </div>
                  
                  <!-- Info text -->
                  <div class="min-w-0 flex-1 space-y-1.5 flex flex-col justify-between">
                    <div>
                      <h4 class="text-xs font-black text-slate-200 truncate group-hover:text-teal-400 transition" :class="{ 'text-teal-400': selectedDestination?.id === dest.id }">
                        {{ dest.name }}
                      </h4>
                      <span class="text-[9px] text-slate-500 block font-bold uppercase tracking-wider">
                        {{ dest.category?.name }} | {{ dest.regionName }}
                      </span>
                    </div>
                    
                    <!-- Has indicators -->
                    <div class="flex items-center space-x-2">
                      <span 
                        v-if="dest.hasVirtualTour" 
                        class="px-2 py-0.5 bg-amber-500 text-slate-950 rounded text-[8px] font-black uppercase tracking-wider shadow shadow-amber-500/10 flex items-center space-x-0.5"
                      >
                        <CompassIcon class="w-2.5 h-2.5" />
                        <span>360° Tour</span>
                      </span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </aside>

      <!-- Cesium 3D Map Panel Right (70%) -->
      <main class="flex-grow h-full w-full z-10 relative bg-slate-950">
        <!-- 3D Canvas component -->
        <CesiumViewer 
          ref="cesiumViewerRef"
          :destinations="destinations"
          :selected-destination-id="selectedDestination?.id"
          :route-data="activeRouteData"
          @select-destination="onSelectDestinationFromMap"
          @clear-selection="onClearSelection"
          @update-popup-position="onUpdatePopupPosition"
          @viewer-ready="onViewerReady"
        />

        <!-- Floating Destination Popup Overlay -->
        <CesiumDestinationPopup
          v-if="selectedDestination"
          :destination="selectedDestination"
          :position="popupPosition"
          @close="onClearSelection"
          @fly-to="triggerFlyToSelected"
        />

        <!-- 2D Mode Toggle link -->
        <router-link 
          :to="activeRouteData ? `/explore?routeId=${activeRouteData.id}` : '/explore'" 
          class="absolute top-5 right-5 z-20 px-4 py-2.5 bg-white border border-slate-200 hover:bg-slate-100 text-slate-800 rounded-2xl text-xs font-black shadow-2xl flex items-center space-x-1.5 transition"
        >
          <MapIcon class="w-4 h-4 text-teal-600 animate-pulse" />
          <span>Bản đồ 2D</span>
        </router-link>

        <!-- Reset Camera overlay button -->
        <button 
          @click="resetCamera" 
          class="absolute bottom-5 right-5 z-20 p-3 bg-slate-900 border border-slate-800 hover:border-slate-700 hover:bg-slate-850 rounded-2xl text-slate-300 shadow-2xl transition"
          title="Đặt lại góc nhìn 3D"
        >
          <CompassIcon class="w-5 h-5 text-teal-500" />
        </button>
      </main>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import api from '../utils/api'
import { trackEvent } from '../services/analytics'
import PublicHeader from '../components/public/PublicHeader.vue'
import CesiumViewer from '../components/public/CesiumViewer.vue'
import CesiumDestinationPopup from '../components/public/CesiumDestinationPopup.vue'
import { 
  Search as SearchIcon, 
  Image as ImageIcon, 
  Inbox as InboxIcon, 
  Compass as CompassIcon,
  Map as MapIcon,
  ChevronLeft as ChevronLeftIcon,
  ChevronRight as ChevronRightIcon
} from 'lucide-vue-next'

const router = useRouter()
const routeQuery = useRoute()

const isSidebarOpen = ref(true)
const loading = ref(false)
const destinations = ref([])
const categories = ref([])

const searchKeyword = ref('')
const selectedCategory = ref(null)
const filterHasTour = ref(false)

// Active route context state
const activeRouteData = ref(null)

// Selected destination and its floating screen position coordinates
const selectedDestination = ref(null)
const popupPosition = ref({ x: 0, y: 0 })

// Reference to CesiumViewer child component
const cesiumViewerRef = ref(null)

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

      // If active query parameter has destinationId, verify and auto-select
      const destId = routeQuery.query.destinationId
      if (destId && !selectedDestination.value) {
        const matching = response.data.find(d => d.id === destId)
        if (matching) {
          selectedDestination.value = matching
        }
      }
    }
  } catch (err) {
    console.error('Không thể tải danh sách địa điểm:', err)
  } finally {
    loading.value = false
  }
}

const fetchActiveRoute = async (routeId) => {
  try {
    const res = await api.get(`/api/routes/${routeId}`)
    if (res.success) {
      activeRouteData.value = res.data
      
      // Track view_3d_route event
      trackEvent({
        eventName: 'view_3d_route',
        targetType: 'route',
        targetId: res.data.id,
        metadata: { source: 'explore_3d' }
      })
    }
  } catch (err) {
    console.error('Không thể tải chi tiết tuyến tham quan:', err)
  }
}

const clearActiveRoute = () => {
  activeRouteData.value = null
  router.push('/explore-3d')
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

// Focusing a destination via sidebar click
const focusDestination = (dest) => {
  selectedDestination.value = dest
  // Trigger flying camera through the ref
  if (cesiumViewerRef.value) {
    cesiumViewerRef.value.flyToCoordinates(parseFloat(dest.longitude), parseFloat(dest.latitude), 2000)
    
    // Log analytics interaction
    trackEvent({
      eventName: 'fly_to_destination',
      targetType: 'destination',
      targetId: dest.id,
      metadata: { source: 'sidebar_click' }
    })
  }
}

// When selected from the 3D map canvas directly
const onSelectDestinationFromMap = ({ destination, screenPosition }) => {
  selectedDestination.value = destination
  popupPosition.value = screenPosition

  trackEvent({
    eventName: 'click_3d_marker',
    targetType: 'destination',
    targetId: destination.id,
    metadata: { source: '3d_globe_click' }
  })
}

const onClearSelection = () => {
  selectedDestination.value = null
}

const onUpdatePopupPosition = (screenPos) => {
  popupPosition.value = screenPos
}

const triggerFlyToSelected = () => {
  if (selectedDestination.value && cesiumViewerRef.value) {
    cesiumViewerRef.value.flyToCoordinates(
      parseFloat(selectedDestination.value.longitude),
      parseFloat(selectedDestination.value.latitude),
      1500
    )

    trackEvent({
      eventName: 'fly_to_destination',
      targetType: 'destination',
      targetId: selectedDestination.value.id,
      metadata: { source: 'popup_flyto' }
    })
  }
}

const resetCamera = () => {
  if (cesiumViewerRef.value) {
    cesiumViewerRef.value.resetView()
  }
}

const onViewerReady = () => {
  // Post view_3d_map event
  trackEvent({
    eventName: 'view_3d_map',
    targetType: 'map',
    metadata: { routeId: routeQuery.query.routeId || null }
  })
}

onMounted(() => {
  fetchCategories()
  fetchDestinations()

  const rId = routeQuery.query.routeId
  if (rId) {
    fetchActiveRoute(rId)
  }
})
</script>

<style scoped>
/* Scrollbar styling for sidebar list */
.custom-scrollbar::-webkit-scrollbar {
  width: 5px;
}
.custom-scrollbar::-webkit-scrollbar-track {
  background: rgba(15, 23, 42, 0.05);
}
.custom-scrollbar::-webkit-scrollbar-thumb {
  background: rgba(14, 116, 144, 0.3);
  border-radius: 4px;
}
.custom-scrollbar::-webkit-scrollbar-thumb:hover {
  background: rgba(14, 116, 144, 0.5);
}
</style>
