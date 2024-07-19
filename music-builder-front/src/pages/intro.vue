<template>
    <!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <link rel="stylesheet" href="style.css">
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css"/>
  <title>音籁MLMusicBuilder</title>
</head>
<body>
  <header>
    <a href="#" class="brand">音籁 ML</a>
    <div class="menu-btn"></div>
  </header>
  <section class="home">
    <video  class="video-slide active" src="/src/assets/home.mp4" autoplay muted loop></video>
    <video  class="video-slide" src="/src/assets/home2.mp4" autoplay muted loop></video>
    <video  class="video-slide" src="/src/assets/home3.mp4" autoplay muted loop></video>

    <div class="content active">
        <h1>创作革命<br><span>全新音乐制作方式</span></h1>
        <p>无需懂得乐理，只需您的想象力，您就可以使用音籁创作了不起的音乐</p>
        <a @click="cc" style="cursor: pointer;">立即体验！</a>
    </div>

    <div class="content">
      <h1>AI赋能<br><span>解放您的生产力</span></h1>
      <p>AI生成伴奏，AI生成歌词，AI提取人声，AI生成封面，在音籁让AI提升您的生产力</p>
      <a @click="cc" style="cursor: pointer;"> 立即体验！</a>
    </div>

    <div class="content" >
      <h1>专业团队打造<br><span>爱好者献给爱好者</span></h1> 

      <ul>
        <li style="list-style-type: none;">
          <div class="member-info">
            <div class="avatar-and-name">
              <img src="/src/assets/1.jpg" alt="头像" class="avatar">
              <span class="name">陈郑翰</span>
            </div>
          </div>
        </li>
        <li style="list-style-type: none;">
          <div class="member-info">
            <div class="avatar-and-name">
              <img src="/src/assets/2.jpg" alt="头像" class="avatar">
              <span class="name">袁轲</span>
            </div>
          </div>
        </li>
        <li style="list-style-type: none;">
          <div class="member-info">
            <div class="avatar-and-name">
              <img src="/src/assets/3.jpg" alt="头像" class="avatar">
              <span class="name">唐诗浩</span>
            </div>
          </div>
        </li>
        <li style="list-style-type: none;">
          <div class="member-info">
            <div class="avatar-and-name">
              <img src="/src/assets/4.jpg" alt="头像" class="avatar">
              <span class="name">刘玮祺</span>
            </div>
          </div>
        </li>
      </ul>
      <a style="display: block; margin-top: 5px; width: 200px;text-align: center;cursor: pointer;" @click="cc"> 立即体验！</a>

    </div>


    <div class="media-icons">
      <a href="https://github.com/Cz111111/music-builder"><i class="fab fa-github"></i></a>
      <a href="https://michaels-organization-45.gitbook.io/test"><i class="fa fa-book"></i></a>
    </div>

    <div class="slider-navigation">
      <div class="nav-btn active"></div>
      <div class="nav-btn"></div>    
      <div class="nav-btn"></div>  
    </div>
  </section>   
  <el-dialog
    v-model="dialogVisible"
    width="380"
  >
  <div class="register-container">
            <div class="title">登录</div>
            <div class="rd">
                <form id="registerForm" @submit.prevent="handleSubmit">
                    <div class="form-group">
                        <label for="username">账号</label>
                        <div class="input">
                            <div class="icons"><el-icon :size="17">
                                    <User style="color: #7c7c7c;" />
                                </el-icon></div>
                            <input type="text" id="username" name="username" placeholder="请输入账号"
                                v-model="loginForm.username">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="password">密码</label>
                        <div class="input">
                            <div class="icons"><el-icon :size="17">
                                    <Lock style="color: #7c7c7c;" />
                                </el-icon></div>
                            <input type="password" id="password" name="password" placeholder="请输入密码"
                                v-model="loginForm.password">
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="text">忘记密码？</div>
                    </div>


                    <button type="submit" class="submit">登录</button>
                    <div class="form-group">
                        <div class="form-group-item">
                            <div class="topinfo">第三方登录</div>
                            <div class="coreinfo">
                                <img src="../assets/wechaticon.png" />
                                <img src="../assets/qqicon.png" />
                                <img src="../assets/githubicon.png" />
                            </div>
                            <div class="bottominfo" @click="toRegister">没有账号？立即注册</div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
  </el-dialog>
</body>
</html>
</template>

<script setup>
import {onMounted,ref} from "vue";
const dialogVisible=ref(false)

const cc=()=>{
  dialogVisible.value=true;
}
import { useRouter } from "vue-router";
import { http } from '../http'
import { useTokenScore } from '../stores/token.js'
import { useUsername } from '../stores/username.js'

const tokenScore = useTokenScore()
const usernameScore = useUsername()
const router = useRouter()
const toRegister = () => {
    router.push({ name: 'register' });
}

const loginForm = ref({
    username: '',
    password: ''
})


const handleSubmit = async () => {
    if (!loginForm.value.username
        || !loginForm.value.password) {
        alert('请填写完整数据')
        return
    }


    // 模拟提交数据
    console.log('提交的登录信息:', loginForm.value)



    try {
        //       showLoadingToast({
        //        message: '登陆中...',
        //      });
        const response = await http.post('/user/login', {
            username: loginForm.value.username,
            password: loginForm.value.password
        })
        alert(response.data.message)
        localStorage.setItem("tokenTest", response.data.data.token)

        if (response.data.success) {
            //       closeToast();
            console.log('登录成功');
            tokenScore.setToken(response.data.data.token)
            usernameScore.setUsername(loginForm.value.username)
            /*         const user = {
                        userId: loginForm.value.username,
                        password: loginForm.value.password,
                        token:response.data.token
                    }; */
            //       state.setUser(user);
            router.push({ name: 'createsong' })
        } else {
            //     closeToast();
            console.log('用户名或密码错误')
        }
    } catch (error) {
        //     closeToast();
        console.log('登录失败，请稍后重试')
        console.error(error)
    }
    //   tokenScore.removeToken()
    loginForm.value = {// 清空表单
        username: '',
        password: ''
    }
}




onMounted(() => {
    const menuBtn = document.querySelector(".menu-btn")
 const navigation = document.querySelector(".navigation")

 menuBtn.addEventListener("click",()=>{
  menuBtn.classList.toggle("active");
  navigation.classList.toggle("active");
 });



 const btns = document.querySelectorAll(".nav-btn")
 const slides = document.querySelectorAll(".video-slide")
 const contents = document.querySelectorAll(".content")

 var sliderNav= function(manual){
  btns.forEach((btn) => {
    btn.classList.remove("active")

  })

  slides.forEach((slide) => {
    slide.classList.remove("active")
  })

  contents.forEach((content) => {
    content.classList.remove("active")
  })

  btns[manual].classList.add("active")
  slides[manual].classList.add("active")
  contents[manual].classList.add("active")

 }

 btns.forEach((btn,i)=>{
    btn.addEventListener("click",()=>{
      sliderNav(i)
    })
 });

})
</script>

<style scoped>
.register-container {
    background-color: #ffffff;
    padding: 20px;
    border-radius: 10px;
    width: 350px;
    height: 500px;
}

.title {
    margin-top: 25px;
    margin-bottom: 20px;
    color: #222222;
    font-size: 30px;
    font-weight: 500;
    font-family: SimHei;
    width: 100%;
    text-align: center;
}


.form-group {
    margin-top: 30px;
    padding-left: 20px;
    padding-right: 20px;
}

label {
    display: block;
    margin-bottom: 0px;
    font-weight: 550;
    font-size: 12px;
    color: #6a6363;
}

.text {
    text-align: end;
    font-weight: 550;
    font-size: 12px;
    color: #6a6363;
    position: relative;
    bottom: 22px;
}

.input {
    display: flex;
    border-bottom: 1.5px solid #a9a9a9;
}

.icons {
    margin-right: 10px;
    height: 40px;
    width: 20px;
    display: flex;
    justify-content: center;
    align-items: center;

}

#username,
#password {
    width: 100%;
    height: 40px;
    padding: 5px;
    box-sizing: border-box;
    font-size: 13px;
    font-weight: 300;
    border: none;
    outline: none;
}

button {
    padding: 0;
    margin-left: 17px;
    margin-top: 0;
    background-color: #ac3d3d;
    color: white;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    width: 90%;
    height: 35px;
    font-size: 15px;

}

.form-group-item {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
}

.topinfo,
.coreinfo,
.bottominfo {
    height: 32px;
    line-height: 32px;
    text-align: center;
    font-weight: 550;
    font-size: 12px;
    color: #6a6363;
}

.bottominfo {
    text-decoration: underline;
    cursor: pointer;
}

.coreinfo {
    display: flex;
    justify-content: space-between;
    gap: 8px;
    margin-top: 10px;
    margin-bottom: 10px;
}





*{
  margin: 0px;
  padding: 0px;
  box-sizing: border-box;
}

header{
  z-index: 999;
  display: flex;
  position: absolute;
  top: 0 ;
  left: 0;
  width:100%;
  justify-content: space-between;
  padding: 15px 200px;
  transition: 0.5s ease ;
  align-items: center;
}

.brand{
  color: #fff;
  font-size: 1.5rem;
  font-weight: 700;
  text-transform: uppercase;
  text-decoration: none;
}

header .navigation{
  position: relative;
}

header .navigation .navigation-items a{
  position: relative;
  color: #fff;
  font-size: 1rem;
  font-weight: 500;
  text-decoration: none;
  margin-left: 30px;
  transition: 0.3s ease ;
}

header .navigation .navigation-items a::before{
  content: "";
  position: absolute;
  bottom: 0px;
  left: 0px;
  background-color: #fff;
  width: 0%;
  height: 3px;
  transition: 0.3s ease;
}

header .navigation .navigation-items a:hover:before{
  width: 100%;
}

section{
  padding: 100px 200px;
}



.home{
  position: relative;
  width: 100%;
  min-height: 100vh;
  display: flex;
  justify-content: center;
  flex-direction: column;
  background-color: #f00;
}

.home video{
  z-index: 000;
  position: absolute;
  top:0;
  left: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.home:before{
  z-index: 777;
  content: '';
  position:absolute;
  top: 0;
  left: 0;
  background: rgba(3,96,251,0.3);
  width: 100%;
  height: 100%;
}
ul {
  list-style-type: none;
  padding: 0;
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  grid-gap: 30px;
}
.home .content{
   z-index: 888;
   color: #fff;
   width: 70%;
   margin-top: 50px;
   display: none;
}

.home .content.active{
  display: block;
}

.home .content h1 {
  font-size: 4rem;
  font-weight: 900;
  text-transform: uppercase;
  letter-spacing: 5px;
  line-height: 75px;
  margin-bottom: 40px;
}
.home .content h1 span{
  font-size: 1.2em;
  font-weight: 600;
}

.home .content p{
  margin-bottom: 65px;
}

.home .content a{
  background: #fff;
  padding: 15px 35px;
  color: #f00;
  font-size: 1.1em;
  font-weight: 500;
  text-decoration: none;
  border-radius: 2px;
}
.home .media-icons {
  z-index: 888;
  position: absolute;
  display: flex;
  flex-direction: column;
  right: 30px;
  transition: 0.5s ease;
  justify-content: space-between;
  gap:20px
}

.home .media-icons a{
  color: #fff;
  font-size: 1.6em;
  transition: 0.3s ease;
} 

.home .media-icons a:hover{
  transform: scale(1.2);
}


.slider-navigation{
  z-index: 888;
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  transform: translateY(80px);
  margin-bottom: 12px;
  gap:20px
}

.slider-navigation .nav-btn{
  width: 12px;
  height: 12px;
  background: #fff;
  border-radius: 50%;
  cursor: pointer;
  box-shadow: 0 0 2px rgba(255,255,255,0.5);
  transition: 0.3 ease; 
}

.slider-navigation .nav-btn.active{
  background: #f00;
}

.slider-navigation .nav-btn:hover{
  transform:scale(1.2)
}


.video-slide{
  position: absolute;
  width: 100%;
  clip-path: circle(2.9% at 0 50%);
}

.video-slide.active{
  clip-path: circle(150.0% at 0 50%);
  transition: 0.5s ease;
}




/* 介绍界面 */
.member-info {
  display: flex;
  align-items: center;
  width: 100%;
  /* height: 50px; 设置合适的高度 */
  height: 120px; /* 设置合适的高度 */
  padding: 15px;
  background-color: rgba(255, 255, 255, 0.1); /* 白色背景,透明度为0.1 */
  backdrop-filter: blur(10px); /* 添加模糊效果 */
  border-radius: 10px; /* 添加圆角 */
  margin: 10px;
}
.avatar-and-name {
  flex: 1;
  text-align: center;
}
.avatar {
  width: 70px;
  height: 70px;
  border-radius: 35px; /* 圆形头像 */
  margin-bottom: 10px; /* 名字和头像之间的距离 */
}
.name {
  display: block;
  color: #333;
  font-size: 20px;
}
.details {
  flex: 2;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  justify-content: center;
  padding-left: 20px; /* 与头像部分的间隔 */
}
.position, .experience {
  margin: 5px 0;
  font-size: 18px;
}


@media  (max-width:1040px) {
  header{
    padding:  12px 20px;
  }

  section{
    padding: 100px 20px;
  }
  .home .media-icons {
    right: 15px;

  }
  header .navigation{
    display: none;
  }

  header .navigation.active{
    position: fixed;
    width: 100%;
    height: 100vh ;
    top: 0;
    left: 0;
    display: flex;
    justify-content: center;
    align-items: center;
    background: rgba(1,1,1,0.5);
  }

  header .navigation .navigation-items a::before{
    background-color: rgb(0, 0, 0);
    height: 5px;
  }
  

  header .navigation.active .navigation-items {
    background: #fff;
    width: 600px;
    max-width: 600px;
    margin: 20px;
    padding: 40px;
    display: flex;
    flex-direction: column;
    align-items: center;
    border-radius: 5px;
    box-shadow: 0,5px 25px rgb(1, 1, 1)
   }

 

  header .navigation .navigation-items a{
    color: #000;
    font-size: 1.2em;
    margin: 20px;
 }

  .menu-btn{
    background: url(menu.png)no-repeat;
    background-size: 30px;
    background-position: center;
    width: 40px;
    height: 40px;
    cursor: pointer;
    transition: 0.3 ease;
  }
  .menu-btn.active{
    z-index: 999;
    background: url(close.png)no-repeat;
    background-size: 25px;
    background-position: center;
    transition: 0.3 ease

  }
}
</style>