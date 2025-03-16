# Components 目录

此目录包含应用程序中所有的 自定义Vue 组件。组件是可复用的 UI 单元，可以在不同的视图中使用。

## 用法

- 将通用的 UI 组件放在此目录中，以便在多个视图中复用。
- 组件应尽量保持独立，避免直接依赖其他组件。
- 使用 `props` 和 `events` 来与父组件进行交互。 
> 组件的命名规则为：`组件名称.vue`，例如：`LanguageSwitcher.vue` 详细开发文档参考 [vue3官网](https://cn.vuejs.org/guide/essentials/component-basics.html)