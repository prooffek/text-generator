using Microsoft.AspNetCore.Mvc;
using TextGenerator.Application.Interfaces;

namespace Generator.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected readonly ITextGeneratorManager _textGeneratorManager;

        public BaseController(ITextGeneratorManager textGeneratorManager)
        {
            _textGeneratorManager = textGeneratorManager;
        }
    }
}
