var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
function LoginSuccess(data, status, xhr) {
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if (JsonResult.Record != "false") {
                window.location = appAddress + "Dashboard";
            }
            else {
                $('.card').append('<span class="logfailed">Login Failed</span>');

            }


            break;
        case "ERROR":

            break;
        default:

            break;
    }

}