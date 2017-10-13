var DataTables = {};
$(document).ready(function () {
    try {
        $("#CompanyCode").select2({
        });
        
        DataTables.saleSummaryReportTable = $('#saleSummaryTable').DataTable(
         {
            
             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [7,0,1,2, 3,4,5,6]
                              }
             }],
             order: [],
             "ordering": false,
             searching: false,
             paging: true,
             data: GetSaleSummary(),
             pageLength: 50,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search"
             //},
             columns: [
             
               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "OpeningBalance",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Invoiced",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "Paid", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                 { "data": "Credit", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "Balance", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "NetDue", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
             
               
              { "data": "OriginCompany", "defaultContent": "<i>-</i>" },

             ],
             columnDefs: [{ "targets": [7], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [0] },
                  { className: "text-right", "targets": [1, 2, 3, 4, 5, 6] }],
             
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(7, { page: 'current' }).data().each(function (group, i) {
                     debugger;
                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>Company</b> : ' + group + '</td></tr>');
                         last = group;
                     }
                 });
             }
         });
       
      
        $(".buttons-excel").hide();
        $('input[name="GroupSelect"]').on('change', function () {
            RefreshSaleSummaryTable();
        });

       
    } catch (x) {

        notyAlert('error', x.message);

    }

});


function GetSaleSummary() {
    try {
        debugger;
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var search = $("#Search").val();
        var internal;
        $('#IncludeInternal').attr('checked', false);
        $('#IncludeTax').attr('checked', true);
        if ($('#IncludeInternal').prop("checked") == true) {
            internal = true;
        }
        else {
            internal = false;
        }
        var tax;
        if ($('#IncludeTax').prop("checked") == true) {
            tax = true;
        }
        else {
            tax = false;
        }
        if (companycode === "ALL")
        {
            if ($("#all").prop('checked')) {
                companycode = $("#all").val();
            }
            else {
                companycode = $("#companywise").val();
            }
        }        
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "search": search, "IsInternal": internal, "IsTax": tax };
            var ds = {};
            ds = GetDataFromServer("Report/GetSaleSummary/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.TotalAmount != '') {
                $("#salessummaryamount").text(ds.TotalAmount);
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


function RefreshSaleSummaryTable() {
    try {
        debugger;
       
        var IsInternalCompany = $('#IncludeInternal').prop('checked');
        var IsTax = $('#IncludeTax').prop('checked');
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        if (companycode === "")
        {
            return false;
        }
            
        if (companycode === "ALL") {
            $("#all").prop("disabled", false);
            $("#companywise").prop("disabled", false);
        }
        else {
            $("#all").prop("disabled", true);
            $("#companywise").prop("disabled", true);
         
        }
        if (DataTables.saleSummaryReportTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            DataTables.saleSummaryReportTable.clear().rows.add(GetSaleSummary()).draw(false);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function PrintReport() {
    debugger;
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

function Reset() {
    debugger;

    $("#CompanyCode").val('ALL').trigger('change');
    $("#Search").val('');
    $("#all").prop('checked',true).trigger('change');
}


function RemoveDatatableOrder() {
    debugger;

    DataTables.saleSummaryReportTable.dataTable({
        ordering: false
    });
   DataTables.saleSummaryReportTable.clear().rows.add(GetSaleSummary()).draw(true);
}

function OnChangeCall() {
    debugger;
    RefreshSaleSummaryTable();

}