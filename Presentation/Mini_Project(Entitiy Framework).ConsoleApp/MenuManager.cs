using Microsoft.Extensions.DependencyInjection;
using Mini_Project_Entitiy_Framework_.Application.Exceptions;
using Mini_Project_Entitiy_Framework_.Application.Interfaces.Services;
using Mini_Project_Entitiy_Framework_.Domain.Enums;
using System.Globalization;

namespace Mini_Project_Entitiy_Framework_.ConsoleApp;

public class MenuManager
{
    private readonly IServiceProvider _serviceProvider;
    private static bool _globalRunning = true;

    public MenuManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Run()
    {
        while (_globalRunning)
        {
            Console.Clear();
            PrintMenu();
            var choice = Console.ReadLine();

            if (choice == "0" || choice == "00")
            {
                _globalRunning = false;
                break;
            }

            try
            {
                switch (choice)
                {
                    case "1": CreateBook(_serviceProvider); break;
                    case "2": DeleteBook(_serviceProvider); break;
                    case "3": GetBookById(_serviceProvider); break;
                    case "4": ShowAllBooks(_serviceProvider); break;
                    case "5": CreateAuthor(_serviceProvider); break;
                    case "6": ShowAllAuthors(_serviceProvider); break;
                    case "7": ShowAuthorBooks(_serviceProvider); break;
                    case "8": ReserveBook(_serviceProvider); break;
                    case "9": ShowReservationList(_serviceProvider); break;
                    case "10": ChangeReservationStatus(_serviceProvider); break;
                    case "11": ShowUserReservations(_serviceProvider); break;
                    default:
                        Console.WriteLine("\nYanlış seçim, zəhmət olmasa yenidən cəhd edin.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGözlənilməz xəta baş verdi: {ex.Message}");
            }

            if (_globalRunning)
            {
                Console.WriteLine("\nMenyuya qayıtmaq üçün istənilən düyməyə basın...");
                Console.ReadKey();
            }
        }

        Console.Clear();
        Console.WriteLine("Proqram bitdi. Sağ olun!");
    }

    private static void PrintMenu()
    {
        Console.WriteLine(
@"=======================================
            ONLINE LIBRARY             
=======================================

 [ Book Services ]
 1.  Create Book
 2.  Delete Book
 3.  Get Book by Id
 4.  Show All Books
---------------------------------------

 [ Author Services ]
 5.  Create Author
 6.  Show All Authors
 7.  Show Author's Books
---------------------------------------

 [ Reservation Services ]
 8.  Reserve Book
 9.  Reservation List
 10. Change Reservation Status
 11. User's Reservations List


 0.  Exit

=======================================");
        Console.Write("Seçiminizi daxil edin: ");
    }
      

    private static int InputString(string label, out string result)
    {
        result = string.Empty;
        Console.WriteLine($"(Əvvəlki addım: 0, Ana Menyu: 00)");
        Console.Write($"{label}: ");
        var input = Console.ReadLine();

        if (input == "00") return -1;
        if (input == "0") return 0;

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine($"Xəta: {label} boş buraxıla bilməz!\n");
            Console.WriteLine("Davam etmək üçün istənilən düyməyə basın...");
            Console.ReadKey();
            return 2;
        }

        result = input.Trim();
        return 1;
    }

    private static int InputInt(string label, out int result, int min = int.MinValue, int max = int.MaxValue)
    {
        result = 0;
        Console.WriteLine($"(Əvvəlki addım: 0, Ana Menyu: 00)");
        Console.Write($"{label}: ");
        var input = Console.ReadLine();

        if (input == "00") return -1;
        if (input == "0") return 0;

        if (int.TryParse(input, out var value))
        {
            if (value >= min && value <= max)
            {
                result = value;
                return 1;
            }
            Console.WriteLine($"Xəta: Daxil edilən ədəd {min} ilə {max} arasında olmalıdır!\n");
            return 2;
        }

        Console.WriteLine("Xəta: Zəhmət olmasa düzgün rəqəm daxil edin!\n");
        return 2;
    }

    private static int InputDate(string label, out DateTime result)
    {
        result = DateTime.MinValue;
        Console.WriteLine($"(Əvvəlki addım: 0, Ana Menyu: 00)");
        Console.Write($"{label}: ");
        var input = Console.ReadLine();

        if (input == "00") return -1;
        if (input == "0") return 0;

        if (DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var value))
        {
            result = value;
            return 1;
        }

        Console.WriteLine("Xəta: Tarix formatı düzgün deyil (Nümunə: 25.12.2026)!\n");
        return 2;
    }


    private static void CreateBook(IServiceProvider provider)
    {
        var bookService = provider.GetRequiredService<IBookService>();
        string name = "";
        int pageCount = 0;
        int authorId = 0;

        int step = 1;
        while (step <= 3)
        {
            Console.Clear();
            Console.WriteLine("--- Yeni Kitab Yaradılması ---\n");

            if (step == 1)
            {
                int status = InputString("Kitabın adı", out name);
                if (status == -1 || status == 0) return;
                if (status == 1) step++;
            }
            else if (step == 2)
            {
                Console.WriteLine($"Kitabın adı: {name}");
                int status = InputInt("Səhifə sayı", out pageCount, min: 1);
                if (status == -1) return;
                if (status == 0) { step--; continue; }
                if (status == 1) step++;
            }
            else if (step == 3)
            {
                Console.WriteLine($"Kitabın adı: {name}");
                Console.WriteLine($"Səhifə sayı: {pageCount}");
                int status = InputInt("Author Id", out authorId);
                if (status == -1) return;
                if (status == 0) { step--; continue; }
                if (status == 1) step++;
            }
        }

        try
        {
            var book = bookService.CreateBook(name, pageCount, authorId);
            Console.WriteLine($"\nUğurlu: Kitab uğurla yaradıldı. Id: {book.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nXəta: {ex.Message}");
        }
    }

    private static void DeleteBook(IServiceProvider provider)
    {
        Console.Clear();
        Console.WriteLine("--- Kitabın Silinməsi ---\n");
        var bookService = provider.GetRequiredService<IBookService>();

        int id;
        while (true)
        {
            int status = InputInt("Silinəcək kitabın Id-si", out id);
            if (status == -1 || status == 0) return;
            if (status == 1) break;
        }

        try
        {
            bookService.DeleteBook(id);
            Console.WriteLine("\nUğurlu: Kitab uğurla silindi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nXəta: {ex.Message}");
        }
    }

    private static void GetBookById(IServiceProvider provider)
    {
        Console.Clear();
        Console.WriteLine("--- Kitab Axtarışı (Id ilə) ---\n");
        var bookService = provider.GetRequiredService<IBookService>();

        int id;
        while (true)
        {
            int status = InputInt("Kitabın Id-si", out id);
            if (status == -1 || status == 0) return;
            if (status == 1) break;
        }

        var book = bookService.GetBookById(id);
        if (book is null)
        {
            Console.WriteLine("\nBu Id-li kitab tapılmadı.");
            return;
        }

        Console.WriteLine($"\nId: {book.Id}");
        Console.WriteLine($"Ad: {book.Name}");
        Console.WriteLine($"Səhifə sayı: {book.PageCount}");
        Console.WriteLine($"Author: {book.Author?.Name} {book.Author?.Surname}".TrimEnd());
        Console.WriteLine("Rezervasiya tarixçəsi:");

        if (book.ReservedItems == null || book.ReservedItems.Count == 0)
        {
            Console.WriteLine("  Heç bir rezervasiya yoxdur.");
        }
        else
        {
            foreach (var r in book.ReservedItems)
            {
                Console.WriteLine($"  Id:{r.Id} FinCode:{r.FinCode} {r.StartDate:dd.MM.yyyy} - {r.EndDate:dd.MM.yyyy} Status:{r.Status}");
            }
        }
    }

    private static void ShowAllBooks(IServiceProvider provider)
    {
        Console.Clear();
        Console.WriteLine("--- Bütün Kitabların Siyahısı ---\n");
        var bookService = provider.GetRequiredService<IBookService>();
        var books = bookService.GetAllBooks();

        if (books.Count == 0)
        {
            Console.WriteLine("Heç bir kitab yoxdur.");
            return;
        }

        foreach (var b in books)
        {
            Console.WriteLine($"Id:{b.Id} | Ad:{b.Name} | Səhifə sayı:{b.PageCount} | Author:{b.Author?.Name} {b.Author?.Surname}".TrimEnd());
        }
    }


    private static void CreateAuthor(IServiceProvider provider)
    {
        var authorService = provider.GetRequiredService<IAuthorService>();
        string name = "";
        string? surname = null;
        int genderChoice = 0;

        int step = 1;
        while (step <= 3)
        {
            Console.Clear();
            Console.WriteLine("--- Yeni Müəllif Yaradılması ---\n");

            if (step == 1)
            {
                int status = InputString("Author-un adı", out name);
                if (status == -1 || status == 0) return;
                if (status == 1) step++;
            }
            else if (step == 2)
            {
                Console.WriteLine($"Author-un adı: {name}");
                Console.WriteLine($"(Əvvəlki addım: 0, Ana Menyu: 00)");
                Console.Write("Soyadı: ");
                var surnameInput = Console.ReadLine();

                if (surnameInput == "00") return;
                if (surnameInput == "0") { step--; continue; }

                surname = string.IsNullOrWhiteSpace(surnameInput) ? "XXX" : surnameInput;
                step++;
            }
            else if (step == 3)
            {
                Console.WriteLine($"Author-un adı: {name}");
                Console.WriteLine($"Soyadı: {surname ?? "(Boş)"}");
                Console.WriteLine("\nCins seçin: 1-Female 2-Male 3-Other 4-Unknown");
                int status = InputInt("Seçim", out genderChoice, min: 1, max: 4);
                if (status == -1) return;
                if (status == 0) { step--; continue; }
                if (status == 1) step++;
            }
        }

        var gender = genderChoice switch
        {
            1 => Gender.Female,
            2 => Gender.Male,
            3 => Gender.Other,
            _ => Gender.Unknown
        };

        try
        {
            var author = authorService.CreateAuthor(name, surname, gender);
            Console.WriteLine($"\nUğurlu: Author uğurla yaradıldı. Id: {author.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nXəta: {ex.Message}");
        }
    }

    private static void ShowAllAuthors(IServiceProvider provider)
    {
        Console.Clear();
        Console.WriteLine("--- Bütün Müəlliflərin Siyahısı ---\n");
        var authorService = provider.GetRequiredService<IAuthorService>();
        var authors = authorService.GetAllAuthors();

        if (authors.Count == 0)
        {
            Console.WriteLine("Heç bir author yoxdur.");
            return;
        }

        foreach (var a in authors)
        {
            Console.WriteLine($"Id:{a.Id} | Ad:{a.Name} {a.Surname} | Kitab sayı:{a.Books?.Count ?? 0}".TrimEnd());
        }
    }

    private static void ShowAuthorBooks(IServiceProvider provider)
    {
        Console.Clear();
        Console.WriteLine("--- Müəllifin Kitabları ---\n");
        var authorService = provider.GetRequiredService<IAuthorService>();

        int id;
        while (true)
        {
            int status = InputInt("Author Id", out id);
            if (status == -1 || status == 0) return;
            if (status == 1) break;
        }

        var books = authorService.GetAuthorBooks(id);
        if (books.Count == 0)
        {
            Console.WriteLine("\nBu author-un kitabı yoxdur.");
            return;
        }

        foreach (var b in books)
        {
            Console.WriteLine($"Id:{b.Id} | Ad:{b.Name} | Səhifə sayı:{b.PageCount}");
        }
    }

    private static void ReserveBook(IServiceProvider provider)
    {
        var reservationService = provider.GetRequiredService<IReservationService>();
        int bookId = 0;
        string finCode = "";
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MinValue;

        int step = 1;
        while (step <= 4)
        {
            Console.Clear();
            Console.WriteLine("--- Kitab Rezervasiyası ---\n");

            if (step == 1)
            {
                int status = InputInt("Kitab Id", out bookId);
                if (status == -1 || status == 0) return;
                if (status == 1) step++;
            }
            else if (step == 2)
            {
                Console.WriteLine($"Kitab Id: {bookId}");
                int status = InputString("FinCode", out finCode);
                if (status == -1) return;
                if (status == 0) { step--; continue; }
                if (status == 1) step++;
            }
            else if (step == 3)
            {
                Console.WriteLine($"Kitab Id: {bookId}");
                Console.WriteLine($"FinCode: {finCode}");
                int status = InputDate("Başlanğıc tarixi (dd.MM.yyyy)", out startDate);
                if (status == -1) return;
                if (status == 0) { step--; continue; }
                if (status == 1) step++;
            }
            else if (step == 4)
            {
                Console.WriteLine($"Kitab Id: {bookId}");
                Console.WriteLine($"FinCode: {finCode}");
                Console.WriteLine($"Başlanğıc tarixi: {startDate:dd.MM.yyyy}");
                int status = InputDate("Bitmə tarixi (dd.MM.yyyy)", out endDate);
                if (status == -1) return;
                if (status == 0) { step--; continue; }
                if (status == 1) step++;
            }
        }

        try
        {
            var reservation = reservationService.ReserveBook(bookId, finCode, startDate, endDate);
            Console.WriteLine($"\nUğurlu: Rezervasiya uğurla yaradıldı. Id: {reservation.Id}, Status: {reservation.Status}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nXəta: {ex.Message}");
        }
    }

    private static void ShowReservationList(IServiceProvider provider)
    {
        Console.Clear();
        Console.WriteLine("--- Bütün Rezervasiyaların Siyahısı ---\n");
        var reservationService = provider.GetRequiredService<IReservationService>();
        var reservations = reservationService.GetReservationList();

        if (reservations.Count == 0)
        {
            Console.WriteLine("Heç bir rezervasiya yoxdur.");
            return;
        }

        foreach (var r in reservations)
        {
            Console.WriteLine($"Id:{r.Id} | Kitab:{r.Book?.Name} | FinCode:{r.FinCode} | {r.StartDate:dd.MM.yyyy}-{r.EndDate:dd.MM.yyyy} | Status:{r.Status}");
        }
    }

    private static void ChangeReservationStatus(IServiceProvider provider)
    {
        var reservationService = provider.GetRequiredService<IReservationService>();
        int id = 0;
        int statusChoice = 0;

        int step = 1;
        while (step <= 2)
        {
            Console.Clear();
            Console.WriteLine("--- Rezervasiya Statusunun Dəyişdirilməsi ---\n");

            if (step == 1)
            {
                int status = InputInt("Reservation Id", out id);
                if (status == -1 || status == 0) return;
                if (status == 1) step++;
            }
            else if (step == 2)
            {
                Console.WriteLine($"Reservation Id: {id}");
                Console.WriteLine("\nYeni status seçin: 1-Confirmed 2-Started 3-Completed 4-Canceled");
                int status = InputInt("Seçim", out statusChoice, min: 1, max: 4);
                if (status == -1) return;
                if (status == 0) { step--; continue; }
                if (status == 1) step++;
            }
        }

        var statusEnum = statusChoice switch
        {
            1 => Status.Confirmed,
            2 => Status.Started,
            3 => Status.Completed,
            4 => Status.Canceled,
            _ => Status.Confirmed
        };

        try
        {
            reservationService.ChangeStatus(id, statusEnum);
            Console.WriteLine("\nUğurlu: Status uğurla dəyişdirildi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nXəta: {ex.Message}");
        }
    }

    private static void ShowUserReservations(IServiceProvider provider)
    {
        Console.Clear();
        Console.WriteLine("--- İstifadəçinin Rezervasiyaları ---\n");
        var reservationService = provider.GetRequiredService<IReservationService>();

        string finCode;
        while (true)
        {
            int status = InputString("FinCode", out finCode);
            if (status == -1 || status == 0) return;
            if (status == 1) break;
        }

        var reservations = reservationService.GetUserReservations(finCode);
        if (reservations.Count == 0)
        {
            Console.WriteLine("\nBu FinCode üçün rezervasiya tapılmadı.");
            return;
        }

        foreach (var r in reservations)
        {
            Console.WriteLine($"Id:{r.Id} | Kitab:{r.Book?.Name} | {r.StartDate:dd.MM.yyyy}-{r.EndDate:dd.MM.yyyy} | Status:{r.Status}");
        }
    }
}