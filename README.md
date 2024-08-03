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


---

#### 后端
先安装java17并且在配置文件中将数据库信息改为本地数据库信息，然后在intelijIDEA里启动项目即可
<br/>

#### k歌模块后端

---
music-builder-lwq即为k歌模块后端所在，直接使用visual studio启动即可
<br/>

#### 歌词生成后端

---
music-builder-czh即为歌词生成后端所在，使用了llama3中文微调模型，需要使用pycharm，配置python版本为3.11，然后根据电脑选择合适的pytorch，推荐CUDA版本12.1，由于空间限制，模型源码请邮件940527909@qq.com询问。
<br/>

#### 五个AI相关功能模块后端

---
五个功能的web-api已在music-builder-back-yk中的vs项目中封装好，启动相关AI本地服务后，使用music-builder-back中封装好的相关请求调用即可。

<br/>


### 🌾技术说明
本部分是说明项目的技术架构和重难点
<br/>

#### k歌模块

---
前端接收用户上传的音频文件和歌词文件，歌词文件直接在前端js实时渲染，音频文件传输到后端，后端再调用spleeter模型进行音频的伴奏和人声分离，再将存储的url返回到前端。前端根据相应的url找到对应文
所在地就可以开始k歌了。
<br/>

#### 音乐编辑

---
在这个界面用户可自由对音乐进行创作。使用Tone.js来编辑音符，使用konva.js来渲染钢琴卷等界面UI
<br/>

#### 前端intro
---
主要是css上的工作，截取了三个MP4视频作为背景，点击不同的按钮可以切换到不同的界面

#### AI清唱音乐合成
---
是一个基于DiffSinger模型实现的，可指定歌手音色，可指定歌词（歌词文本，语种，转音字等），可指定曲谱（MIDI）的AI人声清唱音乐的合成功能。
我们使用的DiffSinger就是一种基于扩散概率模型的SVS声学模型。
DiffSinger和Diff-SVC/SoVITS这类SVC模型不同。DiffSinger是歌声合成软件，输入乐谱和歌词，输出歌声。Diff-SVC Diff-SVC/SoVITS是人声转换软件，即变声器，用于将一个人的声音转换为另一个人的音色。
SoVITS模型早在去年暑期就有了较为成熟的版本，而DS则要稍慢一些，社区规模也稍小。但是DS的优势显而易见， 
人声音色，歌词，曲谱均可以任意指定，渲染速度快，且人声音色音库面向歌曲有特化效果，除了单纯的人声音色信息，还有人声的气息，歌唱时的音高曲线波动，转音的细节经验等。
可以说是唱歌是高级的说话，因此其基于发声原理的模型，参数设计，使SVS模型拥有更开放度，自由度更高的应用方式以及更加逼真，精准的人声信息细节模拟。
<br/>

#### AI人声提取MIDI
---
本功能使用的是DS社区提供工具集中的some模型，用于从单音轨人声音色wav文件中，提取记录音高，时间轴信息的MIDI曲谱文件。
<br/>

#### AI歌曲封面绘制
---
本功能采用的是目前市面上已经较为成熟的StableDiffusion绘画模型，封装了TAG风格方案来稳定出图质量，以及接入翻译接口，来帮助模型理解中文。
<br/>

#### AI生成伴奏MIDI 
---
本功能是基于GitHub上一指定乐器音乐生成项目，并进行训练集的重编和参数微调，对输出MIDI格式的音轨数量进行限制提高其稳定性与质量。
<br/>

#### AI生成整首歌曲
---
本功能使用的是facebook于今年开源的最新audiocraft大模型，策划了一些歌曲风格方案来提高输出音乐类型纯度，并接入翻译接口来兼容中文输入。

<br/>

### ⚡All Skills
本项目使用的技术栈如下
- Vue框架
- SpringBoot框架
- SpringSecurity框架
- C# EFcore框架
- spleeter分离模型
- FastAPI框架
- .net的Avalonia UI框架
-	DiffSinger推理模型及其工具集中的some模型
-	StableDiffusion绘画模型
-	Meta的audiocraft大模型
-	Arrange采样模型



[![ALL Skills](https://skillicons.dev/icons?i=java,vue,nodejs,net,c#)](https://skillicons.dev)

<br/>

### 🐰作者信息与分工

- [BV003](https://github.com/BV003/)
  k歌模块，前端intro部分的设计

- [P1erreCashon](https://github.com/P1erreCashon/)
  前后端框架搭建，后端SpringSecurity框架搭建，后端实现
- [Cz111111](https://github.com/Cz111111/)
  音乐编辑前端实现，文生词模型部署与微调
- [renfangwangzhongwang](https://github.com/renfangwangzhongwang/) 五个AI功能模块

