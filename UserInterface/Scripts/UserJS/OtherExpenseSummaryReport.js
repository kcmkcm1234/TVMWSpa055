var DataTables = {};
var startdate = '';
var enddate = '';
$(document).ready(function () {
    $("#CompanyCode,#AccountCode,#Subtype,#Employee").select2({
    });
  
    $("#STContainer").hide();
    try {

        DataTables.otherExpenseSummaryReportAHTable = $('#otherExpenseSummaryAHTable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             //dom: 'Bfrtip',
             buttons: [{
                 extend: 'excel',
                 
                 exportOptions:
                              {
                                  columns: [0, 1, 2,3]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: false,
             paging: true,
             data: GetExpenseSummaryReport(),
             pageLength: 50,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search"
             //},
             columns: [
               { "data": "OriginCompany", "defaultContent": "<i>-</i>"},
               { "data": "AccountHeadORSubtype", "defaultContent": "<i>-</i>", render: function (data, type, row) {
                       return '<a href="#" onclick="ViewOtherExpensesDetail(this);">' + data + ' </a>';
                   }
               },
               { "data": "SubTypeDesc", "defaultContent": "<i>-</i>" },
               //{ "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [1,2] },
                  { className: "text-right", "targets": [3] }],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(0, { page: 'current' }).data().each(function (group, i) {
                     debugger;
                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="3" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
                         last = group;
                     }
                 });
             }
         });

        $(".buttons-excel").hide();
        startdate = $("#todate").val();
        enddate = $("#fromdate").val();

        DataTables.otherExpenseDetailsReportAHTable = $('#otherExpenseDetailsAHTable').DataTable(
 {

     dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
     order: [],
     searching: false,
     paging: true,
     data: null,
     pageLength: 50,

     columns: [
       { "data": "Company", "defaultContent": "<i>-</i>" },
       { "data": "ExpenseType", "defaultContent": "<i>-</i>" },
        { "data": "Date", "defaultContent": "<i>-</i>" },
       { "data": "AccountHead", "defaultContent": "<i>-</i>" },
       { "data": "SubType", "defaultContent": "<i>-</i>" },
       { "data": "EmpCompany", "defaultContent": "<i>-</i>" },
       { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
       { "data": "PaymentReference", "defaultContent": "<i>-</i>" },
       { "data": "Description", "defaultContent": "<i>-</i>" },
       { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
       { "data": "OriginCompany", "defaultContent": "<i>-</i>" }
     ],
     columnDefs: [{ "targets": [10], "visible": false, "searchable": false },
     { className: "text-left", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8] },
      { "width": "15%", "targets": [0] },
       { "width": "10%", "targets": [2] },
     { className: "text-right", "targets": [9] },
 { className: "text-center", "targets": [2] }]
 });


        $('input[name="GroupSelect"]').on('change', function () {
            RefreshOtherExpenseSummaryAHTable();
        });

    } catch (x) {

        notyAlert('error', x.message);

    }

    //try
    //{
      //  DataTables.otherExpenseSummaryReportSTTable = $('#otherExpenseSummarySTTable').DataTable(
      //{

      //    // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
      //    dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
      //    buttons: [{
      //        extend: 'excel',
      //        exportOptions:
      //                     {
      //                         columns: [0, 1, 2,3]
      //                     }
      //    }],
      //    order: [],
      //    searching: false,
      //    paging: true,
      //    data: null,
      //    pageLength: 50,
      //    //language: {
      //    //    search: "_INPUT_",
      //    //    searchPlaceholder: "Search"
      //    //},
      //    columns: [
      //        { "data": "OriginCompany", "defaultContent": "<i>-</i>" },
      //      { "data": "SubTypeDesc", "defaultContent": "<i>-</i>" },
      //      { "data": "AccountHeadORSubtype", "defaultContent": "<i>-</i>" },
      //      { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
            
      //    ],
      //    columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
      //         { className: "text-left", "targets": [1,2] },
      //         { className: "text-right", "targets": [3] }],
      //        drawCallback: function (settings) {
      //        var api = this.api();
      //        var rows = api.rows({ page: 'current' }).nodes();
      //        var last = null;

      //        api.column(0, { page: 'current' }).data().each(function (group, i) {
      //            if (last !== group) {
      //                $(rows).eq(i).before('<tr class="group "><td colspan="3" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
      //                last = group;
      //            }
      //        });
      //    }
      //});
    //}
    //catch(e)
    //{
    //    notyAlert('error', e.message);
    //}

});


function GetExpenseSummaryReport() {
    try {
        debugger;
        
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        //var reporttype = $("#ReportType").val();
        var orderby;
        var AccountHead = $("#AccountCode").val();
        var Subtype = $("#Subtype").val();
        var Employeeorother = $("#Employee").val();
        var Employeecompany = $("#EmpCompany").val();
        var search = $("#Search").val();
        var ExpenseType = $("#ExpenseType").val();
        var reporttype = "";
        if ($("#headwise").prop('checked')) {
                reporttype = $("#headwise").val();
            }
            else {
                reporttype = $("#subtypewise").val();
            }
     
       
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode ) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "ReportType": reporttype, "OrderBy": orderby, "accounthead": AccountHead, "subtype": Subtype, "employeeorother": Employeeorother, "employeecompany": Employeecompany, "search": search, "ExpenseType": ExpenseType };
            var ds = {};
            ds = GetDataFromServer("Report/GetOtherExpenseSummary/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            debugger;
            if (ds.TotalAmount != '') {
                $("#otherexpenseamount").text(ds.TotalAmount);
            }


            if (ds.Result == "OK") {
                return ds.Records;
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
            }
        }



    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function RefreshOtherExpenseSummaryAHTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        //var AccountHead = $("#AccountCode").val();
        //var Subtype = $("#Subtype").val();
        //var Employeeorother = $("#Employee").val();
        //var search = $("#Search").val();
       
        if (DataTables.otherExpenseSummaryReportAHTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.otherExpenseSummaryReportAHTable.clear().rows.add(GetExpenseSummaryReport()).draw(true);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

//function RefreshOtherExpenseSummarySTTable() {
//    try {
//        var fromdate = $("#fromdate").val();
//        var todate = $("#todate").val();
//        var companycode = $("#CompanyCode").val();

//        if (DataTables.otherExpenseSummaryReportSTTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
//            DataTables.otherExpenseSummaryReportSTTable.clear().rows.add(GetExpenseSummaryReport()).draw(false);
//        }
//    }
//    catch (e) {
//        notyAlert('error', e.message);
//    }
//}

function PrintReport() {
    try {

        $(".buttons-excel").trigger('click');


    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Back() {
    window.location = appAddress + "Report/Index/";
}

function OnChangeCall()
{
    RefreshOtherExpenseSummaryAHTable();
   
}



function AccountCodeOnchange(curobj) {
    debugger;
   
    var AcodeCombined = $(curobj).val();
    if (AcodeCombined) {
        var len = AcodeCombined.indexOf(':');
        var IsEmploy = AcodeCombined.substring(len + 1, (AcodeCombined.length));
        if (IsEmploy == "True") {
            $("#Subtype").prop('disabled', false);
            $("#Employee").prop('disabled', false);
        }
        else {
            $("#Subtype").prop('disabled', true);
            $("#Employee").prop('disabled', true);
        }
    }
  
    if (AcodeCombined == "ALL") {
       
        $("#Subtype").prop('disabled', false);
        $("#Employee").prop('disabled', false);
    }
   
    OnChangeCall();
}

function BindEmployeeDropDown(type)
{
    debugger;
    try {
        var employees = GetAllEmployeesByType(type);
        if (employees) {
            $('#Employee').empty();
            $('#Employee').append(new Option('-- Select--',''));
            for (var i = 0; i < employees.length; i++) {
                var opt = new Option(employees[i].Name, employees[i].ID);
                $('#Employee').append(opt);

            }
        }



    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function EmployeeTypeOnchange(curobj) {
    debugger;
    var emptypeselected = $(curobj).val();
    if (emptypeselected) {
        BindEmployeeDropDown(emptypeselected);
       //if ($("#Subtype").val() != "") $("#sbtyp").html($("#Subtype option:selected").text());
    }   
    OnChangeCall();
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
function Reset() {
    debugger;
    $("#todate").val(startdate);
    $("#fromdate").val(enddate);
    $("#CompanyCode").val('ALL').trigger('change')
    $("#AccountCode").val('ALL').trigger('change')
    $("#Subtype").val('').trigger('change')
    $("#Employee").val('').trigger('change')
    $("#Search").val('').trigger('change')
    $("#EmpCompany").val('ALL').trigger('change')
    $("#headwise").prop('checked', true).trigger('change');
    $("#ExpenseType").val('ALL').trigger('change')
    RefreshOtherExpenseSummaryAHTable();
}



function ViewOtherExpensesDetail(row_obj) {
    debugger;
    var rowData = DataTables.otherExpenseSummaryReportAHTable.row($(row_obj).parents('tr')).data();

    openNav();
    DataTables.otherExpenseDetailsReportAHTable.clear().rows.add(GetOtherExpenseDetailsReport(rowData)).draw(true);
}

function GetOtherExpenseDetailsReport(rowData) {
    try {
        debugger
        if (rowData.SubTypeDesc!=null)
            $("#lblDetailsHead").text(rowData.AccountHeadORSubtype + '-' + rowData.SubTypeDesc);
        else
            $("#lblDetailsHead").text(rowData.AccountHeadORSubtype);
        if (rowData.AccountHeadORSubtype == "No_Head")
            var AccountHead = "NO_HEAD";
        else
            var AccountHead = rowData.AccountHead;
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var Subtype = $("#Subtype").val();
        if (rowData.EmployeeID != '00000000-0000-0000-0000-000000000000')
            var Employeeorother = rowData.EmployeeID;
        else
            var Employeeorother = $("#Employee").val();
        var Employeecompany = $("#EmpCompany").val();
        var ExpenseType = $("#ExpenseType").val();
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "accounthead": AccountHead, "subtype": Subtype, "employeeorother": Employeeorother, "employeecompany": Employeecompany, "ExpenseType": ExpenseType };
            var ds = {};
            ds = GetDataFromServer("Report/GetOtherExpenseDetailsReport/", data);
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
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function ExpenseTypeOnChange(curobj) {
    debugger;
    var ExpenseTypeSelected= $(curobj).val();
    if (ExpenseTypeSelected) {
        BindAccountsDropDown(ExpenseTypeSelected);
    }   
    OnChangeCall();
}

function BindAccountsDropDown(type) {
    debugger;
    try {
        var accounts = GetAllAccountsByType(type);
        if (accounts) {
            $('#AccountCode').empty();
            //$('#AccountCode').append(new Option('-- Select--', ''));
            for (var i = 0; i < accounts.length; i++) {
                var opt = new Option(accounts[i].Text, accounts[i].Value);
                $('#AccountCode').append(opt);

            }
        }
    }
    catch (ex) {
        notyAlert('error', ex.message);
    }
}

function GetAllAccountsByType(type) {
    try {
        debugger;
        var data = { "Type": type };
        var ds = {};
        ds = GetDataFromServer("Report/GetAllAccountTypes/", data);
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
    catch (ex) {
        notyAlert('error', ex.message);
    }
}