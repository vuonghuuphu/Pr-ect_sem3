using System.Web;
using System.Web.Optimization;

namespace Project_3
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
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            
            //client
            //css
            bundles.Add(new StyleBundle("~/frontend/css").Include(
                    "~/Content/Temp/css/bootstrap.min.css",
                     "~/Content/Temp/css/flaticon.css",
                     "~/Content/Temp/css/font-awesome.min.css",
                     "~/Content/Temp/css/jquery-ui.min.css",
                     "~/Content/Temp/css/owl.carousel.min.css",
                     "~/Content/Temp/css/slicknav.min.css",
                     "~/Content/Temp/css/style.css"
              ));
            // js
            bundles.Add(new ScriptBundle("~/frontend/js").Include(
            "~/Content/Temp/js/bootstrap.min.js",
            "~/Content/Temp/js/circle-progress.min.js",
            "~/Content/Temp/js/jquery-3.2.1.min.js",
            "~/Content/Temp/js/jquery.slicknav.min.js",
            "~/Content/Temp/js/main.js",
            "~/Content/Temp/js/owl.carousel.min.js"
              ));
        //admin
        }
    }
}
