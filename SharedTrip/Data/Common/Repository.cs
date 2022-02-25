using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SharedTrip.Data.Common
{
    public class Repository : IRepository
    {
        private DbContext context;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void Add<T>(T entity) where T : class
            => DbSet<T>().Add(entity);

        public IQueryable<T> All<T>() where T : class
            => DbSet<T>().AsQueryable();

        public int SaveChanges()
            => context.SaveChanges();

        private DbSet<T> DbSet<T>() where T : class
            => context.Set<T>();
    }
}
