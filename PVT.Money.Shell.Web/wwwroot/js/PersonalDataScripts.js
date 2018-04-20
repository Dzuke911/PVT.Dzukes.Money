$(document).ready(function () {

    $("#PasswordChangingMessage").delay(2000).fadeOut(1000);

    $("#ChangePasswordShowButton").click(function () {
        $("#PersonalDataDiv").hide("slow"); 
        $("#PasswordChangeDiv").show("slow");
    });

    $("#Photo").on("change", function (ev) {
        let file = $("#Photo").val();
    });
});

$(document).ready(function () {

    $("#ImageErrorDiv").delay(2000).fadeOut(1000);

});