
namespace Mini_Project_Entitiy_Framework_.Applications.İnterfaces.Services
{
    internal interface IBookService
    {
        Book CreateBook(string name, int pageCount, int authorId);
        bool DeleteBook(int id);
        Book? GetBookById(int id);
        List<Book> GetAllBooks();
    }
}
