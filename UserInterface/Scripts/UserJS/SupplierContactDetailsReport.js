var DataTables = {};
$(document).ready(function () {
    try {

        DataTables.SupplierContactDetailReportTable = $('#SupplierContactDetailTable').DataTable(
         {

             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5,6]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             data: GetSupplierContactDetail(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [

               { "data": "SupplierName", "defaultContent": "<i>-</i>" },
               { "data": "PhoneNumber", "defaultContent": "<i>-</i>" },
                { "data": "OtherPhoneNos", "defaultContent": "<i>-</i>" },

               { "data": "Email", "defaultContent": "<i>-</i>" },
               { "data": "ContactName", "defaultContent": "<i>-</i>" },
               { "data": "BillingAddress", "defaultContent": "<i>-</i>" },
               { "data": "ShippingAddress", "defaultContent": "<i>-</i>" }


             ],
             columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [0, 1, 2, 3, 4, 5, 6] }

             ]

         });

        $(".buttons-excel").hide();

    } catch (x) {

        notyAlert('error', x.message);

    }

});


function GetSupplierContactDetail() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("Report/GetSupplierContactDetails/", data);
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