using System.Web;
using System.Web.Optimization;

namespace Base.Api
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.1.11.3.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/owl.carousel.min.js",
                      "~/Scripts/waypoints.min.js",
                      "~/Scripts/active.js",
                      "~/Scripts/bootstrap-table.js",
                      "~/Scripts/bootstrap-notify.js",
                      "~/Scripts/chatScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/projeto").Include(
                "~/Scripts/JsHelpers.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/bootstrap.min.css",
                       "~/Content/main.css",
                       "~/Content/font-awesome.min.css",
                       "~/Content/animate.css",
                        "~/Content/bootstrap-table.css",
                       "~/Content/creative-brands.css",
                       "~/Content/vertical-carousel.css",
                       "~/Content/custom.css"));
        }
    }
}
