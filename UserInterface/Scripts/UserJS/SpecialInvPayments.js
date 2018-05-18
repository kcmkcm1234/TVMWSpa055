var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000';
var sum = 0;
var index = 0;
var AmountReceived = 0;
var pbBalance = 0;
var actionmode=0;
$(document).ready(function () {
    try {
        $("#Customerddl").select2();
     
        DataTables.CustomerSpclPaymentTable = $('#CustPayTable').DataTable(
          {
              dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
              order: [],
              searching: false,
              paging: true,
              data: GetAllCustomerSpecialPayments(0),
              columns: [
                   { "data": "GroupID", "defaultContent": "<i>-</i>" },
                   { "data": "paymentDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "PaymentRef", "defaultContent": "<i>-</i>" },
                  { "data": "customerObj.CompanyName", "defaultContent": "<i>-</i>" },
                  // { "data": "companiesObj.Name", "defaultContent": "<i>-</i>" },//render customerObj.ContactPerson
                 
                   { "data": "Remarks", "defaultContent": "<i>-</i>" },
                     { "data": "specialDetailObj.PaidAmount", "defaultContent": "<i>-</i>" },
                   { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
              ],
              columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [5] },
                    { className: "text-Left", "targets": [2, 3, 4] },
                  { className: "text-center", "targets": [1, 6] }
              ]
          });
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }

    $('#CustPayTable tbody').on('dblclick', 'td', function () {
        Edit(this)
    });
   
    try {
        $("#Customer").select2();
      
        DataTables.OutStandingInvoices = $('#tblOutStandingDetails').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
            paging: false,
            data: null,
            columns: [
               { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": "Checkbox", "defaultContent": "", "width": "5%" },
                 {
                     "data": "Description", 'render': function (data, type, row) {
                        
                         return 'Inv# :<b>' + row.InvoiceNo + '</b>  Date :<b>' + row.InvoiceDate + '</b><br/>Company :<b>' + row.companiesObj.Name+'</b>'
                     }, "width": "30%"
                 },
                 { "data": "PaymentDueDate", "defaultContent": "<i>-</i>", "width": "10%" },
                 {
                     "data": "InvoiceAmount", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                        
                         return roundoff(row.InvoiceAmount)
                     }
                 },
                {
                    "data": "specialDetailObj.PaidAmount", "defaultContent": "<i>-</i>", "width": "10%",
                    'render': function (data, type, row) {
                        return roundoff(data)
                    }
                },
                
                 
                 {
                     "data": "BalanceDue", "defaultContent": "<i>-</i>", "width": "10%",
                     'render': function (data, type, row) {
                         return roundoff(row.BalanceDue)
                     }
                 },
                  {
                      "data": "specialDetailObj.CurrentAmount", 'render': function (data, type, row) {
                          index = index + 1
                          
                          return '<input class="form-control text-right paymentAmount" name="Markup" value="' + data + '" onfocus="paymentAmountFocus(this);" onchange="PaymentAmountChanged(this);" id="PaymentValue_' + index + '" type="text">';
                      }, "width": "15%"
                  },

            ],
            columnDefs: [{ orderable: false, className: 'select-checkbox', targets: 1 }
                , { className: "text-right", "targets": [4, 5, 6,7] },
                  { className: "text-left", "targets": [2] }
                , { "targets": [0], "visible": false, "searchable": false }
                , { "targets": [2, 3, 4, 5, 6,7], "bSortable": false }],

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
            if ((allData[rt].specialDetailObj.CurrentAmount) > 0) {
                allData[rt].specialDetailObj.CurrentAmount = roundoff(0)
                DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);
                var sum = 0;
                AmountReceived = parseFloat($('#TotalRecdAmt').val());
                for (var i = 0; i < allData.length; i++) {
                    sum = sum + parseFloat(allData[i].specialDetailObj.CurrentAmount);
                }
                $('#lblPaymentApplied').text(roundoff(sum));
               
                Selectcheckbox();
            }
        });
    }
    catch (e) {
        notyAlert('error', e.message);
    }
});

//function PaymentAmountChanged(this_Obj) {
//    debugger;
//    AmountReceived = parseFloat($('#TotalRecdAmt').val())
//    sum = 0;

//    var allData = DataTables.OutStandingInvoices.rows().data();
//   // var OldDataReplace = new Object();
//  //  OldDataReplace = Object.assign(OldDataReplace, allData);

//    var table = DataTables.OutStandingInvoices;
//    var rowtable = table.row($(this_Obj).parents('tr')).data();

//    for (var i = 0; i < allData.length; i++)
//    {
//        if (allData[i].ID == rowtable.ID) {
//         var oldamount = parseFloat(allData[i].specialDetailObj.CurrentAmount)
//            if (parseFloat(allData[i].BalanceDue) < parseFloat(this_Obj.value))
//            {
//               allData[i].specialDetailObj.CurrentAmount = parseFloat(allData[i].BalanceDue)
//               sum = sum + parseFloat(allData[i].BalanceDue);
//            }
//            else
//            {
               
//                allData[i].specialDetailObj.CurrentAmount = this_Obj.value;
                
//                sum = sum + parseFloat(this_Obj.value);
//                if (sum > AmountReceived)
//                {
//                    allData[i].specialDetailObj.CurrentAmount = oldamount;
        
//                }
//            }
          
//        }
//        else {
//            sum = sum + parseFloat(allData[i].specialDetailObj.CurrentAmount);
//        }
        
//    }
//  //  if (sum > AmountReceived) {
       
//        // DataTables.OutStandingInvoices.clear().rows.add(OldDataReplace).draw(false);
//  //  }
//   // else
//    //{
//       DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);

//  //  }
  

//    $('#lblTotalRecdAmt').text(roundoff(sum));
//    Selectcheckbox();
//}

function PaymentAmountChanged(this_Obj) {
    debugger;
    AmountReceived = parseFloat($('#TotalRecdAmt').val())
    sum = 0;
    
    var allData = DataTables.OutStandingInvoices.rows().data();
    var table = DataTables.OutStandingInvoices;
    var rowtable = table.row($(this_Obj).parents('tr')).data();

    for (var i = 0; i < allData.length; i++) {
        if (allData[i].ID == rowtable.ID) {

            var oldamount = parseFloat(allData[i].specialDetailObj.CurrentAmount)
            var outstandingAmount = parseFloat($("#lbloutstandingAmount").text())
            if (outstandingAmount > 0) {
                var currenttotal = AmountReceived + parseFloat(this_Obj.value) - (outstandingAmount + oldamount)
            }
            else {
                var currenttotal = AmountReceived + parseFloat(this_Obj.value) - oldamount
            }
          

            if (parseFloat(allData[i].BalanceDue) < parseFloat(this_Obj.value))
            {
                if (currenttotal < AmountReceived)
                {
                    allData[i].specialDetailObj.CurrentAmount = parseFloat(allData[i].BalanceDue)
                    sum = sum + parseFloat(allData[i].BalanceDue);
                }
                else
                {
                    allData[i].specialDetailObj.CurrentAmount = oldamount
                    sum = sum + oldamount
                }
                   
               
            }
            else 
            {

                if (currenttotal > AmountReceived) {
                    allData[i].specialDetailObj.CurrentAmount = oldamount
                    sum = sum + oldamount;
                }
                else {
                    allData[i].specialDetailObj.CurrentAmount = this_Obj.value
                    sum = sum + parseFloat(this_Obj.value);
                }
              
                //if (parseFloat(this_Obj.value) != AmountReceived)
                //{
                //    if (AmountReceived > parseFloat(this_Obj.value))
                //    {
                     
                //        allData[i].specialDetailObj.CurrentAmount = parseFloat(this_Obj.value)
                //    }
                    
                //}
                //else
                //{
                //    allData[i].specialDetailObj.CurrentAmount = AmountReceived;
                //}
              
              }
               
            }
       
        else {
            sum = sum + parseFloat(allData[i].specialDetailObj.CurrentAmount);
        }
    //    var outstandingAmount = AmountReceived - sum;
    }
    DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);

    $('#lblTotalRecdAmt').text(roundoff(sum));
    $('#lbloutstandingAmount').text(roundoff(AmountReceived - sum));
    Selectcheckbox();
}
function CustomerChange() {

    if ($('#Customer').val() != "") {
       BindOutstandingAmount();
        $('#TotalRecdAmt').val('');
        debugger;
        AmountChanged();
    }
   BindOutstanding();
}


function AmountChanged() {
    debugger;
    if ($('#TotalRecdAmt').val() != "")
    {
        AmountReceived = parseFloat($('#TotalRecdAmt').val());
    }
    else
    {
        AmountReceived = 0.00;
    }


    if (actionmode == 0)
    { 
        if(AmountReceived!=0.00)
        {
            if (parseInt($('#TotalRecdAmt').val()) > parseInt(pbBalance)) {

                $('#paidAmt').text('0.00');
              
                notyAlert('error', "Maximum Receivable Amount Limited to "+pbBalance+"");
              //  $('#TotalRecdAmt').val(pbBalance);
                //BindOutstanding();
            }
            else {
                $('#paidAmt').text(roundoff(AmountReceived));
                BindOutstanding();
            }
        }
    }
    else
    {
        var table = $('#tblOutStandingDetails').DataTable();
        var outstanding = table.rows().data();
        var sum = 0;
        
        for (var i = 0; i < outstanding.length; i++) {
            sum = sum + parseFloat(outstanding[i].BalanceDue);
        }
        var balanceTotal = sum;
        if (AmountReceived != 0.00)
        {
            if (parseInt($('#TotalRecdAmt').val()) > (balanceTotal)) {

                $('#paidAmt').text('0.00');
                //    DataTables.OutStandingInvoices.clear().rows.add(outstanding).draw(false);
                notyAlert('error', "Maximum Receivable Amount Limited to " + balanceTotal + "");


                //  $('#TotalRecdAmt').val(balanceTotal);
                $('#lblTotalRecdAmt').val(balanceTotal);
                // BindOutstanding();
            }
            else {
                $('#paidAmt').text(roundoff(AmountReceived));
                BindOutstandingByID();
            }
        }

    }
    var sum = 0;
    DataTables.OutStandingInvoices.rows().deselect();
    if ($('#TotalRecdAmt').val() < 0 || $('#TotalRecdAmt').val() == "") {
        $('#TotalRecdAmt').val(0);
    }

    AmountReceived = parseFloat($('#TotalRecdAmt').val())

    if (!isNaN(AmountReceived)) {
        $('#TotalRecdAmt').val(roundoff(AmountReceived));
   //     $('#lblTotalRecdAmt').text(roundoff(AmountReceived));
        $('#paidAmt').text(roundoff(AmountReceived));

        var table = $('#tblOutStandingDetails').DataTable();
        var allData = table.rows().data();

        var RemainingAmount = AmountReceived;

        for (var i = 0; i < allData.length; i++) {
            var SpecialObj = new Object;
          
            CurrentAmount = SpecialObj;

            if (RemainingAmount != 0) {
                if (parseFloat(allData[i].BalanceDue) < RemainingAmount) {
                  // allData[i].CurrentAmount = parseFloat(allData[i].BalanceDue)
                  allData[i].specialDetailObj.CurrentAmount = parseFloat(allData[i].BalanceDue)
                    RemainingAmount = RemainingAmount - parseFloat(allData[i].BalanceDue);
                    sum = sum + parseFloat(allData[i].BalanceDue);
                }
                else {
                  // allData[i].CurrentAmount = RemainingAmount
                    allData[i].specialDetailObj.CurrentAmount = RemainingAmount
                    sum = sum + RemainingAmount;
                    RemainingAmount = 0
                }
            }
            else {
                allData[i].CurrentAmount = 0;
            }
        }
        DataTables.OutStandingInvoices.clear().rows.add(allData).draw(false);
        Selectcheckbox();
        $('#lblTotalRecdAmt').text(roundoff(sum));
        //$('#lblPaymentApplied').text(roundoff(sum));
        //$('#lblCredit').text(roundoff(AmountReceived - sum));
    }
}

function BindOutstanding() {
    debugger;
    index = 0;
    DataTables.OutStandingInvoices.clear().rows.add(GetSpecialInvPayments()).draw(false);
    actionmode = 0;
}
function BindOutstandingByID() {
    debugger;
    index = 0;
    var groupID = $('#hdfGroupID').val();
    DataTables.OutStandingInvoices.clear().rows.add(GetCustomerPBPayments($('#hdfGroupID').val())).draw(false);

}


function GetSpecialInvPayments() {
    try {
        debugger;
        var ID = $('#Customer').val() == "" ? emptyGUID : $('#Customer').val();
        var PaymentID = $('#ID').val() == "" ? emptyGUID : $('#ID').val();

        var data = { "ID": ID, "PaymentID": PaymentID };
        var ds = {};
        ds = GetDataFromServer("SpecialInvPayments/GetSpecialInvPayments/", data);
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
function paymentAmountFocus(event) {
    event.select();
}
function Selectcheckbox() {
    debugger;
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    for (var i = 0; i < allData.length; i++) {
        if (allData[i].specialDetailObj.CurrentAmount == "" || allData[i].specialDetailObj.CurrentAmount == roundoff(0)) {
            DataTables.OutStandingInvoices.rows(i).deselect();
        }
        else {
            DataTables.OutStandingInvoices.rows(i).select();
        }
    }
}
function PaymentModeChanged() {
    if ($('#PaymentMode').val() == "ONLINE") {
        $("#ChequeDate").val('');
        $("#ReferenceBank").val('');
        $('#ChequeDate').prop('disabled', true);
        $('#ReferenceBank').prop('disabled', true);
    }
    else if ($('#PaymentMode').val() == "CHEQUE") {
        $('#ChequeDate').prop('disabled', false);
        $('#ReferenceBank').prop('disabled', false);
    }
    else if ($('#PaymentMode').val() == "CASH") {
        $("#ChequeDate").val('');
        $("#ReferenceBank").val('');
        $('#ChequeDate').prop('disabled', true);
        $('#ReferenceBank').prop('disabled', true);
    }


}
function BindOutstandingAmount() {
    debugger;
    var thisitem = GetOutstandingSpecialAmountByCustomer($('#Customer').val())
    if (thisitem != null) {
      // $('#invoicedAmt').text(thisitem.InvoiceOutstanding == null ? "₹ 0.00" : thisitem.InvoiceOutstanding);
        $('#invoicedAmt').text(thisitem.BalanceOutstanding == null ? "₹ 0.00" : thisitem.BalanceOutstanding);
        pbBalance = thisitem.BalanceOutstanding;
    }
}

function GetOutstandingSpecialAmountByCustomer(ID) {
    try {
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("SpecialInvPayments/GetOutstandingSpecialAmountByCustomer/", data);
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


function SavePayments() {
    debugger;
   
    if ($('#SpecialPaymentForm').valid()) 
        Validate();
    sum = 0;
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();  
        AmountReceived = parseFloat($('#TotalRecdAmt').val());
        for (var i = 0; i < allData.length; i++) {
            sum = sum + parseFloat(allData[i].specialDetailObj.CurrentAmount);
        }
    if(sum!=AmountReceived)
    {
        if ($('#TotalRecdAmt').val() != 0)
        {
            notyAlert('error', "Current payment not matches to  Received  amount");
        }
    }
        else
    {
        SaveValidateData();
        $('#btnSave').trigger('click');

    }
}
function Validate()
{
   debugger;
    var SpecialInvPaymentsViewModel = new Object();
    SpecialInvPaymentsViewModel.PaymentRef = $("#PaymentRef").val();
    SpecialInvPaymentsViewModel.InvoiceID = $("#ID").val();
    var data = "{'SpecialInvObj':" + JSON.stringify(SpecialInvPaymentsViewModel) + "}";
    PostDataToServer("SpecialInvPayments/Validate/", data, function (jsonResult) {
        debugger;
        if (jsonResult != '') {
            switch (jsonResult.Result) {
                case "OK":
                    if (jsonResult.Records.status == 1) {
                        if (actionmode == 0) {

                            notyConfirm(jsonResult.Records.message, 'SaveValidateData();', '', "Yes,Proceed!", 1)
                        }
                        else
                        {
                            SaveValidateData();
                        }
                    }
                    else {
                        SaveValidateData();
                    }
                    break;
                case "ERROR":
                    notyAlert('error', jsonResult.message);
                    break;
                default:
                    break;
            }
        }
    });

}
function SaveValidateData() {
    debugger;
    $(".Cancel").click();
    var SelectedRows = DataTables.OutStandingInvoices.rows(".selected").data();
    if ((SelectedRows) && (SelectedRows.length > 0)) {
              var arr = [];
        for (r = 0; r < SelectedRows.length; r++)
        {
            var SpecialInvDetailViewModel = new Object();
            SpecialInvDetailViewModel.InvoiceID = SelectedRows[r].ID;
            SpecialInvDetailViewModel.CurrentAmount = SelectedRows[r].specialDetailObj.CurrentAmount;
            SpecialInvDetailViewModel.PaidAmount = SelectedRows[r].specialDetailObj.PaidAmount;
          //  SpecialInvDetailViewModel.GroupID = SelectedRows[r].GroupID;
          // SpecialInvDetailViewModel.InvoiceID = SelectedRows[r].specialDetailObj.InvoiceID;
            arr.push(SpecialInvDetailViewModel);
        }

        $('#hdfpaymentDetail').val('');
        $('#hdfpaymentDetail').val(JSON.stringify(arr));
      
    }
}

function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            debugger;
            notyAlert('success', JsonResult.Message);
            GetCustomerPBPaymentsByID(JsonResult.Records.GroupID);
            BindCustomerPaymentsHeader();// List();
           
            actionmode = 1; // For checking amount with balnce in register page and edit page.

            
           // closeNav();
          //  DataTables.CustomerSpclPaymentTable.clear().rows.add(GetAllCustomerSpecialPayments()).draw(false);
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}
function GetCustomerPBPaymentsByID(GroupID)
{
   
    ChangeButtonPatchView('SpecialInvPayments', 'btnPatchAdd', 'Edit');
    debugger;
    var thisitem = GetCustomerPBPayments(GroupID)
 //   $('#lblheader').text('Invoice No: ' + thisitem.InvoiceNo);
    $('#ID').val(GroupID);
    $('#deleteId').val(GroupID);
    $("#Customer").val(thisitem[0].CustID).select2();
    $('#hdfGroupID').val(GroupID);
//   $("#Customer").val(thisitem[0].ID).trigger('change');
 // $("#Customer").val(thisitem[0].customerObj.CompanyName);
    $('#hdfCustomerID').val(thisitem[0].CustID);
 // $('#hdfInvoiceID').val(thisitem.specialDetailObj.InvoiceID);
    $('#lblheader').text('Edit PB Payment');
    $('#Customer').prop('disabled', true);
    $('#ReferenceBank').val(thisitem[0].RefBank);
    $('#PaymentDate').val(thisitem[0].paymentDateFormatted);
    $('#ChequeDate').val(thisitem[0].chequeDateFormatted);
    $('#PaymentRef').val(thisitem[0].PaymentRef);
    $('#PaymentMode').val(thisitem[0].PaymentMode);
    $('#txtRemarks').val(thisitem[0].Remarks);
  // $('#TotalRecdAmt').val(roundoff(thisitem[0].specialDetailObj.CurrentAmount));
  //  $('#paidAmt').text(roundoff(thisitem[0].specialDetailObj.CurrentAmount));
    $('#Type').prop('disabled', true);
    $('#invoicedAmt').text('');
    $('#pbpay').hide();
    $('#hdfBalanceDue').val(thisitem[0].BalanceDue);
  
  //  BindOutstandingAmount();
    BindOutstandingByID();
    var actionmode = 1;

  
    //edit outstanding table Payment text binding
    var table = $('#tblOutStandingDetails').DataTable();
    var allData = table.rows().data();
    var sum = 0;
    AmountReceived = roundoff($('#TotalRecdAmt').val())
    for (var i = 0; i < allData.length; i++) {
        sum = sum + parseFloat(allData[i].specialDetailObj.CurrentAmount);
    }
    $('#lblTotalRecdAmt').text(roundoff(sum));
    $('#TotalRecdAmt').val(roundoff(sum));
    $('#paidAmt').text(roundoff(sum));
    //$('#paidAmt').text(roundoff(sum));
    Selectcheckbox();
}

function GetCustomerPBPayments(GroupID) {
    
    try {
        debugger;
        var data = { "GroupID": GroupID};
        
        var ds = {};
        ds = GetDataFromServer("SpecialInvPayments/GetSpecialPaymentsDetails/", data);
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
function GetAllCustomerSpecialPayments(SpecialPaymentsSearchObject)
{
    try {
        if (SpecialPaymentsSearchObject === 0) {
            var data = {};
        }
        else {
            var data = { "SpecialPaymentsSearchObject": JSON.stringify(SpecialPaymentsSearchObject) };
        }
        
        var ds = {};
        ds = GetDataFromServer("SpecialInvPayments/GetAllSpecialInvPayments/", data);
        debugger;
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
function openNavClick() {
    $('#pbpay').show();
    fieldsclear();
    $('#lblOutstandingdetails').text('');
    BindOutstanding();
    $('#lblheader').text('New PB Payment');
    ChangeButtonPatchView('SpecialInvPayments', 'btnPatchAdd', 'Add');
    $('#Customer').prop('disabled', false);
    $('#ChequeDate').prop('disabled', true);
    $('#ReferenceBank').prop('disabled', true);   
    $('#PaymentMode').prop('disabled', false);
    $('#TotalRecdAmt').prop('disabled', false);
    openNav();
    //clearUploadControl();
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
    $('#Type').val('P');
    $('#hdfType').val('');
    $('#hdfGroupID').val('');
    $('#hdfCustomerID').val('');
    $('#ReferenceBank').val('');
    $('#hdfpaymentDetail').val('');
    
}
function Resetform() {
    debugger;
    var validator = $("#SpecialPaymentForm").validate();
    $('#SpecialPaymentForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    $('#SpecialPaymentForm')[0].reset();
}
function Reset() {
    $("#fromdate").val('');
    $("#todate").val('');
    $("#Customerddl").select2();
    $("#Customerddl").val('').trigger('change');
    $("#paymode").val('');
    $("#search").val('');
    RefreshPayments();
}
function RefreshPayments() {
    try {
        debugger
        var fromdate = $("#fromdate");
        var todate = $("#todate");
        var customercode = $("#Customerddl");
        var paymentMode = $("#paymode");
        var companycode = $("#Companyddl");
        var search = $("#search");

        var SpecialPaymentsSearch = new Object();
        SpecialPaymentsSearch.FromDate = fromdate[0].value !== "" ? fromdate[0].value : null;
        SpecialPaymentsSearch.ToDate = todate[0].value !== "" ? todate[0].value : null;
        SpecialPaymentsSearch.Customer = customercode[0].value !== "" ? customercode[0].value : null;
        SpecialPaymentsSearch.PaymentMode = paymentMode[0].value !== "" ? paymentMode[0].value : null;
        SpecialPaymentsSearch.Company = companycode[0].value !== "" ? companycode[0].value : null;
        SpecialPaymentsSearch.Search = search[0].value !== "" ? search[0].value : null;

        DataTables.CustomerSpclPaymentTable.clear().rows.add(GetAllCustomerSpecialPayments(SpecialPaymentsSearch)).draw(true);

    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function Edit(currentObj) {
    debugger;
    openNav();
    Resetform();
    actionmode = 1;
    var rowData = DataTables.CustomerSpclPaymentTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.GroupID != null)) {
        GetCustomerPBPaymentsByID(rowData.GroupID)
       
    }
    
}
function DeletePayments() {
    notyConfirm('Are you sure to delete?', 'Delete()', '', "Yes, delete it!");
}


function Delete() {
    debugger;
    $('#btnFormDelete').trigger('click');
}
function DeleteSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            closeNav();
            BindCustomerPaymentsHeader()
            notyAlert('success', JsonResult.Message);
            $('#lblOutstandingdetails').text('');
           // List();
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
function BindCustomerPaymentsHeader() {
    DataTables.CustomerSpclPaymentTable.clear().rows.add(GetAllCustomerSpecialPayments()).draw(false);
}