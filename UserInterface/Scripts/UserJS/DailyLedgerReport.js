var DataTables = {};
$(document).ready(function () {
    
    try {
        debugger;
        $("#MainHead").select2();
        DataTables.DailyLedgerTable = $('#dailyLedgerDetailAHTable').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                                 }
                }],
                order: [],
                searching: false,
                paging: true,
                data: GetDailyLedgerDetails(),
                pageLength: 50,
                columns: [
               { "data": "TransactionDate", "defaultContent": "<i>-</i>" },
               { "data": "EntryType", "defaultContent": "<i>-</i>" },
               { "data": "MainHead", "defaultContent": "<i>-</i>" },
               { "data": "AccountHead", "defaultContent": "<i>-</i>" },
               { "data": "ReferenceNo", "defaultContent": "<i>-</i>" },
               { "data": "CustomerORemployee", "defaultContent": "<i>-</i>" },
                 { "data": "PayMode", "defaultContent": "<i>-</i>" },
               { "data": "Remarks", "defaultContent": "<i>-</i>" },
               { "data": "Debit", "defaultContent": "<i>-</i>" },
               { "data": "Credit", "defaultContent": "<i>-</i>" },
             
                ],
                columnDefs: [{  "searchable": false}, 
                  { className: "text-left", "targets": [1, 2, 3, 4, 5, 6, 7] },
                  { className: "text-right", "targets": [8, 9] },
                  { className: "text-center", "targets": [0] }]
            });
        $(".buttons-excel").hide();
        if($('.DateFilterDiv').is(":hidden"))
        {
            $('.DateFilterDiv').find('input').prop('disabled', true);
        }
        if($('.SingleDateFilterDiv').is(":hidden"))
        {
            $('.SingleDateFilterDiv').find('input').prop('disabled', true);
        }
                
                

    } catch (x) {
        //console.Write(x.message);
    }
});


function GetDailyLedgerDetails(){
    try {
        debugger;
        var fromdate = $('.DateFilterDiv').is(":hidden") ? "" : $("#fromdate").val();
        var todate = $('.DateFilterDiv').is(":hidden") ? "" : $("#todate").val();
        var ondate = $('.SingleDateFilterDiv').is(":hidden") ? "" : $("#ondate").val();
        var mainHead = $("#MainHead").val();
        var search = $("#Search").val();
        if ((IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate)) || IsVaildDateFormat(ondate))
        {
            var data = { "FromDate": fromdate, "ToDate": todate, "OnDate": ondate, "MainHead": mainHead, "search": search };
            var ds = {};
            ds = GetDataFromServer("Report/GetDailyLedgerDetails/", data);
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
     catch (x) {
            //console.Write(x.message);
        }
}


function RefreshDailyLedgerDetails() {
    try {
        var fromdate = $('.DateFilterDiv').is(":hidden")?"":$("#fromdate").val();
        var todate = $('.DateFilterDiv').is(":hidden") ? "" : $("#todate").val();
        var ondate = $('.SingleDateFilterDiv').is(":hidden") ? "" : $("#ondate").val();
        var mainHead = $("#MainHead").val();

        if ((DataTables.DailyLedgerTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) ) || (DataTables.DailyLedgerTable != undefined && IsVaildDateFormat(ondate)))
        {
            DataTables.DailyLedgerTable.clear().rows.add(GetDailyLedgerDetails()).draw(false);
        }
    }
    catch (e) {
        //notyAlert('error', e.message);
    }
}


function OnChangeCall() {
    RefreshDailyLedgerDetails();

}


function Back() {
    window.location = appAddress + "Report/Index/";
}

function PrintReport() {
    try {

        $(".buttons-excel").trigger('click');


    }
    catch (e) {
        //notyAlert('error', e.message);
    }
}

function Reset() {
    debugger;
    $("#MainHead").val('All').trigger('change')
    $("#Search").val('').trigger('change')
    RefreshDailyLedgerDetails()
}


