var DataTables = {};
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
             searching: false,
             paging: true,
             data: GetExpenseSummaryReport(),
             pageLength: 50,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search"
             //},
             columns: [
               { "data": "OriginCompany", "defaultContent": "<i>-</i>" },
               { "data": "AccountHeadORSubtype", "defaultContent": "<i>-</i>" },
               { "data": "SubTypeDesc", "defaultContent": "<i>-</i>" },
               //{ "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             
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
        var reporttype = "";
        if ($("#headwise").prop('checked')) {
                reporttype = $("#headwise").val();
            }
            else {
                reporttype = $("#subtypewise").val();
            }
     
       
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode ) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode,"ReportType":reporttype,"OrderBy": orderby, "accounthead": AccountHead, "subtype": Subtype, "employeeorother": Employeeorother, "employeecompany": Employeecompany, "search": search };
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
            DataTables.otherExpenseSummaryReportAHTable.clear().rows.add(GetExpenseSummaryReport()).draw(false);
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

    $("#CompanyCode").val('ALL').trigger('change')
    $("#AccountCode").val('ALL').trigger('change')
    $("#Subtype").val('').trigger('change')
    $("#Employee").val('').trigger('change')
    $("#Search").val('').trigger('change')
    $("#EmpCompany").val('ALL').trigger('change')
    $("#headwise").prop('checked', true).trigger('change');
    RefreshOtherExpenseSummaryAHTable();
}