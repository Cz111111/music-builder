<template>
  <div class="car">
    <div class="lyric-container" @mouseover="startAnimation" @mouseleave="stopAnimation">
      <div class="lyric-content" >
        <div class="lyric-text":style="contentStyle">{{ item.songword }}</div>
      </div>
    </div>
    <!-- 其他内容 -->
      <div class="footer-container">
        <div class="song-info">
          {{ item.songname }}
        </div>
        <div class="btn" @click="handleDownload" @mouseover="btnmouseover1" @mouseleave="btnmouseleave1">导出</div>
        <div class="btn" @click="openDialog" @mouseover="btnmouseover2" @mouseleave="btnmouseleave2">编辑</div>
        <div class="btn" @click="handleDelete" @mouseover="btnmouseover3" @mouseleave="btnmouseleave3">删除</div>
      </div>
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
  </div>
</template>

<script setup>
import { defineProps, defineEmits, ref } from 'vue';
import { http } from '../http/index.js';

const props = defineProps(['item']);
const emit = defineEmits(['refreshParent']);
const dialogVisible = ref(false); // 控制弹窗显示的响应式引用

const btnmouseover1=(e)=>{
  e.target.style.backgroundColor = 'rgb(88, 88, 96)'; 
}
const btnmouseleave1=(e)=>{
  e.target.style.backgroundColor = 'rgb(53,53,57)'; 
}
const btnmouseover2=(e)=>{
  e.target.style.backgroundColor = 'rgb(88, 88, 96)'; 
}
const btnmouseleave2=(e)=>{
  e.target.style.backgroundColor = 'rgb(53,53,57)'; 
}
const btnmouseover3=(e)=>{
  e.target.style.backgroundColor = 'rgb(88, 88, 96)'; 
}
const btnmouseleave3=(e)=>{
  e.target.style.backgroundColor = 'rgb(53,53,57)'; 
}




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
  if (animationInterval) {
    clearInterval(animationInterval);
    animationInterval = null;
    translateY = 0; // 重置 translateY 为初始值
    contentStyle.transform = `translateY(0px)`; // 重置 transform 为初始状态
  }
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
  formData.append('originName',props.item.songname);

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
.car{
  width: 300px;
  height: 200px;
  background-color: rgb(40,39,39);
  border-radius: 8px;
  display: flex;
  flex-direction: column;
}


.lyric-container {
  border-radius: 8px 8px 0 0;
  background-image: linear-gradient(-225deg, #5e5e5e 0%, #867e7e 20%, #817474 48%, #6f6767 100%);
  /* background-image: linear-gradient(to right,#939393 0%, #a5a5a5 20%, #8f8f8f 45%, #d0d0d0 100%); */
  height: 135px; /* 根据需要设置固定高度 */
  width: 100%;
  overflow: hidden;
}

.lyric-content {
  margin-top: 15px;
  margin-left: 10px;
  width: 200px;
  height: 110px;
  padding-left: 5px;
  padding-right: 5px;
  transition: transform 0.1s linear; /* 过渡效果，根据需要调整速度 */
  overflow: hidden;
}
.lyric-text{
  width: 100%;
  line-height: 30px;
  font-size: 14px;
}




.footer-container {
  padding-left: 15px;
  display: flex;
  align-items: center;
  height: 65px;
  width: 100%;
}

.song-info {
  width: 105px;
  height: 40px;
  margin-right: 5px;
  overflow: hidden;
  line-height: 40px;
  text-align: left;
  font-size: 16px;
  color: #ebebeb;
  font-weight: 550;
}

.btn{
  width: 50px;
  height: 30px;
  background-color:rgb(53,53,57);
  border-radius: 15px;
  margin-left: 5px;
  text-align: center;
  line-height: 30px;
  color: #d3d3d3;
  font-size: 13px;
  cursor: pointer;
}
</style>
