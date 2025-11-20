using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public class ConsoleDynamicViwe : RunnableBase
    {
        public ConsoleDynamicViwe(Func<string> updateText, int updateAccuracy, ILoggerProvider? loggerProvider = null) : base(loggerProvider)
        {
            ArgumentNullException.ThrowIfNull(updateText, nameof(updateText));
            ThrowHelper.ArgumentOutOfMin(0, updateAccuracy, nameof(updateAccuracy));

            _updateText = updateText;
            UpdateAccuracy = updateAccuracy;
            _textBuffer = new(CursorPosition.Current);
        }

        private readonly ConsoleTextBuffer _textBuffer;

        private readonly Func<string> _updateText;

        public int UpdateAccuracy { get; }

        protected override void Run()
        {
            while (IsRunning)
            {
                ClearText();
                UpdateText();
                WriteText();
                Thread.Sleep(UpdateAccuracy);
            }
        }

        private void WriteText()
        {
            _textBuffer.ExpressionConsoleHeight();
            _textBuffer.InitialPosition.Apply();
            Console.Write(_textBuffer.ToString());
            _textBuffer.CurrentPosition.Apply();
        }

        private void ClearText()
        {
            Console.CursorTop = _textBuffer.InitialPosition.Y;
            Console.CursorLeft = 0;

            string whiteSpace = new(' ', _textBuffer.Width);
            for (int i = 0; i < _textBuffer.Height; i++)
                Console.Write(whiteSpace);

            _textBuffer.CurrentPosition.Apply();
        }

        private void UpdateText()
        {
            _textBuffer.Clear();
            string text = _updateText.Invoke();
            _textBuffer.Write(text);
        }

        protected override void OnStarted(IRunnable sender, EventArgs e)
        {
            if (Console.CursorLeft != 0)
            {
                _textBuffer.OffsetBuffer(-Console.CursorLeft, 1);
                Console.WriteLine();
            }

            _textBuffer.SetInitialPosition(CursorPosition.Current);
            _textBuffer.Clear();
            _textBuffer.Update();
        }

        protected override void OnStopped(IRunnable sender, EventArgs e)
        {
            _textBuffer.EndPosition.Apply();
            Console.WriteLine();
        }
    }
}
