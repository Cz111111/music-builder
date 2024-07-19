<template>
    <el-dropdown>
      <span class="el-dropdown-link">
        {{ usernameScore.username }}
        <el-icon class="el-icon--right">
          <arrow-down />
        </el-icon>
      </span>
      <template #dropdown>
        <el-dropdown-menu class = "el-dropdown-link1">
          <el-dropdown-item>退出登录</el-dropdown-item>
     <!--      <el-dropdown-item>Action 3</el-dropdown-item>
          <el-dropdown-item disabled>Action 4</el-dropdown-item>
          <el-dropdown-item divided>Action 5</el-dropdown-item>--> 
        </el-dropdown-menu>
      </template>
    </el-dropdown>

  </template>
  
  <script lang="ts" setup>
  import { ArrowDown } from '@element-plus/icons-vue'
import { useUsername } from '../stores/username.js'
const usernameScore = useUsername()

  </script>
  <style scoped>
  .el-dropdown-link {
    cursor: pointer;
  /*   color: rgb(150, 150, 150); */
    display: flex;
   align-items: center;
    outline: unset;
/*     background-color:rgba(19, 15, 15, 0.977); */
  }
  .el-dropdown{
 /*   min-width: 100%;*/
    height: 40px;
    left:10%;
    top:30%;
    
  }
  </style>
  