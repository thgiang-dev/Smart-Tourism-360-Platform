<template>
  <div class="space-y-16 pb-20">
    <!-- Hero Banner Section -->
    <section class="relative bg-gradient-to-br from-teal-900 via-slate-900 to-slate-950 text-white py-24 md:py-32 overflow-hidden">
      <!-- Decorative background glow spheres -->
      <div class="absolute w-[450px] h-[450px] rounded-full bg-teal-600/10 blur-3xl -top-32 -right-16 z-0"></div>
      <div class="absolute w-80 h-80 rounded-full bg-amber-500/5 blur-3xl bottom-10 left-10 z-0"></div>
      
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative z-10 grid grid-cols-1 lg:grid-cols-2 gap-12 items-center">
        <!-- Hero Text -->
        <div class="space-y-6 text-center lg:text-left">
          <span class="inline-flex items-center space-x-1.5 px-3.5 py-1.5 bg-teal-500/10 border border-teal-500/20 text-teal-400 font-bold rounded-full text-xs uppercase tracking-wider">
            <CompassIcon class="w-4 h-4 animate-spin-slow" />
            <span>Nền tảng Du lịch Thông minh Cần Thơ</span>
          </span>
          
          <h1 class="text-4xl md:text-6xl font-black tracking-tight leading-tight">
            Khám Phá Di Sản Số <br />
            Qua <span class="text-teal-400">Không Gian 360°</span>
          </h1>
          
          <p class="text-xs md:text-sm text-slate-300 leading-relaxed max-w-xl mx-auto lg:mx-0">
            Trải nghiệm tham quan thực tế ảo tương tác cao các khu di tích lịch sử, danh lam thắng cảnh, văn hóa nghệ thuật và làng nghề truyền thống trước khi đặt chân tới thực địa.
          </p>

          <div class="flex flex-col sm:flex-row justify-center lg:justify-start gap-4 pt-4">
            <router-link 
              id="btn-explore-map"
              to="/explore" 
              class="px-8 py-3.5 bg-teal-500 hover:bg-teal-400 text-slate-950 font-black rounded-2xl shadow-xl shadow-teal-500/10 active:scale-[0.98] transition duration-150 flex items-center justify-center space-x-2"
            >
              <MapIcon class="w-4 h-4" />
              <span>Khám phá bản đồ</span>
            </router-link>
            <router-link 
              to="/destinations" 
              class="px-8 py-3.5 bg-slate-900/60 hover:bg-slate-800/80 backdrop-blur-sm border border-slate-700/50 text-white font-bold rounded-2xl active:scale-[0.98] transition duration-150 flex items-center justify-center space-x-2"
            >
              <span>Danh sách địa danh</span>
              <ArrowRightIcon class="w-4 h-4" />
            </router-link>
          </div>
        </div>

        <!-- Right side Hero Visual representation -->
        <div class="hidden lg:flex justify-center relative">
          <!-- Floating UI mockup -->
          <div class="w-full max-w-md aspect-[4/3] bg-slate-800/40 backdrop-blur-md border border-slate-700/40 rounded-3xl p-4 shadow-2xl relative overflow-hidden group">
            <div class="absolute inset-0 bg-gradient-to-tr from-teal-500/10 to-transparent"></div>
            <div class="w-full h-full bg-slate-950 rounded-2xl overflow-hidden relative border border-slate-800/80 flex items-center justify-center">
              <img 
                src="https://images.unsplash.com/photo-1540959733332-eab4deceeaf7?q=80&w=600" 
                class="w-full h-full object-cover opacity-60 group-hover:scale-102 transition duration-700" 
              />
              <div class="absolute inset-0 flex flex-col items-center justify-center space-y-3 p-6 text-center">
                <div class="w-16 h-16 bg-teal-500/10 text-teal-400 border border-teal-500/20 rounded-full flex items-center justify-center backdrop-blur-sm">
                  <PlayIcon class="w-8 h-8 translate-x-[2px]" />
                </div>
                <h4 class="text-sm font-extrabold tracking-wider uppercase">Chùa Ông Cần Thơ 360°</h4>
                <p class="text-[10px] text-slate-400 font-medium max-w-xs">Bấm khám phá bản đồ để trải nghiệm trọn vẹn không gian tương tác ảo.</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Categories section -->
    <section class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 space-y-8">
      <div class="text-center space-y-2 max-w-2xl mx-auto">
        <h2 class="text-xl md:text-2xl font-black text-slate-800 uppercase tracking-tight">Loại hình du lịch nổi bật</h2>
        <p class="text-xs md:text-sm text-slate-500">
          Hệ thống phân loại đa dạng giúp du khách dễ dàng định vị các giá trị văn hóa muốn khám phá.
        </p>
      </div>

      <!-- Categories grid loading / dynamic -->
      <div v-if="loadingCategories" class="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-7 gap-4 animate-pulse">
        <div v-for="i in 7" :key="i" class="h-28 bg-slate-100 rounded-2xl"></div>
      </div>
      <div v-else class="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-7 gap-4">
        <div 
          v-for="cat in categories" 
          :key="cat.id"
          @click="navigateToCategory(cat.id)"
          class="bg-white p-5 rounded-2xl border border-slate-200/60 shadow-sm hover:border-teal-500/40 hover:shadow-md cursor-pointer transition flex flex-col items-center justify-center text-center space-y-3 group"
        >
          <!-- Color indicator circle -->
          <div 
            class="w-10 h-10 rounded-full flex items-center justify-center text-white shadow-sm transition group-hover:scale-105"
            :style="{ backgroundColor: cat.color || '#0d9488' }"
          >
            <CompassIcon class="w-5 h-5" />
          </div>
          <span class="text-xs font-bold text-slate-700 group-hover:text-teal-700 transition">{{ cat.name }}</span>
        </div>
      </div>
    </section>

    <!-- Featured Destination Grid -->
    <section class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 space-y-8">
      <div class="flex justify-between items-end border-b border-slate-200/60 pb-4">
        <div class="space-y-1">
          <h2 class="text-xl md:text-2xl font-black text-slate-800 uppercase tracking-tight">Địa danh nổi bật</h2>
          <p class="text-xs text-slate-500">Các điểm đến hấp dẫn mới nhất được số hóa không gian.</p>
        </div>
        <router-link 
          to="/destinations" 
          class="text-xs font-bold text-teal-600 hover:text-teal-700 flex items-center space-x-1"
        >
          <span>Xem tất cả</span>
          <ArrowRightIcon class="w-3.5 h-3.5" />
        </router-link>
      </div>

      <!-- Featured destinations grids -->
      <div v-if="loadingDestinations" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 animate-pulse">
        <div v-for="i in 3" :key="i" class="h-80 bg-slate-100 rounded-2xl"></div>
      </div>
      <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        <div 
          v-for="dest in featuredDestinations" 
          :key="dest.id"
          class="bg-white rounded-2xl border border-slate-200/60 overflow-hidden shadow-sm hover:shadow-md hover:border-teal-500/20 group flex flex-col justify-between transition"
        >
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

            <!-- Badges -->
            <div class="absolute top-3 left-3 z-10">
              <span 
                class="inline-flex px-2 py-0.5 rounded-md text-[9px] font-bold border uppercase tracking-wider"
                :style="{
                  backgroundColor: dest.category?.color ? `${dest.category.color}15` : '#f1f5f9',
                  borderColor: dest.category?.color ? `${dest.category.color}35` : '#cbd5e1',
                  color: dest.category?.color || '#475569'
                }"
              >
                {{ dest.category?.name }}
              </span>
            </div>
            
            <div class="absolute bottom-3 right-3 flex gap-1 z-10">
              <span 
                v-if="dest.hasVirtualTour" 
                class="px-1.5 py-0.5 bg-slate-900/80 text-teal-400 font-bold rounded text-[8px] uppercase tracking-wider"
              >
                360° Pano
              </span>
            </div>
          </div>

          <!-- Body details -->
          <div class="p-5 flex-grow flex flex-col justify-between space-y-4">
            <div class="space-y-1">
              <span class="text-[9px] font-bold text-slate-400 block uppercase tracking-wider">{{ dest.regionName }}</span>
              <h3 class="text-sm font-extrabold text-slate-800 group-hover:text-teal-700 transition line-clamp-1">
                {{ dest.name }}
              </h3>
              <p class="text-xs text-slate-500 line-clamp-3 leading-relaxed">
                {{ dest.shortDescription }}
              </p>
            </div>

            <router-link 
              :to="`/destinations/${dest.slug || dest.id}`"
              class="w-full py-2 bg-slate-50 hover:bg-teal-50 border border-slate-200/80 hover:border-teal-500/20 text-slate-700 hover:text-teal-700 font-bold text-xs rounded-xl transition flex items-center justify-center space-x-1"
            >
              <span>Xem chi tiết</span>
              <ArrowRightIcon class="w-3 h-3" />
            </router-link>
          </div>
        </div>
      </div>
    </section>

    <!-- Experience 360 virtual tour info banner section -->
    <section class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="bg-gradient-to-r from-teal-800 to-slate-900 text-white rounded-3xl p-8 md:p-12 shadow-xl grid grid-cols-1 md:grid-cols-2 gap-8 items-center relative overflow-hidden">
        <div class="absolute w-64 h-64 rounded-full bg-teal-500/10 blur-3xl -top-20 -right-20"></div>
        <div class="space-y-5 text-center md:text-left">
          <h2 class="text-2xl md:text-3xl font-black uppercase tracking-tight">Trải nghiệm du lịch thực tế ảo 360°</h2>
          <p class="text-xs md:text-sm text-teal-200/80 leading-relaxed max-w-md mx-auto md:mx-0">
            Xem toàn cảnh 360° sắc nét cao các danh lam thắng cảnh, di chuyển tự do giữa các góc nhìn thông qua các điểm liên kết (Hotspot) tương tác và giọng thuyết minh bản địa hấp dẫn.
          </p>
          <div class="pt-2">
            <router-link 
              to="/explore" 
              class="inline-flex px-6 py-3 bg-teal-500 hover:bg-teal-400 text-slate-950 font-black rounded-xl text-xs shadow-md transition"
            >
              Thử trải nghiệm ngay
            </router-link>
          </div>
        </div>

        <div class="flex justify-center">
          <div class="w-full max-w-sm aspect-video bg-slate-950 rounded-2xl overflow-hidden border border-slate-700/30 shadow-2xl relative flex items-center justify-center">
            <img 
              src="https://images.unsplash.com/photo-1476514525535-07fb3b4ae5f1?q=80&w=600" 
              class="w-full h-full object-cover opacity-50" 
            />
            <div class="absolute text-center space-y-2 p-4">
              <CompassIcon class="w-10 h-10 mx-auto text-teal-400 animate-spin-slow" />
              <span class="text-xs font-bold uppercase tracking-wider block">Tham quan tương tác ảo 360°</span>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '../utils/api'
import { 
  Compass as CompassIcon, 
  Map as MapIcon, 
  ArrowRight as ArrowRightIcon,
  Play as PlayIcon,
  Image as ImageIcon 
} from 'lucide-vue-next'

const router = useRouter()

const loadingCategories = ref(false)
const loadingDestinations = ref(false)

const categories = ref([])
const featuredDestinations = ref([])

const fetchCategories = async () => {
  loadingCategories.value = true
  try {
    const response = await api.get('/api/categories')
    if (response.success) {
      categories.value = response.data
    }
  } catch (err) {
    console.error('Không thể tải danh mục.', err)
  } finally {
    loadingCategories.value = false
  }
}

const fetchFeaturedDestinations = async () => {
  loadingDestinations.value = true
  try {
    const response = await api.get('/api/destinations', {
      params: {
        page: 1,
        pageSize: 3
      }
    })
    if (response.success) {
      featuredDestinations.value = response.data.items
    }
  } catch (err) {
    console.error('Không thể tải địa danh nổi bật.', err)
  } finally {
    loadingDestinations.value = false
  }
}

const navigateToCategory = (catId) => {
  router.push({
    path: '/destinations',
    query: { category: catId }
  })
}

onMounted(() => {
  fetchCategories()
  fetchFeaturedDestinations()
})
</script>

<style scoped>
.animate-spin-slow {
  animation: spin 10s linear infinite;
}
@keyframes spin {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}
</style>
