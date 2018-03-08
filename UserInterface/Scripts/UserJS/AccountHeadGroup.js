var DataTables = {};
var emptyGUID = '00000000-0000-0000-0000-000000000000'
$(document).ready(function () {
    try {

        DataTables.AccountHeadGroupTable = $('#AccountHeadGroupTable').DataTable(
         {
             //dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [1, 2 ]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             data: GetAllAccountHeadGroup(),
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "ID" },
               { "data": "GroupName", "defaultContent": "<i>-</i>" },
               { "data": "AccountHeads", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { "width": "15%", "targets": [] },
                 { className: "text-right", "targets": [] },
                  { className: "text-left", "targets": [1,2] },
            { className: "text-center", "targets": [] }

             ],
         });

        $('#AccountHeadGroupTable tbody').on('dblclick', 'td', function () {

            Edit(this);
        });
       
        $(".buttons-excel").hide();
         

    } catch (x) {

        notyAlert('error', x.message);

    }

});


function GetAllAccountHeadGroup() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("AccountHeadGroup/GetAllAccountHeadGroup/", data);
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

function Save() {
    debugger;
    try {
        var selected = [];
        $('#accountCodeCheckBox input:checked').each(function () {
            selected.push($(this).attr('id'));
        });
        $("#AccountHeads").val(selected.toString());
        $("#btnInsertUpdateAccountHeadGroup").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
        return 0;
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

function Edit(currentObj) {
    //Tab Change on edit click
    debugger;
    ResetForm();
    var rowData = DataTables.AccountHeadGroupTable.row($(currentObj).parents('tr')).data();
    FillAccountHeadGroupDetails(rowData.ID);
    DisableCode(rowData.ID);
    openNav();
}

function FillAccountHeadGroupDetails(ID) {
    debugger;
    ChangeButtonPatchView("AccountHeadGroup", "btnPatchAdd", "Edit");
    //ControllerName,id of the container div,Name of the action
    var thisItem = GetAccountHeadGroupDetailsByID(ID); //Binding Data
    //Hidden
    debugger;
    $("#ID").val(thisItem.ID);
    $("#GroupName").val(thisItem.GroupName);
    // $("#AccountHeads").val(thisItem.AccountHeads);
    if (thisItem.AccountHeads != null) {
        var AHGarray = thisItem.AccountHeads.split(",");
        $('input:checkbox').prop('checked', false);
        for (var i = 0 ; i < AHGarray.length; i++) {
            $("#" + AHGarray[i].trim()).prop('checked', true);
        }
    }
}

function GetAccountHeadGroupDetailsByID(ID) {
    try {

        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("AccountHeadGroup/GetAccountHeadGroupDetailsByID/", data);
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

function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            BindAllAccountHeadGroup();
            //ClearFields();
            //DisableCode();
            notyAlert('success', JsonResult.Message);
            debugger;
            //if ($("#ID").val() != "") {
            //    FillAccountHeadGroupDetails($("#ID").val());
            //}
            //else {
            //    FillAccountHeadGroupDetails(JsonResult.Records.ID);
            //}
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}

function BindAllAccountHeadGroup() {
    try {
        DataTables.AccountHeadGroupTable.clear().rows.add(GetAllAccountHeadGroup()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Delete() {
    notyConfirm('Are you sure to delete?', 'DeleteAccountHeadGroup()', '', "Yes, delete it!");
}

function DeleteAccountHeadGroup() {
    debugger;
        try {
            var ID = $('#ID').val();
            if (ID != '' && ID != null) {
                var data = { "ID": ID };
                var ds = {};
                ds = GetDataFromServer("AccountHeadGroup/DeleteAccountHeadGroup/", data);
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


function ClearFields() {
    $("#ID").val("");
    $("#AccountHeads").val("");
    $('#accountCodeCheckBox input:checked').each(function () {
        $('input:checkbox').prop('checked', false);
    });
    $("#GroupName").val("");
    ResetForm();
    ChangeButtonPatchView("AccountHeadGroup", "btnPatchAdd", "Add"); //ControllerName,id of the container div,Name of the action
}

function ResetForm() {

    var validator = $("#AccountHeadGroupForm").validate();
    $('#AccountHeadGroupForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    validator.resetForm();
}

function Reset() {
    if ($("#ID").val() == "0") {
        ClearFields();
    }
    else {
        FillAccountHeadGroupDetails($("#ID").val());
    }
    ResetForm();
}

function goBack() {
    ClearFields();
    closeNav();
    BindAllAccountHeadGroup();
}

function AddNew()
{
    openNav();
    ClearFields();
    DisableCode();
}

function GetDisabledCode(ID)
{
    debugger;
    try {
    var data = { "ID": ID};
    var ds = {};
    ds = GetDataFromServer("AccountHeadGroup/GetDisabledCodeForAccountHeadGroup/", data);
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

function DisableCode(ID) 
{
    debugger;
    $('input:checkbox').prop('disabled', false);
    var thisItem = GetDisabledCode(ID)
    if (thisItem.length > 0) {
        for (var i = 0 ; i < thisItem.length; i++) {
            $("#" + thisItem[i].AccountHeads.trim()).prop('disabled', true);
        }
    }
}