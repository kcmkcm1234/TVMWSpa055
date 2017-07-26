
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
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": null },
               { "data": "ID" },              
               { "data": "AccountCode", "defaultContent": "<i>-</i>" },
               { "data": "AccountDesc", "defaultContent": "<i>-</i>" },
               { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
               { "data": "IncomeDateFormatted", "defaultContent": "<i>-</i>" },
                 { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" title="Edit OtherIncome" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
               { "data": null, "orderable": false, "defaultContent": '<a data-toggle="tp" data-placement="top" data-delay={"show":2000, "hide":3000} title="Delete OtherIncome" href="#" class="DeleteLink" onclick="Delete(this)"><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [1,2], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [7] },
             { className: "text-center", "targets": [0,3,4,5, 6,8] }

             ]
         });
        DataTables.OtherIncomeTable.on('order.dt search.dt', function () {
            DataTables.OtherIncomeTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
        $('#OtherIncomeTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });


     
        $("#BankCode").prop('disabled', true);


    } catch (x) {

        notyAlert('error', x.message);

    }

});

function IncomeDefaultDateOnchange()
{
    $("#IncomeDate").val("");
    var IncomeDefaultDate = $("#DefaultDate").val();
    if (DataTables.OtherIncomeTable != undefined) {
        BindAllOtherIncome("",IncomeDefaultDate)
    }
    else {
        GetAllOtherIncome();
    }
}
function IncomeDateOnchange()
{
    debugger;
    if (DataTables.OtherIncomeTable != undefined)
    {
        $("#DefaultDate").val("");
        var IncomeDate = $("#IncomeDate").val();        
        BindAllOtherIncome(IncomeDate,"")
    }
    else
    {
        GetAllOtherIncome();
    }
   
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

function Delete(currObj) {
    debugger;    
    var rowData = DataTables.OtherIncomeTable.row($(currObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        var ID = rowData.ID;
        notyConfirm('Are you sure to delete?', 'DeleteOtherIncome("' + ID + '")', '', "Yes, delete it!");
    }
    
}

function DeleteOtherIncome(ID) {
    try {
        debugger;
        var id = ID;
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("OtherIncome/DeleteOtherIncome/", data);
            debugger;
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message.Message);
                BindAllOtherIncome($("#IncomeDate").val(), $("#DefaultDate").val());
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
    $('span[data-valmsg-for="BankCode"]').empty();
}


function ShowModal()
{
    $("#AddOtherIncomeModel").modal('show');
    ClearFields();
    $("#AddOrEditSpan").text("Add New");
}

function Validation() {
    debugger;
    var fl = true;
    var pm = $("#PaymentMode").val();
    if ((pm) && (pm == "ONLINE")) {
        if ($("#BankCode").val() == "") {
            fl = false;

            $('span[data-valmsg-for="BankCode"]').append('<span for="EmpID" class="">BankCode required</span>')
        }
        else {
            $('span[data-valmsg-for="BankCode"]').empty();
        }

    }
    return fl;
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


function GetAllOtherIncome(IncomeDate,DefaultDate) {
    try {
        debugger;
        if (IncomeDate == undefined && DefaultDate == undefined)
        {
            DefaultDate = $("#DefaultDate").val();
        }
        var data = { "IncomeDate": IncomeDate,"DefaultDate":DefaultDate };
        var ds = {};
        ds = GetDataFromServer("OtherIncome/GetAllOtherIncome/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
            $("#TotalAmt").text("");
            $("#TotalAmt").text(ds.TotalAmt);
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


function BindAllOtherIncome(IncomeDate,DefaultDate) {
    try {
        debugger;
        DataTables.OtherIncomeTable.clear().rows.add(GetAllOtherIncome(IncomeDate, DefaultDate)).draw(false);
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
        $("#AddOrEditSpan").text("Add New");
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
    $("#DepWithdID").val(thisItem.DepWithdID);
    $("#AddOrEditSpan").text("Edit");
}

function SaveSuccess(data, status)
{
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#IncomeDate").val() == undefined || $("#IncomeDate").val() == "")
            {
                IncomeDefaultDateOnchange();
            }
            else
            {
                IncomeDateOnchange();
            }
            //IncomeDateOnchange();
            notyAlert('success', JsonResult.Message);
            debugger;
            if ($("#ID").val() != "" && $("#ID").val() != "0") {
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
    $("#ID").val("");
    $("#DepWithdID").val(emptyGUID);
    ResetForm();
}