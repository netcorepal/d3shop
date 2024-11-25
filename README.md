# d3shop

这是一个基于领域驱动设计实现的在线商城项目，用以展示需求分析-建模设计-代码实现的思考过程和决策结果，从而帮助开发者更深入地理解和掌握DDD的精髓。
这里的`d3`即表示`3`个字母`d`，也就是`Domain-Driven Design`的`DDD`。

## 如何参与

本项目完全通过开源协作的方式进行，所有的需求通过[issues](https://github.com/netcorepal/d3shop/issues)管理，提交[PR](https://github.com/netcorepal/d3shop/pulls)来贡献代码。

关注公众号`老肖想当外语大佬`，获取系列文章[老肖的领域驱动设计之路](https://mp.weixin.qq.com/mp/appmsgalbum?__biz=Mzg3Mzg5NjI0Ng==&action=getalbum&album_id=3587530562086371329&scene=126#wechat_redirect)，菜单栏加群参与讨论。

视频与直播在B站：[https://b23.tv/hUNoBjA](https://b23.tv/hUNoBjA)

## 技术原则

+ 完善的自动化测试覆盖
+ 基于事件驱动
+ 基于CQRS模式
+ 多数据库支持
+ 多MQ支持
+ 对二次开发友好


## 环境准备

参考[这里](./docker/README.md)的文档使用`docker-compose`搭建开发环境。

## 依赖对框架与组件

+ [NetCorePal Cloud Framework](https://github.com/netcorepal/netcorepal-cloud-framework)
+ [ASP.NET Core](https://github.com/dotnet/aspnetcore)
+ [EFCore](https://github.com/dotnet/efcore)
+ [CAP](https://github.com/dotnetcore/CAP)
+ [MediatR](https://github.com/jbogard/MediatR)
+ [FluentValidation](https://docs.fluentvalidation.net/en/latest)
+ [Swashbuckle.AspNetCore.Swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
+ [Ant Design Blazor](https://antblazor.com/)

## Admin UI

基于 Ant Design Blazor 实现，参见：

https://antblazor.com/zh-CN/components/overview

## 数据库迁移

```shell
# 安装工具  SEE： https://learn.microsoft.com/zh-cn/ef/core/cli/dotnet#installing-the-tools
dotnet tool install --global dotnet-ef --version 8.0.0

# 强制更新数据库
dotnet ef database update -p src/NetCorePal.D3Shop.Web 

# 创建迁移 SEE：https://learn.microsoft.com/zh-cn/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
dotnet ef migrations add InitialCreate -p src/NetCorePal.D3Shop.Web 
```

## 关于监控

这里使用了`prometheus-net`作为与基础设施prometheus集成的监控方案，默认通过地址 `/metrics` 输出监控指标。

更多信息请参见：[https://github.com/prometheus-net/prometheus-net](https://github.com/prometheus-net/prometheus-net)