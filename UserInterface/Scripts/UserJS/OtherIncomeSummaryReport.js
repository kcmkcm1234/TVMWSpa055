var DataTables = {};
var startdate = '';
var enddate = '';
$(document).ready(function () {
    debugger;
    $("#CompanyCode,#AccountCode").select2({
    });
    $("#STContainer").hide();
    try {

        DataTables.otherIncomeSummaryReportAHTable = $('#otherIncomeSummaryAHTable').DataTable(
         {

             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 
                 exportOptions:
                              {
                                  columns: [0, 1,3]
                              }
             }],
             order: [],
             searching: false,
             paging: true,
             data: GetIncomeSummaryReport(),
             pageLength: 50,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search"
             //},
             columns: [
               
               {
                   "data": "AccountHeadORSubtype", "defaultContent": "<i>-</i>", render: function (data, type, row) { 
                       return '<a href="#" onclick="ViewOtherIncomeDetail(this);">' + data + ' </a>';
                   }
               },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             { "data": "OriginCompany", "defaultContent": "<i>-</i>" },
             ],
             columnDefs: [{ "targets": [2], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [0] },
                  { className: "text-right", "targets": [1,2] }]
             ,drawCallback: function (settings) {
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

        DataTables.otherIncomeDetailsReportAHTable = $('#otherIncomeDetailsAHTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">', 
             order: [],
             searching: false,
             paging: true,
             data:null,
             pageLength: 50,

             columns: [
               { "data": "Company", "defaultContent": "<i>-</i>" },
                { "data": "Date", "defaultContent": "<i>-</i>" },
               { "data": "AccountHead", "defaultContent": "<i>-</i>" },
               { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
               { "data": "PaymentReference", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "OriginCompany", "defaultContent": "<i>-</i>" }
             ],
             columnDefs: [{ "targets": [7], "visible": false, "searchable": false },
             { className: "text-left", "targets": [0, 2, 3, 4, 5] },
                { "width": "15%", "targets": [0] },
               { "width": "10%", "targets": [1] },
             { className: "text-right", "targets": [6] },
         { className: "text-center", "targets": [1] }] 
         });

    } catch (x) {
        notyAlert('error', x.message);
    }

});


function GetIncomeSummaryReport() {
    try {
        debugger;
        
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var AccountHead = $("#AccountCode").val();
        var search = $("#Search").val();
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate , "CompanyCode": companycode, "accounthead": AccountHead, "search": search };
            var ds = {};
            ds = GetDataFromServer("Report/GetOtherIncomeSummary/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            debugger;
            if (ds.TotalAmount != '') {
                $("#otherincomeamount").text(ds.TotalAmount);
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


function RefreshOtherIncomeSummaryAHTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();

        if (DataTables.otherIncomeSummaryReportAHTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.otherIncomeSummaryReportAHTable.clear().rows.add(GetIncomeSummaryReport()).draw(false);
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
    RefreshOtherIncomeSummaryAHTable();

}

function Reset() {
    debugger;

    $("#todate").val(startdate);
    $("#fromdate").val(enddate);
    $("#CompanyCode").val('ALL').trigger('change')
    $("#AccountCode").val('ALL').trigger('change')
    $("#Search").val('').trigger('change')
    RefreshOtherIncomeSummaryAHTable();
}


function ViewOtherIncomeDetail(row_obj) {
    debugger;
    var rowData = DataTables.otherIncomeSummaryReportAHTable.row($(row_obj).parents('tr')).data();

    openNav();
    DataTables.otherIncomeDetailsReportAHTable.clear().rows.add(GetIncomeDetailsReport(rowData)).draw(false);
}

function GetIncomeDetailsReport(rowData) {
    try {
        debugger;
        $("#lblDetailsHead").text(rowData.AccountHeadORSubtype);
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var AccountHead = rowData.AccountHead;


        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "accounthead": AccountHead};
            var ds = {};
            ds = GetDataFromServer("Report/GetOtherIncomeDetailsReport/", data);
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