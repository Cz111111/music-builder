<script setup>
import { useRouter } from "vue-router";
import { ref } from 'vue'
import { useTokenScore } from '../../../stores/token.js'
import { useUsername } from '../../../stores/username.js'
const router = useRouter()
const tokenScore = useTokenScore()
const usernameScore = useUsername()

import asideMenu from "../../../components/menu.vue";
import userDropdown from "../../../components/dropdown.vue";
import sidebyside1 from "../../../components/sidebyside1.vue";
import sidebyside2 from "../../../components/sidebyside2.vue";
import sidebyside3 from "../../../components/sidebyside3.vue";
import sidebyside4 from "../../../components/sidebyside4.vue";
import sidebyside5 from "../../../components/sidebyside5.vue";

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
                    <h2>AI创作</h2>
                </el-col>
                <el-col :span="20">
                    <userDropdown></userDropdown>
                </el-col>
            </el-row>
          
        </el-header>
        <el-main class = "main">
          <el-row :gutter="12"  class="row-box"> 
              <sidebyside1/>
          </el-row>
          <el-row :gutter="12"  class="row-box"> 
              <sidebyside2/>
          </el-row>
          <el-row :gutter="12"  class="row-box"> 
              <sidebyside3/>
          </el-row>
          <el-row :gutter="12"  class="row-box"> 
              <sidebyside4/>
          </el-row>
          <el-row :gutter="12"  class="row-box"> 
              <sidebyside5/>
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


.aside-menu{
  height: 100vh;
  border-right:none;
}
.el-aside{
  background-color:rgb(40, 39, 39);
}
</style>


