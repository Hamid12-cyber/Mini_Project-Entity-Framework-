using Microsoft.EntityFrameworkCore;
using Mini_Project_Entitiy_Framework_.Persistence.Data;
using Mini_Project_Entitiy_Framework_.Persistence.Implementation.Common;
using Mini_Project_Entitiy_Framework_.Persistence.JsonServices;
using Mini_Project_Entitiy_Framework_.Application.Interfaces.Repositories;
using Mini_Project_Entitiy_Framework_.Domain.Entities;

namespace Mini_Project_Entitiy_Framework_.Persistence.Implementation.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(LibraryDbContext context) : base(context, new BookJsonRepository()) { }

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

    public List<Book> SearchByName(string keyword)
    {
        return DbSet
            .Include(b => b.Author)
            .Where(b => b.Name.Contains(keyword))
            .ToList();
    }

    // JSON backup-a həm Author, həm də ReservedItems (tam naviqasiya) daxil olsun.
    protected override List<Book> GetDataForBackup()
    {
        return DbSet
            .Include(b => b.Author)
            .Include(b => b.ReservedItems)
            .ToList();
    }
}
