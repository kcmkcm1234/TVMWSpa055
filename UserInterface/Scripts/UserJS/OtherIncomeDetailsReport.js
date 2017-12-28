var DataTables = {};
var startdate = '';
var enddate = '';
$(document).ready(function () {


    $("#CompanyCode,#AccountCode,#Subtype,#Employee").select2({

    });
    $("#STContainer").hide();
    try {

        DataTables.otherIncomeDetailsReportAHTable = $('#otherIncomeDetailsAHTable').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5, 6]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: false,
             paging: true,
             data: GetOtherIncomeDetailsReport(),
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
         { className: "text-center", "targets": [1] }],
             createdRow: function (row, data, index) {
                 if (data.Company == "<b>GrantTotal</b>") {

                     $('td', row).addClass('totalRow');
                 }
             },
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(7, { page: 'current' }).data().each(function (group, i) {

                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="8" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
                         last = group;
                     }

                 });
             }
         });

        $(".buttons-excel").hide();
        startdate = $("#todate").val();
        enddate = $("#fromdate").val();

    } catch (x) {

        notyAlert('error', x.message);

    }


});


function GetOtherIncomeDetailsReport() {
    try {
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
            ds = GetDataFromServer("Report/GetOtherIncomeDetails/", data);
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


function RefreshOtherIncomeDetailsAHTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();

        if (DataTables.otherIncomeDetailsReportAHTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.otherIncomeDetailsReportAHTable.clear().rows.add(GetOtherIncomeDetailsReport()).draw(true);
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
    RefreshOtherIncomeDetailsAHTable();

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
    RefreshOtherIncomeDetailsAHTable();
}


function EmployeeTypeOnChange(curobj) {
    debugger;
    var emptypeselected = $(curobj).val();
    if (emptypeselected) {
        BindEmployeeDropDown(emptypeselected);
        //if ($("#Subtype").val() != "") $("#sbtyp").html($("#Subtype option:selected").text());
    }
    OnChangeCall();
}

function BindEmployeeDropDown(type) {
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


function AccountCodeOnchange(curobj) {
    //debugger;
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
