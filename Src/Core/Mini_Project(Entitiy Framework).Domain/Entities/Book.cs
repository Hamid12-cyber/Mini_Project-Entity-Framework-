using Mini_Project_Entitiy_Framework_.Domain.Entities;

namespace Mini_Project_Entitiy_Framework_.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int PageCount { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
        public List<ReservedItem> ReservedItems { get; set; } = new();

        public static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ┌───────┬─────────────────────────┬───────────┬─────────────────────────┐");
            Console.WriteLine("  │ ID    │ Ad                      │ Səhifə    │ Müəllif                 │");
            Console.WriteLine("  ├───────┼─────────────────────────┼───────────┼─────────────────────────┤");
            Console.ResetColor();
        }

        public void PrintInfo()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  │ {0,-5} │ {1,-23} │ {2,-9} │ {3,-23} │",
                Id, Name, PageCount, $"{Author?.Name} {Author?.Surname}".Trim());
            Console.ResetColor();
        }

        public static void PrintFooter()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  └───────┴─────────────────────────┴───────────┴─────────────────────────┘");
            Console.ResetColor();
        }
    }
}