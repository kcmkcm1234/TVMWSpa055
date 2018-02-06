var dataTables = {};
var toDay = '';
var contactUser = '';
var contactNumber = '';
$(document).ready(function () {
    try {
        $('input.timepicker').timepicker({
            timeFormat: 'hh:mm:p',
            interval: 10,
            minTime: '1',
            maxTime: '11:50pm',
            defaultTime: '11',
            startTime: '10:00am',
            dynamic: false,
            dropdown: true,
            scrollbar: true
        });
        $("#ddlCompany,#ddlCustomer").select2({

        });

        dataTables.CustomerExpeditingDetailTable = $('#CustomerExpeditingDetailTable').DataTable(
         {

             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [1,2,3, 5, 6, 7,8,9,10]
                              }
             }],
             order: [],
             fixedHeader: {
                 header: true
             },
             searching: true,
             ordering: false,
             paging: true,
             data: GetCustomerExpeditingDetail(),
             pageLength: 50,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
                { "data": "CustomerObj.ID", "defaultContent": "<i></i>" },
               { "data": "CustomerName", "defaultContent": "<i></i>", "width": "10%" },
               { "data": "customerContactObj.ContactName", "defaultContent": "<i></i>", "width": "10%" },
               { "data": "ContactNo", "defaultContent": "<i>-</i>", "width": "10%" },
               { "data": null, "orderable": false, 
               render: function (data, type, row) {
                   if(row.CustomerName)
                   return '<a href="#" class="actionLink"  onclick="Edit(this)" >Add/Edit</a>'
               else
                    return ''}

                   , "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" >Add/Edit</a>'
               },
               { "data": "companyObj.Name", "defaultContent": "<i>-<i>"  },
               { "data": "InvoiceNo", "defaultContent": "<i>-</i>" },
               { "data": "InvoiceDate", "defaultContent": "<i>-</i>" },
               { "data": "PaymentDueDate", "defaultContent": "<i>-</i>" },
               { "data": "NoOfDays", "defaultContent": "<i>-</i>" },
               { "data": "Amount", "defaultContent": "<i>-</i>", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>"}
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false },                
                  { className: "text-left", "targets": [1, 2, 4,5,6,9] },
                 
             { className: "text-right", "targets": [ 10] },
             { className: "text-center", "targets": [ 7,4] }
             ]
         });
        $(".buttons-excel").hide();
        toDay = $("#todate").val();
    } catch (x) {

        notyAlert('error', x.message);

    }

});

function GetCustomerExpeditingDetail() {
    try {
        debugger;
        var toDate = $("#todate").val();
        var filter = $("#ddlBasicFilters").val();
        var company = $("#ddlCompany").val();
        var customer = $("#ddlCustomer").val();
        var outstanding = $("#ddlOutStanding").val();
        if (IsVaildDateFormat(toDate))
            var data = { "ToDate": toDate, "Filter": filter, "Company": company, "Customer": customer, "outstanding": outstanding };
        var ds = {};
        ds = GetDataFromServer("PaymentFollowup/GetCustomerPaymentExpeditingDetails/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records.customerExpeditingDetailsList;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function OnCallChange() {
    debugger;
    RefreshCustomerExpeditingDetailTable();
}

//To refresh based on filter
function RefreshCustomerExpeditingDetailTable() {
    debugger;
    try {
        var toDate = $("#todate").val();
        var filter = $("#ddlBasicFilters").val();
        var company = $("#ddlCompany").val();
        var customer = $("#ddlCustomer").val();
        var outstanding = $("#ddlOutStanding").val();
        if (dataTables.CustomerExpeditingDetailTable != undefined && IsVaildDateFormat(toDate)) {
            dataTables.CustomerExpeditingDetailTable.clear().rows.add(GetCustomerExpeditingDetail()).draw(true);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

//To trigger export button
function PrintReport() {
    try {
        $(".buttons-excel").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function Edit(currentObj) {
    debugger;
    $('#divflist').show();
    $('#followUpResetbtn').hide();
    var rowData = dataTables.CustomerExpeditingDetailTable.row($(currentObj).parents('tr')).data();
    if ((rowData != null) && (rowData.customerObj.ID != null)) {
        FollowUpList(rowData.customerObj.ID);
        $("#lblCustomer").text(rowData.CustomerName);
        $("#lblContact").text(rowData.customerContactObj.ContactName);
        var str = rowData.ContactNo;
        if (str.length > 35) str = str.substring(0, 35);
        $("#lblmobile").text(str);
        $("#CustomerID").val(rowData.customerObj.ID);
        $("#lblmobile").attr('title', rowData.ContactNo);
        contactUser = rowData.customerContactObj.ContactName
        contactNumber=rowData.Mobile
        FollowUp(1);
    }
}

//--To Diplay Default Contact Details--//
function ShowDefaultContact()
{
    debugger;
    if($("#txtContactName").val()=='')
        $("#txtContactName").val(contactUser);
    if ($("#txtContactNO").val() == '')
        $("#txtContactNO").val(contactNumber);
}
 //--To Get FollowUp list from server corresponding to an CustomerID--//
 function FollowUpList(ID) {
    debugger;
    var data = { CustomerID: ID };
    var ds = {};
    ds = GetDataFromServer('PaymentFollowup' + "/FollowUp/", data);
    if (ds == "Nochange") {
        return; 0
      }
    $("#divflist").empty();
    $("#divflist").html(ds);

    }
//To reset 
function Reset() {
    debugger;
    $("#todate").val(toDay);
    $("#ddlBasicFilters").val('ALL');
    $("#ddlCompany").val('ALL').trigger('change');
    $("#ddlCustomer").val('').trigger('change');
    $("#ddlOutStanding").val('Outstanding');
    RefreshCustomerExpeditingDetailTable();
}

function FollowUp(flag, status) {
    //--This function have 2 parameters ,
    //'flag' checks whether new followup is added or edit action is performed 
    //'status' checks whether the  element is first record  or not                                  
    debugger;
    //--To Disable,enable textbox and display FollowUp modal PopUp on load  --//
    if ((flag == 1)) {
        var Count = $('#hdnCountOpen').val()
        if (parseInt(Count) !== 0) {
            $('#ModelReset').trigger('click');
            $("#hdnFollowUpID").val(ID);
            FollowUpClosed();
            $('#followUpObj_Status').prop("disabled", true);
            $('#followUpObj_Status').attr('onchange', 'EnableTextbox("new")');
            $("#btnFollowUps").modal('show');
            $('#divfollowUpDeletebtn').hide();
            $('#followUpDeletebtn').hide();
            $('#followUpResetbtn').hide();
            $('#btnSave').hide();
            $('#lblMsg').text("Cannot Add New Follow Up, Open FollowUp Exists..!");
           
        }
        else {
            $('#ModelReset').trigger('click');
            $("#hdnFollowUpID").val(ID);
            $("#ddlCustomer").val();
            FollowUpOpen();
            $('#followUpObj_Status').prop("disabled", false);
            $('#followUpObj_Status').attr('onchange', 'EnableTextbox("new")');
            $("#btnFollowUps").modal('show');
            $('#divfollowUpDeletebtn').hide();
            $('#followUpDeletebtn').hide();
            $('#followUpResetbtn').show();
            $('#btnSave').show();
            $('#lblMsg').text('');
        }
    }
    else {
        //--To disable textbox and display FollowUp modal PopUp on Edit Button Click --//
        var ID = flag;
        debugger;
        $("#btnFollowUps").modal('show');
        $("#hdnFollowUpID").val(ID);
        $('#followUpObj_Status').attr('onchange', 'EnableTextbox("Edit")');

        FillFollowUpDetails(ID);
        if (status == 1) {
            debugger;
            $('#followUpObj_Status').prop("disabled", false);
            $('#divfollowUpDeletebtn').show();
            $('#followUpDeletebtn').show();
            $('#btnSave').show();
            $('#followUpResetbtn').show();
            EnableTextbox("Edit");
            $('#btnSave').show();
        }
        else {
            $('#followUpObj_Status').prop("disabled", true);
            $('#divfollowUpDeletebtn').hide();
            $('#followUpDeletebtn').hide();
            $('#followUpResetbtn').hide();
            $('#btnSave').hide();
            $('#followUpResetbtn').hide();
        }
    }
}

//--Saves Follow Up to the server by triggering hidden button--//
function SaveFollowUp() {
    try {
        var time = hrsTo24hrormat();
        $("#hdnFollowUpTime").val(time);
        $("#btnFollowUpSave").trigger('click');
    }
    catch (e) {
        notyAlert('error', e.Message);   //--Shows error id save is failed--//
    }

}

//Function onsucess ajax post event for follow Up form save
function FollowUpSaveSuccess(data) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            debugger;
            FollowUpList(JsonResult.Records.CustomerID);
            var Count = $('#hdnCountOpen').val()
            ClearFollowUp();
            FollowUp(1);
            //$('#followUpResetbtn').hide();
            notyAlert('success', JsonResult.Message);
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}

//--Function to Convert AM,PM format to 24 hour time format-----//
function hrsTo24hrormat() {
    try {
        debugger;
        var h = 0;
        var addTime = 12;
        var time = $("#FollowUpTime").val();
        var hours = parseInt(time.split(":")[0]);
        var minutes = parseInt(time.split(":")[1]);
        var AMPM = time.split(":")[2];

        if (AMPM == "PM" && hours < 12) {
            hours = parseInt(hours) + parseInt(addTime);
        }
        if (AMPM == "AM" && hours == 12) {
            hours = parseInt(hours) - parseInt(addTime);
        }
        var h = hours;
        var sHours = hours.toString();
        var sMinutes = minutes.toString();
        if (h < 10) sHours = sHours;
        if (minutes < 10) sMinutes = sMinutes;
        return sHours + ":" + sMinutes;
    }
    catch (e) {
        noty({ type: 'error', text: e.message });
    }
}
//--Fills the Edit FollowUp form  with details corresponding to FollowUp ID--//
function FillFollowUpDetails(ID) {
    debugger;

    var thisItem = GetFollowUpDetailsByFollowUpID(ID); //Binding Data
    debugger;
    if (thisItem.Status == "Closed") {
        FollowUpClosed()
    }
    else {
        FollowUpOpen()
    }
    $('#FollowUpDate').val(thisItem.FollowUpDate);
    $('#FollowUpTime').val(thisItem.FollowUpTime);
    $('#txtContactName').val(thisItem.ContactName);
    $('#txtContactNO').val(thisItem.ContactNO);
    $('#followUpObj_Status').val(thisItem.Status);
    $('#Status').val(thisItem.Status);
    $('#txtRemarks').val(thisItem.Remarks);
    $('#btnSave').show();
    $('#lblMsg').text('');
}

function FollowUpClosed() {
    $('#FollowUpDate').prop("readonly", true);
    $('#FollowUpDate').unbind('focus');
    $('#FollowUpTime').prop("disabled", true);
    $('#txtContactName').prop("readonly", true);
    $('#txtContactNO').prop("readonly", true);
    $('#txtRemarks').prop("readonly", true);
    $('#Status').prop("disabled", true);
    $('#followUpResetbtn').hide();
}
function FollowUpOpen() {
    $('#FollowUpDate').prop("readonly", false);
    $("#FollowUpDate").bind('focus',function () {
       $("#FollowUpDate").datepicker('show');
    });
    $('#FollowUpTime').prop("disabled", false);
    $('#txtContactName').prop("readonly", false);
    $('#txtContactNO').prop("readonly", false);
    $('#txtRemarks').prop("readonly", false);
    $('#Status').prop("disabled", false);
    $('#followUpResetbtn').show();
    $('#followUpObj_Status').prop("disabled", false);
}

function EnableTextbox(flag) {
    debugger;
    if (flag === "Edit") {
        if ($('#followUpObj_Status').val() == 'Open') {
            FollowUpOpen()
        }
        else if ($('#followUpObj_Status').val() == 'Closed') {
            FollowUpClosed()
        }
    }
}

function ResetFollowup() {
    debugger;
    var ID = $("#hdnFollowUpID").val()
    $('#ModelReset').trigger('click');
    if (ID)
        FollowUp(ID, 1);
}

//--To Get FollowUp details from server corresponding to  ID--//
function GetFollowUpDetailsByFollowUpID(ID) {
    try {
        debugger;

        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("PaymentFollowup/GetFollowUpDetailByFollowUpId/", data);
        debugger;
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
    catch (e) {
        notyAlert('error', e.message);
    }
}

//--Delete Followup---//
function DeleteFollowUp() {
    notyConfirm('Are you sure to delete?', 'FollowUpDelete();');
}

function FollowUpDelete() {
    try {
        debugger;
        var ID = $('#hdnFollowUpID').val();
        if (ID != '' && ID != null) {
            var data = { "ID": ID };
            var ds = {};
            ds = GetDataFromServer("PaymentFollowup/DeleteFollowUp/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                debugger;
                notyAlert('success', ds.Record.Message);
                if ($('#followUpObj_Status').val("Closed")){
                    $('#followUpObj_Status').val("Open");
                    FollowUpOpen();
                }
                $('#hdnFollowUpID').val('');
                var id = $('#CustomerID').val();
                ClearFollowUp();
                FollowUpList(id);
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

function ClearFollowUp()
{
    $("#FollowUpDate").val('');
    $("#FollowUpTime").val('');
    $("#txtContactName").val('');
    $("#txtContactNO").val('');
    $("#txtRemarks").val('');
    $('#divflist').show();
    $('#followUpResetbtn').hide();
    $('#divfollowUpDeletebtn').hide();
    $('#followUpDeletebtn').hide();
}