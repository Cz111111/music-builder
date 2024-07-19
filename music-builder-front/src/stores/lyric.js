import {ref} from 'vue'
import { defineStore } from 'pinia'

export const lyric_music = defineStore('lyric_music',()=>{
    const lrcContext = ref('');
    const lrc_filename=ref('');
    const highData = ref('');
    const setLrcContext = (newLrcContext)=>{
        lrcContext.value = newLrcContext
    }

    const removeLrcContext = ()=>{
        lrcContext.value = ''
    }

    const setLrc_filename = (newLrc_filename)=>{
        lrc_filename.value = newLrc_filename
    }

    const removeLrc_filename = ()=>{
        lrc_filename.value = ''
    }
    const setHighData = (newHighData)=>{
        highData.value = newHighData
    }

    const removeHighData = ()=>{
        highData.value = ''
    }
    return{
        lrcContext,lrc_filename,highData,setLrcContext, removeLrcContext,setLrc_filename,removeLrc_filename,setHighData,removeHighData
    }
})