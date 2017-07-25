var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {
        $('#btnUpload').click(function () {
            //Pass the controller name
            var FileObject = new Object;
            FileObject.ParentID = $('#ID').val();
            FileObject.ParentType = "CusInvoice";
            FileObject.Controller = "FileUpload";
            UploadFile(FileObject);
        });
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
               { "data": "customerObj.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "PaymentDueDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "TotalInvoiceAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
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
        $('input[type="text"].Roundoff').on('focus', function () {
            $(this).select();
        });
        showLoader();
        List();
       
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
            $('#txtTaxAmt').val(CustomerInvoiceViewModel.TaxAmount);
            $('#txtTotalInvAmt').val(CustomerInvoiceViewModel.TotalInvoiceAmount);
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
    }
    catch (x) {
        notyAlert('error', e.message);
    }
});

function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            $('#ID').val(JsonResult.Records.ID);
            PaintInvoiceDetails()
            List();
            debugger;
            var res = notyAlert('success', JsonResult.Message);
            Advanceadjustment(); //calling advance adjustment popup
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
    if (thisitem != null) {
        $('#AdvanceAmount').text(roundoff(thisitem.customerObj.AdvanceAmount));
        $('#AdvAdjustmentModel').modal('show');
        DataTables.OutStandingInvoices.clear().rows.add(GetOutStandingInvoices(customerId)).draw(false);
        AmountChanged();
    }
}
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
        CustomerPaymentsViewModel.customerObj=CustomerViewModel;
        }
        //PostDataToServer
        try {
            debugger;
            var data = "{'_custpayObj':" + JSON.stringify(CustomerPaymentsViewModel) + "}";
            PostDataToServer("CustomerPayments/InsertPaymentAdjustment/", data, function (JsonResult) {
                if (JsonResult != '') {
                    switch (JsonResult.Result) {
                        case "OK":
                            notyAlert('success', JsonResult.Record.Message);
                            break;
                        case "ERROR":
                            notyAlert('error', JsonResult.Record.Message);
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
    debugger;
    $('#CustomerInvoiceForm')[0].reset();
    var rowData = DataTables.CustInvTable.row($(Obj).parents('tr')).data();
    $('#ID').val(rowData.ID);
    PaintInvoiceDetails();
    openNav();
}
function AddNew()
{
    $('#CustomerInvoiceForm')[0].reset();
    $('#lblinvoicedAmt').text("₹ 0.00");
    $('#lblpaidAmt').text("₹ 0.00");
    $('#lblbalalnceAmt').text("₹ 0.00");
    $('#ID').val('');
    $('#lblInvoiceNo').text("New Invoice");
    openNav();
}
function PaintInvoiceDetails()
{
    debugger;
    var InvoiceID = $('#ID').val();
    var CustomerInvoicesViewModel = GetCustomerInvoiceDetails(InvoiceID);
    $('#lblInvoiceNo').text(CustomerInvoicesViewModel.InvoiceNo);
    $('#txtInvNo').val(CustomerInvoicesViewModel.InvoiceNo);
    $('#txtInvDate').val(CustomerInvoicesViewModel.InvoiceDateFormatted);
    $('#ddlCompany').val(CustomerInvoicesViewModel.companiesObj.Code);
    $('#ddlCustomer').val(CustomerInvoicesViewModel.customerObj.ID);
    $('#txtBillingAddress').val(CustomerInvoicesViewModel.BillingAddress);
    $('#ddlPaymentTerm').val(CustomerInvoicesViewModel.paymentTermsObj.Code);
    $('#txtPayDueDate').val(CustomerInvoicesViewModel.PaymentDueDateFormatted);
    $('#txtGrossAmt').val(CustomerInvoicesViewModel.GrossAmount);
    $('#txtDiscount').val(CustomerInvoicesViewModel.Discount);
    $('#txtNetTaxableAmt').val(CustomerInvoicesViewModel.GrossAmount - CustomerInvoicesViewModel.Discount);
    $('#ddlTaxType').val(CustomerInvoicesViewModel.TaxTypeObj.Code);
    $('#txtTaxPercApp').val(CustomerInvoicesViewModel.TaxPercApplied);
    $('#txtTaxAmt').val(CustomerInvoicesViewModel.TaxAmount);
    $('#txtTotalInvAmt').val(CustomerInvoicesViewModel.TotalInvoiceAmount);
    $('#txtNotes').val(CustomerInvoicesViewModel.Notes);
    $('#ID').val(CustomerInvoicesViewModel.ID);
    $('#lblinvoicedAmt').text(CustomerInvoicesViewModel.TotalInvoiceAmountstring);
    $('#lblpaidAmt').text(CustomerInvoicesViewModel.PaidAmountstring);
    $('#lblbalalnceAmt').text(CustomerInvoicesViewModel.BalanceDuestring);


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
//onchange function for the customer dropdown to fill:- due date and Address 
function FillCustomerDefault(this_Obj)
{
    try
    {
        var ID = this_Obj.value;
        var CustomerViewModel = GetCustomerDetails(ID);
        $('#txtBillingAddress').val(CustomerViewModel.BillingAddress);
        $('#ddlPaymentTerm').val(CustomerViewModel.PaymentTermCode);
        $('#ddlPaymentTerm').trigger('change');
    }
    catch(e)
    {

    }
}
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
        var data = { "Code":Code };
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
