import { createRouter, createWebHistory } from 'vue-router';
import type { RouteRecordRaw } from 'vue-router';
import DefaultLayout from '@/layouts/DefaultLayout.vue';
import { useAuthStore } from '@/store/auth';

const routes: RouteRecordRaw[] = [
    {
        path: '/login',
        component: () => import('@/views/login/login.vue'),
        meta: { requiresAuth: false, useLayout: false }
    },
    {
        path: '/',
        component: DefaultLayout,
        children: [
            {
                path: 'home',
                component: () => import('@/views/home/index.vue'),
                meta: { requiresAuth: false, showNavBar: false, title: "首页" }
            },
            {
                path: 'category',
                component: () => import('@/views/category/index.vue'),
                meta: { requiresAuth: false, showNavBar: false, title: "分类" }
            },
            {
                path: 'cart',
                component: () => import('@/views/cart/index.vue'),
                meta: { requiresAuth: false, showNavBar: true, title: "购物车" }
            },
            {
                path: 'profile',
                component: () => import('@/views/profile/index.vue'),
                meta: { requiresAuth: false, showNavBar: false, title: "我的" }
            }
        ]
    },
    {
        path: '/',
        redirect: '/home'
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

// 路由守卫
router.beforeEach((to, from, next) => {
    const authStore = useAuthStore();

    if (to.meta.requiresAuth && !authStore.isAuthenticated) {
        next({
            path: '/login',
            query: { redirect: to.fullPath }
        });
    } else {
        next();
    }
});

export default router;