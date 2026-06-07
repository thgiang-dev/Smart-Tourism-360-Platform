<template>
  <Transition name="fade">
    <div 
      v-if="confirmStore.isOpen" 
      class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-slate-950/60 backdrop-blur-sm"
      @click.self="confirmStore.cancel"
    >
      <div 
        class="bg-white border border-slate-100 rounded-3xl w-full max-w-sm shadow-2xl p-6 relative overflow-hidden animate-zoom-in"
      >
        <div class="flex flex-col items-center text-center space-y-4">
          <!-- Icon Circle -->
          <div class="w-12 h-12 rounded-2xl bg-amber-50 border border-amber-200 text-amber-500 flex items-center justify-center">
            <AlertTriangleIcon class="w-6 h-6" />
          </div>

          <div class="space-y-1.5 px-2">
            <h3 class="text-sm font-bold text-slate-800 uppercase tracking-wide">{{ confirmStore.title }}</h3>
            <p class="text-xs text-slate-500 leading-relaxed">{{ confirmStore.message }}</p>
          </div>

          <!-- Buttons -->
          <div class="flex space-x-3 w-full pt-2">
            <button 
              @click="confirmStore.cancel"
              class="flex-1 py-2.5 bg-slate-50 border border-slate-200 hover:bg-slate-100 text-slate-700 font-bold text-xs rounded-xl transition active:scale-[0.98]"
            >
              Hủy bỏ
            </button>
            <button 
              @click="confirmStore.confirm"
              class="flex-1 py-2.5 bg-red-500 hover:bg-red-600 text-white font-bold text-xs rounded-xl transition shadow-lg shadow-red-500/10 active:scale-[0.98]"
            >
              Xác nhận
            </button>
          </div>
        </div>
      </div>
    </div>
  </Transition>
</template>

<script setup>
import { useConfirmStore } from '../stores/confirm'
import { AlertTriangle as AlertTriangleIcon } from 'lucide-vue-next'

const confirmStore = useConfirmStore()
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

.animate-zoom-in {
  animation: zoomIn 0.25s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}

@keyframes zoomIn {
  from {
    opacity: 0;
    transform: scale(0.95);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}
</style>
