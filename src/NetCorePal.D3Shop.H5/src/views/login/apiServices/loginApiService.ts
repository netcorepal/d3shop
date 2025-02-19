import { loginApi } from '@/api/auth';
import { useAuthStore } from '@/store/auth';
import { LoginInputDto, LoginResultDto } from './LoginDto';

export class LoginApiService {
    async login(loginInputDto: LoginInputDto): Promise<LoginResultDto> {
        const response = await loginApi(loginInputDto);
        
        // 登录成功后更新 store
        const authStore = useAuthStore();
        if (response.token) {
            authStore.setToken(response.token);
            authStore.setUserInfo({
                username: loginInputDto.username,
                // 其他用户信息...
            });
        }

        return response;
    }
}

export default new LoginApiService();