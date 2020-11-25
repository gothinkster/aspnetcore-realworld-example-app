using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Conduit.Domain;
using Conduit.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Users
{
    public class AllList
    {
        public class Query : IRequest<UsersEnvelope>
        {
            public Query(string login, string email, int? limit, int? offset, bool ban = false)
            {
                Login = login;
                Email = email;
                Limit = limit;
                Offset = offset;
                Ban = ban;
            }

            public string Login { get; }
            public string Email { get; }
            public int? Limit { get; }
            public int? Offset { get; }
            public bool Ban {get;set;}
        }

        public class QueryHandler : IRequestHandler<Query, UsersEnvelope>
        {
            private readonly ConduitContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(ConduitContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UsersEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                IQueryable<Person> queryable = _context.Persons.GetAllData();


                if (!string.IsNullOrWhiteSpace(message.Login))
                {
                    queryable = queryable.Where(x => x.Username.Contains(message.Login));
                }
                if (!string.IsNullOrWhiteSpace(message.Email))
                {
                    queryable = queryable.Where(x => x.Email.Contains(message.Email));
                }
              

                var personas = await queryable
                    .Where(x=>x.IsAdmin == false)
                    .Skip(message.Offset ?? 0)
                    .Take(message.Limit ?? 20)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var users = new List<User>();

                foreach (var person in personas)
                {
                    users.Add( _mapper.Map<Domain.Person, User>(person));
                }

                if (message.Ban)
                {
                    users = users.Where(x => x.ArticleBan >= 3 || x.CommentBan >= 3).ToList();
                }

                return new UsersEnvelope()
                {
                    Users = users,
                    UsersCount = users.Count
                };
            }
        }
    }
}