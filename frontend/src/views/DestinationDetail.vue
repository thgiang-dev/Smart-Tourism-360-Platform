<template>
  <div class="space-y-8 pb-16">
    <!-- Loading State -->
    <div v-if="loading" class="max-w-7xl mx-auto px-4 py-16 sm:px-6 lg:px-8 space-y-8 animate-pulse">
      <div class="h-96 bg-slate-200 rounded-3xl w-full"></div>
      <div class="grid grid-cols-1 lg:grid-cols-12 gap-8">
        <div class="lg:col-span-8 space-y-6">
          <div class="h-6 bg-slate-200 rounded w-1/4"></div>
          <div class="h-10 bg-slate-200 rounded w-3/4"></div>
          <div class="h-40 bg-slate-200 rounded w-full"></div>
        </div>
        <div class="lg:col-span-4 h-80 bg-slate-200 rounded-2xl"></div>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="max-w-3xl mx-auto px-4 py-16 text-center space-y-4">
      <div class="inline-flex p-4 bg-red-50 text-red-600 rounded-full border border-red-100">
        <AlertCircleIcon class="w-12 h-12" />
      </div>
      <h3 class="text-lg font-bold text-slate-800">Không tải được thông tin địa điểm</h3>
      <p class="text-sm text-slate-500 max-w-sm mx-auto">{{ error }}</p>
      <router-link to="/explore" class="inline-flex text-sm font-bold text-teal-600 hover:text-teal-700 underline">
        Quay lại bản đồ khám phá
      </router-link>
    </div>

    <!-- Main Detail View (when data is successfully loaded) -->
    <div v-else-if="detail" class="space-y-10 animate-fade-in">
      
      <!-- Top Cover Image Hero Block -->
      <div class="relative w-full h-[380px] md:h-[520px] bg-slate-900 overflow-hidden shadow-2xl">
        <img 
          v-if="detail.coverImageUrl" 
          :src="detail.coverImageUrl" 
          class="w-full h-full object-cover opacity-80 scale-102 hover:scale-105 transition-all duration-700" 
          :alt="detail.name"
        />
        <div class="absolute inset-0 bg-gradient-to-t from-slate-950 via-slate-950/50 to-transparent"></div>
        <div class="absolute inset-0 bg-gradient-to-r from-slate-950/60 to-transparent pointer-events-none"></div>
        
        <!-- Back Navigation in Cover -->
        <div class="absolute top-6 left-4 md:left-8 z-10">
          <button 
            @click="goBack"
            class="premium-btn inline-flex items-center space-x-2 px-4.5 py-2.5 bg-slate-950/80 hover:bg-slate-900 backdrop-blur-md border border-slate-700/60 text-white font-extrabold rounded-2xl text-xs shadow-lg transition duration-200"
          >
            <ArrowLeftIcon class="w-4 h-4 text-slate-300" />
            <span>Quay lại</span>
          </button>
        </div>

        <!-- Meta Text overlays -->
        <div class="absolute bottom-10 left-4 md:left-8 right-4 md:right-8 z-10 max-w-4xl space-y-4">
          <div class="flex items-center space-x-3">
            <span 
              class="px-3.5 py-1 rounded-full text-[10px] font-black border uppercase tracking-wider bg-teal-500/20 border-teal-500/30 text-teal-300 backdrop-blur-md"
            >
              {{ detail.category?.name }}
            </span>
            <span class="text-xs text-slate-300 font-bold uppercase tracking-wider flex items-center space-x-1">
              <MapIcon class="w-3.5 h-3.5 text-slate-400" />
              <span>{{ detail.regionName }}</span>
            </span>
          </div>

          <h1 class="text-3xl md:text-5xl font-black text-white leading-tight drop-shadow-md">
            {{ detail.name }}
          </h1>

          <div class="flex items-center text-xs md:text-sm text-slate-200 space-x-2 font-medium bg-slate-950/20 py-1.5 px-3 rounded-xl backdrop-blur-sm inline-flex">
            <MapPinIcon class="w-4 h-4 text-teal-400" />
            <span>{{ detail.address || 'Đang cập nhật địa chỉ' }}</span>
          </div>
        </div>
      </div>

      <!-- Main Columns Grid Layout -->
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 grid grid-cols-1 lg:grid-cols-12 gap-8 items-start">
        
        <!-- Left Side Column: Details info & Gallery & Audio (8 Columns) -->
        <div class="lg:col-span-8 space-y-8">
          
          <!-- Detailed content description block -->
          <div class="bg-white p-6 md:p-10 rounded-3xl border border-slate-200/80 shadow-sm space-y-8">
            <div class="space-y-4">
              <h2 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-3">Giới thiệu tổng quan</h2>
              <p class="text-sm font-bold text-slate-700 leading-relaxed italic border-l-4 border-teal-600 pl-5 bg-teal-50/25 py-4 rounded-r-2xl">
                {{ detail.shortDescription }}
              </p>
            </div>
            
            <div 
              v-if="detail.fullDescription"
              class="text-sm text-slate-600 leading-relaxed space-y-4 whitespace-pre-line font-medium"
            >
              {{ detail.fullDescription }}
            </div>
          </div>

          <!-- Premium Audio Guide Component (renders if guides exist) -->
          <div 
            v-if="detail.audioGuides && detail.audioGuides.length > 0"
            class="bg-gradient-to-br from-slate-900 to-slate-950 p-6 md:p-8 rounded-3xl text-white shadow-xl space-y-6 border border-slate-800"
          >
            <div class="flex justify-between items-center border-b border-slate-850 pb-4">
              <div class="flex items-center space-x-3">
                <div class="p-2.5 bg-teal-500/10 text-teal-400 border border-teal-500/20 rounded-xl shadow-inner">
                  <HeadphonesIcon class="w-5 h-5 animate-pulse text-teal-400" />
                </div>
                <div>
                  <h3 class="text-xs font-black uppercase tracking-wider text-slate-200">Thuyết minh âm thanh</h3>
                  <p class="text-[10px] text-slate-400 font-bold uppercase tracking-wider">Audio Guide</p>
                </div>
              </div>
              
              <!-- Indicator of duration -->
              <span class="text-xs font-mono text-slate-300 bg-slate-800/80 px-3 py-1 rounded-xl border border-slate-800">
                {{ formatAudioDuration(audioGuidesList[0]?.duration) }}
              </span>
            </div>

            <!-- Custom player deck layout -->
            <div class="space-y-4" v-for="guide in audioGuidesList" :key="guide.id">
              <h4 class="text-xs font-black text-slate-300 uppercase tracking-wider">{{ guide.title }}</h4>
              
              <div class="flex items-center space-x-4 bg-slate-950/40 p-4 rounded-2xl border border-slate-900">
                <!-- Action Play button -->
                <button 
                  @click="toggleAudioPlay(guide.audioUrl)"
                  class="premium-btn w-12 h-12 rounded-full bg-teal-500 hover:bg-teal-400 text-slate-950 font-black flex items-center justify-center shadow-lg hover:shadow-teal-500/25"
                >
                  <PauseIcon v-if="audioPlaying && activeAudioUrl === guide.audioUrl" class="w-5 h-5 fill-slate-950" />
                  <PlayIcon v-else class="w-5 h-5 fill-slate-950 translate-x-[1px]" />
                </button>

                <!-- Scrubber Slider -->
                <div class="flex-1 space-y-1.5">
                  <div class="flex justify-between text-[10px] text-slate-400 font-mono">
                    <span>{{ formatTime(audioCurrentTime) }}</span>
                    <span>{{ formatTime(audioDuration) }}</span>
                  </div>
                  
                  <div 
                    class="relative w-full h-1.5 bg-slate-800 rounded-full cursor-pointer overflow-hidden shadow-inner"
                    @click="seekAudio"
                    ref="progressBarRef"
                  >
                    <div 
                      class="bg-teal-400 h-1.5 transition-all duration-100"
                      :style="{ width: `${audioProgress}%` }"
                    ></div>
                  </div>
                </div>
              </div>

              <!-- Audio tag hidden -->
              <audio 
                ref="audioTagRef" 
                class="hidden" 
                :src="guide.audioUrl"
                @timeupdate="onAudioTimeUpdate"
                @loadedmetadata="onAudioMetadataLoaded"
                @ended="onAudioEnded"
              ></audio>

              <!-- Collapsible Transcript accordion -->
              <div v-if="guide.description" class="border border-slate-850 rounded-2xl overflow-hidden bg-slate-950/25">
                <button 
                  @click="showTranscript = !showTranscript"
                  class="w-full px-5 py-3.5 bg-slate-950/40 hover:bg-slate-950 text-xs font-black text-slate-300 flex justify-between items-center transition duration-200 border-b border-transparent focus:outline-none"
                  :class="showTranscript ? 'border-slate-850' : ''"
                >
                  <span>Xem văn bản thuyết minh (Transcript)</span>
                  <ChevronDownIcon class="w-4 h-4 text-slate-400 transition-transform duration-200" :class="showTranscript ? 'rotate-180' : ''" />
                </button>
                <div 
                  v-if="showTranscript" 
                  class="p-5 text-xs text-slate-400 leading-relaxed max-h-48 overflow-y-auto whitespace-pre-line"
                >
                  {{ guide.description }}
                </div>
              </div>
            </div>
          </div>

          <!-- Quick Information Cards Block -->
          <div class="bg-white p-6 md:p-10 rounded-3xl border border-slate-200/80 shadow-sm space-y-6">
            <h2 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-3">Thông tin chi tiết</h2>
            
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-8 text-xs leading-relaxed font-semibold">
              <div class="space-y-5">
                <div class="flex items-start space-x-3.5">
                  <div class="p-2 bg-slate-50 border border-slate-200/60 rounded-xl text-slate-600 shadow-sm flex-shrink-0">
                    <ClockIcon class="w-4 h-4 text-teal-600" />
                  </div>
                  <div class="space-y-0.5">
                    <span class="text-slate-400 font-bold uppercase tracking-wider block text-[10px]">Giờ hoạt động</span>
                    <span class="font-extrabold text-slate-800 text-sm">{{ detail.openingHours || 'Mở cửa tự do' }}</span>
                  </div>
                </div>
                
                <div class="flex items-start space-x-3.5">
                  <div class="p-2 bg-slate-50 border border-slate-200/60 rounded-xl text-slate-600 shadow-sm flex-shrink-0">
                    <TicketIcon class="w-4 h-4 text-teal-600" />
                  </div>
                  <div class="space-y-0.5">
                    <span class="text-slate-400 font-bold uppercase tracking-wider block text-[10px]">Giá vé tham khảo</span>
                    <span class="font-extrabold text-slate-800 text-sm">{{ detail.ticketPrice || 'Miễn phí' }}</span>
                  </div>
                </div>
              </div>

              <div class="space-y-5">
                <div v-if="detail.contactPhone" class="flex items-start space-x-3.5">
                  <div class="p-2 bg-slate-50 border border-slate-200/60 rounded-xl text-slate-600 shadow-sm flex-shrink-0">
                    <PhoneIcon class="w-4 h-4 text-teal-600" />
                  </div>
                  <div class="space-y-0.5">
                    <span class="text-slate-400 font-bold uppercase tracking-wider block text-[10px]">Điện thoại</span>
                    <a :href="`tel:${detail.contactPhone}`" class="font-extrabold text-teal-600 hover:text-teal-700 text-sm">{{ detail.contactPhone }}</a>
                  </div>
                </div>

                <div v-if="detail.contactEmail" class="flex items-start space-x-3.5">
                  <div class="p-2 bg-slate-50 border border-slate-200/60 rounded-xl text-slate-600 shadow-sm flex-shrink-0">
                    <MailIcon class="w-4 h-4 text-teal-600" />
                  </div>
                  <div class="space-y-0.5">
                    <span class="text-slate-400 font-bold uppercase tracking-wider block text-[10px]">Email liên hệ</span>
                    <a :href="`mailto:${detail.contactEmail}`" class="font-extrabold text-teal-600 hover:text-teal-700 text-sm truncate block max-w-[200px]">{{ detail.contactEmail }}</a>
                  </div>
                </div>

                <div v-if="detail.websiteUrl" class="flex items-start space-x-3.5">
                  <div class="p-2 bg-slate-50 border border-slate-200/60 rounded-xl text-slate-600 shadow-sm flex-shrink-0">
                    <GlobeIcon class="w-4 h-4 text-teal-600" />
                  </div>
                  <div class="space-y-0.5">
                    <span class="text-slate-400 font-bold uppercase tracking-wider block text-[10px]">Website chính thức</span>
                    <a :href="detail.websiteUrl" target="_blank" class="font-extrabold text-teal-600 hover:text-teal-700 underline underline-offset-4 text-sm truncate max-w-[200px] block">{{ detail.websiteUrl }}</a>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Photo Gallery (images) -->
          <div 
            v-if="detail.gallery && detail.gallery.length > 0"
            class="bg-white p-6 md:p-10 rounded-3xl border border-slate-200/80 shadow-sm space-y-5"
          >
            <h2 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-3">Thư viện hình ảnh</h2>
            
            <div class="grid grid-cols-2 sm:grid-cols-3 gap-5">
              <div 
                v-for="(img, idx) in detail.gallery" 
                :key="img.id"
                class="aspect-[4/3] rounded-2xl overflow-hidden cursor-pointer border border-slate-200/80 hover:border-teal-500/40 relative group bg-slate-100 transition duration-300"
                @click="openLightbox(idx)"
              >
                <img :src="img.url" class="w-full h-full object-cover group-hover:scale-105 transition duration-500" />
                <div class="absolute inset-0 bg-slate-950/40 opacity-0 group-hover:opacity-100 transition flex items-end p-4">
                  <p class="text-[10px] text-white font-black truncate w-full shadow-sm">{{ img.caption || 'Xem ảnh lớn' }}</p>
                </div>
              </div>
            </div>
          </div>

          <!-- Video presentation (MP4) -->
          <div 
            v-if="detail.videos && detail.videos.length > 0"
            class="bg-white p-6 md:p-10 rounded-3xl border border-slate-200/80 shadow-sm space-y-5"
          >
            <h2 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-3">Video giới thiệu</h2>
            
            <div class="space-y-6">
              <div v-for="vid in detail.videos" :key="vid.id" class="rounded-3xl overflow-hidden shadow-md border border-slate-200 bg-slate-950 aspect-video">
                <video controls class="w-full h-full" :src="vid.url"></video>
                <div v-if="vid.caption" class="p-4 bg-white text-xs text-slate-600 font-bold border-t border-slate-100">
                  {{ vid.caption }}
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Right Side Column: CTA Tour & Leaflet Position Map (4 Columns) -->
        <div class="lg:col-span-4 space-y-6">
          
          <!-- Virtual Tour 360 Call to Action Button Card -->
          <div 
            v-if="detail.hasVirtualTour"
            class="bg-gradient-to-r from-teal-500 to-emerald-600 p-6 md:p-8 rounded-3xl text-slate-950 shadow-xl space-y-5 text-center border border-teal-400/20 animate-pulse-glow transition duration-300 hover:scale-101"
          >
            <div class="w-16 h-16 bg-slate-950/10 rounded-2xl flex items-center justify-center mx-auto text-slate-950 shadow-inner">
              <CompassIcon class="w-9 h-9 animate-spin-slow text-slate-950" />
            </div>
            <div class="space-y-2">
              <h3 class="text-xl font-black uppercase tracking-wider text-slate-950">Trải nghiệm Tour 360°</h3>
              <p class="text-xs text-slate-900/85 font-extrabold leading-relaxed">Tham quan không gian thực tế ảo tương tác trực tiếp</p>
            </div>
            
            <router-link
              :to="`/destinations/${detail.id}/tour`"
              class="premium-btn w-full py-3.5 bg-slate-950 hover:bg-slate-900 text-white font-black rounded-2xl text-xs flex items-center justify-center space-x-2 shadow-lg"
            >
              <span>Vào tham quan ảo</span>
              <CompassIcon class="w-4 h-4 fill-white" />
            </router-link>
          </div>

          <!-- Position display card -->
          <div class="bg-white p-6 rounded-3xl border border-slate-200/80 shadow-sm space-y-5">
            <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-3">Định vị địa lý</h3>
            
            <!-- Map box Leaflet -->
            <div class="h-64 rounded-2xl overflow-hidden border border-slate-200 bg-slate-100 relative z-10">
              <div ref="miniMapContainer" class="w-full h-full"></div>
            </div>

            <!-- Latitude longitude logs -->
            <div class="grid grid-cols-2 gap-4 text-[10px] font-mono text-slate-500 bg-slate-50 p-3.5 rounded-2xl border border-slate-150 shadow-inner">
              <div>
                <span class="text-slate-400 block uppercase font-black mb-0.5">Vĩ độ (Lat)</span>
                <span class="font-extrabold text-slate-700">{{ detail.latitude }}</span>
              </div>
              <div>
                <span class="text-slate-400 block uppercase font-black mb-0.5">Kinh độ (Lng)</span>
                <span class="font-extrabold text-slate-700">{{ detail.longitude }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Gallery Lightbox Modal Carousel Overlay -->
    <div 
      v-if="lightboxOpen" 
      class="fixed inset-0 z-50 bg-slate-950/90 backdrop-blur-sm flex items-center justify-center p-4"
      @click.self="closeLightbox"
      @keydown.esc="closeLightbox"
      tabindex="0"
      ref="lightboxRef"
    >
      <button 
        @click="closeLightbox"
        class="absolute top-6 right-6 p-2 bg-slate-800 text-white rounded-full hover:bg-slate-700 transition"
      >
        <XIcon class="w-5 h-5" />
      </button>

      <!-- Carousel Content -->
      <div class="flex items-center justify-between w-full max-w-5xl gap-4">
        <!-- Prev button -->
        <button 
          @click="prevLightboxImage"
          class="p-3 bg-slate-800 hover:bg-slate-700 text-white rounded-full transition active:scale-95"
        >
          <ChevronLeftIcon class="w-6 h-6" />
        </button>

        <!-- Current image preview -->
        <div class="text-center space-y-4 flex-1">
          <img 
            :src="detail.gallery[lightboxIndex]?.url" 
            class="max-w-full max-h-[75vh] object-contain mx-auto rounded-lg shadow-2xl" 
          />
          <p class="text-sm font-bold text-white max-w-xl mx-auto">
            {{ detail.gallery[lightboxIndex]?.caption || '' }}
          </p>
        </div>

        <!-- Next button -->
        <button 
          @click="nextLightboxImage"
          class="p-3 bg-slate-800 hover:bg-slate-700 text-white rounded-full transition active:scale-95"
        >
          <ChevronRightIcon class="w-6 h-6" />
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, nextTick } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../utils/api'
import L from 'leaflet'
import { 
  AlertCircle as AlertCircleIcon,
  MapPin as MapPinIcon,
  Clock as ClockIcon,
  Ticket as TicketIcon,
  Phone as PhoneIcon,
  Mail as MailIcon,
  Globe as GlobeIcon,
  Image as ImageIcon,
  Compass as CompassIcon,
  Music as MusicIcon,
  ArrowLeft as ArrowLeftIcon,
  X as XIcon,
  ChevronLeft as ChevronLeftIcon,
  ChevronRight as ChevronRightIcon,
  Play as PlayIcon,
  Pause as PauseIcon,
  Headphones as HeadphonesIcon,
  Map as MapIcon,
  ChevronDown as ChevronDownIcon
} from 'lucide-vue-next'

// Redefine Leaflet default icon paths to prevent broken marker images under Vite/bundler
delete L.Icon.Default.prototype._getIconUrl
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png',
  iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
  shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png'
})

const route = useRoute()
const router = useRouter()

const loading = ref(false)
const error = ref(null)
const detail = ref(null)

// Gallery Lightbox Controls
const lightboxOpen = ref(false)
const lightboxIndex = ref(0)
const lightboxRef = ref(null)

// Custom Audio Guide controls
const audioTagRef = ref(null)
const audioPlaying = ref(false)
const activeAudioUrl = ref(null)
const audioCurrentTime = ref(0)
const audioDuration = ref(0)
const audioProgress = ref(0)
const progressBarRef = ref(null)
const showTranscript = ref(false)

const audioGuidesList = ref([])

// Mini position Map
const miniMapContainer = ref(null)
let miniMap = null
let miniMapMarker = null

const fetchDestinationDetail = async () => {
  loading.value = true
  error.value = null
  try {
    const id = route.params.id
    const response = await api.get(`/api/destinations/${id}`)
    if (response.success) {
      detail.value = response.data
      audioGuidesList.value = response.data.audioGuides || []
    }
  } catch (err) {
    error.value = err.message || 'Không thể tải thông tin chi tiết địa điểm.'
  } finally {
    loading.value = false
  }

  if (detail.value) {
    nextTick(() => {
      initMiniMap()
    })
  }
}

const initMiniMap = () => {
  if (!miniMapContainer.value || !detail.value) return
  
  const lat = parseFloat(detail.value.latitude)
  const lng = parseFloat(detail.value.longitude)
  
  if (isNaN(lat) || isNaN(lng)) return

  miniMap = L.map(miniMapContainer.value, {
    zoomControl: false,
    dragging: false,
    scrollWheelZoom: false
  }).setView([lat, lng], 15)

  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
  }).addTo(miniMap)

  miniMapMarker = L.marker([lat, lng]).addTo(miniMap)

  setTimeout(() => {
    if (miniMap) miniMap.invalidateSize()
  }, 300)
}

// Lightbox modal carousel controls
const openLightbox = (index) => {
  lightboxIndex.value = index
  lightboxOpen.value = true
  nextTick(() => {
    lightboxRef.value?.focus()
  })
}

const closeLightbox = () => {
  lightboxOpen.value = false
}

const prevLightboxImage = () => {
  const listLen = detail.value.gallery.length
  lightboxIndex.value = (lightboxIndex.value - 1 + listLen) % listLen
}

const nextLightboxImage = () => {
  const listLen = detail.value.gallery.length
  lightboxIndex.value = (lightboxIndex.value + 1) % listLen
}

// Audio Player controls
const toggleAudioPlay = (url) => {
  const audio = audioTagRef.value?.[0]
  if (!audio) return

  if (activeAudioUrl.value !== url) {
    activeAudioUrl.value = url
    audioCurrentTime.value = 0
    audioProgress.value = 0
    audio.src = url
    audio.play()
    audioPlaying.value = true
  } else {
    if (audioPlaying.value) {
      audio.pause()
      audioPlaying.value = false
    } else {
      audio.play()
      audioPlaying.value = true
    }
  }
}

const onAudioTimeUpdate = () => {
  const audio = audioTagRef.value?.[0]
  if (!audio) return
  audioCurrentTime.value = audio.currentTime
  audioProgress.value = (audio.currentTime / audio.duration) * 100
}

const onAudioMetadataLoaded = () => {
  const audio = audioTagRef.value?.[0]
  if (!audio) return
  audioDuration.value = audio.duration
}

const onAudioEnded = () => {
  audioPlaying.value = false
  audioCurrentTime.value = 0
  audioProgress.value = 0
}

const seekAudio = (e) => {
  const audio = audioTagRef.value?.[0]
  const bar = progressBarRef.value?.[0]
  if (!audio || !bar) return

  const rect = bar.getBoundingClientRect()
  const clickX = e.clientX - rect.left
  const width = rect.width
  const percent = Math.min(Math.max(clickX / width, 0), 1)
  
  audio.currentTime = percent * audio.duration
  audioCurrentTime.value = audio.currentTime
  audioProgress.value = percent * 100
}

const formatAudioDuration = (seconds) => {
  if (!seconds) return 'Đang cập nhật'
  const mins = Math.floor(seconds / 60)
  const secs = seconds % 60
  return `${mins} phút ${secs > 0 ? secs + ' giây' : ''}`
}

const formatTime = (secs) => {
  if (isNaN(secs)) return '0:00'
  const minutes = Math.floor(secs / 60)
  const seconds = Math.floor(secs % 60)
  return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`
}

const goBack = () => {
  if (window.history.length > 1) {
    router.back()
  } else {
    router.push('/explore')
  }
}

onBeforeUnmount(() => {
  if (miniMap) {
    miniMap.remove()
    miniMap = null
  }
  const audio = audioTagRef.value?.[0]
  if (audio) {
    audio.pause()
  }
})

onMounted(() => {
  fetchDestinationDetail()
})
</script>

<style scoped>
.animate-spin-slow {
  animation: spin 8s linear infinite;
}
@keyframes spin {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}

.animate-pulse-glow {
  box-shadow: 0 0 15px rgba(20, 184, 166, 0.3);
  animation: pulse-glow 2.5s infinite alternate ease-in-out;
}

@keyframes pulse-glow {
  0% {
    box-shadow: 0 0 10px rgba(20, 184, 166, 0.2), 0 0 5px rgba(16, 185, 129, 0.1);
    transform: scale(1);
  }
  100% {
    box-shadow: 0 0 20px rgba(20, 184, 166, 0.5), 0 0 12px rgba(16, 185, 129, 0.3);
    transform: scale(1.01);
  }
}
</style>
