<script setup>
import { useRouter } from "vue-router";
import { ref } from 'vue'
import { useTokenScore } from '../../../stores/token.js'
import { useUsername } from '../../../stores/username.js'
import {http} from '../../../http'
const router = useRouter()
const tokenScore = useTokenScore()
const usernameScore = useUsername()

import asideMenu from "../../../components/menu.vue";
import userDropdown from "../../../components/dropdown.vue";
import scroll from "../../../components/scrollbox.vue";

const songForm=ref({
    songName: '',
    songWord: '',
    address: ''
})


const handleSubmit = async() => {
    if (!songForm.value.songname
    ||!songForm.value.songword
    ||!songForm.value.address) {
        alert('请填写完整数据')
        return
    }
    console.log('提交的信息:', songForm.value)
    console.log(usernameScore.username)
    // 模拟提交数据
    try {
 //       showLoadingToast({
  //        message: '登陆中...',
  //      });
    const response = await http.post('/song/insert', 
    {
      username:usernameScore.username,
      songname:songForm.value.songname,
      songword:songForm.value.songword,
      address:songForm.value.address
    },{
      headers:{
        'Authorization':tokenScore.token
      }
    }
    )
    alert(response.data.message)
    if (response.data.success) {
 //       closeToast();
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
    songForm.value = {// 清空表单
      songname: '',
      songword: '',
      address: ''
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
                <form id="createsongForm" @submit.prevent="handleSubmit">
                <div class="form-group">
                    <label for="歌曲名">歌曲名</label>
                    <div class="input">
                        <div class="icons"><el-icon :size="17"><User style="color: #7c7c7c;"/></el-icon></div>
                        <input type="text" id="songname" name="songname" placeholder="歌曲名" v-model="songForm.songname">
                    </div>
                </div>
                <div class="form-group">
                    <label for="歌词">歌词</label>
                    <div class="input">
                        <div class="icons"><el-icon :size="17"><Lock style="color: #7c7c7c;" /></el-icon></div>
                        <input type="text" id="songword" name="songword" placeholder="歌词" v-model="songForm.songword">
                    </div>
                </div>
                <div class="form-group">
                    <label for="地址">地址</label>
                    <div class="input">
                        <div class="icons"><el-icon :size="17"><Lock style="color: #7c7c7c;" /></el-icon></div>
                        <input type="text" id="address" name="address" placeholder="地址" v-model="songForm.address">
                    </div>
                </div>
                <button type="submit" class="submit">确定</button>
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


