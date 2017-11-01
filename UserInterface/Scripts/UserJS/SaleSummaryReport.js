var DataTables = {};
var startdate = '';
var enddate = '';
$(document).ready(function () {
    try {
        $("#CompanyCode").select2({
        });
        
        DataTables.saleSummaryReportTable = $('#saleSummaryTable').DataTable(
         {
            
             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0,1,2, 3,4,5,6,7]
                              }
             }],
             order: [],
             "ordering": false,
             searching: false,
             paging: true,
             data: GetSaleSummary(),
             pageLength: 50,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search"
             //},
             columns: [ 
               {
                   "data": "CustomerName", "defaultContent": "<i>-</i>", render: function (data, type, row) {
                       if (data == '<b>GrantTotal</b>')
                           return data;  
                       else
                           return '<a href="#" onclick="ViewCustomerDetail(this);">' + data + ' </a>';
                   }
               },
               { "data": "Invoiced",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "TaxAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Total", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Paid", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Credit", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Balance", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "NetDue", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             
               
              { "data": "OriginCompany", "defaultContent": "<i>-</i>" },

             ],
             columnDefs: [{ "targets": [7,8], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [0] },
                  { className: "text-right", "targets": [1, 2, 3, 4, 5, 6] }],
             
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(8, { page: 'current' }).data().each(function (group, i) {
                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="8" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
                         last = group;
                     }
                 });
             }
         });
        $(".buttons-excel").hide();
        startdate = $("#todate").val();
        enddate = $("#fromdate").val();

        DataTables.saleDetailReportTable = $('#saleDetailTable').DataTable(
        {
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
            paging: true,
            data: null,
            pageLength: 50,
            columns: [
              { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "Date", "defaultContent": "<i>-</i>", "width": "10%" },
                 { "data": "PaymentDueDate", "defaultContent": "<i>-</i>", "width": "10%" },
              { "data": "InvoiceAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                { "data": "TaxAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                  { "data": "Total", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "PaidAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "BalanceDue", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             { "data": "Credit", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "GeneralNotes", "defaultContent": "<i></i>" },
            { "data": "OriginCompany", "defaultContent": "<i>-</i>" },
            { "data": "Origin", "defaultContent": "<i>-</i>" }

            ],
            columnDefs: [{ "targets": [9, 11], "visible": false, "searchable": false },

                 { className: "text-left", "targets": [0, 1, 10] },
                  { className: "text-center", "targets": [2, 3] },
                 { className: "text-right", "targets": [4, 5, 6, 8, 7, 9] }]
        });
        
        ////$('input[name="GroupSelect"]').on('change', function () {
        //    RefreshSaleSummaryTable();
        ////});
        $("#saleTotals").attr('style', 'visibility:true');
       
    } catch (x) {

        notyAlert('error', x.message);

    }

});


function GetSaleSummary() {
    try {
        debugger;
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var search = $("#Search").val();
        var internal;
        $('#IncludeInternal').attr('checked', false);
        $('#IncludeTax').attr('checked', true);
        if ($('#IncludeInternal').prop("checked") == true) {
            internal = true;
        }
        else {
            internal = false;
        }
        var tax;
        if ($('#IncludeTax').prop("checked") == true) {
            tax = true;
        }
        else {
            tax = false;
        }
        //if (companycode === "ALL")
        //{
        //    if ($("#all").prop('checked')) {
        //        companycode = $("#all").val();
        //    }
        //    else {
        //        companycode = $("#companywise").val();
        //    }
        //}        
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "search": search, "IsInternal": internal, "IsTax": tax };
            var ds = {};
            ds = GetDataFromServer("Report/GetSaleSummary/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.TotalAmount != '') {
                $("#salessummaryamount").text(ds.TotalAmount);
            }
            if (ds.InvoicedAmount != '') {
                $("#salessummaryinvoiceamount").text(ds.InvoicedAmount);
            }
            if (ds.PaidAmount != '') {
                $("#salessummarypaidamount").text(ds.PaidAmount);
            }
            if (ds.TaxAmount != '') {
                $("#salessummarytax").text(ds.TaxAmount);
            }
            if (ds.Invoiced != '') {
                $("#salessummaryinvoiced").text(ds.Invoiced);
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


function RefreshSaleSummaryTable() {
    try {
        debugger;
       
        var IsInternalCompany = $('#IncludeInternal').prop('checked');
        var IsTax = $('#IncludeTax').prop('checked');
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        //if (companycode === "")
        //{
        //    return false;
        //}
            
        //if (companycode === "ALL") {
        //    $("#all").prop("disabled", false);
        //    $("#companywise").prop("disabled", false);
        //}
        //else {
        //    $("#all").prop("disabled", true);
        //    $("#companywise").prop("disabled", true);
         
        //}
        if (DataTables.saleSummaryReportTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.saleSummaryReportTable.clear().rows.add(GetSaleSummary()).draw(false);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function PrintReport() {
    debugger;
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
    //$("#all").prop('checked',true).trigger('change');
}


function RemoveDatatableOrder() {
    debugger;

    DataTables.saleSummaryReportTable.dataTable({
        ordering: false
    });
   DataTables.saleSummaryReportTable.clear().rows.add(GetSaleSummary()).draw(true);
}

function OnChangeCall() {
    debugger;
    RefreshSaleSummaryTable();

}

function ViewCustomerDetail(row_obj)
{
    debugger;
    var rowData = DataTables.saleSummaryReportTable.row($(row_obj).parents('tr')).data();

    openNav();
    DataTables.saleDetailReportTable.clear().rows.add(GetSaleDetail(rowData)).draw(false);
}

function GetSaleDetail(rowData) {
    try {
        debugger
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var customer = rowData.CustomerID;
       
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode,"Customer": customer };
            var ds = {};
            ds = GetDataFromServer("Report/GetRPTViewCustomerDetail/", data);
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
