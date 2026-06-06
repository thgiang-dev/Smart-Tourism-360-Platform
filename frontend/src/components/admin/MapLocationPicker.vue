<template>
  <div class="flex flex-col h-full bg-white rounded-2xl border border-slate-200 overflow-hidden shadow-sm">
    <div class="px-4 py-3 bg-slate-50 border-b border-slate-200 flex justify-between items-center">
      <span class="text-xs font-semibold uppercase tracking-wider text-slate-500">Bản đồ chọn tọa độ</span>
      <div class="text-[10px] bg-teal-100 text-teal-800 font-bold px-2 py-0.5 rounded-full">
        Click hoặc Kéo thả marker
      </div>
    </div>
    
    <!-- Map Container -->
    <div ref="mapContainer" class="flex-grow min-h-[300px] h-full w-full z-10"></div>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, watch } from 'vue'
import L from 'leaflet'

// Redefine Leaflet default icon paths to prevent broken marker images under Vite/bundler
delete L.Icon.Default.prototype._getIconUrl
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png',
  iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
  shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png'
})

const props = defineProps({
  latitude: {
    type: [Number, String],
    default: 10.03711 // Default to Cần Thơ Demo Center
  },
  longitude: {
    type: [Number, String],
    default: 105.78825
  },
  regionCenter: {
    type: Object,
    default: null
  }
})

const emit = defineEmits(['update:latitude', 'update:longitude'])

const mapContainer = ref(null)
let map = null
let marker = null

// Initialize map on mount
onMounted(() => {
  const initLat = parseFloat(props.latitude) || 10.03711
  const initLng = parseFloat(props.longitude) || 105.78825

  map = L.map(mapContainer.value).setView([initLat, initLng], 13)

  // Load OpenStreetMap tiles
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
  }).addTo(map)

  // Create draggable marker
  marker = L.marker([initLat, initLng], { draggable: true }).addTo(map)

  // Dragend marker handler
  marker.on('dragend', () => {
    const position = marker.getLatLng()
    emitUpdate(position.lat, position.lng)
  })

  // Click map handler to place marker and update coords
  map.on('click', (e) => {
    const { lat, lng } = e.latlng
    marker.setLatLng([lat, lng])
    emitUpdate(lat, lng)
  })

  // Force map to render correctly size issues
  setTimeout(() => {
    if (map) map.invalidateSize()
  }, 200)
})

// Clean up map instance on destruction
onBeforeUnmount(() => {
  if (map) {
    map.remove()
    map = null
  }
})

// Helper to format and emit coordinates
const emitUpdate = (lat, lng) => {
  const formattedLat = parseFloat(lat.toFixed(7))
  const formattedLng = parseFloat(lng.toFixed(7))
  emit('update:latitude', formattedLat)
  emit('update:longitude', formattedLng)
}

// Watch props coordinate changes (e.g. from parent manual input fields)
watch(
  () => [props.latitude, props.longitude],
  ([newLat, newLng]) => {
    const latVal = parseFloat(newLat)
    const lngVal = parseFloat(newLng)
    
    if (isNaN(latVal) || isNaN(lngVal)) return

    if (marker && map) {
      const currentPos = marker.getLatLng()
      // Update only if distance is changed to avoid feedback loops
      if (Math.abs(currentPos.lat - latVal) > 0.00001 || Math.abs(currentPos.lng - lngVal) > 0.00001) {
        marker.setLatLng([latVal, lngVal])
        map.panTo([latVal, lngVal])
      }
    }
  }
)

// Watch selected region center changes to pan map and zoom
watch(
  () => props.regionCenter,
  (newCenter) => {
    if (newCenter && newCenter.lat && newCenter.lng && map) {
      const latVal = parseFloat(newCenter.lat)
      const lngVal = parseFloat(newCenter.lng)
      const zoomVal = parseInt(newCenter.zoom) || 13

      if (!isNaN(latVal) && !isNaN(lngVal)) {
        map.setView([latVal, lngVal], zoomVal)
        marker.setLatLng([latVal, lngVal])
        emitUpdate(latVal, lngVal)
      }
    }
  },
  { deep: true }
)
</script>

<style scoped>
/* Ensure map layer container styled cleanly */
:deep(.leaflet-tile-container) {
  filter: contrast(1.05) saturate(1.05); /* Slight color enhancement for OS tiles */
}
</style>
