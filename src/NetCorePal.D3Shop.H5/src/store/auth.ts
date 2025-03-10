import { loginApi } from '@/api/clientUser';
import { defineStore } from 'pinia';
import { useRouter } from 'vue-router';
import { storage } from '@/utils/cache';

interface UserInfo {
  username: string;
  // 根据实际需求添加其他用户信息字段
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: storage.get<string>("token"),
    refreshToken: storage.get<string>("refreshToken"), 
    userInfo: null as UserInfo | null,
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
  },

  actions: {
    async login(userName: string, password: string) {
      // 登录逻辑
      const res = await loginApi({
        userName,
        password,
      });
      if (res && res.success) {
        this.setToken(res.data?.accessToken, res.data?.expires);
        this.setRefreshToken(res.data?.refreshToken, res.data?.refreshExpires);
        return true;
      } else {
        return false;
      }
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
    },

    logout() {
      const router = useRouter();
      this.token = '';
      this.userInfo = null;
      storage.remove('token');
      storage.remove('refreshToken');
      router.push('/login');
    }
  },
});