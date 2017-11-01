var DataTables = {};
var startdate = '';
var enddate = '';
$(document).ready(function () {
    try {
        $("#CompanyCode").select2({
        });
        DataTables.purchaseSummaryReportTable = $('#purchaseSummaryTable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [7,0, 1, 2, 3, 4, 5, 6]
                              }
             }],
             order: [],
             searching: false,
             paging: true,
             data: GetPurchaseSummary(),
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [

               { "data": "SupplierName","defaultContent": "<i>-</i>", render: function (data, type, row) {
                   if (data == '<b>GrantTotal</b>')
                       return data;  
                   else
                       return '<a href="#" onclick="ViewSupplierDetail(this);">' + data + ' </a>';
               } },
               { "data": "OpeningBalance", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Invoiced", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Paid", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Credit", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Balance", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "NetDue", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "OriginCompany", "defaultContent": "<i>-</i>" },

             ],
             columnDefs: [{ "targets": [7], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [0] },
                  { className: "text-right", "targets": [1, 2, 3, 4, 5, 6] }],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(7, { page: 'current' }).data().each(function (group, i) {
                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
                         last = group;
                     }
                 });
             }
         });

        $(".buttons-excel").hide();
        startdate = $("#todate").val();
        enddate = $("#fromdate").val();
        $('input[name="GroupSelect"]').on('change', function () {
            RefreshPurchaseSummaryTable();
        });
        $("#purchasesummarytotals").attr('style', 'visibility:true');

        debugger;
        DataTables.purchaseDetailReportTable = $('#PurchaseDetailTable').DataTable(
       {
           dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
           order: [],
           searching: false,
           paging: true,
           data: null,
           pageLength: 50,
          
           columns: [

             { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },
             { "data": "SupplierName", "defaultContent": "<i>-</i>" },
              { "data": "Date", "defaultContent": "<i>-</i>", "width": "10%" },
              { "data": "PaymentDueDate", "defaultContent": "<i>-</i>", "width": "10%" },

             { "data": "InvoiceAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             { "data": "PaidAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             { "data": "PaymentProcessed", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             { "data": "BalanceDue", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
            { "data": "Credit", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "GeneralNotes", "defaultContent": "<i></i>" },
           { "data": "OriginCompany", "defaultContent": "<i>-</i>" },
           { "data": "Origin", "defaultContent": "<i>-</i>" }

           ],
           columnDefs: [{ "targets": [10, 8], "visible": false, "searchable": false },
                 { className: "text-left", "targets": [0, 1] },
                 { className: "text-center", "targets": [2, 3] },
                { className: "text-right", "targets": [4, 5, 6, 7, 9] }] 
         
       });

    } catch (x) {

        notyAlert('error', x.message);

    }

});


function GetPurchaseSummary() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var search = $("#Search").val();
        $('#IncludeInternal').attr('checked', false);
        $('#IncludeTax').attr('checked', true);
        var internal;
        if ($('#IncludeInternal').prop("checked") == true) {
            internal = true;
        }
        else {
            internal = false;
        }
        if (companycode === "ALL") {
            if ($("#all").prop('checked')) {
                companycode = $("#all").val();
            }
            else {
                companycode = $("#companywise").val();
            }
        }
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "search": search, "IsInternal": internal };
            var ds = {};
            ds = GetDataFromServer("Report/GetPurchaseSummaryDetails/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.TotalAmount != '') {
                $("#purchasesummaryamount").text(ds.TotalAmount);
            }
            if (ds.InvoicedAmount != '') {
                $("#purchasesummaryinvoiceamount").text(ds.InvoicedAmount);
            }
            if (ds.PaidAmount != '') {
                $("#purchasesummarypaidamount").text(ds.PaidAmount);
            }
            if (ds.Result == "OK") {
                return ds.Records;
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
            }
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function RefreshPurchaseSummaryTable() {
    try {

        debugger;
        var IsInternalCompany = $('#IncludeInternal').prop('checked');
        var IsTax = $('#IncludeTax').prop('checked');
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        if (companycode === "") {
            return false;
        }

        if (companycode === "ALL") {
            $("#all").prop("disabled", false);
            $("#companywise").prop("disabled", false);
        }
        else {
            $("#all").prop("disabled", true);
            $("#companywise").prop("disabled", true);

        }
        if (DataTables.purchaseSummaryReportTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.purchaseSummaryReportTable.clear().rows.add(GetPurchaseSummary()).draw(false);
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

function Reset() {
    debugger;
    $("#todate").val(startdate);
    $("#fromdate").val(enddate);
    $("#CompanyCode").val('ALL').trigger('change');
    $("#Search").val('');
    $("#all").prop('checked', true).trigger('change');
}


function OnChangeCall() {
    debugger;
    RefreshPurchaseSummaryTable();

}

function ViewSupplierDetail(row_obj) {
    debugger;
    var rowData = DataTables.purchaseSummaryReportTable.row($(row_obj).parents('tr')).data();

    openNav();
    DataTables.purchaseDetailReportTable.clear().rows.add(GetPurchaseDetail(rowData)).draw(false);
}

function GetPurchaseDetail(rowData) {
    try {
        debugger
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var Supplier = rowData.SupplierID;

        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "Supplier": Supplier };
            var ds = {};
            ds = GetDataFromServer("Report/GetRPTViewPurchaseDetail/", data);
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
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}