<template>
  <div class="cart">

    <div v-if="cartItems.length > 0">
      <van-checkbox-group v-model="checkedItems">
        <van-swipe-cell v-for="item in cartItems" :key="item.id">
          <van-card :price="item.price" :title="item.title" :thumb="item.thumb">
            <template #tags>
              <van-tag plain type="danger">{{ item.tag }}</van-tag>
            </template>
            <template #num>
              <van-stepper v-model="item.count" />
            </template>
          </van-card>
          <template #right>
            <van-button square text="删除" type="danger" class="delete-button" />
          </template>
        </van-swipe-cell>
      </van-checkbox-group>

      <van-submit-bar :price="totalPrice" button-text="提交订单" @submit="onSubmit">
        <van-checkbox v-model="checkAll">全选</van-checkbox>
      </van-submit-bar>
    </div>

    <van-empty v-else description="购物车空空如也" />
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed } from 'vue';

const cartItems = ref([
  {
    id: 1,
    title: '商品1',
    price: '99.99',
    count: 1,
    thumb: 'https://fastly.jsdelivr.net/npm/@vant/assets/apple-1.jpeg',
    tag: '新品'
  },
  // ... 更多商品
]);

const checkedItems = ref([]);
const checkAll = ref(false);

const totalPrice = computed(() => {
  return cartItems.value.reduce((total, item) => {
    return total + Number(item.price) * item.count;
  }, 0) * 100;
});

const onSubmit = () => {
  // 提交订单逻辑
};
</script>

<style scoped>
.delete-button {
  height: 100%;
}
</style>