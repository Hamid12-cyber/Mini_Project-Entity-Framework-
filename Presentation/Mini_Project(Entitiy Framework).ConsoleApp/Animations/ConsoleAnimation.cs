using System;
using System.Threading;

namespace Mini_Project_Entitiy_Framework_.ConsoleApp.Animations
{
    internal static class ConsoleAnimation
    {
        private static readonly ConsoleColor Primary = ConsoleColor.Red;
        private static readonly ConsoleColor Second = ConsoleColor.Yellow;
        private static readonly ConsoleColor Bright = ConsoleColor.White;
        private static readonly ConsoleColor Dim = ConsoleColor.DarkRed;

        public static void SplashScreen(string title = "ONLINE LIBRARY")
        {
            Console.Clear();
            Console.CursorVisible = false;

            int w = Console.WindowWidth;
            int cx = w / 2;
            int cy = Console.WindowHeight / 2;

            Console.ForegroundColor = Dim;
            for (int x = 0; x < w; x += 3)
            {
                SafeSetCursor(x, cy);
                Console.Write('·');
                Thread.Sleep(2);
            }
            Thread.Sleep(60);
            Console.Clear();

            string border = new string('═', title.Length + 6);
            int bx = cx - (border.Length + 2) / 2;
            int by = cy - 1;

            for (int f = 0; f < 4; f++)
            {
                Console.ForegroundColor = f % 2 == 0 ? Primary : Dim;
                SafeSetCursor(bx, by); Console.Write($"╔{border}╗");
                SafeSetCursor(bx, by + 1); Console.Write($"║   {title}   ║");
                SafeSetCursor(bx, by + 2); Console.Write($"╚{border}╝");
                Thread.Sleep(60);
            }

            Console.ForegroundColor = Primary;
            SafeSetCursor(bx, by); Console.Write($"╔{border}╗");
            SafeSetCursor(bx, by + 1);
            Console.Write("║   ");
            Console.ForegroundColor = Bright;
            Console.Write(title);
            Console.ForegroundColor = Primary;
            Console.Write("   ║");
            SafeSetCursor(bx, by + 2); Console.Write($"╚{border}╝");

            for (int len = 0; len <= title.Length + 8; len++)
            {
                SafeSetCursor(cx - len / 2, by + 4);
                Console.ForegroundColor = Second;
                Console.Write(new string('─', len));
                Thread.Sleep(12);
            }

            Thread.Sleep(400);
            Console.Clear();
            Console.CursorVisible = true;
            Console.ResetColor();
        }

        public static void Loading(string message = "Yüklənir", int durationMs = 400)
        {
            Console.CursorVisible = false;
            int barLen = Math.Min(Console.WindowWidth - 6, 44);
            long end = Environment.TickCount64 + durationMs;
            int frame = 0;

            while (Environment.TickCount64 < end)
            {
                double pct = 1.0 - (double)(end - Environment.TickCount64) / durationMs;
                int filled = (int)(pct * barLen);

                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = Second;
                Console.Write(frame % 2 == 0 ? "  ▰  " : "  ▱  ");
                Console.ForegroundColor = Bright;
                Console.Write(message + "          ");

                Console.SetCursorPosition(0, 1);
                Console.ForegroundColor = Dim;
                Console.Write("  [");
                Console.ForegroundColor = Primary;
                Console.Write(new string('█', Math.Max(0, filled)));
                Console.ForegroundColor = Dim;
                Console.Write(new string('░', Math.Max(0, barLen - filled)));
                Console.Write("]  ");

                frame++;
                Thread.Sleep(30);
            }

            Console.SetCursorPosition(0, 0); Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, 1); Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = true;
            Console.ResetColor();
        }

        public static void PrintMenu(string title, string[] items)
        {
            Console.Clear();

            // Ən uzun item-ə görə eni hesabla, amma çox da geniş olmasın deyə +2 sabit boşluq
            int contentWidth = items
                .Where(i => !i.StartsWith("##") && i != "---")
                .Select(i => i.Length)
                .DefaultIfEmpty(0)
                .Max();

            contentWidth = Math.Max(contentWidth, title.Length) + 2;

            string fullBorder = new string('─', contentWidth);

            // Əsas başlıq qutusu
            Console.WriteLine();
            Console.ForegroundColor = Dim;
            Console.WriteLine($"  ┌{fullBorder}┐");
            Console.Write("  │");
            Console.ForegroundColor = Bright;
            Console.Write(CenterText(title, contentWidth));
            Console.ForegroundColor = Dim;
            Console.WriteLine("│");
            Console.WriteLine($"  └{fullBorder}┘");
            Console.WriteLine();

            foreach (string item in items)
            {
                if (item.StartsWith("##"))
                {
                    // Bölmə başlığı — mətn ortada
                    string headerText = item.Substring(2);

                    Console.ForegroundColor = Dim;
                    Console.WriteLine($"  ┌{fullBorder}┐");
                    Console.Write("  │");
                    Console.ForegroundColor = Primary;
                    Console.Write(CenterText(headerText, contentWidth));
                    Console.ForegroundColor = Dim;
                    Console.WriteLine("│");
                    Console.WriteLine($"  └{fullBorder}┘");
                    Console.WriteLine();
                }
                else if (item == "---")
                {
                    Console.ForegroundColor = Dim;
                    Console.WriteLine($"  {fullBorder}");
                    Console.WriteLine();
                }
                else
                {
                    Console.ForegroundColor = Second;
                    Console.Write("  › ");
                    Console.ForegroundColor = Bright;
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine();
            Console.ForegroundColor = Primary;
            Console.Write("  » ");
            Console.ForegroundColor = Bright;
            Console.Write("Seçim: ");
            Console.ResetColor();
        }

        private static string CenterText(string text, int width)
        {
            if (text.Length >= width) return text;
            int totalPadding = width - text.Length;
            int left = totalPadding / 2;
            int right = totalPadding - left;
            return new string(' ', left) + text + new string(' ', right);
        }

        public static void Success(string message)
        {
            Console.ForegroundColor = Second;
            Console.WriteLine($"\n  ✓ {message}");
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = Primary;
            Console.WriteLine($"\n  ✗ {message}");
            Console.ResetColor();
        }

        public static void Warning(string message)
        {
            Console.ForegroundColor = Second;
            Console.WriteLine($"\n  ⚠ {message}");
            Console.ResetColor();
        }

        public static void Print(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void Write(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void DrawRightBorder(int targetColumn)
        {
            Console.CursorLeft = targetColumn;
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = Dim;
            Console.WriteLine("║");
            Console.ForegroundColor = oldColor;
        }

        private static void SafeSetCursor(int x, int y)
        {
            try
            {
                x = Math.Max(0, Math.Min(x, Console.WindowWidth - 1));
                y = Math.Max(0, Math.Min(y, Console.WindowHeight - 1));
                Console.SetCursorPosition(x, y);
            }
            catch { }
        }
    }
}