var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000';
var DefaultDate = "";
$(document).ready(function () {
    try {
        debugger;
        $("#DefaultDate").val("");
      
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
               { "data": "chartOfAccountsObj.TypeDesc", "defaultContent": "<i>-</i>" },
               { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
                
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                { "data": "ExpenseDate", "defaultContent": "<i>-</i>" },
                 { "data": "companies.Name", "defaultContent": "<i>-</i>" },
                 { "data": "ReferenceNo", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
               { "data": null, "orderable": false, "defaultContent": '<a data-toggle="tp" data-placement="top" data-delay={"show":2000, "hide":3000} title="Delete" href="#" class="DeleteLink" onclick="Delete(this)"><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
               { "data": "ID" }

             ],
             columnDefs: [{ "targets": [10], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [1, 2,3,6,9,7] },
             { className: "text-right", "targets": [4] },
             { className: "text-center", "targets": [5,8] }

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

            $('#expenseDetailTable tbody').on('dblclick', 'td', function () {
                Edit(this)
            });
            DefaultDate = $("#ExpDate").val();
            BindOpeningBalance()

            debugger;
            if ($('#BindValue').val() != '') {
                dashboardBind($('#BindValue').val())
            }

    try {

    DataTables.tblbankWiseBalanceTable = $('#tblbankWiseBalanceTable').DataTable({
        dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
        order: [],
        searching: false,
        paging: true,
        data: null,
        pageLength: 10,
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Search"
        },
        columns: [
          { "data": "BankCode", "defaultContent": "<i>-</i>" },
          { "data": "BankName", "defaultContent": "" },
          { "data": "TotalAmount", render: function (data, type, row) { return formatCurrency(data); }, "defaultContent": "<i>-</i>" },
    
        ],
        columnDefs: [
             { className: "text-right", "targets": [2] },
             { className: "text-left", "targets": [0, 1] },
             { className: "text-center", "targets": [] },
             { "bSortable": false, "aTargets": [0, 1, 2] }
        ],
    });
    

    } catch (x) {

        notyAlert('error', x.message);
    }



});

function dashboardBind(ID) {
    ClearFields();
    FillOtherExpenseDetails(ID);
    $("#AddOtherexpenseModel").modal('show');
}

function BindOpeningBalance() {
    debugger;
    var OpeningDate = $("#ExpDate").val();
    if (OpeningDate != "" && IsVaildDateFormat(OpeningDate)) {
       
        var items = GetOpeningBalance();
        $('#OpeningDate').text('');
        $('#OpeningDate').append('<b>' + $("#ExpDate").val() + '</b>')
        $('#OpeningBank').text('');
        $('#OpeningBank').append('<span><b> ' + items.OpeningBank + '</b></span>');
        $('#OpeningCash').text('');
        $('#OpeningCash').append('<span><b> ' + items.OpeningCash + '</b></span>');
        $('#OpeningNCBank').text('');
        $('#OpeningNCBank').append('<span><b> ' + items.OpeningNCBank + '</b></span>');
        $('#UndepositedCheque').text('');
        $('#UndepositedCheque').append('<span><b> ' + items.UndepositedCheque + '</b></span>');
    }
    else {
        $("#ExpDate").val(DefaultDate).trigger('change');
}

   
}

function GetOpeningBalance() {
    try {
        debugger;
        var OpeningDate = $("#ExpDate").val();
        if (OpeningDate != "" && IsVaildDateFormat(OpeningDate))
        {
            var data = { "OpeningDate": OpeningDate };
            var ds = {};
            ds = GetDataFromServer("OtherExpenses/GetOpeningBalance/", data);
            ds = JSON.parse(ds);
            if (ds.Result == "OK") {
                return ds.Records;
            }
            if (ds.Result == "ERROR") {
                alert(ds.Message);
            }
        }
        
    }
    catch (e) {
        notyAlert('error', e.message);
    }

}

function BankwiseBalance() {
    debugger;
    $("#BankWiseBalanceList").modal('show');

    DataTables.tblbankWiseBalanceTable.clear().rows.add(GetBankWiseBalance()).draw(false);

}

function GetBankWiseBalance() {
    try {
        
        debugger;
        var Date = $("#ExpDate").val();
        if (Date != "" && IsVaildDateFormat(Date))
        {
            var data = { "Date": Date };
            var ds = {};
            ds = GetDataFromServer("OtherExpenses/GetBankWiseBalance/", data);
            ds = JSON.parse(ds);
            $("#TotalBlnce").text("");
            $("#TotalBlnce").text(ds.TotalAmount);
            if (ds.Result == "OK") {
                return ds.Records;
            }
            if (ds.Result == "ERROR") {
                alert(ds.Message);
            }
}
        
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}



function GetAllExpenseDetails(expDate, DefaultDate) {
    try {
      
        if (expDate == undefined && DefaultDate == undefined) {
            expDate = $("#ExpDate").val();
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
        debugger;
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
        //$('#myModal').modal('hide')
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
        $("#ChequeDate").prop('disabled', false);
        $("#ReferenceBank").prop('disabled', false);
    }
    else {
        $("#BankCode").val("");
        $("#BankCode").prop('disabled', true);
    }
    if (curobj.value == "CHEQUE")
    {
        $("#BankCode").prop('disabled', false);
        $("#ChequeDate").prop('disabled', false);
        $("#ReferenceBank").prop('disabled', true);
    }
    else
    {
        $("#ChequeDate").prop('disabled', true);
        $("#ReferenceBank").prop('disabled', true);
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
    if ((pm) && (pm == "CHEQUE"))
    {
        if ($("#ChequeDate").val() == "") {
            fl = false;

            $('span[data-valmsg-for="ChequeDate"]').append('<span for="ChequeDate" class="">Cheque Date required</span>')
        }
        else {
            $('span[data-valmsg-for="ChequeDate"]').empty();
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
            if (($("#EmpID").val() == "-1" || $("#EmpID").val() == null) && $("#EmpName").val() == "")
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
    $("#EmpTypeCode").val('');
    $("#ReferenceBank").val('');
    $("#BankCode").val('');
    $("#ExpenseRef").val('');
    $("#Amount").val('');
    $("#Description").val('');
    $("#ChequeDate").val('');
    $("#IsReverse").val('false');
    $("#EmpID").prop('disabled', true);
    $("#EmpTypeCode").prop('disabled', true);
    $("#BankCode").prop('disabled', true);
    $("#ChequeDate").prop('disabled', true);
    $("#ReferenceBank").prop('disabled', true);
    $('#EmpID').empty();
    $('#EmpID').append(new Option('-- Select Employee --', -1));
    $('#EmpID').val("-1");
    $("#EmpName").val("");
    $("#btnAddEmployee").css("pointer-events", "none");
    $("#EmployeeDiv").hide();
    $("#creditdAmt").text("₹ 0.00");
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
            //$("#AddOtherexpenseModel").modal('hide');
            notyAlert('success', JsonResult.Message);
            $('#AddOtherexpenseModel').modal('hide');
            debugger;
            if ($("#ID").val() != "" && $("#ID").val() != "0" && $("#ID").val()!=emptyGUID) {
                FillOtherExpenseDetails($("#ID").val());
               
            }
            else
            {
                FillOtherExpenseDetails(JsonResult.Record.ID);
            }
           //$("#ID").val(JsonResult.Record.ID);
                      
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
    $('span[data-valmsg-for="ChequeDate"]').empty();
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
    debugger;
    if (thisItem)
    {
        $("#ID").val(thisItem.ID);
        $("#expenseDateModal").val(thisItem.ExpenseDate);
        $("#AccountCode").val(thisItem.AccountCode);
        $("#CompanyCode").val(thisItem.companies.Code);
        $("#paymentMode").val(thisItem.PaymentMode);
        $("#ChequeDate").val(thisItem.ChequeDate);
        $("#ReferenceBank").val(thisItem.ReferenceBank);
        $("#creditdAmt").text(thisItem.creditAmountFormatted);
        if (thisItem.PaymentMode != "ONLINE")
        {
            $("#BankCode").val("");
            $("#BankCode").prop('disabled', true);
        }
        else
        {
            $("#BankCode").prop('disabled', false);
        }
        if (thisItem.PaymentMode != "CHEQUE")
        {
            $("#ChequeDate").prop('disabled', true);
            $("#ReferenceBank").prop('disabled', true);
        }
        else
        {
            $("#ChequeDate").prop('disabled', false);
            $("#ReferenceBank").prop('disabled', false);
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
        if (thisItem.Amount < 0)
            $("#IsReverse").val('true');
            else
            $("#IsReverse").val('false');

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
                $("#btnAddEmployee").css("pointer-events", "auto");
            }
            else {
                $("#EmpTypeCode").val('');
                $("#EmpID").val('');
                $("#EmpTypeCode").prop('disabled', true);
                $("#EmpID").prop('disabled', true);
                $("#btnAddEmployee").css("pointer-events", "none");
               
            }
        }
    }
   

   
}

function AddEmployee()
{
    debugger;
    if ($("#EmpTypeCode").val() != "") $("#sbtyp").html($("#EmpTypeCode option:selected").text());
    if ($("#CompanyCode").val() != "") $("#cmpny").html($("#CompanyCode option:selected").text());
    $("#EmployeeDiv").fadeIn();
}

function CancelEmployee()
{
    $("#sbtyp").html("Not Selected");
    $("#cmpny").html("Not Selected");

    $("#EmpName").val("");
    $("#EmployeeDiv").hide();
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
        //$("#EmpID").prop('disabled', true);
        //$("#EmpTypeCode").prop('disabled', true);
        //$("#BankCode").prop('disabled', true);
        //$("#ChequeDate").prop('disabled', true);
        $("#btnAddEmployee").css("pointer-events", "none");
        $("#EmployeeDiv").hide();
        
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
    BindOpeningBalance();
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



function SaveEmployee()
{
    debugger;
    try {
        //$("#btnSaveEmployee").trigger('click');
        var f1 = true;
        if($("#EmpTypeCode").val()=="" )
        {
            f1 = false;
            $('span[data-valmsg-for="EmpTypeCode"]').empty();
            $('span[data-valmsg-for="EmpTypeCode"]').append('<span for="EmpTypeCode" class="">Employee Type required</span>')
        }
        else
        {
            
            $('span[data-valmsg-for="EmpTypeCode"]').empty();
        }
        if($("#CompanyCode").val()=="")
        {
            f1 = false;
            $('span[data-valmsg-for="PaidFromCompanyCode"]').empty();
            $('span[data-valmsg-for="PaidFromCompanyCode"]').append('<span for="PaidFromCompanyCode" class="">Company Code required</span>')
        }
        else
        {
            $('span[data-valmsg-for="PaidFromCompanyCode"]').empty();
        }
        if ($("#EmpName").val() == "")
        {
            f1 = false;
            $('span[data-valmsg-for="EmpName"]').empty();
            $('span[data-valmsg-for="EmpName"]').append('<span for="EmpName" class="">Employee Name required</span>')
        }
        else
        {
            $('span[data-valmsg-for="EmpName"]').empty();
        }
        if(f1==true)
        {
            AddNewEmployee();
        }
        
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function AddNewEmployee()
{
    debugger;
    try {
        var EmployeeViewModel = new Object();
        EmployeeViewModel.Name = $("#EmpName").val();
        EmployeeViewModel.EmployeeType = $("#EmpTypeCode").val();
        EmployeeViewModel.companyID = $("#CompanyCode").val();
        EmployeeViewModel.Code = Math.floor((Math.random() * 10000) + 1);
        var data = "{'_employeeObj':" + JSON.stringify(EmployeeViewModel) + "}";
        PostDataToServer('OtherExpenses/InsertUpdateEmployee/', data, function (JsonResult) {
            debugger;
            if (JsonResult != '') {
                switch (JsonResult.Result) {
                    case "OK":
                        notyAlert('success', "Success");
                        BindEmployeeDropDown($("#EmpTypeCode").val());
                        $('#EmpID').val(JsonResult.Records.ID);
                        $("#EmployeeDiv").hide();

                        break;
                    case "ERROR":
                        notyAlert('error', JsonResult.Message.Message);
                        break;
                    default:
                        break;
                }
            }
        })
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
            $("#btnAddEmployee").css("pointer-events", "auto");
          
        }
        else
        {
            $("#EmpTypeCode").val('');
            $('#EmpID').empty();
            $('#EmpID').append(new Option('-- Select Employee --'));
            //$('#EmpID').val("-1");
            $("#EmpTypeCode").prop('disabled', true);
            $("#EmpID").prop('disabled', true);
            $("#btnAddEmployee").css("pointer-events", "none");
            $("#EmployeeDiv").hide();
        }
        
    }
    //$('span[data-valmsg-for="EmpTypeCode"]').empty();
    //$('span[data-valmsg-for="EmpID"]').empty();
    if(AcodeCombined=="")
    {
        $("#EmpTypeCode").val('');
        $('#EmpID').empty();
        $('#EmpID').append(new Option('-- Select Employee --'));
        $("#EmpTypeCode").prop('disabled', true);
        $("#EmpID").prop('disabled', true);
    }
}

function EmployeeTypeOnchange(curobj)
{
    
    var emptypeselected = $(curobj).val();
    if(emptypeselected)
    {
        BindEmployeeDropDown(emptypeselected);
        if ($("#EmpTypeCode").val() != "") $("#sbtyp").html($("#EmpTypeCode option:selected").text());
    }
}

function SelectEmployeeCompanyOnchange(curObj)
{
    try {
        debugger;
        if (curObj.value != "-1") {
            var ID = curObj.value;
            var OtherExpenseViewModel = GetEmployeesCompany(ID);
            debugger;

            $('#CompanyCode').val(OtherExpenseViewModel[0].companies.Code);
        }
    }
    catch (e) {

    }
}
function companyChange(curobj) {
    var emptypeselected = $(curobj).val();
    if (emptypeselected) {
      
        if ($("#CompanyCode").val() != "") $("#cmpny").html($("#CompanyCode option:selected").text());
    }
}

function BindEmployeeDropDown(type)
{
    debugger;
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
        debugger;
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

function GetEmployeesCompany(ID) {
    try {
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("OtherExpenses/GetEmployeeCompanyDetails/", data);
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

















