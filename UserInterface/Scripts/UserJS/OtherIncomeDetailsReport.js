var DataTables = {};
var startdate = '';
var enddate = '';
$(document).ready(function () {


    $("#CompanyCode,#AccountCode").select2({

    });
    $("#STContainer").hide();
    try {

        DataTables.otherIncomeDetailsReportAHTable = $('#otherIncomeDetailsAHTable').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5, 6, 7]
                              }
             }],
             order: [],
             searching: false,
             paging: true,
             data: GetOtherIncomeDetailsReport(),
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
         { className: "text-center", "targets": [1] }],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(7, { page: 'current' }).data().each(function (group, i) {

                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="8" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
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


function GetOtherIncomeDetailsReport() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var AccountHead = $("#AccountCode").val();
        var search = $("#Search").val();

        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "accounthead": AccountHead,  "search": search };
            var ds = {};
            ds = GetDataFromServer("Report/GetOtherIncomeDetails/", data);
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


function RefreshOtherIncomeDetailsAHTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();

        if (DataTables.otherIncomeDetailsReportAHTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.otherIncomeDetailsReportAHTable.clear().rows.add(GetOtherIncomeDetailsReport()).draw(false);
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
    RefreshOtherIncomeDetailsAHTable();

}

function Reset() {
    debugger;
    $("#todate").val(startdate);
    $("#fromdate").val(enddate);
    $("#CompanyCode").val('ALL').trigger('change')
    $("#AccountCode").val('ALL').trigger('change')
    $("#Search").val('').trigger('change')
    RefreshOtherIncomeDetailsAHTable();
}