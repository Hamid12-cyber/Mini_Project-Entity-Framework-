using Microsoft.EntityFrameworkCore;
using Mini_Project_Entitiy_Framework_.Persistence.Data;
using Mini_Project_Entitiy_Framework_.Persistence.Implementation.Common;
using Mini_Project_Entitiy_Framework_.Application.Interfaces.Repositories;
using Mini_Project_Entitiy_Framework_.Domain.Entities;
using Mini_Project_Entitiy_Framework_.Domain.Enums;

namespace Mini_Project_Entitiy_Framework_.Persistence.Implementation.Repositories;

public class ReservedItemRepository : GenericRepository<ReservedItem>, IReservedItemRepository
{
    public ReservedItemRepository(LibraryDbContext context) : base(context) { }

    public List<ReservedItem> GetAllWithBook()
    {
        return DbSet
            .Include(r => r.Book)
            .ToList();
    }

    public List<ReservedItem> GetByFinCode(string finCode)
    {
        return DbSet
            .Include(r => r.Book)
            .Where(r => r.FinCode == finCode)
            .ToList();
    }

    public List<ReservedItem> GetActiveByFinCode(string finCode)
    {
        return DbSet
            .Where(r => r.FinCode == finCode &&
                        (r.Status == Status.Confirmed || r.Status == Status.Started))
            .ToList();
    }

    public List<ReservedItem> GetByBookId(int bookId)
    {
        return DbSet
            .Where(r => r.BookId == bookId)
            .ToList();
    }

    public bool HasActiveReservation(int bookId)
    {
        return DbSet.Any(r =>
            r.BookId == bookId &&
            (r.Status == Status.Confirmed || r.Status == Status.Started));
    }
}
