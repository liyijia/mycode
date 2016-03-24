using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace System.Web.Mvc
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static IHtmlString AuthCode(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Raw("<img class='YZCode' width='100' height='35' title='看不清？换一张' />");
        }

        /// <summary>
        /// 生成发送短信
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="pageId">1 校内 2 校外 3 收件箱 4 发件箱</param>
        /// <param name="isClazzTeacher">是否是年级主任或者班主任</param>
        /// <returns></returns>
        public static IHtmlString AuthMessage(this HtmlHelper htmlHelper,  bool isClazzTeacher = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='left copeleft'>");
            sb.Append(" <ul>");
            sb.Append("<li class='link-top'><a href='/Message/teacherMessage'>教师短信</a></li>");
            if (isClazzTeacher)
                sb.Append("<li class='link-top'><a href='/Message/'>学生短信</a></li>");
            sb.Append(" <li class='link-top'><a href='/Message/MessageIn'>收件箱</a></li>");
            sb.Append(" <li class='link-top'><a href='/Message/MessageOut'>发件箱</a></li>");
            sb.Append("</div>");
            return htmlHelper.Raw(sb.ToString());
        }

        /// <summary>
        /// 家长消息导航
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="pageId"></param>
        /// <param name="isClazzTeacher"></param>
        /// <returns></returns>
        public static IHtmlString AuthNavigation(this HtmlHelper htmlHelper,int Choose=1)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='list-choose-narrow eee'><div class='biaoti-information'>");
            sb.Append("<ul class='information-in'>");
            sb.Append("<li class='information-border pointer' link='/Message/'><span class='information-in-img'></span><span class='information-in-text'>发信</span></li>");
            sb.Append("<li class='pointer' link='/Message/MessageIn'><span class='information-out-img'></span><span class='information-in-text'>收信</span></li>");
            sb.Append("</ul></div>");
            sb.Append("<ul class='choose eee'>");
            sb.AppendFormat("<li class='pointer {0}'><a href='/Message/MessageOut'>发件箱</a> <span class='number-people'></span><span class='dian'></span></li>", Choose == 2 ? "choose-on" : "");
            sb.AppendFormat("<li class='pointer {0}'><a href='/Message/MessageIn'>收件箱</a> <span class='number-people'></span><span class='dian'></span></li>", Choose == 1 ? "choose-on" : "");
            sb.AppendFormat("<li class='pointer {0}'><a href='/Message/MessageDraft'>草稿箱</a> <span class='number-people'></span><span class='dian'></span></li>", Choose == 3 ? "choose-on" : "");
            sb.Append("</ul></div>");
            return htmlHelper.Raw(sb.ToString());
        }
        /// <summary>
        /// 生成学校选择菜单
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="different">唯一标识</param>
        /// <param name="Choose">显示：1、学校2、学校年级3、学校年级班级</param>
        /// <param name="isEdit">是否修改</param>
        /// <returns></returns>
        public static IHtmlString AuthSchool(this HtmlHelper htmlHelper, string different, int Choose = 3, bool isEdit = false, int schoolId = 0, int gradeId = 0, int clazzId = 0, string wrap = "<br/>")
        {
            StringBuilder html = new StringBuilder();
            if (isEdit)
            {
                html.AppendFormat("<input type='hidden' class='hide_school_{0}' value='{1}'/>", different, schoolId);
                html.AppendFormat("<input type='hidden' class='hide_grade_{0}' value='{1}'/>", different, gradeId);
                html.AppendFormat("<input type='hidden' class='hide_clazz_{0}' value='{1}'/>", different, clazzId);
            }
            //学校
            html.AppendFormat("<span class='span_school_{0}'>学校：</span>", different);
            html.AppendFormat("<select Id='select_school_{0}' different='{0}' name='School.Id'></select>" + wrap, different);
            //年级
            if (Choose >= 2)
            {
                html.AppendFormat("<span class='span_grade_{0}'>年级：</span>", different);
                html.AppendFormat("<select Id='select_grade_{0}' different='{0}' name='Grade.Id'></select>" + wrap, different);
            }
            //班级
            if (Choose == 3)
            {
                html.AppendFormat("<span class='span_clazz_{0}'>班级：</span>", different);
                html.AppendFormat("<select Id='select_clazz_{0}' different='{0}' name='Clazz.Id'></select>", different);
            }

            return htmlHelper.Raw(html.ToString());
        }
    }
}
