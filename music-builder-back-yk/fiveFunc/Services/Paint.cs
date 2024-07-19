using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.IO;
using SkiaSharp;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace WebApplication1
{
    public class Paint
    {
        public async Task<string> GetPaint(string keyWord,string name)
        {

            var targetUrl = "http://127.0.0.1:7860/sdapi/v1/txt2img";
            var payload = new
            {
                prompt = trans(keyWord),
                negative_prompt = "",
                steps = 15,
                override_settings = new
                {
                    sd_model_checkpoint = "anything-v5-PrtRE",
                    CLIP_stop_at_last_layers = 1
                },
                width = "512",
                height = "512"
            };

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(180); // 设置超时时间
                var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(targetUrl, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject r = JObject.Parse(jsonResponse);
                    //var r = JsonConvert.DeserializeObject<dynamic>(jsonResponse); // 使用dynamic类型来处理JSON
                    //Console.WriteLine(r["images"]);
                    //var imageBytes = Convert.FromBase64String(r[0]["images"][0]);
                    JArray imagesArray = (JArray)r["images"];
                    string base64Image = imagesArray[0].ToString();
                    var imageBytes = Convert.FromBase64String(base64Image);
                    //Console.WriteLine(6666);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        // 使用 SKBitmap 从流中加载图像
                        SKBitmap bitmap = SKBitmap.Decode(ms);

                        // 确保流的位置回到开始位置


                        // 使用 SKImage 从 SKBitmap 创建图像
                        SKImage image = SKImage.FromBitmap(bitmap);

                        // 保存图像为PNG格式到指定的文件路径
                        using (var imageDest = new FileStream("C:\\Users\\yuanke\\Desktop\\PaintFile\\"+name+".png", FileMode.Create))
                        {
                            // 编码图像为PNG并保存
                            image.Encode(SKEncodedImageFormat.Png, 100).SaveTo(imageDest);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                }
            }
            return "C:\\Users\\yuanke\\Desktop\\PaintFile\\" + name+".png";
        }

        public string trans (string keyword_zh)
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
