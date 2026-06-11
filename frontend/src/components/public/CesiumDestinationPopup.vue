<template>
  <div 
    v-if="destination"
    class="absolute z-30 bg-white rounded-2xl shadow-2xl border border-slate-200/60 p-3 w-64 select-none transition-all duration-150 pointer-events-auto"
    :style="{
      left: `${position.x}px`,
      top: `${position.y}px`,
      transform: 'translate(-50%, -100%) translateY(-15px)'
    }"
  >
    <!-- Card Content -->
    <div class="space-y-3 relative">
      <!-- Close Button -->
      <button 
        @click="$emit('close')" 
        class="absolute -top-1 -right-1 p-1 bg-slate-100 hover:bg-slate-200 rounded-full text-slate-400 hover:text-slate-700 transition z-10"
      >
        <svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" stroke-width="2.5" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>

      <!-- Thumbnail Cover Image -->
      <div v-if="destination.coverImageUrl" class="relative overflow-hidden rounded-xl border border-slate-100 shadow-sm aspect-[16/10]">
        <img 
          :src="destination.coverImageUrl" 
          class="w-full h-full object-cover" 
          alt="Destination cover"
        />
        <span 
          v-if="destination.hasVirtualTour" 
          class="absolute top-2 right-2 px-1.5 py-0.5 bg-amber-500 text-slate-950 font-black rounded text-[8px] uppercase tracking-wider shadow"
        >
          360°
        </span>
      </div>

      <!-- Text details -->
      <div class="space-y-1">
        <span 
          class="inline-flex px-2 py-0.5 rounded-full text-[8px] font-black uppercase tracking-wider"
          :style="{
            backgroundColor: `${destination.category?.color || '#0F766E'}15`,
            color: destination.category?.color || '#0F766E',
            border: `1px solid ${destination.category?.color || '#0F766E'}35`
          }"
        >
          {{ destination.category?.name || 'Địa danh' }}
        </span>
        <h4 class="font-extrabold text-xs text-slate-800 leading-snug truncate">{{ destination.name }}</h4>
        <p class="text-[9px] text-slate-400 font-bold uppercase tracking-wider">{{ destination.regionName }}</p>
      </div>

      <!-- Description if exists -->
      <p v-if="destination.shortDescription" class="text-[10px] text-slate-500 line-clamp-2 leading-relaxed">
        {{ destination.shortDescription }}
      </p>

      <!-- Actions CTA -->
      <div class="flex gap-2 pt-2 border-t border-slate-100 w-full">
        <!-- Fly To -->
        <button 
          @click="$emit('fly-to')" 
          class="flex-1 py-1.5 bg-slate-50 hover:bg-slate-100 text-slate-700 text-[10px] font-black text-center rounded-lg border border-slate-200 transition duration-150"
        >
          Bay tới
        </button>

        <!-- Details -->
        <router-link 
          :to="`/destinations/${destination.slug || destination.id}`" 
          class="flex-1 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-800 text-[10px] font-black text-center rounded-lg transition duration-150"
        >
          Chi tiết
        </router-link>
        
        <!-- Virtual Tour 360 -->
        <router-link 
          v-if="destination.hasVirtualTour"
          :to="`/destinations/${destination.id}/tour`" 
          class="flex-1 py-1.5 bg-teal-600 hover:bg-teal-500 text-white text-[10px] font-black text-center rounded-lg shadow-sm transition duration-150 flex items-center justify-center"
        >
          Vào 360°
        </router-link>
      </div>
    </div>

    <!-- Bubble Caret pointing down -->
    <div class="absolute bottom-0 left-1/2 -translate-x-1/2 translate-y-full w-0 h-0 border-x-8 border-x-transparent border-t-8 border-t-white"></div>
  </div>
</template>

<script setup>
defineProps({
  destination: {
    type: Object,
    default: null
  },
  position: {
    type: Object,
    default: () => ({ x: 0, y: 0 })
  }
})

defineEmits(['close', 'fly-to'])
</script>

<style scoped>
/* Keep tooltip styled cleanly and responsive */
</style>
