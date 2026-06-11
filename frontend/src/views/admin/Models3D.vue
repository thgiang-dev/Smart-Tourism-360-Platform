<template>
  <div class="space-y-6">
    <!-- Action Header -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 bg-white p-6 rounded-2xl border border-slate-200/85 shadow-sm">
      <div>
        <h2 class="text-base font-black text-slate-800 uppercase tracking-wide">Danh sách Mô hình 3D</h2>
        <p class="text-xs text-slate-450 font-medium">Quản lý và thiết lập liên kết mô hình 3D cho các địa điểm du lịch.</p>
      </div>
      <router-link 
        to="/admin/models-3d/new"
        class="premium-btn inline-flex items-center space-x-2 px-5 py-3 bg-teal-700 hover:bg-teal-850 text-white font-extrabold rounded-xl text-xs shadow-md transition duration-150"
      >
        <PlusIcon class="w-4 h-4 stroke-[2.5]" />
        <span>Thêm mô hình mới</span>
      </router-link>
    </div>

    <!-- Filters Bar -->
    <div class="bg-white p-5 rounded-2xl border border-slate-200/80 shadow-sm flex flex-col md:flex-row gap-4">
      <div class="flex-1 relative">
        <SearchIcon class="w-4 h-4 absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400" />
        <input 
          v-model="filters.keyword"
          type="text"
          placeholder="Tìm theo tên mô hình..."
          class="w-full pl-10 pr-4 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/5 rounded-xl text-xs focus:outline-none transition font-medium"
          @input="debounceSearch"
        />
      </div>

      <div class="flex gap-4">
        <!-- Status Filter -->
        <select 
          v-model="filters.status"
          class="px-3.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white rounded-xl text-xs focus:outline-none transition font-semibold cursor-pointer appearance-none min-w-[140px]"
          @change="fetchModels"
        >
          <option value="">Tất cả trạng thái</option>
          <option value="draft">Bản nháp</option>
          <option value="published">Đã xuất bản</option>
          <option value="archived">Lưu trữ</option>
        </select>

        <!-- Target Destination Filter -->
        <select 
          v-model="filters.targetId"
          class="px-3.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white rounded-xl text-xs focus:outline-none transition font-semibold cursor-pointer appearance-none min-w-[180px]"
          @change="fetchModels"
        >
          <option value="">Tất cả địa điểm</option>
          <option 
            v-for="d in destinations" 
            :key="d.id" 
            :value="d.id"
          >
            {{ d.name }}
          </option>
        </select>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 animate-pulse">
      <div v-for="i in 6" :key="i" class="bg-white border border-slate-200 rounded-2xl h-80"></div>
    </div>

    <!-- Empty State -->
    <div v-else-if="models.length === 0" class="bg-white rounded-2xl border border-slate-200/80 p-16 text-center shadow-sm space-y-4">
      <div class="inline-flex p-4.5 bg-slate-50 border border-slate-150 rounded-3xl text-slate-350 shadow-inner">
        <BoxIcon class="w-12 h-12 stroke-[1.5]" />
      </div>
      <div class="space-y-1 max-w-sm mx-auto">
        <h3 class="text-base font-black text-slate-800">Thư viện mô hình trống</h3>
        <p class="text-xs text-slate-450 leading-relaxed font-semibold">
          Hệ thống chưa có mô hình 3D nào khớp với bộ lọc của bạn. Hãy tải lên mô hình `.glb` đầu tiên!
        </p>
      </div>
      <router-link 
        to="/admin/models-3d/new"
        class="premium-btn inline-flex items-center space-x-1 px-4 py-2.5 bg-slate-900 hover:bg-slate-800 text-white font-extrabold rounded-xl text-xs shadow-sm transition"
      >
        <PlusIcon class="w-3.5 h-3.5" />
        <span>Bắt đầu ngay</span>
      </router-link>
    </div>

    <!-- Grid List View -->
    <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      <div 
        v-for="m in models" 
        :key="m.id"
        class="bg-white border border-slate-200 rounded-2xl overflow-hidden shadow-sm hover:shadow-md hover:border-slate-300/85 transition duration-200 flex flex-col"
      >
        <!-- Preview / Card header thumbnail -->
        <div class="relative aspect-[16/10] bg-slate-900 flex items-center justify-center border-b border-slate-100 overflow-hidden group/card">
          <img 
            v-if="m.thumbnailUrl" 
            :src="m.thumbnailUrl" 
            class="w-full h-full object-cover transition duration-300 group-hover/card:scale-103" 
            :alt="m.title"
          />
          <div v-else class="flex flex-col items-center space-y-2 text-slate-500">
            <BoxIcon class="w-10 h-10 stroke-[1.5]" />
            <span class="text-[9px] font-mono uppercase font-black tracking-wide">{{ m.format || 'glb' }}</span>
          </div>

          <!-- Quick status tag overlay -->
          <div class="absolute top-3 left-3">
            <span 
              class="px-2.5 py-1 rounded-lg text-[9px] font-black uppercase tracking-wider border backdrop-blur-md"
              :class="getStatusClasses(m.status)"
            >
              {{ getStatusLabel(m.status) }}
            </span>
          </div>
        </div>

        <!-- Metadata Info -->
        <div class="p-5 flex-grow flex flex-col justify-between space-y-4">
          <div class="space-y-1.5">
            <span class="text-[9px] font-black text-teal-600 uppercase tracking-wide">
              {{ m.targetType === 'destination' ? 'Địa điểm' : m.targetType }}
            </span>
            <h3 class="font-bold text-slate-800 text-sm leading-snug line-clamp-2" :title="m.title">
              {{ m.title }}
            </h3>
            <div class="flex items-center text-slate-500 text-[10px] font-semibold space-x-1">
              <MapPinIcon class="w-3.5 h-3.5 text-slate-400" />
              <span class="truncate block max-w-[200px]" :title="m.targetName">{{ m.targetName || 'Không có liên kết' }}</span>
            </div>
          </div>

          <div class="pt-3.5 border-t border-slate-100 flex items-center justify-between">
            <span class="text-[9px] font-mono font-bold text-slate-400">Tạo: {{ formatDate(m.createdAt) }}</span>
            
            <div class="flex items-center space-x-1.5">
              <!-- Quick toggle publish action -->
              <button 
                @click="toggleStatus(m)"
                class="p-2 hover:bg-slate-50 rounded-xl border border-slate-200 text-slate-600 hover:text-teal-650 transition"
                :title="m.status === 'published' ? 'Chuyển sang bản nháp' : 'Xuất bản công khai'"
              >
                <EyeOffIcon v-if="m.status === 'published'" class="w-3.5 h-3.5" />
                <EyeIcon v-else class="w-3.5 h-3.5" />
              </button>

              <router-link 
                :to="`/admin/models-3d/${m.id}/edit`"
                class="p-2 hover:bg-slate-50 rounded-xl border border-slate-200 text-slate-650 hover:text-teal-650 transition"
                title="Chỉnh sửa chi tiết"
              >
                <EditIcon class="w-3.5 h-3.5" />
              </router-link>

              <button 
                @click="confirmDelete(m.id)"
                class="p-2 hover:bg-rose-50 border border-slate-200 hover:border-rose-100 rounded-xl text-slate-600 hover:text-rose-600 transition"
                title="Xóa mô hình"
              >
                <TrashIcon class="w-3.5 h-3.5" />
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Simple Pagination -->
    <div 
      v-if="totalPages > 1" 
      class="bg-white p-4 rounded-2xl border border-slate-200/80 shadow-sm flex justify-between items-center text-xs"
    >
      <span class="text-slate-500 font-semibold">Trang {{ page }} / {{ totalPages }} (Tổng {{ totalItems }} mô hình)</span>
      <div class="flex items-center space-x-2">
        <button 
          @click="changePage(page - 1)"
          :disabled="page === 1"
          class="premium-btn px-3.5 py-2 bg-white border border-slate-200 rounded-xl hover:bg-slate-50 transition disabled:opacity-40 shadow-sm font-bold"
        >
          Trước
        </button>
        <button 
          @click="changePage(page + 1)"
          :disabled="page === totalPages"
          class="premium-btn px-3.5 py-2 bg-white border border-slate-200 rounded-xl hover:bg-slate-50 transition disabled:opacity-40 shadow-sm font-bold"
        >
          Sau
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../../utils/api'
import { useToastStore } from '../../stores/toast'
import { useConfirmStore } from '../../stores/confirm'
import { 
  Plus as PlusIcon, 
  Search as SearchIcon, 
  Box as BoxIcon, 
  MapPin as MapPinIcon, 
  Edit as EditIcon, 
  Trash as TrashIcon,
  Eye as EyeIcon,
  EyeOff as EyeOffIcon
} from 'lucide-vue-next'

const toastStore = useToastStore()
const confirmStore = useConfirmStore()

const loading = ref(false)
const models = ref([])
const destinations = ref([])
const totalItems = ref(0)
const totalPages = ref(1)
const page = ref(1)
const pageSize = ref(9)

const filters = ref({
  keyword: '',
  status: '',
  targetId: ''
})

let searchTimeout = null
const debounceSearch = () => {
  if (searchTimeout) clearTimeout(searchTimeout)
  searchTimeout = setTimeout(() => {
    page.value = 1
    fetchModels()
  }, 400)
}

const fetchModels = async () => {
  loading.value = true
  try {
    const res = await api.get('/api/admin/models-3d', {
      params: {
        keyword: filters.value.keyword || null,
        status: filters.value.status || null,
        targetType: filters.value.targetId ? 'destination' : null,
        targetId: filters.value.targetId || null,
        page: page.value,
        pageSize: pageSize.value
      }
    })
    if (res.success) {
      models.value = res.data.items || []
      totalItems.value = res.data.totalItems
      totalPages.value = res.data.totalPages
    }
  } catch (err) {
    toastStore.error(err.message || 'Không thể tải danh sách mô hình 3D.')
  } finally {
    loading.value = false
  }
}

const fetchDestinations = async () => {
  try {
    const res = await api.get('/api/admin/destinations', {
      params: { page: 1, pageSize: 100 }
    })
    if (res.success) {
      destinations.value = res.data.items || []
    }
  } catch (err) {
    console.error('Failed to load destinations:', err)
  }
}

const changePage = (newPage) => {
  if (newPage >= 1 && newPage <= totalPages.value) {
    page.value = newPage
    fetchModels()
  }
}

const toggleStatus = async (m) => {
  const newStatus = m.status === 'published' ? 'draft' : 'published'
  try {
    const res = await api.patch(`/api/admin/models-3d/${m.id}/status`, { status: newStatus })
    if (res.success) {
      toastStore.success('Cập nhật trạng thái thành công!')
      m.status = newStatus
    }
  } catch (err) {
    toastStore.error(err.message || 'Lỗi xảy ra khi cập nhật trạng thái.')
  }
}

const confirmDelete = async (id) => {
  const result = await confirmStore.show({
    title: 'Xóa mô hình 3D',
    message: 'Bạn có chắc chắn muốn xóa mô hình này không? Thao tác này không thể hoàn tác.'
  })
  if (!result) return

  try {
    const res = await api.delete(`/api/admin/models-3d/${id}`)
    if (res.success) {
      toastStore.success('Xóa mô hình 3D thành công!')
      await fetchModels()
    }
  } catch (err) {
    toastStore.error(err.message || 'Lỗi xảy ra khi xóa mô hình.')
  }
}

// Helpers
const getStatusLabel = (s) => {
  if (s === 'published') return 'Đã xuất bản'
  if (s === 'draft') return 'Bản nháp'
  return 'Lưu trữ'
}

const getStatusClasses = (s) => {
  if (s === 'published') return 'bg-teal-500/15 border-teal-550/20 text-teal-600'
  if (s === 'draft') return 'bg-amber-500/15 border-amber-550/20 text-amber-600'
  return 'bg-slate-500/15 border-slate-550/20 text-slate-600'
}

const formatDate = (dateStr) => {
  if (!dateStr) return ''
  const date = new Date(dateStr)
  return date.toLocaleDateString('vi-VN')
}

onMounted(() => {
  fetchModels()
  fetchDestinations()
})
</script>
