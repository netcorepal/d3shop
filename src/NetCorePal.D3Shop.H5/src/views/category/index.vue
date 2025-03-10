<template>
  <div class="category">
    <van-search
      :placeholder="t('category.searchPlaceholder')"
      shape="round"
      background="transparent"
      class="category-search"
    />

    <div class="category-layout">
      <div class="sidebar-wrapper">
        <van-sidebar v-model="activeCategory">
          <van-sidebar-item
            v-for="item in categories"
            :key="item.id"
            :title="item.name"
          />
        </van-sidebar>
      </div>

      <div class="category-content">
        <van-grid :column-num="2" :border="false" :gutter="10">
          <van-grid-item v-for="item in subCategories" :key="item.id">
            <van-image :src="item.image" class="category-image">
              <template #loading>
                <van-loading type="spinner" size="20" />
              </template>
            </van-image>
            <span class="category-name">{{ item.name }}</span>
          </van-grid-item>
        </van-grid>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import { useI18n } from 'vue-i18n';

const { t } = useI18n();
const activeCategory = ref(0);

const categories = reactive([
  { id: 1, name: t('category.digital') },
  { id: 2, name: t('category.appliance') },
  { id: 3, name: t('category.computer') },
  { id: 4, name: t('category.furniture') },
  { id: 5, name: t('category.clothing') },
]);

const subCategories = reactive([
  { id: 1, name: t('category.phone'), image: 'https://fastly.jsdelivr.net/npm/@vant/assets/apple-1.jpeg' },
  { id: 2, name: t('category.tablet'), image: 'https://fastly.jsdelivr.net/npm/@vant/assets/apple-2.jpeg' },
]);
</script>

<style scoped>
.category {
  height: 100%;
  background: var(--van-background);
  display: flex;
  flex-direction: column;
}

.category-search {
  flex-shrink: 0;
}

.category-layout {
  flex: 1;
  display: flex;
  overflow: hidden;
}

.sidebar-wrapper {
  width: 85px;
  overflow-y: auto;
  flex-shrink: 0;
  background: var(--van-background);
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.03);
}

.category-content {
  flex: 1;
  padding: 12px;
  overflow-y: auto;
  background: var(--van-background);
}

:deep(.van-sidebar-item) {
  background: var(--van-background);
  border: none;
}

:deep(.van-sidebar-item--select) {
  background: var(--van-background);
  border-color: var(--van-primary-color);
}

:deep(.van-grid) {
  border: none;
  background: none;
}

:deep(.van-grid-item) {
  border: none;
  background: none;
}

:deep(.van-grid-item__content) {
  background: var(--van-background);
  padding: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.03);
  border: none;
}

:deep(.van-grid-item__content::after) {
  border: none;
}

.category-image {
  width: 100%;
  height: 100px;
}

.category-name {
  margin-top: 8px;
  font-size: 14px;
  color: var(--van-text-color);
  display: block;
  text-align: center;
}

@media (prefers-color-scheme: dark) {
  :deep(.van-search) {
    background: var(--van-background);
  }

  :deep(.van-search__content) {
    background: var(--van-gray-8);
  }

  .sidebar-wrapper,
  :deep(.van-grid-item__content) {
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
  }
}
</style>