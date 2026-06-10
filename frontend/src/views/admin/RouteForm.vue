<template>
  <div class="max-w-4xl mx-auto space-y-8 font-sans">
    <!-- Back Header -->
    <div class="flex items-center">
      <router-link to="/admin/routes" class="inline-flex items-center space-x-2 text-slate-500 hover:text-teal-600 font-bold text-xs transition">
        <ArrowLeftIcon class="w-4 h-4" />
        <span>Quay lại danh sách tuyến</span>
      </router-link>
    </div>

    <!-- Header info -->
    <div class="space-y-1">
      <h2 class="text-xl md:text-2xl font-black text-slate-800 tracking-tight">
        {{ isEditMode ? 'Chỉnh sửa tuyến tham quan' : 'Tạo tuyến tham quan mới' }}
      </h2>
      <p class="text-xs md:text-sm text-slate-400 font-medium">Thiết lập các thông tin cơ bản, khoảng cách, thời lượng và ảnh đại diện cho tuyến.</p>
    </div>

    <!-- Error alert -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-xl flex items-center space-x-3 shadow-sm">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0 text-red-500" />
      <span class="text-sm font-bold">{{ error }}</span>
    </div>

    <form @submit.prevent="handleSubmit" class="grid grid-cols-1 md:grid-cols-3 gap-6 items-start">
      <!-- Left side: Basic Details (2 Columns) -->
      <div class="md:col-span-2 space-y-6">
        <div class="bg-white p-6 md:p-8 rounded-2xl border border-slate-200/80 shadow-sm space-y-6">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-3">Thông tin cơ bản</h3>
          
          <!-- Title -->
          <div class="space-y-2">
            <label class="block text-xs font-bold text-slate-500 uppercase tracking-wider">Tên tuyến *</label>
            <input 
              v-model="form.title"
              type="text" 
              placeholder="Ví dụ: Tuyến khám phá sông nước nửa ngày" 
              required
              class="w-full px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm focus:outline-none focus:ring-4 focus:ring-teal-500/5 transition font-semibold"
              @input="handleTitleInput"
            />
          </div>

          <!-- Slug -->
          <div class="space-y-2">
            <label class="block text-xs font-bold text-slate-500 uppercase tracking-wider">Slug URL *</label>
            <input 
              v-model="form.slug"
              type="text" 
              placeholder="ví-dụ-tuyen-kham-pha-song-nuoc" 
              required
              class="w-full px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm focus:outline-none focus:ring-4 focus:ring-teal-500/5 transition font-mono font-semibold"
            />
          </div>

          <!-- Description -->
          <div class="space-y-2">
            <label class="block text-xs font-bold text-slate-500 uppercase tracking-wider">Mô tả tuyến</label>
            <textarea 
              v-model="form.description"
              rows="5"
              placeholder="Mô tả tóm tắt về hành trình du lịch này để du khách nắm bắt..."
              class="w-full px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm focus:outline-none focus:ring-4 focus:ring-teal-500/5 transition font-medium"
            ></textarea>
          </div>

          <div class="grid grid-cols-2 gap-4">
            <!-- Estimated Duration -->
            <div class="space-y-2">
              <label class="block text-xs font-bold text-slate-500 uppercase tracking-wider">Thời lượng dự kiến</label>
              <input 
                v-model="form.estimatedDuration"
                type="text" 
                placeholder="Ví dụ: Nửa ngày, 1 ngày"
                class="w-full px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm focus:outline-none focus:ring-4 focus:ring-teal-500/5 transition font-semibold"
              />
            </div>

            <!-- Distance Km -->
            <div class="space-y-2">
              <label class="block text-xs font-bold text-slate-500 uppercase tracking-wider">Quãng đường (km)</label>
              <input 
                v-model.number="form.distanceKm"
                type="number" 
                step="0.1"
                min="0"
                placeholder="Ví dụ: 12.5"
                class="w-full px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm focus:outline-none focus:ring-4 focus:ring-teal-500/5 transition font-semibold"
              />
            </div>
          </div>

          <div class="grid grid-cols-2 gap-4">
            <!-- RegionId Selection -->
            <div class="space-y-2">
              <label class="block text-xs font-bold text-slate-500 uppercase tracking-wider">Khu vực *</label>
              <select 
                v-model="form.regionId"
                required
                class="w-full px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm focus:outline-none focus:ring-4 focus:ring-teal-500/5 transition font-bold"
              >
                <option :value="null" disabled>Chọn khu vực</option>
                <option v-for="r in regions" :key="r.id" :value="r.id">{{ r.name }}</option>
              </select>
            </div>

            <!-- Theme Selection -->
            <div class="space-y-2">
              <label class="block text-xs font-bold text-slate-500 uppercase tracking-wider">Chủ đề *</label>
              <select 
                v-model="form.theme"
                required
                class="w-full px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm focus:outline-none focus:ring-4 focus:ring-teal-500/5 transition font-bold"
              >
                <option :value="null" disabled>Chọn chủ đề</option>
                <option value="culture">Văn hóa - Lịch sử</option>
                <option value="eco">Sinh thái sông nước</option>
                <option value="food">Ẩm thực địa phương</option>
                <option value="craft">Làng nghề</option>
              </select>
            </div>
          </div>
        </div>
      </div>

      <!-- Right side: Publishing config & Thumbnail Cover (1 Column) -->
      <div class="space-y-6">
        <!-- Thumbnail select area -->
        <div class="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-4">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-3">Ảnh đại diện tuyến</h3>
          
          <div class="relative w-full aspect-[16/10] rounded-xl overflow-hidden border border-slate-200 bg-slate-50 flex flex-col items-center justify-center group">
            <img 
              v-if="form.thumbnailUrl" 
              :src="form.thumbnailUrl" 
              class="w-full h-full object-cover" 
            />
            <div v-else class="text-center p-6 space-y-2 text-slate-400">
              <ImageIcon class="w-8 h-8 mx-auto" />
              <p class="text-[10px] font-bold uppercase tracking-wider">Chưa chọn ảnh</p>
            </div>

            <!-- Hover actions -->
            <div 
              v-if="form.thumbnailUrl" 
              class="absolute inset-0 bg-slate-900/60 opacity-0 group-hover:opacity-100 transition flex items-center justify-center space-x-2"
            >
              <button 
                type="button"
                @click="openMediaSelector"
                class="px-3 py-1.5 bg-white text-slate-900 rounded-lg text-xs font-bold hover:bg-slate-50 transition"
              >
                Thay đổi
              </button>
              <button 
                type="button"
                @click="removeCoverImage"
                class="px-3 py-1.5 bg-red-600 text-white rounded-lg text-xs font-bold hover:bg-red-700 transition"
              >
                Xóa
              </button>
            </div>
          </div>

          <!-- Buttons when no image selected -->
          <div v-if="!form.thumbnailUrl" class="grid grid-cols-2 gap-3">
            <button
              type="button"
              @click="openMediaSelector"
              class="py-2.5 px-3 border border-slate-200 hover:bg-slate-50 text-slate-700 font-semibold rounded-xl text-xs transition flex items-center justify-center space-x-1"
            >
              <ImageIcon class="w-3.5 h-3.5" />
              <span>Thư viện</span>
            </button>
            <button
              type="button"
              @click="triggerDirectUpload"
              class="py-2.5 px-3 bg-slate-900 hover:bg-slate-800 text-white font-semibold rounded-xl text-xs transition flex items-center justify-center space-x-1"
            >
              <UploadIcon class="w-3.5 h-3.5" />
              <span>Tải ảnh</span>
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
        <div class="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-4">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-3">Xuất bản</h3>
          
          <div class="space-y-3">
            <label class="flex items-center space-x-2 cursor-pointer">
              <input type="radio" v-model="form.status" value="draft" class="text-teal-600 focus:ring-teal-500" />
              <span class="text-sm font-medium text-slate-700">Lưu nháp (Draft)</span>
            </label>
            <label class="flex items-center space-x-2 cursor-pointer">
              <input type="radio" v-model="form.status" value="published" class="text-teal-600 focus:ring-teal-500" />
              <span class="text-sm font-medium text-slate-700">Xuất bản (Published)</span>
            </label>
          </div>
        </div>

        <!-- Submit buttons -->
        <button 
          type="submit" 
          class="w-full py-3 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold rounded-xl shadow-lg active:scale-[0.98] transition flex items-center justify-center space-x-2 disabled:opacity-50"
          :disabled="submitting"
        >
          <Loader2Icon v-if="submitting" class="w-4 h-4 animate-spin" />
          <span>{{ isEditMode ? 'Cập nhật tuyến du lịch' : 'Tạo tuyến du lịch mới' }}</span>
        </button>
      </div>
    </form>

    <!-- Modal Media Selector Dialog -->
    <div 
      v-if="showMediaModal" 
      class="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/60 backdrop-blur-sm p-4"
    >
      <div class="bg-white w-full max-w-4xl rounded-3xl overflow-hidden shadow-2xl flex flex-col max-h-[85vh] border border-slate-200">
        <!-- Header -->
        <div class="p-6 border-b border-slate-100 flex justify-between items-center bg-slate-50">
          <div class="space-y-1">
            <h3 class="text-base font-black text-slate-800">Chọn ảnh đại diện từ thư viện</h3>
            <p class="text-xs text-slate-400 font-medium">Chọn một tập tin hình ảnh có sẵn trong thư viện media.</p>
          </div>
          <button @click="showMediaModal = false" class="p-2 text-slate-400 hover:bg-slate-100 hover:text-slate-700 rounded-xl transition">
            <XIcon class="w-5 h-5" />
          </button>
        </div>

        <!-- Search / Filter -->
        <div class="p-6 border-b border-slate-100 flex items-center justify-between">
          <p class="text-xs font-bold text-slate-400 uppercase tracking-wider">Danh sách hình ảnh</p>
        </div>

        <!-- Grid Images -->
        <div class="p-6 overflow-y-auto flex-grow min-h-[300px]">
          <div v-if="selectorLoading" class="grid grid-cols-3 sm:grid-cols-5 gap-4 animate-pulse">
            <div v-for="i in 10" :key="i" class="aspect-square bg-slate-100 rounded-xl"></div>
          </div>
          <div v-else-if="selectorImages.length === 0" class="text-center py-12 space-y-3">
            <ImageIcon class="w-8 h-8 mx-auto text-slate-300" />
            <p class="text-sm font-medium text-slate-500">Thư viện ảnh trống.</p>
          </div>
          <div v-else class="grid grid-cols-3 sm:grid-cols-5 gap-4">
            <div 
              v-for="img in selectorImages" 
              :key="img.id"
              :class="[
                'aspect-square rounded-xl overflow-hidden cursor-pointer border-2 transition relative group bg-slate-50',
                form.thumbnailId === img.id ? 'border-teal-500 shadow-md ring-2 ring-teal-500/20' : 'border-slate-100 hover:border-slate-300'
              ]"
              @click="selectImage(img)"
            >
              <img :src="img.fileUrl" class="w-full h-full object-cover" />
              <!-- Hover visual checkmark -->
              <div 
                v-if="form.thumbnailId === img.id"
                class="absolute inset-0 bg-teal-500/10 flex items-center justify-center"
              >
                <div class="p-1 bg-teal-500 text-slate-950 rounded-full shadow-md">
                  <CheckIcon class="w-4 h-4" />
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Pagination Footer -->
        <div 
          v-if="selectorImages.length > 0 && selectorTotalPages > 1"
          class="p-4 bg-slate-50 border-t border-slate-100 flex justify-between items-center text-xs"
        >
          <span class="text-slate-500 font-medium">Trang {{ selectorPage }} / {{ selectorTotalPages }}</span>
          <div class="flex items-center space-x-2">
            <button 
              type="button"
              @click="selectorPrevPage"
              :disabled="selectorPage === 1"
              class="px-2.5 py-1 bg-white border border-slate-200 rounded-md hover:bg-slate-50 transition disabled:opacity-40"
            >
              Trước
            </button>
            <button 
              type="button"
              @click="selectorNextPage"
              :disabled="selectorPage === selectorTotalPages"
              class="px-2.5 py-1 bg-white border border-slate-200 rounded-md hover:bg-slate-50 transition disabled:opacity-40"
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
import { ref, reactive, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import api from '../../utils/api'
import { 
  ArrowLeft as ArrowLeftIcon, 
  AlertCircle as AlertCircleIcon,
  Image as ImageIcon, 
  Upload as UploadIcon,
  Loader2 as Loader2Icon,
  Check as CheckIcon,
  X as XIcon
} from 'lucide-vue-next'

const router = useRouter()
const route = useRoute()
const props = defineProps({
  id: {
    type: String,
    default: null
  }
})

const isEditMode = ref(!!props.id || !!route.params.id)
const routeId = props.id || route.params.id

const submitting = ref(false)
const loading = ref(false)
const error = ref(null)
const regions = ref([])

const form = reactive({
  regionId: null,
  title: '',
  slug: '',
  description: '',
  estimatedDuration: '',
  distanceKm: null,
  theme: null,
  thumbnailId: null,
  thumbnailUrl: '',
  status: 'draft'
})

// Media selector variables
const showMediaModal = ref(false)
const selectorLoading = ref(false)
const selectorImages = ref([])
const selectorPage = ref(1)
const selectorTotalPages = ref(1)

const slugify = (text) => {
  return text
    .toString()
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '') // Remove accents
    .replace(/[đĐ]/g, 'd')
    .replace(/[^a-z0-9 -]/g, '') // Remove invalid chars
    .replace(/\s+/g, '-') // Replace spaces with -
    .replace(/-+/g, '-') // Collapse dashes
    .trim()
}

const handleTitleInput = () => {
  if (!isEditMode.value) {
    form.slug = slugify(form.title)
  }
}

// Fetch basic parameters
const fetchParams = async () => {
  try {
    const res = await api.get('/api/admin/regions')
    if (res.success) {
      regions.value = res.data
    }
  } catch (err) {
    console.error('Error fetching regions:', err)
  }
}

// Fetch route for edit mode
const fetchRoute = async () => {
  if (!isEditMode.value) return
  loading.value = true
  try {
    const res = await api.get(`/api/admin/routes/${routeId}`)
    if (res.success) {
      Object.assign(form, res.data)
    }
  } catch (err) {
    error.value = err.message || 'Không thể tải chi tiết tuyến tham quan.'
  } finally {
    loading.value = false
  }
}

// Submit action
const handleSubmit = async () => {
  submitting.value = true
  error.value = null
  try {
    const payload = { ...form }
    
    let res
    if (isEditMode.value) {
      res = await api.put(`/api/admin/routes/${routeId}`, payload)
    } else {
      res = await api.post('/api/admin/routes', payload)
    }

    if (res.success) {
      router.push('/admin/routes')
    }
  } catch (err) {
    error.value = err.message || 'Lỗi lưu thông tin tuyến.'
  } finally {
    submitting.value = false
  }
}

// Media selectors functions
const openMediaSelector = async () => {
  showMediaModal.value = true
  fetchSelectorImages()
}

const fetchSelectorImages = async () => {
  selectorLoading.value = true
  try {
    const res = await api.get('/api/admin/media', {
      params: {
        mediaType: 'image',
        page: selectorPage.value,
        pageSize: 15
      }
    })
    if (res.success) {
      selectorImages.value = res.data.items
      selectorTotalPages.value = res.data.totalPages
    }
  } catch (err) {
    console.error('Error loading selector images:', err)
  } finally {
    selectorLoading.value = false
  }
}

const selectImage = (img) => {
  form.thumbnailId = img.id
  form.thumbnailUrl = img.fileUrl
  showMediaModal.value = false
}

const removeCoverImage = () => {
  form.thumbnailId = null
  form.thumbnailUrl = ''
}

// Direct upload
const directUploadInput = ref(null)
const triggerDirectUpload = () => {
  directUploadInput.value?.click()
}

const handleDirectUpload = async (event) => {
  const file = event.target.files[0]
  if (!file) return

  const formData = new FormData()
  formData.append('file', file)
  formData.append('mediaType', 'image')
  formData.append('caption', `Ảnh bìa tuyến du lịch ${form.title || ''}`)

  submitting.value = true
  try {
    const res = await api.post('/api/admin/media/upload', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })
    if (res.success) {
      form.thumbnailId = res.data.id
      form.thumbnailUrl = res.data.fileUrl
    }
  } catch (err) {
    alert(err.message || 'Lỗi tải ảnh lên.')
  } finally {
    submitting.value = false
  }
}

const selectorPrevPage = () => {
  if (selectorPage.value > 1) {
    selectorPage.value--
    fetchSelectorImages()
  }
}

const selectorNextPage = () => {
  if (selectorPage.value < selectorTotalPages.value) {
    selectorPage.value++
    fetchSelectorImages()
  }
}

onMounted(() => {
  fetchParams()
  fetchRoute()
})
</script>
