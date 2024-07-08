using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApp1
{
    public class Gender
    {
        public string GetGender(string input)
        {

            // 创建一个新的Process实例
            Process process = new Process();
            string firstCommand = "cd C:\\Users\\yuanke\\Desktop\\OpenUtau-0.1.501.42\\OpenUtau";
            string secondCommand = "dotnet run -- 起风了.mid JiHeHong-diffsinger-openutau-V2 这一路上走走停停顺着少年漂流的痕迹 成功了";
            // 配置ProcessStartInfo对象
            process.StartInfo.FileName = "cmd.exe"; // 指定要执行的命令行程序，这里是cmd.exe
            process.StartInfo.Arguments = $"/c {firstCommand} & {secondCommand}";; // 指定要执行的命令，这里是列出当前目录下的文件
            process.StartInfo.RedirectStandardOutput = true; // 允许将标准输出重定向到程序中
            process.StartInfo.UseShellExecute = false; // 不使用Shell执行程序
            process.StartInfo.CreateNoWindow = true; // 不创建新的窗口

            // 启动进程
            process.Start();

            // 读取输出
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(); // 等待进程结束

            // 显示输出
            Console.WriteLine(output);

            // 等待用户输入，以便查看输出
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            return "successfly make";
        }
    }
}
