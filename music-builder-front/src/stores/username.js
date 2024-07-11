
import {ref} from 'vue'
import { defineStore } from 'pinia'

export const useUsername = defineStore('username',()=>{
    const username = ref('')

    const setUsername = (newUsername)=>{
        username.value = newUsername
    }

    const removeUsername = ()=>{
        username.value = ''
    }

    return{
        username,setUsername,removeUsername
    }
})