var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {
                
        DataTables.CustInvTable = $('#CustInvTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: null,
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [              
               { "data": "ID" },
               { "data": "InvoiceNo" },
               { "data": "InvoiceDateFormatted" },
               { "data": "customerObj.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "TotalInvoiceAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "BalanceDue", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },          
               { "data": "LastPaymentDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Status", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [5,6] },
             { className: "text-center", "targets": [1,2, 3, 4, 7,8,9] }

             ]
         });

        $('#CustInvTable tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
        $('input[type="text"].Roundoff').on('focus', function () {
            $(this).select();
        });
        showLoader();
        List();
       
        $('.Roundoff').on('change', function () {
            var CustomerInvoiceViewModel = new Object();
            CustomerInvoiceViewModel.GrossAmount = $('#txtGrossAmt').val();
            CustomerInvoiceViewModel.Discount = ((parseInt($('#txtDiscount').val())) > (parseInt($('#txtGrossAmt').val()))) ? "0.00" : $('#txtDiscount').val();
            CustomerInvoiceViewModel.NetTaxableAmount = (CustomerInvoiceViewModel.GrossAmount - CustomerInvoiceViewModel.Discount)
            CustomerInvoiceViewModel.TaxType = $('#ddlTaxType').val() != "" ? GetTaxRate($('#ddlTaxType').val()) : $('#txtTaxPercApp').val();
            CustomerInvoiceViewModel.TaxPercentage = CustomerInvoiceViewModel.TaxType
            CustomerInvoiceViewModel.TaxAmount =(CustomerInvoiceViewModel.NetTaxableAmount*CustomerInvoiceViewModel.TaxPercentage)/100
            CustomerInvoiceViewModel.TotalInvoiceAmount = (CustomerInvoiceViewModel.NetTaxableAmount + CustomerInvoiceViewModel.TaxAmount) 
            $('#txtNetTaxableAmt').val(CustomerInvoiceViewModel.NetTaxableAmount);
            $('#txtTaxPercApp').val(CustomerInvoiceViewModel.TaxPercentage);
            $('#txtTaxAmt').val(CustomerInvoiceViewModel.TaxAmount);
            $('#txtTotalInvAmt').val(CustomerInvoiceViewModel.TotalInvoiceAmount);
            if((parseInt($('#txtDiscount').val())) > (parseInt($('#txtGrossAmt').val())))
            {
                $('#txtDiscount').val("0.00");
            }
            
        });
        $('#txtTaxPercApp').on('keypress', function () {
            debugger;
            if($('#ddlTaxType').val()!="")
                $('#ddlTaxType').val('')
        });

    } catch (x) {

        notyAlert('error', e.message);

    }

});
function SaveSuccess(data, status) {
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#ID').val(JsonResult.Records.ID);
            PaintInvoiceDetails()
            List();
            notyAlert('success', JsonResult.Message);
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}
function GetTaxRate(Code)
{
    debugger;
    try {
        var data = { "Code": Code };
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetTaxRate/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            return 0;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function List() {
    var result = GetAllInvoicesAndSummary();
    if (result != null) {
        if (result.CustomerInvoices!=null)
            DataTables.CustInvTable.clear().rows.add(result.CustomerInvoices).draw(false);
        if (result.CustomerInvoiceSummary!=null) {        
            Summary(result.CustomerInvoiceSummary);
        }
    }

}
function Summary(Records) {
    $('#overdueamt').html(Records.OverdueAmountFormatted);
    $('#overdueinvoice').html(Records.OverdueInvoices);
    $('#openamt').html(Records.OpenAmountFormatted);
    $('#openinvoice').html(Records.OpenInvoices);
    $('#paidamt').html(Records.PaidAmountFormatted);
    $('#paidinvoice').html(Records.PaidInvoices);
}


function Edit(Obj) {
    debugger;
    $('#CustomerInvoiceForm')[0].reset();
    var rowData = DataTables.CustInvTable.row($(Obj).parents('tr')).data();
    $('#ID').val(rowData.ID);
    PaintInvoiceDetails();
    openNav();
}
function AddNew()
{
    $('#CustomerInvoiceForm')[0].reset();
    $('#lblinvoicedAmt').text("₹ 0.00");
    $('#lblpaidAmt').text("₹ 0.00");
    $('#lblbalalnceAmt').text("₹ 0.00");
    $('#ID').val('');
    $('#lblInvoiceNo').text("New Invoice");
    openNav();
}
function PaintInvoiceDetails()
{
    debugger;
    var InvoiceID = $('#ID').val();
    var CustomerInvoicesViewModel = GetCustomerInvoiceDetails(InvoiceID);
    $('#lblInvoiceNo').text(CustomerInvoicesViewModel.InvoiceNo);
    $('#txtInvNo').val(CustomerInvoicesViewModel.InvoiceNo);
    $('#txtInvDate').val(CustomerInvoicesViewModel.InvoiceDateFormatted);
    $('#ddlCompany').val(CustomerInvoicesViewModel.companiesObj.Code);
    $('#ddlCustomer').val(CustomerInvoicesViewModel.customerObj.ID);
    $('#txtBillingAddress').val(CustomerInvoicesViewModel.BillingAddress);
    $('#ddlPaymentTerm').val(CustomerInvoicesViewModel.paymentTermsObj.Code);
    $('#txtPayDueDate').val(CustomerInvoicesViewModel.PaymentDueDateFormatted);
    $('#txtGrossAmt').val(CustomerInvoicesViewModel.GrossAmount);
    $('#txtDiscount').val(CustomerInvoicesViewModel.Discount);
    $('#txtNetTaxableAmt').val(CustomerInvoicesViewModel.GrossAmount - CustomerInvoicesViewModel.Discount);
    $('#ddlTaxType').val(CustomerInvoicesViewModel.TaxTypeObj.Code);
    $('#txtTaxPercApp').val(CustomerInvoicesViewModel.TaxPercApplied);
    $('#txtTaxAmt').val(CustomerInvoicesViewModel.TaxAmount);
    $('#txtTotalInvAmt').val(CustomerInvoicesViewModel.TotalInvoiceAmount);
    $('#txtNotes').val(CustomerInvoicesViewModel.Notes);
    $('#ID').val(CustomerInvoicesViewModel.ID);
    $('#lblinvoicedAmt').text("₹ " + CustomerInvoicesViewModel.TotalInvoiceAmount);
    $('#lblpaidAmt').text("₹ " + (CustomerInvoicesViewModel.TotalInvoiceAmount - CustomerInvoicesViewModel.BalanceDue));
    $('#lblbalalnceAmt').text("₹ " + CustomerInvoicesViewModel.BalanceDue);


}
//---------------Bind logics-------------------
function GetAllInvoicesAndSummary() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetInvoicesAndSummary/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
//onchange function for the customer dropdown to fill:- due date and Address 
function FillCustomerDefault(this_Obj)
{
    try
    {
        var ID = this_Obj.value;
        var CustomerViewModel = GetCustomerDetails(ID);
        $('#txtBillingAddress').val(CustomerViewModel.BillingAddress);
        $('#ddlPaymentTerm').val(CustomerViewModel.PaymentTermCode);
        $('#ddlPaymentTerm').trigger('change');
    }
    catch(e)
    {

    }
}
function GetCustomerDetails(ID)
{
    try {
        var data = {"ID":ID};
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetCustomerDetails/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function GetDueDate(this_Obj)
{
    try
    {
        debugger;
        var Code = this_Obj.value;
        var PaymentTermViewModel = GetPaymentTermDetails(Code);
         $('#txtPayDueDate').val(PaymentTermViewModel);
    }
    catch(e)
    {

    }
}
function GetPaymentTermDetails(Code)
{
    debugger;
    try {
        var data = { "Code":Code };
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetDueDate/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function GetCustomerInvoiceDetails()
{
    try {
        var InvoiceID = $('#ID').val();
        var data = {"ID":InvoiceID};
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetCustomerInvoiceDetails/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
