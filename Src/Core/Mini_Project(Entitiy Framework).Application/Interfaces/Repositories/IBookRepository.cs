using Mini_Project_Entitiy_Framework_.Domain.Entities;
namespace Mini_Project_Entitiy_Framework_.Application.Interfaces.Repositories
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Book? GetByIdWithDetails(int id);
        List<Book> GetAllWithAuthor();
        List<Book> GetBooksByAuthorId(int authorId);
        List<Book> SearchByName(string keyword);
    }
}
