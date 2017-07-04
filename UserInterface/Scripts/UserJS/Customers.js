var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.CustInvTable = $('#CustomerTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: null,
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "ID" },
               { "data": "CompanyName" },
               { "data": "ContactPerson" },
               { "data": "ContactTitle", "defaultContent": "<i>-</i>" },
               { "data": "Mobile", "defaultContent": "<i>-</i>" },
               { "data": "TaxRegNo", "defaultContent": "<i>-</i>" },
               { "data": "PANNo",  "defaultContent": "<i>-</i>" },               
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [5, 6] },
             { className: "text-center", "targets": [1, 2, 3, 4, 7] }

             ]
         });

        $('#CustomerTable tbody').on('dblclick', 'td', function () {

            //Edit(this);
        });

      
     



    } catch (x) {

        notyAlert('error', e.message);

    }

});