<template>
  <div class="max-w-7xl mx-auto px-4 py-8 sm:px-6 lg:px-8 space-y-8">
    <!-- Header Hero Banner -->
    <div class="bg-gradient-to-r from-teal-800 to-slate-900 rounded-3xl p-8 md:p-12 text-white shadow-xl relative overflow-hidden">
      <div class="absolute w-64 h-64 rounded-full bg-teal-600/10 blur-3xl -top-20 -right-20"></div>
      <div class="absolute w-48 h-48 rounded-full bg-amber-500/5 blur-2xl -bottom-10 left-1/3"></div>
      
      <div class="relative z-10 max-w-2xl space-y-3">
        <span class="inline-flex items-center px-3 py-1 bg-teal-500/20 border border-teal-500/30 text-teal-300 font-semibold rounded-full text-xs uppercase tracking-wider">
          Khám phá du lịch số
        </span>
        <h1 class="text-3xl md:text-4xl font-extrabold tracking-tight">Danh sách Địa điểm Du lịch</h1>
        <p class="text-xs md:text-sm text-teal-200/80 leading-relaxed">
          Tìm kiếm các di tích lịch sử, địa điểm văn hóa, khu sinh thái và các làng nghề truyền thống nổi bật tại vùng đồng bằng sông Cửu Long.
        </p>
      </div>
    </div>

    <!-- Filters and Search Controls -->
    <div class="grid grid-cols-1 md:grid-cols-4 gap-6 bg-white p-6 rounded-2xl border border-slate-200/60 shadow-sm">
      <!-- Search input -->
      <div class="relative md:col-span-2">
        <input 
          v-model="searchKeyword"
          type="text" 
          placeholder="Tìm địa điểm, địa chỉ, từ khóa..." 
          class="w-full pl-10 pr-4 py-2.5 bg-slate-50 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:bg-white transition"
          @keyup.enter="handleSearch"
        />
        <span class="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400">
          <SearchIcon class="w-5 h-5" />
        </span>
      </div>

      <!-- Categories Filter Dropdown -->
      <select 
        v-model="selectedCategory"
        class="px-3 py-2.5 bg-slate-50 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:bg-white transition"
        @change="handleSearch"
      >
        <option :value="null">Tất cả Danh mục</option>
        <option v-for="cat in categories" :key="cat.id" :value="cat.id">
          {{ cat.name }}
        </option>
      </select>

      <!-- Search Trigger Button -->
      <button 
        @click="handleSearch"
        class="py-2.5 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold text-sm rounded-xl transition duration-150 shadow-md hover:shadow-teal-500/10 active:scale-[0.98]"
      >
        Tìm kiếm
      </button>
    </div>

    <!-- Category Quick Chips list -->
    <div class="flex flex-wrap gap-2 items-center">
      <span class="text-xs font-bold text-slate-400 uppercase mr-2">Lọc nhanh:</span>
      <button 
        @click="selectCategoryChip(null)"
        :class="[
          'px-3.5 py-1.5 rounded-full text-xs font-bold transition border',
          selectedCategory === null 
            ? 'bg-teal-600 text-white border-teal-600 shadow-sm' 
            : 'bg-white text-slate-600 border-slate-200 hover:border-slate-350'
        ]"
      >
        Tất cả
      </button>
      <button 
        v-for="cat in categories" 
        :key="cat.id"
        @click="selectCategoryChip(cat.id)"
        :class="[
          'px-3.5 py-1.5 rounded-full text-xs font-bold transition border',
          selectedCategory === cat.id 
            ? 'bg-teal-600 text-white border-teal-600 shadow-sm' 
            : 'bg-white text-slate-600 border-slate-200 hover:border-slate-350'
        ]"
      >
        {{ cat.name }}
      </button>
    </div>

    <!-- Error Alert state -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-xl flex items-center space-x-3">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0" />
      <span class="text-sm font-medium">{{ error }}</span>
    </div>

    <!-- Loading Skeleton Grid -->
    <div v-else-if="loading" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      <div v-for="i in 6" :key="i" class="bg-white rounded-2xl border border-slate-200/50 p-4 space-y-4 animate-pulse">
        <div class="aspect-[16/10] bg-slate-100 rounded-xl w-full"></div>
        <div class="space-y-2">
          <div class="h-4 bg-slate-100 rounded w-1/3"></div>
          <div class="h-5 bg-slate-100 rounded w-3/4"></div>
          <div class="h-3 bg-slate-100 rounded w-full"></div>
        </div>
      </div>
    </div>

    <!-- Empty Results State -->
    <div v-else-if="destinations.length === 0" class="bg-white py-20 text-center rounded-2xl border border-slate-200/60 shadow-sm space-y-4">
      <div class="inline-flex p-4 bg-slate-50 text-slate-400 rounded-full border border-slate-100">
        <InboxIcon class="w-12 h-12" />
      </div>
      <div class="space-y-1">
        <h3 class="text-base font-bold text-slate-700">Không tìm thấy địa điểm nào</h3>
        <p class="text-xs text-slate-500 max-w-sm mx-auto">Vui lòng thay đổi từ khóa tìm kiếm hoặc lọc theo danh mục khác.</p>
      </div>
      <button @click="resetFilters" class="text-xs font-bold text-teal-600 hover:text-teal-700 underline">
        Đặt lại bộ lọc
      </button>
    </div>

    <!-- Destinations Cards Grid List -->
    <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      <div 
        v-for="dest in destinations" 
        :key="dest.id" 
        class="bg-white rounded-2xl border border-slate-200/60 overflow-hidden shadow-sm hover:shadow-md hover:border-teal-500/30 group flex flex-col justify-between transition duration-200"
      >
        <!-- Card Cover image container -->
        <div class="aspect-[16/10] bg-slate-50 relative overflow-hidden border-b border-slate-100">
          <img 
            v-if="dest.coverImageUrl" 
            :src="dest.coverImageUrl" 
            class="w-full h-full object-cover group-hover:scale-102 transition duration-300"
            loading="lazy" 
          />
          <div v-else class="w-full h-full flex items-center justify-center text-slate-400">
            <ImageIcon class="w-10 h-10" />
          </div>

          <!-- Indication badge overlay -->
          <div class="absolute top-3 left-3 flex flex-col gap-1.5 z-10">
            <!-- Category badge -->
            <span 
              class="inline-flex px-2.5 py-0.5 rounded-md text-[10px] font-bold border uppercase tracking-wider"
              :style="{
                backgroundColor: dest.category?.color ? `${dest.category.color}15` : '#f1f5f9',
                borderColor: dest.category?.color ? `${dest.category.color}35` : '#cbd5e1',
                color: dest.category?.color || '#475569'
              }"
            >
              {{ dest.category?.name }}
            </span>
          </div>

          <!-- Badges for features (Tour 360 / Audio Guide) -->
          <div class="absolute bottom-3 right-3 flex items-center gap-1.5 z-10">
            <span 
              v-if="dest.hasVirtualTour" 
              class="flex items-center space-x-1 px-2 py-1 bg-slate-900/85 backdrop-blur-sm text-teal-400 font-bold rounded-lg text-[9px] uppercase tracking-wider shadow-sm"
              title="Có tham quan ảo 360°"
            >
              <CompassIcon class="w-3 h-3" />
              <span>360° Tour</span>
            </span>
            <span 
              v-if="dest.hasAudioGuide" 
              class="flex items-center space-x-1 px-2 py-1 bg-slate-900/85 backdrop-blur-sm text-amber-400 font-bold rounded-lg text-[9px] uppercase tracking-wider shadow-sm"
              title="Có audio guide thuyết minh"
            >
              <MusicIcon class="w-3 h-3" />
              <span>Audio</span>
            </span>
          </div>
        </div>

        <!-- Card Body details section -->
        <div class="p-6 flex-1 flex flex-col justify-between space-y-4">
          <div class="space-y-2">
            <span class="text-[10px] font-bold text-slate-400 uppercase block tracking-wider">{{ dest.regionName }}</span>
            <h3 class="text-base font-extrabold text-slate-800 group-hover:text-teal-700 transition line-clamp-1">
              {{ dest.name }}
            </h3>
            <p class="text-xs text-slate-500 leading-relaxed line-clamp-3">
              {{ dest.shortDescription }}
            </p>
          </div>

          <!-- Action CTA Link -->
          <router-link 
            :to="`/destinations/${dest.slug || dest.id}`"
            class="w-full py-2.5 bg-slate-50 hover:bg-teal-50 border border-slate-200/80 hover:border-teal-500/20 text-slate-700 hover:text-teal-700 font-bold text-xs rounded-xl transition flex items-center justify-center space-x-1"
          >
            <span>Khám phá chi tiết</span>
            <ArrowRightIcon class="w-3.5 h-3.5" />
          </router-link>
        </div>
      </div>
    </div>

    <!-- Pagination footer -->
    <div 
      v-if="destinations.length > 0 && totalPages > 1" 
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
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import api from '../utils/api'
import { 
  Search as SearchIcon, 
  Image as ImageIcon, 
  Inbox as InboxIcon, 
  AlertCircle as AlertCircleIcon, 
  Compass as CompassIcon,
  Music as MusicIcon,
  ArrowRight as ArrowRightIcon
} from 'lucide-vue-next'

const loading = ref(false)
const error = ref(null)

const destinations = ref([])
const categories = ref([])

const searchKeyword = ref('')
const selectedCategory = ref(null)

const currentPage = ref(1)
const pageSize = ref(6)
const totalItems = ref(0)
const totalPages = ref(0)

const fetchDestinations = async () => {
  loading.value = true
  error.value = null
  try {
    const response = await api.get('/api/destinations', {
      params: {
        keyword: searchKeyword.value || undefined,
        categoryId: selectedCategory.value || undefined,
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

const fetchCategories = async () => {
  try {
    const response = await api.get('/api/categories')
    if (response.success) {
      categories.value = response.data
    }
  } catch (err) {
    console.error('Không thể tải danh mục.', err)
  }
}

const handleSearch = () => {
  currentPage.value = 1
  fetchDestinations()
}

const selectCategoryChip = (catId) => {
  selectedCategory.value = catId
  currentPage.value = 1
  fetchDestinations()
}

const resetFilters = () => {
  searchKeyword.value = ''
  selectedCategory.value = null
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

const route = useRoute()

onMounted(async () => {
  await fetchCategories()
  if (route.query.category) {
    selectedCategory.value = route.query.category
  }
  await fetchDestinations()
})
</script>
