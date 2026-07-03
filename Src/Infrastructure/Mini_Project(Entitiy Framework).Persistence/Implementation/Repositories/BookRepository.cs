using Microsoft.EntityFrameworkCore;
using Mini_Project_Entitiy_Framework_.Persistence.Data;
using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Domain.Entities;
using OnlineLibrary.Infrastructure.Persistence;

namespace Mini_Project_Entitiy_Framework_.Persistence.Implementation.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(LibraryDbContext context) : base(context) { }

    public Book? GetByIdWithDetails(int id)
    {
        return DbSet
            .Include(b => b.Author)
            .Include(b => b.ReservedItems)
            .FirstOrDefault(b => b.Id == id);
    }

    public List<Book> GetAllWithAuthor()
    {
        return DbSet
            .Include(b => b.Author)
            .ToList();
    }

    public List<Book> GetBooksByAuthorId(int authorId)
    {
        return DbSet
            .Where(b => b.AuthorId == authorId)
            .ToList();
    }
}
