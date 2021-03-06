﻿//Declaration of global variables,which can be accessible through out the program
var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
//A way to run JavaScript code as soon as the page's Document Object Model (DOM) becomes safe to manipulate. 
$(document).ready(function () {
    try {
        debugger;
        //For implementating select2
        $("#ddlCustomer,#Customerddl").select2({
        });
        $('#btnUpload').click(function () {
            //Pass the controller name
            var FileObject = new Object;
            if ($('#hdnFileDupID').val() != emptyGUID)
            {
                FileObject.ParentID = (($('#ID').val()) != "" ? ($('#ID').val()) : $('#hdnFileDupID').val());
            }
            else
            {
                FileObject.ParentID = $('#ID').val();
            }
            
            FileObject.ParentType = "CusInvoice";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });
        //To bind values to datatable
        DataTables.CustInvTable = $('#CustInvTable').DataTable(
         {
             //dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                              }
             }],
             order: [],
             searching: false,
             paging: true,
             data: null,
             pageLength: 15,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search"
             //},
             columns: [              
               { "data": "ID" },
               { "data": "InvoiceNo" },
               { "data": "InvoiceDateFormatted" },
               { "data": "customerObj.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "TotalInvoiceAmount", "defaultContent": "<i>-</i>" },
               { "data": "BalanceDue", "defaultContent": "<i>-</i>" },          
               { "data": "LastPaymentDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "companiesObj.Name", "defaultContent": "<i>-</i>" },
               { "data": "Status", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [5, 6] },
                   { className: "text-left", "targets": [1,3,8,9] },
             { className: "text-center", "targets": [2,4,7,10] }

             ]
         });
        //print button is hided in the page
        $(".buttons-excel").hide();
        //on double clicking edit it will redirect to edit page
        $('#CustInvTable tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
        $('input[type="text"].Roundoff').on('focus', function () {
            $(this).select();
        });
        //show loading gif on refreshing the page
        showLoader();
        List();
       //roundoff the digits upto two decimal place
        $('.Roundoff').on('change', function () {
            var CustomerInvoiceViewModel = new Object();
            CustomerInvoiceViewModel.GrossAmount = $('#txtGrossAmt').val();
            CustomerInvoiceViewModel.Discount = ((parseInt($('#txtDiscount').val())) > (parseInt($('#txtGrossAmt').val()))) ? "0.00" : $('#txtDiscount').val();
            CustomerInvoiceViewModel.NetTaxableAmount = (CustomerInvoiceViewModel.GrossAmount - CustomerInvoiceViewModel.Discount)
            CustomerInvoiceViewModel.TaxType = $('#ddlTaxType').val() != "" ? GetTaxRate($('#ddlTaxType').val()) : $('#txtTaxPercApp').val();
            CustomerInvoiceViewModel.TaxPercentage = CustomerInvoiceViewModel.TaxType
            CustomerInvoiceViewModel.TaxAmount =(CustomerInvoiceViewModel.NetTaxableAmount*CustomerInvoiceViewModel.TaxPercentage)/100
            CustomerInvoiceViewModel.TotalInvoiceAmount = (CustomerInvoiceViewModel.NetTaxableAmount + CustomerInvoiceViewModel.TaxAmount) 
            $('#txtNetTaxableAmt').val(CustomerInvoiceViewModel.NetTaxableAmount);
            $('#txtTaxPercApp').val(CustomerInvoiceViewModel.TaxPercentage);
            $('#txtTaxAmt').val(roundoff(Math.round(CustomerInvoiceViewModel.TaxAmount)));
            $('#txtTotalInvAmt').val(roundoff(Math.round(CustomerInvoiceViewModel.TotalInvoiceAmount)));
            if((parseInt($('#txtDiscount').val())) > (parseInt($('#txtGrossAmt').val())))
            {
                $('#txtDiscount').val("0.00");
            }
            
        });
        $('#txtTaxPercApp').on('keypress', function () {
            debugger;
            if($('#ddlTaxType').val()!="")
                $('#ddlTaxType').val('')
        });


        //Bind value to modal pop up
        //------------------------Modal Popup Advance Adujustment-------------------------------------//
        DataTables.OutStandingInvoices = $('#tblOutStandingDetails').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: false,
            paging: false,
            data: null,
            columns: [
                 { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": null, "defaultContent": "" },
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
                     "data": "CustPaymentObj.CustPaymentDetailObj.PaidAmount", 'render': function (data, type, row) {
                         return '<input class="form-control text-right paymentAmount" name="Markup" value="' + roundoff(data) + '" onfocus="this.select();" onchange="PaymentAmountChanged(this);" id="PaymentValue" type="text">';
                     }, "width": "15%"
                 },
                 {  "data": "CustPaymentObj.CustPaymentDetailObj.ID", "defaultContent": "<i>-</i>"  }
            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 1 }
                , { className: "text-right", "targets": [4, 5, 6] }
                , { "targets": [0, 8], "visible": false, "searchable": false }
                , { "targets": [2, 3, 4, 5, 6, 7], "bSortable": false }],

            select: { style: 'multi', selector: 'td:first-child' }    
        });

        $('#tblOutStandingDetails tbody').on('click', 'td:first-child', function (e) {
            debugger;
            var rt = DataTables.OutStandingInvoices.row($(this).parent()).index()
            var table = $('#tblOutStandingDetails').DataTable();
            var allData = table.rows().data();
            if ((allData[rt].CustPaymentObj.CustPaymentDetailObj.PaidAmount) > 0) {
                allData[rt].CustPaymentObj.CustPaymentDetailObj.PaidAmount = roundoff(0)
                DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);
                var sum = 0;
                AmountReceived = parseFloat($('#AdvanceAmount').text());
                for (var i = 0; i < allData.length; i++) {
                    sum = sum + parseFloat(allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount);
                }
                $('#lblPaymentApplied').text(roundoff(sum));
                $('#lblCredit').text(roundoff(AmountReceived - sum));
                Selectcheckbox();
            }
        });


        //------------------------Modal Popup Special Payment-------------------------------------//
        DataTables.SpecialPayments = $('#tblSpecialPayments').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: false,
            paging: true,
            pageLength: 4,
            data: null,
            columns: [
                 { "data": "SpecialPayObj.ID", "defaultContent": "<i>-</i>" },
                 { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },
                 { "data": "SpecialPayObj.SpecialPaymentDate", "defaultContent": "<i>-</i>", "width": "15%" },
                 { "data": "SpecialPayObj.PaymentMode", "defaultContent": "<i>-</i>" },
                 { "data": "SpecialPayObj.ChequeDate", "defaultContent": "<i>-</i>", "width": "15%" },
                 { "data": "SpecialPayObj.PaymentRef", "defaultContent": "<i>-</i>" },
                 { "data": "SpecialPayObj.RefBank", "defaultContent": "<i>-</i>" },
                 { "data": "SpecialPayObj.Remarks", "defaultContent": "<i>-</i>" },
                 { "data": "SpecialPayObj.SpecialPaidAmount", "defaultContent": "<i>-</i>" },
                 { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditSpecialPayment(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
                 { "data": null, "orderable": false, "defaultContent": '<a data-toggle="tp" data-placement="top" data-delay={"show":2000, "hide":3000} title="Delete Payment" href="#" class="DeleteLink" onclick="DeleteSpecialPayment(this)"><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }

            ],
            createdRow: function (row, data, index) {
                if (data.Type == "<b>Total</b>") {
                    $('td', row).addClass('totalRow');
                }
            },
            columnDefs: [
                { className: "text-right", "targets": [8] },
                { className: "text-center", "targets": [2,4] },
                { className: "text-left", "targets": [1,3,5,6,7] },
                { "targets": [0], "visible": false, "searchable": false }]

        });
        debugger;
        if ($('#BindValue').val() != '') {
            dashboardBind($('#BindValue').val())
        } 

    }
    catch (x) {
        notyAlert('error', e.message);
    }
});


//To print the page to excel format up on clicking the print button,which triggers the hidden button
function PrintReport() {
    try {
        debugger;

        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}



//Bind values to dashboard
function dashboardBind(ID) {
    Resetform();
    $('#ID').val(ID);
    $('#deleteId').val(ID);
    $('#InvoiceId').val(ID);
    PaintInvoiceDetails();
    openNav();
}

//alert msg on clicking delete button ,whether to proceed or not with the delete
function DeleteInvoices() {
    notyConfirm('Are you sure to delete?', 'Delete()', '', "Yes, delete it!");
}

//Check the discount amount is zero if so,then round off to 0.00
function CheckAmount() {
    if($("#txtDiscount").val() == "")
    $("#txtDiscount").val(roundoff(0));
}

//Trigger the delete button
function Delete()
{ 
    $('#btnFormDelete').trigger('click'); 
}

//trigger the save button to save the invoices
function saveInvoices() {
    debugger; 
      $('#btnSave').trigger('click');
}

//Notification on delete-whether success or failure
function DeleteSuccess(data, status) {
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            AddNew();
            List();
            notyAlert('success', JsonResult.Message);
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

//Notification on Save-whether save success or failure
function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            var res;
            if ($('#ID').val() == "") {
                res = Advanceadjustment(); //calling advance adjustment popup if inserting
            }
            if (!res) {
                notyAlert('success', JsonResult.Message);
            }
            $('#ID').val(JsonResult.Records.ID);
            $('#deleteId').val(JsonResult.Records.ID);
            $('#InvoiceId').val(JsonResult.Records.ID);

            PaintInvoiceDetails()
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

//------------------------Modal Popup Advance Adujustment-------------------------------------//
function Advanceadjustment() {
    debugger;
    var customerId= $('#ddlCustomer').val();
    //get advances of customer
    var thisitem = GetCustomerAdvances(customerId);
    if (thisitem != null && thisitem.customerObj.AdvanceAmount > 0) {
        $('#AdvanceAmount').text(roundoff(thisitem.customerObj.AdvanceAmount));
        $('#AdvAdjustmentModel').modal('show');
        DataTables.OutStandingInvoices.clear().rows.add(GetOutStandingInvoices(customerId)).draw(false);
        AmountChanged();
        return true;
    }
    else {
        return false;
    }
}

//save advanceadjust upon inserting or updating
function SaveAdvanceAdujust() {
    debugger;
    var SelectedRows = DataTables.OutStandingInvoices.rows(".selected").data();
    if ((SelectedRows) && (SelectedRows.length > 0)) {
        var CustomerPaymentsViewModel=new Object();
        CustomerPaymentsViewModel.CustomerPaymentsDetail=[]
        for (var r = 0; r < SelectedRows.length; r++) {
            var PaymentDetailViewModel = new Object();

            PaymentDetailViewModel.InvoiceID = SelectedRows[r].ID;//Invoice ID
            PaymentDetailViewModel.ID = SelectedRows[r].CustPaymentObj.CustPaymentDetailObj.ID//Detail ID
            PaymentDetailViewModel.PaidAmount = SelectedRows[r].CustPaymentObj.CustPaymentDetailObj.PaidAmount;
            CustomerPaymentsViewModel.CustomerPaymentsDetail.push(PaymentDetailViewModel)
        }
        var CustomerViewModel=new Object();
        CustomerViewModel.ID=$('#ddlCustomer').val();
        CustomerPaymentsViewModel.customerObj = CustomerViewModel;
        //PostDataToServer
        try {
            var data = "{'_custpayObj':" + JSON.stringify(CustomerPaymentsViewModel) + "}";
            PostDataToServer("CustomerPayments/InsertPaymentAdjustment/", data, function (JsonResult) {
                if (JsonResult != '') {
                    switch (JsonResult.Result) {
                        case "OK":
                            notyAlert('success', JsonResult.Message);
                            List();
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
    $('#AdvAdjustmentModel').modal('hide');
}

function AmountChanged() {
    debugger;
    var sum = 0;
    DataTables.OutStandingInvoices.rows().deselect();  
    AmountReceived = parseFloat($('#AdvanceAmount').text())  
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
function PaymentAmountChanged(this_Obj) {
    debugger;
    AmountReceived = parseFloat($('#AdvanceAmount').text())
    sum = 0;
    var allData = DataTables.OutStandingInvoices.rows().data();
    var table = DataTables.OutStandingInvoices;
    var rowtable = table.row($(this_Obj).parents('tr')).data();

    for (var i = 0; i < allData.length; i++) {
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
                if (currenttotal < AmountReceived) {
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
        if (allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount == "" || allData[i].CustPaymentObj.CustPaymentDetailObj.PaidAmount == roundoff(0)) {
            DataTables.OutStandingInvoices.rows(i).deselect();
        }
        else {
            DataTables.OutStandingInvoices.rows(i).select();
        }
    }
}
//Get Customer advances based on Id
function GetCustomerAdvances(ID) {
    try {
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetCustomerAdvances/", data);
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

//Get outstanding invoices based on customer Id
function GetOutStandingInvoices(CustID) {
    try {
        debugger;
        var PaymentID = emptyGUID; 
        var data = { "CustID": CustID, "PaymentID": PaymentID };
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
//------------------------Modal Popup Advance Adujustment functions Ends-------------------------------------//



function GetTaxRate(Code)
{
    debugger;
    try {
        var data = { "Code": Code };
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetTaxRate/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            return 0;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function List() {
    $('#filter').hide();
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

//edit the data
function Edit(Obj) {
    debugger;

    Resetform();
    var rowData = DataTables.CustInvTable.row($(Obj).parents('tr')).data();
    $('#ID').val(rowData.ID);
    $('#deleteId').val(rowData.ID); 
    $('#InvoiceId').val(rowData.ID);
    PaintInvoiceDetails();
    openNav();
}

//Form is resetted 
function Resetform() {
    var validator = $("#CustomerInvoiceForm").validate();
    $('#CustomerInvoiceForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    $('#CustomerInvoiceForm')[0].reset();
}

function FilterReset()
{
    $("#fromdate").val('');
    $("#todate").val('');
    $("#Customerddl").val('').trigger('change')
    $("#ddlInvoiceTypes").val('');
    $("#Companyddl").val('');
    $("#ddlInvoiceTypesStatus").val('');
    $("#search").val('');
    List();
}
//New form to add data
function AddNew()
{
    debugger;
    Resetform();
    $('#lblinvoicedAmt').text("₹ 0.00");
    $('#lblpaidAmt').text("₹ 0.00");
    $('#lblbalalnceAmt').text("₹ 0.00");
    $('#ID').val('');
    $('#lblInvoiceNo').text("New Invoice");
    $("#ddlCustomer").select2();
    $("#ddlCustomer").val('').trigger('change');
    $('#ddlCustomer').prop('disabled', false);
    $('#txtInvNo').prop('disabled', false);
    $('#ddlRefInvoice').prop('disabled', true);
    $('#ddlInvoiceType').prop('disabled', false);
    $('#paymentStatus').hide();
    ChangeButtonPatchView('CustomerInvoices', 'btnPatchAdd', 'Add');
    openNav();
    clearUploadControl();
}

function Reset() {
    debugger; 
        PaintInvoiceDetails(); 
}
function PaintInvoiceDetails()
{
    debugger;
    ChangeButtonPatchView('CustomerInvoices', 'btnPatchAdd', 'Edit');
    var InvoiceID = $('#ID').val();
    var CustomerInvoicesViewModel = GetCustomerInvoiceDetails(InvoiceID);
    $('#lblInvoiceNo').text(CustomerInvoicesViewModel.InvoiceNo); 
    $('#txtInvDate').val(CustomerInvoicesViewModel.InvoiceDateFormatted);
    $("#ddlCustomer").select2();
    $("#ddlCustomer").val(CustomerInvoicesViewModel.customerObj.ID).trigger('change');
    $('#ddlCompany').val(CustomerInvoicesViewModel.companiesObj.Code);
    //$('#ddlCustomer').val(CustomerInvoicesViewModel.customerObj.ID);
    $('#hdfCustomerID').val(CustomerInvoicesViewModel.customerObj.ID);
    $('#ddlCustomer').prop('disabled', true);
    //------------------------------------------------
    $('#ddlInvoiceType').prop('disabled', true);
 
    debugger;
    $('#ddlInvoiceType').val(CustomerInvoicesViewModel.InvoiceType);
   
    BindInvocieReferenceDropDown(CustomerInvoicesViewModel.customerObj.ID);//dropdownbinding
    if ($('#ddlInvoiceType').val() == 'PB') 
        $('#ddlRefInvoice').val(CustomerInvoicesViewModel.RefInvoice);
    else
        $('#ddlRefInvoice').val(-1);
    if ($('#ddlInvoiceType').val() == 'PB' || $('#ddlInvoiceType').val() == 'WB')
        $('#paymentStatus').show();
    else
        $('#paymentStatus').hide();

    InvoicesTypeChange();
    $('#ddlSpecialPayStatus').val(CustomerInvoicesViewModel.SpecialPayStatus);    
    
    $('#txtInvNo').val(CustomerInvoicesViewModel.InvoiceNo);
    //------------------------------------------------
    $('#txtBillingAddress').val(CustomerInvoicesViewModel.BillingAddress);
    $('#ddlPaymentTerm').val(CustomerInvoicesViewModel.paymentTermsObj.Code);
    $('#txtPayDueDate').val(CustomerInvoicesViewModel.PaymentDueDateFormatted);
    $('#txtGrossAmt').val(CustomerInvoicesViewModel.GrossAmount);
    $('#txtDiscount').val(CustomerInvoicesViewModel.Discount);
    $('#txtNetTaxableAmt').val(CustomerInvoicesViewModel.GrossAmount - CustomerInvoicesViewModel.Discount);
    $('#ddlTaxType').val(CustomerInvoicesViewModel.TaxTypeObj.Code);
    $('#txtTaxPercApp').val(CustomerInvoicesViewModel.TaxPercApplied);
    $('#txtTaxAmt').val(roundoff(CustomerInvoicesViewModel.TaxAmount));
    $('#txtTotalInvAmt').val(roundoff(CustomerInvoicesViewModel.TotalInvoiceAmount));
    $('#txtNotes').val(CustomerInvoicesViewModel.Notes);
    $('#ID').val(CustomerInvoicesViewModel.ID);
    $('#lblinvoicedAmt').text(CustomerInvoicesViewModel.TotalInvoiceAmountstring);
    $('#lblpaidAmt').text(CustomerInvoicesViewModel.PaidAmountstring);
    $('#lblbalalnceAmt').text(CustomerInvoicesViewModel.BalanceDuestring);
    clearUploadControl();
    PaintImages(InvoiceID);

}
//Filter the table based on the filter conditions
//---------------Bind logics-------------------
function GetAllInvoicesAndSummary(filter) {
    try {
        if ($("#fromdate").val() !== "")
            var fromdate = $("#fromdate").val();
        if ($("#todate").val() !== "")
            var todate = $("#todate").val();
        if($("#Customerddl").val()!="")
            var customercode = $("#Customerddl").val();
        if($("#ddlInvoiceTypes").val()!= "")
            var invoicetype = $("#ddlInvoiceTypes").val();
        if($("#Companyddl").val() != ""  )
            var companycode = $("#Companyddl").val();
        if($("#ddlInvoiceTypesStatus").val() != "" )
            var status = $("#ddlInvoiceTypesStatus").val();
        if($("#search").val()!= "")
        var search = $("#search").val();
        var data = { "filter": filter ,"FromDate":fromdate,"ToDate":todate,"Customer":customercode,"InvoiceType":invoicetype,"Company":companycode,"Status":status,"Search":search};
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

//to refresh datatable on clicking button 'Apply'
function RefreshInvoicesAndSummary() {
    try {
        $('#filter').hide();
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#Companyddl").val();
        var result = GetAllInvoicesAndSummary();
        if (result != null) {
            if (result.CustomerInvoices != null)
                DataTables.CustInvTable.clear().rows.add(result.CustomerInvoices).draw(true);
            if (result.CustomerInvoiceSummary != null) {
                Summary(result.CustomerInvoiceSummary);
            }
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

//onchange function for the customer dropdown to fill:- due date and Address 
function FillCustomerDefault(this_Obj)
{
    try {
        debugger;
        if (this_Obj.value != "")
        {
            var ID = this_Obj.value;
            var CustomerViewModel = GetCustomerDetails(ID);
            $('#txtBillingAddress').val(CustomerViewModel.BillingAddress);
            $('#ddlPaymentTerm').val(CustomerViewModel.PaymentTermCode);
            $('#ddlPaymentTerm').trigger('change');
            //if ($('#ddlInvoiceType').val() == "PB") {
            //Bind only if invoice type is PB 
            BindInvocieReferenceDropDown(ID);
            // }

        }
    }
    catch(e)
    {

    }
}
//Get Customer Details based on ID
function GetCustomerDetails(ID)
{
    try {
        var data = {"ID":ID};
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetCustomerDetails/", data);
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
//Bind Payment due date based on Payment date
function GetDueDate(this_Obj)
{
    try
    {
        debugger;
        var Code = this_Obj.value;
        var PaymentTermViewModel = GetPaymentTermDetails(Code);
         $('#txtPayDueDate').val(PaymentTermViewModel);
    }
    catch(e)
    {

    }
}
function GetPaymentTermDetails(Code)
{
    debugger;
    try {
        var data = { "Code":Code,"InvDate":$('#txtInvDate').val() };
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetDueDate/", data);
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
//Based on invoice id get customer invoice details
function GetCustomerInvoiceDetails()
{
    try {
        var InvoiceID = $('#ID').val();
        var data = {"ID":InvoiceID};
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetCustomerInvoiceDetails/", data);
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

//Disabling and enabling invoice reference based on invoice type
//-----------------------------------------------------------------------------------------------------------------//

function InvoicesTypeChange() {
    debugger;
    if ($('#ddlInvoiceType').val() == "PB" ) {
        $('#ddlRefInvoice').prop('disabled', false);
        $('#txtInvNo').prop('disabled', true);
        $('#txtInvNo').val('');
    }
    else if ($('#ddlInvoiceType').val() == "WB") {
        $('#ddlRefInvoice').prop('disabled', true);
        $('#txtInvNo').prop('disabled', true);
        $('#txtInvNo').val('');
        $('#ddlRefInvoice').val(-1);
    }
    else {
        $('#ddlRefInvoice').prop('disabled', true);
        $('#ddlRefInvoice').val(-1);
        $('#txtInvNo').prop('disabled', false);
    }
     
    
}

function BindInvocieReferenceDropDown(ID) {
    debugger;
    try {
        var item = GetAllCustomerInvociesByID(ID);
        if (item) {
            $('#ddlRefInvoice').empty();
            $('#ddlRefInvoice').append(new Option('-- Select Invoice --', -1));
            for (var i = 0; i < item.length; i++) {
                var opt = new Option(item[i].InvoiceNo + '-' + item[i].companiesObj.Name, item[i].ID);
                $('#ddlRefInvoice').append(opt);
            }
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetAllCustomerInvociesByID(CustomerID) {
    try {
        debugger;
        
        var data = { "CustomerID": CustomerID };
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetAllCustomerInvociesByCustomerID/", data);
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
//--------------------------------------Special Payments--------------------------------------------//
function SpecialPayments() {
    debugger
    $('#SpecialPaymentModel').modal('show');//
    $("#newBtn").attr("disabled", true);
    $("#saveBtn").attr("disabled", true);
    NewSpecialPayments() 
    BindSpecialPaymentsTable();
}

function BindSpecialPaymentSummary() {
    debugger;
    var items = SpecialPaymentSummary();
    $('#SplInvAmount').text('');
    $('#SplInvAmount').append('<b>' + items.TotalInvoiceAmountstring + '</b>');
    $('#SplAmountReceived').text('');
    $('#SplAmountReceived').append('<b>' + items.PaidAmountstring + '</b>');
    $('#SplBalDue').text('');
    $('#SplBalDue').append('<b>' + items.BalanceDuestring + '</b>');
    $('#hdfBalanceDue').val(items.BalanceDue);

}
function SpecialPaymentSummary() {
    debugger;
    try {
        debugger;
        var InvoiceID = $('#ID').val();
        var data = { "InvoiceID": InvoiceID };
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/SpecialPaymentSummary/", data);
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

function BindSpecialPaymentsTable() {
    DataTables.SpecialPayments.clear().rows.add(GetSpecialPayments()).draw(false);

}

function GetSpecialPayments() {
    try {
        debugger;
        var InvoiceID = $('#ID').val();
        var data = { "InvoiceID": InvoiceID };
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetAllSpecialPayments/", data);
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

function EditSpecialPayment(this_obj)
{
    debugger;
    var rowDataSplPay = DataTables.SpecialPayments.row($(this_obj).parents('tr')).data(); 
    BindSpecialPayment(rowDataSplPay.SpecialPayObj.ID)
}

function BindSpecialPayment(ID) {
    debugger;
    var itemsdetails = GetSpecialPaymentsDetails(ID)
    $('#PaymentID').val(ID);
    $('#txtRemarks').val(itemsdetails.SpecialPayObj.Remarks);
    $('#txtSpecialPaidAmount').val(itemsdetails.SpecialPayObj.SpecialPaidAmount);
    $('#txtSplPaymentDate').val(itemsdetails.SpecialPayObj.SpecialPaymentDate);
    $('#PaymentMode').prop('disabled', true);
    $('#PaymentMode').val(itemsdetails.SpecialPayObj.PaymentMode);
    $('#ChequeDate').val(itemsdetails.SpecialPayObj.ChequeDate);
    $('#PaymentRef').val(itemsdetails.SpecialPayObj.PaymentRef);
    $('#RefBank').val(itemsdetails.SpecialPayObj.RefBank);
    PaymentModeChanged();
    BindSpecialPaymentSummary(); //resetting Balance due Amount 
    $('#hdfBalanceDue').val(parseInt($('#hdfBalanceDue').val())+parseInt($('#txtSpecialPaidAmount').val()));

}

function GetSpecialPaymentsDetails(ID) {
    try {
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("CustomerInvoices/GetSpecialPaymentsDetails/", data);
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


function SaveSpecialPayments() {
    debugger;
    $('#btnSaveSpecialPayments').trigger('click');
}

function PaymentModeChanged() {
    debugger;
    //if ($('#PaymentMode').val() == "ONLINE") {
    //}
    //else {
    //}
    if ($('#PaymentMode').val() == "CHEQUE") {
        $('#ChequeDate').prop('disabled', false);
        $('#RefBank').prop('disabled', false);

    }
    else {
        $("#ChequeDate").val('');
        $("#RefBank").val('');
        $('#ChequeDate').prop('disabled', true);
        $('#RefBank').prop('disabled', true);

    }

}



function NewSpecialPayments() {
    debugger;

    BindSpecialPaymentSummary();
    $('#txtRemarks').val('');
    $('#PaymentID').val('');
    $('#txtSpecialPaidAmount').val('');
    $('#txtSplPaymentDate').val('');
    $('#PaymentMode').val('');
    $('#ChequeDate').val('');
    $('#PaymentRef').val('');
    $('#RefBank').val('');
    $('#PaymentMode').prop('disabled', false);
    PaymentModeChanged();
}


function SuccessSpecialPayments(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindSpecialPaymentsTable();
            BindSpecialPaymentSummary();
            PaintInvoiceDetails()
            BindSpecialPayment(JsonResult.Records.SpecialPayObj.ID);
            List();
            notyAlert('success', JsonResult.Message);
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}



function DeleteSpecialPayment(currObj) {
    debugger;
    var rowData = DataTables.SpecialPayments.row($(currObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteSpecialPay("'+ rowData.SpecialPayObj.ID +'")', '', "Yes, delete it!");
    } 
}
function DeleteSpecialPay(ID) {
    try {
        debugger;
        if (ID) {
            var data = { "SpecialPayObj.ID": ID };
            var ds = {};
            ds = GetDataFromServer("CustomerInvoices/DeleteSpecialPayments/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message.Message);
                BindSpecialPaymentsTable(); 
                PaintInvoiceDetails()
                NewSpecialPayments();
                List();
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return 0;
            }
            return 1;
        }

    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function PaidAmountonblur(thisObj) {
    debugger;
    if (thisObj.value!="")
        if (parseInt($('#hdfBalanceDue').val()) < parseInt(thisObj.value))
        {
            notyAlert('error', "Amount should be less than Balance due");
            $('#txtSpecialPaidAmount').val($('#hdfBalanceDue').val());
        } 
}

//------------------------------------------------Summary Filter clicks------------------------------------------------------------//

function Gridfilter(thisobj) {
    debugger;
    $('#filter').show();

    $('#ODfilter').hide();
    $('#OIfilter').hide();
    $('#FPfilter').hide();

    if (thisobj == 'OD')   {
        $('#ODfilter').show();
    }
    else if (thisobj == 'OI')    {
        $('#OIfilter').show();
    }
    else if (thisobj == 'FP')    {
        $('#FPfilter').show();
    }
   var result= GetAllInvoicesAndSummary(thisobj); 
    if (result != null) {
        if (result.CustomerInvoices != null)
            DataTables.CustInvTable.clear().rows.add(result.CustomerInvoices).draw(false);
        if (result.CustomerInvoiceSummary != null) {
            Summary(result.CustomerInvoiceSummary);
        }
    }
}


function OpenCustomerPbPayment() {
    
    var url = '../SpecialInvPayments/Index';
    window.location.href = url;
}
