<script setup>
import { defineProps } from 'vue';
defineProps(['item']);
</script>



<!-- <template>
    <el-card 
    active-text-color="#ffd04b"
    style="max-width: 360px">
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
</style> -->

<template>
  <el-card 
    active-text-color="#ffd04b"
    style="max-width: 360px;">
    <div class="lyric-container">
      <p>{{ item.songword }}</p>
    </div>
    <!-- 其他内容 -->
    <template #footer>
      <div class="footer-container">
        <div class="song-info">
          <span>{{ item.songname }}</span>
        </div>
        <div class="button">
          <el-button round>导出</el-button>
          <el-button round>编辑</el-button>
          <el-button round>删除</el-button>
        </div>
      </div>
      </template>
  </el-card>
</template>



<style>
.lyric-container {
  height: 100px; /* 根据需要设置固定高度 */
  overflow-y: hidden; /* 隐藏垂直滚动条 */
  overflow-x: hidden; /* 隐藏垂直滚动条 */
  padding: 10px; /* 根据需要添加内边距 */
}

/* 确保内容不会溢出到容器外部 */
.lyric-content p {
  overflow: hidden;
  word-wrap: break-word;
} 
.el-card{
  width:360px;
}
.footer-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px;
}

.song-info {
  flex: 1;
}

.button {
  display: flex;
}

/* 重置button的样式，因为我们已经使用了flex布局 */
.button button {
  float: none;
  margin-left: 5px;
}
</style>