using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public readonly struct CursorPosition
    {
        public CursorPosition()
        {
            X = 0;
            Y = 0;
        }

        public CursorPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static readonly CursorPosition Zero = new(0, 0);

        public static CursorPosition Current => new(Console.CursorLeft, Console.CursorTop);

        public int X { get; }

        public int Y { get; }

        public CursorPosition Offset(int xOffset, int yOffset)
        {
            return new(X + xOffset, Y + yOffset);
        }

        public void Apply()
        {
            Console.SetCursorPosition(X, Y);
        }

        public void Apply(int xOffset, int yOffset)
        {
            Console.SetCursorPosition(X + xOffset, Y + yOffset);
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}
