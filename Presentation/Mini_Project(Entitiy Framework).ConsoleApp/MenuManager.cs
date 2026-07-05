using Microsoft.Extensions.DependencyInjection;
using Mini_Project_Entitiy_Framework_.Application.Exceptions;
using Mini_Project_Entitiy_Framework_.Application.Interfaces.Services;
using Mini_Project_Entitiy_Framework_.ConsoleApp.Animations;
using Mini_Project_Entitiy_Framework_.Domain.Entities;
using Mini_Project_Entitiy_Framework_.Domain.Enums;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mini_Project_Entitiy_Framework_.ConsoleApp;

public class MenuManager
{
    private readonly IServiceProvider _serviceProvider;
    private static bool _globalRunning = true;

    private static readonly string[] MenuItems =
    {
        "1.  Create Book",
        "2.  Delete Book",
        "3.  Get Book by Id",
        "4.  Show All Books",
        "5.  Create Author",
        "6.  Show All Authors",
        "7.  Show Author's Books",
        "8.  Reserve Book",
        "9.  Reservation List",
        "10. Change Reservation Status",
        "11. User's Reservations List",
        "0.  Exit"
    };

    public MenuManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Run()
    {
        ConsoleAnimation.SplashScreen("ONLINE LIBRARY");

        while (_globalRunning)
        {
            ConsoleAnimation.PrintMenu("ONLINE LIBRARY", MenuItems);
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
                    case "1": Console.Clear(); ConsoleAnimation.Loading("Kitab yaradılır"); CreateBook(_serviceProvider); break;
                    case "2": Console.Clear(); ConsoleAnimation.Loading("Silmə paneli"); DeleteBook(_serviceProvider); break;
                    case "3": Console.Clear(); ConsoleAnimation.Loading("Axtarılır", 400); GetBookById(_serviceProvider); break;
                    case "4": Console.Clear(); ConsoleAnimation.Loading("Kitablar yüklənir"); ShowAllBooks(_serviceProvider); break;
                    case "5": Console.Clear(); ConsoleAnimation.Loading("Müəllif yaradılır"); CreateAuthor(_serviceProvider); break;
                    case "6": Console.Clear(); ConsoleAnimation.Loading("Müəlliflər yüklənir"); ShowAllAuthors(_serviceProvider); break;
                    case "7": Console.Clear(); ConsoleAnimation.Loading("Kitablar yüklənir"); ShowAuthorBooks(_serviceProvider); break;
                    case "8": Console.Clear(); ConsoleAnimation.Loading("Rezervasiya paneli"); ReserveBook(_serviceProvider); break;
                    case "9": Console.Clear(); ConsoleAnimation.Loading("Siyahı yüklənir"); ShowReservationList(_serviceProvider); break;
                    case "10": Console.Clear(); ConsoleAnimation.Loading("Status paneli"); ChangeReservationStatus(_serviceProvider); break;
                    case "11": Console.Clear(); ConsoleAnimation.Loading("Tarixçə yüklənir"); ShowUserReservations(_serviceProvider); break;
                    default:
                        ConsoleAnimation.Warning("Yanlış seçim, zəhmət olmasa yenidən cəhd edin.");
                        break;
                }
            }
            catch (Exception ex)
            {
                ConsoleAnimation.Error($"Gözlənilməz xəta baş verdi: {ex.Message}");
            }

            if (_globalRunning)
            {
                ConsoleAnimation.Write("\n  [ Enter - menyuya qayıt ]", ConsoleColor.DarkRed);
                Console.ReadLine();
            }
        }

        Console.Clear();
        ConsoleAnimation.Print("Proqram bitdi. Sağ olun!", ConsoleColor.White);
    }

    // --- DAXİLETME METODLARI ---
    // 1 = Düzgün məlumat, 0 = Bir addım geriyə (0), -1 = Ana menyu (00)

    private static void PauseOnError()
    {
        ConsoleAnimation.Write("Davam etmək üçün istənilən düyməyə basın...", ConsoleColor.Yellow);
        Console.ReadKey();
    }

    private static int InputString(string label, out string result)
    {
        result = string.Empty;
        ConsoleAnimation.Print("(Əvvəlki addım: 0, Ana Menyu: 00)", ConsoleColor.Yellow);
        ConsoleAnimation.Write($"{label}: ", ConsoleColor.White);
        var input = Console.ReadLine();

        if (input == "00") return -1;
        if (input == "0") return 0;

        if (string.IsNullOrWhiteSpace(input))
        {
            ConsoleAnimation.Error($"{label} boş buraxıla bilməz!");
            PauseOnError();
            return 2;
        }

        result = input.Trim();
        return 1;
    }

    private static int InputFinCode(string label, out string result)
    {
        result = string.Empty;
        ConsoleAnimation.Print("(Əvvəlki addım: 0, Ana Menyu: 00)", ConsoleColor.Yellow);
        ConsoleAnimation.Write($"{label}: ", ConsoleColor.White);
        var input = Console.ReadLine();

        if (input == "00") return -1;
        if (input == "0") return 0;

        if (string.IsNullOrWhiteSpace(input))
        {
            ConsoleAnimation.Error($"{label} boş buraxıla bilməz!");
            PauseOnError();
            return 2;
        }

        input = input.Trim();

        if (input.Length != 7)
        {
            ConsoleAnimation.Error("FinCode tam 7 simvoldan ibarət olmalıdır!");
            PauseOnError();
            return 2;
        }

        if (!Regex.IsMatch(input, "^[a-zA-Z0-9]+$"))
        {
            ConsoleAnimation.Error("FinCode yalnız Latın hərfləri (a-z, A-Z) və rəqəmlərdən (0-9) ibarət ola bilər!");
            PauseOnError();
            return 2;
        }

        result = input;
        return 1;
    }

    private static int InputInt(string label, out int result, int min = int.MinValue, int max = int.MaxValue)
    {
        result = 0;
        ConsoleAnimation.Print("(Əvvəlki addım: 0, Ana Menyu: 00)", ConsoleColor.Yellow);
        ConsoleAnimation.Write($"{label}: ", ConsoleColor.White);
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
            ConsoleAnimation.Error($"Daxil edilən ədəd {min} ilə {max} arasında olmalıdır!");
            PauseOnError();
            return 2;
        }

        ConsoleAnimation.Error("Zəhmət olmasa düzgün rəqəm daxil edin!");
        PauseOnError();
        return 2;
    }

    private static int InputDate(string label, out DateTime result)
    {
        result = DateTime.MinValue;
        ConsoleAnimation.Print("(Əvvəlki addım: 0, Ana Menyu: 00)", ConsoleColor.Yellow);
        ConsoleAnimation.Write($"{label}: ", ConsoleColor.White);
        var input = Console.ReadLine();

        if (input == "00") return -1;
        if (input == "0") return 0;

        var parts = input?.Split('.');
        if (parts != null && parts.Length == 3
            && int.TryParse(parts[0], out var day)
            && int.TryParse(parts[1], out var month)
            && int.TryParse(parts[2], out _))
        {
            if (month < 1 || month > 12)
            {
                ConsoleAnimation.Error("Ay 1 ilə 12 arasında olmalıdır!");
                PauseOnError();
                return 2;
            }
            if (day < 1 || day > 31)
            {
                ConsoleAnimation.Error("Gün 1 ilə 31 arasında olmalıdır!");
                PauseOnError();
                return 2;
            }
        }

        if (DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var value))
        {
            result = value;
            return 1;
        }

        ConsoleAnimation.Error("Tarix formatı düzgün deyil (Nümunə: 25.12.2026)!");
        PauseOnError();
        return 2;
    }

    // --- KİTAB METODLARI ---

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
            ConsoleAnimation.Print("--- Yeni Kitab Yaradılması ---\n", ConsoleColor.Yellow);

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
            ConsoleAnimation.Success($"Kitab uğurla yaradıldı. Id: {book.Id}");
        }
        catch (Exception ex)
        {
            ConsoleAnimation.Error(ex.Message);
        }
    }

    private static void DeleteBook(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Kitabın Silinməsi ---\n", ConsoleColor.Yellow);
        var bookService = provider.GetRequiredService<IBookService>();

        var books = bookService.GetAllBooks();
        if (books.Count == 0)
        {
            ConsoleAnimation.Warning("Heç bir kitab yoxdur.");
            return;
        }

        Book.PrintHeader();
        foreach (var b in books) b.PrintInfo();
        Book.PrintFooter();
        Console.WriteLine();

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
            ConsoleAnimation.Success("Kitab uğurla silindi.");
        }
        catch (Exception ex)
        {
            ConsoleAnimation.Error(ex.Message);
        }
    }

    private static void GetBookById(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Kitab Axtarışı (Id ilə) ---\n", ConsoleColor.Yellow);
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
            ConsoleAnimation.Warning("Bu Id-li kitab tapılmadı.");
            return;
        }

        Book.PrintHeader();
        book.PrintInfo();
        Book.PrintFooter();

        ConsoleAnimation.Print("\nRezervasiya tarixçəsi:", ConsoleColor.Yellow);
        if (book.ReservedItems == null || book.ReservedItems.Count == 0)
        {
            Console.WriteLine("  Heç bir rezervasiya yoxdur.");
        }
        else
        {
            ReservedItem.PrintHeader();
            foreach (var r in book.ReservedItems) r.PrintInfo();
            ReservedItem.PrintFooter();
        }
    }

    private static void ShowAllBooks(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Bütün Kitabların Siyahısı ---\n", ConsoleColor.Yellow);
        var bookService = provider.GetRequiredService<IBookService>();
        var books = bookService.GetAllBooks();

        if (books.Count == 0)
        {
            ConsoleAnimation.Warning("Heç bir kitab yoxdur.");
            return;
        }

        Book.PrintHeader();
        foreach (var b in books) b.PrintInfo();
        Book.PrintFooter();
    }

    // --- MÜƏLLİF METODLARI ---

    private static void CreateAuthor(IServiceProvider provider)
    {
        var authorService = provider.GetRequiredService<IAuthorService>();
        string name = "";
        string surname = "";
        int genderChoice = 0;

        int step = 1;
        while (step <= 3)
        {
            Console.Clear();
            ConsoleAnimation.Print("--- Yeni Müəllif Yaradılması ---\n", ConsoleColor.Yellow);

            if (step == 1)
            {
                int status = InputString("Author-un adı", out name);
                if (status == -1 || status == 0) return;
                if (status == 1) step++;
            }
            else if (step == 2)
            {
                Console.WriteLine($"Author-un adı: {name}");
                ConsoleAnimation.Print("(Əvvəlki addım: 0, Ana Menyu: 00)", ConsoleColor.Yellow);
                ConsoleAnimation.Write("Soyadı (boş buraxıla bilər): ", ConsoleColor.White);
                var surnameInput = Console.ReadLine();

                if (surnameInput == "00") return;
                if (surnameInput == "0") { step--; continue; }

                surname = string.IsNullOrWhiteSpace(surnameInput) ? "XXX" : surnameInput;
                step++;
            }
            else if (step == 3)
            {
                Console.WriteLine($"Author-un adı: {name}");
                Console.WriteLine($"Soyadı: {surname}");
                ConsoleAnimation.Print("\nCins seçin: 1-Female 2-Male 3-Other 4-Unknown", ConsoleColor.Yellow);
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
            ConsoleAnimation.Success($"Author uğurla yaradıldı. Id: {author.Id}");
        }
        catch (Exception ex)
        {
            ConsoleAnimation.Error(ex.Message);
        }
    }

    private static void ShowAllAuthors(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Bütün Müəlliflərin Siyahısı ---\n", ConsoleColor.Yellow);
        var authorService = provider.GetRequiredService<IAuthorService>();
        var authors = authorService.GetAllAuthors();

        if (authors.Count == 0)
        {
            ConsoleAnimation.Warning("Heç bir author yoxdur.");
            return;
        }

        Author.PrintHeader();
        foreach (var a in authors) a.PrintInfo();
        Author.PrintFooter();
    }

    private static void ShowAuthorBooks(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Müəllifin Kitabları ---\n", ConsoleColor.Yellow);
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
            ConsoleAnimation.Warning("Bu author-un kitabı yoxdur.");
            return;
        }

        Book.PrintHeader();
        foreach (var b in books) b.PrintInfo();
        Book.PrintFooter();
    }

    // --- REZERVASIYA METODLARI ---

    private static void ReserveBook(IServiceProvider provider)
    {
        var reservationService = provider.GetRequiredService<IReservationService>();
        var bookService = provider.GetRequiredService<IBookService>();

        int bookId = 0;
        string finCode = "";
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MinValue;

        int step = 1;
        while (step <= 4)
        {
            Console.Clear();
            ConsoleAnimation.Print("--- Kitab Rezervasiyası ---\n", ConsoleColor.Yellow);

            if (step == 1)
            {
                var books = bookService.GetAllBooks();
                if (books.Count == 0)
                {
                    ConsoleAnimation.Warning("Rezerv ediləcək kitab yoxdur.");
                    return;
                }
                Book.PrintHeader();
                foreach (var b in books) b.PrintInfo();
                Book.PrintFooter();
                Console.WriteLine();

                int status = InputInt("Kitab Id", out bookId);
                if (status == -1 || status == 0) return;
                if (status == 1) step++;
            }
            else if (step == 2)
            {
                Console.WriteLine($"Kitab Id: {bookId}");
                int status = InputFinCode("FinCode", out finCode);
                if (status == -1) return;
                if (status == 0) { step--; continue; }
                if (status == 1) step++;
            }
            else if (step == 3)
            {
                Console.WriteLine($"Kitab Id: {bookId}");
                Console.WriteLine($"FinCode: {finCode}");
                Console.WriteLine();

                var selectedBook = bookService.GetBookById(bookId);
                var activeReservations = selectedBook?.ReservedItems?
                    .Where(r => r.Status == Status.Confirmed || r.Status == Status.Started)
                    .OrderBy(r => r.StartDate)
                    .ToList();

                if (activeReservations is { Count: > 0 })
                {
                    ConsoleAnimation.Print($"\"{selectedBook!.Name}\" kitabının tutulu olan tarixləri:", ConsoleColor.Red);
                    ReservedItem.PrintHeader();
                    foreach (var r in activeReservations) r.PrintInfo();
                    ReservedItem.PrintFooter();
                    ConsoleAnimation.Print("Bu aralıqlardan başqa boş tarix seçin.\n", ConsoleColor.Yellow);
                }
                else
                {
                    ConsoleAnimation.Print($"\"{selectedBook?.Name}\" kitabı hazırda tamamilə boşdur.\n", ConsoleColor.Yellow);
                }

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
            ConsoleAnimation.Success($"Rezervasiya uğurla yaradıldı. Id: {reservation.Id}, Status: {reservation.Status}");
        }
        catch (Exception ex)
        {
            ConsoleAnimation.Error(ex.Message);
        }
    }

    private static void ShowReservationList(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Bütün Rezervasiyaların Siyahısı ---\n", ConsoleColor.Yellow);
        var reservationService = provider.GetRequiredService<IReservationService>();
        var reservations = reservationService.GetReservationList();

        if (reservations.Count == 0)
        {
            ConsoleAnimation.Warning("Heç bir rezervasiya yoxdur.");
            return;
        }

        ReservedItem.PrintHeader();
        foreach (var r in reservations) r.PrintInfo();
        ReservedItem.PrintFooter();
    }

    private static void ChangeReservationStatus(IServiceProvider provider)
    {
        var reservationService = provider.GetRequiredService<IReservationService>();
        var reservations = reservationService.GetReservationList();
        int id = 0;
        int statusChoice = 0;
        ReservedItem? selected = null;

        int step = 1;
        while (step <= 2)
        {
            Console.Clear();
            ConsoleAnimation.Print("--- Rezervasiya Statusunun Dəyişdirilməsi ---\n", ConsoleColor.Yellow);

            if (step == 1)
            {
                if (reservations.Count == 0)
                {
                    ConsoleAnimation.Warning("Heç bir rezervasiya yoxdur.");
                    return;
                }

                ReservedItem.PrintHeader();
                foreach (var r in reservations) r.PrintInfo();
                ReservedItem.PrintFooter();
                Console.WriteLine();

                int status = InputInt("Reservation Id", out id);
                if (status == -1 || status == 0) return;
                if (status == 1)
                {
                    selected = reservations.FirstOrDefault(r => r.Id == id);
                    if (selected is null)
                    {
                        ConsoleAnimation.Error($"Id-si {id} olan rezervasiya tapılmadı.");
                        Console.ReadLine();
                        continue;
                    }
                    step++;
                }
            }
            else if (step == 2)
            {
                ReservedItem.PrintHeader();
                selected!.PrintInfo();
                ReservedItem.PrintFooter();
                Console.WriteLine($"\nHazırkı status: {selected.Status}");
                ConsoleAnimation.Print("\nYeni status seçin: 1-Confirmed 2-Started 3-Completed 4-Canceled", ConsoleColor.Yellow);
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
            ConsoleAnimation.Success("Status uğurla dəyişdirildi.");
        }
        catch (Exception ex)
        {
            ConsoleAnimation.Error(ex.Message);
        }
    }

    private static void ShowUserReservations(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- İstifadəçinin Rezervasiyaları ---\n", ConsoleColor.Yellow);
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
            ConsoleAnimation.Warning("Bu FinCode üçün rezervasiya tapılmadı.");
            return;
        }

        ReservedItem.PrintHeader();
        foreach (var r in reservations) r.PrintInfo();
        ReservedItem.PrintFooter();
    }
}