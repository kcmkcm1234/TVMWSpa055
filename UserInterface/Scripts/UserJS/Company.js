var DataTables = {};

$(document).ready(function () {
    try {
        debugger;
        DataTables.CompanyTable = $('#CompanyTable').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             //--For Export button ,but not used now--//
             buttons: [{
                 extend: 'excel',
                 exportOptions:
                              {
                                  columns: [0, 1, 2,3,4,5]
                              }
             }],
             order: [],
             searching: true,
             paging: true,
             data: GetAllCompanies(),
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
               { "data": "Code", "defaultContent": "<i>-</i>" },
               { "data": "Name", "defaultContent": "<i>-</i>" },
               { "data": "BillingAddress", "defaultContent": "<i>-</i>" },
               { "data": "ShippingAddress", "defaultContent": "<i>-</i>" },
               { "data": "ApproverID", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
             ],
             columnDefs: [{ "targets": [4], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [] },
                  { className: "text-left", "targets": [1, 2,3] },
                  { className: "text-center", "targets": [0,5] }

             ]
         });
        //--For editing Company details on table row click---//
        $('#CompanyTable tbody').on('dblclick', 'td', function () {
            
            Edit(this);
        });
        //--To hide inbuilt export button--Export button is not used now---//
        $(".buttons-excel").hide();


    } catch (x) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }

});

//--To Get List of Companies from server --// 

function GetAllCompanies() {
    try {

        var data = {};
        var ds = {};
        ds = GetDataFromServer("Company/GetAllCompanies/", data);
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
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
    //----function to Edit the Compnay details----//
    function Edit(currentObj) {
        debugger;
        openNav("");
        ResetForm();
        var rowData = DataTables.CompanyTable.row($(currentObj).parents('tr')).data();
        if ((rowData != null) && (rowData.Code != null)) {
            FillCompanyDetails(rowData.Code);
        }
    }


    //---------------------------------------Fill Company Details--------------------------------------------------//
    function FillCompanyDetails(Code) {
        
        ChangeButtonPatchView("Company", "btnPatchAdd", "Edit"); //ControllerName,id of the container div,Name of the action
        var thisItem = GetCompanyDetailsByCode(Code); //Binding Data

        $("#Code").val(thisItem.Code);
        $("#Name").val(thisItem.Name);
        $("#BillingAddress").val(thisItem.BillingAddress);
        $("#ShippingAddress").val(thisItem.ShippingAddress);
        $("#ddlApprover").val(thisItem.ApproverID);
        $("#LogoURL").val(thisItem.LogoURL);
        $('#imgid').attr('src', thisItem.LogoURL);
        $("#hdnCode").val(thisItem.Code);
        $("#Code").prop('disabled', true);
    }

    //--function to Get details corresponding to CompanyCode
    function GetCompanyDetailsByCode(Code) {
        try {

            var data = { "Code": Code };
            var ds = {};
            ds = GetDataFromServer("Company/GetCompanyDetailsByCode/", data);
         
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                return ds.Records;
            }
            if (ds.Result == "ERROR") {
                //alert(ds.Message);
                //this will show the error msg in the browser console(F12) 
                console.log(e.message);
            }
        }
        catch (e) {
            //this will show the error msg in the browser console(F12) 
            console.log(e.message);
        }
    }

    //--function to save changes--//
    function Save() {
        try {
         
            $("#btnInsertUpdateCompany").trigger('click');
        }
        catch (e) {
            //this will show the error msg in the browser console(F12) 
            console.log(e.message);
            return 0;
        }
    }

    //--Function onsucess ajax post event for enquiry form save
    function SaveSuccess(data, status) {
        debugger;
        var JsonResult = JSON.parse(data)
        switch (JsonResult.Result) {
            case "OK":
                BindAllCompanies();
                notyAlert('success', JsonResult.Message);

                if ($("#hdnCode").val() != "") {
                    FillCompanyDetails($("#hdnCode").val());
                }
                //else {
                //    FillCompanyDetails(JsonResult.Records.Code);
                //}
                break;
            case "ERROR":
                notyAlert('error', JsonResult.Message);
                break;
            default:
                //this will show the error msg in the browser console(F12) 
                console.log(e.message);
                break;
        }
    }
    //--Clearing Company table and Rebinding it with all Company details --//
    function BindAllCompanies() {
        try {
            debugger;
            DataTables.CompanyTable.clear().rows.add(GetAllCompanies()).draw(false);
        }
        catch (e) {
            //this will show the error msg in the browser console(F12) 
            console.log(e.message);
        }
    }


    function ResetForm() {
        debugger;
    $("btnResetCompany").trigger('click');
}