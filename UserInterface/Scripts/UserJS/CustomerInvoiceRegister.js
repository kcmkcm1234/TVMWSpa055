var DataTables = {};
var today = '';
$(document).ready(function () {
    try {
        $("#CustomerID").select2({
            multiple: true,
            placeholder: "Select a Customers..",
        });

        DataTables.CustomerInvoiceRegisterReportTable = $('#CustomerInvoiceRegisterTable').DataTable(
         {

             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 5,6,7, 8, 9, 10,11]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: false,
             ordering: false,
             paging: true,
             data: GetCustomerInvoiceRegister(),
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
                 {
                     "data": "CustomerName", render: function (data, type, row) {
                         
                         if (data == null || data == '<b>Total</b>')
                         {
                             return row.CustomerName
                         }                       
                         else
                         {
                             if (row.customerContactObj.ContactName == null)
                             {
                                 return '<b>' + row.CustomerName + '</b>'
                             }
                             else
                             {
                                 return '<b>' + row.CustomerName + '</b><br/>Contact Name :<b>' + row.customerContactObj.ContactName + '</b>'//<br/>  Contact No:<b>' + row.ContactNo + '</b>'
                             }
                            
                         }
                        
                     }, "defaultContent": "<i></i>", "width": "15%"
                 },      
               { "data": "companyObj.Name", "defaultContent": "<i>-<i>", "width": "15%" },
               { "data": "InvoiceNo", "defaultContent": "<i>-</i>", "width": "8%" },
               { "data": "InvoiceDate", "defaultContent": "<i>-</i>", "width": "10%" },
                { "data": "CustomerName1", "defaultContent": "<i>-</i>", "width": "2%" },
             
             { "data": "PaymentDueDate", "defaultContent": "<i>-</i>", "width": "10%" },
            { "data": "Cr", "defaultContent": "<i>-</i>", "width": "5%" },
             { "data": "Age", "defaultContent": "<i>-</i>", "width": "5%" },
             { "data": "OverDue", "defaultContent": "<i>-</i>", "width": "5%" },
             { "data": "InvoiceAmount",  render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>", "width": "5%" },


        {
               "data": "PaidAmount", render: function (data, type, row)
                  
               { return '<a href="#" class="actionLink" onclick="InvoiceDetail(this)">' + roundoff(data, 1) + '</a>'; }, "defaultContent": "<i>-</i>", "width": "5%"
           },
           
        { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>", "width": "5%" },
             { "data": "Type", "defaultContent": "<i>-</i>", "width": "5%" },
             { "data": "AdvAmt", "defaultContent": "<i>-</i>", "width": "5%" },
             { "data": "PaidAmt", "defaultContent": "<i>-</i>", "width": "5%" },
             ],
            
             createdRow: function (row, data, index) {
                 if (data.CustomerName == "<b>Total</b>") {
                     $('td', row).addClass('finalRow');
                 }
                 if (data.CustomerName == "<b>Advance</b>") {
                    
                     $('td', row).addClass('totalRow');
                 }

             },
             columnDefs: [{ "searchable": false },
                { "targets": [4], "visible": false },
                 { "targets": [12], "visible": false },
                  { "targets": [13], "visible": false },
                   { "targets": [14], "visible": false },
                  { className: "text-left", "targets": [0, 1, 2] },
                  { "width": "12%", "targets": [2] },
             { className: "text-right", "targets": [9,10,11] },
             { className: "text-center", "targets": [3,5,6,7,8] },

             ]

         });

        $(".buttons-excel").hide();
        today = $("#todate").val();
        AdvanceSearchContent();


        //------------------------Modal Popup PaidAmount Detail-------------------------------------//
        DataTables.PaidAmountDetail = $('#tblDetailPaidAmount').DataTable({
            dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: false,
            paging: false,
            pageLength: 5,
            data: null,
            columns: [
                 { "data": "CustomerID", "defaultContent": "<i>-</i>" },
                 { "data": "PaymentDate", "defaultContent": "<i>-</i>" },
                 { "data": "Type", "defaultContent": "<i>-</i>"},
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
                { className: "text-left", "targets": [ 3] },
                { "targets": [0], "visible": false, "searchable": false }]

        });



    } catch (x) {

        notyAlert('error', x.message);

    }

});

//to bind values to report
//function GetCustomerInvoiceRegister(cur) {
//    try {
//        debugger;
//      //  var toDate = $("#todate").val();
//        var filter = $("#BasicFilters").val();
//        var company = $("#Company").val();
//        var customer = (cur != "ALL" ? $("#CustomerID").val() : "ALL");
//        var invoicetype = $("#ddlInvoiceTypes").val();
//        var search = $("#Search").val();
//        //if (IsVaildDateFormat(toDate)  )
//            var data = { "Filter": filter, "Company": company, "Customer": customer, "InvoiceType": invoicetype,"Search":search };
//        var ds = {};
//        ds = GetDataFromServerTraditional("Report/GetCustomerInvoiceRegister/", data);
//        if (ds != '') {
//            ds = JSON.parse(ds);
//        }
//        if (ds.Result == "OK") {
//            return ds.Records.CustomerInvoiceRegisterList;
//        }
//        if (ds.Result == "ERROR") {
//            notyAlert('error', ds.Message);
//        }
//    }
//    catch (e) {
//        notyAlert('error', e.message);
//    }
//}


function GetCustomerInvoiceRegister(CustomerInvoiceRegisterAdvanceSearch) {
    debugger;

    try {
        if (CustomerInvoiceRegisterAdvanceSearch === 0) {
            var data = {};
        }
        else {
            var data = { "CustomerInvoiceRegisterAdvanceSearchSearchObject": JSON.stringify(CustomerInvoiceRegisterAdvanceSearch) };
        }
        var data;
        var ds = {};
        ds = GetDataFromServerTraditional("Report/GetCustomerInvoiceRegister/", data);
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
            return ds.Records.CustomerInvoiceRegisterList;
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
    if ($("#Customer").val() == '') {
        DataTables.CustomerInvoiceRegisterReportTable.clear().rows.add(GetCustomerInvoiceRegister('ALL')).draw(true);
    }
    RefreshCustomerInvoiceRegisterTable();
   // AdvanceSearchContent()
}


//To refresh report based on filter
function RefreshCustomerInvoiceRegisterTable() {
    debugger;
    try {
        var toDate = $("#todate").val();
        var filter = $("#BasicFilters").val();
        var company = $("#Company").val();
        var customer = $("#CustomerID").val();

        if (DataTables.CustomerInvoiceRegisterReportTable != undefined && IsVaildDateFormat(toDate)) {
            DataTables.CustomerInvoiceRegisterReportTable.clear().rows.add(GetCustomerInvoiceRegister()).draw(true);
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
function Reset() {
    debugger;
    $("#todate").val(today);
    $("#BasicFilters").val('ALL');
    $("#Company").val('ALL').trigger('change');
    $("#CustomerID").val('').trigger('change');
    $("#ddlInvoiceTypes").val('RB');
    //RefreshCustomerInvoiceRegisterTable();
    AdvanceSearchContent();
}

//function AdvanceSearchContent() {
//    debugger;

//  //  var toDate = $("#todate").val();
//    var filter = $("#BasicFilters").val();
//    var company = $("#Company").val();
//    var customer = ($("#CustomerID").val() !=""? $("#CustomerID").val():"ALL");
//    var invoiceType = $("#ddlInvoiceTypes").val();
//    var search = $("#Search");
//    var CustomerInvoiceRegisterAdvanceSearch = new Object();
//  //  CustomerInvoiceRegisterAdvanceSearch.ToDate = toDate[0].value !== "" ? toDate[0].value : null;
//    CustomerInvoiceRegisterAdvanceSearch.Filter = filter[0].value !== "" ? filter[0].value : null;
//    CustomerInvoiceRegisterAdvanceSearch.Customer = customer[0].value !== "" ? customer[0].value : null;
//    CustomerInvoiceRegisterAdvanceSearch.Company = company[0].value !== "" ? company[0].value : null;
//    CustomerInvoiceRegisterAdvanceSearch.InvoiceType = invoiceType[0].value !== "" ? invoiceType[0].value : null;
//   CustomerInvoiceRegisterAdvanceSearch.Search = search[0].value !== "" ? search[0].value : null;
    

//    DataTables.CustomerInvoiceRegisterReportTable.clear().rows.add(GetCustomerInvoiceRegister(CustomerInvoiceRegisterAdvanceSearch)).draw(false);
//}

function AdvanceSearchContent() {
    debugger;

    var filter = $("#BasicFilters");
    var company = $("#Company");
    var customer = ($("#CustomerID").val() != "" ? $("#CustomerID").val() : null);
    var invoiceType = $("#ddlInvoiceTypes");
    var search = $("#search").val();
    if ($("#summary").prop('checked')) {
      var  reporttype = $("#summary").val();
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

    var CustomerInvoiceRegisterAdvanceSearch = new Object();
    CustomerInvoiceRegisterAdvanceSearch.Filter = filter[0].value !== "" ? filter[0].value : null;
    CustomerInvoiceRegisterAdvanceSearch.company = company[0].value !== "" ? company[0].value : null;
    CustomerInvoiceRegisterAdvanceSearch.customer = customer !== "" ? customer : null;
    CustomerInvoiceRegisterAdvanceSearch.invoiceType = invoiceType[0].value !== "" ? invoiceType[0].value : null;
    CustomerInvoiceRegisterAdvanceSearch.Search = search !== "" ? search : null;
    CustomerInvoiceRegisterAdvanceSearch.ReportType = reporttype !== "" ? reporttype : null;
    CustomerInvoiceRegisterAdvanceSearch.IncludeInternal = IncludeInternal !== "" ? IncludeInternal : null;
    DataTables.CustomerInvoiceRegisterReportTable.clear().rows.add(GetCustomerInvoiceRegister(CustomerInvoiceRegisterAdvanceSearch)).draw(false);
}

function InvoiceDetail(currentObj) {
    debugger
    $('#InvoiceDetailModel').modal('show');//
    var rowData = DataTables.CustomerInvoiceRegisterReportTable.row($(currentObj).parents('tr')).data();
    //$("#newBtn").attr("disabled", true);
    //$("#saveBtn").attr("disabled", true);
    //NewSpecialPayments()
    if ((rowData != null) ) {
        DataTables.PaidAmountDetail.clear().rows.add(GetPaidAmountDetail(rowData.InvoiceID, rowData.CustomerID, rowData.RowType)).draw(false);

    }
   //BindPaidAmountDetailTable();
}
function BindPaidAmountDetailTable() {
    DataTables.PaidAmountDetail.clear().rows.add(GetPaidAmountDetail(rowData.InvoiceID, rowData.CustomerID, rowData.RowType)).draw(false);

}
function GetPaidAmountDetail(InvoiceID, CustomerID, RowType) {
    try {
        debugger;
        var InvoiceID = InvoiceID;
        var CustomerID = CustomerID;
        var RowType = RowType;

        var filter = $("#BasicFilters").val();
        var company = $("#Company").val();
       
        var invoiceType = $("#ddlInvoiceTypes").val();

        var data = { "InvoiceID": InvoiceID, "CustomerID": CustomerID, "RowType": RowType, "Filter": filter, "Company": company, "InvoiceType": invoiceType };
        var ds = {};
        ds = GetDataFromServer("Report/GetPaidAmountDetail/", data);
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