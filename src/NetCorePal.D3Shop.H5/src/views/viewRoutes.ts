import type { RouteRecordRaw } from 'vue-router';

import { loginRoutes } from './login/loginRoutes.ts'

export const viewRoutes: RouteRecordRaw[] = [
    ...loginRoutes
]