<template>
    <el-card 
    active-text-color="#ffd04b"
    style="max-width: 360px">
    <!--  <template #header>
        <div class="card-header">
          <span>歌</span>
        </div>
      </template> -->
      <p v-for="o in 4" :key="o" class="text item">{{ 'List item ' + '歌词' }}</p>
      <template #footer>
        <div class="mb-4">
            <span>这是歌名</span>
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