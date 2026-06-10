<template>
  <div class="h-screen w-screen bg-slate-950 text-white relative overflow-hidden select-none font-sans">
    <!-- Main 360 Canvas Viewer -->
    <div ref="viewerContainer" class="w-full h-full"></div>

    <!-- Full-screen Loading Overlay -->
    <Transition name="fade">
      <div 
        v-if="loadingScene" 
        class="absolute inset-0 bg-slate-950/90 backdrop-blur-md z-50 flex flex-col items-center justify-center space-y-4"
      >
        <div class="relative w-20 h-20 flex items-center justify-center">
          <div class="absolute inset-0 border-4 border-teal-500/20 rounded-full"></div>
          <div class="absolute inset-0 border-4 border-t-teal-400 border-r-transparent border-b-transparent border-l-transparent rounded-full animate-spin"></div>
          <CompassIcon class="w-8 h-8 text-teal-400 animate-pulse" />
        </div>
        <div class="text-center space-y-2 max-w-sm px-6">
          <h3 class="text-lg font-bold text-white uppercase tracking-wider">Đang tải không gian</h3>
          <p class="text-sm text-teal-300 font-semibold">{{ activePanorama?.title || 'Đang chuẩn bị...' }}</p>
          <p class="text-xs text-slate-500 leading-relaxed">Vui lòng chờ trong giây lát khi hình ảnh góc nhìn 360° được tải về thiết bị của bạn.</p>
        </div>
      </div>
    </Transition>

    <!-- Header Control Bar -->
    <header class="absolute top-0 left-0 right-0 p-4 md:p-6 flex justify-between items-start pointer-events-none z-20 bg-gradient-to-b from-slate-950/80 via-slate-950/30 to-transparent">
      <!-- Left Info -->
      <div class="flex items-center space-x-3 pointer-events-auto bg-slate-950/60 backdrop-blur-md border border-white/10 px-4 py-2.5 rounded-2xl shadow-xl">
        <button 
          @click="exitTour" 
          class="flex items-center space-x-1.5 px-3 py-1.5 bg-white/10 hover:bg-white/20 text-white font-bold rounded-xl text-xs transition active:scale-95"
        >
          <ChevronLeftIcon class="w-4 h-4" />
          <span>Thoát</span>
        </button>
        <span class="text-white/20 font-light">|</span>
        <div class="max-w-[200px] md:max-w-md">
          <h1 class="text-xs md:text-sm font-black text-white truncate uppercase tracking-wide">
            {{ tour?.title || 'Trình xem 360°' }}
          </h1>
          <p class="text-[10px] md:text-xs text-teal-300 font-medium truncate">
            {{ activePanorama?.title || 'Đang nạp...' }}
          </p>
        </div>
      </div>

      <!-- Right Help & Info Panel -->
      <div class="flex items-center space-x-2 pointer-events-auto">
        <button 
          @click="showHelp = !showHelp" 
          class="p-2.5 bg-slate-950/60 backdrop-blur-md border border-white/10 hover:bg-slate-900 rounded-xl text-slate-300 hover:text-white transition shadow-xl"
          title="Hướng dẫn sử dụng"
        >
          <HelpIcon class="w-4 h-4" />
        </button>
      </div>
    </header>

    <!-- Side Zoom Controls (Floating Right) -->
    <div class="absolute right-4 top-1/2 -translate-y-1/2 flex flex-col space-y-2 z-20">
      <button 
        @click="zoomIn" 
        class="w-10 h-10 bg-slate-950/60 backdrop-blur-md border border-white/10 hover:bg-slate-900 text-white font-bold rounded-xl flex items-center justify-center shadow-xl transition active:scale-95"
        title="Phóng to"
      >
        <span class="text-lg">+</span>
      </button>
      <button 
        @click="zoomOut" 
        class="w-10 h-10 bg-slate-950/60 backdrop-blur-md border border-white/10 hover:bg-slate-900 text-white font-bold rounded-xl flex items-center justify-center shadow-xl transition active:scale-95"
        title="Thu nhỏ"
      >
        <span class="text-lg">-</span>
      </button>
      <button 
        @click="resetView" 
        class="w-10 h-10 bg-slate-950/60 backdrop-blur-md border border-white/10 hover:bg-slate-900 text-white rounded-xl flex items-center justify-center shadow-xl transition active:scale-95"
        title="Reset góc nhìn"
      >
        <CompassIcon class="w-4 h-4" />
      </button>
      <button 
        @click="toggleFullscreen" 
        class="w-10 h-10 bg-slate-950/60 backdrop-blur-md border border-white/10 hover:bg-slate-900 text-white rounded-xl flex items-center justify-center shadow-xl transition active:scale-95"
        :title="isFullscreen ? 'Thoát toàn màn hình' : 'Toàn màn hình'"
      >
        <MinimizeIcon v-if="isFullscreen" class="w-4 h-4 text-teal-400" />
        <MaximizeIcon v-else class="w-4 h-4" />
      </button>
    </div>

    <!-- Toggle Scene List Drawer Button (Floating Left Bottom) -->
    <div class="absolute left-4 bottom-4 z-20">
      <button 
        @click="toggleSceneList" 
        class="flex items-center space-x-2 px-4 py-3 bg-slate-950/80 backdrop-blur-md border border-white/10 hover:bg-slate-900 text-white font-bold rounded-2xl shadow-xl transition active:scale-95"
      >
        <MenuIcon class="w-4 h-4 text-teal-400" />
        <span class="text-xs uppercase tracking-wider">Danh sách cảnh</span>
        <span class="px-2 py-0.5 bg-teal-500/20 text-teal-300 border border-teal-500/30 rounded-lg text-[9px] font-bold">
          {{ tour?.panoramas?.length || 0 }}
        </span>
      </button>
    </div>

    <!-- Floating Audio Guide Player Overlay (Bottom Center) -->
    <Transition name="slide-up">
      <div 
        v-if="activeAudio" 
        class="absolute bottom-20 md:bottom-6 left-1/2 -translate-x-1/2 w-[calc(100%-32px)] max-w-lg bg-slate-950/90 backdrop-blur-md border border-teal-500/30 rounded-3xl p-4 shadow-2xl z-30 flex items-center space-x-4"
      >
        <!-- Equalizer animation/Icon -->
        <button 
          @click="toggleAudioPlayback"
          class="w-12 h-12 bg-teal-500 hover:bg-teal-400 text-slate-950 font-black rounded-full flex items-center justify-center flex-shrink-0 shadow-lg transition active:scale-95"
        >
          <PauseIcon v-if="audioPlaying" class="w-5 h-5 fill-slate-950" />
          <PlayIcon v-else class="w-5 h-5 fill-slate-950 translate-x-[1px]" />
        </button>

        <div class="flex-grow space-y-1.5 overflow-hidden">
          <div class="flex justify-between items-center pr-2">
            <div class="overflow-hidden">
              <span class="text-[10px] font-bold uppercase tracking-wider text-teal-400 block">Đang phát thuyết minh</span>
              <span class="text-xs font-bold text-white block truncate">{{ activeAudio.title }}</span>
            </div>
            <!-- Live equalizer dots -->
            <div v-if="audioPlaying" class="flex items-end space-x-0.5 h-3.5 pb-0.5 flex-shrink-0">
              <div class="w-0.5 bg-teal-400 animate-eq-bar-1 rounded-full"></div>
              <div class="w-0.5 bg-teal-400 animate-eq-bar-2 rounded-full"></div>
              <div class="w-0.5 bg-teal-400 animate-eq-bar-3 rounded-full"></div>
            </div>
          </div>

          <!-- Progress track -->
          <div class="flex items-center space-x-2">
            <span class="text-[9px] font-mono text-slate-400 select-none">{{ formatTime(audioCurrentTime) }}</span>
            <div 
              class="flex-grow relative h-1.5 bg-slate-800 rounded-full cursor-pointer overflow-hidden"
              @click="seekAudio"
              ref="audioProgressBarRef"
            >
              <div 
                class="bg-teal-400 h-1.5 rounded-full transition-all duration-100"
                :style="{ width: `${audioProgress}%` }"
              ></div>
            </div>
            <span class="text-[9px] font-mono text-slate-400 select-none">{{ formatTime(audioDuration) }}</span>
          </div>
        </div>

        <!-- Close / Dismiss Player -->
        <button 
          @click="stopAudio"
          class="p-2 hover:bg-white/10 rounded-xl text-slate-400 hover:text-white transition flex-shrink-0"
        >
          <XIcon class="w-4 h-4" />
        </button>

        <audio 
          ref="audioRef" 
          class="hidden" 
          :src="activeAudio.mediaUrl"
          @timeupdate="onAudioTimeUpdate"
          @loadedmetadata="onAudioMetadataLoaded"
          @ended="onAudioEnded"
        ></audio>
      </div>
    </Transition>

    <!-- Scene List Bottom Drawer Drawer -->
    <Transition name="slide-up">
      <div 
        v-if="showSceneList" 
        class="absolute bottom-0 left-0 right-0 bg-slate-950/95 backdrop-blur-md border-t border-white/10 rounded-t-[32px] p-6 shadow-2xl z-40 max-h-[280px] flex flex-col"
      >
        <!-- Drawer Drag/Title Bar -->
        <div class="flex justify-between items-center mb-4">
          <div class="space-y-0.5">
            <h3 class="text-xs font-bold uppercase tracking-wider text-teal-400">Không gian tham quan</h3>
            <p class="text-[10px] text-slate-400">Chọn góc ngắm cảnh khác của địa danh này</p>
          </div>
          <button 
            @click="showSceneList = false" 
            class="p-2 hover:bg-white/10 rounded-xl text-slate-400 hover:text-white transition"
          >
            <XIcon class="w-4 h-4" />
          </button>
        </div>

        <!-- Scrollable Horizontal List -->
        <div class="flex-grow overflow-x-auto overflow-y-hidden py-1 pr-4">
          <div class="flex space-x-4 min-w-full">
            <div 
              v-for="p in tour?.panoramas" 
              :key="p.id"
              @click="switchSceneDirectly(p)"
              :class="[
                'w-40 md:w-48 aspect-[16/10] rounded-2xl overflow-hidden cursor-pointer relative group flex-shrink-0 border-2 transition duration-200',
                activePanorama?.id === p.id ? 'border-teal-400 scale-[1.02] shadow-[0_0_15px_rgba(20,184,166,0.3)]' : 'border-white/5 hover:border-white/20'
              ]"
            >
              <!-- Image Cover -->
              <img 
                v-if="p.panoramaImageUrl" 
                :src="p.panoramaImageUrl" 
                class="w-full h-full object-cover group-hover:scale-105 transition duration-300 opacity-60 group-hover:opacity-80" 
              />
              <div v-else class="w-full h-full bg-slate-900 flex items-center justify-center">
                <CompassIcon class="w-8 h-8 text-slate-700" />
              </div>

              <!-- Title overlay -->
              <div class="absolute inset-0 bg-gradient-to-t from-slate-950/90 via-slate-950/30 to-transparent flex items-end p-3">
                <div class="overflow-hidden w-full">
                  <span class="text-[10px] font-bold text-teal-300 block uppercase tracking-wide truncate">
                    {{ activePanorama?.id === p.id ? 'Hiện tại' : `Cảnh ${p.displayOrder + 1}` }}
                  </span>
                  <span class="text-xs font-bold text-white block truncate">{{ p.title }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </Transition>

    <!-- Help Modal Overlay -->
    <Transition name="fade">
      <div 
        v-if="showHelp" 
        class="fixed inset-0 z-50 bg-slate-950/80 backdrop-blur-sm flex items-center justify-center p-4"
        @click.self="closeHelp"
      >
        <div class="bg-slate-900 border border-slate-800 text-white rounded-3xl w-full max-w-md shadow-2xl p-6 relative">
          <button 
            @click="closeHelp" 
            class="absolute top-4 right-4 p-1.5 hover:bg-white/10 rounded-xl text-slate-400 hover:text-white transition"
          >
            <XIcon class="w-4 h-4" />
          </button>
          
          <div class="space-y-4 text-center">
            <div class="w-12 h-12 bg-teal-500/10 text-teal-400 border border-teal-500/20 rounded-2xl flex items-center justify-center mx-auto mb-2">
              <CompassIcon class="w-6 h-6 animate-spin-slow" />
            </div>
            
            <h3 class="text-base font-bold uppercase tracking-wider text-slate-200">Hướng dẫn tham quan</h3>
            
            <div class="space-y-3 text-left text-xs text-slate-400 leading-relaxed pt-2">
              <div class="flex items-start space-x-2.5">
                <span class="w-5 h-5 rounded-full bg-slate-800 border border-slate-700 text-teal-400 font-bold flex items-center justify-center flex-shrink-0">1</span>
                <p>Nhấn và giữ chuột trái (hoặc vuốt trên màn hình điện thoại) để xoay camera xung quanh không gian 360°.</p>
              </div>
              <div class="flex items-start space-x-2.5">
                <span class="w-5 h-5 rounded-full bg-slate-800 border border-slate-700 text-teal-400 font-bold flex items-center justify-center flex-shrink-0">2</span>
                <p>Sử dụng nút cuộn chuột (hoặc chạm zoom hai ngón tay) để phóng to / thu nhỏ góc quan sát chi tiết.</p>
              </div>
              <div class="flex items-start space-x-2.5">
                <span class="w-5 h-5 rounded-full bg-slate-800 border border-slate-700 text-teal-400 font-bold flex items-center justify-center flex-shrink-0">3</span>
                <p>Nhấp vào các biểu tượng <b class="text-emerald-400">MapPin (Emerald)</b> để di chuyển sang các vị trí cảnh lân cận.</p>
              </div>
              <div class="flex items-start space-x-2.5">
                <span class="w-5 h-5 rounded-full bg-slate-800 border border-slate-700 text-teal-400 font-bold flex items-center justify-center flex-shrink-0">4</span>
                <p>Nhấp vào các điểm hotspot màu sắc khác để mở thông tin thuyết minh, audio thuyết minh, video hoặc hình ảnh xem trước tại chỗ.</p>
              </div>
            </div>

            <button 
              @click="closeHelp" 
              class="w-full py-2.5 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold text-xs rounded-xl transition shadow-lg"
            >
              Bắt đầu tham quan
            </button>
          </div>
        </div>
      </div>
    </Transition>

    <!-- General Content Lightbox Modal (For Info, Image, Video, Model3D) -->
    <Transition name="fade">
      <div 
        v-if="previewingHotspot"
        class="fixed inset-0 z-50 bg-slate-950/80 backdrop-blur-md flex items-center justify-center p-4"
        @click.self="closePreview"
      >
        <div class="bg-slate-900 border border-slate-800 text-white rounded-3xl w-full max-w-lg shadow-2xl overflow-hidden flex flex-col max-h-[85vh]">
          <!-- Modal Header -->
          <div class="p-5 border-b border-slate-800 flex justify-between items-center bg-slate-950/40">
            <div>
              <span class="text-[9px] font-bold uppercase tracking-wider text-teal-400 block mb-0.5">{{ previewingHotspot.type }}</span>
              <h4 class="font-black text-sm text-white">{{ previewingHotspot.title }}</h4>
            </div>
            <button @click="closePreview" class="p-1.5 hover:bg-white/10 rounded-xl text-slate-400 hover:text-white transition">
              <XIcon class="w-5 h-5" />
            </button>
          </div>

          <!-- Modal Body -->
          <div class="p-6 overflow-y-auto space-y-4 flex flex-col">
            <!-- Text Description if present -->
            <p v-if="previewingHotspot.description" class="text-xs text-slate-300 leading-relaxed whitespace-pre-line bg-slate-950/40 p-4 rounded-xl border border-white/5">
              {{ previewingHotspot.description }}
            </p>

            <!-- Image preview -->
            <div v-if="previewingHotspot.type === 'image' && previewingHotspot.mediaUrl" class="w-full rounded-2xl overflow-hidden bg-black border border-slate-800 flex items-center justify-center aspect-[4/3]">
              <img :src="previewingHotspot.mediaUrl" class="w-full h-full object-contain" />
            </div>

            <!-- Video player -->
            <div v-else-if="previewingHotspot.type === 'video' && previewingHotspot.mediaUrl" class="w-full rounded-2xl overflow-hidden bg-black border border-slate-800 flex items-center justify-center aspect-video">
              <video :src="previewingHotspot.mediaUrl" controls autoplay class="w-full h-full"></video>
            </div>

            <!-- 3D Model Placeholder -->
            <div v-else-if="previewingHotspot.type === 'model3d'" class="text-center py-12 space-y-4 border border-dashed border-teal-500/20 bg-teal-500/[0.02] rounded-2xl">
              <CompassIcon class="w-12 h-12 mx-auto animate-spin text-teal-400" />
              <div class="space-y-1">
                <p class="text-xs font-bold text-slate-200">Mô hình 3D thực tế tăng cường</p>
                <p class="text-[10px] text-slate-500">Tính năng đang được chuẩn bị và sẽ triển khai tại Sprint 10.</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </Transition>

    <!-- Error/No Tour State View -->
    <div v-if="error" class="absolute inset-0 bg-slate-950 flex flex-col items-center justify-center p-6 z-50 text-center space-y-6">
      <div class="inline-flex p-4 bg-red-500/10 text-red-400 rounded-full border border-red-500/20">
        <AlertCircleIcon class="w-12 h-12" />
      </div>
      <div class="space-y-2 max-w-sm">
        <h3 class="text-lg font-black uppercase tracking-wider text-slate-100">Không nạp được Tour 360°</h3>
        <p class="text-xs text-slate-400 leading-relaxed">{{ error }}</p>
      </div>
      <button 
        @click="exitTour"
        class="px-6 py-2.5 bg-white/10 hover:bg-white/20 text-white font-bold rounded-xl text-xs transition active:scale-95 border border-white/10 shadow-md"
      >
        Quay lại trang chi tiết
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, nextTick, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../utils/api'
import { Viewer } from '@photo-sphere-viewer/core'
import { MarkersPlugin } from '@photo-sphere-viewer/markers-plugin'
import '@photo-sphere-viewer/core/index.css'
import '@photo-sphere-viewer/markers-plugin/index.css'

import {
  Compass as CompassIcon,
  HelpCircle as HelpIcon,
  ChevronLeft as ChevronLeftIcon,
  X as XIcon,
  Menu as MenuIcon,
  Play as PlayIcon,
  Pause as PauseIcon,
  AlertCircle as AlertCircleIcon,
  Maximize as MaximizeIcon,
  Minimize as MinimizeIcon
} from 'lucide-vue-next'

const route = useRoute()
const router = useRouter()

const loadingScene = ref(false)
const isFullscreen = ref(false)
const error = ref(null)
const tour = ref(null)
const activePanorama = ref(null)

// UI Triggers
const showHelp = ref(false)
const showSceneList = ref(false)

// Audio Guide state
const activeAudio = ref(null)
const audioPlaying = ref(false)
const audioRef = ref(null)
const audioCurrentTime = ref(0)
const audioDuration = ref(0)
const audioProgress = ref(0)
const audioProgressBarRef = ref(null)

// Media Lightbox popup state
const previewingHotspot = ref(null)

// Photo Sphere Viewer Instance variables
const viewerContainer = ref(null)
let viewerInstance = null
let markersPluginInstance = null

// Auto-rotation when idle variables
const IDLE_TIME_MS = 4000 // 6 seconds of inactivity before starting rotation
const ROTATION_SPEED = 0.001 // Speed of rotation (radians per frame)
let idleTimer = null
let rotationFrameId = null
let isAutoRotating = false

// Hotspot styles mapping matching admin interface
const colors = {
  navigation: '#10b981', // emerald
  info: '#f59e0b',       // amber
  audio: '#d946ef',      // fuchsia
  video: '#ef4444',      // red
  image: '#0ea5e9',      // sky
  model3d: '#8b5cf6',    // violet
  link: '#6366f1'        // indigo
}

const getHotspotColor = (type) => colors[type] || '#0d9488'

const getHotspotHtml = (h) => {
  const hex = getHotspotColor(h.type)
  let svgContent = ''
  let bgStyle = `background-color: #0f172a; border: 2px solid ${hex}; color: ${hex};`

  if (h.type === 'navigation') {
    svgContent = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><path d="M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z"></path><circle cx="12" cy="10" r="3"></circle></svg>`
  } else if (h.type === 'info') {
    svgContent = `<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"></circle><line x1="12" y1="16" x2="12" y2="12"></line><line x1="12" y1="8" x2="12.01" y2="8"></line></svg>`
  } else if (h.type === 'audio') {
    svgContent = `<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><path d="M9 18V5l12-2v13"></path><circle cx="6" cy="18" r="3"></circle><circle cx="18" cy="16" r="3"></circle></svg>`
  } else if (h.type === 'video') {
    svgContent = `<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><polygon points="23 7 16 12 23 17 23 7"></polygon><rect x="1" y="5" width="15" height="14" rx="2" ry="2"></rect></svg>`
  } else if (h.type === 'image') {
    svgContent = `<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><rect x="3" y="3" width="18" height="18" rx="2" ry="2"></rect><circle cx="8.5" cy="8.5" r="1.5"></circle><polyline points="21 15 16 10 5 21"></polyline></svg>`
  } else if (h.type === 'model3d') {
    svgContent = `<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"></path><polyline points="3.27 6.96 12 12.01 20.73 6.96"></polyline><line x1="12" y1="22.08" x2="12" y2="12"></line></svg>`
  } else {
    svgContent = `<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"><path d="M10 13a5 5 0 0 0 7.54.54l3-3a5 5 0 0 0-7.07-7.07l-1.72 1.71"></path><path d="M14 11a5 5 0 0 0-7.54-.54l-3 3a5 5 0 0 0 7.07 7.07l1.71-1.71"></path></svg>`
  }

  const animationStyle = h.type === 'navigation' ? 'animation: pulse-ring 1.5s cubic-bezier(0.215, 0.610, 0.355, 1) infinite;' : ''
  const floatStyle = 'animation: float-marker 2.5s ease-in-out infinite;'

  return `
    <div style="position: relative; display: flex; align-items: center; justify-content: center; width: 36px; height: 36px; cursor: pointer; ${floatStyle}">
      <div style="position: absolute; width: 36px; height: 36px; border-radius: 50%; background-color: ${hex}35; border: 1px solid ${hex}60; ${animationStyle}"></div>
      <div style="position: relative; width: 28px; height: 28px; border-radius: 50%; ${bgStyle} display: flex; align-items: center; justify-content: center; box-shadow: 0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1);">
        ${svgContent}
      </div>
    </div>
  `
}

const mapToMarker = (h) => {
  return {
    id: `hotspot-${h.id}`,
    position: {
      yaw: parseFloat(h.yaw),
      pitch: parseFloat(h.pitch)
    },
    html: getHotspotHtml(h),
    tooltip: {
      content: h.title,
      position: 'top'
    },
    data: h
  }
}

// Fetch tour details from backend public endpoint
const loadTourData = async () => {
  error.value = null
  loadingScene.value = true
  try {
    const id = route.params.id
    const response = await api.get(`/api/destinations/${id}/virtual-tour`)
    
    if (response.success) {
      tour.value = response.data
      
      if (!tour.value.panoramas || tour.value.panoramas.length === 0) {
        throw new Error("Tour du lịch 360° này chưa có cảnh panorama nào được xuất bản.")
      }

      // Determine default panorama
      let defaultPano = null
      if (tour.value.defaultPanoramaId) {
        defaultPano = tour.value.panoramas.find(p => p.id === tour.value.defaultPanoramaId)
      }
      if (!defaultPano) {
        defaultPano = tour.value.panoramas[0]
      }

      activePanorama.value = defaultPano

      await nextTick()
      initViewer()
    } else {
      throw new Error(response.message || "Không thể tải thông tin tour ảo.")
    }
  } catch (err) {
    error.value = err.message || "Lỗi xảy ra khi tải dữ liệu tour ảo."
  } finally {
    loadingScene.value = false
  }
}

// Auto-rotation helper functions
const startAutoRotation = () => {
  if (isAutoRotating || !viewerInstance || loadingScene.value) return
  isAutoRotating = true

  const rotateStep = () => {
    if (!isAutoRotating || !viewerInstance) return
    try {
      const position = viewerInstance.getPosition()
      let currentYaw = position.yaw + ROTATION_SPEED
      if (currentYaw > Math.PI) {
        currentYaw -= 2 * Math.PI
      }
      viewerInstance.rotate({
        yaw: currentYaw,
        pitch: position.pitch
      })
    } catch (e) {
      console.warn("Failed to rotate viewer automatically:", e)
      stopAutoRotation()
      return
    }
    rotationFrameId = requestAnimationFrame(rotateStep)
  }
  rotationFrameId = requestAnimationFrame(rotateStep)
}

const stopAutoRotation = () => {
  isAutoRotating = false
  if (rotationFrameId) {
    cancelAnimationFrame(rotationFrameId)
    rotationFrameId = null
  }
}

const startIdleTimer = () => {
  stopIdleTimer()
  if (loadingScene.value) return
  idleTimer = setTimeout(() => {
    startAutoRotation()
  }, IDLE_TIME_MS)
}

const stopIdleTimer = () => {
  if (idleTimer) {
    clearTimeout(idleTimer)
    idleTimer = null
  }
}

const resetIdle = () => {
  stopAutoRotation()
  startIdleTimer()
}

const setupIdleListeners = () => {
  const container = viewerContainer.value
  if (!container) return
  container.addEventListener('pointerdown', resetIdle)
  container.addEventListener('wheel', resetIdle)
  window.addEventListener('keydown', resetIdle)
  startIdleTimer()
}

const cleanupIdleListeners = () => {
  const container = viewerContainer.value
  if (container) {
    container.removeEventListener('pointerdown', resetIdle)
    container.removeEventListener('wheel', resetIdle)
  }
  window.removeEventListener('keydown', resetIdle)
  stopIdleTimer()
  stopAutoRotation()
}

// Initialize Photo Sphere Viewer instance
const initViewer = () => {
  cleanupIdleListeners()

  if (viewerInstance) {
    viewerInstance.destroy()
    viewerInstance = null
    markersPluginInstance = null
  }

  if (!viewerContainer.value || !activePanorama.value) return

  const yaw = parseFloat(activePanorama.value.initialYaw) || 0
  const pitch = parseFloat(activePanorama.value.initialPitch) || 0

  viewerInstance = new Viewer({
    container: viewerContainer.value,
    panorama: activePanorama.value.panoramaImageUrl,
    caption: activePanorama.value.title,
    defaultYaw: yaw,
    defaultPitch: pitch,
    navbar: false, // Disable default bottom controls completely to avoid overlapping
    plugins: [
      [MarkersPlugin, {}]
    ]
  })

  markersPluginInstance = viewerInstance.getPlugin(MarkersPlugin)

  // Listen to fullscreen state changes
  viewerInstance.addEventListener('fullscreen-updated', (e) => {
    isFullscreen.value = e.fullscreen
  })

  // Load hotspots
  if (activePanorama.value.hotspots) {
    const markers = activePanorama.value.hotspots.map(mapToMarker)
    markersPluginInstance.setMarkers(markers)
  }

  // Event listener when clicking a marker
  markersPluginInstance.addEventListener('select-marker', (e) => {
    const h = e.marker.data
    handleHotspotInteraction(h)
  })

  setupIdleListeners()
}

// Interacting with Hotspots
const handleHotspotInteraction = (h) => {
  resetIdle()
  if (h.type === 'navigation') {
    // Traverse scenes in place
    const targetScene = tour.value.panoramas.find(p => p.id === h.targetPanoramaId)
    if (targetScene) {
      transitionToScene(targetScene)
    }
  } else if (h.type === 'audio') {
    // Launch audio player at footer
    activeAudio.value = h
    audioPlaying.value = false
    audioCurrentTime.value = 0
    audioProgress.value = 0
    
    // Stop old audio if playing
    if (audioRef.value) {
      audioRef.value.pause()
    }
    
    // Autoplay new audio on nextTick
    nextTick(() => {
      if (audioRef.value) {
        audioRef.value.src = h.mediaUrl
        audioRef.value.play().then(() => {
          audioPlaying.value = true
        }).catch(err => {
          console.warn("Autoplay audio blocked by browser settings.", err)
        })
      }
    })
  } else if (h.type === 'link') {
    if (h.externalUrl) {
      window.open(h.externalUrl, '_blank')
    }
  } else {
    // Open image, video, info, or 3d details popups
    previewingHotspot.value = h
  }
}

const toggleFullscreen = () => {
  resetIdle()
  if (viewerInstance) {
    viewerInstance.toggleFullscreen()
  }
}

// Smooth Scene Transition loading
const transitionToScene = (scene) => {
  if (!viewerInstance || !markersPluginInstance) return
  
  loadingScene.value = true
  showSceneList.value = false

  // Stop auto-rotation and timer during transition
  stopAutoRotation()
  stopIdleTimer()

  // 1. Clear old markers to avoid overlay glitches and click conflicts
  markersPluginInstance.clearMarkers()
  stopAudio()

  // 2. Delay loading slightly to let the click handler and tooltip animations resolve
  setTimeout(() => {
    if (!viewerInstance || !markersPluginInstance) return

    viewerInstance.setPanorama(scene.panoramaImageUrl, { showLoading: false })
      .then(() => {
        onSceneChanged(scene)
      })
      .catch((err) => {
        console.warn("setPanorama promise rejected, checking if panorama changed:", err)
        // If it was just an abort conflict but the panorama image did change, recover and proceed
        onSceneChanged(scene)
      })
  }, 100)
}

const onSceneChanged = (scene) => {
  activePanorama.value = scene
  
  // Align view to initial values
  const targetYaw = parseFloat(scene.initialYaw) || 0
  const targetPitch = parseFloat(scene.initialPitch) || 0
  viewerInstance.rotate({ yaw: targetYaw, pitch: targetPitch })

  // Reset new markers
  if (scene.hotspots && scene.hotspots.length > 0) {
    const markers = scene.hotspots.map(mapToMarker)
    markersPluginInstance.setMarkers(markers)
  } else {
    markersPluginInstance.clearMarkers()
  }
  
  loadingScene.value = false
  startIdleTimer()
}

const switchSceneDirectly = (scene) => {
  resetIdle()
  if (scene.id === activePanorama.value?.id) return
  transitionToScene(scene)
}

// Viewer Quick Control Wrappers
const zoomIn = () => {
  resetIdle()
  if (viewerInstance) {
    const currentZoom = viewerInstance.getZoomLevel()
    viewerInstance.zoom(currentZoom + 10)
  }
}

const zoomOut = () => {
  resetIdle()
  if (viewerInstance) {
    const currentZoom = viewerInstance.getZoomLevel()
    viewerInstance.zoom(currentZoom - 10)
  }
}

const resetView = () => {
  resetIdle()
  if (viewerInstance && activePanorama.value) {
    const yaw = parseFloat(activePanorama.value.initialYaw) || 0
    const pitch = parseFloat(activePanorama.value.initialPitch) || 0
    viewerInstance.animate({
      yaw: yaw,
      pitch: pitch,
      zoom: 50,
      speed: 1000
    })
  }
}

// Toggle drawer list
const toggleSceneList = () => {
  resetIdle()
  showSceneList.value = !showSceneList.value
}

// Audio Player Handlers
const toggleAudioPlayback = () => {
  const audio = audioRef.value
  if (!audio) return

  if (audioPlaying.value) {
    audio.pause()
    audioPlaying.value = false
  } else {
    audio.play().then(() => {
      audioPlaying.value = true
    })
  }
}

const stopAudio = () => {
  const audio = audioRef.value
  if (audio) {
    audio.pause()
  }
  activeAudio.value = null
  audioPlaying.value = false
  audioCurrentTime.value = 0
  audioProgress.value = 0
}

const onAudioTimeUpdate = () => {
  const audio = audioRef.value
  if (!audio) return
  audioCurrentTime.value = audio.currentTime
  audioProgress.value = (audio.currentTime / audio.duration) * 100
}

const onAudioMetadataLoaded = () => {
  const audio = audioRef.value
  if (!audio) return
  audioDuration.value = audio.duration
}

const onAudioEnded = () => {
  audioPlaying.value = false
  audioCurrentTime.value = 0
  audioProgress.value = 0
}

const seekAudio = (e) => {
  const audio = audioRef.value
  const bar = audioProgressBarRef.value
  if (!audio || !bar) return

  const rect = bar.getBoundingClientRect()
  const clickX = e.clientX - rect.left
  const width = rect.width
  const percent = Math.min(Math.max(clickX / width, 0), 1)

  audio.currentTime = percent * audio.duration
  audioCurrentTime.value = audio.currentTime
  audioProgress.value = percent * 100
}

const formatTime = (secs) => {
  if (isNaN(secs)) return '0:00'
  const minutes = Math.floor(secs / 60)
  const seconds = Math.floor(secs % 60)
  return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`
}

// Dialog Previews
const closePreview = () => {
  resetIdle()
  previewingHotspot.value = null
}

const closeHelp = () => {
  resetIdle()
  showHelp.value = false
  localStorage.setItem('tourGuideSeen', 'true')
}

const exitTour = () => {
  stopAudio()
  router.push(`/destinations/${route.params.id}`)
}

// Watch route destination ID changes
watch(() => route.params.id, async (newId) => {
  if (newId) {
    stopAudio()
    await loadTourData()
  }
})

onMounted(() => {
  loadTourData()
  const guideSeen = localStorage.getItem('tourGuideSeen')
  if (!guideSeen) {
    showHelp.value = true
  }
})

onBeforeUnmount(() => {
  cleanupIdleListeners()
  if (viewerInstance) {
    viewerInstance.destroy()
    viewerInstance = null
    markersPluginInstance = null
  }
  stopAudio()
})
</script>

<style scoped>
.animate-spin-slow {
  animation: spin 10s linear infinite;
}
@keyframes spin {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}

/* Audio wave bar animation */
@keyframes eq-pulse {
  0%, 100% { height: 4px; }
  50% { height: 14px; }
}

.animate-eq-bar-1 {
  animation: eq-pulse 0.8s ease-in-out infinite;
}

.animate-eq-bar-2 {
  animation: eq-pulse 0.5s ease-in-out infinite 0.2s;
}

.animate-eq-bar-3 {
  animation: eq-pulse 0.7s ease-in-out infinite 0.1s;
}

/* Transitions helper classes */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.4s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

.slide-up-enter-active,
.slide-up-leave-active {
  transition: transform 0.35s cubic-bezier(0.16, 1, 0.3, 1), opacity 0.35s ease;
}
.slide-up-enter-from,
.slide-up-leave-to {
  transform: translate(-50%, 30px);
  opacity: 0;
}

/* Fix drawer slide transition specifically */
.slide-up-enter-from.absolute.bottom-0,
.slide-up-leave-to.absolute.bottom-0 {
  transform: translateY(100%);
  opacity: 0.9;
}
</style>

<style>
/* Global style to apply the pulse ring & float animation to custom HTML markers inside Photo Sphere Viewer container */
@keyframes float-marker {
  0% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-6px);
  }
  100% {
    transform: translateY(0);
  }
}

@keyframes pulse-ring {
  0% {
    transform: scale(0.65);
    opacity: 1;
  }
  100% {
    transform: scale(1.35);
    opacity: 0;
  }
}
</style>
