<script setup>
import { useRouter } from "vue-router";
import { ref } from 'vue'
import {http} from '../../http'
import {useTokenScore} from '../../stores/token.js'

const tokenScore=useTokenScore()
const router = useRouter()
const toRegister=()=>{
    router.push({ name: 'register' });
}

const loginForm=ref({
    username: '',
    password: ''
})


const handleSubmit = async() => {
    if (!loginForm.value.username
    ||!loginForm.value.password ) {
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
    
    if (response.data.success) {
 //       closeToast();
        console.log('登录成功');
        tokenScore.setToken(response.data.data.token)
/*         const user = {
            userId: loginForm.value.username,
            password: loginForm.value.password,
            token:response.data.token
        }; */
 //       state.setUser(user);
        router.push({ name: 'render' })
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
    
   


</script>

<template>
  <div class="common-layout">
    <el-container>
      <el-header class="header">Header</el-header>
      <el-main class="main">
        <div class="register-container">
        <div class="title">登录</div>
        <div class="rd">
            <form id="registerForm" @submit.prevent="handleSubmit">
                <div class="form-group">
                    <label for="username">账号</label>
                    <div class="input">
                        <div class="icons"><el-icon :size="17"><User style="color: #7c7c7c;"/></el-icon></div>
                        <input type="text" id="username" name="username" placeholder="请输入账号" v-model="loginForm.username">
                    </div>
                </div>
                <div class="form-group">
                    <label for="password">密码</label>
                    <div class="input">
                        <div class="icons"><el-icon :size="17"><Lock style="color: #7c7c7c;" /></el-icon></div>
                        <input type="password" id="password" name="password" placeholder="请输入密码" v-model="loginForm.password">
                    </div>
                </div>
                <div class="form-group">
                    <div class="text" >忘记密码？</div>
                </div>
                
                
                <button type="submit" class="submit">登录</button>
                <div class="form-group">
                    <div class="form-group-item">
                        <div class="topinfo">第三方登录</div>
                        <div class="coreinfo">
                            <img src="../../assets/wechaticon.png"/>
                            <img src="../../assets/qqicon.png"/>
                            <img src="../../assets/githubicon.png"/>
                        </div>
                        <div class="bottominfo" @click="toRegister">没有账号？立即注册</div>
                    </div>
                </div>
            </form>
        </div>
    </div>
    </el-main>
      
    </el-container>
  </div>
</template>

<style scoped>
.common-layout{
    height: 100%;
}

.el-container{
    height: 100%;
    
}

.header{
    height: 40px;
    background-color:#ffffff;
}






.main{
    height: 700px;
    background-color: rgb(220, 220, 220);
    display: flex;
    justify-content: center;
    align-items: center;
    margin: 0;
    padding: 0;

    /* background-image: url(../../assets/post.webp);
    background-repeat: no-repeat;
    background-attachment: fixed;
    background-size: cover; */
    
}

.register-container {
    position: relative;
    left: 380px;
    background-color: #ffffff;
    padding: 20px;
    border-radius: 10px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    width: 350px;
    height:500px;
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
.text{
    text-align: end;
    font-weight: 550;
    font-size: 12px;
    color: #6a6363;
    position: relative;
    bottom: 22px;
}
.input{
    display: flex;
    border-bottom: 1.5px solid #a9a9a9;
}

.icons{
    margin-right: 10px;
    height: 40px;
    width: 20px;
    display: flex;
    justify-content: center;
    align-items: center;
    
}

#username, #password {
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

.form-group-item{
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
}

.topinfo,
.coreinfo,
.bottominfo{
    height: 32px;
    line-height:32px;
    text-align: center;
    font-weight: 550;
    font-size: 12px;
    color: #6a6363;
}

.bottominfo{
    text-decoration: underline;
    cursor: pointer;
}

.coreinfo{
    display: flex;
    justify-content: space-between;
    gap: 8px;
    margin-top: 10px;
    margin-bottom: 10px;
}

img{
    cursor: pointer;
}

</style>
