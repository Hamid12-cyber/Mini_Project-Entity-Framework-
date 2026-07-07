using Mini_Project_Entitiy_Framework_.Domain.Entities;
namespace Mini_Project_Entitiy_Framework_.Application.Interfaces.Repositories
{
    public interface IReservedItemRepository : IGenericRepository<ReservedItem>
    {
        List<ReservedItem> GetAllWithBook();
        List<ReservedItem> GetByFinCode(string finCode);
        List<ReservedItem> GetActiveByFinCode(string finCode);
        List<ReservedItem> GetByBookId(int bookId);
        bool HasActiveReservation(int bookId);
        List<int> GetActiveBookIds();
        List<ReservedItem> GetOverdue();
    }
}
