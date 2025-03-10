<template>
  <van-dropdown-menu class="language-switcher">
    <van-dropdown-item v-model="currentLang" :options="options" @change="handleLanguageChange" />
  </van-dropdown-menu>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { setLanguage } from '@/i18n'

const { locale } = useI18n()
const currentLang = ref(locale.value)

const options = [
  { text: '简体中文', value: 'zh-CN' },
  { text: 'English', value: 'en-US' },
]

const handleLanguageChange = (value: string) => {
  setLanguage(value as 'zh-CN' | 'en-US')
}

onMounted(() => {
  currentLang.value = locale.value
})
</script>

<style scoped>
.language-switcher {
  --van-dropdown-menu-height: 24px;
  --van-dropdown-menu-background: transparent;
  --van-dropdown-menu-box-shadow: none;
  --van-dropdown-menu-title-font-size: var(--font-size-sm);
  --van-dropdown-menu-title-text-color: var(--text-color-2);
}

:deep(.van-dropdown-menu__bar) {
  height: 24px;
  background: transparent;
}

:deep(.van-dropdown-menu__title) {
  padding: 0;
  line-height: 24px;
}

:deep(.van-dropdown-menu__title::after) {
  display: none;
}

:deep(.van-cell) {
  align-items: center;
}

:deep(.van-dropdown-item__content) {
  max-height: 400px;
}

:deep(.van-dropdown-item__option) {
  padding: 12px 16px;
}

:deep(.van-dropdown-item__option--active) {
  color: var(--van-primary-color);
}

@media (prefers-color-scheme: dark) {
  :deep(.van-dropdown-item__content) {
    background-color: var(--van-background);
  }

  :deep(.van-dropdown-item__option) {
    color: var(--van-text-color);
  }

  :deep(.van-dropdown-item__option--active) {
    background-color: var(--van-background-2);
  }
}
</style> 