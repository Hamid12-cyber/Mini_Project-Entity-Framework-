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
    "##Book Services",
    "1.  Create Book",
    "2.  Delete Book",
    "3.  Get Book by Id",
    "4.  Show All Books",
    "5.  Update Book",
    "6.  Search Books",
    "7.  Available Books",
    "##Author Services",
    "8.  Create Author",
    "9.  Show All Authors",
    "10. Show Author's Books",
    "11. Update Author",
    "12. Delete Author",
    "13. Search Authors",
    "##Reservation Services",
    "14. Reserve Book",
    "15. Reservation List",
    "16. Change Reservation Status",
    "17. User's Reservations List",
    "18. Cancel Reservation",
    "19. Overdue Reservations",
    "20. Most Reserved Books",
    "21. Update Reservation",
    "---",
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
                    case "5": Console.Clear(); ConsoleAnimation.Loading("Kitab yenilənir"); UpdateBook(_serviceProvider); break;
                    case "6": Console.Clear(); ConsoleAnimation.Loading("Axtarılır", 400); SearchBooks(_serviceProvider); break;
                    case "7": Console.Clear(); ConsoleAnimation.Loading("Kitablar yüklənir"); ShowAvailableBooks(_serviceProvider); break;
                    case "8": Console.Clear(); ConsoleAnimation.Loading("Müəllif yaradılır"); CreateAuthor(_serviceProvider); break;
                    case "9": Console.Clear(); ConsoleAnimation.Loading("Müəlliflər yüklənir"); ShowAllAuthors(_serviceProvider); break;
                    case "10": Console.Clear(); ConsoleAnimation.Loading("Kitablar yüklənir"); ShowAuthorBooks(_serviceProvider); break;
                    case "11": Console.Clear(); ConsoleAnimation.Loading("Müəllif yenilənir"); UpdateAuthor(_serviceProvider); break;
                    case "12": Console.Clear(); ConsoleAnimation.Loading("Silmə paneli"); DeleteAuthor(_serviceProvider); break;
                    case "13": Console.Clear(); ConsoleAnimation.Loading("Axtarılır", 400); SearchAuthors(_serviceProvider); break;
                    case "14": Console.Clear(); ConsoleAnimation.Loading("Rezervasiya paneli"); ReserveBook(_serviceProvider); break;
                    case "15": Console.Clear(); ConsoleAnimation.Loading("Siyahı yüklənir"); ShowReservationList(_serviceProvider); break;
                    case "16": Console.Clear(); ConsoleAnimation.Loading("Status paneli"); ChangeReservationStatus(_serviceProvider); break;
                    case "17": Console.Clear(); ConsoleAnimation.Loading("Tarixçə yüklənir"); ShowUserReservations(_serviceProvider); break;
                    case "18": Console.Clear(); ConsoleAnimation.Loading("Ləğv edilir"); CancelReservation(_serviceProvider); break;
                    case "19": Console.Clear(); ConsoleAnimation.Loading("Yoxlanılır"); ShowOverdueReservations(_serviceProvider); break;
                    case "20": Console.Clear(); ConsoleAnimation.Loading("Hesablanır"); ShowMostReservedBooks(_serviceProvider); break;
                    case "21": Console.Clear(); ConsoleAnimation.Loading("Vaxt yenilənir"); UpdateReservationTime(_serviceProvider); break;
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
                Console.WriteLine($"Səhifə sayı: {pageCount}\n");

                var authorService = provider.GetRequiredService<IAuthorService>();
                var authors = authorService.GetAllAuthors();

                if (authors.Count == 0)
                {
                    ConsoleAnimation.Warning("Heç bir Author yoxdur. Əvvəlcə Author yaratmalısınız.");
                    Console.ReadLine();
                    step--;
                    continue;
                }

                Author.PrintHeader();
                foreach (var a in authors) a.PrintInfo();
                Author.PrintFooter();
                Console.WriteLine();

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
        var reservationService = provider.GetRequiredService<IReservationService>();

        var books = bookService.GetAllBooks();
        if (books.Count == 0)
        {
            ConsoleAnimation.Warning("Heç bir kitab yoxdur.");
            return;
        }

        var activeBookIds = reservationService.GetReservationList()
            .Where(r => r.Status == Status.Confirmed || r.Status == Status.Started)
            .Select(r => r.BookId)
            .ToHashSet();

        Book.PrintHeader();
        foreach (var b in books) b.PrintInfo();
        Book.PrintFooter();
        Console.WriteLine();

        if (activeBookIds.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Aktiv rezervasiyası olan (");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Silinə Bilməyən");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(") Kitab Id-ləri: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(string.Join(", ", activeBookIds.OrderBy(x => x)));
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
        }

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
        var authorService = provider.GetRequiredService<IAuthorService>();

        while (true)
        {
            Console.Clear();
            ConsoleAnimation.Print("--- Müəllifin Kitabları ---\n", ConsoleColor.Yellow);

            var authors = authorService.GetAllAuthors();
            if (authors.Count == 0)
            {
                ConsoleAnimation.Warning("Heç bir author yoxdur.");
                return;
            }

            Author.PrintHeader();
            foreach (var a in authors) a.PrintInfo();
            Author.PrintFooter();
            Console.WriteLine();

            int id;
            int status = InputInt("Author Id", out id);
            if (status == -1 || status == 0) return;
            if (status == 2) continue;

            var books = authorService.GetAuthorBooks(id);
            if (books.Count == 0)
            {
                ConsoleAnimation.Warning("Bu author-un kitabı yoxdur.");
            }
            else
            {
                Book.PrintHeader();
                foreach (var b in books) b.PrintInfo();
                Book.PrintFooter();
            }

            ConsoleAnimation.Write("\n  [ Enter - başqa author, 00 - ana menyu ]", ConsoleColor.DarkRed);
            var cont = Console.ReadLine();
            if (cont == "00") return;
        }
    }
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
                if (status == 1)
                {
                    if (endDate <= startDate)
                    {
                        ConsoleAnimation.Error("Bitmə tarixi başlanğıc tarixi ilə eyni və ya ondan əvvəl ola bilməz! Zəhmət olmasa yenidən daxil edin.");
                        PauseOnError();
                        continue; 
                    }
                    step++;
                }
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
    private static void UpdateBook(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Kitabın Yenilənməsi ---\n", ConsoleColor.Yellow);
        var bookService = provider.GetRequiredService<IBookService>();
        var authorService = provider.GetRequiredService<IAuthorService>();

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
            int status = InputInt("Yenilənəcək kitabın Id-si", out id);
            if (status == -1 || status == 0) return;
            if (status == 1) break;
        }

        var existing = books.FirstOrDefault(b => b.Id == id);
        if (existing is null)
        {
            ConsoleAnimation.Error($"Id-si {id} olan kitab tapılmadı.");
            return;
        }

        string name = existing.Name;
        int pageCount = existing.PageCount;
        int authorId = existing.AuthorId;

        int step = 1;
        while (step <= 3)
        {
            Console.Clear();
            ConsoleAnimation.Print("--- Kitabın Yenilənməsi ---\n", ConsoleColor.Yellow);
            Console.WriteLine($"Hazırkı ad: {existing.Name}");

            if (step == 1)
            {
                int status = InputString("Yeni ad (dəyişməmək üçün Enter)", out var input);
                if (status == -1 || status == 0) return;
                if (status == 1) name = input;
                step++;
            }
            else if (step == 2)
            {
                Console.WriteLine($"Hazırkı səhifə sayı: {existing.PageCount}");
                int status = InputInt("Yeni səhifə sayı", out pageCount, min: 1);
                if (status == -1) return;
                if (status == 0) { step--; continue; }
                if (status == 1) step++;
            }
            else if (step == 3)
            {
                var authors = authorService.GetAllAuthors();
                Author.PrintHeader();
                foreach (var a in authors) a.PrintInfo();
                Author.PrintFooter();
                Console.WriteLine();

                int status = InputInt("Yeni Author Id", out authorId);
                if (status == -1) return;
                if (status == 0) { step--; continue; }
                if (status == 1) step++;
            }
        }

        try
        {
            bookService.UpdateBook(id, name, pageCount, authorId);
            ConsoleAnimation.Success("Kitab uğurla yeniləndi.");
        }
        catch (Exception ex)
        {
            ConsoleAnimation.Error(ex.Message);
        }
    }

    private static void UpdateAuthor(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Müəllifin Yenilənməsi ---\n", ConsoleColor.Yellow);
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
        Console.WriteLine();

        int id;
        while (true)
        {
            int status = InputInt("Yenilənəcək Author Id", out id);
            if (status == -1 || status == 0) return;
            if (status == 1) break;
        }

        var existing = authors.FirstOrDefault(a => a.Id == id);
        if (existing is null)
        {
            ConsoleAnimation.Error($"Id-si {id} olan author tapılmadı.");
            return;
        }

        string name = existing.Name;
        string? surname = existing.Surname;
        int genderChoice = existing.Gender switch
        {
            Gender.Female => 1,
            Gender.Male => 2,
            Gender.Other => 3,
            _ => 4
        };

        int step = 1;
        while (step <= 3)
        {
            Console.Clear();
            ConsoleAnimation.Print("--- Müəllifin Yenilənməsi ---\n", ConsoleColor.Yellow);

            if (step == 1)
            {
                Console.WriteLine($"Hazırkı ad: {existing.Name}");
                int status = InputString("Yeni ad (dəyişməmək üçün Enter)", out var input);
                if (status == -1 || status == 0) return;
                if (status == 1) name = input;
                step++;
            }
            else if (step == 2)
            {
                Console.WriteLine($"Hazırkı soyad: {existing.Surname}");
                ConsoleAnimation.Print("(Əvvəlki addım: 0, Ana Menyu: 00)", ConsoleColor.Yellow);
                ConsoleAnimation.Write("Yeni soyad (boş buraxıla bilər): ", ConsoleColor.White);
                var surnameInput = Console.ReadLine();

                if (surnameInput == "00") return;
                if (surnameInput == "0") { step--; continue; }

                if (!string.IsNullOrWhiteSpace(surnameInput))
                    surname = surnameInput;
                step++;
            }
            else if (step == 3)
            {
                Console.WriteLine($"Hazırkı cins: {existing.Gender}");
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
            authorService.UpdateAuthor(id, name, surname, gender);
            ConsoleAnimation.Success("Author uğurla yeniləndi.");
        }
        catch (Exception ex)
        {
            ConsoleAnimation.Error(ex.Message);
        }
    }

    private static void DeleteAuthor(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Müəllifin Silinməsi ---\n", ConsoleColor.Yellow);
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
        Console.WriteLine();

        int id;
        while (true)
        {
            int status = InputInt("Silinəcək Author Id", out id);
            if (status == -1 || status == 0) return;
            if (status == 1) break;
        }

        try
        {
            authorService.DeleteAuthor(id);
            ConsoleAnimation.Success("Author uğurla silindi.");
        }
        catch (Exception ex)
        {
            ConsoleAnimation.Error(ex.Message);
        }
    }

    private static void SearchBooks(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Kitab Axtarışı (Ad ilə) ---\n", ConsoleColor.Yellow);
        var bookService = provider.GetRequiredService<IBookService>();

        string keyword;
        while (true)
        {
            int status = InputString("Axtarış açar sözü", out keyword);
            if (status == -1 || status == 0) return;
            if (status == 1) break;
        }

        try
        {
            var results = bookService.SearchBooks(keyword);
            if (results.Count == 0)
            {
                ConsoleAnimation.Warning("Uyğun kitab tapılmadı.");
                return;
            }

            Book.PrintHeader();
            foreach (var b in results) b.PrintInfo();
            Book.PrintFooter();
        }
        catch (Exception ex)
        {
            ConsoleAnimation.Error(ex.Message);
        }
    }

    private static void SearchAuthors(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Müəllif Axtarışı (Ad ilə) ---\n", ConsoleColor.Yellow);
        var authorService = provider.GetRequiredService<IAuthorService>();

        string keyword;
        while (true)
        {
            int status = InputString("Axtarış açar sözü", out keyword);
            if (status == -1 || status == 0) return;
            if (status == 1) break;
        }

        try
        {
            var results = authorService.SearchAuthors(keyword);
            if (results.Count == 0)
            {
                ConsoleAnimation.Warning("Uyğun author tapılmadı.");
                return;
            }

            Author.PrintHeader();
            foreach (var a in results) a.PrintInfo();
            Author.PrintFooter();
        }
        catch (Exception ex)
        {
            ConsoleAnimation.Error(ex.Message);
        }
    }

    private static void ShowAvailableBooks(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Boş (Rezerv Olunmamış) Kitablar ---\n", ConsoleColor.Yellow);
        var bookService = provider.GetRequiredService<IBookService>();

        var books = bookService.GetAvailableBooks();
        if (books.Count == 0)
        {
            ConsoleAnimation.Warning("Hazırda boş kitab yoxdur.");
            return;
        }

        Book.PrintHeader();
        foreach (var b in books) b.PrintInfo();
        Book.PrintFooter();
    }

    private static void CancelReservation(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Rezervasiyanın Ləğvi ---\n", ConsoleColor.Yellow);
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
        Console.WriteLine();

        int id;
        while (true)
        {
            int status = InputInt("Ləğv ediləcək Reservation Id", out id);
            if (status == -1 || status == 0) return;
            if (status == 1) break;
        }

        try
        {
            reservationService.CancelReservation(id);
            ConsoleAnimation.Success("Rezervasiya uğurla ləğv edildi.");
        }
        catch (Exception ex)
        {
            ConsoleAnimation.Error(ex.Message);
        }
    }

    private static void ShowOverdueReservations(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Müddəti Keçmiş Rezervasiyalar ---\n", ConsoleColor.Yellow);
        var reservationService = provider.GetRequiredService<IReservationService>();

        var overdue = reservationService.GetOverdueReservations();
        if (overdue.Count == 0)
        {
            ConsoleAnimation.Success("Müddəti keçmiş rezervasiya yoxdur.");
            return;
        }

        ReservedItem.PrintHeader();
        foreach (var r in overdue) r.PrintInfo();
        ReservedItem.PrintFooter();
    }

    private static void ShowMostReservedBooks(IServiceProvider provider)
    {
        Console.Clear();
        ConsoleAnimation.Print("--- Ən Çox Rezerv Olunan Kitablar (Top 5) ---\n", ConsoleColor.Yellow);
        var reservationService = provider.GetRequiredService<IReservationService>();

        var stats = reservationService.GetMostReservedBooks(5);
        if (stats.Count == 0)
        {
            ConsoleAnimation.Warning("Heç bir rezervasiya tapılmadı.");
            return;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  ┌───────┬─────────────────────────┬───────────────┐");
        Console.WriteLine("  │ ID    │ Ad                      │ Rezervasiya   │");
        Console.WriteLine("  ├───────┼─────────────────────────┼───────────────┤");
        Console.ResetColor();

        foreach (var (book, count) in stats)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  │ {0,-5} │ {1,-23} │ {2,-13} │", book.Id, book.Name, count);
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  └───────┴─────────────────────────┴───────────────┘");
        Console.ResetColor();
    }
    private static void UpdateReservationTime(IServiceProvider provider)
    {
        var reservationService = provider.GetRequiredService<IReservationService>();
        var reservations = reservationService.GetReservationList();

        int id = 0;
        ReservedItem? selected = null;
        DateTime newStart = DateTime.MinValue;
        DateTime newEnd = DateTime.MinValue;

        int step = 1;
        while (step <= 3)
        {
            Console.Clear();
            ConsoleAnimation.Print("--- Rezervasiya Vaxtının Dəyişdirilməsi ---\n", ConsoleColor.Yellow);

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

                int status = InputInt("Vaxtı dəyişdiriləcək Reservation Id", out id);
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
                Console.WriteLine($"\nHazırkı başlanğıc tarixi: {selected.StartDate:dd.MM.yyyy}");

                int status = InputDate("Yeni başlanğıc tarixi (dd.MM.yyyy)", out newStart);
                if (status == -1) return;
                if (status == 0) { step--; continue; }
                if (status == 1) step++;
            }
            else if (step == 3)
            {
                Console.WriteLine($"Yeni başlanğıc tarixi: {newStart:dd.MM.yyyy}");
                Console.WriteLine($"Hazırkı bitmə tarixi: {selected!.EndDate:dd.MM.yyyy}");

                int status = InputDate("Yeni bitmə tarixi (dd.MM.yyyy)", out newEnd);
                if (status == -1) return;
                if (status == 0) { step--; continue; }
                if (status == 1) step++;
            }
        }

        try
        {
            reservationService.UpdateReservationDates(id, newStart, newEnd);
            ConsoleAnimation.Success("Rezervasiyanın vaxtı uğurla yeniləndi.");
        }
        catch (Exception ex)
        {
            ConsoleAnimation.Error(ex.Message);
        }
    }
}