import { createI18n } from 'vue-i18n'
import zhCN from '../locales/zh-CN'
import enUS from '../locales/en-US'

const messages = {
  'zh-CN': zhCN,
  'en-US': enUS,
}

// 获取浏览器语言设置
const getBrowserLanguage = () => {
  const language = navigator.language
  if (language.includes('zh')) {
    return 'zh-CN'
  }
  return 'en-US'
}

// 获取本地存储的语言设置
const getStoredLanguage = () => {
  return localStorage.getItem('language') || getBrowserLanguage()
}

const i18n = createI18n({
  legacy: false, // 使用 Composition API 模式
  locale: getStoredLanguage(),
  fallbackLocale: 'en-US',
  messages,
})

// 提供切换语言的方法
export const setLanguage = (lang: 'zh-CN' | 'en-US') => {
  i18n.global.locale.value = lang
  localStorage.setItem('language', lang)
  document.querySelector('html')?.setAttribute('lang', lang)
}

export default i18n 