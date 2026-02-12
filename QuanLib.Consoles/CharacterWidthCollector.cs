using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.Consoles
{
    public static class CharacterWidthCollector
    {
        public static byte[] Run()
        {
            int maxWidth = 0;
            byte[] buffer = new byte[65536];

            if (Console.CursorLeft > 0)
                Console.WriteLine();

            for (int ctr = 0; ctr < buffer.Length; ctr++)
            {
                char ch = Convert.ToChar(ctr);
                if (char.IsControl(ch))
                {
                    buffer[ctr] = 0;
                }
                else
                {
                    Console.Write(ch);
                    int width = Console.CursorLeft;
                    if (width > maxWidth)
                        maxWidth = width;

                    buffer[ctr] = width > byte.MaxValue ? byte.MaxValue : (byte)width;
                    Console.CursorLeft = 0;
                }
            }

            Console.Write(new string(' ', maxWidth));
            Console.CursorLeft = 0;

            return buffer;
        }
    }
}
