import { NetCorePalD3ShopWebApiAxios } from '@/utils';

import { LoginInputDto, LoginResultDto } from './LoginDto.ts'

export class LoginApiService {
    login(input: LoginInputDto) {
        return NetCorePalD3ShopWebApiAxios.post<LoginInputDto, LoginResultDto>('/api/Login', input);
    }
}

export default new LoginApiService();