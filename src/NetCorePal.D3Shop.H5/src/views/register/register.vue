<template>
  <div class="register-container">
    <van-nav-bar title="注册" class="register-nav">
      <template #left>
        <van-icon name="cross" size="18" @click="onClose" class="nav-icon" />
      </template>
    </van-nav-bar>

    <div class="register-form">
      <van-form>
        <van-cell-group inset>
          <van-field v-model="registerViewData.nickName" name="nickName" label="昵称" placeholder="请输入昵称"
            :rules="[{ required: true, message: '请填写昵称' }]" :error-message="registerViewData.errors.nickName" />
          <van-field v-model="registerViewData.phone" name="phone" label="手机号" placeholder="请输入手机号"
            :rules="[{ required: true, message: '请填写手机号' }]" :error-message="registerViewData.errors.phone" />
          <van-field v-model="registerViewData.password" type="password" name="password" label="密码" placeholder="请输入密码"
            :rules="[{ required: true, message: '请填写密码' }]" :error-message="registerViewData.errors.password" />
          <van-field v-model="registerViewData.confirmPassword" type="password" name="confirmPassword" label="确认密码"
            placeholder="请输入确认密码" :rules="[{
              required: true,
              message: '请填写确认密码',
              validator: (value: string) => {
                if (value !== registerViewData.password) {
                  return '两次输入的密码不一致';
                }
                return true;
              }
            }]" :error-message="registerViewData.errors.confirmPassword" />
          <van-field v-model="registerViewData.email" name="email" label="邮箱" placeholder="请输入邮箱"
            :rules="[{ required: false, message: '请填写邮箱' }]" :error-message="registerViewData.errors.email" />
        </van-cell-group>

        <div style="margin: 16px">
          <van-button round block type="primary" @click="registerBtnClick" native-type="submit">
            {{ t('auth.register') }}
          </van-button>
        </div>
      </van-form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { useRouter } from 'vue-router';
import { showToast } from 'vant';
import { useI18n } from 'vue-i18n';
import { registerClientUser } from '@/api/clientUser';
import type { ClientUserRegisterRequest } from '@/api/model/clientUser';
import { mapErrorsToFields } from '@/utils/form/validate';

const router = useRouter();
const { t } = useI18n();

const registerViewData = reactive<ClientUserRegisterRequest & { errors: Record<string, string> }>({
  nickName: '',
  phone: '',
  password: '',
  confirmPassword: '',
  email: '',
  avatar: '',
  errors: {}
});

const onClose = () => {
  router.back();
};

async function registerBtnClick() {
  try {
    await registerClientUser(registerViewData);
    showToast('注册成功');
    router.push('/login');
  } catch (errors: any) {
    registerViewData.errors = mapErrorsToFields(errors, registerViewData.errors)
  }
}
</script>

<style scoped>
.register-container {
  height: 100vh;
  background-color: var(--van-background-2);
}

.register-form {
  padding: 16px;
  margin-top: 20px;
}

:deep(.van-nav-bar) {
  background-color: var(--van-background-2);
}

:deep(.van-nav-bar__title) {
  color: var(--van-text-color);
}

:deep(.van-nav-bar__left) {
  cursor: pointer;
  padding-left: 16px;
}

:deep(.nav-icon) {
  color: var(--van-text-color);
}

@media (prefers-color-scheme: dark) {
  :deep(.nav-icon) {
    color: var(--van-white);
  }

  :deep(.van-nav-bar__title) {
    color: var(--van-white);
  }

  :deep(.van-nav-bar) {
    background-color: var(--van-background-2);
  }

  .register-container {
    background-color: var(--van-black);
  }
}
</style>