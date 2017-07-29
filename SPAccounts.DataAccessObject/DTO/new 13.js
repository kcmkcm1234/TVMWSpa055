//Save priest (functon with insert and update)
//function savePriest()
//{
//    try
//    {
//        debugger;
//        var AppImgURL = '';
//        var priestID = $("#hdfPriestID").val();

//        //-----------------------INSERT-------------------//

//        if (priestID == null || priestID == "") {
//            var guid = createGuid();

//            if (guid != null) {
//                var i = "0";
//                var imgresult = "";
//                var _URL = window.URL || window.webkitURL;
//                var formData = new FormData();
//                var imagefile, logoFile, img;

//                if (((imagefile = $('#priestimg')[0].files[0]) != undefined)) {
//                    var formData = new FormData();
//                    var tempFile;
//                    if ((tempFile = $('#priestimg')[0].files[0]) != undefined) {
//                        tempFile.name = guid;
//                        formData.append('NoticeAppImage', tempFile, tempFile.name);
//                        formData.append('GUID', guid);
//                        formData.append('createdby', 'sadmin');
//                    }
//                    formData.append('ActionTyp', 'NoticeAppImageInsert');
//                    AppImgURL = postBlobAjax(formData, "../ImageHandler/UploadHandler.ashx");
//                    i = "1";
//                }

//            }

//            var Priest = new Object();
//            Priest.priestName = $('#txtPriestName').val();
//            Priest.BaptisumName = $('#txtPriestBaptismName').val();
//            Priest.Parish = $('#txtParish').val();
//            Priest.Diocese = $('#txtDiocese').val();
//            Priest.Status = $('#ddlstatus').val();       
//            Priest.dob = $('#priestDOB').val();
//            Priest.about = $('#txtAboutPriest').val();
//            Priest.dateOrdination = $('#OrdinationDate').val();
//            Priest.designation = $('#txtDesignation').val();
//            Priest.address = $('#txtAddress').val();
//            Priest.emailId = $('#txtEmail').val();
//            Priest.mobile = $('#txtMobile').val();
//            if (i == "1")
//            {
//                Priest.imageId = guid;
//            }
//            Priest.priestID = guid;
//            result = InsertPriest(Priest);

//            if (result.result == "1") {
//                noty({ text: Messages.SavedSuccessfull, type: 'success' });

//            }
//            else
//            {
//                noty({ text: result.result, type: 'error' });
//            }

//            $('#assVicardiv').remove();
//            $("<div id='assVicardiv'><div id='AsstVicarDefault'></div></div>").appendTo("#AsstVicartask");
//            check();


//        }
//            //-----------------------UPDATE-------------------//
//        else {
//            var Priest = new Object();
//            Priest.priestName = $('#txtPriestName').val();
//            Priest.BaptisumName = $('#txtPriestBaptismName').val();
//            Priest.Parish = $('#txtParish').val();
//            Priest.Diocese = $('#txtDiocese').val();
//            Priest.Status = $('#ddlstatus').val();
//            Priest.dob = $('#priestDOB').val();
//            Priest.about = $('#txtAboutPriest').val();
//            Priest.dateOrdination = $('#OrdinationDate').val();
//            Priest.designation = $('#txtDesignation').val();
//            Priest.address = $('#txtAddress').val();
//            Priest.emailId = $('#txtEmail').val();
//            Priest.mobile = $('#txtMobile').val();
//            Priest.priestID = $("#hdfPriestID").val();
//            var guid = createGuid();
//            if (((imagefile = $('#priestimg')[0].files[0]) != undefined)) {
//                var formData = new FormData();
//                var tempFile;
//                if ((tempFile = $('#priestimg')[0].files[0]) != undefined) {
//                    tempFile.name = guid;
//                    formData.append('NoticeAppImage', tempFile, tempFile.name);
//                    formData.append('GUID', guid);
//                    formData.append('createdby', 'sadmin');
//                }
//                formData.append('ActionTyp', 'NoticeAppImageInsert');
//                AppImgURL = postBlobAjax(formData, "../ImageHandler/UploadHandler.ashx");
//                Priest.imageId = guid;
//            }

//            result = UpdatePriest(Priest);

//            if (result.result == "1") {
//                noty({ text: Messages.UpdationSuccessFull, type: 'success' });
//            }
//            else
//            {
//                noty({ text: result.result, type: 'error' });
//            }
//            $('#assVicardiv').remove(); 
        
//            $("<div id='assVicardiv'><div id='AsstVicarDefault'></div></div>").appendTo("#AsstVicartask");
//            check();
//        }
//    }
//    catch(e)
//    {
//        noty({ type: 'error', text: e.message });
//    }
//}