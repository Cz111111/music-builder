<template>
    <div>
      <h1>{{ message }}</h1>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  
  const message = ref('Hello, Vue!');
  </script>
  
  <style scoped>
  h1 {
    color: #42b983;
  }
  </style>