function SetDashboard() {
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/GetAllUnAprrovedCupons",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var d = (JSON.parse(data.d));
            $("#NumOfUnapproved").html(d.length);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/BussinessServices.asmx/GetAllUnapprovedBussiness",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var d = (JSON.parse(data.d));
            $("#numOfUnapprovedBussinesses").html(d.length);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/GetAllUnAprrovedSocialCupons",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var d = (JSON.parse(data.d));
            $("#numOfSocialCupons").html(d.length);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/GetAllBussinessCupons",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            $("#topBody").html("");
            var d = (JSON.parse(data.d));
            d.sort(function(a, b) {
                return b.PurchasedCupons.length - a.PurchasedCupons.length;
            });
            var cat = ["Resturants","Shopping","HealthCare","Office & Electronics","Vacations","Pleasure"];
            for (var i = 0; i < d.length; i++) {
                var j = "<tr><td>" + d[i].PurchasedCupons.length + "</td><td>" + d[i].Name + "</td><td>" + d[i].Bussiness.Name +
                    "</td><td>" + cat[d[i].Category] + "</td><td>" + d[i].Price + "</td></tr>";
                document.getElementById("topBody").innerHTML+=j;
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
    
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/UserServices.asmx/GetLoggingHistory",
        data: JSON.stringify({ "adminId": window.location.search.substring(1) }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var d = (JSON.parse(data.d));
            CreateEventLog(d);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
}

SetDashboard();
function SetView(view) {
    document.getElementById("page-wrapper").setAttribute("hidden", true);
    document.getElementById("contact").setAttribute("hidden", true);
    document.getElementById("about").setAttribute("hidden", true);
    document.getElementById(view).removeAttribute("hidden");
}

$(function () {

    $('#side-menu').metisMenu();

});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
// Sets the min-height of #page-wrapper to window size
$(function () {
    $(window).bind("load resize", function () {
        topOffset = 50;
        width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.navbar-collapse').addClass('collapse');
            topOffset = 100; // 2-row-menu
        } else {
            $('div.navbar-collapse').removeClass('collapse');
        }

        height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;
        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            $("#page-wrapper").css("min-height", (height) + "px");
        }
    });

    var url = window.location;
    var element = $('ul.nav a').filter(function () {
        return this.href == url || url.href.indexOf(this.href) == 0;
    }).addClass('active').parent().parent().addClass('in').parent();
    if (element.is('li')) {
        element.addClass('active');
    }
});

function ApproveCupons() {
    SetView("CuponsDiv");
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/GetAllUnAprrovedCupons",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            ShowCupons(JSON.parse(data.d));
            $('#WaitModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
}

function ShowCupons(cuponList) {
    document.getElementById("CuponsDiv").innerHTML = "";
    for (var i = 0; i < cuponList.length; i++) {
        var j = '<div class="col-sm-4 col-lg-4 col-md-4" onclick="ShowCuponModal(' + cuponList[i].Id +
            ')"> <div class="thumbnail"> <img src="images/Coupon.png" alt=""> <div class="caption">' +
            '<h4 class="pull-right">' + cuponList[i].Price + '</h4> <h4><a href="#">' + cuponList[i].Name +
            '</a> </h4> <p>' + cuponList[i].Description + '</p>' +
            '</div> <div class="ratings"> <p class="pull-right"></p></div> </div> </div>';
        document.getElementById("CuponsDiv").innerHTML += j;
    }
}
function ShowCuponModal(cuponId) {
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/FindCuponsById",
        data: JSON.stringify({ "cuponId": cuponId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var cupon = JSON.parse(data.d);
            var myLatlng = new google.maps.LatLng(cupon.Location.Latitude, cupon.Location.Longtitude);
            var geocoder = new google.maps.Geocoder();
            var address;
            geocoder.geocode({ 'latLng': myLatlng }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        address = results[0].formatted_address;
                        $('#cuponDescription').html(cupon.Description);
                        $('#CuponName').html(cupon.Name);
                        $('#cuponLocation').html(address);
                        $('#cuponExpiration').html(new Date(cupon.ExpirationDate).toDateString());
                        $('#cuponPrice').html(cupon.Price + '&#8362;');
                        $('#cuponOriginalPrice').html(cupon.OriginalPrice + '&#8362;');
                        $('#PurchaseButton').click(function () { ApproveCupon(cuponId); });
                        $('#CuponModal').modal('show');

                    } else {
                        alert('No results found');
                    }
                } else {
                    alert('Geocoder failed due to: ' + status);
                }
            });

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}
function ApproveCupon(id) {
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/ApproveCupon",
        data: "{'cuponId':'" + id +"','adminId':'"+window.location.search.substring(1)+"'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var d = JSON.parse(data.d);
            if (!d) {
                return;
            }
            $('#CuponModal').modal('hide');
            $('#generalModalBody').html('Cupon Approved!');
            $('#GeneralModal').modal('show');
            ApproveCupons();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}

function ApproveBussinesses() {
    SetView("CuponsDiv");
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/BussinessServices.asmx/GetAllUnapprovedBussiness",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            ShowBussinesses(JSON.parse(data.d));
            $('#WaitModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
}
function ShowBussinesses(bussinessList) {
    document.getElementById("CuponsDiv").innerHTML = "";
    if (bussinessList == "false") {
        document.getElementById("CuponsDiv").innerHTML = "No Bussinesses waiting for approval!";
    }
    for (var i = 0; i < bussinessList.length; i++) {
        var address;
        var myLatlng = new google.maps.LatLng(bussinessList[i].Location.Latitude, bussinessList[i].Location.Longtitude);
        address = GetLocation(myLatlng, i);
        var j = '<div class="col-sm-6 col-lg-6 col-md-6" onclick="ShowBussinessModal(\'' + bussinessList[i].Name + '\',' + i + ')">' +
            ' <div class="thumbnail"> <img src="' + bussinessList[i].Photo + '" alt=""> <div class="caption">' +
        ' <h4><a href="#">' + bussinessList[i].Name + '</a> </h4> <p>' + bussinessList[i].Description + '</p>' + '<div id="address' + i + '"></div>' +
        '</div> </div> </div>';
        document.getElementById("CuponsDiv").innerHTML += j;
    }
}
function GetLocation(myLatlng, i) {
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'latLng': myLatlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[0]) {
                var address = results[0].formatted_address;
                $('#address' + i).html(address);
                return address;
            }
        }
    });
}
function ShowBussinessModal(name, i) {
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/BussinessServices.asmx/FindBussinessByName",
        data: JSON.stringify({ "bussinessName": name }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var d = JSON.parse(data.d);
            var j = "<div><div><b>Description:</b><p>" + d.Description + "</p></div>" +
            "<div><b>Location:</b><p>" + document.getElementById('address' + i).innerHTML + "</p></div><div>" +
            "<b>Live Cupons:</b><p>" + d.BussinessCupons.length + "</p><b>Old Cupons:</b><p>" + d.BussinessCupons.length + "</p></div></div>";
            $("#generalModalTitle").html(d.Name);
            $("#generalModalBody").html(j);
            if ($("#AddBussinessbutton").length == 0)
                $("#generalModalCloseButton").after('<button class="btn btn-success" id="AddBussinessbutton" ' +
                    'onclick="ApproveBussiness(' + d.Id + ');$(\'#AddCuponModal\').modal(\'show\');' +
                    '$(\'#GeneralModal\').modal(\'hide\');">Approve Bussiness</button>');
            else {
                $("#AddBussinessbutton").click(function () {
                    ApproveBussinesses(d.id);
                    $('#AddBussinesModal').modal('show');
                    $('#GeneralModal').modal('hide');
                });
            }
            $("#GeneralModal").modal("show");
            $('#WaitModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}
function ApproveBussiness(id) {
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/BussinessServices.asmx/ApproveBussiness",
        data: "{'bussinessId':'" + id + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var d = JSON.parse(data.d);
            if (!d) {
                return;
            }

            $('#generalModalBody').html('Bussiness Approved!');
            $('#GeneralModal').modal('show');
            $('#AddBussinessbutton').remove();
            $('#generalModalCloseButton').click(function () {
                $('#GeneralModal').modal('hide');
                ApproveBussinesses();
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}

function ApproveSocialCupons() {
    SetView("CuponsDiv");
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/GetAllUnAprrovedSocialCupons",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            ShowSocialCupons(JSON.parse(data.d));
            $('#WaitModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
}
function ShowSocialCupons(cuponList) {
    document.getElementById("CuponsDiv").innerHTML = "";
    for (var i = 0; i < cuponList.length; i++) {
        var j = '<div class="col-sm-4 col-lg-4 col-md-4"> <div class="thumbnail"> <img src="images/Coupon.png" alt="">' +
            ' <h4 ><a href="#" onclick="ShowAddCuponModal(\'' + cuponList[i].Name + '\')">' + cuponList[i].Name +
            '</a> </h4> <p><a href="http://' + cuponList[i].URL + '" target="_blank">' + cuponList[i].URL + '</a></p>' +
            '</div> <div class="ratings"> <p class="pull-right"></p></div> </div> </div>';
        document.getElementById("CuponsDiv").innerHTML += j;
    }
}

function ShowAddCuponModal(name) {

    $("#NewCuponName").val(name);
    $("#AddCuponModal").modal('show');
}

function AddCuponModal(id) {
    var cupon = new Object();
    cupon.name = $('#NewCuponName').val();
    cupon.description = $('#NewCuponDescription').val();
    cupon.originalPrice = $('#NewCuponOriginalPrice').val();
    cupon.price = $('#NewCuponPrice').val();
    cupon.expirationDate = $('#NewCuponDate').val();
    cupon.category = $('#NewCuponCategory').val();
    cupon.longitude = -0.1276250;
    cupon.latitude = 51.5033630;
    cupon.bussinessId = $('#bussinessIdHidden').val();
    cupon.photo = "images/Coupon.png";
    //$('#WaitModal').modal('show');
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/AddBussinessCupon",
        data: JSON.stringify(cupon),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $('#AddCuponModal').modal('hide');
            $('#WaitModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}

$(".dropdown-menu li a").click(function () {
    var selText = $(this).text();
    $(this).parents('.btn-group').find('.dropdown-toggle').html(selText + ' <span class="caret"></span>');
});

function CreateEventLog(log) {
    var logEventList = "";
    for (var i = 0; i < log.length && i<15; i++) {
        var now = new Date();
        var time = new Date(log[0].Timestamp.replace("T"," "));
        var elapsed = now.getTime() - time.getTime();
        var timelapse;
        if (elapsed < 1000 * 60) {//seconds
            timelapse = Math.floor(elapsed / (1000)) + " seconds ago";
        }
        else if (elapsed < 1000 * 60 * 60) {//minutes
            timelapse = Math.floor(elapsed / (1000 * 60)) + " minutes ago";
        }
        else if (now.getDate() == time.getDate() && now.getMonth() == time.getMonth()) {//today
            timelapse = Math.floor(elapsed / (1000 * 60 * 60)) + " hours ago";
        }
        else if (time.getDate() + 1 == now.getDate() && now.getMonth() == time.getMonth()) {
            timelapse = "Yesterday";
        }
        else {
            timelapse = time.getDate() + "/" + time.getMonth() + "/" + time.getFullYear() +
                " " + time.getHours() + ":" + time.getMinutes();
        }
        var title;
        if (log[0].Data.length > 15)
            title = log[0].Data.substring(0, 12) + "...";
        else {
            title = log[0].Data;
        }
        var icon = "glyphicon glyphicon-envelope";
        if (log[0].Data.indexOf("approve")!=-1) {
            icon = "glyphicon glyphicon-ok";
        }
        logEventList += '<a title="'+log[0].Data+'" class="list-group-item"><i class="'+icon+'"></i>' +title+
        '<span class="pull-right text-muted small"><em>' + timelapse + '</em></span></a>';
       
    }
    $("#EventLog").html(logEventList);
}