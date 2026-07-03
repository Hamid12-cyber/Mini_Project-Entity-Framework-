using Mini_Project_Entitiy_Framework_.Domain.Entities;
using Mini_Project_Entitiy_Framework_.Domain.Enums;

namespace Mini_Project_Entitiy_Framework_.Application.Interfaces.Services
{
    public interface IBookService
    {
        Book CreateBook(string name, int pageCount, int authorId);
        bool DeleteBook(int id);
        Book? GetBookById(int id);
        List<Book> GetAllBooks();
    }
}
