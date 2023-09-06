$(document).ready(function () {
    function onGoogleSignIn(googleUser) {
    // Get the Google ID token
    var idToken = googleUser.getAuthResponse().id_token;

    // Send the token to your server for further processing
    // You can make an AJAX call to your server or use it as needed
    console.log(":e");
    // Example AJAX call
    $.ajax({
        url: '/api/RegisterUser/register',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ googleIdToken: idToken }),
        success: function (response) {
            // Handle the successful response
            console.log('Registration successful');
            console.log('Token:', response.Token);
            console.log('UID:', response.Uid);
            // Do something with the token and UID, e.g., store them in local storage
        },
        error: function (jqXHR, textStatus, errorThrown) {
            // Handle the error response
            console.log('Registration failed');
            console.log('Error:', errorThrown);
        }
    });
    }
});
$("#signUp").click(function () {
    var registrationData = {
        email: $("#email").val().replace(/\s/g, ''),
        password: $("#password").val(),
    };
    var storename = $("#storeNmae").val();
    console.log(storename);
    $.ajax({
        url: "/api/DashBoardData/CheckForStoreDuplicate",
        type: "POST",
        dataType: "text", // Set the expected response type to plain text
        contentType: "application/json",
        data: JSON.stringify(storename),
        success: function (response) {
            // Handle successful response
            $.ajax({
                url: '/api/RegisterUser',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(registrationData),
                success: function (response) {
                    var storeName = $("#storeNmae").val();
                    var uidObject = response.uid;
                    var uidString = String(uidObject); // Convert the token object to a string
                    $.ajax({
                        url: "/api/DashBoardData/create",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({ UID: uidString, storeName: storeName }),
                        success: function (response) {

                            window.location.href = "https://localhost:7277/login/";


                        },
                        error: function (xhr, textStatus, errorThrown) {
                            // Handle error
                        }
                    });
                },
                error: function (xhr, textStatus, errorThrown) {
                    // Handle registration error
                    console.log(xhr.responseText);
                    if (xhr.responseText === "Email already exists") {
                        $(".emailExists").show();
                        $(".invaildEmail").hide();
                        $(".passwordCount").hide();
                        $(".HasNumberAndLetter").hide();
                    }
                    else if (xhr.responseText === "Invalid email address") {
                        $(".emailExists").hide();
                        $(".passwordCount").hide();
                        $(".HasNumberAndLetter").hide();
                        $(".invaildEmail").show();
                    }
                    else if (xhr.responseText === "Password must be at least 6 characters long") {

                        $(".emailExists").hide();
                        $(".invaildEmail").hide();
                        $(".HasNumberAndLetter").hide();
                        $(".passwordCount").show();

                    }
                    else if (xhr.responseText === "Password must contain both numbers and letters") {
                        $(".emailExists").hide();
                        $(".invaildEmail").hide();
                        $(".passwordCount").hide();
                        $(".HasNumberAndLetter").show();
                    }
                    else {
                        $(".emailExists").hide();
                        $(".invaildEmail").hide();
                        $(".passwordCount").hide();
                        $(".HasNumberAndLetter").hide();
                    }
                }
            });
            $(".storeExists").hide();
        },
        error: function (xhr, status, error) {
            // Handle error response
         
            if (xhr.responseText ==="Store name already exists") {
                $(".storeExists").show();
            }
            else {
                $(".storeExists").hide();
            }
            console.error(error);
        }
    });

  
 /*   $.ajax({
        url: "/api/DashBoardData/CheckForStoreDuplicate",
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify({storeName:storename}),
        success: function (response) {
  $.ajax({
                url: '/api/RegisterUser',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(registrationData),
                success: function (response) {
                    var storeName = $("#storeNmae").val();
                    var uidObject = response.uid;
                    var uidString = String(uidObject); // Convert the token object to a string
                    $.ajax({
                        url: "/api/DashBoardData/create",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({ UID: uidString, storeName: storeName }),
                        success: function (response) {
                        },
                        error: function (xhr, textStatus, errorThrown) {
                            // Handle error
                        }
                    });
                },
                error: function (xhr, textStatus, errorThrown) {
                    // Handle registration error
                    console.log(xhr.responseText);
                    if (xhr.responseText === "Email already exists") {
                        $(".emailExists").show();
                        $(".invaildEmail").hide();
                        $(".passwordCount").hide();
                        $(".HasNumberAndLetter").hide();
                    }
                    else if (xhr.responseText === "Invalid email address") {
                        $(".emailExists").hide();
                        $(".passwordCount").hide();
                        $(".HasNumberAndLetter").hide();
                        $(".invaildEmail").show();
                    }
                    else if (xhr.responseText === "Password must be at least 6 characters long") {

                        $(".emailExists").hide();
                        $(".invaildEmail").hide();
                        $(".HasNumberAndLetter").hide();
                        $(".passwordCount").show();
                        
                    }
                    else if (xhr.responseText === "Password contains both numbers and letters") {

                        $(".emailExists").hide();
                        $(".invaildEmail").hide();
                        $(".passwordCount").hide();
                        $(".HasNumberAndLetter").show();
                    }
                    else {
                        $(".emailExists").hide();
                        $(".invaildEmail").hide();
                        $(".passwordCount").hide();
                        $(".HasNumberAndLetter").hide();
                    }
                }
  });
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log(xhr.responseText);
            if (xhr.responseText === "No Duplicate for this store") {
                // No duplicate store name
                console.log("No duplicate store name");
                $("#storeExists").hide();

             
            } else {
                // Store name already exists
                console.log("Store name already exists");
                $("#storeExists").show();
            }


            // Handle the error response
        }
    });
  
  */
});



 