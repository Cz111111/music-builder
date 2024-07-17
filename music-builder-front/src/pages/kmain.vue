<template>
  <div class="kmain-container">
    <div class="top-nav">
      <el-button type="danger" @click="goBack">返回</el-button>
    </div>
    <div class="lyrics-display">
      <div class="lyric-line" v-for="(line, index) in currentLines" :key="index">
        {{ line.text }}
      </div>
    </div>
    <div class="waveformcontainer">
      <div id="waveform" style="width: 500px; height: 100px"></div>
    </div>
    <div class="pitch-visualization">
      <div class="tag-container">
        <div class="line"></div>
        <div class="arrow" :style="{ transform: `translateY(${level}px)` }"></div>
      </div>
      <div id="echarts-container" style="height: 275px; width: 500px"></div>
    </div>
    <div class="progress-controller">
      <div>
        <el-card class="black-card">
          <div class="controls">
            <el-button @click="togglePlay" type="danger">点击播放</el-button>
            <el-button @click="rePlay" type="danger">重唱</el-button>
            <el-button @click="stopRecording" type="danger">结束</el-button>
            <el-button type="danger" @click="reload"><a :href="downloadUrl" download="recording.wav">下载录音</a></el-button>
            <div class="pei-ban">
              <p>伴奏:</p>
            </div>
            <el-slider v-model="audioVolume" :max="100" @input="setVolume"></el-slider>
            <div class="pei-ban">
              <p>人声:</p>
            </div>
            <el-slider v-model="vocalaudioVolume" :max="100" @input="vocalsetVolume"></el-slider>
          </div>
          <div>
            <el-slider v-model="audiocurrentTime" :max="audioduration" class="custom-slider"></el-slider>
            <audio ref="audio" :src="currentTrack.src"></audio>
            <audio ref="vocalaudio" :src="vocalcurrentTrack.src"></audio>
            <div class="time-display">
              <div>{{ shownowTime }} / {{ showduration }}</div>
            </div>
          </div>
        </el-card>
      </div>
    </div>
  </div>
</template>

<script setup>
import WaveSurfer from "wavesurfer.js";
import { ref } from "vue";
import { useRouter } from "vue-router";
import { computed, onMounted } from "vue";
import * as echarts from "echarts";
import { defineProps } from "vue";

//下载后刷新页面
const reload =()=>{
  window.location.reload();
};

//图表
const totalData = ref(JSON.parse(localStorage.getItem('responseData')));
const index = ref(54); // 新增指标index
const windowData = ref(totalData.value.slice(0, 50));
const chartInstance = ref(null);
const timer = ref(null);
const collectData = () => {
  windowData.value.shift();
  windowData.value.push(totalData.value[index.value]);
  index.value = index.value + 1;
  // 更新图表数据
  chartInstance.value.setOption({
    title: {
      text: "Hz",
    },
    xAxis: {
      data: [],
    },
    yAxis: {
      splitLine: {
        // 去除网格线
        show: false,
      },
      max: 601,
      min: 0,
    },
    series: [
      {
        type: "bar",
        barGap: 0,
        barCategoryGap: 0,
        data: windowData.value,
      },
    ],
  });
};

//游标与录音连接
const volumeLevel = ref(0); // 当前音量值
const audioContext = ref(null);
const analyser = ref(null);

//左侧游标
const level = ref(-20);
const syncLevelWithVolumeLevel = () => {
  level.value = volumeLevel.value * 3;
  level.value = calculateArrowPosition(level.value);
};

const calculateArrowPosition = (level) => {
  const y = -1.4 * level - 20;
  return y;
};

//MP3波形显示部分
var wavesurfer = null;
onMounted(() => {
  vocalaudioVolume.value = 100;
  audioVolume.value = 100;
  let basePath = "/src/assets/output/";
  myurl.value = basePath + localStorage.getItem("Name");
  //游标部分
  const timerId = setInterval(syncLevelWithVolumeLevel, 100);
  wavesurfer = WaveSurfer.create({
    container: "#waveform",
    waveColor: "rgb(200, 0, 200)",
    progressColor: "rgb(100, 0, 100)",
    url: myurl.value + "/accompaniment.wav",
  });
  const dom = document.getElementById("echarts-container");
  if (!dom) return;
  chartInstance.value = echarts.init(dom);
  chartInstance.value.setOption({
    animation: false,
    title: {
      text: "Hz",
    },
    xAxis: {
      data: [],
    },
    yAxis: {
      splitLine: {
        // 去除网格线
        show: false,
      },
      max: 601,
      min: 0,
    },
    series: [
      {
        type: "bar",
        barGap: 0,
        barCategoryGap: 0,
        data: windowData.value,
      },
    ],
  });
});
//返回按钮部分
const router = useRouter();
function goBack() {
  router.push({ path: "/upload" });

}
const myurl = ref(null);
//播放音量控制部分
const showduration = ref(null); //显示字符串
const shownowTime = ref(null); //显示字符串
const audio = ref(null);
const vocalaudio = ref(null);
const currentTrack = ref({
  src:
    "/src/assets/output/" + localStorage.getItem("Name") + "/accompaniment.wav",
});
const vocalcurrentTrack = ref({
  src: "/src/assets/output/" + localStorage.getItem("Name") + "/vocals.wav",
});
//const volume = ref(70);
const isPlaying = ref(false); //辅助变量判断有没有播放
const audioVolume = ref(1); // 假设初始音量为 1（100%）
const vocalaudioVolume = ref(1); // 假设初始音量为 1（100%）
const audiocurrentTime = ref(0);
const audioduration = ref(0);
// 定义一个引用来保存定时器的ID
const intervalId = ref(null);
const transfer = (second) => {
  // 使用 Math.floor 确保分钟和秒数都是整数
  const minutes = Math.floor(second / 60);
  const seconds = Math.floor(second % 60);
  // 使用 padStart 确保秒数是两位数格式
  const formattedSeconds = seconds.toString().padStart(2, "0");
  // 返回格式化的时间字符串
  return `${minutes}:${formattedSeconds}`;
};
const update = () => {
  // 这里添加你希望每秒执行的代码
  //console.log(audiocurrentTime.value);
  audiocurrentTime.value = audio.value.currentTime;
  shownowTime.value = transfer(audiocurrentTime.value); // "2:00"
  updateLyrics(audiocurrentTime.value);
};
const setVolume = (value) => {
  // 更新 audio 元素的音量，将值转换为 0-1 范围
  audio.value.volume = value / 100;
  // 同时更新响应式引用的音量值
  audioVolume.value = value;
};
const vocalsetVolume = (value) => {
  // 更新 audio 元素的音量，将值转换为 0-1 范围
  vocalaudio.value.volume = value / 100;
  // 同时更新响应式引用的音量值
  vocalaudioVolume.value = value;
};

const rePlay = () => {
  audio.value.load();
  audio.value.play();
};
const togglePlay = () => {
  if (audio.value === null) {
    audio.value = new Audio();
  }
  audioduration.value = audio.value.duration;
  showduration.value = transfer(audioduration.value);
  if (vocalaudio.value === null) {
    vocalaudio.value = new Audio();
  }
  parseLRC(mylrcText.value);
  audio.value.play();
  vocalaudio.value.play();
  isPlaying.value = !isPlaying.value;
  intervalId.value = setInterval(update, 1000);
  startRecording();
  wavesurfer.play();
  timer.value = setInterval(() => {
    collectData();
  }, 250);
};

//歌词滚动部分
const lyrics = ref([]); // 存储解析后的歌词数据
const currentLines = ref([]); // 当前显示的歌词行
const mylrcText = ref(localStorage.getItem("lrcData"));
const parseLRC = (lrcText) => {
  const lines = lrcText.split("\n").filter((line) => line.trim() !== "");
  lyrics.value = lines.map((line) => {
    const [time, text] = line.split("]");
    const [mm, ss, ms] = time
      .substring(1)
      .split(":")
      .map((part) => parseInt(part, 10));
    // 将时间转换为秒（包括毫秒部分）
    const timeInSeconds = mm * 60 + ss;
    return { time: timeInSeconds, text: text.trim() };
  });
  // 根据时间戳对歌词进行排序
  lyrics.value.sort((a, b) => a.time - b.time);
};
const updateLyrics = (currentTime) => {
  let currentLineIndex = -1;
  for (let i = 0; i < lyrics.value.length; i++) {
    if (currentTime >= lyrics.value[i].time) {
      currentLineIndex = i;
    }
  }
  // 显示当前时间的歌词行和接下来的两行
  const startLineIndex = Math.max(currentLineIndex - 1, 0);
  const endLineIndex = Math.min(startLineIndex + 3, lyrics.value.length);
  currentLines.value = lyrics.value.slice(startLineIndex, endLineIndex);
};

//录音部分
const isRecording = ref(false);
const mediaRecorder = ref(null);
const volumeInterval = ref(null);
const chunks = ref([]);
const recordingBlob = ref(null);
const downloadUrl = ref(null);
const startRecording = () => {
  navigator.mediaDevices
    .getUserMedia({ audio: true })
    .then((stream) => {
      mediaRecorder.value = new MediaRecorder(stream);
      mediaRecorder.value.ondataavailable = (event) => {
        chunks.value.push(event.data);
      };
      mediaRecorder.value.start();
      isRecording.value = true;

      //开始杂交
      const audioContextInstance = new (window.AudioContext ||
        window.webkitAudioContext)();
      analyser.value = audioContextInstance.createAnalyser();
      const source = audioContextInstance.createMediaStreamSource(stream);
      source.connect(analyser.value);
      analyser.value.connect(audioContextInstance.destination);
      audioContext.value = audioContextInstance;

      // 设置分析器参数
      analyser.value.fftSize = 512;
      const bufferLength = analyser.value.frequencyBinCount;
      const dataArray = new Uint8Array(bufferLength);

      // 开始录音并实时更新音量
      audioContext.value.resume().then(() => {
        volumeInterval.value = setInterval(() => {
          analyser.value.getByteFrequencyData(dataArray);
          const volume = dataArray[0]; // 取第一个数据点作为音量估计
          volumeLevel.value = volume ? Math.round(10 * Math.log10(volume)) : 0; // 转换为分贝值
        }, 100); // 每秒更新10次
      });
    })
    .catch((err) => {
      console.error("无法访问麦克风:", err);
    });
};
const stopRecording = () => {
  mediaRecorder.value.stop();
  isRecording.value = false;
  mediaRecorder.value.onstop = () => {
    recordingBlob.value = new Blob(chunks.value, { type: "audio/wav" });
    downloadUrl.value = URL.createObjectURL(recordingBlob.value);
    chunks.value = []; // 重置chunks以便下次录音
  };
  // 停止 AudioContext
  if (audioContext.value) {
    audioContext.value.suspend().then(() => {
      // 断开所有连接
      if (analyser.value) analyser.value.disconnect();
      // 如果有其他音频节点，也需要在这里断开连接
    });
    audioContext.value.close(); // 关闭音频上下文
    audioContext.value = null; // 重置引用
  }
  wavesurfer.pause();
  audio.value.pause();
  vocalaudio.value.pause();
  clearInterval(intervalId.value);
  clearInterval(timer.value);
  clearInterval(volumeInterval.value);
};
</script>

<style scoped>
.drawcontainer {
  display: flex;
  /* 使用Flexbox布局 */
  justify-content: space-between;
  /* 左右排列，并且中间留有间隔 */
  align-items: flex-start;
  /* 垂直方向上对齐方式，这里为顶部对齐，根据需要调整 */
  height: 600px;
  /* 根据需要调整 */
  overflow: hidden;
}

.waveformcontainer {
  display: flex;
  justify-content: center;
  /* 水平居中 */
  align-items: center;
  /* 垂直居中 */
  height: 200px;
  /* 视口高度 */
}

.tag-container {
  position: relative;
  height: 100%;
  width: auto;
  /* 宽度根据内容自动调整 */
  /* 根据需要调整高度 */
}

.line {
  width: 2px;
  /* 线的宽度 */
  height: 95%;
  /* 线的高度 */
  background: gray;
  margin-left: 10px;
  /* 根据需要调整位置 */
}

.arrow {
  width: 0;
  height: 0;
  border-top: 10px solid transparent;
  /* 左上角的透明边框 */
  border-bottom: 10px solid transparent;
  /* 右下角的透明边框 */
  border-left: 15px solid white;
  /* 改变这里来创建向右的箭头 */
  position: absolute;
  top: 100%;
  left: 0;
  /* 可以根据需要调整箭头的位置 */
  transform: translateY(-50%);
  /* 将箭头垂直居中对齐 */
  transition: transform 0.3s;
  /* 平滑过渡效果 */
}

/* 覆盖 Element UI 的 el-slider 组件样式 */
/* 选择所有的 <a> 元素并改变它们的颜色 */
.left-div {
  height: 200px;
  padding: 0px;
  box-sizing: border-box;
  /* 边框和内边距计算在宽度内 */
  text-align: center;
  /* 文本居中 */
  width: 30px;
  /* 直接指定一个固定的宽度值 */
}

.right-div {
  flex: 1;
  /* 子元素按比例分配空间，可以根据需要调整 */
  height: 200px;
  min-width: 100px;
  /* 或者其他适当的值 */
  padding: 10px;
  box-sizing: border-box;
  /* 边框和内边距计算在宽度内 */
  text-align: center;
  /* 文本居中 */
}

.myleft,
.myright {
  float: left;
  /* 使元素左右排列 */
  width: 50%;
  /* 各占50%宽度，根据需要调整 */
  box-sizing: border-box;
  /* 边框和内边距计算在宽度内 */
}

.el-slider {
  --el-slider-main-bg-color: red;
}

a {
  color: white;
}

.black-card {
  background-color: rgb(49, 46, 46);
  color: white;
  /* 设置文字颜色为白色，以提高可读性 */
}

.kmain-container {
  display: flex;
  flex-direction: column;
  height: 700px;
  width: 1000px;
  overflow: hidden;
  /* 防止内容溢出 */
}

.top-nav {
  flex: 0 1 auto;
  /* 不扩展，保持自身大小 */
  padding: 10px;
  display: flex;
  justify-content: flex-start;
  /* 左对齐 */
  align-items: center;
  justify-content: flex-start;
  /* 按钮靠左对齐 */
}

.lyrics-display {
  flex: 1;
  /* 占据可用空间 */
  overflow-y: auto;
  /* 垂直滚动 */
  padding: 10px;
  /* 内边距 */
  border: 1px solid white;
  /* 白色边框 */
  border-radius: 8px;
  /* 添加圆角边框，8px是圆角的半径大小，可以根据需要调整 */
  width: 800px;
  margin-left: auto;
  margin-right: auto;
}

.pitch-visualization {
  flex: 2;
  /* 比歌词显示区域多一倍空间 */
  overflow-y: auto;
  /* 垂直滚动 */
  padding: 10px;
  display: flex;
  /* 使父容器成为Flexbox容器 */
  justify-content: center;
  /* 水平居中 */
  align-items: center;
  /* 垂直居中 */

  overflow: hidden;
}

.progress-controller {
  flex: 0 1 auto;
  /* 不扩展，保持自身大小 */
  padding: 10px;
}

.time-display {
  margin-left: auto;
  /* 将时间推向右侧对齐 */
  font-family: Arial, sans-serif;
  /* 设置字体 */
  font-size: 14px;
  /* 设置字体大小 */
  color: #f3ecec;
  /* 设置字体颜色 */
}

.controls {
  display: flex;
  /* 使用Flexbox布局 */
  align-items: center;
  /* 垂直居中对齐 */
  justify-content: space-around;
  /* 沿主轴（水平方向）分配空间 */
}

.pei-ban {
  display: block;
  /* 使元素成为块级元素 */
  margin-left: auto;
  margin-right: 10px;
  width: 25%;
  /* 可以设置为你需要的宽度 */
  /* 其他样式属性 */
}
</style>
