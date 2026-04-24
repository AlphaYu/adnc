# 🚀 ADNC 项目 Docker Compose 部署指南

## 📦 环境准备

在开始之前，请确保你的环境已经安装了以下工具：

- [Docker](https://docs.docker.com/get-docker/)
- [Docker Compose](https://docs.docker.com/compose/install/)

## 🐳 Docker Compose 配置

在 `adnc` 目录下找到并进入 `doc/dev-ops/docker-compose` 文件夹

## 🚢 启动服务

<p data-start="44" data-end="102"><strong data-start="44" data-end="50">提示</strong>：若首次启动失败，请删除所有 adnc 容器并清理 <code data-start="76" data-end="82">.env</code> 下的 <code data-start="86" data-end="92">adnc</code> 文件夹后重试。</p>

执行以下命令来启动 Docker Compose 服务：
```bash
# 部署平台所需基本服务
docker-compose up -d
````

## 🧹 清理资源

如需删除所有相关容器、网络和卷：

```bash
docker-compose down -v
```

## 🎯 结语

通过以上步骤，你已经成功使用 Docker Compose 部署了 adnc 项目！如果有任何问题，欢迎提 Issue 或查阅官方文档。

祝你使用愉快！🚀

### **调整点：**

1. **新增** `🔄 初始化 Git 子模块` **章节**
2. **添加** `git submodule` **初始化和更新命令**
3. **保证用户在拉取项目后能正确获取子模块**

