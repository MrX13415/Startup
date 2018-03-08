using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Startup.Extensions;
using WindowsDesktop;

namespace Startup.Handler
{
    class StartupHandler
    {
        public const String LinkFilesExtentionFilter = "*.lnk";
        
        public delegate void ReportErrorEvent(ErrorInfo errorInfo);
        public delegate void WriteElementEvent(StartupElement element);

        public ReportErrorEvent ReportError;
        public WriteElementEvent WriteElement;

        private List<StartupElement> elements = new List<StartupElement>();

        public DirectoryInfo Folder { get; private set; }

        public int Count { get => elements.Count; }

  
        private void FindLinkFiles()
        {
            elements.Clear();
            var files = Directory.EnumerateFiles(Folder.FullName, LinkFilesExtentionFilter, SearchOption.AllDirectories);
            foreach (string file in files)
            {
                StartupElement element = new StartupElement(file);
                elements.Add(element);
            }

            elements = elements.OrderBy(o => o.StartIndex).ToList();
        }

        public void Initialize(DirectoryInfo folder)
        {
            this.Folder = folder;

            if (!this.Folder.Exists)
            {
                ReportError?.Invoke(new ErrorInfo(ErrorInfo.ErrorType.ConfigFolderNotFound, "Configuration folder not found!"));
            }

            FindLinkFiles();
        }

        public void Run()
        {
            foreach (StartupElement element in elements)
            {
                if (element.Status == StartupElement.StartupStatus.Disabled)
                {
                    WriteElement?.Invoke(element);
                    continue;
                }

                ReportDelay(element);
                element.Status = StartupElement.StartupStatus.Starting;
                WriteElement?.Invoke(element);

                ManageVDGroup(element);


                bool result = StartElement(element);

                Thread.Sleep(300);

                element.Status = StartupElement.StartupStatus.Done;
                element.Result = result;
                WriteElement?.Invoke(element);
            }
        }

        private void WaitForIdle(Process proc, int timeout=5000)
        {
            try
            {
                proc.WaitForInputIdle(timeout);
            }
            catch
            {
                int index = 0;
                while (proc.MainWindowHandle == IntPtr.Zero)
                {
                    Thread.Sleep(250);
                    index++;
                    if (index >= (timeout/250)) break;
                }
            }
        }

        private bool MoveWindow(Process proc, StartupElement element)
        {
            try
            {
                var vDesktops = VirtualDesktop.GetDesktops();
                VirtualDesktop virtualDesktop = null;

                if (element.GroupID < vDesktops.Length)
                {
                    virtualDesktop = vDesktops[element.GroupID];
                }
                else
                {
                    for (int index = 0; index < (element.GroupID - vDesktops.Length + 1); index++)
                    {
                        virtualDesktop = VirtualDesktop.Create();
                    }
                }

                VirtualDesktopHelper.MoveToDesktop(proc.MainWindowHandle, virtualDesktop);
                return true;
            }
            catch (Exception ex)
            {
                element.Error = new ErrorInfo(ErrorInfo.ErrorType.UnabelToMoveWindow, ex.Message);
                return false;
            }
        }

        private bool ManageVDGroup(StartupElement element)
        {
            try
            {
                var vDesktops = VirtualDesktop.GetDesktops();
                VirtualDesktop virtualDesktop = null;

                if (element.GroupID < vDesktops.Length)
                {
                    virtualDesktop = vDesktops[element.GroupID];
                }
                else
                {
                    for (int index = 0; index < (element.GroupID - vDesktops.Length + 1); index++)
                    {
                        virtualDesktop = VirtualDesktop.Create();
                    }
                }

               // VirtualDesktopHelper.MoveToDesktop(Process.GetCurrentProcess().MainWindowHandle, virtualDesktop);
                virtualDesktop.Switch();
                return true;
            }
            catch (Exception ex)
            {
                element.Error = new ErrorInfo(ErrorInfo.ErrorType.UnabelToMoveWindow, ex.Message);
                return false;
            }
        }

        private bool StartElement(StartupElement element)
        {
            try
            {
                Process proc = new Process();
                proc.StartInfo.FileName = element.File.FullName;
                //proc.IsRunning();
                proc.Start();

                WaitForIdle(proc);

                return true;
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                element.Error = new ErrorInfo(ErrorInfo.ErrorType.UnKnown, ex.Message);
                return false;
#endif
            }
        }

        private void ReportDelay(StartupElement element)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            do
            {
                var timeLeft = element.Delay - (timer.ElapsedMilliseconds / 1000.0);
                timeLeft = timeLeft < 0 ? 0 : timeLeft;

                element.Status = StartupElement.StartupStatus.Delay;
                element.TimeLeft = timeLeft;
                WriteElement?.Invoke(element);

                Thread.Sleep(16);

                if (timeLeft == 0) timer.Stop();
            }
            while (timer.IsRunning);
        }

    }
}
