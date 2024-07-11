<script setup>
import { defineProps } from 'vue';
defineProps(['item']);
</script>




<template>
    <el-card 
    active-text-color="#ffd04b"
    style="max-width: 360px">
    <!--  <template #header>
        <div class="card-header">
          <span>歌</span>
        </div>
      </template> -->
      <div><p>{{ item.songword }}</p></div>
      <template #footer>
        <div class="mb-4">
            <div>{{item.songname}}</div>
            <div class = "button">
                <el-button round>导出</el-button>
                <el-button round>编辑</el-button>
                <el-button round>删除</el-button>
            </div>
        </div>
      </template>
    </el-card>
  </template>


<style>
.button{
    float:right;
    color:azure;
}
</style>