export interface ClientUserLoginRequest{
  phone: string;
  password: string;
  loginMethod: string;
}
export interface ClientUserLoginResponse {
  isSuccess: boolean;
  token: string;
  refreshToken: string;
  failedMessage: string;
}

export interface ClientUserRegisterRequest {
  nickName: string;
  avatar: string;
  phone: string;
  password: string;
  email: string;
}


export interface ClientUserInfoResponseData {
  id: string;
  nickName: string;
  avatar: string;
  phone: string;
  email: string;
}


export interface ClientUserRegisterRequest{
  nickName: string;
  avatar: string;
  phone: string;
  password: string;
  confirmPassword: string;
  email: string;
}



export interface ClientUserGetRefreshTokenRequest {
  token?: string;
  refreshToken?: string;
}

export interface ClientUserGetRefreshTokenResponse {
  token: string;
  refreshToken: string;
}

export interface ClientUserExternalLoginResponse {
  isSuccess: boolean;
  accessToken?: string;
  refreshToken?: string;
  requiresSignUp: boolean;
  signupToken?: string;
}

export interface ClientUserExternalSignUpRequest {
  signupToken?: string;
  phone?: string;
  password?: string;
}

export interface ClientUserExternalSignUpResponse {
  token?: string;
  refreshToken?: string;
} 


export interface ClientUserAddDeliveryAddressRequest {
  recipientName: string;
  phone: string;
  address: string;
  setAsDefault: boolean;
}

export interface ClientUserUpdateDeliveryAddressRequest {
  deliveryAddressId: string;
  recipientName: string;
  phone: string;
  address: string;
  setAsDefault: boolean;
}

export interface ClientUserDeliveryAddressInfo {
  id: string;
  recipientName: string;
  phone: string;
  address: string;
  isDefault: boolean;
}
