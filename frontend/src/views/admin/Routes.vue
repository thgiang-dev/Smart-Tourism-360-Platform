<template>
  <div class="space-y-8 font-sans">
    <!-- Header with Action -->
    <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
      <div class="space-y-1">
        <h2 class="text-xl md:text-2xl font-black text-slate-800 tracking-tight">Danh sách tuyến tham quan</h2>
        <p class="text-xs md:text-sm text-slate-400 font-medium">Tổ chức các địa điểm du lịch thành các tuyến theo lịch trình và chủ đề.</p>
      </div>
      <router-link 
        to="/admin/routes/new"
        class="premium-btn flex items-center justify-center space-x-2 px-5 py-3 bg-teal-600 hover:bg-teal-700 text-white font-bold text-sm rounded-xl shadow transition"
      >
        <PlusIcon class="w-5 h-5" />
        <span>Thêm tuyến du lịch mới</span>
      </router-link>
    </div>

    <!-- Error alert -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-xl flex items-center space-x-3 shadow-sm">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0 text-red-500" />
      <span class="text-sm font-bold">{{ error }}</span>
    </div>

    <!-- Filter Bar -->
    <div class="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm flex flex-col md:flex-row gap-4 items-center justify-between">
      <!-- Search input -->
      <div class="relative w-full md:w-80">
        <SearchIcon class="absolute left-4 top-3 h-4 w-4 text-slate-400" />
        <input 
          v-model="filters.keyword"
          type="text" 
          placeholder="Tìm theo tên hoặc mô tả..." 
          class="w-full pl-10 pr-4 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm font-semibold transition focus:outline-none placeholder:text-slate-400"
          @input="handleFilterChange"
        />
      </div>

      <!-- Filters dropdowns -->
      <div class="flex flex-wrap gap-3 w-full md:w-auto items-center justify-end">
        <!-- Region Filter -->
        <select 
          v-model="filters.regionId" 
          class="px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm font-bold transition focus:outline-none text-slate-700"
          @change="handleFilterChange"
        >
          <option :value="null">Tất cả khu vực</option>
          <option v-for="r in regions" :key="r.id" :value="r.id">{{ r.name }}</option>
        </select>

        <!-- Theme Filter -->
        <select 
          v-model="filters.theme" 
          class="px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm font-bold transition focus:outline-none text-slate-700"
          @change="handleFilterChange"
        >
          <option :value="null">Tất cả chủ đề</option>
          <option value="culture">Văn hóa - Lịch sử</option>
          <option value="eco">Sinh thái sông nước</option>
          <option value="food">Ẩm thực địa phương</option>
          <option value="craft">Làng nghề</option>
        </select>

        <!-- Status Filter -->
        <select 
          v-model="filters.status" 
          class="px-3 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500 focus:bg-white rounded-xl text-sm font-bold transition focus:outline-none text-slate-700"
          @change="handleFilterChange"
        >
          <option :value="null">Tất cả trạng thái</option>
          <option value="draft">Bản nháp (Draft)</option>
          <option value="published">Đã xuất bản (Published)</option>
          <option value="archived">Đã lưu trữ (Archived)</option>
        </select>
      </div>
    </div>

    <!-- Table content -->
    <div class="bg-white rounded-2xl border border-slate-200/80 shadow-sm overflow-hidden">
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-slate-50 border-b border-slate-200/80 text-[10px] font-black text-slate-400 uppercase tracking-wider">
              <th class="py-4 px-6">Ảnh bìa</th>
              <th class="py-4 px-6">Tên tuyến / Khu vực</th>
              <th class="py-4 px-6">Chủ đề</th>
              <th class="py-4 px-6">Thông số</th>
              <th class="py-4 px-6 text-center">Số địa điểm</th>
              <th class="py-4 px-6">Trạng thái</th>
              <th class="py-4 px-6 text-center">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-100 text-sm">
            <tr v-if="loading">
              <td colspan="7" class="py-12 text-center text-slate-400 font-medium">
                <div class="flex items-center justify-center space-x-2">
                  <div class="w-4 h-4 border-2 border-teal-500 border-t-transparent rounded-full animate-spin"></div>
                  <span>Đang tải dữ liệu tuyến...</span>
                </div>
              </td>
            </tr>
            <tr v-else-if="routes.length === 0">
              <td colspan="7" class="py-12 text-center text-slate-400 font-medium">
                Không tìm thấy tuyến tham quan nào.
              </td>
            </tr>
            <tr 
              v-else 
              v-for="r in routes" 
              :key="r.id"
              class="hover:bg-slate-50/50 transition duration-150"
            >
              <!-- Thumbnail -->
              <td class="py-4 px-6">
                <div class="w-16 h-10 rounded-lg overflow-hidden bg-slate-100 border border-slate-200 flex-shrink-0">
                  <img 
                    :src="r.thumbnailUrl || 'https://images.unsplash.com/photo-1599707367072-cd6ada2bc375?auto=format&fit=crop&w=100&q=80'" 
                    alt="cover" 
                    class="w-full h-full object-cover"
                  />
                </div>
              </td>

              <!-- Title & Region -->
              <td class="py-4 px-6 font-semibold text-slate-800">
                <div class="space-y-0.5">
                  <p class="font-bold text-slate-800 leading-tight">{{ r.title }}</p>
                  <p class="text-[10px] text-slate-400 tracking-wide font-black uppercase">{{ r.regionName }}</p>
                </div>
              </td>

              <!-- Theme -->
              <td class="py-4 px-6 font-medium text-slate-600">
                <span class="text-xs px-2.5 py-1 rounded-full bg-slate-100 border border-slate-200/50 text-slate-700 font-bold">
                  {{ getThemeLabel(r.theme) }}
                </span>
              </td>

              <!-- Distance / Duration -->
              <td class="py-4 px-6 text-xs text-slate-500 font-bold space-y-1">
                <div class="flex items-center space-x-1">
                  <ClockIcon class="w-3.5 h-3.5 text-slate-400" />
                  <span>{{ r.estimatedDuration || 'N/A' }}</span>
                </div>
                <div class="flex items-center space-x-1" v-if="r.distanceKm">
                  <CompassIcon class="w-3.5 h-3.5 text-slate-400" />
                  <span>{{ r.distanceKm }} km</span>
                </div>
              </td>

              <!-- Destination Count -->
              <td class="py-4 px-6 text-center font-bold text-slate-700">
                {{ r.destinationCount }}
              </td>

              <!-- Status Badge -->
              <td class="py-4 px-6">
                <span 
                  :class="{
                    'bg-slate-100 text-slate-600 border-slate-200': r.status === 'draft',
                    'bg-emerald-50 text-emerald-700 border-emerald-200': r.status === 'published',
                    'bg-amber-50 text-amber-700 border-amber-200': r.status === 'archived'
                  }"
                  class="text-[10px] font-black uppercase px-2.5 py-1 rounded-full border shadow-sm inline-block"
                >
                  {{ getStatusLabel(r.status) }}
                </span>
              </td>

              <!-- Action Buttons -->
              <td class="py-4 px-6">
                <div class="flex items-center justify-center space-x-2">
                  <router-link 
                    :to="`/admin/routes/${r.id}/destinations`"
                    class="p-2 text-indigo-600 hover:bg-indigo-50 border border-transparent hover:border-indigo-100 rounded-xl transition"
                    title="Quản lý địa điểm của Tuyến"
                  >
                    <ListIcon class="w-4 h-4" />
                  </router-link>
                  <router-link 
                    :to="`/admin/routes/${r.id}/edit`"
                    class="p-2 text-teal-600 hover:bg-teal-50 border border-transparent hover:border-teal-100 rounded-xl transition"
                    title="Chỉnh sửa thông tin cơ bản"
                  >
                    <EditIcon class="w-4 h-4" />
                  </router-link>
                  
                  <!-- Publish / Archive actions -->
                  <button 
                    v-if="r.status !== 'published'"
                    class="p-2 text-emerald-600 hover:bg-emerald-50 border border-transparent hover:border-emerald-100 rounded-xl transition"
                    title="Xuất bản tuyến"
                    @click="updateStatus(r.id, 'published')"
                  >
                    <CheckCircleIcon class="w-4 h-4" />
                  </button>
                  <button 
                    v-if="r.status === 'published'"
                    class="p-2 text-amber-600 hover:bg-amber-50 border border-transparent hover:border-amber-100 rounded-xl transition"
                    title="Hạ xuống bản nháp"
                    @click="updateStatus(r.id, 'draft')"
                  >
                    <XCircleIcon class="w-4 h-4" />
                  </button>

                  <button 
                    class="p-2 text-rose-600 hover:bg-rose-50 border border-transparent hover:border-rose-100 rounded-xl transition"
                    title="Lưu trữ / Xóa tuyến"
                    @click="deleteRoute(r.id)"
                  >
                    <TrashIcon class="w-4 h-4" />
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import api from '../../utils/api'
import { 
  Plus as PlusIcon, 
  Search as SearchIcon, 
  AlertCircle as AlertCircleIcon,
  Clock as ClockIcon, 
  Compass as CompassIcon,
  List as ListIcon,
  Edit2 as EditIcon,
  Trash as TrashIcon,
  CheckCircle as CheckCircleIcon,
  XCircle as XCircleIcon
} from 'lucide-vue-next'

const loading = ref(true)
const routes = ref([])
const regions = ref([])
const error = ref(null)

const filters = reactive({
  keyword: '',
  regionId: null,
  theme: null,
  status: null
})

// Fetch all regions for filters
const fetchRegions = async () => {
  try {
    const res = await api.get('/api/admin/regions')
    if (res.success) {
      regions.value = res.data
    }
  } catch (err) {
    console.error('Error fetching regions:', err)
  }
}

// Fetch admin routes list
const fetchRoutes = async () => {
  loading.value = true
  error.value = null
  try {
    const params = {
      page: 1,
      pageSize: 50
    }
    if (filters.regionId) params.regionId = filters.regionId
    if (filters.theme) params.theme = filters.theme
    if (filters.status) params.status = filters.status
    if (filters.keyword.trim()) params.keyword = filters.keyword.trim()

    const res = await api.get('/api/admin/routes', { params })
    if (res.success) {
      routes.value = res.data.items
    }
  } catch (err) {
    error.value = err.message || 'Không thể tải danh sách tuyến tham quan.'
  } finally {
    loading.value = false
  }
}

let timeout = null
const handleFilterChange = () => {
  clearTimeout(timeout)
  timeout = setTimeout(() => {
    fetchRoutes()
  }, 300)
}

const updateStatus = async (id, newStatus) => {
  try {
    const res = await api.patch(`/api/admin/routes/${id}/status`, { status: newStatus })
    if (res.success) {
      fetchRoutes()
    }
  } catch (err) {
    alert(err.message || 'Lỗi cập nhật trạng thái xuất bản.')
  }
}

const deleteRoute = async (id) => {
  if (!confirm('Bạn có chắc chắn muốn đưa tuyến tham quan này vào Lưu trữ (Archived)?')) return
  try {
    const res = await api.delete(`/api/admin/routes/${id}`)
    if (res.success) {
      fetchRoutes()
    }
  } catch (err) {
    alert(err.message || 'Lỗi xóa tuyến tham quan.')
  }
}

const getThemeLabel = (theme) => {
  const themes = {
    culture: 'Văn hóa - Lịch sử',
    eco: 'Sinh thái sông nước',
    food: 'Ẩm thực địa phương',
    craft: 'Làng nghề'
  }
  return themes[theme] || 'Khám phá'
}

const getStatusLabel = (status) => {
  const statuses = {
    draft: 'Bản nháp',
    published: 'Đã xuất bản',
    archived: 'Lưu trữ'
  }
  return statuses[status] || status
}

onMounted(() => {
  fetchRegions()
  fetchRoutes()
})
</script>
