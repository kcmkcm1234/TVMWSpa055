var DataTables = {};
$(document).ready(function () {
    try{
        debugger;
        $("#prelaoder").show();
        DataTables.expenseDetailTable = $('#expenseDetailTable').DataTable({
                 dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                 order: [],
                 fixedHeader: {
                     header: true
                 },
                 searching: true,
                 paging: true,
                 data: null,
                 pageLength: 2,
                 columns: [
                   {"data": "ErrorRow",
                    render: function (data, type, row) { return ((data !== null) || (data !== undefined)) ?
                             '<div style="color:Red">[' + data + ']</div>' : '<i>-</i>'; },
                    "defaultContent": "<i>-</i>"},
                   { "data": "ExpenseDate", "defaultContent": "<i>-</i>" },
                   { "data": "AccountCode", "defaultContent": "<i>-</i>" },
                   { "data": "Company", "defaultContent": "<i>-</i>" },
                   { "data": "EmpCode", "defaultContent": "<i>-</i>" },
                   { "data": "EmpName", "defaultContent": "<i>-</i>" },
                   { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
                   { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
                   {
                       "data": "Error",
                       render: function (data, type, row) { return ((data !== null) || (data !== undefined)) ?
                           '<div style="color:Red">' + data + '</div>' : '<i>-</i>'; },
                       "defaultContent": "<i>-</i>"
                   }
                 ],
                 columnDefs: [  { className: "text-left", "targets": [ 2, 3, 4, 5, 6, 8] },
                                { "width": "10%", "targets": [1] },
                                { "width": "17%", "targets": [8] },
                                { className: "text-right", "targets": [7] },
                                { className: "text-center", "targets": [0, 1] }]
        });

        DataTables.expenseImportTable = $('#expenseImportTable').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            order: [],
            fixedHeader: {
                header: true
            },
            searching: true,
            paging: true,
            data: null,
            pageLength: 2,
            columns: [
              { "data": "ExpenseDate", "defaultContent": "<i>-</i>" },
              { "data": "AccountCode", "defaultContent": "<i>-</i>" },
              { "data": "Company", "defaultContent": "<i>-</i>" },
              { "data": "EmpCode", "defaultContent": "<i>-</i>" },
              { "data": "EmpName", "defaultContent": "<i>-</i>" },
              { "data": "PaymentMode", "defaultContent": "<i>-</i>" },
              { "data": "Amount", render: function (data, type, row) { return roundoff(data, 1); }, "defaultContent": "<i>-</i>" },
            ],
            columnDefs: [{ className: "text-left", "targets": [2, 3, 4, 5, 1] },
                           { className: "text-right", "targets": [6] },
                           { className: "text-center", "targets": [0] }]
        });

        DataTables.UploadedFilesHistoryTable = $('#tblHistoryList').DataTable({
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            order: [],
            searching: true,
            paging: true,
            data: null,
            pageLength: 8,
            language: {
                search: "_INPUT_",
                searchPlaceholder: "Search"
            },
            columns: [
              { "data": "CreatedDate", "defaultContent": "<i>-</i>" },
              { "data": "FilePath", "defaultContent": "<i>-</i>" },
              { "data": "FileType", "defaultContent": "<i>-</i>" },
              { "data": "RecordCount", "defaultContent": "<i>-</i>" },
              {
                  "data": "FileStatus", render: function (data, type, row) {
                      if (data === "Successfully Imported")
                          return "<span style='color:green'>Successfully Imported</span>"
                      else if (data === "Unvalidated")
                          return "<span style='color:red'>Unvalidated</span>"
                      else return data
                  }, "defaultContent": "<i>-</i>"
              }
            ],
            columnDefs: [
               { className: "text-center","targets":[0] },
               { className: "text-right", "targets": [3] },
               { className: "text-left", "targets": [1, 2, 4] } ]
        });
        Cancel()//Cancel does the all the reset options needed in the initial page loading
    }
    catch (ex) {
        notyAlert('error', ex.message);
        throw ex;
    }
});

//-----------ajax post for Upload files into controller--------------//
function UploadFile(FileObject) {
    try{
        debugger;
        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {
            debugger;
            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                // Create FormData object  
                var fileData = new FormData();

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    debugger;
                    fileData.append(files[i].name, files[i]);
                }
                $.ajax({
                    url: FileObject.Controller,
                    type: "POST",
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    success: function (result) {
                        if (result.Result === "OK") {
                            debugger;
                            if (FileObject.Controller !== "/ImportOtherExpenses/UploadFile") {
                                BindTableOtherExpenses(JSON.parse(result.ImportExpenseList));
                            }
                            $("#prelaoder").hide();
                            if (result.RemovedCount === 0) {
                                $("#btnUpload").prop('disabled',false);
                                $("#AHContainer").hide();
                                $("#ErrorBar").hide();
                                $("#noError").show();
                                BindImportExpenseTable(JSON.parse(result.ImportExpenseList), result.TotalCount, result.TotalCount, result.RemovedCount, result.FileName, result.TotalAmount);
                            }
                            else {
                                $("#ErrorData").show();
                                $("#dataErrorList").text('');
                                $("#dataErrorList").append('<b> Data Error ! </b>');
                                $('#dataError').show();
                            }
                        }
                        else {
                            $("#prelaoder").hide();
                            $("#ErrorBar").show();
                            $("#ErrorList").text('');
                            $("#ErrorList").append('<b>  ' + result.Message + '</b>');

                            if (result.Result === "WARNING") {
                                $("#fileError").show();
                            }
                            else if (result.Result === "EXCEPTION") {
                                $("#sysError").show();
                            }
                        }
                    },
                    error: function (err) {
                        $("#prelaoder").hide();
                        notyAlert('error', err.statusText);
                    }

                });
            }
            else {
                Cancel();
                $("#noFile").show();
            }

        } else {
            $("#prelaoder").hide();
            notyAlert('error','FormData is not supported.');
        }
    } catch (ex) {
        $("#prelaoder").hide();
        notyAlert('error', ex.message);
    }
}

//----------Binding table for removed datas------------//
function BindTableOtherExpenses(ImportExpenseList) {
    try {
        debugger;
        $("#prelaoder").show();
        $("#AHContainer").show();
        $("#SideBar").show();
        $("#finalRow").hide();
        DataTables.expenseDetailTable.clear().rows.add(ImportExpenseList).draw(true);
        $("#prelaoder").hide();
    }
    catch (ex) {
        $("#prelaoder").hide();
        notyAlert("error", ex.message);
    }
}

//----------Binding table for approved datas-----------//
function BindImportExpenseTable(ImportExpenseList, Count, TotalCount, RemovedCount, files, TotalAmount) {
    $("#AHContainer").hide();
    $("#TableContainer").show();
    $('#fileName').text('');
    $('#fileName').append('<span><b> ' + files + '</b></span>');
    $('#TotalRows').text('');
    $('#TotalRows').append('<span><b> ' + TotalCount + '</b></span>');
    $('#TotalAmount').text('');
    $('#TotalAmount').append('<span><b> ' + roundoff(TotalAmount) + '</b></span>');
    $('#InsertedRowCount').text('');
    $('#InsertedRowCount').append('<span><b> ' + Count + '</b></span>');
    $('#RemovedRowCount').text('');
    $('#RemovedRowCount').append('<span><b> ' + RemovedCount + '</b></span>');
    DataTables.expenseImportTable.clear().rows.add(ImportExpenseList).draw(true);
    $("#prelaoder").hide();
}

//--------Function to get data from GetAlluploadedFiles Method on controller----------//
 function FetchHistory() {
    try{
        debugger;
        $("#HistoryModel").modal('show');
        $('a[href="#HistoryList"]').click();
        var ds = {};
        ds = GetDataFromServer("ImportOtherExpenses/GetAllUploadedFile/");
        debugger;
        if (ds !== '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result === "OK") {
            DataTables.UploadedFilesHistoryTable.clear().rows.add(ds.Records).draw(true);
        }
        if (ds.Result === "ERROR") {
            alert(ds.Message);
        }
    }
    catch (ex) {
        notyAlert('error', ex.message);
    }
}

//--------Function to download sample template from DownloadTemplate Method----------//
 function DownloadTemplate() {
    try{
        debugger;
        $.ajax({

            url: '/ImportOtherExpenses/DownloadTemplate',
            type: "GET",
            contentType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',// content type for excel file of office 2007 or more
            success: function (returnValue) {
                debugger;
                window.location = '/ImportOtherExpenses/DownloadTemplate?file=' + returnValue.filename;
                
            },
            error:function(Error){
                notyAlert('error', Error);
            }
        });

    } catch (ex) {
        notyAlert('error', ex.message);
    }
 }

//----------On click of Cancel Button-----------//
 function Cancel() {
     debugger;
     $("#noFile").hide();
     $("#FileUpload1").val('');
     $("#prelaoder").hide();
     $("#AHContainer").hide();
     $("#TableContainer").hide();
     $("#SideBar").hide();
     $("#btnUpload").prop('disabled',true);
     $('#UploadPreview').empty();
     //$("#uploadRow").hide(); //given for testing requirement (not necessarily needed)
     $("#uploadRow").show();
     $("#importRow").hide();
     $("#finalRow").hide(); 
     $("#ErrorBar").hide();
     $("#ErrorData").hide();
     $("#li1").addClass("active");
     $("#li2").removeClass("active");
     $("#li3").removeClass("active");
     $("fileError").hide();
     $("noError").hide();
     $('#dataError').hide();
     $("sysError").hide();
 }

//------------On click of Validate Button------------//
 function Validate() {
     try {
         debugger;
         $("#fileError").hide();
         $("#noError").hide();
         $('#dataError').hide();
         $("#sysError").hide();
         $("#AHContainer").hide();
         $("#SideBar").hide();
         $("#btnUpload").prop('disabled',true);
         $("#prelaoder").show();
         $("#ErrorBar").hide();
         $("#ErrorData").hide();
         $("#importRow").show();
         $("#uploadRow").hide();
         $("#finalRow").hide();
         $("#li2").addClass("active");
         $("#li1").removeClass("active");
         $("#li3").removeClass("active");
         var FileObject = new Object;
         FileObject.Controller = "/ImportOtherExpenses/ValidateUploadFile";
         UploadFile(FileObject);
     }
     catch (ex) {
         notyAlert('error', ex.message);
     }
 }

 //----------On click of import button------------//
 function SaveSuccess() {
     try {
         debugger;
         $("#prelaoder").show();
         $("#uploadRow").hide();
         $("#importRow").hide();
         $("#finalRow").show();
         var FileObject = new Object;
         FileObject.Controller = "/ImportOtherExpenses/UploadFile";
         UploadFile(FileObject);
         $("#li3").addClass("active");
         $("#li1").removeClass("active");
         $("#li2").removeClass("active");
     }
     catch (e) {
         notyAlert('error', e.message);
     }
 }

//------------On chnage of FileUpload-------------//
function BindFileName(){
     try{
         debugger;
         var fileUpload = $("#FileUpload1").get(0);
         var files = fileUpload.files;

         // Looping over all files and add it to FormData object  
         $('#UploadPreview').empty();
         for (var i = 0; i < files.length; i++) {
             var html = '<br/><br/><div class="col-md-12 list" style="font-size:large;"><i class="fa fa-file-excel-o" aria-hidden="true"></i><span> ' + (files[i].name).substring(0, (files[i].name).lastIndexOf('.')) + '</span><a href="javascript:void(0)" fileguid="' + files[i].lastModified + '" onclick="Attachment_Remove(this)" style="float:right;"></a></div>'
             $('#UploadPreview').append(html);
         }
         if (files.length === 0) {
             $('#UploadPreview').append('');
         }
         $("#noFile").hide();
     }
     catch (ex) {
         notyAlert('error', ex.message);
     }
 }