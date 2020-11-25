using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Conduit.Domain
{
    public static class PersonExtensions
    {
        public static IQueryable<Person> GetAllData(this DbSet<Person> model)
        {
            return model
                .AsNoTracking();
        }
    }
}
