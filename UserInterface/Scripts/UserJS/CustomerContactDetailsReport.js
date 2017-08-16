var DataTables = {};
$(document).ready(function () {
    try {

        DataTables.CustomerContactDetailReportTable = $('#CustomerContactDetailTable').DataTable(
         {

             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             data: GetCustomerContactDetail(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [

               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "PhoneNumber", "defaultContent": "<i>-</i>" },
               { "data": "Email", "defaultContent": "<i>-</i>" },
               { "data": "ContactName", "defaultContent": "<i>-</i>" },
               { "data": "BillingAddress","defaultContent": "<i>-</i>" },
               { "data": "ShippingAddress", "defaultContent": "<i>-</i>" }
               

             ],
             columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [0, 1,2,3,4,5] }
                
             ]
            
         });

        $(".buttons-excel").hide();

    } catch (x) {

        notyAlert('error', x.message);

    }

});


function GetCustomerContactDetail() {
    try {
             var data = {};
            var ds = {};
            ds = GetDataFromServer("Report/GetCustomerContactDetails/", data);
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