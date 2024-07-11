<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { http } from '../../http/index.js'
import { useTokenScore } from '../../stores/token.js'
const router = useRouter()
const tokenScore = useTokenScore()
const form = ref({
  midi: '',
  singer: '',
  lyrics: '',
  wavname: ''
})

const handleSubmit = async () => {
  if (!form.value.midi || !form.value.singer || !form.value.lyrics || !form.value.wavname) {
    alert('请填写完整数据')
    return
  }

  try {
    const response = await http.post('render/submit', 
    {
      midi: form.value.midi,
      singer: form.value.singer,
      lyrics: form.value.lyrics,
      wavName: form.value.wavname
    },{
      headers:{
        'Authorization':tokenScore.token
      }
    }
    )
    alert(response.data.message)
    if (response.data.success) {
      console.log('提交成功');
      // 这里可以添加提交成功后的逻辑，例如跳转到另一个页面
      router.push({ name: 'desired-route-name' }) // 替换为成功后跳转的路由名称
    } else {
      console.log('提交失败');
    }
  } catch (error) {
    console.error('提交失败:', error)
  }

  // 清空表单
  form.value = {
    midi: '',
    singer: '',
    lyrics: '',
    wavname: ''
  }
}
</script>

<template>
  <div class="common-layout">
    <el-container>
      <el-header class="header">Header</el-header>
      <el-main class="main">
        <div class="register-container">
          <div class="title">提交信息</div>
          <form @submit.prevent="handleSubmit">
            <div class="form-group">
              <label for="midi">MIDI 文件路径:</label>
              <input type="text" id="midi" v-model="form.midi" required>
            </div>
            <div class="form-group">
              <label for="singer">歌手名:</label>
              <input type="text" id="singer" v-model="form.singer" required>
            </div>
            <div class="form-group">
              <label for="lyrics">歌词:</label>
              <textarea id="lyrics" v-model="form.lyrics" required></textarea>
            </div>
            <div class="form-group">
              <label for="wavname">WAV 文件路径:</label>
              <input type="text" id="wavname" v-model="form.wavname" required>
            </div>
            <button type="submit">提交</button>
          </form>
        </div>
      </el-main>
    </el-container>
  </div>
</template>

<style scoped>
/* 保留原始样式，根据需要进行调整 */
</style>