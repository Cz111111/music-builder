<script setup>
import { useRouter } from "vue-router";
import { ref } from 'vue'
const router = useRouter()

import asideMenu from "../../components/menu.vue";
import userDropdown from "../../components/dropdown.vue";
import uploadfile from "@/components/uploadfile.vue";

const largeButtonRef = ref(null); // 创建一个引用，用于获取按钮元素

// 定义一个方法用于处理按钮点击事件
function handleCreateButtonClick() {
  // 使用Vue Router进行跳转
  router.push('/index/createsong');
}

function handleImportButtonClick() {
  // 使用Vue Router进行跳转
  router.push('/your-destination-path');
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
          <userDropdown></userDropdown>
        </el-header>
        <el-main class = "main">
          <el-row :gutter="24"  class="row-box">
            
            <el-col  :span="12">
            <div class="button-container">
              <el-button 
                class="square-button" 
                type="primary" 
                @click="handleCreateButtonClick"
                ref="largeButtonRef">
                开始创作
              </el-button>
            </div>
            </el-col>
            <el-col  :span="12">
              <div class="upload-container">
              <!-- uploadFile 组件 -->
                <uploadfile></uploadfile>
              </div>
            </el-col>
          </el-row>
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<style scoped>
.el-aside{
  background-color:rgb(40, 39, 39);
}
.button-container {
  display: flex;
  justify-content: center; /* 水平居中 */
/*   align-items: center; /* 垂直居中 */ 
  margin-top: 200px; /* 添加上边距 */
}

/* 按钮样式 */
.square-button {
  width: 200px; /* 按钮宽度，根据需要调整 */
  height: 200px; /* 按钮高度，接近正方形的比例 */
  padding: 0; /* 移除内边距 */
  text-align: center;
  line-height: 200px; /* 使文字垂直居中，与按钮高度一致 */
  font-size: 20px; /* 根据需要调整按钮文字大小 */
  color: #fff; /* 按钮文字颜色 */
  background-color: #4f5356; /* 按钮背景颜色 */
  border: none; /* 移除边框 */
  border-radius: 10px; /* 根据需要调整圆角 */
}

@media (max-width: 768px) {
  .square-button {
    width: 150px;
    height: 150px;
    line-height: 150px;
    font-size: 16px;
  }
}
.header{
  background-color:rgba(19, 15, 15, 0.977);
  border-left:none;
}
.main{
  background-color:rgba(19, 15, 15, 0.977);
  border-left: none;
  color:azure;
}
.el-row {
  display: flex;
  justify-content: center; /* 水平居中 */
  margin-bottom: 20px;
}
.upload-container {
  display: flex;
  justify-content: center; /* 水平居中 */
  width: 100%; /* 占满其父容器的宽度 */
  margin-top: 300px; /* 添加上边距 */
}
.row-box {

  display: flex;
  flex-flow: wrap;
}
.aside-menu{
  height: 100vh;
  border-right:none;
}
</style>


