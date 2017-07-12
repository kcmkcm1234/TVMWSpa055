var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {
        DataTables.SupplInvTable = $('#SuppInvTable').DataTable(
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
              { "data": "suppliersObj.CompanyName", "defaultContent": "<i>-</i>" },
              { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>" },
              { "data": "TotalInvoiceAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "BalanceDue", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "LastPaymentDateFormatted", "defaultContent": "<i>-</i>" },
              { "data": "Status", "defaultContent": "<i>-</i>" },
              { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-right", "targets": [5, 6] },
            { className: "text-center", "targets": [1, 2, 3, 4, 7, 8, 9] }

            ]
        });
        
        $('.Roundoff').on('change', function () {
            debugger;
            var SupplierInvoiceViewModel = new Object();
            SupplierInvoiceViewModel.GrossAmount = $('#txtGrossAmt').val();
            SupplierInvoiceViewModel.Discount = ((parseInt($('#txtDiscount').val())) > (parseInt($('#txtGrossAmt').val()))) ? "0.00" : $('#txtDiscount').val();
            SupplierInvoiceViewModel.NetTaxableAmount = (SupplierInvoiceViewModel.GrossAmount - SupplierInvoiceViewModel.Discount)
            SupplierInvoiceViewModel.TaxType = $('#ddlTaxType').val() != "" ? GetTaxRate($('#ddlTaxType').val()) : $('#txtTaxPercApp').val();
            SupplierInvoiceViewModel.TaxPercentage = SupplierInvoiceViewModel.TaxType
            SupplierInvoiceViewModel.TaxAmount = (SupplierInvoiceViewModel.NetTaxableAmount * SupplierInvoiceViewModel.TaxPercentage) / 100
            SupplierInvoiceViewModel.TotalInvoiceAmount = (SupplierInvoiceViewModel.NetTaxableAmount + SupplierInvoiceViewModel.TaxAmount)
            $('#txtNetTaxableAmt').val(SupplierInvoiceViewModel.NetTaxableAmount);
            $('#txtTaxPercApp').val(SupplierInvoiceViewModel.TaxPercentage);
            $('#txtTaxAmt').val(SupplierInvoiceViewModel.TaxAmount);
            $('#txtTotalInvAmt').val(SupplierInvoiceViewModel.TotalInvoiceAmount);
            if ((parseInt($('#txtDiscount').val())) > (parseInt($('#txtGrossAmt').val()))) {
                $('#txtDiscount').val("0.00");
            }

        });
        $('#txtTaxPercApp').on('keypress', function () {
            debugger;
            if ($('#ddlTaxType').val() != "")
                $('#ddlTaxType').val('')
        });
        $('#SuppInvTable tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
        $('input[type="text"].Roundoff').on('focus', function () {
            $(this).select();
        });
        showLoader();
        List();
    }
    catch (e) {

    }
});
function Edit(Obj) {
    debugger;
    $('#SupplierInvoiceForm')[0].reset();
    var rowData = DataTables.SupplInvTable.row($(Obj).parents('tr')).data();
    $('#ID').val(rowData.ID);
    PaintInvoiceDetails();
    openNav();
}
function PaintInvoiceDetails() {
    debugger;
    var InvoiceID = $('#ID').val();
    var SupplierInvoiceViewModel = GetSupplierInvoiceDetails(InvoiceID);
    $('#lblInvoiceNo').text(SupplierInvoiceViewModel.InvoiceNo);
    $('#txtInvNo').val(SupplierInvoiceViewModel.InvoiceNo);
    $('#txtInvDate').val(SupplierInvoiceViewModel.InvoiceDateFormatted);
    $('#ddlCompany').val(SupplierInvoiceViewModel.companiesObj.Code);
    $('#ddlSupplier').val(SupplierInvoiceViewModel.suppliersObj.ID);
    $('#txtBillingAddress').val(SupplierInvoiceViewModel.BillingAddress);
    $('#ddlPaymentTerm').val(SupplierInvoiceViewModel.paymentTermsObj.Code);
    $('#txtPayDueDate').val(SupplierInvoiceViewModel.PaymentDueDateFormatted);
    $('#txtGrossAmt').val(SupplierInvoiceViewModel.GrossAmount);
    $('#txtDiscount').val(SupplierInvoiceViewModel.Discount);
    $('#txtNetTaxableAmt').val(SupplierInvoiceViewModel.GrossAmount - SupplierInvoiceViewModel.Discount);
    $('#ddlTaxType').val(SupplierInvoiceViewModel.TaxTypeObj.Code);
    $('#txtTaxPercApp').val(SupplierInvoiceViewModel.TaxPercApplied);
    $('#txtTaxAmt').val(SupplierInvoiceViewModel.TaxAmount);
    $('#txtTotalInvAmt').val(SupplierInvoiceViewModel.TotalInvoiceAmount);
    $('#txtNotes').val(SupplierInvoiceViewModel.Notes);
    $('#ID').val(SupplierInvoiceViewModel.ID);
    $('#lblinvoicedAmt').text(SupplierInvoiceViewModel.TotalInvoiceAmountstring);
    $('#lblpaidAmt').text(SupplierInvoiceViewModel.PaidAmountstring);
    $('#lblbalalnceAmt').text(SupplierInvoiceViewModel.BalanceDuestring);


}
function List() {
    debugger;
    var result = GetAllInvoicesAndSummary();
    if (result != null) {
        if (result.SupplierInvoices != null)
            DataTables.SupplInvTable.clear().rows.add(result.SupplierInvoices).draw(false);
        if (result.SupplierInvoiceSummary != null) {
            Summary(result.SupplierInvoiceSummary);
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
//---------------Bind logics-------------------
function GetAllInvoicesAndSummary() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("SupplierInvoices/GetInvoicesAndSummary/", data);
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
function AddNew() {
    $('#SupplierInvoiceForm')[0].reset();
    $('#lblinvoicedAmt').text("₹ 0.00");
    $('#lblpaidAmt').text("₹ 0.00");
    $('#lblbalalnceAmt').text("₹ 0.00");
    $('#ID').val('');
    $('#lblInvoiceNo').text("New Invoice");
    openNav();
}
function FillCustomerDefault(this_Obj) {
    try {
        var ID = this_Obj.value;
        var SupplierViewModel = GetSupplierDetails(ID);
        $('#txtBillingAddress').val(SupplierViewModel.BillingAddress);
        $('#ddlPaymentTerm').val(SupplierViewModel.PaymentTermCode);
        $('#ddlPaymentTerm').trigger('change');
    }
    catch (e) {

    }
}
function GetTaxRate(Code) {
    debugger;
    try {
        var data = { "Code": Code };
        var ds = {};
        ds = GetDataFromServer("SupplierInvoices/GetTaxRate/", data);
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
function GetSupplierDetails(ID) {
    try {
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("SupplierInvoices/GetSupplierDetails/", data);
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
function GetDueDate(this_Obj) {
    try {
        debugger;
        var Code = this_Obj.value;
        var PaymentTermViewModel = GetPaymentTermDetails(Code);
        $('#txtPayDueDate').val(PaymentTermViewModel);
    }
    catch (e) {

    }
}
function GetPaymentTermDetails(Code) {
    debugger;
    try {
        var data = { "Code": Code };
        var ds = {};
        ds = GetDataFromServer("SupplierInvoices/GetDueDate/", data);
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
function GetSupplierInvoiceDetails() {
    try {
        var InvoiceID = $('#ID').val();
        var data = { "ID": InvoiceID };
        var ds = {};
        ds = GetDataFromServer("SupplierInvoices/GetSupplierInvoiceDetails/", data);
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