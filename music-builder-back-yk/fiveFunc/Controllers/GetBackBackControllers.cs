using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Text.Json;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetBackBackControllers : ControllerBase
    {

        private readonly ILogger<GetBackBackControllers> _logger;
        public GetBackBackControllers(ILogger<GetBackBackControllers> logger)
        {
            _logger = logger;
            _render = new Render();
            _some = new Some();
            _paint = new Paint();
            _arrange = new Arrange();
            _music = new Music();
        }

        private readonly Render _render;
        [HttpPost]
        [Route("render")]
        public async Task<IActionResult> PostRender([FromQuery] IFormFile midiFile, string singer, string lyrics, string wavName)
        {
            if (midiFile == null || midiFile.Length == 0)
            {
                return BadRequest("MIDI file is required.");
            }

            // 确保文件具有MIDI扩展名
            if (!midiFile.FileName.EndsWith(".mid", StringComparison.OrdinalIgnoreCase) &&
                !midiFile.FileName.EndsWith(".midi", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid file type. Only MIDI files are allowed.");
            }

            // 存储上传的MIDI文件到临时位置或直接传递流给渲染服务
            var midipath = "C:\\Users\\yuanke\\Desktop\\RenderFile\\1.mid"; // 生成临时文件路径
            using (var fileStream0 = new FileStream(midipath, FileMode.Create))
            {
                await midiFile.CopyToAsync(fileStream0);
            }

            // 调用渲染服务，传入MIDI文件路径和其他参数

            string local = _render.GetRender(midipath, singer, lyrics, wavName);

            Response.Headers.Add("Content-Disposition", "attachment; filename=" + Path.GetFileName(local));
            byte[] fileBytes = System.IO.File.ReadAllBytes(local);
            MemoryStream fileStream = new MemoryStream(fileBytes);
            Response.ContentType = "audio/wav";
            return File(fileStream, Response.ContentType);
        }

        private readonly Some _some;
        [HttpPost]
        [Route("some")]
        public async Task<IActionResult> PostSome([FromQuery] IFormFile wavFile)
        {
            if (wavFile == null || wavFile.Length == 0)
            {
                return BadRequest("wav file is required.");
            }

            // 确保文件具有MIDI扩展名
            if (!wavFile.FileName.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid file type. Only MIDI files are allowed.");
            }

            // 存储上传的MIDI文件到临时位置或直接传递流给渲染服务
            var wavpath = "C:\\Users\\yuanke\\Desktop\\SomeFile\\1.wav"; // 生成临时文件路径
            using (var fileStream0 = new FileStream(wavpath, FileMode.Create))
            {
                await wavFile.CopyToAsync(fileStream0);
            }

            // 调用渲染服务，传入MIDI文件路径和其他参数
            string local = _some.GetSome(wavpath);
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + Path.GetFileName(local));
            byte[] fileBytes = System.IO.File.ReadAllBytes(local);
            MemoryStream fileStream = new MemoryStream(fileBytes);
            Response.ContentType = "audio/midi";
            return File(fileStream, Response.ContentType);
        }

        private readonly Paint _paint;
        [HttpGet]
        [Route("paint")]
        public async Task<IActionResult> GetPaint([FromQuery] string keyWord, string Pname)
        {
            string local = await _paint.GetPaint(keyWord, Pname);
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + Path.GetFileName(local));
            byte[] fileBytes = System.IO.File.ReadAllBytes(local);
            MemoryStream fileStream = new MemoryStream(fileBytes);
            Response.ContentType = "image/png";
            return File(fileStream, Response.ContentType);
        }

        private readonly Arrange _arrange;
        [HttpGet]
        [Route("arrange")]
        public ActionResult<string> GetArrange([FromQuery] string instrument)
        {
            string local = _arrange.GetArrange(instrument);
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + Path.GetFileName(local));
            byte[] fileBytes = System.IO.File.ReadAllBytes(local);
            MemoryStream fileStream = new MemoryStream(fileBytes);
            Response.ContentType = "audio/midi";
            return File(fileStream, Response.ContentType);
        }

        private readonly Music _music;
        [HttpGet]
        [Route("music")]
        public ActionResult<string> GetMusic([FromQuery] string description)
        {
            string local = _music.GetMusic(description);
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + Path.GetFileName(local));
            byte[] fileBytes = System.IO.File.ReadAllBytes(local);
            MemoryStream fileStream = new MemoryStream(fileBytes);
            Response.ContentType = "audio/wav";
            return File(fileStream, Response.ContentType);
        }
    }
}
