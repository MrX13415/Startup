using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Startup.Handler
{
    class ErrorInfo
    {
        public enum ErrorType
        {
            UnKnown,
            ConfigFolderNotFound,
            UnabelToMoveWindow
        }

        public ErrorType Type { get; private set; }

        public String Message { get; private set; }

        public ErrorInfo(ErrorType type, String message)
        {
            this.Type = type;
            this.Message = message;
        }
    }
}
