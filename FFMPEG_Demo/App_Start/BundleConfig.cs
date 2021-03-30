using System.Web.Optimization;

namespace FFMPEG_Demo
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //------------------ dev's choice bundle --------------------------
            bundles.Add(new StyleBundle("~/Content/bootstrap/css").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Content/Custom/welcome.css"));
            bundles.Add(new ScriptBundle("~/Content/bootstrap/js").Include(
                     "~/Scripts/jquery-3.5.1.slim.min.js",
                     "~/Scripts/bootstrap.bundle.min.js",
                     "~/Scripts/Custom/welcome.footer.css"));
        }
    }
}
