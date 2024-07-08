using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using NAudio.Utils;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;

namespace SingScore
{
    //定义了几种不同的混响风格
    public enum ReverbStyle
    {
        Acoustic,
        Pop,
        Rock,
        Classical,
        ThreeD
    }
    public struct ReverbParameters
    {
        public float Decay;
        public int DelayMilliseconds;

        public ReverbParameters(float decay, int delayMilliseconds)
        {
            Decay = decay;
            DelayMilliseconds = delayMilliseconds;
        }
    }
    //这段代码定义了一个 C# 结构体（struct）名为 ReverbParameters，用于存储混响效果的参数。结构体是一种值类型，通常用于存储相关的数据项，这些数据项作为一个单一的实体被使用。


    public class HandleMusic
    {
        VolumeSampleProvider vocalsVolumeProvider;
        VolumeSampleProvider backgroundVolumeProvider;
        AudioFileReader vocalReader;
        AudioFileReader backgroundReader;
        private MixingSampleProvider mixer;
        public IWavePlayer waveOutDevice;
        private WaveInEvent waveIn;
        private MemoryStream memoryStream;
        private WaveFileWriter recordedAudioWriter;
        private string recordedAudioFilePath;
        private bool isRecordingPaused = false;//录音暂停
        private bool stopRecordingCompletely = false;//录音完全停止
        public bool isRecording = false;
        private List<LyricLine> lyricsList;
        private ReverbEffectProvider vocalsReverb;
        private ReverbEffectProvider backgroundReverb;
        private Dictionary<ReverbStyle, ReverbParameters> reverbStyles;
        private SmbPitchShiftingSampleProvider backgroundPitchShiftingProvider;
        private SmbPitchShiftingSampleProvider vocalsPitchShiftingProvider;

        public HandleMusic()
        {
            reverbStyles = new Dictionary<ReverbStyle, ReverbParameters>
    {
        { ReverbStyle.Acoustic, new ReverbParameters(0.3f, 20) },
        { ReverbStyle.Pop, new ReverbParameters(0.2f, 60) },
        { ReverbStyle.Rock, new ReverbParameters(0.5f, 120) },
        { ReverbStyle.Classical, new ReverbParameters(0.25f, 140) },
        { ReverbStyle.ThreeD, new ReverbParameters(0.3f, 150) },
    };
        }

        public bool IsPlaying
        {
            get
            {
                return waveOutDevice.PlaybackState == PlaybackState.Playing;
            }
        }
        //是否在播放
        public bool IsPaused
        {
            get
            {
                return waveOutDevice.PlaybackState == PlaybackState.Paused;
            }
        }
        //是否停顿
        public bool IsStopped
        {
            get
            {
                return waveOutDevice.PlaybackState == PlaybackState.Stopped;
            }
        }

        //TotalTime属性代表该文件的总播放时间
        public TimeSpan TotalTime
        {
            get
            {
                if (vocalReader != null)
                    return vocalReader.TotalTime;
                else
                    return TimeSpan.Zero;
            }
        }

        //现在播放的时间
        public TimeSpan CurrentPosition
        {
            get
            {
                if (vocalReader != null)
                {
                    return vocalReader.CurrentTime;
                }
                else
                    return TimeSpan.Zero;
            }
        }

        //设置播放时间
        public void SetPosition(long milliseconds)
        {

            if (vocalReader != null || backgroundReader != null)
            {
                if (IsPlaying || IsPaused)
                {
                    Stop();
                    vocalReader.CurrentTime = TimeSpan.FromSeconds(milliseconds / 1000);
                    backgroundReader.CurrentTime = TimeSpan.FromSeconds(milliseconds / 1000);
                    //audioFileReader.Position = seconds * (long)waveOut.OutputWaveFormat.AverageBytesPerSecond / 1000;
                    Play();
                }
                else
                {
                    vocalReader.CurrentTime = TimeSpan.FromSeconds(milliseconds / 1000);
                    backgroundReader.CurrentTime = TimeSpan.FromSeconds(milliseconds / 1000);
                    //audioFileReader.Position = seconds * (long)waveOut.OutputWaveFormat.AverageBytesPerSecond / 1000;
                }
            }
        }

        //触发音频分离的过程，调用Run
        public async Task TransformAsync(string filepath)
        {
            int exitCode = await RunExternalProcess(filepath);


        }

        //得到分离后人声的路径
        public string GetVocalPath(string filepath)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filepath);
            var vocalsPath = Path.Combine(fileNameWithoutExtension, "vocals.wav");
            return vocalsPath;
        }

        //得到分离后伴奏的路径
        public string GetBackPath(string filepath)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filepath);
            var backgroundPath = Path.Combine(fileNameWithoutExtension, "accompaniment.wav");
            return backgroundPath;
        }

        //运行脚本
        private Task<int> RunExternalProcess(string audioFilePath)
        {
            return Task.Run(() =>
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "python",
                    //使用脚本，包括脚本路径
                    Arguments = $"\"{AppDomain.CurrentDomain.BaseDirectory}Scripts\\split.py\" \"{audioFilePath}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var process = new Process { StartInfo = psi })
                {
                    process.Start();
                    // Asynchronously read the standard output and standard error
                    var output = process.StandardOutput.ReadToEndAsync();
                    var errors = process.StandardError.ReadToEndAsync();
                    process.WaitForExit();
                    // Await the asynchronous read tasks
                    Task.WaitAll(new Task[] { output, errors });

                    // Optionally log the output and errors or handle them as needed
                    Debug.WriteLine($"Output: {output.Result}");
                    Debug.WriteLine($"Errors: {errors.Result}");

                    return process.ExitCode;
                }
            });
        }


        //这个方法用于初始化音频播放器，包括加载分离后的音轨，并设置初始音量等。
        public async Task InitializeAndPlayAsync(string FilePath, float initialVocalsVolume, float initialBackgroundVolume)
        {
            Dispose();
            // 使用AudioFileReader读取人声和背景音
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(FilePath);

            // 构建人声和背景音的完整路径
            var vocalsFilePath = Path.Combine(fileNameWithoutExtension, "vocals.wav");
            var backgroundFilePath = Path.Combine(fileNameWithoutExtension, "accompaniment.wav");
            vocalReader = new AudioFileReader(vocalsFilePath);
            backgroundReader = new AudioFileReader(backgroundFilePath);

            // 创建音量控制提供者
            vocalsVolumeProvider = new VolumeSampleProvider(vocalReader) { Volume = initialVocalsVolume / 100f };
            backgroundVolumeProvider = new VolumeSampleProvider(backgroundReader) { Volume = initialBackgroundVolume / 100f };


            // 创建音调处理提供者
            backgroundPitchShiftingProvider = new SmbPitchShiftingSampleProvider(backgroundVolumeProvider);
            backgroundPitchShiftingProvider.PitchFactor = 1.0f; // 初始化 PitchFactor
            vocalsPitchShiftingProvider = new SmbPitchShiftingSampleProvider(vocalsVolumeProvider);
            vocalsPitchShiftingProvider.PitchFactor = 1.0f; // 初始化 PitchFactor

            ReverbParameters defaultReverbParams = new ReverbParameters(0.3f, 120); // 这是原声风格的假设参数
            vocalsReverb = new ReverbEffectProvider(vocalsPitchShiftingProvider, defaultReverbParams.Decay, TimeSpan.FromMilliseconds(defaultReverbParams.DelayMilliseconds));
            backgroundReverb = new ReverbEffectProvider(backgroundPitchShiftingProvider, defaultReverbParams.Decay, TimeSpan.FromMilliseconds(defaultReverbParams.DelayMilliseconds));

            mixer = new MixingSampleProvider(new[] { vocalsReverb, backgroundReverb });
            // 初始化
            //await LoadLyricsFromLRCFileAsync(FilePath);
            waveOutDevice = new WaveOutEvent();
            waveOutDevice.Init(mixer);
        }

        public void ApplyReverbStyle(ReverbStyle style)
        {
            if (reverbStyles.TryGetValue(style, out ReverbParameters rp))
            {
                vocalsReverb?.UpdateReverb(rp.Decay, TimeSpan.FromMilliseconds(rp.DelayMilliseconds));
                backgroundReverb?.UpdateReverb(rp.Decay, TimeSpan.FromMilliseconds(rp.DelayMilliseconds));
            }
        }



        //音乐播放
        public void Play()
        {
            //播放
            waveOutDevice.Play();
        }
        //暂停
        public void Pause()
        {
            if (waveOutDevice != null && waveOutDevice.PlaybackState == PlaybackState.Playing)
            {
                waveOutDevice.Pause();
            }
        }
        //继续播放
        public void Resume()
        {
            if (waveOutDevice != null && waveOutDevice.PlaybackState == PlaybackState.Paused)
            {
                waveOutDevice.Play();
            }
        }
        //结束
        public void Stop()
        {
            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
            }
        }

        //重放
        public void Replay()
        {
            if (waveOutDevice != null)
            {
                if (waveOutDevice.PlaybackState == PlaybackState.Playing)
                {
                    waveOutDevice.Stop();
                }
                vocalReader.Position = 0; // 将音频播放位置重置到开头
                backgroundReader.Position = 0;
                waveOutDevice.Play();
                recordedAudioWriter?.Dispose();
                recordedAudioWriter = null;

                waveIn?.Dispose();
                waveIn = null;
            }
            else
            {
                waveOutDevice.Play(); // 如果没有正在播放的音频，则直接开始播放
            }
        }
        //释放
        public void Dispose()
        {
            // 清除当前混音器和播放设备，如果有的话
            waveOutDevice?.Dispose();
            vocalReader?.Dispose();
            backgroundReader?.Dispose();
        }

        // 调整人声音量 
        public void AdjustVocalsVolume(float volume)
        {
            if (vocalsVolumeProvider != null)
            {
                vocalsVolumeProvider.Volume = volume / 100f;
            }
        }
        //调节人声音调
        public void SetVocalsPitchFactor(float pitchFactor)
        {
            if (vocalsPitchShiftingProvider != null)
            {
                // 锁定对象以确保线程安全
                lock (vocalsPitchShiftingProvider)
                {
                    vocalsPitchShiftingProvider.PitchFactor = pitchFactor;
                }
            }
        }
        // 调整背景音量
        public void AdjustBackgroundVolume(float volume)
        {
            if (backgroundVolumeProvider != null)
            {
                backgroundVolumeProvider.Volume = volume / 100f;
            }
        }
        //调节背景音调
        public void SetBackgroundPitchFactor(float pitchFactor)
        {
            if (backgroundPitchShiftingProvider != null)
            {
                // 锁定对象以确保线程安全
                lock (backgroundPitchShiftingProvider)
                {
                    backgroundPitchShiftingProvider.PitchFactor = pitchFactor;
                }
            }
        }
        //开始录音
        public void StartRecording(string filepath)
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
            }

            if (recordedAudioWriter != null)
            {
                recordedAudioWriter.Dispose();
                recordedAudioWriter = null;
            }

            if (memoryStream != null)
            {
                memoryStream.Dispose();
                memoryStream = null;
            }

            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(44100, 1)
            };
            waveIn.DataAvailable += WaveIn_DataAvailable;
            //waveIn.RecordingStopped += WaveIn_RecordingStopped;

            // Initialize memory stream and writer
            memoryStream = new MemoryStream();
            recordedAudioWriter = new WaveFileWriter(new IgnoreDisposeStream(memoryStream), waveIn.WaveFormat);
            waveIn.StartRecording();
            isRecording = true;
        }
        public void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (recordedAudioWriter != null)
            {
                recordedAudioWriter.Write(e.Buffer, 0, e.BytesRecorded);
                recordedAudioWriter.Flush();
            }
        }
        public void StopRecording()
        {
            stopRecordingCompletely = true; // 表示我们完全停止录音
            waveIn?.StopRecording();
            isRecordingPaused = true;
        }
        public void ResumeRecording()
        {
            if (isRecordingPaused && waveIn != null)
            {
                waveIn.StartRecording();
                isRecordingPaused = false;
            }
        }
        public void PauseRecording()
        {
            stopRecordingCompletely = false; // 表示我们只是想要暂停录音
            waveIn.StopRecording();
            isRecordingPaused = true;
        }
        // 停止录音并释放资源的方法
        public void StopAndDisposeRecording()
        {
            try
            {
                // 停止录音
                if (waveIn != null)
                {
                    waveIn.StopRecording();
                    waveIn.Dispose();
                    waveIn = null;
                }

                // 停止文件写入器，并释放资源
                if (recordedAudioWriter != null)
                {
                    // 在释放之前确保所有数据已被正确写入文件
                    recordedAudioWriter.Flush();
                    recordedAudioWriter.Dispose();
                    recordedAudioWriter = null;
                }

                // 停止播放设备，并释放资源
                if (waveOutDevice != null)
                {
                    waveOutDevice.Stop();
                    waveOutDevice.Dispose();
                    waveOutDevice = null;
                }
            }
            catch (Exception ex)
            {
                // 可选：记录异常信息
                Console.WriteLine($"Error stopping and disposing recording: {ex.Message}");
            }
            finally
            {
                // 确保记录中断状态
                isRecording = false;
            }
        }
        //public void WaveIn_RecordingStopped(object sender, StoppedEventArgs e)
        //{

        //    if (stopRecordingCompletely)
        //    {
        //        recordedAudioWriter?.Dispose();
        //        recordedAudioWriter = null;

        //        waveIn?.Dispose();
        //        waveIn = null;
        //        isRecording = false;

        //        // Ask the user for the file name
        //        SaveFileDialog saveFileDialog = new SaveFileDialog
        //        {
        //            Filter = "WAV Files|*.wav",
        //            Title = "Save recorded audio"
        //        };

        //        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            var outputPath = saveFileDialog.FileName;

        //            // Save the memory stream to the specified file
        //            using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //            {
        //                memoryStream.Seek(0, SeekOrigin.Begin);
        //                memoryStream.CopyTo(fileStream);
        //            }

        //            memoryStream.Dispose();
        //            recordedAudioFilePath = outputPath;

        //            MessageBox.Show($"录音已保存至: {recordedAudioFilePath}", "录音保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        else
        //        {
        //            // Clean up the memory stream if the save dialog is cancelled
        //            memoryStream.Dispose();
        //        }
        //    }
        //}
        public (string currentLyric, string nextLyric) GetCurrentAndNextLyrics(TimeSpan currentPosition)
        {
            if (lyricsList == null || !lyricsList.Any())
            {
                return ("", "");
            }

            // 找到当前时间点最接近的歌词行的索引
            var currentIndex = lyricsList.FindLastIndex(lyric => currentPosition >= lyric.Time);

            // 提取当前歌词和下一行歌词
            string currentLyric = currentIndex != -1 ? lyricsList[currentIndex].Text : "";
            string nextLyric = (currentIndex + 1) < lyricsList.Count ? lyricsList[currentIndex + 1].Text : "";

            return (currentLyric, nextLyric);
        }
        // 增加存储歌词列表的成员变量

        // 方法来加载和解析lrc文件
        //简而言之，LoadLyricsFromLRCFileAsync方法是一个用于从LRC文件异步加载歌词的辅助方法，它首先检查文件是否存在，
        //然后读取并解析歌词内容，最后可能将解析后的歌词存储在某个列表中供后续使用
        //public async Task LoadLyricsFromLRCFileAsync(string filepath)
        //{
        //    var lrcFilePath = Path.ChangeExtension(filepath, ".lrc");
        //    if (!File.Exists(lrcFilePath))
        //    {
        //        MessageBox.Show($"LRC歌词文件 {lrcFilePath} 不存在。", "歌词加载错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    // 异步读取整个文件的内容
        //    // 解析歌词
        //    lyricsList = await LoadLyricsAsync(lrcFilePath);
        //}
        public async Task<string> ReadFileTextAsync(string path)
        {
            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }

        // 实现LoadLyrics方法来解析lrc格式的歌词和时间标签
        private async Task<List<LyricLine>> LoadLyricsAsync(string lyricFilePath)
        {
            var lyrics = new List<LyricLine>();

            using (var reader = new StreamReader(lyricFilePath))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    // 解析行，格式预期为 "[mm:ss.xx]歌词"
                    var match = Regex.Match(line, @"\[(\d+):(\d+)(.\d+)?\](.*)");
                    if (match.Success)
                    {
                        var minutes = int.Parse(match.Groups[1].Value);
                        var seconds = int.Parse(match.Groups[2].Value);
                        // 确保毫秒数正确转换，防止数值太小被忽略
                        var millisecondsText = match.Groups[3].Value.Substring(1);
                        var milliseconds = millisecondsText.Length > 0 ? int.Parse(millisecondsText) : 0;
                        if (millisecondsText.Length == 2)
                        {
                            milliseconds *= 10;
                        }
                        else if (millisecondsText.Length == 1)
                        {
                            milliseconds *= 100;
                        }
                        var timeSpan = new TimeSpan(0, 0, minutes, seconds, milliseconds);
                        var text = match.Groups[4].Value;
                        lyrics.Add(new LyricLine(timeSpan, text));
                    }
                }
            }

            return lyrics;
        }


    }

        public class LyricLine//歌词
        {
            public TimeSpan Time { get; set; }
            public string Text { get; set; }

            public LyricLine(TimeSpan time, string text)
            {
                Time = time;
                Text = text;
            }
        }

        public class ReverbEffectProvider : ISampleProvider
        {
            private readonly ISampleProvider source;
            private float decay;
            private int delaySamples;
            private float[] delayBuffer;
            private int readPosition;
            private int writePosition;
            private readonly object lockObject = new object();

            public ReverbEffectProvider(ISampleProvider source, float decay, TimeSpan delay)
            {
                this.source = source;
                SetReverbParameters(decay, delay);
            }

            public void UpdateReverb(float newDecay, TimeSpan newDelay)
            {
                lock (lockObject)
                {
                    SetReverbParameters(newDecay, newDelay);
                }
            }
            private void SetReverbParameters(float newDecay, TimeSpan newDelay)
            {
                decay = newDecay;
                var newDelaySamples = (int)(newDelay.TotalSeconds * source.WaveFormat.SampleRate);

                // 如果 delayBuffer 还没有初始化, 先进行初始化
                if (delayBuffer == null)
                {
                    delayBuffer = new float[newDelaySamples];
                }

                // 仅在新的延迟样本数量和当前的不一致时进行缓冲区的重新初始化和数据复制 
                if (newDelaySamples != delaySamples)
                {
                    float[] newDelayBuffer = new float[newDelaySamples];

                    // Copy any existing delay to the new buffer
                    // 使用 Math.Min 确保不会超出源数组的界限，但首先检查 delayBuffer 是否为 null
                    if (delayBuffer != null)
                    {
                        Array.Copy(delayBuffer, newDelayBuffer, Math.Min(newDelaySamples, delaySamples));
                    }
                    delayBuffer = newDelayBuffer;
                    delaySamples = newDelaySamples;
                }

                // Adjust read and write positions if the circular buffer size changed
                readPosition = Math.Min(readPosition, delaySamples - 1);
                writePosition = Math.Min(writePosition, delaySamples - 1);
            }
            public WaveFormat WaveFormat => source.WaveFormat;

            public int Read(float[] buffer, int offset, int count)
            {
                int samplesRead = source.Read(buffer, offset, count);

                lock (lockObject)
                {
                    for (int i = 0; i < samplesRead; i++)
                    {
                        float wetSample = delayBuffer[readPosition];
                        buffer[offset + i] += wetSample * decay;

                        delayBuffer[writePosition] = buffer[offset + i];

                        readPosition = (readPosition + 1) % delaySamples;
                        writePosition = (writePosition + 1) % delaySamples;
                    }
                }

                return samplesRead;
            }

        }

   
}
