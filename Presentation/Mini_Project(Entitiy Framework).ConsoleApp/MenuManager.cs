using Microsoft.Extensions.DependencyInjection;
using OnlineLibrary.Application.Exceptions;
using OnlineLibrary.Application.Interfaces.Services;
using OnlineLibrary.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Reflection;

namespace OnlineLibrary.ConsoleApp;

public class MenuManager
{
    private readonly IServiceProvider _serviceProvider;

    public MenuManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Run()
    {
        var isRunning = true;

        while (isRunning)
        {
            PrintMenu();
            var choice = Console.ReadLine();

            using var scope = _serviceProvider.CreateScope();

            try
            {
                switch (choice)
                {
                    case "1": CreateBook(scope); break;
                    case "2": DeleteBook(scope); break;
                    case "3": GetBookById(scope); break;
                    case "4": ShowAllBooks(scope); break;
                    case "5": CreateAuthor(scope); break;
                    case "6": ShowAllAuthors(scope); break;
                    case "7": ShowAuthorBooks(scope); break;
                    case "8": ReserveBook(scope); break;
                    case "9": ShowReservationList(scope); break;
                    case "10": ChangeReservationStatus(scope); break;
                    case "11": ShowUserReservations(scope); break;
                    case "0": isRunning = false; break;
                    default:
                        Console.WriteLine("Yanlış seçim, zəhmət olmasa yenidən cəhd edin.");
                        break;
                }
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine($"Xəta: {ex.Message}");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Xəta: {ex.Message}");
            }
            catch (BusinessRuleException ex)
            {
                Console.WriteLine($"Xəta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gözlənilməz xəta baş verdi: {ex.Message}");
            }

            if (isRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Davam etmək üçün istənilən düyməyə basın...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("===================================");
        Console.WriteLine("         ONLINE LIBRARY");
        Console.WriteLine("===================================");
        Console.WriteLine("1.  Create Book");
        Console.WriteLine("2.  Delete Book");
        Console.WriteLine("3.  Get Book by Id");
        Console.WriteLine("4.  Show All Books");
        Console.WriteLine("5.  Create Author");
        Console.WriteLine("6.  Show All Authors");
        Console.WriteLine("7.  Show Author's Books");
        Console.WriteLine("8.  Reserve Book");
        Console.WriteLine("9.  Reservation List");
        Console.WriteLine("10. Change Reservation Status");
        Console.WriteLine("11. User's Reservations List");
        Console.WriteLine("0.  Exit");
        Console.Write("Seçiminizi daxil edin: ");
    }

    private static void CreateBook(IServiceScope scope)
    {
        var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();

        Console.Write("Kitabın adı: ");
        var name = Console.ReadLine() ?? string.Empty;

        Console.Write("Səhifə sayı: ");
        var pageCount = ReadInt();

        Console.Write("Author Id: ");
        var authorId = ReadInt();

        var book = bookService.CreateBook(name, pageCount, authorId);
        Console.WriteLine($"Kitab uğurla yaradıldı. Id: {book.Id}");
    }

    private static void DeleteBook(IServiceScope scope)
    {
        var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();

        Console.Write("Silinəcək kitabın Id-si: ");
        var id = ReadInt();

        bookService.DeleteBook(id);
        Console.WriteLine("Kitab uğurla silindi.");
    }

    private static void GetBookById(IServiceScope scope)
    {
        var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();

        Console.Write("Kitabın Id-si: ");
        var id = ReadInt();

        var book = bookService.GetBookById(id);
        if (book is null)
        {
            Console.WriteLine("Bu Id-li kitab tapılmadı.");
            return;
        }

        Console.WriteLine($"Id: {book.Id}");
        Console.WriteLine($"Ad: {book.Name}");
        Console.WriteLine($"Səhifə sayı: {book.PageCount}");
        Console.WriteLine($"Author: {book.Author.Name} {book.Author.Surname}".TrimEnd());
        Console.WriteLine("Rezervasiya tarixçəsi:");

        if (book.ReservedItems.Count == 0)
        {
            Console.WriteLine("  Heç bir rezervasiya yoxdur.");
        }
        else
        {
            foreach (var r in book.ReservedItems)
            {
                Console.WriteLine(
                    $"  Id:{r.Id} FinCode:{r.FinCode} {r.StartDate:dd.MM.yyyy} - {r.EndDate:dd.MM.yyyy} Status:{r.Status}");
            }
        }
    }

    private static void ShowAllBooks(IServiceScope scope)
    {
        var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();
        var books = bookService.GetAllBooks();

        if (books.Count == 0)
        {
            Console.WriteLine("Heç bir kitab yoxdur.");
            return;
        }

        foreach (var b in books)
        {
            Console.WriteLine($"Id:{b.Id} | Ad:{b.Name} | Səhifə sayı:{b.PageCount} | Author:{b.Author.Name} {b.Author.Surname}".TrimEnd());
        }
    }

    private static void CreateAuthor(IServiceScope scope)
    {
        var authorService = scope.ServiceProvider.GetRequiredService<IAuthorService>();

        Console.Write("Author-un adı: ");
        var name = Console.ReadLine() ?? string.Empty;

        Console.Write("Soyadı (boş buraxıla bilər): ");
        var surnameInput = Console.ReadLine();
        var surname = string.IsNullOrWhiteSpace(surnameInput) ? null : surnameInput;

        Console.WriteLine("Cins seçin: 1-Female 2-Male 3-Other 4-Unknown");
        Console.Write("Seçim: ");
        var genderChoice = ReadInt();
        var gender = genderChoice switch
        {
            1 => Gender.Female,
            2 => Gender.Male,
            3 => Gender.Other,
            _ => Gender.Unknown
        };

        var author = authorService.CreateAuthor(name, surname, gender);
        Console.WriteLine($"Author uğurla yaradıldı. Id: {author.Id}");
    }

    private static void ShowAllAuthors(IServiceScope scope)
    {
        var authorService = scope.ServiceProvider.GetRequiredService<IAuthorService>();
        var authors = authorService.GetAllAuthors();

        if (authors.Count == 0)
        {
            Console.WriteLine("Heç bir author yoxdur.");
            return;
        }

        foreach (var a in authors)
        {
            Console.WriteLine($"Id:{a.Id} | Ad:{a.Name} {a.Surname} | Kitab sayı:{a.Books.Count}".TrimEnd());
        }
    }

    private static void ShowAuthorBooks(IServiceScope scope)
    {
        var authorService = scope.ServiceProvider.GetRequiredService<IAuthorService>();

        Console.Write("Author Id: ");
        var id = ReadInt();

        var books = authorService.GetAuthorBooks(id);
        if (books.Count == 0)
        {
            Console.WriteLine("Bu author-un kitabı yoxdur.");
            return;
        }

        foreach (var b in books)
        {
            Console.WriteLine($"Id:{b.Id} | Ad:{b.Name} | Səhifə sayı:{b.PageCount}");
        }
    }

    private static void ReserveBook(IServiceScope scope)
    {
        var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();

        Console.Write("Kitab Id: ");
        var bookId = ReadInt();

        Console.Write("FinCode: ");
        var finCode = Console.ReadLine() ?? string.Empty;

        Console.Write("Başlanğıc tarixi (dd.MM.yyyy): ");
        var startDate = ReadDate();

        Console.Write("Bitmə tarixi (dd.MM.yyyy): ");
        var endDate = ReadDate();

        var reservation = reservationService.ReserveBook(bookId, finCode, startDate, endDate);
        Console.WriteLine($"Rezervasiya uğurla yaradıldı. Id: {reservation.Id}, Status: {reservation.Status}");
    }

    private static void ShowReservationList(IServiceScope scope)
    {
        var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();
        var reservations = reservationService.GetReservationList();

        if (reservations.Count == 0)
        {
            Console.WriteLine("Heç bir rezervasiya yoxdur.");
            return;
        }

        foreach (var r in reservations)
        {
            Console.WriteLine(
                $"Id:{r.Id} | Kitab:{r.Book.Name} | FinCode:{r.FinCode} | {r.StartDate:dd.MM.yyyy}-{r.EndDate:dd.MM.yyyy} | Status:{r.Status}");
        }
    }

    private static void ChangeReservationStatus(IServiceScope scope)
    {
        var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();

        Console.Write("Reservation Id: ");
        var id = ReadInt();

        Console.WriteLine("Yeni status seçin: 1-Confirmed 2-Started 3-Completed 4-Canceled");
        Console.Write("Seçim: ");
        var statusChoice = ReadInt();
        var status = statusChoice switch
        {
            1 => Status.Confirmed,
            2 => Status.Started,
            3 => Status.Completed,
            4 => Status.Canceled,
            _ => throw new ValidationException("Yanlış status seçimi.")
        };

        reservationService.ChangeStatus(id, status);
        Console.WriteLine("Status uğurla dəyişdirildi.");
    }

    private static void ShowUserReservations(IServiceScope scope)
    {
        var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();

        Console.Write("FinCode: ");
        var finCode = Console.ReadLine() ?? string.Empty;

        var reservations = reservationService.GetUserReservations(finCode);
        if (reservations.Count == 0)
        {
            Console.WriteLine("Bu FinCode üçün rezervasiya tapılmadı.");
            return;
        }

        foreach (var r in reservations)
        {
            Console.WriteLine(
                $"Id:{r.Id} | Kitab:{r.Book.Name} | {r.StartDate:dd.MM.yyyy}-{r.EndDate:dd.MM.yyyy} | Status:{r.Status}");
        }
    }

    private static int ReadInt()
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (int.TryParse(input, out var value))
                return value;

            Console.Write("Zəhmət olmasa düzgün rəqəm daxil edin: ");
        }
    }

    private static DateTime ReadDate()
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var value))
                return value;

            Console.Write("Zəhmət olmasa düzgün formatda tarix daxil edin (dd.MM.yyyy): ");
        }
    }
}
