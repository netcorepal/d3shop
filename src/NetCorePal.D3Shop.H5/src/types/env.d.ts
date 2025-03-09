interface ImportMetaEnv {
  readonly VITE_NetCorePalD3Shop_EnvironmentName: string
  readonly VITE_NetCorePalD3Shop_WebApiUrl: string
  readonly VITE_API_TIMEOUT: number
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}