
import { useAuthStore } from '@/store/auth';
import { LoginInputDto } from './LoginDto';
import { showToast } from 'vant'

export class LoginApiService {
    async login(loginInputDto: LoginInputDto): Promise<boolean> {
        const authStore = useAuthStore();
        await authStore.login(loginInputDto.userName, loginInputDto.password);
        showToast('登录成功');
        return true;
    }
}

export default new LoginApiService();