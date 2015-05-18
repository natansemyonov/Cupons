var bussinesses = [];
var bussinessCupons = [];

main();
function main() {
    GetBussiness();
}


function GetBussinessDetails(name) {
    $('#WaitModal').modal('show');
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/BussinessServices.asmx/FindBussinessByName",
        data: JSON.stringify({ "bussinessName": name }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var d = JSON.parse(data.d);
            bussinessId = d.Id;
            $('#WaitModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}

function GetCuponForBussiness(bussinessId,type) {
    $('#WaitModal').modal('show');
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/FindCuponByBussiness",
        data: JSON.stringify({ "bussinessId": parseInt(bussinessId) }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            if (type == 0)
                bussinessCupons += JSON.parse(data.d);
            if (type == 1)
                bussinessCupons = JSON.parse(data.d);
            $('#WaitModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
    $('#WaitModal').modal('hide');

}

function ShowCupons(cuponList) {
    document.getElementById("cupons_div").innerHTML = "";
    for (var i = 0; i < cuponList.length; i++) {
        var j = '<div class="col-sm-4 col-lg-4 col-md-4"> <div class="thumbnail"> <img src="http://placehold.it/320x150" alt=""> <div class="caption">' +
            '<h4 class="pull-right">' + cuponList[i].Price + '</h4> <h4><a href="#">' + cuponList[i].Name +
            '</a> </h4> <p>' + cuponList[i].Description + '</p>' +
            '</div> <div class="ratings"> <p class="pull-right"></p> ' + stars(cuponList[i].Rate) + '</div> </div> </div>';
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

function AddCupon() {
    var cupon = new Object();
    cupon.name = $('#NewCuponName').val();
    cupon.description = $('#NewCuponDescription').val();
    cupon.originalPrice = $('#NewCuponOriginalPrice').val();
    cupon.price = $('#NewCuponPrice').val();
    cupon.expirationDate = $('#NewCuponDate').val();
    cupon.category = $('#NewCuponCategory').val();
    cupon.longitude = -0.1276250;
    cupon.latitude = 51.5033630;
    cupon.bussinessId = 12;
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

function GetBussiness() {
    $('#WaitModal').modal('show');
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/BussinessServices.asmx/FindBussinessByOwner",
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            bussinesses = JSON.parse(data.d);
            $('#WaitModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
    $('#WaitModal').modal('hide');
}

function ShowBussiness() {
    document.getElementById("cupons_div").innerHTML = "";
    var geocoder = new google.maps.Geocoder();
    for (var i = 0; i < bussinesses.length; i++) {
        var address;
        var myLatlng = new google.maps.LatLng(bussinesses[i].Location.Latitude, bussinesses[i].Location.Longtitude);
        geocoder.geocode({ 'latLng': myLatlng }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                if (results[0]) {
                    address = results[0].formatted_address;
                }
            }
        });
        var j = '<div class="col-sm-6 col-lg-6 col-md-6" onclick="GetBussinessDetails(' + bussinesses[i].Name + ')> <div class="thumbnail"> <img src="http://placehold.it/320x150" alt=""> <div class="caption">' +
        ' <h4><a href="#">' + bussinesses[i].Name + '</a> </h4> <p>' + bussinesses[i].Description + '</p>' + '<p>' + address + '</p>' +
        '</div> </div> </div>';
        document.getElementById("cupons_div").innerHTML += j;
    }
}

function GetCuponsForOwner() {
    bussinessCupons = [];
    for (var i = 0; i < bussinesses.length; i++) {
        GetCuponForBussiness(bussinesses.Id,0);
    }
    ShowCupons(bussinesses);
}