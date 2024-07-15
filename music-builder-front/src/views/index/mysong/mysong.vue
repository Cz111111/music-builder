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
onMounted( fetchObjectList);
const parentKey = ref(0);
function handleRefresh() {
  // 这里可以执行需要重新挂载的逻辑
  // 例如，重新获取数据或重置状态
  songList.value = empty;
  fetchObjectList()
  console.log('父组件被重新挂载');
  // 触发重新创建父组件
  parentKey.value += 1;
}

const groupedArray = computed(() => {
  const groups = [];
  for (let i = 0; i < songList.value.length; i += 3) {
    groups.push(songList.value.slice(i, i + 3));
  }
  return groups;
});
</script>


<template>
  <div class="common-layout">
    <el-container>
      <el-aside width="120px">
        <asideMenu></asideMenu>
      </el-aside>
      <el-container>
        <el-header class = "header">
            <el-row>
                <el-col :span="4" class = "head">
                    <h2>我的歌曲</h2>
                </el-col>
                <el-col :span="20">
                    <userDropdown></userDropdown>
                </el-col>
            </el-row>
          
        </el-header>
        <el-main class = "main">
          <div v-for="(group, index) in groupedArray" :key="index">
            <el-row :gutter="12"  class="row-box">
            <!-- 遍历组内的每个元素 -->
              <div v-for="item in group" :key="item.id || item">
                  <el-col  :span="24" v-bind:key="parentKey">
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
.head{
    top:40px;
    left:20%;
}
.header{
  background-color:rgba(19, 15, 15, 0.977);
  border-left:none;
  color:white;
}
.main{
  background-color:rgba(19, 15, 15, 0.977);
  border-left: none;
  color:azure;
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
</style>


