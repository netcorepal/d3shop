// 移除现有的接口定义和API调用
// 这些将被迁移到新的model目录中

// 保留必要的import语句
import request from '@/utils/request';
import type {
    ClientUserLoginRequest, ClientUserLoginResponse, ClientUserGetRefreshTokenResponse,
    ClientUserGetRefreshTokenRequest, ClientUserInfoResponseData, ClientUserAddDeliveryAddressRequest, ClientUserDeliveryAddressInfo,
    ClientUserRegisterRequest,
    ClientUserUpdateDeliveryAddressRequest
} from './model/clientUser';

/**
 * 注册用户
 * @param requestData 
 * @returns 
 */
export async function registerClientUser(requestData: ClientUserRegisterRequest): Promise<number> {
    const response = await request.post<ClientUserRegisterRequest, number>('/api/ClientUserAccount/Register', requestData);
    return response;
}


/**
 * 登录
 * @param requestData 
 * @returns 
 */
export async function clientUserLogin(requestData: ClientUserLoginRequest): Promise<ClientUserLoginResponse> {
    const response = await request.post<ClientUserLoginRequest, ClientUserLoginResponse>('/api/ClientUserAccount/Login', requestData);
    return response;
}

/**
 * 刷新Token
 * @param refresh 
 * @returns 
 */
export async function refreshToken(refresh: string) {
    const response = await request.put<ClientUserGetRefreshTokenRequest, ClientUserGetRefreshTokenResponse>('/api/ClientUserAccount/getRefreshToken', { refresh });
    return response;
}

/**
 * 获取用户信息
 * @returns 
 */
export async function getClientUserInfo(): Promise<ClientUserInfoResponseData> {
    const response = await request.get<{}, ClientUserInfoResponseData>('/api/ClientUser/GetClientUserInfo');
    return response;
}

/**
 * 添加收货地址
 * @param requestData 
 * @returns 
 */
export async function addDeliveryAddress(requestData: ClientUserAddDeliveryAddressRequest): Promise<any> {
    const response = await request.post<ClientUserAddDeliveryAddressRequest, any>('/api/ClientUser/AddDeliveryAddress', requestData);
    return response;
}

/**
 * 更新收货地址
 * @param requestData 
 * @returns 
 */
export async function updateDeliveryAddress(requestData: ClientUserUpdateDeliveryAddressRequest): Promise<any> {
    const response = await request.put<ClientUserUpdateDeliveryAddressRequest, any>('/api/ClientUser/UpdateDeliveryAddress', requestData);
    return response;
}

/** 
 * 删除收货地址
 * @param requestData 
 * @returns 
 */
export async function deleteDeliveryAddress(deliveryAddressId: string): Promise<any> {
    const response = await request.delete<{}, any>(`/api/ClientUser/RemoveDeliveryAddress?deliveryAddressId=${deliveryAddressId}`, {});
    return response;
}


/**
 * 获取收货地址列表
 * @returns 
 */
export async function getDeliveryAddresses(): Promise<ClientUserDeliveryAddressInfo[]> {
    const response = await request.get<{}, ClientUserDeliveryAddressInfo[]>('/api/ClientUser/GetDeliveryAddresses');
    return response;
}

