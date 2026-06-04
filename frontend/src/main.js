import { createApp } from 'vue'
import { createPinia } from 'pinia'
import router from './router'
import 'leaflet/dist/leaflet.css'
import '@photo-sphere-viewer/core/index.css'
import './style.css'
import App from './App.vue'

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.mount('#app')
