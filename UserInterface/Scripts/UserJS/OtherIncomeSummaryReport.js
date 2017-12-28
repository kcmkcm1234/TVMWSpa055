var DataTables = {};
var startdate = '';
var enddate = '';
$(document).ready(function () {
    debugger;
    $("#CompanyCode,#AccountCode,#Subtype,#Employee").select2({
    });
    $("#STContainer").hide();
    try {

        DataTables.otherIncomeSummaryReportAHTable = $('#otherIncomeSummaryAHTable').DataTable(
         {

             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 
                 exportOptions:
                              {
                                  columns: [0, 1,3]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: false,
             paging: true,
             data: GetIncomeSummaryReport(),
             pageLength: 50,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search"
             //},
             columns: [
               
               
             
               {"data": "AccountHeadORSubtype", "defaultContent": "<i>-</i>", render: function (data, type, row) { 
                 return '<a href="#" onclick="ViewOtherIncomeDetail(this);">' + data + ' </a>';
               }
               },
                { "data": "SubTypeDesc", "defaultContent": "<i>-</i>" },
                { "data": "OriginCompany", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             
             ],
             columnDefs: [{ "targets": [2], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [0,2] },
                  { className: "text-right", "targets": [3] },
                  { className: "text-center", "targets": [1] }
             ]
             ,drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(2, { page: 'current' }).data().each(function (group, i) {
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

        DataTables.otherIncomeDetailsReportAHTable = $('#otherIncomeDetailsAHTable').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">', 
             order: [],
             searching: false,
             paging: true,
             data:null,
             pageLength: 50,

             columns: [
               { "data": "Company", "defaultContent": "<i>-</i>" },
                { "data": "Date", "defaultContent": "<i>-</i>" },
               { "data": "AccountHead", "defaultContent": "<i>-</i>" },
               { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
               { "data": "PaymentReference", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "OriginCompany", "defaultContent": "<i>-</i>" }
             ],
             columnDefs: [{ "targets": [7], "visible": false, "searchable": false },
             { className: "text-left", "targets": [0, 2, 3, 4, 5] },
                { "width": "15%", "targets": [0] },
               { "width": "10%", "targets": [1] },
             { className: "text-right", "targets": [6] },
         { className: "text-center", "targets": [1] }] 
         });

    } catch (x) {
        notyAlert('error', x.message);
    }

});


function GetIncomeSummaryReport() {
    try {
        debugger;
        
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var AccountHead = $("#AccountCode").val();
        var Subtype = $("#Subtype").val();
        var Employeeorother = $("#Employee").val();
        var search = $("#Search").val();
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "accounthead": AccountHead, "subtype": Subtype, "employeeorother": Employeeorother, "search": search };
            var ds = {};
            ds = GetDataFromServer("Report/GetOtherIncomeSummary/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            debugger;
            if (ds.TotalAmount != '') {
                $("#otherincomeamount").text(ds.TotalAmount);
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


function RefreshOtherIncomeSummaryAHTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();

        if (DataTables.otherIncomeSummaryReportAHTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.otherIncomeSummaryReportAHTable.clear().rows.add(GetIncomeSummaryReport()).draw(true);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

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

function OnChangeCall() {
    RefreshOtherIncomeSummaryAHTable();

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
    RefreshOtherIncomeSummaryAHTable();
}


function ViewOtherIncomeDetail(row_obj) {
    debugger;
    var rowData = DataTables.otherIncomeSummaryReportAHTable.row($(row_obj).parents('tr')).data();

    openNav();
    DataTables.otherIncomeDetailsReportAHTable.clear().rows.add(GetIncomeDetailsReport(rowData)).draw(true);
}

function GetIncomeDetailsReport(rowData) {
    try {
        debugger;
        // $("#lblDetailsHead").text(rowData.AccountHeadORSubtype);

        if (rowData.SubTypeDesc != null)
            $("#lblDetailsHead").text(rowData.AccountHeadORSubtype + '-' + rowData.SubTypeDesc);
        else
            $("#lblDetailsHead").text(rowData.AccountHeadORSubtype);

        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var AccountHead = rowData.AccountHead;
        var Subtype = $("#Subtype").val();
        if (rowData.EmployeeID != '00000000-0000-0000-0000-000000000000')
            var Employeeorother = rowData.EmployeeID;
        else
            var Employeeorother = $("#Employee").val();
        //var Employeecompany = $("#EmpCompany").val();


        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "accounthead": AccountHead, "subtype": Subtype, "employeeorother": Employeeorother};
            var ds = {};
            ds = GetDataFromServer("Report/GetOtherIncomeDetailsReport/", data);
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


function BindEmployeeDropDown(type) {
    debugger;
    try {
        var employees = GetAllEmployeesByType(type);
        if (employees) {
            $('#Employee').empty();
            $('#Employee').append(new Option('-- Select--', ''));
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