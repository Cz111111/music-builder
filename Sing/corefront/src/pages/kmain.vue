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
      <div id="waveform" style="width: 500px; height: 100px;"></div>
    </div>
    <div class="pitch-visualization">
      <div class="tag-container">
        <div class="line"></div>
        <div class="arrow" :style="{ transform: `translateY(${level}px)` }"></div>
      </div>
      <div id="echarts-container" style="height:275px;width:500px"></div>
    </div>
    <div class="progress-controller">
      <div>
        <el-card class="black-card">
          <div class="controls">
            <el-button @click="togglePlay" type="danger">点击播放</el-button>
            <el-button @click="rePlay" type="danger">重唱</el-button>
            <el-button @click="stopRecording" type="danger">结束</el-button>
            <el-button type="danger"><a :href="downloadUrl" download="recording.wav">下载录音</a></el-button>
            <div class="pei-ban">
              <p>伴奏:</p>
            </div>
            <el-slider v-model="audioVolume" :max="100" @input="setVolume"></el-slider>
            <div class="pei-ban">
              <p>人声:</p>
            </div>
            <el-slider v-model="audioVolume" :max="100" @input="setVolume"></el-slider>
          </div>
          <div>
            <el-slider v-model="audiocurrentTime" :max="audioduration" class="custom-slider"></el-slider>
            <audio ref="audio" :src="currentTrack.src"></audio>
            <div class="time-display">
              <div>{{ shownowTime }} / {{ showduration }} </div>
            </div>
          </div>
        </el-card>
      </div>
    </div>
  </div>
</template>

<script>
//专门的一个script来展示图表
import * as echarts from 'echarts'
export default {
  data() {
    return {
      totalData: [0,
        174,
        44,
        44,
        44,
        146,
        390,
        290,
        130,
        66,
        32,
        32,
        32,
        34,
        44,
        44,
        312,
        44,
        312,
        390,
        146,
        32,
        116,
        124,
        122,
        36,
        124,
        44,
        174,
        132,
        44,
        116,
        32,
        36,
        66,
        66,
        32,
        32,
        96,
        58,
        94,
        344,
        44,
        44,
        504,
        32,
        116,
        32,
        32,
        34,
        38,
        34,
        146,
        68,
        162,
        308,
        16,
        298,
        48,
        294,
        312,
        60,
        316,
        58,
        22,
        48,
        18,
        20,
        50,
        308,
        28,
        48,
        66,
        18,
        48,
        14,
        40,
        48,
        36,
        44,
        20,
        46,
        142,
        20,
        30,
        304,
        72,
        26,
        38,
        62,
        32,
        38,
        84,
        40,
        32,
        294,
        62,
        600,
        484,
        34,
        22,
        368,
        48,
        190,
        40,
        20,
        50,
        600,
        20,
        14,
        48,
        14,
        18,
        62,
        42,
        24,
        360,
        58,
        20,
        52,
        16,
        600,
        48,
        18,
        20,
        14,
        48,
        68,
        66,
        66,
        16,
        600,
        54,
        44,
        396,
        26,
        30,
        66,
        28,
        358,
        58,
        346,
        360,
        30,
        392,
        40,
        20,
        18,
        16,
        58,
        30,
        26,
        130,
        40,
        18,
        14,
        126,
        266,
        120,
        126,
        132,
        248,
        244,
        90,
        124,
        136,
        134,
        128,
        126,
        88,
        434,
        228,
        124,
        134,
        152,
        110,
        112,
        134,
        136,
        148,
        140,
        128,
        182,
        44,
        184,
        232,
        258,
        314,
        66,
        250,
        32,
        32,
        146,
        270,
        268,
        116,
        44,
        104,
        236,
        254,
        160,
        370,
        78,
        388,
        230,
        182,
        62,
        340,
        20,
        54,
        308,
        40,
        48,
        18,
        26,
        24,
        62,
        42,
        28,
        42,
        44,
        110,
        32,
        136,
        38,
        18,
        342,
        40,
        348,
        348,
        382,
        344,
        38,
        168,
        62,
        26,
        146,
        64,
        134,
        30,
        58,
        66,
        58,
        44,
        148,
        54,
        80,
        40,
        16,
        122,
        34,
        44,
        22,
        44,
        328,
        334,
        366,
        160,
        96,
        92,
        216,
        106,
        108,
        108,
        132,
        64,
        64,
        148,
        32,
        78,
        70,
        208,
        102,
        108,
        98,
        104,
        100,
        314,
        98,
        102,
        210,
        206,
        100,
        520,
        120,
        116,
        246,
        240,
        116,
        102,
        88,
        66,
        126,
        100,
        168,
        340,
        298,
        230,
        78,
        222,
        294,
        32,
        600,
        506,
        324,
        168,
        366,
        204,
        216,
        600,
        326,
        40,
        600,
        46,
        298,
        294,
        306,
        60,
        320,
        38,
        22,
        48,
        18,
        20,
        50,
        308,
        52,
        24,
        310,
        46,
        48,
        14,
        40,
        48,
        62,
        44,
        20,
        46,
        142,
        40,
        302,
        304,
        58,
        26,
        38,
        62,
        32,
        46,
        84,
        40,
        32,
        288,
        50,
        44,
        58,
        38,
        22,
        34,
        48,
        190,
        40,
        20,
        56,
        600,
        20,
        14,
        48,
        14,
        18,
        62,
        42,
        30,
        354,
        58,
        20,
        54,
        16,
        600,
        48,
        18,
        20,
        14,
        48,
        52,
        68,
        66,
        16,
        600,
        66,
        44,
        396,
        26,
        30,
        66,
        28,
        358,
        58,
        346,
        360,
        30,
        392,
        38,
        20,
        34,
        16,
        58,
        30,
        26,
        198,
        66,
        176,
        378,
        136,
        270,
        50,
        28,
        134,
        24,
        34,
        26,
        24,
        38,
        288,
        600,
        38,
        20,
        244,
        234,
        238,
        32,
        22,
        274,
        46,
        288,
        348,
        20,
        36,
        86,
        104,
        38,
        20,
        26,
        88,
        12,
        34,
        34,
        56,
        24,
        24,
        304,
        50,
        32,
        120,
        40,
        128,
        38,
        166,
        160,
        34,
        352,
        168,
        346,
        40,
        600,
        344,
        62,
        300,
        42,
        48,
        18,
        26,
        26,
        62,
        44,
        362,
        66,
        44,
        20,
        32,
        314,
        38,
        18,
        32,
        72,
        48,
        32,
        16,
        48,
        46,
        288,
        20,
        26,
        20,
        46,
        392,
        224,
        56,
        66,
        46,
        216,
        136,
        66,
        40,
        30,
        16,
        48,
        22,
        260,
        318,
        62,
        130,
        54,
        48,
        18,
        138,
        142,
        130,
        408,
        118,
        126,
        106,
        74,
        124,
        126,
        32,
        194,
        318,
        88,
        100,
        564,
        124,
        112,
        128,
        276,
        136,
        300,
        142,
        356,
        34,
        230,
        236,
        234,
        258,
        66,
        76,
        188,
        94,
        118,
        126,
        122,
        458,
        426,
        126,
        126,
        92,
        122,
        124,
        226,
        42,
        26,
        36,
        316,
        22,
        40,
        38,
        36,
        94,
        40,
        108,
        108,
        38,
        90,
        92,
        30,
        90,
        86,
        24,
        44,
        36,
        50,
        46,
        26,
        118,
        20,
        92,
        36,
        86,
        110,
        38,
        26,
        22,
        20,
        48,
        36,
        100,
        28,
        38,
        76,
        42,
        32,
        42,
        20,
        34,
        52,
        210,
        210,
        28,
        34,
        38,
        26,
        600,
        56,
        32,
        60,
        42,
        38,
        38,
        44,
        62,
        34,
        24,
        36,
        18,
        28,
        396,
        62,
        44,
        370,
        360,
        38,
        20,
        54,
        32,
        600,
        34,
        18,
        28,
        14,
        48,
        68,
        66,
        46,
        16,
        600,
        54,
        26,
        396,
        24,
        26,
        44,
        28,
        34,
        34,
        352,
        56,
        30,
        86,
        66,
        34,
        32,
        16,
        28,
        34,
        26,
        166,
        66,
        48,
        52,
        16,
        26,
        12,
        12,
        12,
        296,
        600,
        600,
        600,
        156,
        50,
        16,
        52,
        16,
        12,
        12,
        12,
        302,
        298,
        306,
        318,
        316,
        394,
        392,
        384,
        52,
        24,
        12,
        12,
        130,
        298,
        298,
        600,
        600,
        156,
        174,
        52,
        44,
        38,
        22,
        24,
        12,
        12,
        296,
        304,
        600,
        310,
        394,
        378,
        600,
        600,
        600,
        378,
        392,
        600,
        378,
        392,
        600,
        378,
        392,
        600,
        378],
      index: 54, // 新增指标index
      windowData: [174,
        44,
        44,
        44,
        146,
        390,
        290,
        130,
        66,
        32,
        32,
        32,
        34,
        44,
        44,
        312,
        44,
        312,
        390,
        146,
        32,
        116,
        124,
        122,
        36,
        124,
        44,
        174,
        132,
        44,
        116,
        32,
        36,
        66,
        66,
        32,
        32,
        96,
        58,
        94,
        344,
        44,
        44,
        504,
        32,
        116,
        32,
        32,
        34,
        38,
        34],
      chartInstance: null,
      timer: null,
    };
  },
  mounted() {
    const dom = document.getElementById('echarts-container')
    if (!dom) return
    this.chartInstance = echarts.init(dom);
    this.chartInstance.setOption({
      animation: false,
      title: {
        text: 'Hz'
      },
      xAxis: {

        data: []
      },
      yAxis: {
        splitLine: { // 去除网格线
          show: false
        },
        max: 601,
        min: 0,
      },
      series: [{
        type: 'bar',
        barGap: 0,
        barCategoryGap: 0,
        //data: this.windowData
        data: [100, 200, 300, 400, 500]
      }]
    });
    this.timer = setInterval(() => {
      this.collectData();
    }, 250);
  },
  methods: {
    collectData() {
      this.windowData.shift();
      this.windowData.push(this.totalData[this.index]);
      this.index = this.index + 1;
      // 更新图表数据
      this.chartInstance.setOption({
        title: {
          text: 'Hz'
        },
        xAxis: {

          data: []
        },
        yAxis: {
          splitLine: { // 去除网格线
            show: false
          },
          max: 601,
          min: 0,
        },
        series: [{
          type: 'bar',
          barGap: 0,
          barCategoryGap: 0,
          data: this.windowData
        }]
      });
    }
  }

}
</script>
<script setup>
import WaveSurfer from 'wavesurfer.js'
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { computed, onMounted } from 'vue';
import { defineProps } from 'vue';

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
  //游标部分
  const timerId = setInterval(syncLevelWithVolumeLevel, 100);
  wavesurfer = WaveSurfer.create({
    container: '#waveform',
    waveColor: 'rgb(200, 0, 200)',
    progressColor: 'rgb(100, 0, 100)',
    url: 'https://m801.music.126.net/20240711152519/f3a1bcd93a6e02488eaa497f4ec5eadd/jdymusic/obj/wo3DlMOGwrbDjj7DisKw/33312789506/de02/6cc9/7c79/2906296b7a9670281506ab334db0deaf.mp3',
  })
});
//返回按钮部分
const router = useRouter();
function goBack() {
  router.push({ path: '/' });
}

//播放音量控制部分
const showduration = ref(null);//显示字符串
const shownowTime = ref(null);//显示字符串
const audio = ref(null);
const currentTrack = ref({ src: "https://m801.music.126.net/20240712164610/3da1952f22b601006ebc2982f9fe0898/jdymusic/obj/wo3DlMOGwrbDjj7DisKw/33312789506/de02/6cc9/7c79/2906296b7a9670281506ab334db0deaf.mp3" });
//const volume = ref(70);
const isPlaying = ref(false);//辅助变量判断有没有播放
const audioVolume = ref(1); // 假设初始音量为 1（100%）
const audiocurrentTime = ref(0);
const audioduration = ref(0);
// 定义一个引用来保存定时器的ID
const intervalId = ref(null);
const transfer = (second) => {
  // 使用 Math.floor 确保分钟和秒数都是整数
  const minutes = Math.floor(second / 60);
  const seconds = Math.floor(second % 60);
  // 使用 padStart 确保秒数是两位数格式
  const formattedSeconds = seconds.toString().padStart(2, '0');
  // 返回格式化的时间字符串
  return `${minutes}:${formattedSeconds}`;
};
const update = () => {
  // 这里添加你希望每秒执行的代码
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
const loadTrack = (track) => {
  if (!audio.value) return;
  audio.value.src = track.src;
  audio.value.load(); // 加载音频

};
const rePlay = () => {
  audio.value.load();
  audio.value.play();
}
const togglePlay = () => {
  if (audio.value === null) {
    audio.value = new Audio();
    loadTrack(currentTrack.value);
  }
  parseLRC(mylrcText.value);
  audioduration.value = audio.value.duration;
  showduration.value = transfer(audioduration.value);
  audio.value.play();
  isPlaying.value = !isPlaying.value;
  intervalId.value = setInterval(update, 1000);
  startRecording();
  wavesurfer.play()
}

//歌词滚动部分
const lyrics = ref([]); // 存储解析后的歌词数据
const currentLines = ref([]); // 当前显示的歌词行
const mylrcText = ref(`[00:00.000] 作词Lyrics : 盛宇D-SHINE
[00:00.231] 作曲Composition : 盛宇D-SHINE
[00:00.462] 编曲Arrangement : K ELEVEN
[00:00.693] 制作人Producer : K ELEVEN/盛宇D-SHINE/Ranzer叶润泽
[00:00.924] 监制Executive producer : Ranzer叶润泽
[00:01.155] 录音Sound recordist : 弗兰德斯坦
[00:01.386] 混音Mixing : Jio @顶穿音频工作室
[00:01.617] 和声Backing Vocals : 東别
[00:01.852]演唱Vocal：盛宇D-SHINE
[00:02.173]作词Lyrics： 盛宇D-SHINE
[00:02.416]作曲Composition：盛宇D-SHINE
[00:02.605]编曲Arrangement：K ELEVEN
[00:02.804]制作人Producer：K ELEVEN/盛宇D-SHINE/Ranzer叶润泽
[00:03.023]监制Executive producer：Ranzer叶润泽
[00:03.222]录音Sound recordist：弗兰德斯坦
[00:03.471]混音Mixing：Jio @顶穿音频工作室
[00:03.723]和声Backing Vocals：東别
[00:10.501]S U P D SHINE
[00:12.439]HOOK
[00:13.337]别跟我有的没的老子想发就发 这首歌用来开头炮
[00:17.037]新的篇章起伏跌宕 欢迎当面来搞别戴头套
[00:20.263]体育馆音乐节club商演 我是看谁在带头燥
[00:23.413]Yeah 是谁在开头
[00:26.134]pow pow pow谁在带头
[00:29.582]燥 燥 燥  于斯为盛
[00:32.884]pow pow pow 我在带头
[00:35.997]燥 燥 燥
[00:38.008]D to the shine
[00:38.907]Verse 1
[00:39.300]成语说唱因为听不惯他们没有文采唧唧复唧唧
[00:42.564]搞得这番田地 好货不便宜 别bb又bb
[00:45.801]复制人 下的药 学老外瞎**叫
[00:48.626]老子字正腔圆 正规说唱 我腔***调
[00:51.792]哈跟老子矮哒 把他们脸踩哒
[00:54.862]再赢它八九十下 直到你们高喊oh my god
[00:58.337]我对 loser的戾气远比这个时代都还要重
[01:01.309]野家拳 截拳道 能杀死忍者的才叫痛
[01:04.553]那些押韵不够新颖 得从清醒中惊醒
[01:07.842]有眼不识泰山Dragonfly 被误认为蜻蜓
[01:11.046]Diss我的四舍五入 约等于在为我卖命
[01:14.147]你的粉丝都在 跟我唱诀爱·尽
[01:17.024]HOOK：
[01:17.156]别跟我有的没的老子想发就发 这首歌用来开头炮
[01:20.916]新的篇章起伏跌宕 欢迎当面来找别戴头套
[01:24.196]体育馆音乐节club商演 我是看谁在带头燥
[01:27.402]Woo 是谁在开头
[01:30.002]Pow pow pow 谁在带头
[01:33.397]燥 燥 燥     于斯为盛
[01:36.489]pow pow pow 我在带头
[01:39.841]燥 燥 燥
[01:41.990]D to the SHINE
[01:43.370]Verse 2
[01:43.455]吹灭硝烟 烟头踩灭 放进口袋 管他们垃圾话乱扔
[01:46.284]放声高歌 惹过我的 既往不咎 大发慈悲全都放生
[01:49.307]一路经历了多少的挫折
[01:50.970]你以为老子靠节目火的
[01:52.596]大胆预言 明年top3金曲它还是我的
[01:55.808]掀起波澜 还是这么壮阔 这一把升级到壮观
[01:59.055]持续创作 依然那么忘我 翘起地球给我一个杠杆
[02:02.169]他们澎湃 得像是虫害 与我同在 是那条龙脉
[02:05.418]不管你在哪个门派 把文化输出到名扬中外
[02:08.588]你想吃蛋糕 你想吃甜筒 你的文字是米田共
[02:11.718]哥是什么人 别只看舆论 哈圈紫薇星帮你圆梦
[02:14.905]那天跟腱断了 隔天依然录音棚照常
[02:18.161]我哈圈男主角 懒得鸟巅峰龙套王
[02:21.410]Bridge：
[02:21.665]推墙的一众人等 在乌云下集合 怎能办到
[02:24.446]带不带新人 你听闻 清晨 我跟兄弟在分蛋糕
[02:27.718]看看市面上那些*硬得像石头真绊脚
[02:31.040]我不畏浮云遮望眼一览众山小
[02:33.872]HOOK
[02:34.186]Pow pow pow 谁在带头
[02:37.489]燥 燥 燥 于斯为盛
[02:40.514]pow pow pow 我在带头
[02:43.779]燥 燥 燥
[02:45.930]D to the SHINE
[02:55.930]版权声明：未经著作权人书面许可，任何人不得以任何方式使用（包括翻唱、翻录等）
[02:57.309] 版权声明：未经著作权人书面许可，任何人不得以任何方式使用（包括翻唱、翻录等）`);
const parseLRC = (lrcText) => {
  const lines = lrcText.split('\n').filter(line => line.trim() !== '');
  lyrics.value = lines.map(line => {
    const [time, text] = line.split(']');
    const [mm, ss, ms] = time.substring(1).split(':').map(part => parseInt(part, 10));
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
const chunks = ref([]);
const recordingBlob = ref(null);
const downloadUrl = ref(null);
const startRecording = () => {
  navigator.mediaDevices
    .getUserMedia({ audio: true })
    .then(stream => {
      mediaRecorder.value = new MediaRecorder(stream);
      mediaRecorder.value.ondataavailable = event => {
        chunks.value.push(event.data);
      };
      mediaRecorder.value.start();
      isRecording.value = true;

      //开始杂交
      const audioContextInstance = new (window.AudioContext || window.webkitAudioContext)();
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
        setInterval(() => {
          analyser.value.getByteFrequencyData(dataArray);
          const volume = dataArray[0]; // 取第一个数据点作为音量估计
          volumeLevel.value = volume ? Math.round(10 * Math.log10(volume)) : 0; // 转换为分贝值
        }, 100); // 每秒更新10次
      });


    })
    .catch(err => {
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
  background:gray;
  ;
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