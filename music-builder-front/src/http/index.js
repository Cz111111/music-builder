import axios from "axios";
// import { useTokenScore } from '../stores/token.js'

// const tokenScore = useTokenScore()
// console.log(tokenScore.token,8888)
let baseURL = '/api'        //关键代码
export const http = axios.create({
    baseURL ,
    timeout : 5000,
    headers: {
    'Accept': 'application/json'
    },
  });

  
export const post = (url,data) => {
  http.post(url,data)
    .then(function (response) {
      const data = response.data;
      console.log(data);
    })
    .catch(function (error) {
      console.log("error");
    })
    .finally(function () {
      
    });
};


export const get = (url,data) => {
  http.get(url,data)
    .then(function (response) {
      const data = response.data;
      console.log(data);
    })
    .catch(function (error) {
      console.log("error");
    })
    .finally(function () {
      // 总是会执行
    });
};