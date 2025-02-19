<template>
  <ConfigProvider :theme="appStore.themeVars.themeColor">
    <router-view></router-view>
  </ConfigProvider>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useAppStore } from '@/store/app';
import { ConfigProvider } from 'vant';

const appStore = useAppStore();

onMounted(() => {
  appStore.initThemeColor();
});
</script>

<style></style>