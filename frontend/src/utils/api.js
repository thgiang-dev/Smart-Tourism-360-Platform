import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5074',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Request interceptor to inject Bearer token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('admin_token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Response interceptor to handle 401 and parse standardized API Response
api.interceptors.response.use(
  (response) => {
    // Return the custom API response object directly (contains success, message, data, errors)
    return response.data
  },
  (error) => {
    if (error.response) {
      const { status, data } = error.response
      
      // Automatic logout on 401 Unauthorized
      if (status === 401) {
        localStorage.removeItem('admin_token')
        localStorage.removeItem('admin_user')
        // Redirect to login if not already there, using window.location to break import cycles
        if (window.location.pathname !== '/admin/login') {
          window.location.href = '/admin/login'
        }
      }
      
      // If server returned structured error response matching ApiResponse
      if (data && data.success === false) {
        return Promise.reject(data)
      }
    }
    
    // Fallback error format matching ApiResponse
    return Promise.reject({
      success: false,
      message: error.message || 'Có lỗi xảy ra kết nối đến máy chủ.',
      data: null,
      errors: null
    })
  }
)

export default api
