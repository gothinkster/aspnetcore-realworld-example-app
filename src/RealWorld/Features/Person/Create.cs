using System;
using System.Linq;
using AutoMapper;
using MediatR;
using RealWorld.Infrastructure;

namespace RealWorld.Features.Person
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public string Bio { get; set; }

            public string Image { get; set; }

            public string Hash { get; set; }

            public string Salt { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly RealWorldContext _db;

            public Handler(RealWorldContext db)
            {
                _db = db;
            }

            public void Handle(Command message)
            {
                var person = Mapper.Map<Command, Domain.Person>(message);
                _db.Persons.Add(person);
                _db.SaveChanges();
            }
        }
    }
}
