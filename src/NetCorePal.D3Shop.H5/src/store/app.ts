import { defineStore } from 'pinia';
import type { ConfigProviderTheme } from 'vant';

function getSystemThemeColor(): ConfigProviderTheme {
  // 检查是否支持 color-scheme-dark
  if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
    return 'dark'; // 深色模式默认主题色
  }
  return 'light'; // 浅色模式默认主题色
}

export const useAppStore = defineStore('app', {
  state: () => ({
    themeVars: {
      themeColor: localStorage.getItem('theme') as ConfigProviderTheme || getSystemThemeColor() as ConfigProviderTheme,
    }
  }),

  actions: {
    setThemeColor(color: ConfigProviderTheme) {
      this.themeVars.themeColor = color;
      localStorage.setItem('themeColor', color);
    },



    initThemeColor() {
      // 监听系统主题变化
      if (window.matchMedia) {
        const darkMediaQuery = window.matchMedia('(prefers-color-scheme: dark)');

        // 初始化主题
        if (!localStorage.getItem('theme')) {
          this.themeVars.themeColor = (darkMediaQuery.matches ? 'dark' : 'light') as ConfigProviderTheme;
        }

        darkMediaQuery.addEventListener('change', (e) => {
          if (!localStorage.getItem('theme')) {
            this.themeVars.themeColor = e.matches ? 'dark' : 'light';
          }
        });
      }
    },
  },
});