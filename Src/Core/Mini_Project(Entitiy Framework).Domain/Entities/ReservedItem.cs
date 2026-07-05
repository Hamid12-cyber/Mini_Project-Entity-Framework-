using Mini_Project_Entitiy_Framework_.Domain.Entities;
using Mini_Project_Entitiy_Framework_.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_Entitiy_Framework_.Domain.Entities
{
    public class ReservedItem : BaseEntity
    {
        public string FinCode { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public Status Status { get; set; }

        public static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ┌───────┬─────────────────────┬────────────┬────────────┬────────────┬────────────┐");
            Console.WriteLine("  │ ID    │ Kitab               │ FinCode    │ Başlanğıc  │ Bitmə      │ Status     │");
            Console.WriteLine("  ├───────┼─────────────────────┼────────────┼────────────┼────────────┼────────────┤");
            Console.ResetColor();
        }

        public void PrintInfo()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  │ {0,-5} │ {1,-19} │ {2,-10} │ {3,-10} │ {4,-10} │ {5,-10} │",
                Id, Book?.Name, FinCode, StartDate.ToString("dd.MM.yyyy"), EndDate.ToString("dd.MM.yyyy"), Status);
            Console.ResetColor();
        }

        public static void PrintFooter()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  └───────┴─────────────────────┴────────────┴────────────┴────────────┴────────────┘");
            Console.ResetColor();
        }
    }
}