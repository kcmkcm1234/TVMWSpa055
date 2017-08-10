var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.BankTable = $('#BankTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllBanks(),
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "Code", "defaultContent": "<i>-</i>" },
               { "data": "Name", "defaultContent": "<i>-</i>" },
               { "data": "Company.Name", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [] },
                   { className: "text-left", "targets": [1,2] },
             { className: "text-center", "targets": [0] }

             ]
         });

        $('#BankTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });






    } catch (x) {

        notyAlert('error', x.message);

    }

});


//---------------------------------------Edit Bank--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    openNav("0");
    ResetForm();

    
    var rowData = DataTables.BankTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.Code != null)) {
        FillBankDetails(rowData.Code);
    }
}

function openNav(id)
{
    var left = $(".main-sidebar").width();
    var total = $(document).width();
  
    $('.main').fadeOut();
    document.getElementById("myNav").style.left = "3%";
    $('#main').fadeOut();

    if ($("body").hasClass("sidebar-collapse")) {

    }
    else {
        $(".sidebar-toggle").trigger("click");
    }
    if(id!="0")
    {
        ClearFields();
    }
}

function goBack()
{
    ClearFields();
    closeNav();
    BindAllBanks();
}

//---------------------------------------Fill Bank Details--------------------------------------------------//
function FillBankDetails(Code) {
    debugger;
    ChangeButtonPatchView("Bank", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var thisItem = GetBankDetailsByCode(Code); //Binding Data
    //Hidden
    debugger;
    $("#Code").val(thisItem.Code);
    $("#Name").val(thisItem.Name);
    $("#CompanyCode").val(thisItem.CompanyCode)    
   // $("#deleteId").val(thisItem[0].Code);
    $("#isUpdate").val("1");
    $("#hdnCode").val(thisItem.Code);
    $("#Code").prop('disabled', true);
}

function GetBankDetailsByCode(Code) {
    try {

        var data = {"Code":Code};
        var ds = {};
        ds = GetDataFromServer("Bank/GetBankDetailsByCode/", data);
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
    if($("#isUpdate").val()=="0")
    {
        ClearFields();
    }
    else
    {
        FillBankDetails($("#hdnCode").val());
    }
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#BankForm").validate();
    $('#BankForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}


function GetAllBanks() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("Bank/GetAllBanks/", data);
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

function ClearFields()
{
    $("#isUpdate").val("0");
    $("#Code").val("");
    $("#Name").val("");
    $("#CompanyCode").val("");
    $("#Code").prop('disabled', false);
    $("#hdnCode").val("");
    ResetForm();
    ChangeButtonPatchView("Bank", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}


function SaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllBanks();
            notyAlert('success', JsonResult.Message);
            debugger;
            if ($("#hdnCode").val() != "")
            {
                FillBankDetails($("#hdnCode").val());
            }
            else
            {
                FillBankDetails(JsonResult.Records.Code);
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

function BindAllBanks() {
    try {
        DataTables.BankTable.clear().rows.add(GetAllBanks()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Save()
{
    try
    {
        $("#btnInsertUpdateBank").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function Delete()
{
    notyConfirm('Are you sure to delete?', 'DeleteBank()', '', "Yes, delete it!");
}

function DeleteBank()
{
    try {
        var code = $('#hdnCode').val();
        if (code != '' && code != null) {
            var data = { "code": code };
            var ds = {};
            ds = GetDataFromServer("Bank/DeleteBank/", data);
            debugger;
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message.Message);
                goBack();
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