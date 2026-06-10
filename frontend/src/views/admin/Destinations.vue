<template>
  <div class="space-y-6">
    <!-- Header Controls -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 bg-gradient-to-r from-slate-900 via-slate-800 to-teal-950 p-6 md:p-8 rounded-3xl text-white shadow-xl relative overflow-hidden">
      <div class="absolute inset-0 opacity-10 bg-[radial-gradient(#e5e7eb_1px,transparent_1px)] [background-size:16px_16px]"></div>
      <div class="absolute w-48 h-48 rounded-full bg-teal-500/10 blur-2xl -bottom-10 -right-10 pointer-events-none"></div>
      <div class="relative z-10 space-y-1">
        <h2 class="text-xl md:text-2xl font-black tracking-tight">Danh sách địa điểm du lịch</h2>
        <p class="text-xs text-teal-200/70 font-medium">Quản lý trạng thái hiển thị, vị trí bản đồ và các tour thực tế ảo 360 độ.</p>
      </div>
      <router-link 
        to="/admin/destinations/new" 
        class="premium-btn inline-flex items-center space-x-2 px-5 py-3 bg-amber-500 hover:bg-amber-600 text-slate-950 font-black text-sm rounded-xl shadow-lg shadow-amber-500/15 hover:shadow-amber-500/25 active:scale-[0.98] transition relative z-10"
      >
        <PlusIcon class="w-4 h-4 stroke-[3]" />
        <span>Thêm địa điểm</span>
      </router-link>
    </div>

    <!-- Filters Bar -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-4 bg-white p-5 rounded-2xl border border-slate-200/80 shadow-sm">
      <!-- Search -->
      <div class="relative lg:col-span-2">
        <input 
          v-model="searchKeyword"
          type="text" 
          placeholder="Tìm theo tên, địa chỉ, từ khóa..." 
          class="w-full pl-10 pr-4 py-2.5 bg-slate-50 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:ring-4 focus:ring-teal-500/5 focus:bg-white transition"
          @keyup.enter="handleSearch"
        />
        <span class="absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400">
          <SearchIcon class="w-4 h-4" />
        </span>
      </div>

      <!-- Category Filter -->
      <select 
        v-model="selectedCategory"
        class="px-3 py-2.5 bg-slate-50 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:ring-4 focus:ring-teal-500/5 focus:bg-white transition cursor-pointer appearance-none"
      >
        <option :value="null">Tất cả Danh mục</option>
        <option v-for="cat in categories" :key="cat.id" :value="cat.id">
          {{ cat.name }}
        </option>
      </select>

      <!-- Status Filter -->
      <select 
        v-model="selectedStatus"
        class="px-3 py-2.5 bg-slate-50 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:ring-4 focus:ring-teal-500/5 focus:bg-white transition cursor-pointer appearance-none"
      >
        <option :value="null">Tất cả Trạng thái</option>
        <option value="published">Đã xuất bản (Published)</option>
        <option value="draft">Bản nháp (Draft)</option>
        <option value="archived">Lưu trữ (Archived)</option>
      </select>

      <!-- Action Search Button -->
      <button 
        @click="handleSearch"
        class="premium-btn py-2.5 bg-teal-700 hover:bg-teal-850 text-white font-bold text-sm rounded-xl transition duration-150 shadow-md shadow-teal-700/10 hover:shadow-teal-700/20"
      >
        Lọc kết quả
      </button>
    </div>

    <!-- Loading Skeleton State -->
    <div v-if="loading" class="bg-white rounded-2xl border border-slate-200/80 shadow-sm overflow-hidden p-6 space-y-4">
      <div v-for="i in 3" :key="i" class="flex items-center justify-between py-4 border-b border-slate-100 last:border-0 animate-pulse">
        <div class="flex items-center space-x-4">
          <div class="w-16 h-12 bg-slate-100 rounded-lg"></div>
          <div class="space-y-2">
            <div class="h-4 w-40 bg-slate-100 rounded"></div>
            <div class="h-3 w-24 bg-slate-100 rounded"></div>
          </div>
        </div>
        <div class="h-8 w-24 bg-slate-100 rounded-lg"></div>
      </div>
    </div>

    <!-- Error Alert -->
    <div v-else-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-xl flex items-center space-x-3">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0 text-red-500" />
      <span class="text-sm font-semibold">{{ error }}</span>
    </div>

    <!-- Empty State -->
    <div v-else-if="destinations.length === 0" class="bg-white py-16 text-center rounded-2xl border border-slate-200/80 shadow-sm space-y-4">
      <div class="inline-flex p-4 bg-slate-50 text-slate-400 rounded-full border border-slate-100">
        <InboxIcon class="w-10 h-10" />
      </div>
      <div class="space-y-1">
        <h3 class="text-base font-black text-slate-700">Không tìm thấy địa điểm nào</h3>
        <p class="text-xs text-slate-500 max-w-sm mx-auto">Không có kết quả nào phù hợp với bộ lọc tìm kiếm hiện tại.</p>
      </div>
      <button @click="resetFilters" class="text-xs font-bold text-teal-600 hover:text-teal-700 underline">
        Đặt lại bộ lọc
      </button>
    </div>

    <!-- Data Table Card -->
    <div v-else class="bg-white rounded-2xl border border-slate-200/80 shadow-sm overflow-hidden">
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-slate-50 border-b border-slate-200/80 text-slate-400 text-[10px] font-black uppercase tracking-wider">
              <th class="py-4 px-6">Ảnh</th>
              <th class="py-4 px-6">Tên địa điểm</th>
              <th class="py-4 px-6">Danh mục & Khu vực</th>
              <th class="py-4 px-6">Tọa độ</th>
              <th class="py-4 px-6">Trạng thái</th>
              <th class="py-4 px-6 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-100 text-sm">
            <tr v-for="dest in destinations" :key="dest.id" class="hover:bg-slate-50/50 transition duration-150">
              <!-- Cover Image -->
              <td class="py-4 px-6">
                <div class="w-16 h-12 bg-slate-100 border border-slate-200/60 rounded-xl overflow-hidden flex items-center justify-center text-slate-400 shadow-sm">
                  <img v-if="dest.coverImageUrl" :src="dest.coverImageUrl" class="w-full h-full object-cover" />
                  <ImageIcon v-else class="w-5 h-5 opacity-60" />
                </div>
              </td>
              <!-- Name & Slug -->
              <td class="py-4 px-6">
                <div class="space-y-0.5">
                  <h4 class="font-bold text-slate-800 hover:text-teal-600 transition cursor-pointer">{{ dest.name }}</h4>
                  <span class="text-[11px] text-slate-400 block font-mono">{{ dest.slug }}</span>
                </div>
              </td>
              <!-- Category & Region -->
              <td class="py-4 px-6">
                <div class="space-y-1.5">
                  <!-- Category Badge -->
                  <span 
                    class="inline-flex items-center px-2.5 py-0.5 rounded-full text-[11px] font-bold border shadow-sm"
                    :style="{
                      backgroundColor: dest.category?.color ? `${dest.category.color}10` : '#f1f5f9',
                      borderColor: dest.category?.color ? `${dest.category.color}30` : '#cbd5e1',
                      color: dest.category?.color || '#475569'
                    }"
                  >
                    {{ dest.category?.name || 'Chưa phân loại' }}
                  </span>
                  <span class="text-xs text-slate-500 block font-semibold">{{ dest.regionName }}</span>
                </div>
              </td>
              <!-- Coordinates -->
              <td class="py-4 px-6">
                <div class="font-mono text-xs text-slate-500 space-y-0.5">
                  <div class="flex items-center space-x-1">
                    <span class="text-slate-400">Lat:</span>
                    <span class="font-semibold text-slate-700">{{ dest.latitude }}</span>
                  </div>
                  <div class="flex items-center space-x-1">
                    <span class="text-slate-400">Lng:</span>
                    <span class="font-semibold text-slate-700">{{ dest.longitude }}</span>
                  </div>
                </div>
              </td>
              <!-- Status Badge -->
              <td class="py-4 px-6">
                <span 
                  class="inline-flex items-center px-3 py-1 rounded-full text-xs font-bold shadow-sm"
                  :class="statusBadgeClass(dest.status)"
                >
                  <span class="w-1.5 h-1.5 rounded-full mr-1.5 animate-pulse" :class="statusDotClass(dest.status)"></span>
                  {{ statusText(dest.status) }}
                </span>
              </td>
              <!-- Actions -->
              <td class="py-4 px-6 text-right">
                <div class="flex items-center justify-end space-x-1">
                  <!-- Quick status toggler (eye/eye-off) -->
                  <button 
                    @click="toggleStatus(dest)"
                    class="p-2 hover:bg-slate-100 rounded-xl text-slate-500 hover:text-slate-700 transition"
                    :title="dest.status === 'published' ? 'Chuyển về bản nháp' : 'Xuất bản công khai'"
                    :disabled="dest.status === 'archived'"
                  >
                    <EyeIcon v-if="dest.status === 'published'" class="w-4 h-4 text-teal-600" />
                    <EyeOffIcon v-else class="w-4 h-4" :class="dest.status === 'archived' ? 'opacity-30' : 'text-slate-400'" />
                  </button>

                  <!-- Edit button -->
                  <router-link 
                    :to="`/admin/destinations/${dest.id}/edit`"
                    class="p-2 hover:bg-slate-100 rounded-xl text-slate-500 hover:text-teal-600 transition"
                    title="Chỉnh sửa địa điểm"
                  >
                    <EditIcon class="w-4 h-4" />
                  </router-link>

                  <!-- Delete button -->
                  <button 
                    @click="confirmDelete(dest)"
                    class="p-2 hover:bg-slate-100 rounded-xl text-slate-500 hover:text-red-600 transition"
                    title="Xóa địa điểm (Lưu trữ)"
                    :disabled="dest.status === 'archived'"
                  >
                    <TrashIcon class="w-4 h-4" :class="dest.status === 'archived' ? 'opacity-30' : 'text-rose-500'" />
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Pagination controls -->
      <div class="px-6 py-4 bg-slate-50 border-t border-slate-200/60 flex flex-col sm:flex-row justify-between items-center gap-4">
        <span class="text-xs text-slate-500 font-bold">
          Hiển thị trang {{ currentPage }}/{{ totalPages }} (Tổng {{ totalItems }} kết quả)
        </span>
        <div class="flex items-center space-x-2">
          <button 
            @click="prevPage" 
            :disabled="currentPage === 1"
            class="premium-btn px-4 py-2 bg-white border border-slate-200 rounded-xl text-xs font-bold text-slate-600 hover:bg-slate-50 active:scale-[0.98] transition disabled:opacity-40 disabled:cursor-not-allowed shadow-sm"
          >
            Trang trước
          </button>
          <button 
            @click="nextPage" 
            :disabled="currentPage === totalPages || totalPages === 0"
            class="premium-btn px-4 py-2 bg-white border border-slate-200 rounded-xl text-xs font-bold text-slate-600 hover:bg-slate-50 active:scale-[0.98] transition disabled:opacity-40 disabled:cursor-not-allowed shadow-sm"
          >
            Trang sau
          </button>
        </div>
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
  Image as ImageIcon, 
  Eye as EyeIcon, 
  EyeOff as EyeOffIcon, 
  Edit as EditIcon, 
  Trash as TrashIcon, 
  Inbox as InboxIcon, 
  AlertCircle as AlertCircleIcon 
} from 'lucide-vue-next'

const toastStore = useToastStore()
const confirmStore = useConfirmStore()

const loading = ref(false)
const error = ref(null)

const destinations = ref([])
const categories = ref([])

const searchKeyword = ref('')
const selectedCategory = ref(null)
const selectedStatus = ref(null)

const currentPage = ref(1)
const pageSize = ref(10)
const totalItems = ref(0)
const totalPages = ref(0)

const fetchDestinations = async () => {
  loading.value = true
  error.value = null
  try {
    const response = await api.get('/api/admin/destinations', {
      params: {
        keyword: searchKeyword.value || undefined,
        categoryId: selectedCategory.value || undefined,
        status: selectedStatus.value || undefined,
        page: currentPage.value,
        pageSize: pageSize.value
      }
    })

    if (response.success) {
      destinations.value = response.data.items
      currentPage.value = response.data.page
      pageSize.value = response.data.pageSize
      totalItems.value = response.data.totalItems
      totalPages.value = response.data.totalPages
    }
  } catch (err) {
    error.value = 'Không thể tải danh sách địa điểm.'
  } finally {
    loading.value = false
  }
}

const fetchFilterData = async () => {
  try {
    const catResponse = await api.get('/api/admin/categories')
    if (catResponse.success) {
      categories.value = catResponse.data
    }
  } catch (err) {
    console.error('Không thể tải bộ lọc danh mục.', err)
  }
}

const handleSearch = () => {
  currentPage.value = 1
  fetchDestinations()
}

const resetFilters = () => {
  searchKeyword.value = ''
  selectedCategory.value = null
  selectedStatus.value = null
  currentPage.value = 1
  fetchDestinations()
}

const prevPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--
    fetchDestinations()
  }
}

const nextPage = () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++
    fetchDestinations()
  }
}

// Quick status toggle
const toggleStatus = async (dest) => {
  if (dest.status === 'archived') return

  const targetStatus = dest.status === 'published' ? 'draft' : 'published'
  try {
    const response = await api.patch(`/api/admin/destinations/${dest.id}/status`, {
      status: targetStatus
    })
    
    if (response.success) {
      dest.status = targetStatus
      toastStore.success(`Đã chuyển địa điểm sang ${targetStatus === 'published' ? 'Đã xuất bản' : 'Bản nháp'}`)
    }
  } catch (err) {
    toastStore.error(err.message || 'Không thể cập nhật trạng thái.')
  }
}

// Soft deletion
const confirmDelete = async (dest) => {
  if (dest.status === 'archived') return

  const confirmResult = await confirmStore.show({
    title: 'Xóa địa điểm',
    message: `Bạn có chắc chắn muốn xóa (lưu trữ) địa điểm "${dest.name}"?`
  })
  if (!confirmResult) return

  try {
    const response = await api.delete(`/api/admin/destinations/${dest.id}`)
    if (response.success) {
      dest.status = 'archived'
      toastStore.success('Đã chuyển địa điểm vào kho lưu trữ.')
    }
  } catch (err) {
    toastStore.error(err.message || 'Không thể xóa địa điểm.')
  }
}

// Badge styling styling computations
const statusBadgeClass = (status) => {
  if (status === 'published') return 'bg-emerald-50 text-emerald-700 border border-emerald-200'
  if (status === 'draft') return 'bg-amber-50 text-amber-700 border border-amber-200'
  return 'bg-slate-100 text-slate-600 border border-slate-200'
}

const statusDotClass = (status) => {
  if (status === 'published') return 'bg-emerald-500'
  if (status === 'draft') return 'bg-amber-500'
  return 'bg-slate-400'
}

const statusText = (status) => {
  if (status === 'published') return 'Đã xuất bản'
  if (status === 'draft') return 'Bản nháp'
  return 'Lưu trữ'
}

onMounted(() => {
  fetchDestinations()
  fetchFilterData()
})
</script>
