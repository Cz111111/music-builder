<template>
    <div class="side-by-side-component">
      <el-button
        type="primary"
        class="square-button"
        @click="openDialog"
      >
      袁轲功能三
      </el-button>
      <div class="text-container">
        袁轲功能三描述栏
      </div>
      <!-- 添加el-dialog组件 -->
      <el-dialog v-model="dialogVisible" title="弹窗标题" width="500">
        <el-form :model="form" @submit.prevent="handleSubmit">
          <el-form-item label="关键词（最好英文）" :label-width="formLabelWidth">
            <el-input v-model="form.keyword" autocomplete="off" />
          </el-form-item>
          <button type="submit">提交</button>
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
  keyword: ''
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
  if (!form.value.keyword) {
    alert('请填写完整数据')
    return
  }
  console.log(form.value)
  try {
    const response = await http.post('render/submit', 
        form.value.keyword,{
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
    keyword: ''
  }
}
  </script>
  
<style scoped>
.side-by-side-component {
  display: flex;
  align-items: center;
}
.square-button {
  width: 100px; /* 设置宽度为100px */
  height: 100px; /* 设置高度为100px，与宽度相同，形成正方形 */
  padding: 0; /* 移除内边距，确保按钮大小不变 */
  display: flex;
  justify-content: center;
  align-items: center;
  /* 其他样式保持不变 */
}
.text-container {
  flex-grow: 1;
  /* 其他样式保持不变 */
}
  </style>