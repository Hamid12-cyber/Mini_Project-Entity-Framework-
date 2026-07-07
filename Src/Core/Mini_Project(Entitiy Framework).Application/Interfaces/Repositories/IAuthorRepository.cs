using Mini_Project_Entitiy_Framework_.Domain.Entities;
namespace Mini_Project_Entitiy_Framework_.Application.Interfaces.Repositories
{
    public interface IAuthorRepository: IGenericRepository<Author>
    {
        List<Author> GetAllWithBooks();
        Author? GetByIdWithBooks(int id);
        List<Author> SearchByName(string keyword);
    }
}
