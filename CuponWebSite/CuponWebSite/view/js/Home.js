function SetView(view) {
    document.getElementById("signupbox").setAttribute("hidden", true);
    document.getElementById("loginbox").setAttribute("hidden", true);
    document.getElementById("about").setAttribute("hidden", true);
    document.getElementById("Login").setAttribute("hidden", true);
    document.getElementById("Contact").setAttribute("hidden", true);
    if (view.indexOf("box")>-1) {
        document.getElementById("Login").removeAttribute("hidden");
    }
    document.getElementById(view).removeAttribute("hidden");
}

function register() {
    var user = new Object();
    user.userName = $('#username').val();
    user.password = $('#password').val();
    user.email = $('#email').val();
    user.gender = $('#gender').val();
    user.phoneNumber = $('#phone').val();
    user.birthDate = $('#birthdate').val();
    user.longitude = "31.123";
    user.latitude = "7.1343";
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/UserServices.asmx/BasicUserRegister",
        data: JSON.stringify(user),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.d) {
                setCookie("user", user.userName, 365);
                setCookie("pass", user.password, 365);
                setCookie("id", data.d, 365);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
    //set account tab

}

function login() {
    var userID;
    var userName = document.getElementById("login-username").value;
    var password = document.getElementById("login-password").value;
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/UserServices.asmx/AuthenticateUser",
        data: JSON.stringify({ "userName": userName, "password": password }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.d) {
                setCookie("user", userName, 365);
                setCookie("pass", password, 365);
                window.location.href = "index.html?"+data.d;
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
    //set account tab
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponSystemWebService.asmx/FindUserByID",
        data: JSON.stringify({ "id": userID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.d) {
                var d = JSON.parse(data.d);
                $("#accountImg").attr("src", "http://placehold.it/120x120");
                $("#accountName").text(d.userName);
                $("#accountMail").text(d.Email);
                $("#accountView").click();
                document.getElementById("head_login").setAttribute("hidden", true);
                document.getElementById("head_account").removeAttribute("hidden");
                SetView("products");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });

}
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

function deleteCookie(cname) {
    document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
};