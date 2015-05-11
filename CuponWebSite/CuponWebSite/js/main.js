var category;
function main(){
	var j = '<div class="col-sm-4 col-lg-4 col-md-4"> <div class="thumbnail"> <img src="http://placehold.it/320x150" alt=""> <div class="caption">'+
                '<h4 class="pull-right">$84.99</h4> <h4><a href="#">FUCK Product</a> </h4> <p>This is a short description. Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>'+
                '</div> <div class="ratings"> <p class="pull-right">6 reviews</p> <p> <span class="glyphicon glyphicon-star"></span> <span class="glyphicon glyphicon-star"></span>'+
                '<span class="glyphicon glyphicon-star"></span><span class="glyphicon glyphicon-star-empty"></span><span class="glyphicon glyphicon-star-empty"></span>'+
                '</p> </div> </div> </div>'
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

SetAccount(12);

function SetAccount(accountId){
	//ajax for user profile
	$("#accountImg").attr("src","http://placehold.it/120x120");
	$("#accountName").text("Ido Hubara");
	$("#accountMail").text("idohubara@gmail.com");
	$("#accountView").click();
}