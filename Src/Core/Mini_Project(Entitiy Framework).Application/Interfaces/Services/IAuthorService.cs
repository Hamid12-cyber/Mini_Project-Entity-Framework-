using Mini_Project_Entitiy_Framework_.Domain.Entities;
using Mini_Project_Entitiy_Framework_.Domain.Enums;

namespace Mini_Project_Entitiy_Framework_.Application.Interfaces.Services
{
    public interface IAuthorService
    {
        Author CreateAuthor(string name, string? surname, Gender gender);
        List<Author> GetAllAuthors();
        Author? GetAuthorById(int id);
        List<Book> GetAuthorBooks(int authorId);
    }
}
