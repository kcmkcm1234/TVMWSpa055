var DataTables = {};
$(document).ready(function () {


    try {

        DataTables.ReceivableAgeingSummaryReportTable = $('#ReceivableAgeingSummaryTable').DataTable(
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
             searching: true,
             paging: true,
             data: GetReceivableAgeingSummaryReport(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [

         
              
               { "data": "Customer", "defaultContent": "<i>-</i>" },
               { "data": "Current", "defaultContent": "<i>-</i>" },
               { "data": "OneToThirty", "defaultContent": "<i>-</i>" },
               { "data": "ThirtyOneToSixty", "defaultContent": "<i>-</i>" },
               { "data": "SixtyOneToNinety", "defaultContent": "<i>-</i>" },
               { "data": "NinetyOneAndOver", "defaultContent": "<i>-</i>" }
              
             ],
             columnDefs: [
             { className: "text-left", "targets": [0] },
             { className: "text-center", "targets": [1,2,3,4,5] }]
            
         });

        $(".buttons-excel").hide();

    } catch (x) {
        notyAlert('error', x.message);
    }
});


function GetReceivableAgeingSummaryReport() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode };
            var ds = {};
            ds = GetDataFromServer("Report/GetAccountsReceivableAgeingSummary/", data);
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

function RefreshReceivableAgeingReportTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        if (DataTables.ReceivableAgeingSummaryReportTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.ReceivableAgeingSummaryReportTable.clear().rows.add(GetReceivableAgeingSummaryReport()).draw(false);
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




