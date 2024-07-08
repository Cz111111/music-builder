<script setup>
import { ref } from 'vue'
import { useRouter } from "vue-router"
import {http} from '../../http'
const router = useRouter()
const registerInfo=ref({
      username: '',
      email: '',
      name: '',
      password: '',
      confirmPassword:'',
    })

const handleSubmit = async() => {
    if (!registerInfo.value.username 
    ||!registerInfo.value.name 
    ||!registerInfo.value.email
    ||!registerInfo.value.password
    ||!registerInfo.value.confirmPassword ) {
        alert('请填写完整数据')
        return
    }

    if (registerInfo.value.password !== registerInfo.value.confirmPassword) {
        alert('密码和确认密码不匹配')
        return
    }

    // 模拟提交数据
    console.log('提交的注册信息:', registerInfo.value)
    try {
 //       showLoadingToast({
  //        message: '登陆中...',
  //      });
    const response = await http.post('/user/register', {
      username: registerInfo.value.username,
      password: registerInfo.value.password,
      email:registerInfo.value.email,
      nickname:registerInfo.value.name
    })
    alert(response.data.message)
    if (response.data.success) {
 //       closeToast();
        console.log('注册成功');
        const user = {
            userId: registerInfo.value.username,
            password: registerInfo.value.password
        };
        router.push({ name: 'login' })
 //       state.setUser(user);
    } else {
   //     closeToast();
        console.log('注册失败')
    }
    } catch (error) {
   //     closeToast();
        console.log('注册失败，请稍后重试')
        console.error(error)
    }
    // 清空表单
    registerInfo.value = {
        username: '',
        email: '',
        name: '',
        password: '',
        confirmPassword: ''
    }

    // 跳转到登录页面
    
}

</script>

<template>
  <div class="common-layout">
    <el-container>
      <el-header class="header">Header</el-header>
      <el-main class="main">
        <div class="register-container">
            <div class="rd">
                <form id="registerForm"  @submit.prevent="handleSubmit">
                    <div class="form-group">
                        <label for="username">账号</label>
                        <div class="input">
                            <div class="icons"><el-icon :size="17"><User style="color: #7c7c7c;"/></el-icon></div>
                            <input type="text" id="username" name="username" placeholder="请输入账号" v-model="registerInfo.username">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="email">邮箱</label>
                        <div class="input">
                            <div class="icons"><el-icon :size="17"><Message style="color: #7c7c7c;"/></el-icon></div>
                            <input type="text" id="email" name="email" placeholder="请输入邮箱" v-model="registerInfo.email">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="name">昵称</label>
                        <div class="input">
                            <div class="icons"><el-icon :size="17"><EditPen style="color: #7c7c7c;"/></el-icon></div>
                            <input type="text" id="name" name="name" placeholder="请输入昵称" v-model="registerInfo.name">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="password">密码</label>
                        <div class="input">
                            <div class="icons"><el-icon :size="17"><Lock style="color: #7c7c7c;" /></el-icon></div>
                            <input type="password" id="password" name="password" placeholder="请输入密码" v-model="registerInfo.password">
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="confirmPassword">确认密码</label>
                        <div class="input">
                            <div class="icons"><el-icon :size="17"><Lock style="color: #7c7c7c;" /></el-icon></div>
                            <input type="password" id="confirmPassword" name="confirmPassword" placeholder="请输入密码" v-model="registerInfo.confirmPassword">
                        </div>
                    </div>
                    <div class="form-group">
                        <button type="submit" class="submit" >注册</button>
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
    background-color: #ffffff;
    padding: 20px;
    border-radius: 10px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    width: 350px;
    height:520px;
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

#username, #password ,#name,#confirmPassword,#email{
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
</style>

