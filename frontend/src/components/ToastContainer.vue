<template>
  <div class="fixed top-4 right-4 z-50 pointer-events-none flex flex-col space-y-3 max-w-md w-full sm:w-96">
    <TransitionGroup name="toast">
      <div 
        v-for="toast in toastStore.toasts" 
        :key="toast.id"
        class="pointer-events-auto flex items-start p-4 bg-slate-900/95 backdrop-blur-md border rounded-2xl shadow-xl transition-all duration-300"
        :class="borderClass(toast.type)"
      >
        <!-- Icon -->
        <component 
          :is="getIcon(toast.type)" 
          class="w-5 h-5 mr-3 flex-shrink-0 mt-0.5" 
          :class="iconColorClass(toast.type)"
        />

        <!-- Message -->
        <div class="flex-grow">
          <p class="text-xs font-semibold text-slate-100 leading-relaxed">{{ toast.message }}</p>
        </div>

        <!-- Close Button -->
        <button 
          @click="toastStore.remove(toast.id)"
          class="ml-4 text-slate-400 hover:text-slate-200 transition"
        >
          <XIcon class="w-4 h-4" />
        </button>
      </div>
    </TransitionGroup>
  </div>
</template>

<script setup>
import { useToastStore } from '../stores/toast'
import { 
  CheckCircle as CheckCircleIcon,
  AlertCircle as AlertCircleIcon,
  AlertTriangle as AlertTriangleIcon,
  Info as InfoIcon,
  X as XIcon 
} from 'lucide-vue-next'

const toastStore = useToastStore()

const getIcon = (type) => {
  switch (type) {
    case 'success': return CheckCircleIcon
    case 'error': return AlertCircleIcon
    case 'warning': return AlertTriangleIcon
    default: return InfoIcon
  }
}

const borderClass = (type) => {
  switch (type) {
    case 'success': return 'border-emerald-500/20 shadow-emerald-500/[0.03]'
    case 'error': return 'border-red-500/20 shadow-red-500/[0.03]'
    case 'warning': return 'border-amber-500/20 shadow-amber-500/[0.03]'
    default: return 'border-sky-500/20 shadow-sky-500/[0.03]'
  }
}

const iconColorClass = (type) => {
  switch (type) {
    case 'success': return 'text-emerald-400'
    case 'error': return 'text-red-400'
    case 'warning': return 'text-amber-400'
    default: return 'text-sky-400'
  }
}
</script>

<style scoped>
.toast-enter-from {
  opacity: 0;
  transform: translateY(-20px) scale(0.9);
}
.toast-enter-to {
  opacity: 1;
  transform: translateY(0) scale(1);
}
.toast-leave-from {
  opacity: 1;
  transform: scale(1);
}
.toast-leave-to {
  opacity: 0;
  transform: scale(0.9);
}
</style>
