var DataTables = {};
var today = '';
$(document).ready(function () {
    try {
        $("#Company,#Supplier").select2({

        });
        DataTables.SupplierExpeditingDetailTableReportTable = $('#SupplierExpeditingDetailTable').DataTable(
         {

             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4, 5,6, 7,9]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: true,
             ordering: false,
             paging: true,
             data:GetSupplierExpeditingDetail(),
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [

               { "data": "SupplierName", "defaultContent": "<i></i>" },
               { "data": "supplierContactObj.ContactName", "defaultContent": "<i>-</i>" },
               { "data": "ContactNo", "defaultContent": "<i>-</i>" },
               { "data": "companyObj.Name", "defaultContent": "<i>-</i>" },
               { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },              
               { "data": "InvoiceDate", "defaultContent": "<i>-</i>" },
               { "data": "Amount", "defaultContent": "<i>-</i>",render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>"  },
               { "data": "NoOfDays", "defaultContent": "<i>-</i>" },
               { "data": "SupplierName1", "defaultContent": "<i></i>" },
               { "data": "Remarks", "defaultContent": "<i></i>" }

             ],
             columnDefs: [{ "targets": [9], "visible": false, "searchable": false },
                  { "targets": [8], "visible": false },
                  { className: "text-left", "targets": [0, 1, 2, 3, 4] },
                  { "width": "15%", "targets": [0,1,2] },
                  { className: "text-right", "targets": [6] },
                  {className:"text-center","targets":[5,7]}
             ]
         });

        $(".buttons-excel").hide();
        today = $("#todate").val();
    }
    catch (x)
    {
        notyAlert('error', x.message);
    }
});

//Bind Values to Datatable
function GetSupplierExpeditingDetail(supplierPayementAdvanceSearchObj)
{
    debugger;
    try
    {       
        if (supplierPayementAdvanceSearchObj === 0)
        {
          var data = {};
        }
        else
        {
          var data = { "supplierPayementAdvanceSearchObj": JSON.stringify(supplierPayementAdvanceSearchObj) };
        }
          var ds = {};
        //var todate = $("#todate").val();
        //var filter = $("#BasicFilters").val();
        //var company = $("#Company").val();
        //var supplier = $("#Supplier").val();
        //if (IsVaildDateFormat(todate))
        //    var data = { "ToDate": todate, "Filter": filter, "Company":company, "Supplier":supplier };
        //var ds = {};
        ds = GetDataFromServer("Report/GetSupplierPaymentExpeditingDetails/", data);
        if (ds != '')
        {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK")
        {
            return ds.Records.SupplierExpeditingDetailsList;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }
    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
}

//advanced filter
function RefreshSupplierExpeditingDetailTable()
{
    debugger;
    try
    {
        var toDate = $("#todate");
        var filter = $("#BasicFilters");
        var company = $("#Company");
        var supplier = $("#Supplier");
        var invoicetype = $("#ddlInvoiceTypes");
        var supplierAdvanceSearch = new Object();
        supplierAdvanceSearch.ToDate = toDate[0].value !== "" ? toDate[0].value : null;
        supplierAdvanceSearch.Filter = filter[0].value !== "" ? filter[0].value : null;
        supplierAdvanceSearch.Company = company[0].value !== "" ? company[0].value : null;
        supplierAdvanceSearch.Supplier = supplier[0].value !== "" ? supplier[0].value : null;
        supplierAdvanceSearch.InvoiceType = invoicetype[0].value !== "" ? invoicetype[0].value : null;
        //if (DataTables.SupplierExpeditingDetailTableReportTable != undefined && IsVaildDateFormat(todate)) {
        //    DataTables.SupplierExpeditingDetailTableReportTable.clear().rows.add(GetSupplierExpeditingDetail()).draw(true);
        //}  
        if (DataTables.SupplierExpeditingDetailTableReportTable != undefined )
        {       
        DataTables.SupplierExpeditingDetailTableReportTable.clear().rows.add(GetSupplierExpeditingDetail(supplierAdvanceSearch)).draw(false);
        }
        }       
   
    catch (e)
    {
        notyAlert('error', e.message);
    }
}

//to trigger export button
function PrintReport()
{
    try
    {
        $(".buttons-excel").trigger('click');
    }
    catch (e)
    {
        notyAlert('error', e.message);
    }
}

function Back()
{
    window.location = appAddress + "Report/Index/";
}

//To reset report
function Reset()
{
    debugger;
    $("#todate").val(today);
    $("#BasicFilters").val('ALL');
    $("#Company").val('ALL').trigger('change')
    $("#Supplier").val('').trigger('change')
    $("#ddlInvoiceTypes").val('RB');
    RefreshSupplierExpeditingDetailTable();
}