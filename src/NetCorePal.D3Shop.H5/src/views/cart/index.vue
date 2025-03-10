<template>
  <div class="cart">
    
    <div class="cart-content">
      <template v-if="cartStore.items.length">
        <div class="cart-list">
          <van-swipe-cell v-for="item in cartStore.items" :key="item.id">
            <van-card
              :price="item.price"
              :title="item.title"
              :thumb="item.thumb"
              class="cart-item"
            >
              <template #num>
                <van-stepper
                  v-model="item.quantity"
                  :min="1"
                  :max="99"
                  @change="(value) => cartStore.updateQuantity(item.id, value)"
                />
              </template>
            </van-card>
            <template #right>
              <van-button
                square
                text="delete"
                type="danger"
                class="delete-button"
                @click="cartStore.removeFromCart(item.id)"
              >
                {{ t('cart.delete') }}
              </van-button>
            </template>
          </van-swipe-cell>
        </div>

        <van-submit-bar
          :price="cartStore.totalAmount * 100"
          :button-text="t('cart.submit')"
          @submit="onSubmit"
        >
          <van-button
            type="default"
            size="small"
            @click="cartStore.clearCart"
          >
            {{ t('cart.clear') }}
          </van-button>
        </van-submit-bar>
      </template>

      <van-empty
        v-else
        :description="t('cart.empty')"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import { useCartStore } from '@/store/cart';
import { showToast } from 'vant';

const { t } = useI18n();
const cartStore = useCartStore();

const onSubmit = () => {
  showToast('提交订单');
};
</script>

<style scoped>
.cart {
  min-height: 100vh;
  background: var(--van-background);
  display: flex;
  flex-direction: column;
}

.cart-content {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.cart-list {
  flex: 1;
  padding: 12px;
}

.cart-item {
  margin-bottom: 12px;
  background: var(--van-background-2);
  border-radius: 8px;
}

.delete-button {
  height: 100%;
}

:deep(.van-submit-bar) {
  background: var(--van-background-2);
  border-top: 1px solid var(--van-border-color);
}

:deep(.van-card) {
  background: var(--van-background-2);
  border-radius: 8px;
}

:deep(.van-stepper) {
  margin-top: 8px;
}

:deep(.van-empty) {
  margin-top: 40%;
  background: transparent;
}

@media (prefers-color-scheme: dark) {
  :deep(.van-card),
  :deep(.van-submit-bar) {
    background: var(--van-background-2);
  }
}
</style>