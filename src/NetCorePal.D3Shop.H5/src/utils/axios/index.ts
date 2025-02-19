import type {AxiosInstance, AxiosRequestConfig, AxiosError, AxiosResponse} from 'axios';
import axios from 'axios';
import { AxiosConfig, NetCorePalD3ShopWebApiUrl } from '@/config/index';

export class AxiosHttpClient {

    private instance: AxiosInstance;

    constructor(baseURL: string) {
        this.instance = axios.create({
            baseURL,
            timeout: AxiosConfig.timeout,
        });

        // 请求拦截器
        this.instance.interceptors.request.use(
            (config) => {
                // 在发送请求之前做些什么，例如添加 token
                const token = localStorage.getItem('token');
                if (token) {
                    config.headers = config.headers || {};
                    config.headers['Authorization'] = `Bearer ${token}`;
                }
                return config;
            },
            (error: AxiosError) => {
                // 对请求错误做些什么
                return Promise.reject(error);
            }
        );

        // 响应拦截器
        this.instance.interceptors.response.use(
            (response) => {
                // 对响应数据做些什么
                if (response.data.code !== 200) {
                    // 处理业务逻辑错误
                    console.error(response.data.message);
                    return Promise.reject(response.data);
                }
                return response.data.data;
            },
            (error: AxiosError) => {
                // 对响应错误做些什么
                if (error.response) {
                    // 处理 HTTP 状态码错误
                    console.error(`HTTP error: ${error.response.status}`);
                } else {
                    console.error('Network error');
                }
                return Promise.reject(error);
            }
        );
    }

    // GET 请求
    public get<TResult>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<TResult>> {
        return this.instance.get<TResult, AxiosResponse<TResult>, any>(url, config);
    }

    // POST 请求
    public post<TInput, TResult>(url: string, data?: TInput, config?: AxiosRequestConfig): Promise<AxiosResponse<TResult>> {
        return this.instance.post<TResult, AxiosResponse<TResult>, TInput>(url, data, config);
    }
}

export const NetCorePalD3ShopWebApiAxios = new AxiosHttpClient(NetCorePalD3ShopWebApiUrl);