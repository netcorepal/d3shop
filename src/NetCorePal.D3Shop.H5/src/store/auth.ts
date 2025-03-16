import { clientUserLogin, getClientUserInfo, refreshToken as refreshTokenApi } from '@/api/clientUser';
import { defineStore } from 'pinia';
import { storage } from '@/utils/cache';

interface UserInfo {
  username: string;
  phone: string;
  avatar: string;
  // 根据实际需求添加其他用户信息字段
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: storage.get<string>("token"),
    refreshToken: storage.get<string>("refreshToken"),
    userInfo: storage.get<UserInfo>("userInfo"),
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
  },

  actions: {
    async login(phone: string, password: string) {
      // 登录逻辑
      const res = await clientUserLogin({
        phone: phone,
        password: password,
        loginMethod: 'password',
      });
      this.setToken(res?.token, 60 * 60);
      this.setRefreshToken(res?.refreshToken, 7 * 24 * 60 * 60);
      const userInfo = await getClientUserInfo();
      console.log(userInfo, 'userInfo')
      this.setUserInfo({ username: userInfo.nickName, phone: userInfo.phone, avatar: userInfo.avatar });
    },

    async tokenRefresh() {
      if (!this.refreshToken) {
        return;
      }
      const res = await refreshTokenApi(this.refreshToken);
      this.setToken(res?.token, 60 * 60);
      this.setRefreshToken(res?.refreshToken, 7 * 24 * 60 * 60);
      const userInfo = await getClientUserInfo();
      console.log(userInfo, 'userInfo')
      this.setUserInfo({ username: userInfo.nickName, phone: userInfo.phone, avatar: userInfo.avatar });
    },
    setToken(token: string | undefined, expiresIn: number | undefined) {
      if (!token) {
        return;
      }
      this.token = token;
      storage.set("token", token, expiresIn ?? 3600);
    },

    setRefreshToken(refreshToken: string | undefined, expiresIn: number | undefined) {
      if (!refreshToken) {
        return;
      }
      this.refreshToken = refreshToken;
      storage.set("refreshToken", refreshToken, expiresIn ?? 3600);
    },

    setUserInfo(userInfo: UserInfo) {
      this.userInfo = userInfo;
      storage.set("userInfo", userInfo, 3600);
    },

    logout() {
      this.token = '';
      this.userInfo = null;
      storage.remove('token');
      storage.remove('refreshToken');
    }
  },
});