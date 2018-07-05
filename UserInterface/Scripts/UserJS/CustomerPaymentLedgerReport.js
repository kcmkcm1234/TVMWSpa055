var DataTables = {};
var startDate = '';
var endDate = '';
var Chkurl = 0;
$(document).ready(function () {   
    debugger;
    try {
        debugger;
        $("#CustomerCode").select2({
            multiple: true,
            placeholder: "Select a Customers..",
        });
      
        debugger;
        var CustomerPaymentLeger = new Object();
        var field = 'CustomerCode';
        var url = window.location.href;
        var fromField = 'FromDate';
        if (url.indexOf('?' + field + '=') != -1) {
            //  var CustomerPaymentLeger = new Object();
            if (url.indexOf('&' + fromField + '=') != -1) {

                var FromDate = GetParameterValues('FromDate')
                $("#fromDate").val(FromDate);
                CustomerPaymentLeger.FromDate = FromDate;

                CustomerPaymentLeger.FromDate = FromDate;
                var ToDate = GetParameterValues('ToDate')
                $("#toDate").val(ToDate);
                CustomerPaymentLeger.ToDate = ToDate;
                var InvoiceType = GetParameterValues('InvoiceType')
                $("#ddlInvoiceTypes").val(InvoiceType);
                CustomerPaymentLeger.InvoiceType = InvoiceType;
                var Company = GetParameterValues('Company')
                $("#companyCode").val(Company);
                CustomerPaymentLeger.Company = Company;
                CustomerPaymentLeger.CustomerID = GetParameterValues('CustomerCode');
             
            }
            else
            {
                var fromDate = $("#fromDate").val();
                CustomerPaymentLeger.FromDate = fromDate
                var toDate = $("#toDate").val();
                CustomerPaymentLeger.ToDate = toDate               
                //  var customerIds = $("#CustomerCode").val();
                var customerIds = GetParameterValues('CustomerCode');
                CustomerPaymentLeger.CustomerID = GetParameterValues('CustomerCode');
              
            }
            
        }
        else
        {
            var fromDate = $("#fromDate").val();
            CustomerPaymentLeger.FromDate = fromDate
            var toDate = $("#toDate").val();
            CustomerPaymentLeger.ToDate = toDate
            var customerIds = $("#CustomerCode").val();
            CustomerPaymentLeger.CustomerID = customerIds
            
        }
        DataTables.customerPaymentLedgerTable = $('#customerpaymentledgertable').DataTable({

            // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1, 2, 4, 5, 6, 7, 8, 9,10]
                             }
            }],
            order: [],
            fixedHeader: {
                header: true
            },
            searching: false,
            paging: true,
            data: GetCustomerPaymentLedger(CustomerPaymentLeger),

            pageLength: 50,
            columns: [

              { "data": "Date", "defaultContent": "<i>-</i>" },
              {
                  "data": "Type", "width": "20%", "defaultContent": "<i>-</i>",
                 
               },

                 {
                     "data": "PAYTYPE", "defaultContent": "<i>-</i>",
                 },

              { "data": "Ref", "defaultContent": "<i>-</i>" },
              { "data": "ID", "defaultContent": "<i>-</i>" },
              { "data": "CustomerName", "defaultContent": "<i>-</i>" },
              { "data": "Company", "defaultContent": "<i>-</i>" },
                { "data": "Remarks", "defaultContent": "<i>-</i>" },
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
             { "data": "Advance", "width": "20%", "defaultContent": "<i>-</i>" },

            ],
            columnDefs: [{ "targets": [2,4, 5,11], "visible": false, "searchable": false },
            { className: "text-left", "targets": [0,2, 3,  6, 7, 8, 9,10] },
            { "width": "10%", "targets": [0] },
            { "width": "7%", "targets": [1,2] },
              { "width": "15%", "targets": [7] },
            { className: "text-right", "targets": [] },
            { className: "text-center", "targets": [1] },
            { "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10] }],
            createdRow: function (row, data, index) {
                if (data.Type == "<b>Total</b>") {
                    $('td', row).addClass('totalRow');
                }

            },          
            drawCallback: function (settings) {
                var api = this.api();
                var rows = api.rows({ page: 'current' }).nodes();
                var last = null;
                api.column(5, { page: 'current' }).data().each(function (group, i) {

                    if (last !== group) {
                        $(rows).eq(i).before('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>CustomerName</b> : ' + group + '</td></tr>');
                        last = group;
                    }
                    //if (api.column(6, { page: 'current' }).data().count() === i)
                    //{
                    //    $(rows).eq(i).after('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>Sub Total</b> : ' + group + '</td></tr>');
                    //}
                });
            }
        });

        debugger;
        $(".buttons-excel").hide();
        $("#btnSend").hide();
        startDate = $("#toDate").val();
        endDate = $("#fromDate").val();

        var name = [];
        name.push(GetParameterValues('CustomerCode'));
        if (name != "")
        {
            $("#customernameddl").attr('style', 'visibility:hidden');
        }
        else
        {
            $("#customernameddl").attr('style', 'visibility:true');
        }
       


       
        }
    
    catch (x)
    {
        notyAlert('error', x.message);
    }
});


function GetParameterValues(param) {
    debugger;
         var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }

//To bind values to report
function GetCustomerPaymentLedger(Obj)
{
    debugger;
    try
    {
        

        var company = $("#companyCode").val();
        var search = $("#Search").val();
        var invoiceType = $("#ddlInvoiceTypes").val();
        if (IsVaildDateFormat(Obj.FromDate) && IsVaildDateFormat(Obj.ToDate) && Obj.CustomerID ) {
            var data = { "FromDate": Obj.FromDate, "ToDate": Obj.ToDate, "CustomerIDs": Obj.CustomerID, "Company": company, "InvoiceType": invoiceType, "Search": search };
            var ds = {};
            ds = GetDataFromServerTraditional("Report/GetCustomerPaymentLedger/", data);
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
    catch (e)
    {
        notyAlert('error', e.message);
    }
}


//To refresh table based on filter
function OnCallChange(){
    debugger;
    try
    {
        var CustomerPaymentLeger = new Object();
       
            var fromDate = $("#fromDate").val();
            CustomerPaymentLeger.FromDate = fromDate
            var toDate = $("#toDate").val();
            CustomerPaymentLeger.ToDate = toDate
           
            var invoiceType = $("#ddlInvoiceTypes").val();
            var company = $("#companyCode").val();
 
            var field = 'CustomerCode';
            var url = window.location.href;
            if (url.indexOf('?' + field + '=') != -1)
            {
                var customerIds = GetParameterValues('CustomerCode');
                CustomerPaymentLeger.CustomerID = GetParameterValues('CustomerCode');
            }
            else
            {
                var customerIds = $("#CustomerCode").val();
                CustomerPaymentLeger.CustomerID = customerIds
            }
         
        if (DataTables.customerPaymentLedgerTable != undefined && IsVaildDateFormat(fromDate) && IsVaildDateFormat(toDate) && customerIds) {
            DataTables.customerPaymentLedgerTable.clear().rows.add(GetCustomerPaymentLedger(CustomerPaymentLeger)).draw(true);
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

function RefreshCustomerPaymentLedgerTable()
{
    if ($("#CustomerCode").val() == '')
    {
       DataTables.customerPaymentLedgerTable.clear().rows.add(GetCustomerPaymentLedger('ALL')).draw(true);
    }   
   OnCallChange();
}

//To reset CustomerPayementLedger report
function Reset()
{
   
    var field = 'CustomerCode';
    var url = window.location.href;
    if (url.indexOf('?' + field + '=') != -1) {
        $("#toDate").val(startDate);
        $("#fromDate").val(endDate);
        
        $("#companyCode").val('ALL');
        $("#ddlInvoiceTypes").val('ALL');
        $("#Search").val('');
        OnChangeCall();
    }
    else {
        $("#toDate").val(startDate);
        $("#fromDate").val(endDate);
        $("#CustomerCode").val('').select2();
       // $("#CustomerCode").val('').trigger('change')
        $("#companyCode").val('ALL');
        $("#ddlInvoiceTypes").val('ALL');
        $("#Search").val('');
        OnChangeCall();
    }
    DataTables.customerPaymentLedgerTable.clear().rows.add(GetCustomerPaymentLedger('ALL')).draw(true);
}

function OnChangeCall() {

    var CustomerPaymentLeger = new Object();

    var fromDate = $("#fromDate").val();
    CustomerPaymentLeger.FromDate = fromDate
    var toDate = $("#toDate").val();
    CustomerPaymentLeger.ToDate = toDate
    var field = 'CustomerCode';
    var url = window.location.href;
    if (url.indexOf('?' + field + '=') != -1) {
        var customerIds = GetParameterValues('CustomerCode');
        CustomerPaymentLeger.CustomerID = GetParameterValues('CustomerCode');
    }
    else {
        var customerIds = $("#CustomerCode").val();
        CustomerPaymentLeger.CustomerID = customerIds
    }

    try {
        DataTables.customerPaymentLedgerTable.clear().rows.add(GetCustomerPaymentLedger(CustomerPaymentLeger)).draw(true);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}



//To trigger download button
function DownloadReport()
{
    $('#btnSend').trigger('click');
}

//To download file in PDF
function GetHtmlData()
{
    debugger;
    var CustomerPaymentLeger = new Object();
    var field = 'CustomerCode';
    var url = window.location.href;
    if (url.indexOf('?' + field + '=') != -1) {
        var customerIds = GetParameterValues('CustomerCode');
        CustomerPaymentLeger.CustomerID = GetParameterValues('CustomerCode');
    }
    else {
        var customerIds = $("#CustomerCode").val();
        CustomerPaymentLeger.CustomerID = customerIds
    }
    DrawTable({
        Action: "Report/GetCustomerPaymentLedger/",
        data: { "FromDate": $('#fromDate').val(), "ToDate": $('#toDate').val(), "CustomerIDs": customerIds, "Company": $('#companyCode').val(), "InvoiceType": $('#ddlInvoiceTypes').val() },
        Exclude_column: ["CustomerID", "customerList", "CustomerCode", "pdfToolsObj", "CompanyCode", "CompanyList", "companiesList", "InvoiceType", "Remarks", "InvoiceTypeAccess", "Advance", "Search", "Type", "CustomerName"],
        Header_column_style: {
            "Date": { "style": "width:10%;font-size:12px;text-align:left;border-bottom:2px solid;border-bottom-color: #000000; font-weight: 600;", "custom_name": "Date" },
            "PAYTYPE": { "style": "font-size:12px;text-align:left;border-bottom:2px solid ;border-bottom-color: #000000; width:15%;font-weight: 600;", "custom_name": "Type" },
            "Ref": { "style": "font-size:12px;text-align:left;border-bottom:2px solid ;border-bottom-color: #000000; width:10%;font-weight: 600;", "custom_name": "Ref" },
            "Company": { "style": "width:15%;text-align:left;font-size:12px;border-bottom:2px solid ;border-bottom-color: #000000; font-weight: 600;", "custom_name": "Company" },
            "CustomerName": { "style": "width:20%;text-align:left;font-size:12px;border-bottom:2px solid ;border-bottom-color: #000000; font-weight: 600;", "custom_name": "Customer" },
            "Debit": { "style": "width:10%;text-align:right;font-size:12px;border-bottom:2px solid ;border-bottom-color: #000000; font-weight: 600;", "custom_name": "Debit" },
            "Credit": { "style": "width:10%;text-align:right;font-size:12px;border-bottom:2px solid ;border-bottom-color: #000000; font-weight: 600;", "custom_name": "Credit" },
            "Balance": { "style": "width:10%;text-align:right;font-size:12px;border-bottom:2px solid ;border-bottom-color: #000000; font-weight: 600;", "custom_name": "Balance" }
        },      
        Row_color: { "Odd": "White", "Even": " White" },
        Body_Column_style: {
            "Date": "font-size:11px;font-weight: 100;width:10%;border-bottom:2px solid;border-bottom-color: #e4dfdf;height:30px",
            "PAYTYPE": "font-size:11px;font-weight: 100;border-bottom:2px solid;border-bottom-color: #e4dfdf;height:30px",
            "Ref": "font-size:11px;font-weight: 100;width:150px;border-bottom:2px solid;border-bottom-color: #e4dfdf;height:30px",
            "Company": "font-size:11px;font-weight: 100;border-bottom:2px solid;border-bottom-color: #e4dfdf;height:30px",
            "CustomerName": "font-size:11px;font-weight: 100;width:20%;border-bottom:2px solid;border-bottom-color: #e4dfdf;height:30px",
            "Debit": "text-align:right;font-size:11px;font-weight: 100;width:10%;border-bottom:2px solid;border-bottom-color: #e4dfdf;height:30px",
            "Credit": "text-align:right;font-size:11px;font-weight: 100;width:150px;border-bottom:2px solid;border-bottom-color: #e4dfdf;height:30px",
            "Balance": "text-align:right;font-size:11px;font-weight: 100;width:150px;border-bottom:2px solid;border-bottom-color: #e4dfdf;height:30px",
            
        }
    });
    //to give backround color to balance row
        $('.balanceRowColor').parent('tr').css('background-color', '#d98cd9');  
        var bodyContent = $('#customtbl').html();
        var headerContent = $('#divHeader').html();
        $("#hdnContent").val(bodyContent);
        $('#hdnHeadContent').val("<h1></h1>");
        var CustomerName = "Customer : " + $("#CustomerCode option:selected").text();
        $('#hdnCustomerName').val(CustomerName);

}




