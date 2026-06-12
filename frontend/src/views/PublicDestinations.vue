<template>
  <div class="max-w-7xl mx-auto px-4 py-8 sm:px-6 lg:px-8 space-y-10">
    <!-- Header Hero Banner -->
    <div class="bg-gradient-to-r from-teal-900 via-teal-800 to-slate-900 rounded-3xl p-8 md:p-14 text-white shadow-xl relative overflow-hidden">
      <!-- Background pattern overlay -->
      <div class="absolute inset-0 opacity-10 bg-[radial-gradient(#e5e7eb_1px,transparent_1px)] [background-size:16px_16px]"></div>
      <div class="absolute w-[400px] h-[400px] rounded-full bg-teal-500/10 blur-3xl -top-20 -right-20 pointer-events-none"></div>
      <div class="absolute w-[300px] h-[300px] rounded-full bg-amber-500/5 blur-3xl -bottom-10 left-1/3 pointer-events-none"></div>
      
      <div class="relative z-10 max-w-2xl space-y-4">
        <span class="inline-flex items-center px-4 py-1.5 bg-teal-500/20 border border-teal-500/30 text-teal-300 font-extrabold rounded-full text-xs uppercase tracking-wider shadow-sm">
          Khám phá du lịch số
        </span>
        <h1 class="text-3xl md:text-5xl font-black tracking-tight leading-tight">
          Danh Sách Địa Điểm Du Lịch
        </h1>
        <p class="text-xs md:text-sm text-teal-200/80 leading-relaxed font-medium">
          Tìm kiếm các di tích lịch sử, địa điểm văn hóa, khu sinh thái và các làng nghề truyền thống nổi bật tại vùng đồng bằng sông Cửu Long.
        </p>
      </div>
    </div>

    <!-- Filters and Search Controls -->
    <div class="grid grid-cols-1 md:grid-cols-12 gap-4 bg-white p-6 rounded-3xl border border-slate-200/80 shadow-sm items-center">
      <!-- Search input -->
      <div class="relative md:col-span-6">
        <input 
          v-model="searchKeyword"
          type="text" 
          placeholder="Tìm địa điểm, địa chỉ, từ khóa..." 
          class="w-full pl-11 pr-4 py-3 bg-slate-50 border border-slate-200 rounded-2xl text-sm focus:outline-none focus:border-teal-500 focus:bg-white transition duration-200"
          @keyup.enter="handleSearch"
        />
        <span class="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400">
          <SearchIcon class="w-5 h-5 text-slate-400" />
        </span>
      </div>

      <!-- Categories Filter Dropdown -->
      <div class="md:col-span-3">
        <select 
          v-model="selectedCategory"
          class="w-full px-4 py-3 bg-slate-50 border border-slate-200 rounded-2xl text-sm focus:outline-none focus:border-teal-500 focus:bg-white transition duration-200 cursor-pointer appearance-none font-medium text-slate-700"
          @change="handleSearch"
        >
          <option :value="null">Tất cả Danh mục</option>
          <option v-for="cat in categories" :key="cat.id" :value="cat.id">
            {{ cat.name }}
          </option>
        </select>
      </div>

      <!-- Search Trigger Button -->
      <button 
        @click="handleSearch"
        class="premium-btn md:col-span-3 py-3 bg-teal-500 hover:bg-teal-400 text-slate-950 font-black text-sm rounded-2xl shadow-md shadow-teal-500/15"
      >
        Tìm kiếm địa danh
      </button>
    </div>

    <!-- Category Quick Chips list -->
    <div class="flex flex-wrap gap-2 items-center">
      <span class="text-xs font-bold text-slate-400 uppercase mr-3">Lọc nhanh theo danh mục:</span>
      <button 
        @click="selectCategoryChip(null)"
        class="premium-btn px-4 py-2 rounded-full text-xs font-black transition-all border duration-200 shadow-sm"
        :class="[
          selectedCategory === null 
            ? 'bg-teal-600 text-white border-teal-600 shadow-teal-600/10' 
            : 'bg-white text-slate-600 border-slate-200 hover:border-slate-350 hover:bg-slate-55'
        ]"
      >
        Tất cả
      </button>
      <button 
        v-for="cat in categories" 
        :key="cat.id"
        @click="selectCategoryChip(cat.id)"
        class="premium-btn px-4 py-2 rounded-full text-xs font-black transition-all border duration-200 shadow-sm"
        :class="[
          selectedCategory === cat.id 
            ? 'text-white border-transparent' 
            : 'bg-white text-slate-600 border-slate-200 hover:border-slate-350 hover:bg-slate-55'
        ]"
        :style="selectedCategory === cat.id ? { backgroundColor: cat.color || '#0d9488', boxShadow: `0 4px 12px ${cat.color}33` } : {}"
      >
        {{ cat.name }}
      </button>
    </div>

    <!-- Error Alert state -->
    <div v-if="error" class="bg-red-50 border border-red-200 text-red-700 px-6 py-4 rounded-2xl flex items-center space-x-3 shadow-sm">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0 text-red-500" />
      <span class="text-sm font-bold">{{ error }}</span>
    </div>

    <!-- Loading Skeleton Grid -->
    <div v-else-if="loading" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
      <div v-for="i in 6" :key="i" class="bg-white rounded-3xl border border-slate-200/50 p-5 space-y-4 animate-pulse">
        <div class="aspect-[16/10] bg-slate-200 rounded-2xl w-full"></div>
        <div class="space-y-3">
          <div class="h-4 bg-slate-200 rounded w-1/4"></div>
          <div class="h-6 bg-slate-200 rounded w-3/4"></div>
          <div class="h-3 bg-slate-200 rounded w-full"></div>
          <div class="h-10 bg-slate-200 rounded w-full"></div>
        </div>
      </div>
    </div>

    <!-- Empty Results State -->
    <div v-else-if="destinations.length === 0" class="bg-white py-24 text-center rounded-3xl border border-slate-200/80 shadow-sm space-y-5">
      <div class="inline-flex p-5 bg-slate-50 text-slate-400 rounded-full border border-slate-100 shadow-inner">
        <InboxIcon class="w-14 h-14 text-slate-400" />
      </div>
      <div class="space-y-2">
        <h3 class="text-lg font-black text-slate-700">Không tìm thấy địa điểm nào</h3>
        <p class="text-xs text-slate-500 max-w-sm mx-auto leading-relaxed">Vui lòng thay đổi từ khóa tìm kiếm hoặc lọc theo danh mục khác.</p>
      </div>
      <button @click="resetFilters" class="text-xs font-black text-teal-600 hover:text-teal-700 underline underline-offset-4 transition">
        Đặt lại bộ lọc
      </button>
    </div>

    <!-- Destinations Cards Grid List -->
    <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
      <div 
        v-for="dest in destinations" 
        :key="dest.id" 
        class="bg-white rounded-3xl border border-slate-200/80 overflow-hidden shadow-sm hover:shadow-lg hover:border-teal-500/25 group flex flex-col justify-between transition-all duration-300 hover:-translate-y-1.5"
      >
        <!-- Card Cover image container -->
        <div class="aspect-[16/10] bg-slate-100 relative overflow-hidden border-b border-slate-100">
          <img 
            v-if="dest.coverImageUrl" 
            :src="dest.coverImageUrl" 
            class="w-full h-full object-cover group-hover:scale-105 transition-all duration-500"
            loading="lazy" 
            :alt="dest.name"
          />
          <div v-else class="w-full h-full flex items-center justify-center text-slate-400">
            <ImageIcon class="w-12 h-12" />
          </div>

          <!-- Indication badge overlay -->
          <div class="absolute top-4 left-4 flex flex-col gap-1.5 z-10">
            <!-- Category badge -->
            <span 
              class="inline-flex px-3 py-1 rounded-full text-[10px] font-black border uppercase tracking-wider shadow-sm backdrop-blur-md"
              :style="{
                backgroundColor: dest.category?.color ? `${dest.category.color}18` : 'rgba(241, 245, 249, 0.9)',
                borderColor: dest.category?.color ? `${dest.category.color}45` : '#cbd5e1',
                color: dest.category?.color || '#475569'
              }"
            >
              {{ dest.category?.name }}
            </span>
          </div>

          <!-- Badges for features (Tour 360 / Audio Guide) -->
          <div class="absolute bottom-4 right-4 flex items-center gap-1.5 z-10">
            <span 
              v-if="dest.hasVirtualTour"
              class="flex items-center space-x-1 px-2.5 py-1 bg-amber-500 text-slate-950 font-black rounded-lg text-[9px] uppercase tracking-wider shadow-md animate-pulse-gold"
              title="Có tham quan ảo 360°"
            >
              <CompassIcon class="w-3.5 h-3.5" />
              <span>360° Tour</span>
            </span>
            <span 
              v-if="dest.hasAudioGuide"
              class="flex items-center space-x-1 px-2.5 py-1 bg-slate-900/85 backdrop-blur-sm text-teal-400 font-black rounded-lg text-[9px] uppercase tracking-wider shadow-md"
              title="Có audio guide thuyết minh"
            >
              <MusicIcon class="w-3.5 h-3.5 text-teal-400" />
              <span>Audio</span>
            </span>
          </div>
        </div>

        <!-- Card Body details section -->
        <div class="p-6 flex-1 flex flex-col justify-between space-y-5">
          <div class="space-y-2">
            <div class="flex items-center space-x-1 text-[10px] font-bold text-slate-400 uppercase tracking-wider">
              <span>{{ dest.regionName }}</span>
            </div>
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
            class="premium-btn w-full py-3 bg-slate-50 hover:bg-teal-600 border border-slate-200/85 hover:border-teal-600 text-slate-700 hover:text-white font-black text-xs rounded-xl flex items-center justify-center space-x-2"
          >
            <span>Khám phá chi tiết địa danh</span>
            <ArrowRightIcon class="w-3.5 h-3.5" />
          </router-link>
        </div>
      </div>
    </div>

    <!-- Pagination footer -->
    <div 
      v-if="destinations.length > 0 && totalPages > 1" 
      class="bg-white px-6 py-5 rounded-3xl border border-slate-200/80 shadow-sm flex flex-col sm:flex-row justify-between items-center gap-4"
    >
      <span class="text-xs text-slate-500 font-bold">
        Hiển thị trang {{ currentPage }}/{{ totalPages }} (Tổng {{ totalItems }} kết quả)
      </span>
      <div class="flex items-center space-x-3">
        <button 
          @click="prevPage" 
          :disabled="currentPage === 1"
          class="premium-btn px-4 py-2.5 bg-white border border-slate-200 rounded-xl text-xs font-black text-slate-600 hover:bg-slate-55 disabled:opacity-40 disabled:cursor-not-allowed shadow-sm"
        >
          Trang trước
        </button>
        <button 
          @click="nextPage" 
          :disabled="currentPage === totalPages || totalPages === 0"
          class="premium-btn px-4 py-2.5 bg-white border border-slate-200 rounded-xl text-xs font-black text-slate-600 hover:bg-slate-55 disabled:opacity-40 disabled:cursor-not-allowed shadow-sm"
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
