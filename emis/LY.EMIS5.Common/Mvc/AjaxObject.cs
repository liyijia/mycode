using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Mvc
{
    public class AjaxObject
    {
        public int HResult { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
