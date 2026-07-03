
namespace Mini_Project_Entitiy_Framework_.Applications.İnterfaces.Services
{
    internal interface IAuthorService
    {
        Author CreateAuthor(string name, string? surname, Gender gender);
        List<Author> GetAllAuthors();
        Author? GetAuthorById(int id);
        List<Book> GetAuthorBooks(int authorId);
    }
}
