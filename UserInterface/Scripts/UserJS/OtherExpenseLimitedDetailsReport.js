var DataTables = {};
var startDate = '';
var endDate = '';
$(document).ready(function () {
    try {
        $("#OtherExpenseLEAdvSearch_AccountHead,#OtherExpenseLEAdvSearch_EmployeeOrOther").select2({
        });
        DataTables.OtherExpenseLimitedDetailReportTable=$('#OtherExpenseLimitedTable').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10]
                                 }
                }],
                order: [],
                fixedHeader: {
                    header: true
                },
                searching: false,
                "lengthChange": false,
                fixedHeader: {
                    header: true
                },
                data: GetOtherExpenseLimitedDetailReport(),
               
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
                  { "data": "Amount", "defaultContent": "<i>-</i>" },
                   { "data": "ReversedAmount", "defaultContent": "<i>-</i>" },
                  { "data": "OriginCompany", "defaultContent": "<i>-</i>" }
                ],
                columnDefs: [{ "targets": [11], "visible": false },
                { className: "text-left", "targets": [0, 1, 3, 4, 5,6,7,8] },
                { "width": "15%", "targets": [0] },
                { "width": "10%", "targets": [2] },
                { className: "text-right", "targets": [9,10] },
                { className: "text-center", "targets": [2] }],
               
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(11, { page: 'current' }).data().each(function (group, i) {

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
            startDate = $("#todate").val();
            endDate = $("#fromdate").val();
            $("#otherexpenselimiteddetailtotalreversed").attr('style', 'visibility:true');
    } catch (x) {

        notyAlert('error', x.message);
    }
});

function GetOtherExpenseLimitedDetailReport(OtherExpenseLimitedExpenseAdvanceSearch) {
    debugger;
    try {
        if (OtherExpenseLimitedExpenseAdvanceSearch === 0) {
            var data = {};
        }
        else {
            var data = { "otherExpenseLimitedDetailsAdvanceSearchObject": JSON.stringify(OtherExpenseLimitedExpenseAdvanceSearch) };
        }
            var data;
            var ds = {};
            ds = GetDataFromServer("Report/GetOtherExpenseLimitedDetailReport/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.TotalAmount != '') {
                $("#otherexpenselimitedamount").text(ds.TotalAmount);
            }
            if (ds.ReversedTotal != '') {
                $("#otherexpenselimitedreversed").text(ds.ReversedTotal);
            }
            if (ds.Total != '') {
                $("#otherexpenselimitedreversedtotaldetail").text(ds.Total);
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

function AdvanceSearchContent() {
    debugger;
    var expenseType = $("#ExpenseType");
    var fromDate = $("#fromdate");
    var toDate = $("#todate");
    var company = $("#OtherExpenseLEAdvSearch_Company");
    var accountHead = $("#OtherExpenseLEAdvSearch_AccountHead");
    var subType = $("#OtherExpenseLEAdvSearch_SubType");
    var employeeOrOther = $("#OtherExpenseLEAdvSearch_EmployeeOrOther");
    var employeeCompany = $("#OtherExpenseLEAdvSearch_EmpCompany");
    var search = $("#search");
    var OtherExpenseLimitedExpenseAdvanceSearch = new Object();
    OtherExpenseLimitedExpenseAdvanceSearch.ExpenseType = expenseType[0].value !== "" ? expenseType[0].value : null;
    OtherExpenseLimitedExpenseAdvanceSearch.FromDate = fromDate[0].value !== "" ? fromDate[0].value : null;
    OtherExpenseLimitedExpenseAdvanceSearch.ToDate = toDate[0].value !== "" ? toDate[0].value : null;
    OtherExpenseLimitedExpenseAdvanceSearch.Company = company[0].value !== "" ? company[0].value : null;
    OtherExpenseLimitedExpenseAdvanceSearch.AccountHead = accountHead[0].value !== "" ? accountHead[0].value : null;
    OtherExpenseLimitedExpenseAdvanceSearch.SubType = subType[0].value !== "" ? subType[0].value : null;
    OtherExpenseLimitedExpenseAdvanceSearch.EmployeeOrOther = employeeOrOther[0].value !== "" ? employeeOrOther[0].value : null;
    OtherExpenseLimitedExpenseAdvanceSearch.EmpCompany = employeeCompany[0].value !== "" ? employeeCompany[0].value : null;
    OtherExpenseLimitedExpenseAdvanceSearch.Search = search[0].value !== "" ? search[0].value : null;
  
    DataTables.OtherExpenseLimitedDetailReportTable.clear().rows.add(GetOtherExpenseLimitedDetailReport(OtherExpenseLimitedExpenseAdvanceSearch)).draw(false);
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



function EmployeeTypeOnchange(curobj) {
    debugger;
    var emptypeselected = $(curobj).val();
    if (emptypeselected) {
        BindEmployeeDropDown(emptypeselected);
    }
    AdvanceSearchContent();
}



function BindEmployeeDropDown(type) {
    debugger;
    try {
        var employees = GetAllEmployeesByType(type);
        if (employees) {
            $('#OtherExpenseLEAdvSearch_EmployeeOrOther').empty();
            $('#OtherExpenseLEAdvSearch_EmployeeOrOther').append(new Option('-- Select --', ''));
            for (var i = 0; i < employees.length; i++) {
                var opt = new Option(employees[i].Name, employees[i].ID);
                $('#OtherExpenseLEAdvSearch_EmployeeOrOther').append(opt);

            }
        }



    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


function GetAllEmployeesByType(type) {
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


function Reset() {
    debugger;
    $("#todate").val(startDate);
    $("#fromdate").val(endDate);
    $("#OtherExpenseLEAdvSearch_Company").val('ALL');
    $("#OtherExpenseLEAdvSearch_AccountHead").val('ALL').trigger('change');
    $("#OtherExpenseLEAdvSearch_SubType").val('');
    $("#OtherExpenseLEAdvSearch_EmployeeOrOther").val('').trigger('change')
    $("#search").val('');
    $("#OtherExpenseLEAdvSearch_EmpCompany").val('ALL');
    $("#ExpenseType").val('ALL').trigger('change')
    AdvanceSearchContent();
}
