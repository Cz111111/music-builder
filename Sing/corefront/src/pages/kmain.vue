<template>
    <div>
      <h1>{{ message }}</h1>
    </div>
  </template>
  
  <script>
  export default {
    name: 'HelloWorld',
    data() {
      return {
        message: 'Hello, Vue!'
      };
    }
  }
  </script>
  
  <style scoped>
  h1 {
    color: #42b983;
  }
  </style>