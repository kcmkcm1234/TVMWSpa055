var DataTables = {};
var today = '';
$(document).ready(function () {
    try {
        $("#Company,#Customer").select2({

        });
       
        DataTables.CustomerExpeditingDetailTableReportTable = $('#CustomerExpeditingDetailTable').DataTable(
         {

             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5,6,7]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: true,
             ordering: false,
             paging: true,
             data: GetCustomerExpeditingDetail(),
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [

               { "data": "CustomerName", "defaultContent": "<i></i>" },
               { "data": "customerContactObj.ContactName", "defaultContent": "<i></i>" },
               { "data": "ContactNo", "defaultContent": "<i>-</i>" },
               { "data": "companyObj.Name", "defaultContent": "<i>-<i>" },
               { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },             
               { "data": "InvoiceDate", "defaultContent": "<i>-</i>" },
               { "data": "Amount", "defaultContent": "<i>-</i>" },
                 { "data": "NoOfDays", "defaultContent": "<i>-</i>" },
                  { "data": "CustomerName1", "defaultContent": "<i></i>" },
               { "data": "Remarks", "defaultContent": "<i></i>" }


             ],
             columnDefs: [{ "targets": [9], "visible": false, "searchable": false },
                 { "targets": [8], "visible": false },
                  { className: "text-left", "targets": [0,1,2, 3, 4] },
                  { "width": "15%", "targets": [0] },
             { className: "text-right", "targets": [4, 6] },
             { className: "text-center", "targets": [5,7] }

                 ]

         });

        $(".buttons-excel").hide();
     today = $("#todate").val();
    } catch (x) {

        notyAlert('error', x.message);

    }

});

//to bind values to report
function GetCustomerExpeditingDetail() {
    try {
        debugger;       
        var toDate = $("#todate").val();
        var filter = $("#BasicFilters").val();
        var company = $("#Company").val();
        var customer = $("#Customer").val();       
        if (IsVaildDateFormat(toDate))
            var data = { "ToDate": toDate, "Filter": filter, "Company": company, "Customer": customer };        
        var ds = {};
        ds = GetDataFromServer("Report/GetCustomerPaymentExpeditingDetails/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records.customerExpeditingDetailsList;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function OnCallChange() {
    debugger;   
    //if ($("#Customer").val() == '') {
    //    DataTables.customerpaymentledgertable.clear().rows.add(GetCustomerPaymentLedger('ALL')).draw(true);
    //}
    RefreshCustomerExpeditingDetailTable();
}

//To refresh report based on filter
function RefreshCustomerExpeditingDetailTable() {
    debugger;
    try {
        var toDate = $("#todate").val();
        var filter = $("#BasicFilters").val();
        var company = $("#Company").val();
        var customer = $("#Customer").val();   
            
        if (DataTables.CustomerExpeditingDetailTableReportTable != undefined && IsVaildDateFormat(toDate)) {
            DataTables.CustomerExpeditingDetailTableReportTable.clear().rows.add(GetCustomerExpeditingDetail()).draw(true);
        }        
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

//To trigger export button
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

//To reset report
function Reset()
{
    debugger;
    $("#todate").val(today);
    $("#BasicFilters").val('ALL');
    $("#Company").val('ALL').trigger('change')
    $("#Customer").val('').trigger('change')
    RefreshCustomerExpeditingDetailTable();
}