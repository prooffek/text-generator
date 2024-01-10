using Microsoft.AspNetCore.Mvc;
using TextGenerator.Application.Interfaces;
using TextGenerator.Application.Models;

namespace Generator.WebApi.Controllers
{
    public class TemplateController : BaseController
    {
        public TemplateController(ITextGeneratorManager textGeneratorManager) : base(textGeneratorManager)
        {
        }

        [HttpPost("invitation-email")]
        public async Task<IActionResult> GetInvitationEmail([FromBody] InvitationEmail model)
        {
            var text = Task.FromResult(_textGeneratorManager.Handle(model));
            return Ok(text);
        }
    }
}
