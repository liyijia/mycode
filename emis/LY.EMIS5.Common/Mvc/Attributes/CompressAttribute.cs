using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LY.EMIS5.Common.Mvc.Attributes
{
    public class CompressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string acceptEncoding = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (String.IsNullOrEmpty(acceptEncoding))
                return;
            var response = filterContext.HttpContext.Response;
            acceptEncoding = acceptEncoding.ToUpperInvariant();

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-Encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-Encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}
