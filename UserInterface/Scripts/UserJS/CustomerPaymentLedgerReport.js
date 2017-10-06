var DataTables = {};
$(document).ready(function () {
   
               
    try {
        $("#customerCode").select2({
            placeholder: "Select a Customers.."
        });
        
        DataTables.customerpaymentledgertable = $('#customerpaymentledgertable').DataTable(
         {

             // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2,4, 5,6,7,8]
                              }
             }],
             order: [],
             searching: false,
             paging: true,
             data: GetCustomerPaymentLedger(),
             pageLength: 50,
             columns: [
                
               { "data": "Date", "defaultContent": "<i>-</i>" },
               { "data": "Type","width": "20%", "defaultContent": "<i>-</i>" },
               { "data": "Ref", "defaultContent": "<i>-</i>" },
               { "data": "ID", "defaultContent": "<i>-</i>" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>" },
               { "data": "Company", "defaultContent": "<i>-</i>" },
               {
                   "data": "Debit", render: function (data, type, row) {
                       if (data != 0) {
                           return roundoff(data, 1);
                       }
                       else {
                           return data;
                       }
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "Credit", render: function (data, type, row) {
                       if (data != 0) {
                           return roundoff(data, 1);
                       }
                       else {
                           return data;
                       }
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "Balance", render: function (data, type, row) {
                       if (data != 0) {
                           return roundoff(data, 1);
                       }
                       else {
                           return data;
                       }
                   }, "defaultContent": "<i>-</i>"
               },
              
             ],
             columnDefs: [{ "targets": [3,4], "visible": false, "searchable": false },
             { className: "text-left", "targets": [0,2,5,6,7,8] },
              { "width": "15%", "targets": [0] },
               { "width": "10%", "targets": [1] },
             { className: "text-right", "targets": [] },
         { className: "text-center", "targets": [1] },
             {"bSortable": false, "aTargets": [0,1,2,3,4,5,6,7,8 ] }], 
             drawCallback: function (settings) {
                 var api = this.api();
                 var rows = api.rows({ page: 'current' }).nodes();
                 var last = null;
                 api.column(4, { page: 'current' }).data().each(function (group, i) {
                

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

        $(".buttons-excel").hide();

    } catch (x) {

        notyAlert('error', x.message);

    }


});


function GetCustomerPaymentLedger() {
    try {
        debugger;
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var customerids = $("#customerCode").val();
       
        if (IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && customerids) {
            var data = { "FromDate": fromdate, "ToDate": todate, "CustomerIDs": customerids };
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
    catch (e) {
        notyAlert('error', e.message);
    }
}


function RefreshCustomerPaymentLedgerTable()
{
    debugger;
    try
    {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var customerids = $("#customerCode").val();
          
        if (DataTables.customerpaymentledgertable != undefined && IsVaildDateFormat(fromdate) && IsVaildDateFormat(todate) && customerids) {
            DataTables.customerpaymentledgertable.clear().rows.add(GetCustomerPaymentLedger()).draw(false);
            }
        }
    catch (e)
    {
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

function Back()
{
    window.location = appAddress + "Report/Index/";
}

function OnCallChange()
{
    RefreshCustomerPaymentLedgerTable();
}

function Reset() {
    $("#customerCode").val('ALL').trigger('change')
   
}

