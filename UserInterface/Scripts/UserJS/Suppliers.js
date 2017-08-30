var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.SupplierTable = $('#SupplierTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllSuppliers(),
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "ID" },
               { "data": "CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "ContactPerson", "defaultContent": "<i>-</i>" },
               { "data": "Mobile", "defaultContent": "<i>-</i>" },
               { "data": "TaxRegNo", "defaultContent": "<i>-</i>" },
               { "data": "PANNO", "defaultContent": "<i>-</i>" },
                { "data": "OutStanding", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [] },
                   { className: "text-left", "targets": [1, 2] },
             { className: "text-center", "targets": [3,4,5, 6] }

             ]
         });

        $('#SupplierTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });






    } catch (x) {

        notyAlert('error', x.message);

    }

});

function GetAllSuppliers() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("Suppliers/GetAllSuppliers/", data);
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
    BindAllSuppliers();
}

function Save() {
    try {
        $("#btnInsertUpdateSuppliers").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteSupplier()', '', "Yes, delete it!");
}

function DeleteSupplier() {
    try {
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("Suppliers/DeleteSupplier/", data);
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
    $("#CompanyName").val("");
    $("#ContactTitle").val("");
    $("#ContactPerson").val("");
    //$("#CompanyName").prop('disabled', false);
    $("#ContactEmail").val("");
    $("#Website").val("");
    $("#LandLine").val("");
    $("#Mobile").val("");
    $("#Fax").val("");
    $("#OtherPhoneNos").val("");
    $("#BillingAddress").val("");
    $("#PaymentTermCode").val("");
    $("#TaxRegNo").val("");
    $("#PANNO").val("");
    $("#GeneralNotes").val("");
    ResetForm();
    ChangeButtonPatchView("Suppliers", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#SuppliersForm").validate();
    $('#SuppliersForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function BindAllSuppliers() {
    try {
        DataTables.SupplierTable.clear().rows.add(GetAllSuppliers()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllSuppliers();
            notyAlert('success', JsonResult.Message);
            debugger;
            if ($("#ID").val() != "") {
                FillSupplierDetails($("#ID").val());
            }
            else {
                FillSupplierDetails(JsonResult.Records.ID);
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
        FillSupplierDetails($("#ID").val());
    }
    ResetForm();
}

function GetSupplierDetailsByID(ID) {
    try {

        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("Suppliers/GetSupplierDetails/", data);
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
function FillSupplierDetails(ID) {
    debugger;
    ChangeButtonPatchView("Suppliers", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var thisItem = GetSupplierDetailsByID(ID); //Binding Data
    //Hidden
    debugger;

    $("#ID").val(thisItem.ID);
    $("#CompanyName").val(thisItem.CompanyName);
    $("#ContactTitle").val(thisItem.ContactTitle);
    $("#ContactPerson").val(thisItem.ContactPerson);
    //$("#CompanyName").prop('disabled', true);
    $("#ContactEmail").val(thisItem.ContactEmail);
    $("#Website").val(thisItem.Website);
    $("#LandLine").val(thisItem.LandLine);
    $("#Mobile").val(thisItem.Mobile);
    $("#Fax").val(thisItem.Fax);
    $("#OtherPhoneNos").val(thisItem.OtherPhoneNos);
    $("#BillingAddress").val(thisItem.BillingAddress);
    $("#PaymentTermCode").val(thisItem.PaymentTermCode);
    $("#TaxRegNo").val(thisItem.TaxRegNo);
    $("#PANNO").val(thisItem.PANNO);
    $("#GeneralNotes").val(thisItem.GeneralNotes);
}

//---------------------------------------Edit Bank--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    openNav("0");
    ResetForm();

    var rowData = DataTables.SupplierTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        FillSupplierDetails(rowData.ID);
    }
}