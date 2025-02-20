<template>
  <div class="home">
    <van-nav-bar title="首页" />
    
    <!-- 搜索栏 -->
    <van-search placeholder="请输入搜索关键词" />
    
    <!-- 轮播图 -->
    <van-swipe class="banner" :autoplay="3000" indicator-color="white">
      <van-swipe-item v-for="(item, index) in banners" :key="index">
        <img :src="item.image" />
      </van-swipe-item>
    </van-swipe>

    <!-- 导航菜单 -->
    <van-grid :column-num="5">
      <van-grid-item v-for="(item, index) in menus" :key="index" :icon="item.icon" :text="item.text" />
    </van-grid>

    <!-- 商品列表 -->
    <van-pull-refresh v-model="refreshing" @refresh="onRefresh">
      <van-list v-model:loading="loading" :finished="finished" @load="onLoad">
        <van-card
          v-for="item in goods"
          :key="item.id"
          :price="item.price"
          :title="item.title"
          :thumb="item.thumb"
        >
          <!-- <template #tags>
            <van-tag plain type="danger">新品</van-tag>
          </template> -->
          <template #footer>
            <van-button size="mini" v-auth>加入购物车</van-button>
          </template>
        </van-card>
      </van-list>
    </van-pull-refresh>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';

const banners = reactive([
  { image: 'https://fastly.jsdelivr.net/npm/@vant/assets/apple-1.jpeg' },
  { image: 'https://fastly.jsdelivr.net/npm/@vant/assets/apple-2.jpeg' }
]);

const menus = reactive([
  { text: '新品', icon: 'new-o' },
  { text: '热卖', icon: 'fire-o' },
  { text: '促销', icon: 'discount' },
  { text: '限时', icon: 'clock-o' },
  { text: '推荐', icon: 'like-o' }
]);

const goods = ref([] as any[]);
const loading = ref(false);
const finished = ref(false);
const refreshing = ref(false);

const onLoad = () => {
  // 如果已经完成或正在刷新，则不再加载
  if (finished.value || refreshing.value) {
    return;
  }
  
  loading.value = true; // 设置加载状态
  
  // 模拟加载数据
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
</script>

<style scoped>
.banner {
  height: 200px;
}
.banner img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}
</style>