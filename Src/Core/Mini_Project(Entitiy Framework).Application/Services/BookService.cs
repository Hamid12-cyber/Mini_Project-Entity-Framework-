using Mini_Project_Entitiy_Framework_.Domain.Entities;
using Mini_Project_Entitiy_Framework_.Domain.Enums;
using Mini_Project_Entitiy_Framework_.Application.Exceptions;
using Mini_Project_Entitiy_Framework_.Application.Interfaces.Repositories;
using Mini_Project_Entitiy_Framework_.Application.Interfaces.Services;
using System.Linq;


namespace Mini_Project_Entitiy_Framework_.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IReservedItemRepository _reservedItemRepository;

    public BookService(
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        IReservedItemRepository reservedItemRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _reservedItemRepository = reservedItemRepository;
    }

    public Book CreateBook(string name, int pageCount, int authorId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Kitabın adı boş ola bilməz.");

        if (pageCount <= 0)
            throw new ValidationException("Səhifə sayı müsbət ədəd olmalıdır.");

        var author = _authorRepository.GetById(authorId);
        if (author is null)
            throw new NotFoundException($"Id-si {authorId} olan Author tapılmadı.");

        var book = new Book
        {
            Name = name.Trim(),
            PageCount = pageCount,
            AuthorId = authorId
        };

        _bookRepository.Add(book);
        _bookRepository.SaveChanges();
        return book;
    }

    public bool DeleteBook(int id)
    {
        var book = _bookRepository.GetById(id);
        if (book is null)
            throw new NotFoundException($"Id-si {id} olan Book tapılmadı.");

        var isInUse = _reservedItemRepository.HasActiveReservation(id);
        if (isInUse)
            throw new BusinessRuleException("Bu kitab hazırda istifadədədir (aktiv rezervasiyası var), silinə bilməz.");

        _bookRepository.Delete(book);
        _bookRepository.SaveChanges();
        return true;
    }

    public Book? GetBookById(int id)
    {
        return _bookRepository.GetByIdWithDetails(id);
    }

    public List<Book> GetAllBooks()
    {
        return _bookRepository.GetAllWithAuthor();
    }

    public Book UpdateBook(int id, string name, int pageCount, int authorId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Kitabın adı boş ola bilməz.");

        if (pageCount <= 0)
            throw new ValidationException("Səhifə sayı müsbət ədəd olmalıdır.");

        var book = _bookRepository.GetById(id);
        if (book is null)
            throw new NotFoundException($"Id-si {id} olan Book tapılmadı.");

        var author = _authorRepository.GetById(authorId);
        if (author is null)
            throw new NotFoundException($"Id-si {authorId} olan Author tapılmadı.");

        book.Name = name.Trim();
        book.PageCount = pageCount;
        book.AuthorId = authorId;

        _bookRepository.Update(book);
        _bookRepository.SaveChanges();
        return book;
    }

    public List<Book> SearchBooks(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            throw new ValidationException("Axtarış açar sözü boş ola bilməz.");

        return _bookRepository.SearchByName(keyword.Trim());
    }

    public List<Book> GetAvailableBooks()
    {
        var allBooks = _bookRepository.GetAllWithAuthor();
        var activeBookIds = _reservedItemRepository.GetActiveBookIds();
        return allBooks.Where(b => !activeBookIds.Contains(b.Id)).ToList();
    }
}
