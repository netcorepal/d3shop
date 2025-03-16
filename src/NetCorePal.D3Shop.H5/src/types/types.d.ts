// 模块化类型定义
declare interface ResponseData<T = any> {
  success: boolean;
  message: string;
  code: number;
  errorData: FieldError[];
  data?: T;
}

declare interface StorageItem<T = any> {
  value: T;
  expire: number;
}

declare interface FieldError {
  errorCode: string;
  errorMessage: string;
  propertyName: string;
}

declare interface FieldErrors {
  [key: string]: FieldError[];
}
