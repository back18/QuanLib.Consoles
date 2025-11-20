using QuanLib.Core;
using QuanLib.Core.Events;
using QuanLib.Core.Proxys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public class ConsoleTextReader : ConsoleKeyReader
    {
        static ConsoleTextReader()
        {
            ConsoleOutProxy = new(Console.Out);
            Console.SetOut(ConsoleOutProxy);
        }

        public ConsoleTextReader(ILoggerProvider? loggerProvider = null) : base(loggerProvider)
        {
            _textBuffer = new(CursorPosition.Current);

            KeyEventHandler.Subscribe(new(ConsoleModifiers.None, ConsoleKey.Enter), HandleEnterKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.None, ConsoleKey.Escape), HandleEscapeKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.None, ConsoleKey.Backspace), HandleBackspaceKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.Control, ConsoleKey.H), HandleBackspaceKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.None, ConsoleKey.Spacebar), HandleSpacebarKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.Control, ConsoleKey.Enter), HandleControlEnterKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.Control, ConsoleKey.Backspace), HandleControlBackspaceKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.None, ConsoleKey.LeftArrow), HandleLeftArrowKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.None, ConsoleKey.RightArrow), HandleRightArrowKey);
        }

        protected readonly ConsoleTextBuffer _textBuffer;

        protected static TextWriterProxy ConsoleOutProxy { get; }

        public string Text => _textBuffer.ToString();

        protected override void OnKeyRead(ConsoleKeyReader sender, EventArgs<ConsoleKeyInfo> e)
        {
            ClearText();
            HandleKeyEvent(e.Argument);
            WriteText();
        }

        protected override void OnKeyNotAvailable(ConsoleKeyReader sender, EventArgs e)
        {
            string text = ConsoleOutProxy.GetInterceptionText();
            if (string.IsNullOrEmpty(text))
                return;

            ClearText();

            ConsoleTextBuffer outputBuffer = new(_textBuffer.InitialPosition);
            outputBuffer.Write(text);
            outputBuffer.ExpressionConsoleHeight(ConsoleOutProxy.WriteOnly);
            outputBuffer.InitialPosition.Apply();
            ConsoleOutProxy.WriteOnly(text);

            if (Console.CursorLeft != 0)
                ConsoleOutProxy.WriteLineOnly();

            _textBuffer.SetInitialPosition(CursorPosition.Current);

            WriteText();
        }

        protected virtual void HandleKeyEvent(ConsoleKeyInfo consoleKeyInfo)
        {
            if (KeyEventHandler.Invoke(KeyInfo.From(consoleKeyInfo)))
                return;

            if (consoleKeyInfo.KeyChar is '\0' or '\b' or '\u007f' || char.IsWhiteSpace(consoleKeyInfo.KeyChar))
                return;

            _textBuffer.Write(consoleKeyInfo.KeyChar);
        }

        protected virtual void WriteText()
        {
            _textBuffer.ExpressionConsoleHeight(ConsoleOutProxy.WriteOnly);
            _textBuffer.InitialPosition.Apply();
            ConsoleOutProxy.WriteOnly(_textBuffer.ToString());
            _textBuffer.CurrentPosition.Apply();
        }

        protected virtual void ClearText()
        {
            Console.CursorTop = _textBuffer.InitialPosition.Y;
            Console.CursorLeft = 0;

            string whiteSpace = new(' ', _textBuffer.Width);
            for (int i = 0; i < _textBuffer.Height; i++)
                ConsoleOutProxy.WriteOnly(whiteSpace);

            _textBuffer.CurrentPosition.Apply();
        }

        protected override void OnStarted(IRunnable sender, EventArgs e)
        {
            if (Console.CursorLeft != 0)
            {
                _textBuffer.OffsetBuffer(-Console.CursorLeft, 1);
                ConsoleOutProxy.WriteLineOnly();
            }

            _textBuffer.SetInitialPosition(CursorPosition.Current);
            _textBuffer.Clear();
            _textBuffer.Update();
            ClearText();
            WriteText();

            ConsoleOutProxy.RequestInterception = true;
        }

        protected override void OnStopped(IRunnable sender, EventArgs e)
        {
            ConsoleOutProxy.RequestInterception = false;
            _textBuffer.EndPosition.Apply();
            ConsoleOutProxy.WriteLineOnly();
        }

        protected virtual void HandleEnterKey()
        {
            IsRunning = false;
        }

        protected virtual void HandleEscapeKey()
        {

        }

        protected virtual void HandleBackspaceKey()
        {
            _textBuffer.Backspace();
        }

        protected virtual void HandleSpacebarKey()
        {
            _textBuffer.Write(' ');
        }

        protected virtual void HandleControlEnterKey()
        {
            _textBuffer.Write('\n');
        }

        protected virtual void HandleControlBackspaceKey()
        {
            _textBuffer.Clear();
        }

        protected virtual void HandleLeftArrowKey()
        {
            _textBuffer.OffsetPosition(-1);
        }

        protected virtual void HandleRightArrowKey()
        {
            _textBuffer.OffsetPosition(1);
        }
    }
}
