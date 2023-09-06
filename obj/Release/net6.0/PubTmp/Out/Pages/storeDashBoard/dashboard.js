$(document).ready(function () {
    window.addEventListener('unload', function () {
        localStorage.clear(); // Remove all items from local storage
    });
    if (localStorage.getItem("idToken") === null) {

        window.location.href = "https://localhost:7277/login/";
    }
    else {
        var token = localStorage.getItem('idToken');
    }
  
    var storeName = "";
    getStoreName(token)
        .then(function (storeName) {
            // Handle the store name here
            loadProducts(storeName);
            localStorage.setItem("storeName", storeName);
            // Continue with any other code that depends on the store name value
        })
        .catch(function (error) {
            // Handle the error here
            console.log('Error:', error);
            // Continue with any other error handling logic
        });
$(document).on('click', '#addNewProductButton', function () {
    $("#backgroundWrap").show();
    $(".productSpecificationsDialog").show();
    $("#addProductImg").removeClass("disabledImg");
    $('#productCategory').prop('disabled', false);
    $('.productInfo').prop('readonly', false);
    $("#saveChanges").hide();
    showSaveButtonHideEditButton();
});
  $("#editButton").click(function () {
        $(this).hide();
        editProduct();
       
    });
  $("#exitDialog").click(function () {
    $(".productSpecificationsDialog").hide();
    $("#backgroundWrap").hide();
    $(".dropdown-container").css("position", "relative");
    $("#backgroundWrap").css("overflow", "auto");
});
  $("#addProductImg").click(function () {
        if (!$('#addProductImg').hasClass('disabledImg')) {
            $("#imageInput").click();
        } 
  });
  $("#imageInput").change(function() {
    var file = this.files[0];
    var reader = new FileReader();

    reader.onload = function(e) {
      $("#addProductImg").empty(); // Clear existing content
      $("#addProductImg").append('<img id="addedImg" src="' + e.target.result + '">');
    };

    reader.readAsDataURL(file);
  });
  $("#productPriceOriginal, #productPriceDiscount").on("input", function(event) {
    let value = $(this).val();
    value = value.replace(/[^0-9]/g, ""); // Remove non-numeric characters
    $(this).val(value);
  });
  $("#saveButton").click(function () {
        // Get the input values

      var isFormValid = true;

      // Reset validation
      $(".productInput").removeClass("invalid-input");
      $("#fillFields").hide();

      // Check if any input is empty
      $(".productInput").each(function () {
          if ($(this).val() === "") {
              $(this).addClass("invalid-input");
              $("#fillFields").show();

              isFormValid = false;
          }
      });

      // Check if category select is not valid
      if ($("#productCategory").val() === "category" || $("#productCategory").val() === "") {
          $("#productCategory").addClass("invalid-input");
          $("#fillFields").show();
          isFormValid = false;
      }

      // Check if image is added
      if ($("#addedImg").length === 0 || !$("#addedImg").attr("src")) {
          // Image is not added, display warning or perform necessary action
          $("#fillFields").show();
          isFormValid = false;
      }

      // Prevent form submission if any validation fails
      if (!isFormValid) {
          return false;
      }
      if (isFormValid) { 
       token = localStorage.getItem('idToken');
      var productName = $("#productName").val();
        var productCategory = $("#productCategory").val();
        var productBrand = $("#productBrand").val();
        var productPriceOriginal = $("#productPriceOriginal").val();
        var productPriceDiscount = $("#productPriceDiscount").val();
        var productContact = $("#productContact").val();
        var productDescription = $("#productDescription").val();
          var imageFile = $("#imageInput")[0].files[0];
          var currentStoreName = localStorage.getItem("storeName");
 
        // Create a FormData object
        var formData = new FormData();
       formData.append("UID", token); // Replace with the actual Firebase token
       formData.append("StoreName", currentStoreName); 
      formData.append("Name", productName);
      formData.append("Image", "Empty");
        formData.append("Category", productCategory);
        formData.append("Img", imageFile);
      formData.append("Price", productPriceOriginal);
      formData.append("OldPrice", productPriceDiscount);
        formData.append("Brand", productBrand);
      formData.append("description", productDescription);
      formData.append("productUrl", productContact);

        // Make the AJAX call
        $.ajax({
            url: "/api/addCustomProducts/addProduct",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                // Handle the successful response
                $("#exitDialog").click();
                loadProducts(currentStoreName)
                const toast = $('#toast-SSQxZFzM');
                toast.removeClass('hide');
                toast.addClass('show');
                toast.css("opacity", 1);
                $("#alert").text("Product added");
                setTimeout(() => {
                    toast.addClass('hide');
                }, 4000);
                // Do something with the response, such as showing a success message
            },
            error: function (xhr, status, error) {
                // Handle the error response
                console.error(xhr.responseText);
                if (xhr.responseText =="Product already exists") {
                    const toast = $('#toast-SSQxZFzM');
                    toast.removeClass('hide');
                    toast.addClass('show');
                    toast.css("opacity", 1);
                    $("#alert").text("Product already exists");
                    setTimeout(() => {
                        toast.addClass('hide');
                    }, 4000);
                }
                // Do something with the error, such as showing an error message
            }
        });
      }
});
  $("#exitDialog").click(function() {
    $(".productInput").removeClass("invalid-input");
    $("#fillFields").hide();
    $(".productInput,.productInputDiscount").val("");
    $("#productCategory").removeClass("invalid-input").val("category");
      $("#addProductImg").empty();
      $("#addProductImg").append('<div class="d-flex justify-content-center"><a>+</a></div> <h1>Add image</h1>');

  });
  $("#saveChanges").click(function () {
        
        var productId = $(this).attr("productId");
      
        var productCategory = $(this).attr("productCategory");
        saveProductChanges(productId, productCategory);

  });
  $(document).on('click', '#deleteButton', function () {
        var productId = $(this).data("id");
        var productCategory = $(this).data("category");
        var productStore = $(this).data("storename");
        deleteProduct(productId, productCategory, productStore);
    });
   $("#logOut").click(function () {
        logOut();
    });
  $(document).on('click', '#productCard', function () {
        $("#backgroundWrap").css("overflow", "hidden");
        var productId = $(this).data("id");
        $(".dropdown-container").css("position", "inherit");
        var productCategory = $(this).data("category");
      var productsCount = $("#addedProducts").val();
        loadProductDeatils(productCategory, productId);

  });
});
function getStoreName(token) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: '/api/DashBoardData/getStoreName',
            type: 'GET',
            data: { Token: token }, // Replace 'your-uid-value' with the actual UID value
            success: function (response) {
                // Handle the successful response
                resolve(response);
                // Do something with the store name, such as displaying it on the page
            },
            error: function (xhr, status, error) {
                // Handle the error response
                console.log('Error:', xhr.responseText);
                // Do something with the error, such as displaying an error message
            }
        });
    });
    
}
function loadProducts(storeNameValue) {
    var productsCount = 0;
    $.ajax({
        url: "/api/dashBoardProducts/" + storeNameValue , // Replace with your actual controller route
        type: "GET",
        success: function (data) {
            console.log(data.length);
            if (data.length > 0) {
                $(".productCol").remove();
                data.forEach(function (component) {
                    productsCount = productsCount + 1;
                    $("#componentsRow").prepend(
                        "<div class='productCol col-lg-3 col-md-4 col-sm-12 m-3' data-storename=" +
                        component.storeName +
                        " data-price=" +
                        component.price +
                        ">" +
                        "<div class='componentCard h-100 d-flex flex-column product'>" +
                        "<div class='deatils' id='productCard' data-storename=" +
                        component.storeName +
                        " data-id=" +
                        component.productId +
                        " data-category=" +
                        component.category +
                        ">" +
                        "<div class='componentImg'>" +
                        "<img src='" +
                        component.image +
                        "' class='img-fluid'>" +
                        "</div>" +
                        "<h1 class='text-center'>" +
                        component.name +
                        "</h1>" +
                        "<div class='prices d-flex justify-content-center'>" +
                        "<del class='oldPrice' id='oldPrice'>" +
                        component.oldPrice +
                        "</del>" +
                        "<span class='price'>" +
                        component.price +
                        "JOD</span>" +
                        "</div>" +
                        "<div class='storeName'>" +
                        "<span>" +
                        component.storeName +
                        "</span>" +
                        "</div>" +
                        "</div>" +
                        "<div class='deleteButton'>" +
                        "<a class='delete' id='deleteButton' data-id=" +
                        component.productId +
                        " data-category=" +
                        component.category +
                        " data-storename=" +
                        component.storeName +
                        ">Delete</a>" +
                        "</div>" +
                        "</div>" +
                        "</div>"
                    );
                });
                $("#addedProducts").text(productsCount);
                $("#newProduct").toggle(productsCount < 5);
            }
            else if (data.length <= 0) {
                // Handle empty response
                $("#componentRow").append("<p>No products found.</p>");
            }
        },
        error: function (error) {
            // Handle error
        }
    });
}
function loadProductDeatils(category, id) {
    showEditButtonHideShowButton();
    $('#saveChanges').attr('productId', id);
    $('#saveChanges').attr('productCategory', category);
    $(".productInfo,#productCategory").css("background-color", "#120830");
    $('.productInfo').prop('readonly', true);
    $('#productCategory').prop('disabled', true);
    $("#addProductImg").addClass("disabledImg");
    $("#addProductImg").empty(); 
    if ($('#addProductImg').hasClass('disabledImg')) {
        $("#saveChanges").addClass("disabledButton");
    }
    $.ajax({
        url: '/api/dashBoardProducts/' + category + '/' + id,
        type: 'GET',
        success: function (data) {
           
            data.forEach(function (component) {
                console.log(component.name);
            // Populate the input fields with the product data
                $('#productName').val(component.name);
                $("#addProductImg").append('<img id="addedImg" src="' + component.image + '">');
                $('#productCategory').val(component.category);
                $('#productBrand').val(component.brand);
                $('#productPriceOriginal').val(component.price);
                $('#productPriceDiscount').val(component.oldPrice);
                $('#productContact').val(component.productUrl);
                $('#productDescription').val(component.description.join('\n'));
            $("#backgroundWrap").show();
                $(".productSpecificationsDialog").show();
            });
        },
        error: function (xhr, status, error) {
            console.log(error); // Handle the error accordingly
        }
    });
}
function showSaveButtonHideEditButton() {

    if(!$("#saveButton").is(":visible")) {
        $("#saveButton").show();
    }
    if ($("#editButton").is(":visible")) {
        $("#editButton").hide();
    }
}
function showEditButtonHideShowButton() {
    $("#saveButton").hide();
    
    if (!$("#editButton").is(":visible")) {
        $("#editButton").show();
    }
}
function editProduct() {

    $('#addProductImg').removeClass('disabledImg');
    $(".productInfo,#productCategory").css("background-color", "#1a0c45");
    $('.productInfo').prop('readonly', false);
    $("#imageInput").removeClass("disabledImg");
    $('#productCategory').prop('disabled', true);
    $("#saveChanges").show();
    if (!$('#addProductImg').hasClass('disabledImg')) {
        $("#saveChanges").removeClass("disabledButton");
    }
}
function saveProductChanges(productId, productCategory, productStore) {
    var isFormValid = true;
    // Reset validation
    $(".productInput").removeClass("invalid-input");
    $("#fillFields").hide();

    // Check if any input is empty
    $(".productInput").each(function () {
        if ($(this).val() === "") {
            $(this).addClass("invalid-input");
            isFormValid = false;
            $("#fillFields").show();
        }
    });

    // Check if category select is not valid
    if ($("#productCategory").val() === "category" || $("#productCategory").val() === "") {
        $("#productCategory").addClass("invalid-input");
        isFormValid = false;
    }

    // Check if image is added
    if ($("#addedImg").length === 0 || !$("#addedImg").attr("src")) {
        // Image is not added, display warning or perform necessary action
        $("#fillFields").show();
        isFormValid = false;
    }

    // Prevent form submission if any validation fails
    if (!isFormValid) {
        return false;
    }

    if (isFormValid) {

        var storeName = $(".productCol").data("storename");
        console.log(storeName);
    var productName = $("#productName").val();
    var productBrand = $("#productBrand").val();
    var productPriceOriginal = $("#productPriceOriginal").val();
    var productPriceDiscount = $("#productPriceDiscount").val();
    var productContact = $("#productContact").val();
    var productDescription = $("#productDescription").val();
    var imageFile = $("#imageInput")[0].files[0]; // Get the selected image file
    
    // Create a FormData object
    var formData = new FormData();
    if (imageFile !== undefined && imageFile !== null) {
        formData.append("Img", imageFile);
        formData.append("Image", "Empty");

    } else {
        // Include null or an empty value for Img to indicate no new image is being uploaded
        formData.append("Img", "");
        formData.append("Image", "Empty");
    }
// or formData.append("Img", null);
    
    formData.append("StoreName", storeName);
    formData.append("Name", productName);
    formData.append("Price", productPriceOriginal);
    formData.append("OldPrice", productPriceDiscount);
    formData.append("Brand", productBrand);
    formData.append("description", productDescription);
    formData.append("productUrl", productContact);
    $.ajax({
        url: '/api/addCustomProducts/updateProduct/' + productCategory + "/" + productId,  // Replace 'category-value' and 'productId-value' with actual values
        type: 'PUT',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            // Request succeeded
            $("#exitDialog").click();
            const toast = $('#toast-SSQxZFzM');
            toast.removeClass('hide');
            toast.addClass('show');
            toast.css("opacity", 1);
            $("#alert").text("Product updated");
            setTimeout(() => {
                toast.addClass('hide');
            }, 4000);
            console.log(response);
        },
        error: function (xhr, status, error) {
            // Request failed
            console.error(xhr.response);
        }
    });
    }
}
function deleteProduct(productId, productCategory) {
    $.ajax({
        url: `/api/addCustomProducts/deleteProduct/${productCategory}/${productId}`,
        type: 'Delete',
        success: function (response) {
            var currentStoreName = localStorage.getItem("storeName");
            var numberOfProducts = $("#addedProducts").text();
            if (numberOfProducts === "1") {
                location.reload();
            }
            else {
                loadProducts(currentStoreName)
            }
            const toast = $('#toast-SSQxZFzM');
            toast.removeClass('hide');
            toast.addClass('show');
            toast.css("opacity", 1);
            $("#alert").text("Product deleted");
            setTimeout(() => {
                toast.addClass('hide');
            }, 4000);
        },
        error: function (xhr, status, error) {
            // Error occurred during product deletion
            console.error(xhr.responseText);
        }
    });
}
function logOut() {
    localStorage.removeItem("idToken");
    window.location.href = "https://localhost:7277/login/";

}