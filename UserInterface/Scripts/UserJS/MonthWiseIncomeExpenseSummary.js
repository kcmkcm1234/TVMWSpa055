var DataTables = {};
var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
$(document).ready(function () {
    try {
        //$("#groupCode").select2()({
        //});
      
        //$("#groupCode").on('change', function () {
        debugger;
        Rebind();
       // });
    //    $("#DynamictblContainer").append('<table id="tblMonthWiseIncomeExpenseSummaryList" class="table compact dataTable no-footer" cellspacing="0" width="100%" role="grid" aria-describedby="tblMonthWiseIncomeExpenseSummaryList_info" style="width: 100%;"><thead><tr class="text-center" id="trMonthWise" role="row"><th tabindex="0" aria-controls="tblMonthWiseIncomeExpenseSummaryList" rowspan="1" colspan="1" style="width: 48px;">Date</th><th  tabindex="0" aria-controls="tblMonthWiseIncomeExpenseSummaryList" rowspan="1" colspan="1" style="width: 32px;">Minor</th><th  tabindex="0" aria-controls="tblMonthWiseIncomeExpenseSummaryList" rowspan="1" colspan="1" style="width: 32px;">Major</th><th tabindex="0" aria-controls="tblMonthWiseIncomeExpenseSummaryList" rowspan="1" colspan="1" style="width: 36px;">Mndty</th><th  tabindex="0" aria-controls="tblMonthWiseIncomeExpenseSummaryList" rowspan="1" colspan="1" style="width: 35px;">Install</th><th tabindex="0" aria-controls="tblMonthWiseIncomeExpenseSummaryList" rowspan="1" colspan="1" style="width: 42px;">Repeat</th><th tabindex="0" aria-controls="tblMonthWiseIncomeExpenseSummaryList" rowspan="1" colspan="1" style="width: 36px;">AMC1</th><th tabindex="0" aria-controls="tblMonthWiseIncomeExpenseSummaryList" rowspan="1" colspan="1"  style="width: 37px;">AMC2</th></tr></thead><tbody id="tbodyMonthWise"></tbody></table>');
    
        DataTables.paymentDetailReportTable = $('#monthlyIncomeExpenseTable').DataTable(
               {
                   dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                   order: [],
                   searching: true,
                   paging: true,
                   data: null,
                   pageLength: 50,
                   columns: [
                     { "data": "DocNo", "defaultContent": "<i>-</i>", "width": "10%" },
                      { "data": "DocDateFormatted", "defaultContent": "<i>-</i>", "width": "20%" },
                      { "data": "DocType", "defaultContent": "<i>-</i>", "width": "20%" },
                        { "data": "Amount", "defaultContent": "<i>-</i>", "width": "10%" },
                       
                   ],
                   columnDefs: [ //{ "targets": [9, 11], "visible": false, "searchable": false },

                       // { className: "text-left", "targets": [0,2] },
                         { className: "text-center", "targets": [0, 1, 2, 3] }, ], 
                   createdRow: function (row, data, index) {
                             if (data.DocNo == "<b>GrantTotal</b>") {

                                 $('td', row).addClass('totalRow');
                             }
                         },
                    
               });

    }
    catch (e) {

    }
});
function DrawTable() {
    var Records = GetItemsSummary();
    debugger;
    if (Records != null) {
        $("#DynamictblContainer").empty();
        $("#DynamictblContainer").append('<table id="tblMonthWiseIncomeExpenseSummaryList" class="table compact" cellspacing="0" width="100%">'
                            + '<thead><tr class="text-center" id="trMonthWise"></tr><tr class="text-center" id="trExpenseWise"><th></th></tr></thead>'
                            + '<tbody id="tbodyMonthWise" ></tbody>'
                        + '</table>')
        var Header = [];
        $.each(Records, function (index, Records) {
          //  debugger;
            if (Header.length == 0) {
               // debugger;
                $.each(Records, function (key, value) {
                   
                    Header.push(key);
                  
                   
                });
                var i = 0;
                //  debugger;
                   
              
                for (i = 0; i < Header.length; i=i+2) {
                    debugger;
               
                  var str1 = Header[i];
                  //  try {
                    //
                    var head2 = str1.substring(str1.indexOf(' ') + 1);
                     
                    if (i == 0)
                    {
                        $("#trMonthWise").append('<th>' + Header[i] + ' </th>')
                    }
                    else {
                        $("#trMonthWise").append('<th style="text-align:center;" colspan="2">' + head2 + ' </th>')
                        $('#trExpenseWise').append('<th >Debit</th>' + '<th >Credit</th>');
                    }
                    
              


                 
               
                }
               

            }
            var html = "";
           $.each(Records, function (key, value) {
           
                  html = html + "<td>" + ((value != null && value != 0) ? value : "-") + "</td>"
         

            });
            $("#tbodyMonthWise").append('<tr>' + html + '</tr>')
           // $('#tblMonthWiseIncomeExpenseSummaryList').append('<tr><td colspan="2">' + headng + " " + headng1 +  '</td></tr>');
        });
    }

}
function FireDatatable() {
    DataTables.tblMonthWiseIncomeExpenseSummaryList=$('#tblMonthWiseIncomeExpenseSummaryList').DataTable({
        dom: 'Bfrtip',
        buttons: [
           'excel'
        ],
       // order: [[0, "asc"]],
        scrollX: true,
        scrollY: true,
        scrollCollapse: true,
        searching: true,
        paging: true,
        autoWidth: false,
    //   ordering: false,
       // aoColumnDefs: { "bSortable": false, "aTargets": [ 0, 1, 2, 3 ] }, 
        //order:false,
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Search Items..."
        },
        fixedColumns:  {
            leftColumns: 1,
           // fixedColumnsRight: 1
        },
        columnDefs: [
        {
            targets: [1],
            visible: true,
            searchable: false,
          
        },
        {
            width: "100px",
            "targets": "_all"
        },
        {
            targets: [1],
            visible: false
        }
        ]
    });
    $(".buttons-print").hide();
    $(".buttons-excel").hide();
}
function GetItemsSummary() {
    debugger;
    try {
        var groupCode = $("#groupCode").val();
        var isGrouped = $("#ddlGroupingRequest").val();
        var search = $("#Search").val();
        
        if ((isGrouped)) {
            data = { "IsGrouped": isGrouped, "Search": search };
            
            var ds = {};
            ds = GetDataFromServer("Report/GetMonthWiseIncomeExpenseSummary/", data);
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
function Rebind() {
    debugger;
    DrawTable();
    FireDatatable();
}
function PrintReport() {
    debugger;
    try {
        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function goBack() {
    window.location = appAddress + "Report/Index/";
}

function AdvanceSearchContent() {
    try {
        debugger;
        Rebind();
     
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function ViewPaymentDetail(Month, Year, this_obj, IsGrouped, Transaction) {
    debugger;
    try {
        var rowData = DataTables.tblMonthWiseIncomeExpenseSummaryList.row($(this_obj).parents('tr')).data();
        var months = [
      'January', 'February', 'March', 'April', 'May',
      'June', 'July', 'August', 'September',
      'October', 'November', 'December'
        ];

        var monthName = months[Month - 1] || '';
      
        $("#lblDetailsHead").text(rowData[0] + " :" + " " + monthName + " " + Year);
        var month = Month;
        var year = Year;              
        var GroupCode = rowData[0];
        var IsGrouped = IsGrouped;
        var Transaction = Transaction;

        var data = { "Month": month, "Year": year, "IsGrouped": IsGrouped, "GroupCode": GroupCode, "Transaction": Transaction };
        var ds = {};
        ds = GetDataFromServer("Report/GetMonthWiseIncomeExpenseDetail/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            //return ;
            openNav();
            DataTables.paymentDetailReportTable.clear().rows.add(ds.Records).draw(true);
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }
        
       
     //   DataTables.paymentDetailReportTable.clear().rows.add(GetPaymentDetail()).draw(true);
    }
    catch (e) {
        notyAlert('error', e.message);
    }


    //console.log(arguments);
  //  var rowData = DataTables.tblMonthWiseIncomeExpenseSummaryList.row($(row_obj).parents('tr')).data();
  //  var month = ViewPaymentDetail.arguments;
  
  //  GetPaymentDetail();
  //  DataTables.tblMonthWiseIncomeExpenseSummaryList.clear().rows.add(GetPaymentDetail(rowData)).draw(true);
}


function GetPaymentDetail() {
    debugger;
    openNav();
       
    DataTables.paymentDetailReportTable.clear().rows.add(ViewPaymentDetail()).draw(true);

}
function Back()
{
    window.location = appAddress + "Report/Index/";
}

