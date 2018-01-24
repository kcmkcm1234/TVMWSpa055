var DataTables = {};
var today = '';
var fromday = '';
$(document).ready(function () {


    try {
        $("#supplierCode").select2({
            placeholder: "Select a Suppliers..",

        });

        DataTables.PayableAgeingSummaryReportTable = $('#PayableAgeingSummaryTable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: true,
             paging: true,
             data: GetPayableAgeingSummaryReport(),
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "Supplier", "defaultContent": "<i>-</i>" },
               { "data": "Current", "defaultContent": "<i>-</i>" },
               { "data": "OneToThirty", "defaultContent": "<i>-</i>" },
               { "data": "ThirtyOneToSixty", "defaultContent": "<i>-</i>" },
               { "data": "SixtyOneToNinety", "defaultContent": "<i>-</i>" },
               { "data": "NinetyOneAndOver", "defaultContent": "<i>-</i>" }

             ],
             columnDefs: [
             { className: "text-left", "targets": [0] },
             { className: "text-center", "targets": [1, 2, 3, 4, 5] }]

         });

        $(".buttons-excel").hide();
        $("#ddlSupplier").attr('style', 'visibility:true');
        today = $("#todate").val();
        fromday = $("#fromdate").val();
    } catch (x) {
        notyAlert('error', x.message);
    }
});


function GetPayableAgeingSummaryReport(reportAdvanceSearchObject) {
    try {
        debugger;
        if (reportAdvanceSearchObject == 0) {
            var data = {};
        }
        else {
            var data = { "reportAdvanceSearchObject": JSON.stringify(reportAdvanceSearchObject) };
        }
            var ds = {};
            ds = GetDataFromServerTraditional("Report/GetAccountsPayableAgeingSummary/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                return ds.Records;
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
            }
        }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function RefreshPayableAgeingSummaryReportTable() {
    try {
        debugger;
        var fromDate = $("#fromdate");
        var toDate = $("#todate");
        var companyCode = $("#CompanyCode");
        var supplierIds = $("#supplierCode");
        var invoicetype = $("#ddlInvoiceTypes");
        var PayableAdvanceSearch = new Object();
        PayableAdvanceSearch.FromDate = fromDate[0].value !== "" ? fromDate[0].value : null;
        PayableAdvanceSearch.ToDate = toDate[0].value !== "" ? toDate[0].value : null;
        PayableAdvanceSearch.CompanyCode = companyCode[0].value !== "" ? companyCode[0].value : null;
        PayableAdvanceSearch.InvoiceType = invoicetype[0].value !== "" ? invoicetype[0].value : null;
        PayableAdvanceSearch.SupplierIDs = supplierIds[0].value !== "" ? $("#supplierCode").val() : null;
        if (DataTables.PayableAgeingSummaryReportTable != undefined ) {
            DataTables.PayableAgeingSummaryReportTable.clear().rows.add(GetPayableAgeingSummaryReport(PayableAdvanceSearch)).draw(true);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}



function PrintReport() {
    try {

        $(".buttons-excel").trigger('click');


    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Back() {
    window.location = appAddress + "Report/Index/";
}


function Reset()
{
    debugger;
    $("#todate").val(today);
    $("#fromdate").val(fromday);
    $("#CompanyCode").val('ALL');
    $("#ddlInvoiceTypes").val('RB');
    $("#supplierCode").val('').trigger('change');
    RefreshPayableAgeingSummaryReportTable();
}




