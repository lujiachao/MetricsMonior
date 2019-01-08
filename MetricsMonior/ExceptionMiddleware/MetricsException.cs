using MetricsMonior.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsMonior.ExceptionMiddleware
{
    public class MetricsException : Exception
    {
        public EnumMetricsException EnumMetricsEx
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
                return (int)EnumMetricsEx;
            }
        }

        public MetricsException(int Code, string message) : base(message)
        {
            _code = Code;
        }

        public MetricsException(EnumMetricsException enumMetricsEx) : base(enumMetricsEx.ToString())
        {
            EnumMetricsEx = enumMetricsEx;
        }

        public MetricsException(EnumMetricsException enumMetricsEx, string message) : base(message)
        {
            EnumMetricsEx = enumMetricsEx;
        }

        public MetricsException(EnumMetricsException enumMetricsEx, Exception ex) : base(enumMetricsEx.ToString(), ex)
        {
            EnumMetricsEx = enumMetricsEx;
        }

        public MetricsException(EnumMetricsException enumMetricsEx, string message, Exception ex) : base(message, ex)
        {
            EnumMetricsEx = enumMetricsEx;
        }

        public override string ToString()
        {
            return $"[Code={Code}][Message={Message}][InnerMessage={(InnerException == null ? string.Empty : InnerException.Message)}]";
        }
    }
}
