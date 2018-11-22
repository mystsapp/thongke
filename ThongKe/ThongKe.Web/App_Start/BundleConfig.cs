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

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/boostrap/vendor/metisMenu/metisMenu.min.css",
            //          "~/Content/boostrap/dist/css/sb-admin-2.css",
            //          "~/Content/boxover/css/boxover.css",
            //          "~/Content/boostrap/vendor/morrisjs/morris.css",
            //          "~/Content/boostrap/vendor/font-awesome/css/font-awesome.min.css",
            //          "~/Content/Site.css"));


            //bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
            //         "~/Content/boostrap/vendor/metisMenu/metisMenu.min.js",
            //         "~/Content/boostrap/vendor/raphael/raphael.min.js",
            //         "~/Content/boostrap/dist/js/sb-admin-2.js",
            //         "~/Content/boxover/js/boxover.js"));

            
        }
    }
}
