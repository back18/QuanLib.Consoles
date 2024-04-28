using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public readonly struct FontColor
    {
        public FontColor()
        {
            BackgroundColor = DefaultBackgroundColor;
            ForegroundColor = DefaultForegroundColor;
        }

        public FontColor(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
        }

        public const ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;

        public const ConsoleColor DefaultForegroundColor = ConsoleColor.White;

        public static readonly FontColor Default = new();

        public static FontColor Current => new(Console.BackgroundColor, Console.ForegroundColor);

        public ConsoleColor BackgroundColor { get; }

        public ConsoleColor ForegroundColor { get; }

        public void SetToConsole()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;
        }
    }
}
