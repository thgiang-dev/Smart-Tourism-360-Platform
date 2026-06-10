<template>
  <div class="space-y-6 font-sans">
    <!-- Header Page Actions -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-6 bg-gradient-to-r from-teal-900 via-teal-800 to-slate-900 rounded-3xl p-8 text-white shadow-xl relative overflow-hidden">
      <div class="absolute inset-0 opacity-10 bg-[radial-gradient(#e5e7eb_1px,transparent_1px)] [background-size:16px_16px]"></div>
      <div class="absolute w-64 h-64 rounded-full bg-teal-500/10 blur-3xl -top-20 -right-20 pointer-events-none"></div>
      <div class="relative z-10 space-y-1">
        <h2 class="text-xl md:text-2xl font-black tracking-tight text-white">Danh sách Tour ảo 360°</h2>
        <p class="text-xs md:text-sm text-teal-200/80 font-medium">Quản lý các tour tham quan thực tế ảo của địa điểm du lịch.</p>
      </div>
      <button 
        @click="openCreateModal"
        class="relative z-10 inline-flex items-center space-x-2 px-5 py-3 bg-amber-500 hover:bg-amber-600 text-slate-950 font-bold rounded-xl shadow-lg hover:shadow-amber-500/20 active:scale-[0.98] transition duration-150"
      >
        <PlusIcon class="w-4 h-4" />
        <span>Thêm Tour ảo</span>
      </button>
    </div>

    <!-- Filters Bar -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 bg-white p-5 rounded-2xl border border-slate-200/70 shadow-sm">
      <!-- Search -->
      <div class="relative lg:col-span-2">
        <input 
          v-model="searchKeyword"
          type="text" 
          placeholder="Tìm theo tiêu đề, địa điểm..." 
          class="w-full pl-10 pr-4 py-3 bg-slate-50 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/10 transition"
        />
        <span class="absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400">
          <SearchIcon class="w-4 h-4" />
        </span>
      </div>

      <!-- Status Filter -->
      <select 
        v-model="selectedStatus"
        class="px-3 py-3 bg-slate-50 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/10 transition"
      >
        <option :value="null">Tất cả Trạng thái</option>
        <option value="published">Đã xuất bản (Published)</option>
        <option value="draft">Bản nháp (Draft)</option>
      </select>

      <button 
        @click="resetFilters"
        class="premium-btn py-3 border border-slate-200 hover:bg-slate-50 text-slate-600 font-semibold text-sm rounded-xl transition duration-150"
      >
        Đặt lại bộ lọc
      </button>
    </div>

    <!-- Loading state -->
    <div v-if="loading" class="bg-white rounded-2xl border border-slate-200/60 shadow-sm p-6 space-y-4">
      <div v-for="i in 3" :key="i" class="flex items-center justify-between py-4 border-b border-slate-100 last:border-0 animate-pulse">
        <div class="flex items-center space-x-4">
          <div class="w-20 h-14 bg-slate-100 rounded-lg"></div>
          <div class="space-y-2">
            <div class="h-4 w-48 bg-slate-100 rounded"></div>
            <div class="h-3 w-32 bg-slate-100 rounded"></div>
          </div>
        </div>
        <div class="h-8 w-24 bg-slate-100 rounded-lg"></div>
      </div>
    </div>

    <!-- Error Alert -->
    <div v-else-if="error" class="bg-rose-50 border border-rose-200 text-rose-700 px-4 py-3 rounded-xl flex items-center space-x-3 shadow-sm">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0 text-rose-500" />
      <span class="text-sm font-medium">{{ error }}</span>
    </div>

    <!-- Empty State -->
    <div v-else-if="filteredTours.length === 0" class="bg-white py-16 text-center rounded-2xl border border-slate-200/60 shadow-sm space-y-4">
      <div class="inline-flex p-4 bg-slate-50 text-slate-400 rounded-full border border-slate-100">
        <InboxIcon class="w-10 h-10" />
      </div>
      <div class="space-y-1">
        <h3 class="text-base font-bold text-slate-700">Không tìm thấy Tour ảo nào</h3>
        <p class="text-xs text-slate-500 max-w-sm mx-auto">Chưa có Tour ảo được tạo hoặc không có kết quả phù hợp với bộ lọc tìm kiếm.</p>
      </div>
      <button @click="resetFilters" class="text-xs font-bold text-teal-600 hover:text-teal-700 underline">
        Đặt lại bộ lọc
      </button>
    </div>

    <!-- Grid Layout of Tours -->
    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div 
        v-for="tour in filteredTours" 
        :key="tour.id"
        class="premium-card bg-white rounded-2xl border border-slate-200/70 shadow-sm overflow-hidden flex flex-col group"
      >
        <!-- Thumbnail -->
        <div class="aspect-[16/10] w-full bg-slate-100 border-b border-slate-100 relative overflow-hidden flex items-center justify-center text-slate-400">
          <img 
            v-if="tour.thumbnailUrl" 
            :src="tour.thumbnailUrl" 
            class="w-full h-full object-cover group-hover:scale-105 transition duration-500"
            alt="Tour cover image"
          />
          <NavigationIcon v-else class="w-10 h-10 text-slate-300" />
          
          <!-- Status Badge -->
          <span 
            class="absolute top-4 right-4 inline-flex items-center px-3 py-1 rounded-full text-[10px] font-black uppercase tracking-wider shadow-sm border"
            :class="tour.status === 'published' ? 'bg-teal-50 text-teal-700 border-teal-200/60' : 'bg-amber-50 text-amber-700 border-amber-200/60'"
          >
            {{ tour.status === 'published' ? 'Đã xuất bản' : 'Bản nháp' }}
          </span>
        </div>

        <!-- Details -->
        <div class="p-6 flex-grow flex flex-col justify-between space-y-4">
          <div class="space-y-2">
            <h3 class="font-bold text-slate-800 text-base line-clamp-1 group-hover:text-teal-700 transition">{{ tour.title }}</h3>
            <div class="flex items-center text-xs text-slate-500 space-x-1">
              <MapPinIcon class="w-3.5 h-3.5 text-slate-400" />
              <span class="truncate font-semibold text-slate-700">{{ tour.destinationName }}</span>
            </div>
            <p class="text-xs text-slate-400 line-clamp-2">{{ tour.description || 'Không có mô tả cho tour này.' }}</p>
          </div>

          <div class="pt-4 border-t border-slate-100 flex items-center justify-between">
            <span class="inline-flex items-center text-xs font-medium text-slate-500">
              <ImageIcon class="w-3.5 h-3.5 text-slate-400 mr-1.5" />
              <strong>{{ tour.panoramasCount }}</strong>&nbsp;cảnh 360°
            </span>

            <div class="flex items-center space-x-1">
              <!-- Manage scenes button -->
              <router-link
                :to="`/admin/virtual-tours/${tour.id}/panoramas`"
                class="p-2 hover:bg-teal-50 rounded-lg text-slate-500 hover:text-teal-600 transition"
                title="Quản lý các cảnh"
              >
                <CompassIcon class="w-4.5 h-4.5" />
              </router-link>

              <!-- Edit button -->
              <button
                @click="openEditModal(tour)"
                class="p-2 hover:bg-indigo-50 rounded-lg text-slate-500 hover:text-indigo-600 transition"
                title="Chỉnh sửa tour"
              >
                <EditIcon class="w-4 h-4" />
              </button>

              <!-- Delete button -->
              <button
                @click="confirmDelete(tour)"
                class="p-2 hover:bg-rose-50 rounded-lg text-slate-500 hover:text-rose-600 transition"
                title="Xóa tour ảo"
              >
                <TrashIcon class="w-4 h-4" />
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal Form (Create / Edit) -->
    <div 
      v-if="showFormModal" 
      class="fixed inset-0 z-50 overflow-y-auto bg-slate-900/60 backdrop-blur-sm flex items-center justify-center p-4"
      @click.self="closeFormModal"
    >
      <div class="bg-white rounded-3xl w-full max-w-lg shadow-2xl border border-slate-100 overflow-hidden flex flex-col animate-in fade-in zoom-in-95 duration-200">
        <!-- Modal Header -->
        <div class="p-6 border-b border-slate-100 flex justify-between items-center bg-slate-50/50">
          <div class="space-y-1">
            <h3 class="text-base font-black text-slate-800">
              {{ isEditMode ? 'Chỉnh sửa Tour ảo 360°' : 'Thêm Tour ảo mới' }}
            </h3>
            <p class="text-xs text-slate-500 font-medium">
              Thiết lập thông tin chung cho tour ảo của địa điểm.
            </p>
          </div>
          <button 
            type="button"
            @click="closeFormModal"
            class="p-2 hover:bg-slate-100 rounded-xl text-slate-400 hover:text-slate-600 transition"
          >
            <XIcon class="w-4 h-4" />
          </button>
        </div>

        <!-- Modal Body -->
        <form @submit.prevent="submitForm" class="p-6 space-y-5">
          <!-- Destination selection -->
          <div>
            <label class="block text-[10px] font-black text-slate-400 uppercase tracking-wider mb-2">Địa điểm liên kết *</label>
            <select 
              v-model="modalForm.destinationId"
              required
              :disabled="isEditMode"
              class="w-full px-3 py-3 bg-slate-50 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/10 transition disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <option :value="null" disabled>Chọn địa điểm du lịch</option>
              <option 
                v-for="dest in availableDestinations" 
                :key="dest.id" 
                :value="dest.id"
              >
                {{ dest.name }}
              </option>
            </select>
          </div>

          <!-- Title -->
          <div>
            <label class="block text-[10px] font-black text-slate-400 uppercase tracking-wider mb-2">Tiêu đề Tour ảo *</label>
            <input 
              v-model="modalForm.title"
              type="text"
              required
              placeholder="Ví dụ: Tour tham quan 360° Chùa Ông"
              class="w-full px-4 py-3 bg-slate-50 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/10 transition"
            />
          </div>

          <!-- Description -->
          <div>
            <label class="block text-[10px] font-black text-slate-400 uppercase tracking-wider mb-2">Mô tả ngắn</label>
            <textarea 
              v-model="modalForm.description"
              rows="3"
              placeholder="Giới thiệu đôi nét về tour tham quan ảo này..."
              class="w-full px-4 py-3 bg-slate-50 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/10 transition resize-none"
            ></textarea>
          </div>

          <!-- Thumbnail Picker -->
          <div>
            <label class="block text-[10px] font-black text-slate-400 uppercase tracking-wider mb-2">Ảnh đại diện (Thumbnail)</label>
            <div class="relative w-full aspect-[16/9] rounded-2xl overflow-hidden border border-slate-200 bg-slate-50 flex flex-col items-center justify-center group">
              <img 
                v-if="modalForm.thumbnailUrl" 
                :src="modalForm.thumbnailUrl" 
                class="w-full h-full object-cover" 
              />
              <div v-else class="text-center p-6 space-y-2 text-slate-400">
                <ImageIcon class="w-8 h-8 mx-auto text-slate-300" />
                <p class="text-xs font-semibold">Chưa chọn ảnh đại diện</p>
              </div>
              
              <div 
                v-if="modalForm.thumbnailUrl" 
                class="absolute inset-0 bg-slate-900/60 opacity-0 group-hover:opacity-100 transition flex items-center justify-center space-x-2"
              >
                <button 
                  type="button"
                  @click="openMediaSelector"
                  class="px-3.5 py-2 bg-white text-slate-900 rounded-xl text-xs font-black hover:bg-slate-50 transition"
                >
                  Thay đổi
                </button>
                <button 
                  type="button"
                  @click="removeThumbnail"
                  class="px-3.5 py-2 bg-rose-600 text-white rounded-xl text-xs font-black hover:bg-rose-700 transition"
                >
                  Xóa
                </button>
              </div>
            </div>

            <div v-if="!modalForm.thumbnailUrl" class="mt-2 flex items-center space-x-2">
              <button
                type="button"
                @click="openMediaSelector"
                class="premium-btn py-2 px-3 border border-slate-200 hover:bg-slate-50 text-slate-700 font-bold rounded-xl text-xs transition flex items-center space-x-1"
              >
                <ImageIcon class="w-3.5 h-3.5 text-slate-500" />
                <span>Chọn từ thư viện</span>
              </button>
              
              <button
                type="button"
                @click="triggerDirectUpload"
                class="premium-btn py-2 px-3 bg-slate-900 hover:bg-slate-800 text-white font-bold rounded-xl text-xs transition flex items-center space-x-1"
              >
                <UploadIcon class="w-3.5 h-3.5" />
                <span>Tải ảnh mới</span>
              </button>
              <input 
                ref="directUploadInput" 
                type="file" 
                accept="image/*" 
                class="hidden" 
                @change="handleDirectUpload"
              />
            </div>
          </div>

          <!-- Status selection -->
          <div>
            <label class="block text-[10px] font-black text-slate-400 uppercase tracking-wider mb-2">Trạng thái xuất bản</label>
            <div class="flex items-center space-x-6">
              <label class="flex items-center space-x-2 cursor-pointer group">
                <input type="radio" v-model="modalForm.status" value="draft" class="text-teal-600 focus:ring-teal-500" />
                <span class="text-sm font-bold text-slate-700 group-hover:text-slate-900">Lưu nháp (Draft)</span>
              </label>
              <label class="flex items-center space-x-2 cursor-pointer group">
                <input type="radio" v-model="modalForm.status" value="published" class="text-teal-600 focus:ring-teal-500" />
                <span class="text-sm font-bold text-slate-700 group-hover:text-slate-900">Xuất bản (Published)</span>
              </label>
            </div>
          </div>

          <!-- Error Message inside Modal -->
          <div v-if="modalError" class="text-xs font-bold text-rose-600 bg-rose-50 p-3 rounded-xl border border-rose-100">
            {{ modalError }}
          </div>

          <!-- Actions -->
          <div class="flex items-center space-x-3 pt-4 border-t border-slate-100">
            <button 
              type="submit" 
              class="flex-grow py-3 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold rounded-xl shadow-lg hover:shadow-teal-500/10 active:scale-[0.98] transition flex items-center justify-center space-x-2 disabled:opacity-50"
              :disabled="submitting"
            >
              <Loader2Icon v-if="submitting" class="w-4 h-4 animate-spin" />
              <span>{{ isEditMode ? 'Lưu cập nhật' : 'Tạo Tour ảo' }}</span>
            </button>
            <button 
              type="button"
              @click="closeFormModal"
              class="px-5 py-3 border border-slate-200 hover:bg-slate-50 text-slate-600 font-bold rounded-xl text-sm transition"
              :disabled="submitting"
            >
              Hủy
            </button>
          </div>
        </form>
      </div>
    </div>

    <!-- Media Selector Modal -->
    <div 
      v-if="showMediaSelector" 
      class="fixed inset-0 z-[60] overflow-y-auto bg-slate-900/60 backdrop-blur-sm flex items-center justify-center p-4"
      @click.self="closeMediaSelector"
    >
      <div class="bg-white rounded-3xl w-full max-w-2xl shadow-2xl border border-slate-100 overflow-hidden flex flex-col max-h-[80vh] animate-in fade-in zoom-in-95 duration-200">
        <!-- Header -->
        <div class="p-6 border-b border-slate-100 flex justify-between items-center bg-slate-50/50">
          <div class="space-y-1">
            <h3 class="text-base font-black text-slate-800">Chọn ảnh làm bìa</h3>
            <p class="text-xs text-slate-500 font-medium">Chọn một hình ảnh có sẵn trong thư viện media hoặc tải lên ảnh mới.</p>
          </div>
          <button 
            type="button"
            @click="closeMediaSelector"
            class="p-2 hover:bg-slate-100 rounded-xl text-slate-400 hover:text-slate-600 transition"
          >
            <XIcon class="w-4 h-4" />
          </button>
        </div>

        <!-- Modal Upload Section inside Selector -->
        <div class="px-6 py-4 bg-slate-50 border-b border-slate-100 flex items-center justify-between">
          <span class="text-xs text-slate-500 font-semibold">Tải ảnh mới lên thư viện:</span>
          <button 
            type="button"
            @click="triggerModalUpload"
            class="premium-btn px-4 py-2 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold rounded-xl text-xs transition flex items-center space-x-1"
            :disabled="modalUploading"
          >
            <Loader2Icon v-if="modalUploading" class="w-3.5 h-3.5 animate-spin" />
            <UploadIcon v-else class="w-3.5 h-3.5" />
            <span>Tải lên</span>
          </button>
          <input 
            ref="modalUploadInput" 
            type="file" 
            accept="image/*" 
            class="hidden" 
            @change="handleModalUpload"
          />
        </div>

        <!-- Content Area -->
        <div class="p-6 overflow-y-auto flex-1 min-h-[250px]">
          <div v-if="selectorLoading" class="grid grid-cols-4 gap-4 animate-pulse">
            <div v-for="i in 8" :key="i" class="aspect-square bg-slate-100 rounded-2xl"></div>
          </div>

          <div v-else-if="selectorImages.length === 0" class="text-center py-12 space-y-3">
            <ImageIcon class="w-8 h-8 mx-auto text-slate-300" />
            <p class="text-sm font-bold text-slate-500">Thư viện ảnh trống.</p>
          </div>

          <div v-else class="grid grid-cols-4 gap-4">
            <div 
              v-for="img in selectorImages" 
              :key="img.id"
              :class="[
                'aspect-square rounded-2xl overflow-hidden cursor-pointer border-2 transition relative group bg-slate-50 shadow-sm',
                modalForm.thumbnailId === img.id ? 'border-teal-500 shadow-md ring-2 ring-teal-500/20' : 'border-slate-100 hover:border-slate-300'
              ]"
              @click="selectImage(img)"
            >
              <img :src="img.fileUrl" class="w-full h-full object-cover" />
              <div 
                v-if="modalForm.thumbnailId === img.id"
                class="absolute inset-0 bg-teal-500/10 flex items-center justify-center"
              >
                <div class="p-1 bg-teal-500 text-slate-950 rounded-full shadow-md">
                  <CheckIcon class="w-3.5 h-3.5" />
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Footer Pagination -->
        <div 
          v-if="selectorImages.length > 0 && selectorTotalPages > 1"
          class="p-4 bg-slate-50 border-t border-slate-100 flex justify-between items-center text-xs"
        >
          <span class="text-slate-500 font-bold">Trang {{ selectorPage }} / {{ selectorTotalPages }}</span>
          <div class="flex items-center space-x-2">
            <button 
              type="button"
              @click="selectorPrevPage"
              :disabled="selectorPage === 1"
              class="px-3 py-1.5 bg-white border border-slate-200 rounded-lg font-bold hover:bg-slate-50 transition disabled:opacity-40"
            >
              Trước
            </button>
            <button 
              type="button"
              @click="selectorNextPage"
              :disabled="selectorPage === selectorTotalPages"
              class="px-3 py-1.5 bg-white border border-slate-200 rounded-lg font-bold hover:bg-slate-50 transition disabled:opacity-40"
            >
              Sau
            </button>
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
  Plus as PlusIcon, 
  Search as SearchIcon, 
  Image as ImageIcon, 
  Edit as EditIcon, 
  Trash as TrashIcon, 
  Inbox as InboxIcon, 
  AlertCircle as AlertCircleIcon,
  Navigation as NavigationIcon,
  Compass as CompassIcon,
  MapPin as MapPinIcon,
  X as XIcon,
  Loader2 as Loader2Icon,
  Upload as UploadIcon,
  Check as CheckIcon
} from 'lucide-vue-next'

const toastStore = useToastStore()
const confirmStore = useConfirmStore()

const loading = ref(false)
const error = ref(null)

const tours = ref([])
const destinations = ref([])

const searchKeyword = ref('')
const selectedStatus = ref(null)

// Modal Form State
const showFormModal = ref(false)
const isEditMode = ref(false)
const submitting = ref(false)
const modalError = ref(null)
const editingTourId = ref(null)

const modalForm = ref({
  destinationId: null,
  title: '',
  description: '',
  thumbnailId: null,
  thumbnailUrl: null,
  status: 'draft'
})

// Image Selector Logic
const showMediaSelector = ref(false)
const selectorLoading = ref(false)
const selectorImages = ref([])
const selectorPage = ref(1)
const selectorTotalPages = ref(1)
const selectorPageSize = ref(12)

const directUploadInput = ref(null)
const modalUploadInput = ref(null)
const modalUploading = ref(false)

// Fetching initial data
const fetchData = async () => {
  loading.value = true
  error.value = null
  try {
    const [toursRes, destsRes] = await Promise.all([
      api.get('/api/admin/virtual-tours'),
      api.get('/api/admin/destinations', { params: { pageSize: 100 } })
    ])
    
    if (toursRes.success) {
      tours.value = toursRes.data
    }
    if (destsRes.success) {
      destinations.value = destsRes.data.items
    }
  } catch (err) {
    error.value = err.message || 'Không thể tải dữ liệu tour ảo.'
  } finally {
    loading.value = false
  }
}

// Compute filtered tours on client side
const filteredTours = computed(() => {
  return tours.value.filter(t => {
    const keyword = searchKeyword.value.toLowerCase().trim()
    const matchesKeyword = !keyword || 
      t.title.toLowerCase().includes(keyword) || 
      t.destinationName.toLowerCase().includes(keyword)
    
    const matchesStatus = !selectedStatus.value || t.status === selectedStatus.value
    
    return matchesKeyword && matchesStatus
  })
})

// Compute available destinations for new tours (destinations that do not have virtual tours yet)
const availableDestinations = computed(() => {
  if (isEditMode.value) {
    // In edit mode, include all destinations so we don't block the selected one
    return destinations.value
  }
  return destinations.value.filter(d => !d.hasVirtualTour)
})

const resetFilters = () => {
  searchKeyword.value = ''
  selectedStatus.value = null
}

const openCreateModal = () => {
  isEditMode.value = false
  modalError.value = null
  modalForm.value = {
    destinationId: null,
    title: '',
    description: '',
    thumbnailId: null,
    thumbnailUrl: null,
    status: 'draft'
  }
  showFormModal.value = true
}

const openEditModal = (tour) => {
  isEditMode.value = true
  modalError.value = null
  editingTourId.value = tour.id
  modalForm.value = {
    destinationId: tour.destinationId,
    title: tour.title,
    description: tour.description || '',
    thumbnailId: tour.thumbnailId,
    thumbnailUrl: tour.thumbnailUrl,
    status: tour.status
  }
  showFormModal.value = true
}

const closeFormModal = () => {
  showFormModal.value = false
}

const submitForm = async () => {
  if (!modalForm.value.destinationId) {
    modalError.value = 'Vui lòng chọn địa điểm du lịch.'
    return
  }
  if (!modalForm.value.title.trim()) {
    modalError.value = 'Vui lòng nhập tiêu đề tour ảo.'
    return
  }

  submitting.value = true
  modalError.value = null
  try {
    let response
    if (isEditMode.value) {
      response = await api.put(`/api/admin/virtual-tours/${editingTourId.value}`, {
        title: modalForm.value.title,
        description: modalForm.value.description,
        thumbnailId: modalForm.value.thumbnailId,
        status: modalForm.value.status
      })
    } else {
      response = await api.post(`/api/admin/destinations/${modalForm.value.destinationId}/virtual-tours`, {
        title: modalForm.value.title,
        description: modalForm.value.description,
        thumbnailId: modalForm.value.thumbnailId
      })
    }

    if (response.success) {
      toastStore.success(isEditMode.value ? 'Cập nhật tour ảo thành công!' : 'Tạo tour ảo mới thành công!')
      showFormModal.value = false
      await fetchData() // reload the page data
    }
  } catch (err) {
    modalError.value = err.message || 'Lỗi xảy ra khi lưu thông tin.'
  } finally {
    submitting.value = false
  }
}

const confirmDelete = async (tour) => {
  const confirmResult = await confirmStore.show({
    title: 'Xóa tour ảo',
    message: `Bạn có chắc muốn xóa tour ảo "${tour.title}"? Tất cả cảnh (Panoramas) liên quan cũng sẽ bị xóa.`
  })
  if (!confirmResult) return

  try {
    const response = await api.delete(`/api/admin/virtual-tours/${tour.id}`)
    if (response.success) {
      toastStore.success('Xóa tour ảo thành công.')
      await fetchData()
    }
  } catch (err) {
    toastStore.error(err.message || 'Không thể xóa tour ảo.')
  }
}

// Media Selector methods
const openMediaSelector = async () => {
  selectorPage.value = 1
  showMediaSelector.value = true
  await fetchSelectorImages()
}

const closeMediaSelector = () => {
  showMediaSelector.value = false
}

const fetchSelectorImages = async () => {
  selectorLoading.value = true
  try {
    const response = await api.get('/api/admin/media', {
      params: {
        mediaType: 'image',
        page: selectorPage.value,
        pageSize: selectorPageSize.value
      }
    })
    if (response.success) {
      selectorImages.value = response.data.items
      selectorPage.value = response.data.page
      selectorTotalPages.value = response.data.totalPages
    }
  } catch (err) {
    console.error('Không thể tải thư viện ảnh.', err)
  } finally {
    selectorLoading.value = false
  }
}

const selectImage = (img) => {
  modalForm.value.thumbnailId = img.id
  modalForm.value.thumbnailUrl = img.fileUrl
  closeMediaSelector()
}

const removeThumbnail = () => {
  modalForm.value.thumbnailId = null
  modalForm.value.thumbnailUrl = null
}

const triggerDirectUpload = () => {
  directUploadInput.value.click()
}

const triggerModalUpload = () => {
  modalUploadInput.value.click()
}

const handleDirectUpload = async (e) => {
  const file = e.target.files?.[0]
  if (!file) return
  
  const formData = new FormData()
  formData.append('file', file)
  formData.append('mediaType', 'image')

  submitting.value = true
  try {
    const response = await api.post('/api/admin/media/upload', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })
    if (response.success) {
      modalForm.value.thumbnailId = response.data.id
      modalForm.value.thumbnailUrl = response.data.fileUrl
      toastStore.success('Tải ảnh đại diện lên thành công.')
    }
  } catch (err) {
    toastStore.error(err.message || 'Lỗi khi tải ảnh lên.')
  } finally {
    submitting.value = false
    e.target.value = ''
  }
}

const handleModalUpload = async (e) => {
  const file = e.target.files?.[0]
  if (!file) return
  
  const formData = new FormData()
  formData.append('file', file)
  formData.append('mediaType', 'image')

  modalUploading.value = true
  try {
    const response = await api.post('/api/admin/media/upload', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })
    if (response.success) {
      selectorPage.value = 1
      await fetchSelectorImages()
      toastStore.success('Tải ảnh lên thành công.')
    }
  } catch (err) {
    toastStore.error(err.message || 'Lỗi khi tải ảnh lên.')
  } finally {
    modalUploading.value = false
    e.target.value = ''
  }
}

const selectorPrevPage = async () => {
  if (selectorPage.value > 1) {
    selectorPage.value--
    await fetchSelectorImages()
  }
}

const selectorNextPage = async () => {
  if (selectorPage.value < selectorTotalPages.value) {
    selectorPage.value++
    await fetchSelectorImages()
  }
}

onMounted(() => {
  fetchData()
})
</script>
