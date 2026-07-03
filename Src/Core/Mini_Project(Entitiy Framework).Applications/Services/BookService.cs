using Mini_Project_Entitiy_Framework_.Applications.Exceptions;
using Mini_Project_Entitiy_Framework_.Applications.İnterfaces.Repositories;
using Mini_Project_Entitiy_Framework_.Applications.İnterfaces.Services;


namespace OnlineLibrary.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly ReservedItemRepository _reservedItemRepository;

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

        // Mütləq: göndərilən AuthorId-nin mövcud olub olmadığını yoxlayırıq.
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
}
