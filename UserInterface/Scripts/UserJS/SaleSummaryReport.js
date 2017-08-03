var DataTables = {};
$(document).ready(function () {
    try {

        DataTables.saleSummaryReportTable = $('#saleSummaryTable').DataTable(
         {

             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: true,
             paging: true,
             data: GetSaleSummary(),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": null },
               { "data": "chartOfAccounts.TypeDesc", "defaultContent": "<i>-</i>" },
               { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
               { "data": "Description", "defaultContent": "<i>-</i>" },
               { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                { "data": "ExpenseDate", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
               { "data": null, "orderable": false, "defaultContent": '<a data-toggle="tp" data-placement="top" data-delay={"show":2000, "hide":3000} title="Delete" href="#" class="DeleteLink" onclick="Delete(this)"><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>' },
               { "data": "ID" }

             ],
             columnDefs: [{ "targets": [8], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [1, 2, 3] },
             { className: "text-right", "targets": [4] },
             { className: "text-center", "targets": [5, 6, 7] }

             ]
         });
        //DataTables.expenseDetailTable.on('order.dt search.dt', function () {
        //    DataTables.expenseDetailTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
        //        cell.innerHTML = i + 1;
        //    });
        //}).draw();

    } catch (x) {

        notyAlert('error', x.message);

    }

});


function GetSaleSummary() {
    try {

      
        var data = { "ExpenseDate": expDate, "DefaultDate": DefaultDate };
        var ds = {};
        ds = GetDataFromServer("Report/GetSale/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
            $("#TotalAmt").text("");
            $("#TotalAmt").text(ds.TotalAmount);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }

}