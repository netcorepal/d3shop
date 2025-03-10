<template>
  <ConfigProvider
    :theme="appStore.themeVars.dark ? 'dark' : 'light'"
    :theme-vars="{
      primaryColor: appStore.themeVars.primaryColor,
      primaryColorLight: appStore.themeVars.primaryColorLight,
      background: appStore.themeVars.background,
      background2: appStore.themeVars.background2,
      textColor: appStore.themeVars.textColor,
      textColor2: appStore.themeVars.textColor2,
      borderColor: appStore.themeVars.borderColor,
      cardBackground: appStore.themeVars.cardBackground
    }"
  >
    <router-view></router-view>
  </ConfigProvider>
</template>

<script setup lang="ts">
import { useAppStore } from '@/store/app';
import { ConfigProvider } from 'vant';

const appStore = useAppStore();

</script>

<style>
:deep(.van-search__content) {
  border: none;
}

:deep(.van-field__left-icon::after) {
  display: none;
}
</style>