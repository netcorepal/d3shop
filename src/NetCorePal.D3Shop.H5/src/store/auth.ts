import { defineStore } from 'pinia';

interface UserInfo {
  username: string;
  // 根据实际需求添加其他用户信息字段
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || '',
    userInfo: null as UserInfo | null,
  }),
  
  getters: {
    isAuthenticated: (state) => !!state.token,
  },
  
  actions: {
    setToken(token: string) {
      this.token = token;
      localStorage.setItem('token', token);
    },
    
    setUserInfo(userInfo: UserInfo) {
      this.userInfo = userInfo;
    },
    
    logout() {
      this.token = '';
      this.userInfo = null;
      localStorage.removeItem('token');
    }
  },
});