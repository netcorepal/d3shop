# I18n 目录

此目录包含应用程序中所有的国际化文件。国际化文件用于定义应用程序的多语言支持。

## 用法

- 将所有的语言文件放在此目录中，以便在应用程序中使用。
- 使用 JSON 或 JavaScript 来定义语言字符串。
- 定义不同语言的翻译文件，以支持多语言切换。 

### 开发指导

- 国际化文件的命名规则为：`locale名称.ts`，例如：`locale.ts` 详细开发文档参考 [vue-i18n 官网](https://vue-i18n.intlify.dev/zh/guide/)
> 目前只定义了中文和英文，如果需要定义其他语言，请参考 [vue-i18n 官网](https://vue-i18n.intlify.dev/zh/guide/)

- 可以安装 vscode 插件 [i18n-ally](https://github.com/lokalise/i18n-ally/wiki) 来提高开发效率