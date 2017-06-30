var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {
                
        DataTables.CustInvTable = $('#CustInvTable').DataTable(
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
               { "data": "InvoiceNo" },
               { "data": "InvoiceDateFormatted" },
               { "data": "Customer", "defaultContent": "<i>-</i>" },
               { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "InvoiceAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "BalanceDue", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },          
               { "data": "LastPaymentDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Status", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [5,6] },
             { className: "text-center", "targets": [1,2, 3, 4, 7,8,9] }

             ]
         });

        $('#CustInvTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });

        
        List();
       


    } catch (x) {

        notyAlert('error', e.message);

    }

});


function List() {
    var result = GetAllInvoicesAndSummary();
    if (result != null) {
        if (result.CustomerInvoices!=null)
            DataTables.CustInvTable.clear().rows.add(result.CustomerInvoices).draw(false);
        if (result.CustomerInvoiceSummary!=null) {        
            Summary(result.CustomerInvoiceSummary);
        }
    }

}
function Summary(Records) {
    $('#overdueamt').html(Records.OverdueAmountFormatted);
    $('#overdueinvoice').html(Records.OverdueInvoices);
    $('#openamt').html(Records.OpenAmountFormatted);
    $('#openinvoice').html(Records.OpenInvoices);
    $('#paidamt').html(Records.PaidAmountFormatted);
    $('#paidinvoice').html(Records.PaidInvoices);
}


function Edit(Obj) {
    alert("edit clicked")
}





//---------------Bind logics-------------------
function GetAllInvoicesAndSummary() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetInvoicesAndSummary/", data);
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
