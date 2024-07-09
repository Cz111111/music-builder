<template>
    <el-input
      v-model="inputText"
      placeholder="请输入内容"
    />
    <el-button type="primary" @click="submit()" round>Primary</el-button>
</template>
  
<script  lang="ts" setup>
import { ref } from 'vue'
import { http } from '../http';
const inputText = ref('');




const submit = async () => {
      const url = '/sing/uploadfile';
      try {

        // 调用后端API
        const response = await http.post(url,
        { 
            audioFile:inputText.value
        }
      );
        
        console.log(response.data);
      } catch (error) {
        console.log(error);
      }
}


</script>
  
<style scoped>
h1 {
  color: #42b983;
}
</style>