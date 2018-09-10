$(document).ready(function () {
    try {
        debugger;


    }
  catch (x) {

      notyAlert('error', x.message);
  }

});


function IsInternalChange() {
    debugger;
    var IsInternalCompany;
    if($(IsInternal). prop("checked")==true)
    {
        IsInternalCompany = true;
    }
    else
    {
        IsInternalCompany = false; 
    }
    //-----------------Graph--------------------//
    var MonthlyRecapViewModel = new Object();
    MonthlyRecapViewModel.IsInternal = IsInternalCompany;
    MonthlyRecapViewModel.CompanyName = "All";
    ChangePartialView("Dashboard", "MonthlyRecap",MonthlyRecapViewModel);//ControllerName,id of the container div,Name of the action
   

    if ($("#Invoice").prop('checked')) {
        var summaryType = $("#Invoice").val();
    }
    else {
        var summaryType = $("#Payment").val();
    }

    var MonthlySalesPurchaseViewModel = new Object();
    MonthlySalesPurchaseViewModel.summarytype = summaryType;
    MonthlySalesPurchaseViewModel.IsInternal = IsInternalCompany;
    ChangePartialView("Dashboard", "MonthlySalesPurchase", MonthlySalesPurchaseViewModel);




    var ExpenseSummaryViewModel = new Object();
    ExpenseSummaryViewModel.IsInternal = IsInternalCompany;
    ExpenseSummaryViewModel.CompanyName = "All";
    ChangePartialView("Dashboard", "ExpenseSummary",ExpenseSummaryViewModel);//ControllerName,id of the container div,Name of the action

    //-----------------Tables------------------//
    var OutstandingSummaryViewModel = new Object();
    OutstandingSummaryViewModel.IsInternal = IsInternalCompany;
    OutstandingSummaryViewModel.CompanyName = "All";
    ChangePartialView("Dashboard", "OutstandingSummary", OutstandingSummaryViewModel);//ControllerName,id of the container div,Name of the action


    var TopCustomersViewModel = new Object();
    TopCustomersViewModel.IsInternal = IsInternalCompany;
    TopCustomersViewModel.CompanyName = "All";
    ChangePartialView("Dashboard", "TopCustomers", TopCustomersViewModel);//ControllerName,id of the container div,Name of the action

    var TopSuppliersViewModel = new Object();
    TopSuppliersViewModel.IsInternal = IsInternalCompany;
    TopSuppliersViewModel.CompanyName = "All";
    ChangePartialView("Dashboard", "TopSuppliers", TopSuppliersViewModel);//ControllerName,id of the container div,Name of the action

    LoadSalesChart();
}

function ChangePartialView(Controller, Dom, Action) {
    debugger;
   // var data = {data: Action };
    var ds = {};
    ds = GetDataFromServer(Controller + "/" + Dom + "/", Action);
    if (ds == "Nochange") {
        return; 0
    }
    $("#" + Dom).empty();
    $("#" + Dom).html(ds);

    if (Dom == "MonthlyRecap")
        MRinit();
    if (Dom == "ExpenseSummary")
        ExpenseSummaryGraph();

    if (Dom == "MonthlySalesPurchase")
        SalesPurchaseChart();
   
}




 