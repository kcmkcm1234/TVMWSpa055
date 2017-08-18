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
               { "data": "TransactionType", "defaultContent": "<i>-</i>" },
               { "data": "ReferenceNo", "defaultContent": "<i>-</i>" },
               { "data": "DateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "BankName", "defaultContent": "<i>-</i>" },
                { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
                 { "data": "ChequeStatus", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" title="Edit DepositWithdrawal" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }              
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [7] },
                    { className: "text-left", "targets": [1,2,4,5,6] },
             { className: "text-center", "targets": [3,8] },
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
                                  else
                                  {
                                      return "NA";
                                  }
                              },
                              "targets": 6

                          },
                         {
                             "render": function (data, type, row) {
                               
                              return (data == "D" ? "Deposit " : "Withdrawal");
                            },
                            "targets": 1

                         }

             ]
         });
        

        $('#DepositAndWithdrawalsTable tbody').on('dblclick', 'td', function () {

            Edit(this);
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
             { "data": "ReferenceNo", "defaultContent": "<i>-</i>" },             
             { "data": "BankName", "defaultContent": "<i>-</i>" },
             { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" title="Edit Deposit" class="actionLink"  onclick="EditDeposit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
           ],
           columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, { orderable: false, className: 'select-checkbox', targets: 1 },
               { orderable: false, "visible": false, targets: 6 },
                { className: "text-right", "targets": [5] },
                  { className: "text-left", "targets": [] },
           { className: "text-center", "targets": [1, 2, 3, 4,6] }
          

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
        //$("#ChequeStatus").val("");
    }
}

function FillDepositWithdrawalDetails(ID) {
    var thisItem = GetDepositWithdrawalDetailsByID(ID); //Binding Data
    //Hidden
    debugger; 
        $("#ID").val(thisItem.ID);
        $("#TransactionType").val(thisItem.TransactionType);
        $("#ReferenceNo").val(thisItem.ReferenceNo);
        $("#Date").val(thisItem.DateFormatted);
        $("#Amount").val(roundoff(thisItem.Amount));
        $("#BankCodeModal").val(thisItem.BankCode);
        $("#GeneralNotes").val(thisItem.GeneralNotes);
        $("#ChequeStatus").val(thisItem.ChequeStatus);
        $("#PaymentMode").val(thisItem.PaymentMode);
        $("#PaymentMode").prop('disabled', true);
        if (thisItem.TransactionType == "D") {
            $("#lblPaymentMode").text("Deposit Mode");
        }
        else {
            $("#lblPaymentMode").text("Withdrawal Mode");
        }
        DepositModeOnchange(); 
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
    $("#Amount").val("");
    $("#BankCode").val("");
    $("#GeneralNotes").val("");
    $("#ChequeStatus").val("");
    $("#BankCodeModal").val("");
    $("#PaymentMode").val("");
    $("#lblPaymentMode").text("Deposit Mode");
    ResetForm();
    $("#ChequeStatus").prop('disabled', true);
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
    $("#tabDepositwithdrawalEntry").text("Deposit Entry");
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
                $("#TotalAmt").text("");
                $("#TotalAmt").text(ds.TotalAmt);
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
    $("#tabDepositwithdrawalEntry").css('display', '');
    $("#DepositwithdrawalEntry").css('display', '');
    $("#AddOrEditSpan").text("Deposit");
    BindDepositWithdrawals('D', "");
    $('a[href="#DepositwithdrawalList"]').click();
    $("#btnCheque").css('display', 'none');
    $('#tblDepositwithdrawalList tbody td:nth-child(6) ').show();
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
    else
    {
        var SelectedRows = DataTables.tblDepositwithdrawalList.rows(".selected").data();
        if ((SelectedRows) && (SelectedRows.length > 0)) {
            var CheckedIDs = [];
            for (var r = 0; r < SelectedRows.length; r++) {
                CheckedIDs.push(SelectedRows[r].ID);
            }

        }
        var data = "{'data':" + JSON.stringify(CheckedIDs) + "}";
        PostDataToServer('DepositAndWithdrawals/ClearCheque/', data, function (JsonResult) {
            debugger;
            if (JsonResult != '') {
                switch (JsonResult.Result) {
                    case "OK":
                        ShowChequeClear();
                        $("#AddDepositAndWithdrawalModel").modal('hide');
                        notyAlert('success', "Success");
                        CheckedIDs.length = 0;
                        BindDepositAndWithdrawals();
                        break;
                    case "ERROR":
                        notyAlert('error', JsonResult.Message.Message);
                        break;
                    default:
                        break;
                }
            }
        })
    }

    }

function SaveDeposit()
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
            $("#btnDepositWithdrawalSave").trigger('click');
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
    $('#tblDepositwithdrawalList tbody td:nth-child(6) ').hide();
    $("#editRow").hide();
    $('#tblDepositwithdrawalList tbody td:nth-child(1) ').hide();
    $("#editCheckBox").hide();
    $("#btnDepositSave").hide();
    $(".modal-footer").show();
    $("#tabDepositwithdrawalEntry").text("Withdrawal Entry");
    $("#lblBankCode").text("Withdrawal From");
    $("#lblBankDiv").css('display', '');
    $("#lblPaymentMode").text("Withdrawal Mode");
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
    $('#tblDepositwithdrawalList tbody td:nth-child(6) ').hide();
    $("#editRow").hide();
    $('#tblDepositwithdrawalList tbody td:nth-child(1) ').show();
    $("#editCheckBox").show();
    $("#btnDepositSave").hide();
    $(".chqHeader").hide();
    $("#BankCode").hide();
    $(".modal-footer").hide();
    $("#tabDepositwithdrawalList").text("Deposited Cheques");
    $("#lblBankDiv").css('display', 'none');
    
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
