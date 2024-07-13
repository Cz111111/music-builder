<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { http } from '../../http/index.js'
import { useTokenScore } from '../../stores/token.js'
import asideMenu from '../../components/menu.vue'
import userDropdown from '../../components/dropdown.vue'
const router = useRouter()
const tokenScore = useTokenScore()
const form = ref({
  midi: null,
  singer: '',
  lyrics: '',
  wavname:null
})
const handleMidiChange = (event) => {
  const file = event.target.files[0];
  if (file) {
    form.value.midi = file; 
  }
};

const handleWavChange = (event) => {
  const file = event.target.files[0];
  if (file) {
    form.value.wavname = file; 
  }
};

const handleSubmit = async () => {
  console.log(form.value)
  if (!form.value.midi || !form.value.singer || !form.value.lyrics || !form.value.wavname) {
    alert('请填写完整数据')
    return
  }
  console.log(form.value)
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
      <el-aside width="120px">
        <asideMenu></asideMenu>
      </el-aside>
      <el-container>
      <el-header class="header">
        <userDropdown></userDropdown>
      </el-header>
      <el-main class="main">
        <div class="register-container">
          <div class="title">提交信息</div>
          <form @submit.prevent="handleSubmit">
            <div class="form-group">
              <label for="midi">MIDI 文件路径:</label>
              <input type="file" id="midi" @change="handleMidiChange" />
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
              <input type="file" id="wavname" @change="handleWavChange" />
            </div>
            <button type="submit">提交</button>
          </form>
        </div>
      </el-main>
    </el-container>
    </el-container>
  </div>
</template>

<template>
  <el-button plain @click="dialogTableVisible = true">
    Open a Table nested Dialog
  </el-button>

  <el-button plain @click="dialogFormVisible = true">
    Open a Form nested Dialog
  </el-button>

  <el-dialog v-model="dialogFormVisible" title="Shipping address" width="500">
    <el-form :model="form">
      <el-form-item label="Promotion name" :label-width="formLabelWidth">
        <el-input v-model="form.name" autocomplete="off" />
      </el-form-item>
      <el-form-item label="Zones" :label-width="formLabelWidth">
        <el-select v-model="form.region" placeholder="Please select a zone">
          <el-option label="Zone No.1" value="shanghai" />
          <el-option label="Zone No.2" value="beijing" />
        </el-select>
      </el-form-item>
    </el-form>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="dialogFormVisible = false">Cancel</el-button>
        <el-button type="primary" @click="dialogFormVisible = false">
          Confirm
        </el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from 'vue'

const dialogTableVisible = ref(false)
const dialogFormVisible = ref(false)
const formLabelWidth = '140px'

const form = reactive({
  name: '',
  region: '',
  date1: '',
  date2: '',
  delivery: false,
  type: [],
  resource: '',
  desc: '',
})

const gridData = [
  {
    date: '2016-05-02',
    name: 'John Smith',
    address: 'No.1518,  Jinshajiang Road, Putuo District',
  },
  {
    date: '2016-05-04',
    name: 'John Smith',
    address: 'No.1518,  Jinshajiang Road, Putuo District',
  },
  {
    date: '2016-05-01',
    name: 'John Smith',
    address: 'No.1518,  Jinshajiang Road, Putuo District',
  },
  {
    date: '2016-05-03',
    name: 'John Smith',
    address: 'No.1518,  Jinshajiang Road, Putuo District',
  },
]
</script>

<style scoped>
/* 保留原始样式，根据需要进行调整 */
</style>