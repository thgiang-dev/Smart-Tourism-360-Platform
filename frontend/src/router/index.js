import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import Explore from '../views/Explore.vue'
import DestinationDetail from '../views/DestinationDetail.vue'
import PublicDestinations from '../views/PublicDestinations.vue'
import TourViewer from '../views/TourViewer.vue'
import Login from '../views/admin/Login.vue'
import Dashboard from '../views/admin/Dashboard.vue'
import Destinations from '../views/admin/Destinations.vue'
import DestinationForm from '../views/admin/DestinationForm.vue'
import AdminLayout from '../components/admin/AdminLayout.vue'
import PublicLayout from '../components/public/PublicLayout.vue'
import Media from '../views/admin/Media.vue'
import VirtualTours from '../views/admin/VirtualTours.vue'
import Panoramas from '../views/admin/Panoramas.vue'
import HotspotEditor from '../views/admin/HotspotEditor.vue'
import { useAuthStore } from '../stores/auth'

const routes = [
  {
    path: '/',
    component: PublicLayout,
    children: [
      {
        path: '',
        name: 'Home',
        component: Home
      },
      {
        path: 'destinations',
        name: 'PublicDestinations',
        component: PublicDestinations
      },
      {
        path: 'destinations/:id',
        name: 'DestinationDetail',
        component: DestinationDetail
      }
    ]
  },
  {
    path: '/explore',
    name: 'Explore',
    component: Explore
  },
  {
    path: '/destinations/:id/tour',
    name: 'TourViewer',
    component: TourViewer
  },
  {
    path: '/admin/login',
    name: 'AdminLogin',
    component: Login,
    meta: { requiresGuest: true }
  },
  {
    path: '/admin',
    component: AdminLayout,
    meta: { requiresAuth: true },
    children: [
      {
        path: '',
        name: 'AdminDashboard',
        component: Dashboard
      },
      {
        path: 'destinations',
        name: 'AdminDestinations',
        component: Destinations
      },
      {
        path: 'destinations/new',
        name: 'AdminCreateDestination',
        component: DestinationForm
      },
      {
        path: 'destinations/:id/edit',
        name: 'AdminEditDestination',
        component: DestinationForm,
        props: true
      },
      {
        path: 'media',
        name: 'AdminMedia',
        component: Media
      },
      {
        path: 'virtual-tours',
        name: 'AdminVirtualTours',
        component: VirtualTours
      },
      {
        path: 'virtual-tours/:id/panoramas',
        name: 'AdminPanoramas',
        component: Panoramas,
        props: true
      },
      {
        path: 'panoramas/:id/hotspots',
        name: 'AdminHotspots',
        component: HotspotEditor,
        props: true
      }
    ]
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  
  // Ensure Pinia state is initialized from localStorage before guard check
  authStore.initializeAuth()

  const isAuth = authStore.isAuthenticated()

  if (to.meta.requiresAuth && !isAuth) {
    next('/admin/login')
  } else if (to.meta.requiresGuest && isAuth) {
    next('/admin')
  } else {
    next()
  }
})

export default router
