var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.CustomerTable = $('#CustomerTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllCustomers(),
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "ID" },
               {
                   "data": "CompanyName", render: function (data, type, row) { 
                       if (row.IsInternalComp) {
                           return data + " ( <label><i><b> Internal </b></i></label> )";
                       }
                       else {
                           return data;
                       } 
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "ContactPerson", "defaultContent": "<i>-</i>" },
               { "data": "Mobile", "defaultContent": "<i>-</i>" },
               { "data": "TaxRegNo", "defaultContent": "<i>-</i>" },
               { "data": "PANNO", "defaultContent": "<i>-</i>" },
                { "data": "PaymentTerm", "defaultContent": "<i>-</i>",
                   'render': function (data, type, row) {
                       if (data != null) {
                           return data + " Days"
                       }
                       else {
                           return "-"
                       }
                   
                }
                 },
                { "data": "OutStanding", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [6,7] },
                   { className: "text-left", "targets": [1,2] },
             { className: "text-center", "targets": [3,4,5] }

             ]
         });

        $('#CustomerTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });

        debugger;
        if ($('#BindValue').val() != '') {
            dashboardBind($('#BindValue').val())
        }

      
     


    } catch (x) {

        notyAlert('error', x.message);

    }

});



function GetAllCustomers() {
    try {
        debugger;
        var data = {};
        var ds = {};
        ds = GetDataFromServer("Customers/GetAllCustomers/", data);
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
    BindAllCutomers();
}

function Save() {
    try {
        $("#btnInsertUpdateCustomers").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteCustomers()', '', "Yes, delete it!");
}

function DeleteCustomers() {
    try {
        var id = $('#ID').val();
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("Customers/DeleteCustomer/", data);
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
    $("#IsInternalComp").val('false');
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
    $("#ShippingAddress").val("");
    $("#PaymentTermCode").val("");
    $("#TaxRegNo").val("");
    $("#PANNO").val("");
    $("#GeneralNotes").val("");
    ResetForm();
    ChangeButtonPatchView("Customers", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#CustomersForm").validate();
    $('#CustomersForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function BindAllCutomers() {
    try {
        DataTables.CustomerTable.clear().rows.add(GetAllCustomers()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllCutomers();
            notyAlert('success', JsonResult.Message);
            debugger;
            if ($("#ID").val() != "") {
                FillCustomerDetails($("#ID").val());
            }
            else {
                FillCustomerDetails(JsonResult.Records.ID);
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
        FillCustomerDetails($("#ID").val());
    }
    ResetForm();
}

function GetCustomerDetailsByID(ID) {
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

//---------------------------------------Fill Customer Details--------------------------------------------------//
function FillCustomerDetails(ID) {
    debugger;
    ChangeButtonPatchView("Customers", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var thisItem = GetCustomerDetailsByID(ID); //Binding Data
    //Hidden
    debugger;
    
    $("#ID").val(thisItem.ID);
    $("#CompanyName").val(thisItem.CompanyName);
    if (thisItem.IsInternalComp==true)
        $("#IsInternalComp").val('true');
    else
        $("#IsInternalComp").val('false');
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
    $("#ShippingAddress").val(thisItem.ShippingAddress);
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

    var rowData = DataTables.CustomerTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        FillCustomerDetails(rowData.ID);
    }
}

function dashboardBind(ID) {
    openNav("0");
    ResetForm();
    FillCustomerDetails(ID);
}