﻿using System.Web;
using System.Web.Optimization;

namespace PlagiarismCore
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

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/3rdParties").Include(
                      "~/Scripts/umd/popper.min.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/DataTables/jquery.dataTables.min.js",
                      "~/Scripts/DataTables/dataTables.bootstrap4.min.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/PlagiarismChecker/Global.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/DataTables/css/dataTables.bootstrap4.min.css",
                      "~/Content/site.css"));
        }
    }
}
