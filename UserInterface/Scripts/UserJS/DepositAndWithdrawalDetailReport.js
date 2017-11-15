var DataTables = {};
var startdate = '';
var enddate = '';
$(document).ready(function () {
    $("#bankList").select2({
    });

    try {

        DataTables.depositAndWithdrawalDetailReportDetailTable = $('#depositAndWithdrawalDetailReportDetailTable').DataTable(
         {

             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: false,
             paging: true,
             data: GetDepositAndWithdrawalDetailReportDetailTable(),
             pageLength: 50,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search"
             //},
             columns: [
                { "data": "TransactionDate", "defaultContent": "<i>-</i>", "width": "15%" },
               { "data": "ReferenceNumber", "defaultContent": "<i>-</i>", "width": "5%" },
               { "data": "ReferenceBank", "defaultContent": "<i>-</i>", "width": "10%" },
               { "data": "OurBank", "defaultContent": "<i>-</i>", "width": "10%" },
               { "data": "Mode", "defaultContent": "<i>-</i>", "width": "10%" },
                { "data": "CheckClearDate", "defaultContent": "<i>-</i>", "width": "15%" },
               { "data": "Withdrawal", "defaultContent": "<i>-</i>", "width": "10%" },
               { "data": "Deposit", "defaultContent": "<i>-</i>", "width": "10%" },
               { "data": "DepositNotCleared", "defaultContent": "<i>-</i>", "width": "10%" },

             ],
             
             columnDefs: [{  "searchable": false}, //"targets": [0], "visible": false,},
                  { className: "text-left", "targets": [1, 2,3,4] },
                  { className: "text-right", "targets": [6,7,8] },
                  { className: "text-center", "targets": [0, 5] }],
             createdRow: function (row, data, index) 
             {
                 if (data.ReferenceNumber == "<b>Grant Total</b>") {

                     $('td', row).addClass('totalRow');
                 }
             },

         });

        $(".buttons-excel").hide();
        startdate = $("#todate").val();
        enddate = $("#fromdate").val();

    } catch (x) {

        notyAlert('error', x.message);

    }


});


function GetDepositAndWithdrawalDetailReportDetailTable() {
    try {
        debugger;

        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var bankList = $("#bankList").val();
        var search = $("#Search").val();
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && bankList) {
            var data = { "FromDate": fromdate, "ToDate": todate, "BankCode": bankList, "search": search };
            var ds = {};
            ds = GetDataFromServer("Report/GetDepositAndWithdrawalDetail/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            debugger;


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


function RefreshDepositAndWithdrawalDetailReportDetailTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#bankList").val();

        if (DataTables.depositAndWithdrawalDetailReportDetailTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && bankList) {
            DataTables.depositAndWithdrawalDetailReportDetailTable.clear().rows.add(GetDepositAndWithdrawalDetailReportDetailTable()).draw(true);
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

function OnChangeCall() {
    RefreshDepositAndWithdrawalDetailReportDetailTable();

}

function Reset() {
    debugger;
    $("#todate").val(startdate);
    $("#fromdate").val(enddate);
    $("#bankList").val('ALL').trigger('change');
     $("#Search").val('').trigger('change');

    RefreshDepositAndWithdrawalDetailReportDetailTable()
}




