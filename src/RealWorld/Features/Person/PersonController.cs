using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealWorld.Features.Person
{
    [Route("persons")]
    public class PersonController : Controller
    {
        private readonly IMediator _mediator;

        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = await _mediator.Send(new Index.Query());
            return new JsonResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Create.Command command)
        {
            await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }
    }
}