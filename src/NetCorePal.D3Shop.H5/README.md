# NetCorePal.D3Shop.H5

# 运行所需

### node.js
https://nodejs.org/zh-cn

```
node -v

npm -v
```

### pnpm
https://pnpm.nodejs.cn/installation
```
npm install -g pnpm@latest-10
```

# 相关技术栈
- Vue3 [https://cn.vuejs.org/guide/quick-start](https://cn.vuejs.org/guide/quick-start)
- Vue Router [https://router.vuejs.org/zh/introduction.html](https://router.vuejs.org/zh/introduction.html)
- Vite [https://www.vitejs.net/guide/](https://www.vitejs.net/guide/)
- TypeScript [https://www.typescriptlang.org](https://www.typescriptlang.org)
- Axios [http://www.axios-js.com](http://www.axios-js.com)
- Pinia [https://pinia.vuejs.org/zh/](https://pinia.vuejs.org/zh/)
- Vant [https://vant-ui.github.io/vant/#/zh-CN/](https://vant-ui.github.io/vant/#/zh-CN/)


# 环境描述
- localhost 本地开发环境
- development 部署：开发环境
- test 部署：测试环境
- production 部署：生产环境


# 命令

### 启动命令
```
pnpm run dev-localhost

pnpm run dev-development

pnpm run dev-test

pnpm run dev-production
```

### 打包命令
```
pnpm run build-localhost

pnpm run build-development

pnpm run build-test

pnpm run build-production
```


# 开始

### 进入项目根目录
```
pnpm install
```

### 启动项目
```
pnpm run dev-localhost
```

## 项目目录

- [API](./src/api/README.md): 包含与 API 交互相关的所有文件和模块。
- [Components](./src/components/README.md): 包含应用程序中所有的 Vue 组件。
- [Directives](./src/directives/README.md): 包含应用程序中所有的自定义指令。
- [Utils](./src/utils/README.md): 包含应用程序中所有的工具函数和实用程序模块。
- [Views](./src/views/README.md): 包含应用程序中所有的视图组件。
- [Store](./src/store/README.md): 包含应用程序中所有的状态管理模块。
- [Styles](./src/styles/README.md): 包含应用程序中所有的全局样式文件。
- [Types](./src/types/README.md): 包含应用程序中所有的类型定义文件。
- [I18n](./src/i18n/README.md): 包含应用程序中所有的国际化文件。
- [Layouts](./src/layouts/README.md): 包含应用程序中所有的布局组件。
- [Router](./src/router/README.md): 包含应用程序中所有的路由配置文件。