var DataTables = {};
var today = '';
$(document).ready(function () {
    try {

        DataTables.SupplierExpeditingDetailTableReportTable = $('#SupplierExpeditingDetailTable').DataTable(
         {

             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5, 7]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: true,
             ordering: false,
             paging: true,
             data: GetSupplierExpeditingDetail(),
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [

               { "data": "SupplierName", "defaultContent": "<i></i>" },
               { "data": "ContactNo", "defaultContent": "<i>-</i>" },
               { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },              
               { "data": "InvoiceDate", "defaultContent": "<i>-</i>" },
               { "data": "Amount", "defaultContent": "<i>-</i>" },
                { "data": "NoOfDays", "defaultContent": "<i>-</i>" },
                { "data": "SupplierName1", "defaultContent": "<i></i>" },
               { "data": "Remarks", "defaultContent": "<i></i>" }


             ],
             columnDefs: [{ "targets": [7], "visible": false, "searchable": false },
                  { "targets": [6], "visible": false },
                  { className: "text-left", "targets": [0, 1, 2, 3, 4] },
                  { "width": "20%", "targets": [0,1] },
             { className: "text-right", "targets": [5] }

             ]

         });

        $(".buttons-excel").hide();
        today = $("#todate").val();
    } catch (x) {

        notyAlert('error', x.message);

    }

});


function GetSupplierExpeditingDetail() {
    try {
        var todate = $("#todate").val();
        var filter = $("#BasicFilters").val();
        if (IsVaildDateFormat(todate))
            var data = { "ToDate": todate, "Filter": filter };
        var ds = {};
        ds = GetDataFromServer("Report/GetSupplierPaymentExpeditingDetails/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records.SupplierExpeditingDetailsList;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function RefreshSupplierExpeditingDetailTable() {
    debugger;
    try {
        var todate = $("#todate").val();

        if (DataTables.SupplierExpeditingDetailTableReportTable != undefined && IsVaildDateFormat(todate)) {
            DataTables.SupplierExpeditingDetailTableReportTable.clear().rows.add(GetSupplierExpeditingDetail()).draw(true);
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


function Reset() {
    debugger;
    $("#todate").val(today);
    $("#BasicFilters").val('ALL');
    RefreshSupplierExpeditingDetailTable();
}