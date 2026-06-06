<template>
  <div class="space-y-6">
    <!-- Header Page Title & Upload Control -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 bg-white p-6 rounded-2xl border border-slate-200/60 shadow-sm">
      <div class="space-y-1">
        <h2 class="text-lg font-bold text-slate-800">Thư viện Media</h2>
        <p class="text-xs text-slate-500">Quản lý và tải lên hình ảnh, video, hướng dẫn âm thanh và ảnh toàn cảnh 360°.</p>
      </div>
      <button 
        @click="showUploadZone = !showUploadZone"
        class="inline-flex items-center space-x-2 px-4 py-2.5 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold rounded-xl shadow-lg hover:shadow-teal-500/10 active:scale-[0.98] transition duration-150"
      >
        <UploadIcon class="w-4 h-4" />
        <span>{{ showUploadZone ? 'Ẩn bảng tải lên' : 'Tải lên tập tin' }}</span>
      </button>
    </div>

    <!-- Drag & Drop Upload Zone -->
    <div 
      v-if="showUploadZone" 
      class="bg-white p-6 rounded-2xl border border-slate-200/60 shadow-sm space-y-4"
    >
      <div 
        @dragover.prevent="dragOver = true"
        @dragleave.prevent="dragOver = false"
        @drop.prevent="handleFileDrop"
        :class="[
          'border-2 border-dashed rounded-xl p-8 text-center transition cursor-pointer flex flex-col items-center justify-center space-y-3',
          dragOver ? 'border-teal-500 bg-teal-50/20' : 'border-slate-200 hover:border-slate-300 bg-slate-50/50'
        ]"
        @click="triggerFileInput"
      >
        <input 
          ref="fileInputRef" 
          type="file" 
          multiple 
          class="hidden" 
          @change="handleFileSelect"
        />
        <div class="p-4 bg-white text-teal-600 rounded-2xl border border-teal-100/50 shadow-sm">
          <UploadCloudIcon class="w-8 h-8 animate-bounce" />
        </div>
        <div class="space-y-1">
          <p class="text-sm font-bold text-slate-700">Kéo thả các tập tin vào đây hoặc click để duyệt</p>
          <p class="text-xs text-slate-400">Hỗ trợ: Hình ảnh (JPG, PNG, WebP), 360° (Panorama), Video (MP4), Âm thanh (MP3, WAV)</p>
          <p class="text-[11px] text-slate-400">Giới hạn dung lượng: Ảnh & Âm thanh &lt; 10MB | Panorama &lt; 15MB | Video &lt; 50MB</p>
        </div>
      </div>

      <!-- Upload Queue progress list -->
      <div v-if="uploadQueue.length > 0" class="border border-slate-100 rounded-xl overflow-hidden divide-y divide-slate-100 bg-slate-50/30">
        <div class="p-3 bg-slate-50 text-xs font-bold text-slate-500 flex justify-between items-center">
          <span>Tiến trình tải lên ({{ completedUploadsCount }}/{{ uploadQueue.length }})</span>
          <button @click="clearUploadQueue" class="text-red-500 hover:text-red-600 transition">Xóa danh sách</button>
        </div>
        <div 
          v-for="(item, idx) in uploadQueue" 
          :key="idx" 
          class="p-4 flex items-center justify-between text-sm gap-4"
        >
          <div class="flex items-center space-x-3 min-w-0 flex-1">
            <!-- Icon by extensions -->
            <ImageIcon v-if="isImageType(item.name)" class="w-5 h-5 text-teal-500 flex-shrink-0" />
            <FilmIcon v-else-if="isVideoType(item.name)" class="w-5 h-5 text-rose-500 flex-shrink-0" />
            <MusicIcon v-else-if="isAudioType(item.name)" class="w-5 h-5 text-indigo-500 flex-shrink-0" />
            <FileIcon v-else class="w-5 h-5 text-slate-400 flex-shrink-0" />

            <div class="min-w-0 flex-1 space-y-1">
              <p class="text-xs font-bold text-slate-700 truncate" :title="item.name">{{ item.name }}</p>
              <div class="w-full bg-slate-200/60 rounded-full h-1.5 overflow-hidden">
                <div 
                  class="bg-teal-500 h-1.5 transition-all duration-300" 
                  :style="{ width: `${item.progress}%` }"
                ></div>
              </div>
            </div>
          </div>
          
          <div class="flex items-center space-x-2 flex-shrink-0">
            <span class="text-xs font-medium text-slate-500">{{ item.progress }}%</span>
            
            <span v-if="item.status === 'success'" class="p-1 bg-green-50 text-green-600 rounded-full">
              <CheckIcon class="w-4 h-4" />
            </span>
            <span v-else-if="item.status === 'error'" class="p-1 bg-red-50 text-red-600 rounded-full" :title="item.errorMessage">
              <AlertCircleIcon class="w-4 h-4" />
            </span>
            <span v-else-if="item.status === 'uploading'" class="p-1 text-teal-600">
              <Loader2Icon class="w-4 h-4 animate-spin" />
            </span>
            <span v-else class="text-xs text-slate-400">Đang chờ</span>
          </div>
        </div>
      </div>
    </div>

    <!-- Media Tabs Filter & Search -->
    <div class="flex flex-col md:flex-row gap-4 items-start md:items-center justify-between bg-white p-4 rounded-2xl border border-slate-200/60 shadow-sm">
      <!-- Media category types tabs -->
      <div class="flex flex-wrap gap-1 p-1 bg-slate-100 rounded-xl w-full md:w-auto">
        <button
          v-for="tab in mediaTabs"
          :key="tab.value"
          @click="changeTab(tab.value)"
          :class="[
            'px-4 py-2 text-xs font-bold rounded-lg transition duration-200 flex-1 md:flex-initial text-center',
            activeTab === tab.value 
              ? 'bg-white text-slate-900 shadow-sm' 
              : 'text-slate-500 hover:text-slate-800'
          ]"
        >
          {{ tab.label }}
        </button>
      </div>

      <!-- Quick manual page sizes -->
      <div class="flex items-center space-x-2 text-xs text-slate-500 font-medium w-full md:w-auto justify-end">
        <span>Hiển thị:</span>
        <select 
          v-model="pageSize" 
          @change="handlePageSizeChange"
          class="bg-slate-50 border border-slate-200 rounded-lg px-2 py-1 text-slate-700 focus:outline-none"
        >
          <option :value="12">12</option>
          <option :value="24">24</option>
          <option :value="48">48</option>
        </select>
      </div>
    </div>

    <!-- Error state -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-xl flex items-center space-x-3">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0" />
      <span class="text-sm font-medium">{{ error }}</span>
    </div>

    <!-- Loading grid state -->
    <div v-if="loading" class="grid grid-cols-2 sm:grid-cols-4 lg:grid-cols-6 gap-6">
      <div v-for="i in pageSize" :key="i" class="bg-white rounded-2xl border border-slate-100 p-3 space-y-3 animate-pulse shadow-sm">
        <div class="aspect-square bg-slate-100 rounded-xl w-full"></div>
        <div class="h-4 bg-slate-100 rounded w-3/4"></div>
        <div class="h-3 bg-slate-100 rounded w-1/2"></div>
      </div>
    </div>

    <!-- Empty library state -->
    <div 
      v-else-if="mediaFiles.length === 0" 
      class="bg-white py-16 text-center rounded-2xl border border-slate-200/60 shadow-sm space-y-4"
    >
      <div class="inline-flex p-4 bg-slate-50 text-slate-400 rounded-full border border-slate-100">
        <InboxIcon class="w-10 h-10" />
      </div>
      <div class="space-y-1">
        <h3 class="text-base font-bold text-slate-700">Chưa có tập tin media nào</h3>
        <p class="text-xs text-slate-500 max-w-sm mx-auto">Tải lên các ảnh, video, audio đầu tiên để sử dụng làm tư liệu hiển thị.</p>
      </div>
      <button 
        @click="showUploadZone = true" 
        class="text-xs font-bold text-teal-600 hover:text-teal-700 underline"
      >
        Tải lên ngay bây giờ
      </button>
    </div>

    <!-- Media Gallery Grid Display -->
    <div v-else class="grid grid-cols-2 sm:grid-cols-4 lg:grid-cols-6 gap-6">
      <div 
        v-for="m in mediaFiles" 
        :key="m.id" 
        class="bg-white rounded-2xl border border-slate-200/60 hover:border-teal-500/50 shadow-sm overflow-hidden flex flex-col group hover:shadow-md cursor-pointer transition duration-200"
        @click="openDetailModal(m)"
      >
        <!-- Preview Container -->
        <div class="aspect-square bg-slate-50 relative overflow-hidden flex items-center justify-center border-b border-slate-100">
          
          <!-- IMAGE / PANORAMA -->
          <template v-if="m.mediaType === 'image' || m.mediaType === 'panorama'">
            <img 
              :src="m.fileUrl" 
              class="w-full h-full object-cover group-hover:scale-105 transition duration-300" 
              loading="lazy"
            />
            <!-- 360 Badge for Panorama -->
            <span 
              v-if="m.mediaType === 'panorama'" 
              class="absolute top-2 right-2 px-1.5 py-0.5 bg-slate-900/80 text-[9px] font-black tracking-wider text-teal-400 rounded uppercase"
            >
              360° Pano
            </span>
          </template>

          <!-- VIDEO -->
          <template v-else-if="m.mediaType === 'video'">
            <div class="w-full h-full flex flex-col items-center justify-center bg-slate-900/5 text-slate-500 group-hover:text-rose-500 transition">
              <FilmIcon class="w-8 h-8" />
              <span class="text-[10px] text-slate-400 font-bold uppercase mt-1">Video</span>
            </div>
            <!-- Video Badge -->
            <span class="absolute top-2 right-2 px-1.5 py-0.5 bg-slate-900/80 text-[9px] font-black tracking-wider text-rose-400 rounded uppercase">
              {{ getFileExt(m.fileName) }}
            </span>
          </template>

          <!-- AUDIO -->
          <template v-else-if="m.mediaType === 'audio'">
            <div class="w-full h-full flex flex-col items-center justify-center bg-slate-900/5 text-slate-500 group-hover:text-indigo-500 transition">
              <MusicIcon class="w-8 h-8" />
              <span class="text-[10px] text-slate-400 font-bold uppercase mt-1">Audio</span>
            </div>
            <!-- Audio Badge -->
            <span class="absolute top-2 right-2 px-1.5 py-0.5 bg-slate-900/80 text-[9px] font-black tracking-wider text-indigo-400 rounded uppercase">
              {{ getFileExt(m.fileName) }}
            </span>
          </template>

          <!-- OTHER UNKNOWN -->
          <template v-else>
            <div class="w-full h-full flex flex-col items-center justify-center bg-slate-50 text-slate-400">
              <FileIcon class="w-8 h-8" />
              <span class="text-[9px] font-bold mt-1">Unknown</span>
            </div>
          </template>

          <!-- Cover Image Indicators (Shows if destination uses this image as cover) -->
          <span 
            v-if="m.isCover"
            class="absolute bottom-2 left-2 px-2 py-0.5 bg-teal-500 text-slate-950 font-bold text-[9px] rounded-full shadow-sm"
          >
            Ảnh bìa
          </span>
        </div>

        <!-- Meta Details Box -->
        <div class="p-3 space-y-1 flex-1 flex flex-col justify-between">
          <p class="text-xs font-bold text-slate-700 truncate" :title="m.caption || m.fileName">
            {{ m.caption || m.fileName }}
          </p>
          <div class="flex items-center justify-between text-[10px] text-slate-400 font-medium">
            <span>{{ formatBytes(m.fileSize) }}</span>
            <span class="font-mono text-[9px] uppercase">{{ m.extension.replace('.', '') }}</span>
          </div>
        </div>
      </div>
    </div>

    <!-- Pagination controls -->
    <div 
      v-if="mediaFiles.length > 0" 
      class="bg-white px-6 py-4 rounded-2xl border border-slate-200/60 shadow-sm flex flex-col sm:flex-row justify-between items-center gap-4"
    >
      <span class="text-xs text-slate-500 font-medium">
        Hiển thị trang {{ currentPage }}/{{ totalPages }} (Tổng {{ totalItems }} kết quả)
      </span>
      <div class="flex items-center space-x-2">
        <button 
          @click="prevPage" 
          :disabled="currentPage === 1"
          class="px-3 py-1.5 bg-white border border-slate-200 rounded-lg text-xs font-semibold text-slate-600 hover:bg-slate-50 active:scale-[0.98] transition disabled:opacity-40 disabled:cursor-not-allowed"
        >
          Trang trước
        </button>
        <button 
          @click="nextPage" 
          :disabled="currentPage === totalPages || totalPages === 0"
          class="px-3 py-1.5 bg-white border border-slate-200 rounded-lg text-xs font-semibold text-slate-600 hover:bg-slate-50 active:scale-[0.98] transition disabled:opacity-40 disabled:cursor-not-allowed"
        >
          Trang sau
        </button>
      </div>
    </div>

    <!-- Media Detail & Metadata Editor Modal Dialog -->
    <div 
      v-if="showDetailModal" 
      class="fixed inset-0 z-50 overflow-y-auto bg-slate-900/60 backdrop-blur-sm flex items-center justify-center p-4"
      @click.self="closeDetailModal"
    >
      <div class="bg-white rounded-2xl w-full max-w-4xl shadow-2xl border border-slate-100 overflow-hidden flex flex-col md:flex-row max-h-[90vh]">
        
        <!-- Left half: Preview Block (45%) -->
        <div class="md:w-1/2 bg-slate-950 p-6 flex flex-col items-center justify-center min-h-[300px] md:min-h-0 relative">
          <!-- Close Modal Icon (Mobile) -->
          <button 
            @click="closeDetailModal"
            class="md:hidden absolute top-4 right-4 p-2 bg-slate-800 text-white rounded-full hover:bg-slate-700 transition"
          >
            <XIcon class="w-4 h-4" />
          </button>

          <!-- IMAGE / PANORAMA PREVIEW -->
          <img 
            v-if="selectedMedia.mediaType === 'image' || selectedMedia.mediaType === 'panorama'"
            :src="selectedMedia.fileUrl" 
            class="max-w-full max-h-[50vh] md:max-h-[60vh] object-contain rounded-lg"
          />

          <!-- VIDEO PREVIEW -->
          <video 
            v-else-if="selectedMedia.mediaType === 'video'" 
            controls 
            class="max-w-full max-h-[50vh] md:max-h-[60vh] rounded-lg shadow-lg"
            :src="selectedMedia.fileUrl"
          ></video>

          <!-- AUDIO PREVIEW -->
          <div 
            v-else-if="selectedMedia.mediaType === 'audio'"
            class="text-center space-y-4 w-full px-6 py-10"
          >
            <div class="w-20 h-20 bg-teal-500/10 text-teal-400 rounded-full flex items-center justify-center mx-auto border border-teal-500/20">
              <MusicIcon class="w-10 h-10 animate-pulse" />
            </div>
            <div class="space-y-1">
              <p class="text-sm font-bold text-white truncate max-w-xs mx-auto">{{ selectedMedia.fileName }}</p>
              <p class="text-xs text-slate-400">Audio Player Guide</p>
            </div>
            <audio 
              controls 
              class="w-full mx-auto shadow-md"
              :src="selectedMedia.fileUrl"
            ></audio>
          </div>

          <!-- UNKNOWN -->
          <div v-else class="text-center text-slate-400 py-10">
            <FileIcon class="w-16 h-16 mx-auto text-slate-600 mb-2" />
            <p class="text-sm font-bold">Không hỗ trợ xem thử trực tiếp</p>
          </div>
        </div>

        <!-- Right half: Detailed Metadata & Action Form (55%) -->
        <div class="md:w-1/2 p-6 md:p-8 flex flex-col justify-between overflow-y-auto max-h-[90vh]">
          <!-- Header -->
          <div class="flex justify-between items-start">
            <div class="space-y-1">
              <h3 class="text-base font-bold text-slate-800">Chi tiết tập tin</h3>
              <p class="text-xs text-slate-500">Xem thông số kỹ thuật và chỉnh sửa mô tả hiển thị.</p>
            </div>
            <button 
              @click="closeDetailModal"
              class="hidden md:block p-1.5 hover:bg-slate-100 rounded-lg text-slate-400 hover:text-slate-600 transition"
            >
              <XIcon class="w-5 h-5" />
            </button>
          </div>

          <!-- Details fields info list -->
          <div class="mt-6 space-y-4 text-xs">
            <div class="grid grid-cols-3 gap-2 py-1 border-b border-slate-100">
              <span class="text-slate-400 font-medium">Tên file gốc:</span>
              <span class="col-span-2 font-bold text-slate-700 break-all">{{ selectedMedia.fileName }}</span>
            </div>
            <div class="grid grid-cols-3 gap-2 py-1 border-b border-slate-100">
              <span class="text-slate-400 font-medium">Loại tệp tin:</span>
              <span class="col-span-2 font-bold text-slate-700 capitalize font-mono text-[10px]">{{ selectedMedia.mediaType }} ({{ selectedMedia.mimeType }})</span>
            </div>
            <div class="grid grid-cols-3 gap-2 py-1 border-b border-slate-100">
              <span class="text-slate-400 font-medium">Kích thước:</span>
              <span class="col-span-2 font-bold text-slate-700">{{ formatBytes(selectedMedia.fileSize) }}</span>
            </div>
            <div v-if="selectedMedia.width && selectedMedia.height" class="grid grid-cols-3 gap-2 py-1 border-b border-slate-100">
              <span class="text-slate-400 font-medium">Độ phân giải:</span>
              <span class="col-span-2 font-bold text-slate-700 font-mono">{{ selectedMedia.width }} x {{ selectedMedia.height }} px</span>
            </div>
            <div class="grid grid-cols-3 gap-2 py-1 border-b border-slate-100">
              <span class="text-slate-400 font-medium">Ngày tải lên:</span>
              <span class="col-span-2 font-bold text-slate-700">{{ formatDate(selectedMedia.createdAt) }}</span>
            </div>
            
            <!-- URL copy component -->
            <div class="space-y-1.5 pt-2">
              <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Đường dẫn công khai (URL)</label>
              <div class="flex items-center space-x-2">
                <input 
                  type="text" 
                  readonly 
                  :value="selectedMedia.fileUrl" 
                  class="flex-1 bg-slate-50 border border-slate-200 rounded-lg px-3 py-2 text-slate-600 focus:outline-none font-mono text-[11px]"
                />
                <button 
                  @click="copyUrlToClipboard(selectedMedia.fileUrl)"
                  class="p-2 border border-slate-200 rounded-lg hover:bg-slate-50 text-slate-500 active:scale-[0.98] transition flex items-center justify-center"
                  :title="copied ? 'Đã sao chép!' : 'Sao chép URL'"
                >
                  <CheckIcon v-if="copied" class="w-4 h-4 text-green-500" />
                  <CopyIcon v-else class="w-4 h-4" />
                </button>
              </div>
            </div>

            <!-- Form Edit Metadata Fields -->
            <div class="space-y-3 pt-3 border-t border-slate-100">
              <div class="space-y-1.5">
                <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Alt Text (Mô tả nội dung cho SEO) *</label>
                <input 
                  v-model="metaForm.altText"
                  type="text" 
                  placeholder="Ví dụ: Cận cảnh cổng tam quan chùa cổ..."
                  class="w-full px-3 py-2 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500"
                />
              </div>

              <div class="space-y-1.5">
                <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Chú thích hiển thị (Caption)</label>
                <textarea 
                  v-model="metaForm.caption"
                  rows="2"
                  placeholder="Chú thích hiển thị dưới ảnh trong bộ sưu tập hoặc bài viết..."
                  class="w-full px-3 py-2 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500 resize-none"
                ></textarea>
              </div>
            </div>
          </div>

          <!-- Actions Buttons bottom panel -->
          <div class="mt-8 flex items-center justify-between gap-4 border-t border-slate-100 pt-6">
            <!-- Delete Button -->
            <button 
              @click="confirmDeleteMedia(selectedMedia)"
              class="px-4 py-2.5 border border-red-200 hover:bg-red-50 text-red-600 font-bold rounded-xl text-xs active:scale-[0.98] transition flex items-center space-x-1.5"
              :disabled="updatingMeta"
            >
              <TrashIcon class="w-4 h-4" />
              <span>Xóa vĩnh viễn</span>
            </button>

            <div class="flex items-center space-x-2">
              <button 
                @click="closeDetailModal"
                class="px-4 py-2.5 border border-slate-200 hover:bg-slate-50 text-slate-500 font-bold rounded-xl text-xs transition"
                :disabled="updatingMeta"
              >
                Hủy bỏ
              </button>
              <button 
                @click="saveMetadata"
                class="px-4 py-2.5 bg-slate-900 hover:bg-slate-800 text-white font-bold rounded-xl text-xs shadow-lg flex items-center space-x-1.5 disabled:opacity-50"
                :disabled="updatingMeta"
              >
                <Loader2Icon v-if="updatingMeta" class="w-4 h-4 animate-spin" />
                <span>Lưu thông tin</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import api from '../../utils/api'
import { useToastStore } from '../../stores/toast'
import { useConfirmStore } from '../../stores/confirm'
import { 
  Upload as UploadIcon,
  UploadCloud as UploadCloudIcon,
  Image as ImageIcon,
  Film as FilmIcon,
  Music as MusicIcon,
  File as FileIcon,
  Check as CheckIcon,
  Copy as CopyIcon,
  Trash as TrashIcon,
  X as XIcon,
  Inbox as InboxIcon,
  AlertCircle as AlertCircleIcon,
  Loader2 as Loader2Icon
} from 'lucide-vue-next'

const toastStore = useToastStore()
const confirmStore = useConfirmStore()

const showUploadZone = ref(false)
const dragOver = ref(false)
const fileInputRef = ref(null)

// Upload Queue management
const uploadQueue = ref([])

const completedUploadsCount = computed(() => {
  return uploadQueue.value.filter(i => i.status === 'success' || i.status === 'error').length
})

// Tab filter setup
const activeTab = ref('')
const mediaTabs = [
  { label: 'Tất cả', value: '' },
  { label: 'Hình ảnh', value: 'image' },
  { label: '360° Panorama', value: 'panorama' },
  { label: 'Video', value: 'video' },
  { label: 'Âm thanh', value: 'audio' }
]

const loading = ref(false)
const error = ref(null)
const mediaFiles = ref([])

// Pagination
const currentPage = ref(1)
const pageSize = ref(12)
const totalItems = ref(0)
const totalPages = ref(0)

// Modal Details State
const showDetailModal = ref(false)
const selectedMedia = ref(null)
const metaForm = ref({
  caption: '',
  altText: ''
})
const updatingMeta = ref(false)
const copied = ref(false)

const fetchMediaFiles = async () => {
  loading.value = true
  error.value = null
  try {
    const response = await api.get('/api/admin/media', {
      params: {
        mediaType: activeTab.value || undefined,
        page: currentPage.value,
        pageSize: pageSize.value
      }
    })

    if (response.success) {
      mediaFiles.value = response.data.items
      currentPage.value = response.data.page
      pageSize.value = response.data.pageSize
      totalItems.value = response.data.totalItems
      totalPages.value = response.data.totalPages
    }
  } catch (err) {
    error.value = err.message || 'Không thể tải danh sách tập tin media.'
  } finally {
    loading.value = false
  }
}

const changeTab = (tabValue) => {
  activeTab.value = tabValue
  currentPage.value = 1
  fetchMediaFiles()
}

const handlePageSizeChange = () => {
  currentPage.value = 1
  fetchMediaFiles()
}

const prevPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--
    fetchMediaFiles()
  }
}

const nextPage = () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++
    fetchMediaFiles()
  }
}

// Upload zone handling
const triggerFileInput = () => {
  fileInputRef.value.click()
}

const handleFileSelect = (e) => {
  const files = e.target.files
  if (files) {
    enqueueFiles(Array.from(files))
  }
  // Reset select input
  e.target.value = ''
}

const handleFileDrop = (e) => {
  dragOver.value = false
  const files = e.dataTransfer.files
  if (files) {
    enqueueFiles(Array.from(files))
  }
}

const enqueueFiles = (filesList) => {
  filesList.forEach(file => {
    const item = {
      file: file,
      name: file.name,
      progress: 0,
      status: 'pending',
      errorMessage: ''
    }
    uploadQueue.value.push(item)
    uploadSingleFile(item)
  })
}

const uploadSingleFile = async (queueItem) => {
  queueItem.status = 'uploading'
  
  const formData = new FormData()
  formData.append('file', queueItem.file)
  
  // Detect Panorama type if name contains panorama to help auto classify
  const ext = getFileExt(queueItem.name).toLowerCase()
  let autoType = null
  if (ext === '.jpg' || ext === '.jpeg' || ext === '.png' || ext === '.webp') {
    if (queueItem.name.toLowerCase().includes('pano') || queueItem.name.toLowerCase().includes('panorama')) {
      autoType = 'panorama'
    } else {
      autoType = 'image'
    }
  } else if (ext === '.mp4') {
    autoType = 'video'
  } else if (ext === '.mp3' || ext === '.wav') {
    autoType = 'audio'
  }
  
  if (autoType) {
    formData.append('mediaType', autoType)
  }

  try {
    const response = await api.post('/api/admin/media/upload', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      },
      onUploadProgress: (progressEvent) => {
        const percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total)
        queueItem.progress = percentCompleted
      }
    })

    if (response.success) {
      queueItem.status = 'success'
      queueItem.progress = 100
      // Refresh listing
      fetchMediaFiles()
    } else {
      queueItem.status = 'error'
      queueItem.errorMessage = response.message || 'Lỗi tải lên.'
    }
  } catch (err) {
    queueItem.status = 'error'
    queueItem.errorMessage = err.message || 'Lỗi mạng hoặc kích thước quá lớn.'
  }
}

const clearUploadQueue = () => {
  uploadQueue.value = []
}

// Modal actions
const openDetailModal = (media) => {
  selectedMedia.value = media
  metaForm.value.caption = media.caption || ''
  metaForm.value.altText = media.altText || ''
  copied.value = false
  showDetailModal.value = true
}

const closeDetailModal = () => {
  showDetailModal.value = false
  selectedMedia.value = null
}

const copyUrlToClipboard = async (url) => {
  try {
    await navigator.clipboard.writeText(url)
    copied.value = true
    setTimeout(() => {
      copied.value = false
    }, 2000)
  } catch (err) {
    console.error('Không thể sao chép URL.', err)
  }
}

const saveMetadata = async () => {
  updatingMeta.value = true
  try {
    const response = await api.put(`/api/admin/media/${selectedMedia.value.id}`, {
      caption: metaForm.value.caption,
      altText: metaForm.value.altText
    })

    if (response.success) {
      // Update locally
      selectedMedia.value.caption = response.data.caption
      selectedMedia.value.altText = response.data.altText
      // Find in list and update
      const index = mediaFiles.value.findIndex(m => m.id === selectedMedia.value.id)
      if (index !== -1) {
        mediaFiles.value[index].caption = response.data.caption
        mediaFiles.value[index].altText = response.data.altText
      }
      toastStore.success('Cập nhật thông tin media thành công.')
      closeDetailModal()
    }
  } catch (err) {
    toastStore.error(err.message || 'Không thể lưu thông tin media.')
  } finally {
    updatingMeta.value = false
  }
}

const confirmDeleteMedia = async (media) => {
  const confirmResult = await confirmStore.show({
    title: 'Xóa vĩnh viễn tệp media',
    message: `Bạn có chắc chắn muốn xóa vĩnh viễn tập tin "${media.fileName}"? Hành động này không thể hoàn tác.`
  })
  if (!confirmResult) return

  updatingMeta.value = true
  try {
    const response = await api.delete(`/api/admin/media/${media.id}`)
    if (response.success) {
      toastStore.success('Đã xóa tập tin media thành công.')
      closeDetailModal()
      fetchMediaFiles()
    }
  } catch (err) {
    toastStore.error(err.message || 'Không thể xóa tập tin media. Tập tin có thể đang được liên kết sử dụng.')
  } finally {
    updatingMeta.value = false
  }
}

// Helpers
const getFileExt = (filename) => {
  return filename.slice(((filename.lastIndexOf(".") - 1) >>> 0) + 2)
}

const isImageType = (filename) => {
  const ext = getFileExt(filename).toLowerCase()
  return ['jpg', 'jpeg', 'png', 'webp', 'gif'].includes(ext)
}

const isVideoType = (filename) => {
  const ext = getFileExt(filename).toLowerCase()
  return ['mp4', 'mov', 'webm'].includes(ext)
}

const isAudioType = (filename) => {
  const ext = getFileExt(filename).toLowerCase()
  return ['mp3', 'wav', 'ogg'].includes(ext)
}

const formatBytes = (bytes, decimals = 2) => {
  if (!+bytes) return '0 Bytes'
  const k = 1024
  const dm = decimals < 0 ? 0 : decimals
  const sizes = ['Bytes', 'KB', 'MB', 'GB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return `${parseFloat((bytes / Math.pow(k, i)).toFixed(dm))} ${sizes[i]}`
}

const formatDate = (dateStr) => {
  if (!dateStr) return ''
  const date = new Date(dateStr)
  return date.toLocaleString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  })
}

onMounted(() => {
  fetchMediaFiles()
})
</script>
