var DataTables = {};
var startDate = '';
var endDate = '';
$(document).ready(function () {
    debugger;
    try {

        //$("#CustomerCode").select2({
        //    multiple: true,
        //    placeholder: "Select a Customers..",
        //});

        DataTables.customeroutstandingTable = $('#customerOutstandingTable').DataTable({

            // dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [ 1, 2,3, 4, 5]
                             }
            }],
            order: [],
            fixedHeader: {
                header: true
            },
            searching: false,
            paging: true,
            data: GetCustomerOutstanding(),

            pageLength: 50,
            columns: [

              { "data": "CustomerID", "defaultContent": "<i>-</i>" },
              { "data": "CustomerName" , "defaultContent": "<i>-</i>" },             
               {
                   "data": "OpeningBalance", render: function (data, type, row)  {
                       data != 0 ? data = formatCurrency(roundoff(data, 1)) : data = "-";
                       return  data;
                   }, "defaultContent": "<i>-</i>"
               },
               
              {
                  "data": "Debit", render: function (data, type, row) {
                      data != 0 ? data = formatCurrency(roundoff(data, 1)) : data = "-";
                      return data;
                  }, "defaultContent": "<i>-</i>"
              },
              {
                  "data": "Credit", render: function (data, type, row) {
                      data != 0 ? data = formatCurrency(roundoff(data, 1)) : data = "-";
                      return data;
                  }, "defaultContent": "<i>-</i>"
              },
              {
                  "data": "OutStanding", render: function (data, type, row) {
                      data != 0 ? data = formatCurrency(roundoff(data, 1)) : data = "-";
                      return data;
                  }, "defaultContent": "<i>-</i>"
              },

            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
           
            //{ "width": "10%", "targets": [0] },
            { "width": "7%", "targets": [1] },
            { "width": "7%", "targets": [2] },
            { "width": "7%", "targets": [3] },
            { "width": "7%", "targets": [4] },
            { "width": "7%", "targets": [5] },
            { className: "text-right", "targets": [2,3,4,5] },
            { className: "text-left", "targets": [1] },
            { "bSortable": true, "aTargets": [1, 2, 3, 4, 5] },
            ],
           
            //createdRow: function (row, data, index) {
            //    if (data.Type == "<b>Total</b>") {
            //        $('td', row).addClass('totalRow');
            //    }

            //},
            //drawCallback: function (settings) {
            //    var api = this.api();
            //    var rows = api.rows({ page: 'current' }).nodes();
            //    var last = null;
            //    api.column(4, { page: 'current' }).data().each(function (group, i) {

            //        if (last !== group) {
            //            $(rows).eq(i).before('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>CustomerName</b> : ' + group + '</td></tr>');
            //            last = group;
            //        }
            //        //if (api.column(6, { page: 'current' }).data().count() === i)
            //        //{
            //        //    $(rows).eq(i).after('<tr class="group "><td colspan="7" class="rptGrp">' + '<b>Sub Total</b> : ' + group + '</td></tr>');
            //        //}
            //    });
            //}
        });

        //debugger;
        $(".buttons-excel").hide();
        //$("#btnSend").hide();
        startDate = $("#todate").val();
        endDate = $("#fromdate").val();

        //var name = [];
        //name.push(GetParameterValues('CustomerCode'));
        //if (name != "") {
        //    $("#customernameddl").attr('style', 'visibility:hidden');
        //}
        //else {
        //    $("#customernameddl").attr('style', 'visibility:true');
        //}


    }

    catch (x) {
        notyAlert('error', x.message);
    }
});
function GetCustomerOutstanding() {
    try {
        debugger;
      
        // $('#CustomerCode').select2('val', name[1]);
        var fromDate = $("#fromdate").val();
        var toDate = $("#todate").val();
        var search = $("#Search").val();
        var companyCode = $("#companyCode").val();
       //// if (name != "") {
       //     var customerIds = name;
            // $("#customernameddl").attr('style', 'visibility:hidden');
       // }
       // else {
        //    var customerIds = (cur != "ALL" ? $("#CustomerCode").val() : cur);

        //}
       // var company = $("#companyCode").val();
        var invoiceType = $("#ddlInvoiceTypes").val();
        if (IsVaildDateFormat(fromDate) && IsVaildDateFormat(toDate)) {
            var data = { "FromDate": fromDate, "ToDate": toDate, "InvoiceType": invoiceType,"Company":companyCode, "Search": search, };
            var ds = {};
            ds = GetDataFromServerTraditional("Report/GetCustomerOutstanding/", data);
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
function OnCallChange() {
    debugger;
    try {
        var fromDate = $("#fromdate").val();
        var toDate = $("#todate").val();     
        var invoiceType = $("#ddlInvoiceTypes").val();
        var companyCode = $("#companyCode").val();
        if (DataTables.customeroutstandingTable != undefined && IsVaildDateFormat(fromDate) && IsVaildDateFormat(toDate)) {
            DataTables.customeroutstandingTable.clear().rows.add(GetCustomerOutstanding()).draw(true);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}



function Back() {
    window.location = appAddress + "Report/Index/";
}
function Reset() {
    $("#todate").val(startDate);
    $("#fromdate").val(endDate);
    $("#ddlInvoiceTypes").val('ALL');
    DataTables.customeroutstandingTable.clear().rows.add(GetCustomerOutstanding('ALL')).draw(true);
}
function OnChangeCall() {
    try
    {
        DataTables.customeroutstandingTable.clear().rows.add(GetCustomerOutstanding()).draw(true);
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