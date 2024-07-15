<template>
<!--     <div class="file-upload-component">
      <form @submit.prevent="handleUpload" class="upload-form">
        <div class="form-group">
          <label for="fileInput">选择文件上传:</label>
          <input type="file" id="fileInput" @change="selectFile" />
        </div>
        <div class="form-group">
          <button type="submit" :disabled="!fileSelected">上传文件</button>
        </div>
      </form>
      <div v-if="uploadMessage" class="upload-message">
        {{ uploadMessage }}
      </div>
    </div> -->
    <div class="side-by-side-component">
      <el-button
        type="primary"
        class="square-button"
        @click="openDialog"
      >
      上传文件
      </el-button>
      <!-- 添加el-dialog组件 -->
      <el-dialog v-model="dialogVisible" title="上传文件" width="500">
        <el-form :model="form" @submit.prevent="handleUpload">
          <el-form-item label="文件" :label-width="formLabelWidth">
            <input input type="file" id="wavname" @change="handleChange" />
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
  import {http} from '../http/index.js'
  
  const fileSelected = ref(false);
  const fileData = ref(null);
  const uploadMessage = ref('');
  
  const handleChange = (event) => {
  const file = event.target.files[0];
  if (file) {
    form.value.wavname = file;
  }
};

function openDialog() {
  dialogVisible.value = true; // 打开弹窗
}


  const selectFile = (event) => {
    const file = event.target.files[0];
    if (file) {
      fileData.value = file;
      fileSelected.value = true;
    } else {
      fileSelected.value = false;
    }
  };
  
  const handleUpload = async () => {

            // 创建 FormData 对象
    let formData = new FormData();
    // 将文件数据添加到 FormData 对象中
    formData.append('file', fileData.value);
    const response =await http.post('/upload',// 替换为你的上传API端点
        formData,{
        headers:{
        'Authorization':localStorage.getItem("tokenTest")}}
    );
  
    alert(response.data.message)
  
    fileSelected.value = false; // 重置文件选择状态
    fileData.value = null; // 重置文件数据
  };
  </script>
  
  <style scoped>
  /* 你可以添加自己的样式 */
  .upload-form {
  display: flex;           /* 表单内部也使用Flexbox */
  flex-direction: column; /* 表单项垂直排列 */
  align-items: center;     /* 表单项水平居中 */
}
  .form-group {
    margin-bottom: 1em;
  }
  .upload-message {
    margin-top: 1em;
    color: green;
  }

  .side-by-side-component {
  display: flex;
  align-items: center;
}
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
.text-container {
  flex-grow: 1;
  margin-left: 50px;
  /* 其他样式保持不变 */
}
  </style>