navigator.geolocation.getCurrentPosition(setPosition);
var pos = new Object();
function SetView(view) {
    document.getElementById("signupbox").setAttribute("hidden", true);
    document.getElementById("loginbox").setAttribute("hidden", true);
    document.getElementById("about").setAttribute("hidden", true);
    document.getElementById("Login").setAttribute("hidden", true);
    document.getElementById("Contact").setAttribute("hidden", true);
    document.getElementById("BussinessOwner").setAttribute("hidden", true);
    document.getElementById("bLogin").setAttribute("hidden", true);
    document.getElementById("bRegister").setAttribute("hidden", true);
    if (view.indexOf("box")>-1) {
        document.getElementById("Login").removeAttribute("hidden");
    }
    if (view == "bLogin" || view == "bRegister")
        document.getElementById("BussinessOwner").removeAttribute("hidden");
    document.getElementById(view).removeAttribute("hidden");
}

function sysAdminLogin() {
    $('#generalModalTitle').html('System Administrator Login');
    var j = '<div class="row"><div class="col-md-4">Username:</div>'+
        '<div class="col-md-8"><input id="sysUser" type="text" /></div>' +
        '</div><div class="row"><div class="col-md-4">Password:</div>' +
        '<div class="col-md-8"><input id="sysPass" type="password" /></div></div>';
    $('#generalModalBody').html(j);
    $('#generalModalFooter').html('<button type="button" class="btn btn-default" data-dismiss="modal">' +
        'Close</button><button type="button" class="btn btn-default" data-dismiss="modal" onclick="SignInAdmin()">Login</button>');
    $('#GeneralModal').modal('show');
}

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(register);
    } else {
        x.innerHTML = "Geolocation is not supported by this browser.";
    }
}
function register(position) {
    var d = new Date($('#birthdate').val());
    var user = new Object();
    user.userName = $('#username').val();
    user.password = $('#password').val();
    user.email = $('#email').val();
    user.gender = $('#gender').val() == "Male" ? 0 : 1;
    user.phoneNumber = $('#phone').val();
    user.birthDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
    user.photo = "images/Male_User.jpg";
    user.longitude = position.coords.longitude;
    user.latitude = position.coords.latitude;
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
                window.location.href = "index.html?" + data.d;
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
            $('#signupalert').html("Error: " + xhr.status);
            $('#signupalert').css("display", "block");
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
        data: JSON.stringify({ "userName": userName, "password": password, "latitude": pos.latitude, "longitude": pos.longitude }),
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
function registerBO() {
    var user = new Object();
    user.userName = $('#username').val();
    user.password = $('#password').val();
    user.email = $('#email').val(); 
    user.photo = "images/Business.jpg";
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/UserServices.asmx/BussinessOwnerRegister",
        data: JSON.stringify(user),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.d) {
                setCookie("user", user.userName, 365);
                setCookie("pass", user.password, 365);
                setCookie("id", data.d, 365);
                window.location.href = "BussinessOwner.html?" + data.d;
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
            $('#signupalert').html("Error: " + xhr.status);
            $('#signupalert').css("display", "block");
        }
    });
    //set account tab

}

function loginBO() {
    var userName = document.getElementById("bUsername").value;
    var password = document.getElementById("bPassword").value;
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/UserServices.asmx/AuthenticateOwner",
        data: JSON.stringify({ "userName": userName, "password": password }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.d) {
                setCookie("user", userName, 365);
                setCookie("pass", password, 365);
                window.location.href = "BussinessOwner.html?" + data.d;
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

function SignInAdmin() {
    var userName = document.getElementById("sysUser").value;
    var password = document.getElementById("sysPass").value;
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/UserServices.asmx/AuthenticateSystemAdmin",
        data: JSON.stringify({ "userName": userName, "password": password }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.d) {
                setCookie("user", userName, 365);
                setCookie("pass", password, 365);
                window.location.href = "sysAdmin.html?" + data.d;
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
}

function setPosition(position) {
    pos = position.coords;
}