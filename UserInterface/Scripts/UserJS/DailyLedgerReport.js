var DataTables = {};
var startdate = '';
var enddate = '';
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
                                     columns: [0, 1, 2, 3, 4, 5]
                                 }
                }],
                
                order: [],
                searching: false,
                paging: true,
                data: GetDailyLedgerDetails(),
                pageLength: 50,
                columns: [
               { "data": "TransactionDate", "defaultContent": "<i>-</i>", "width": "10%" },
               { "data": "EntryType", "defaultContent": "<i>-</i>", "width": "10%" },
               { "data": "Particulars", "defaultContent": "<i>-</i>", "width": "40%" },
               { "data": "CustomerORemployee", "defaultContent": "<i></i>", "width": "5%" },
               { "data": "Debit", render: function (data, type, row) { return formatCurrency(data); }, "defaultContent": "<i>-</i>", "width": "5%" },
               { "data": "Credit", render: function (data, type, row) { return formatCurrency(data); }, "defaultContent": "<i>-</i>", "width": "5%" },
             
                ],
                columnDefs: [{ "searchable": false },
                  { "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5] },
                  { className: "text-left", "targets": [1, 2, 3] },
                  { className: "text-right", "targets": [4,5] },
                  { className: "text-center", "targets": [0] }
                    
                ]
            });
        $(".buttons-excel").hide();
        startdate = $("#todate").val();
        enddate = $("#fromdate").val();
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
        var bank = $("#BankCode").val();
        if ((IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate)) || IsVaildDateFormat(ondate))
        {
            var data = { "FromDate": fromdate, "ToDate": todate, "OnDate": ondate, "MainHead": mainHead, "search": search, "Bank": bank };
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
            DataTables.DailyLedgerTable.clear().rows.add(GetDailyLedgerDetails()).draw(true);
        }
    }
    catch (e) {
        //notyAlert('error', e.message);
    }
}


function OnChangeCall() {
   
    if($("#MainHead").val()=='Bank')
    {
        $("#BankCode").prop('disabled', false);
    }
    else
    {
        $("#BankCode").val('ALL');
        $("#BankCode").prop('disabled', true);
    }
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
    $("#todate").val(startdate);
    $("#fromdate").val(enddate);
    $("#MainHead").val('All').trigger('change')
    $("#Search").val('').trigger('change')
    $("#BankCode").val('').trigger('change')
    RefreshDailyLedgerDetails()
}


