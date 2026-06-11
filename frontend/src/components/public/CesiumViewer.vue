<template>
  <div class="relative w-full h-full">
    <!-- Cesium Canvas Container -->
    <div ref="cesiumContainer" class="w-full h-full bg-slate-900"></div>
    
    <!-- Fallback indicator when viewer is loading or if it fails -->
    <div v-if="!viewerReady" class="absolute inset-0 flex items-center justify-center bg-slate-950/40 backdrop-blur-sm z-10 transition duration-300">
      <div class="text-center space-y-4">
        <div class="w-12 h-12 border-4 border-teal-500 border-t-transparent rounded-full animate-spin mx-auto"></div>
        <p class="text-xs font-black text-white tracking-widest uppercase">Đang tải bản đồ 3D...</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, watch, nextTick } from 'vue'
import * as Cesium from 'cesium'

// Set Cesium access token from Vite environment variable
if (import.meta.env.VITE_CESIUM_ION_TOKEN) {
  Cesium.Ion.defaultAccessToken = import.meta.env.VITE_CESIUM_ION_TOKEN
}

const props = defineProps({
  destinations: {
    type: Array,
    default: () => []
  },
  selectedDestinationId: {
    type: String,
    default: null
  },
  routeData: {
    type: Object,
    default: null
  }
})

const emit = defineEmits([
  'select-destination',
  'clear-selection',
  'update-popup-position',
  'viewer-ready'
])

const cesiumContainer = ref(null)
const viewerReady = ref(false)

let viewer = null
let handler = null
let postRenderListener = null

// Tracking map of generated entities
const markersMap = new Map()
let routeEntities = []

// Cần Thơ city center default coords
const defaultLon = 105.78825
const defaultLat = 10.03711

// Clean up existing route entities
const clearRouteEntities = () => {
  if (!viewer) return
  routeEntities.forEach(entity => {
    viewer.entities.remove(entity)
  })
  routeEntities = []
}

// Draw route polyline and numbered markers
const drawRoute = (route) => {
  if (!viewer || !route || !route.destinations || route.destinations.length === 0) return

  clearRouteEntities()

  const positions = []
  const validDests = route.destinations.filter(d => {
    const lat = parseFloat(d.latitude)
    const lon = parseFloat(d.longitude)
    return !isNaN(lat) && !isNaN(lon)
  })

  // Sort destinations by display order if they have it
  const sortedDests = [...validDests].sort((a, b) => (a.displayOrder || 0) - (b.displayOrder || 0))

  sortedDests.forEach((dest, index) => {
    const lat = parseFloat(dest.latitude)
    const lon = parseFloat(dest.longitude)
    const pos = Cesium.Cartesian3.fromDegrees(lon, lat, 0)
    positions.push(pos)

    // Numbered marker entity
    const marker = viewer.entities.add({
      position: pos,
      point: {
        pixelSize: 24,
        color: Cesium.Color.fromCssColorString('#0d9488'), // teal-600
        outlineColor: Cesium.Color.WHITE,
        outlineWidth: 2,
        heightReference: Cesium.HeightReference.CLAMP_TO_GROUND,
        disableDepthTestDistance: Number.POSITIVE_INFINITY
      },
      label: {
        text: (index + 1).toString(),
        font: 'bold 11px sans-serif',
        fillColor: Cesium.Color.WHITE,
        style: Cesium.LabelStyle.FILL,
        horizontalOrigin: Cesium.HorizontalOrigin.CENTER,
        verticalOrigin: Cesium.VerticalOrigin.CENTER,
        heightReference: Cesium.HeightReference.CLAMP_TO_GROUND,
        disableDepthTestDistance: Number.POSITIVE_INFINITY
      },
      // Store reference destination data
      properties: {
        destinationData: {
          id: dest.destinationId || dest.id,
          name: dest.name,
          slug: dest.slug || '',
          latitude: lat,
          longitude: lon,
          coverImageUrl: dest.coverImageUrl || '',
          hasVirtualTour: dest.hasVirtualTour || false,
          category: {
            name: dest.categoryName || 'Địa danh',
            color: '#0d9488'
          }
        }
      }
    })

    routeEntities.push(marker)
  })

  // Draw polyline connecting points
  if (positions.length > 1) {
    const polyline = viewer.entities.add({
      polyline: {
        positions: positions,
        width: 4,
        material: new Cesium.PolylineDashMaterialProperty({
          color: Cesium.Color.fromCssColorString('#0d9488'),
          dashLength: 16.0
        }),
        clampToGround: true
      }
    })
    routeEntities.push(polyline)
  }

  // Zoom to route
  if (positions.length > 0) {
    viewer.zoomTo(routeEntities, new Cesium.HeadingPitchRange(
      Cesium.Math.toRadians(0),
      Cesium.Math.toRadians(-45),
      35000.0 // height range
    ))
  }
}

// Render regular destination markers on the map
const updateDestinations = (destList) => {
  if (!viewer) return

  // Clear existing markers first
  markersMap.forEach(entity => viewer.entities.remove(entity))
  markersMap.clear()

  destList.forEach(dest => {
    const lat = parseFloat(dest.latitude)
    const lon = parseFloat(dest.longitude)

    if (isNaN(lat) || isNaN(lon)) return

    const colorHex = dest.category?.color || '#0F766E'
    const color = Cesium.Color.fromCssColorString(colorHex)

    const entity = viewer.entities.add({
      position: Cesium.Cartesian3.fromDegrees(lon, lat, 0),
      point: {
        pixelSize: 12,
        color: color,
        outlineColor: Cesium.Color.WHITE,
        outlineWidth: 2,
        heightReference: Cesium.HeightReference.CLAMP_TO_GROUND,
        disableDepthTestDistance: Number.POSITIVE_INFINITY
      },
      label: {
        text: dest.name,
        font: 'bold 11px sans-serif',
        fillColor: Cesium.Color.WHITE,
        outlineColor: Cesium.Color.BLACK,
        outlineWidth: 2,
        style: Cesium.LabelStyle.FILL_AND_OUTLINE,
        pixelOffset: new Cesium.Cartesian2(0, -18),
        horizontalOrigin: Cesium.HorizontalOrigin.CENTER,
        verticalOrigin: Cesium.VerticalOrigin.BOTTOM,
        heightReference: Cesium.HeightReference.CLAMP_TO_GROUND,
        disableDepthTestDistance: Number.POSITIVE_INFINITY,
        distanceDisplayCondition: new Cesium.DistanceDisplayCondition(0.0, 100000.0) // Hide when zoomed far
      },
      properties: {
        destinationData: dest
      }
    })

    markersMap.set(dest.id, entity)
  })
}

// Camera control helper: Fly to coordinate and center it in viewport
const flyToCoordinates = (lon, lat, range = 2500) => {
  if (!viewer) return
  const center = Cesium.Cartesian3.fromDegrees(lon, lat, 0)
  const sphere = new Cesium.BoundingSphere(center, 0.0)
  viewer.camera.flyToBoundingSphere(sphere, {
    offset: new Cesium.HeadingPitchRange(
      Cesium.Math.toRadians(0.0),
      Cesium.Math.toRadians(-45.0),
      range
    ),
    duration: 2.0
  })
}

// Reset camera to default region center view
const resetView = () => {
  flyToCoordinates(defaultLon, defaultLat, 35000)
}

// Public API methods exposed to parent vue components
defineExpose({
  flyToCoordinates,
  resetView
})

watch(() => props.destinations, (newVal) => {
  updateDestinations(newVal)
}, { deep: true })

watch(() => props.selectedDestinationId, (newId) => {
  if (newId) {
    const entity = markersMap.get(newId) || routeEntities.find(e => e.properties?.destinationData?.id === newId)
    if (entity) {
      const cartesian = entity.position.getValue(Cesium.JulianDate.now())
      const cartographic = Cesium.Cartographic.fromCartesian(cartesian)
      const lon = Cesium.Math.toDegrees(cartographic.longitude)
      const lat = Cesium.Math.toDegrees(cartographic.latitude)
      flyToCoordinates(lon, lat, 2000)

      // Emit clicked popup position
      setTimeout(() => {
        if (!viewer) return
        const toWindowCoords = Cesium.SceneTransforms.worldToWindowCoordinates || Cesium.SceneTransforms.wgs84ToWindowCoordinates
        const screenPos = toWindowCoords(viewer.scene, cartesian)
        if (screenPos) {
          const destData = entity.properties.destinationData.getValue()
          emit('select-destination', { destination: destData, screenPosition: screenPos })
        }
      }, 500)
    }
  }
})

watch(() => props.routeData, (newRoute) => {
  if (newRoute) {
    drawRoute(newRoute)
  } else {
    clearRouteEntities()
  }
}, { deep: true })

onMounted(() => {
  nextTick(() => {
    if (!cesiumContainer.value) return

    try {
      // Initialize Cesium Viewer
      viewer = new Cesium.Viewer(cesiumContainer.value, {
        animation: false,
        timeline: false,
        baseLayerPicker: false,
        geocoder: false,
        homeButton: false,
        navigationHelpButton: false,
        sceneModePicker: false,
        infoBox: false,
        selectionIndicator: false,
        fullscreenButton: false,
        // Let Cesium use default Ion imagery if token is set, otherwise fall back to OSM
        imageryProvider: (import.meta.env.VITE_CESIUM_ION_TOKEN && !import.meta.env.VITE_CESIUM_ION_TOKEN.includes('placeholder') && !import.meta.env.VITE_CESIUM_ION_TOKEN.includes('your_cesium'))
          ? undefined 
          : new Cesium.OpenStreetMapImageryProvider({
              url: 'https://tile.openstreetmap.org/'
            })
      })

      // Set camera default viewpoint (shifted slightly south to center Cần Thơ coordinates at -45 deg pitch)
      viewer.camera.setView({
        destination: Cesium.Cartesian3.fromDegrees(defaultLon, defaultLat - 0.22, 25000),
        orientation: {
          heading: Cesium.Math.toRadians(0.0),
          pitch: Cesium.Math.toRadians(-45.0),
          roll: 0.0
        }
      })

      // Screen space interaction click listener
      handler = new Cesium.ScreenSpaceEventHandler(viewer.scene.canvas)
      handler.setInputAction((click) => {
        const pickedObject = viewer.scene.pick(click.position)
        if (Cesium.defined(pickedObject) && Cesium.defined(pickedObject.id)) {
          const entity = pickedObject.id
          if (entity.properties && entity.properties.destinationData) {
            const destData = entity.properties.destinationData.getValue()
            const cartesian = entity.position.getValue(Cesium.JulianDate.now())
            const toWindowCoords = Cesium.SceneTransforms.worldToWindowCoordinates || Cesium.SceneTransforms.wgs84ToWindowCoordinates
            const screenPos = toWindowCoords(viewer.scene, cartesian)
            
            emit('select-destination', { destination: destData, screenPosition: screenPos })
            return
          }
        }
        emit('clear-selection')
      }, Cesium.ScreenSpaceEventType.LEFT_CLICK)

      // Post-render event listener to keep floating popup coordinates in-sync during pans/zooms
      postRenderListener = viewer.scene.postRender.addEventListener(() => {
        if (props.selectedDestinationId) {
          const entity = markersMap.get(props.selectedDestinationId) || routeEntities.find(e => e.properties?.destinationData?.id === props.selectedDestinationId)
          if (entity) {
            const cartesian = entity.position.getValue(Cesium.JulianDate.now())
            if (cartesian) {
              const toWindowCoords = Cesium.SceneTransforms.worldToWindowCoordinates || Cesium.SceneTransforms.wgs84ToWindowCoordinates
              const screenPos = toWindowCoords(viewer.scene, cartesian)
              if (screenPos) {
                emit('update-popup-position', screenPos)
              }
            }
          }
        }
      })

      viewerReady.value = true
      emit('viewer-ready')

      // Load initial destinations if already available
      if (props.destinations.length > 0) {
        updateDestinations(props.destinations)
      }

      // Load route details if already available
      if (props.routeData) {
        drawRoute(props.routeData)
      }

    } catch (err) {
      console.error('Lỗi khi khởi tạo bản đồ Cesium 3D:', err)
    }
  })
})

onBeforeUnmount(() => {
  // Clean up resources properly to prevent memory leaks
  if (handler) {
    handler.destroy()
    handler = null
  }
  if (viewer && postRenderListener) {
    viewer.scene.postRender.removeEventListener(postRenderListener)
    postRenderListener = null
  }
  if (viewer) {
    viewer.destroy()
    viewer = null
  }
  viewerReady.value = false
})
</script>

<style>
/* Hide the default Cesium bottom branding credit widget if needed, or keep it styled nicely */
.cesium-viewer-bottom {
  display: none !important;
}
</style>
