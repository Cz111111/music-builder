<script setup>
import { useRouter } from "vue-router";
import { ref } from 'vue'
import { useTokenScore } from '../../../stores/token.js'
import {http} from '../../../http'
const router = useRouter()
const tokenScore = useTokenScore()

import asideMenu from "../../../components/menu.vue";
import userDropdown from "../../../components/dropdown.vue";
import scroll from "../../../components/scrollbox.vue";

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
    console.log('111信息:', tokenScore.token)
    try {
 //       showLoadingToast({
  //        message: '登陆中...',
  //      });
    const response = await http.post('/createsong', 
    {
      username:loginForm.value.username,
      password: loginForm.value.password
    },{
      headers:{
        'Authorization':tokenScore.token
      }
    }
    )
    alert(response.data.message)
    if (response.data.success) {
 //       closeToast();
        console.log('测试成功');
        const user = {
            userId: loginForm.value.username,
            password: loginForm.value.password,
            token:response.data.token
        };
 //       state.setUser(user);
 //       router.push({ name: 'index' })
    } else {
   //     closeToast();
        console.log('测试失败')
    }
    } catch (error) {
   //     closeToast();
        console.log('测试失败，请稍后重试')
        console.error(error)
    }
    loginForm.value = {// 清空表单
        username: '',
        password: ''
    }
}



</script>


<template>
  <div class="common-layout">
    <el-container>
      <el-aside width="120px">
        <asideMenu></asideMenu>
      </el-aside>
      <el-container>
        <el-header class = "header">
            <el-row>
                <el-col :span="4" class = "head">
                    <h2>这是袁轲的功能</h2>
                </el-col>
                <el-col :span="20">
                    <userDropdown></userDropdown>
                </el-col>
            </el-row>
          
        </el-header>
        <el-main class = "main">
          <el-row :gutter="12"  class="row-box">
            
            <el-col  :span="24">
                <form id="registerForm" @submit.prevent="handleSubmit">
                <div class="form-group">
                    <label for="test1">test1</label>
                    <div class="input">
                        <div class="icons"><el-icon :size="17"><User style="color: #7c7c7c;"/></el-icon></div>
                        <input type="text" id="username" name="username" placeholder="test1" v-model="loginForm.username">
                    </div>
                </div>
                <div class="form-group">
                    <label for="test2">test2</label>
                    <div class="input">
                        <div class="icons"><el-icon :size="17"><Lock style="color: #7c7c7c;" /></el-icon></div>
                        <input type="password" id="password" name="password" placeholder="test2" v-model="loginForm.password">
                    </div>
                </div>
                <button type="submit" class="submit">test</button>
            </form>
            </el-col>
          </el-row>
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<style scoped>
.head{
    top:40px;
    left:20%;
}
.header{
  background-color:rgba(19, 15, 15, 0.977);
  border-left:none;
  color:white;
}
.main{
  background-color:rgba(19, 15, 15, 0.977);
  border-left: none;
  color:azure;
}
.el-row{
    margin-bottom: 20px;
}
.row-box {

  display: flex;
  flex-flow: wrap;
}
.row-box .el-card {
  min-width: 0%;
  margin-left:0px;
  height: 100%;
  top:10%;
  border: 0px;
  box-shadow: 0 2px 12px 0 rgb(0 0 0 / 10%);
  background-color:rgb(40, 39, 39);
  
}
.el-card{
  color:azure;
}
.aside-menu{
  height: 100vh;
  border-right:none;
}
</style>


