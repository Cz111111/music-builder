using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WebApplication1
{
    public class Arrange
    {
        public string GetArrange(string instrument)
        {
            Process process = new Process();
            string firstCommand = "cd C:\\Users\\yuanke\\Desktop\\arrangement";
            string secondCommand = "python arrangement.py " + instrument;

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

                 // 正则表达式匹配文件路径
                 string pattern = @"'(.*?)'";

                // 创建正则表达式对象
                Regex regex = new Regex(pattern);

                 // 匹配输入字符串中的所有文件路径
                MatchCollection matches = regex.Matches(output);

            // 遍历所有匹配项并打印


            // Group 1 是括号内匹配的内容，即文件路径
            
            
            // 打印输出和错误



            // 返回脚本输出
             return matches[0].Groups[1].Value;
           // }
           // catch (Exception ex)
           // {
           //     Console.WriteLine("Exception: " + ex.Message);
           //     return "Error: " + ex.Message;
          //  }
        }
    }

}


