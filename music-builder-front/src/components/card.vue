<template>
  <el-card 
    active-text-color="#ffd04b"
    style="max-width: 360px;">
    <div class="lyric-container" @mouseover="startAnimation" @mouseleave="stopAnimation">
      <div class="lyric-content" :style="contentStyle">
        <p>{{ item.songword }}</p>
      </div>
    </div>
    <!-- 其他内容 -->
    <template #footer>
      <div class="footer-container">
        <div class="song-info">
          <span>{{ item.songname }}</span>
        </div>
        <div class="button">
          <el-button round @click="handleDownload">导出</el-button>
          <el-button round @click="openDialog">编辑</el-button>
          <el-button round @click="handleDelete">删除</el-button>
        </div>
      </div>
    </template>
    <el-dialog v-model="dialogVisible" title="修改音乐信息" width="500">
      <el-form :model="form" @submit.prevent="handleUpdate">
        <el-form-item label="歌词:" :label-width="formLabelWidth">
          <el-input v-model="form.songword" autocomplete="off" />
        </el-form-item>
        <el-form-item label="歌名" :label-width="formLabelWidth">
          <el-input v-model="form.songname" autocomplete="off" />
        </el-form-item>
        <button type="submit">确定</button>
      </el-form>
      <template #footer>
      </template>
    </el-dialog>
  </el-card>
</template>

<script setup>
import { defineProps, defineEmits, ref } from 'vue';
import { http } from '../http/index.js';

const props = defineProps(['item']);
const emit = defineEmits(['refreshParent']);
const dialogVisible = ref(false); // 控制弹窗显示的响应式引用

const animationSpeed = 20; // 滚动速度，数字越大滚动越慢
let animationInterval = null;
let translateY = 0;

function startAnimation() {
  stopAnimation(); // 先确保停止之前的动画
  animationInterval = setInterval(() => {
    translateY -= 1; // 步进值可以根据实际情况调整
    contentStyle.value = `transform: translateY(${translateY}px);`;

    // 当歌词完全滚动出视图时，从底部重新开始滚动
    if (Math.abs(translateY) >= contentHeight.value) {
      translateY = 110;
      contentStyle.value = `transform: translateY(${contentHeight.value}px);`;
      setTimeout(() => {
        contentStyle.value = `transition: transform 0s linear; transform: translateY(0);`;
        setTimeout(() => {
          contentStyle.value = `transition: transform 0.1s linear; transform: translateY(-1px);`;
        }, 0);
      }, 0);
    }
  }, animationSpeed);
}

function stopAnimation() {
  clearInterval(animationInterval);
}

const contentStyle = ref(`transform: translateY(${translateY}px); transition: transform 0.1s linear;`);
const contentHeight = ref(150);

const form = ref({
  songword: props.item.songword,
  songname: props.item.songname
});

function openDialog() {
  dialogVisible.value = true; // 打开弹窗
}

const handleDownload = async () => {
  let formData = new FormData();
  formData.append('songname', props.item.songname);

  try {
    // 发送POST请求并设置响应类型为blob
    const response = await http.post('/song/download', formData, {
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

const handleUpdate = async () => {
  if (!form.value.songname) {
    alert('歌曲名不能为空');
    return;
  }
  let formData = new FormData();

  formData.append('songname', form.value.songname);
  formData.append('songword', form.value.songword);

  try {
    const response = await http.post('/song/update',
      formData, {
      headers: {
        'Authorization': localStorage.getItem("tokenTest")
      }
    });

    alert(response.data.message);
    if (response.data.success) {
      console.log('提交成功');
      // 这里可以添加提交成功后的逻辑，例如跳转到另一个页面
      emit('refreshParent');
    } else {
      console.log('提交失败');
    }
  } catch (error) {
    console.error('提交失败:', error);
  }

  // 清空表单
  form.value = {
    songname: '',
    songword: ''
  };
}

const handleDelete = async () => {
  let formData = new FormData();
  formData.append('songname', props.item.songname);

  try {
    const response = await http.post('/song/delete',
      formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        'Authorization': localStorage.getItem("tokenTest")
      }
    });

    alert(response.data.message);
    if (response.data.success) {
      console.log('提交成功');
      emit('refreshParent');
      // 这里可以添加提交成功后的逻辑，例如跳转到另一个页面
    } else {
      console.log('提交失败');
    }
  } catch (error) {
    console.error('提交失败:', error);
  }
}
</script>

<style>
.lyric-container {
  height: 100px; /* 根据需要设置固定高度 */
  overflow-y: hidden; /* 隐藏垂直滚动条 */
  overflow-x: hidden; /* 隐藏水平滚动条 */
  position: relative; /* 设置为相对定位 */
  padding: 10px; /* 根据需要添加内边距 */
}

.lyric-content {
  position: absolute; /* 设置为绝对定位，以便使用transform进行动画 */
  bottom: 0;
  left: 0;
  width: 100%; /* 宽度100% */
  transition: transform 0.1s linear; /* 过渡效果，根据需要调整速度 */
}

/* 确保内容不会溢出到容器外部 */
.lyric-content p {
  overflow: hidden;
  word-wrap: break-word;
}

.el-card {
  width: 360px;
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
