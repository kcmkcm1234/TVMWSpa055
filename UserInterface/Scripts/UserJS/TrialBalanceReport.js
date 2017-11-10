var DataTables = {};
var CurrentDate = '';
 
$(document).ready(function () {
    debugger;

    try { 
        DataTables.tableTrialBalance = $('#tblTrialBalance').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2]
                              }
             }],
             order: [],
             searching: false,
             paging: true,
             data: GetTrialBalanceReport(),
             pageLength: 50,

             columns: [
                { "data": "Account", "defaultContent": "<i>-</i>" },
               {
                   "data": "Debit", render: function (data, type, row) {

                       return roundoff(data, 1);

                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "Credit", render: function (data, type, row) {
                       return roundoff(data, 1);
                   }, "defaultContent": "<i>-</i>"
               }  
             ],
             columnDefs: [
             { className: "text-left", "targets": [0] },
              { "width": "50%", "targets": [0] },
               { "width": "10%", "targets": [1,2] },
             { className: "text-right", "targets": [1,2] },
         { className: "text-center", "targets": [] },
            { "bSortable": false, "aTargets": [0, 1, 2] }],
             createdRow: function (row, data, index) {
                 if (data.Account == "<b>Total</b>") {

                $('td', row).addClass('totalRow');
            }
        },
         });

        $(".buttons-excel").hide(); 

    } catch (x) {

        notyAlert('error', x.message);

    }
    CurrentDate = $("#Date").val();


});


function GetTrialBalanceReport() {
    try {
        debugger;
        var date = $("#Date").val();

        if (IsVaildDateFormat(date)) {
            var data = { "Date": date};
            var ds = {};
            ds = GetDataFromServerTraditional("Report/GetTrialBalanceReport/", data);
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

function DateOnChange() {
    debugger;
    if ($("#Date").val() != '' && DataTables.tableTrialBalance!=undefined) {
        DataTables.tableTrialBalance.clear().rows.add(GetTrialBalanceReport()).draw(true);
    }
}



function Reset() {
    debugger;
    $("#Date").val(CurrentDate);
    DataTables.tableTrialBalance.clear().rows.add(GetTrialBalanceReport()).draw(true);
}


