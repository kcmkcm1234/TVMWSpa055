

$(document).ready(function () {
    debugger;

    //DataTables.CustomerPaymentTable = $('#CustPayTable').DataTable(
    //{
    //    dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
    //    order: [],
    //    searching: true,    
    //    paging: true,
    //    data: GetAllCustomerPayments(),
    //    columns: [
    //         { "data": "ID", "defaultContent": "<i>-</i>" },
    //         { "data": "PaymentDateFormatted", "defaultContent": "<i>-</i>" },
    //         { "data": "PaymentRef", "defaultContent": "<i>-</i>" },
    //         { "data": "Customer", "defaultContent": "<i>-</i>" },
    //         { "data": "PaymentMode ", "defaultContent": "<i>-</i>" },
    //         { "data": "AmountReceived ", "defaultContent": "<i>-</i>" },
    //         { "data": "AdvanceReceived ", "defaultContent": "<i>-</i>" },
    //         { "data": "LastPaymentDate ", "defaultContent": "<i>-</i>" },
    //         { "data": "Status", "defaultContent": "<i>-</i>" },
    //         { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
    //    ],
    //    columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
    //        { className: "text-right", "targets": [8] },
    //        { className: "text-center", "targets": [1, 2, 3, 5, 6, 9] },
    //        { className: "text-left", "targets": [7] },
    //    ]
    //});  




  
    $('#tblOutStandingDetails').DataTable({
        dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
        order: [],
        searching: true,
        paging: false,
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 1
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[2, 'asc']]
    });
});