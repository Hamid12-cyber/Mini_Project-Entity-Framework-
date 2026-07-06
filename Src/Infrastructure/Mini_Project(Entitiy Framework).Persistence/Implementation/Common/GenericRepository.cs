using Microsoft.EntityFrameworkCore;
using Mini_Project_Entitiy_Framework_.Persistence.Data;
using Mini_Project_Entitiy_Framework_.Persistence.JsonServices;
using System.Threading.Tasks;


namespace Mini_Project_Entitiy_Framework_.Persistence.Implementation.Common
{
    public class GenericRepository<T> where T : class
    {
        protected readonly LibraryDbContext Context;
        protected readonly DbSet<T> DbSet;
        protected readonly JsonRepository<T> JsonRepository;

        public GenericRepository(LibraryDbContext context, JsonRepository<T> jsonRepository)
        {
            Context = context;
            DbSet = context.Set<T>();
            JsonRepository = jsonRepository;
        }

        public T? GetById(int id) => DbSet.Find(id);

        public List<T> GetAll() => DbSet.ToList();

        public void Add(T entity) => DbSet.Add(entity);

        public void Update(T entity) => DbSet.Update(entity);

        public void Delete(T entity) => DbSet.Remove(entity);

        public int SaveChanges()
        {
            var result = Context.SaveChanges();

            var backupData = GetDataForBackup();
            Task.Run(() => JsonRepository.Serialize(backupData));

            return result;
        }
        protected virtual List<T> GetDataForBackup() => DbSet.ToList();
    }
}
