<template>
  <div class="all">
    <el-dialog v-model="cloudimportVisible" title="导入文件" width="500"  >
        <el-form :model="fileform">
        <el-form-item label="选择文件" :label-width="formLabelWidth">
            <el-select v-model="chosenMidi" placeholder="选择一个文件">
            <!-- <el-option v-for="instrument in instrumentList" :label="instrument.name" :value="instrument.name" /> -->
            <el-option v-for="file in updateInfo" :key="file.songname"  :label="file.songname" :value="file.songname" />
            </el-select>
        </el-form-item>
        </el-form>
        <template #footer>
        <div class="dialog-footer">
            <el-button @click="cloudimportVisible = false">Cancel</el-button>
            <el-button type="primary" @click="addcloudfileSubmit">
            OK
            </el-button>
        </div>
        </template>
    </el-dialog>
    <div class="header">
        <div class="thennone1"></div>
        <input type="file" ref="fileInput" style="display: none" accept="audio/midi" @change="handleFileChange" />
        <div class="theimport" @mouseenter="theimportEnter" @mouseleave="theimportLeave" @click="fileSelect">
            <el-icon :size="20"  class="theIcon"><FolderOpened color="#8f8f8f"/></el-icon>&nbsp;本地导入
        </div>
        <div class="thecloudImport" @mouseenter="thecloudImportEnter" @mouseleave="thecloudImportLeave" @click="cloudImportClick" >
            <el-icon :size="20"  class="theIcon"><Headset color="#8f8f8f"/></el-icon>&nbsp;云导入
        </div>
        <div class="thecloud" @mouseenter="thecloudEnter" @mouseleave="thecloudLeave" @click="cloudSaveClick">
            <el-icon :size="20"  class="theIcon"><MostlyCloudy color="#8f8f8f"/></el-icon>&nbsp;云保存
        </div>
        <div class="thesave" @mouseenter="thesaveEnter" @mouseleave="thesaveLeave" @click="exportMidi">
            <el-icon :size="20"  class="theIcon"><Download color="#8f8f8f"/></el-icon>&nbsp;另存为
        </div>
        <div class="thennone">
        </div>
        <div class="thehelp" @mouseenter="thehelpEnter" @mouseleave="thehelpLeave" @click = "gotoHelp">
            <el-icon :size="20"  class="theIcon"><QuestionFilled color="#8f8f8f"/></el-icon>&nbsp;帮助
        </div>
        <div class="currentTime">
            {{ currentTime }}
        </div>
        <div class="thennone1"></div>
    </div>
    <div class="split">
        <div class="splitLeft" id="splitLeft">
            <div 
            class="track-item" 
            v-for="(track, index) in tracks_info" 
            :key="index"
            :class="{ 'selected-track-item': activateTrack === index }"
            @click="selectTrack(index)"
            >
                <el-dialog v-model="updateTrackVisible" title="属性" width="500" >
                    <el-form :model="updateform">
                    <el-form-item label="音轨名" :label-width="formLabelWidth">
                        <el-input v-model="updateform.name" autocomplete="off" />
                    </el-form-item>
                    <el-form-item label="乐器" :label-width="formLabelWidth">
                        <el-select v-model="updateform.region" placeholder="选择一个乐器">
                        <el-option v-for="instrument in instrumentList" :label="instrument.name" :value="instrument.name" />
                        </el-select>
                    </el-form-item>
                    </el-form>
                    <template #footer>
                    <div class="dialog-footer">
                        <el-button @click="updateTrackVisible = false">Cancel</el-button>
                        <el-button type="primary" @click="updateTrackSubmit(track,index)">
                        OK
                        </el-button>
                    </div>
                    </template>
                </el-dialog>
                <el-dropdown trigger="contextmenu" placement="bottom-end" size="default">
                    <div>
                        <div class="logo">
                            {{ track.logo }}
                        </div>
                        <div class="name">
                            {{ track.name }}
                        </div>
                        <div class="instrument">
                            {{ track.instrument }}
                        </div>
                    </div>
                    <template #dropdown>
                        <el-dropdown-menu class="dropdown-menu" slot="dropdown">
                            <el-dropdown-item class="dropdown-item" @click="updateTrack(track,index)">属性</el-dropdown-item>
                            <el-dropdown-item class="dropdown-item" @click="deleteTrack(track,index)">删除</el-dropdown-item>
                        </el-dropdown-menu>
                    </template>
                    
                </el-dropdown>
            </div>
            <div class="addtrack" @click="dialogFormVisible = true">
                <el-icon class="addtrackIcon"><Plus color="#8f8f8f" /></el-icon>&nbsp;添加音轨
            </div>
            <el-dialog v-model="dialogFormVisible" title="添加音轨" width="500" >
                <el-form :model="form">
                <el-form-item label="音轨名" :label-width="formLabelWidth">
                    <el-input v-model="form.name" autocomplete="off" />
                </el-form-item>
                <el-form-item label="乐器" :label-width="formLabelWidth">
                    <el-select v-model="form.region" placeholder="选择一个乐器">
                    <el-option v-for="instrument in instrumentList" :label="instrument.name" :value="instrument.name" />
                    </el-select>
                </el-form-item>
                </el-form>
                <template #footer>
                <div class="dialog-footer">
                    <el-button @click="dialogFormVisible = false">Cancel</el-button>
                    <el-button type="primary" @click="addTrack">
                        OK
                    </el-button>
                </div>
                </template>
            </el-dialog>
        </div>
        <div class="splitRight" id="splitRight">
            <div class="timeLineBar">
                <div class="fillBlock">&emsp;</div>
                <div class="timeLineContainer" ref="timeLineContainer" id="timeLineContainer">
                </div>
            </div>
            <div class="content" id="content">
                <div class="piano-keys" ref="piano_keys" id="piano_keys"> 
                    <!-- <div
                    v-for="(key, index) in keys"
                    :key="index"
                    :class="['key', { 'black-key': key.isBlack, 'white-key': !key.isBlack, 'pressed': key.active ,'has-child': isHasChild(index),'is25':key.isWhite_25,'is24':key.isWhite_24,'is22':key.isWhite_22}]"
                    @mouseenter="keyOnMouseEnter(key, index)"
                    @mouseleave="keyOnMouseLeave(key, index)"
                    @mouseup="keyOnMouseUp(key, index)"
                    @mousedown="keyOnMouseDown(key, index)"
                    >
                        <div v-if="isHasChild(index)" class="child">C{{ Math.floor((95-index) / 12) }}</div>
                        <div v-if="!isHasChild(index)" class="nchild">&emsp;</div>
                    </div> -->
                </div>
                <div class="canvas-container" ref="canvas_container" id="canvas_container">
                </div>
            </div>
        </div>
    </div>
    <div class="controll" id="controll">
        <div class="inputText">
            <el-input
            type="textarea"
            resize="none"
            :autosize="{ minRows: 7, maxRows: 7 }"
            placeholder="输入描述生成歌词"
            style="width: 240px"
            :maxlength=90
            v-model="inputLyrics"
            />
        </div>
        <div class="submit" @click="genLyrics">
            <svg t="1720842981242" class="icon" viewBox="0 0 1035 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="7743" width="16" height="16"><path d="M530.850448 573.163313c32.005172-31.883636 32.111192-87.434343-0.069818-119.489939C399.437114 322.845737 268.507336 191.603071 137.436629 60.506505c-12.621576-12.625455-27.585939-20.686869-45.26804-23.540364-3.529697-0.570182-7.085253-0.999434-10.630465-1.499798-1.025293 0-2.055758 0-3.081051 0-4.035232 0.575354-8.111838 0.943838-12.091475 1.757091-33.298101 6.818909-54.838303 26.616242-63.959919 59.278222-3.201293 11.479919-2.806949 24.85398-1.135192 35.701657 1.751919 11.344162 7.11499 21.818182 14.479515 30.904889 3.444364 4.252444 7.050343 8.418263 10.918788 12.291879 111.045818 111.127273 222.142061 222.222222 333.247354 333.293899 1.444202 1.444202 3.227152 2.554828 5.187232 4.080485-2.121697 2.222545-3.475394 3.696485-4.879515 5.100606C252.653437 625.450667 145.164468 733.112889 37.416912 840.506182c-15.829333 15.773737-33.541172 31.162182-36.146424 54.273293-1.233455 10.908444-1.859232 22.111677 0 32.929616 0.919273 5.363071 3.262061 10.423596 5.494949 15.358707 23.655434 52.318384 89.514667 64.241778 130.66602 23.186101C268.668953 835.315071 399.501761 703.987071 530.850448 573.163313L530.850448 573.163313zM78.451902 35.466343l-12.091475 1.757091C70.345235 36.410182 74.421841 36.041697 78.451902 35.466343L78.451902 35.466343zM551.467498 36.441212c-38.000485 2.858667-70.434909 37.034667-71.651556 75.106263-0.79903 24.979394 8.131232 45.650747 25.691798 63.197091 111.383273 111.252687 222.661818 222.615273 334.015354 333.893818 1.534707 1.534707 3.686141 2.448808 6.595232 4.327434-3.35903 2.667313-5.116121 3.803798-6.565495 5.253172-68.050747 68.000323-136.075636 136.035556-204.100525 204.065616C591.916791 765.814949 548.391619 809.360808 504.845761 852.880808c-19.449535 19.439192-28.328081 42.706747-24.045899 70.054788 4.829091 30.919111 22.440081 52.516202 51.712 63.010909 29.450343 10.550303 56.692364 4.803232 80.49002-15.848727 1.944566-1.682101 3.798626-3.470222 5.616485-5.288081 130.121697-130.111354 259.955071-260.509737 390.546101-390.141414 33.272242-33.035636 33.454545-88.934141 1.206303-121.050505C879.016912 322.804364 748.093599 191.557818 617.028064 60.456081c-12.626747-12.626747-27.611798-20.655838-45.288727-23.505455-3.534869-0.570182-7.090424-0.994263-10.635636-1.484283-1.030465 0-2.055758 0-3.086222 0.005172C555.835013 35.799919 553.659013 36.274424 551.467498 36.441212L551.467498 36.441212zM551.467498 36.441212" fill="#8f8f8f" p-id="7744"></path></svg>
        </div>
        <div class="outputText">
            <el-input
            type="textarea"
            resize="none"
            :autosize="{ minRows: 7, maxRows: 7 }"
            placeholder="生成歌词结果"
            style="width: 500px"
            v-model="outputLyrics"
            />
        </div>
        <div class="importText" @click="importLyricsClick">
            <div class="importTextinfo">导入歌词</div>
        </div>
        <div class="controllpart">
            <el-slider v-model="masterVolume" vertical height="90px" class="controllslider" @input="masterVolumeChange"/>
            <div class="controllinfo">主音量</div>
        </div>
        <div class="controllpart">
            <el-slider v-model="channelPanning" vertical height="90px" class="controllslider" @input="channelPanningChange"/>
            <div class="controllinfo">声道平移</div>
        </div>
        <div class="controllpart">
            <el-slider v-model="reverbWet" vertical height="90px" class="controllslider":min="0" :max="10" @input="reverbWetChange"/>
            <div class="controllinfo">混响控制</div>
        </div>
        <div class="controllpart">
            <el-slider v-model="velocityControl" vertical height="90px" class="controllslider" :disabled="activateRect === null" @change="rectvelocityChange"/>
            <div class="controllinfo">力度控制</div>
        </div>
        <div class="controllpart">
            <el-slider v-model="sustainControl" vertical height="90px" class="controllslider" :min="1" :max="128" :disabled="activateRect === null" @input="sustainChange"/>
            <div class="controllinfo">延音控制</div>
        </div>
    </div>
    <div class="player" id="player">
        <div class="backIcon" @click="backClick" @dblclick="backDblclick">
            <div class="circle">
                <svg t="1720800092607" class="icon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="8973" width="24" height="24"><path d="M490.666667 298.453333v426.666667l-256-213.333333 256-213.333334z m298.666666 0v426.666667l-256-213.333333 256-213.333334z" fill="#ffffff" opacity=".99" p-id="8974"></path></svg>
            </div>
        </div>
        <div class="endIcon" @click="endClick">
            <div class="circle">
                <svg t="1720801636974" class="icon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="17738" width="13" height="13"><path d="M128 128h768v768H128z" fill="#ffffff" p-id="17739"></path></svg>
            </div>   
        </div>
        <div class="playerIcon" @click="play" v-if="!isPlaying">
            <svg t="1720800759186" class="playericon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="13615" width="35" height="35"><path d="M512 896c212.074667 0 384-171.925333 384-384S724.074667 128 512 128 128 299.925333 128 512s171.925333 384 384 384z" fill="#42b983" p-id="13616" data-spm-anchor-id="a313x.search_index.0.i44.1fc63a81dJiVJ6" class="selected"></path><path d="M85.333333 512C85.333333 276.352 276.352 85.333333 512 85.333333s426.666667 191.018667 426.666667 426.666667-191.018667 426.666667-426.666667 426.666667S85.333333 747.648 85.333333 512zM512 170.666667C323.477333 170.666667 170.666667 323.477333 170.666667 512s152.810667 341.333333 341.333333 341.333333 341.333333-152.810667 341.333333-341.333333S700.522667 170.666667 512 170.666667z" fill="#42b983" p-id="13617"></path><path d="M426.666667 512v-149.333333l117.333333 74.666666L661.333333 512l-117.333333 74.666667L426.666667 661.333333v-149.333333z" fill="#ffffff" p-id="13618" data-spm-anchor-id="a313x.search_index.0.i42.1fc63a81dJiVJ6" class=""></path><path d="M406.144 325.269333a42.666667 42.666667 0 0 1 43.434667 1.408l234.666666 149.333334a42.666667 42.666667 0 0 1 0 71.978666l-234.666666 149.333334A42.666667 42.666667 0 0 1 384 661.333333V362.666667a42.666667 42.666667 0 0 1 22.144-37.397334zM469.333333 440.384v143.232L581.866667 512 469.333333 440.384z" fill="#ffffff" p-id="13619" data-spm-anchor-id="a313x.search_index.0.i43.1fc63a81dJiVJ6" class=""></path></svg>
        </div>
        <div class="pauseIcon" @click="pause" v-if="isPlaying">
            <svg t="1721120093010" class="pauseicon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="4217" width="30" height="30"><path d="M1018.428 511.158c0 279.757-227.513 507.27-507.157 507.27-279.645 0-507.158-227.513-507.271-507.27C4 231.514 231.626 4 511.27 4c279.645 0 507.158 227.626 507.158 507.158z" fill="#42b983" p-id="4218" data-spm-anchor-id="a313x.search_index.0.i1.2b003a81iXsl5y" class="selected"></path><path d="M622.266 292c-25.164 0-47.164 22-47.164 47.143v345.714c0 25.143 22 47.143 47.143 47.143s47.163-22 47.163-47.143V339.143c0-25.143-22-47.143-47.142-47.143z m-220 0c-25.143 0-47.143 22-47.143 47.143v345.714c0 25.143 22 47.143 47.143 47.143 25.163 0 47.163-22 47.163-47.143V339.143C449.41 314 427.41 292 402.287 292h-0.021z" fill="#ffffff" p-id="4219" data-spm-anchor-id="a313x.search_index.0.i0.2b003a81iXsl5y" class="selected"></path></svg>
        </div>
        <div class="forwardIcon" @click="forwardClick">
            <div class="circle">
                <svg t="1720800176223" class="icon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="11656" width="24" height="24"><path d="M533.333333 298.453333v426.666667l256-213.333333-256-213.333334z m-298.666666 0v426.666667l256-213.333333-256-213.333334z" fill="#ffffff" opacity=".99" p-id="11657"></path></svg>
            </div>
        </div>
        <div class="lineIcon">
            <svg t="1720802108244" class="icon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="28653" width="20" height="50"><path d="M531.185 943.532c0 8.9-6.493 16.114-14.503 16.114h-9.364c-8.01 0-14.504-7.214-14.504-16.114V80.467c0-8.899 6.493-16.114 14.504-16.114h9.364c8.01 0 14.503 7.215 14.503 16.114v863.065z" p-id="28654" fill="#8f8f8f"></path></svg>
        </div>
        <div class="bpmIcon">
            <div class="circle">
                <svg t="1720801807076" class="icon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="27105" width="20" height="20"><path d="M820 883.7l-16.1-165.5-73.1-658.8C727 25.5 698.5 0 664.7 0H359.5c-33.6 0-62.6 26.1-66.1 59.5l-73 657.3L204 883.6c-2.8 29.1 20 54.3 49.2 54.3h-7.5c3.7 48.8 39.7 86.1 85.5 86.1H693c21.8 0 42.5-8.7 58.3-24.6 15.9-15.9 25.2-38 27-61.5h-7.5c29.2 0 52-25.2 49.2-54.2zM347.8 65.5v-0.2c0.6-5.8 5.9-10.6 11.7-10.6h305.2c5.9 0 11.1 4.7 11.7 10.8l70.8 638.4C683.9 643 604.6 602.1 538.4 593.1V263.8h47.2c15.1 0 27.4-12.2 27.4-27.4 0-15.1-12.2-27.4-27.4-27.4h-47.2v-87c0-14.6-11.8-26.4-26.4-26.4-14.6 0-26.4 11.8-26.4 26.4v87.2h-47.2c-15.1 0-27.4 12.2-27.4 27.4 0 15.1 12.2 27.4 27.4 27.4h47.2v329c-67.1 8.8-145.9 49.2-208.5 109.4l70.7-636.9zM512 780c14.6 0 26.4-11.8 26.4-26.4v-41c18.9 9.6 31.9 29.3 31.9 52 0 32.2-26.1 58.3-58.3 58.3-32.2 0-58.3-26.1-58.3-58.3 0-22.7 13-42.3 31.9-52v41c0 14.6 11.8 26.4 26.4 26.4z m206.3 186.6c-5 5-13.4 10.9-25.4 10.9H331.2c-19.9 0-36-16.8-39.1-39.6H732c-1.5 11.2-6.3 21.3-13.7 28.7z" fill="#8f8f8f" p-id="27106"></path></svg>
            </div>
        </div>
        <div class="bpmValue">
            <div class="c_bpmValue">
                <div class="bpm">BPM</div>
                <input type="text" id="bValue" class="bValue" v-model="bpm" name="bValue"/>
            </div>
        </div>
        <div class="lineIcon">
            <svg t="1720802108244" class="icon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="28653" width="20" height="50"><path d="M531.185 943.532c0 8.9-6.493 16.114-14.503 16.114h-9.364c-8.01 0-14.504-7.214-14.504-16.114V80.467c0-8.899 6.493-16.114 14.504-16.114h9.364c8.01 0 14.503 7.215 14.503 16.114v863.065z" p-id="28654" fill="#8f8f8f"></path></svg>
        </div>
        <div class="timeIcon">
            <div class="timeValue">{{playSeconds}}</div>
        </div>
    </div>
</div>
</template>
  
<script setup>
import { computed, onMounted, ref,reactive,watch} from 'vue';
import * as Tone from "tone";
import { Midi } from '@tonejs/midi'
import sampleArr from './sampleArr';
import Split from 'split.js'
import Konva from 'konva';
import { ElMessage, ElMessageBox,ElSelect,ElOption } from 'element-plus'
import { Decimal } from 'decimal.js'
import {http} from '../../http/index'

import { useUsername } from '../../stores/username'
const usernameScore = useUsername()

//声音素材
import piano from './sampler/piano';
import accordian from './sampler/accordian';
import acoustic_bass from './sampler/acoustic_bass';
import acoustic_guitar from './sampler/acoustic_guitar';
import dulcimer from './sampler/dulcimer';
import fx_5 from './sampler/fx_5';
import lead_6_voice from './sampler/lead_6_voice';
import pad_2_warm from './sampler/pad_2_warm';
import recorder from './sampler/recorder';
import string_ensemble from './sampler/string_ensemble';
import taiko_drum from './sampler/taiko_drum';
import tenor_sax from './sampler/tenor_sax';
import trombone from './sampler/trombone';
import violin from './sampler/violin';

const reflectSampler=(value)=>{
    if (value>=8&&value<=15) {//大扬琴
        return dulcimer;
    }else if (value>=16&&value<=23) {//手风琴
        return accordian;
    }else if (value>=24&&value<=31) {//钢弦吉他
        return acoustic_guitar;
    }else if (value>=32&&value<=39) {//大贝司
        return acoustic_bass;
    }else if (value>=40&&value<=47) {//小提琴
        return violin;
    }else if (value>=48&&value<=55) {//弦乐合奏音色1
        return string_ensemble;
    }else if (value>=56&&value<=63) {//长号
        return trombone;
    }else if (value>=64&&value<=71) {//中音萨克斯风
        return tenor_sax;
    }else if (value>=72&&value<=79) {//竖笛
        return recorder;
    }else if (value>=80&&value<=87) {//合成主音6（人声）
        return lead_6_voice;
    }else if (value>=88&&value<=95) {//合成音色2 （温暖）
        return pad_2_warm;
    }else if (value>=96&&value<=103) {//合成效果 5 明亮
        return fx_5;
    }else if (value>=112&&value<=119) {//太鼓
        return taiko_drum;
    }else {//大钢琴
        return piano;
    }
}
const reflectInstrumentId=(value)=>{
    if (value>=8&&value<=15) {//大扬琴
        return 15;
    }else if (value>=16&&value<=23) {//手风琴
        return 21;
    }else if (value>=24&&value<=31) {//钢弦吉他
        return 25;
    }else if (value>=32&&value<=39) {//大贝司
        return 32;
    }else if (value>=40&&value<=47) {//小提琴
        return 40;
    }else if (value>=48&&value<=55) {//弦乐合奏音色1
        return 48;
    }else if (value>=56&&value<=63) {//长号
        return 57;
    }else if (value>=64&&value<=71) {//中音萨克斯风
        return 65;
    }else if (value>=72&&value<=79) {//竖笛
        return 74;
    }else if (value>=80&&value<=87) {//合成主音6（人声）
        return 85;
    }else if (value>=88&&value<=95) {//合成音色2 （温暖）
        return 89;
    }else if (value>=96&&value<=103) {//合成效果 5 明亮
        return 100;
    }else if (value>=112&&value<=119) {//太鼓
        return 116;
    }else {//大钢琴
        return 1;
    }
}
// 大钢琴1🎹 大扬琴15🎼 手风琴21🪗 钢弦吉他25🎸 
//大贝司32🪕 小提琴40🎻 弦乐合奏音色48🔔  长号57 🎺
// 中音萨克斯65🎷 竖笛74🪈 合成主音人声85🧑‍🤝‍🧑 合成音色温暖89 ☀️
//合成效果明亮100⚡ 太鼓116🥁
const instrumentList =[
    {        
        "id":1,
        "name":"大钢琴",
        "emoji":"🎹",
    },
    {        
        "id":15,
        "name":"大扬琴",
        "emoji":"🎼",
    },
    {        
        "id":21,
        "name":"手风琴",
        "emoji":"🪗",
    },
    {        
        "id":25,
        "name":"钢弦吉他",
        "emoji":"🎸",
    },
    {        
        "id":32,
        "name":"大贝司",
        "emoji":"🪕",
    },
    {        
        "id":40,
        "name":"小提琴",
        "emoji":"🎻",
    },
    {        
        "id":48,
        "name":"弦乐合奏音色",
        "emoji":"🔔",
    },
    {        
        "id":57,
        "name":"长号",
        "emoji":"🎺",
    },
    {        
        "id":65,
        "name":"中音萨克斯",
        "emoji":"🎷",
    },
    {        
        "id":74,
        "name":"竖笛",
        "emoji":"🪈",
    },
    {        
        "id":85,
        "name":"合成主音人声",
        "emoji":"🧑‍🤝‍🧑",
    },
    {        
        "id":89,
        "name":"合成音色温暖",
        "emoji":"☀️",
    },
    {        
        "id":100,
        "name":"合成效果明亮",
        "emoji":"⚡",
    },
    {        
        "id":116,
        "name":"太鼓",
        "emoji":"🥁",
    },
]

/*  
header
 */

const theimportEnter=()=>{
    var theimport=document.querySelector('.theimport');
    theimport.style.backgroundColor='rgb(67, 68, 73)';
}
const theimportLeave=()=>{
    var theimport=document.querySelector('.theimport');
    theimport.style.backgroundColor='rgb(30,31,36)';
}
const thecloudImportEnter=()=>{
    var thecloudImport=document.querySelector('.thecloudImport');
    thecloudImport.style.backgroundColor='rgb(67, 68, 73)';
}
const thecloudImportLeave=()=>{
    var thecloudImport=document.querySelector('.thecloudImport');
    thecloudImport.style.backgroundColor='rgb(30,31,36)';
}
const thecloudEnter=()=>{
    var theimport=document.querySelector('.thecloud');
    theimport.style.backgroundColor='rgb(67, 68, 73)';
}
const thecloudLeave=()=>{
    var thecloud=document.querySelector('.thecloud');
    thecloud.style.backgroundColor='rgb(30,31,36)';
}
const thesaveEnter=()=>{
    var thesave=document.querySelector('.thesave');
    thesave.style.backgroundColor='rgb(67, 68, 73)';
}
const thesaveLeave=()=>{
    var thesave=document.querySelector('.thesave');
    thesave.style.backgroundColor='rgb(30,31,36)';
}
const thehelpEnter=()=>{
    var thehelp=document.querySelector('.thehelp');
    thehelp.style.backgroundColor='rgb(67, 68, 73)';
}
const thehelpLeave=()=>{
    var thehelp=document.querySelector('.thehelp');
    thehelp.style.backgroundColor='rgb(30,31,36)';
}

const gotoHelp = () =>{
    window.open("https://michaels-organization-45.gitbook.io/test/yin-yue-chuang-zuo", "_blank");
}

const cloudimportVisible=ref(false)
const updateInfo = ref([]);
// const chosenMidi = ref({
//     // songname:'',
//     // songword:'',
//     // midi:null
// });
const chosenMidi = ref('');
const getMidi = async () => {
    try {
        const response = await http.post('/song/getUserMidi',
    {
      username:usernameScore.username,
      name:null,
      address:null,
      songword:null
      
    },{
      headers:{
        'Authorization':localStorage.getItem("tokenTest")
      }
    }
    )
        // 将后端返回的数据赋值给响应式状态
        updateInfo.value = response.data;
        console.log("114514",updateInfo.value)
    } catch (error) {
        console.error('Error fetching object list:', error);
    }
};
const cloudImportClick=()=>{
    cloudimportVisible.value=true;

    getMidi()
}
const fileform=reactive({
    "name":"",
})

const currentTime = ref(new Date().toLocaleTimeString());

function updateTime() {
  currentTime.value = new Date().toLocaleTimeString();
}
//导入文件后解析成midi对象
var currentMidi=null;

const fileInput=ref(null)
const fileSelect=()=>{
    fileInput.value.click();
}
const handleFileChange=(event) =>{
    const files = event.target.files;
    if (files && files.length > 0) {
        var file = files[0];
        console.log('选中的文件:', file);
    }else{
        alert('未选择文件');
        return;
    }
    
    const reader = new FileReader();
        // 读取文件
    reader.readAsArrayBuffer(file);
    // 文件读取完成后将文件转化为json对象
    reader.addEventListener('load', (e) => {
    currentMidi = new Midi(e.target.result);
    //console.log(JSON.stringify(currentMidi));// 应该输出 'object'
    console.log(currentMidi);
    midiToRect(currentMidi);
    });

}
//
function base64ToUint8Array(base64) {
    var binaryString = window.atob(base64);
    var byteArray = new Uint8Array(binaryString.length);
    for (var i = 0; i < binaryString.length; i++) {
        byteArray[i] = binaryString.charCodeAt(i);
    }
    return byteArray;
}
const addcloudfileSubmit= async()=>{
    var obj=null;
    updateInfo.value.forEach((item)=>{
        if (item.songname===chosenMidi.value) {
            obj=item;
        }
    })
    outputLyrics.value=obj.songword;
    console.log(obj.songword)
    var base64File = obj.file;

    if (base64File) {
        console.log('选中的文件 Base64:', base64File);

        // 解码 Base64 字符串为 Uint8Array
        var byteArray = base64ToUint8Array(base64File);
        // 确保 byteArray 是 Uint8Array
        var uint8Array = new Uint8Array(byteArray);

        // 创建 Blob 对象
        var blob = new Blob([uint8Array], { type: 'audio/midi' });

        // 将 Blob 转换为 File 对象
        var file = new File([blob], 'filename.mid', { type: 'audio/midi' });

    if (file) {
        console.log('选中的文件:', file);
    }else{
        alert('未选择文件');
        return;
    }
    
    const reader = new FileReader();
        // 读取文件
    reader.readAsArrayBuffer(file);
    // 文件读取完成后将文件转化为json对象
    reader.addEventListener('load', (e) => {
    const cloudCurrentMidi = new Midi(e.target.result);
    //console.log(JSON.stringify(currentMidi));// 应该输出 'object'
    midiToRect(cloudCurrentMidi);
    });    
    }
    cloudimportVisible.value=false;
}

// 云导入

import { ElLoading } from 'element-plus';
const cloudSaveClick = async()=>{
    console.log(111);
  // 假设midiData是一个包含MIDI数据的数组
const midiDataCloud = midi.toArray();
const blob = new Blob([midiDataCloud], { type: 'audio/midi' });

// 创建一个文件名
const fileName = 'myMidiFile.mid';

// 创建一个File对象
const file = new File([blob], fileName, { type: blob.type });
  let formData = new FormData();
  formData.append('midi', file);
  formData.append('songword', outputLyrics.value);
  try {
    const loading = ElLoading.service({
    lock: true,
    text: '加载中',
    background: 'rgba(0, 0, 0, 0.7)',
  })
    const response = await http.post('render/six',
      formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        'Authorization': localStorage.getItem("tokenTest")
      },
      timeout: 900000
    }
    )
    alert(response.data.message)
    if (response.data.success) {
      console.log('提交成功');
      // 这里可以添加提交成功后的逻辑，例如跳转到另一个页面
    } else {
      console.log('提交失败');
    }
    loading.close();
  } catch (error) {
    console.error('提交失败:', error)
  }

  // 清空表单
  form.value = {
    wavname: null,
    name: null,
    songword:''
  }
}


//解析midi对象映射为音符
const midiToRect=(cmidi)=>{
    //遍历绘制矩形
    ppq.value=cmidi.header.ppq;
    bpm.value=Math.floor(cmidi.header.tempos[0].bpm);
    cmidi.tracks.forEach(track => {

        //const track_volume=new Tone.Volume(0);
        
        var trackItem={
            "instrumentId":reflectInstrumentId(track.instrument.number),
            "logo":instrumentList.find(instrument => instrument.id === reflectInstrumentId(track.instrument.number)).emoji,
            "instrument":instrumentList.find(instrument => instrument.id === reflectInstrumentId(track.instrument.number)).name,
            "name":reflectInstrumentId(track.instrument.number),
            "notes":[],
            "cc":[],
            "volume":new Tone.Volume(0)

        };
        Object.entries(track.controlChanges).forEach(([key, value]) => {
            
            value.forEach((ccitem)=>{
                trackItem.cc.push({
                    number:ccitem.number,
                    time:ccitem.time,
                    value:ccitem.value
                });
            })
        });
        
        track.notes.forEach(note => {
            //note.midi, note.time, note.duration, note.name
            var rect_x=new Decimal(bpm.value).mul(new Decimal(note.time)).mul(new Decimal(gridSize_x.value)).div(new Decimal(60)).toNumber();
            var rect_width=new Decimal(bpm.value).mul(new Decimal(note.duration)).mul(new Decimal(gridSize_x.value)).div(new Decimal(60)).toNumber();
            if (new Decimal(rect_x).add(new Decimal(rect_width))<BarWidth.value) {
                var rect = new Konva.Rect({
                    x: new Decimal(bpm.value).mul(new Decimal(note.time)).mul(new Decimal(gridSize_x.value)).div(new Decimal(60)).toNumber(),
                    y: (95-sampleArr.indexOf(note.name))*gridSize_y.value + 0.25,
                    width: new Decimal(bpm.value).mul(new Decimal(note.duration)).mul(new Decimal(gridSize_x.value)).div(new Decimal(60)).toNumber(),
                    height: gridSize_y.value,
                    fill: '#8f8f8f',
                    name:'rect'
                });
                rect.on('dblclick',function(e) {
                    if (!isPlaying.value&&isRectInActivateTrack(e.target)&&(activateRect.value===null)&&(e.evt.button===2)){
                        tracks_info.value[activateTrack.value].notes.forEach((item)=>{
                            if(item.note_rect._id===e.target._id){
                                if (item.note_char!==null) {
                                    textArr=textArr.filter(text=>text._id===item.note_char._id);
                                    console.log(textArr);
                                    item.note_char.destroy();
                                    item.note_char=null;
                                }
                            }
                        })
                        tracks_info.value[activateTrack.value].notes=tracks_info.value[activateTrack.value].notes.filter(t_notes=>t_notes.note_rect._id!==e.target._id);
                        e.target.destroy();
                        layer_rect.batchDraw();
                    }
                });
                rect.on('click', (e) => {
                    if (!isPlaying.value&&e.evt.button===0 ){
                        //去活
                        if (activateRect.value!==null) {
                            activateRect.value.fill('rgb(139, 236, 171)');
                            activateRect.value.draggable(false);
                            tracks_info.value.forEach((track)=>{
                                track.notes.forEach((item)=>{
                                    if(item.note_rect._id===activateRect.value._id){
                                        velocity=item.note_velocity;
                                        if (item.note_char!==null) {
                                            item.note_char.draggable(false);
                                        }
                                    }
                            })
                        })
                            tr_rect.nodes([]);
                            activateRect.value=null;
                        }
                        //若在激活音轨内，进行可编辑操作
                        if (isRectInActivateTrack(e.target)) {
                            
                            // tr_rect.nodes([e.target]);
                            // tr_rect.show();
                            activateRect.value=e.target;
                            e.target.fill('rgb(204, 242, 217)');
                            e.target.draggable(true);
                            layer_rect.batchDraw();
                            stage.container().style.cursor = 'move';
                            var velocity=1;
                            tracks_info.value.forEach((track)=>{
                                track.notes.forEach((item)=>{
                                    if(item.note_rect._id===activateRect.value._id){
                                        velocity=item.note_velocity;
                                        
                                    }
                                })
                            })
                            tr_rect.nodes([e.target]);
                            tr_rect.show();
                            reflectSampler(tracks_info.value[activateTrack.value].instrumentId).triggerAttackRelease(
                                sampleArr[95-Math.floor(e.target.y()/gridSize_y.value)],
                                new Decimal(e.target.width()).mul(new Decimal(e.target.scaleX())).mul(new Decimal(60)).div(new Decimal(bpm.value)).div(new Decimal(gridSize_x.value)).toNumber(),
                                Tone.now(),
                                velocity
                            );
                            
                        }
                    }
                });
                rect.on('mouseenter', function(e) {
                    if (activateRect.value&&activateRect.value._id===e.target._id) {
                        stage.container().style.cursor = 'move';
                    }
                });

                rect.on('mouseleave', function() {
                    stage.container().style.cursor = 'default';
                });
                
                rect.on('transform', function(e) {
                    sustainControl.value=e.target.scaleX()*16;
                });
                rect.on('dragmove', function(e) {
                    let newY = e.target.y();
                    let nearestHeight = allowedHeights.value.reduce((prev, curr) => {
                        return (Math.abs(curr - newY) < Math.abs(prev - newY) ? curr : prev);
                    });

                    // 如果当前高度不在允许的范围内，就更新矩形的位置
                    if (allowedHeights.value.indexOf(newY) === -1) {
                        e.target.y(nearestHeight); // 设置矩形的新高度
                        
                    }
                    tracks_info.value.forEach((track)=>{
                        track.notes.forEach((item)=>{
                            if(item.note_rect._id===e.target._id && item.note_char!==null){
                                item.note_char.x(e.target.x()+4);
                                item.note_char.y(e.target.y()+4);
                            }
                        })
                    })
                    //移动音符赋音 横向移动不会发出音 y改变才有声音发出
                    if (start_y !==e.target.y()) {
                        var velocity=1;
                        tracks_info.value.forEach((track)=>{
                            track.notes.forEach((item)=>{
                                if(item.note_rect._id===activateRect.value._id){
                                    velocity=item.note_velocity;
                                }
                            })
                        })
                        reflectSampler(tracks_info.value[activateTrack.value].instrumentId).triggerAttackRelease(
                            sampleArr[95-Math.floor(e.target.y()/gridSize_y.value)],
                            new Decimal(e.target.width()).mul(new Decimal(e.target.scaleX())).mul(new Decimal(60)).div(new Decimal(bpm.value)).div(new Decimal(gridSize_x.value)).toNumber(),
                            Tone.now(),
                            velocity
                        );
                    }
                    start_y=e.target.y()
                });
                trackItem.notes.push({
                    "note_rect":rect,
                    "note_velocity":note.velocity,
                    "note_char":null,
                });
                layer_rect.add(rect);
            }
            
            
        })
        tracks_info.value.push(trackItem)
        layer_rect.batchDraw();
    })
}



const exportMidi=()=>{
    if (midi==null) {
        alert('文件为空，无法导出');
        return;
    }
    const midiData=midi.toArray();
    const blob=new Blob([midiData],{type:'audio/midi'});
    const url=URL.createObjectURL(blob);
    const a=document.createElement('a');
    a.href=url;
    a.download='output.mid';
    a.click();
}



/*
音轨
*/
const ppq=ref(0)
const bpm=ref(120)
const tracks_info=ref([
    {
        "instrumentId":instrumentList.find(instrument => instrument.name === "大钢琴").id,
        "logo":instrumentList.find(instrument => instrument.name === "大钢琴").emoji,
        "instrument":"大钢琴",
        "name":"音轨1",
        "notes":[],
        "cc":[],
        "volume":new Tone.Volume(0)
    },
])
const activateTrack=ref(0)
const selectTrack = (index) => {
    activateTrack.value = index;
    
    tracks_info.value[activateTrack.value].notes.forEach((item)=>{
        item.note_rect.fill("rgb(139, 236, 171)");
    })
    tracks_info.value.forEach((item,index)=>{
        if (index!==activateTrack.value) {
            item.notes.forEach((note)=>{
                note.note_rect.fill("#8f8f8f");
            })
        }
    })
    if ((activateRect.value!==null)) {
        activateRect.value.fill('#8f8f8f');
            activateRect.value.draggable(false);
            activateRect.value=null;
            tr_rect.nodes([]);
            layer_rect.batchDraw();
    }
};
//删除音轨
const deleteTrack=(track,index)=>{
    ElMessageBox.confirm(
    '你确定要删除这个音轨吗？',
    track.name,
    {
      confirmButtonText: 'OK',
      cancelButtonText: 'Cancel',
      type: 'warning',
    }
  )
    .then(() => {
      ElMessage({
        type: 'success',
        message: '删除成功',
      })
      tracks_info.value[index].notes.forEach((item)=>{
        item.note_rect.destroy();
      })
      tracks_info.value.splice(index, 1);
    })
    .catch(() => {
      ElMessage({
        type: 'info',
        message: '取消删除',
      })
    })
}

const dialogFormVisible = ref(false)
const formLabelWidth = '70px'
const form = reactive({
  name: '',
  region: '',
})

const addTrack=()=>{
    if (form.name===''||form.region==='') {
        alert('输入不能为空');
        return;
    }
    if (form.name.length>4) {
        alert('音轨名必须小于等于4字符');
        return;
    }
    dialogFormVisible.value = false;
    tracks_info.value.push({
        "instrumentId":instrumentList.find(instrument => instrument.name === form.region).id,
        "logo":instrumentList.find(instrument => instrument.name === form.region).emoji,
        "instrument":form.region,
        "name":form.name,
        "notes":[],
        "cc":[],
        "volume":new Tone.Volume(0)
    })
    form.name='';
    form.region='';
}

const updateTrackVisible=ref(false)
const updateform=reactive({
  name: '',
  region: '',
})

const updateTrack=(track,index)=>{ 
    updateform.name=track.name;
    updateform.region=track.instrument;
    updateTrackVisible.value=true;
}
const updateTrackSubmit=(track,index)=>{
    if (updateform.name.length>4) {
        alert('音轨名必须小于等于4字符');
        return;
    }
    tracks_info.value[index].instrumentId=instrumentList.find(instrument => instrument.name === updateform.region).id;
    tracks_info.value[index].logo=instrumentList.find(instrument => instrument.name === updateform.region).emoji;
    tracks_info.value[index].name=updateform.name;
    tracks_info.value[index].instrument=updateform.region;
    updateTrackVisible.value=false;

}




import axios from 'axios';

const activateRect=ref(null)
/*
controll
*/
const inputLyrics=ref('')

const outputLyrics=ref('')

const state = reactive({
      messages: []
    });
const genLyrics=async()=>{
    if (inputLyrics.value==='') {
        alert('请输入描述在提交');
        return;
    }
    if (outputLyrics.value!=="") {
        outputLyrics.value="";
    }
    try {
    let response = await fetch(`/api/chat/?prompt=${inputLyrics.value}`, {
    method: 'get', 
    "Access-Control-Allow-Origin" : "*",
    "Access-Control-Allow-Credentials" : true});
    console.log(response);
    
    inputLyrics.value='';
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }

    const reader = response.body.getReader();
    const textDecoder = new TextDecoder();
    let result = true;

    while (result) {
      const { done, value } = await reader.read();

      if (done) {
        console.log('Stream ended');
        result = false;
        break;
      }

      const chunkText = textDecoder.decode(value);
      console.log('Received chunk:', chunkText);
      outputLyrics.value=decodeUnicode(chunkText).match(/[\u4e00-\u9fa5]+/g);
    }
  } catch (e) {
    console.log(e);
  }
}
function decodeUnicode(str) {
    return str.replace(/\\u[0-9A-Fa-f]{4}/g, function(match) {
        return String.fromCharCode(parseInt(match.slice(2), 16));
    });
}

var textArr=[]
const importLyricsClick=()=>{
    if (textArr.length!==0) {
        tracks_info.value[activateTrack.value].notes.forEach((item)=>{
            if (item.note_char!==null) {
                textArr=textArr.filter(text=>text._id===item.note_char._id);
                console.log(textArr);
                item.note_char.destroy();
                item.note_char=null;
            }
        })
        layer_rect.batchDraw();
    }
    var num=0;
    tracks_info.value[activateTrack.value].notes.forEach((item)=>{
        num++;
    })
    var lyrics_importLyricsClick=outputLyrics.value.replace(/[^\u4e00-\u9fa5+]/g, '');
    if (lyrics_importLyricsClick.length!==num) {
        alert(`请输入${num}个中文的歌词保证与音符数目匹配`);
        return;
    }
    var i=0;
    tracks_info.value[activateTrack.value].notes.forEach((item)=>{
        var nowX=item.note_rect.x();
        var nowY=item.note_rect.y();
        item.note_char = new Konva.Text({
            x: nowX+4,
            y: nowY+4,
            text: outputLyrics.value[i],
            fontSize: 10,
            fontFamily: 'Calibri',
            fill: 'black'
        });
        textArr.push(item.note_char);
        // item.note_char.on('dragmove', function(e) {
        //     tracks_info.value[activateTrack.value].notes.forEach((lyric)=>{
        //         if (lyric.note_char._id===e.target._id) {
        //             e.target.y(lyric.note_rect.y()+4);
        //             e.target.x(lyric.note_rect.x()+4);
        //         }
        //     })
            
        // });
        layer_rect.add(item.note_char);
        i++;
    })
}

const masterVolume=ref(40) //主音量
const channelPanning=ref(50) //声道平移
const reverbWet=ref(0) //混响
const velocityControl=ref(0) //力度控制
const sustainControl=ref(0) //延音控制

watch(activateRect,(newValue, oldValue)=>{
    if (newValue) {
        tracks_info.value.forEach((track)=>{
            track.notes.forEach((item)=>{
                if(item.note_rect._id===newValue._id){
                    velocityControl.value=item.note_velocity*100;
                }
            })
        })
        sustainControl.value=newValue.scaleX()*16;
    }else{
        velocityControl.value=0;
        sustainControl.value=1;
    }
})
const rectvelocityChange=()=>{
    tracks_info.value.forEach((track)=>{
        track.notes.forEach((item)=>{
            if(item.note_rect._id===activateRect.value._id){
                item.note_velocity=velocityControl.value/100;
            }
        })
    })
}

const channelPanningChange=()=>{
    panner.pan.value=(channelPanning.value/50)-1;
}

const sustainChange=()=>{
    tracks_info.value.forEach((track)=>{
        track.notes.forEach((item)=>{
            if(item.note_rect._id===activateRect.value._id){
                item.note_rect.scaleX(sustainControl.value/16);
            }
        })
    })
}

const masterVolumeChange=()=>{
    volume.volume.value=masterVolume.value-40;
}

const reverbWetChange=()=>{
    reverb.wet.value=reverbWet.value/10;
}
/*
给每个键绑定声音
*/

/*
钢琴键
*/

// 定义钢琴键的颜色模式，0 表示白键，1 表示黑键
const Colors = [0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0];
//const Colors = [0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0];

// 创建钢琴键数组
const keys = ref(
Array.from({ length: 96 }, (_, i) => ({
    isBlack: Colors[(95-i) % Colors.length] === 1,
    active: false,
    isWhite_25:Colors[(95-i) % Colors.length] === 0 && ((95-i) % Colors.length ===5 ||(95-i) % Colors.length ===11),
    isWhite_24:Colors[(95-i) % Colors.length] === 0 && ((95-i) % Colors.length ===0 ||(95-i) % Colors.length ===4 ||(95-i) % Colors.length ===7 ||(95-i) % Colors.length ===9),
    isWhite_22:Colors[(95-i) % Colors.length] === 0 && ((95-i) % Colors.length ===2),
}))
);
//定义钢琴键是否被按压


const keyOnMouseEnter=(key, index)=>{
    if (isMouseDown.value) {
        key.active=true;
        Tone.loaded().then(() => {
	        reflectSampler(tracks_info.value[activateTrack.value].instrumentId).triggerAttackRelease(sampleArr[95-index], "4n");
        });
    }
    
}
const keyOnMouseLeave=(key, index)=>{
    key.active=false;
}
const keyOnMouseUp=(key, index)=>{
    isMouseDown.value=false;
    key.active=false;
    
}
const keyOnMouseDown=(key, index)=>{
    isMouseDown.value=true;
    key.active=true;
    Tone.loaded().then(() => {
        reflectSampler(tracks_info.value[activateTrack.value].instrumentId).triggerAttackRelease(sampleArr[95-index], "4n");
    });
    
}

const isHasChild=(index)=>{
    return (95-index) % Colors.length ===0
}


/*
绘制栅格
*/

const gridSize_y=ref(14)
const gridSize_x=ref(48)

//绘制水平线
const draw_x_line=()=>{
    for (var i = 0; i< 96; i++) {
        // ctx.beginPath(); 
        // ctx.moveTo(0, gridSize_y.value * i -0.5); // -0.5是为了解决像素模糊问题
        // ctx.lineTo(width, gridSize_y.value * i-0.5);
        // if (i%12===0||(i-5)%12===0) {
        //     ctx.globalAlpha = 0.25;
        // }else{
        //     ctx.globalAlpha = 0.1;
        // }
        // ctx.strokeStyle = "white"; // 设置每个线条的颜色
        // ctx.stroke();
      var line = new Konva.Line({
          points: [0, gridSize_y.value * i , stage.width(), gridSize_y.value * i],
          stroke: 'white',
          opacity:(i%12===0||(i-7)%12===0)?0.25:0.1,
          strokeWidth: 1
      });
      layer_back.add(line);
    }
}

//绘制竖直线
const draw_y_line=()=>{
    var yLineTotals = Math.floor(stage.width() / gridSize_x.value)+1; // 计算需要绘画y轴的条数
    for (var j = 0; j < yLineTotals; j++) {
        // ctx.beginPath(); // 开启路径，设置不同的样式
        // ctx.moveTo(gridSize_x.value * j, 0);
        // ctx.lineTo(gridSize_x.value * j, height);
        // if (j%4===0) {
        //     ctx.globalAlpha = 0.25;
        // }else{
        //     ctx.globalAlpha = 0.1;
        // }
        // ctx.strokeStyle = "white"; // 设置每个线条的颜色
        // ctx.stroke();
      var line = new Konva.Line({
        points: [gridSize_x.value * j, 0, gridSize_x.value * j, stage.height()],
        stroke: 'white',
        opacity:(j%4===0)?0.25:0.1,
        strokeWidth: 1
      });
      layer_back.add(line);
    }
}

//绘制背景填充色
const draw_color=()=>{
    var rowCount = stage.height()/gridSize_y.value;

    // 上下区域交替颜色
    for (var row = 0; row < rowCount; row++) {
        var _y = row * gridSize_y.value;

        var color = row  % 2 === 0 ? "rgb(37,38,45)" : "rgb(30,31,36)";
        // ctx.fillStyle = color;

        // ctx.fillRect(0, y,width, gridSize_y.value+0.7);

        var rect = new Konva.Rect({
            x: 0,
            y: _y,
            width: stage.width(),
            height: gridSize_y.value+0.7,
            fill: color
        });
        layer_back.add(rect);
    }
}

//钢琴键
const piano_keys=ref(null)
//栅格
const canvas_container=ref(null)
//时间轴容器
const timeLineContainer=ref(null)
//时间轴
const timeLine=ref(null)
//画布宽度
const BarWidth=ref(9000)
//画布高度
const BarHeight=ref(1344)
//初始stage、layer、transformer
var timeLineStage=null
var stage=null;
var layer_back=null;
var layer_rect=null;
var layer_redLine=null
var layer_timeLine=null;
var tr_rect=null;
const MIN_WIDTH=3;
const MAX_WIDTH=48*8;
var red_line=null;
//初始光标动画
var anim=null;


var pianoStage=null;
var layer_white=null;
var layer_black=null;
const color=[0,1,0,1,0,1,0,0,1,0,1,0]
const whiiteHeightArr=[25,24,24,25,24,22,24]

const isMouseDown=ref(false);
onMounted(() => {

    instrumentList.forEach((item)=>{
        reflectSampler(item.id).connect(reverb);
    });

    Split(['#splitLeft', '#splitRight'], {
        minSize: [227, 900],
        sizes:[15,85],
        gutterSize: 5,
    })

    setInterval(updateTime, 1000); // 每秒更新时间
    setInterval(formatSecondsToTime, 41); // 每秒更新时间
    
    // const canvas = pianoRoll.value;
    // const ctx = canvas.getContext('2d');

    // ctx.canvas.width=BarWidth.value;
    // ctx.canvas.height=1360;
    
    // draw_color(ctx,ctx.canvas.width,ctx.canvas.height);
    // draw_x_line(ctx,ctx.canvas.width,ctx.canvas.height);
    // draw_y_line(ctx,ctx.canvas.width,ctx.canvas.height);

    timeLineStage = new Konva.Stage({
        container: 'timeLineContainer',  
        width: BarWidth.value+20,
        height: 30,
    });
    layer_timeLine=new Konva.Layer();  
    timeLineStage.add(layer_timeLine);
    draw_timeLine();
    layer_timeLine.batchDraw();




    stage = new Konva.Stage({
        container: 'canvas_container',  
        width: BarWidth.value,
        height: BarHeight.value,
    });
    layer_back = new Konva.Layer();  
    stage.add(layer_back);

    draw_color();
    draw_x_line();
    draw_y_line();
    layer_back.listening(false);
    layer_back.batchDraw();

    layer_rect= new Konva.Layer(); 
    stage.add(layer_rect); 

    tr_rect=new Konva.Transformer({
        boundBoxFunc: (oldBox, newBox) => {
            if (Math.abs(newBox.width) < MIN_WIDTH) {
                return oldBox;
            }
            if (Math.abs(newBox.width) > MAX_WIDTH) {
                return oldBox;
            }
            return newBox;
        },
        enabledAnchors: ['middle-left', 'middle-right'], // only allow resizing from left and right
        ignoreStroke: true,
        borderEnabled: false,
        anchorSize: 5,
        rotateEnabled:false,
    });

    layer_rect.add(tr_rect);

    layer_redLine=new Konva.Layer(); 
    red_line=new Konva.Line({
        points: [0,0,0,BarHeight.value],
        stroke: 'red',
        strokeWidth: 1,
    });
    // red_line.on('pointsChange',(e)=>{
    //     console.log(1);
    //     if (Number.isInteger(e.target)) {
            
    //     }
    // })
    stage.add(layer_redLine);
    layer_redLine.add(red_line);
    layer_redLine.draw();





    document.body.addEventListener('contextmenu', function(event) {
      event.preventDefault(); // 阻止默认行为
    });

    stage.on('click',handleStageClick);
    stage.on('dblclick',handleStageDblClick);
    stage.on('mousedown',handleStageMouseDown);
    stage.on('mouseup',handleStageMouseUp);
    stage.on('mousemove',handleStageMouseMove);


    var whiteKeysArr=[];
    var blackKeysArr=[];
    // 创建舞台
    pianoStage = new Konva.Stage({
        container: 'piano_keys',
        width: 60,
        height: 1350,
    });

    // 创建图层
    layer_white = new Konva.Layer();
    layer_black = new Konva.Layer();
    pianoStage.add(layer_white);
    pianoStage.add(layer_black);
    // 键盘配置
    var whiteKeyWidth = 60;
    var blackKeyWidth = 30;
    var blackKeyHeight = 15;


   var white_num=0
   var black_num=1
   var whiteLevel=0
   var textNum=7
    for (let i = 0; i < 96; i++) {
        if (color[i%12]===1) {
            if (black_num%7===4||black_num%7===0) {
                black_num++;
            }
            var blackKey = new Konva.Rect({
                x: 0,
                y: whiteLevel-blackKeyHeight/2,
                width: blackKeyWidth,
                height: blackKeyHeight,
                fill: 'black',
                stroke: '#8f8f8f',
                strokeWidth: 1,
                name: ''+i
            });
            blackKeysArr.push(blackKey);
            layer_black.add(blackKey);
            
            black_num++;

        }else{
            var whiteKey=null;
            if (whiiteHeightArr[white_num%7]===25) {
                whiteKey = new Konva.Rect({
                    x: 0,
                    y: whiteLevel,
                    width: whiteKeyWidth,
                    height: 25,
                    fill: 'white',
                    stroke: '#8f8f8f',
                    strokeWidth: 1,
                    name: ''+i
                });
                whiteLevel+=25;
            }else if (whiiteHeightArr[white_num%7]===24) {
                whiteKey = new Konva.Rect({
                    x: 0,
                    y: whiteLevel,
                    width: whiteKeyWidth,
                    height: 24,
                    fill: 'white',
                    stroke: '#8f8f8f',
                    strokeWidth: 1,
                    name: ''+i
                });
                whiteLevel+=24;
            }else if (whiiteHeightArr[white_num%7]===22) {
                whiteKey = new Konva.Rect({
                    x: 0,
                    y: whiteLevel,
                    width: whiteKeyWidth,
                    height: 22,
                    fill: 'white',
                    stroke: '#8f8f8f',
                    strokeWidth: 1,
                    name: ''+i
                });
                
                
                whiteLevel+=22;
            }
            whiteKeysArr.push(whiteKey);
            layer_white.add(whiteKey);
            white_num++;
        }
        if (i%12===11) {
            var text = new Konva.Text({
                x: 40,
                y: whiteLevel-18,
                text: `C${textNum--}`,
                fontSize: 15,
                fontFamily: 'Calibri',
                fill: '#8f8f8f'
            });
            layer_white.add(text);
        }
        
    }
    layer_white.draw();
    layer_black.draw();


    
    pianoStage.on('mouseleave', function() {
        isMouseDown.value=false;
    });
    whiteKeysArr.forEach((key)=>{
        key.on('mousedown', function() {
            isMouseDown.value=true;
            key.fill('rgb(139, 236, 171)');
            layer_white.draw();
            Tone.loaded().then(() => {
                reflectSampler(tracks_info.value[activateTrack.value].instrumentId).triggerAttackRelease(sampleArr[95-parseInt(key.name(), 10)], "4n");
            });

        });
        key.on('mouseup', function() {
            isMouseDown.value=false;
            key.fill('white');
            layer_white.draw();
        });
        key.on('mouseleave', function() {
            key.fill('white');
            layer_white.draw();
        });
        key.on('mouseenter', function() {
            if (isMouseDown.value) {
                key.fill('rgb(139, 236, 171)');
                layer_white.draw();
                Tone.loaded().then(() => {
                    reflectSampler(tracks_info.value[activateTrack.value].instrumentId).triggerAttackRelease(sampleArr[95-parseInt(key.name(), 10)], "4n");
                });
            }
            
        });
    });
    blackKeysArr.forEach((key)=>{
        key.on('mousedown', function() {
            isMouseDown.value=true;
            key.fill('rgb(139, 236, 171)');
            layer_black.draw();
            Tone.loaded().then(() => {
                reflectSampler(tracks_info.value[activateTrack.value].instrumentId).triggerAttackRelease(sampleArr[95-parseInt(key.name(), 10)], "4n");
            });
        });
        key.on('mouseup', function() {
            isMouseDown.value=false;
            key.fill('black');
            layer_black.draw();
        });
        key.on('mouseleave', function() {
            key.fill('black');
            layer_white.draw();
        });
        key.on('mouseenter', function() {
            if (isMouseDown.value) {
                key.fill('rgb(139, 236, 171)');
                layer_white.draw();
                Tone.loaded().then(() => {
                    reflectSampler(tracks_info.value[activateTrack.value].instrumentId).triggerAttackRelease(sampleArr[95-parseInt(key.name(), 10)], "4n");
                });
            }
        });
    });





    const piano_keysElement = piano_keys.value;
    const timeLineContainerElement =timeLineContainer.value;
    const canvas_containerElement = canvas_container.value;
    piano_keysElement.scrollTop = canvas_containerElement.scrollTop=450;

    canvas_containerElement.addEventListener('scroll', () => {
        piano_keysElement.scrollTop = canvas_containerElement.scrollTop;
        timeLineContainerElement.scrollLeft = canvas_containerElement.scrollLeft;

    });
    piano_keysElement.addEventListener('wheel', (event) => {
        
        var deltaY = event.deltaY;
        piano_keysElement.scrollTop=piano_keysElement.scrollTop+deltaY;
        canvas_containerElement.scrollTop = piano_keysElement.scrollTop;

    });
    
    reverb.connect(panner);
    panner.connect(volume);
    volume.toDestination();

    window.addEventListener('beforeunload',(event)=>{
        var confirmationMessage = '请确认文件是否保存';

    // 兼容不同的浏览器
    (event||window.event).returnValue = confirmationMessage; // Gecko, Trident, Chrome 34+
    return confirmationMessage;
    })

});

//绘制时间轴,每个结点之间间距 gridSize_x
const draw_timeLine=()=>{
    let num =1;
    var totals = Math.floor(timeLineStage.width() / gridSize_x.value)+1;
    for (var j = 0; j < totals; j++) {
        if (j%4===0) {
            var line = new Konva.Line({
                points: [j*gridSize_x.value, 10, j*gridSize_x.value, 30],
                stroke: 'white',
                opacity:0.25,
                strokeWidth: 1,
            });
            layer_timeLine.add(line);
            var numText = new Konva.Text({
                x: j*gridSize_x.value+5,
                y: 5,
                text: num++,
                fontSize: 15,
                fontFamily: 'Calibri',
                fill: '#8f8f8f'
            });
            layer_timeLine.add(numText);
        }else{
            var line = new Konva.Line({
                points: [j*gridSize_x.value, 20, j*gridSize_x.value, 30],
                stroke: 'white',
                opacity:0.1,
                strokeWidth: 1,
            });
            layer_timeLine.add(line);
        }
        

    }
}

/*
键入音符
*/
const isEditMode=ref(true)
const isSelectMode=ref(false)
const isCreateRectMode=ref(true)
const isMoveRectMode=ref(false)
const isEditRectMode=ref(false)
var isHasActivateRect=false
const allowedHeights = computed(()=>{
    const heights = [];
      for (let i = 0; i <= BarHeight.value; i += gridSize_y.value) {
        heights.push(i);
      }
      return heights;
});
const allowedTextHeights = computed(()=>{
    const heights = [];
      for (let i = 0; i <= BarHeight.value; i += gridSize_y.value) {
        heights.push(i+4);
      }
      return heights;
});
//rgb(204, 242, 217)

//定义事件
const handleStageClick=(event)=>{
    //去活
    if ((event.evt.button===0)&&(activateRect.value!==null)&&(event.target._id !== activateRect.value._id)) {
        activateRect.value.fill('rgb(139, 236, 171)');
            activateRect.value.draggable(false);
            tracks_info.value.forEach((track)=>{
                track.notes.forEach((item)=>{
                    if(item.note_rect._id===activateRect.value._id){
                        if (item.note_char!==null) {
                            item.note_char.draggable(false);
                        }
                    }
                })
            })
            activateRect.value=null;
            tr_rect.nodes([]);
            layer_rect.batchDraw();
    }
}
var start_y=9999;
const handleStageDblClick=(event)=>{
    if (event.evt.button===0) {  //左键
        if (!isPlaying.value&&isEditMode&&isCreateRectMode&&(activateRect.value==null)) {
            var rect = new Konva.Rect({
                x: Math.floor(stage.getPointerPosition().x / gridSize_x.value)*gridSize_x.value,
                y: Math.floor(stage.getPointerPosition().y / gridSize_y.value)*gridSize_y.value,
                width: gridSize_x.value,
                height: gridSize_y.value,
                fill: 'rgb(139, 236, 171)',
                name:'rect'
            });
            rect.on('dblclick',function(e) {
                if (!isPlaying.value&&isRectInActivateTrack(e.target)&&(activateRect.value===null)&&(e.evt.button===2)){
                    tracks_info.value[activateTrack.value].notes.forEach((item)=>{
                            if(item.note_rect._id===e.target._id){
                                if (item.note_char!==null) {
                                    textArr=textArr.filter(text=>text._id===item.note_char._id);
                                    console.log(textArr);
                                    item.note_char.destroy();
                                    item.note_char=null;
                                }
                            }
                        })
                    tracks_info.value[activateTrack.value].notes=tracks_info.value[activateTrack.value].notes.filter(t_note=>t_note.note_rect._id!==e.target._id);
                    e.target.destroy();
                    layer_rect.batchDraw();
                }
            });
            rect.on('click', (e) => {
                if (!isPlaying.value&&e.evt.button===0 ){
                    //去活
                    
                    if (activateRect.value!==null) {
                        activateRect.value.fill('rgb(139, 236, 171)');
                        activateRect.value.draggable(false);
                        tracks_info.value.forEach((track)=>{
                            track.notes.forEach((item)=>{
                                if(item.note_rect._id===activateRect.value._id){
                                    if (item.note_char!==null) {
                                        item.note_char.draggable(false);
                                    }
                                }
                            })
                        })

                        tr_rect.nodes([]);
                        activateRect.value=null;
                    }
                    //若在激活音轨内，进行可编辑操作
                    if (isRectInActivateTrack(e.target)) {
                        

                        activateRect.value=e.target;
                        e.target.fill('rgb(204, 242, 217)');
                        e.target.draggable(true);
                        layer_rect.batchDraw();
                        stage.container().style.cursor = 'move';
                        var velocity=1;
                        tracks_info.value.forEach((track)=>{
                            track.notes.forEach((item)=>{
                                if(item.note_rect._id===activateRect.value._id){
                                    velocity=item.note_velocity;

                                }
                            })
                        })
                        tr_rect.nodes([e.target]);
                        tr_rect.show();
                        reflectSampler(tracks_info.value[activateTrack.value].instrumentId).triggerAttackRelease(
                            sampleArr[95-Math.floor(e.target.y()/gridSize_y.value)], 
                            new Decimal(e.target.width()).mul(new Decimal(e.target.scaleX())).mul(new Decimal(60)).div(new Decimal(bpm.value)).div(new Decimal(gridSize_x.value)).toNumber(),
                            Tone.now(),
                            velocity
                        );
                        
                    }
                }
            });
            rect.on('mouseenter', function(e) {
                if (activateRect.value&&activateRect.value._id===e.target._id) {
                    stage.container().style.cursor = 'move';
                }
            });

            rect.on('mouseleave', function() {
                stage.container().style.cursor = 'default';
            });
            rect.on('transform', function(e) {
                sustainControl.value=e.target.scaleX()*16;
            });
            rect.on('dragmove', function(e) {
                let newY = e.target.y();
                let nearestHeight = allowedHeights.value.reduce((prev, curr) => {
                    return (Math.abs(curr - newY) < Math.abs(prev - newY) ? curr : prev);
                });

                // 如果当前高度不在允许的范围内，就更新矩形的位置
                if (allowedHeights.value.indexOf(newY) === -1) {
                    e.target.y(nearestHeight); // 设置矩形的新高度
                    
                }
                tracks_info.value.forEach((track)=>{
                    track.notes.forEach((item)=>{
                        if(item.note_rect._id===e.target._id && item.note_char!==null){
                            item.note_char.x(e.target.x()+4);
                            item.note_char.y(e.target.y()+4);
                        }
                    })
                })
                //移动音符赋音 横向移动不会发出音 y改变才有声音发出
                if (start_y !==e.target.y()) {
                    var velocity=1;
                    tracks_info.value.forEach((track)=>{
                        track.notes.forEach((item)=>{
                            if(item.note_rect._id===activateRect.value._id){
                                velocity=item.note_velocity;
                            }
                        })
                    })
                    reflectSampler(tracks_info.value[activateTrack.value].instrumentId).triggerAttackRelease(
                        sampleArr[95-Math.floor(e.target.y()/gridSize_y.value)],
                        new Decimal(e.target.width()).mul(new Decimal(e.target.scaleX())).mul(new Decimal(60)).div(new Decimal(bpm.value)).div(new Decimal(gridSize_x.value)).toNumber(),
                        Tone.now(),
                        velocity
                    );
                }
                start_y=e.target.y()
            });
            
           // console.log(rect);
            tracks_info.value[activateTrack.value].notes.push({
                "note_rect":rect,
                "note_velocity":1,
                "note_char":null,
            });
            layer_rect.add(rect);
            layer_rect.batchDraw();
        }
    }
}
const handleStageMouseDown=(event)=>{
}
const handleStageMouseUp=(event)=>{

}
const handleStageMouseMove=()=>{
    // if (activateRect!==null) {
    //     console.log(activateRect.width()*activateRect.scaleX());
    // }
}
//判断矩形是否在激活的音轨内
const isRectInActivateTrack=(rect)=>{
    var flag=false;
    tracks_info.value[activateTrack.value].notes.forEach((fnote)=>{
        if(fnote.note_rect._id === rect._id){
            flag= true;
        }
    })
    return flag;
}


/*
player
*/
//bpm在585行定义
const panner= new Tone.Panner(0)
const volume=new Tone.Volume(0)
const reverb=new Tone.Reverb({
    decay: 1.5, // 混响时间，单位为秒
    preDelay: 0.01, // 预延迟时间，单位为秒
    wet: 0 // 湿信号的初始增益值
})


const isPlaying=ref(false)
const pause=()=>{
    isPlaying.value=false;
    Tone.getTransport().pause();
    if (anim!==null) {
        anim.stop();
    }
}

var midi=null;
const play=()=>{
    isPlaying.value=true;
    if ((activateRect.value!==null)) {
        activateRect.value.fill('rgb(139,236,171)');
            activateRect.value.draggable(false);
            activateRect.value=null;
            tr_rect.nodes([]);
            layer_rect.batchDraw();
    }

    if (Tone.getTransport().seconds===0) {
        midi = new Midi();
        midi.header.setTempo(bpm.value);
        tracks_info.value.forEach((item)=>{
            if (item.notes.length>0) {

                reflectSampler(item.instrumentId).connect(reverb);
                //item.volume.connect(panner);

                const track = midi.addTrack();
                item.notes.forEach((i_note)=>{
                    var note_time=new Decimal(i_note.note_rect.x()).mul(new Decimal(60)).div(new Decimal(bpm.value)).div(new Decimal(gridSize_x.value)).toNumber();
                    var note_duration=new Decimal(i_note.note_rect.width()).mul(new Decimal(i_note.note_rect.scaleX())).mul(new Decimal(60)).div(new Decimal(bpm.value)).div(new Decimal(gridSize_x.value)).toNumber();
                    track.addNote({
                        name : sampleArr[95-Math.floor(i_note.note_rect.y()/gridSize_y.value)],
                        time : note_time,
                        duration: note_duration,
                        velocity: i_note.note_velocity
                    })  
                    Tone.getTransport().schedule(time=>{

                        reflectSampler(item.instrumentId).triggerAttackRelease(
                            sampleArr[95-Math.floor(i_note.note_rect.y()/gridSize_y.value)],         // 音名
                            note_duration,     // 持续时间
                            time,   // 开始发声时间
                            i_note.note_velocity
                        );
            //             new Tone.PolySynth(Tone.Synth, {
			// 		envelope: {
            // // 声音的生命周期：按下按键 - 渐入 - 攻击阶段 - 衰减阶段 - 衰减结束 - 松开按键 - 声音消逝
			// 			attack: 0.01,     // 渐入时间
			// 			decay: 0.2,       // 攻击阶段（最大音量）持续时间
			// 			sustain: 0.7,     // 衰减结束后的最小声音
			// 			release: 0.5,       // 从松开按键到声音彻底消失所需的时间
			// 		},
			// 	}).connect(panner).triggerAttackRelease(
            //                 sampleArr[95-Math.floor(i_note.note_rect.y()/gridSize_y.value)],         // 音名
            //                 note_duration,     // 持续时间
            //                 time,   // 开始发声时间
            //                 i_note.note_velocity
            //             );
                    },note_time)
                }) 
            }
        })
        
        anim=new Konva.Animation((frame)=>{
            var timeDiff=frame.timeDiff/1000;
            var new_x=red_line.x()+timeDiff*gridSize_x.value*bpm.value/60;
            if (new_x>stage.width()) {
                new_x=0;
            }
            red_line.x(new_x);
        },layer_redLine);
    }
    Tone.getTransport().start();
    anim.start();

}

const playSeconds=ref('00:00:000')
const formatSecondsToTime=()=> {
    var seconds=Tone.getTransport().seconds;
    const minutes = Math.floor(seconds / 60).toString().padStart(2, '0');
    const remainingSeconds = seconds % 60;
    const milliseconds = Math.floor((remainingSeconds % 1) * 1000).toString().padStart(3, '0');
    const wholeSeconds = Math.floor(remainingSeconds).toString().padStart(2, '0');

    playSeconds.value= `${minutes}:${wholeSeconds}:${milliseconds}`;
}
//终止键
const endClick=()=>{
    isPlaying.value=false;
    Tone.getTransport().stop();
    Tone.getTransport().cancel(0);
    anim.stop();
    red_line.x(0);

}
//前进键
const forwardClick=()=>{
    var now =Tone.getTransport().seconds;
    var dtime=Math.floor(now*bpm.value/240)+1;
    Tone.getTransport().seconds=dtime*240/bpm.value;
    var dx=Math.floor(red_line.x()/(4*gridSize_x.value))+1;
    red_line.x(dx*gridSize_x.value*4);
}
//后退键
const backClick=()=>{
    var now =Tone.getTransport().seconds;
    var dtime=Math.floor(now*bpm.value/240);
    Tone.getTransport().seconds=dtime*240/bpm.value;
    var dx=Math.floor(red_line.x()/(4*gridSize_x.value));
    red_line.x(dx*gridSize_x.value*4);
}
const backDblclick=()=>{
    var now =Tone.getTransport().seconds;
    var dtime=Math.floor(now*bpm.value/240)-1;
    if (dtime<0) {
        dtime=0;
    }
    var dx=Math.floor(red_line.x()/(4*gridSize_x.value))-1;
    if (dx<0) {
        dx=0;
    }
    red_line.x(dx*gridSize_x.value*4);
    Tone.getTransport().seconds=dtime*240/bpm.value;
}


</script>
  
<style scoped>
.all{
    height:100%;
    width: 100%;
    background-color: #ffffff;
    display: flex;
    flex-direction: column;
}
.header{
    width: 100%;
    height: 50px;
    background-color: rgb(30,31,36);
    display: flex;
}
.theimport,.thecloudImport,.thecloud,.thesave,.thehelp{
    height: 50px;
    width: 120px;
    color: #8f8f8f;
    text-align: center;
    line-height: 50px;
    font-weight:normal;
    font-size: 12px;
    cursor:pointer;
    user-select: none;
}
.currentTime{
    height: 50px;
    width: 100px;
    color: #8f8f8f;
    text-align: center;
    line-height: 50px;
    font-weight:normal;
    font-size: 15px;
    user-select: none;
}
.thennone1{
    height: 50px;
    width: 15px;
    user-select: none;
}
.thennone{
    height: 50px;
    width: 100%;
    color: #8f8f8f;
    text-align: center;
    line-height: 50px;
    font-weight:normal;
    font-size: 15px;
    user-select: none;
}
.theIcon{
    position: relative;
    top: 5px;
}


.split {
    display: flex;
    flex-direction: row;
    border-top:1px solid rgb(77, 77, 77);
}
.gutter {
    background-color: #a2a2a2;
    background-repeat: no-repeat;
    background-position: 50%;
}
.gutter.gutter-horizontal {
    background-image: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAeCAYAAADkftS9AAAAIklEQVQoU2M4c+bMfxAGAgYYmwGrIIiDjrELjpo5aiZeMwF+yNnOs5KSvgAAAABJRU5ErkJggg==');
    cursor: col-resize;
}

.splitLeft{
    background-color: rgb(37, 38, 45);
    padding-top: 5px;
    padding-bottom: 5px;
    height: 520.5px;
    overflow: auto;
}

.splitLeft::-webkit-scrollbar {
    width: 8px;
    height:  8px;
}

.splitLeft::-webkit-scrollbar-track {
    background: rgb(40, 34, 34);
}

.splitLeft::-webkit-scrollbar-thumb {
    background: #646464;
}

.splitLeft::-webkit-scrollbar-thumb:hover {
    background: #cccccc;
}



.addtrack{
    width: calc(100% - 12px);
    text-align: center;
    height: 30px;
    line-height: 30px;
    color: #8f8f8f;
    margin-top: 8px;
    margin-left: 6px;
    border-radius: 8px;
    font-size: 14px;
    font-weight: 550;
    margin-bottom: 8px;
}
.addtrack:hover{
    background-color: rgb(46, 48, 57);
    cursor: pointer;
    user-select: none;
}
.addtrackIcon{
    position: relative;
    top: 2px;
}

.track-item{
    width: calc(100% - 12px);
    height: 60px;
    margin-top: 8px;
    margin-left: 6px;
    border-radius: 8px;
    user-select: none;

}
.track-item:hover{
    background-color: rgb(46, 48, 57);
}
.track-item.selected-track-item{
    background-color: rgb(46, 48, 57);
}
.el-dropdown{
    height: 100%;
    width: 100%;
}
.el-tooltip__trigger:has(.logo){
    height: 100%;
    width: 100%;
    display: flex;
    align-items: center; 
}
.logo{
    width: 40px;
    height: 40px;
    border-radius: 21px;
    background-color: rgb(50,53,62);
    text-align: center;
    line-height: 40px;
    border: 2px solid #42b983;
    margin-left: 10px;
}
.name{
    width: 60px;
    height: 50px;
    text-align: center;
    line-height: 50px;
    margin-left: 10px;
    color: #e2e2e2;
}
.instrument{
    width: 100px;
    height: 50px;
    text-align: center;
    line-height: 50px;
    margin-left: 5px;
    color: #8f8f8f;
}







.timeLineBar{
    width: 100%;
    height: 30px;
    display: flex;
    background-color: rgb(37,38,45);
    
    border-bottom: 1px solid rgba(255, 255, 255,0.25);
}
.fillBlock{
    height: 30px;
    width: 60px;
    /* margin-left: 6.9px; */
    background-color: rgb(37,38,45);
}

.timeLineContainer{
    height: 30px;
    width: 100%;
    background-color: rgb(37,38,45);
    overflow: hidden;
    margin-left: 0;
    
    /* border-left: 1px solid rgba(255, 255, 255,0.25); */
}

#timeLine{
    background-color:rgb(37,38,45);
}


.content{
    display: flex;
    height: 500px; 
    width: 100%; 
}
.piano-keys {

    height: 500px; 
    cursor:pointer;
    width: 60px;
    overflow: hidden;
    
}

.canvas-container{
    overflow: auto;
    height: 500px; 
    width: 100%;
    background-color: rgb(37,38,45);

}

.canvas-container::-webkit-scrollbar {
    width: 8px;
    height:  8px;
}

.canvas-container::-webkit-scrollbar-track {
    background: rgb(40, 34, 34);
}

.canvas-container::-webkit-scrollbar-thumb {
    background: #646464;
}

.canvas-container::-webkit-scrollbar-thumb:hover {
    background: #cccccc;
}
.controll {
    flex-grow: 1;
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: rgb(30,31,36);
    border-top:1px solid rgb(77, 77, 77);
    border-bottom:1px solid rgb(77, 77, 77);

    
}

.controllpart{
    height: 90%;
    width: 120px;
    border-radius: 10px;
    background-color: rgb(37, 38, 45);
    display: flex;
    justify-content: center;
    flex-direction: column;
    margin-left: 5px;
    margin-right: 5px;
}
.controllslider{
    margin-left: auto;
    margin-right: auto;
}

.el-slider {
  --el-slider-main-bg-color: #42b983;
  --el-slider-button-size: 12px;
  --el-slider-runway-bg-color:#8f8f8f;
}

.controllinfo{
    margin-top: 10px;
    width: 100%;
    text-align: center;
    height: 25px;
    line-height: 25px;
    color: #8f8f8f;
    font-size: 13px;
}

.inputText{
    width: 240px;
    border-radius: 10px;
    background-color: rgb(37, 38, 45);
    height: 90%;
    margin-left: 5px;
    margin-right: 5px;

}

.el-textarea{
    --el-input-bg-color: rgb(37, 38, 45);
    --el-input-border-color: #8f8f8f;
    --el-input-text-color:rgb(222, 222, 222);
    --el-input-focus-border-color: #4bd597;
}
.submit{
    width: 50px;
    border-radius: 10px;
    background-color: rgb(37, 38, 45);
    height: 90%;
    margin-left: 5px;
    margin-right: 5px;
    display: flex;
    justify-content: center;
    align-items: center;
    user-select: none;
    cursor: pointer;
}
.submit:hover{
    background-color: rgb(44, 45, 53);
}

.outputText{
    height: 90%;
    margin-left: 5px;
    margin-right: 5px;
    width: 500px;
    background-color: rgb(37, 38, 45);
    border-radius: 10px;
}
.importText{
    width: 50px;
    height: 90%;
    margin-left: 5px;
    margin-right: 5px;
    background-color: rgb(37, 38, 45);
    border-radius: 10px;
    display: flex;
    justify-content: center;
    align-items: center;
    user-select: none;
    cursor: pointer;
}
.importText:hover{
    background-color: rgb(44, 45, 53);
}
.importTextinfo{
    width: 15px;
    font-size: 15px;
    line-height: 20px;
    height: 80px;
    text-align: center;
    color: #8f8f8f;
    font-weight: 400;
}





.player{
    height: 50px;
    width: 100%;
    background-color: rgb(37,38,45);
    display: flex;
    justify-content: center;
}
.backIcon,.endIcon,.playerIcon,.forwardIcon,.bpmIcon,.lineIcon,.pauseIcon{
    height: 50px;
    width: 35px;
    display: flex;
    flex-direction: column;
    justify-content: center;

}
.pauseicon{
    border-radius: 17.5px;
    position: relative;
    left: 2px;
}
.pauseicon:hover{
    background-color: rgb(67, 68, 73);
    cursor: pointer;
}
.playericon{
    border-radius: 17.5px;
}
.playericon:hover{
    background-color: rgb(67, 68, 73);
    cursor: pointer;
}
.lineIcon{
    height: 50px;
    width: 20px;
    display: flex;
    flex-direction: column;
    justify-content: center;
}
.circle{
    height: 30px;
    width: 30px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    margin: 0;
    padding: 0;
    border-radius: 15px;
}
.circle:hover{
    background-color: rgb(67, 68, 73);
    cursor: pointer;
}
.bpmValue{
    height: 50px;
    width: 100px;
    display: flex;
    flex-direction: column;
    justify-content: center;
}
.c_bpmValue{
    height: 30px;
    width: 100%;
    margin-left:5px;
    margin-right:5px;
    display: flex;
}
.bpm{
    width:30px;
    font-size: 10px;
    line-height: 30px;
    text-align: start;
    color: #8f8f8f;
    user-select: none; 
}
.bValue{
    width: 70px;
    /* font-size: 15px;
    font-weight: 500;
    font-family:'Courier New', Courier, monospace;
    line-height: 30px;
    text-align: center; */
    background-color: rgb(37, 38, 45);
    outline: none;
    border: none;
    line-height: 30px;
    height: 30px;
    color: #ffffff;
    text-align: center; 
    font-weight: 500;
    font-size: 15px;
    
}

.timeIcon{
    height: 50px;
    width: 90px;
    display: flex;
    flex-direction: column;
    justify-content: center;
}
.timeValue{
    height: 30px;
    width: 100%;
    text-align: center;
    line-height: 30px;
    font-weight: 500;
    font-size: 15px;
    color: #8f8f8f;
    user-select: none;

}





/* 弹出框效果 */
.el-button.el-button--primary{
    background-color: #42b983;
    border: none;
    color: #efefef;
}
.el-button.el-button--primary:hover{
    background-color: #42b983;
    border: none;
    color: #efefef;
}
.el-message-box{
    background-color: rgb(37,38,45);
}
.el-message-box__title{
    color: #efefef;
}
.el-message-box__message{
    color: #efefef;
}
.el-button:has(span){
    background-color: rgb(37,38,45);
    border: none;
    color: #efefef;
}
.el-button:has(span):hover{
    background-color: rgb(46, 48, 57);
    color: #efefef;
}


.el-dialog__title{
    --el-text-color-primary: #efefef;
}
.el-form-item__label{
    color: #efefef;
}
</style>
<!-- <style>
:root{
    --el-text-color-primary:#efefef;
}
</style> -->