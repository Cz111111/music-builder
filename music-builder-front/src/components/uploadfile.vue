<template>
    <div class="file-upload-component">
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
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import {http} from '../http/index.js'
  
  const fileSelected = ref(false);
  const fileData = ref(null);
  const uploadMessage = ref('');
  
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
  </style>