<template>
  <div class="space-y-6">
    <!-- Header Page Actions -->
    <div class="flex justify-between items-center bg-white p-6 rounded-2xl border border-slate-200/60 shadow-sm">
      <div class="space-y-1">
        <h2 class="text-lg font-bold text-slate-800">
          {{ isEditMode ? 'Chỉnh sửa địa điểm' : 'Thêm địa điểm mới' }}
        </h2>
        <p class="text-xs text-slate-500">
          {{ isEditMode ? 'Cập nhật lại các thông tin của địa điểm du lịch.' : 'Tạo mới một địa điểm và thiết lập định vị địa lý.' }}
        </p>
      </div>
      <button 
        @click="goBack"
        class="px-4 py-2 border border-slate-200 hover:bg-slate-50 text-slate-600 font-semibold rounded-xl text-sm transition"
      >
        Quay lại
      </button>
    </div>

    <!-- Error Summary Alert -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 p-4 rounded-xl flex items-start space-x-3">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0 mt-0.5" />
      <div class="text-sm">
        <p class="font-bold mb-1">{{ error }}</p>
        <ul v-if="validationErrors.length > 0" class="list-disc list-inside space-y-1 text-xs text-red-600">
          <li v-for="(err, idx) in validationErrors" :key="idx">
            <span class="font-semibold capitalize">{{ err.field }}:</span> {{ err.message }}
          </li>
        </ul>
      </div>
    </div>

    <!-- Split Layout Form -->
    <form @submit.prevent="submitForm" class="grid grid-cols-1 lg:grid-cols-12 gap-6 items-start">
      <!-- Left side: Form Input Fields (7 columns) -->
      <div class="lg:col-span-7 bg-white p-6 md:p-8 rounded-2xl border border-slate-200/60 shadow-sm space-y-6">
        <h3 class="text-sm font-bold text-slate-800 uppercase tracking-wider border-b border-slate-100 pb-3">Thông tin chi tiết</h3>
        
        <!-- Name & Slug -->
        <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Tên địa điểm *</label>
            <input 
              v-model="form.name"
              type="text" 
              required
              placeholder="Ví dụ: Chùa Ông Cần Thơ" 
              class="w-full px-4 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition"
            />
          </div>
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Slug định danh *</label>
            <input 
              v-model="form.slug"
              type="text" 
              required
              placeholder="vi-du-chua-ong-can-tho" 
              class="w-full px-4 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition font-mono"
            />
          </div>
        </div>

        <!-- Region & Category -->
        <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Khu vực du lịch *</label>
            <select 
              v-model="form.regionId"
              required
              class="w-full px-3 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition"
              @change="handleRegionChange"
            >
              <option :value="null" disabled>Chọn Khu vực</option>
              <option v-for="reg in regions" :key="reg.id" :value="reg.id">
                {{ reg.name }} ({{ reg.province }})
              </option>
            </select>
          </div>
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Danh mục địa điểm *</label>
            <select 
              v-model="form.categoryId"
              required
              class="w-full px-3 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition"
            >
              <option :value="null" disabled>Chọn Danh mục</option>
              <option v-for="cat in categories" :key="cat.id" :value="cat.id">
                {{ cat.name }}
              </option>
            </select>
          </div>
        </div>

        <!-- Descriptions -->
        <div class="space-y-4">
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Mô tả ngắn * (Tối đa 500 ký tự)</label>
            <textarea 
              v-model="form.shortDescription"
              required
              rows="3"
              maxlength="500"
              placeholder="Tóm tắt ngắn gọn nét đặc trưng của địa điểm..." 
              class="w-full px-4 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition resize-none"
            ></textarea>
          </div>
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Mô tả đầy đủ</label>
            <textarea 
              v-model="form.fullDescription"
              rows="6"
              placeholder="Giới thiệu chi tiết về lịch sử, kiến trúc, sự tích, hoặc hành trình trải nghiệm tại đây..." 
              class="w-full px-4 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition resize-y"
            ></textarea>
          </div>
        </div>

        <!-- Address & Contact Info -->
        <div class="space-y-4 border-t border-slate-100 pt-6">
          <h4 class="text-sm font-semibold text-slate-700">Thông tin liên hệ & địa chỉ</h4>
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Địa chỉ</label>
            <input 
              v-model="form.address"
              type="text" 
              placeholder="Số 123 Hai Bà Trưng, Ninh Kiều, Cần Thơ" 
              class="w-full px-4 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition"
            />
          </div>
          <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Điện thoại liên hệ</label>
              <input 
                v-model="form.contactPhone"
                type="text" 
                placeholder="0292xxxxxx" 
                class="w-full px-4 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition"
              />
            </div>
            <div>
              <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Email liên hệ</label>
              <input 
                v-model="form.contactEmail"
                type="email" 
                placeholder="info@dia-diem.com" 
                class="w-full px-4 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition"
              />
            </div>
          </div>
          <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Website URL</label>
              <input 
                v-model="form.websiteUrl"
                type="url" 
                placeholder="https://dia-diem.com" 
                class="w-full px-4 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition font-mono text-xs"
              />
            </div>
            <div>
              <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Facebook URL</label>
              <input 
                v-model="form.facebookUrl"
                type="url" 
                placeholder="https://facebook.com/dia-diem" 
                class="w-full px-4 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition font-mono text-xs"
              />
            </div>
          </div>
          <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div>
              <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Giờ mở cửa</label>
              <input 
                v-model="form.openingHours"
                type="text" 
                placeholder="Ví dụ: 07:00 - 18:00 hàng ngày" 
                class="w-full px-4 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition"
              />
            </div>
            <div>
              <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Giá vé tham khảo</label>
              <input 
                v-model="form.ticketPrice"
                type="text" 
                placeholder="Ví dụ: Miễn phí hoặc 50.000đ/vé" 
                class="w-full px-4 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition"
              />
            </div>
          </div>
        </div>
      </div>

      <!-- Right side: Geographic Coordinate Bindings & Map Picker (5 columns) -->
      <div class="lg:col-span-5 space-y-6">
        <!-- Coordinate Card Input fields -->
        <div class="bg-white p-6 rounded-2xl border border-slate-200/60 shadow-sm space-y-4">
          <h3 class="text-sm font-bold text-slate-800 uppercase tracking-wider border-b border-slate-100 pb-3">Tọa độ địa lý</h3>
          
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Vĩ độ (Latitude) *</label>
              <input 
                v-model.number="form.latitude"
                type="number" 
                step="0.0000001"
                min="-90"
                max="90"
                required
                class="w-full px-3 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition font-mono"
              />
            </div>
            <div>
              <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Kinh độ (Longitude) *</label>
              <input 
                v-model.number="form.longitude"
                type="number" 
                step="0.0000001"
                min="-180"
                max="180"
                required
                class="w-full px-3 py-2.5 bg-slate-50/50 border border-slate-200 focus:border-teal-500/85 focus:bg-white rounded-xl text-sm focus:outline-none transition font-mono"
              />
            </div>
          </div>
        </div>

        <!-- Embedded Interactive Leaflet Map picker -->
        <div class="h-[400px]">
          <MapLocationPicker 
            v-model:latitude="form.latitude"
            v-model:longitude="form.longitude"
            :regionCenter="activeRegionCenter"
          />
        </div>

        <!-- Cover Image Card -->
        <div class="bg-white p-6 rounded-2xl border border-slate-200/60 shadow-sm space-y-4">
          <h3 class="text-sm font-bold text-slate-800 uppercase tracking-wider border-b border-slate-100 pb-3">Ảnh đại diện (Cover Image)</h3>
          
          <!-- Image Preview Area -->
          <div class="relative w-full aspect-[16/10] rounded-xl overflow-hidden border border-slate-200 bg-slate-50 flex flex-col items-center justify-center group">
            <img 
              v-if="form.coverImageUrl" 
              :src="form.coverImageUrl" 
              class="w-full h-full object-cover" 
            />
            <div v-else class="text-center p-6 space-y-2 text-slate-400">
              <ImageIcon class="w-8 h-8 mx-auto" />
              <p class="text-xs">Chưa chọn ảnh đại diện</p>
            </div>
            
            <!-- Hover actions for existing image -->
            <div 
              v-if="form.coverImageUrl" 
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

          <!-- Buttons when no image is selected -->
          <div v-if="!form.coverImageUrl" class="grid grid-cols-2 gap-3">
            <button
              type="button"
              @click="openMediaSelector"
              class="py-2.5 px-3 border border-slate-200 hover:bg-slate-50 text-slate-700 font-semibold rounded-xl text-xs transition flex items-center justify-center space-x-1"
            >
              <ImageIcon class="w-3.5 h-3.5" />
              <span>Chọn từ thư viện</span>
            </button>
            
            <button
              type="button"
              @click="triggerDirectUpload"
              class="py-2.5 px-3 bg-slate-900 hover:bg-slate-800 text-white font-semibold rounded-xl text-xs transition flex items-center justify-center space-x-1"
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

        <!-- Publishing configurations -->
        <div class="bg-white p-6 rounded-2xl border border-slate-200/60 shadow-sm space-y-4">
          <h3 class="text-sm font-bold text-slate-800 uppercase tracking-wider border-b border-slate-100 pb-3">Cấu hình hiển thị</h3>
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Trạng thái xuất bản</label>
            <div class="flex items-center space-x-6">
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
        </div>

        <!-- Submit actions buttons -->
        <div class="flex items-center space-x-3">
          <button 
            type="submit" 
            class="flex-grow py-3 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold rounded-xl shadow-lg hover:shadow-teal-500/10 active:scale-[0.98] transition duration-150 flex items-center justify-center space-x-2 disabled:opacity-50"
            :disabled="submitting"
          >
            <Loader2Icon v-if="submitting" class="w-4 h-4 animate-spin" />
            <span>{{ isEditMode ? 'Lưu cập nhật' : 'Tạo địa điểm' }}</span>
          </button>
          <button 
            type="button"
            @click="goBack"
            class="px-5 py-3 border border-slate-200 hover:bg-slate-50 text-slate-600 font-semibold rounded-xl text-sm transition"
            :disabled="submitting"
          >
            Hủy bỏ
          </button>
        </div>
      </div>
    </form>
    <!-- Media Selector Popup Modal -->
    <div 
      v-if="showMediaSelector" 
      class="fixed inset-0 z-50 overflow-y-auto bg-slate-900/60 backdrop-blur-sm flex items-center justify-center p-4"
      @click.self="closeMediaSelector"
    >
      <div class="bg-white rounded-2xl w-full max-w-4xl shadow-2xl border border-slate-100 overflow-hidden flex flex-col max-h-[85vh]">
        <!-- Header -->
        <div class="p-6 border-b border-slate-100 flex justify-between items-center">
          <div class="space-y-1">
            <h3 class="text-base font-bold text-slate-800">Chọn ảnh từ thư viện</h3>
            <p class="text-xs text-slate-500 font-medium">Chọn một hình ảnh có sẵn trong thư viện media hoặc tải lên ảnh mới.</p>
          </div>
          <button 
            type="button"
            @click="closeMediaSelector"
            class="p-1.5 hover:bg-slate-100 rounded-lg text-slate-400 hover:text-slate-600 transition"
          >
            <XIcon class="w-5 h-5" />
          </button>
        </div>

        <!-- Quick Upload Section inside Selector -->
        <div class="px-6 py-4 bg-slate-50 border-b border-slate-100 flex items-center justify-between">
          <span class="text-xs text-slate-500 font-medium">Bạn có thể tải trực tiếp ảnh mới lên thư viện:</span>
          <button 
            type="button"
            @click="triggerModalUpload"
            class="px-3 py-1.5 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold rounded-lg text-xs transition flex items-center space-x-1"
            :disabled="modalUploading"
          >
            <Loader2Icon v-if="modalUploading" class="w-3 h-3 animate-spin" />
            <UploadIcon v-else class="w-3 h-3" />
            <span>{{ modalUploading ? 'Đang tải lên...' : 'Tải ảnh lên' }}</span>
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
        <div class="p-6 overflow-y-auto flex-1 min-h-[300px]">
          <!-- Loading state -->
          <div v-if="selectorLoading" class="grid grid-cols-3 sm:grid-cols-5 gap-4 animate-pulse">
            <div v-for="i in 10" :key="i" class="aspect-square bg-slate-100 rounded-xl"></div>
          </div>

          <!-- Empty state -->
          <div v-else-if="selectorImages.length === 0" class="text-center py-12 space-y-3">
            <ImageIcon class="w-8 h-8 mx-auto text-slate-300" />
            <p class="text-sm font-medium text-slate-500">Thư viện ảnh trống.</p>
          </div>

          <!-- Grid of Images -->
          <div v-else class="grid grid-cols-3 sm:grid-cols-5 gap-4">
            <div 
              v-for="img in selectorImages" 
              :key="img.id"
              :class="[
                'aspect-square rounded-xl overflow-hidden cursor-pointer border-2 transition relative group bg-slate-50',
                form.coverImageId === img.id ? 'border-teal-500 shadow-md ring-2 ring-teal-500/20' : 'border-slate-100 hover:border-slate-300'
              ]"
              @click="selectImage(img)"
            >
              <img :src="img.fileUrl" class="w-full h-full object-cover" />
              <!-- Hover visual checkmark -->
              <div 
                v-if="form.coverImageId === img.id"
                class="absolute inset-0 bg-teal-500/10 flex items-center justify-center"
              >
                <div class="p-1 bg-teal-500 text-slate-950 rounded-full shadow-md">
                  <CheckIcon class="w-4 h-4" />
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
import { ref, onMounted, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import api from '../../utils/api'
import { useToastStore } from '../../stores/toast'
import MapLocationPicker from '../../components/admin/MapLocationPicker.vue'
import { 
  AlertCircle as AlertCircleIcon, 
  Loader2 as Loader2Icon,
  Image as ImageIcon,
  Upload as UploadIcon,
  Check as CheckIcon,
  X as XIcon
} from 'lucide-vue-next'

const toastStore = useToastStore()

const props = defineProps({
  id: {
    type: String,
    default: null
  }
})

const router = useRouter()
const route = useRoute()

const isEditMode = ref(!!props.id)
const submitting = ref(false)
const error = ref(null)
const validationErrors = ref([])

const regions = ref([])
const categories = ref([])
const activeRegionCenter = ref(null)

const form = ref({
  regionId: null,
  categoryId: null,
  name: '',
  slug: '',
  shortDescription: '',
  fullDescription: '',
  address: '',
  latitude: 10.03711, // Default coordinates Cần Thơ Demo Center
  longitude: 105.78825,
  coverImageId: null,
  coverImageUrl: null,
  openingHours: '',
  ticketPrice: '',
  contactPhone: '',
  contactEmail: '',
  websiteUrl: '',
  facebookUrl: '',
  status: 'draft'
})

// Slug generator for Vietnamese unicode strings
const slugify = (text) => {
  return text
    .toString()
    .toLowerCase()
    .normalize('NFD') // Split base and accent characters
    .replace(/[\u0300-\u036f]/g, '') // Strip accents
    .replace(/[đĐ]/g, 'd')
    .replace(/[^a-z0-9\s-]/g, '') // Remove symbols
    .trim()
    .replace(/\s+/g, '-') // Convert whitespace to hyphens
    .replace(/-+/g, '-') // Merge duplicate hyphens
}

// Auto generate slug from name in CREATE mode
watch(() => form.value.name, (newName) => {
  if (!isEditMode.value) {
    form.value.slug = slugify(newName)
  }
})

// Triggers centering when selected Region changes
const handleRegionChange = () => {
  const selected = regions.value.find(r => r.id === form.value.regionId)
  if (selected && selected.centerLatitude && selected.centerLongitude) {
    activeRegionCenter.value = {
      lat: selected.centerLatitude,
      lng: selected.centerLongitude,
      zoom: selected.defaultZoom || 13
    }
  }
}

const fetchFilterMetadata = async () => {
  try {
    const [regRes, catRes] = await Promise.all([
      api.get('/api/admin/regions'),
      api.get('/api/admin/categories')
    ])

    if (regRes.success) regions.value = regRes.data
    if (catRes.success) categories.value = catRes.data
  } catch (err) {
    error.value = 'Không thể tải siêu dữ liệu Danh mục hoặc Khu vực.'
  }
}

const fetchDestinationDetails = async () => {
  if (!props.id) return
  try {
    const response = await api.get(`/api/admin/destinations/${props.id}`)
    if (response.success) {
      const d = response.data
      form.value = {
        regionId: d.regionId,
        categoryId: d.category?.id || null,
        name: d.name,
        slug: d.slug,
        shortDescription: d.shortDescription,
        fullDescription: d.fullDescription || '',
        address: d.address || '',
        latitude: d.latitude,
        longitude: d.longitude,
        coverImageId: d.coverImageId || null,
        coverImageUrl: d.coverImageUrl || null,
        openingHours: d.openingHours || '',
        ticketPrice: d.ticketPrice || '',
        contactPhone: d.contactPhone || '',
        contactEmail: d.contactEmail || '',
        websiteUrl: d.websiteUrl || '',
        facebookUrl: d.facebookUrl || '',
        status: d.status
      }
    }
  } catch (err) {
    error.value = 'Không thể tải thông tin địa điểm chỉnh sửa.'
  }
}

const submitForm = async () => {
  submitting.value = true
  error.value = null
  validationErrors.value = []

  // Custom client-side validations
  if (!form.value.regionId) {
    error.value = 'Vui lòng chọn Khu vực du lịch.'
    submitting.value = false
    return
  }
  if (!form.value.categoryId) {
    error.value = 'Vui lòng chọn Danh mục địa điểm.'
    submitting.value = false
    return
  }
  if (form.value.latitude < -90 || form.value.latitude > 90) {
    error.value = 'Vĩ độ không hợp lệ (-90 đến 90).'
    submitting.value = false
    return
  }
  if (form.value.longitude < -180 || form.value.longitude > 180) {
    error.value = 'Kinh độ không hợp lệ (-180 đến 180).'
    submitting.value = false
    return
  }

  try {
    let response
    if (isEditMode.value) {
      response = await api.put(`/api/admin/destinations/${props.id}`, form.value)
    } else {
      response = await api.post('/api/admin/destinations', form.value)
    }

    if (response.success) {
      toastStore.success(isEditMode.value ? 'Cập nhật thông tin địa điểm thành công!' : 'Tạo địa điểm mới thành công!')
      router.push('/admin/destinations')
    }
  } catch (err) {
    error.value = err.message || 'Đã có lỗi xảy ra khi lưu thông tin.'
    if (err.errors) {
      validationErrors.value = err.errors
    }
  } finally {
    submitting.value = false
  }
}

const goBack = () => {
  router.push('/admin/destinations')
}

// Media Library Selector State & Upload Logic
const showMediaSelector = ref(false)
const selectorLoading = ref(false)
const selectorImages = ref([])
const selectorPage = ref(1)
const selectorTotalPages = ref(1)
const selectorPageSize = ref(15)

const directUploadInput = ref(null)
const modalUploadInput = ref(null)
const modalUploading = ref(false)

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
  form.value.coverImageId = img.id
  form.value.coverImageUrl = img.fileUrl
  closeMediaSelector()
}

const removeCoverImage = () => {
  form.value.coverImageId = null
  form.value.coverImageUrl = null
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
      form.value.coverImageId = response.data.id
      form.value.coverImageUrl = response.data.fileUrl
      toastStore.success('Tải lên ảnh bìa thành công.')
    }
  } catch (err) {
    toastStore.error(err.message || 'Lỗi khi tải ảnh đại diện lên.')
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
      toastStore.success('Tải lên ảnh thành công.')
    }
  } catch (err) {
    toastStore.error(err.message || 'Lỗi khi tải ảnh đại diện lên.')
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

onMounted(async () => {
  await fetchFilterMetadata()
  if (isEditMode.value) {
    await fetchDestinationDetails()
  }
})
</script>
