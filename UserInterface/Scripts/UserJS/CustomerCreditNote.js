﻿var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {
        $("#Customer").select2({
        });
        DataTables.CustomerCreditNoteTable = $('#CustomerCreditNoteTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllCustomerCreditNotes(),
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "ID" },
               { "data": "CreditNoteNo", "defaultContent": "<i>-</i>" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "CreditNoteDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "CreditAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               {
                   "data": "", render: function (data, type, row) {
                 
                       var CreditUsed = row.CreditAmount - row.AvailableCredit
                       return roundoff(CreditUsed);
                    }, "defaultContent": "<i>-</i>"
                },
               { "data": "AvailableCredit", render: function (data, type, row) {
                       return roundoff(data);
                   }, "defaultContent": "<i>-</i>"
                },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" title="Edit Credit Note" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [4,5,6] },
                   { className: "text-Left", "targets": [1,2] },
             { className: "text-center", "targets": [3,7] }

             ]
         });

        $('#CustomerCreditNoteTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });


        $("#BillingAddress").prop('disabled', true);



    } catch (x) {

        notyAlert('error', x.message);

    }
    debugger;
    if ($('#BindValue').val() != '') {
        dashboardBind($('#BindValue').val())
    }

});
function dashboardBind(ID) {
    debugger;
    openNav();
    ResetForm();
    FillCustomerCreditNoteDetails(ID);
}
function BindBillingAddress(curObj) {
    debugger;
    if (curObj.value != "") {
        var ID = curObj.value;
        var thisItem = GetCustomerDetails(ID);
        $("#BillingAddress").val(thisItem.BillingAddress);
    }
}

function GetCustomerDetails(ID) {
    try {

        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("Customers/GetCustomerDetailsByID/", data);
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

function GetAllCustomerCreditNotes() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("CustomerCreditNote/GetAllCustomerCreditNotes/", data);
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
    BindAllCutomerCreditNote();
}

function Save() {
    try {
        $("#btnInsertUpdateCustomerCreditNote").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteCustomerCreditNote()', '', "Yes, delete it!");
}

function DeleteCustomerCreditNote() {
    try {
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("CustomerCreditNote/DeleteCustomerCreditNote/", data);
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
    $("#Customer").val("");
    $("#Company").val("");
    $("#CreditNoteDate").val("");
    $("#BillingAddress").val("");
    $("#Customer").select2();
    $("#Customer").val('').trigger('change');
    $("#CreditAmount").val("0.00");
    $("#TaxAmount").val("0.00");
    $("#Amount").val("0.00");
    $("#GeneralNotes").val("");
    $("#creditdAmt").text("₹ 0.00");
    $("#adjusteddAmt").text("₹ 0.00");
    ResetForm();
    ChangeButtonPatchView("CustomerCreditNote", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#CustomerCreditNoteForm").validate();
    $('#CustomerCreditNoteForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function BindAllCutomerCreditNote() {
    try {
        DataTables.CustomerCreditNoteTable.clear().rows.add(GetAllCustomerCreditNotes()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllCutomerCreditNote();
            notyAlert('success', JsonResult.Message);
            debugger;
            if ($("#ID").val() != "") {
                FillCustomerCreditNoteDetails($("#ID").val());
            }
            else {
                FillCustomerCreditNoteDetails(JsonResult.Records.ID);
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
        FillCustomerCreditNoteDetails($("#ID").val());
    }
    ResetForm();
}

function GetCustomerCreditNoteDetailsByID(ID) {
    try {

        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("CustomerCreditNote/GetCustomerCreditNoteDetails/", data);
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

//---------------------------------------Fill Customer Credit Note Details--------------------------------------------------//
function FillCustomerCreditNoteDetails(ID) {
    debugger;
    ChangeButtonPatchView("CustomerCreditNote", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var thisItem = GetCustomerCreditNoteDetailsByID(ID); //Binding Data
    //Hidden
    debugger;

    $("#ID").val(thisItem.ID);
    $("#CreditNoteNo").val(thisItem.CreditNoteNo);
    $("#Customer").select2();
    $("#Customer").val(thisItem.CustomerID).trigger('change');
    $("#Company").val(thisItem.OriginComanyCode);
    $("#CreditNoteDate").val(thisItem.CreditNoteDateFormatted);
    $("#BillingAddress").val(thisItem.BillingAddress);
    $("#Amount").val(roundoff(thisItem.Amount));
    $("#TaxAmount").val(roundoff(thisItem.TaxAmount));
    $("#CreditAmount").val(roundoff(thisItem.CreditAmount));
    $("#creditdAmt").text(thisItem.creditAmountFormatted);
    $("#adjusteddAmt").text(thisItem.adjustedAmountFormatted);
    $("#GeneralNotes").val(thisItem.GeneralNotes);
}

//---------------------------------------Edit Customer Credit Note--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    openNav("0");
    ResetForm();

    var rowData = DataTables.CustomerCreditNoteTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        FillCustomerCreditNoteDetails(rowData.ID);
    }
}
//-------------------------------------Calculate Total Credit Amount--------------------------------------------------//
function CalculateCreditAmount()
{
    debugger;
    if ($("#TaxAmount").val() != "" && $("#CreditAmount").val() != "") {
        var TaxAmt = parseFloat($("#TaxAmount").val());
        var CreditAmt = parseFloat($("#CreditAmount").val());
        var Total = parseFloat(TaxAmt + CreditAmt);
        $("#Amount").val(roundoff(Total));
    }
    else {
        if ($("#TaxAmount").val() == "") {
            $("#TaxAmount").val("0.00");
            //$("TaxAmount").trigger("change");
            CalculateCreditAmount();
        }
        if ($("#CreditAmount").val() == "") {
            $("#CreditAmount").val("0.00");
            //$("CreditAmount").trigger("change");
            CalculateCreditAmount();
        }
    }
}