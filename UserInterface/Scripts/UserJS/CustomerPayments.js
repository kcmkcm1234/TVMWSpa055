var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var sum = 0;
var index = 0;
var AmountReceived;

$(document).ready(function () {
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

 
    DataTables.OutStandingInvoices = $('#tblOutStandingDetails').DataTable({
        dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
        order: [],
        searching: true,
        paging:false,
        data:null,
        columns: [
             { "data": "ID", "defaultContent": "<i>-</i>" },
             { "data": "Checkbox","defaultContent":""},
             {
                 "data": "Description", 'render': function (data, type, row) {
                     return ' Invoice # ' + row.InvoiceNo + '(Date:' + row.InvoiceDateFormatted + ')'
                 }, "width": "30%"
                },
             { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>", "width": "20%" },
             { "data": "TotalInvoiceAmount", "defaultContent": "<i>-</i>", "width": "15%",'render': function (data, type, row) {
                 return roundoff(row.TotalInvoiceAmount)
             } },
             {
                 "data": "BalanceDue", "defaultContent": "<i>-</i>", "width": "15%", 'render': function (data, type, row) {
                     return roundoff(row.BalanceDue)
                 }
             },
             {
                 "data": "Payment", 'render': function (data, type, row) {
                     index = index+1
                     return '<input class="form-control text-right paymentAmount" name="Markup" onchange="PaymentAmountChanged();" id="PaymentValue_' + index + '" type="text">';
                 }, "width": "15%"
             }
        ],
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 1
        }, { className: "text-right", "targets": [4,5] }

        ,{ "targets": [0], "visible": false, "searchable": false }],
        select: {
            style: 'multi',
            selector: 'tr'
        }
    });


    $('input[type="text"].selecttext').on('focus', function () {
        $(this).select();
    });
    
});

function GetAllCustomerPayments() {
    try { 
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
    openNav();
    var rowData = DataTables.CustomerPaymentTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null))
    {
        var thisitem = GetCustomerPayments(rowData.ID)
        //ID NOT SET THIS IN PAGE
        $('#ID').val(rowData.ID);
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
    //BIND OUTSTANDING INVOICE TABLE USING CUSTOMER ID AND PAYMENT HEADER 
    BindOutstanding();

}

function openNavClick() {
    $('#CustomerPaymentForm')[0].reset();
    BindOutstanding();
    openNav();
} 

function BindOutstanding() {
    DataTables.OutStandingInvoices.clear().rows.add(GetOutStandingInvoices()).draw(false);
}

function PaymentModeChanged() {
    if ($('#PaymentMode').val()=="ONLINE")
        $("#depositTo").css("visibility", "visible");
    else
    $("#depositTo").css("visibility", "hidden");

}

function GetCustomerPayments(ID) {
    try {
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
        var ID = $('#Customer').val();
        if (ID == "")
            ID = emptyGUID;
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

function AmountChanged() {
    DataTables.OutStandingInvoices.rows().deselect();
    debugger; 
    AmountReceived = roundoff(parseFloat($('#TotalRecdAmt').val()))
    $('#TotalRecdAmt').val(AmountReceived);
    $('#lblTotalRecdAmt').text(AmountReceived);
    $('#paidAmt').text(AmountReceived);
    

    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    var data = table.column(5).data();
    var RemainingAmount = AmountReceived;
   
    for (var i = 0; i < allData.length;i++)
    {
        if (parseFloat(data[i]) < RemainingAmount) {
            $("#PaymentValue_" + (i + 1)).val( roundoff(parseFloat(data[i])))
            DataTables.OutStandingInvoices.rows(i).select();
            RemainingAmount = roundoff(RemainingAmount - parseFloat(data[i]));
        }
        else {
            $("#PaymentValue_" + (i + 1)).val(RemainingAmount);
            DataTables.OutStandingInvoices.rows(i).select();
            break;
        } 
        if (RemainingAmount == 0)
        {
            break;
        } 
    }
    PaymentAmountChanged();
}

function PaymentAmountChanged() {
    sum = 0;
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    var data = table.column(5).data();

    for (var i = 0; i < allData.length; i++)
    { 
        if (parseFloat(data[i]) < $("#PaymentValue_" + (i + 1)).val()) {
            $("#PaymentValue_" + (i + 1)).val(roundoff(data[i])) 
        }
    } 

    $('.paymentAmount').each(function () { 
        if ($(this).val() != "") {
            sum += parseFloat($(this).val());
            var credit = AmountReceived - sum;
            if (credit < 0) {
                $(this).val("");
            }
            else {
                $('#lblPaymentApplied').text(roundoff(sum));
                $('#lblCredit').text(roundoff(credit));
            }
        }
    });
    debugger;
        for (var i = 0; i < allData.length; i++) {
        if ($("#PaymentValue_" + (i + 1)).val() != "") {
            DataTables.OutStandingInvoices.rows(i).select();
        }
        else {
            DataTables.OutStandingInvoices.rows(i).deselect();
        }
    }

}