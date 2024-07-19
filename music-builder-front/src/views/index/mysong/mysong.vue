<script setup>
import { useRouter } from "vue-router";
import { ref ,onMounted,computed} from 'vue'

import { useUsername } from '../../../stores/username.js'
import {http} from '../../../http'
import asideMenu from "../../../components/menu.vue";
import userDropdown from "../../../components/dropdown.vue";
import card from "../../../components/card.vue"
const router = useRouter()

const usernameScore = useUsername()

const songList = ref([]);
const empty = ref([]);

    // 定义获取对象列表的函数
const fetchObjectList = async () => {
    console.log("13321232132")
    try {
        const response = await http.post('/song/getUserSongs',
    {
      username:usernameScore.username,
      songname:null,
      songword:null,
      address:null
      
    },{
      headers:{
        'Authorization':localStorage.getItem("tokenTest")
      }
    }
    )
        // 将后端返回的数据赋值给响应式状态
        songList.value = response.data;
        console.log(response.data)
    } catch (error) {
        console.error('Error fetching object list:', error);
    }


};
    // 组件挂载时获取数据
onMounted(()=>{
  fetchObjectList();

});
const parentKey = ref(0);
function handleRefresh() {
  // 这里可以执行需要重新挂载的逻辑
  // 例如，重新获取数据或重置状态
  songList.value = empty.value;
  fetchObjectList()
  console.log('父组件被重新挂载');
  // 触发重新创建父组件
  parentKey.value += 1;
}

const groupedArray = computed(() => {
  const groups = [];
  for (let i = 0; i < songList.value.length; i += 4) {
    groups.push(songList.value.slice(i, i + 4));
  }
  return groups;
});
const dialogFileListVisible=ref(false)
const allFileList =ref([
])
const openList=()=>{
  fetchFileList();
  dialogFileListVisible.value=true;
}

function getBeijingTime() {
  const date = new Date();
  // 北京时间比 UTC 时间快 8 小时
  const beijingDate = new Date(date.getTime() + 8 * 60 * 60 * 1000);

  const year = beijingDate.getFullYear();
  const month = (beijingDate.getMonth() + 1).toString().padStart(2, '0');
  const day = beijingDate.getDate().toString().padStart(2, '0');

  return `${year}-${month}-${day}`;
}

const fileList = ref([]);

const fetchFileList = async () => {
    try {
        const response = await http.post('/download/getUserFile',
    {
      username:usernameScore.username,
      name:null,
      address:null
      
    },{
      headers:{
        'Authorization':localStorage.getItem("tokenTest")
      }
    }
    )
        // 将后端返回的数据赋值给响应式状态
        fileList.value = response.data;
        allFileList.value = [];
        fileList.value.forEach((item)=>{
          var node = {
            "name":"",
            "type":"png",
            "time":"",
            "addr":"",
          }
          node.name=item.name;
          node.time=getBeijingTime();
          node.addr=item.address;
          allFileList.value.push(node);
        })
        console.log(response.data)
    } catch (error) {
        console.error('Error fetching object list:', error);
    }
};
const download = async () => {
  console.log(currentRow.value);
  if (!currentRow.value) {
    alert('请先选择文件');
    return;
  }
  let formData = new FormData();
  formData.append('name', currentRow.value.name);

  try {
    // 发送POST请求并设置响应类型为blob
    const response = await http.post('/download/download', formData, {
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

  alert('下载文件成功');
  dialogFileListVisible.value=true;
}
const currentRow=ref()
const handleCurrentChange=(obj)=>{
  currentRow.value = obj;
}


import * as Tone from "tone";
import { Midi } from '@tonejs/midi'

import piano from '../../musicEdit/sampler/piano';
import accordian from '../../musicEdit/sampler/accordian';
import acoustic_bass from '../../musicEdit/sampler/acoustic_bass';
import acoustic_guitar from '../../musicEdit/sampler/acoustic_guitar';
import dulcimer from '../../musicEdit/sampler/dulcimer';
import fx_5 from '../../musicEdit/sampler/fx_5';
import lead_6_voice from '../../musicEdit/sampler/lead_6_voice';
import pad_2_warm from '../../musicEdit/sampler/pad_2_warm';
import recorder from '../../musicEdit/sampler/recorder';
import string_ensemble from '../../musicEdit/sampler/string_ensemble';
import taiko_drum from '../../musicEdit/sampler/taiko_drum';
import tenor_sax from '../../musicEdit/sampler/tenor_sax';
import trombone from '../../musicEdit/sampler/trombone';
import violin from '../../musicEdit/sampler/violin';

const reflectSampler=(value)=>{
    if (value>=8&&value<=15) {//大扬琴
        return dulcimer;
    }else if (value>=16&&value<=23) {//手风琴
        return accordian;
    }else if (value>=24&&value<=31) {//钢弦吉他
        return acoustic_guitar;
    }else if (value>=32&&value<=39) {//大贝司
        return acoustic_bass;
    }else if (value>=40&&value<=47) {//小提琴
        return violin;
    }else if (value>=48&&value<=55) {//弦乐合奏音色1
        return string_ensemble;
    }else if (value>=56&&value<=63) {//长号
        return trombone;
    }else if (value>=64&&value<=71) {//中音萨克斯风
        return tenor_sax;
    }else if (value>=72&&value<=79) {//竖笛
        return recorder;
    }else if (value>=80&&value<=87) {//合成主音6（人声）
        return lead_6_voice;
    }else if (value>=88&&value<=95) {//合成音色2 （温暖）
        return pad_2_warm;
    }else if (value>=96&&value<=103) {//合成效果 5 明亮
        return fx_5;
    }else if (value>=112&&value<=119) {//太鼓
        return taiko_drum;
    }else {//大钢琴
        return piano;
    }
}
const reflectInstrumentId=(value)=>{
    if (value>=8&&value<=15) {//大扬琴
        return 15;
    }else if (value>=16&&value<=23) {//手风琴  
        return 21;
    }else if (value>=24&&value<=31) {//钢弦吉他
        return 25;
    }else if (value>=32&&value<=39) {//大贝司
        return 32;
    }else if (value>=40&&value<=47) {//小提琴
        return 40;
    }else if (value>=48&&value<=55) {//弦乐合奏音色1
        return 48;
    }else if (value>=56&&value<=63) {//长号
        return 57;
    }else if (value>=64&&value<=71) {//中音萨克斯风
        return 65;
    }else if (value>=72&&value<=79) {//竖笛
        return 74;
    }else if (value>=80&&value<=87) {//合成主音6（人声）
        return 85;
    }else if (value>=88&&value<=95) {//合成音色2 （温暖）
        return 89;
    }else if (value>=96&&value<=103) {//合成效果 5 明亮
        return 100;
    }else if (value>=112&&value<=119) {//太鼓
        return 116;
    }else {//大钢琴
        return 1;
    }
}


var currentMidi=null
const btnplayClick=()=>{
  if(!currentMidi&&!fileUrl.value) {
      alert('未加载文件');
      return;
    }else if(!currentMidi&&fileUrl.value){
      alert('未加载音频文件');
      return;
    }else if(currentMidi&&!fileUrl.value){
      alert('未加载人声文件');
      return;
    }
    currentMidi.tracks.forEach((track)=>{
      var notes = track.notes;
      console.log(notes);
      var instrumentId=track.instrument.number;
      notes.forEach(note => {
        Tone.getTransport().schedule(time=>{

          reflectSampler(instrumentId).toDestination().triggerAttackRelease(
            note.name,         // 音名
            note.duration,     // 持续时间
            time,   // 开始发声时间
            note.velocity
          );
        },note.time)

      })
    })
    audioElement.value = new Audio();
    audioElement.value.src = fileUrl.value;
    console.log(fileUrl.value);
    audioElement.value.play();
    Tone.getTransport().start();
    

}
const midifile=ref(null)
const importMidfileClick=()=>{
  midifile.value.click();

  
}
const handleMidiChange=(event)=>{
  const files = event.target.files;
    if (files && files.length > 0) {
        var file = files[0];
        console.log('选中的文件:', file);
    }else{
        alert('未选择文件');
        return;
    }
    parseMidi(file);
    alert('音频导入成功');
}


const parseMidi=(file) =>{
    // 创建文件读取器
    const reader = new FileReader();
    // 读取文件
    reader.readAsArrayBuffer(file);
    // 文件读取完成后将文件转化为json对象
    reader.addEventListener('load', (e) => {
      currentMidi = new Midi(e.target.result);
      console.log(JSON.stringify(currentMidi)); // 应该输出 'object'
    })
    
}
const audioElement = ref(null);
const fileUrl = ref(null);
const wavfile=ref(null);

const importWavClick = (e) => {
  wavfile.value.click();
};
const handleWavChange=(event)=>{
  const files = event.target.files;
    if (files && files.length > 0) {
        var file = files[0];
        console.log('选中的文件:', file);
        fileUrl.value = URL.createObjectURL(file);
    }else{
        alert('未选择文件');
        return;
    }
    alert('人声导入成功');
}






</script>


<template>
  <el-dialog v-model="dialogFileListVisible" title="文件列表" width="800">
    <el-table :data="allFileList" style="width: 100%" highlight-current-row @current-change="handleCurrentChange">
      <el-table-column property="name" label="文件名" width="250" />
      <el-table-column property="type" label="文件类型" width="250" />
      <el-table-column property="time" label="上传时间" width="250" />
    </el-table>
    <template #footer>
    <div style="margin-top: 10px">
    <el-button type="danger" round @click="dialogFileListVisible=false">取消</el-button>
    <el-button round type="info"  @click="download">下载文件</el-button>
  </div>
</template>
  </el-dialog>
  <div class="common-layout">
    <el-container>
      <el-aside width="150px">
        <asideMenu></asideMenu>
      </el-aside>
      <el-container>
        <el-header class = "header">
            <el-row style="width: 100%;">
                <el-col :span="4" class = "head">
                    <h2>我的歌曲</h2>
                </el-col>
                <el-col :span="16">
                  <div style="width: 100%;height: 100%;" class="btn-container">
                    <div class="importMidfile" @click="importMidfileClick">导入音频</div>
                    <input type="file" id="midifile" accept="audio/*" style="display: none;" @change="handleMidiChange" ref="midifile">
                    <div class="importWav" @click="importWavClick">导入人声</div>
                    <input type="file" id="wavfile" accept="audio/*" style="display: none;" @change="handleWavChange" ref="wavfile">
                    <div class="btnplay" @click="btnplayClick">播放</div>
                    <div class="btnfile" @click="openList">文件列表</div>
                  </div>
                  
                </el-col>
                <el-col :span="4">
                  <userDropdown></userDropdown>
                </el-col>
            </el-row>
          
        </el-header>
        <el-main class = "main">
          <div v-for="(group, index) in groupedArray" :key="index">
            <el-row :gutter="24"  class="row-box">
            <!-- 遍历组内的每个元素 -->
              <div v-for="item in group" :key="item.id || item">
                  <el-col  :span="6" v-bind:key="parentKey">
                    <card :item = "item"  @refresh-parent="handleRefresh"></card>
                  </el-col>
              </div>
            <!-- 可以添加一些分隔符或样式，以区分不同的组 -->
            </el-row>
          </div>  
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<style scoped>




ul {
  list-style-type: none;
  padding: 0;
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  grid-gap: 30px;
}
.head{
    top:40px;
    left:20%;
}
.header{
  background-color:rgba(19, 15, 15, 0.977);
  border-left:none;
  color:white;
  border-bottom: 1px solid #6e6e6e;
}
.main{
  background-color:rgba(19, 15, 15, 0.977);
  border-left: none;
  color:azure;
  padding-top: 10px;
  padding-left: 60px;
}
.el-row{
    margin-bottom: 20px;
}
.row-box {

  display: flex;
  flex-flow: wrap;
}
.row-box .el-card {
  min-width: 0%;
  margin-left:0px;
  height: 100%;
  top:10%;
  border: 0px;
  box-shadow: 0 2px 12px 0 rgb(0 0 0 / 10%);
  background-color:rgb(40, 39, 39);
  
}
.el-card{
  color:azure;
}
.aside-menu{
  height: 100vh;
  border-right:none;
}
.el-aside{
  background-color:rgb(40, 39, 39);
}

.btn-container{
  display: flex;
  align-items: center;
  justify-content: end;
}
.btnfile,.importMidfile,.importWav,.btnplay{
  width: 75px;
  background-color:rgb(53,53,57);
  height: 30px;
  border-radius: 15px;
  text-align: center;
  line-height: 30px;
  font-size: 13px;
  cursor: pointer;
  margin-left: 8px;

}
/* rgb(88, 88, 96) */
.btnfile:hover,.importMidfile:hover,.importWav:hover,.btnplay:hover{
  background-color: rgb(88, 88, 96) ;
}
</style>


