using System.Web;
using System.Web.Optimization;

namespace LY.EMIS5.Admin
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/content/flaty/jquery_004.js",
                "~/Scripts/jquery-1.10.2.min.js", 
                "~/Scripts/jquery-migrate-1.2.1.js",
                "~/Scripts/jquery-ui-1.10.3.min.js"
                //"~/Scripts/jquery.cookie.js"                
            ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
               "~/Scripts/jquery.validate.js",
               "~/Scripts/jquery.validate.messages_cn.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
               "~/Scripts/jquery-ui-1.10.3.js",
               "~/Scripts/jquery-ui-i18n.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                "~/Scripts/bui/build/bui.js",
                "~/Content/flaty/bootstrap.js",
                "~/Content/flaty/jquery_010.js",
                "~/Content/flaty/jquery.js",
                "~/Content/flaty/jquery_006.js",
                "~/Content/flaty/jquery_009.js",
                "~/Content/flaty/jquery_007.js",
                "~/Content/flaty/jquery_002.js",
                "~/Content/flaty/jquery_005.js",
                "~/Content/flaty/jquery_003.js",
                "~/Content/flaty/jquery_008.js",
                "~/Content/flaty/flaty.js",
                //"~/Scripts/select2-3.4.5/select2.js",
                "~/Scripts/DataTables-1.9.4/media/js/jquery.dataTables.js",
                "~/Scripts/emis.framewok.js",
                "~/Scripts/jquery.form.js",
                "~/Scripts/jquery.showLoading.min.js",
                "~/Scripts/Util.js",
                "~/Scripts/Menus.js"
            ));

            bundles.Add(new StyleBundle("~/Content/themes/main").Include(
                        "~/Content/DataTables-1.9.4/media/css/jquery.dataTables.css",
                        "~/Content/validate.css",
                        "~/Scripts/build/css/bui-min.css",
                        "~/Scripts/build/css/dpl-fb.css",
                        "~/Content/style.css",
                        "~/Content/expand_style.css",
                        "~/Content/flaty/bootstrap.css",
                        "~/Content/flaty/normalize.css",
                        "~/Content/flaty/flaty.css",
                        "~/Scripts/select2-3.4.5/select2.css",
                        "~/Content/ui-lightness/jquery-ui-1.9.2.custom.min.css",
                        "~/Content/flaty/font-awesome.css"
                        ));
        }
    }
}