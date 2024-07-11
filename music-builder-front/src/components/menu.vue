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
        <el-menu-item index="/index" class = "menu1">
            <span>这是logo</span>
            </el-menu-item>
        <el-menu-item index="/index/createsong" class = "menu2">
            <el-icon><icon-menu /></el-icon>
            <span>编曲创作</span>
        </el-menu-item>
        <el-menu-item index="3" class = "menu3">
          <el-icon><Mic /></el-icon>
          <span>歌唱评分</span>
        </el-menu-item>
        <el-menu-item index="4" class = "menu4">
          <el-icon><Document /></el-icon>
          <span>我的草稿</span>
        </el-menu-item>
        <el-menu-item index="/index/mysong" class = "menu5">
          <el-icon><Headset /></el-icon>
          <span>我的歌曲</span>
        </el-menu-item>
        <el-menu-item index="6" class = "menu6">
          <el-icon><FolderOpened /></el-icon>
          <span>导出文件</span>
        </el-menu-item>
      </el-menu>
    </el-col>
  </el-row>


</template>

<style scoped>
.col{
  padding: 0px;
}
.menu1{
  height:100px;
}
.menu2{
  height:100px;
}
.menu3{
  height:100px;
}
.menu4{
  height:100px;
}
.menu5{
  height:100px;
}
.menu6{
  height:100px;
}
  .el-menu-vertical-demo{
    background-color:rgb(40, 39, 39);
    height: 100vh;
    border-right:none;
  }
</style>
