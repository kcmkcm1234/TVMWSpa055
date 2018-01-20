//Declare Global Variables
var DataTables = {};
////var startDate = '';
////var endDate = '';

$(document).ready(function () {
    try {

        DataTables.OutGoingChequeTable = $('#OutGoingChequeTable').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                    {
                        columns: [1, 2, 3, 4,5,6,7,8,9]
                    }
             }],
             order: [],
             searching: false,
             paging: true,
             data: GetOutGoingCheques(0),
             pageLength: 50,            
             columns: [
               { "data": "ID", "defaultContent": "<i>-</i>" },
               { "data": "ChequeNo", "defaultContent": "<i>-</i>" },
               { "data": "ChequeDate", "defaultContent": "<i>-</i>"},
               { "data": "Bank","defaultContent": "<i>-</i>"},
               {
                  "data": "Amount", render: function (data, type, row) {
                                      return roundoff(data, 1);
                                  }, "defaultContent": "<i>-</i>"
                              },
               { "data": "Party", "defaultContent": "<i>-</i>", "width": "10%" },
               { "data": "Status", "defaultContent": "<i>-</i>" },
                { "data": "Remarks", "defaultContent": "<i>-</i>", "width": "20%" },
               { "data": "Company", "defaultContent": "<i>-</i>" },
               { "data": "CreatedDate", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditRecord(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [0], "visible": false, "searchable": false }, 
                  { className: "text-left", "targets": [1] },
                  { className: "text-right", "targets": [4] },
                  { className: "text-center", "targets": [2,3,6,5,7,8,9] }]
         });
        $(".buttons-excel").hide();
        startDate = $("#todate").val();
        endDate = $("#fromdate").val();

    } catch (x) {

        //this will show the error msg in the browser console(F12) 
        console.log(x.message);
    }
});


//Bind Values to Datatable
function GetOutGoingCheques(outGoingChequesAdvanceSearchObject) {
    debugger;
    try {
        if (outGoingChequesAdvanceSearchObject === 0) {
            var data = {};
        }
        else {
            var data = { "outGoingChequesAdvanceSearchObject": JSON.stringify(outGoingChequesAdvanceSearchObject) };
        }
        var ds = {};
        ds = GetDataFromServer("DepositAndWithdrawals/GetOutGoingCheques/", data);
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
        console.log(e.message);
    }
}


//filter the table based on advanced search
function FilterContent() {
    debugger;
    var fromDate = $("#fromdate");
    var toDate = $("#todate");
    var company = $("#CompanyCode");
    var status = $("#ddlStatus");
    var search = $("#txtOutSearch");
    var OutgoingChequeAdvanceSearch = new Object();
    OutgoingChequeAdvanceSearch.FromDate = fromDate[0].value !== "" ? fromDate[0].value : null;
    OutgoingChequeAdvanceSearch.ToDate = toDate[0].value !== "" ? toDate[0].value : null;
    OutgoingChequeAdvanceSearch.Status = status[0].value !== "" ? status[0].value : null;
    OutgoingChequeAdvanceSearch.Search = search[0].value !== "" ? search[0].value : null;
    OutgoingChequeAdvanceSearch.Company = company[0].value !== "" ? company[0].value : null;
    DataTables.OutGoingChequeTable.clear().rows.add(GetOutGoingCheques(OutgoingChequeAdvanceSearch)).draw(false);   
}

//Add form for inserting new elements
function AddNew()
{
    debugger;
    Resetform();
    openNav();
    ChangeButtonPatchView('DepositAndWithdrawals', 'btnPatchAdd', 'AddSub');

    
}

//Save the added data to database
function SaveForm() {
    debugger;
  
    ValidateChequeNo();
}



//Message alerts for Save Sucess or Failure
function SaveSuccessOutGoingCheque(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            {
                notyAlert('success', JsonResult.Message);
            }
            $('#OutGoingObj_ID').val(JsonResult.Records.ID);
            BindAllOutgoingCheques();
          
            //List();
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}


//Get Data based on ID
function GetOutgoingCheques(ID) {
    debugger;
    try {
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("DepositAndWithdrawals/GetOutgoingChequeById/", data);
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

//Opens Edit form to edit data
function EditRecord(Obj) {
    debugger;
    ChangeButtonPatchView('DepositAndWithdrawals', 'btnPatchAdd', 'EditOutGoing');
    var rowData = DataTables.OutGoingChequeTable.row($(Obj).parents('tr')).data();
    $('#OutGoingObj_ID').val(rowData.ID);
    PaintOutGoingCheques(rowData.ID);
    openNav();
}


//Resets the form
function Resetform() {
    $("#OutGoingObj_ID").val("");
    var validator = $("#OutgoingForm").validate();
    $('#OutgoingForm').find('.field-validation-error span').each(function () {
        validator.settings.success($(this));
    });
    $('#OutgoingForm')[0].reset();
}

//Fills data on required fields for editing  
function PaintOutGoingCheques(ID) {
    debugger;
    var thisItem = GetOutgoingCheques(ID);
    if (thisItem) {
        $('#OutGoingObj_ID').val(thisItem.ID);
        $('#hdnOutgoingChequeDeleteID').val(thisItem.ID);
        $('#txtChequeNo').val(thisItem.ChequeNo);
        $('#txtChequeDate').val(thisItem.ChequeDate);
        $('#ddlBank').val(thisItem.Bank);
        $('#ddlCompany').val(thisItem.Company);
        $('#ddlStatusType').val(thisItem.Status);
        $('#txtAmount').val(thisItem.Amount);
        $('#txtParty').val(thisItem.Party);
        $('#OutGoingObj_Remarks').val(thisItem.Remarks);

    }
}

//Deletes the Current record
function DeleteOutgoingCheque() {
    debugger
    notyConfirm('Are you sure to delete?', 'DeleteRecord()', '', "Yes, delete it!");
}

//To trigger the delete button
function DeleteRecord() {
    debugger
    $('#btnFormDelete').trigger('click');
}

//Shows Success or Failure Message
function DeleteSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            Resetform();
            BindAllOutgoingCheques();
            notyAlert('success', JsonResult.Records.Message);
            break;
        case "Error":
            notyAlert('error', JsonResult.Records.Message);
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Records.Message);
            break;
        default:
            break;
    }
}



//Bind the Datatable
function BindAllOutgoingCheques() {
    try {
       
        DataTables.OutGoingChequeTable.clear().rows.add(GetOutGoingCheques()).draw(false);
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


//Print the table in excel format
function PrintReport() {
    try {
        $(".buttons-excel").trigger('click');

    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

//Resets advanced Search
function FilterReset() {
  
    $("#todate").val('');
    $("#fromdate").val('');
    $("#CompanyCode").val('')
    $("#ddlStatus").val('');
    $("#txtOutSearch").val('');
  
    BindAllOutgoingCheques();
}

//Saves the validated data
function SaveValidatedData() {
    debugger;
    $(".cancel").click();
    setTimeout(function () {
        $('#btnSave').trigger('click');
    }, 1000);
}




//Validate Whether same chequeno exists or not

function ValidateChequeNo() {
    debugger;
    var OutGoingChequesViewModel = new Object();
    OutGoingChequesViewModel.ChequeNo = $("#txtChequeNo").val();
    OutGoingChequesViewModel.ID = $("#OutGoingObj_ID").val();
    OutGoingChequesViewModel.Bank = $("#ddlBank").val();
    var data = "{'outGoingChequeObj': " + JSON.stringify(OutGoingChequesViewModel) + "}";
    PostDataToServer("DepositAndWithdrawals/ValidateChequeNo/", data, function (JsonResult) {
        debugger;
        if (JsonResult != '') {
            switch (JsonResult.Result) {
                case "OK":
                    if (JsonResult.Records.Status == 1)
                        notyConfirm(JsonResult.Records.Message, 'SaveValidatedData();', '', "Yes,Proceed!", 1);
                    else {
                        SaveValidatedData();
                    }
                    break;
                case "ERROR":
                    notyAlert('error', JsonResult.Message);
                    break;
                default:
                    break;
            }
        }
    });
}