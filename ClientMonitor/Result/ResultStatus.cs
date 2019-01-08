using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientMonitor.Result
{
    public class ResultStatus
    {
        private int resultStatusCode;

        public ResultStatus(int resultStatusCode)
        {
            this.resultStatusCode = resultStatusCode;
        }

        public ResultStatus(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }
    }
}
