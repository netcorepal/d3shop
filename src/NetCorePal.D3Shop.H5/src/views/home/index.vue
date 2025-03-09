<template>
  <div class="home">
    <!-- 固定导航栏 -->
    <van-nav-bar :title="t('home.title')" class="home-nav" />
    
    <!-- 可滚动的内容区域 -->
    <div class="scrollable-content" ref="scrollRef">
      <!-- 搜索栏 -->
      <van-search
        :placeholder="t('home.searchPlaceholder')"
        shape="round"
        background="transparent"
        class="home-search"
      />
      
      <!-- 轮播图 -->
      <van-swipe class="banner" :autoplay="3000" indicator-color="white">
        <van-swipe-item v-for="(item, index) in banners" :key="index">
          <van-image :src="item.image" class="banner-image" />
        </van-swipe-item>
      </van-swipe>

      <!-- 导航菜单 -->
      <div class="menu-container" :class="{ 'is-fixed': isMenuFixed }">
        <van-grid :column-num="5" :border="false" class="menu-grid">
          <van-grid-item
            v-for="(item, index) in menus"
            :key="index"
            :icon="item.icon"
            :text="item.text"
          />
        </van-grid>
      </div>

      <!-- 商品列表 -->
      <van-pull-refresh v-model="refreshing" @refresh="onRefresh" class="goods-list">
        <van-list
          v-model:loading="loading"
          :finished="finished"
          finished-text="没有更多了"
          @load="onLoad"
        >
          <van-card
            v-for="item in goods"
            :key="item.id"
            :price="item.price"
            :title="item.title"
            :thumb="item.thumb"
            class="goods-card"
          >
            <template #footer>
              <van-button 
                size="mini" 
                type="primary" 
                v-auth 
                @click="addToCart(item)"
              >
                {{ t('home.addToCart') }}
              </van-button>
            </template>
          </van-card>
        </van-list>
      </van-pull-refresh>
    </div>

    <!-- 返回顶部按钮 -->
    <van-back-top 
      :right="16" 
      :bottom="80" 
      :target="scrollRef"
      :immediate="false"
      :duration="300"
      :offset="300"
      :z-index="100"
      class="custom-back-top"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, onUnmounted, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useCartStore } from '@/store/cart';
import { showToast } from 'vant';

const { t } = useI18n();
const cartStore = useCartStore();
const scrollRef = ref<HTMLElement | null>(null);
const isMenuFixed = ref(false);
const menuOffsetTop = ref(0);

const banners = reactive([
  { image: 'https://fastly.jsdelivr.net/npm/@vant/assets/apple-1.jpeg' },
  { image: 'https://fastly.jsdelivr.net/npm/@vant/assets/apple-2.jpeg' }
]);

const menus = reactive([
  { text: t('home.menu.new'), icon: 'new-o' },
  { text: t('home.menu.hot'), icon: 'fire-o' },
  { text: t('home.menu.promotion'), icon: 'discount' },
  { text: t('home.menu.limited'), icon: 'clock-o' },
  { text: t('home.menu.recommend'), icon: 'like-o' }
]);

const goods = ref([] as any[]);
const loading = ref(false);
const finished = ref(false);
const refreshing = ref(false);

const onLoad = () => {
  if (finished.value || refreshing.value) {
    return;
  }
  
  loading.value = true;
  
  setTimeout(() => {
    const items = Array(10).fill(null).map((_, index) => ({
      id: goods.value.length + index,
      title: `商品${goods.value.length + index + 1}`,
      price: '99.99',
      thumb: 'https://fastly.jsdelivr.net/npm/@vant/assets/apple-1.jpeg'
    }));
    goods.value.push(...items);
    loading.value = false;
    
    if (goods.value.length >= 40) {
      finished.value = true;
    }
  }, 1000);
};

const onRefresh = () => {
  finished.value = false;
  goods.value = [];
  onLoad();
  refreshing.value = false;
};

// 监听滚动事件
const handleScroll = () => {
  if (!scrollRef.value) return;
  
  const scrollTop = scrollRef.value.scrollTop;

  if (menuOffsetTop.value === 0) {
    // 获取菜单距离顶部的距离（只需获取一次）
    const menuElement = document.querySelector('.menu-container');
    if (menuElement) {
      menuOffsetTop.value = menuElement.getBoundingClientRect().top + scrollTop;
    }
  }

  isMenuFixed.value = scrollTop > menuOffsetTop.value;
};

// 添加到购物车
const addToCart = (item: any) => {
  cartStore.addToCart({
    id: item.id,
    title: item.title,
    price: item.price,
    thumb: item.thumb
  });
  showToast(t('home.addedToCart'));
};

onMounted(() => {
  if (scrollRef.value) {
    scrollRef.value.addEventListener('scroll', handleScroll);
  }
});

watch(scrollRef, (newVal) => {
  if (newVal) {
    newVal.addEventListener('scroll', handleScroll);
  }
});

onUnmounted(() => {
  if (scrollRef.value) {
    scrollRef.value.removeEventListener('scroll', handleScroll);
  }
});
</script>

<style scoped>
.home {
  height: 100vh;
  display: flex;
  flex-direction: column;
  background: var(--van-background);
  overflow: hidden;
}

.home-nav {
  flex-shrink: 0;
}

.home-search {
  padding: 8px 12px;
}

.scrollable-content {
  flex: 1;
  overflow-y: auto;
  position: relative;
}

.menu-container {
  position: relative;
  z-index: 2;
  transition: transform 0.3s;
}

.menu-container.is-fixed {
  position: sticky;
  top: 0;
}

.banner {
  height: 180px;
  margin: 12px;
}

.banner-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.menu-grid {
  margin: 12px;
}

:deep(.van-grid-item__content) {
  background: var(--van-background);
  padding: 16px 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.03);
  border: none;
}

:deep(.van-grid-item__text) {
  color: var(--van-text-color);
  font-size: 12px;
  margin-top: 8px;
}

:deep(.van-grid-item__icon) {
  color: var(--van-primary-color);
  font-size: 24px;
}

.goods-list {
  flex: 1;
  padding: 0 12px;
}

.goods-card {
  margin: 8px 0;
}

:deep(.van-card) {
  background: var(--van-background);
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.03);
  border: none;
}

:deep(.van-grid) {
  border: none;
  background: none;
}

:deep(.van-grid-item) {
  border: none;
  background: none;
}

:deep(.van-grid-item__content::after) {
  border: none;
}

:deep(.van-card__header),
:deep(.van-card__body) {
  border: none;
}

:deep(.van-card__title) {
  color: var(--van-text-color);
}

:deep(.van-card__price) {
  color: var(--van-danger-color);
}

:deep(.custom-back-top) {
  --van-back-top-size: 40px;
  --van-back-top-icon-size: 20px;
  --van-back-top-text-color: var(--van-primary-color);
  --van-back-top-background: var(--van-background);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
}

:deep(.van-search__content) {
  background: var(--van-background-2);
}

:deep(.van-search__content) {
  border: none;
}

:deep(.van-field__left-icon::after) {
  display: none;
}

@media (prefers-color-scheme: dark) {
  :deep(.van-search) {
    background: transparent;
  }

  :deep(.van-search__content) {
    background: var(--van-background-2);
  }

  :deep(.van-grid-item__content),
  :deep(.van-card) {
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
  }
}
</style>