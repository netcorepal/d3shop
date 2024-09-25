# 环境准备

## 安装docker和docker-compose

参见[官网文档](https://docs.docker.com/engine/install/)

## 使用docker-compose启动服务

- 启动全部容器 `docker-compose up -d`
- 启动指定容器 `docker-compose up -d <container_name>`

### .env文件配置

> Docker Compose 中的 .env 文件是一个纯文本文件,用于定义当运行 `docker compose up` 时应在 Docker 容器中提供哪些环境变量.此文件通常包含环境变量的键值对,可用于集中管理各处配置.

目前支持的变量:

| 变量名   | 默认值 | 描述         |
| -------- | ------ | ------------ |
| DATA_DIR | .      | 挂载路径前缀 |

修改之后可以使用` docker-compose config `查看效果
