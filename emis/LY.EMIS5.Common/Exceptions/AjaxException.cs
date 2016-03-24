using LY.EMIS5.Common.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Exceptions
{
    public class AjaxException : EmisException
    {
        public new object Data { get; set; }

        public AjaxException(int error,  string title = null, string message = null,object data = null, Exception innerException = null)
            : base(error, title, message, innerException)
        {
            Util.GetLogger().Error("Ajax错误", innerException);
            this.Data = data;
        }

        public AjaxObject ToAjaxObject()
        {
            return new AjaxObject { HResult = this.HResult, Title = this.Title, Message = this.Message, Data = this.Data };
        }
    }
}
