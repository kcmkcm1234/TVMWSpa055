var DataTables = {};
$(document).ready(function () {
    debugger;
    try {
        DataTables.tblbankWiseBalanceTable = $('#tblbankWiseBalanceTable').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
            paging: true,
            data: GetBankWiseBalance(),
            pageLength: 10,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
              { "data": "BankCode", "defaultContent": "<i>-</i>" },
              { "data": "BankName", "defaultContent": "" },
              { "data": "TotalAmount", render: function (data, type, row) { return formatCurrency(data); }, "defaultContent": "<i>-</i>" },
              {
                  "data": "UnClearedAmount", render: function (data, type, row) {
                      return formatCurrency(data);
                  }, "defaultContent": "<i>-</i>"
              },
              {
                  "data": "TotalAmount", render: function (data, type, row) {
                      return formatCurrency(row.TotalAmount + row.UnClearedAmount);
                  }, "defaultContent": "<i>-</i>"
              },

            ],
            columnDefs: [
                 { className: "text-right", "targets": [2,3,4] },
                 { className: "text-left", "targets": [0, 1] },
                 { className: "text-center", "targets": [] },
            ]
        });
       

    } catch (x) {

        notyAlert('error', x.message);
    }
});


function GetBankWiseBalance() {
    try {

        debugger;
        var Date = $("#fromdate").val();
        if (Date != "" && IsVaildDateFormat(Date)) {
            var data = { "Date": Date };
            var ds = {};
            ds = GetDataFromServer("OtherExpenses/GetBankWiseBalance/", data);
            ds = JSON.parse(ds);
            $("#TotalBlnce").text("");
            $("#TotalBlnce").text(ds.TotalAmount);
            $("#TotalUnClrAmt").text("");
            $("#TotalUnClrAmt").text(ds.TotalUnClrAmt);
            $("#ActualBlnce").text("");
            $("#ActualBlnce").text(ds.ActualBlnce);
            if (ds.Result == "OK") {
                return ds.Records;
            }
            if (ds.Result == "ERROR") {
                alert(ds.Message);
            }
        }

    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function BankwiseBalance() { 

    DataTables.tblbankWiseBalanceTable.clear().rows.add(GetBankWiseBalance()).draw(false);

}