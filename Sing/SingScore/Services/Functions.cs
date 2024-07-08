using MathNet.Numerics.IntegralTransforms;
using NAudio.Lame;
using NAudio.Wave;
using System.Numerics;

namespace SingScore
{
    //输入MP3文件，输出两个复数数组，左声道和右声道的频谱数据
    public class Functions
    {
        public static async Task<List<(Complex[] leftSpectrum, Complex[] rightSpectrum)>> CalculateSpectrum(string filepath, int windowsize = 4096)
        {
            List<(Complex[] leftSpectrum, Complex[] rightSpectrum)> spectrumBlocks = new List<(Complex[] leftSpectrum, Complex[] rightSpectrum)>();

            using (var mp3reader = new Mp3FileReader(filepath))
            {
                double[] window = HammingWindow(windowsize); // 创建汉明窗

                while (mp3reader.Position < mp3reader.Length)
                {
                    byte[] buffer = new byte[windowsize * mp3reader.WaveFormat.BlockAlign];
                    int bytesRead = await mp3reader.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0) break;
                    if (bytesRead < buffer.Length) Array.Clear(buffer, bytesRead, buffer.Length - bytesRead);

                    float[] leftChannel, rightChannel;
                    if (mp3reader.WaveFormat.Channels == 2)
                    {
                        // 如果是立体声，需要分别处理左右声道
                        (leftChannel, rightChannel) = StereoConvert(buffer, mp3reader.WaveFormat);
                    }
                    else
                    {
                        // 单声道情况下，左右声道数据相同
                        leftChannel = SingleConvert(buffer, mp3reader.WaveFormat);
                        rightChannel = leftChannel;
                    }

                    // 应用窗函数并执行FFT
                    Complex[] leftComplexBuffer = await Task.Run(() => FFT(leftChannel, window, windowsize));
                    Complex[] rightComplexBuffer = await Task.Run(() => FFT(rightChannel, window, windowsize));

                    // 将每个窗口的频谱数据放入列表中
                    spectrumBlocks.Add((leftComplexBuffer, rightComplexBuffer));
                }
            }

            // 返回左右声道的频谱数据
            return spectrumBlocks;
        }
        //汉明窗
        private static double[] HammingWindow(int size)
        {
            double[] window = new double[size];
            for (int i = 0; i < size; i++)
            {
                window[i] = 0.54 - 0.46 * Math.Cos((2 * Math.PI * i) / (size - 1));
            }
            return window;
        }

        // 转byte数组为能够进行音频处理的立体声float数组（左右声道），分16位和24位
        private static (float[] left, float[] right) StereoConvert(byte[] byteArray, WaveFormat waveformat)
        {
            int bytesPerSample = waveformat.BitsPerSample / 8;
            int totalSamples = byteArray.Length / bytesPerSample;
            int numSamplesPerChannel = totalSamples / waveformat.Channels; // 考虑声道数
            float[] leftChannel = new float[numSamplesPerChannel];
            float[] rightChannel = new float[numSamplesPerChannel];

            for (int i = 0; i < numSamplesPerChannel; i++)
            {
                if (waveformat.BitsPerSample == 16) // 16位音频
                {
                    int byteIndex = i * 2 * bytesPerSample;
                    leftChannel[i] = BitConverter.ToInt16(byteArray, byteIndex) / 32768f;
                    rightChannel[i] = BitConverter.ToInt16(byteArray, byteIndex + bytesPerSample) / 32768f;
                }
                else if (waveformat.BitsPerSample == 24) // 24位音频
                {
                    int byteIndex = i * 2 * 3;
                    leftChannel[i] = Convert24BitByteArrayToFloat(byteArray, byteIndex);
                    rightChannel[i] = Convert24BitByteArrayToFloat(byteArray, byteIndex + 3);
                }
            }

            return (leftChannel, rightChannel);
        }
        // 将24位数据转换为浮点数
        private static float Convert24BitByteArrayToFloat(byte[] byteArray, int start)
        {
            // 将24位byte数组转为32位整数
            int value = (byteArray[start + 0] << 16) | (byteArray[start + 1] << 8) | byteArray[start + 2];
            // 处理符号扩展
            if ((value & 0x800000) > 0)
            {
                value |= unchecked((int)0xFF000000);
            }
            else
            {
                value &= 0x00FFFFFF;
            }
            return value / 8388608f; // 2^23
        }

        // 转byte数组为能够进行音频处理的单声道声float数组，分16位和24位
        private static float[] SingleConvert(byte[] byteArray, WaveFormat waveformat)
        {
            int bytesPerSample = waveformat.BitsPerSample / 8;
            int numSamples = byteArray.Length / bytesPerSample;
            float[] floatArray = new float[numSamples];

            if (waveformat.BitsPerSample == 16)//16位存储的音频
            {
                for (int i = 0; i < numSamples; i++)
                {
                    short sample = BitConverter.ToInt16(byteArray, i * bytesPerSample);
                    floatArray[i] = sample / 32768f;
                }
            }
            else if (waveformat.BitsPerSample == 24)//24位存储的音频
            {
                for (int i = 0; i < numSamples; i++)
                {
                    byte[] byteArraySample = new byte[4];
                    byteArraySample[0] = byteArray[i * bytesPerSample];
                    byteArraySample[1] = byteArray[i * bytesPerSample + 1];
                    byteArraySample[2] = byteArray[i * bytesPerSample + 2];
                    byteArraySample[3] = 0;
                    int value = BitConverter.ToInt32(byteArraySample, 0);
                    floatArray[i] = value / 2147483648f;
                }
            }


            return floatArray;
        }

        //FFT处理频谱
        private static Complex[] FFT(float[] channelData, double[] window, int windowsize)
        {
            Complex[] complexBuffer = new Complex[windowsize];
            for (int i = 0; i < windowsize; i++)
            {
                double windowValue = (i < channelData.Length) ? window[i] : 0;
                complexBuffer[i] = new Complex(channelData[i] * windowValue, 0);
            }

            Fourier.Forward(complexBuffer, FourierOptions.NoScaling); // 执行FFT
            return complexBuffer;
        }

        //平均频谱
        private static Complex[] AverageSpectrum(Complex[] spectrumSum, int blockCount)
        {
            Complex[] averageSpectrum = new Complex[spectrumSum.Length];
            for (int i = 0; i < spectrumSum.Length; i++)
            {
                averageSpectrum[i] = spectrumSum[i] / blockCount;
            }
            return averageSpectrum;
        }

        //简而言之，CalculateAmplitude方法通过读取音频文件，计算每个时间窗口内的平均振幅，并将这些振幅值作为列表返回。这对于音频分析和可视化非常有用。
        //计算波形
        public static async Task<List<float>> CalculateAmplitude(string filepath)
        {
            var wavedata = new List<float>();

            using (var audioFileReader = new AudioFileReader(filepath))
            {
                double t = 0.2;
                int windowSize = (int)(audioFileReader.WaveFormat.SampleRate * t); // 0.2秒的样本数量(窗口)
                var buffer = new float[windowSize];
                int bytesRead;

                while ((bytesRead = audioFileReader.Read(buffer, 0, windowSize)) > 0)
                {
                    // 计算振幅
                    var avgAmplitude = await Task.Run(() =>
                    {
                        float sum = 0;
                        for (int i = 0; i < bytesRead; i++)
                        {
                            sum += Math.Abs(buffer[i]);
                        }
                        return sum / bytesRead;
                    });

                    wavedata.Add(avgAmplitude);
                }

                int number = (int)(audioFileReader.TotalTime.TotalSeconds / t); // 总的时间窗口数量
                return wavedata.Take(number).ToList();
            }
        }

        //计算频率
        public static async Task<List<float>> CalculateFrequency(string filepath)
        {
            var frequencyData = new List<float>();

            using (var audioFileReader = new AudioFileReader(filepath))
            {
                double t = 0.2; // 每个窗口的时长
                int windowSize = (int)(audioFileReader.WaveFormat.SampleRate * t); // 0.2秒的样本数量
                var buffer = new float[windowSize];
                int bytesRead;
                int sampleRate = audioFileReader.WaveFormat.SampleRate;

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
                            if (complexBuffer[i].Magnitude > maxMagnitude)
                            {
                                maxMagnitude = complexBuffer[i].Magnitude;
                                idxMax = i;
                            }
                        }
                        return (float)(idxMax * sampleRate / (double)windowSize);
                    });

                    frequencyData.Add(dominantFrequency);
                }

                int number = (int)(audioFileReader.TotalTime.TotalSeconds / t); // 总窗口数量
                return frequencyData.Take(number).ToList();
            }
        }

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
                        return (float)(idxMax * sampleRate / (double)windowSize);
                    });

                    frequencyData.Add(dominantFrequency);
                }

                return frequencyData;
            }
        }
        //滤波器，它允许低频信号通过，同时抑制高于截止频率的高频信号。
        public static List<float> LowPassButterworth(List<float> data, double fc, double fs)
        {
            int n = data.Count;
            double ita = 1.0 / Math.Tan(Math.PI * fc / fs);
            double q = Math.Sqrt(2);
            double b0 = 1.0 / (1.0 + q * ita + ita * ita);
            double b1 = 2 * b0;
            double b2 = b0;
            double a1 = 2.0 * (ita * ita - 1.0) * b0;
            double a2 = -(1.0 - q * ita + ita * ita) * b0;

            List<float> filteredData = new List<float>();
            double[] buffer = new double[3]; // 存储过去的输入
            double[] outBuffer = new double[3]; // 存储过去的输出

            for (int i = 0; i < n; i++)
            {
                double input = data[i];
                double output =
                    b0 * input + b1 * buffer[1] + b2 * buffer[2]
                    - a1 * outBuffer[1] - a2 * outBuffer[2];

                // 更新缓存
                buffer[2] = buffer[1];
                buffer[1] = input;
                outBuffer[2] = outBuffer[1];
                outBuffer[1] = output;

                filteredData.Add((float)output);
            }

            return filteredData;
        }

        //将wav转换为MP3格式的文件
        public static string ConvertWavToMp3(string inputWavFile)
        {
            string outputMp3File = Path.ChangeExtension(inputWavFile, ".mp3");

            using (var reader = new WaveFileReader(inputWavFile))
            {
                using (var writer = new LameMP3FileWriter(outputMp3File, reader.WaveFormat, 128))
                {
                    reader.CopyTo(writer);
                }
            }

            return outputMp3File;
        }


    }
}
