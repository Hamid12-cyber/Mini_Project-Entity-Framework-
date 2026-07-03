using Mini_Project_Entitiy_Framework_.Domain.Entities;
namespace Mini_Project_Entitiy_Framework_.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T? GetById(int id);
        List<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int SaveChanges();
    }
}
