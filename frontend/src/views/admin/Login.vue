<template>
  <div class="min-h-screen bg-slate-950 flex items-center justify-center p-4 relative overflow-hidden font-sans">
    <!-- Visual Gradient Circles background -->
    <div class="absolute w-80 h-80 rounded-full bg-teal-500/10 blur-[80px] -top-20 -left-20"></div>
    <div class="absolute w-80 h-80 rounded-full bg-indigo-500/10 blur-[80px] -bottom-20 -right-20"></div>

    <div class="max-w-md w-full bg-slate-900/60 backdrop-blur-md rounded-2xl shadow-2xl border border-slate-800/80 p-8 z-10">
      <div class="text-center mb-8">
        <div class="inline-flex p-3 rounded-2xl bg-teal-500/10 border border-teal-500/20 text-teal-400 mb-4 animate-pulse">
          <LockIcon class="w-8 h-8" />
        </div>
        <span class="text-3xl font-black tracking-wider text-teal-400 block">SMART TOURISM 360</span>
        <h2 class="text-lg font-bold text-slate-200 mt-2">Đăng nhập Quản trị viên</h2>
      </div>

      <form @submit.prevent="handleLogin" class="space-y-6">
        <!-- Error Alert -->
        <div 
          v-if="authStore.error" 
          class="flex items-start space-x-3 p-4 bg-red-950/20 border border-red-500/30 text-red-400 rounded-xl text-sm transition-all duration-300"
        >
          <AlertCircleIcon class="w-5 h-5 flex-shrink-0 mt-0.5" />
          <span>{{ authStore.error }}</span>
        </div>

        <!-- Email Field -->
        <div>
          <label class="block text-xs font-semibold text-slate-400 uppercase tracking-wider mb-2">Email</label>
          <div class="relative">
            <input 
              v-model="email"
              type="email" 
              required
              placeholder="admin@smarttourism360.vn" 
              class="w-full pl-11 pr-4 py-3 bg-slate-950/60 border border-slate-800 focus:border-teal-500/80 rounded-xl focus:outline-none text-slate-200 text-sm placeholder-slate-600 transition"
              :disabled="authStore.loading"
            />
            <span class="absolute left-4 top-1/2 -translate-y-1/2 text-slate-600">
              <MailIcon class="w-5 h-5" />
            </span>
          </div>
        </div>

        <!-- Password Field -->
        <div>
          <label class="block text-xs font-semibold text-slate-400 uppercase tracking-wider mb-2">Mật khẩu</label>
          <div class="relative">
            <input 
              v-model="password"
              type="password" 
              required
              placeholder="••••••••" 
              class="w-full pl-11 pr-4 py-3 bg-slate-950/60 border border-slate-800 focus:border-teal-500/80 rounded-xl focus:outline-none text-slate-200 text-sm placeholder-slate-600 transition"
              :disabled="authStore.loading"
            />
            <span class="absolute left-4 top-1/2 -translate-y-1/2 text-slate-600">
              <KeyIcon class="w-5 h-5" />
            </span>
          </div>
        </div>

        <!-- Submit Button -->
        <button 
          type="submit" 
          class="w-full py-3 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold rounded-xl shadow-lg hover:shadow-teal-500/20 active:scale-[0.98] transition duration-200 flex items-center justify-center space-x-2 disabled:opacity-50 disabled:cursor-not-allowed disabled:active:scale-100"
          :disabled="authStore.loading"
        >
          <Loader2Icon v-if="authStore.loading" class="w-5 h-5 animate-spin" />
          <span>{{ authStore.loading ? 'Đang xử lý...' : 'Đăng nhập' }}</span>
        </button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/auth'
import { Lock as LockIcon, Mail as MailIcon, Key as KeyIcon, AlertCircle as AlertCircleIcon, Loader2 as Loader2Icon } from 'lucide-vue-next'

const router = useRouter()
const authStore = useAuthStore()

const email = ref('')
const password = ref('')

const handleLogin = async () => {
  if (!email.value || !password.value) return
  
  const success = await authStore.login(email.value, password.value)
  if (success) {
    router.push('/admin')
  }
}
</script>

<style scoped>
/* Focus borders ring shadow */
input:focus {
  box-shadow: 0 0 0 2px rgba(20, 184, 166, 0.1);
}
</style>
