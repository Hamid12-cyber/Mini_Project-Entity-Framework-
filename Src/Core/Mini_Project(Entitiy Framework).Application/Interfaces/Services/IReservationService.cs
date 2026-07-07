using Mini_Project_Entitiy_Framework_.Domain.Entities;
using Mini_Project_Entitiy_Framework_.Domain.Enums;

namespace Mini_Project_Entitiy_Framework_.Application.Interfaces.Services
{
    public interface IReservationService
    {
        ReservedItem ReserveBook(int bookId, string finCode, DateTime startDate, DateTime endDate);       
        List<ReservedItem> GetReservationList();
        bool ChangeStatus(int reservationId, Status newStatus);
        List<ReservedItem> GetUserReservations(string finCode);
        bool CancelReservation(int reservationId);
        List<ReservedItem> GetOverdueReservations();
        List<(Book Book, int Count)> GetMostReservedBooks(int topN = 5);
    }
}
