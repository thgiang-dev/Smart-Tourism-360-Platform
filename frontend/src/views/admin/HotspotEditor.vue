<template>
  <div class="flex flex-col lg:flex-row gap-6 min-h-[calc(100vh-140px)] font-sans">
    <!-- Left: 360 Canvas Viewer (75% on desktop) -->
    <div class="flex-grow lg:w-3/4 flex flex-col space-y-4">
      <!-- Top controls bar -->
      <div class="flex items-center justify-between bg-white p-4 rounded-2xl border border-slate-200/80 shadow-sm">
        <div class="flex items-center space-x-3">
          <button 
            @click="goBack" 
            class="premium-btn p-2 hover:bg-slate-50 border border-slate-200 rounded-xl text-slate-500 hover:text-slate-800 transition"
            title="Quay lại danh sách cảnh"
          >
            <ChevronLeftIcon class="w-4 h-4" />
          </button>
          <div>
            <h3 class="font-bold text-slate-800 text-sm">Góc cảnh: <span class="text-teal-600 font-black">{{ panorama?.title || 'Đang tải...' }}</span></h3>
            <p class="text-[10px] text-slate-400 font-medium">Click trực tiếp vào ảnh 360° để chọn tọa độ đặt Hotspot mới.</p>
          </div>
        </div>

        <!-- Mode Toggle -->
        <div class="flex items-center bg-slate-100 rounded-xl p-1 border border-slate-200/20">
          <button 
            @click="activeMode = 'edit'"
            :class="[
              'premium-btn px-3.5 py-1.5 rounded-lg text-xs font-bold transition flex items-center space-x-1.5',
              activeMode === 'edit' ? 'bg-teal-700 text-white shadow-sm' : 'text-slate-500 hover:text-slate-850'
            ]"
          >
            <EditIcon class="w-3.5 h-3.5" />
            <span>Chỉnh sửa</span>
          </button>
          <button 
            @click="activeMode = 'preview'"
            :class="[
              'premium-btn px-3.5 py-1.5 rounded-lg text-xs font-bold transition flex items-center space-x-1.5',
              activeMode === 'preview' ? 'bg-teal-700 text-white shadow-sm' : 'text-slate-500 hover:text-slate-850'
            ]"
          >
            <EyeIcon class="w-3.5 h-3.5" />
            <span>Xem thử</span>
          </button>
        </div>
      </div>

      <!-- 360 Canvas -->
      <div class="relative w-full h-[550px] bg-slate-950 rounded-2xl overflow-hidden border border-slate-800/80 shadow-inner group">
        <div ref="viewerContainer" class="w-full h-full"></div>

        <!-- Mode Indicator watermark -->
        <div class="absolute top-4 left-4 pointer-events-none z-10 select-none">
          <span 
            class="inline-flex items-center px-3.5 py-2 rounded-xl text-[10px] font-black uppercase tracking-wider backdrop-blur-md shadow-md border border-white/10"
            :class="activeMode === 'edit' ? 'bg-amber-500/20 text-amber-400' : 'bg-emerald-500/20 text-emerald-400'"
          >
            <span class="w-2 h-2 rounded-full mr-2 animate-pulse" :class="activeMode === 'edit' ? 'bg-amber-400' : 'bg-emerald-400'"></span>
            Chế độ: {{ activeMode === 'edit' ? 'Chỉnh sửa' : 'Xem thử (Click thử tương tác)' }}
          </span>
        </div>

        <!-- Crosshair in center for position target mapping -->
        <div v-if="activeMode === 'edit'" class="absolute inset-0 pointer-events-none flex items-center justify-center opacity-70">
          <div class="w-10 h-10 border-2 border-dashed border-teal-400 rounded-full flex items-center justify-center animate-pulse-glow">
            <div class="w-2.5 h-2.5 bg-teal-400 rounded-full"></div>
          </div>
        </div>
      </div>
    </div>

    <!-- Right: Control Panel / Sidebar (25% on desktop) -->
    <div class="lg:w-1/4 flex flex-col space-y-6">
      <!-- Hotspots List & General Status (When NOT editing) -->
      <div v-if="!showFormModal" class="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm flex flex-col h-full space-y-4">
        <div class="flex justify-between items-center border-b border-slate-100 pb-3">
          <h4 class="text-xs font-black text-slate-400 uppercase tracking-wider">Danh sách Hotspot</h4>
          <span class="px-2.5 py-0.5 bg-slate-100 text-slate-600 rounded-full text-[10px] font-bold">
            {{ hotspots.length }} điểm
          </span>
        </div>

        <!-- Sidebar Content -->
        <div class="flex-grow overflow-y-auto max-h-[400px] space-y-2 pr-1 custom-scrollbar">
          <div v-if="hotspots.length === 0" class="text-center py-12 text-slate-400 space-y-2">
            <CompassIcon class="w-8 h-8 mx-auto opacity-30 animate-spin" />
            <p class="text-xs font-bold">Chưa có điểm tương tác nào.</p>
          </div>

          <div 
            v-for="h in hotspots" 
            :key="h.id"
            class="flex items-center justify-between p-3 rounded-xl border border-slate-150 hover:border-teal-500/35 hover:bg-slate-50/50 transition duration-150 group"
          >
            <div class="flex items-center space-x-2.5 overflow-hidden">
              <!-- Type Indicator Dot -->
              <span 
                class="w-2 h-2 rounded-full flex-shrink-0"
                :style="{ backgroundColor: getHotspotColor(h.type) }"
              ></span>
              <div class="overflow-hidden">
                <span class="text-xs font-bold text-slate-700 block truncate cursor-pointer hover:text-teal-650" @click="focusOnHotspot(h)">{{ h.title }}</span>
                <span class="text-[9px] text-slate-450 block font-mono font-bold capitalize">{{ h.type }}</span>
              </div>
            </div>

            <!-- Actions -->
            <div class="flex items-center space-x-0.5 opacity-0 group-hover:opacity-100 transition duration-150">
              <button 
                @click="focusOnHotspot(h)" 
                class="p-1 hover:bg-slate-100 rounded text-slate-500 hover:text-slate-800 transition"
                title="Xem vị trí"
              >
                <TargetIcon class="w-3.5 h-3.5" />
              </button>
              <button 
                @click="openEditModal(h)" 
                class="p-1 hover:bg-slate-100 rounded text-slate-500 hover:text-teal-650 transition"
                title="Chỉnh sửa"
              >
                <EditIcon class="w-3.5 h-3.5" />
              </button>
              <button 
                @click="confirmDelete(h.id)" 
                class="p-1 hover:bg-slate-100 rounded text-slate-500 hover:text-rose-600 transition"
                title="Xóa"
              >
                <TrashIcon class="w-3.5 h-3.5 text-rose-500" />
              </button>
            </div>
          </div>
        </div>

        <button 
          v-if="activeMode === 'edit'"
          @click="openCreateModalFromCenter"
          class="premium-btn w-full py-3 bg-slate-900 hover:bg-slate-850 text-white font-bold text-xs rounded-xl transition flex items-center justify-center space-x-1 shadow-sm"
        >
          <PlusIcon class="w-3.5 h-3.5 stroke-[2.5]" />
          <span>Đặt Hotspot ở tâm màn hình</span>
        </button>
      </div>

      <!-- Hotspot Form Panel (When editing/creating) -->
      <div v-else class="bg-white p-5 rounded-2xl border border-slate-200/80 shadow-sm flex flex-col h-full space-y-4 overflow-y-auto max-h-[calc(100vh-140px)] animate-fade-in">
        <div class="flex justify-between items-center border-b border-slate-100 pb-3">
          <div>
            <h4 class="text-xs font-black text-slate-800 uppercase tracking-wider">
              {{ isEditMode ? 'Sửa Hotspot' : 'Thêm Hotspot' }}
            </h4>
            <p class="text-[9px] text-slate-400 font-semibold">Điều chỉnh thông tin và vị trí.</p>
          </div>
          <button 
            type="button"
            @click="closeFormModal"
            class="p-1.5 hover:bg-slate-100 rounded-lg text-slate-400 hover:text-slate-600 transition"
          >
            <XIcon class="w-4 h-4" />
          </button>
        </div>

        <form @submit.prevent="submitForm" class="space-y-4 pt-1">
          <!-- Type selection -->
          <div>
            <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Loại điểm tương tác *</label>
            <select 
              v-model="modalForm.type"
              required
              class="w-full px-2.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/5 rounded-xl text-xs focus:outline-none transition cursor-pointer appearance-none"
            >
              <option value="navigation">Chuyển cảnh (Navigation)</option>
              <option value="info">Thông tin văn bản (Info)</option>
              <option value="audio">Thuyết minh âm thanh (Audio)</option>
              <option value="video">Phát Video (Video)</option>
              <option value="image">Hiển thị Hình ảnh (Image)</option>
              <option value="model3d">Xem Mô hình 3D (Model 3D)</option>
              <option value="link">Liên kết ngoài (Link)</option>
            </select>
          </div>

          <!-- Title -->
          <div>
            <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Tiêu đề điểm *</label>
            <input 
              v-model="modalForm.title"
              type="text"
              required
              placeholder="Ví dụ: Đi tới Chính điện..."
              class="w-full px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/5 rounded-xl text-xs focus:outline-none transition font-medium"
            />
          </div>

          <!-- Description -->
          <div>
            <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Mô tả chi tiết</label>
            <textarea 
              v-model="modalForm.description"
              rows="2"
              placeholder="Mô tả bổ sung..."
              class="w-full px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/5 rounded-xl text-xs focus:outline-none transition resize-none font-medium"
            ></textarea>
          </div>

          <!-- Navigation Type: Target Panorama selection -->
          <div v-if="modalForm.type === 'navigation'">
            <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Cảnh chuyển đến *</label>
            <select 
              v-model="modalForm.targetPanoramaId"
              required
              class="w-full px-2.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/5 rounded-xl text-xs focus:outline-none transition cursor-pointer appearance-none"
            >
              <option :value="null" disabled>Chọn cảnh đích</option>
              <option 
                v-for="p in tourPanoramas" 
                :key="p.id" 
                :value="p.id"
              >
                {{ p.title }}
              </option>
            </select>
          </div>

          <!-- Media Types (audio/video/image/model3d): Media Selector -->
          <div v-if="['audio', 'video', 'image', 'model3d'].includes(modalForm.type)">
            <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Tập tin đa phương tiện *</label>
            <div class="flex items-center space-x-1.5">
              <input 
                type="text"
                readonly
                required
                :value="modalForm.mediaUrl ? getFileNameFromUrl(modalForm.mediaUrl) : 'Chưa chọn file'"
                class="flex-grow px-2.5 py-2 bg-slate-100 border border-slate-200 rounded-xl text-[10px] focus:outline-none truncate font-mono"
              />
              <button 
                type="button" 
                @click="openMediaSelector"
                class="premium-btn px-3 py-2 bg-slate-900 hover:bg-slate-800 text-white rounded-xl text-[10px] font-bold transition shadow-sm"
              >
                Chọn
              </button>
            </div>
          </div>

          <!-- External URL Link type -->
          <div v-if="modalForm.type === 'link'">
            <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Đường dẫn ngoài *</label>
            <input 
              v-model="modalForm.externalUrl"
              type="url"
              required
              placeholder="https://..."
              class="w-full px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/5 rounded-xl text-[10px] focus:outline-none transition font-mono"
            />
          </div>

          <!-- Yaw / Pitch Coordinates -->
          <div class="grid grid-cols-2 gap-2">
            <div>
              <label class="block text-[9px] font-bold text-slate-500 uppercase tracking-wider mb-1">Yaw (Radian)</label>
              <input 
                :value="parseFloat(modalForm.yaw).toFixed(4)"
                type="text"
                readonly
                class="w-full px-2 py-1.5 bg-slate-100 border border-slate-200 rounded-xl text-[10px] focus:outline-none font-mono font-bold text-slate-600"
              />
            </div>
            <div>
              <label class="block text-[9px] font-bold text-slate-500 uppercase tracking-wider mb-1">Pitch (Radian)</label>
              <input 
                :value="parseFloat(modalForm.pitch).toFixed(4)"
                type="text"
                readonly
                class="w-full px-2 py-1.5 bg-slate-100 border border-slate-200 rounded-xl text-[10px] focus:outline-none font-mono font-bold text-slate-600"
              />
            </div>
          </div>

          <!-- Coordinate Picker Guide & Button -->
          <div class="flex items-center justify-between text-[9px] text-slate-450 bg-slate-50 p-2.5 rounded-xl border border-slate-200/40">
            <span class="font-medium">Click trên ảnh để đổi vị trí.</span>
            <button 
              type="button" 
              @click="updateFormCoordsFromCenter" 
              class="text-teal-650 hover:text-teal-700 font-bold transition flex items-center space-x-1"
            >
              <TargetIcon class="w-3 h-3" />
              <span>Lấy tọa độ tâm</span>
            </button>
          </div>

          <!-- Error Alert Inside Modal -->
          <div v-if="modalError" class="text-[10px] font-semibold text-red-600 bg-red-50 p-2.5 rounded-xl border border-red-100">
            {{ modalError }}
          </div>

          <!-- Action Buttons -->
          <div class="flex items-center space-x-2 pt-3 border-t border-slate-100">
            <button 
              type="submit" 
              class="premium-btn flex-grow py-2.5 bg-teal-700 hover:bg-teal-850 text-white font-bold rounded-xl shadow-lg hover:shadow-teal-700/10 active:scale-[0.98] transition flex items-center justify-center space-x-1.5 disabled:opacity-50 text-xs"
              :disabled="submitting"
            >
              <Loader2Icon v-if="submitting" class="w-3.5 h-3.5 animate-spin" />
              <span>Lưu</span>
            </button>
            <button 
              type="button"
              @click="closeFormModal"
              class="premium-btn px-3 py-2.5 border border-slate-200 hover:bg-slate-50 text-slate-500 font-bold rounded-xl text-xs transition animate-fade-in"
              :disabled="submitting"
            >
              Hủy
            </button>
          </div>
        </form>
      </div>
    </div>



    <!-- Media Library Selector Modal -->
    <div 
      v-if="showMediaSelector" 
      class="fixed inset-0 z-[60] overflow-y-auto bg-slate-900/60 backdrop-blur-sm flex items-center justify-center p-4 animate-fade-in"
      @click.self="closeMediaSelector"
    >
      <div class="bg-white rounded-3xl w-full max-w-2xl shadow-2xl border border-slate-100 overflow-hidden flex flex-col max-h-[80vh] animate-scale-up">
        <div class="p-6 border-b border-slate-100 flex justify-between items-center">
          <div class="space-y-1">
            <h3 class="text-base font-black text-slate-800">Chọn tệp từ thư viện</h3>
            <p class="text-xs text-slate-500 font-medium">Lọc các tệp thuộc định dạng khớp với hotspot.</p>
          </div>
          <button 
            type="button"
            @click="closeMediaSelector"
            class="p-2 hover:bg-slate-100 rounded-xl text-slate-400 hover:text-slate-600 transition"
          >
            <XIcon class="w-5 h-5" />
          </button>
        </div>

        <!-- Content Area -->
        <div class="p-6 overflow-y-auto flex-1 min-h-[200px] bg-white">
          <div v-if="selectorLoading" class="grid grid-cols-4 gap-3 animate-pulse">
            <div v-for="i in 8" :key="i" class="aspect-square bg-slate-100 rounded-xl"></div>
          </div>

          <div v-else-if="selectorMedia.length === 0" class="text-center py-12 space-y-3">
            <ImageIcon class="w-8 h-8 mx-auto text-slate-350" />
            <p class="text-sm font-medium text-slate-550">Thư viện trống hoặc không có tệp phù hợp.</p>
          </div>

          <div v-else class="grid grid-cols-3 sm:grid-cols-4 gap-4">
            <div 
              v-for="img in selectorMedia" 
              :key="img.id"
              :class="[
                'aspect-square rounded-xl overflow-hidden cursor-pointer border-2 transition relative flex flex-col items-center justify-center p-2 text-center bg-slate-50',
                modalForm.mediaId === img.id ? 'border-teal-500 shadow-md ring-2 ring-teal-500/20' : 'border-slate-100 hover:border-slate-300'
              ]"
              @click="selectMedia(img)"
            >
              <!-- Thumbnail preview based on type -->
              <img v-if="img.mediaType === 'image'" :src="img.fileUrl" class="w-full h-full object-cover rounded-lg" />
              <div v-else class="space-y-2 text-slate-400">
                <MusicIcon v-if="img.mediaType === 'audio'" class="w-8 h-8 mx-auto text-indigo-500 stroke-[1.5]" />
                <VideoIcon v-else-if="img.mediaType === 'video'" class="w-8 h-8 mx-auto text-rose-500 stroke-[1.5]" />
                <CompassIcon v-else class="w-8 h-8 mx-auto text-slate-400" />
                <span class="text-[10px] block truncate max-w-[100px] font-bold text-slate-600">{{ img.fileName }}</span>
              </div>

              <!-- Selection badge -->
              <div 
                v-if="modalForm.mediaId === img.id"
                class="absolute inset-0 bg-teal-500/10 flex items-center justify-center"
              >
                <div class="p-1 bg-teal-500 text-slate-950 rounded-full shadow-md">
                  <CheckIcon class="w-3.5 h-3.5 stroke-[2.5]" />
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Footer Pagination -->
        <div 
          v-if="selectorMedia.length > 0 && selectorTotalPages > 1"
          class="p-4 bg-slate-50 border-t border-slate-100 flex justify-between items-center text-xs"
        >
          <span class="text-slate-550 font-bold">Trang {{ selectorPage }} / {{ selectorTotalPages }}</span>
          <div class="flex items-center space-x-2">
            <button 
              type="button"
              @click="selectorPrevPage"
              :disabled="selectorPage === 1"
              class="premium-btn px-2.5 py-1.5 bg-white border border-slate-200 rounded-lg hover:bg-slate-50 transition disabled:opacity-40 shadow-sm"
            >
              Trước
            </button>
            <button 
              type="button"
              @click="selectorNextPage"
              :disabled="selectorPage === selectorTotalPages"
              class="premium-btn px-2.5 py-1.5 bg-white border border-slate-200 rounded-lg hover:bg-slate-50 transition disabled:opacity-40 shadow-sm"
            >
              Sau
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Media Preview Dialog (Audio / Video / Image popup previews) -->
    <div 
      v-if="previewingHotspot"
      class="fixed inset-0 z-50 bg-slate-950/80 backdrop-blur-md flex items-center justify-center p-4 animate-fade-in"
      @click.self="closePreview"
    >
      <div class="bg-slate-900 border border-slate-800 text-white rounded-3xl w-full max-w-lg shadow-2xl overflow-hidden flex flex-col animate-scale-up">
        <!-- Preview Header -->
        <div class="p-5 border-b border-slate-800 flex justify-between items-center bg-slate-950/40">
          <div>
            <span class="text-[9px] font-black uppercase tracking-wider text-teal-400 block mb-0.5">{{ previewingHotspot.type }}</span>
            <h4 class="font-bold text-sm text-white">{{ previewingHotspot.title }}</h4>
          </div>
          <button @click="closePreview" class="p-2 hover:bg-slate-800 rounded-xl text-slate-400 hover:text-white transition">
            <XIcon class="w-5 h-5" />
          </button>
        </div>

        <!-- Preview Body -->
        <div class="p-6 flex flex-col items-center justify-center space-y-4">
          <!-- Description if exists -->
          <p v-if="previewingHotspot.description" class="text-xs text-slate-300 w-full text-center font-medium">{{ previewingHotspot.description }}</p>

          <!-- Type specific preview views -->

          <!-- Image Preview -->
          <div v-if="previewingHotspot.type === 'image' && previewingHotspot.mediaUrl" class="w-full aspect-[4/3] rounded-xl overflow-hidden bg-black flex items-center justify-center border border-white/5 shadow-lg">
            <img :src="previewingHotspot.mediaUrl" class="w-full h-full object-contain" />
          </div>

          <!-- Video Preview -->
          <div v-else-if="previewingHotspot.type === 'video' && previewingHotspot.mediaUrl" class="w-full aspect-video rounded-xl overflow-hidden bg-black flex items-center justify-center border border-white/5 shadow-lg">
            <video :src="previewingHotspot.mediaUrl" controls autoplay class="w-full h-full"></video>
          </div>

          <!-- Audio Preview -->
          <div v-else-if="previewingHotspot.type === 'audio' && previewingHotspot.mediaUrl" class="w-full p-4 bg-slate-950 border border-slate-850 rounded-2xl flex items-center space-x-4">
            <button 
              @click="toggleAudioPlay" 
              class="premium-btn w-12 h-12 bg-teal-500 text-slate-950 rounded-full flex items-center justify-center hover:scale-105 active:scale-95 transition"
            >
              <PauseIcon v-if="audioPlaying" class="w-5 h-5 fill-slate-950" />
              <PlayIcon v-else class="w-5 h-5 fill-slate-950 ml-0.5" />
            </button>
            <div class="flex-grow overflow-hidden">
              <span class="text-xs font-bold text-slate-200 block truncate">{{ getFileNameFromUrl(previewingHotspot.mediaUrl) }}</span>
              <span class="text-[10px] text-slate-500 font-mono font-semibold">Bản thu thuyết minh</span>
            </div>
            <audio ref="audioPlayer" :src="previewingHotspot.mediaUrl" class="hidden" @ended="audioPlaying = false"></audio>
          </div>

          <!-- Model 3D Placeholder -->
          <div v-else-if="previewingHotspot.type === 'model3d'" class="text-center py-8 space-y-3 text-slate-400">
            <CompassIcon class="w-12 h-12 mx-auto animate-spin text-teal-400" />
            <p class="text-xs font-bold text-slate-300">Khởi chạy Mô hình 3D thực tế ảo...</p>
          </div>

          <!-- Info Fallback -->
          <div v-else-if="previewingHotspot.type === 'info'" class="py-6 text-center text-slate-450">
            <span class="text-xs font-bold">Không có xem trước đa phương tiện cho nội dung văn bản.</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, nextTick } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../../utils/api'
import { useToastStore } from '../../stores/toast'
import { useConfirmStore } from '../../stores/confirm'
import { Viewer } from '@photo-sphere-viewer/core'
import { MarkersPlugin } from '@photo-sphere-viewer/markers-plugin'
import '@photo-sphere-viewer/core/index.css'
import '@photo-sphere-viewer/markers-plugin/index.css'

import { 
  ChevronLeft as ChevronLeftIcon,
  Edit as EditIcon,
  Eye as EyeIcon,
  Plus as PlusIcon,
  Compass as CompassIcon,
  Trash as TrashIcon,
  X as XIcon,
  Loader2 as Loader2Icon,
  Image as ImageIcon,
  Check as CheckIcon,
  Music as MusicIcon,
  Video as VideoIcon,
  Play as PlayIcon,
  Pause as PauseIcon,
  Target as TargetIcon
} from 'lucide-vue-next'

const toastStore = useToastStore()
const confirmStore = useConfirmStore()

const route = useRoute()
const router = useRouter()

const panoramaId = ref(route.params.id)
const panorama = ref(null)
const tourPanoramas = ref([])
const hotspots = ref([])

const loading = ref(false)
const error = ref(null)
const activeMode = ref('edit') // edit / preview

// Modal states
const showFormModal = ref(false)
const isEditMode = ref(false)
const submitting = ref(false)
const modalError = ref(null)
const editingHotspotId = ref(null)

const modalForm = ref({
  type: 'navigation',
  title: '',
  description: '',
  yaw: 0,
  pitch: 0,
  targetPanoramaId: null,
  mediaId: null,
  mediaUrl: null,
  externalUrl: null,
  icon: null,
  displayOrder: 0
})

// Media selector states
const showMediaSelector = ref(false)
const selectorLoading = ref(false)
const selectorMedia = ref([])
const selectorPage = ref(1)
const selectorTotalPages = ref(1)
const selectorPageSize = ref(12)

// Media preview popup state
const previewingHotspot = ref(null)
const audioPlaying = ref(false)
const audioPlayer = ref(null)

// Photo Sphere Viewer Instance
const viewerContainer = ref(null)
let viewerInstance = null
let markersPluginInstance = null

// Color scheme mapping
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
    // MapPin icon for navigation unified style
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

  // Animation styles: pulse ring outer and float wrapper
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

const loadData = async () => {
  loading.value = true
  error.value = null
  try {
    // 1. Fetch current panorama details
    const panoRes = await api.get(`/api/admin/panoramas/${panoramaId.value}`)
    if (panoRes.success) {
      panorama.value = panoRes.data
      
      // 2. Fetch sibling panoramas belonging to same virtual tour
      const tourPanosRes = await api.get(`/api/admin/virtual-tours/${panorama.value.virtualTourId}/panoramas`)
      if (tourPanosRes.success) {
        tourPanoramas.value = tourPanosRes.data.filter(p => p.id !== panorama.value.id)
      }
    }

    // 3. Fetch hotspots
    await fetchHotspots()

    // 4. Load Photo Sphere Viewer
    await nextTick()
    initViewer()
  } catch (err) {
    error.value = err.message || 'Lỗi xảy ra khi tải dữ liệu.'
  } finally {
    loading.value = false
  }
}

const fetchHotspots = async () => {
  const hotspotsRes = await api.get(`/api/admin/panoramas/${panoramaId.value}/hotspots`)
  if (hotspotsRes.success) {
    hotspots.value = hotspotsRes.data
    // Update markers in real-time if viewer is initialized
    if (markersPluginInstance) {
      const markers = hotspots.value.map(mapToMarker)
      markersPluginInstance.setMarkers(markers)
    }
  }
}

// Map HotspotDto to Photo Sphere Viewer marker configuration
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

const initViewer = () => {
  if (viewerInstance) {
    viewerInstance.destroy()
    viewerInstance = null
    markersPluginInstance = null
  }

  if (!viewerContainer.value || !panorama.value) return

  // Standard initial viewport angles
  const yaw = parseFloat(panorama.value.initialYaw) || 0
  const pitch = parseFloat(panorama.value.initialPitch) || 0

  viewerInstance = new Viewer({
    container: viewerContainer.value,
    panorama: panorama.value.panoramaImageUrl,
    caption: panorama.value.title,
    defaultYaw: yaw,
    defaultPitch: pitch,
    navbar: [
      'zoom',
      'caption',
      'fullscreen'
    ],
    plugins: [
      [MarkersPlugin, {}]
    ]
  })

  markersPluginInstance = viewerInstance.getPlugin(MarkersPlugin)

  // Load existing hotspots as markers
  const markers = hotspots.value.map(mapToMarker)
  markersPluginInstance.setMarkers(markers)

  // Listeners
  viewerInstance.addEventListener('click', (e) => {
    if (activeMode.value !== 'edit') return
    const yaw = e.data?.yaw
    const pitch = e.data?.pitch
    if (typeof yaw !== 'number' || typeof pitch !== 'number' || isNaN(yaw) || isNaN(pitch)) {
      return
    }

    if (showFormModal.value) {
      modalForm.value.yaw = yaw
      modalForm.value.pitch = pitch
      modalError.value = null
    } else {
      openCreateModal(yaw, pitch)
    }
  })

  markersPluginInstance.addEventListener('select-marker', (e) => {
    const marker = e.marker
    const h = marker.data
    handleHotspotClick(h)
  })
}

const handleHotspotClick = (h) => {
  if (activeMode.value === 'edit') {
    // In edit mode, clicking a marker opens its edit modal
    openEditModal(h)
  } else {
    // In preview mode, trigger interactive features
    if (h.type === 'navigation') {
      // Traverse to target panorama in-place
      router.push(`/admin/panoramas/${h.targetPanoramaId}/hotspots`)
    } else if (h.type === 'link') {
      window.open(h.externalUrl, '_blank')
    } else {
      // Audio, video, image, info previews
      previewingHotspot.value = h
      audioPlaying.value = false
    }
  }
}

// Focus camera view on hotspot coordinates
const focusOnHotspot = (h) => {
  if (!viewerInstance) return
  viewerInstance.animate({
    yaw: parseFloat(h.yaw),
    pitch: parseFloat(h.pitch),
    zoom: 50,
    speed: 600 // Faster pan transition in milliseconds
  })
}

// Modal Form Actions
const openCreateModal = (yaw, pitch) => {
  isEditMode.value = false
  modalError.value = null
  modalForm.value = {
    type: 'navigation',
    title: '',
    description: '',
    yaw: yaw,
    pitch: pitch,
    targetPanoramaId: tourPanoramas.value.length > 0 ? tourPanoramas.value[0].id : null,
    mediaId: null,
    mediaUrl: null,
    externalUrl: '',
    icon: null,
    displayOrder: hotspots.value.length
  }
  showFormModal.value = true
}

const openCreateModalFromCenter = () => {
  if (!viewerInstance) return
  const position = viewerInstance.getPosition()
  openCreateModal(position.yaw, position.pitch)
}

const updateFormCoordsFromCenter = () => {
  if (!viewerInstance) return
  const position = viewerInstance.getPosition()
  modalForm.value.yaw = position.yaw
  modalForm.value.pitch = position.pitch
}

const openEditModal = (h) => {
  isEditMode.value = true
  modalError.value = null
  editingHotspotId.value = h.id
  modalForm.value = {
    type: h.type,
    title: h.title,
    description: h.description || '',
    yaw: parseFloat(h.yaw),
    pitch: parseFloat(h.pitch),
    targetPanoramaId: h.targetPanoramaId,
    mediaId: h.mediaId,
    mediaUrl: h.mediaUrl,
    externalUrl: h.externalUrl || '',
    icon: h.icon,
    displayOrder: h.displayOrder
  }
  showFormModal.value = true
}

const closeFormModal = () => {
  showFormModal.value = false
}

const submitForm = async () => {
  // Client side validation
  if (!modalForm.value.title.trim()) {
    modalError.value = 'Tiêu đề là bắt buộc.'
    return
  }
  if (modalForm.value.type === 'navigation' && !modalForm.value.targetPanoramaId) {
    modalError.value = 'Cảnh chuyển đến là bắt buộc.'
    return
  }
  if (['audio', 'video', 'image', 'model3d'].includes(modalForm.value.type) && !modalForm.value.mediaId) {
    modalError.value = 'Vui lòng chọn tệp tin đa phương tiện.'
    return
  }
  if (modalForm.value.type === 'link' && !modalForm.value.externalUrl.trim()) {
    modalError.value = 'Vui lòng nhập đường dẫn liên kết ngoài.'
    return
  }

  submitting.value = true
  modalError.value = null
  try {
    let response
    const payload = {
      type: modalForm.value.type,
      title: modalForm.value.title,
      description: modalForm.value.description,
      yaw: modalForm.value.yaw,
      pitch: modalForm.value.pitch,
      targetPanoramaId: modalForm.value.type === 'navigation' ? modalForm.value.targetPanoramaId : null,
      mediaId: ['audio', 'video', 'image', 'model3d'].includes(modalForm.value.type) ? modalForm.value.mediaId : null,
      externalUrl: modalForm.value.type === 'link' ? modalForm.value.externalUrl : null,
      icon: modalForm.value.icon,
      displayOrder: modalForm.value.displayOrder
    }

    if (isEditMode.value) {
      payload.status = 'published' // Auto published on edit
      response = await api.put(`/api/admin/hotspots/${editingHotspotId.value}`, payload)
    } else {
      response = await api.post(`/api/admin/panoramas/${panoramaId.value}/hotspots`, payload)
    }

    if (response.success) {
      toastStore.success(isEditMode.value ? 'Cập nhật điểm tương tác thành công!' : 'Tạo điểm tương tác mới thành công!')
      showFormModal.value = false
      await fetchHotspots()
    }
  } catch (err) {
    modalError.value = err.message || 'Lỗi xảy ra khi lưu thông tin.'
  } finally {
    submitting.value = false
  }
}

const confirmDelete = async (id) => {
  const confirmResult = await confirmStore.show({
    title: 'Xóa điểm tương tác',
    message: 'Bạn có chắc chắn muốn xóa điểm tương tác này không?'
  })
  if (!confirmResult) return

  try {
    const response = await api.delete(`/api/admin/hotspots/${id}`)
    if (response.success) {
      toastStore.success('Xóa điểm tương tác thành công.')
      await fetchHotspots()
    }
  } catch (err) {
    toastStore.error(err.message || 'Không thể xóa điểm tương tác.')
  }
}

// Media selector library modal
const openMediaSelector = async () => {
  selectorPage.value = 1
  showMediaSelector.value = true
  await fetchSelectorMedia()
}

const closeMediaSelector = () => {
  showMediaSelector.value = false
}

const fetchSelectorMedia = async () => {
  selectorLoading.value = true
  try {
    const response = await api.get('/api/admin/media', {
      params: {
        mediaType: modalForm.value.type, // Load only matching types (audio/video/image)
        page: selectorPage.value,
        pageSize: selectorPageSize.value
      }
    })
    if (response.success) {
      selectorMedia.value = response.data.items
      selectorPage.value = response.data.page
      selectorTotalPages.value = response.data.totalPages
    }
  } catch (err) {
    console.error('Không thể tải media.', err)
  } finally {
    selectorLoading.value = false
  }
}

const selectMedia = (media) => {
  modalForm.value.mediaId = media.id
  modalForm.value.mediaUrl = media.fileUrl
  closeMediaSelector()
}

const selectorPrevPage = async () => {
  if (selectorPage.value > 1) {
    selectorPage.value--
    await fetchSelectorMedia()
  }
}

const selectorNextPage = async () => {
  if (selectorPage.value < selectorTotalPages.value) {
    selectorPage.value++
    await fetchSelectorMedia()
  }
}

// Media Previews
const closePreview = () => {
  if (audioPlayer.value) {
    audioPlayer.value.pause()
  }
  previewingHotspot.value = null
  audioPlaying.value = false
}

const toggleAudioPlay = () => {
  if (!audioPlayer.value) return
  if (audioPlaying.value) {
    audioPlayer.value.pause()
    audioPlaying.value = false
  } else {
    audioPlayer.value.play()
    audioPlaying.value = true
  }
}

// Helpers
const getFileNameFromUrl = (url) => {
  if (!url) return ''
  return url.substring(url.lastIndexOf('/') + 1)
}

const goBack = () => {
  if (panorama.value) {
    router.push(`/admin/virtual-tours/${panorama.value.virtualTourId}/panoramas`)
  } else {
    router.push('/admin/virtual-tours')
  }
}

// Watch routes changes for in-place scene traversal
watch(() => route.params.id, async (newId) => {
  if (newId) {
    panoramaId.value = newId
    await loadData()
  }
})

onMounted(() => {
  loadData()
})
</script>

<style scoped>
/* Glowing ring effect for double arrow pulse */
@keyframes pulse-ring {
  0% {
    transform: scale(0.65);
    opacity: 1;
  }
  100% {
    transform: scale(1.3);
    opacity: 0;
  }
}
</style>

<style>
/* Global style to apply the float keyframes to custom HTML elements inside Photo Sphere Viewer */
@keyframes float-marker {
  0% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-5px);
  }
  100% {
    transform: translateY(0);
  }
}
</style>
