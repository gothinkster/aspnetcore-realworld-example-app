using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWorld.Features.Profiles;
using RealWorld.Infrastructure.Security;

namespace RealWorld.Features.Articles
{
    [Route("articles")]
    public class ArticlesController : Controller
    {
        private readonly IMediator _mediator;

        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ArticlesEnvelope> Get()
        {
            return await _mediator.Send(new List.Query());
        }

        [HttpPost]
        [Authorize(ActiveAuthenticationSchemes = JwtIssuerOptions.Scheme)]
        public async Task<ArticleEnvelope> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{slug}")]
        [Authorize(ActiveAuthenticationSchemes = JwtIssuerOptions.Scheme)]
        public Task<ProfileEnvelope> Delete(string slug)
        {
            throw new NotImplementedException();
        }
    }
}