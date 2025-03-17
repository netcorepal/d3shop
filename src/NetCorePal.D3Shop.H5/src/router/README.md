# Router 目录

此目录包含应用程序中所有的路由配置文件。路由用于定义应用程序的导航结构。

## 用法

- 将所有的路由配置文件放在此目录中，以便在应用程序中使用。
- 使用 Vue Router 来定义路由规则和导航守卫。
- 定义不同页面的路由，以支持应用程序的导航。 

### 开发指导

- 路由文件的命名规则为：`router名称.ts`，例如：`router.ts` 详细开发文档参考 [vue-router官网](https://router.vuejs.org/zh/)
> 目前功能较少并没有分离开，所有路由都在index.ts中定义

### 路由守卫

- 路由守卫用于在路由导航时进行权限验证、登录状态检查等操作。
- 路由守卫的命名规则为：`guard名称.ts`，例如：`guard.ts` 详细开发文档参考 [vue-router 路由守卫 官网](https://router.vuejs.org/zh/guide/advanced/navigation-guards.html)
> 目前功能较少并没有分离开，所有路由守卫都在index.ts中定义