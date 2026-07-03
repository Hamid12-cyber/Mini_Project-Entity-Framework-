using Microsoft.EntityFrameworkCore;
using Mini_Project_Entitiy_Framework_.Persistence.Data;


namespace Mini_Project_Entitiy_Framework_.Persistence.Implementation.Common
{
    public class GenericRepository<T> where T : class
    {
        protected readonly LibraryDbContext Context;
        protected readonly DbSet<T> DbSet;

        public GenericRepository(LibraryDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public T? GetById(int id) => DbSet.Find(id);

        public List<T> GetAll() => DbSet.ToList();

        public void Add(T entity) => DbSet.Add(entity);

        public void Update(T entity) => DbSet.Update(entity);

        public void Delete(T entity) => DbSet.Remove(entity);

        public int SaveChanges() => Context.SaveChanges();
    }
}
