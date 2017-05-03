using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Security;

namespace RealWorld.Features.Users
{
    public class Login
    {
        public class UserData
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class Command : IRequest<UserEnvelope>
        {
            public UserData User { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Command, UserEnvelope>
        {
            private readonly RealWorldContext _db;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;

            public Handler(RealWorldContext db, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
            {
                _db = db;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
            }

            public async Task<UserEnvelope> Handle(Command message)
            {
                var person = await _db.Persons.Where(x => x.Email == message.User.Email).SingleOrDefaultAsync();
                if (person == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }

                if (!person.Hash.SequenceEqual(_passwordHasher.Hash(message.User.Password, person.Salt)))
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }
             
                var user  = Mapper.Map<Domain.Person, User>(person); ;
                user.Token = await _jwtTokenGenerator.CreateToken(person.Username);
                return new UserEnvelope(user);
            }
        }
    }
}
