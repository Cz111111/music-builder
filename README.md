# ML-music-builder 音籁音乐制作器

![License](https://img.shields.io/badge/license-MIT-blue)

### 🤔简介
我们创作了一个简易音乐制作器
<br/>
同时，我们完成这个项目来作为我们暑假实训的大作业。
<br/>
在
[音籁-用户使用手册](https://michaels-organization-45.gitbook.io/test)
可以找到音籁音乐制作器的功能说明与效果展示

<br/>

### 🪐启动
本部分是说明如何在本地启动音籁项目
<br/>



#### 前端

---
music-builder-front即为前端文件所在，打开文件夹，输入

`npm install`

进行前端依赖的安装，再输入

`npm run dev`

即可以启动前端
<br/>

#### 后端
先安装java17并且在配置文件中将数据库信息改为本地数据库信息，然后在intelijIDEA里启动项目即可

<br/>

#### k歌模块后端

---
music-builder-lwq即为k歌模块后端所在，直接使用visual studio启动即可

<br/>

### 🌾技术说明
本部分是说明项目的技术架构和重难点
<br/>

#### k歌模块

---
前端接收用户上传的音频文件和歌词文件，歌词文件直接在前端js实时渲染，音频文件传输到后端，后端再调用spleeter模型进行音频的伴奏和人声分离，再将存储的url返回到前端。前端根据相应的url找到对应文件所在地就可以开始k歌了。
<br/>

#### 前端intro
---
主要是css上的工作，截取了三个MP4视频作为背景，点击不同的按钮可以切换到不同的界面

<br/>

### ⚡All Skills
本项目使用的技术栈如下
- Vue框架
- SpringBoot框架
- SpringSecurity框架
- C# EFcore框架
- spleeter分离模型

[![ALL Skills](https://skillicons.dev/icons?i=vue,c#)](https://skillicons.dev)

<br/>

### 🐰作者信息与分工

- [BV003](https://github.com/BV003/)
  k歌模块，前端intro部分的设计

- [P1erreCashon](https://github.com/P1erreCashon/)
  前后端框架搭建，后端SpringSecurity框架搭建，后端实现
- [姓名3](https://github.com/用户名3)
