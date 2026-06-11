<template>
  <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-10 space-y-10 font-sans">
    <!-- Back Button -->
    <div class="flex items-center">
      <router-link to="/routes" class="inline-flex items-center space-x-2 text-slate-500 hover:text-teal-600 font-bold text-sm transition">
        <ArrowLeftIcon class="w-4 h-4" />
        <span>Quay lại danh sách tuyến</span>
      </router-link>
    </div>

    <!-- Error Alert -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-6 py-4 rounded-2xl flex items-center space-x-3 shadow-sm">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0 text-red-500" />
      <span class="text-sm font-bold">{{ error }}</span>
    </div>

    <div v-else-if="loading" class="space-y-8 animate-pulse">
      <div class="bg-slate-200 h-64 rounded-3xl"></div>
      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <div class="lg:col-span-2 space-y-6">
          <div class="bg-slate-200 h-96 rounded-3xl"></div>
        </div>
        <div class="space-y-6">
          <div class="bg-slate-200 h-96 rounded-3xl"></div>
        </div>
      </div>
    </div>

    <div v-else-if="route" class="grid grid-cols-1 lg:grid-cols-3 gap-8 items-start">
      <!-- Left Column: Details & Timeline (2/3 width) -->
      <div class="lg:col-span-2 space-y-8">
        <!-- Route Banner -->
        <div class="bg-gradient-to-r from-teal-950 via-teal-900 to-slate-900 rounded-3xl p-8 text-white shadow-xl relative overflow-hidden">
          <div class="absolute inset-0 opacity-10 bg-[radial-gradient(#e5e7eb_1px,transparent_1px)] [background-size:16px_16px]"></div>
          <div class="absolute w-64 h-64 rounded-full bg-teal-500/10 blur-3xl -top-20 -right-20 pointer-events-none"></div>
          
          <div class="relative z-10 space-y-4">
            <span class="text-[10px] font-black tracking-widest text-teal-300 uppercase bg-teal-800/60 px-3 py-1 rounded-full border border-teal-700/50 inline-block">
              {{ getThemeLabel(route.theme) }}
            </span>
            <h2 class="text-2xl md:text-4xl font-black tracking-tight leading-tight">{{ route.title }}</h2>
            <p class="text-sm text-teal-100/80 leading-relaxed font-medium">
              {{ route.description }}
            </p>

            <div class="flex flex-wrap gap-6 pt-4 border-t border-teal-800/40 text-teal-200/95 text-xs font-bold">
              <div class="flex items-center space-x-2">
                <ClockIcon class="w-4 h-4 text-teal-400" />
                <span>Thời lượng: {{ route.estimatedDuration || 'N/A' }}</span>
              </div>
              <div class="flex items-center space-x-2" v-if="route.distanceKm">
                <CompassIcon class="w-4 h-4 text-teal-400" />
                <span>Khoảng cách: {{ route.distanceKm }} km</span>
              </div>
              <div class="flex items-center space-x-2">
                <MapPinIcon class="w-4 h-4 text-teal-400" />
                <span>Số địa điểm: {{ route.destinations.length }}</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Interactive Route Map -->
        <div class="bg-white p-6 rounded-3xl border border-slate-200/80 shadow-sm space-y-4">
          <div class="flex items-center justify-between border-b border-slate-100 pb-4">
            <h3 class="text-lg font-black text-slate-800">Bản đồ tuyến tham quan</h3>
            <span class="text-xs font-medium text-slate-500 flex items-center space-x-1">
              <CompassIcon class="w-4 h-4 text-teal-500 animate-spin" style="animation-duration: 4s" />
              <span>Đường vẽ chỉ mang tính chất minh họa thứ tự tham quan.</span>
            </span>
          </div>
          <div ref="mapContainer" class="h-96 rounded-2xl overflow-hidden border border-slate-200 z-10"></div>
        </div>

        <!-- Timeline / Step Itinerary -->
        <div class="bg-white p-6 md:p-8 rounded-3xl border border-slate-200/80 shadow-sm space-y-6">
          <h3 class="text-lg font-black text-slate-800 border-b border-slate-100 pb-4">Lịch trình chi tiết</h3>
          
          <!-- Timeline list -->
          <div class="relative pl-6 space-y-8 before:absolute before:left-2 before:top-2 before:bottom-2 before:w-0.5 before:bg-slate-200/80">
            <div 
              v-for="(dest, index) in route.destinations" 
              :key="dest.id"
              class="relative space-y-4"
            >
              <!-- Numbered Indicator node -->
              <div class="absolute -left-[30px] top-0.5 w-6 h-6 rounded-full bg-teal-600 border-2 border-white shadow-sm flex items-center justify-center text-white text-[10px] font-black">
                {{ index + 1 }}
              </div>

              <!-- Destination Row Card -->
              <div 
                @click="trackRouteDestinationClick(dest, index)"
                class="bg-slate-50/50 hover:bg-slate-50 p-5 rounded-2xl border border-slate-200/40 hover:border-slate-200 transition-all flex flex-col sm:flex-row gap-5 items-start cursor-pointer"
              >
                <!-- Cover Image -->
                <div class="w-full sm:w-36 h-24 rounded-xl overflow-hidden bg-slate-100 flex-shrink-0">
                  <img 
                    :src="dest.coverImageUrl || 'https://images.unsplash.com/photo-1596422846543-75c6fc18a52b?auto=format&fit=crop&w=300&q=80'" 
                    :alt="dest.name"
                    class="w-full h-full object-cover"
                  />
                </div>

                <!-- Info detail -->
                <div class="flex-grow space-y-2">
                  <div class="flex flex-wrap gap-2 items-center">
                    <h4 class="text-base font-black text-slate-800">{{ dest.name }}</h4>
                    <span class="text-[9px] font-black uppercase px-2 py-0.5 rounded bg-teal-50 border border-teal-100 text-teal-700">
                      {{ dest.categoryName }}
                    </span>
                  </div>
                  <p class="text-xs text-slate-500 font-medium leading-relaxed line-clamp-2">
                    {{ dest.shortDescription }}
                  </p>
                  
                  <div class="flex flex-wrap gap-x-6 gap-y-2 pt-2 text-[11px] font-bold text-slate-400">
                    <span v-if="dest.estimatedStayTime" class="flex items-center space-x-1">
                      <ClockIcon class="w-3.5 h-3.5 text-slate-400" />
                      <span>Dừng chân: {{ dest.estimatedStayTime }}</span>
                    </span>
                    <span v-if="dest.note" class="flex items-center space-x-1 text-teal-600 bg-teal-50/50 px-2 py-0.5 rounded-md">
                      <span>Lưu ý: {{ dest.note }}</span>
                    </span>
                  </div>
                </div>

                <!-- Actions -->
                <div class="flex sm:flex-col gap-2 w-full sm:w-auto self-stretch justify-end">
                  <router-link 
                    :to="`/destinations/${dest.destinationId}`"
                    @click.stop="trackOpenDestinationFromRoute(dest)"
                    class="flex-1 sm:flex-none text-center px-4 py-2 bg-white hover:bg-slate-50 border border-slate-200 text-slate-700 text-xs font-bold rounded-xl transition shadow-sm"
                  >
                    Chi tiết
                  </router-link>
                  <router-link 
                    v-if="dest.hasVirtualTour"
                    :to="`/destinations/${dest.destinationId}/tour`"
                    @click.stop="trackOpenTourFromRoute(dest)"
                    class="flex-1 sm:flex-none text-center px-4 py-2 bg-teal-600 hover:bg-teal-700 text-white text-xs font-bold rounded-xl transition shadow shadow-teal-600/10"
                  >
                    Tour 360°
                  </router-link>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Right Column: Sidebar (1/3 width) -->
      <div class="space-y-6">
        <!-- Thumbnail preview card -->
        <div class="bg-white rounded-3xl overflow-hidden border border-slate-200/80 shadow-sm">
          <div class="h-48 w-full bg-slate-100 relative">
            <img 
              :src="route.thumbnailUrl || 'https://images.unsplash.com/photo-1599707367072-cd6ada2bc375?auto=format&fit=crop&w=400&q=80'" 
              :alt="route.title"
              class="w-full h-full object-cover"
            />
          </div>
          <div class="p-6 space-y-4">
            <h4 class="text-sm font-black text-slate-400 uppercase tracking-wider">Tóm tắt hành trình</h4>
            <div class="space-y-3">
              <div class="flex items-center justify-between text-sm">
                <span class="text-slate-500 font-medium">Khu vực:</span>
                <span class="font-bold text-slate-800">{{ route.regionName }}</span>
              </div>
              <div class="flex items-center justify-between text-sm">
                <span class="text-slate-500 font-medium">Chủ đề:</span>
                <span class="font-bold text-slate-800">{{ getThemeLabel(route.theme) }}</span>
              </div>
              <div class="flex items-center justify-between text-sm">
                <span class="text-slate-500 font-medium">Số điểm dừng:</span>
                <span class="font-bold text-slate-800">{{ route.destinations.length }} địa điểm</span>
              </div>
              <div class="flex items-center justify-between text-sm" v-if="route.distanceKm">
                <span class="text-slate-500 font-medium">Tổng quãng đường:</span>
                <span class="font-bold text-slate-800">{{ route.distanceKm }} km</span>
              </div>
              <div class="flex items-center justify-between text-sm">
                <span class="text-slate-500 font-medium">Thời gian dự kiến:</span>
                <span class="font-bold text-slate-800">{{ route.estimatedDuration || 'N/A' }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, nextTick } from 'vue'
import { useRoute } from 'vue-router'
import api from '../utils/api'
import { trackEvent } from '../services/analytics'
import L from 'leaflet'
import { 
  ArrowLeft as ArrowLeftIcon, 
  AlertCircle as AlertCircleIcon,
  Clock as ClockIcon, 
  Compass as CompassIcon, 
  MapPin as MapPinIcon 
} from 'lucide-vue-next'

const routeQuery = useRoute()
const loading = ref(true)
const error = ref(null)
const route = ref(null)

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

const getThemeLabel = (theme) => {
  const themes = {
    culture: 'Văn hóa - Lịch sử',
    eco: 'Sinh thái sông nước',
    food: 'Ẩm thực địa phương',
    craft: 'Làng nghề'
  }
  return themes[theme] || 'Khám phá'
}

const trackRouteDestinationClick = (dest, index) => {
  trackEvent({
    eventName: 'click_route_destination',
    targetType: 'destination',
    targetId: dest.destinationId,
    metadata: { routeId: route.value?.id, displayOrder: index + 1 }
  })
}

const trackOpenDestinationFromRoute = (dest) => {
  trackEvent({
    eventName: 'open_destination_from_route',
    targetType: 'destination',
    targetId: dest.destinationId,
    metadata: { routeId: route.value?.id }
  })
}

const trackOpenTourFromRoute = (dest) => {
  trackEvent({
    eventName: 'open_virtual_tour_from_route',
    targetType: 'destination',
    targetId: dest.destinationId,
    metadata: { routeId: route.value?.id }
  })
}

const fetchRouteDetail = async () => {
  loading.value = true
  error.value = null
  try {
    const slug = routeQuery.params.slug
    const res = await api.get(`/api/routes/${slug}`)
    if (res.success) {
      route.value = res.data
      
      // Track view_route event
      trackEvent({
        eventName: 'view_route',
        targetType: 'route',
        targetId: res.data.id
      })
    }
  } catch (err) {
    error.value = err.message || 'Không thể tải chi tiết tuyến tham quan.'
  } finally {
    loading.value = false
  }

  if (route.value) {
    nextTick(() => {
      initRouteMap()
    })
  }
}

const initRouteMap = () => {
  if (!mapContainer.value || !route.value || route.value.destinations.length === 0) return

  // Find center coordinate
  const validLats = route.value.destinations.map(d => parseFloat(d.latitude)).filter(lat => !isNaN(lat))
  const validLngs = route.value.destinations.map(d => parseFloat(d.longitude)).filter(lng => !isNaN(lng))

  let centerLat = 10.03711
  let centerLng = 105.78825

  if (validLats.length > 0 && validLngs.length > 0) {
    centerLat = validLats.reduce((sum, val) => sum + val, 0) / validLats.length
    centerLng = validLngs.reduce((sum, val) => sum + val, 0) / validLngs.length
  }

  map = L.map(mapContainer.value).setView([centerLat, centerLng], 13)

  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenStreetMap contributors'
  }).addTo(map)

  const latlngs = []

  // Add numbered markers
  route.value.destinations.forEach((dest, index) => {
    const lat = parseFloat(dest.latitude)
    const lng = parseFloat(dest.longitude)
    if (isNaN(lat) || isNaN(lng)) return

    latlngs.push([lat, lng])

    // Custom numbered circle icon
    const numberIcon = L.divIcon({
      html: `<div class="w-8 h-8 rounded-full bg-teal-600 text-white font-extrabold flex items-center justify-center border-2 border-white shadow-md select-none transform hover:scale-110 hover:bg-teal-700 transition">${index + 1}</div>`,
      className: 'custom-numbered-marker-wrapper',
      iconSize: [32, 32],
      iconAnchor: [16, 16]
    })

    // Popup content
    const coverHtml = dest.coverImageUrl 
      ? `<img src="${dest.coverImageUrl}" class="w-full h-20 object-cover rounded-md mb-2"/>` 
      : ''
    
    const virtualTourBtn = dest.hasVirtualTour 
      ? `<a href="/destinations/${dest.destinationId}/tour" class="flex-1 text-center py-1 bg-teal-600 text-white rounded text-[10px] font-bold decoration-none">Tour 360°</a>` 
      : ''

    const popupHtml = `
      <div class="font-sans space-y-1 p-1" style="min-width: 150px">
        ${coverHtml}
        <h4 class="text-xs font-bold text-slate-800 my-0 leading-tight">${dest.name}</h4>
        <span class="text-[9px] text-slate-400 font-bold uppercase block">${dest.categoryName}</span>
        <div class="flex gap-2 pt-2">
          <a href="/destinations/${dest.destinationId}" class="flex-grow text-center py-1 border border-slate-200 text-slate-700 rounded text-[10px] font-bold decoration-none">Chi tiết</a>
          ${virtualTourBtn}
        </div>
      </div>
    `

    const marker = L.marker([lat, lng], { icon: numberIcon })
      .bindPopup(popupHtml)
      .addTo(map)

    markers.push(marker)
  })

  // Fit bounds to cover all points
  if (latlngs.length > 0) {
    const bounds = L.latLngBounds(latlngs)
    map.fitBounds(bounds, { padding: [40, 40] })

    // Draw straight connecting line polyline
    polyline = L.polyline(latlngs, {
      color: '#0d9488', // teal-600
      weight: 3.5,
      dashArray: '7, 7',
      opacity: 0.85
    }).addTo(map)
  }
}

onMounted(() => {
  fetchRouteDetail()
})
</script>

<style>
/* Leaflet popup overrides to look cleaner and align with our custom popup html styles */
.leaflet-popup-content-wrapper {
  border-radius: 12px !important;
  box-shadow: 0 4px 20px -2px rgba(15, 23, 42, 0.15) !important;
  border: 1px solid rgba(226, 232, 240, 0.8) !important;
}
.leaflet-popup-content {
  margin: 8px 10px !important;
}
.custom-numbered-marker-wrapper {
  overflow: visible !important;
}
</style>
