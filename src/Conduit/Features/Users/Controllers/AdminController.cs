using Conduit.Features.Articles;
using Conduit.Features.Comments;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("getAllBannedArticles")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<ArticlesEnvelope> GetAllBannedArticles([FromQuery] string title, [FromQuery] string author, [FromQuery] string createdDate, [FromQuery] string updatedDate, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new Articles.AllList.Query(author, title, createdDate, updatedDate, limit, offset, false));
        }

        [HttpGet("getAllBannedComments")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<CommentsEnvelope> GetAllBannedComments([FromQuery] int articleId, [FromQuery] string author, [FromQuery] string createdDate, [FromQuery] string updatedDate, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new Comments.AllList.Query(author, articleId, createdDate, updatedDate, limit, offset, false)) ;
        }

        [HttpGet("getNewBannedArticles")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<ArticlesEnvelope> GetNewBannedArticles([FromQuery] string title, [FromQuery] string author, [FromQuery] string createdDate, [FromQuery] string updatedDate, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new Articles.AllList.Query(author, title, createdDate, updatedDate, limit, offset, true));
        }

        [HttpGet("getNewBannedComments")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<CommentsEnvelope> GetNewBannedComments([FromQuery] int articleId, [FromQuery] string author, [FromQuery] string createdDate, [FromQuery] string updatedDate, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new Comments.AllList.Query(author, articleId, createdDate, updatedDate, limit, offset, true));
        }

        [HttpGet("getAllUser")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<UsersEnvelope> GetAllUser([FromQuery] string login, [FromQuery] string email,[FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new Users.AllList.Query(login, email, limit, offset));
        }

        [HttpGet("getAllBanUsers")]        
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<UsersEnvelope> GetAllBanUsers([FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new Users.AllList.Query(null, null, limit, offset, true));
        }
     
        [HttpPut("banUser")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<UserEnvelope> BanUser([FromBody] Edit.Command command)
        {
            command.User.Banned = true;
            return await _mediator.Send(command);
        }
     
        [HttpPut("unbanedUser")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<UserEnvelope> UnbanedUser([FromBody] Edit.Command command)
        {
            command.User.Banned = false;
            return await _mediator.Send(command);
        }
    }
}

