var bussinesses = [];
var bussinessCupons = [];
var ownerID;
main();
function main() {
    ownerID = window.location.search.substring(1);
    GetOwner();
    GetBussiness();
    ShowBussiness();
}

function GetOwner() {
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/UserServices.asmx/FindBussinessOwnerByID",
        data: JSON.stringify({ "id": ownerID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var d = JSON.parse(data.d);
            $('#ownerImg').attr("src", d.Photo);
            $("#accountImg").attr("src", d.Photo);
            $("#accountName").text(d.userName);
            $("#accountMail").text(d.Email);
            //$("#accountView").click(SetView('profile'));
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
}
function GetBussinessDetails(name,i) {
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
            var j = "<div><div><b>Description:</b><p>" + d.Description + "</p></div>"+
            "<div><b>Location:</b><p>"+document.getElementById('address'+i).innerHTML+"</p></div><div>" +
            "<b>Live Cupons:</b><p>" + d.BussinessCupons.length + "</p><b>Old Cupons:</b><p>" + d.BussinessCupons.length + "</p></div></div>";
            $("#generalModalTitle").html(d.Name);
            $("#generalModalBody").html(j);
            if ($("#editbutton").length==0)
                $("#generalModalCloseButton").after('<button class="btn btn-success" id="AddCuponbutton" onclick="document.getElementById(\'bussinessIdHidden\').value='+d.Id+';$(\'#AddCuponModal\').modal(\'show\');$(\'#GeneralModal\').modal(\'hide\');">Add Cupon</button><button class="btn btn-success" id="editbutton" onclick="EditBussiness(' + d.Id + ')">Edit</button>');
            else {
                $("#editbutton").click(function (){EditBussiness(d.Id)});
            }
            $("#GeneralModal").modal("show");
            $('#WaitModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}

function GetCuponForBussiness(bussinessId,type) {
   // $('#WaitModal').modal('show');
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/FindCuponByBussiness",
        data: JSON.stringify({ "bussinessId": parseInt(bussinessId) }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.d!="false") {
                if (type == 0)
                    bussinessCupons.push(JSON.parse(data.d));
                if (type == 1)
                    bussinessCupons = JSON.parse(data.d);
                // $('#WaitModal').modal('hide');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
        }
    });
    $('#WaitModal').modal('hide');

}

function ShowCupons(cuponList) {
    document.getElementById("cupons_div").innerHTML = "";
    
    if (cuponList == "false") {
        document.getElementById("cupons_div").innerHTML = "No Cupons Avialable";
        return;
    }
    for (var i = 0; i < cuponList.length; i++) {
        var j = '<div class="col-sm-4 col-lg-4 col-md-4 cupon ' + cuponList[i][0].Bussiness.Name.replace(new RegExp(' ', 'g'), '') +'"> <div class="thumbnail">' +
            ' <img src="'+cuponList[i][0].Photo+'" alt=""> <div class="caption">' +
            '<h4 class="pull-right">' + cuponList[i][0].Price + '</h4> <h4><a href="#">' + cuponList[i][0].Name +
            '</a> </h4> <p>' + cuponList[i][0].Description + '</p>' +
            '</div> <div class="ratings"> <p class="pull-right"></p> ' + stars(cuponList[i][0].Rate) + '</div> </div> </div>';
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

function GetBussiness() {
    $('#WaitModal').modal('show');
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/BussinessServices.asmx/FindBussinessOfOwner",
        data: JSON.stringify({ "ownerID": ownerID }),
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
    for (var i = 0; i < bussinesses.length; i++) {
        var address;
        var myLatlng = new google.maps.LatLng(bussinesses[i].Location.Latitude, bussinesses[i].Location.Longtitude);
        address = GetLocation(myLatlng, i);
        var style = ' style="border:1px solid green" ';
        if (!bussinesses[i].Approved) {
            style = ' style="border:1px solid red" ';
        }
        var j = '<div class="col-sm-6 col-lg-6 col-md-6"'+style+' onclick="GetBussinessDetails(\'' + bussinesses[i].Name + '\',' + i + ')">' +
            ' <div class="thumbnail"> <img src="' +bussinesses[i].Photo+ '" alt=""> <div class="caption">' +
        ' <h4><a href="#">' + bussinesses[i].Name + '</a> </h4> <p>' + bussinesses[i].Description + '</p>' + '<div id="address' + i+ '"></div>' +
        '</div> </div> </div>';
        document.getElementById("cupons_div").innerHTML += j;
    }
}

function GetLocation(myLatlng,i) {
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

function GetCuponsForOwner() {
    bussinessCupons = [];
    var x = "<div id='bussinessSelector'><select id='bussinessSelect' onchange='filterCupons();'><option value='all'>All</option>";
    for (var i = 0; i < bussinesses.length; i++) {
        GetCuponForBussiness(bussinesses[i].Id, 0);
        x += "<option value='" + bussinesses[i].Name.replace(new RegExp(' ', 'g'), '') + "'>" + bussinesses[i].Name + "</option>";
    }
    x += "</select></div><hr>";
    ShowCupons(bussinessCupons);
    $("#cupons_div").before(x);
}

function showPosition(position) {
    var myLatlng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
    var mapOptions = {
        zoom: 14,
        center: myLatlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP

    }
    mapholder = document.getElementById('mapCanvas');
    mapholder.style.height = '400px';
    mapholder.style.width = '400px';
    var map = new google.maps.Map(document.getElementById('mapCanvas'), mapOptions);
    $('#AddBussinessModal').modal('show');
    // Place a draggable marker on the map
    var marker = new google.maps.Marker({
        position: myLatlng,
        map: map,
        draggable: true
    });
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'latLng': marker.position }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[0]) {
                document.getElementById("location").value = results[0].formatted_address;
                document.getElementById("NewBussinessLongitude").value = marker.position.lng();
                document.getElementById("NewBussinessLatitude").value = marker.position.lat();
            }
        }
    });
    google.maps.event.addListener(marker, 'dragend', function (evt) {
        geocoder.geocode({ 'latLng': marker.position }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                if (results[0]) {
                    document.getElementById("location").value = results[0].formatted_address;
                    document.getElementById("NewBussinessLongitude").value = marker.position.lng();
                    document.getElementById("NewBussinessLatitude").value = marker.position.lat();
                }
            }
        });
    });
}

function AddBussiness() {
    var bussiness = new Object();
    bussiness.name = $('#NewBussinessName').val();
    bussiness.description = $('#NewBussinessDescription').val();
    bussiness.photo = "images/Business.jpg";
    bussiness.category = $('#NewCuponCategory').val();
    bussiness.longtitude = $('#NewBussinessLongitude').val();
    bussiness.latitude = $('#NewBussinessLatitude').val();
    bussiness.bussinessOwnerId = ownerID;
    $('#WaitModal').modal('show');
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/BussinessServices.asmx/AddBussiness",
        data: JSON.stringify(bussiness),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#AddBussinessModal').modal('hide');
            $('#WaitModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}

function filterCupons() {
    $(".cupon").hide();
    $("." + $("#bussinessSelect").val()).show();
}