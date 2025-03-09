import { defineStore } from 'pinia';
import type { ConfigProviderTheme } from 'vant';

interface ThemeVars {
  dark: boolean;
  primaryColor: string;
  primaryColorLight: string;
  background: string;
  background2: string;
  textColor: string;
  textColor2: string;
  borderColor: string;
  cardBackground: string;
}

export const useAppStore = defineStore('app', {
  state: () => ({
    themeVars: {
      dark: localStorage.getItem('dark-mode') === null
        ? window.matchMedia('(prefers-color-scheme: dark)').matches
        : localStorage.getItem('dark-mode') === 'true',
      primaryColor: '#1989fa',
      primaryColorLight: 'rgba(25, 137, 250, 0.9)',
      background: '#f7f8fa',
      background2: '#ffffff',
      textColor: '#323233',
      textColor2: '#969799',
      borderColor: '#ebedf0',
      cardBackground: '#ffffff'
    } as ThemeVars,
    followSystem: localStorage.getItem('follow-system') !== 'false'
  }),

  actions: {
    updateThemeVars(vars: Partial<ThemeVars>) {
      this.themeVars = {
        ...this.themeVars,
        ...Object.fromEntries(
          Object.entries(vars).filter(([_, value]) => value !== undefined)
        )
      } as ThemeVars;

      // 更新 CSS 变量
      Object.entries(this.themeVars).forEach(([key, value]) => {
        if (typeof value === 'string') {
          document.documentElement.style.setProperty(`--van-${key.replace(/([A-Z])/g, '-$1').toLowerCase()}`, value);
        }
      });

      // 更新 HTML class
      document.documentElement.classList.toggle('dark', this.themeVars.dark);
      // 更新状态栏颜色
      document.querySelector('meta[name="theme-color"]')?.setAttribute('content', this.themeVars.dark ? '#121212' : '#ffffff');
    },

    setDarkMode(isDark: boolean) {
      this.followSystem = false;
      localStorage.setItem('follow-system', 'false');
      localStorage.setItem('dark-mode', isDark.toString());

      this.updateThemeVars({
        dark: isDark,
        ...(isDark ? {
          background: '#121212',
          background2: '#1e1e1e',
          textColor: '#ffffff',
          textColor2: 'rgba(255, 255, 255, 0.85)',
          borderColor: 'rgba(255, 255, 255, 0.12)',
          cardBackground: '#1e1e1e'
        } : {
          background: '#f7f8fa',
          background2: '#ffffff',
          textColor: '#323233',
          textColor2: '#969799',
          borderColor: '#ebedf0',
          cardBackground: '#ffffff'
        })
      });
    },

    followSystemTheme() {
      this.followSystem = true;
      localStorage.setItem('follow-system', 'true');
      localStorage.removeItem('dark-mode');
      const isDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
      this.updateThemeVars({
        dark: isDark,
        ...(isDark ? {
          background: '#121212',
          background2: '#1e1e1e',
          textColor: '#ffffff',
          textColor2: 'rgba(255, 255, 255, 0.85)',
          borderColor: 'rgba(255, 255, 255, 0.12)',
          cardBackground: '#1e1e1e'
        } : {
          background: '#f7f8fa',
          background2: '#ffffff',
          textColor: '#323233',
          textColor2: '#969799',
          borderColor: '#ebedf0',
          cardBackground: '#ffffff'
        })
      });
    },

    initThemeVars() {
      const darkModeMediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
      const themeColor = localStorage.getItem('theme-color') || '#1989fa';

      // 监听系统深色模式变化
      darkModeMediaQuery.addEventListener('change', (e) => {
        if (this.followSystem) {
          this.updateThemeVars({
            dark: e.matches,
            ...(e.matches ? {
              background: '#121212',
              background2: '#1e1e1e',
              textColor: '#ffffff',
              textColor2: 'rgba(255, 255, 255, 0.85)',
              borderColor: 'rgba(255, 255, 255, 0.12)',
              cardBackground: '#1e1e1e'
            } : {
              background: '#f7f8fa',
              background2: '#ffffff',
              textColor: '#323233',
              textColor2: '#969799',
              borderColor: '#ebedf0',
              cardBackground: '#ffffff'
            })
          });
        }
      });

      // 初始化主题
      const isDark = this.themeVars.dark;
      this.updateThemeVars({
        dark: isDark,
        primaryColor: themeColor,
        primaryColorLight: this.adjustColor(themeColor, 0.9),
        ...(isDark ? {
          background: '#121212',
          background2: '#1e1e1e',
          textColor: '#ffffff',
          textColor2: 'rgba(255, 255, 255, 0.85)',
          borderColor: 'rgba(255, 255, 255, 0.12)',
          cardBackground: '#1e1e1e'
        } : {
          background: '#f7f8fa',
          background2: '#ffffff',
          textColor: '#323233',
          textColor2: '#969799',
          borderColor: '#ebedf0',
          cardBackground: '#ffffff'
        })
      });
    },

    // 辅助函数：调整颜色透明度
    adjustColor(color: string, opacity: number): string {
      if (color.startsWith('#')) {
        const r = parseInt(color.slice(1, 3), 16);
        const g = parseInt(color.slice(3, 5), 16);
        const b = parseInt(color.slice(5, 7), 16);
        return `rgba(${r}, ${g}, ${b}, ${opacity})`;
      }
      return color;
    }
  }
});