using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure;

namespace RealWorld.Features.Users
{
    public class Login
    {
        public class Command : IRequest<Domain.User>
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Command, Domain.User>
        {
            private readonly RealWorldContext _db;
            private readonly IPasswordHasher _passwordHasher;

            public Handler(RealWorldContext db, IPasswordHasher passwordHasher)
            {
                _db = db;
                _passwordHasher = passwordHasher;
            }

            public async Task<Domain.User> Handle(Command message)
            {
                var person = await _db.Persons.Where(x => x.Email == message.Email).SingleOrDefaultAsync();
                if (person == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }

                if (!person.Hash.SequenceEqual(_passwordHasher.Hash(message.Password, person.Salt)))
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }

                return Mapper.Map<Domain.Person, Domain.User>(person); ;
            }
        }
    }
}
