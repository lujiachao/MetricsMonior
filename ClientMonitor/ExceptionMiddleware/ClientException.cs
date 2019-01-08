using ClientMonitor.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientMonitor.ExceptionMiddleware
{
    public class ClientException : Exception
    {
        public EnumClientMonitor EnumClientEx
        {
            get;
        }

        private int _code;

        public int Code
        {
            get
            {
                if (_code > 0)
                {
                    return _code;
                }
                return (int)EnumClientEx;
            }
        }

        public ClientException(int Code, string message) : base(message)
        {
            _code = Code;
        }

        public ClientException(EnumClientMonitor enumClientEx) : base(enumClientEx.ToString())
        {
            EnumClientEx = enumClientEx;
        }

        public ClientException(EnumClientMonitor enumClientEx, string message) : base(message)
        {
            EnumClientEx = enumClientEx;
        }

        public ClientException(EnumClientMonitor enumClientEx, Exception ex) : base(enumClientEx.ToString(), ex)
        {
            EnumClientEx = enumClientEx;
        }

        public ClientException(EnumClientMonitor enumClientEx, string message, Exception ex) : base(message, ex)
        {
            EnumClientEx = enumClientEx;
        }

        public override string ToString()
        {
            return $"[Code={Code}][Message={Message}][InnerMessage={(InnerException == null ? string.Empty : InnerException.Message)}]";
        }
    }
}
