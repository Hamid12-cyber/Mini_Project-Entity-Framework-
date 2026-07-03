using Microsoft.EntityFrameworkCore;
using Mini_Project_Entitiy_Framework_.Persistence.Data;
using Mini_Project_Entitiy_Framework_.Domain.Entities;


namespace Mini_Project_Entitiy_Framework_.Persistence.Implementation.Repositories;

public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
{
    public AuthorRepository(LibraryDbContext context) : base(context) { }

    public List<Author> GetAllWithBooks()
    {
        return DbSet
            .Include(a => a.Books)
            .ToList();
    }

    public Author? GetByIdWithBooks(int id)
    {
        return DbSet
            .Include(a => a.Books)
            .FirstOrDefault(a => a.Id == id);
    }
}
