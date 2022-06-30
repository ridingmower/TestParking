$(function () {
    tblAccessController.Disable();

    $("body").on("change","#IsAddOutput", function () {
        tblAccessController.Disable();
    })
})

var tblAccessController = {
    Disable: function () {
        var isChk = $("#IsAddOutput").is(":checked");

        if (isChk) {
            $("#NumberOutput").prop("disabled", false);
        } else {
            $("#NumberOutput").prop("disabled", true);
        }
    }
}