using System.Web.Optimization;

namespace UserInterface.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/boot").Include("~/Content/bootstrap.css", "~/Content/bootstrap-theme.css", "~/Content/font-awesome.min.css", "~/Content/Custom.css", "~/Content/sweetalert.css", "~/Content/Custom.css", "~/Content/sweetalert.css"));
            bundles.Add(new StyleBundle("~/Content/AdminLTE/css/plugins").Include("~/Content/AdminLTE/css/jvectormap/jquery-jvectormap-1.2.2.css", "~/Content/AdminLTE/css/AdminLTE.min.css", "~/Content/AdminLTE/css/skins/_all-skins.min.css"));
            //bundles.Add(new StyleBundle("~/AdminLTE/bootstrap/css/plugins").Include("~/AdminLTE/plugins/jvectormap/jquery-jvectormap-1.2.2.css", "~/AdminLTE/dist/css/AdminLTE.min.css", "~/AdminLTE/dist/css/skins/_all-skins.min.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrapdatepicker").Include("~/Content/bootstrap-datepicker3.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatable").Include("~/Content/DataTables/css/dataTables.bootstrap.min.css", "~/Content/DataTables/css/responsive.bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatablecheckbox").Include("~/Content/DataTables/css/dataTables.checkboxes.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableSelect").Include("~/Content/DataTables/css/select.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableButtons").Include("~/Content/DataTables/css/buttons.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableFixedColumns").Include("~/Content/DataTables/css/fixedColumns.dataTables.min.css"));

            

            //-------------------
            bundles.Add(new StyleBundle("~/Content/UserCSS/Login").Include("~/Content/UserCSS/Login.css"));


            //---------------------
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-3.1.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include("~/Scripts/jquery.form.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include("~/Scripts/AdminLTE/fastclick.min.js", "~/Scripts/AdminLTE/adminlte.min.js", "~/Scripts/AdminLTE/jquery.sparkline.min.js", "~/Scripts/AdminLTE/jquery.slimscroll.min.js", "~/Scripts/AdminLTE/Chart.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminLTEDash").Include("~/Scripts/AdminLTE/dashboard2.js"));
          bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusiveajaxvalidate").Include("~/Scripts/jquery.validate.min.js", "~/Scripts/jquery.validate.unobtrusive.min.js", "~/Scripts/jquery.unobtrusive-ajax.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include("~/Scripts/DataTables/jquery.dataTables.min.js", "~/Scripts/DataTables/dataTables.bootstrap.min.js", "~/Scripts/DataTables/dataTables.responsive.min.js", "~/Scripts/DataTables/responsive.bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableSelect").Include("~/Scripts/DataTables/dataTables.select.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatablecheckbox").Include("~/Scripts/DataTables/dataTables.checkboxes.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableButtons").Include("~/Scripts/DataTables/dataTables.buttons.min.js", "~/Scripts/DataTables/buttons.flash.min.js", "~/Scripts/DataTables/buttons.html5.min.js", "~/Scripts/DataTables/buttons.print.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableFixedColumns").Include("~/Scripts/DataTables/dataTables.fixedColumns.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jsZip").Include("~/Scripts/jszip.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/userpluginjs").Include("~/Scripts/jquery.noty.packaged.min.js", "~/Scripts/custom.js", "~/Scripts/Chart.js", "~/Scripts/sweetalert.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrapdatepicker").Include("~/Scripts/bootstrap-datepicker.min.js"));

            //----------------------
            bundles.Add(new ScriptBundle("~/bundles/ManageAccess").Include("~/Scripts/UserJS/ManageAccess.js"));
            bundles.Add(new ScriptBundle("~/bundles/ManageSubObjectAccess").Include("~/Scripts/UserJS/ManageSubObjectAccess.js"));
            bundles.Add(new ScriptBundle("~/bundles/Login").Include("~/Scripts/UserJS/Login.js"));
            bundles.Add(new ScriptBundle("~/bundles/User").Include("~/Scripts/UserJS/User.js"));
            bundles.Add(new ScriptBundle("~/bundles/Privileges").Include("~/Scripts/UserJS/Privileges.js"));
            bundles.Add(new ScriptBundle("~/bundles/PrivilegesView").Include("~/Scripts/UserJS/PrivilegesView.js"));
            bundles.Add(new ScriptBundle("~/bundles/Application").Include("~/Scripts/UserJS/Application.js"));
            bundles.Add(new ScriptBundle("~/bundles/AppObject").Include("~/Scripts/UserJS/AppObject.js"));
            bundles.Add(new ScriptBundle("~/bundles/AppSubobject").Include("~/Scripts/UserJS/AppSubobject.js"));
            bundles.Add(new ScriptBundle("~/bundles/Roles").Include("~/Scripts/UserJS/Roles.js"));

            //----------------------
            bundles.Add(new ScriptBundle("~/bundles/CustomerInvoices").Include("~/Scripts/UserJS/CustomerInvoices.js"));
            bundles.Add(new ScriptBundle("~/bundles/Customers").Include("~/Scripts/UserJS/Customers.js"));
            bundles.Add(new ScriptBundle("~/bundles/Suppliers").Include("~/Scripts/UserJS/Suppliers.js"));
            bundles.Add(new ScriptBundle("~/bundles/CustomerCreditNote").Include("~/Scripts/UserJS/CustomerCreditNote.js"));
            bundles.Add(new ScriptBundle("~/bundles/Bank").Include("~/Scripts/UserJS/Bank.js"));
            bundles.Add(new ScriptBundle("~/bundles/TaxTypes").Include("~/Scripts/UserJS/TaxTypes.js"));
            bundles.Add(new ScriptBundle("~/bundles/CustomerPayments").Include("~/Scripts/UserJS/CustomerPayments.js"));
            bundles.Add(new ScriptBundle("~/bundles/SupplierPayments").Include("~/Scripts/UserJS/SupplierPayments.js"));
            bundles.Add(new ScriptBundle("~/bundles/SupplierInvoices").Include("~/Scripts/UserJS/SupplierInvoices.js"));
            bundles.Add(new ScriptBundle("~/bundles/SupplierCreditNotes").Include("~/Scripts/UserJS/SupplierCreditNotes.js"));
            bundles.Add(new ScriptBundle("~/bundles/OtherIncome").Include("~/Scripts/UserJS/OtherIncome.js"));
            bundles.Add(new ScriptBundle("~/bundles/OtherExpense").Include("~/Scripts/UserJS/OtherExpense.js"));
            bundles.Add(new ScriptBundle("~/bundles/MonthlyRecap").Include("~/Scripts/UserJS/MonthlyRecap.js"));
            bundles.Add(new ScriptBundle("~/bundles/ExpenseSummary").Include("~/Scripts/UserJS/ExpenseSummary.js"));
            bundles.Add(new ScriptBundle("~/bundles/Outstanding").Include("~/Scripts/UserJS/Outstanding.js"));
            bundles.Add(new ScriptBundle("~/bundles/TopCustomers").Include("~/Scripts/UserJS/TopCustomers.js"));
            bundles.Add(new ScriptBundle("~/bundles/TopSuppliers").Include("~/Scripts/UserJS/TopSuppliers.js"));
            bundles.Add(new ScriptBundle("~/bundles/Employee").Include("~/Scripts/UserJS/Employee.js"));

            bundles.Add(new ScriptBundle("~/bundles/AdminDash").Include("~/Scripts/UserJS/MonthlyRecap.js", "~/Scripts/UserJS/ExpenseSummary.js","~/Scripts/UserJS/Outstanding.js", "~/Scripts/UserJS/TopCustomers.js", "~/Scripts/UserJS/TopSuppliers.js"));
            bundles.Add(new ScriptBundle("~/bundles/SaleSummaryReport").Include("~/Scripts/UserJS/SaleSummaryReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/SalesDetailReport").Include("~/Scripts/UserJS/SalesDetailReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/OtherExpenseSummaryReport").Include("~/Scripts/UserJS/OtherExpenseSummaryReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/OtherExpenseDetailsReport").Include("~/Scripts/UserJS/OtherExpenseDetailsReport.js"));
            
         }
            bundles.Add(new ScriptBundle("~/bundles/DepositAndWithdrawals").Include("~/Scripts/UserJS/DepositAndWithdrawals.js"));
        }

    }
}