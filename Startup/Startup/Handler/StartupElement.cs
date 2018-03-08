using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Startup.Handler
{
    class StartupElement
    {
        public enum StartupStatus
        {
            Disabled,
            Queue,
            Delay,
            Starting,
            Done
        }

        public const string FileNameFormatRegex = @"(?<disabled>!?)?(?<index>\d+)\.(?<group>\d+) - (?<name>.+)(?:\.lnk)";

        public FileInfo File { get; private set; }

        public StartupStatus Status { get; internal set; }

        public double Delay { get; internal set; }
        public double TimeLeft { get; internal set; }

        public bool Result { get; internal set; }
        public ErrorInfo Error { get; internal set; }

        public string ServiceName { get; internal set; }
        public int StartIndex { get; internal set; }
        public int GroupID { get; internal set; }

        public StartupElement(string fileStr)
        {
            this.Delay = Properties.Settings.Default.ServiceTimeout;
            this.File = new FileInfo(fileStr);
            this.Status = StartupStatus.Queue;

            processFile();
        }

        private void processFile()
        {
            Regex r = new Regex(FileNameFormatRegex);
            Match m = r.Match(File.Name);

            if (m.Success)
            {
                var d = m.Groups["disabled"];
                var i = m.Groups["index"];
                var g = m.Groups["group"];
                var n = m.Groups["name"];
                if (d.Length > 0) this.Status = StartupStatus.Disabled;
                StartIndex = int.Parse (i.Value);
                GroupID = int.Parse(g.Value);
                ServiceName = n.Value;
            }
        }

    }
}
