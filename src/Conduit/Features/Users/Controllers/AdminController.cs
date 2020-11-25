using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Features.Users
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[HttpGet]
        //public async Task<TagsEnvelope> Get()
        //{
        //    return await _mediator.Send(new List.Query());
        //}
    }
}

