using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Exceptions
{
    public class EmisException : Exception
    {
        public string Title { get; private set; }

        public EmisException(int error, string title, string message, Exception innerException = null)
            : base(message, innerException)
        {
            this.Title = title;
            this.HResult = error;
        }
    }
}
