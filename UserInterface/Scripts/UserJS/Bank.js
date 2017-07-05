var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.BankTable = $('#BankTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllBanks(),
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "Code" },
               { "data": "Name" },
               { "data": "CompanyCode" },             
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [] },
             { className: "text-center", "targets": [0,1, 2] }

             ]
         });

        $('#BankTable tbody').on('dblclick', 'td', function () {

            //Edit(this);
        });






    } catch (x) {

        notyAlert('error', x.message);

    }

});

function GetAllBanks() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("Bank/GetAllBanks/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}