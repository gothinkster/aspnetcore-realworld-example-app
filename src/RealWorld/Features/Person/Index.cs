using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealWorld.Infrastructure;

namespace RealWorld.Features.Person
{
    public class Index
    {
        public class Query : IRequest<List<Domain.Person>>
        {
        }
        
        public class Handler : IRequestHandler<Query, List<Domain.Person>>
        {
            private readonly RealWorldContext _db;

            public Handler(RealWorldContext db)
            {
                _db = db;
            }

            public List<Domain.Person> Handle(Query message)
            {
                return _db.Persons.ToList();
            }
        }
    }
}