using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public class ConsoleTextBuffer
    {
        public ConsoleTextBuffer(CursorPosition initialPosition)
        {
            InitialPosition = initialPosition;

            _buffer = new();
            _cursorPositions = [];
        }

        private readonly StringBuilder _buffer;

        private readonly List<CursorPosition> _cursorPositions;

        public int Width { get; private set; }

        public int Height
        {
            get
            {
                if (_cursorPositions.Count == 0)
                    return 0;

                return _cursorPositions[^1].Y - _cursorPositions[0].Y + 1;
            }
        }

        public int Length => _buffer.Length;

        public int Index
        {
            get => _Index;
            set
            {
                ThrowHelper.ArgumentOutOfRange(0, _buffer.Length, value, nameof(value));
                _Index = value;
            }
        }
        private int _Index;

        public CursorPosition InitialPosition { get; private set; }

        public CursorPosition CurrentPosition
        {
            get
            {
                if (_cursorPositions.Count == 0)
                    return InitialPosition;

                return _cursorPositions[Index];
            }
        }

        public CursorPosition StartPosition
        {
            get
            {
                if (_cursorPositions.Count == 0)
                    return InitialPosition;

                return _cursorPositions[0];
            }
        }

        public CursorPosition EndPosition
        {
            get
            {
                if (_cursorPositions.Count == 0)
                    return InitialPosition;

                return _cursorPositions[^1];
            }
        }

        public void Update()
        {
            _cursorPositions.Clear();
            Width = Console.BufferWidth > Console.WindowWidth ? Console.BufferWidth : Console.WindowWidth;
            int x = InitialPosition.X;
            int y = InitialPosition.Y;

            if (x > Width)
            {
                x = 0;
                y++;
            }

            for (int i = 0; i < _buffer.Length; i++)
            {
                _cursorPositions.Add(new(x, y));
                x += CharacterWidthMapping.Instance.GetWidth(_buffer[i]);

                if (_buffer[i] == '\n')
                {
                    x = 0;
                    y++;
                }

                if (x == Width)
                {
                    x = 0;
                    y++;
                }
                else if (x > Width)
                {
                    x = CharacterWidthMapping.Instance.GetWidth(_buffer[i]);
                    y++;
                }
            }

            _cursorPositions.Add(new(x, y));

            //int heightOffset = Console.BufferHeight - 1 - _cursorPositions[^1].Y;
            //OffsetBuffer(heightOffset);
        }

        public void OffsetBuffer(int heightOffset)
        {
            if (heightOffset < 0)
            {
                InitialPosition = InitialPosition.Offset(0, heightOffset);
                for (int i = 0; i < _cursorPositions.Count; i++)
                    _cursorPositions[i] = _cursorPositions[i].Offset(0, heightOffset);
            }
        }

        public void OffsetPosition(int indexOffset)
        {
            Index = Math.Clamp(Index + indexOffset, 0, _buffer.Length);
        }

        public void MoveToStartPosition()
        {
            Index = 0;
        }

        public void MoveToEndPosition()
        {
            Index = _buffer.Length;
        }

        public CursorPosition GetCursorPosition(int index)
        {
            return _cursorPositions[index];
        }

        public void Write(char value)
        {
            _buffer.Insert(Index, value);
            Update();
            OffsetPosition(1);
        }

        public void Write(string value)
        {
            _buffer.Insert(Index, value);
            Update();
            OffsetPosition(value.Length);
        }

        public void Backspace()
        {
            if (_buffer.Length == 0 || Index == 0)
                return;

            _buffer.Remove(Index - 1, 1);
            Update();
            OffsetPosition(-1);
        }

        public void Clear()
        {
            _buffer.Clear();
            Update();
            MoveToStartPosition();
        }

        public override string ToString()
        {
            return _buffer.ToString();
        }
    }
}
