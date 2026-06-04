<template>
  <div class="min-h-screen flex flex-col bg-slate-50 font-sans antialiased text-slate-800">
    <!-- Common Header -->
    <PublicHeader />

    <!-- Main Content Section -->
    <main class="flex-grow">
      <router-view />
    </main>

    <!-- Common Footer -->
    <PublicFooter />
  </div>
</template>

<script setup>
import PublicHeader from './PublicHeader.vue'
import PublicFooter from './PublicFooter.vue'
</script>
