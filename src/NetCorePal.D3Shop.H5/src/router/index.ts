import { createRouter, createWebHistory } from 'vue-router';
import type { RouteRecordRaw } from 'vue-router';
import DefaultLayout from '@/layouts/DefaultLayout.vue';
import { useAuthStore } from '@/store/auth';

import i18n from '@/i18n';
const { t } = i18n.global;
const routes: RouteRecordRaw[] = [
    {
        path: '/login',
        component: () => import('@/views/login/login.vue'),
        meta: { requiresAuth: false, useLayout: false }
    },
    {
        path: '/register',
        component: () => import('@/views/register/register.vue'),
        meta: { requiresAuth: false, useLayout: false }
    },
    {
        path: '/',
        component: DefaultLayout,
        children: [
            {
                path: 'home',
                component: () => import('@/views/home/index.vue'),
                meta: { requiresAuth: false, showNavBar: false, title: t('home.title') }
            },
            {
                path: 'category',
                component: () => import('@/views/category/index.vue'),
                meta: { requiresAuth: false, showNavBar: false, title: t('category.title') }
            },
            {
                path: 'cart',
                component: () => import('@/views/cart/index.vue'),
                meta: { requiresAuth: true, showNavBar: true, title: t('cart.title') }
            },
            {
                path: 'profile',
                component: () => import('@/views/profile/index.vue'),
                meta: { requiresAuth: false, showNavBar: false, title: t('profile.title') }
            },
            {
                path: 'settings',
                component: () => import('@/views/settings/index.vue'),
                meta: { requiresAuth: false, showNavBar: false, title: t('settings.title') }
            },
            {
                path: 'address',
                component: () => import('@/views/address/index.vue'),
                meta: { requiresAuth: true, showNavBar: true, title: t('address.title') }
            },
            {
                path: '/coming-soon',
                name: 'ComingSoon',
                component: () => import('@/views/commingsoon/index.vue'),
                meta: { requiresAuth: false, showNavBar: true, title: t('comingsoon.title') }
            }
        ]
    },
    {
        path: '/',
        redirect: '/home'
    },

    {
        path: '/:pathMatch(.*)*',
        redirect: '/coming-soon'
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

// 路由守卫
router.beforeEach((to, from, next) => {
    const authStore = useAuthStore();

    if (to.meta.requiresAuth) {
        console.log(authStore.isAuthenticated, 'authStore.isAuthenticated')
        if (!authStore.isAuthenticated && authStore.refreshToken) {
            console.log('refreshToken', authStore.refreshToken)
            authStore.tokenRefresh();
        }
        if (!authStore.isAuthenticated) {
            next({
                path: '/login',
                query: { redirect: to.fullPath }
            });
        }
        else {
            next();
        }
    } else {
        next();
    }
});

export default router;