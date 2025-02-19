import { NetCorePalD3ShopWebApiAxios } from '@/utils';
import { LoginInputDto, LoginResultDto } from '@/views/login/apiServices/LoginDto';

export async function loginApi(loginInputDto: LoginInputDto): Promise<LoginResultDto> {
    // 模拟 API 调用
    const response = await NetCorePalD3ShopWebApiAxios.post<LoginInputDto, LoginResultDto>('/api/Login', loginInputDto);
    return response;
}