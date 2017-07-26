var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.expenseDetailTable = $('#expenseDetailTable').DataTable(
         {

             dom: '<"pull-right"f>rt<"bottom"p><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllExpenseDetails(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": null },
               { "data": "AccountCode", "defaultContent": "<i>-</i>" },
               { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
               { "data": null, "orderable": false, "defaultContent": '<a data-toggle="tp" data-placement="top" data-delay={"show":2000, "hide":3000} title="Delete" href="#" class="DeleteLink" onclick="Delete(this)"><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
               { "data": "ID" }

             ],
             columnDefs: [{ "targets": [7], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [1, 2] },
             { className: "text-center", "targets": [3, 4, 5, 6] }

             ]
         });
        DataTables.expenseDetailTable.on('order.dt search.dt', function () {
            DataTables.expenseDetailTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();

    } catch (x) {

        notyAlert('error', x.message);

    }

});



function GetAllExpenseDetails() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("OtherExpenses/GetAllOtherExpenses/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            $("#creditdAmt").text(ds.TotalAmount);
           
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

//function openNav(id) {
//    var left = $(".main-sidebar").width();
//    var total = $(document).width();

//    $('.main').fadeOut();
//    document.getElementById("myNav").style.left = "3%";
//    $('#main').fadeOut();

//    if ($("body").hasClass("sidebar-collapse")) {

//    }
//    else {
//        $(".sidebar-toggle").trigger("click");
//    }
//    if (id != "0") {
//        ClearFields();
//    }
//}

function goBack() {
    ClearFields();
    closeNav();
    // BindAllCutomers();
}

function Save() {
    try {
        $("#btnSaveOtherExpense").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}


function PaymentModeOnchange(curobj)
{
    if (curobj.value == "ONLINE") {
        $("#BankCode").prop('disabled', false);
    }
    else {
        $("#BankCode").val("");
        $("#BankCode").prop('disabled', true);
    }
    $('span[data-valmsg-for="BankCode"]').empty();
}
function BankOnchange()
{
    $('span[data-valmsg-for="BankCode"]').empty();
}
function Validation()
{
    var fl = true;
    var pm = $("#paymentMode").val();
    if((pm)&&(pm=="ONLINE"))
    {
        if($("#BankCode").val()=="")
        {
            fl = false;
           
            $('span[data-valmsg-for="BankCode"]').append('<span for="EmpID" class="">BankCode required</span>')
        }
        else
        {
            $('span[data-valmsg-for="BankCode"]').empty();
        }

    }
    return fl;
}

function ClearFields() {
    $("#ID").val(emptyGUID);
    $("#expenseDateModal").val('');
    $("#AccountCode").val('');
    $("#CompanyCode").val('');
    $("#paymentMode").val('');
    $("#EmpID").val('');
    $("#BankCode").val('');
    $("#ExpenseRef").val('');
    $("#Amount").val('');
    $("#Description").val('');
 
    var validator = $("#OtherExpenseModal").validate();
    $('#OtherExpenseModal').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
    //ChangeButtonPatchView("Customers", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}



function BindAllExpenseDetails() {
    try {
        DataTables.expenseDetailTable.clear().rows.add(GetAllExpenseDetails()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllExpenseDetails();
            notyAlert('success', JsonResult.Message);
            $("#ID").val(JsonResult.Record.ID);

            //if ($("#ID").val() != "") {
            //    FillCustomerDetails($("#ID").val());
            //}
            //else {
            //    FillCustomerDetails(JsonResult.Records.ID);
            //}
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
    if ($("#ID").val() == emptyGUID) {
        ClearFields();
    }
    else {
        FillCustomerDetails($("#ID").val());
    }
    ResetForm();
}

function GetExpenseDetailsByID(ID) {
    try {

        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("OtherExpenses/GetExpenseDetailsByID/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Record;
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
function FillOtherExpenseDetails(ID) {
    debugger;
    //ChangeButtonPatchView("Customers", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var thisItem = GetExpenseDetailsByID(ID); //Binding Data
    
    if (thisItem)
    {
        $("#ID").val(thisItem.ID);
        $("#expenseDateModal").val(thisItem.ExpenseDate);
        $("#AccountCode").val(thisItem.AccountCode);
        $("#CompanyCode").val(thisItem.companies.Code);
        $("#paymentMode").val(thisItem.PaymentMode);
        if (thisItem.PaymentMode != "ONLINE")
        {
            $("#BankCode").val("");
            $("#BankCode").prop('disabled', true);
        }
        else
        {
            $("#BankCode").prop('disabled', false);
        }
        $("#EmpID").val(thisItem.employee.ID);
        $("#BankCode").val(thisItem.BankCode);
        $("#ExpenseRef").val(thisItem.ExpenseRef);
        $("#Amount").val(thisItem.Amount);
        $("#Description").val(thisItem.Description);
    }
   

   
}

//---------------------------------------Edit Bank--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    // openNav("0");
    //ResetForm();

    var rowData = DataTables.expenseDetailTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
       
        ClearFields();
        FillOtherExpenseDetails(rowData.ID);
        $("#AddOtherexpenseModel").modal('show');
    }
}

function AddOtherExpense() {
    try {
        ClearFields();
        $("#AddOtherexpenseModel").modal('show');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Delete(currObj) {
    debugger;
    var rowData = DataTables.expenseDetailTable.row($(currObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        notyConfirm('Are you sure to delete?', 'DeleteOtherExpense("' + rowData.ID + '")', '', "Yes, delete it!");
    }

}

function DeleteOtherExpense(ID) {
    try {
       
        
        if (ID) {
            var data = { "ID": ID };
            var ds = {};
            ds = GetDataFromServer("OtherExpenses/DeleteOtherExpense/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message.Message);
              //  IncomeDateOnchange();
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