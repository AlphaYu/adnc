# ADNC-子分支自动化脚本

<!-- PROJECT SHIELDS -->

[![贡献者][contributors-shield]][contributors-url]
[![分叉][forks-shield]][forks-url]
[![星标][stars-shield]][stars-url]
[![问题][issues-shield]][issues-url]
[![MIT 许可证][license-shield]][license-url]


<!-- PROJECT LOGO -->
<p align="center">
简体中文
   <!--  |
 English -->
</p>

<div  align="center">

[**Docker Compose 脚本**](https://github.com/woyaodangrapper/adnc-devops/tree/main/docker-compose) 

[**报告 Bug**](https://github.com/woyaodangrapper/adnc-devops/issues) :earth_asia: [**查看发布**](https://github.com/woyaodangrapper/adnc-devops/releases) :earth_asia: [**建议新功能**](https://github.com/woyaodangrapper/adnc-devops/issues) :earth_asia:

</div>

## 🔄 初始化 Git 子模块
- [Git](https://git-scm.com/)
在克隆 ADNC 项目后，**如果包含子模块**，请先执行命令初始化和更新子模块：

````bash
git submodule init
git submodule update --recursive  # 拉取所有子模块
git submodule foreach --recursive git pull origin main  # 更新子模块至最新版本
````

## 📥 下载子模块分支的发布版本

也可以**下载子模块的特定分支发布版本**，而不需要拉取完整的 Git 仓库：
[<img src="https://user-images.githubusercontent.com/30566970/172445052-b0e62327-1d2e-4663-bc0f-af50c7f23615.svg" width="320"/>](https://github.com/woyaodangrapper/adnc-devops/archive/refs/heads/main.zip)


要查看所有版本， [**单击此处**](https://github.com/woyaodangrapper/adnc-devops/releases).


## 许可证声明

**项目以 MIT 许可证发布。**
[MIT License](https://mit-license.org/) 被授予此作品修改部分. 请参阅 [LICENSE](LICENSE)

Copyright © 2024 Adnc Devops

<!-- links -->

[contributors-shield]: https://img.shields.io/github/contributors/woyaodangrapper/adnc-devops.svg?style=flat-square
[contributors-url]: https://github.com/woyaodangrapper/adnc-devops/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/woyaodangrapper/adnc-devops.svg?style=flat-square
[forks-url]: https://github.com/woyaodangrapper/adnc-devops/network/members
[stars-shield]: https://img.shields.io/github/stars/woyaodangrapper/adnc-devops.svg?style=flat-square
[stars-url]: https://github.com/woyaodangrapper/adnc-devops/stargazers
[issues-shield]: https://img.shields.io/github/issues/woyaodangrapper/adnc-devops.svg?style=flat-square
[issues-url]: https://img.shields.io/github/issues/woyaodangrapper/adnc-devops.svg
[license-shield]: https://img.shields.io/github/license/woyaodangrapper/adnc-devops.svg?style=flat-square
[license-url]: https://github.com/woyaodangrapper/adnc-devops/blob/master/LICENSE.md
