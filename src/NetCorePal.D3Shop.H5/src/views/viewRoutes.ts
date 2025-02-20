import type { RouteRecordRaw } from 'vue-router';

import { loginRoutes } from '@/views/login/loginRoutes'

export const viewRoutes: RouteRecordRaw[] = [
    ...loginRoutes
]