<template>
  <div class="form">
    <div class="file-picker">
      <input type="file" id="MusicFileInput" accept=".mp3" @change="handleFileChange" >
      <button @click="selectmusic" style="background-color: red; color: white; border: none; padding: 10px 20px; font-size: 16px; cursor: pointer;">上传音频文件</button>
    </div>
    <div class="file-picker">
      <input type="file" id="lrcFileInput" accept=".lrc">
      <button id="uploadButton" @click="selectLrc" style="background-color: red; color: white; border: none; padding: 10px 20px; font-size: 16px; cursor: pointer;">上传歌词文件</button>
    </div>
    <el-button type="danger" @click="submit()">开始k歌</el-button>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import { filehttp } from '../http/index.js';
import { useRouter } from 'vue-router';
import { ElLoading } from 'element-plus'

const fullscreenLoading = ref(false)
onMounted(() => {
  //window.location.reload(); // 刷新当前页面
});
const file = ref(null);
const router = useRouter();
const handleFileChange = (event) => {
  // 当文件选择后，更新 file 数据属性
  file.value = event.target.files[0];
};

const selectmusic = async () => {
  const url = '/ksong/sing/upload'; // 你的上传接口URL
  let formData = new FormData();
  formData.append('file', file.value);
  const fileName = file.value.name;

  // 移除文件扩展名
  const fileBaseName = fileName.substring(0, fileName.lastIndexOf('.'));
  // 将不包含扩展名的文件名存储到 localStorage
  localStorage.setItem('Name', fileBaseName);
  const loading = ElLoading.service({
    lock: true,
    text: '加载中',
    background: 'rgba(0, 0, 0, 0.7)',
  })
  const response = await filehttp.post(url, formData);
  //罩子消失
  localStorage.setItem('responseData', JSON.stringify(response.data));
  loading.close()
};
const selectLrc = () => {
  // 获取按钮和文件输入元素
  var uploadButton = document.getElementById('uploadButton');
  var lrcFileInput = document.getElementById('lrcFileInput');
  // 为按钮添加点击事件监听器
  uploadButton.addEventListener('click', function () {
    var file = lrcFileInput.files[0]; // 获取文件列表中的第一个文件对象
    if (file) {
      var reader = new FileReader();
      // 读取文件内容的回调函数
      reader.onload = function (e) {
        var lrcContent = e.target.result; // 读取到的文件内容
        localStorage.setItem('lrcData', lrcContent); // 存储到localStorage
        // 这里应该在文件读取完成后再打印
        console.log(lrcContent);
      };
      reader.onerror = function (e) {
        console.error('读取文件时发生错误:', e);
      };
      // 开始读取文件内容
      reader.readAsText(file);
    }
  });
};

const submit = async () => {
  // const loading = ElLoading.service({
  //   lock: true,
  //   text: '加载中',
  //   background: 'rgba(0, 0, 0, 0.7)',
  // })
  router.push('/kmain');
}


</script>

<style scoped>
.form{
  max-width: 1280px;
  margin: 0 auto;
  padding: 2rem;
  text-align: center;
}
.loading-modal {
  display: none; /* 默认隐藏 */
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5); /* 半透明背景 */
  justify-content: center;
  align-items: center;
  z-index: 1000; /* 确保在页面最上层 */
}
.loading-content {
  text-align: center;
  padding: 20px;
  background: white;
  border-radius: 5px;
}
h1 {
  color: #42b983;
}


.file-picker {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
  /* 根据需要调整间距 */
}

.input-with-button {
  flex: 1;
  margin-left: 20px;
  /* 增加与按钮的水平间距 */
}
/* ::v-deep .el-loading-spinner .path{
  stroke:#42b983 !important;
} */
.el-loading-spinner .el-loading-text{
  color:#f00 !important;
}

</style>