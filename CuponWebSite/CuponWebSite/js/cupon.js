var category;


function SearchByLocation(coords, radius) {
    $('#WaitModal').modal('show');
    $.ajax({
        type: "POST",
        url: "http://localhost:20353/CuponSystemWebService.asmx/FindCuponByLocation",
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

function GetCupons(catId) {
    $('#WaitModal').modal('show');
        $.ajax({
            type: "POST",
            url: "http://localhost:20353/CuponSystemWebService.asmx/FindCuponByPreference",
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
        var j = '<div class="col-sm-4 col-lg-4 col-md-4"> <div class="thumbnail"> <img src="http://placehold.it/320x150" alt=""> <div class="caption">' +
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