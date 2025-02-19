<template>
  <div class="category">
    <van-search placeholder="请输入搜索关键词" />

    <div class="category-layout">
      <div class="sidebar-wrapper">
        <van-cell-group>
          <van-cell
            v-for="(item, index) in categories"
            :key="item.id"
            :title="item.name"
            :class="{ active: activeCategory === index }"
            @click="activeCategory = index"
          />
        </van-cell-group>
      </div>

      <div class="category-content">
        <van-space direction="vertical" fill>
          <van-image
            v-for="item in subCategories"
            :key="item.id"
            :src="item.image"
            fit="cover"
          >
            <template #loading>
              <van-loading type="spinner" size="20" />
            </template>
          </van-image>
        </van-space>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';

const activeCategory = ref(0);

const categories = reactive([
  { id: 1, name: '手机数码' },
  { id: 2, name: '家用电器' },
  { id: 3, name: '电脑办公' },
  { id: 4, name: '家居家装' },
  { id: 5, name: '服饰鞋包' },
]);

const subCategories = reactive([
  { id: 1, name: '手机', image: 'https://fastly.jsdelivr.net/npm/@vant/assets/apple-1.jpeg' },
  { id: 2, name: '平板', image: 'https://fastly.jsdelivr.net/npm/@vant/assets/apple-2.jpeg' },
]);
</script>

<style scoped>
.category-layout {
  display: flex;
  height: calc(100vh - 54px);
}

.sidebar-wrapper {
  width: 85px;
  overflow-y: auto;
  background: var(--van-background-2);
}

.category-content {
  flex: 1;
  padding: 12px;
  overflow-y: auto;
}

.active {
  color: var(--van-primary-color);
  background: var(--van-background);
  font-weight: bold;
}

:deep(.van-image) {
  width: 100%;
  height: 100px;
}
</style>