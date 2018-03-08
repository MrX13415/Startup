using Startup.Handler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows;
using WindowsDesktop;
using WindowsDesktop.Interop;

namespace Startup
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Program().Startup();
        }

        static void WriteHeader()
        {
            Console.WriteLine("Startup v2.0 beta (c) 2018 icelane.net");
            Console.WriteLine();
            Console.WriteLine(" [{0}]", Properties.Settings.Default.ServerName);
        }

        static void WriteDate()
        {
            Console.WriteLine();
            Console.Write(" Date: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0}", DateTime.Now);
            Console.ResetColor();
        }

        static void WriteStartHeader(StartupHandler handler)
        {
            Console.WriteLine();
            Console.Write(" Launching Services ...");
            Console.SetCursorPosition(70, Console.CursorTop);
            Console.Write("Count: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("{0}", handler.Count);
            Console.WriteLine();
            Console.ResetColor();
        }

        static void WriteDelay(StartupElement element)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("{0," + (element.Delay.ToString().Length + 4) + ":0.000}", element.TimeLeft); // (element.Delay.ToString().Length + 4)  =>  Padding
            Console.ResetColor();
        }

        public void ReportError(ErrorInfo errorInfo)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(" Error: {0}", errorInfo.Message);
            Console.ResetColor();
        }

        public void WriteElement(StartupElement element)
        {
            var cl = Console.CursorLeft;
            var ct = Console.CursorTop;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  -> ");
            Console.ResetColor();
            Console.Write("{0} - {1}", element.StartIndex, element.ServiceName);

            switch (element.Status)
            {
                case StartupElement.StartupStatus.Queue:
                case StartupElement.StartupStatus.Delay:
                    Console.SetCursorPosition(70, Console.CursorTop);
                    WriteDelay(element);
                    break;

                case StartupElement.StartupStatus.Starting:
                    Console.SetCursorPosition(77, Console.CursorTop);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("Starting...".PadRight(13));
                    break;

                case StartupElement.StartupStatus.Disabled:
                    Console.SetCursorPosition(70, Console.CursorTop);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Disabled".PadRight(20));
                    ct++;   // Next line ...
                    break;

                case StartupElement.StartupStatus.Done:
                    Console.SetCursorPosition(70, Console.CursorTop);
                    if (element.Result)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("OK ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Desktop \"{0}\"".PadRight(20), VirtualDesktop.GetDesktops().ToList().IndexOf(VirtualDesktop.Current) + 1);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Error".PadRight(20));
                        Console.WriteLine("          Error: {0}", element.Error.Message);
                        ct++;   // Next line ...
                    }

                    ct++;  // Next line ...
                    break;
            }
            Console.ResetColor();
            Console.SetCursorPosition(cl, ct);
        }

        public void Startup()
        {
            IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;

            User32.SetWindowPos(hWnd,
                new IntPtr(User32.HWND_TOPMOST),
                0, 0, 0, 0,
                User32.SWP_NOMOVE | User32.SWP_NOSIZE);

            VirtualDesktop.PinWindow(Process.GetCurrentProcess().MainWindowHandle);

            WriteHeader();
            WriteDate();

            try
            {
                StartupHandler handler = new StartupHandler();
                handler.ReportError += ReportError;
                handler.WriteElement += WriteElement;

                handler.Initialize(new DirectoryInfo(Properties.Settings.Default.ConfigDirectory));

                WriteStartHeader(handler);
                handler.Run();

            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                ReportError(new ErrorInfo(ErrorInfo.ErrorType.UnKnown, ex.Message));
#endif
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press END to exit...");

            while (Console.ReadKey().Key == ConsoleKey.End)
            {
                Thread.Sleep(250);
            }
        }
    }
}
