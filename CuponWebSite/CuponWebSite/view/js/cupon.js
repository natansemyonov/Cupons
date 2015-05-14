var category;
GetAllCupons();

function SearchByLocation(coords, radius) {
    $('#WaitModal').modal('show');
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponSystemWebService.asmx/FindCuponByLocation",
        data: JSON.stringify({ "latitude":coords.latitude,"longtitude":coords.longitude }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            ShowCupons(JSON.parse(data.d));
            $('#WaitModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}
function GetAllCupons() {
    $('#WaitModal').modal('show');
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/GetAllCupons",
        
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            ShowCupons(JSON.parse(data.d));
            $('#WaitModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}
function GetCupons(catId) {
    $('#WaitModal').modal('show');
        $.ajax({
            type: "POST",
            url: "http://localhost:20353/Controller/CuponSystemWebService.asmx/FindCuponByPreference",
            data: JSON.stringify({ "c": catId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
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
    document.getElementById("cupons_div").innerHTML = "";
    for (var i = 0; i < cuponList.length; i++) {
        var j = '<div class="col-sm-4 col-lg-4 col-md-4" onclick="ShowCuponModal(' + cuponList[i].Id +
            ')"> <div class="thumbnail"> <img src="http://placehold.it/320x150" alt=""> <div class="caption">' +
            '<h4 class="pull-right">'+cuponList[i].Price+'</h4> <h4><a href="#">' + cuponList[i].Name + 
            '</a> </h4> <p>'+cuponList[i].Description + '</p>' +
            '</div> <div class="ratings"> <p class="pull-right"></p> '+stars(cuponList[i].Rate)+'</div> </div> </div>';
        document.getElementById("cupons_div").innerHTML += j;
    }
}

function stars(rating) {
    var s = '<p> ';
    for (var j = 0; j < 5; j++) {
        if (j < rating)
            s += '<span class="glyphicon glyphicon-star"></span>';
        else
            s += '<span class="glyphicon glyphicon-star-empty"></span>';

    }
    return s + '</p>';
}

function ShowCuponModal(cuponId) {
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponSystemWebService.asmx/FindCuponById",
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
                        $('#PurchaseButton').click(BuyCupon(cuponId));
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

function BuyCupon(cuponId) {
    var uId = getCookie("id");
    var purchase = new Object();
    purchase.cuponId = 347;
    purchase.userId = 21;
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/PurchaseCupon",
        data: JSON.stringify(purchase),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var d = JSON.parse(data.d);
            if (!d) {
                return;
            }
            

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}