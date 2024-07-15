using MathNet.Numerics.IntegralTransforms;
using NAudio.Lame;
using NAudio.Wave;
using System.Numerics;

namespace SingScore
{
    //输入MP3文件，输出两个复数数组，左声道和右声道的频谱数据
    public class Functions
    {
        //计算0.5秒内的频率
        public static async Task<List<float>> CalculateFrequency_halfsecond(string filepath)
        {
            var frequencyData = new List<float>();

            using (var audioFileReader = new AudioFileReader(filepath))
            {
                double windowDuration = 0.5; // 每个窗口的时长为0.5秒
                int windowSize = (int)(audioFileReader.WaveFormat.SampleRate * windowDuration); // 0.5秒的样本数量
                
                var buffer = new float[windowSize];
                int bytesRead;
                int sampleRate = audioFileReader.WaveFormat.SampleRate;
                int a = 0;
                while ((bytesRead = audioFileReader.Read(buffer, 0, windowSize)) > 0)
                {
                    var complexBuffer = new Complex[windowSize];
                    for (int i = 0; i < bytesRead; i++)
                    {
                        complexBuffer[i] = new Complex(buffer[i], 0);
                    }
                    Fourier.Forward(complexBuffer, FourierOptions.Matlab);

                    float dominantFrequency = await Task.Run(() =>
                    {
                        int idxMax = 0;
                        double maxMagnitude = 0.0;
                        for (int i = 0; i < windowSize / 2; i++) // 分析一半的FFT结果
                        {
                            if (complexBuffer[i].Magnitude > maxMagnitude && complexBuffer[i].Magnitude <= 800)
                            {
                                if (maxMagnitude <= 10) maxMagnitude = 10;
                                maxMagnitude = complexBuffer[i].Magnitude;
                                idxMax = i;
                            }
                        }
                        // return (float)Math.Max(800, (idxMax * sampleRate / (double)windowSize));
                        return (float)(idxMax * sampleRate / (double)windowSize);
                    });
                    if(dominantFrequency>600)
                    {
                        dominantFrequency = (float)600;
                    }
                    frequencyData.Add(dominantFrequency);
                }

                return frequencyData;
            }
        }
      
    }
}
