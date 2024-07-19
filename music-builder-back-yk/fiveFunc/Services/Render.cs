using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class Render
    {
        public string GetRender(string midi, string singer, string lyrics, string wavName)
        {

            // 创建一个新的Process实例
            Process process = new Process();
            string firstCommand = "cd C:\\Users\\yuanke\\Desktop\\OpenUtau-0.1.501.42\\OpenUtau";
            string secondCommand = "dotnet run --" + " " + midi + " " + singer + " " + " C:\\Users\\yuanke\\Desktop\\RenderFile\\" + wavName + " " + lyrics;
            // 配置ProcessStartInfo对象
            process.StartInfo.FileName = "cmd.exe"; // 指定要执行的命令行程序，这里是cmd.exe
            process.StartInfo.Arguments = $"/c {firstCommand} & {secondCommand}"; ; // 指定要执行的命令，这里是列出当前目录下的文件
            process.StartInfo.RedirectStandardOutput = true; // 允许将标准输出重定向到程序中
            process.StartInfo.UseShellExecute = false; // 不使用Shell执行程序
            process.StartInfo.CreateNoWindow = true; // 不创建新的窗口

            // 启动进程
            process.Start();

            // 读取输出
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(); // 等待进程结束
            string local = "C:\\Users\\yuanke\\Desktop\\RenderFile\\" + wavName + ".wav";

            return local;
        }

    }
}
