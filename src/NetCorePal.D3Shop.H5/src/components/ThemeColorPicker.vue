<template>
  <div class="theme-picker">
    <div 
      v-for="color in themeColors" 
      :key="color.value"
      class="color-item"
      :class="{ active: modelValue === color.value }"
      :style="{ backgroundColor: color.value }"
      @click="onChange(color.value)"
    >
      <van-icon v-if="modelValue === color.value" name="success" class="check-icon" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';

const props = defineProps<{
  modelValue: string;
}>();

const emit = defineEmits<{
  (e: 'update:modelValue', value: string): void;
}>();

const themeColors = [
  { value: '#1989fa' },  // 默认蓝色
  { value: '#07c160' },  // 绿色
  { value: '#ee0a24' },  // 红色
  { value: '#ff976a' },  // 橙色
  { value: '#7232dd' },  // 紫色
  { value: '#2c2c2c' },  // 深色
];

const onChange = (value: string) => {
  emit('update:modelValue', value);
};
</script>

<style scoped>
.theme-picker {
  display: flex;
  flex-wrap: wrap;
  gap: 12px;
  padding: 8px 16px;
}

.color-item {
  width: 24px;
  height: 24px;
  border-radius: 50%;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  border: 2px solid transparent;
  transition: all 0.3s;
}

.color-item.active {
  border-color: var(--van-gray-3);
  transform: scale(1.1);
}

.check-icon {
  color: white;
  font-size: 14px;
  filter: drop-shadow(0 0 2px rgba(0, 0, 0, 0.3));
}

@media (prefers-color-scheme: dark) {
  .color-item.active {
    border-color: var(--van-gray-7);
  }
}
</style> 