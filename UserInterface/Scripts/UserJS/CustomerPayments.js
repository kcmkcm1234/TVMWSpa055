var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'

$(document).ready(function () {
    debugger;

    DataTables.CustomerPaymentTable = $('#CustPayTable').DataTable(
    {
        dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
        order: [],
        searching: true,    
        paging: true,
        data: GetAllCustomerPayments(),
        columns: [
             { "data": "ID", "defaultContent": "<i>-</i>" },
             { "data": "EntryNo", "defaultContent": "<i>-</i>" },
             { "data": "PaymentDateFormatted", "defaultContent": "<i>-</i>" },
             { "data": "PaymentRef", "defaultContent": "<i>-</i>" },
             { "data": "customerObj.CompanyName", "defaultContent": "<i>-</i>" },//render customerObj.ContactPerson
             { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
             { "data": "TotalRecdAmt", "defaultContent": "<i>-</i>" },
             { "data": "AdvanceAmount", "defaultContent": "<i>-</i>" }, 
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
        ],
        columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
            { className: "text-right", "targets": [6,7] },
            { className: "text-center", "targets": [1, 2, 3,4,5,8] } 
        ]
    });    


    debugger;
    DataTables.OutStandingInvoices = $('#tblOutStandingDetails').DataTable({
        dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
        order: [],
        searching: true,
        paging:false,
        data:null,
        columns: [
             { "data": "ID", "defaultContent": "<i>-</i>" },
             { "data": "Checkbox", "defaultContent": "<i>-</i>" },
             { "data": "Description", 'render': function (data, type, row)
             { 
                 return ' Invoice # ' + row.InvoiceNo + '(Date:' + row.InvoiceDateFormatted + ')'
                 }
             },
             { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>" },
             { "data": "TotalInvoiceAmount", "defaultContent": "<i>-</i>" },
             { "data": "BalanceDue", "defaultContent": "<i>-</i>" },
             { "data": "Payment","defaultContent": "<i>-</i>" },
        ],
        columnDefs: [{
            'targets': 1,
            'searchable': false,
            'orderable': false,
            'width': '1%',
            'className': 'dt-body-center',
            'render': function (data, type,row) {
                return '<input type="checkbox">';
            }
        },{ "targets": [0], "visible": false, "searchable": false }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[2, 'asc']]
    });
});



function GetAllCustomerPayments() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("CustomerPayments/GetAllCustomerPayments/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function Edit(currentObj)
{
    debugger;
    openNav();
    var rowData = DataTables.CustomerPaymentTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null))
    {
        var thisitem = GetCustomerPayments(rowData.ID)
        //ID NOT SET THIS IN PAGE
        $('#Customer').val(thisitem.customerObj.ID);
        $('#PaymentDate').val(thisitem.PaymentDateFormatted);
        $('#PaymentRef').val(thisitem.PaymentRef);
        $('#RecdToComanyCode').val(thisitem.RecdToComanyCode);
        $('#PaymentMode').val(thisitem.PaymentMode);
        $('#BankCode').val(thisitem.BankCode);
        $('#Notes').val(thisitem.GeneralNotes);
        $('#TotalRecdAmt').val(thisitem.TotalRecdAmt);
        $('#lblTotalRecdAmt').text(thisitem.TotalRecdAmt);
    }

    debugger;
    //BIND OUTSTANDING INVOICE TABLE USING CUSTOMER ID AND PAYMENT HEADER 
    DataTables.OutStandingInvoices.clear().rows.add(GetOutStandingInvoices()).draw(false);

}


function GetCustomerPayments(ID) {
    try {
        debugger;
        var data = {"ID":ID};
        var ds = {};
        ds = GetDataFromServer("CustomerPayments/GetCustomerPaymentsByID/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetOutStandingInvoices() {
    try {
        debugger;
        var ID = $('#Customer').val();
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("CustomerPayments/GetOutStandingInvoices/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}