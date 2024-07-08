using NAudio.Wave;

namespace SingScore
{ 
    public class Audio
    {
    public int Id { get; set; } // 主键
    public int Num { get; set; }//序号
    public string Title { get; set; } // 音频标题
    public string FilePath { get; set; } // 音频文件路径
    public DateTime CreatedAt { get; set; } // 创建时间
    public WaveOutEvent waveOut;
    public AudioFileReader audioFileReader;     //读取音频文件

        //文件的总时间
        public TimeSpan TotalTime
        {
            get
            {
                audioFileReader = new AudioFileReader(FilePath);
                if (audioFileReader != null)
                    return audioFileReader.TotalTime;
                else
                    return TimeSpan.Zero;
            }
        }
        //当前的时间位置
        public TimeSpan CurrentPosition
        {
            get
            {
                if (audioFileReader != null)
                {
                    return audioFileReader.CurrentTime;
                }
                else
                    return TimeSpan.Zero;
            }
        }
        //构造方法1
        public Audio()
        {
            Num = 10000;
            CreatedAt = DateTime.Now;
        }
        //构造方法2
        public Audio(int num, string title, string filePath)
        {
            Num = num;
            Title = title;
            FilePath = filePath;
            CreatedAt = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Audio other = (Audio)obj;
            return this.Id == other.Id &&
                   this.Title == other.Title &&
                   this.FilePath == other.FilePath;
        }
        public void PlayAudio()
        {
            // 创建 AudioFileReader 和 WaveOutEvent 对象
            audioFileReader = new AudioFileReader(FilePath);
            waveOut = new WaveOutEvent();
            waveOut.Init(audioFileReader);
            waveOut.Play();

        }

        public void PauseAudio()
        {
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Pause();
            }
        }
        public void ReplayAudio()
        {
            if (waveOut != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                }
                audioFileReader.Position = 0; // 将音频播放位置重置到开头
                waveOut.Play();
            }
            else
            {
                PlayAudio(); // 如果没有正在播放的音频，则直接开始播放
            }
        }


        public void SetVolume(float volume)//设置音量
        {
            if (waveOut != null)
            {
                waveOut.Volume = volume;
            }
        }
        public float GetVolume()//设置音量
        {
            if (waveOut != null)
            {
                return waveOut.Volume;
            }
            else return 1;
        }
        public TimeSpan GetTotalTime()
        {
            if (audioFileReader != null)
            {
                return audioFileReader.TotalTime;
            }
            return TimeSpan.Zero;
        }
        public void SetPosition(long milliseconds)
        {
            if (audioFileReader != null)
            {
                var newTime = TimeSpan.FromSeconds(milliseconds / 1000.0);
                if (waveOut.PlaybackState == PlaybackState.Playing || waveOut.PlaybackState == PlaybackState.Paused)
                {
                    waveOut.Stop();
                    audioFileReader.CurrentTime = newTime;
                    waveOut.Init(audioFileReader);
                    waveOut.Play();
                }
                else
                {
                    audioFileReader.CurrentTime = newTime;
                }
            }
        }

        public void Dispose()//释放资源
        {
            if (waveOut != null)
            {
                waveOut.Dispose();
            }
            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
            }
        }



    }
}
