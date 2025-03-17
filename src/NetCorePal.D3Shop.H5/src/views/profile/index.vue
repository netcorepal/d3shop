<template>
  <div class="profile">
    <div class="user-info">
      <van-image
        round
        width="80"
        height="80"
        class="avatar"
        :src="userInfo.avatar || 'https://fastly.jsdelivr.net/npm/@vant/assets/cat.jpeg'"
      />
      <div v-if="authStore.isAuthenticated" class="user-detail">
        <h3>{{ userInfo.nickname }}</h3>
        <p>{{ userInfo.phone }}</p>
      </div>
      <van-button v-else type="primary" size="small" to="/login" class="login-btn">
        {{ t('profile.loginNow') }}
      </van-button>
    </div>

    <div class="profile-content">
      <van-cell-group class="order-group" :border="false">
        <van-cell :title="t('profile.myOrders')" is-link to="/orders" />
        <van-grid :border="false" :column-num="4" class="order-grid">
          <van-grid-item
            v-for="(item, index) in orderMenus"
            :key="index"
            :icon="item.icon"
            :text="item.text"
            v-auth
            class="order-grid-item"
          />
        </van-grid>
      </van-cell-group>

      <van-cell-group class="service-group" :border="false">
        <van-cell :title="t('profile.address')" is-link to="/address" v-auth />
        <van-cell :title="t('profile.coupons')" is-link to="/coupons" v-auth />
        <van-cell :title="t('profile.favorites')" is-link to="/favorites" v-auth />
      </van-cell-group>

      <van-cell-group class="settings-group" :border="false">
        <van-cell :title="t('profile.settings')" is-link to="/settings" />
        <van-cell
          v-if="authStore.isAuthenticated"
          :title="t('profile.logout')"
          @click="onLogout"
          class="logout-cell"
        />
      </van-cell-group>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { useAuthStore } from '@/store/auth';
import { showDialog } from 'vant';
import { useI18n } from 'vue-i18n';
import { useAppStore } from '@/store/app';
import { useRouter } from 'vue-router';


const { t } = useI18n();
const authStore = useAuthStore();
const appStore = useAppStore();
const router = useRouter();

const userInfo = reactive({
  nickname: authStore.userInfo?.username,
  phone: authStore.userInfo?.phone,
  avatar: authStore.userInfo?.avatar || 'https://fastly.jsdelivr.net/npm/@vant/assets/cat.jpeg'
});

const orderMenus = [
  { icon: 'pending-payment', text: t('profile.orderStatus.pending') },
  { icon: 'logistics', text: t('profile.orderStatus.shipping') },
  { icon: 'comment-o', text: t('profile.orderStatus.comment') },
  { icon: 'after-sale', text: t('profile.orderStatus.afterSale') }
];

const onLogout = () => {
  showDialog({
    title: t('profile.logoutConfirm.title'),
    message: t('profile.logoutConfirm.message'),
    showCancelButton: true,
    cancelButtonText: t('profile.logoutConfirm.cancel'),
    confirmButtonText: t('profile.logoutConfirm.confirm'),
    confirmButtonColor: appStore.themeVars.primaryColor,
  }).then(() => {
    authStore.logout();
    router.go(-1);
  });
};
</script>

<style scoped>
.profile {
  min-height: 100%;
  background: var(--van-background-2);
}

.user-info {
  position: relative;
  padding: 24px;
  text-align: center;
  background: var(--van-primary-color);
  color: white;
}

.avatar {
  display: block;
  margin: 0 auto;
  border: 2px solid rgba(255, 255, 255, 0.6);
}

.user-detail {
  margin-top: 12px;
}

.user-detail h3 {
  margin: 0;
  font-size: 18px;
  font-weight: 500;
}

.user-detail p {
  margin: 6px 0 0;
  font-size: 14px;
  opacity: 0.8;
}

.login-btn {
  margin-top: 12px;
}

.profile-content {
  padding: 12px;
}

.order-group,
.service-group,
.settings-group {
  margin-bottom: 12px;
  border-radius: 8px;
  overflow: hidden;
  background: var(--van-background);
}

.order-grid {
  background: var(--van-background);
}

:deep(.order-grid-item) {
  .van-grid-item__text {
    color: var(--van-text-color);
    font-size: 12px;
    margin-top: 8px;
  }

  .van-grid-item__icon {
    color: var(--van-primary-color);
    font-size: 24px;
  }
}

:deep(.van-cell) {
  background: var(--van-background);
  color: var(--van-text-color);
}

.logout-cell {
  color: var(--van-danger-color);
}

@media (prefers-color-scheme: dark) {
  .order-group,
  .service-group,
  .settings-group {
    border: 1px solid var(--van-gray-8);
  }

  :deep(.van-grid-item__content) {
    background: transparent;
  }

  :deep(.van-cell) {
    &::after {
      border-color: var(--van-gray-8);
    }
  }
}
</style>