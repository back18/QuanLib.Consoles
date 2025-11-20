using QuanLib.Core;
using QuanLib.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Consoles
{
    public class ConsoleKeyReader : RunnableBase
    {
        public ConsoleKeyReader(ILoggerProvider? loggerProvider = null) : base(loggerProvider)
        {
            ReadTimeSpan = 1;
            KeyEventHandler = new();

            KeyRead += OnKeyRead;
            KeyNotAvailable += OnKeyNotAvailable;
        }

        public int ReadTimeSpan { get; set; }

        public KeyEventHandler KeyEventHandler { get; }

        public event EventHandler<ConsoleKeyReader, EventArgs<ConsoleKeyInfo>> KeyRead;

        public event EventHandler<ConsoleKeyReader, EventArgs> KeyNotAvailable;

        protected virtual void OnKeyRead(ConsoleKeyReader sender, EventArgs<ConsoleKeyInfo> e)
        {
            KeyEventHandler.Invoke(KeyInfo.From(e.Argument));
        }

        protected virtual void OnKeyNotAvailable(ConsoleKeyReader sender, EventArgs e)
        {

        }

        protected override void Run()
        {
            while (IsRunning)
            {
                ConsoleKeyInfo keyInfo;
                while (true)
                {
                    if (!IsRunning)
                    {
                        return;
                    }

                    if (Console.KeyAvailable)
                    {
                        keyInfo = Console.ReadKey(true);
                        break;
                    }

                    KeyNotAvailable.Invoke(this, EventArgs.Empty);
                    Thread.Sleep(ReadTimeSpan);
                }

                KeyRead.Invoke(this, new(keyInfo));
            }
        }

        protected override void OnStarted(IRunnable sender, EventArgs e)
        {
            if (Console.CursorLeft != 0)
                Console.WriteLine();
        }
    }
}
