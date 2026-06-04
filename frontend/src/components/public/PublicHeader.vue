<template>
  <header class="sticky top-0 z-40 bg-white/80 backdrop-blur-md border-b border-slate-200/50 shadow-sm">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between h-16 items-center">
        <!-- Logo Section -->
        <div class="flex items-center">
          <router-link to="/" class="flex items-center space-x-2 group">
            <div class="w-9 h-9 bg-teal-600 rounded-xl flex items-center justify-center text-white shadow-md shadow-teal-600/10 group-hover:scale-105 transition">
              <CompassIcon class="w-5 h-5" />
            </div>
            <span class="text-lg font-black tracking-wider bg-gradient-to-r from-teal-800 to-slate-900 bg-clip-text text-transparent">
              SMART TOURISM 360
            </span>
          </router-link>
        </div>

        <!-- Desktop Navigation Links -->
        <nav class="hidden md:flex items-center space-x-8 text-sm font-bold">
          <router-link 
            v-for="link in navLinks" 
            :key="link.to" 
            :to="link.to"
            exact-active-class="text-teal-600"
            class="text-slate-600 hover:text-teal-600 transition"
          >
            {{ link.label }}
          </router-link>
        </nav>

        <!-- Right side actions -->
        <div class="hidden md:flex items-center space-x-4">
          <router-link 
            v-if="authStore.isAuthenticated()" 
            to="/admin" 
            class="px-4 py-2 bg-slate-900 hover:bg-slate-800 text-white text-xs font-bold rounded-xl shadow-md transition duration-150"
          >
            Trang Quản trị
          </router-link>
          <router-link 
            v-else 
            to="/admin/login" 
            class="px-4 py-2 border border-slate-200 hover:bg-slate-50 text-slate-700 text-xs font-bold rounded-xl transition duration-150"
          >
            Đăng nhập
          </router-link>
        </div>

        <!-- Mobile Menu Toggle -->
        <div class="flex md:hidden">
          <button 
            @click="isMobileMenuOpen = !isMobileMenuOpen"
            class="p-2 text-slate-500 hover:text-slate-800 focus:outline-none rounded-lg hover:bg-slate-100 transition"
          >
            <MenuIcon v-if="!isMobileMenuOpen" class="w-6 h-6" />
            <XIcon v-else class="w-6 h-6" />
          </button>
        </div>
      </div>
    </div>

    <!-- Mobile Navigation Drawer Overlay -->
    <div 
      v-if="isMobileMenuOpen" 
      @click="isMobileMenuOpen = false"
      class="fixed inset-0 top-16 bg-slate-950/25 backdrop-blur-sm z-30 md:hidden"
    ></div>

    <!-- Mobile Navigation Drawer -->
    <div 
      v-if="isMobileMenuOpen"
      class="absolute top-16 left-0 right-0 bg-white border-b border-slate-200 z-40 md:hidden shadow-lg p-6 space-y-4 animate-in fade-in slide-in-from-top-5 duration-200"
    >
      <nav class="flex flex-col space-y-3 font-bold text-sm">
        <router-link 
          v-for="link in navLinks" 
          :key="link.to" 
          :to="link.to"
          exact-active-class="text-teal-600"
          class="px-3 py-2 text-slate-600 hover:bg-slate-50 rounded-lg transition"
          @click="isMobileMenuOpen = false"
        >
          {{ link.label }}
        </router-link>
      </nav>
      <div class="border-t border-slate-100 pt-4 flex flex-col space-y-2">
        <router-link 
          v-if="authStore.isAuthenticated()" 
          to="/admin" 
          class="w-full text-center py-2.5 bg-slate-900 hover:bg-slate-800 text-white text-xs font-bold rounded-xl transition"
          @click="isMobileMenuOpen = false"
        >
          Trang Quản trị
        </router-link>
        <router-link 
          v-else 
          to="/admin/login" 
          class="w-full text-center py-2.5 border border-slate-200 hover:bg-slate-50 text-slate-700 text-xs font-bold rounded-xl transition"
          @click="isMobileMenuOpen = false"
        >
          Đăng nhập
        </router-link>
      </div>
    </div>
  </header>
</template>

<script setup>
import { ref } from 'vue'
import { useAuthStore } from '../../stores/auth'
import { Compass as CompassIcon, Menu as MenuIcon, X as XIcon } from 'lucide-vue-next'

const authStore = useAuthStore()
const isMobileMenuOpen = ref(false)

const navLinks = [
  { label: 'Trang chủ', to: '/' },
  { label: 'Bản đồ khám phá', to: '/explore' },
  { label: 'Địa điểm du lịch', to: '/destinations' }
]
</script>
