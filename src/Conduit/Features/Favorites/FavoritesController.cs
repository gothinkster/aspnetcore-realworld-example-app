using System.Threading.Tasks;
using Conduit.Features.Articles;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Features.Favorites
{
    [Route("articles")]
    public class FavoritesController : Controller
    {
        private readonly IMediator _mediator;

        public FavoritesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{slug}/favorite")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<ArticleEnvelope> FavoriteAdd(string slug)
        {
            return await _mediator.Send(new Add.Command(slug));
        }

        [HttpDelete("{slug}/favorite")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<ArticleEnvelope> FavoriteDelete(string slug)
        {
            return await _mediator.Send(new Delete.Command(slug));
        }
    }
}