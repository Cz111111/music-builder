using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using NAudio.Utils;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;

namespace SingScore
{
    public class HandleMusic
    {
        //自写，调用模型转换
        public void trans(string audioFilePath)
        {
            // 定义Spleeter命令行工具的路径，如果spleeter已经添加到环境变量中，可以直接使用spleeter
            string spleeterExecutablePath = "spleeter";

            // 定义要使用的Spleeter分离模型，这里使用2stems模型
            string model = "2stems";

            // 定义输出目录，根据需要可以修改
            string outputDirectory = "C:/Users/123/Documents/GitHub/music-builder/music-builder-front/src/assets/output";
            // 定义ProcessStartInfo对象
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "spleeter",
                Arguments = $"separate {audioFilePath} -p spleeter:2stems -o {outputDirectory}",
                UseShellExecute = false,
                RedirectStandardOutput = true, // 重定向标准输出
                RedirectStandardError = true,  // 重定向错误输出
                CreateNoWindow = true        // 不显示命令行窗口
            };
            Console.WriteLine(startInfo.Arguments);

            // 启动进程并等待其完成
            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit(); // 等待进程结束

                // 读取输出和错误信息
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(errors))
                {
                    Console.WriteLine("Error: " + errors);
                }
                else
                {
                    Console.WriteLine("Output: " + output);
                    Console.WriteLine("Processing completed successfully for file: " + audioFilePath);
                }
            }
        }
    }
}
