
import { useAuthStore } from '@/store/auth';
import { LoginInputDto } from './LoginDto';
import { showToast } from 'vant'

export class LoginApiService {
    async login(loginInputDto: LoginInputDto): Promise<boolean> {
        const authStore = useAuthStore();
        const result = await authStore.login(loginInputDto.userName, loginInputDto.password);
        if (result) {
            showToast('登录成功');
        } else {
            showToast('登录失败');
        }
        return result;
    }
}

export default new LoginApiService();