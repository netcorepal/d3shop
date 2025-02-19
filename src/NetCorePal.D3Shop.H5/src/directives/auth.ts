import type { Directive, App } from 'vue';
import type { Router } from 'vue-router';

let router: Router;
let authStore: any;

export const vAuth: Directive = {
  mounted(el, binding) {
    el.addEventListener('click', (e: Event) => {
      if (!authStore.isAuthenticated) {
        e.stopPropagation();
        e.preventDefault();
        
        const currentPath = window.location.pathname;
        router.push({
          path: '/login',
          query: { redirect: currentPath }
        });
      }
    });
  }
};

export function setupAuth(app: App, routerInstance: Router, store: any) {
  router = routerInstance;
  authStore = store;
  app.directive('auth', vAuth);
}