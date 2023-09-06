var dropdownBtn = $('#dropdown-btn');
var dropdownContent = $('#dropdown-content');
$(document).ready(function () {
    $("#getStarted").click(function () {
        $("#getStartedRow").fadeOut(500, function () {
            // Callback function to show the second code block after fade-out is complete
            $("#categoryRow").css("display", "flex").fadeIn(900);
            $("#fillBar").css("width", "40%");
        });

    });
    $(".categoryCard").click(function () {
        $(".categoryCard").removeClass("selected");
        $(this).addClass("selected");
        var selectedCategory = $(this).attr("value");
        localStorage.setItem("Category", selectedCategory);
        console.log("Selected category: " + selectedCategory);
        $("#selectCategory").css("pointer-events", "auto");
    });
    $(document).on("click", ".optionCard", function () {
        $(".optionCard").removeClass("selected");
        $(this).addClass("selected");
        var selectedBrand = $(this).attr("value");
        localStorage.setItem("Brand", selectedBrand);
        console.log("Selected brand: " + selectedBrand);
        $("#selectBrand").css("pointer-events", "auto");

    });
    $("#selectCategory").click(function () {
        $("#categoryRow").fadeOut(500, function () {
            $("#brandRow").css("display", "flex").fadeIn(900);
            $("#fillBar").css("width", "70%");
        });

    });
    $("#selectBrand").click(function () {
        $("#brandRow").fadeOut(500, function () {
            $("#priceRow").css("display", "flex").fadeIn(900);
            $("#fillBar").css("width", "90%");
        });

    });
    $("#finshButton").click(function () {
        var input = $("#priceInput").val();
        if (input != "") {
            var priceValue = parseInt($("#priceInput").val());
            if (priceValue < 200) {
                const toast = $('#toast-SSQxZFzM');
                toast.removeClass('hide');
                toast.addClass('show');
                toast.css("opacity", 1);
                $("#alert").text("The minimum price is 200");
                setTimeout(() => {
                    toast.addClass('hide');
                }, 4000);
                return;
            } else {
                var category = localStorage.getItem("Category");
                var brand = localStorage.getItem("Brand");
                var priceValue = parseFloat($("#priceInput").val());
                $.ajax({
                    url: '/api/Guide/' + category + '/' + brand + '/' + priceValue,
                    type: 'GET',
                    success: function (data) {
                      
                        data.forEach(function (component) {
                            $("#priceRow").fadeOut(500, function () {
                                $("#body").addClass("bodyLastPage");
                                $("#componentRow").css("display", "flex").fadeIn(900);
                                $(".progressBar").hide();
                                $('.mainSection').css('background', 'none');
                                $('.mainSection').css('height', 'auto');
                                $("#navBar").show();
                                var oldPriceElement = $("<del class='oldPrice' id='oldPrice'>" + component.oldPrice + "</del>");
                                var newComponent = $("<div class=' mb-2 productCol col-lg-2 col-md-4 col-sm-12 m-3' data-storename=" + component.storeName + " data-price=" + component.price + ">  <div class='componentCard h-100 d-flex flex-column product' > <div class='deatils' id='productCard' data-storename=" + component.storeName + " data-id=" + component.productId + " data-category=" + component.category + " > <div class='componentImg'> <img src='" + component.image + " ' class='img-fluid animated-image '></div><h1 class='text-center'>" + component.name + "</h1><div class='prices d-flex justify-content-center'></div><div class='storeName'><span>" + component.storeName + "</span></div></div></div> </div></div>");
                                if (component.oldPrice === "0") {
                                    oldPriceElement.hide();
                                } else {
                                    oldPriceElement.show();
                                }
                                newComponent.find('.prices').append(oldPriceElement).append("<span class='price'>" + component.price + "JOD</span>");
                                $("#componentRow").append(newComponent.hide().fadeIn(100));
                            });
                        });
                    },
                    error: function (xhr, status, error) {
                        // Handle the error here
                       
                        const toast = $('#toast-SSQxZFzM');
                        toast.removeClass('hide');
                        toast.addClass('show');
                        toast.css("opacity", 1);
                        $("#alert").text("We're sorry we couldn't find your request");
                        setTimeout(() => {
                            toast.addClass('hide');
                        }, 4000);
                        return;
                    }
                });
            }
        }
        else {
            const toast = $('#toast-SSQxZFzM');
            toast.removeClass('hide');
            toast.addClass('show');
            toast.css("opacity", 1);
            $("#alert").text("Please enter price");
            setTimeout(() => {
                toast.addClass('hide');
            }, 4000);
            return;
        }
    });
    $(document).on('click', '#productCard', function () {
        $("#backgroundWrap").css("overflow", "hidden");
        var productId = $(this).data("id");
        var productStoreName = $(this).data("storename");
        var productCategory = $(this).data("category");
        $(".dropdown-container").css("position", "inherit");
        loadProductDeatils(productCategory, productId);
        $('body').css('overflow', 'hidden');
    });
    $(document).on('click', '#productLink', function () {
        var link = $(this).attr("data-link");
        // Create a temporary input element to copy the link to the clipboard
        var tempInput = $("<input>");
        $("body").append(tempInput);
        tempInput.val(link).select();
        document.execCommand("copy");
        tempInput.remove();
        const toast = $('#toast-SSQxZFzM');
        toast.removeClass('hide');
        toast.addClass('show');
        toast.css("opacity", 1);
        $("#alert").text("Product link copied");
        setTimeout(() => {
            toast.addClass('hide');
        }, 4000);

    });
    $("#exitDialog").click(function () {
        $(".productSpecificationsDialog").hide();
        $("#backgroundWrap").hide();
        $(".dropdown-container").css("position", "relative");
        $("#backgroundWrap").css("overflow", "auto");
        $(".componentImg img").addClass("animated-image");
        dropdownContent.removeClass('show');
        $(".description").removeClass("scrollAble");
        $("#dialogDescreptionUl").removeClass("ulShow");
        $(".desciptionListItem").removeClass("show");
        $('body').css('overflow', 'auto');
    });
    dropdownBtn.on('click', function () {
        dropdownContent.toggleClass('show');
        $(".description").toggleClass("scrollAble");
        $("#dialogDescreptionUl").toggleClass("ulShow");
        $(".desciptionListItem").toggleClass("show");

    });
});

function loadProductDeatils(category, productId) {
    $.ajax({
        url: '/api/ProductDetails/' + category + '/' + productId,
        method: 'GET',
        success: function (data) {
            $(".componentImg img").removeClass("animated-image");

            data.forEach(function (component) {
                var name = component.name;

                var oldPrice = component.oldPrice;
                var price = component.price;
                var brand = component.brand;
                var img = component.image;
                const description = component.description;
                var date = component.date;
                var time = component.time;
                var productUrl = component.productUrl;
                var storeName = component.storeName;

                $(".desciptionListItem").remove();
                $('#dialogImg').attr('src', img);
                $("#dialogName").text(name);
                $("#dialogBrand").text("Brand : " + brand);
                $("#dialogPrice").text("Price : " + price + "");
                $("#dialogDateTime").text("Last update : " + date + ":" + time);
                $("#dialogStoreName").text("Store name " + ": " + storeName);
                for (desc in description) {
                    $("#dialogDescreptionUl").append('<li class="desciptionListItem">' + description[desc] + '</li>');
                }
                if (oldPrice != "0") {

                    $(".productPrice").append('<del id="dialogOldPrice"> </del>');
                    $("#dialogOldPrice").text(oldPrice);
                    $("#dialogOldPrice").show
                }
                else {
                    $("#dialogOldPrice").remove();
                }
                if (name != null) {
                    $("#backgroundWrap").show();
                    $(".productSpecificationsDialog").show();
                }
                else {
                    const toast = $('#toast-SSQxZFzM');
                    toast.removeClass('hide');
                    toast.addClass('show');
                    toast.css("opacity", 1);
                    $("#alert").text("Error loading detail");
                    setTimeout(() => {
                        toast.addClass('hide');
                    }, 4000);

                }
                if (productUrl != null) {
                    $("#productLink").attr("data-link", productUrl);
                }
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log('Error: ' + textStatus + ' - ' + errorThrown);
        }
    });

}