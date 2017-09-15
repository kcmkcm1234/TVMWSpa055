var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.EmployeeCategoryTable = $('#EmployeeCategoryTable').DataTable(
         {
             //dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             data: GetAllEmployeeCategorys(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "Code", "defaultContent": "<i>-</i>" },
               { "data": "Name", "defaultContent": "<i>-</i>" },
               { "data": "commonObj.CreatedDateString", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" title="Edit OtherIncome" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [

               { className: "text-left", "targets": [0, 1] },
             { className: "text-center", "targets": [2] }

             ]
         });

        $('#EmployeeCategoryTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });

        $(".buttons-excel").hide();

    } catch (x) {

        notyAlert('error', x.message);

    }

});

function PrintReport() {
    try {
        debugger;

        $(".buttons-excel").trigger('click');


    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function GetAllEmployeeCategorys() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("EmployeeCategory/GetAllEmployeeCategories/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {

            notyAlert('error', ds.Message);
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
        $("#Operation").val('Insert');
    }

}

function goBack() {
    ClearFields();
    closeNav();
    BindAllEmployeeCategorys();
}

function Save() {

    try {
       
        $("#btnInsertUpdateCategory").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);

    }
}

function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteEmployeeCategory()', '', "Yes, delete it!");
}

function DeleteEmployeeCategory() {
    try {
        var id = $('#Code').val();
        if (id != '' && id != null) {
            var data = { "Code": id };
            var ds = {};
            ds = GetDataFromServer("EmployeeCategory/DeleteEmployeeCategory/", data);

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

    $("#Code").val("");
    $("#Name").val("");
    $("#Code").prop("readonly", false);

    ResetForm();
    ChangeButtonPatchView("EmployeeCategory", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#CategoryForm").validate();
    $('#CategoryForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function BindAllEmployeeCategorys() {
    try {
        DataTables.EmployeeCategoryTable.clear().rows.add(GetAllEmployeeCategorys()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllEmployeeCategorys();
            $("#Operation").val('Update');
            $("#Code").prop("readonly", true);
            notyAlert('success', JsonResult.Record.Message);
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
    if ($("#Code").val() == "") {
        ClearFields();
    }
    else {
        FillEmployeeCategoryDetails($("#Code").val());
    }
    ResetForm();
}

function GetEmployeeCategoryByID(Code) {
    try {

        var data = { "Code": Code };
        var ds = {};
        ds = GetDataFromServer("EmployeeCategory/GetEmployeeCategories/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);

        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

//---------------------------------------Fill Customer Details--------------------------------------------------//
function FillEmployeeCategoryDetails(Code) {

    ChangeButtonPatchView("EmployeeCategory", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var thisItem = GetEmployeeCategoryByID(Code); //Binding Data
    //Hidden
    $("#Code").val(thisItem.Code);
    $("#Name").val(thisItem.Name);

}

//---------------------------------------Edit Bank--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click

    openNav("0");
    ResetForm();
    $("#Code").prop("readonly", true);
    $("#Operation").val('Update');
    var rowData = DataTables.EmployeeCategoryTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.Code != null)) {
        FillEmployeeCategoryDetails(rowData.Code);
    }
}