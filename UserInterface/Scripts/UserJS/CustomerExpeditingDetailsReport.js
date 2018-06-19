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
                                  columns: [0, 1, 2, 3, 4, 5,8,9,10]
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

               { "data": "CustomerName", "defaultContent": "<i></i>", "width": "7%" },
               { "data": "customerContactObj.ContactName", "defaultContent": "<i></i>", "width": "7%" },
               { "data": "ContactNo", "defaultContent": "<i>-</i>" },
               { "data": "companyObj.Name", "defaultContent": "<i>-<i>", "width": "7%" },
               { "data": "InvoiceNo", "defaultContent": "<i>-</i>", "width": "7%" },
               { "data": "InvoiceDate", "defaultContent": "<i>-</i>", "width": "8%" },
                  { "data": "CustomerName1", "defaultContent": "<i></i>", "width": "5%" },
               { "data": "Remarks", "defaultContent": "<i></i>", "width": "5%" },
             { "data": "PaymentDueDate", "defaultContent": "<i></i>", "width": "8%" },
             { "data": "NoOfDays", "defaultContent": "<i>-</i>", "width": "7%" },
             { "data": "Amount", "defaultContent": "<i>-</i>", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>", "width": "5%" }

             ],
             columnDefs: [{ "targets": [7], "visible": false, "searchable": false },
                 { "targets": [6], "visible": false },
                  { className: "text-left", "targets": [0,1,2, 3, 4] },
                  { "width": "12%", "targets": [2] },
             { className: "text-right", "targets": [4, 10] },
             { className: "text-center", "targets": [5,8,9] }

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
        var invoicetype = $("#ddlInvoiceTypes").val();
        if (IsVaildDateFormat(toDate))
            var data = { "ToDate": toDate, "Filter": filter, "Company": company, "Customer": customer,"InvoiceType":invoicetype };        
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
    $("#Company").val('ALL').trigger('change');
    $("#Customer").val('').trigger('change');
    $("#ddlInvoiceTypes").val('RB');
    RefreshCustomerExpeditingDetailTable();
}