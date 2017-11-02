var DataTables = {};
$(document).ready(function () {

    $("#STContainer").hide();
    try {

        DataTables.employeeExpenseSummaryReportAHTable = $('#employeeExpenseSummaryTable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             data: GetEmployeeExpenseSummaryReport(),
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "EmployeeCode", "defaultContent": "<i>-</i>" },
               { "data": "EmployeeName", "defaultContent": "<i>-</i>" },
               { "data": "EmployeeCompany", "defaultContent": "<i>-</i>" },
                { "data": "AccountHead", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },

             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [1, 2, 3] },
                  { className: "text-right", "targets": [4] }],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(0, { page: 'current' }).data().each(function (group, i) {
                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="3" class="rptGrp">' + '<b>Employee</b> : ' + group + '</td></tr>');
                         last = group;
                     }
                 });
             }
         });

        $(".buttons-excel").hide();

    } catch (x) {

        notyAlert('error', x.message);

    }

});


    function GetEmployeeExpenseSummaryReport() {
        try {
            var fromdate = $("#fromdate").val();
            var todate = $("#todate").val();
            var employeecode = $("#EmployeeCode").val();
            //var orderby;
            //if ($("#AH").prop('checked')) {
            //    orderby = $("#AH").val();
            //}
            //else {
            //    orderby = $("#ST").val();
            //}
            if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && employeecode) {
                var data = { "FromDate": fromdate, "ToDate": todate, "EmployeeCode": employeecode, "OrderBy": orderby };
                var ds = {};
                ds = GetDataFromServer("Report/GetEmployeeExpenseSummary/", data);
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



    function RefreshEmployeeExpenseSummaryTable() {
        try {
            var fromdate = $("#fromdate").val();
            var todate = $("#todate").val();
            var companycode = $("#EmployeeCode").val();

            if (DataTables.employeeExpenseSummaryReportTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && employeecode) {
                DataTables.employeeExpenseSummaryReportTable.clear().rows.add(GetEmployeeExpenseSummaryReport()).draw(true);
            }
        }
        catch (e) {
            notyAlert('error', e.message);
        }
    }

    function RefreshOtherExpenseSummarySTTable() {
        try {
            var fromdate = $("#fromdate").val();
            var todate = $("#todate").val();
            var employeecode = $("#EmployeeCode").val();

            if (DataTables.employeeExpenseSummaryReportTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && employeecode) {
                DataTables.employeeExpenseSummaryReportTable.clear().rows.add(GetEmployeeExpenseSummaryReport()).draw(true);
            }
        }
        catch (e) {
            notyAlert('error', e.message);
        }
    }

    //function PrintReport() {
    //    try {
    //        if ($("#AH").prop('checked')) {
    //            $("#AHContainer .buttons-excel").trigger('click');
    //        }
    //        else {
    //            $("#STContainer .buttons-excel").trigger('click');
    //        }

    //    }
    //    catch (e) {
    //        notyAlert('error', e.message);
    //    }
    //}

    function Back()
    {
        window.location = appAddress + "Report/Index/";
    }

    //function OnChangeCall() {
    //    if ($("#AH").prop('checked')) {
    //        RefreshOtherExpenseSummaryAHTable();
    //    }
    //    else {
    //        RefreshOtherExpenseSummarySTTable();
    //    }
    //}

    //function RadioOnChange(curobj) {
    //    try {

    //        var res = $(curobj).val();
    //        switch (res) {
    //            case "AH":
    //                $(".buttons-excel").hide();
    //                $("#AHContainer").show();
    //                $("#STContainer").hide();
    //                RefreshOtherExpenseSummaryAHTable();
    //                break;
    //            case "ST":
    //                $(".buttons-excel").hide();
    //                $("#AHContainer").hide();
    //                $("#STContainer").show();
    //                RefreshOtherExpenseSummarySTTable();
    //                break;
    //        }

    //    }
    //    catch (e) {
    //        notyAlert('error', e.message);
    //    }


