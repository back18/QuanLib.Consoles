using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public readonly struct ConsoleText
    {
        public ConsoleText()
        {
            Text = string.Empty;
            FontColor = FontColor.Default;
        }

        public ConsoleText(string text)
        {
            ArgumentNullException.ThrowIfNull(text, nameof(text));

            Text = text;
            FontColor = FontColor.Default;
        }

        public ConsoleText(string text, FontColor fontColor)
        {
            ArgumentNullException.ThrowIfNull(text, nameof(text));

            Text = text;
            FontColor = fontColor;
        }

        public string Text { get; }

        public FontColor FontColor { get; }

        public readonly static ConsoleText SpaceOfDefaultColor = new(" ", FontColor.Default);

        public readonly static ConsoleText SpaceOfCurrentColor = new(" ", FontColor.Current);

        public readonly static ConsoleText NewLineOfDefaultColor = new(Environment.NewLine, FontColor.Default);

        public readonly static ConsoleText NewLineOfCurrentColor = new(Environment.NewLine, FontColor.Current);

        public void WriteToConsole()
        {
            FontColor current = FontColor.Current;
            FontColor.SetToConsole();
            Console.Write(Text);
            current.SetToConsole();
        }

        public void WriteLineToConsole()
        {
            WriteToConsole();
            Console.WriteLine();
        }

        public static void Write(string text, FontColor fontColor)
        {
            new ConsoleText(text, fontColor).WriteToConsole();
        }

        public static void WriteLine(string text, FontColor fontColor)
        {
            new ConsoleText(text, fontColor).WriteLineToConsole();
        }
    }
}
