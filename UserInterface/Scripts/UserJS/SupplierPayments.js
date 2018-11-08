var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
var sum = 0;
var AmountReceived = 0;

$(document).ready(function () {
    try {
        debugger;
        $("#Supplier,#Supplierddl").select2();
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

            FileObject.ParentType = "SuppPayment";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });
        DataTables.SupplierPaymentTable = $('#SupPayTable').DataTable(
        {
            
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [ 1, 2, 3, 4, 5, 6,7,8,9,10,11]
                             }
            }],
            order: [],
            searching: false,
            paging: true,
            data: GetAllSupplierPayments(),
            pageLength: 15,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
                 { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": "EntryNo", "defaultContent": "<i>-</i>" },
                 { "data": "ApprovalStatusObj.Description", "defaultContent": "<i>-</i>" },
                 { "data": "PaymentDateFormatted", "defaultContent": "<i>-</i>" },
                 { "data": "PaymentRef", "defaultContent": "<i>-</i>" },
                 {
                     "data": "supplierObj.CompanyName", "defaultContent": "<i>-</i>", 'render': function (data, type, row) {                       
                         if (row.GeneralNotes!=null)
                         if (row.GeneralNotes.length>50)
                             return  data + '<br/>[ ' + row.GeneralNotes.substring(0, 50) + '.... ]'
                         else
                             return  data + '<br/>[ ' + row.GeneralNotes+' ]'
                         return  data 
                     }
                 },//render supplierObj.ContactPerson
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
                 { "data": "TotalPaidAmt", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                 { "data": "AdvanceAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                 { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-right", "targets": [10,11] },
                 { className: "text-Left", "targets": [2,4, 5, 6, 7,8,9] },
                 { className: "text-center", "targets": [1, 3,12] }
            ]
        });
        $(".buttons-excel").hide();
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
                     //    return ' Invoice # ' + row.InvoiceNo + '(Date:' + row.InvoiceDateFormatted + ')'
                         if (row.AccountCode != null && row.EmpName != null)
                             return ' Invoice # ' + row.InvoiceNo + '(Date:' + row.InvoiceDateFormatted + ')<br/><b>Company:</b>' + row.companiesObj.Name + '<br/><b>Acc.Head:</b>' + row.AccountCode + '<br/><b>Sub Type:</b>' + row.EmpName;
                         else if (row.AccountCode != null && row.EmpName == null)
                             return ' Invoice # ' + row.InvoiceNo + '(Date:' + row.InvoiceDateFormatted + ')<br/><b>Company:</b>' +row.companiesObj.Name + '<br/><b>Acc.Head:</b>' +row.AccountCode;
                         else
                             return ' Invoice # ' + row.InvoiceNo + '(Date:' + row.InvoiceDateFormatted + ')<br/><b>Company:</b>' + row.companiesObj.Name;

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
                , { className: "text-left", "targets": [2] }
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

    if ($('#BindValue').val() != '') {
        dashboardBind($('#BindValue').val())
    }
   
});

function dashboardBind(ID) {
    openNav();
    GetSupplierPaymentsByID(ID)
}

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
    $('#notApproved').html(Records.NotApproved);
    $('#Approved').html(Records.Approved);
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


function GetAllSupplierPayments(filter, supplierPaymentsAdvanceSearchObj) {
    try {
        debugger;
        var supplierPaymentsAdvanceSearch = JSON.stringify(supplierPaymentsAdvanceSearchObj);
        var data = { "filter": filter , "supplierPaymentsAdvanceSearch" : supplierPaymentsAdvanceSearch };
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
    debugger;
    openNav();
        var rowData = DataTables.SupplierPaymentTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        GetSupplierPaymentsByID(rowData.ID)
    }
}

function GetSupplierPaymentsByID(PaymentID) {
    debugger;
    var thisitem = GetSupplierPayments(PaymentID)
    $('#lblheader').text('Entry No: ' + thisitem.EntryNo);
    $('#ID').val(PaymentID);
    $('#deleteId').val(PaymentID);
    $("#Supplier").select2();
    $("#Supplier").val(thisitem.supplierObj.ID).trigger('change');

    $('#hdnDocNo').val(thisitem.EntryNo);
    //$('#Supplier').val(thisitem.supplierObj.ID);
    debugger;
    $('#ddlApprovalStatus').val(thisitem.ApprovalStatus);
    $('#lblApprovalStatus').text($("#ddlApprovalStatus option:selected").text());
    $('#ApprovalDate').val(thisitem.ApprovalDate);
    $('#ddlApprovalStatus').prop('disabled', false)
   
  
  
    $('#hdfSupplierID').val(thisitem.supplierObj.ID);
    $('#Supplier').prop('disabled', true);
    $('#PaymentDate').val(thisitem.PaymentDateFormatted);
    $('#ChequeDate').val(thisitem.ChequeDate);
    $('#ChequeClearDate').val(thisitem.ChequeClearDate);
    $('#PaymentRef').val(thisitem.PaymentRef);
    $('#ReferenceBank').val(thisitem.ReferenceBank);
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
    //to get notification
    $('#lblIsNotificationSuccess').text(thisitem.IsNotificationSuccess==null?'':'Notification Sent');
    BindOutstandingAmount();

    if ($('#Type').val() == 'C') {
        $("#CreditID").html("");
        //Get Available Credit and Add with  TotalPaidAmt
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
        $('#TotalPaidAmt').prop('readonly', true);
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
    //-------------------------------------------------------------------------------------------
    debugger;
    if (thisitem.HasAccess)  //true for SEO and SAdmin
    {
        if (thisitem.ApprovalStatus == 1||thisitem.ApprovalStatus==2) {
            $('#ddlApprovalStatus').prop('disabled', false)
        }
        else {
            $('#ddlApprovalStatus').prop('disabled', true)
        }
        // ------------ Button Patch ----------------//
        if (thisitem.ApprovalStatus == 2)
            ChangeButtonPatchView('SupplierPayments', 'btnPatchAdd', 'Approve');
        else
            ChangeButtonPatchView('SupplierPayments', 'btnPatchAdd', 'Edit');
      
    }
    else  // false for manager
    {
        $('#ddlApprovalStatus').prop('disabled', true)
        $('#TotalPaidAmt').prop('readonly', true);
        if (thisitem.ApprovalStatus == 1) {
            $('#TotalPaidAmt').prop('disabled', false);
        }
        // ------------ Button Patch ----------------//
        if (thisitem.ApprovalStatus == 2)
        {
            ChangeButtonPatchView('SupplierPayments', 'btnPatchAdd', 'PaidApproved');
        }
        else if (thisitem.ApprovalStatus == 3) {
            ChangeButtonPatchView('SupplierPayments', 'btnPatchAdd', 'PaidManager');
        }
        else
        {
            ChangeButtonPatchView('SupplierPayments', 'btnPatchAdd', 'Edit');
        }
    

    }

    //-------------------------------------------------------------------------------------------

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

    if (thisitem.ApprovalStatus != 1) {
        if (!thisitem.HasAccess) {
            //enable datatable checkbox and  payment textbox
            $('.select-checkbox').prop('disabled', true)
            $('.paymentAmount').prop('disabled', true);
        }
    }
    else {
            //enable datatable checkbox and  payment textbox
            $('.select-checkbox').prop('disabled', false)
            $('.paymentAmount').prop('disabled', false);
    }

    Selectcheckbox();
    clearUploadControl();
    PaintImages(PaymentID);
}

function openNavClick() {
    debugger;
    fieldsclear();
    BindOutstanding();
    $('#lblOutstandingdetails').text('');//gibin
    $('#lblheader').text('New Payment');

    //$("#lblIsNotificationSuccess").text('Notification Not Sent');
    ChangeButtonPatchView('SupplierPayments', 'btnPatchAdd', 'Add');
    $('#Supplier').prop('disabled', false);
    $('#ReferenceBank').prop('disabled', true);
    $('#BankCode').prop('disabled', true);
    $('#ChequeDate').prop('disabled', true);
    $('#ChequeClearDate').prop('disabled', true);
    $('#Type').prop('disabled', false);
    $('#PaymentMode').prop('disabled', false);
    openNav();
    clearUploadControl();
}

function TypeOnChange() {
    if ($('#Type').val() == "C") {
        $("#ddlCreditDiv").css("visibility", "visible");
        $('#PaymentMode').val('');
        $('#BankCode').val('');
        $('#PaymentMode').prop('disabled', true);
        $('#BankCode').prop('disabled', true);
        $('#ChequeDate').prop('disabled', true);
        $('#ChequeClearDate').prop('disabled', true);
        $('#CreditID').prop('disabled', false);
        $('#TotalPaidAmt').prop('readonly', true);
        CaptionChangeCredit()
    }
    else {
        $("#ddlCreditDiv").css("visibility", "hidden");
        $('#PaymentMode').prop('disabled', false);
        $('#BankCode').prop('disabled', true); 
        $('#TotalPaidAmt').val(0);
        $('#TotalPaidAmt').prop('disabled', false);
        $('#TotalPaidAmt').prop('readonly', false);
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
    var creditID = $("#CreditID").val();
    var SupplierID = $("#Supplier").val();
    if (creditID != emptyGUID) {
        var ds = GetCreditNoteAmount(creditID, SupplierID);
        $('#TotalPaidAmt').val(ds.AvailableCredit);
        $('#TotalPaidAmt').prop('readonly', true);
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
           // $("#Type").val('P');
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

function SaveValidatedData()
{
    $(".cancel").click();
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
    //$('#hdfCreditAmount').val($('#lblPaymentApplied').text());
    $('#hdfCreditAmount').val($('#lblTotalPaidAmt').text());
    $('#AdvanceAmount').val($('#lblCredit').text());
    setTimeout(function () {
        $('#btnSave').trigger('click');
    }, 1000);
}


function savePayments() {
    debugger;
    //if ($('#PaymentMode').val() == "CHEQUE" && $("#BankCode").val() == "")
    //{
    //    notyAlert('error', 'Please Select Bank');
    //}
    if ($('#PaymentMode').val() == "ONLINE" && $("#BankCode").val() == "" || $('#PaymentMode').val() == "CHEQUE" && $("#BankCode").val() == "") {
        notyAlert('error', 'Please Select Bank');
    }
        //else if ($('#TotalPaidAmt').val() == 0) {
        //    notyAlert('error', 'Please Enter Amount');
        //}
    else {
        validate();
    }
}


   

function validate()
    {
        debugger;
        var SupplierPaymentsViewModel = new Object();
        SupplierPaymentsViewModel.PaymentRef = $("#PaymentRef").val();
        SupplierPaymentsViewModel.ID = $("#ID").val();
        var data = "{'_supplierpayObj': " + JSON.stringify(SupplierPaymentsViewModel) + "}";
        PostDataToServer("SupplierPayments/Validate/", data, function (JsonResult) {
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
            BindSupplierPaymentsHeader()
            //$('#lblOutstandingdetails').text('');//gibin
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
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
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
    $("#Supplier").select2();
    $("#Supplier").val('').trigger('change');
    $("#CreditID").html("");
    $('#Type').val('P');
    $('#ReferenceBank').val('');
    $('#hdfType').val('');
    $('#paymentDetailhdf').val('');
    $('#TotalPaidAmt').prop('disabled', false);

    $('#lblInvoiceOutstanding').text('0');
    $('#lblPaymentBooked').text('0');
    $('#lblPaymentProcessed').text('0');
    $('#lblCreditOutstanding').text('0');
    $('#lblAdvOutstanding').text('0');
    $("#lblIsNotificationSuccess").text("");
    $('#lblApprovalStatus').text($("#ddlApprovalStatus option:selected").text());
  //  $('#ddlApprovalStatus').prop('disabled', false)s
    $("#ddlCreditDiv").css("visibility", "hidden");
    CaptionChangePayment();
}
function SupplierChange() {
    if ($('#Supplier').val() != "") {
        BindCreditDropDown();
        BindOutstandingAmount();
        $('#TotalPaidAmt').val('');
        AmountChanged();
    }
    BindOutstanding();
}

function BindOutstandingAmount() {
    var thisitem = GetOutstandingAmountBySupplier($('#Supplier').val())
    if (thisitem != null) {
        debugger;
        $('#invoicedAmt').text(thisitem.OutstandingAmount == null ? "₹ 0.00" : thisitem.OutstandingAmount);
        $('#lblInvoiceOutstanding').text(thisitem.InvoiceOutstanding);
        $('#lblPaymentBooked').text(thisitem.PaymentBooked);
        $('#lblPaymentProcessed').text(thisitem.PaymentOutstanding);
        $('#lblCreditOutstanding').text(thisitem.CreditOutstanding);
        $('#lblAdvOutstanding').text(thisitem.AdvOutstanding);
    }
}
function BindOutstanding() {
    DataTables.OutStandingInvoices.clear().rows.add(GetOutStandingInvoices()).draw(false);
}

function BindSupplierPaymentsHeader() {
    try{
        debugger;
        $("#filter").hide();

        var fromdate = $("#fromdate");
        var todate = $("#todate");
        var suppliercode = $("#Supplierddl");
        var paymentMode = $("#paymode");
        var companycode = $("#Companyddl");
        var approvalStatus = $("#Statusddl");
        var search = $("#search");

        var SupplierPaymentsSearch = new Object();
        SupplierPaymentsSearch.FromDate = fromdate[0].value !== "" ? fromdate[0].value : null;
        SupplierPaymentsSearch.ToDate = todate[0].value !== "" ? todate[0].value : null;
        SupplierPaymentsSearch.Supplier = suppliercode[0].value !== "" ? suppliercode[0].value : null;
        SupplierPaymentsSearch.PaymentMode = paymentMode[0].value !== "" ? paymentMode[0].value : null;
        SupplierPaymentsSearch.Company = companycode[0].value !== "" ? companycode[0].value : null;
        SupplierPaymentsSearch.ApprovalStatus = approvalStatus[0].value !== "" ? approvalStatus[0].value : null;
        SupplierPaymentsSearch.Search = search[0].value !== "" ? search[0].value : null;

        DataTables.SupplierPaymentTable.clear().rows.add(GetAllSupplierPayments(undefined, SupplierPaymentsSearch)).draw(true);
    } catch (Ex) {
        notyAlert('error',Ex.message)
    }
}

function PaymentModeChanged() {

    //if ($('#PaymentMode').val() == "ONLINE") {
    //    $('#BankCode').prop('disabled', false);
    //}
    //else {
    //    $("#BankCode").val('');
    //    $('#BankCode').prop('disabled', true);
    //}
    //if ($('#PaymentMode').val() == "CHEQUE") {
    //    $('#BankCode').prop('disabled', false);
    //    $('#ChequeDate').prop('disabled', false);
    //    $('#ReferenceBank').prop('disabled', true);

    //}
    //else {
    //    $("#ChequeDate").val('');
    //    $('#ChequeDate').prop('disabled', true);
    //    $('#ReferenceBank').prop('disabled', true);

    //}




    if ($('#PaymentMode').val() == "ONLINE" || $('#PaymentMode').val() == "CHEQUE") {
        $('#BankCode').prop('disabled', false);

        if ($('#PaymentMode').val() == "CHEQUE") {
            $('#ChequeDate').prop('disabled', false);
            $('#ChequeClearDate').prop('disabled', false);
            $('#ReferenceBank').prop('disabled', true);
        }
        else {
            $("#ChequeDate").val('');
            $('#ChequeDate').prop('disabled', true);
            $("#ChequeClearDate").val('');
            $('#ChequeClearDate').prop('disabled', true);

            $('#ReferenceBank').prop('disabled', true);
        }
    }
    else {
        $("#BankCode").val('');
        $('#BankCode').prop('disabled', true);
        $("#ChequeDate").val('');
        $('#ChequeDate').prop('disabled', true);
        $("#ChequeClearDate").val('');
        $('#ChequeClearDate').prop('disabled', true);
        $('#ReferenceBank').prop('disabled', true);

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
//--------------------------------------------Notification,Approval,Payment Proceeding methods ---------------------------------------------------------//
function SendNotification() {
    debugger;
    $("#NotificationMessagemodal").modal('show');
    debugger; 
    $('#GeneralNotes').val($('#Notes').val());
    var description = $('#lblheader').text() + ", Supplier: " + $('#Supplier').select2('data')[0].text + ", Amount: " + $('#TotalPaidAmt').val();
    $('#lblNotyMessage').html('<b>' + description + '</b><br/>' + $('#GeneralNotes').val());
}
function ChangeNotificationMessage() {
    debugger;
    var description = $('#lblheader').text() + ", Supplier: " + $('#Supplier').select2('data')[0].text + ", Amount: " + $('#TotalPaidAmt').val();
                                
    $('#lblNotyMessage').html('<b>' + description + '</b><br/>' + $('#GeneralNotes').val());

}



function SendNotificationConfirm() {
    try {
        debugger;
        var SupplierPaymentsViewModel = new Object();
        SupplierPaymentsViewModel.ID = $('#ID').val();
        SupplierPaymentsViewModel.EntryNo = $('#lblheader').text()
        SupplierPaymentsViewModel.TotalPaidAmt = $('#TotalPaidAmt').val();
        SupplierPaymentsViewModel.GeneralNotes = $('#GeneralNotes').val().substring(0, 250)
        SupplierPaymentsViewModel.supplierObj = new Object();
        SupplierPaymentsViewModel.supplierObj.CompanyName = $('#Supplier').select2('data')[0].text;

        var data = "{'supobj':" + JSON.stringify(SupplierPaymentsViewModel) + "}";

        PostDataToServer("SupplierPayments/SendNotification/", data, function (JsonResult) {
            if (JsonResult != '') {
                switch (JsonResult.Result) {
                    case "OK":
                        notyAlert('success', JsonResult.Message);
                        GetSupplierPaymentsByID($('#ID').val());
                        $("#NotificationMessagemodal").modal('hide');
                        $("#IsNotificationSuccess").text('Notification Sent');
                        break;
                    case "ERROR":
                        notyAlert('error', JsonResult.Message);
                        GetSupplierPaymentsByID($('#ID').val());
                        $("#NotificationMessagemodal").modal('hide');

                        break;
                    default:
                        break;
                }
            }
        });
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function ApprovedPayment() {
    try {
        debugger;
        var ID = $('#ID').val();
        var SupplierPaymentsViewModel = new Object();
        SupplierPaymentsViewModel.ID = ID;
        var data = "{'supobj':" + JSON.stringify(SupplierPaymentsViewModel) + "}";

        PostDataToServer("SupplierPayments/ApprovedPayment/", data, function (JsonResult) {
            if (JsonResult != '') {
                switch (JsonResult.Result) {
                    case "OK":
                        notyAlert('success', JsonResult.Message);
                        GetSupplierPaymentsByID(ID);
                        List();
                        BindSupplierPaymentsHeader()
                        break;
                    case "ERROR":
                        notyAlert('error', JsonResult.Message);
                        break;
                    default:
                        break;
                }
            }
        }); 
    }
    catch (e) {
        notyAlert('error', e.message);
    }

    
}

//------------------------------------------------Summary Filter clicks------------------------------------------------------------//

function Gridfilter(thisobj) {
    debugger;
    $('#filter').show();
    $('#APfilter').hide();
    $('#NAfilter').hide();

    if (thisobj == 'AP') {
        $('#APfilter').show();
    }
    else if (thisobj == 'NA') {
        $('#NAfilter').show();
    } 
    $("#fromdate").val('');
    $("#todate").val('');
    $("#Supplierddl").select2();
    $("#Supplierddl").val('').trigger('change');
    $("#paymode").val('');
    $("#Statusddl").val('');
    $("#Companyddl").val('');
    $("#search").val('');
    if ($("#demo").is(":visible")) {
        $("#advfilter").click();//toggle("collapse");
    }
    var result = GetAllSupplierPayments(thisobj,undefined);
    if (result != null) {
        if (result!= null)
            DataTables.SupplierPaymentTable.clear().rows.add(result).draw(false); 
    }
}

function Reset() {
    debugger;
    $("#fromdate").val('');
    $("#todate").val('');
    $("#Supplierddl").select2();
    $("#Supplierddl").val('').trigger('change');
    $("#paymode").val('');
    $("#Statusddl").val('');
    $("#Companyddl").val('');
    $("#search").val('');
    if($( "#demo" ).is( ":visible" )){
        $("#advfilter").click();//toggle("collapse");
    }
    $('#filter').hide();
    DataTables.SupplierPaymentTable.clear().rows.add(GetAllSupplierPayments()).draw(false);
    
}


function PrintReport() {
    debugger;
    try {
        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function openRevisePopup() {
    $('#divRevisemodalPopUp').modal('show');
    
}
function PopupSaveClick() {
    debugger;
    var Reason = "Old Amount of entry no: " +  $('#hdnDocNo').val() + " is " + $('#TotalPaidAmt').val()+ " Reason of revising document is ";
    $('#hdnOldAmount').val(Reason);
   // $('#hdnDocNo').val($('#lblheader').text());
    $('#hdnDocType').val('Make Payment');
  
    $('#hdnDocumentID').val($('#ID').val());
    $('#btnPoupSave').trigger('click');
}
function PopupCancelClick() {
    //$('#divRevisemodalPopUp').modal('show');
    //setTimeout(function () {
    //    $('#btnSave').trigger('click');
    //}, 1000);
}
function ReviseSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":     
            GetSupplierPaymentsByID(JsonResult.Records.DocumentID)
            BindSupplierPaymentsHeader()
            debugger;
            if ($('#Type').val() == "C") {
                $('#TotalPaidAmt').prop('readonly', true);
            } else {
                $('#TotalPaidAmt').prop('readonly', false);
            }
            notyAlert('success', "Document Revised Successfully");
            $('#divRevisemodalPopUp').modal('hide');
           
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
