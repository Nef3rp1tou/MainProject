$(document).ready(function () {
    // Generate Token
    $("#generateTokenBtn").click(function () {
        $.ajax({
            url: "/auth/generate",
            type: "POST",
            success: function (response) {
                if (response.statusCode === 200) {
                    $("#publicToken").val(response.data);

                    $("#submitTokenBtn").show();

                    toastr.success("Token Generated Successfully!");
                } else {
                    toastr.error("Failed to generate token. Error: " + response.message);
                }
            },
            error: function () {
                toastr.error("An error occurred while generating the token.");
            }
        });
    });

});
