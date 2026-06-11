<template>
  <div 
    ref="containerRef"
    class="relative w-full bg-slate-950 rounded-2xl overflow-hidden border border-slate-800 shadow-inner group"
    :style="{ height: containerHeight }"
  >
    <!-- Lazy Load / Activation Placeholder -->
    <div 
      v-if="lazy && !isActivated"
      class="absolute inset-0 bg-slate-950 flex flex-col items-center justify-center space-y-4 z-20 cursor-pointer overflow-hidden group/lazy"
      @click="activateModel"
    >
      <!-- Thumbnail Background with Glassmorphism Overlay -->
      <div v-if="thumbnailUrl" class="absolute inset-0 z-0">
        <img 
          :src="thumbnailUrl" 
          class="w-full h-full object-cover opacity-30 group-hover/lazy:opacity-45 group-hover/lazy:scale-[1.03] transition-all duration-700" 
          alt="3D Model Thumbnail"
        />
        <div class="absolute inset-0 bg-gradient-to-t from-slate-950 via-slate-950/60 to-slate-950/20"></div>
      </div>
      
      <!-- Fallback sleek grid/dots background if no thumbnail -->
      <div v-else class="absolute inset-0 z-0 opacity-[0.03] bg-[linear-gradient(to_right,#ffffff_1px,transparent_1px),linear-gradient(to_bottom,#ffffff_1px,transparent_1px)] bg-[size:32px_32px]"></div>

      <!-- Centered Interactive Content -->
      <div class="relative z-10 flex flex-col items-center text-center px-6 space-y-4">
        <!-- Glowing 3D Box Icon Container -->
        <div class="relative w-16 h-16 rounded-2xl bg-teal-500/15 border border-teal-500/30 text-teal-400 flex items-center justify-center shadow-lg group-hover/lazy:border-teal-400/50 group-hover/lazy:bg-teal-500/25 transition duration-300">
          <BoxIcon class="w-8 h-8 animate-pulse text-teal-400" />
          
          <!-- Orbit Ring decoration -->
          <div class="absolute -inset-2 border border-teal-500/10 rounded-full animate-[spin_12s_linear_infinite] pointer-events-none"></div>
          <div class="absolute -inset-4 border border-dashed border-teal-500/5 rounded-full animate-[spin_20s_linear_infinite_reverse] pointer-events-none"></div>
        </div>

        <div class="space-y-1 max-w-sm">
          <h4 class="text-xs font-black uppercase text-slate-200 tracking-wider">Mô hình 3D tương tác</h4>
          <p class="text-[10px] text-slate-400 font-medium leading-relaxed">
            Nhấn để kích hoạt và khám phá mô hình 3D trong không gian đa chiều.
          </p>
        </div>

        <!-- Premium Activate Button -->
        <button 
          @click.stop="activateModel"
          class="px-5 py-2.5 bg-teal-500 hover:bg-teal-400 text-slate-950 text-[10px] font-black uppercase tracking-wider rounded-xl shadow-lg hover:shadow-teal-500/20 active:scale-95 transition-all duration-200 flex items-center space-x-2 border border-teal-400/20"
        >
          <Rotate3dIcon class="w-4 h-4 text-slate-950 animate-spin-slow" />
          <span>Kích hoạt mô hình 3D</span>
        </button>
      </div>
    </div>

    <!-- Loading State (shown until model-viewer is ready) -->
    <div 
      v-if="isActivated && isLoading && !error"
      class="absolute inset-0 bg-slate-950 flex flex-col items-center justify-center space-y-3 z-10"
    >
      <div class="relative w-14 h-14 flex items-center justify-center">
        <div class="absolute inset-0 border-4 border-teal-500/20 rounded-full"></div>
        <div class="absolute inset-0 border-4 border-t-teal-400 border-r-transparent border-b-transparent border-l-transparent rounded-full animate-spin"></div>
        <BoxIcon class="w-6 h-6 text-teal-400 animate-pulse" />
      </div>
      <div class="text-center">
        <p class="text-xs font-bold text-white tracking-wider uppercase">Đang nạp mô hình 3D</p>
        <p class="text-[10px] text-teal-300 font-medium mt-1">
          {{ progressPercent > 0 ? `Đã tải ${progressPercent}%...` : 'Vui lòng chờ trong giây lát...' }}
        </p>
      </div>
      
      <!-- Progress Bar Indicator -->
      <div class="w-40 h-1 bg-slate-800 rounded-full overflow-hidden mt-2">
        <div class="h-full bg-teal-400 transition-all duration-150" :style="{ width: `${progressPercent}%` }"></div>
      </div>
    </div>

    <!-- Error State overlay -->
    <div 
      v-if="isActivated && error" 
      class="absolute inset-0 bg-slate-950 flex flex-col items-center justify-center space-y-4 p-6 text-center z-20"
    >
      <div class="p-3 bg-rose-500/10 text-rose-400 border border-rose-500/20 rounded-2xl">
        <AlertCircleIcon class="w-8 h-8" />
      </div>
      <div class="space-y-1">
        <h4 class="text-xs font-black uppercase text-slate-200">Không tải được mô hình 3D</h4>
        <p class="text-[10px] text-slate-400 leading-relaxed max-w-xs mx-auto">
          {{ errorMessage }}
        </p>
      </div>
      <button 
        @click="retryLoad"
        class="px-4 py-2 bg-teal-500/20 hover:bg-teal-500/30 text-teal-300 text-xs font-bold rounded-xl border border-teal-500/20 transition"
      >
        Thử lại
      </button>
    </div>

    <!-- model-viewer mount point -->
    <div ref="viewerMountRef" class="w-full h-full"></div>

    <!-- Instructions overlay (shown after load) -->
    <div 
      v-if="isActivated && !isLoading && !error"
      class="absolute bottom-4 left-4 right-4 pointer-events-none z-10 select-none flex justify-center opacity-0 group-hover:opacity-100 transition-opacity duration-300"
    >
      <span class="inline-flex items-center px-3 py-1.5 rounded-xl bg-slate-950/80 backdrop-blur-md border border-white/10 text-[9px] font-black uppercase tracking-wider text-slate-300 shadow-lg">
        <Rotate3dIcon class="w-3.5 h-3.5 mr-1.5 text-teal-400" />
        Kéo chuột để xoay • Cuộn để phóng to
      </span>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, watch } from 'vue'
import { 
  Box as BoxIcon, 
  AlertCircle as AlertCircleIcon, 
  Rotate3d as Rotate3dIcon 
} from 'lucide-vue-next'

// Import model-viewer for side-effects (registers <model-viewer> custom element)
import '@google/model-viewer'

const props = defineProps({
  src: {
    type: String,
    required: true
  },
  autoRotate: {
    type: Boolean,
    default: true
  },
  thumbnailUrl: {
    type: String,
    default: null
  },
  lazy: {
    type: Boolean,
    default: false
  }
})

const containerRef = ref(null)
const viewerMountRef = ref(null)
const containerHeight = '480px'

const isActivated = ref(!props.lazy)
const isLoading = ref(true)
const error = ref(false)
const errorMessage = ref('Tệp tin 3D không tồn tại, bị lỗi định dạng hoặc kết nối bị gián đoạn.')
const progressPercent = ref(0)

let viewerElement = null

const activateModel = () => {
  isActivated.value = true
  if (props.src) {
    createViewer(props.src)
  }
}

const createViewer = (srcUrl) => {
  if (!viewerMountRef.value || !srcUrl) return

  // Cleanup previous viewer
  destroyViewer()

  isLoading.value = true
  error.value = false
  progressPercent.value = 0

  // Create model-viewer element imperatively
  viewerElement = document.createElement('model-viewer')
  viewerElement.setAttribute('src', srcUrl)
  viewerElement.setAttribute('camera-controls', '')
  viewerElement.setAttribute('shadow-intensity', '1')
  viewerElement.setAttribute('exposure', '1')
  viewerElement.setAttribute('touch-action', 'pan-y')
  viewerElement.setAttribute('loading', 'eager')
  
  if (props.autoRotate) {
    viewerElement.setAttribute('auto-rotate', '')
  }

  // Style the element with explicit, non-collapsible dimensions
  viewerElement.style.cssText = `
    display: block;
    width: 100%;
    height: 100%;
    min-height: ${containerHeight};
    background-color: #020617;
    outline: none;
  `

  // Event handlers
  viewerElement.addEventListener('load', onLoad)
  viewerElement.addEventListener('error', onError)
  viewerElement.addEventListener('progress', onProgress)

  // Append to DOM
  viewerMountRef.value.appendChild(viewerElement)

  console.log('[ModelViewer] Created viewer for:', srcUrl)
}

const destroyViewer = () => {
  if (viewerElement) {
    viewerElement.removeEventListener('load', onLoad)
    viewerElement.removeEventListener('error', onError)
    viewerElement.removeEventListener('progress', onProgress)
    viewerElement.remove()
    viewerElement = null
  }
}

const onLoad = () => {
  console.log('[ModelViewer] Model loaded successfully!')
  isLoading.value = false
  progressPercent.value = 100
}

const onProgress = (event) => {
  if (event.detail && event.detail.totalProgress != null) {
    progressPercent.value = Math.round(event.detail.totalProgress * 100)
    console.log('[ModelViewer] Progress:', progressPercent.value, '%')
  }
}

const onError = (e) => {
  console.error('[ModelViewer] Load error:', e)
  error.value = true
  isLoading.value = false
  errorMessage.value = 'Không thể tải mô hình 3D. Vui lòng kiểm tra lại URL hoặc tệp tin.'
}

const retryLoad = () => {
  createViewer(props.src)
}

// Watch for src changes
watch(() => props.src, (newSrc) => {
  if (newSrc && isActivated.value) {
    createViewer(newSrc)
  }
})

onMounted(() => {
  if (isActivated.value && props.src) {
    // Small delay to ensure DOM is ready
    setTimeout(() => {
      createViewer(props.src)
    }, 100)
  }
})

onBeforeUnmount(() => {
  destroyViewer()
})
</script>

<style scoped>
.animate-spin {
  animation: spin 1s linear infinite;
}
.animate-spin-slow {
  animation: spin 6s linear infinite;
}
@keyframes spin {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}
</style>
