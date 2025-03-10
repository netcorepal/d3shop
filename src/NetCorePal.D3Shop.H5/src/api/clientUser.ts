import { NetCorePalD3ShopWebApiAxios } from '@/utils';
import { LoginInputDto } from '@/views/login/apiServices/LoginDto';
import request from '@/utils/request';

export interface Token {
    accessToken: string;
    expires: number;
    refreshToken: string;
    refreshExpires: number;
}

export async function loginApi(request: LoginInputDto): Promise<ResponseData<Token>> {
    // 模拟 API 调用
    const response = await NetCorePalD3ShopWebApiAxios.post<LoginInputDto, ResponseData<Token>>('/api/Login', request);
    return response.data;
}



export async function refreshToken(refresh: string) {
    const response = await request.post('/api/Account/RefreshToken', { refresh });
    return response.data;
}