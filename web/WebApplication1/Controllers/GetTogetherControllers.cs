using Microsoft.AspNetCore.Mvc;
using ClassLibrary1;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetTogetherControllers : ControllerBase
    {
        private readonly Gender _gender;

        private readonly ILogger<GetTogetherControllers> _logger;

        public GetTogetherControllers(ILogger<GetTogetherControllers> logger)
        {
            _logger = logger;
            _gender = new Gender();
        }

        [HttpGet]
        [Route("gender")]
        public ActionResult<string> GetGender([FromQuery] string midi, string singer, string lyrics, string wavName)
        {
            return _gender.GetGender(midi, singer, lyrics, wavName);
        }
    }
}
