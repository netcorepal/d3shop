// 模块化类型定义
declare interface ResponseData<T = any> {
  success: boolean;
  message: string;
  code: number;
  errorData: any[];
  data?: T;
}

declare interface StorageItem<T = any> {
  value: T;
  expire: number;
}