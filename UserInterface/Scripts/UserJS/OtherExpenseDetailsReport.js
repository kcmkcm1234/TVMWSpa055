var DataTables = {};
$(document).ready(function () {

    $("#STContainer").hide();
    try {

        DataTables.otherExpenseDetailsReportAHTable = $('#otherExpenseDetailsAHTable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3,4,5,6]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             data: GetOtherExpenseDetailsReport(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "Company", "defaultContent": "<i>-</i>" },
               { "data": "AccountHead", "defaultContent": "<i>-</i>" },
               { "data": "SubType", "defaultContent": "<i>-</i>" },
               { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
               { "data": "PaymentReference", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "OriginCompany", "defaultContent": "<i>-</i>" }
             ],
             columnDefs: [{ "targets": [7], "visible": false, "searchable": false },
             { className: "text-left", "targets": [0,1,2,3,4,5] },
             { className: "text-right", "targets": [6] }],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(7, { page: 'current' }).data().each(function (group, i) {
                   
                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
                         
                         last = group;
                     }
                     //if (api.column(6, { page: 'current' }).data().count() === i)
                     //{
                     //    $(rows).eq(i).after('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>Sub Total</b> : ' + group + '</td></tr>');
                     //}
                    
                 });
             }
         });

        $(".buttons-excel").hide();

    } catch (x) {

        notyAlert('error', x.message);

    }

    try {
        DataTables.otherExpenseDetailsReportSTTable = $('#otherExpenseDetailsSTTable').DataTable(
      {

          // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
          dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
          buttons: [{
              extend: 'excel',
              exportOptions:
                           {
                               columns: [0, 1, 2, 3, 4, 5, 6]
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
            { "data": "Company", "defaultContent": "<i>-</i>" },
             { "data": "SubType", "defaultContent": "<i>-</i>" },
            { "data": "AccountHead", "defaultContent": "<i>-</i>" },
           
            { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
            { "data": "PaymentReference", "defaultContent": "<i>-</i>" },
            { "data": "Description", "defaultContent": "<i>-</i>" },
            { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
            { "data": "OriginCompany", "defaultContent": "<i>-</i>" }
          ],
          columnDefs: [{ "targets": [7], "visible": false, "searchable": false },
          { className: "text-left", "targets": [0, 1, 2, 3, 4, 5] },
          { className: "text-right", "targets": [6] }],
          drawCallback: function (settings) {
              var api = this.api();
              var rows = api.rows({ page: 'current' }).nodes();
              var last = null;

              api.column(7, { page: 'current' }).data().each(function (group, i) {
                  if (last !== group) {
                      $(rows).eq(i).before('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
                      last = group;
                  }
              });
          }
      });
    }
    catch (e) {
        notyAlert('error', e.message);
    }

});


function GetOtherExpenseDetailsReport() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var orderby;
        if ($("#AH").prop('checked')) {
            orderby = $("#AH").val();
        }
        else {
            orderby = $("#ST").val();
        }
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "OrderBy": orderby };
            var ds = {};
            ds = GetDataFromServer("Report/GetOtherExpenseDetails/", data);
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


function RefreshOtherExpenseDetailsAHTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();

        if (DataTables.otherExpenseDetailsReportAHTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.otherExpenseDetailsReportAHTable.clear().rows.add(GetOtherExpenseDetailsReport()).draw(false);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function RefreshOtherExpenseDetailsSTTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();

        if (DataTables.otherExpenseDetailsReportSTTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.otherExpenseDetailsReportSTTable.clear().rows.add(GetOtherExpenseDetailsReport()).draw(false);
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

function OnChangeCall() {
    if ($("#AH").prop('checked')) {
        RefreshOtherExpenseDetailsAHTable();
    }
    else {
        RefreshOtherExpenseDetailsSTTable();
    }
}

function RadioOnChange(curobj) {
    try {

        var res = $(curobj).val();
        switch (res) {
            case "AH":
                $(".buttons-excel").hide();
                $("#AHContainer").show();
                $("#STContainer").hide();
                RefreshOtherExpenseDetailsAHTable();
                break;
            case "ST":
                $(".buttons-excel").hide();
                $("#AHContainer").hide();
                $("#STContainer").show();
                RefreshOtherExpenseDetailsSTTable();
                break;
        }

    }
    catch (e) {
        notyAlert('error', e.message);
    }

}

