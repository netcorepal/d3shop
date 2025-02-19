import { createRouter, createWebHistory } from 'vue-router';

import { viewRoutes } from '@/views/viewRoutes.ts'

const router = createRouter({
    history: createWebHistory((import.meta as any).env.BASE_URL),
    routes : [
        ...viewRoutes
    ]
})


export default router;