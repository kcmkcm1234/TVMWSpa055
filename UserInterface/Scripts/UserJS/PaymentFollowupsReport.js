var DataTables = {};
var today = '';
var fromday = '';
$(document).ready(function () {
    try {
        $("#ddlCustomer").select2({
            placeholder: "Select a Customers..",

        });
        DataTables.paymentFollowupsTable = $('#PaymentFollowupsTable').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [ 1, 2,3,4,5,6,7]
                             }
            }],
            order: [],
            searching: false,
            paging: true,
            data: GetFollowupReport(),
            pageLength: 15,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
              { "data": "ID", "defaultContent": "<i>-</i>" },
              { "data": "FollowUpDate", "defaultContent": "<i>-</i>" },
              { "data": "FollowUpTime", "defaultContent": "<i>-</i>" },
              { "data": "Status", "defaultContent": "<i>-</i>" },
              { "data": "CutomerName", "defaultContent": "<i>-</i>" },
              { "data": "ContactName", "defaultContent": "<i>-</i>" },
              { "data": "ContactNO", "defaultContent": "<i>-</i>" },
              { "data": "Remarks", "defaultContent": "<i>-</i>" } 
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-left", "targets": [2, 3, 4, 5, 6] },
                  { "width": "20%", "targets": [7] },
           { className: "text-center", "targets": [1] }

            ]
           
        });

        today = $("#todate").val();
        fromday = $("#fromdate").val();

        $(".buttons-excel").hide();


    } catch (x) {

        notyAlert('error', x.message);

    }

});

//Followup details
function GetFollowupReport(paymentFollowupAdvanceSearchObj) {
    try {
        debugger;
        if (paymentFollowupAdvanceSearchObj === 0) {
            var data = {};
        }
        else {
            var data = { "paymentFollowupAdvanceSearchObj": JSON.stringify(paymentFollowupAdvanceSearchObj) };
        }
        var ds = {};
        ds = GetDataFromServer("Report/GetFollowupReport/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records.FollowUpList;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function RefreshFollowupTable() {
    debugger;
    try {
        if (DataTables.paymentFollowupsTable != undefined) {
        var toDate = $("#todate");
        var fromDate = $("#fromdate");
        var status = $("#ddlStatus");
        var customer = $("#ddlCustomer");
        var Search = $("#Search");
        var followupAdvanceSearchObj = new Object();
        followupAdvanceSearchObj.FromDate = fromDate[0].value !== "" ? fromDate[0].value : null;
        followupAdvanceSearchObj.ToDate = toDate[0].value !== "" ? toDate[0].value : null;
        followupAdvanceSearchObj.Status = status[0].value !== "" ? status[0].value : null;
        followupAdvanceSearchObj.Customer = customer[0].value !== "" ? $("#ddlCustomer").val() : null;
        followupAdvanceSearchObj.Search = Search[0].value !== "" ? Search[0].value : null;
            DataTables.paymentFollowupsTable.clear().rows.add(GetFollowupReport(followupAdvanceSearchObj)).draw(false);
        }
    }

    catch (e) {
        notyAlert('error', e.message);
    }
}

function Back() {
    window.location = appAddress + "Report/Index/";
}
function PrintReport() {
    try {

        $(".buttons-excel").trigger('click');


    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Reset() {
    debugger;
    $("#todate").val(today);
    $("#fromdate").val(fromday);
    $("#CompanyCode").val('ALL');
    $("#ddlStatus").val('Open');
    $("#Search").val('');
    $("#ddlCustomer").val('').trigger('change');
    RefreshReceivableAgeingReportTable();
}