$(function () {
    tblLaneController.CheckLoop();

    $("body").on("click", "#IsLoop", function () {
        tblLaneController.CheckLoop();
    })

    $("body").on("change", "#ddlOptionLoop", function () {
        $("#cardgroup").val($(this).val());
    })
})

var tblLaneController = {
    OptionLoop: function () {
        $.ajax({
            url: _prefixParkingDomain + '/tblLane/Partial_OptionLoop',
            type: 'POST',
            data: {
                laneid: $("#LaneID").val(),
                selected: $("#cardgroup").val()
            },
            success: function (response) {
                $('#boxOptionLoop').html('');
                $('#boxOptionLoop').html(response);
            }
        });
    },
    CheckLoop: function () {
        var cmd = $("#IsLoop");

        if (cmd.is(":checked")) {
            tblLaneController.OptionLoop();
        } else {
            $('#boxOptionLoop').html('');
        }
    }
}