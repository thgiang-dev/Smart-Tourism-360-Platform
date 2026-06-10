<template>
  <div class="space-y-16 pb-20">
    <!-- Hero Banner Section -->
    <section class="relative bg-slate-950 text-white py-24 md:py-32 overflow-hidden">
      <!-- Background image of Can Tho river/boats -->
      <div class="absolute inset-0 z-0 opacity-25">
        <img 
          src="https://images.unsplash.com/photo-1507525428034-b723cf961d3e?q=80&w=1600"
          class="w-full h-full object-cover"
          alt="Can Tho background"
        />
      </div>
      <!-- Gradient masks -->
      <div class="absolute inset-0 bg-gradient-to-br from-teal-950 via-teal-900/35 to-slate-950 z-0"></div>
      <div class="absolute inset-0 bg-[radial-gradient(rgba(20,184,166,0.12)_1px,transparent_1px)] [background-size:24px_24px] z-0"></div>
      <!-- Decorative glow spheres -->
      <div class="absolute w-[500px] h-[500px] rounded-full bg-teal-500/15 blur-[120px] -top-40 -right-20 pointer-events-none z-0"></div>
      <div class="absolute w-[350px] h-[350px] rounded-full bg-amber-500/8 blur-[100px] bottom-10 left-10 pointer-events-none z-0"></div>
      
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative z-10 grid grid-cols-1 lg:grid-cols-12 gap-12 items-center">
        <!-- Hero Text (7 Columns) -->
        <div class="lg:col-span-7 space-y-6 text-center lg:text-left reveal-on-scroll">
          <span class="inline-flex items-center space-x-2 px-4 py-2 bg-teal-500/10 border border-teal-500/20 text-teal-400 font-black rounded-full text-[10px] uppercase tracking-wider shadow-inner">
            <span class="w-2 h-2 rounded-full bg-teal-400 animate-ping"></span>
            <span>Hệ thống Du lịch Thông minh Cần Thơ</span>
          </span>
          
          <h1 class="text-4xl md:text-6xl font-black tracking-tight leading-tight">
            Khám Phá Di Sản Số <br />
            Qua <span class="bg-clip-text text-transparent bg-gradient-to-r from-teal-400 via-emerald-400 to-amber-300">Không Gian 360°</span>
          </h1>
          
          <p class="text-xs md:text-sm text-slate-300/95 leading-relaxed max-w-xl mx-auto lg:mx-0">
            Trải nghiệm tham quan thực tế ảo tương tác cao các khu di tích lịch sử, danh lam thắng cảnh, văn hóa nghệ thuật và làng nghề truyền thống trước khi đặt chân tới thực địa.
          </p>

          <div class="flex flex-col sm:flex-row justify-center lg:justify-start gap-4 pt-4">
            <router-link 
              id="btn-explore-map"
              to="/explore" 
              class="premium-btn px-8 py-3.5 bg-teal-500 hover:bg-teal-400 text-slate-950 font-black rounded-2xl shadow-xl shadow-teal-500/25 active:scale-[0.98] transition duration-150 flex items-center justify-center space-x-2"
            >
              <MapIcon class="w-4 h-4" />
              <span>Khám phá bản đồ</span>
            </router-link>
            <router-link 
              to="/destinations" 
              class="premium-btn px-8 py-3.5 bg-slate-900/60 hover:bg-slate-800/80 backdrop-blur-sm border border-slate-700/50 text-white font-bold rounded-2xl active:scale-[0.98] transition duration-150 flex items-center justify-center space-x-2 hover:border-teal-500/30"
            >
              <span>Danh sách địa danh</span>
              <ArrowRightIcon class="w-4 h-4" />
            </router-link>
          </div>
        </div>

        <!-- Right side Hero Visual mockup (5 Columns) -->
        <div class="hidden lg:flex justify-center relative lg:col-span-5 reveal-on-scroll delay-150">
          <!-- Glow behind card -->
          <div class="absolute -inset-1 bg-gradient-to-r from-teal-500 to-amber-500 rounded-3xl blur-xl opacity-30 group-hover:opacity-40 transition duration-500"></div>
          
          <div class="w-full max-w-md aspect-[4/3] bg-slate-900/60 backdrop-blur-xl border border-slate-800 rounded-3xl p-4 shadow-2xl relative overflow-visible group">
            <!-- Base Layer mockup screen -->
            <div class="w-full h-full bg-slate-950 rounded-2xl overflow-hidden relative border border-slate-800 flex items-center justify-center shadow-inner">
              <img 
                src="https://images.unsplash.com/photo-1476514525535-07fb3b4ae5f1?q=80&w=800" 
                class="w-full h-full object-cover opacity-50 group-hover:scale-105 transition duration-700" 
                alt="Can Tho Boat View"
              />
              <!-- Center explore button -->
              <router-link
                to="/explore"
                class="absolute w-16 h-16 bg-teal-500 text-slate-950 rounded-full flex items-center justify-center shadow-lg shadow-teal-500/20 transition duration-300 hover:scale-110 hover:bg-teal-400 group-hover:animate-pulse-glow"
              >
                <PlayIcon class="w-8 h-8 fill-slate-950 translate-x-[2px]" />
              </router-link>
            </div>

            <!-- Floating stats card (Top Left) -->
            <div class="absolute -top-6 -left-6 bg-slate-900/80 backdrop-blur-md border border-slate-800 p-4 rounded-2xl shadow-xl flex items-center space-x-3 transition hover:translate-y-[-2px] duration-300">
              <div class="p-2.5 bg-amber-500/10 text-amber-500 rounded-xl border border-amber-500/20">
                <CompassIcon class="w-5 h-5 text-amber-500 animate-spin-slow" />
              </div>
              <div>
                <span class="text-[9px] font-black text-slate-400 block uppercase tracking-wider">Trải nghiệm</span>
                <span class="text-xs font-black text-white">100+ Điểm 360°</span>
              </div>
            </div>

            <!-- Floating status badge (Bottom Right) -->
            <div class="absolute -bottom-4 -right-4 bg-slate-900/95 backdrop-blur-md border border-slate-800 px-4 py-2.5 rounded-2xl shadow-xl flex items-center space-x-2">
              <span class="w-2.5 h-2.5 rounded-full bg-emerald-500 animate-ping"></span>
              <span class="w-2.5 h-2.5 rounded-full bg-emerald-500 absolute left-4"></span>
              <span class="text-[10px] font-black text-slate-200 uppercase tracking-wider">Hỗ trợ Live Guide</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Categories section -->
    <section class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 space-y-10">
      <div class="text-center space-y-3 max-w-2xl mx-auto reveal-on-scroll">
        <h2 class="text-2xl md:text-3xl font-black text-slate-800 uppercase tracking-tight">Loại hình du lịch nổi bật</h2>
        <div class="w-16 h-1 bg-teal-600 mx-auto rounded-full"></div>
        <p class="text-sm text-slate-500 leading-relaxed">
          Hệ thống phân loại đa dạng giúp du khách dễ dàng định vị các giá trị văn hóa muốn khám phá.
        </p>
      </div>

      <!-- Categories grid loading / dynamic -->
      <div v-if="loadingCategories" class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-7 gap-6">
        <div v-for="i in 7" :key="i" class="h-32 bg-slate-200/60 rounded-2xl animate-pulse"></div>
      </div>
      <div v-else class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-7 gap-6">
        <div 
          v-for="(cat, idx) in categories" 
          :key="cat.id"
          @click="navigateToCategory(cat.id)"
          :class="[
            'bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm hover:border-teal-500/30 hover:shadow-md cursor-pointer transition-all duration-300 flex flex-col items-center justify-center text-center space-y-4 group hover:-translate-y-1 reveal-on-scroll',
            idx === 0 ? 'delay-75' : idx === 1 ? 'delay-150' : idx === 2 ? 'delay-225' : idx === 3 ? 'delay-300' : idx === 4 ? 'delay-375' : idx === 5 ? 'delay-450' : ''
          ]"
        >
          <!-- Color indicator circle -->
          <div 
            class="w-12 h-12 rounded-full flex items-center justify-center text-white shadow-md transition-all duration-300 group-hover:scale-110"
            :style="{ backgroundColor: cat.color || '#0d9488' }"
          >
            <component :is="getCategoryIcon(cat.name)" class="w-6 h-6" />
          </div>
          <span class="text-xs font-black text-slate-700 group-hover:text-teal-700 transition">{{ cat.name }}</span>
        </div>
      </div>
    </section>

    <!-- Featured Destination Grid -->
    <section class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 space-y-10">
      <div class="flex flex-col sm:flex-row justify-between items-start sm:items-end border-b border-slate-200/80 pb-6 gap-4 reveal-on-scroll">
        <div class="space-y-2">
          <h2 class="text-2xl md:text-3xl font-black text-slate-800 uppercase tracking-tight">Địa danh nổi bật</h2>
          <p class="text-sm text-slate-500">Các điểm đến hấp dẫn mới nhất được số hóa không gian.</p>
        </div>
        <router-link 
          to="/destinations" 
          class="premium-btn text-sm font-bold text-teal-600 hover:text-teal-700 flex items-center space-x-1.5 px-4 py-2 bg-teal-50 rounded-xl border border-teal-100/50"
        >
          <span>Xem tất cả</span>
          <ArrowRightIcon class="w-4 h-4" />
        </router-link>
      </div>

      <!-- Featured destinations grids -->
      <div v-if="loadingDestinations" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
        <div v-for="i in 3" :key="i" class="h-96 bg-slate-200/60 rounded-2xl animate-pulse"></div>
      </div>
      <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
        <div 
          v-for="(dest, idx) in featuredDestinations" 
          :key="dest.id"
          :class="[
            'bg-white rounded-2xl border border-slate-200/80 overflow-hidden shadow-sm hover:shadow-lg hover:border-teal-500/25 group flex flex-col justify-between transition-all duration-300 hover:-translate-y-1.5 reveal-on-scroll',
            idx === 0 ? 'delay-75' : idx === 1 ? 'delay-150' : 'delay-225'
          ]"
        >
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

            <!-- Category badge overlay -->
            <div class="absolute top-4 left-4 z-10">
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
            
            <div class="absolute bottom-4 right-4 flex gap-1.5 z-10" v-if="dest.hasVirtualTour">
              <span 
                class="px-2.5 py-1 bg-amber-500 text-slate-950 font-black rounded-lg text-[9px] uppercase tracking-wider shadow-md flex items-center space-x-1 animate-pulse-gold"
              >
                <CompassIcon class="w-3.5 h-3.5" />
                <span>360° Tour</span>
              </span>
            </div>
          </div>

          <!-- Body details -->
          <div class="p-6 flex-grow flex flex-col justify-between space-y-5">
            <div class="space-y-2">
              <div class="flex items-center space-x-1 text-[10px] font-bold text-slate-400 uppercase tracking-wider">
                <MapIcon class="w-3 h-3 text-slate-400" />
                <span>{{ dest.regionName }}</span>
              </div>
              <h3 class="text-lg font-black text-slate-800 group-hover:text-teal-700 transition line-clamp-1">
                {{ dest.name }}
              </h3>
              <p class="text-xs text-slate-500 line-clamp-3 leading-relaxed">
                {{ dest.shortDescription }}
              </p>
            </div>

            <router-link 
              :to="`/destinations/${dest.slug || dest.id}`"
              class="premium-btn w-full py-3 bg-slate-50 hover:bg-teal-600 border border-slate-200/80 hover:border-teal-600 text-slate-700 hover:text-white font-black text-xs rounded-xl flex items-center justify-center space-x-2"
            >
              <span>Xem chi tiết địa danh</span>
              <ArrowRightIcon class="w-3.5 h-3.5" />
            </router-link>
          </div>
        </div>
      </div>
    </section>

    <!-- Experience 360 virtual tour info banner section -->
    <section class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 reveal-on-scroll">
      <div class="bg-gradient-to-r from-teal-850 to-slate-900 text-white rounded-3xl p-8 md:p-16 shadow-xl grid grid-cols-1 md:grid-cols-2 gap-12 items-center relative overflow-hidden">
        <div class="absolute w-[400px] h-[400px] rounded-full bg-teal-500/10 blur-3xl -top-40 -right-40 pointer-events-none"></div>
        <div class="space-y-6 text-center md:text-left">
          <h2 class="text-3xl md:text-4xl font-black uppercase tracking-tight leading-tight">
            Trải nghiệm du lịch <br />
            <span class="text-teal-300">Thực tế ảo 360°</span>
          </h2>
          <p class="text-sm text-slate-300 leading-relaxed max-w-md mx-auto md:mx-0">
            Xem toàn cảnh 360° sắc nét cao các danh lam thắng cảnh, di chuyển tự do giữa các góc nhìn thông qua các điểm liên kết (Hotspot) tương tác và giọng thuyết minh bản địa hấp dẫn.
          </p>
          <div class="pt-2">
            <router-link 
              to="/explore" 
              class="premium-btn inline-flex px-8 py-4 bg-teal-500 hover:bg-teal-400 text-slate-950 font-black rounded-2xl text-sm shadow-lg shadow-teal-500/20"
            >
              Thử trải nghiệm ngay
            </router-link>
          </div>
        </div>

        <div class="flex justify-center">
          <div class="w-full max-w-sm aspect-video bg-slate-950 rounded-2xl overflow-hidden border border-slate-800/80 shadow-2xl relative flex items-center justify-center group">
            <img 
              src="https://images.unsplash.com/photo-1476514525535-07fb3b4ae5f1?q=80&w=600" 
              class="w-full h-full object-cover opacity-50 group-hover:scale-105 transition duration-700" 
              alt="Mekong Boat tour"
            />
            <div class="absolute text-center space-y-3 p-4">
              <CompassIcon class="w-12 h-12 mx-auto text-teal-400 animate-spin-slow" />
              <span class="text-sm font-black uppercase tracking-wider block text-white drop-shadow-md">Khám Phá Toàn Cảnh</span>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, nextTick } from 'vue'
import { useRouter } from 'vue-router'
import api from '../utils/api'
import { 
  Compass as CompassIcon, 
  Map as MapIcon, 
  ArrowRight as ArrowRightIcon,
  Play as PlayIcon,
  Image as ImageIcon,
  Landmark as LandmarkIcon,
  Sparkles as SparklesIcon,
  Leaf as LeafIcon,
  Sprout as SproutIcon,
  Hammer as HammerIcon,
  Utensils as UtensilsIcon,
  Bed as BedIcon
} from 'lucide-vue-next'

const router = useRouter()

const getCategoryIcon = (categoryName) => {
  const name = categoryName ? categoryName.toLowerCase() : ''
  if (name.includes('văn hóa') || name.includes('lịch sử')) return LandmarkIcon
  if (name.includes('tâm linh') || name.includes('chùa')) return SparklesIcon
  if (name.includes('sinh thái') || name.includes('rừng') || name.includes('thiên nhiên')) return LeafIcon
  if (name.includes('nông nghiệp') || name.includes('vườn') || name.includes('đồng')) return SproutIcon
  if (name.includes('làng nghề') || name.includes('thủ công')) return HammerIcon
  if (name.includes('ẩm thực') || name.includes('ăn') || name.includes('uống')) return UtensilsIcon
  if (name.includes('lưu trú') || name.includes('khách sạn') || name.includes('nhà nghỉ')) return BedIcon
  return CompassIcon // fallback
}

const loadingCategories = ref(false)
const loadingDestinations = ref(false)

const categories = ref([])
const featuredDestinations = ref([])

let observer = null

const initScrollObserver = () => {
  if (observer) {
    observer.disconnect()
  }
  
  const options = {
    root: null,
    threshold: 0.05,
    rootMargin: '0px 0px -40px 0px'
  }
  
  observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.classList.add('is-visible')
        observer.unobserve(entry.target)
      }
    })
  }, options)
  
  // Observe all static elements that have reveal class
  const revealElements = document.querySelectorAll('.reveal-on-scroll')
  revealElements.forEach(el => observer.observe(el))
}

const observeDynamicElements = () => {
  requestAnimationFrame(() => {
    requestAnimationFrame(() => {
      if (!observer) return
      const revealElements = document.querySelectorAll('.reveal-on-scroll:not(.is-visible)')
      revealElements.forEach(el => observer.observe(el))
    })
  })
}

const fetchCategories = async () => {
  loadingCategories.value = true
  try {
    const response = await api.get('/api/categories')
    if (response.success) {
      categories.value = response.data
      observeDynamicElements()
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
      observeDynamicElements()
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
  initScrollObserver()
  fetchCategories()
  fetchFeaturedDestinations()
})

onUnmounted(() => {
  if (observer) {
    observer.disconnect()
  }
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
