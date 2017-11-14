var DataTables = {};
var startdate = '';
var enddate = '';
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {
        $("#CompanyCode,#CustomerCode").select2({
        });

        DataTables.saleDetailReportTable = $('#saleDetailTable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5, 6, 7, 8,10,11,12,13]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: false,
             paging: true,
             data: GetSaleDetail(),
             pageLength: 50,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search"
             //},
             columns: [

               { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },
                { "data": "CustomerName", "defaultContent": "<i>-</i>" },
                { "data": "Date", "defaultContent": "<i>-</i>","width": "10%" },
                  { "data": "PaymentDueDate", "defaultContent": "<i>-</i>", "width": "10%" },
               
               { "data": "InvoiceAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                 { "data": "TaxAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                   { "data": "Total", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "PaidAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
               { "data": "BalanceDue", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
              { "data": "Credit", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                { "data": "GeneralNotes", "defaultContent": "<i></i>" },
             { "data": "OriginCompany", "defaultContent": "<i>-</i>" },
             { "data": "Origin", "defaultContent": "<i>-</i>" }

             ],
             columnDefs: [{ "targets": [9,11], "visible": false, "searchable": false },

                  { className: "text-left", "targets": [0,1,10] },
                   { className: "text-center", "targets": [2,3] },
                  { className: "text-right", "targets": [4, 5, 6, 8, 7, 9] }],
             createdRow: function (row, data, index) {
                 if (data.InvoiceNo == "<b>GrantTotal</b>") {

                     $('td', row).addClass('totalRow');
                 }
             },
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;

                 api.column(11, { page: 'current' }).data().each(function (group, i) {
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
        //$('input[name="GroupSelect"]').on('change', function () {
        //    RefreshSaleDetailTable();
        //});
        $("#saledetailtotal").attr('style', 'visibility:true');
    } catch (x) {

        notyAlert('error', x.message);

    }

});


function GetSaleDetail() {
    try {
        debugger
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        var search = $("#Search").val();
        var invoicetype = $("#ddlInvoiceTypes").val();
        $('#IncludeInternal').attr('checked', false);
        $('#IncludeTax').attr('checked', true);
        var customer = $("#CustomerCode").val();
        if (customer =='ALL')
        {
            customer = emptyGUID;
        }
        else
        {
            customer = $("#CustomerCode").val();
        }
        var internal;
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
        //if (companycode === "ALL") {
        //    if ($("#all").prop('checked')) {
        //        companycode = $("#all").val();
        //    }
        //    else {
        //        companycode = $("#companywise").val();
        //    }
        //}
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && companycode) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CompanyCode": companycode, "search": search, "IsInternal": internal, "IsTax": tax, "Customer": customer ,"InvoiceType":invoicetype};
            var ds = {};
            ds = GetDataFromServer("Report/GetSaleDetail/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.TotalAmount != '') {
                $("#salesdetailsamount").text(ds.TotalAmount);
            }
            if (ds.InvoicedAmount != '') {
                $("#salesdetailsinvoiceamount").text(ds.InvoicedAmount);
            }
            if (ds.PaidAmount != '') {
                $("#salesdetailspaidamount").text(ds.PaidAmount);
            }
            if (ds.TaxAmount != '') {
                $("#salesdetailtax").text(ds.TaxAmount);
            }
            if (ds.TotalInvoiced != '') {
                $("#salesdetailInvoiced").text(ds.TotalInvoiced);
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


function RefreshSaleDetailTable() {
    try
    {
        debugger;
        var IsInternalCompany = $('#IncludeInternal').prop('checked');
        var IsTax = $('#IncludeTax').prop('checked');
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var companycode = $("#CompanyCode").val();
        //if (companycode === "") {
        //    return false;
        //}
        //if (companycode === "ALL") {
        //    $("#all").prop("disabled", false);
        //    $("#companywise").prop("disabled", false);
        //}
        //else {
        //    $("#all").prop("disabled", true);
        //    $("#companywise").prop("disabled", true);
        //}
        if (DataTables.saleDetailReportTable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate)) {
            DataTables.saleDetailReportTable.clear().rows.add(GetSaleDetail()).draw(true);
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


function Reset() {
    debugger;
    $("#todate").val(startdate);
    $("#fromdate").val(enddate);
    $("#CompanyCode").val('ALL').trigger('change');
    $("#Search").val('').trigger('change');
    $("#CustomerCode").val('ALL').trigger('change');
    $("#ddlInvoiceTypes").val('');//invoicetype reset
    RefreshSaleDetailTable();

    //$("#all").prop('checked', true).trigger('change');
}

