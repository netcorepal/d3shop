import type { RouteRecordRaw } from 'vue-router';

export const loginRoutes: RouteRecordRaw[] = [
    {
        path: '/login',
        component: () => import('./login.vue')
    }
]