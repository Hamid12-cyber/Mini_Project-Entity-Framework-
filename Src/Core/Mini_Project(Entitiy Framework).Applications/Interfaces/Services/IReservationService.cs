
using System.Net.NetworkInformation;

namespace Mini_Project_Entitiy_Framework_.Applications.İnterfaces.Services
{
    internal interface IReservationService
    {
        ReservedItem ReserveBook(int bookId, string finCode, DateTime startDate, DateTime endDate);
        List<ReservedItem> GetReservationList();
        bool ChangeStatus(int reservationId, Status newStatus);
        List<ReservedItem> GetUserReservations(string finCode);
    }
}
