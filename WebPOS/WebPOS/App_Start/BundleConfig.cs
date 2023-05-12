using System.Web.Optimization;

namespace WebPOS
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/site.css",
                    "~/Content/StylesPOS.css",
                     "~/Content/toast/toastr.min.css",
                    "~/Content/toast/extensions/toastr.css",
                    "~/Content/themes/base/autocomplete.css",
                    "~/Content/themes/base/jquery-ui.css",
                    "~/Content/themes/base/jquery-ui.min.css",
                     "~/Content/DataTables/css/jquery.dataTables.min.css",
                    "~/Content/DataTables/css/keyTable.dataTables.min.css",
                    "~/Content/DataTables/css/buttons.dataTables.min.css",
                    "~/Content/DataTables/css/fixedColumns.dataTables.min.css",
                    "~/Content/DataTables/css/select.dataTables.min.css"
                   ).Include("~/Content/font-awesome.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new Bundle("~/bundles/scripts").Include(
                                "~/ScriptsPaginate/paginate.js"));

            bundles.Add(new Bundle("~/bundles/scripts").Include(
                      "~/Scripts/POS/login.js",
                      "~/Scripts/POS/Home.js",
                      "~/Scripts/POS/Ventas.js",
                      "~/Scripts/POS/Abonos.js",
                      "~/Scripts/POS/Compras.js",
                      "~/Scripts/POS/FacturacionConsultas.js",
                      "~/Scripts/POS/Payback.js",
                      "~/Scripts/Typehead/typeahead.bundle.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/POS/Reportes.js",
                      "~/Scripts/POS/jquery.table2excel.min.js",
                      "~/Scripts/jquery.loading.block.js",
                       "~/Scripts/POS/VentasFranquicias.js",
                       "~/Scripts/POS/AbonosFranquicias.js",
                       "~/Scripts/POS/FacturacionConsultasFranquicias.js"));

            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
                       "~/Scripts/DataTables/jquery.dataTables.min.js",
                       "~/Scripts/DataTables/dataTables.bootstrap4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/toast").Include(
             "~/Scripts/toast/bootstrap-toast.min.js",
             "~/Scripts/toast/toastr.min.js",
             "~/Scripts/toast/toastjs/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/DataTablesJs").Include(
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/DataTables/dataTables.bootstrap4.min.js",
                        "~/Scripts/DataTables/dataTables.fixedColumns.min.js",
                        "~/Scripts/DataTables/dataTables.select.min.js",
                        "~/Scripts/DataTables/dataTables.keyTable.min.js",
                        "~/Scripts/DataTables/dataTables.buttons.min.js",
                        "~/Scripts/DataTables/jszip.min.js",
                        "~/Scripts/DataTables/buttons.html5.min.js",
                       "~/Scripts/DataTables/buttons.print.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/JqueryUI").Include(
                "~/Scripts/jquery-ui-1.12.1.min.js",
                 "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js"
                ));

        }
    }
}
