using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public class ConsoleUtil
    {
        public static void WaitForInputKey(ConsoleKey key, int accuracy)
        {
            ThrowHelper.ArgumentOutOfMin(0, accuracy, nameof(accuracy));

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey(true).Key == key)
                    {
                        break;
                    }
                }
                else
                {
                    Thread.Sleep(accuracy);
                }
            }
        }

        public static async Task WaitForInputKeyAsync(ConsoleKey key, int accuracy, CancellationToken cancellationToken)
        {
            ThrowHelper.ArgumentOutOfMin(0, accuracy, nameof(accuracy));

            while (!cancellationToken.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey(true).Key == key)
                        break;
                }
                else
                {
                    await Task.Delay(accuracy, cancellationToken);
                }
            }
        }

        public static async Task WaitForInputKeyAsync(ConsoleKey key, int accuracy)
        {
            await WaitForInputKeyAsync(key, accuracy, CancellationToken.None);
        }

        public static CursorPosition[] CharacterMapping(string text, CursorPosition initialPosition)
        {
            List<CursorPosition> result = [];
            int width = Console.BufferWidth > Console.WindowWidth ? Console.BufferWidth : Console.WindowWidth;
            int x = initialPosition.X;
            int y = initialPosition.Y;

            if (x > width)
            {
                x = 0;
                y++;
            }

            foreach (char c in text)
            {
                result.Add(new(x, y));
                x += CharacterWidthMapping.Instance.GetWidth(c);

                if (c == '\n')
                {
                    x = 0;
                    y++;
                }

                if (x == width)
                {
                    x = 0;
                    y++;
                }
                else if (x > width)
                {
                    x = CharacterWidthMapping.Instance.GetWidth(c);
                    y++;
                }
            }

            return result.ToArray();
        }
    }
}
