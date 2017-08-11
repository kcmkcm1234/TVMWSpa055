var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var sum = 0;
var AmountReceived = 0;

$(document).ready(function () {
    try { 
        DataTables.SupplierPaymentTable = $('#SupPayTable').DataTable(
        {
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
            paging: true,
            data:GetAllSupplierPayments(),
            columns: [
                 { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": "EntryNo", "defaultContent": "<i>-</i>" },
                 { "data": "PaymentDateFormatted", "defaultContent": "<i>-</i>" },
                 { "data": "PaymentRef", "defaultContent": "<i>-</i>" },
                 { "data": "supplierObj.CompanyName", "defaultContent": "<i>-</i>" },//render supplierObj.ContactPerson
                 { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
                 {
                     "data": "Type", "defaultContent": "<i>-</i>", 'render': function (data, type, row) {
                         if (data == 'C') {
                             return 'Credit'
                         }
                         else {
                             return 'Normal'
                         }
                     }
                 },
                 { "data": "CreditNo", "defaultContent": "<i>-</i>" },
                 { "data": "TotalPaidAmt", "defaultContent": "<i>-</i>" },
                 { "data": "AdvanceAmount", "defaultContent": "<i>-</i>" },
                 { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-right", "targets": [8, 9] },
                 { className: "text-Left", "targets": [4, 5, 6, 7] },
                { className: "text-center", "targets": [1, 2, 3,10] }
            ]
        });
    }
    catch (e) {
        notyAlert('error', e.message);
    }

    $('#SupPayTable tbody').on('dblclick', 'td', function () {
    Edit(this)
    });
    List();

    try {
        DataTables.OutStandingInvoices = $('#tblOutStandingDetails').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
            paging: false,
            data: null,
            columns: [
                 { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": "Checkbox", "defaultContent": "" },
                 {
                     "data": "Description", 'render': function (data, type, row) {
                         return ' Invoice # ' + row.InvoiceNo + '(Date:' + row.InvoiceDateFormatted + ')'
                     }, "width": "30%"
                 },
                 { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>", "width": "10%" },
                 {
                     "data": "TotalInvoiceAmount", "defaultContent": "<i>-</i>", "width": "15%",
                     'render': function (data, type, row) {
                         return roundoff(row.TotalInvoiceAmount)
                     }
                 },
                 {
                     "data": "OtherPayments", "defaultContent": "<i>-</i>", "width": "15%",
                     'render': function (data, type, row) {
                         return roundoff(row.OtherPayments)
                     }
                 },
                 {
                     "data": "BalanceDue", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         return roundoff(row.BalanceDue)
                     }
                 },
                 {
                     "data": "SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount", 'render': function (data, type, row) {
                         return '<input class="form-control text-right paymentAmount" name="Markup" value="' + roundoff(data) + '" onfocus="paymentAmountFocus(this);" onchange="PaymentAmountChanged(this);"  type="text">';
                     }, "width": "15%"
                 },
                 {
                     "data": "SuppPaymentObj.supplierPaymentsDetailObj.ID", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         return roundoff(row.BalanceDue)
                     }
                 }
            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', targets: 1 }
                , { className: "text-right", "targets": [4, 5, 6] }
                , { "targets": [0, 8], "visible": false, "searchable": false }
                , { "targets": [2, 3, 4, 5, 6, 7], "bSortable": false }],

            select: { style: 'multi', selector: 'td:first-child' }
        });
    }
    catch (e) {
        notyAlert('error', e.message);
    }

    try {

        $('input[type="text"].selecttext').on('focus', function () {
            $(this).select();
        });
        $('#tblOutStandingDetails tbody').on('click', 'td:first-child', function (e) {
            var rt = DataTables.OutStandingInvoices.row($(this).parent()).index()
            var table = $('#tblOutStandingDetails').DataTable();
            var allData = table.rows().data();
            if ((allData[rt].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount) > 0) {
                allData[rt].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = roundoff(0)
                DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);
                var sum = 0;
                AmountReceived = parseFloat($('#TotalPaidAmt').val());
                for (var i = 0; i < allData.length; i++) {
                    sum = sum + parseFloat(allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount);
                }
                $('#lblPaymentApplied').text(roundoff(sum));
                $('#lblCredit').text(roundoff(AmountReceived - sum));
                Selectcheckbox();
            }
        });
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});

function paymentAmountFocus(event) {
    event.select();
}
//-------------------------------------------------------------------------------------
function List() {
    var result = GetAllInvoicesAndSummary();
    if (result != null) {
        if (result.SupplierInvoiceSummary != null) {
            Summary(result.SupplierInvoiceSummary);
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

//---------------Bind logics-------------------
function GetAllInvoicesAndSummary() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("SupplierInvoices/GetInvoicesAndSummary/", data);
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

////------------------------------[GetOutstandingAmountBySupplier]-------------------------
function GetOutstandingAmountBySupplier(SupplierID) {
    try {
        var data = { "SupplierID": SupplierID };
        var ds = {};
        ds = GetDataFromServer("SupplierPayments/GetOutstandingAmountBySupplier/", data);
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
////--------------------------------------------------------------------------------------


function GetAllSupplierPayments() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("SupplierPayments/GetAllSupplierPayments/", data);
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

function Edit(currentObj) {
    openNav();
        var rowData = DataTables.SupplierPaymentTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        GetSupplierPaymentsByID(rowData.ID)
    }
}

function GetSupplierPaymentsByID(PaymentID) {
    ChangeButtonPatchView('SupplierPayments', 'btnPatchAdd', 'Edit');
    var thisitem = GetSupplierPayments(PaymentID)
    $('#lblheader').text('Entry No: ' + thisitem.EntryNo);
    $('#ID').val(PaymentID);
    $('#deleteId').val(PaymentID);
    $('#Supplier').val(thisitem.supplierObj.ID);
    $('#hdfSupplierID').val(thisitem.supplierObj.ID);
    $('#Supplier').prop('disabled', true);
    $('#PaymentDate').val(thisitem.PaymentDateFormatted);
    $('#PaymentRef').val(thisitem.PaymentRef);
    $('#PaidFromComanyCode').val(thisitem.PaidFromComanyCode);
    $('#PaymentMode').val(thisitem.PaymentMode);
    $('#BankCode').val(thisitem.BankCode);
    $('#DepWithdID').val(thisitem.DepWithdID);
    $('#Notes').val(thisitem.GeneralNotes);
    $('#TotalPaidAmt').val(roundoff(thisitem.TotalPaidAmt));
    $('#lblTotalPaidAmt').text(roundoff(thisitem.TotalPaidAmt));
    $('#paidAmt').text('₹ ' + roundoff(thisitem.TotalPaidAmt));
    $('#Type').val(thisitem.Type);
    $('#hdfType').val(thisitem.Type);
    $('#Type').prop('disabled', true);
    BindOutstandingAmount();

    if ($('#Type').val() == 'C') {
        $("#CreditID").html("");
        //Get Available Credit and Add with  TotalPaidAmt
        debugger;
        var thisObj = GetCreditNoteByPaymentID(thisitem.supplierObj.ID, PaymentID)
        if (thisObj.length>0)
            var CreditAmount = parseFloat(thisitem.TotalPaidAmt) + parseFloat(thisObj[0].AvailableCredit);
        else
            var CreditAmount = parseFloat(thisitem.TotalPaidAmt);

        $('#TotalPaidAmt').val(roundoff(CreditAmount))
        $('#lblTotalPaidAmt').text(roundoff(CreditAmount))
        $('#paidAmt').text(roundoff(CreditAmount));
        $("#CreditID").append($('<option></option>').val(thisitem.CreditID).html(thisitem.CreditNo + ' ( Credit Amt: ₹' + CreditAmount + ')'));
        $('#CreditID').val(thisitem.CreditID)
        $('#CreditID').prop('disabled', true);
        $('#TotalPaidAmt').prop('disabled', true);
        $('#hdfCreditID').val(thisitem.CreditID);
        $('#PaymentMode').prop('disabled', true);
        $("#ddlCreditDiv").css("visibility", "visible");
        CaptionChangeCredit();
    }
    else {
        $("#CreditID").html(""); // clear before appending new list 
        $("#CreditID").append($('<option></option>').val(emptyGUID).html('--Select Credit Note--'));
        $('#hdfCreditID').val(emptyGUID);
        $('#PaymentMode').prop('disabled', false);
        $('#TotalPaidAmt').prop('disabled', false);
        CaptionChangePayment();
    }

    PaymentModeChanged();
    //BIND OUTSTANDING INVOICE TABLE USING supplier ID AND PAYMENT HEADER 
    BindOutstanding();
    //edit outstanding table Payment text binding
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    var sum = 0;
    AmountReceived = roundoff($('#TotalPaidAmt').val())
    for (var i = 0; i < allData.length; i++) {
        sum = sum + parseFloat(allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount);
    }
    $('#lblPaymentApplied').text(roundoff(sum));
    $('#lblCredit').text(roundoff(AmountReceived - sum));
    // $('#lblPaymentApplied').text(roundoff(thisitem.TotalPaidAmt));
    Selectcheckbox();
}

function openNavClick() {
    debugger;
    fieldsclear();
    BindOutstanding();
    $('#lblheader').text('New Payment');
    ChangeButtonPatchView('SupplierPayments', 'btnPatchAdd', 'Add');
    $('#Supplier').prop('disabled', false);
    $('#BankCode').prop('disabled', true);
    $('#Type').prop('disabled', false);
    $('#PaymentMode').prop('disabled', false);
    openNav();
}

function TypeOnChange() {
    if ($('#Type').val() == "C") {
        $("#ddlCreditDiv").css("visibility", "visible");
        $('#PaymentMode').val('');
        $('#BankCode').val('');
        $('#PaymentMode').prop('disabled', true);
        $('#BankCode').prop('disabled', true);
        $('#CreditID').prop('disabled', false);
        CaptionChangeCredit()
    }
    else {
        $("#ddlCreditDiv").css("visibility", "hidden");
        $('#PaymentMode').prop('disabled', false);
        $('#BankCode').prop('disabled', true); 
        $('#TotalPaidAmt').val(0);
        $('#TotalPaidAmt').prop('disabled', false);
        $('#CreditID').val(emptyGUID);
        CaptionChangePayment()
        AmountChanged();
    }
}
function CaptionChangeCredit() {
    $("#lblTotalPaidAmtCptn").text('Credit Amount');
    $("#lblPaymentAppliedCptn").text('Total Credit Used');
    $("#lblCreditCptn").text('Credit Remaining');
    $("#lblTotalAmtPaidCptn").text('Credit Amount');
    $("#lblpaidAmt").text('Credit Amount');
}
function CaptionChangePayment() {
    $("#lblTotalPaidAmtCptn").text('Total Amount Paid');
    $("#lblPaymentAppliedCptn").text('Payment Applied');
    $("#lblCreditCptn").text('Credit Paid');
    $("#lblTotalAmtPaidCptn").text('Amount Paid');
    $("#lblpaidAmt").text('Amount Paid');
}
function ddlCreditOnChange(event) {
    debugger;
    var creditID = $("#CreditID").val();
    var SupplierID = $("#Supplier").val();
    if (creditID != emptyGUID) {
        var ds = GetCreditNoteAmount(creditID, SupplierID);
        $('#TotalPaidAmt').val(ds.AvailableCredit);
        $('#TotalPaidAmt').prop('disabled', true);
        AmountChanged();
    } 
}

function GetCreditNoteAmount(ID, SupplierID) {
    try {
        var data = { "CreditID": ID, "SupplierID": SupplierID };
        var ds = {};
        ds = GetDataFromServer("SupplierPayments/GetCreditNoteAmount/", data);
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

function GetCreditNoteByPaymentID(ID, PaymentID) {
    try {
        var data = { "ID": ID, "PaymentID": PaymentID };
        var ds = {};
        ds = GetDataFromServer("SupplierPayments/GetCreditNoteByPaymentID/", data);
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

function BindCreditDropDown() {
    debugger;
    var ID = $("#Supplier").val() == "" ? null : $("#Supplier").val();
    if (ID != null) {
        var ds = GetCreditNoteBySupplier(ID);
        if (ds.length > 0) {
            $("#CreditID").html(""); // clear before appending new list 
            $("#CreditID").append($('<option></option>').val(emptyGUID).html('--Select Credit Note--'));
            $.each(ds, function (i, credit) {
                $("#CreditID").append(
                    $('<option></option>').val(credit.ID).html(credit.CRNRefNo + ' ( Credit Amt: ₹' + credit.AvailableCredit + ')'));
            });
        }
        else {
            $("#CreditID").html("");
            $("#CreditID").append($('<option></option>').val(emptyGUID).html('No Credit Notes Available'));
            $("#Type").val('P');
        }
    }
}

function GetCreditNoteBySupplier(ID) {
    try {
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("SupplierPayments/GetCreditNoteBySupplier/", data);
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

function savePayments() {
    debugger;
    if ($('#PaymentMode').val() == "ONLINE" && $("#BankCode").val() == "") {
        notyAlert('error', 'Please Select Bank');
    }
    else if ($('#TotalPaidAmt').val() == 0) {
        notyAlert('error', 'Please Enter Amount');
    }
    else {
        var SelectedRows = DataTables.OutStandingInvoices.rows(".selected").data();
        if ((SelectedRows) && (SelectedRows.length > 0)) {
            var ar = [];
            for (var r = 0; r < SelectedRows.length; r++) {
                var PaymentDetailViewModel = new Object();
                PaymentDetailViewModel.InvoiceID = SelectedRows[r].ID;//Invoice ID
                PaymentDetailViewModel.ID = SelectedRows[r].SuppPaymentObj.supplierPaymentsDetailObj.ID//Detail ID
                PaymentDetailViewModel.PaidAmount = SelectedRows[r].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount;
                ar.push(PaymentDetailViewModel);
            }
            $('#paymentDetailhdf').val(JSON.stringify(ar));
        }
        if ($("#hdfCreditID").val() == undefined)
            $("#hdfCreditID").val(emptyGUID);
        $('#hdfCreditAmount').val($('#lblPaymentApplied').text());
        $('#AdvanceAmount').val($('#lblCredit').text());
        $('#btnSave').trigger('click');
    }
}


function DeletePayments() {
    notyConfirm('Are you sure to delete?', 'Delete()', '', "Yes, delete it!");
}
function Delete() {
    $('#btnFormDelete').trigger('click');
}

function DeleteSuccess(data, status) {
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            openNavClick()
            BindSupplierPaymentsHeader()
            notyAlert('success', JsonResult.Message);
            List();
            break;
        case "Error":
            notyAlert('error', JsonResult.Message);
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            break;
    }
}

function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            debugger;
            fieldsclear();
            GetSupplierPaymentsByID(JsonResult.Records.ID)
            BindSupplierPaymentsHeader()
            notyAlert('success', JsonResult.Message);
            List();
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}

function fieldsclear() {
    $('#SupplierPaymentForm')[0].reset();
    $('#lblTotalPaidAmt').text('0.00');
    $('#lblPaymentApplied').text('0.00');
    $('#lblCredit').text('0.00');
    $('#paidAmt').text('₹ 0.00');
    $('#invoicedAmt').text('₹ 0.00');
    $('#ID').val(emptyGUID);
    $("#CreditID").html("");
    $('#Type').val('P');
    $('#paymentDetailhdf').val('');
    $('#TotalPaidAmt').prop('disabled', false);
    $("#ddlCreditDiv").css("visibility", "hidden");
    CaptionChangePayment();
}
function SupplierChange() {
    debugger;
    if ($('#Supplier').val() != "") {
        BindCreditDropDown();
        BindOutstandingAmount();
    }
    BindOutstanding();
}

function BindOutstandingAmount() {
    debugger;
    var thisitem = GetOutstandingAmountBySupplier($('#Supplier').val())
    if (thisitem != null) {
        $('#invoicedAmt').text(thisitem.OutstandingAmount == null ? "₹ 0.00" : thisitem.OutstandingAmount);
        $('#lblOutstandingdetails').text("(Inv: " + thisitem.InvoiceOutstanding + ", Pay: " + thisitem.PaymentOutstanding +
                                         ", Cr: " + thisitem.CreditOutstanding + ", Adv: " + thisitem.AdvOutstanding + ")");
    }
}
function BindOutstanding() {
    debugger;
    DataTables.OutStandingInvoices.clear().rows.add(GetOutStandingInvoices()).draw(false);
}

function BindSupplierPaymentsHeader() {
    DataTables.SupplierPaymentTable.clear().rows.add(GetAllSupplierPayments()).draw(false);
}

function PaymentModeChanged() {
    if ($('#PaymentMode').val() == "ONLINE") {
        $('#BankCode').prop('disabled', false);
    }
    else {
        $("#BankCode").val('');
        $('#BankCode').prop('disabled', true);
    }
}

function GetSupplierPayments(ID) {
    try {
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("SupplierPayments/GetSupplierPaymentsByID/", data);
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
        var supplierID = $('#Supplier').val() == "" ? emptyGUID : $('#Supplier').val();
        var PaymentID = $('#ID').val() == "" ? emptyGUID : $('#ID').val();

        var data = { "supplierID": supplierID, "PaymentID": PaymentID };
        var ds = {};
        ds = GetDataFromServer("SupplierPayments/GetOutStandingInvoices/", data);
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
//-----------------------------------------------CALCULATION------------------------------------------------------------//
function AmountChanged() {
    debugger;
    var sum = 0;
    DataTables.OutStandingInvoices.rows().deselect();
    if ($('#TotalPaidAmt').val() < 0 || $('#TotalPaidAmt').val() == "") {
        $('#TotalPaidAmt').val(0);
    }
    AmountReceived = parseFloat($('#TotalPaidAmt').val())
    if (!isNaN(AmountReceived)) {
        $('#TotalPaidAmt').val(roundoff(AmountReceived));
        $('#lblTotalPaidAmt').text(roundoff(AmountReceived));
        $('#paidAmt').text('₹ '+roundoff(AmountReceived));
        var table = $('#tblOutStandingDetails').DataTable();
        var allData = table.rows().data();
        var RemainingAmount = AmountReceived;
        for (var i = 0; i < allData.length; i++) {
            var SuppPaymentObj = new Object;
            var supplierPaymentsDetailObj = new Object;
            SuppPaymentObj.supplierPaymentsDetailObj = supplierPaymentsDetailObj;
            if (RemainingAmount != 0) {
                if (parseFloat(allData[i].BalanceDue) < RemainingAmount) {
                    allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = parseFloat(allData[i].BalanceDue)
                    RemainingAmount = RemainingAmount - parseFloat(allData[i].BalanceDue);
                    sum = sum + parseFloat(allData[i].BalanceDue);
                }
                else {
                    allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = RemainingAmount
                    sum = sum + RemainingAmount;
                    RemainingAmount = 0
                }
            }
            else {
                allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = 0;
            }
        }
        DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);
        Selectcheckbox();
        $('#lblPaymentApplied').text(roundoff(sum));
        $('#lblCredit').text(roundoff(AmountReceived - sum));
    }
}

function PaymentAmountChanged(this_Obj) {
    debugger;
    AmountReceived = parseFloat($('#TotalPaidAmt').val())
    sum = 0;
    var allData = DataTables.OutStandingInvoices.rows().data();
    var table = DataTables.OutStandingInvoices;
    var rowtable = table.row($(this_Obj).parents('tr')).data();
    for (var i = 0; i < allData.length; i++) {
        if (allData[i].ID == rowtable.ID) {
            var oldamount = parseFloat(allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount)
            var credit = parseFloat($("#lblCredit").text())
            if (credit > 0) {
                var currenttotal = AmountReceived + parseFloat(this_Obj.value) - (credit + oldamount)
            }
            else {
                var currenttotal = AmountReceived + parseFloat(this_Obj.value) - oldamount
            }
            if (parseFloat(allData[i].BalanceDue) < parseFloat(this_Obj.value)) {
                if (currenttotal < AmountReceived) {
                    allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = parseFloat(allData[i].BalanceDue)
                    sum = sum + parseFloat(allData[i].BalanceDue);
                }
                else {
                    allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = oldamount
                    sum = sum + oldamount
                }
            }
            else {
                if (currenttotal > AmountReceived) {
                    allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = oldamount
                    sum = sum + oldamount;
                }
                else {
                    allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount = this_Obj.value;
                    sum = sum + parseFloat(this_Obj.value);
                }
            }
        }
        else {
            sum = sum + parseFloat(allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount);
        }
    }
    DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);
    $('#lblPaymentApplied').text(roundoff(sum));
    $('#lblCredit').text(roundoff(AmountReceived - sum));
    Selectcheckbox();
}

function Selectcheckbox() {
    debugger;
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    for (var i = 0; i < allData.length; i++) {
        if (allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount == "" || allData[i].SuppPaymentObj.supplierPaymentsDetailObj.PaidAmount == roundoff(0)) {
            DataTables.OutStandingInvoices.rows(i).deselect();
        }
        else {
            DataTables.OutStandingInvoices.rows(i).select();
        }
    }
}