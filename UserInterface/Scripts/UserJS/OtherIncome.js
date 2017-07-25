
var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.OtherIncomeTable = $('#OtherIncomeTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllOtherIncome(),
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "ID" },
               { "data": "slNo" },
               { "data": "AccountCode", "defaultContent": "<i>-</i>" },
               { "data": "AccountDesc", "defaultContent": "<i>-</i>" },
               { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
               { "data": "IncomeDateFormatted", "defaultContent": "<i>-</i>" },
                 { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0,2], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [7] },
             { className: "text-center", "targets": [1,3,4, 6,8] }

             ]
         });

        //$('#CustomerCreditNoteTable tbody').on('dblclick', 'td', function () {

        //    Edit(this);
        //});


     
        $("#BankCode").prop('disabled', true);


    } catch (x) {

        notyAlert('error', x.message);

    }

});

function IncomeDateOnchange()
{
    var IncomeDate = $("#IncomeDate").val();
    GetAllOtherIncome(IncomeDate);
}

//---------------------------------------Edit Other Income--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    ShowModal();
    ResetForm();

    var rowData = DataTables.OtherIncomeTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        FillOtherIncomeDetails(rowData.ID);
    }
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#OtherIncomeAddModal").validate();
    $('#OtherIncomeAddModal').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}


function PaymentModeOnchange(curObj)
{
    debugger;
    if (curObj.value == "ONLINE")
    {
        $("#BankCode").prop('disabled', false);
    }
    else
    {
        $("#BankCode").val("");
        $("#BankCode").prop('disabled', true);
    }
}


function ShowModal()
{
    $("#AddOtherIncomeModel").modal('show');
    ClearFields();
    BindAllAccountCode();
}


function SaveOtherIncome()
{
    try {
        $("#btnOtherIncomeSave").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function BindAllAccountCode() {
    try {

        var data = {"type":"OI"};
        var ds = {};
        ds = GetDataFromServer("OtherIncome/GetChartOfAccountsByType/", data);
        debugger;
        if (ds != '') {
            $('#AccountCode').find('option').not(':first').remove();
            ds = JSON.parse(ds);

            $.each(ds.Records.accountCodeList, function (key,arr) {
                $('#AccountCode')
                    .append($("<option></option>")
                               .attr("value", arr.Value)
                               .text(arr.Text));
            });
                      
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

function GetAllOtherIncome(IncomeDate) {
    try {

        var data = { "IncomeDate": IncomeDate };
        var ds = {};
        ds = GetDataFromServer("OtherIncome/GetAllOtherIncome/", data);
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


function BindAllOtherIncome() {
    try {
        DataTables.OtherIncomeTable.clear().rows.add(GetAllOtherIncome()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetOtherIncomeDetailsByID(ID)
{
    try {

        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("OtherIncome/GetOtherIncomeDetails/", data);
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
    if ($("#ID").val() == "0") {
        ClearFields();
    }
    else {
        FillOtherIncomeDetails($("#ID").val());
    }
    ResetForm();
}

function FillOtherIncomeDetails(ID)
{
    var thisItem = GetOtherIncomeDetailsByID(ID); //Binding Data
    //Hidden
    debugger;

    $("#ID").val(thisItem.ID);
    $("#IncomeDateModal").val(thisItem.IncomeDateFormatted);
    $("#AccountCode").val(thisItem.AccountCode);
    $("#PaymentRcdComanyCode").val(thisItem.PaymentRcdComanyCode);
    $("#PaymentMode").val(thisItem.PaymentMode);
    $("#BankCode").val(thisItem.BankCode);
    $("#Amount").val(roundoff(thisItem.Amount));
    $("#Description").val(thisItem.Description);
    $("#IncomeRef").val(thisItem.IncomeRef);
}

function SaveSuccess(data, status)
{
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllOtherIncome();
            notyAlert('success', JsonResult.Message);
            debugger;
            if ($("#ID").val() != "") {
                FillOtherIncomeDetails($("#ID").val());
            }
            else {
                FillOtherIncomeDetails(JsonResult.Records.ID);
            }
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}

function ClearFields()
{
    $("#AccountCode").val("");
    $("#PaymentRcdComanyCode").val("");
    $("#PaymentMode").val("");
    $("#DepWithdID").val("");
    $("#BankCode").val("");
    $("#IncomeRef").val("");
    $("#Description").val("");
    $("#Amount").val("");
    $("#IncomeDateModal").val("");
}