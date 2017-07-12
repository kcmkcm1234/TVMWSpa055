var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {
        debugger;
        DataTables.TaxTypesTable = $('#TaxTypesTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllTaxTypes(),
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "Code", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Rate", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [2] },
             { className: "text-center", "targets": [0, 1,3] }

             ]
         });

        $('#TaxTypesTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });






    } catch (x) {

        notyAlert('error', x.message);

    }

});

function GetAllTaxTypes() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("TaxType/GetAllTaxTypes/", data);
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

function openNav(id) {
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
    if (id != "0") {
        ClearFields();
    }
}

function goBack() {
    ClearFields();
    closeNav();
    BindAllTaxTypes();
}

function Save() {
    try {
        $("#btnInsertUpdateTaxTypes").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}


function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteTaxType()', '', "Yes, delete it!");
}


function DeleteTaxType()
{
    try {
        var code = $('#hdnCode').val();
        if (code != '' && code != null) {
            var data = { "code": code };
            var ds = {};
            ds = GetDataFromServer("TaxType/DeleteTaxType/", data);
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

function ClearFields() {
    $("#isUpdate").val("0");
    $("#Code").val("");
    $("#Description").val("");
    $("#Rate").val("");
    $("#Code").prop('disabled', false);
    $("#hdnCode").val("");
    ResetForm();
    ChangeButtonPatchView("TaxType", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#TaxTypesForm").validate();
    $('#TaxTypesForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function BindAllTaxTypes() {
    try {
        DataTables.TaxTypesTable.clear().rows.add(GetAllTaxTypes()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    debugger;
    switch (JsonResult.Result) {
        case "OK":
            BindAllTaxTypes();
            notyAlert('success', JsonResult.Message);
            debugger;
            if ($("#hdnCode").val() != "") {
                FillTaxTypeDetails($("#hdnCode").val());
            }
            else {
                FillTaxTypeDetails(JsonResult.Records.Code);
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

function Reset() {
    if ($("#isUpdate").val() == "0") {
        ClearFields();
    }
    else {
        FillTaxTypeDetails($("#hdnCode").val());
    }
}

function GetTaxTypeDetailsByCode(Code) {
    try {

        var data = { "Code": Code };
        var ds = {};
        ds = GetDataFromServer("TaxType/GetTaxTypeDetailsByCode/", data);
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

//---------------------------------------Fill Bank Details--------------------------------------------------//
function FillTaxTypeDetails(Code) {
    debugger;
    ChangeButtonPatchView("TaxType", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var thisItem = GetTaxTypeDetailsByCode(Code); //Binding Data
    //Hidden
    debugger;
    $("#Code").val(thisItem.Code);
    $("#Description").val(thisItem.Description);
    $("#Rate").val(roundoff(thisItem.Rate))
    // $("#deleteId").val(thisItem[0].Code);
    $("#isUpdate").val("1");
    $("#hdnCode").val(thisItem.Code);
    $("#Code").prop('disabled', true);
}

//---------------------------------------Edit Bank--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    openNav("0");
    ResetForm();

    var rowData = DataTables.TaxTypesTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.Code != null)) {
        FillTaxTypeDetails(rowData.Code);
    }
}