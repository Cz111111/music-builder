using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WebApplication1
{
    public class Music
    {
        public string GetMusic(string description)
        {

            // 创建一个新的Process实例
            Process process = new Process();
            string firstCommand = "cd C:\\Users\\yuanke\\Desktop\\makeMusic";
            string secondCommand = "python makeMusic.py " + trans(description);
            // 配置ProcessStartInfo对象
            process.StartInfo.FileName = "cmd.exe"; // 指定要执行的命令行程序，这里是cmd.exe
            process.StartInfo.Arguments = $"/c {firstCommand} & {secondCommand}";  // 指定要执行的命令，这里是列出当前目录下的文件
            process.StartInfo.RedirectStandardOutput = true; // 允许将标准输出重定向到程序中
            process.StartInfo.UseShellExecute = false; // 不使用Shell执行程序
            process.StartInfo.CreateNoWindow = true; // 不创建新的窗口

            // 启动进程
            process.Start();

            // 读取输出
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(); // 等待进程结束

            string pattern = @"'(.*?)'";

            // 创建正则表达式对象
            Regex regex = new Regex(pattern);

            // 匹配输入字符串中的所有文件路径
            MatchCollection matches = regex.Matches(output);

            return matches[1].Groups[1].Value;
        }

        public string trans(string keyword_zh)
        {
            Process process = new Process();
            string firstCommand = "cd C:\\Users\\yuanke\\Desktop\\trans";
            string secondCommand = "python main.py " + keyword_zh;

            process.StartInfo.FileName = "cmd.exe"; // 指定要执行的命令行程序，这里是cmd.exe
            process.StartInfo.Arguments = $"/c {firstCommand} & {secondCommand}";  // 指定要执行的命令，这里是列出当前目录下的文件
            process.StartInfo.RedirectStandardOutput = true; // 允许将标准输出重定向到程序中
            process.StartInfo.UseShellExecute = false; // 不使用Shell执行程序
            process.StartInfo.CreateNoWindow = true; // 不创建新的窗口
                                                     // 启动进程
                                                     //try
                                                     //{
                                                     // 启动进程
            process.Start();

            // 等待进程结束
            process.WaitForExit();

            // 读取输出
            string output = process.StandardOutput.ReadToEnd();
            return output;
        }
    }
}
