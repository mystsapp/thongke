using System.Web;
using System.Web.Optimization;

namespace ThongKe.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Assets/Admin/vendor/bootstrap/css/bootstrap.min.css",
                      "~/Assets/Admin/js/User/jquery-ui/jquery-ui.css",
                      "~/Assets/Admin/vendor/metisMenu/metisMenu.min.css",
                      "~/Assets/Admin/dist/css/sb-admin-2.css",
                      "~/Assets/Admin/vendor/fontawesome-free-5.1.0-web/css/fontawesome.css",
                      "~/Assets/Admin/vendor/fontawesome-free-5.1.0-web/css/all.css",

                      "~/Content/boxover/css/boxover.css",
                      "~/Content/boostrap/vendor/morrisjs/morris.css",
                      "~/Content/Site.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Assets/Admin/vendor/jquery/jquery.min.js",
                "~/Assets/Admin/vendor/bootstrap/js/bootstrap.min.js",
                     "~/Assets/Admin/vendor/metisMenu/metisMenu.min.js",
                     "~/Content/boostrap/vendor/raphael/raphael.min.js",
                     "~/Assets/Admin/dist/js/sb-admin-2.js",
                     "~/Content/boxover/js/boxover.js"
                     ));
        }
    }
}
