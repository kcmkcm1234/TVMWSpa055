
var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {      
    
        DataTables.OtherIncomeTable = $('#OtherIncomeTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetAllOtherIncome(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": null },
               { "data": "ID" },              
               { "data": "AccountCode", "defaultContent": "<i>-</i>" },
               { "data": "AccountDesc", "defaultContent": "<i>-</i>" },
               { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
               { "data": "IncomeDateFormatted", "defaultContent": "<i>-</i>" },
                 { "data": "Description", "defaultContent": "<i>-</i>" },
                  { "data": "IncomeRef", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" title="Edit OtherIncome" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
               { "data": null, "orderable": false, "defaultContent": '<a data-toggle="tp" data-placement="top" data-delay={"show":2000, "hide":3000} title="Delete OtherIncome" href="#" class="DeleteLink" onclick="Delete(this)"><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [1,2], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [8] },
                    { className: "text-left", "targets": [0,6,3,4,7] },
             { className: "text-center", "targets": [5,8] }

             ]
         });
        DataTables.OtherIncomeTable.on('order.dt search.dt', function () {
            DataTables.OtherIncomeTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
        $('#OtherIncomeTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });


     
        $("#BankCode").prop('disabled', true);

        debugger;
        if ($('#BindValue').val() != '') {
            dashboardBind($('#BindValue').val())
        }
    }
    catch (x) {

        notyAlert('error', x.message);
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
    debugger;
    ShowModal();
    ResetForm();
    FillOtherIncomeDetails(ID)
   
}

function BindOpeningBalance() {
    debugger;
    var OpeningDate = $("#IncomeDate").val();
    if (OpeningDate != "" && IsVaildDateFormat(OpeningDate)) {
        var items = GetOpeningBalance();
        if (items != undefined) {
            $('#OpeningDate').text('');
            $('#OpeningDate').append('<b>' + $("#IncomeDate").val() + '</b>')
            $('#OpeningBank').text('');
            $('#OpeningBank').append('<span><b> ' + items.OpeningBank + '</b></span>');
            $('#OpeningCash').text('');
            $('#OpeningCash').append('<span><b> ' + items.OpeningCash + '</b></span>');
            $('#OpeningNCBank').text('');
            $('#OpeningNCBank').append('<span><b> ' + items.OpeningNCBank + '</b></span>');
            $('#UndepositedCheque').text('');
            $('#UndepositedCheque').append('<span><b> ' + items.UndepositedCheque + '</b></span>');
        }
    }
}
function GetOpeningBalance() {
    try {
        debugger;
        var OpeningDate = $("#IncomeDate").val();
        if (OpeningDate != "" && IsVaildDateFormat(OpeningDate)) {
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
        var Date = $("#IncomeDate").val();
        if (Date != "" && IsVaildDateFormat(Date)) {
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
       



function IncomeDefaultDateOnchange()
{ 
    $("#IncomeDate").val("");
    var IncomeDefaultDate = $("#DefaultDate").val();
    if (DataTables.OtherIncomeTable != undefined) {
        BindAllOtherIncome("",IncomeDefaultDate)
    }
    else {
        GetAllOtherIncome();
    }
}
function IncomeDateOnchange()
{
    debugger;
    BindOpeningBalance();
    if (DataTables.OtherIncomeTable != undefined)
    {
        $("#DefaultDate").val("");
        var IncomeDate = $("#IncomeDate").val();
        BindAllOtherIncome(IncomeDate,"")
    }
    else
    {
        GetAllOtherIncome();
    }
   
}

//---------------------------------------Edit Other Income--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    ShowModal();
    ResetForm();
    var rowData = DataTables.OtherIncomeTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        FillOtherIncomeDetails(rowData.ID);
    }
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#OtherIncomeAddModal").validate();
    $('#OtherIncomeAddModal').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function Delete(currObj)
{
    var rowData = DataTables.OtherIncomeTable.row($(currObj).parents('tr')).data();
    if ((rowData != null) && (rowData.ID != null)) {
        var ID = rowData.ID;
        notyConfirm('Are you sure to delete?', 'DeleteOtherIncome("' + ID + '")', '', "Yes, delete it!");
    }
    
}

function DeleteOtherIncome(ID) {
    try {
        debugger;
        var id = ID;
        if (id != '' && id != null) {
            var data = { "ID": id };
            var ds = {};
            ds = GetDataFromServer("OtherIncome/DeleteOtherIncome/", data);
            debugger;
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message.Message);
                BindAllOtherIncome($("#IncomeDate").val(), $("#DefaultDate").val());
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

function PaymentModeOnchange(curObj)
{
    debugger;
    if (curObj.value == "ONLINE")
    {
        $("#BankCode").prop('disabled', false);
    }
    else
    {
        $("#BankCode").val("");
        $("#BankCode").prop('disabled', true);
    }
    if (curObj.value == "CHEQUE") {
        $("#ChequeDate").prop('disabled', false);
        $("#ReferenceBank").prop('disabled', false);
    }
    else {
        $("#ChequeDate").prop('disabled', true);
        $("#ReferenceBank").prop('disabled', true);
    }
    $('span[data-valmsg-for="BankCode"]').empty();
}


function ShowModal()
{
    $("#AddOtherIncomeModel").modal('show');
    ClearFields();
    $("#AddOrEditSpan").text("Add New");
    var IncomeDate = $("#Today").val();
    $("#IncomeDateModal").val(IncomeDate);
    $("#ChequeDate").prop('disabled', true);
    $("#ReferenceBank").prop('disabled', true);
}

function Validation() {
    debugger;
    
    var fl = true;
    var pm = $("#PaymentMode").val();
    if ((pm) && (pm == "ONLINE")) {
        if ($("#BankCode").val() == "") {
            fl = false;
            $('span[data-valmsg-for="BankCode"]').empty();
            $('span[data-valmsg-for="BankCode"]').append('<span for="EmpID" class="">BankCode required</span>')
        }
        else {
            $('span[data-valmsg-for="BankCode"]').empty();
        }

    }
    if ((pm) && (pm == "CHEQUE")) {
        if ($("#ChequeDate").val() == "") {
            fl = false;

            $('span[data-valmsg-for="ChequeDate"]').append('<span for="ChequeDate" class="">Cheque Date required</span>')
        }
        else {
            $('span[data-valmsg-for="ChequeDate"]').empty();
        }
    }
    return fl;
}

function SaveOtherIncome()
{
    debugger;
    try {
        validate();
        
        //$("#btnOtherIncomeSave").trigger('click');
       
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}





function validate() {
    debugger;
    var OtherIncomeViewModel = new Object();
    OtherIncomeViewModel.IncomeRef = $("#IncomeRef").val();
    OtherIncomeViewModel.ID = $("#ID").val();
    var data = "{'_otherincome': " + JSON.stringify(OtherIncomeViewModel) + "}";
    PostDataToServer("OtherIncome/Validate/", data, function (JsonResult) {
        debugger;
        if (JsonResult != '') {
            switch (JsonResult.Result) {
                case "OK":
                    if (JsonResult.Records.Status == 1)
                    {
                        notyConfirm(JsonResult.Records.Message, 'SaveValidatedData();', '', "Yes,Proceed!", 1);
                        return false;
                    }
                    else
                    {
                        SaveValidatedData();
                    }
                    break;
                case "ERROR":
                    notyAlert('error', JsonResult.Message);
                    break;
                default:
                    break;
            }
        }
    });
}



function SaveValidatedData() {
    debugger;
    $(".cancel").click();
    setTimeout(function () {
        $("#btnOtherIncomeSave").trigger('click');
    }, 1000);
}

function GetAllOtherIncome(IncomeDate,DefaultDate) {
    try {
        debugger;
        if (IncomeDate == undefined && DefaultDate == undefined)
        {
            DefaultDate = $("#DefaultDate").val();
        }
        var data = { "IncomeDate": IncomeDate,"DefaultDate":DefaultDate };
        var ds = {};
        ds = GetDataFromServer("OtherIncome/GetAllOtherIncome/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
            $("#TotalAmt").text("");
            $("#TotalAmt").text(ds.TotalAmt);
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


function BindAllOtherIncome(IncomeDate,DefaultDate) {
    try {
        debugger;
        DataTables.OtherIncomeTable.clear().rows.add(GetAllOtherIncome(IncomeDate, DefaultDate)).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function GetOtherIncomeDetailsByID(ID)
{
    try {

        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("OtherIncome/GetOtherIncomeDetails/", data);
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

function Reset()
{
    if ($("#ID").val() == "0") {
        ClearFields();
        $("#AddOrEditSpan").text("Add New");
    }
    else {
        FillOtherIncomeDetails($("#ID").val());
    }
    ResetForm();
    $('span[data-valmsg-for="BankCode"]').empty();
    $('span[data-valmsg-for="ChequeDate"]').empty();
}

function FillOtherIncomeDetails(ID)
{
    var thisItem = GetOtherIncomeDetailsByID(ID); //Binding Data
    //Hidden
    debugger;

    $("#ID").val(thisItem.ID);
    $("#IncomeDateModal").val(thisItem.IncomeDateFormatted);  
    $("#AccountCode").val(thisItem.AccountCode).trigger('change');
   // $("#AccountCode").val(thisItem.AccountCode);     
    $("#PaymentRcdComanyCode").val(thisItem.PaymentRcdComanyCode);
    $("#PaymentMode").val(thisItem.PaymentMode);
    $("#ReferenceBank").val(thisItem.ReferenceBank);
    $("#ChequeDate").val(thisItem.ChequeDate);
    $("#BankCode").val(thisItem.BankCode);
    $("#Amount").val(roundoff(thisItem.Amount));
    $("#Description").val(thisItem.Description);
    $("#IncomeRef").val(thisItem.IncomeRef);
    $("#DepWithdID").val(thisItem.DepWithdID);
    $("#creditdAmt").text(thisItem.creditAmountFormatted);
    $("#AddOrEditSpan").text("Edit");
    if (thisItem.EmpTypeCode) {
        BindEmployeeDropDown(thisItem.EmpTypeCode);
    }
    $("#EmpID").val(thisItem.employeeObj.ID);
    $("#EmployeeType").val(thisItem.EmpTypeCode);
    //$("#AccountCode").val(thisItem.chartOfAccountsObj.TypeDesc); 
  
    
    if (thisItem.PaymentMode != "CHEQUE") {
        $("#ChequeDate").prop('disabled', true);
        $("#ReferenceBank").prop('disabled', true);
    }
    else {
        $("#ChequeDate").prop('disabled', false);
        $("#ReferenceBank").prop('disabled', false);
    }

  
    if (thisItem.AccountCode) {
        var AcodeCombined = thisItem.AccountCode;
        var len = AcodeCombined.indexOf(':');
        var IsEmploy = AcodeCombined.substring(len + 1, (AcodeCombined.length));
        // console.log(str.substring(0, (len)));
        if (IsEmploy == "True") {
            $("#EmployeeType").prop('disabled', false);
            $("#EmpID").prop('disabled', false);
            $("#btnAddEmployee").css("pointer-events", "auto");
        }



        //else {
        //    $("#EmployeeType").val('');
        //    $("#EmpID").val('');
        //    $("#EmployeeType").prop('disabled', true);
        //    $("#EmpID").prop('disabled', true);
        //    $("#btnAddEmployee").css("pointer-events", "none");
               
        //}
    }
}


function SaveSuccess(data, status)
{
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($("#IncomeDate").val() == undefined || $("#IncomeDate").val() == "")
            {
                IncomeDefaultDateOnchange();
            }
            else
            {
                IncomeDateOnchange();
            }
            //IncomeDateOnchange();
            notyAlert('success', JsonResult.Message);
            $('#AddOtherIncomeModel').modal('hide');
            debugger;
            if ($("#ID").val() != "" && $("#ID").val() != "0") {
                FillOtherIncomeDetails($("#ID").val());
            }
            else {
                FillOtherIncomeDetails(JsonResult.Records.ID);
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

function ClearFields()
{
    $("#AccountCode").val("");
    $("#EmployeeType").val("");
    $("#EmpID").val("");
    $("#PaymentRcdComanyCode").val("");
    $("#PaymentMode").val("");
    $("#DepWithdID").val("");
    $("#BankCode").val("");
    $("#IncomeRef").val("");
    $("#Description").val("");
    $("#Amount").val("");
    $("#IncomeDateModal").val("");
    $("#ID").val("");
    $("#DepWithdID").val(emptyGUID);
    $("#ChequeDate").val('');
    $("#creditdAmt").text("₹ 0.00");

    //$("#EmpID").prop('disabled', true);
    //$("#EmployeeType").prop('disabled', true);
    //$('#EmpID').append(new Option('-- Select Employee --', -1));
    //$('#EmpID').val("-1");
    ResetForm();
}


function AccountCodeChange(curobj) {
    debugger;
    
    var AcodeCombined = $(curobj).val();
    if (AcodeCombined) {
        var len = AcodeCombined.indexOf(':');
        var IsEmploy = AcodeCombined.substring(len + 1, (AcodeCombined.length));
        // console.log(str.substring(0, (len)));
        if (IsEmploy == "True") {
            $("#EmployeeType").prop('disabled', false);
            $("#EmpID").prop('disabled', false);
            $("#btnAddEmployee").css("pointer-events", "auto");

        }
        else 
        {
            $("#EmployeeType").val('');
            $('#EmpID').empty();
            $('#EmpID').append(new Option('-- Select Employee --'));
            //$('#EmpID').val("-1");
            $("#EmployeeType").prop('disabled', true);
            $("#EmpID").prop('disabled', true);
            $("#btnAddEmployee").css("pointer-events", "none");
            $("#EmployeeDiv").hide();
        }

    }
   
    if (AcodeCombined == "") {
        $("#EmployeeType").val('');
        $('#EmpID').empty();
        $('#EmpID').append(new Option('-- Select Employee --'));
        $("#EmployeeType").prop('disabled', true);
        $("#EmpID").prop('disabled', true);
    }
   
}


function EmployeeTypeChange(curobj) {
    debugger;
    var emptypeselected = $(curobj).val();
    if (emptypeselected) {
        BindEmployeeDropDown(emptypeselected);
        if ($("#EmployeeType").val() != "") $("#sbtyp").html($("#EmployeeType option:selected").text());
    }
}


function SelectEmployeeChange(curobj)
{
    try {
        debugger;
       

        if (curobj.value != "-1") {
            var ID = curobj.value;
            var OtherIncomeViewModel = GetEmployeesCompany(ID);
            debugger;

            $('#CompanyCode').val(OtherIncomeViewModel[0].companies.Code);
        }
    }
    catch (e) {

    }
}


function BindEmployeeDropDown(type) {
    debugger;
    try {
        var employees = GetAllEmployeesByType(type);
        if (employees) {
            $('#EmpID').empty();
            $('#EmpID').append(new Option('-- Select Employee --', -1));
            for (var i = 0; i < employees.length; i++) {
                var opt = new Option(employees[i].Name, employees[i].ID);
                $('#EmpID').append(opt);

            }
        }



    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function GetAllEmployeesByType(type) {
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
        debugger;
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
