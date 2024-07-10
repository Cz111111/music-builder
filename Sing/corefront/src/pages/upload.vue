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
import { useRouter } from 'vue-router';
const inputText = ref('');
const router = useRouter();



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
        router.push('/kmain');
      } catch (error) {
        console.log(error.response.data);
      }
}


</script>
  
<style scoped>
h1 {
  color: #42b983;
}
</style>