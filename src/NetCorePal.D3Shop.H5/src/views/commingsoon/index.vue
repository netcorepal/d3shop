<template>
  <div class="coming-soon">
    <h1>功能正在开发中</h1>
    <p>敬请期待！</p>
  </div>
</template>

<script setup lang="ts">
// 这里可以添加任何需要的逻辑
</script>

<style scoped>
.coming-soon {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: var(--van-background-2);
  color: var(--van-text-color);
}

h1 {
  font-size: 24px;
  margin-bottom: 16px;
}

p {
  font-size: 16px;
}
</style>
