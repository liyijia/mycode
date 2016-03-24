using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LY.EMIS5.Admin
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";

            HttpPostedFile file = context.Request.Files["Filedata"];
            string uploadPath = "/Content/UploadFile/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";

            if (file != null)
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(uploadPath)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(uploadPath));
                }
                file.SaveAs(HttpContext.Current.Server.MapPath(uploadPath) + file.FileName);
                //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                context.Response.Write(uploadPath + file.FileName);
            }
            else
            {
                context.Response.Write("0");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}