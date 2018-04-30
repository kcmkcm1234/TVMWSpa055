var DataTables = {};
var startdate = '';
var enddate = '';
$(document).ready(function () {

    try {
        debugger;
 

        DataTables.DailyLedgerTable = $('#bankLedgerDetailAHTable').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0, 1, 2, 3, 4, 5,6]
                                 }
                }],

                order: [],
                fixedHeader: {
                    header: true
                },
                searching: false,
                paging: true,
                data: GetBankLedgerDetails(),
                pageLength: 50,
                columns: [
               { "data": "TransactionDate", "defaultContent": "<i>-</i>", "width": "10%" },
               { "data": "EntryType", "defaultContent": "<i>-</i>", "width": "15%" },
               { "data": "Particulars", "defaultContent": "<i>-</i>", "width": "50%" },
               { "data": "CustomerORemployee", "defaultContent": "<i></i>", "width": "10%" },
               { "data": "Debit", render: function (data, type, row) { return formatCurrency(data); }, "defaultContent": "<i>-</i>", "width": "5%" },
               { "data": "Credit", render: function (data, type, row) { return formatCurrency(data); }, "defaultContent": "<i>-</i>", "width": "5%" },
               { "data": "Balance", render: function (data, type, row) { return formatCurrency(data); }, "defaultContent": "<i>-</i>", "width": "5%" },

                ],
                columnDefs: [{ "searchable": false },
                  { "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5,6] },
                  { className: "text-left", "targets": [1, 2, 3] },
                  { className: "text-right", "targets": [4, 5,6] },
                  { className: "text-center", "targets": [0] }

                ],
                createdRow: function (row, data, index) {

                    if (data.EntryType == "<b>Opening</b>") {

                        $('td', row).addClass('opeingRow');
                    }
                    if (data.EntryType == "<b>Closing(Cr)</b>" || data.EntryType == "<b>Closing(Dr)</b>") {

                        $('td', row).addClass('closingRow');
                    }
                    if (data.EntryType == "<b>Total</b>") {

                        $('td', row).addClass('totalRow');
                    }

                }

            });
        $(".buttons-excel").hide();
        startdate = $("#todate").val();
        enddate = $("#fromdate").val();
        if ($('.DateFilterDiv').is(":hidden")) {
            $('.DateFilterDiv').find('input').prop('disabled', true);
        }
        if ($('.SingleDateFilterDiv').is(":hidden")) {
            $('.SingleDateFilterDiv').find('input').prop('disabled', true);
        }



    } catch (x) {
        //console.Write(x.message);
    }
});


function GetBankLedgerDetails() {
    try {
        debugger;
        var fromdate = $('.DateFilterDiv').is(":hidden") ? "" : $("#fromdate").val();
        var todate = $('.DateFilterDiv').is(":hidden") ? "" : $("#todate").val();
        var ondate = $('.SingleDateFilterDiv').is(":hidden") ? "" : $("#ondate").val();        
        var search = $("#Search").val();
        var bank = $("#BankCode").val();
        if ((IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate)) || IsVaildDateFormat(ondate)) {
            var data = { "FromDate": fromdate, "ToDate": todate, "OnDate": ondate,   "search": search, "Bank": bank };
            var ds = {};
            ds = GetDataFromServer("Report/GetBankLedgerDetails/", data);
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
        var fromdate = $('.DateFilterDiv').is(":hidden") ? "" : $("#fromdate").val();
        var todate = $('.DateFilterDiv').is(":hidden") ? "" : $("#todate").val();
        var ondate = $('.SingleDateFilterDiv').is(":hidden") ? "" : $("#ondate").val();
       

        if ((DataTables.DailyLedgerTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate)) || (DataTables.DailyLedgerTable != undefined && IsVaildDateFormat(ondate))) {
            DataTables.DailyLedgerTable.clear().rows.add(GetBankLedgerDetails()).draw(true);
        }
    }
    catch (e) {
        //notyAlert('error', e.message);
    }
}


function OnChangeCall() {
    $("#BankCode").prop('disabled', false);
    
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
    
    $("#Search").val('').trigger('change')
    $("#BankCode").val('').trigger('change')
    RefreshDailyLedgerDetails()
}


