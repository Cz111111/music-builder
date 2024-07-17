<template>
    <div class="side-by-side-component">
      <el-button
        type="primary"
        class="square-button"
        @click="openDialog"
      >
      AI生成伴奏midi
      </el-button>
      <div class="text-container">
<!--         <el-col :span="12"> -->
        选择一种乐器生成一段伴奏midi
<!--         </el-col> -->
      </div>
      <!-- 添加el-dialog组件 -->
      <el-dialog v-model="dialogVisible" title="AI生成伴奏midi" width="500">
        <el-form :model="form" @submit.prevent="handleSubmit">
          <el-form-item label="乐器:" :label-width="formLabelWidth">
            <el-select v-model="form.instrument" placeholder="Please select a zone">
              <el-option label="中提琴" value="Viola" />
              <el-option label="直升机" value="Helicopter" />
            </el-select>
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

  const router = useRouter();
  const dialogVisible = ref(false); // 控制弹窗显示的响应式引用

  const form = ref({
  instrument: null,
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
  if (!form.value.instrument||!form.value.name) {
    alert('请填写完整数据')
    return
  }
  console.log(form.value)
  let formData = new FormData();
    formData.append('instrument', form.value.instrument);
    formData.append('name', form.value.name);
  try {
    const response = await http.post('render/four', 
    formData,{
      headers:{
        'Authorization':localStorage.getItem("tokenTest")
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
    instrument: null,
    name:''
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