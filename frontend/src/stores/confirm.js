import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useConfirmStore = defineStore('confirm', () => {
  const isOpen = ref(false)
  const title = ref('')
  const message = ref('')
  const resolvePromise = ref(null)

  const show = (options = {}) => {
    title.value = options.title || 'Xác nhận'
    message.value = options.message || 'Bạn có chắc chắn muốn thực hiện hành động này?'
    isOpen.value = true

    return new Promise((resolve) => {
      resolvePromise.value = resolve
    })
  }

  const confirm = () => {
    isOpen.value = false
    if (resolvePromise.value) resolvePromise.value(true)
  }

  const cancel = () => {
    isOpen.value = false
    if (resolvePromise.value) resolvePromise.value(false)
  }

  return {
    isOpen,
    title,
    message,
    show,
    confirm,
    cancel
  }
})
