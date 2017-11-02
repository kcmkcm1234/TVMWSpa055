var DataTables = {};
var startdate = '';
var enddate = '';
$(document).ready(function () {
    
  
    $("#CompanyCode,#AccountCode,#Subtype,#Employee").select2({
       
    });
 
    //initSelection: function(element, callback){}
    $("#STContainer").hide();
    try {

        DataTables.otherExpenseDetailsReportAHTable = $('#otherExpenseDetailsAHTable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3,4,5,6,7,8]
                              }
             }],
             order: [],
             searching: false,
             paging: true,
             data: GetOtherExpenseDetailsReport(),
             pageLength: 50,
            
             columns: [
               { "data": "Company", "defaultContent": "<i>-</i>" },
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
             columnDefs: [{ "targets": [9], "visible": false, "searchable": false },
             { className: "text-left", "targets": [0, 2, 3, 4, 5,6,7] },
              { "width": "15%", "targets": [0] },
               { "width": "10%", "targets": [1] },
             { className: "text-right", "targets": [8] },
         { className: "text-center", "targets": [1] }],
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(9, { page: 'current' }).data().each(function (group, i) {
                   
                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="8" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
                         last = group;
                     }
                     //if (api.column(6, { page: 'current' }).data().count() === i)
                     //{
                     //    $(rows).eq(i).after('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>Sub Total</b> : ' + group + '</td></tr>');
                     //}
                    
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


function GetOtherExpenseDetailsReport() {
    try {
        //debugger;
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var orderby;
        var AccountHead = $("#AccountCode").val();
        var Subtype = $("#Subtype").val();
        var Employeeorother = $("#Employee").val();
        var Employeecompany = $("#EmpCompany").val();
        var search = $("#Search").val();
       
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "OrderBy": orderby, "accounthead": AccountHead, "subtype": Subtype, "employeeorother": Employeeorother,"employeecompany": Employeecompany, "search": search };
            var ds = {};
            ds = GetDataFromServer("Report/GetOtherExpenseDetails/", data);
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


function RefreshOtherExpenseDetailsAHTable() {
    try {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();

        if (DataTables.otherExpenseDetailsReportAHTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.otherExpenseDetailsReportAHTable.clear().rows.add(GetOtherExpenseDetailsReport()).draw(true);
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
    RefreshOtherExpenseDetailsAHTable();
   
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

function BindEmployeeDropDown(type) {
    //debugger;
    try {
        var employees = GetAllEmployeesByType(type);
        if (employees) {
            $('#Employee').empty();
            $('#Employee').append(new Option('-- Select --', ''));
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
       // debugger;
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
    RefreshOtherExpenseDetailsAHTable();
}

