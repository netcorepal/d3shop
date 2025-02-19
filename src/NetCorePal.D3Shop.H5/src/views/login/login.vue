<template>
  <h1>登录页面</h1>
  <p>账号：<input type="text" v-model="loginViewData.username" /></p>
  <p>密码：<input type="text" v-model="loginViewData.password" /></p>

  <button @click="loginBtnClick">登录</button>
</template>

<script setup lang="ts">
import { reactive } from 'vue';

import { LoginInputDto, LoginResultDto } from './apiServices/LoginDto.ts'
import loginApiService from './apiServices/loginApiService'

const loginViewData = reactive({
  username: '',
  password: '',
});

function loginBtnClick() {
  const loginInputDto: LoginInputDto = new LoginInputDto(loginViewData.username, loginViewData.password);
  const result: LoginResultDto = loginApiService.login(loginInputDto);

  console.log(result);
}
</script>

