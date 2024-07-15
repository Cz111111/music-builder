<template>

<div class="file-picker">
  <el-input v-model="inputmusic" placeholder="请输入内容" />
  <div class="input-with-button">
  <el-button type="danger" @click="selectmusic()" round>选择音频文件</el-button>
  </div>
</div>
<div class="file-picker">
  <el-input v-model="inputmusic" placeholder="请输入内容" />
  <div class="input-with-button">
  <el-button type="danger" @click="selectlyrics()" round>选择歌词文件</el-button>
  </div>
</div>
  <el-button type="danger" @click="submit()">开始k歌</el-button>

</template>

<script lang="ts" setup>
import { ref } from 'vue'
import { http } from '../http';
import { useRouter } from 'vue-router';
const inputmusic = ref('');
const router = useRouter();
const selectmusic = () => {
  console.log(document.getElementById("file").value)
};

const submit = async () => {
  const url = '/sing/uploadfile';
  try {
    // 调用后端API
    const response = await http.post(url,
      {
        audioFile: inputmusic.value
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

.file-picker {
  display: flex;
  align-items: center;
  margin-bottom: 20px; /* 根据需要调整间距 */
}

.input-with-button {
  flex: 1;
  margin-left: 20px; /* 增加与按钮的水平间距 */
}
</style>