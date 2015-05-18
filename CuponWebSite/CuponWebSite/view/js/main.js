var category;




(function (t, e) { if (typeof exports == "object") module.exports = e(); else if (typeof define == "function" && define.amd) define(e); else t.Spinner = e() })(this, function () { "use strict"; var t = ["webkit", "Moz", "ms", "O"], e = {}, i; function o(t, e) { var i = document.createElement(t || "div"), o; for (o in e) i[o] = e[o]; return i } function n(t) { for (var e = 1, i = arguments.length; e < i; e++) t.appendChild(arguments[e]); return t } var r = function () { var t = o("style", { type: "text/css" }); n(document.getElementsByTagName("head")[0], t); return t.sheet || t.styleSheet }(); function s(t, o, n, s) { var a = ["opacity", o, ~~(t * 100), n, s].join("-"), f = .01 + n / s * 100, l = Math.max(1 - (1 - t) / o * (100 - f), t), u = i.substring(0, i.indexOf("Animation")).toLowerCase(), d = u && "-" + u + "-" || ""; if (!e[a]) { r.insertRule("@" + d + "keyframes " + a + "{" + "0%{opacity:" + l + "}" + f + "%{opacity:" + t + "}" + (f + .01) + "%{opacity:1}" + (f + o) % 100 + "%{opacity:" + t + "}" + "100%{opacity:" + l + "}" + "}", r.cssRules.length); e[a] = 1 } return a } function a(e, i) { var o = e.style, n, r; i = i.charAt(0).toUpperCase() + i.slice(1); for (r = 0; r < t.length; r++) { n = t[r] + i; if (o[n] !== undefined) return n } if (o[i] !== undefined) return i } function f(t, e) { for (var i in e) t.style[a(t, i) || i] = e[i]; return t } function l(t) { for (var e = 1; e < arguments.length; e++) { var i = arguments[e]; for (var o in i) if (t[o] === undefined) t[o] = i[o] } return t } function u(t) { var e = { x: t.offsetLeft, y: t.offsetTop }; while (t = t.offsetParent) e.x += t.offsetLeft, e.y += t.offsetTop; return e } function d(t, e) { return typeof t == "string" ? t : t[e % t.length] } var p = { lines: 12, length: 7, width: 5, radius: 10, rotate: 0, corners: 1, color: "#000", direction: 1, speed: 1, trail: 100, opacity: 1 / 4, fps: 20, zIndex: 2e9, className: "spinner", top: "auto", left: "auto", position: "relative" }; function c(t) { if (typeof this == "undefined") return new c(t); this.opts = l(t || {}, c.defaults, p) } c.defaults = {}; l(c.prototype, { spin: function (t) { this.stop(); var e = this, n = e.opts, r = e.el = f(o(0, { className: n.className }), { position: n.position, width: 0, zIndex: n.zIndex }), s = n.radius + n.length + n.width, a, l; if (t) { t.insertBefore(r, t.firstChild || null); l = u(t); a = u(r); f(r, { left: (n.left == "auto" ? l.x - a.x + (t.offsetWidth >> 1) : parseInt(n.left, 10) + s) + "px", top: (n.top == "auto" ? l.y - a.y + (t.offsetHeight >> 1) : parseInt(n.top, 10) + s) + "px" }) } r.setAttribute("role", "progressbar"); e.lines(r, e.opts); if (!i) { var d = 0, p = (n.lines - 1) * (1 - n.direction) / 2, c, h = n.fps, m = h / n.speed, y = (1 - n.opacity) / (m * n.trail / 100), g = m / n.lines; (function v() { d++; for (var t = 0; t < n.lines; t++) { c = Math.max(1 - (d + (n.lines - t) * g) % m * y, n.opacity); e.opacity(r, t * n.direction + p, c, n) } e.timeout = e.el && setTimeout(v, ~~(1e3 / h)) })() } return e }, stop: function () { var t = this.el; if (t) { clearTimeout(this.timeout); if (t.parentNode) t.parentNode.removeChild(t); this.el = undefined } return this }, lines: function (t, e) { var r = 0, a = (e.lines - 1) * (1 - e.direction) / 2, l; function u(t, i) { return f(o(), { position: "absolute", width: e.length + e.width + "px", height: e.width + "px", background: t, boxShadow: i, transformOrigin: "left", transform: "rotate(" + ~~(360 / e.lines * r + e.rotate) + "deg) translate(" + e.radius + "px" + ",0)", borderRadius: (e.corners * e.width >> 1) + "px" }) } for (; r < e.lines; r++) { l = f(o(), { position: "absolute", top: 1 + ~(e.width / 2) + "px", transform: e.hwaccel ? "translate3d(0,0,0)" : "", opacity: e.opacity, animation: i && s(e.opacity, e.trail, a + r * e.direction, e.lines) + " " + 1 / e.speed + "s linear infinite" }); if (e.shadow) n(l, f(u("#000", "0 0 4px " + "#000"), { top: 2 + "px" })); n(t, n(l, u(d(e.color, r), "0 0 1px rgba(0,0,0,.1)"))) } return t }, opacity: function (t, e, i) { if (e < t.childNodes.length) t.childNodes[e].style.opacity = i } }); function h() { function t(t, e) { return o("<" + t + ' xmlns="urn:schemas-microsoft.com:vml" class="spin-vml">', e) } r.addRule(".spin-vml", "behavior:url(#default#VML)"); c.prototype.lines = function (e, i) { var o = i.length + i.width, r = 2 * o; function s() { return f(t("group", { coordsize: r + " " + r, coordorigin: -o + " " + -o }), { width: r, height: r }) } var a = -(i.width + i.length) * 2 + "px", l = f(s(), { position: "absolute", top: a, left: a }), u; function p(e, r, a) { n(l, n(f(s(), { rotation: 360 / i.lines * e + "deg", left: ~~r }), n(f(t("roundrect", { arcsize: i.corners }), { width: o, height: i.width, left: i.radius, top: -i.width >> 1, filter: a }), t("fill", { color: d(i.color, e), opacity: i.opacity }), t("stroke", { opacity: 0 })))) } if (i.shadow) for (u = 1; u <= i.lines; u++) p(u, -2, "progid:DXImageTransform.Microsoft.Blur(pixelradius=2,makeshadow=1,shadowopacity=.3)"); for (u = 1; u <= i.lines; u++) p(u); return n(e, l) }; c.prototype.opacity = function (t, e, i, o) { var n = t.firstChild; o = o.shadow && o.lines || 0; if (n && e + o < n.childNodes.length) { n = n.childNodes[e + o]; n = n && n.firstChild; n = n && n.firstChild; if (n) n.opacity = i } } } var m = f(o("group"), { behavior: "url(#default#VML)" }); if (!a(m, "transform") && m.adj) h(); else i = a(m, "animation"); return c });

SetView("products");
var opts = {
    lines: 13, // The number of lines to draw
    length: 20, // The length of each line
    width: 10, // The line thickness
    radius: 30, // The radius of the inner circle
    corners: 1, // Corner roundness (0..1)
    rotate: 0, // The rotation offset
    direction: 1, // 1: clockwise, -1: counterclockwise
    color: '#000', // #rgb or #rrggbb or array of colors
    speed: 1, // Rounds per second
    trail: 60, // Afterglow percentage
    shadow: false, // Whether to render a shadow
    hwaccel: false, // Whether to use hardware acceleration
    className: 'spinner', // The CSS class to assign to the spinner
    zIndex: 2e9, // The z-index (defaults to 2000000000)
    top: 'auto', // Top position relative to parent in px
    left: 'auto' // Left position relative to parent in px
};

var target = document.getElementById('searching_spinner_center');
var spinner = new Spinner(opts).spin(target);


function SetView(view){
	document.getElementById("products").setAttribute("hidden",true);
	document.getElementById("profile").setAttribute("hidden",true);
	document.getElementById("search").setAttribute("hidden",true);
	document.getElementById("about").setAttribute("hidden",true);
	document.getElementById("login").setAttribute("hidden",true);
	document.getElementById(view).removeAttribute("hidden");
}

SetAccount(12);

function SetAccount(accountId){
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/UserServices.asmx/FindUserByID",
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
                $("#accountView").click(SetView('profile'));
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

function register() {
    var userID;
    var user = new Object();
    user.userName = "ido1";
    user.password = "ido";
    user.email = "ido";
    user.gender = "0";
    user.phoneNumber = "ido";
    user.birthDate = "2014-05-06T18:49:16.029Z";
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
                userID = data.d;//data.d;
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
        async:false,
        success: function (data) {
            if (data.d) {
                setCookie("user", userName, 365);
                setCookie("pass", password, 365);
                userID = 20;//data.d;
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

function signout() {
    deleteCookie("user");
    deleteCookie("pass");
    document.getElementById("head_account").setAttribute("hidden", true);
    document.getElementById("head_login").removeAttribute("hidden");
}
function Search() {
    SearchByLocation({ "longitude": 31.1254, "latitude": 32.1234 }, 10);
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

function deleteCookie (cname) {
    document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
};