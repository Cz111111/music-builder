<script setup>
import { defineProps,defineEmits} from 'vue';
import {http} from '../http/index.js'
const props =  defineProps(['item']);
const emit = defineEmits(['refreshParent']);


const handleDownload = async () => {
  let formData = new FormData();
  formData.append('songname', props.item.songname );

  try {
    // 发送POST请求并设置响应类型为blob
    const response = await http.post('/song/download', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        'Authorization': localStorage.getItem("tokenTest")
      },
      responseType: 'blob' // 重要：设置响应类型为blob以处理二进制文件数据
    });


    // 创建一个可下载的链接
    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', props.item.songname); // 设置下载文件的名称
    document.body.appendChild(link); // 将链接添加到DOM中
    link.click(); // 模拟点击事件开始下载

    // 清理
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
    alert('文件下载成功！');
  } catch (error) {
    console.error('下载失败:', error);
    alert('下载失败，请稍后重试或联系管理员！');
  }

}

function handleUpdate() {
  // 使用Vue Router进行跳转
  
}

const handleDelete= async () => {
  let formData = new FormData();
  formData.append('songname', props.item.songname );
  try {
    const response = await http.post('/song/delete', 
    formData,{
      headers:{
        'Content-Type': 'multipart/form-data',
        'Authorization':localStorage.getItem("tokenTest")
      }
    }
    )
    alert(response.data.message)
    if (response.data.success) {
      console.log('提交成功123');
      emit('refreshParent');
      // 这里可以添加提交成功后的逻辑，例如跳转到另一个页面
    } else {
      console.log('提交失败');
    }
  } catch (error) {
    console.error('提交失败:', error)
  }
  
}
</script>



<!-- <template>
    <el-card 
    active-text-color="#ffd04b"
    style="max-width: 360px">
      <div><p>{{ item.songword }}</p></div>
      <template #footer>
        <div class="mb-4">
            <div>{{item.songname}}</div>
            <div class = "button">
                <el-button round>导出</el-button>
                <el-button round>编辑</el-button>
                <el-button round>删除</el-button>
            </div>
        </div>
      </template>
    </el-card>
  </template>


<style>
.button{
    float:right;
    color:azure;
}
</style> -->

<template>
  <el-card 
    active-text-color="#ffd04b"
    style="max-width: 360px;">
    <div class="lyric-container">
      <p>{{ item.songword }}</p>
    </div>
    <!-- 其他内容 -->
    <template #footer>
      <div class="footer-container">
        <div class="song-info">
          <span>{{ item.songname }}</span>
        </div>
        <div class="button">
          <el-button round @click="handleDownload">导出</el-button>
          <el-button round @click="handleUpdate">编辑</el-button>
          <el-button round @click="handleDelete">删除</el-button>
        </div>
      </div>
      </template>
  </el-card>
</template>



<style>
.lyric-container {
  height: 100px; /* 根据需要设置固定高度 */
  overflow-y: hidden; /* 隐藏垂直滚动条 */
  overflow-x: hidden; /* 隐藏垂直滚动条 */
  padding: 10px; /* 根据需要添加内边距 */
}

/* 确保内容不会溢出到容器外部 */
.lyric-content p {
  overflow: hidden;
  word-wrap: break-word;
} 
.el-card{
  width:360px;
}
.footer-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px;
}

.song-info {
  flex: 1;
}

.button {
  display: flex;
}

/* 重置button的样式，因为我们已经使用了flex布局 */
.button button {
  float: none;
  margin-left: 5px;
}
</style>