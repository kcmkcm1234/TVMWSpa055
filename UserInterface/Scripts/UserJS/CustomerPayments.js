var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var sum = 0;
var index = 0;
var AmountReceived=0;

$(document).ready(function () {
    try {
        $("#Customer").select2({
        });
        $('#btnUpload').click(function () {
            //Pass the controller name
            debugger;
           
            
            var FileObject = new Object;
            if ($('#hdnFileDupID').val() != emptyGUID) {
                FileObject.ParentID = (($('#ID').val()) != emptyGUID ? ($('#ID').val()) : $('#hdnFileDupID').val());
            }
            else {
                FileObject.ParentID = ($('#ID').val() == emptyGUID) ? "" : $('#ID').val();
            }

            FileObject.ParentType = "CusPayment";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });

        $("#Customerddl").select2();
    DataTables.CustomerPaymentTable = $('#CustPayTable').DataTable(
    {
        dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
        order: [],
        searching: false,    
        paging: true,
        data: GetAllCustomerPayments(0),
        columns: [
             { "data": "ID", "defaultContent": "<i>-</i>" },
             { "data": "EntryNo", "defaultContent": "<i>-</i>" },
             { "data": "PaymentDateFormatted", "defaultContent": "<i>-</i>" },
             { "data": "PaymentRef", "defaultContent": "<i>-</i>" },
             { "data": "customerObj.CompanyName", "defaultContent": "<i>-</i>" },//render customerObj.ContactPerson
             { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
             { "data": "CompanyObj.Name", "defaultContent": "<i>-</i>" },
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
             { "data": "TotalRecdAmt", "defaultContent": "<i>-</i>" },
             { "data": "AdvanceAmount", "defaultContent": "<i>-</i>" }, 
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
        ],
        columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
            { className: "text-right", "targets": [9,10] },
              { className: "text-Left", "targets": [1, 4, 3, 5, 6, 7,8] },
            { className: "text-center", "targets": [2,11] } 
        ]
    }); 
}
    catch (e) {
        notyAlert('error', e.message);
    }

$('#CustPayTable tbody').on('dblclick', 'td', function () {
    Edit(this)
});
List();

try {
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
                 "data": "Description", 'render': function (data, type, row)
                 {
                     return 'Inv# :<b>' + row.InvoiceNo + '</b>  Date :<b>' + row.InvoiceDateFormatted + '</b><br/>Company :<b>' + row.companiesObj.Name+'</b>'
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
                 "data": "CustPaymentObj.CustPaymentDetailObj.PaidAmount", 'render': function (data, type, row) {
                     index = index + 1
                     return '<input class="form-control text-right paymentAmount" name="Markup" value="' + roundoff(data) + '" onfocus="paymentAmountFocus(this);" onchange="PaymentAmountChanged(this);" id="PaymentValue_' + index + '" type="text">';
                 }, "width": "15%"
             },
             {
                 "data": "CustPaymentObj.CustPaymentDetailObj.ID", "defaultContent": "<i>-</i>", "width": "10%",
                 'render': function (data, type, row) {
                     return roundoff(row.BalanceDue)
                 }
             }
        ],
        columnDefs: [{ orderable: false, className: 'select-checkbox', targets: 1 }
            , { className: "text-right", "targets": [4, 5, 6] },
              { className: "text-left", "targets": [2] }
            , { "targets": [0,8], "visible": false, "searchable": false }
            , { "targets": [2, 3, 4, 5, 6, 7], "bSortable": false }],

        select: {style: 'multi', selector: 'td:first-child'   } 
    });
 }
    catch (e) {
        notyAlert('error', e.message);
    }

try{

    $('input[type="text"].selecttext').on('focus', function () {
        $(this).select();
    }); 
    $('#tblOutStandingDetails tbody').on('click', 'td:first-child', function (e) {
        var rt = DataTables.OutStandingInvoices.row($(this).parent()).index()       
        var table = $('#tblOutStandingDetails').DataTable();
        var allData = table.rows().data();
        if ((allData[rt].CustPaymentObj.CustPaymentDetailObj.PaidAmount) > 0)
          {
            allData[rt].CustPaymentObj.CustPaymentDetailObj.PaidAmount = roundoff(0)
            DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);
            var sum = 0;
            AmountReceived = parseFloat($('#TotalRecdAmt').val());
            for (var i = 0; i < allData.length; i++) {
                sum = sum + parseFloat(allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount);
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

debugger;
if ($('#BindValue').val() != '') {
    dashboardBind($('#BindValue').val())
}

});


function dashboardBind(ID) {
    debugger;
    openNav();
    Resetform();
    GetCustomerPaymentsByID(ID);
}

function paymentAmountFocus(event)
{
    event.select();
}
//-------------------------------------------------------------------------------------
function List() {
    var result = GetAllInvoicesAndSummary();
    if (result != null) {
        if (result.CustomerInvoiceSummary != null) {
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

//------------------------------[GetOutstandingAmountByCustomer]-------------------------
function GetOutstandingAmountByCustomer(CustomerID) {
    try {
        var data = { "CustomerID": CustomerID };
        var ds = {};
        ds = GetDataFromServer("CustomerPayments/GetOutstandingAmountByCustomer/", data);
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
//--------------------------------------------------------------------------------------


function GetAllCustomerPayments(customerPaymentsSearchObject) {
    try {
        debugger;
        if (customerPaymentsSearchObject === 0) {
            var data = {};
        }
        else {
            var data = { "customerPaymentsSearchObject": JSON.stringify(customerPaymentsSearchObject) };
        }
        
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
    Resetform();
       var rowData = DataTables.CustomerPaymentTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        GetCustomerPaymentsByID(rowData.ID)
    } 
}

function Resetform() {
    var validator = $("#CustomerPaymentForm").validate();
    $('#CustomerPaymentForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    $('#CustomerPaymentForm')[0].reset();
}

function GetCustomerPaymentsByID(PaymentID) {
    ChangeButtonPatchView('CustomerPayments', 'btnPatchAdd', 'Edit');
    var thisitem = GetCustomerPayments(PaymentID)
    $('#lblheader').text('Entry No: ' + thisitem.EntryNo);
    $('#ID').val(PaymentID);
    $('#deleteId').val(PaymentID);
    $("#Customer").select2();
    $("#Customer").val(thisitem.customerObj.ID).trigger('change');
    //$('#Customer').val(thisitem.customerObj.ID);
    $('#hdfCustomerID').val(thisitem.customerObj.ID);
    $('#Customer').prop('disabled', true);
    $('#ReferenceBank').val(thisitem.ReferenceBank);
    $('#PaymentDate').val(thisitem.PaymentDateFormatted);
    $('#ChequeDate').val(thisitem.ChequeDate);
    $('#PaymentRef').val(thisitem.PaymentRef);
    $('#RecdToComanyCode').val(thisitem.RecdToComanyCode);
    $('#PaymentMode').val(thisitem.PaymentMode);
    $('#BankCode').val(thisitem.BankCode);
    $('#DepWithdID').val(thisitem.DepWithdID);
    $('#Notes').val(thisitem.GeneralNotes);
    $('#TotalRecdAmt').val(roundoff(thisitem.TotalRecdAmt));
    $('#lblTotalRecdAmt').text(roundoff(thisitem.TotalRecdAmt));
    $('#paidAmt').text(roundoff(thisitem.TotalRecdAmt));
    $('#Type').val(thisitem.Type);
    $('#hdfType').val(thisitem.Type);
    $('#Type').prop('disabled', true);
    BindOutstandingAmount();
  
    if ( $('#Type').val() == 'C') {
        $("#CreditID").html("");
        //Get Available Credit and Add with  TotalRecdAmt
        debugger;
        var thisObj = GetCreditNoteByPaymentID(thisitem.customerObj.ID, PaymentID)
        if (thisObj.length > 0)
            var CreditAmount = parseFloat(thisitem.TotalRecdAmt) + parseFloat(thisObj[0].AvailableCredit);
        else
            var CreditAmount = parseFloat(thisitem.TotalRecdAmt);

        $('#TotalRecdAmt').val(roundoff(CreditAmount))
        $('#lblTotalRecdAmt').text(roundoff(CreditAmount))
        $('#paidAmt').text(roundoff(CreditAmount));
        $("#CreditID").append($('<option></option>').val(thisitem.CreditID).html(thisitem.CreditNo + ' ( Credit Amt: ₹' + CreditAmount + ')'));
        $('#CreditID').val(thisitem.CreditID)
        $('#CreditID').prop('disabled', true); 
        $('#TotalRecdAmt').prop('disabled', true); 
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
        CaptionChangePayment();
}

    PaymentModeChanged();
    //BIND OUTSTANDING INVOICE TABLE USING CUSTOMER ID AND PAYMENT HEADER 
    BindOutstanding();
    //edit outstanding table Payment text binding
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    var sum = 0;
    AmountReceived = roundoff( $('#TotalRecdAmt').val())
    for (var i = 0; i < allData.length; i++) {
        sum = sum + parseFloat(allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount);
    }
    $('#lblPaymentApplied').text(roundoff(sum));
    $('#lblCredit').text(roundoff(AmountReceived - sum));
        // $('#lblPaymentApplied').text(roundoff(thisitem.TotalRecdAmt));
    Selectcheckbox();
    clearUploadControl();
    PaintImages(PaymentID);
}

function openNavClick() {
    fieldsclear();
    $('#lblOutstandingdetails').text('');
    BindOutstanding();
    $('#lblheader').text('New Payment');
    ChangeButtonPatchView('CustomerPayments', 'btnPatchAdd', 'Add');
    $('#Customer').prop('disabled', false);
    $('#BankCode').prop('disabled', true);
    $('#ChequeDate').prop('disabled', true);
    $('#ReferenceBank').prop('disabled', true);
    $('#Type').prop('disabled', false);
    $('#PaymentMode').prop('disabled', false);
    $('#TotalRecdAmt').prop('disabled', false);
    openNav();
    clearUploadControl();
}
 
function TypeOnChange() {
    if ($('#Type').val() == "C"){
        $("#ddlCreditDiv").css("visibility", "visible"); 
        $('#PaymentMode').val('');
        $('#BankCode').val('');
        $('#PaymentMode').prop('disabled', true);
        $('#TotalRecdAmt').prop('disabled', true);
        $('#BankCode').prop('disabled', true);
        $('#ChequeDate').prop('disabled', true);
        $('#CreditID').prop('disabled', false);
        CaptionChangeCredit()
    }
    else {
        $("#ddlCreditDiv").css("visibility", "hidden");
        $('#PaymentMode').prop('disabled', false);
        $('#BankCode').prop('disabled', true); 
        $('#TotalRecdAmt').val(0);
        $('#TotalRecdAmt').prop('disabled', false);
        $('#CreditID').val(emptyGUID);
        CaptionChangePayment()
        AmountChanged();
    }
}
function CaptionChangeCredit() {
    $("#lblTotalRecdAmtCptn").text('Credit Amount');
    $("#lblPaymentAppliedCptn").text('Total Credit Used');
    $("#lblCreditCptn").text('Credit Remaining');
    $("#lblTotalAmtRecdCptn").text('Credit Amount');
    $("#lblpaidAmt").text('Credit Amount');
}
function CaptionChangePayment() {
    $("#lblTotalRecdAmtCptn").text('Total Amount Recevied');
    $("#lblPaymentAppliedCptn").text('Payment Applied');
    $("#lblCreditCptn").text('Credit Received');
    $("#lblTotalAmtRecdCptn").text('Amount Received');
    $("#lblpaidAmt").text('Amount Received');
}
function ddlCreditOnChange(event) {
    debugger;
    var creditID = $("#CreditID").val();
    var CustomerID = $("#Customer").val();
    if (creditID != emptyGUID) {
        var ds = GetCreditNoteAmount(creditID, CustomerID);
        $('#TotalRecdAmt').val(ds.AvailableCredit);
        $('#TotalRecdAmt').prop('disabled', true);
        AmountChanged();
    }

}

function GetCreditNoteAmount(ID,CustomerID) {
    try {
        var data = { "CreditID": ID, "CustomerID": CustomerID };
        var ds = {};
        ds = GetDataFromServer("CustomerPayments/GetCreditNoteAmount/", data);
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
    var ID = $("#Customer").val() == "" ? null : $("#Customer").val();
    if (ID != null) {
        var ds = GetCreditNoteByCustomer(ID);
        if (ds.length > 0) {
            $("#CreditID").html(""); // clear before appending new list 
            $("#CreditID").append($('<option></option>').val(emptyGUID).html('--Select Credit Note--'));
            $.each(ds, function (i, credit) {
                $("#CreditID").append(
                    $('<option></option>').val(credit.ID).html(credit.CreditNoteNo + ' ( Credit Amt: ₹' + credit.AvailableCredit + ')'));
            });
        }
        else {
            $("#CreditID").html("");
            $("#CreditID").append($('<option></option>').val(emptyGUID).html('No Credit Notes Available'));
        //    $("#Type").val('P');
        }
    }
}

function GetCreditNoteByPaymentID (ID, PaymentID) {
    try {
        var data = { "ID": ID, "PaymentID": PaymentID };
        var ds = {};
        ds = GetDataFromServer("CustomerPayments/GetCreditNoteByPaymentID/", data);
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

function GetCreditNoteByCustomer(ID) {
    try {
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("CustomerPayments/GetCreditNoteByCustomer/", data);
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


function validate()
{
    debugger;
    var CustomerPaymentsViewModel = new Object();
    CustomerPaymentsViewModel.PaymentRef = $("#PaymentRef").val();
    CustomerPaymentsViewModel.ID = $("#ID").val();
    var data = "{'_customerpayObj': "+JSON.stringify(CustomerPaymentsViewModel)+"}";
    PostDataToServer("CustomerPayments/Validate/", data, function (JsonResult) {
        debugger;
        if (JsonResult != '') {
            switch (JsonResult.Result) {
                case "OK":
                    if (JsonResult.Records.Status==1)
                        notyConfirm(JsonResult.Records.Message, 'SaveValidatedData();', '', "Yes,Proceed!", 1);
                    else
                    {
                        SaveValidatedData();
                    }
                    break;
                case "ERROR":
                    notyAlert('error', JsonResult.Message);
                    break;
                default:
                    break;
            }
        }
    });
    //debugger;
    //var CustomerPaymentsViewModel = new Object();
    //CustomerPaymentsViewModel.PaymentRef = $("#PaymentRef").val();
    //var data = { "_customerpayObj": JSON.stringify(CustomerPaymentsViewModel) };
    //var ds = {};
    //ds = PostDataToServer("CustomerPayments/Validate/", data);
    //if (ds != '') {
    //    ds = JSON.parse(ds);
    //}
    //if (ds.Result == "OK") {
    //    return ds.Records;
    //}
    //if (ds.Result == "ERROR") {
    //    notyAlert('error', ds.Message);
    //}
}

function savePayments() {
    debugger; 
    if ($('#PaymentMode').val() == "ONLINE" &&  $("#BankCode").val()=="" ) {

        notyAlert('error', 'Please Select Bank');
       
    }
    
    //else if ($('#TotalRecdAmt').val()==0) {
    //    notyAlert('error', 'Please Enter Amount');
        //}
    else
    {
        validate();
        //SaveValidatedData();
    }
}

function SaveValidatedData()
{
    debugger;
    $(".cancel").click();
    var SelectedRows = DataTables.OutStandingInvoices.rows(".selected").data();
    if ((SelectedRows) && (SelectedRows.length > 0)) {
        var ar = [];
        for (var r = 0; r < SelectedRows.length; r++) {
            var PaymentDetailViewModel = new Object();
            PaymentDetailViewModel.InvoiceID = SelectedRows[r].ID;//Invoice ID
            PaymentDetailViewModel.ID = SelectedRows[r].CustPaymentObj.CustPaymentDetailObj.ID//Detail ID
            PaymentDetailViewModel.PaidAmount = SelectedRows[r].CustPaymentObj.CustPaymentDetailObj.PaidAmount;
            //
            ar.push(PaymentDetailViewModel);
        }
        $('#paymentDetailhdf').val(JSON.stringify(ar));
    }
    $('#hdfCreditAmount').val($('#lblPaymentApplied').text());
    $('#AdvanceAmount').val($('#lblCredit').text());
    setTimeout(function () {
        $('#btnSave').trigger('click');
    }, 1000);
    
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
            closeNav();
            BindCustomerPaymentsHeader()
            notyAlert('success', JsonResult.Message);
            $('#lblOutstandingdetails').text('');
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
            GetCustomerPaymentsByID(JsonResult.Records.ID)
            BindCustomerPaymentsHeader()
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
    Resetform();
    $('#lblTotalRecdAmt').text('0');
    $('#lblPaymentApplied').text('0');
    $('#lblCredit').text('0');
    $('#paidAmt').text('₹ 0.00');
    $('#invoicedAmt').text('₹ 0.00');
    $("#Customer").select2();
    $("#Customer").val('').trigger('change');
    $('#ID').val(emptyGUID);
    $("#CreditID").html("");
    $('#Type').val('P');
    $('#hdfType').val('');
    $('#hdfCustomerID').val('');
    $('#ReferenceBank').val('');
    $("#ddlCreditDiv").css("visibility", "hidden");
    $('#paymentDetailhdf').val('');
    CaptionChangePayment();
}
function CustomerChange() {
    debugger;
    if ($('#Customer').val() != "") {
        BindCreditDropDown(); 
        BindOutstandingAmount();
        $('#TotalRecdAmt').val('');
        AmountChanged();
    }
    BindOutstanding();
}

function BindOutstandingAmount() {
    var thisitem = GetOutstandingAmountByCustomer($('#Customer').val())
    if (thisitem != null) {
        $('#invoicedAmt').text(thisitem.OutstandingAmount == null ? "₹ 0.00" : thisitem.OutstandingAmount);
        $('#lblOutstandingdetails').text("(Inv: " + thisitem.InvoiceOutstanding + ", Pay: " + thisitem.PaymentOutstanding +
                                         ", Cr: " + thisitem.CreditOutstanding + ", Adv: " + thisitem.AdvOutstanding + ")");
    }    
}
function BindOutstanding() {
    index = 0; 
    DataTables.OutStandingInvoices.clear().rows.add(GetOutStandingInvoices()).draw(false); 
}

function BindCustomerPaymentsHeader()
{
    DataTables.CustomerPaymentTable.clear().rows.add(GetAllCustomerPayments()).draw(false);
}

function PaymentModeChanged() {
    if ($('#PaymentMode').val()=="ONLINE"){
        $('#BankCode').prop('disabled', false);
    }
    else {
        $("#BankCode").val('');
        $('#BankCode').prop('disabled', true);
    }
    if ($('#PaymentMode').val() == "CHEQUE") {
        $('#ChequeDate').prop('disabled', false);
        $('#ReferenceBank').prop('disabled', false);
    }
    else {
        $("#ChequeDate").val('');
        $('#ChequeDate').prop('disabled', true);
        $('#ReferenceBank').prop('disabled', true);
    }


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
        var CustID = $('#Customer').val() == "" ? emptyGUID : $('#Customer').val();
        var PaymentID = $('#ID').val() == "" ? emptyGUID : $('#ID').val();
       
        var data = { "CustID": CustID, "PaymentID":PaymentID };
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
    debugger;
    var sum = 0;
    DataTables.OutStandingInvoices.rows().deselect();
    if ($('#TotalRecdAmt').val() < 0 || $('#TotalRecdAmt').val()=="") {
        $('#TotalRecdAmt').val(0);
    }

    AmountReceived = parseFloat($('#TotalRecdAmt').val())
    if (!isNaN(AmountReceived)) {
        $('#TotalRecdAmt').val(roundoff(AmountReceived));
        $('#lblTotalRecdAmt').text(roundoff(AmountReceived));
        $('#paidAmt').text(roundoff(AmountReceived));

        var table = $('#tblOutStandingDetails').DataTable();
        var allData = table.rows().data();
        var RemainingAmount = AmountReceived;

        for (var i = 0; i < allData.length; i++) {
            var CustPaymentObj = new Object;
            var CustPaymentDetailObj = new Object;
            CustPaymentObj.CustPaymentDetailObj = CustPaymentDetailObj;

            if (RemainingAmount != 0) {
                if (parseFloat(allData[i].BalanceDue) < RemainingAmount) {
                    allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount = parseFloat(allData[i].BalanceDue)
                    RemainingAmount = RemainingAmount - parseFloat(allData[i].BalanceDue);
                    sum = sum + parseFloat(allData[i].BalanceDue);
                }
                else {
                    allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount = RemainingAmount
                    sum = sum + RemainingAmount;
                    RemainingAmount = 0
                }
            }
            else {
                allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount = 0;
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
    AmountReceived = parseFloat($('#TotalRecdAmt').val())
    sum = 0;
    var allData  = DataTables.OutStandingInvoices.rows().data();
    var table = DataTables.OutStandingInvoices;
    var rowtable = table.row($(this_Obj).parents('tr')).data();

    for (var i = 0; i < allData.length; i++)
    {
        if (allData[i].ID == rowtable.ID) {

            var oldamount = parseFloat(allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount)
            var credit = parseFloat($("#lblCredit").text())
            if (credit > 0) {
                var currenttotal = AmountReceived + parseFloat(this_Obj.value) - (credit + oldamount)
            }
            else {
                var currenttotal = AmountReceived + parseFloat(this_Obj.value) - oldamount
            }

            if (parseFloat(allData[i].BalanceDue) < parseFloat(this_Obj.value)) { 
                if (currenttotal<AmountReceived) {
                    allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount = parseFloat(allData[i].BalanceDue)
                    sum = sum + parseFloat(allData[i].BalanceDue);
                }
                else {
                    allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount = oldamount
                    sum = sum + oldamount
                }
            }
            else {
                if (currenttotal > AmountReceived) {
                    allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount = oldamount
                    sum = sum + oldamount;
                }
                else {
                    allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount = this_Obj.value;
                    sum = sum + parseFloat(this_Obj.value);
                }   
            }
        }
        else { 
            sum = sum + parseFloat(allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount);
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
        if (allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount == "" || allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount == roundoff(0))
        {
            DataTables.OutStandingInvoices.rows(i).deselect();
        }
        else {
            DataTables.OutStandingInvoices.rows(i).select();
        }
    }
}

function RefreshCustomerPayments() {
    try {
        debugger
        var fromdate = $("#fromdate");
        var todate = $("#todate");
        var customercode = $("#Customerddl");
        var paymentMode = $("#paymode");
        var companycode = $("#Companyddl");
        var search = $("#search");

        var CustomerPaymentsSearch = new Object();
        CustomerPaymentsSearch.FromDate = fromdate[0].value !== "" ? fromdate[0].value : null;
        CustomerPaymentsSearch.ToDate = todate[0].value !== "" ? todate[0].value : null;
        CustomerPaymentsSearch.Customer = customercode[0].value !== "" ? customercode[0].value : null;
        CustomerPaymentsSearch.PaymentMode = paymentMode[0].value !== "" ? paymentMode[0].value : null;
        CustomerPaymentsSearch.Company = companycode[0].value !== "" ? companycode[0].value : null;
        CustomerPaymentsSearch.Search = search[0].value !== "" ? search[0].value : null;

        DataTables.CustomerPaymentTable.clear().rows.add(GetAllCustomerPayments(CustomerPaymentsSearch)).draw(false);
        
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Reset() {
    $("#fromdate").val('');
    $("#todate").val('');
    $("#Customerddl").select2();
    $("#Customerddl").val('').trigger('change');
    $("#paymode").val('');
    $("#Companyddl").val('');
    $("#search").val('');
    RefreshCustomerPayments();
}