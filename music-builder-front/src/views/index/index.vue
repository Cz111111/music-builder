<script setup>
import { useRouter } from "vue-router";
import { ref } from 'vue'
const router = useRouter()

import asideMenu from "../../components/menu.vue";
import userDropdown from "../../components/dropdown.vue";
import card from "../../components/card.vue";
</script>


<template>
  <div class="common-layout">
    <el-container>
      <el-aside width="120px">
        <asideMenu></asideMenu>
      </el-aside>
      <el-container>
        <el-header class = "header">
          <userDropdown></userDropdown>
        </el-header>
        <el-main class = "main">
          <el-row>
            <p>最近打开</p>
          </el-row>
          <el-row :gutter="12"  class="row-box">
            
<!--             <el-col  :span="8">
                <card shadow="always" class="el-card"></card>
            </el-col>
            <el-col  :span="8">
                <card shadow="always" class="el-card"></card>
            </el-col>
            <el-col  :span="8">
                <card shadow="always" class="el-card"></card>
            </el-col> -->
          </el-row>
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<style scoped>
.header{
  background-color:rgba(19, 15, 15, 0.977);
  border-left:none;
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


