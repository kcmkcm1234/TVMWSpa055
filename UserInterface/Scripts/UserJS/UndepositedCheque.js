
var DataTables = {};
$(document).ready(function () {
    debugger; 
    try {
        debugger;

        DataTables.undepositedChequeTable = $('#undepositedChequeTable').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4]
                              }
             }],
             order: [],
             searching: false,
             paging: true,
             data: GetUndepositedChequeTable(),
             pageLength: 50,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search", "width": "15%" 
             //},
             columns: [
                { "data": "DateFormatted", "defaultContent": "<i>-</i>", "width": "15%" },
               { "data": "ReferenceNo", "defaultContent": "<i>-</i>", "width": "15%" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>", "width": "15%" },
               { "data": "ReferenceBank", "defaultContent": "<i>-</i>", "width": "15%" },
               { "data": "Amount", "defaultContent": "<i>-</i>", "width": "10%" }
             //{ "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "searchable": false }, 
                  { className: "text-left", "targets": [1, 2,3] },
                  { className: "text-right", "targets": [4] },
                  { className: "text-center", "targets": [0] }]
         });

        $(".buttons-excel").hide();

    } catch (x) {

        notyAlert('error', x.message);
    }
});


function GetUndepositedChequeTable() {
    try {
        debugger;
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();

        if ((fromdate == "" ? true : IsVaildDateFormat(fromdate)) && IsVaildDateFormat(todate)) {
            var data = { "FromDate": fromdate, "ToDate": todate };
            var ds = {};
            ds = GetDataFromServer("DepositAndWithdrawals/GetUndepositedCheque/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            debugger;


            if (ds.Result == "OK") {
               
                $("#fromdate").val(ds.FromDate);
                return ds.Records;
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return ds.Records;
            }
        }   
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function RefreshUndepositedChequeTable() {
    try {
        debugger;
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();

        if (DataTables.undepositedChequeTable != undefined && (fromdate==""?true:IsVaildDateFormat(fromdate)) && IsVaildDateFormat(todate)) {
            var records = GetUndepositedChequeTable();
            if (records != undefined)
                DataTables.undepositedChequeTable.clear().rows.add(records).draw(false);

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
    window.location = appAddress + "DepositAndWithdrawals/Index/";
}

function OnChangeCall() {
    RefreshUndepositedChequeTable();

}

function Reset() {
    RefreshUndepositedChequeTable()
}
