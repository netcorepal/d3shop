<template>
  <div class="layout-container" :style="containerStyle">
    <nav-bar v-if="route.meta.showNavBar" :title="route.meta.title as string || ''" left-arrow
      @click-left="onClickLeft" />

    <div class="content-container">
      <router-view></router-view>
    </div>

    <van-tabbar v-model="active" route>
      <van-tabbar-item icon="home-o" to="/home">{{ t('layout.home') }}</van-tabbar-item>
      <van-tabbar-item icon="apps-o" to="/category">{{ t('layout.category') }}</van-tabbar-item>
      <van-tabbar-item icon="shopping-cart-o" to="/cart">{{ t('layout.cart') }}</van-tabbar-item>
      <van-tabbar-item icon="user-o" to="/profile">{{ t('layout.profile') }}</van-tabbar-item>
    </van-tabbar>

  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { NavBar } from 'vant';
import { useRouter, useRoute } from 'vue-router';
import { useI18n } from 'vue-i18n';

const { t } = useI18n();
const router = useRouter();
const route = useRoute();
const active = ref(0);

const containerStyle = computed(() => {
  const navBarHeight = route.meta.showNavBar ? '46px' : '0px';
  const tabBarHeight = '50px';
  return {
    '--nav-bar-height': navBarHeight,
    '--tab-bar-height': tabBarHeight,
  };
});

const onClickLeft = () => {
  router.back();
};
</script>

<style scoped>
.layout-container {
  height: 100vh;
  display: flex;
  flex-direction: column;
  background-color: var(--van-background);
}

.content-container {
  flex: 1;
  height: calc(100vh - var(--nav-bar-height) - var(--tab-bar-height));
  overflow-y: auto;
  background-color: var(--van-background-2);
}

:deep(.van-nav-bar) {
  flex-shrink: 0;
}

:deep(.van-tabbar) {
  flex-shrink: 0;
}

@media (prefers-color-scheme: dark) {
  .layout-container {
    background-color: var(--van-background-2);
  }
  
  .content-container {
    background-color: var(--van-background);
  }
}
</style>