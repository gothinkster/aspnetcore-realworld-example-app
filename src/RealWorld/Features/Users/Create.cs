using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Domain;
using RealWorld.Infrastructure;

namespace RealWorld.Features.Users
{
    public class Create
    {
        public class Command : IRequest<Domain.User>
        {
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
                if (await _db.Persons.Where(x => x.Email == message.Email).AnyAsync())
                {
                    throw new RestException(HttpStatusCode.BadRequest);
                }

                var salt = Guid.NewGuid().ToByteArray();
                var person = new Person
                {
                    Email = message.Email,
                    Hash = _passwordHasher.Hash(message.Password, salt),
                    Salt = salt
                };

                _db.Persons.Add(person);
                await _db.SaveChangesAsync();

                return Mapper.Map<Domain.Person, Domain.User>(person); ;
            }
        }
    }
}
