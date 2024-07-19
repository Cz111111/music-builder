<script setup>
import { useRouter } from "vue-router";
import { ref } from 'vue'
import { useTokenScore } from '../../../stores/token.js'
import { useUsername } from '../../../stores/username.js'
const router = useRouter()
const tokenScore = useTokenScore()
const usernameScore = useUsername()

import asideMenu from "../../../components/menu.vue";
import userDropdown from "../../../components/dropdown.vue";
import sidebyside1 from "../../../components/sidebyside1.vue";
import sidebyside2 from "../../../components/sidebyside2.vue";
import sidebyside3 from "../../../components/sidebyside3.vue";
import sidebyside4 from "../../../components/sidebyside4.vue";
import sidebyside5 from "../../../components/sidebyside5.vue";

</script>


<template>
  <div class="common-layout">
    <el-container>
      <el-aside width="150px">
        <asideMenu></asideMenu>
      </el-aside>
      <el-container>
        <el-header class="header">
          <el-row>
            <el-col :span="4" class="head">
              <h2>AI创作</h2>
            </el-col>
            <el-col :span="16">
            </el-col>
            <el-col :span="4">
              <userDropdown></userDropdown>
            </el-col>
          </el-row>
        </el-header>
        <el-main class="main">
        <ul>
          <li style="list-style-type: none;">
            <sidebyside1 />
          </li>
          <li style="list-style-type: none;">
            <sidebyside2 />
          </li>
          <li style="list-style-type: none;">
            <sidebyside3 />
          </li>
          <li style="list-style-type: none;">
            <sidebyside4 />
          </li>
          <li style="list-style-type: none;">
            <sidebyside5 />
          </li>
        </ul>
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

.head {
  top: 40px;
  left: 20%;
}

.header {
  background-color: rgba(19, 15, 15, 0.977);
  border-left: none;
  color: white;
  border-bottom: 1px solid #ccc;
  /* 添加底部边框 */
}

.main {
  background-color: rgba(19, 15, 15, 0.977);
  border-left: none;
  color: azure;
}

.el-row {
  margin-bottom: 20px;
}

.row-box {
  display: flex;
  flex-flow: wrap;
}

.aside-menu {
  height: 100vh;
  border-right: none;
}

.el-aside {
  background-color: rgb(40, 39, 39);
}
</style>
