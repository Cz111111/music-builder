<template>
    <div class="side-by-side-component">
      <el-button
        type="primary"
        class="square-button"
        @click="openDialog"
      >
      AI清唱音乐合成
      </el-button>
      <div class="text-container">
        <el-col :span="12">
        AI根据歌手列表中指定的歌手，导入的人声midi，输入的相匹配的歌词合成歌曲。需要为歌曲命名
        </el-col>
      </div>
      <!-- 添加el-dialog组件 -->
      <el-dialog v-model="dialogVisible" title="AI清唱音乐合成" width="500">
        <el-form :model="form" @submit.prevent="handleSubmit">
          <el-form-item label="歌词:" :label-width="formLabelWidth">
            <el-input v-model="form.name" autocomplete="off" />
          </el-form-item>
          <el-form-item label="歌手名:" :label-width="formLabelWidth">
            <el-select v-model="form.region" placeholder="Please select a zone">
              <el-option label="云与光" value="云与光" />
              <el-option label="Opencpop" value="Opencpop" />
              <el-option label="星空" value="星空" />
              <el-option label="霖离" value="霖离" />
              <el-option label="霁何虹" value="霁何虹" />
              <el-option label="华晓熊" value="华晓熊" />
              <el-option label="秋绘" value="秋绘" />
            </el-select>
          </el-form-item>
          <el-form-item label="MIDI文件:" :label-width="formLabelWidth">
            <input type="file" id="midi" @change="handleMidiChange" />
          </el-form-item>
          <el-form-item label="歌名" :label-width="formLabelWidth">
            <el-input v-model="form.wavname" autocomplete="off" />
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

  const router = useRouter();
  const dialogVisible = ref(false); // 控制弹窗显示的响应式引用

  const form = ref({
  name: '',
  region: null,
  midi: null,
  wavname: '',
});

const handleMidiChange = (event) => {
  const file = event.target.files[0];
  if (file) {
    form.value.midi = file;
  }
};


function openDialog() {
  dialogVisible.value = true; // 打开弹窗
}

function confirmAction() {
  // 这里可以放置点击确定后需要执行的逻辑
  dialogVisible.value = false; // 关闭弹窗
}

const handleSubmit = async () => {
  console.log(form.value)
  if (!form.value.midi || !form.value.name || !form.value.region || !form.value.wavname) {
    alert('请填写完整数据')
    return
  }
  let formData = new FormData();
    formData.append('name', form.value.name);
    formData.append('region', form.value.region);
    formData.append('midi', form.value.midi);
    formData.append('wavname', form.value.wavname);
  try {
    const response = await http.post('render/submit', 
    formData,{
      headers:{
        'Authorization':tokenScore.token
      }
    }
    )
    alert(response.data.message)
    if (response.data.success) {
      console.log('提交成功');
      // 这里可以添加提交成功后的逻辑，例如跳转到另一个页面
    } else {
      console.log('提交失败');
    }
  } catch (error) {
    console.error('提交失败:', error)
  }

  // 清空表单
  form.value = {
    midi: null,
    region: null,
    name: '',
    wavname: ''
  }
}
  </script>
  
<style scoped>
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