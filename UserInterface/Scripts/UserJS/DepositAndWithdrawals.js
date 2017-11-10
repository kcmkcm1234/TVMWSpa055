var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.DepositAndWithdrawalsTable = $('#DepositAndWithdrawalsTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllDepositAndWithdrawals(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [               
               { "data": "ID" },
               {
                   "data": "TransactionType", render: function (data, type, row) {
             if (row.TransferID!=emptyGUID) {
                 var a = (data == "D" ? "Deposit " : "Withdrawal")+ " ( <label><i><b> Transfer </b></i></label> )";
                 return a;
             }
             else {
                 var b = (data == "D" ? "Deposit " : "Withdrawal");
                 return b;
             } 
         }, "defaultContent": "<i>-</i>"
         },
               { "data": "ReferenceNo", "defaultContent": "<i>-</i>" },
               { "data": "DateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "BankName", "defaultContent": "<i>-</i>" },
                { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
                 { "data": "ChequeStatus", "defaultContent": "<i>-</i>" },
                  { "data": "ChequeFormatted", "defaultContent": "<i>-</i>" },
                   { "data": "TransferID", "defaultContent": "<i>-</i>" },
                      { "data": "GeneralNotes", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               {
                   "data": null, "orderable": false, render: function (data, type, row) {
                       if (row.TransferID != emptyGUID)
                       {
                           return '<a href="#" title="Edit DepositWithdrawal" class="actionLink"  onclick="EditCashTransfer(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>';
                       }   
                       else{

                           return '<a href="#" title="Edit DepositWithdrawal" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>';
                       }
                   }
               },
               {
                   "data": null, "orderable": false, render: function (data, type, row) {
                       if (row.PaymentMode == 'ONLINE' || row.TransferID != emptyGUID) {
                           return '-'
                       }
                       else
                       {
                           return '<a data-toggle="tp" data-placement="top" data-delay={"show":2000, "hide":3000} title="Delete" href="#" class="DeleteLink" onclick="Delete(this)"><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>';
                       }

                   }
               }

             ],
               
             columnDefs: [{ "targets": [0,8], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [10] },
                    { className: "text-left", "targets": [1,2,4,5,6,7,9] },
             { className: "text-center", "targets": [3] },
                          {
                              "render": function (data, type, row) {
                                  if (data == "Cleared")
                                  {
                                      return "Cleared";
                                  }
                                  else if (data == "NotCleared")
                                  {
                                      return "Not Cleared ";
                                  }
                                  else if (data == "Bounced")
                                  {
                                      return "Bounced";
                                  }
                                  else
                                  {
                                      return "NA";
                                  }
                              },
                              "targets": 6

                          },
                         //{
                         //    "render": function (data, type, row) {
                               
                         //     return (data == "D" ? "Deposit " : "Withdrawal");
                         //   },
                         //   "targets": 1

                         //}
                    { "width": "15%", "targets": 1 },
             { "width": "10%", "targets": 3 },
                 { "width": "12%", "targets": 4 }

             ]
         });
        

        $('#DepositAndWithdrawalsTable tbody').on('dblclick', 'td', function () {
            debugger;
            var rowData = DataTables.DepositAndWithdrawalsTable.row($(this).parents('tr')).data();
            if(rowData.TransferID==emptyGUID)
            {
                Edit(this);
            }
            else
            {
                EditCashTransfer(this)
            }
        });
        DataTables.tblDepositwithdrawalList = $('#tblDepositwithdrawalList').DataTable(
       {
           dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
           order: [],
           searching: true,
           paging: true,
           data: GetAllDepositAndWithdrawals(),
           pageLength: 7,
           language: {
               search: "_INPUT_",
               searchPlaceholder: "Search"
           },
           columns: [
             { "data": "ID" },
             { "data": "Checkbox", "defaultContent": "" },
             { "data": "DateFormatted", "defaultContent": "<i>-</i>" },
             { "data": "ChequeDate", "defaultContent": "<i>-</i>" },
             { "data": "CustomerName", "defaultContent": "<i>-</i>" },
             { "data": "ReferenceNo", "defaultContent": "<i>-</i>" },
             { "data": "BankName", "defaultContent": "<i>-</i>" },
             { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" title="Edit Deposit" class="actionLink"  onclick="EditDeposit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
                { "data": "CustomerID", "defaultContent": "<i>-</i>" }
           ],
           columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { orderable: false, className: 'select-checkbox', targets: 1 },
               { orderable: false, "visible": false, targets: [8,9] },
                { className: "text-right", "targets": [6] },
                  { className: "text-left", "targets": [] },
           { className: "text-center", "targets": [1, 2, 3, 4,6,7] }
          

           ],
           select: { style: 'multi', selector: 'td:first-child' }
       });

        $("#PaymentMode option[value=ONLINE]").prop('disabled', true); 
        $("#ChequeStatus").prop('disabled', true);
    } catch (x) {

        notyAlert('error', x.message);

    }

});


function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    ShowModal();
    ResetForm();
    var rowData = DataTables.DepositAndWithdrawalsTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        FillDepositWithdrawalDetails(rowData.ID);
    }
}
function EditDeposit(currentObj)
{
    ShowDepositEdit();
    ResetForm();
    debugger;
    var rowData = DataTables.tblDepositwithdrawalList.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        FillDepositWithdrawalDetails(rowData.ID);
    }
}

function DateChange(curObj)
{
    debugger;
    if (curObj.value != "")
    {
        BindDepositAndWithdrawals();
    }
    
}

function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            notyAlert('success', JsonResult.Message);
            debugger;
            if ($("#ID").val() != "" && $("#ID").val() != "0") {
                FillDepositWithdrawalDetails($("#ID").val());
            }
            else {
                FillDepositWithdrawalDetails(JsonResult.Records.ID);
            }
            BindDepositAndWithdrawals();
            BindDepositWithdrawals('D', "");
            $("#AddDepositAndWithdrawalModel").modal('hide');
            try {
                GetUndepositedChequeBubbleCount();
            } catch (x) { }
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}

function DepositModeOnchange()
{
    debugger;
    if ($("#PaymentMode").val() == "CHEQUE" && $("#ChequeStatus").val() != "Cleared") {
        $("#ChequeStatus").prop('disabled', false); 
    }
    else {
        $("#ChequeStatus").prop('disabled', true);
        $("#ChequeClearDate").prop('disabled', true);
    }
}

function ChequeStatusModeOnchange()
{
    if ($("#PaymentMode").val() == "CHEQUE" && $("#ChequeStatus").val() != "Cleared")
    {
        $("#ChequeClearDate").prop('disabled', true);
    }
    else
    {
        $("#ChequeClearDate").prop('disabled', false);
    }
}

function FillDepositWithdrawalDetails(ID) {
    var thisItem = GetDepositWithdrawalDetailsByID(ID); //Binding Data
    //Hidden
        debugger; 
        $("#ID").val(thisItem.ID);
        $("#TransactionType").val(thisItem.TransactionType);
        $("#ReferenceNo").val(thisItem.ReferenceNo);
        $("#customerCode").val(thisItem.CustomerID);
        $("#Date").val(thisItem.DateFormatted);
        $("#ChequeClearDate").val(thisItem.ChequeFormatted);
        $("#Amount").val(roundoff(thisItem.Amount));
        $("#BankCodeModal").val(thisItem.BankCode);
        $("#GeneralNotes").val(thisItem.GeneralNotes);
        $("#ChequeStatus").val(thisItem.ChequeStatus); 
        $("#PaymentMode").val(thisItem.PaymentMode);
        $("#PaymentMode").prop('disabled', true);
    //Hiding customerid when transaction type is withdrawal
        if (thisItem.TransactionType === "W") {
            $("#customerid").hide();
            $("#ChequeStatus option[value='Bounced']").attr("disabled", "disabled");
        }
        else
        {
            $("#customerid").show();
            $("#ChequeStatus option[value='Bounced']").removeAttr("disabled");
        }
        if (thisItem.TransactionType == "D") {
            $("#lblPaymentMode").text("Deposit Mode");
        }
        else {
            $("#lblPaymentMode").text("Withdrawal Mode");
        }
        DepositModeOnchange();
        if (thisItem.ChequeStatus == 'Bounced') {
            $('#ChequeStatus').prop('disabled', true)
            $("#btnSave").attr("disabled", "disabled");
        }
        else {
            $("#btnSave").removeAttr("disabled");
            if (thisItem.ChequeStatus == 'Cleared')
            $("#ChequeClearDate").prop('disabled',false);

        }

        if (thisItem.PaymentMode == 'ONLINE') {
            $("#btnSave").attr("disabled", "disabled");
            $("#hdnPaymentMode").val(thisItem.PaymentMode); 
        }

        $("#hdnChequeStatus").val(thisItem.ChequeStatus);
        $("#hdnChequeDate").val(thisItem.ChequeFormatted);
}

function GetDepositWithdrawalDetailsByID(ID) {
    try {

        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("DepositAndWithdrawals/GetDepositAndWithdrawalDetails/", data);
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
function Reset()
{
    debugger;
    if ($("#ID").val() == "") {
        ClearFields();       
    }
    else {
        FillDepositWithdrawalDetails($("#ID").val());
    }
    ResetForm();
}

function ClearFields() {
    debugger;
    $("#ID").val("");
    $("#TransactionType").val("");
    $("#ReferenceNo").val("");
    $("#Date").val("");
    $("#customerCode").val("")
    $("#ChequeClearDate").val("");
    $("#Amount").val("");
    $("#BankCode").val("");
    $("#GeneralNotes").val("");
    $("#ChequeStatus").val("");
    $("#ChequeClearDate").val("");
    $("#BankCodeModal").val("");
    $("#PaymentMode").val(""); 
 //   $("#lblPaymentMode").text("Deposit Mode");
    ResetForm();
    $("#ChequeStatus").prop('disabled', true);
    $("#ChequeClearDate").prop('disabled', true);
    $("#hdnChequeStatus").val('');
    $("#hdnChequeDate").val('');
    $("#hdnPaymentMode").val('');
    
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#DepositAndWithdrawal").validate();
    $('#DepositAndWithdrawal').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function Add()
{
    $("#DepositwithdrawalList").hide();
    $("#DepositwithdrawalEntry").show();
    $(".modal-footer").show();
   // $("#tabDepositwithdrawalEntry").text("Deposit Entry");
    ClearFields();
} 
function List()
{
    $("#DepositwithdrawalList").show();
    $("#DepositwithdrawalEntry").hide();
    $(".modal-footer").hide();
    $("#tabDepositwithdrawalList").text("Undeposited Cheques");
}

function BindDepositAndWithdrawals() {
    try {
        debugger;
        DataTables.DepositAndWithdrawalsTable.clear().rows.add(GetAllDepositAndWithdrawals()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function FillDates()
{
    debugger;
    var m_names = new Array("Jan", "Feb", "Mar",
"Apr", "May", "Jun", "Jul", "Aug", "Sep",
"Oct", "Nov", "Dec");
    var today = new Date()
    var priorDate = new Date().setDate(today.getDate() - 30);
    var date = new Date(priorDate);
    var fromDate = (date.getDate()) + '-' + m_names[(date.getMonth())] + '-' + date.getFullYear();
    $("#txtDateFrom").val(fromDate);
    var toDate = (today.getDate()) + '-' + m_names[(today.getMonth())] + '-' + today.getFullYear();
    $("#txtDateTo").val(toDate);
}



function GetAllDepositAndWithdrawals(DepositOrWithdrawal, chqclr) {
    try {
        debugger;
        if ($("#txtDateFrom").val() == "" && $("#txtDateTo").val() == "") {
            FillDates();
        }
        var FromDate = $("#txtDateFrom").val();
        var ToDate = $("#txtDateTo").val();
       
        var data = { "FromDate": FromDate, "ToDate": ToDate, "DepositOrWithdrawal": DepositOrWithdrawal, "chqclr": chqclr };
        var ds = {};
        ds = GetDataFromServer("DepositAndWithdrawals/GetAllDepositAndWithdrawals/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
            if (DepositOrWithdrawal == undefined && chqclr == undefined)
            {
                $("#TotalDpt").text("");
                $("#TotalDpt").text(ds.totalDpt);
                $("#TotalWdl").text("");
                $("#TotalWdl").text(ds.totalWdl);
            }
           
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

function BindDepositWithdrawals(DepositOrWithdrawal,chqclr)
{
    try {
        debugger;
        DataTables.tblDepositwithdrawalList.clear().rows.add(GetAllDepositAndWithdrawals(DepositOrWithdrawal,chqclr)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function ShowDepositModal() {
    debugger;
    $("#PaymentMode").prop('disabled', false);
    $("#tabs").css('display', '');
    $("#AddDepositAndWithdrawalModel").modal('show');
    $("#UndepositedChequeDate").hide();
    $("#tabDepositwithdrawalEntry").css('display', '');
    $("#DepositwithdrawalEntry").css('display', '');
    $("#AddOrEditSpan").text("Deposit");
    BindDepositWithdrawals('D', "");
    $('a[href="#DepositwithdrawalList"]').click();
    $("#btnCheque").css('display', 'none');
    $('#tblDepositwithdrawalList tbody td:nth-child(8) ').show();
    $("#editRow").show();
    $('#tblDepositwithdrawalList tbody td:nth-child(1) ').show();
    $("#editCheckBox").show();
    $("#tabDepositwithdrawalList").css('display', '');
    $("#DepositwithdrawalList").css('display', '');
    $("#DepositwithdrawalList").show();
    $("#btnDepositSave").show();
    $(".chqHeader").show();
    $("#BankCode").show();
    $(".modal-footer").hide();
    $("#tabDepositwithdrawalList").text("Undeposited Cheques");
    $("#tabDepositwithdrawalEntry").text("Deposit Entry");
    $("#lblBankCode").text("Deposit To");
    $("#lblBankDiv").css('display', '');
    $("#BankCode").val("");
    $("#lblPaymentMode").text("Deposit Mode");
    $("#customerid").show();
    $("#ChequeStatus option[value='Bounced']").removeAttr("disabled");
    $("#btnSave").removeAttr("disabled");
}

function ShowDepositEdit()
{
    $("#AddDepositAndWithdrawalModel").modal('show');
    $("#tabDepositwithdrawalEntry").css('display', '');
    $("#DepositwithdrawalEntry").css('display', '');
    $('a[href="#DepositwithdrawalEntry"]').click();
    $(".modal-footer").show();
}

function ShowModal()
{   
    
        $("#AddDepositAndWithdrawalModel").modal('show');
        $("#tabDepositwithdrawalEntry").css('display', '');
        $("#DepositwithdrawalEntry").css('display', '');
        $("#AddOrEditSpan").text("Edit");
        $('a[href="#DepositwithdrawalEntry"]').click();
        $("#tabs").css('display', 'none');
        $("#btnCheque").css('display', 'none');
        $(".modal-footer").show();
}

function ClearCheque()
{
    debugger;
    
    if (DataTables.tblDepositwithdrawalList.rows(".selected").data().length==0)
    {
        notyAlert('error',"Please select atleast one");
    }
    if ($("#ChequeDate").val() == "") {
        notyAlert('error', "Please Select Date");
    }
    else
    {
        var SelectedRows = DataTables.tblDepositwithdrawalList.rows(".selected").data();
        if ((SelectedRows) && (SelectedRows.length > 0)) {
            var CheckedIDs = [];
            var CheckedDate = $("#ChequeDate").val();
                for (var r = 0; r < SelectedRows.length; r++) {
                    CheckedIDs.push(SelectedRows[r].ID);
            }

        }
        debugger;
     
        var data = { "ID": CheckedIDs.join(','), "Date": CheckedDate };
        ds = GetDataFromServer("DepositAndWithdrawals/ClearCheque/", data);
        if (ds !== '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result === "OK") {
            ShowChequeClear();
            $("#AddDepositAndWithdrawalModel").modal('hide');
            notyAlert('success', "Success");
            CheckedIDs.length = 0;
            BindDepositAndWithdrawals();
        }
        if (ds.Result === "ERROR") {
            debugger;
            notyAlert('error', ds.Message);
        }
        //GetDataToServer('DepositAndWithdrawals/ClearCheque/', data, function (JsonResult) {
        //    debugger;
        //    if (JsonResult != '') {
        //        switch (JsonResult.Result) {
        //            case "OK":
        //                ShowChequeClear();
        //                $("#AddDepositAndWithdrawalModel").modal('hide');
        //                notyAlert('success', "Success");
        //                CheckedIDs.length = 0;
        //                BindDepositAndWithdrawals();
        //                break;
        //            case "ERROR":
        //                notyAlert('error', JsonResult.Message.Message);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //})
    }

}
function SaveDeposit() {
    if ($("#ChequeStatus").val() == 'Bounced') {
        notyConfirm('Cheques Status-Bounced', 'SaveDepositConfirm();', '', "Yes,Confirm");
    }
    else
    {
        SaveDepositConfirm();
    }
}

function SaveDepositConfirm()
{ 
    debugger;
    $("#DepositRowValues").val('');
    try { 
        if ($("#TransactionType").val() == "")
        {
            if ($("#AddOrEditSpan").text() == "Deposit") {
                $("#TransactionType").val('D');
            }
            else
            {
                $("#TransactionType").val('W');
            } 
        }
        if ($("#hdnChequeStatus").val() != 'Bounced')//not bounced save disable
            if ($("#hdnPaymentMode").val() != 'ONLINE')//on online update save disable
                $("#btnDepositWithdrawalSave").trigger('click');//save click
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function Validation()
{

}

function ShowWithDrawal()
{
    $("#PaymentMode").prop('disabled', false);
    $("#ChequeClearDate").prop('disabled', false); 
    $("#tabs").css('display', '');
    $("#AddDepositAndWithdrawalModel").modal('show');
    $("#tabDepositwithdrawalEntry").css('display', '');
    $("#DepositwithdrawalEntry").css('display', '');
    $("#DepositwithdrawalEntry").show();
    $("#tabDepositwithdrawalList").css('display', 'none');
    $("#DepositwithdrawalList").css('display', 'none');
    $("#DepositwithdrawalList").hide();
    $("#AddOrEditSpan").text("Withdrawal");
    BindDepositWithdrawals('W', "");
    $('a[href="#DepositwithdrawalEntry"]').click();
    $("#btnCheque").css('display', 'none');
    $('#tblDepositwithdrawalList tbody td:nth-child(8) ').hide();
    $("#editRow").hide();
    $('#tblDepositwithdrawalList tbody td:nth-child(1) ').hide();
    $("#editCheckBox").hide();
    $("#btnDepositSave").hide();
    $(".modal-footer").show();
    $("#tabDepositwithdrawalEntry").text("Withdrawal Entry");
    $("#lblBankCode").text("Withdrawal From");
    $("#lblBankDiv").css('display', '');
    $("#lblPaymentMode").text("Withdrawal Mode");
    $("#customerid").hide();
    $("#ChequeStatus option[value='Bounced']").attr("disabled", "disabled");
    $("#btnSave").removeAttr("disabled");
}
function ShowChequeClear()
{
    $("#tabs").css('display', '');
    $("#AddDepositAndWithdrawalModel").modal('show');
    $("#tabDepositwithdrawalEntry").css('display', 'none');
    $("#DepositwithdrawalEntry").css('display', 'none');
    $("#DepositwithdrawalEntry").hide();
    $("#AddOrEditSpan").text("Clear Cheques");
    BindDepositWithdrawals('', "True");
    $('a[href="#DepositwithdrawalList"]').click();
    $("#btnCheque").css('display', '');
    $('#tblDepositwithdrawalList tbody td:nth-child(8) ').hide();
    $("#editRow").hide();
    $('#tblDepositwithdrawalList tbody td:nth-child(1) ').show();
    $("#editCheckBox").show();
    $("#btnDepositSave").hide();
    $(".chqHeader").hide();
    $("#BankCode").hide();
    $(".modal-footer").hide();
    $("#tabDepositwithdrawalList").text("Deposited Cheques");
    $("#lblBankDiv").css('display', 'none');
    $("#UndepositedChequeDate").show();
    
}
function SaveCheckedDeposit()
{
    debugger;
    try
    {
        if ($("#BankCode").val() == "") {
            notyAlert('error', "Please Select Bank");
        }
        else if (DataTables.tblDepositwithdrawalList.rows(".selected").data().length==0)
        {
            notyAlert('error', "Please Select atleast one");
        }
        else
        {
            var SelectedRows = DataTables.tblDepositwithdrawalList.rows(".selected").data();
            if ((SelectedRows) && (SelectedRows.length > 0)) {
                var ar = [];
                for (var r = 0; r < SelectedRows.length; r++) {
                    var DepositAndWithdrwalViewModel = new Object();
                    DepositAndWithdrwalViewModel.Date = SelectedRows[r].DateFormatted;
                    DepositAndWithdrwalViewModel.TransactionType = 'D';
                    DepositAndWithdrwalViewModel.ReferenceNo = SelectedRows[r].ReferenceNo;
                    DepositAndWithdrwalViewModel.Amount = SelectedRows[r].Amount;
                    DepositAndWithdrwalViewModel.BankCode = $("#BankCode").val();
                    DepositAndWithdrwalViewModel.CustomerID = SelectedRows[r].CustomerID;
                    //DepositAndWithdrwalViewModel.DepositMode = $("#PaymentMode").val();
                    DepositAndWithdrwalViewModel.PaymentMode = "CHEQUE";
                    //if ($("#ChequeStatus").val() == "")
                    //{
                        DepositAndWithdrwalViewModel.ChequeStatus = "NotCleared";
                    //}
                    //DepositAndWithdrwalViewModel.ChequeStatus = $("#ChequeStatus").val();
                    ar.push(DepositAndWithdrwalViewModel);
                }
                $("#DepositRowValues").val(JSON.stringify(ar));
                $("#btnDepositWithdrawalSave").trigger('click');
            }
        }
    
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
    
}


//------------------------Transfer-----------------------//
//Show pop up to add transfer details
function ShowCashTransfer() {
    transferclear();
    $("#AddCashWithdrawalModel").modal('show');
    $("#TransferAmt").show();

}

//Trigger the transfer bank
function TransferCash() {
    debugger;
    try {
        $("#btnBankTransfer").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}


//Clears all textboxes and dropdownlists
function transferclear() {
    $("#TransferID").val("");
    $("#FromBankCode").val("");
    $("#ToBankCode").val("");
    $("#TransferAmount").val("");
    $("#referenceno").val("");
    $("#TransferDate").val("");
}


//Output Save Success or Failure Message
function SaveSuccessTransfer(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindDepositAndWithdrawals();
            $('#AddCashWithdrawalModel').modal('hide');
            notyAlert('success', JsonResult.Message);
            //closepopup


            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}


//Open the pop up in edit mode
function EditCashTransfer(currentObj) {
    //Tab Change on edit click
    debugger;
    ShowCashTransfer();
    ResetForm();
    var rowData = DataTables.DepositAndWithdrawalsTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.TransferID != null)) {
        FillTransferCash(rowData.TransferID);
    }
}

//Fill the fields with values for editing
function FillTransferCash(TransferID) {
    debugger;
    var thisItem = GetCashTransferByID(TransferID);
    
    $("#TransferID").val(thisItem.TransferID);//hidden field
    $("#FromBankCode").val(thisItem.FromBank);
    $("#ToBankCode").val(thisItem.ToBank);
    $("#TransferAmount").val(thisItem.Amount);
    $("#referenceno").val(thisItem.ReferenceNo);
    $("#depositemode").prop('disabled', true);
    //$("#depositemode").val(thisItem.DepositMode);
    $("#TransferDate").val(thisItem.DateFormatted);
}


//Get the Values to the fields based on the Id
function GetCashTransferByID(TransferID) {
    try {
        debugger;

        var data = { "TransferID": TransferID };
        var ds = {};
        ds = GetDataFromServer("DepositAndWithdrawals/GetTransferCashById/", data);
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

//Delete Deposit and withdrawal
function Delete(currObj) {
    debugger;
    var rowData = DataTables.DepositAndWithdrawalsTable.row($(currObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteDepositandwithdrawal("' + rowData.ID + '")', '', "Yes, delete it!");
    }

}

function DeleteDepositandwithdrawal(ID) {
    try {


        if (ID) {
            var data = { "ID": ID };
            var ds = {};
            ds = GetDataFromServer("DepositAndWithdrawals/DeleteDepositandwithdrawal/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message.Message); 
                BindDepositAndWithdrawals();
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



