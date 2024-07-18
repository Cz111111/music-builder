<template>
    <div class="common-layout">
    <el-container>
      <el-aside width="150px">
        <asideMenu></asideMenu>
      </el-aside>
      <el-container>
        <el-header class="header">
          <el-row>
            <el-col :span="4" class="head">
              <h2>导出文件</h2>
            </el-col>
            <el-col :span="20">
              <userDropdown></userDropdown>
            </el-col>
          </el-row>
        </el-header>
        <el-main class="main">
  <div class="form">
    <el-form  :model="form" label-width="120px">
    <el-form-item label="文件选择">
    <div class ="dropdown">
      <el-select v-model="form.name" placeholder="请选择文件" >
        <el-option v-for="file in fileList" :key="file.name"  :label="file.name" :value="file.name" />
      </el-select>
    </div>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="download">下载文件</el-button>
    </el-form-item>
  </el-form>
  </div>
      </el-main>
      </el-container>


    </el-container>
  </div>

</template>

<script lang="ts" setup>
import { ref, onMounted, reactive } from 'vue'
import { http } from '../../../http/index.js';
import { useRouter } from 'vue-router';
import { ElLoading } from 'element-plus'
import { useUsername } from '../../../stores/username.js'
import asideMenu from "../../../components/menu.vue"


const fileList = ref([]);
const router = useRouter();
const usernameScore = useUsername()
const form = ref({
  name:''
});
const fetchObjectList = async () => {
    try {
        const response = await http.post('/download/getUserFile',
    {
      username:usernameScore.username,
      name:null,
      address:null
      
    },{
      headers:{
        'Authorization':localStorage.getItem("tokenTest")
      }
    }
    )
        // 将后端返回的数据赋值给响应式状态
        fileList.value = response.data;
        console.log(response.data)
    } catch (error) {
        console.error('Error fetching object list:', error);
    }
};

const download = async () => {
    console.log( form.value.name)
  let formData = new FormData();
  formData.append('name', form.value.name);

  try {
    // 发送POST请求并设置响应类型为blob
    const response = await http.post('/download/download', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        'Authorization': localStorage.getItem("tokenTest")
      },
      responseType: 'blob' // 重要：设置响应类型为blob以处理二进制文件数据
    });

    const contentDisposition = response.headers['content-disposition'];
    const fileName = contentDisposition ? contentDisposition.split('filename=')[1].replace(/['"]/g, '') : form.value.name;

    // 创建一个可下载的链接
    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', fileName); // 设置下载文件的名称
    document.body.appendChild(link); // 将链接添加到DOM中
    link.click(); // 模拟点击事件开始下载

    // 清理
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
    alert('文件导出成功！');
  } catch (error) {
    console.error('下载失败:', error);
    alert('导出失败，请稍后重试或联系管理员！');
  }
}

onMounted( fetchObjectList);
</script>

<style scoped>
.dropdown{
    width: 600px;
}
.head {
  top: 40px;
  left: 20%;
}

.header {
  background-color: rgba(19, 15, 15, 0.977);
  border-left: none;
  color: white;
  border-bottom: 1px solid #ccc;
  /* 添加底部边框 */
}

.main {
  background-color: rgba(19, 15, 15, 0.977);
  border-left: none;
  color: azure;
}
.form{
  display: flex;
  flex-direction: column; /* 保持内部元素的排列方式 */
  justify-content: center; /* 垂直方向居中 */
  align-items: center; /* 水平方向居中 */
  height: 100%; /* 设置父容器高度为视口高度 */
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