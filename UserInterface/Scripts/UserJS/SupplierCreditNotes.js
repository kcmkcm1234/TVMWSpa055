var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {
      
        DataTables.supplierCreditNoteTable = $('#supplierCreditNoteTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllSupplierCreditNotes(),
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "ID" },
               { "data": "supplier.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Company.Name", "defaultContent": "<i>-</i>" },
               { "data": "CRNRefNo", "defaultContent": "<i>-</i>" },
               { "data": "Amount",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "CRNDate", "defaultContent": "<i>-</i>" },
                //{ "data": "Type", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" title="Edit Credit Note" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },                 
                   { className: "text-right", "targets": [4] },
             { className: "text-center", "targets": [1,2,3, 5, 6] }

             ]
         });

        $('#supplierCreditNoteTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });



        $("#SupplierAddress").prop('disabled', true);


    } catch (x) {

        notyAlert('error', x.message);

    }

});



function GetAllSupplierCreditNotes() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("SupplierCreditNote/GetAllSupplierCreditNotes/", data);
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
        $("#btnInsertUpdateSupplierCreditNote").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteSupplierCreditNote()', '', "Yes, delete it!");
}

function DeleteSupplierCreditNote() {
    try {
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("SupplierCreditNote/DeleteSupplierCreditNote/", data);
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
    $("#CreditNoteNo").val("");
    $("#supplier").val("");
    $("#Company").val("");
    $("#CreditNoteDate").val("");
    $("#SupplierAddress").val("");
    $("#CreditAmount").val("");
    $("#GeneralNotes").val("");
    $("#creditdAmt").text("₹ 0.00");
    //$("#adjusteddAmt").text("₹ 0.00");
    ResetForm();
    ChangeButtonPatchView("SupplierCreditNote", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#SupplierCreditNoteForm").validate();
    $('#SupplierCreditNoteForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function BindAllSuppliers() {
    try {
        DataTables.supplierCreditNoteTable.clear().rows.add(GetAllSupplierCreditNotes()).draw(false);
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
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("SupplierCreditNote/GetSupplierCreditNoteDetails/", data);
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
    ChangeButtonPatchView("SupplierCreditNote", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var thisItem = GetSupplierDetailsByID(ID); //Binding Data
    //Hidden
    debugger;

    $("#ID").val(thisItem.ID);
    $("#CreditNoteNo").val(thisItem.CRNRefNo);
    $("#supplier").val(thisItem.supplier.ID);
    $("#Company").val(thisItem.CompanyCode);
    $("#CreditNoteDate").val(thisItem.CRNDate);
    $("#SupplierAddress").val(thisItem.SupplierAddress);
    $("#CreditAmount").val(roundoff(thisItem.Amount));
    $("#creditdAmt").text(thisItem.creditAmountFormatted);
    //$("#adjusteddAmt").text(thisItem.adjustedAmountFormatted);
    $("#GeneralNotes").val(thisItem.GeneralNotes);
}

function BindSupplierAddress(curObj) {
    debugger;
    var ID = curObj.value;
    var thisItem = GetSupplierByID(ID);
    $("#SupplierAddress").val(thisItem.BillingAddress);
}

function GetSupplierByID(ID) {
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
//---------------------------------------Edit Bank--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    openNav("0");
    ResetForm();

    var rowData = DataTables.supplierCreditNoteTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        FillSupplierDetails(rowData.ID);
    }
}