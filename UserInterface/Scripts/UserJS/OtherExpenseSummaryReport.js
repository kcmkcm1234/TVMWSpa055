var DataTables = {};
$(document).ready(function () {
    
    $("#STContainer").hide();
    try {

        DataTables.otherExpenseSummaryReportAHTable = $('#otherExpenseSummaryAHTable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             data: GetExpenseSummaryReport(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "AccountHeadORSubtype", "defaultContent": "<i>-</i>" },
               { "data": "SubTypeDesc", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "OriginCompany", "defaultContent": "<i>-</i>" },
             ],
             columnDefs: [{ "targets": [3], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [0,1] },
                  { className: "text-right", "targets": [2] }],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(3, { page: 'current' }).data().each(function (group, i) {
                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="3" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
                         last = group;
                     }
                 });
             }
         });

        $(".buttons-excel").hide();

    } catch (x) {

        notyAlert('error', x.message);

    }

    try
    {
        DataTables.otherExpenseSummaryReportSTTable = $('#otherExpenseSummarySTTable').DataTable(
      {

          // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
          dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
          buttons: [{
              extend: 'excel',
              exportOptions:
                           {
                               columns: [0, 1, 2]
                           }
          }],
          order: [],
          searching: true,
          paging: true,
          data: null,
          pageLength: 10,
          language: {
              search: "_INPUT_",
              searchPlaceholder: "Search"
          },
          columns: [
            { "data": "SubTypeDesc", "defaultContent": "<i>-</i>" },
            { "data": "AccountHeadORSubtype", "defaultContent": "<i>-</i>" },
            { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
            { "data": "OriginCompany", "defaultContent": "<i>-</i>" },
          ],
          columnDefs: [{ "targets": [3], "visible": false, "searchable": false },
               { className: "text-left", "targets": [0, 1] },
               { className: "text-right", "targets": [2] }],
          drawCallback: function (settings) {
              var api = this.api();
              var rows = api.rows({ page: 'current' }).nodes();
              var last = null;

              api.column(3, { page: 'current' }).data().each(function (group, i) {
                  if (last !== group) {
                      $(rows).eq(i).before('<tr class="group "><td colspan="3" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
                      last = group;
                  }
              });
          }
      });
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }

});


function GetExpenseSummaryReport() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var orderby;
        if($("#AH").prop('checked'))
        {
            orderby = $("#AH").val();
        }
        else
        {
            orderby = $("#ST").val();
        }
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "OrderBy": orderby };
            var ds = {};
            ds = GetDataFromServer("Report/GetOtherExpenseSummary/", data);
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


function RefreshOtherExpenseSummaryAHTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
       
        if (DataTables.otherExpenseSummaryReportAHTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.otherExpenseSummaryReportAHTable.clear().rows.add(GetExpenseSummaryReport()).draw(false);
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
        var companycode = $("#CompanyCode").val();

        if (DataTables.otherExpenseSummaryReportSTTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.otherExpenseSummaryReportSTTable.clear().rows.add(GetExpenseSummaryReport()).draw(false);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function PrintReport() {
    try {
        if ($("#AH").prop('checked')) {
            $("#AHContainer .buttons-excel").trigger('click');
        }
        else {
            $("#STContainer .buttons-excel").trigger('click');
        }
        
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Back() {
    window.location = appAddress + "Report/Index/";
}

function OnChangeCall()
{
    if ($("#AH").prop('checked')) {
        RefreshOtherExpenseSummaryAHTable();
    }
    else {
        RefreshOtherExpenseSummarySTTable();
    }
}

function RadioOnChange(curobj)
{
    try
    {
    
        var res = $(curobj).val();
        switch(res)
        {
            case "AH":
                $(".buttons-excel").hide();
                $("#AHContainer").show();
                $("#STContainer").hide();
                RefreshOtherExpenseSummaryAHTable();
                break;
            case "ST":
                $(".buttons-excel").hide();
                $("#AHContainer").hide();
                $("#STContainer").show();
                RefreshOtherExpenseSummarySTTable();
                break;
        }
       
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }

}

