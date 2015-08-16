var category;
var RecNew = true;
GetAllCupons();
//navigator.geolocation.getCurrentPosition(RecommendedCupons);

function Search() {
    var lat = $("#SearchlocationLat").val();
    var lan = $("#SearchlocationLon").val();
    var radius = $("#Searchradius").val();
    var text = $("#SearchText").val();
    var category = $("#CategorySelector option:selected");
    var categoryCupons = [];
    var locationCupons = SearchByLocation(lat, lan, radius);
    var textCupons = [];
    if (category.length > 0) {
        var c = [];
        for (var i = 0; i < category.length; i++) {
            c.push(category[i].value);
        }
        if (c.indexOf("6")>-1) {
            c = ["0", "1", "2", "3", "4", "5"];
        }
        categoryCupons = SearchByCategory(c);
    }
    if (text.length > 0) {
        
    }
    var cupons = arrayUnique(categoryCupons.concat(locationCupons));
    ShowCupons(cupons);
}

function arrayUnique(array) {
    var a = array.concat();
    for (var i = 0; i < a.length; ++i) {
        for (var j = i + 1; j < a.length; ++j) {
            if (a[i] === a[j])
                a.splice(j--, 1);
        }
    }

    return a;
};

function SearchByLocation(lat, lon, radius) {
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/FindCuponsByLocation",
        data: JSON.stringify({ "latitude": lat, "longtitude": lon, "distance": radius }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            if (JSON.parse(data.d))
                return (JSON.parse(data.d));
            return [];
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
            return [];
        }
    });
}

function SearchByCategory(catIds) {
    var j = [];
    for (var i = 0; i < catIds.length; i++) {
        $.ajax({
            type: "POST",
            url: "http://localhost:20353/Controller/CuponServices.asmx/FindCuponByPreference",
            data: JSON.stringify({ "ctegory": catIds[i] }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                if (JSON.parse(data.d))
                j = arrayUnique(j.concat(JSON.parse(data.d)));
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log("faliure in ajax call for - " + xhr.status);
            }

        });
    }
    return j;
}
function GetAllCupons() {
    $('#WaitModal').modal('show');
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/GetAllBussinessCupons",
        
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
            url: "http://localhost:20353/Controller/CuponServices.asmx/FindCuponByPreference",
            data: JSON.stringify({ "ctegory": catId }),
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
    document.getElementById("cupons_div").innerHTML = "<h2>New Cupons</h2>";
    for (var i = 0; i < cuponList.length; i++) {
        var name = cuponList[i].Name;
        if (cuponList[i].Name.length > 15)
            name = cuponList[i].Name.substring(0, 12) + "...";
        var j = '<div class="col-sm-3 col-lg-3 col-md-3" onclick="ShowCuponModal(' + cuponList[i].Id +
            ')"> <div class="thumbnail"> <img src="images/Coupon.png" alt=""> <div class="caption">' +
            '<h4 class="pull-right">'+cuponList[i].Price+'</h4> <h4><a href="#">' + name + 
            '</a> </h4> <p>'+cuponList[i].Description + '</p>' +
            '</div> <div class="ratings"> <p class="pull-right"></p> '+stars(cuponList[i].Rate)+'</div> </div> </div>';
        document.getElementById("cupons_div").innerHTML += j;
    }
}

function stars(rating) {
    var s = '<p> ';
    if (rating<6)
    for (var j = 0; j < 5; j++) {
        if (j < rating)
            s += '<span class="glyphicon glyphicon-star"></span>';
        else
            s += '<span class="glyphicon glyphicon-star-empty"></span>';
    }
    else {
        s += "N/A";
    }
    return s + '</p>';
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
                        $('#PurchaseButton').click(function() { BuyCupon(cuponId); });
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
    var purchase = new Object();
    purchase.cuponId = cuponId;
    purchase.userId = window.location.search.substring(1);
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
            var s = '<br/>Your Serial Key is <br/><b>'+d+'</b><br/><br/>Order details were sent to your email account.';
            $('#CuponModal').modal('hide');
            $('#generalModalTitle').html('Purchase Succedded!');
            $('#generalModalBody').html(s);
            $('#GeneralModal').modal('show');

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}

function showPosition(position) {
    var lat = position.coords.latitude;
    var lon = position.coords.longitude;
    var latlon = new google.maps.LatLng(lat, lon);
    var mapholder = document.getElementById('mapCanvas');
    mapholder.style.height = '250px';
    mapholder.style.width = '500px';

    var myOptions = {
        center: latlon,
        zoom: 14,
        mapTypeId:google.maps.MapTypeId.ROADMAP,
        mapTypeControl:false,
        navigationControlOptions: { style: google.maps.NavigationControlStyle.SMALL }
    }
    
    var map = new google.maps.Map(document.getElementById("mapCanvas"), myOptions);
    var marker = new google.maps.Marker({position:latlon,map:map,title:"You are here!"});
}

function showdPosition(position) {
    var myLatlng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
    $("#SearchlocationLat").val(position.coords.latitude);
    $("#SearchlocationLon").val(position.coords.longitude);
    var mapOptions = {
        zoom: 14,
        center: myLatlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    mapholder = document.getElementById('mapCanvas');
    mapholder.style.height = '400px';
    mapholder.style.width = '400px';
    var map = new google.maps.Map(document.getElementById('mapCanvas'), mapOptions);

    // Place a draggable marker on the map
    var marker = new google.maps.Marker({
        position: myLatlng,
        map: map,
        draggable: true
    });
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'latLng': marker.position }, function(results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[0]) {
                document.getElementById("Searchlocation").value = results[0].formatted_address;
            }
        }
    });
    google.maps.event.addListener(marker, 'dragend', function(evt) {
        geocoder.geocode({ 'latLng': marker.position }, function(results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                if (results[0]) {
                    document.getElementById("Searchlocation").value = results[0].formatted_address;
                }
            }
        });
    });
}

function NewSocialCupon() {
    var accountId = window.location.search.substring(1);
    var name = $('#SocialName').val();
    var url = $('#SocialLink').val();
    $('#SocialCupon').modal('hide');
    $('#WaitModal').modal('show');
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/CreateSocialNetworkCupon",
        data: JSON.stringify({ "userId": accountId, "name": name, "url": url }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d) {
                $('#generalModalTitle').html("New Social Cupon");
                $('#generalModalBody').html("Your new cupon was successfully added to our DB");
                $('#GeneralModal').modal('show');
            }
            $('#WaitModal').modal('hide');
            
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);
            
        }

    });
}

function RecommendedCupons(position) {
    var o = new Object();
    o.type = parseInt($("#radioBtn .active")[0].id.substring(6));
    if (o.type == 1) {
        o.data = "{\"Longtitude\":" + position.coords.longitude + ",\"Latitude\":" + position.coords.latitude + "}";
    } else if (o.type == 2) {
        o.data = window.location.search.substring(1);
    } else if (o.type == 3) {
        o.data = new Object();
        o.data.id = window.location.search.substring(1);
        o.data.loc = new Object();
        o.data.loc.Longtitude = position.coords.longitude;
        o.data.loc.Latitude = position.coords.latitude;
        o.data = JSON.stringify(o.data);
    } else {
        return;
    }
   
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/Controller/CuponServices.asmx/GetRecommendation",
        data: JSON.stringify(o),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d == "")
                return; 
            ShowRecommendedCupons(JSON.parse(data.d));
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("faliure in ajax call for - " + xhr.status);

        }

    });
}

function ShowRecommendedCupons(cuponList) {
    document.getElementById("cupons_div").innerHTML = "<h2>Reccomended Cupons</h2>";
    for (var i = 0; i < cuponList.length; i++) {
        var name = cuponList[i].Name;
        if (cuponList[i].Name.length > 15)
            name = cuponList[i].Name.substring(0, 12) + "...";
        var j = '<div class="col-sm-3 col-lg-3 col-md-3" onclick="ShowCuponModal(' + cuponList[i].Id +
            ')"> <div class="thumbnail"> <img src="images/Coupon.png" alt=""> <div class="caption">' +
            '<h4 class="pull-right">' + cuponList[i].Price + '</h4> <h4><a href="#">' + name +
            '</a> </h4> <p>' + cuponList[i].Description + '</p>' +
            '</div> <div class="ratings"> <p class="pull-right"></p> ' + stars(cuponList[i].Rate) + '</div> </div> </div>';
        document.getElementById("cupons_div").innerHTML += j;
    }
}

function RecNewButton_OnClick() {
    if (RecNew) {
        $("#RecNewButton").html("New");
        navigator.geolocation.getCurrentPosition(RecommendedCupons);
    } else {
        $("#RecNewButton").html("Reccomended");
        GetAllCupons();
    }
    RecNew = !RecNew;
}