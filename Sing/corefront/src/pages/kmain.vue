<template>
  <div>
    <el-button @click="goBack">返回</el-button>
  </div>

  <div>
    <h1>{{ audioduration }}</h1>
    <h1>{{ audiocurrentTime }}</h1>
  </div>

  <div class="audio-player">
    <el-card class="player-container">
      <div class="player-controls">
        <el-button @click="togglePlay">点击播放</el-button>
        <el-button @click="rePlay">重唱</el-button>
        <!-- 音量控制 -->
        <el-slider v-model="audioVolume" :max="100" @input="setVolume"></el-slider>
      </div>
      <el-slider v-model="audiocurrentTime" :max="audioduration"></el-slider>
      <audio ref="audio" :src="currentTrack.src"></audio>
    </el-card>
  </div>

  <div class="lyric-container">
    <div class="lyric-line" v-for="(line, index) in currentLines" :key="index">
      {{ line.text }}
    </div>
  </div>

  <div>
    <button @click="toggleRecording" :class="{ recording: isRecording }">
      {{ isRecording ? '停止录音' : '开始录音' }}
    </button>
    <a v-if="recordingBlob" :href="downloadUrl" download="recording.wav">下载录音</a>
  </div>

</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { computed, onMounted } from 'vue';


const audio = ref(null);
const currentTrack = ref({ src: "https://m801.music.126.net/20240710105234/2a78b42a7fba11b5a0917aefe2c3eabe/jdymusic/obj/wo3DlMOGwrbDjj7DisKw/33312789506/de02/6cc9/7c79/2906296b7a9670281506ab334db0deaf.mp3", title: 'b' });
//const volume = ref(70);
const isPlaying = ref(false);//辅助变量判断有没有播放
const audioVolume = ref(1); // 假设初始音量为 1（100%）
const audiocurrentTime=ref(0);
const audioduration=ref(0);
// 定义一个引用来保存定时器的ID
const intervalId = ref(null);
const update= () => {
  // 这里添加你希望每秒执行的代码
  audiocurrentTime.value=audio.value.currentTime;
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

const rePlay=() =>{
  audio.value.load();
  audio.value.play();
}

const togglePlay = () => {
if(audio.value===null)
{
  audio.value =  new Audio();


  loadTrack(currentTrack.value);

}
parseLRC(mylrcText.value);
audioduration.value=audio.value.duration;
audio.value.play();
isPlaying.value = !isPlaying.value;
intervalId.value = setInterval(update, 1000);
};


//返回路由跳转
const router = useRouter();

function goBack() {
  router.push({ path: '/' });
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
    const timeInSeconds= mm*60 + ss;
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

const toggleRecording = () => {
  if (!isRecording.value) {
    startRecording();
  } else {
    stopRecording();
  }
};

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
h1 {
  color: #42b983;
}
.lyric-container {
  max-height: 200px; /* 根据需要调整 */
  overflow-y: auto;
}
.lyric-line {
  padding: 8px;
  text-align: center;
  border-bottom: 1px solid #ddd; /* 为每行歌词添加一点边框 */
  box-sizing: border-box;
  height: 50px; /* 可以调整每行的高度 */
  overflow: hidden; /* 如果文本超出高度则隐藏 */
  white-space: nowrap; /* 防止歌词换行 */
  text-overflow: ellipsis; /* 如果文本超出则显示省略号 */
}

</style>