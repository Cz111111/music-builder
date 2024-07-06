<script setup>
import { useRouter } from "vue-router";
import { ref } from 'vue'
const router = useRouter()

import asideMenu from "../../components/menu.vue";


</script>

<template>
  <div class="common-layout">
    <el-container>
      <el-aside width="200px">
        <asideMenu></asideMenu>
      </el-aside>
      <el-main>Main</el-main>
    </el-container>
  </div>
 
</template>

<style scoped>
</style>
