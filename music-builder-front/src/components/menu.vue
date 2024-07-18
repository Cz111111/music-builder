<script setup>
import {
  Document,
  Menu as IconMenu,
  Location,
  Setting,
} from '@element-plus/icons-vue'
const handleOpen = (key, keyPath) => {
  console.log(key, keyPath)
}
const handleClose = (key, keyPath) => {
  console.log(key, keyPath)
}
</script>

<template>
  <div class="wrapper">
  <el-row class="tac">
    <el-col :span="24" class = "col">
      <el-menu
        active-text-color="#ffd04b"
        background-color="black"
        class="el-menu-vertical-demo"
        default-active="2"
        text-color="#fff"
        @open="handleOpen"
        @close="handleClose"
        style="width: 100%"
        :router="true"
      >
        <el-menu-item  class = "menu1">
          <!-- <img :src="'/src/assets/logo.jpg'" /> -->
          <h2 style="color: white; text-shadow: -1px -1px 0 #ff0000, 1px -1px 0 #ff0000, -1px 1px 0 #ff0000, 1px 1px 0 #ff0000;">音籁ML</h2>
            </el-menu-item>
        <el-menu-item index="/index/createsong" class = "menu2">
            <el-icon><icon-menu /></el-icon>
            <span>AI创作</span>
        </el-menu-item>
        <el-menu-item index="../../upload" class = "menu3">
          <el-icon><Mic /></el-icon>
          <span>K歌评分</span>
        </el-menu-item>
        <el-menu-item index="4" class = "menu4">
          <el-icon><Document /></el-icon>
          <span>音乐创作</span>
        </el-menu-item>
        <el-menu-item index="/index/mysong" class = "menu5">
          <el-icon><Headset /></el-icon>
          <span>我的歌曲</span>
        </el-menu-item>
        <el-menu-item index="/index/download" class = "menu6">
          <el-icon><Document /></el-icon>
          <span>导出文件</span>
        </el-menu-item>
      </el-menu>
    </el-col>
  </el-row>

</div>
</template>

<style scoped>
.wrapper {
  height: 100vh; /* 设置容器高度为视口高度 */
  overflow: hidden; /* 隐藏滚动条 */
}
img {
  max-width: 100%; /* 图片最大宽度不超过其父容器的宽度 */
  max-height: 100%; /* 图片最大高度不超过其父容器的高度 */
}
.col{
  padding: 0px;
  padding-right: 0px;
}
.menu1{
  height:120px;
  padding: 0px;
  margin-left: 0%;
  margin-right: 0%;
}
.menu2{
  height:90px;
  margin-right: 0%;
  padding: 0px;
}
.menu3{
  height:90px;
  margin-right: 0%;
  padding: 0px;
}
.menu4{
  height:90px;
  margin-right: 0%;
  padding: 0px;
}
.menu5{
  height:90px;
  margin-right: 0%;
  padding: 0px;
}
.menu6{
  height:90px;
  margin-right: 0%;
  padding: 0px;
}
  .el-menu-vertical-demo{
    background-color:rgb(40, 39, 39);
    height: 100vh;
    border-right:none;
  }
</style>
