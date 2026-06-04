import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '../utils/api'

export const useAuthStore = defineStore('auth', () => {
  const user = ref(null)
  const token = ref(null)
  const loading = ref(false)
  const error = ref(null)

  const login = async (email, password) => {
    loading.value = true
    error.value = null
    try {
      const response = await api.post('/api/auth/login', { email, password })
      if (response.success) {
        token.value = response.data.accessToken
        user.value = response.data.user
        
        localStorage.setItem('admin_token', token.value)
        localStorage.setItem('admin_user', JSON.stringify(user.value))
        return true
      }
      error.value = response.message
      return false
    } catch (err) {
      // If error is returned from server with specific validation details
      if (err.errors && err.errors.length > 0) {
        error.value = err.errors.map(e => e.message).join(', ')
      } else {
        error.value = err.message || 'Đăng nhập thất bại.'
      }
      return false
    } finally {
      loading.value = false
    }
  }

  const logout = () => {
    user.value = null
    token.value = null
    localStorage.removeItem('admin_token')
    localStorage.removeItem('admin_user')
  }

  const fetchCurrentUser = async () => {
    try {
      const response = await api.get('/api/auth/me')
      if (response.success) {
        user.value = response.data
        localStorage.setItem('admin_user', JSON.stringify(user.value))
      }
    } catch (err) {
      logout()
    }
  }

  const initializeAuth = () => {
    const savedToken = localStorage.getItem('admin_token')
    const savedUser = localStorage.getItem('admin_user')
    if (savedToken && savedUser) {
      token.value = savedToken
      user.value = JSON.parse(savedUser)
    }
  }

  const isAuthenticated = () => {
    return !!token.value
  }

  return {
    user,
    token,
    loading,
    error,
    login,
    logout,
    fetchCurrentUser,
    initializeAuth,
    isAuthenticated
  }
})
