var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
//(function Checker() {
//    var flag = false;
//    $.ajax({
//        url: appAddress + 'Account/AreyouAlive/',
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (data) {

//            if (data.Result == "OK") {
//                switch (data.Record) {
//                    case "dead":
//                        $('.modal').modal('hide');
//                        $("#RedirectToLoginModel").modal('show');
  
//                        flag = true;
//                        break;
//                    case "alive":
//                        flag = false;
//                        break;
//                }


//            }
//            if (data.Result == "ERROR") {
//                notyAlert('error', data.Message);
//            }

//        },
//        complete: function () {
//            // Schedule the next request when the current one's complete
//            //  setTimeout(Checker, 126000);
//            if (flag != true) {
//                //for 15.2 minutes
//                setTimeout(Checker, 912000);
//               // setTimeout(Checker, 126000);
//            }

//        }
//    });
//})();


$(document).ready(function () {
    debugger;
    $('input.datepicker').datepicker({
        format: "dd-M-yyyy",//",
        maxViewMode: 0,
        todayBtn: "linked",
        clearBtn: true,
        autoclose: true,
        todayHighlight: true
    });
   
    $('input').keydown(function (e) {
        var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
        if (key == 13) {
            e.preventDefault();
            var inputs = $(this).closest('form').find(':input:visible');
            inputs.eq(inputs.index(this) + 1).focus();
        }
    });
   
    //menu submenu popup on click 3rd level menus
    $('.navbar a.dropdown-toggle').on('click', function (e) {
        var $el = $(this);
        var $parent = $(this).offsetParent(".dropdown-menu");
        $(this).parent("li").toggleClass('open');

        if (!$parent.parent().hasClass('nav')) {
            $el.next().css({ "top": $el[0].offsetTop, "left": $parent.outerWidth() - 4 });
        }

        $('.nav li.open').not($(this).parents("li")).removeClass("open");

        return false;
    });
   
    $(".dropdown, .btn-group").hover(function () {
       
        var dropdownMenu = $(this).children(".dropdown-menu");
        if (dropdownMenu.is(":visible")) {
            dropdownMenu.parent().toggleClass("open");
        }
    });

    $('.BlockEnter').keydown(function (e) {
    
        try {
            if (e.which === 13) {
                var index = $('.BlockEnter').index(this) + 1;
                $('.BlockEnter').eq(index).focus();
                e.preventDefault();
                
                return false;
            }
        } catch (e) {

        }

    });
   
});

function notyAlert(type, msgtxt,title) {
    var t = '';
    if (title == undefined) {
        t = type;
    }
    else {
        t = title;
    }

    swal({ title: t, text: msgtxt, type: type, timer: 6000 });
    //var n = noty({
    //    text: msgtxt,
    //    type: type,//'alert','information','error','warning','notification','success'
    //    dismissQueue: true,
    //    timeout: 3000,
    //    layout: 'center',
    //    theme: 'defaultTheme',//closeWith: ['click'],
    //    maxVisible: 5
    //});
   
}

function SelectAllValue(e) {
    $(e).select();
}
function PostDataToServer(page, formData, callback)
{
   $.ajax({
        type: "POST",
        url: appAddress+page,
        async: true,
        data: formData,
        beforeSend: function () {
            showLoader();
        },
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            callback(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            notyAlert('error', errorThrown + ',' + textStatus + ',' + jqXHR.statusText);
        },
        complete:function()
        {
            hideLoader();
        }

    });
    
}


function GetDataFromServer(page, formData) {
    var jsonResult = {};
    $.ajax({
        
        type: "GET",
        url: appAddress + page,
        data: formData,
        beforeSend: function () {
            showLoader();
        },
        async: false,
        cache: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
         jsonResult = data;
        },
        error: function (jqXHR, textStatus, errorThrown) {
          notyAlert('error',errorThrown + ',' + textStatus + ',' + jqXHR.statusText);
        },
        complete: function () {
            hideLoader();
        }

    });
    return jsonResult;
}
function ChangeButtonPatchView(Controller,Dom, Action) {
    var data = { ActionType: Action };
    var ds = {};
    ds = GetDataFromServer(Controller + "/ChangeButtonStyle/", data);
    if (ds == "Nochange")
    {
        return;0
    }
    $("#" + Dom).empty();
    $("#" + Dom).html(ds);
}

function NetworkFailure(data, status, xhr) {
    var i = JSON.parse(data)
    notyAlert('error', status);
}


//Common function for clearing input fields
function ClearFields() {
    $(':input').each(function () {

        if (this.type == 'text' || this.type == 'textarea' || this.type == 'file'|| this.type == 'search') {
            this.value = '';
        }
        else if (this.type == 'checkbox') {
            this.checked = false;
        }
        else if (this.type == 'select-one' || this.type == 'select-multiple') {
            this.value = '-1';
        }
    });

}


//only number validation
function isNumber(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) { //if the key isn't the backspace key (which we should allow)
        if (unicode < 48 || unicode > 57) //if not a number
            return false //disable key press
    }
}


function notyConfirm(msg, functionIfSuccess,msg2,btnText,value) {
    var m = 'You will not be able to recover this action!'
    if (msg2 != undefined) {
        m = msg2 + '  ' + m;
    }
    if (value == 1)
    {
        m = '';
    }
    if (btnText == undefined)
    {
        btnText = "Yes, delete it!";
    }
    swal({
        title: msg,
        text: m,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: btnText,
        closeOnConfirm: false
    },
function () {
    //swal("Deleted!", "Your imaginary file has been deleted.", "success");
    eval(functionIfSuccess );
});


    //var text = '<div class="confirmbox"><span class="confirmboxHead">Delete Alert !</span><br/><br/><span class="confirmboxMsg">' + msg + '</span><br/><br/><span class="confirmboxFooter">You cannot reverse this action</span><div>'
    //var n = noty({
    //    text: text,
    //    type: 'confirm',
    //    dismissQueue: false,
    //    layout: 'center',
    //    modal: true,
    //    theme: 'defaultTheme',
    //    buttons: [
    //        {
    //            addClass: 'btn btn-primary', text: '&nbsp&nbsp;&nbsp;&nbsp;Ok&nbsp;&nbsp;&nbsp;&nbsp', onClick: function ($noty) {
    //                $noty.close();
    //                eval(functionIfSuccess + '()');

    //            }
    //        },
    //    {
    //        addClass: 'btn btn-danger', text: 'Cancel', onClick: function ($noty) {
    //            $noty.close();
    //            return false;
    //        }
    //    }
    //    ]
    //})

}

function goHome() {
    window.location = appAddress + '/SAMPanel';
}
function Logout() {
    window.location = appAddress;
}

var loadStatus = 0;
 
function showLoader() {
    try {
        $(".preloader").show();
    } catch (e) {

    }
   
   
}

 
function hideLoader() {
    try {
        $('.preloader').fadeOut();
    } catch (e) {

    }
   
   
}


function roundoff(num) {
    return (Math.round(num * 100) / 100).toFixed(2);
}
function roundoff(num, opt) {
    if (num == '' && opt != undefined) { return ''; }
    return (Math.round(num * 100) / 100).toFixed(2);
}


//---* Order Status Notification * ---//
var Messages = {
     
}

//DATE FORMAT
function IsVaildDateFormat(date) {
    var regExp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]|(?:Jan|Mar|May|Jul|Aug|Oct|Dec)))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2]|(?:Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)(?:0?2|(?:Feb))\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9]|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep))|(?:1[0-2]|(?:Oct|Nov|Dec)))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
    return regExp.test(date);
}



function openNav() {
   
    var left = $(".main-sidebar").width();
    var total = $(document).width();
    //var windowheight = $(document).height();
    //var topheight = $("#myNav").height();
    //var headerheight = $("#OverlayHeader").height();
    //var resultHeight = (topheight - headerheight-200) + 'px!important'
    //$('.EntryForms').attr('style', 'height: ' + resultHeight + '');
  
    $('.main').fadeOut();
    document.getElementById("myNav").style.left = "3%";
    $('#main').fadeOut();

    if ($("body").hasClass("sidebar-collapse")) {
       
        }
    else {
        $(".sidebar-toggle").trigger("click");
    }

    
}

function closeNav() {
    document.getElementById("myNav").style.left = "100%";
    $('#main').fadeIn();
}


$('.EntryForms').scroll(function () {
    var scroll = $('.EntryForms').scrollTop();

    
    if (scroll >= 50) {
        //clearHeader, not clearheader - caps H
        $("#OverlayHeader").addClass("OverlayHeader");
    }
    else {
        $("#OverlayHeader").removeClass("OverlayHeader");
    }
});