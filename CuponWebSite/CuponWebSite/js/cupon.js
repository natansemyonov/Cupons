var category;
function main() {
    var j = '<div class="col-sm-4 col-lg-4 col-md-4"> <div class="thumbnail"> <img src="http://placehold.it/320x150" alt=""> <div class="caption">' +
        '<h4 class="pull-right">$84.99</h4> <h4><a href="#">FUCK Product</a> </h4> <p>This is a short description. Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>' +
        '</div> <div class="ratings"> <p class="pull-right">6 reviews</p> <p> <span class="glyphicon glyphicon-star"></span> <span class="glyphicon glyphicon-star"></span>' +
        '<span class="glyphicon glyphicon-star"></span><span class="glyphicon glyphicon-star-empty"></span><span class="glyphicon glyphicon-star-empty"></span>' +
        '</p> </div> </div> </div>';
	document.getElementById("cupons_div").innerHTML +=j;
}
function SetView(view){
document.getElementById("products").setAttribute("hidden",true);
document.getElementById("profile").setAttribute("hidden",true);
document.getElementById("search").setAttribute("hidden",true);
document.getElementById("about").setAttribute("hidden",true);
document.getElementById("login").setAttribute("hidden",true);
document.getElementById(view).removeAttribute("hidden");
}

//function ajaxCall(type , url , data ,usr , pass,success,error)
//{
//var cat = "Shopping";

//$.ajax({
//    type: "POST",
//    url: "http://localhost:20353/CuponSystemWebService.asmx/FindCuponByPreference",
//    data: JSON.stringify({ "c": "1" }),
//    contentType: "application/json; charset=utf-8",
//    dataType: "json",
//    success: function (data) {
//        ShowCupons(JSON.parse(data.d));
//    },
//    error: function (xhr, ajaxOptions, thrownError) {
//        console.log("faliure in ajax call for " + func + " - " + xhr.status);

//    }

//});
    function GetCupons(catId) {
        $.ajax({
            type: "POST",
            url: "http://localhost:20353/CuponSystemWebService.asmx/FindCuponByPreference",
            data: JSON.stringify({ "c": catId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                ShowCupons(JSON.parse(data.d));
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log("faliure in ajax call for - " + xhr.status);

            }

        });
    }
//}

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
        if (j <= rating)
            s += '<span class="glyphicon glyphicon-star"></span>';
        else
            s += '<span class="glyphicon glyphicon-star-empty"></span>';

    }
    return s + '</p>';
}