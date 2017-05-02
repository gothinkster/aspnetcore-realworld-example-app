using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RealWorld.Infrastructure;

namespace RealWorld.Features.Person
{
    public class Create
    {
        public class Command : IRequest<Domain.Person>
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public string Bio { get; set; }

            public string Image { get; set; }

            public string Hash { get; set; }

            public string Salt { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Command, Domain.Person>
        {
            private readonly RealWorldContext _db;

            public Handler(RealWorldContext db)
            {
                _db = db;
            }

            public async Task<Domain.Person> Handle(Command message)
            {
                var person = Mapper.Map<Command, Domain.Person>(message);
                var add = _db.Persons.Add(person);
                await _db.SaveChangesAsync();
                return person;
            }
        }
    }
}
