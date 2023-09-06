

$("#login").click(function () {
    emailInput = $("#email").val();
    passWordInput = $("#password").val();

    if (emailInput != "" && passWordInput !="") {
        var registrationData = {
            email: $("#email").val().replace(/\s/g, ''),
            password: $("#password").val(),
        };
        console.log($("#email").val());
        $.ajax({
            url: "/api/auth/signin",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(registrationData),
            success: function (idToken) {
                // Login successful, retrieve the ID token
                console.log("ID token:", idToken);
                $(".invalidPassword").hide();
                $(".invalidEmail").hide();
                localStorage.setItem("idToken", idToken);
                window.location.replace("/dashboard");

            },
            error: function (xhr, status, error) {
                // Login failed, handle the error
                if (xhr.responseText === "Incorrect password.") {
                    // Incorrect password error
                    $(".invalidPassword").show();
                    $(".invalidEmail").hide();
                } else if (xhr.responseText === "Email not found.") {
                    // Email not found error
                    $(".invalidPassword").hide();
                    $(".invalidEmail").show();
                } else {
                    // Both email and password are incorrect
                    $(".invalidPassword").show();
                    $(".invalidEmail").show();
                }

                console.error("Login error:", xhr.responseText);
            },
        });
    }
   

});



 