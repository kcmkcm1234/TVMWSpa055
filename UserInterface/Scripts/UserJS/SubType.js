var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.EmployeeTable = $('#EmployeeTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllEmployee(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "ID" },
               { "data": "Code", "defaultContent": "<i>-</i>" },
               { "data": "Name", "defaultContent": "<i>-</i>" },

               //{ "data": "employeeTypeObj.Name", "defaultContent": "<i>-</i>" },
               { "data": "companies.Name", "defaultContent": "<i>-</i>" },
                 
               { "data": null, "orderable": false, "defaultContent": '<a href="#" title="Edit OtherIncome" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },

               { className: "text-left", "targets": [2, 3] },
             { className: "text-center", "targets": [1] }

             ]
         });

        $('#EmployeeTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });



    } catch (x) {

        notyAlert('error', x.message);

    }

});

function GetAllEmployee() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("SubType/GetAllEmployees/", data);
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
    BindAllEmployee();
}

function Save() {
  
    try {
        //    $('#EmployeeType').val("EMP");
        $("#btnInsertUpdateEmployee").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteEmployee()', '', "Yes, delete it!");
}

function DeleteEmployee() {
    try {
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("SubType/DeleteEmployee/", data);
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
    $("#ID").val("");
    $("#Code").val("");
    $("#Name").val("");
    $("#MobileNo").val("");
    $("#Company").val("");
   
    //  $("#EmployeeType").val("");
    $("#Address").val("");
    $("#GeneralNotes").val("");
    ResetForm();
    ChangeButtonPatchView("SubType", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#EmployeeForm").validate();
    $('#EmployeeForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function BindAllEmployee() {
    try {
        DataTables.EmployeeTable.clear().rows.add(GetAllEmployee()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllEmployee();
            notyAlert('success', JsonResult.Message);
            debugger;
            if ($("#ID").val() != "") {
                FillEmployeeDetails($("#ID").val());
            }
            else {
                FillEmployeeDetails(JsonResult.Records.ID);
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
    if ($("#ID").val() == "0") {
        ClearFields();
    }
    else {
        FillEmployeeDetails($("#ID").val());
    }
    ResetForm();
}

function GetEmployeeDetailsByID(ID) {
    try {

        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("SubType/GetEmployeeDetails/", data);
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

//---------------------------------------Fill Customer Details--------------------------------------------------//
function FillEmployeeDetails(ID) {
    debugger;
    ChangeButtonPatchView("SubType", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var thisItem = GetEmployeeDetailsByID(ID); //Binding Data
    //Hidden
    debugger;

    $("#ID").val(thisItem.ID);
    $("#Code").val(thisItem.Code);
    $("#Name").val(thisItem.Name);
    $("#MobileNo").val(thisItem.MobileNo);
 
    $("#Company").val(thisItem.companyID);
    //    $("#EmployeeType").val(thisItem.EmployeeType);   
    $("#Address").val(thisItem.Address);
    $("#GeneralNotes").val(thisItem.GeneralNotes);
}

//---------------------------------------Edit Bank--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    openNav("0");
    ResetForm();

    var rowData = DataTables.EmployeeTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        FillEmployeeDetails(rowData.ID);
    }
}