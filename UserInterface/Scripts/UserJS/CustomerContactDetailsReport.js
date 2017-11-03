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
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [

               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "PhoneNumber", "defaultContent": "<i>-</i>" },
                { "data": "OtherPhoneNos", "defaultContent": "<i>-</i>" },
               
               { "data": "Email", "defaultContent": "<i>-</i>" },
               { "data": "ContactName", "defaultContent": "<i>-</i>" },
               { "data": "BillingAddress","defaultContent": "<i>-</i>" },
               { "data": "ShippingAddress", "defaultContent": "<i>-</i>" }
               

             ],
             columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [0, 1,2,3,4,5,6] }
                
             ]
            
         });

        $(".buttons-excel").hide();

    } catch (x) {

        notyAlert('error', x.message);

    }

});


function GetCustomerContactDetail() {
    try {
        var search = $("#Search").val();
        var data = { "search": search };
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

function RefreshCustomerContactDetailsTable() {
    try {
        debugger;
        
        if (DataTables.CustomerContactDetailReportTable != undefined) {
            DataTables.CustomerContactDetailReportTable.clear().rows.add(GetCustomerContactDetail()).draw(true);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function OnChangeCall() {
    RefreshCustomerContactDetailsTable();

}

function Reset() {
    debugger;
    $("#Search").val('');
    RefreshCustomerContactDetailsTable();
}
