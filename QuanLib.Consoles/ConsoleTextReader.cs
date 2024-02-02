using QuanLib.Consoles.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public class ConsoleTextReader : ConsoleKeyReader
    {
        public ConsoleTextReader()
        {
            _textBuffer = new(CursorPosition.Current);

            KeyEventHandler.Subscribe(new(ConsoleModifiers.None, ConsoleKey.Enter), HandleEnterKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.None, ConsoleKey.Escape), HandleEscapeKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.None, ConsoleKey.Backspace), HandleBackspaceKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.Control, ConsoleKey.H), HandleBackspaceKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.None, ConsoleKey.Spacebar), HandleSpacebarKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.Control, ConsoleKey.Enter), HandleControlEnterKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.None, ConsoleKey.LeftArrow), HandleLeftArrowKey);
            KeyEventHandler.Subscribe(new(ConsoleModifiers.None, ConsoleKey.RightArrow), HandleRightArrowKey);
        }

        protected readonly ConsoleTextBuffer _textBuffer;

        public string Text => _textBuffer.ToString();

        protected override void OnKeyRead(ConsoleKeyReader sender, ConsoleKeyInfoEventArgs e)
        {
            ClearText();
            HandleKeyEvent(e.ConsoleKeyInfo);
            WriteText();
        }

        protected virtual void HandleKeyEvent(ConsoleKeyInfo consoleKeyInfo)
        {
            if (KeyEventHandler.Invoke(KeyInfo.From(consoleKeyInfo)))
                return;

            if (consoleKeyInfo.KeyChar == '\0' || char.IsWhiteSpace(consoleKeyInfo.KeyChar))
                return;

            _textBuffer.Write(consoleKeyInfo.KeyChar);
        }

        protected virtual void ClearText()
        {
            _textBuffer.InitialPosition.Apply();
            string whiteSpace = new('=', _textBuffer.Width);
            for (int i = 0; i < _textBuffer.Height; i++)
                Console.Write(whiteSpace);

            _textBuffer.CurrentPosition.Apply();
        }

        protected virtual void WriteText()
        {
            int heightOffset = Console.BufferHeight - _textBuffer.EndPosition.Y - 1;
            if (heightOffset < 0)
            {
                Console.SetCursorPosition(0, Console.BufferHeight - 1);
                for (int i = heightOffset; i < 0; i++)
                    Console.WriteLine();
                _textBuffer.OffsetBuffer(heightOffset);
            }

            _textBuffer.InitialPosition.Apply();
            Console.Write(_textBuffer.ToString());

            _textBuffer.CurrentPosition.Apply();
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
