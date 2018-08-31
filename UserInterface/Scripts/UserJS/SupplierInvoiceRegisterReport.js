var DataTables = {};
var today = '';
$(document).ready(function () {
    try {
        $("#Company").select2({
           
        });
        $("#Supplier").select2({
            multiple: true,
            placeholder: "Select a Supplier..",
        });

        DataTables.SupplierInvoiceRegisterReportTable = $('#SupplierInvoiceRegisterTable').DataTable(
         {

             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5, 6, 7, 9,10,11]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: false,
             ordering: false,
             paging: true,
             data: GetSupplierInvoiceRegister(),
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
                 {
                     "data": "SupplierName", render: function (data, type, row) {

                         if (data == null || data == '<b>Total</b>') {
                             return row.SupplierName
                         }
                         else {
                             if (row.ContactName == null) {
                                 return '<b>' + row.SupplierName + '</b>'
                             }
                             else {
                                 return '<b>' + row.SupplierName + '</b><br/>Contact Name :<b>' + row.ContactName + '</b>'//<br/>  Contact No:<b>' + row.ContactNo + '</b>'
                             }

                         }

                     }, "defaultContent": "<i></i>", "width": "15%"
                 },
               { "data": "companyObj.Name", "defaultContent": "<i>-<i>", "width": "15%" },
               { "data": "InvoiceNo", "defaultContent": "<i>-</i>", "width": "8%" },
               { "data": "InvoiceDate", "defaultContent": "<i>-</i>", "width": "10%" },
                { "data": "SupplierName1", "defaultContent": "<i>-</i>", "width": "2%" },

             { "data": "PaymentDueDate", "defaultContent": "<i>-</i>", "width": "10%" },
            { "data": "Cr", "defaultContent": "<i>-</i>", "width": "5%" },
             { "data": "Age", "defaultContent": "<i>-</i>", "width": "5%" },
             { "data": "OverDue", "defaultContent": "<i>-</i>", "width": "5%" },
             { "data": "InvoiceAmount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>", "width": "5%" },
           {
               "data": "PaidAmount", render: function (data, type, row)
               { return '<a href="#" class="actionLink" onclick="InvoiceDetail(this)">' + roundoff(data, 1) + '</a>'; }, "defaultContent": "<i>-</i>", "width": "5%"
           },

        { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>", "width": "5%" },
             { "data": "Type", "defaultContent": "<i>-</i>", "width": "5%" },
              { "data": "SupplierID", "defaultContent": "<i>-</i>" },
               { "data": "AdvAmt", "defaultContent": "<i>-</i>", "width": "5%" },
             { "data": "PaidAmt", "defaultContent": "<i>-</i>", "width": "5%" },
            
             ],
             createdRow: function (row, data, index) {
                 if (data.SupplierName == "<b>Total</b>") {
                     $('td', row).addClass('finalRow');
                 }
                 if (data.SupplierName == "<b>Advance</b>" ) {

                    $('td', row).addClass('totalRow');
                   //  $('td', row).remove();
                 }

             },
             columnDefs: [{ "searchable": false },
                { "targets": [4], "visible": false },
                 { "targets": [12], "visible": false },
                  { "targets": [13], "visible": false },
                   { "targets": [14], "visible": false },
                    { "targets": [15], "visible": false },
                  { className: "text-left", "targets": [0, 1, 2] },
                  { "width": "12%", "targets": [2] },
             { className: "text-right", "targets": [9, 10, 11] },
             { className: "text-center", "targets": [3, 5, 6, 7, 8] },

             ]
         });

        $(".buttons-excel").hide();
        today = $("#todate").val();
        RefreshSupplierInvoiceRegisterable();


        //------------------------Modal Popup PaidAmount Detail-------------------------------------//
        DataTables.PaidAmountDetail = $('#tblDetailPaidAmount').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: false,
            paging: false,
            pageLength: 5,
            data: null,
            columns: [
                 { "data": "SupplierID", "defaultContent": "<i>-</i>" },
                 { "data": "PaymentDate", "defaultContent": "<i>-</i>" },
                 { "data": "Type", "defaultContent": "<i>-</i>" },
                 { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
                 { "data": "Amount", "defaultContent": "<i>-</i>" }

            ],
            createdRow: function (row, data, index) {
                if (data.Type == "<b>Total</b>") {
                    $('td', row).addClass('totalRow');
                }
            },
            columnDefs: [
                { className: "text-right", "targets": [4] },
                { className: "text-center", "targets": [1, 2] },
                { className: "text-left", "targets": [3] },
                { "targets": [0], "visible": false, "searchable": false }]

        });

    }
    catch (x) {
        notyAlert('error', x.message);
    }
});

//Bind Values to Datatable
function GetSupplierInvoiceRegister(supplierPayementAdvanceSearchObj) {
    debugger;
    try {
        if (supplierPayementAdvanceSearchObj === 0) {
            var data = {};
        }
        else {
            var data = { "SupplierInvoiceRegisterSearchObject": JSON.stringify(supplierPayementAdvanceSearchObj) };
        }
        var ds = {};
        //var todate = $("#todate").val();
        //var filter = $("#BasicFilters").val();
        //var company = $("#Company").val();
        //var supplier = $("#Supplier").val();
        //if (IsVaildDateFormat(todate))
        //    var data = { "ToDate": todate, "Filter": filter, "Company":company, "Supplier":supplier };
        //var ds = {};
        ds = GetDataFromServer("Report/GetSupplierInvoiceRegister/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.InvAmt != '') {
            $("#Inv").text(ds.InvAmt);
        }
        if (ds.paidAmount != '') {
            $("#Paid").text(ds.paidAmount);
        }
        if (ds.advanceAmount != '') {
            $("#Adv").text(ds.advAmount);
        }
        if (ds.balAmount != '') {
            $("#Bal").text(ds.balAmount);
        }
        if (ds.Result == "OK") {
            return ds.Records.SupplierInvoiceRegisterList;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

//advanced filter
function RefreshSupplierInvoiceRegisterable() {
    debugger;
    try {
       // var toDate = $("#todate");
        var filter = $("#BasicFilters");
        var company = $("#Company");
      //  var supplier = $("#Supplier");
        var supplier = ($("#Supplier").val() != "" ? $("#Supplier").val() : null);
        var invoicetype = $("#ddlInvoiceTypes");
        var supplierAdvanceSearch = new Object();
        var search = $("#search").val();
        if ($("#summary").prop('checked')) {
            var reporttype = $("#summary").val();
        }
        else {
            var reporttype = $("#detail").val();
        }
        var IncludeInternal;
        //$('#IncludeInternal').attr('checked', false);

        if ($('#IncludeInternal').prop("checked") == true) {
            IncludeInternal = true;
        }
        else {
            IncludeInternal = false;
        }

       // supplierAdvanceSearch.ToDate = toDate[0].value !== "" ? toDate[0].value : null;
        supplierAdvanceSearch.Filter = filter[0].value !== "" ? filter[0].value : null;
        supplierAdvanceSearch.Company = company[0].value !== "" ? company[0].value : null;
        supplierAdvanceSearch.Supplier = supplier !== "" ? supplier : null;
        supplierAdvanceSearch.InvoiceType = invoicetype[0].value !== "" ? invoicetype[0].value : null;
        supplierAdvanceSearch.Search = search !== "" ? search : null;
        supplierAdvanceSearch.ReportType = reporttype !== "" ? reporttype : null;
        //if (DataTables.SupplierExpeditingDetailTableReportTable != undefined && IsVaildDateFormat(todate)) {
        //    DataTables.SupplierExpeditingDetailTableReportTable.clear().rows.add(GetSupplierExpeditingDetail()).draw(true);
        //}  
        supplierAdvanceSearch.IncludeInternal = IncludeInternal !== "" ? IncludeInternal : null;

        if (DataTables.SupplierInvoiceRegisterReportTable != undefined) {
            DataTables.SupplierInvoiceRegisterReportTable.clear().rows.add(GetSupplierInvoiceRegister(supplierAdvanceSearch)).draw(false);
        }
    }

    catch (e) {
        notyAlert('error', e.message);
    }
}

//to trigger export button
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
function Reset() {
    debugger;
    $("#todate").val(today);
    $("#BasicFilters").val('ALL');
    $("#Company").val('ALL').trigger('change')
    $("#Supplier").val('').trigger('change')
    $("#ddlInvoiceTypes").val('RB');
    RefreshSupplierInvoiceRegisterable();
}
function InvoiceDetail(currentObj) {
    debugger
    $('#InvoiceDetailModel').modal('show');//
    //var filter = $("#BasicFilters").val();
    //var company = $("#Company").val();;
    //var invoicetype = $("#ddlInvoiceTypes").val();
    var rowData = DataTables.SupplierInvoiceRegisterReportTable.row($(currentObj).parents('tr')).data();
    //$("#newBtn").attr("disabled", true);
    //$("#saveBtn").attr("disabled", true);
    //NewSpecialPayments()
    if ((rowData != null)) {
        DataTables.PaidAmountDetail.clear().rows.add(GetPaidAmountDetail(rowData.InvoiceID, rowData.SupplierID, rowData.RowType)).draw(false); //,filter,company,invoicetype)

    }
    //BindPaidAmountDetailTable();
}
function BindPaidAmountDetailTable() {
  
    DataTables.PaidAmountDetail.clear().rows.add(GetPaidAmountDetail(rowData.InvoiceID, rowData.SupplierID, rowData.RowType)).draw(false);//,filter,company,invoicetype

}
function GetPaidAmountDetail(InvoiceID, SupplierID, RowType) {
    try {
        debugger;
        var InvoiceID = InvoiceID;
        var SupplierID = SupplierID;
        var RowType = RowType;

        var filter = $("#BasicFilters").val();
        var company = $("#Company").val();;
        var invoicetype = $("#ddlInvoiceTypes").val();

        var data = { "InvoiceID": InvoiceID, "SupplierID": SupplierID, "RowType": RowType,"Filter" :filter,"Company":company,"InvoiceType" :invoicetype };
        var ds = {};
        ds = GetDataFromServer("Report/GetSupplierPaidAmountDetail/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}