import { createRouter, createWebHistory } from 'vue-router'
import TasksPage from '@/views/TasksPage.vue'

const routes = [
  { path: '/', name: 'tasks', component: TasksPage },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

export default router
