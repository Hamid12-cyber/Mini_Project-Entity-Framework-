using Mini_Project_Entitiy_Framework_.Domain.Entities;
using Mini_Project_Entitiy_Framework_.Domain.Enums;
using Mini_Project_Entitiy_Framework_.Application.Exceptions;
using Mini_Project_Entitiy_Framework_.Application.Interfaces.Repositories;
using Mini_Project_Entitiy_Framework_.Application.Interfaces.Services;

namespace Mini_Project_Entitiy_Framework_.Application.Services;

public class ReservationService : IReservationService
{
    private const int MaxActiveReservationsPerFinCode = 3;

    private readonly IReservedItemRepository _reservedItemRepository;
    private readonly IBookRepository _bookRepository;

    public ReservationService(IReservedItemRepository reservedItemRepository, IBookRepository bookRepository)
    {
        _reservedItemRepository = reservedItemRepository;
        _bookRepository = bookRepository;
    }

    public ReservedItem ReserveBook(int bookId, string finCode, DateTime startDate, DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(finCode))
            throw new ValidationException("FinCode boş ola bilməz.");

        var book = _bookRepository.GetById(bookId);
        if (book is null)
            throw new NotFoundException($"Id-si {bookId} olan Book tapılmadı.");

        // Tarix yoxlamaları
        if (startDate.Date < DateTime.Today)
            throw new ValidationException("Başlanğıc tarixi bugündən əvvəl ola bilməz.");

        if (endDate.Date <= startDate.Date)
            throw new ValidationException("Bitmə tarixi başlanğıc tarixindən sonra olmalıdır.");

        // Kitab seçilmiş tarix aralığında artıq aktiv şəkildə rezerv olunubsa, icazə vermirik.
        var bookReservations = _reservedItemRepository.GetByBookId(bookId);
        var hasOverlap = bookReservations.Any(r =>
            (r.Status == Status.Confirmed || r.Status == Status.Started) &&
            startDate.Date <= r.EndDate.Date && endDate.Date >= r.StartDate.Date);

        if (hasOverlap)
            throw new BusinessRuleException("Bu kitab seçdiyiniz tarix aralığında artıq rezerv olunub.");

        // Optional qayda: bir FinCode eyni anda maksimum 3 kitabı icarəyə götürə bilər.
        var activeUserReservations = _reservedItemRepository.GetActiveByFinCode(finCode);
        if (activeUserReservations.Count >= MaxActiveReservationsPerFinCode)
            throw new BusinessRuleException(
                $"Bu FinCode ilə eyni anda maksimum {MaxActiveReservationsPerFinCode} kitab icarəyə götürülə bilər.");

        var reservedItem = new ReservedItem
        {
            FinCode = finCode.Trim(),
            StartDate = startDate.Date,
            EndDate = endDate.Date,
            BookId = bookId,
            Status = Status.Confirmed
        };

        _reservedItemRepository.Add(reservedItem);
        _reservedItemRepository.SaveChanges();
        return reservedItem;
    }

    public List<ReservedItem> GetReservationList()
    {
        var items = _reservedItemRepository.GetAllWithBook();
        return items.OrderBy(r => r.Status).ToList();
    }

    public bool ChangeStatus(int reservationId, Status newStatus)
    {
        var reservation = _reservedItemRepository.GetById(reservationId);
        if (reservation is null)
            throw new NotFoundException($"Id-si {reservationId} olan Reservation tapılmadı.");

        if (reservation.Status == Status.Canceled)
            throw new BusinessRuleException("Ləğv edilmiş (Canceled) rezervasiyanın statusu dəyişdirilə bilməz.");

        reservation.Status = newStatus;
        _reservedItemRepository.Update(reservation);
        _reservedItemRepository.SaveChanges();
        return true;
    }

    public List<ReservedItem> GetUserReservations(string finCode)
    {
        if (string.IsNullOrWhiteSpace(finCode))
            throw new ValidationException("FinCode boş ola bilməz.");

        return _reservedItemRepository.GetByFinCode(finCode);
    }
}
