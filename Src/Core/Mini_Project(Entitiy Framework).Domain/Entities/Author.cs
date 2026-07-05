using Mini_Project_Entitiy_Framework_.Domain.Enums;

namespace Mini_Project_Entitiy_Framework_.Domain.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Surname { get; set; }
        public Gender Gender { get; set; }
        public List<Book> Books { get; set; } = new();

        public static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ┌───────┬───────────────────────────────┬───────────┬───────────┐");
            Console.WriteLine("  │ ID    │ Ad Soyad                      │ Cins      │ Kitab sayı│");
            Console.WriteLine("  ├───────┼───────────────────────────────┼───────────┼───────────┤");
            Console.ResetColor();
        }

        public void PrintInfo()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  │ {0,-5} │ {1,-31} │ {2,-9} │ {3,-9} │",
                Id, $"{Name} {Surname}".Trim(), Gender, Books?.Count ?? 0);
            Console.ResetColor();
        }

        public static void PrintFooter()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  └───────┴───────────────────────────────┴───────────┴───────────┘");
            Console.ResetColor();
        }
    }
}