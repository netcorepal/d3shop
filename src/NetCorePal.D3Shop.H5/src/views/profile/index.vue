<template>
  <div class="profile">
    <div class="user-info">
      <van-image
        round
        width="80"
        height="80"
        :src="userInfo.avatar || 'https://fastly.jsdelivr.net/npm/@vant/assets/cat.jpeg'"
      />
      <div v-if="authStore.isAuthenticated" class="user-detail">
        <h3>{{ userInfo.nickname }}</h3>
        <p>{{ userInfo.phone }}</p>
      </div>
      <van-button v-else type="primary" size="small" to="/login">
        立即登录
      </van-button>
    </div>

    <van-cell-group>
      <van-cell title="我的订单" is-link to="/orders" />
      <van-grid :column-num="4">
        <van-grid-item icon="pending-payment" text="待付款" v-auth />
        <van-grid-item icon="logistics" text="待收货" v-auth />
        <van-grid-item icon="comment-o" text="待评价" v-auth />
        <van-grid-item icon="after-sale" text="退换/售后" v-auth />
      </van-grid>
    </van-cell-group>

    <van-cell-group>
      <van-cell title="收货地址" is-link to="/address" v-auth />
      <van-cell title="优惠券" is-link to="/coupons" v-auth />
      <van-cell title="我的收藏" is-link to="/favorites" v-auth />
    </van-cell-group>

    <van-cell-group>
      <van-cell title="设置" is-link to="/settings" />
      <van-cell v-if="authStore.isAuthenticated" title="退出登录" @click="onLogout" />
    </van-cell-group>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { useAuthStore } from '@/store/auth';
import { showDialog } from 'vant';

const authStore = useAuthStore();

const userInfo = reactive({
  nickname: '用户昵称',
  phone: '138****8888',
  avatar: ''
});

const onLogout = () => {
  showDialog({
    title: '提示',
    message: '确认退出登录？',
    showCancelButton: true,
  }).then(() => {
    authStore.logout();
  });
};
</script>

<style scoped>
.user-info {
  padding: 20px;
  text-align: center;
  background: var(--van-primary-color);
  color: white;
}

.user-detail {
  margin-top: 10px;
}

.user-detail h3 {
  margin: 0;
  font-size: 18px;
}

.user-detail p {
  margin: 5px 0 0;
  font-size: 14px;
  opacity: 0.8;
}
</style>