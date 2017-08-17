var DataTables = {};
$(document).ready(function () {

   
    try {

        DataTables.SalesTransactionLogReportTable = $('#salesTransactionTable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2,3,4]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             data: GetSalesTransactionLogReport(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
              
               { "data": "OriginatedCompany", "defaultContent": "<i>-</i>" },
               { "data": "DocNo", "defaultContent": "<i>-</i>" },
               { "data": "Date", "defaultContent": "<i>-</i>" },
               { "data": "TransactionType", "defaultContent": "<i>-</i>" },
               { "data": "Amount",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Remarks", "defaultContent": "<i>-</i>" },
               { "data": "CompanyCode", "defaultContent": "<i>-</i>" }

              
             ],
             columnDefs: [{ "targets": [5,6], "visible": false, "searchable": false },
             { className: "text-left", "targets": [0, 1, 2, 3] },
             { className: "text-right", "targets": [4] }],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(0, { page: 'current' }).data().each(function (group, i) {

                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="5" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
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


function GetSalesTransactionLogReport() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
       
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode };
            var ds = {};
            ds = GetDataFromServer("Report/GetsalesTransactionLog/", data);
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


function RefreshSalesTransactionLogTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();

        if (DataTables.SalesTransactionLogReportTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.SalesTransactionLogReportTable.clear().rows.add(GetSalesTransactionLogReport()).draw(false);
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




