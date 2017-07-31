var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.expenseDetailTable = $('#expenseDetailTable').DataTable(
         {

             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
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
               { "data": "chartOfAccounts.TypeDesc", "defaultContent": "<i>-</i>" },
               { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                { "data": "ExpenseDate", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
               { "data": null, "orderable": false, "defaultContent": '<a data-toggle="tp" data-placement="top" data-delay={"show":2000, "hide":3000} title="Delete" href="#" class="DeleteLink" onclick="Delete(this)"><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
               { "data": "ID" }

             ],
             columnDefs: [{ "targets": [8], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [1, 2,3] },
             { className: "text-right", "targets": [4] },
             { className: "text-center", "targets": [5,6,7] }

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



function GetAllExpenseDetails(expDate, DefaultDate) {
    try {
      
        if (expDate == undefined && DefaultDate == undefined) {
            DefaultDate = $("#DefaultDate").val();
        }
        var data = { "ExpenseDate": expDate, "DefaultDate": DefaultDate };
        var ds = {};
        ds = GetDataFromServer("OtherExpenses/GetAllOtherExpenses/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
            $("#TotalAmt").text("");
            $("#TotalAmt").text(ds.TotalAmount);
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
    debugger;
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
    debugger;
    var AcodeCombined = $("#AccountCode").val();
    if (AcodeCombined) {
        var len = AcodeCombined.indexOf(':');
        var IsEmploy = AcodeCombined.substring(len + 1, (AcodeCombined.length));
        if (IsEmploy == "True") {
            if ($("#EmpTypeCode").val() == "")
            {
                fl = false;
                $('span[data-valmsg-for="EmpTypeCode"]').empty();
                $('span[data-valmsg-for="EmpTypeCode"]').append('<span for="EmpTypeCode" class="">Employee Type required</span>')
            }
            else
            {
                $('span[data-valmsg-for="EmpTypeCode"]').empty();
            }
            if ($("#EmpID").val() == "-1" || $("#EmpID").val() == null)
            {
                fl = false;
                $('span[data-valmsg-for="EmpID"]').empty();
                $('span[data-valmsg-for="EmpID"]').append('<span for="EmpID" class="">Employee required</span>')
            }
            else
            {
                $('span[data-valmsg-for="EmpID"]').empty();
            }
            
            
           
        }
        else
        {
            $('span[data-valmsg-for="EmpTypeCode"]').empty();
            $('span[data-valmsg-for="EmpID"]').empty();
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
   
}



function BindAllExpenseDetails(ExpenseDate, DefaultDate) {
    try {
        DataTables.expenseDetailTable.clear().rows.add(GetAllExpenseDetails(ExpenseDate, DefaultDate)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function SaveSuccess(data, status) {
   
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllExpenseDetails();
            $("#AddOtherexpenseModel").modal('hide');
            notyAlert('success', JsonResult.Message);
            $("#ID").val(JsonResult.Record.ID);
                      
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
        $("#AddOrEditSpan").text("Add New");
    }
    else {
      
        FillOtherExpenseDetails($("#ID").val())
    }
    $('span[data-valmsg-for="EmpTypeCode"]').empty();
    $('span[data-valmsg-for="EmpID"]').empty();
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

//---------------------------------------Fill Expense Details--------------------------------------------------//
function FillOtherExpenseDetails(ID) {
  
   
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
       
        $("#EmpTypeCode").val(thisItem.EmpTypeCode);
        if (thisItem.EmpTypeCode)
        {
            BindEmployeeDropDown(thisItem.EmpTypeCode);
        }
        
        $("#EmpID").val(thisItem.employee.ID);
        $("#BankCode").val(thisItem.BankCode);
        $("#ExpenseRef").val(thisItem.ExpenseRef);
        $("#Amount").val(thisItem.Amount);
        $("#Description").val(thisItem.Description);
        $("#AddOrEditSpan").text("Edit");

        if(thisItem.AccountCode)
        {
            var AcodeCombined = thisItem.AccountCode;
            var len = AcodeCombined.indexOf(':');
            var IsEmploy = AcodeCombined.substring(len + 1, (AcodeCombined.length));
            // console.log(str.substring(0, (len)));
            if (IsEmploy == "True") {
                $("#EmpTypeCode").prop('disabled', false);
                $("#EmpID").prop('disabled', false);
            }
            else {
                $("#EmpTypeCode").val('');
                $("#EmpID").val('');
                $("#EmpTypeCode").prop('disabled', true);
                $("#EmpID").prop('disabled', true);
            }
        }
    }
   

   
}

//---------------------------------------Edit Other expense--------------------------------------------------//
function Edit(currentObj) {
    

    var rowData = DataTables.expenseDetailTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
       
        ClearFields();
        debugger;
        FillOtherExpenseDetails(rowData.ID);
        $("#AddOtherexpenseModel").modal('show');
    }
}

function AddOtherExpense() {
    try {
        ClearFields();
        $("#expenseDateModal").val($("#ExpDate").val());
        $("#AddOtherexpenseModel").modal('show');
        $("#AddOrEditSpan").text("Add New");
        $("#EmpID").prop('disabled', true);
        $("#EmpTypeCode").prop('disabled', true);
        $("#BankCode").prop('disabled', true);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Delete(currObj) {
    
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
                
                BindAllOtherExpense($("#ExpDate").val(), $("#DefaultDate").val());
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

function ExpenseDateOnchange()
{
    if (DataTables.expenseDetailTable != undefined)
    {
      $("#DefaultDate").val("");
      var expDate = $("#ExpDate").val();
      BindAllOtherExpense(expDate, "")
    }
    else {
        //GetAllExpenseDetails();
    }
}

function BindAllOtherExpense(expDate, DefaultDate) {
    try {
       
        DataTables.expenseDetailTable.clear().rows.add(GetAllExpenseDetails(expDate, DefaultDate)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}






function ExpenseDefaultDateOnchange()
{
    $("#ExpDate").val("");
    var ExpenseDefaultDate = $("#DefaultDate").val();
    if (DataTables.expenseDetailTable != undefined) {
       
        BindAllOtherExpense("", ExpenseDefaultDate);
    }
    else {
      
    }
}
function AccountCodeOnchange(curobj)
{
    debugger;
    var AcodeCombined = $(curobj).val();
    if(AcodeCombined)
    {
        var len = AcodeCombined.indexOf(':');
        var IsEmploy = AcodeCombined.substring(len + 1, (AcodeCombined.length));
        // console.log(str.substring(0, (len)));
        if(IsEmploy=="True")
        {
            $("#EmpTypeCode").prop('disabled', false);
            $("#EmpID").prop('disabled', false);
        }
        else
        {
            $("#EmpTypeCode").val('');
            $("#EmpID").val('');
            $("#EmpTypeCode").prop('disabled', true);
            $("#EmpID").prop('disabled', true);
        }
    }
    $('span[data-valmsg-for="EmpTypeCode"]').empty();
    $('span[data-valmsg-for="EmpID"]').empty();
}

function EmployeeTypeOnchange(curobj)
{
    var emptypeselected = $(curobj).val();
    if(emptypeselected)
    {
        BindEmployeeDropDown(emptypeselected);
    }
}


function BindEmployeeDropDown(type)
{
    try
    {
        var employees = GetAllEmployeesByType(type);
        if (employees)
        {
            $('#EmpID').empty();
            $('#EmpID').append(new Option('-- Select Employee --', -1));
            for (var i = 0; i < employees.length; i++) {
                var opt = new Option(employees[i].Name, employees[i].ID);
                $('#EmpID').append(opt);

            }
        }
       

    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}

function GetAllEmployeesByType(type)
{
    try {
        var data = { "Type": type };
        var ds = {};
        ds = GetDataFromServer("OtherExpenses/GetAllEmployeesByType/", data);
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