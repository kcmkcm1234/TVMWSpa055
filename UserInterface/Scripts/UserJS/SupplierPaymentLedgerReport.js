var DataTables = {};
var startDate = '';
var endDate = '';
$(document).ready(function () {
    try {
        $("#supplierCode").select2({
            placeholder: "Select a Suppliers.."
        });

        DataTables.supplierpaymentledgertable = $('#supplierpaymentledgertable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 4, 5,6,7,8]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: false,
             paging: true,
             data: GetSupplierPaymentLedger('ALL'),
             pageLength: 50,

             columns: [
                { "data": "Date", "defaultContent": "<i>-</i>" },
               { "data": "Type", "width": "20%", "defaultContent": "<i>-</i>" },
               { "data": "Ref", "defaultContent": "<i>-</i>" },
                { "data": "ID", "defaultContent": "<i>-</i>" },
                 { "data": "SupplierName", "defaultContent": "<i>-</i>" },
               { "data": "Company", "defaultContent": "<i>-</i>" },
               {
                   "data": "Debit", render: function (data, type, row) {
                       
                           return roundoff(data, 1);
                       
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "Credit", render: function (data, type, row) {
                       return roundoff(data, 1);
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "Balance", render: function (data, type, row) {

                       return roundoff(data, 1);
                   }, "defaultContent": "<i>-</i>"
               },

             ],
             columnDefs: [{ "targets": [3,4], "visible": false, "searchable": false },
             { className: "text-left", "targets": [0, 2, 5, 6,7,8] },
              { "width": "15%", "targets": [0] },
               { "width": "10%", "targets": [1] },
             { className: "text-right", "targets": [] },
         { className: "text-center", "targets": [1] },
            { "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7, 8] }],
             createdRow: function (row, data, index) {
                 if (data.Type == "<b>Total</b>") {

                     $('td', row).addClass('totalRow');
                 }
             },
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(4, { page: 'current' }).data().each(function (group, i) {

                     if (last !== group) {
                         $(rows).eq(i).before('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>SupplierName</b> : ' + group + '</td></tr>');
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
        $("#btnSend").hide();
        startDate = $("#todate").val();
        endDate = $("#fromdate").val();
        $("#suppliernameddl").attr('style', 'visibility:true');

    } catch (x) {

        notyAlert('error', x.message);

    }


});


function GetSupplierPaymentLedger(cur) {
    try {
        debugger;
        var fromDate = $("#fromdate").val();
        var toDate = $("#todate").val();
        var supplierCode = (cur != "ALL" ? $("#supplierCode").val() : cur);
        var company = $("#companyCode").val();

        if (IsVaildDateFormat(fromDate) && IsVaildDateFormat(toDate) && supplierCode) {
            var data = { "FromDate": fromDate, "ToDate": toDate, "Suppliercode": supplierCode,"Company": company };
            var ds = {};
            ds = GetDataFromServerTraditional("Report/GetSupplierPaymentLedger/", data);
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


function RefreshSupplierPaymentLedgerTable() {
    debugger;
    try {
        var fromDate = $("#fromdate").val();
        var toDate = $("#todate").val();
        var supplierCode = $("#supplierCode").val();

        if (DataTables.supplierpaymentledgertable != undefined && IsVaildDateFormat(fromDate) && IsVaildDateFormat(toDate) && supplierCode) {
            DataTables.supplierpaymentledgertable.clear().rows.add(GetSupplierPaymentLedger()).draw(true);
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

function OnCallChange() {
    debugger;
    if ($("#supplierCode").val() == '') {
        DataTables.supplierpaymentledgertable.clear().rows.add(GetSupplierPaymentLedger('ALL')).draw(true);
    }
   
    RefreshSupplierPaymentLedgerTable();
}



function Reset() {
    $("#todate").val(startDate);
    $("#fromdate").val(endDate);
    $("#supplierCode").val('').trigger('change')
    $("#companyCode").val('ALL');
    DataTables.supplierpaymentledgertable.clear().rows.add(GetSupplierPaymentLedger('ALL')).draw(true);
}





function DownloadReport() {
    debugger;
    $('#btnSend').trigger('click');
}

//To download file in PDF
function GetHtmlData() {
    debugger;
    DrawTable({
        Action: "Report/GetSupplierPaymentLedger/",
        data: { "FromDate": $('#fromdate').val(), "ToDate": $('#todate').val(), "Suppliercode": $('#supplierCode').val(), "Company": $('#companyCode').val() },
        Exclude_column: ["SupplierID", "supplierList", "SupplierCode", "Ref", "pdfToolsObj", "CompanyCode", "CompanyList", "companiesList"],
        Header_column_style: {
            "Date":{"style": "width:110px;font-size:12px;border-bottom:2px solid grey;font-weight: 600;","custom_name":"Date"},
            "Type":{ "style": "font-size:12px;border-bottom:2px solid grey;width:110px;font-weight: 600;", "custom_name": "Type" },            
            "Company":{ "style": "width:110px;font-size:12px;border-bottom:2px solid grey;font-weight: 600;", "custom_name": "Company" },
            "SupplierName":{ "style": "width:110px;font-size:12px;border-bottom:2px solid grey;font-weight: 600;", "custom_name": "Supplier" },
            "Debit":{ "style": "width:150px;text-align: center;font-size:12px;border-bottom:2px solid grey;font-weight: 600;", "custom_name": "Debit" },
            "Credit":{ "style": "width:150px;text-align: center;font-size:12px;border-bottom:2px solid grey;font-weight: 600;", "custom_name": "Credit" },
            "Balance":{ "style": "width:150px;text-align: center;font-size:12px;border-bottom:2px solid grey;font-weight: 600;", "custom_name": "Balance" }
        },
        Row_color: { "Odd": "White", "Even": "white" },
        Body_Column_style: { "Date": "font-size:11px;font-weight: 100;width:110px;", "Type": "font-size:11px;font-weight: 100;width:150px;", "Ref": "font-size:11px;font-weight: 100;width:150px;", "SupplierName": "font-size:11px;font-weight: 100;width:110px;", "Company": "font-size:11px;font-weight: 100;", "Debit": "text-align:right;font-size:11px;font-weight: 100;width:150px;", "Credit": "text-align:right;font-size:11px;font-weight: 100;width:150px;", "Balance": "text-align:right;font-size:11px;font-weight: 100;width:150px;" }

    });
    $('.balanceRowColor').parent('tr').css('background-color', '#d98cd9');
    var bodyContent = $('#customtbl').html();
    var headerContent = $('#divHeader').html();
    $("#hdnContent").val(bodyContent);
    $('#hdnHeadContent').val("<h1></h1>");
}



