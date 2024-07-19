<template>
  <div class="side-by-side-component">
    <!-- <el-button
      type="primary"
      class="square-button"
      @click="openDialog"
    >
    AI音乐生成
    </el-button>
    <div class="text-container">
      <el-col :span="12">
      AI根据描述性质的关键词（英文）生成一段音乐可以在关键词中指定是否有人声
      </el-col>
    </div> -->
    <div class="vertical-layout">
    <h2>AI音乐生成</h2>
    <p>根据关键词生成你想要的音乐</p>
    <button @click="openDialog">开始创作</button>
    </div>
    <!-- 添加el-dialog组件 -->
    <el-dialog v-model="dialogVisible" title="AI音乐生成" width="500">
      <el-form :model="form" @submit.prevent="handleSubmit">
        <el-form-item label="关键词（最好英文）" :label-width="formLabelWidth">
          <el-input v-model="form.keyword" autocomplete="off" />
        </el-form-item>
        <el-form-item label="生成文件名:" :label-width="formLabelWidth">
          <el-input v-model="form.name" autocomplete="off" />
        </el-form-item>
        <button type="submit">确定</button>
      </el-form>
      <template #footer>
      </template>
    </el-dialog>
  </div>
</template>
<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
 import {http} from '../http/index.js'
/*  import { ElLoading } from 'element-plus'; */
const router = useRouter();
const dialogVisible = ref(false); // 控制弹窗显示的响应式引用

const form = ref({
keyword: '',
name:''
});


function openDialog() {
dialogVisible.value = true; // 打开弹窗
}

function confirmAction() {
// 这里可以放置点击确定后需要执行的逻辑
dialogVisible.value = false; // 关闭弹窗
}




const handleSubmit = async () => {
console.log(form.value)
dialogVisible.value = false; // 关闭弹窗
if (!form.value.keyword) {
  alert('请填写完整数据')
  return
}
let formData = new FormData();
  formData.append('keyword', form.value.keyword);
  formData.append('name', form.value.name);
try {
/*   const loading = ElLoading.service({
    lock: true,
    text: '加载中',
    background: 'rgba(0, 0, 0, 0.7)',
  }) */
  const response = await http.post('render/five', 
      formData,{
    headers:{
      'Authorization':localStorage.getItem("tokenTest")
    }
  }
  )
  alert("AI音乐生成成功")
  if (response.data.success) {
    console.log('提交成功');
    // 这里可以添加提交成功后的逻辑，例如跳转到另一个页面
  } else {
    console.log('提交失败');
    alert("生成失败")
  }
/*   loading.close(); */
} catch (error) {
  console.error('提交失败:', error)
}

// 清空表单
form.value = {
  keyword: '',
  name:''
}
}
</script>

<style scoped>
.vertical-layout {
display: flex;
flex-direction: column;
align-items: center;
text-align: center;
}

.side-by-side-component {
display: flex;
justify-content:center;  
align-items: center;
background-color:  #FFA500;
/* 深红色背景 */
color: white;
/* 白色文字 */
padding: 20px;
/* 内边距 */
text-align: center;
/* 文字居中 */
border-radius: 8px;
/* 圆角边框 */
box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
/* 阴影效果 */
max-width:400px;
height: 150px;
/* 最大宽度 */
margin: 0 auto;
/* 水平居中 */
}

.side-by-side-component h2 {
font-size: 24px;
/* 标题字号 */
margin-bottom: 10px;
/* 标题下方间距 */
}

.side-by-side-component p {
margin-bottom: 20px;
/* 描述文字下方间距 */
font-size: 16px;
/* 描述文字字号 */
}

.side-by-side-component button {
background-color:  #ce9731;
/* 按钮背景色 */
color: white;
/* 按钮文字颜色 */
border: none;
/* 去掉边框 */
padding: 10px 20px;
/* 按钮内边距 */
font-size: 16px;
/* 按钮文字字号 */
border-radius: 4px;
/* 按钮圆角 */
cursor: pointer;
/* 鼠标悬停样式 */
transition: background-color 0.3s ease;
/* 鼠标悬停过渡效果 */
}

.side-by-side-component button:hover {
background-color:  #d79928;
/* 按钮鼠标悬停背景色 */
}



.side-by-side-component {
display: flex;
align-items: center;
}

.side-by-side-component {
display: flex;
align-items: center;
}
.side-by-side-component {
display: flex;
align-items: center;
}
.square-button {
width: 200px; /* 设置宽度为100px */
height: 100px; /* 设置高度为100px，与宽度相同，形成正方形 */
padding: 0; /* 移除内边距，确保按钮大小不变 */
display: flex;
justify-content: center;
align-items: center;
background-color: darkgrey;
margin-left: 100px;
/* 其他样式保持不变 */
}
.text-container {
flex-grow: 1;
margin-left: 50px;
/* 其他样式保持不变 */
}
</style>