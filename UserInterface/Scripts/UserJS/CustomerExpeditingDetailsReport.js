var DataTables = {};
var today = '';
$(document).ready(function () {
    try {
       
        DataTables.CustomerExpeditingDetailTableReportTable = $('#CustomerExpeditingDetailTable').DataTable(
         {

             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5,7]
                              }
             }],
             order: [],
             searching: true,
             ordering: false,
             paging: true,
             data: GetCustomerExpeditingDetail(),
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [

               { "data": "CustomerName", "defaultContent": "<i></i>" },
               { "data": "ContactNo", "defaultContent": "<i>-</i>" },
               { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },             
               { "data": "InvoiceDate", "defaultContent": "<i>-</i>" },
               { "data": "Amount", "defaultContent": "<i>-</i>" },
                 { "data": "NoOfDays", "defaultContent": "<i>-</i>" },
                  { "data": "CustomerName1", "defaultContent": "<i></i>" },
               { "data": "Remarks", "defaultContent": "<i></i>" }


             ],
             columnDefs: [{ "targets": [7], "visible": false, "searchable": false },
                 { "targets": [6], "visible": false },
                  { className: "text-left", "targets": [0,1,2, 3, 4,5] },
                  { "width": "20%", "targets": [0,1] },
             { className: "text-right", "targets": [4] }

                 ]

         });

        $(".buttons-excel").hide();
     today = $("#todate").val();
    } catch (x) {

        notyAlert('error', x.message);

    }

});


function GetCustomerExpeditingDetail() {
    try {
        var todate = $("#todate").val();
        if (IsVaildDateFormat(todate))
            var data = {"ToDate": todate};
        var ds = {};
        ds = GetDataFromServer("Report/GetCustomerPaymentExpeditingDetails/", data);
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


function RefreshCustomerExpeditingDetailTable() {
    debugger;
    try {
        var todate = $("#todate").val();

        if (DataTables.CustomerExpeditingDetailTableReportTable != undefined && IsVaildDateFormat(todate)) {
            DataTables.CustomerExpeditingDetailTableReportTable.clear().rows.add(GetCustomerExpeditingDetail()).draw(false);
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

function Reset()
{
    debugger;
    $("#todate").val(today);
    RefreshCustomerExpeditingDetailTable();
}