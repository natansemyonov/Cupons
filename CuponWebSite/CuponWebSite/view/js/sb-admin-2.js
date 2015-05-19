function SetView(view) {
    document.getElementById("CuponsDiv").setAttribute("hidden", true);
    document.getElementById("main").setAttribute("hidden", true);
    document.getElementById(view).removeAttribute("hidden");
}

$(function () {

    $('#side-menu').metisMenu();

});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
// Sets the min-height of #page-wrapper to window size
$(function() {
    $(window).bind("load resize", function() {
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
    var element = $('ul.nav a').filter(function() {
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
        url: "http://localhost:20353/Controller/BussinessServices.asmx/FindBussinessByOwner",
        data: JSON.stringify({ "ownerID": 1503 }),
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
        url: "http://localhost:20353/Controller/CuponServices.asmx/FindCuponById",
        data: JSON.stringify({ "cuponId": cuponId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var cupon = JSON.parse(data.d);
            var myLatlng = new google.maps.LatLng(44.4647452, 7.3553838);//cupon.Location.Latitude, cupon.Location.Longtitude);
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
    
}