using System.Diagnostics;

namespace WebApplication1
{
    public class Some
    {
        public string GetSome(string wavname)
        {

            // 创建一个新的Process实例
            Process process = new Process();
            string firstCommand = "conda activate some";
            string secondCommand = "cd C:\\Users\\yuanke\\Desktop\\SOME-1.0.0-baseline";
            string thirdCommand = "python infer.py --model C:\\Users\\yuanke\\Desktop\\0119_continuous128_5spk\\0119_continuous256_5spk\\model_ckpt_steps_100000_simplified.ckpt --wav " + wavname;

            process.StartInfo.FileName = "cmd.exe"; // 指定要执行的命令行程序，这里是cmd.exe
            process.StartInfo.Arguments = $"/c {firstCommand} & {secondCommand} & {thirdCommand}";  // 指定要执行的命令，这里是列出当前目录下的文件
            process.StartInfo.RedirectStandardOutput = true; // 允许将标准输出重定向到程序中
            process.StartInfo.UseShellExecute = false; // 不使用Shell执行程序
            process.StartInfo.CreateNoWindow = true; // 不创建新的窗口

            // 启动进程
            process.Start();

            // 读取输出
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(); // 等待进程结束

            return "C:\\Users\\yuanke\\Desktop\\SomeFile\\1.mid";
        }
    }
}
