<template>
  <router-view />
  <ToastContainer />
  <ConfirmModal />
</template>

<script setup>
import ToastContainer from './components/ToastContainer.vue'
import ConfirmModal from './components/ConfirmModal.vue'
</script>

<style>
/* Global custom styles if any */
</style>
