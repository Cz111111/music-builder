﻿using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace SingScore
{
    [ApiController]
    [Route("api/ksong/sing")]
    public class AudioController : ControllerBase
    {
        private readonly AudioService audioService;
        private readonly HandleMusic _handleMusic = new HandleMusic();
        public AudioController(AudioService audioService)
        {
            this.audioService = audioService;
        }

        //上传文件，并调用模型进行分离
        [HttpPost("uploadfile")]
        public async Task<IActionResult> UploadFile([FromBody] FileDTO file)
        {
            string audioFile = file.audioFile;

            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            //对上传的文件为音频文件进行判断
            string fileExtension = Path.GetExtension(audioFile);
            // 检查文件扩展名是否为 .mp3 或 .wav
            if (fileExtension != ".mp3" && fileExtension != ".wav")
            {
                return BadRequest("Only MP3 or WAV files are allowed.");
            }
            //将上传的文件给存储到数据库中去
            if (!string.IsNullOrWhiteSpace(audioFile))
            {
                // 使用文件路径的文件名部分作为音频标题
                string filePath = audioFile;
                string title = Path.GetFileNameWithoutExtension(filePath); // 去除文件扩展名

                // 创建新的 Audio 对象实例
                int maxNum = audioService.GetMaxNum() + 1; // 获取下一个可用序号
                Audio newAudio = new Audio(maxNum, title, filePath);

                // 调用 AudioService 的方法将新音频对象添加到数据库

                if (!audioService.AddAudio(newAudio))
                {
                    return BadRequest("已经存在");
                }
                // 调用音频分离方法
                _handleMusic.trans(audioFile);

                // 返回分离后的文件信息
                return Ok("成功");
            }
            else
            {
                return BadRequest("文件不存在");
            }
        }

        //进行FFT分析得到k歌的主要频率
        [HttpGet("draw")]
        public async Task<ActionResult<List<float>>> GetFrequencyData(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                return BadRequest("File path is required.");
            }
            try
            {
                // 调用之前定义的CalculateFrequency_halfsecond方法
                var frequencyData = await Functions.CalculateFrequency_halfsecond(filepath);
                return Ok(frequencyData);
            }
            catch (System.Exception ex)
            {
                // 如果有异常发生，返回错误信息
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("upload")]
        public async Task<IActionResult>  Upload([FromQuery] IFormFile file)
        {
            // 检查文件是否存在
            if (file == null || file.Length == 0)
            {
                return BadRequest("没有上传文件。");
            }

            try
            {
                // 获取文件名
                var fileName = Path.GetFileName(file.FileName);

                // 定义文件存储路径
                var folderPath = @"C:\Users\123\Desktop\Sing\SingScore\input";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);

                // 将文件内容写入到服务器的文件路径
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                string audioFile = Path.Combine(folderPath, fileName);
                Console.WriteLine(audioFile);
                _handleMusic.trans(audioFile);
                //// 返回操作成功响应
                //return Ok(new
                //{
                //    message = "文件上传成功",
                //    fileName = fileName // 返回文件的服务器存储路径
                //});
                var frequencyData = await Functions.CalculateFrequency_halfsecond(audioFile);
                return Ok(frequencyData);
            }
            catch (Exception ex)
            {
                // 处理异常，返回错误信息
                return StatusCode(500, $"文件上传失败: {ex.Message}");
            }
        }


    }
}