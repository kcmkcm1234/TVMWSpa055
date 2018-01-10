var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.ChartOfAccounts = $('#ChartOfAccountsTable').DataTable(
         {
             //dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2, 3, 4]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             data: GetAllChartOfAccounts(null),
             pageLength: 10,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "Code", "defaultContent": "<i>-</i>" },
               { "data": "Type", "defaultContent": "<i>-</i>" },
                { "data": "TypeDesc", "defaultContent": "<i>-</i>" },
               { "data": "ISEmploy", "defaultContent": "<i>-</i>" },
               { "data": "IsPurchase", "defaultContent": "<i>-</i>" },
                { "data": "IsReverse", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                  { className: "text-left", "targets": [0, 1, 2, 3, 4, 5] },
             { className: "text-center", "targets": [6] },
               {
                   "render": function (data, type, row) {
                       return (data == false ? "No " : "Yes");
                   },
                   "targets": [3, 4, 5]

               },
                  //{
                  //    "render": function (data, type, row) {
                  //        return (data == false ? "No " : "Yes");
                  //    },
                  //    "targets": 5

                  //},
                    {
                        "render": function (data, type, row) {
                            if (data == "PURCH") {
                                return "Purchase";
                            }
                            else if (data == "OE") {
                                return "Other Expenxes";
                            } else {
                                return "Other Income";
                            }

                        },
                        "targets": 1

                    }

             ]
         });

        $('#ChartOfAccountsTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });


        $(".buttons-excel").hide();



    } catch (x) {

        notyAlert('error', x.message);

    }
});


function InitializeAssignPermissions() {
    try {
        if (DataTables.tblChartOfAccountList != undefined)
        {
            DataTables.tblChartOfAccountList.destroy();
        }        
        DataTables.tblChartOfAccountList = $('#tblAssignmentList').DataTable(
              {
                  dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                  order: [],
                  //scrollY: "500px",
                  //scrollCollapse: true,
                  searching: false,
                  paging: false,
                  data: GetAllChartOfAccounts('OE'),
                  pageLength: 7,
                  language: {
                      search: "_INPUT_",
                      searchPlaceholder: "Search"
                  },
                  columns: [
                    { "data": "Checkbox", "defaultContent": "" },
                    { "data": "Code", "defaultContent": "<i>-</i>" },
                    { "data": "Type", "defaultContent": "<i>-</i>" },
                    { "data": "TypeDesc", "defaultContent": "<i>-</i>" },
                  ],
                  columnDefs: [{ className: 'select-checkbox', targets: 0 },
                      { className: "text-right", "targets": [] },
                      { className: "text-left", "targets": [1, 2, 3] },
                      { className: "text-center", "targets": [0] }
                  ],
                  select: { style: 'multi', selector: 'td:first-child' },
                  rowCallback: function (row, data) {
                      if (data.IsAvailLEReport) {
                          $(row).addClass("selected");
                      }
                  },
                  //initComplete: function (settings, json) {
                  //    debugger;
                  //    for (var i = 0; i < json.data.length; i++) {
                  //        if (json.data[i].IsAvailLEReport) {
                  //            $(row).addClass("selected");
                  //        }
                  //    }
                  //}
              });
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function PrintReport() {
    try {
        debugger;

        $(".buttons-excel").trigger('click');


    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


//---------------------------------------Edit ChartOfAccounts--------------------------------------------------//
function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    openNav("0");
    ResetForm();

    var rowData = DataTables.ChartOfAccounts.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.Code != null)) {
        FillChartOfAccountDetails(rowData.Code);
    }
}

function openNav(id) {
    var left = $(".main-sidebar").width();
    var total = $(document).width();

    $('.main').fadeOut();
    document.getElementById("myNav").style.left = "3%";
    $('#main').fadeOut();

    if ($("body").hasClass("sidebar-collapse")) {

    }
    else {
        $(".sidebar-toggle").trigger("click");
    }
    if (id != "0") {
        ClearFields();
    }
}

function goBack() {
    ClearFields();
    closeNav();
    BindAllChartOfAccounts();
}

//---------------------------------------Fill Chart Of Account Details--------------------------------------------------//
function FillChartOfAccountDetails(Code) {
    debugger;
    ChangeButtonPatchView("ChartOfAccounts", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
    var thisItem = GetChartOfAccountDetailsByCode(Code); //Binding Data
    //Hidden
    debugger;
    $("#Code").val(thisItem.Code);
    $("#Type").val(thisItem.Type);
    $("#Type").prop('disabled', true);
    $("#TypeDesc").val(thisItem.TypeDesc);
    if (thisItem.ISEmploy == true)
    {
        $('#ISEmploy').attr('checked', true);
    }
    else
    {
        $('#ISEmploy').attr('checked', false);
    }
    if (thisItem.IsPurchase == true) {
        $('#IsPurchase').attr('checked', true);
    }
    else {
        $('#IsPurchase').attr('checked', false);
    }
    if (thisItem.IsReverse == true) {
        $('#IsReverse').attr('checked', true);
    }
    else {
        $('#IsReverse').attr('checked', false);
    }
    $("#isUpdate").val("1");
    $("#Code").prop('disabled', true);
    $("#hdnType").val(thisItem.Type);
    $("#hdnCode").val(thisItem.Code);
}

function GetChartOfAccountDetailsByCode(Code) {
    try {

        var data = { "Code": Code };
        var ds = {};
        ds = GetDataFromServer("ChartOfAccounts/GetChartOfAccountDetails/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
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


function Reset() {
    if ($("#isUpdate").val() == "0") {
        ClearFields();
    }
    else {
        FillChartOfAccountDetails($("#hdnCode").val());
    }
}

//-----------------------------------------Reset Validation Messages--------------------------------------//
function ResetForm() {

    var validator = $("#ChartOfAccountsForm").validate();
    $('#ChartOfAccountsForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}


function GetAllChartOfAccounts(type) {
    try {

        var data = {"type":type};
        var ds = {};
        ds = GetDataFromServer("ChartOfAccounts/GetAllChartOfAccounts/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
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

function ClearFields() {
    debugger;
    $("#isUpdate").val("0");
    $('#ISEmploy').prop('checked', false);
    $('#IsReverse').prop('checked', false);
    $('#IsPurchase').prop('checked', false);
    $("#Code").val("");
    $("#Type").val("");
    $("#TypeDesc").val("");
    $("#Code").prop('disabled', false);
    $("#Type").prop('disabled', false);
    $("#hdnCode").val("");
    $("#hdnType").val("");
    ResetForm();
    ChangeButtonPatchView("ChartOfAccounts", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}


function SaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllChartOfAccounts();
            notyAlert('success', JsonResult.Message);
            debugger;
            if ($("#hdnCode").val() != "") {
                FillChartOfAccountDetails($("#hdnCode").val());
            }
            else {
                FillChartOfAccountDetails(JsonResult.Records.Code);
            }
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}

function BindAllChartOfAccounts() {
    try {
        DataTables.ChartOfAccounts.clear().rows.add(GetAllChartOfAccounts()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Save() {
    try {
        $("#btnInsertUpdateChartOfAccounts").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteChartOfAccounts()', '', "Yes, delete it!");
}

function DeleteChartOfAccounts() {
    try {
        var code = $('#hdnCode').val();
        if (code != '' && code != null) {
            var data = { "code": code };
            var ds = {};
            ds = GetDataFromServer("ChartOfAccounts/DeleteChartOfAccounts/", data);
            debugger;
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Message.Message);
                goBack();
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
                return 0;
            }
            return 1;
        }

    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
    }
}

function ShowAssignModal() {
    debugger;
    InitializeAssignPermissions();
    $("#AddAssignChartOfAccountsModel").modal('show');
    $("#editCheckBox").show();
}

function SaveCheckedAssignments() {
        debugger;
        try {
            if (DataTables.tblChartOfAccountList.rows(".selected").data().length == 0) {
                notyAlert('error', "Please select atleast one");
            }
               
                else {
                var SelectedRows = DataTables.tblChartOfAccountList.rows(".selected").data();
                    if ((SelectedRows) && (SelectedRows.length > 0)) {
                        var CheckedCodes = [];
                        for (var r = 0; r < SelectedRows.length; r++) {
                            CheckedCodes.push(SelectedRows[r].Code);
                        }
                    }
                    debugger;
                    var data = { "code": CheckedCodes.join(',')};
                    ds = GetDataFromServer("ChartOfAccounts/UpdateAssignments/", data);
                    if (ds !== '') {
                        ds = JSON.parse(ds);
                    }
                    if (ds.Result === "OK") {
                        DataTables.tblChartOfAccountList.clear().rows.add(GetAllChartOfAccounts()).draw(false);
                        notyAlert('success', "Success");
                        CheckedCodes.length = 0;
                    }
                    if (ds.Result === "ERROR") {
                        debugger;
                        notyAlert('error', ds.Message);
                    }
                }
            }
        
        catch (Ex) {
            notyAlert('error', Ex.message)
            return 0;
        }
    }

