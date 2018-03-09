var DataTables = {};
var startdate = '';
var enddate = '';
$(document).ready(function () {
    $("#CompanyCode,#AccountHead").select2({
    });
  
    $("#STContainer").hide();
    try {

        DataTables.accountHeadGroupReportSummaryTable = $('#accountHeadGroupSummaryTable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             //dom: 'Bfrtip',
             buttons: [{
                 extend: 'excel',
                 
                 exportOptions:
                              {
                                  columns: [1, 2,3,4]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: false,
             paging: true,
             data: GetAccountHeadGroupSummaryReport(),
             pageLength: 50,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search"
             //},
             columns: [
               { "data": "ID", "defaultContent": "<i>-</i>"},
               { "data": "GroupName", "defaultContent": "<i>-</i>", },
               { "data": "CompanyCode", "defaultContent": "<i>-</i>" },
               //{ "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "PaidAmount", render: function (data, type, row) { 
                   return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               {
                   "data": "ReversedAmount", render: function (data, type, row) {
                       return roundoff(data, 1);
                   }, "defaultContent": "<i>-</i>"
               }
             
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [1,2] },
                  { className: "text-right", "targets": [3,4] }],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(2, { page: 'current' }).data().each(function (group, i) {
                     debugger;
                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="3" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
                         last = group;
                     }
                 });
             }
         });

        $(".buttons-excel").hide();
        startdate = $("#todate").val();
        enddate = $("#fromdate").val();
    } catch (x) {

        notyAlert('error', x.message);

    }
});



function GetAccountHeadGroupSummaryReport(accountHeadGroupSummaryAdvanceSearch) {

        debugger;
        try {
            if (accountHeadGroupSummaryAdvanceSearch === 0) {
                var data = {};
            }
            else {
                var data = { "accountHeadGroupSummaryAdvanceSearchObject": JSON.stringify(accountHeadGroupSummaryAdvanceSearch) };
            }
            var data;
            var ds = {};
            ds = GetDataFromServer("Report/GetOtherExpenseAccountHeadGroupSummaryReport/", data);
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
        catch (e) {
            notyAlert('error', e.message);
        }
    }


function AdvanceSearchContent() {
    debugger;
    var expenseType = $("#ExpenseType");
    var fromDate = $("#fromdate");
    var toDate = $("#todate");
    var company = $("#Company");
    var groupName = $("#GroupName");
    var search = $("#Search");
    var accountHeadGroupSummaryAdvanceSearch = new Object();
    accountHeadGroupSummaryAdvanceSearch.ExpenseType = expenseType[0].value !== "" ? expenseType[0].value : null;
    accountHeadGroupSummaryAdvanceSearch.FromDate = fromDate[0].value !== "" ? fromDate[0].value : null;
    accountHeadGroupSummaryAdvanceSearch.ToDate = toDate[0].value !== "" ? toDate[0].value : null;
    accountHeadGroupSummaryAdvanceSearch.Company = company[0].value !== "" ? company[0].value : null;
    accountHeadGroupSummaryAdvanceSearch.GroupName = groupName[0].value !== "" ? groupName[0].value : null;
    accountHeadGroupSummaryAdvanceSearch.Search = search[0].value !== "" ? search[0].value : null;

    DataTables.accountHeadGroupReportSummaryTable.clear().rows.add(GetAccountHeadGroupSummaryReport(accountHeadGroupSummaryAdvanceSearch)).draw(false);
}


function Reset() {
    debugger;
    $("#todate").val(startdate);
    $("#fromdate").val(enddate);
    $("#Company").val('ALL');
    $("#GroupName").val('ALL').trigger('change');
    $("#search").val('');
    $("#ExpenseType").val('ALL').trigger('change')
    AdvanceSearchContent();
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