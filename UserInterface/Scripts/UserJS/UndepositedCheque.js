
var DataTables = {};
var StartDate = '';
var EndDate = '';
$(document).ready(function () {
    try {
        DataTables.undepositedChequeTable = $('#undepositedChequeTable').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4,5,6]
                              }
             }],
             order: [],
             searching: false,
             paging: true,
             data: GetUndepositedChequeTable(),
             pageLength: 50,
             //language: {
             //    search: "_INPUT_",
             //    searchPlaceholder: "Search", "width": "15%" 
             //},
             columns: [
               { "data": "DateFormatted", "defaultContent": "<i>-</i>", "width": "12%" },
               { "data": "ReferenceNo", "defaultContent": "<i>-</i>", "width": "12%" },
               { "data": "CustomerName", "defaultContent": "<i>-</i>", "width": "15%" },
               {"data":"CompanyObj.Name","defaultContent":"<i>-</i>", "width": "10%"},
               { "data": "ReferenceBank", "defaultContent": "<i>-</i>", "width": "12%" },               
               {"data":"GeneralNotes","defaultContent": "<i>-</i>", "width": "12%"},
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" ,"width": "10%"},
             //{ "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "searchable": false }, 
                  { className: "text-left", "targets": [1, 2,3,4,5] },
                  { className: "text-right", "targets": [6] },
                  { className: "text-center", "targets": [0] }]           
         });
        $(".buttons-excel").hide();
        StartDate = $("#todate").val();
        EndDate = $("#fromdate").val();
    } catch (x)
    {
        //this will show the error msg in the browser console(F12) 
        console.log(x.message);
    }
});

//To bind values 
function GetUndepositedChequeTable(undepositedChequeAdvanceSearchObject)
{
    try
    {
        if (undepositedChequeAdvanceSearchObject == 0)
        {
            var data = {};
        }
        else
        {
            var data = { "undepositedChequeAdvanceSearchObject": JSON.stringify(undepositedChequeAdvanceSearchObject) };
        }
        //var fromdate = $("#fromdate").val();
        //var todate = $("#todate").val();

        //if ((fromdate == "" ? true : IsVaildDateFormat(fromdate)) && IsVaildDateFormat(todate)) {
        //    var data = { "FromDate": fromdate, "ToDate": todate };       
            var ds = {};
            ds = GetDataFromServer("DepositAndWithdrawals/GetUndepositedCheque/", data);
            if (ds != '')
            {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK")
            {

                $("#fromdate").val(ds.FromDate);
                return ds.Records;
            }
            if (ds.Result == "ERROR")
            {
                notyAlert('error', ds.Message);
                return ds.Records;
            }
        }    
    catch (e)
    {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

//advanced filter for undeposited cheques
function RefreshUndepositedChequeTable()
{
   try
    {        
        var fromDate = $("#fromdate");
        var toDate   = $("#todate");
        var company  = $("#ddlCompany");      
        var customer = $("#ddlCustomer");
        var bankCode = $("#ddlBank");
        var search = $("#Search");
        var chequeAdvanceSearch = new Object();
        chequeAdvanceSearch.FromDate = fromDate[0].value !== "" ? fromDate[0].value : null;
        chequeAdvanceSearch.ToDate = toDate[0].value !== "" ? toDate[0].value : null;
        chequeAdvanceSearch.Company = company[0].value !== "" ? company[0].value : null;
        chequeAdvanceSearch.Customer = customer[0].value !== "" ? customer[0].value : null;
        chequeAdvanceSearch.BankCode = bankCode[0].value !== "" ? bankCode[0].value : null;
        chequeAdvanceSearch.Search = search[0].value !== "" ? search[0].value : null;
    //    if (DataTables.undepositedChequeTable != undefined && (fromdate==""?true:IsVaildDateFormat(fromdate)) && IsVaildDateFormat(todate)) {
    //        var records = GetUndepositedChequeTable();
    //        if (records != undefined)
    //            DataTables.undepositedChequeTable.clear().rows.add(records).draw(false);
    //    }
    //}
    if (DataTables.undepositedChequeTable != undefined )
    {       
        DataTables.undepositedChequeTable.clear().rows.add(GetUndepositedChequeTable(chequeAdvanceSearch)).draw(false);
    }
   }
    catch (e)
    {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

//To tigger export button
function PrintReport()
{
    try
    {
        $(".buttons-excel").trigger('click');
    }
    catch (e)
    {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

function Back()
{
    window.location = appAddress + "DepositAndWithdrawals/Index/";
}

function OnChangeCall()
{
    RefreshUndepositedChequeTable();
}

//To reset Undeposited cheque
function Reset()
{
    $("#fromdate").val(EndDate); 
    $("#todate").val(StartDate);
    $("#ddlCompany").val('ALL');
    $("#ddlCustomer").val('');
    $("#ddlBank").val('');
    $("#Search").val('');
    RefreshUndepositedChequeTable()
}
