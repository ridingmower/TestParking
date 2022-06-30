var CustomerController = {
    GetFinger: function (finger) {
        var controllerid = $("#controllerid").val();
        var userid = $("#UserIDofFinger").val();

        $.ajax({
            url: _prefixAccessDomain + '/tblCustomer/GetFormatFinger',
            data: { controllerid: controllerid, userid: userid, finger: finger },
            type: 'json',
            async: true,
            success: function (data) {
                if (data.isSuccess) {
                    $("#Finger" + finger).val(data.Message);
                    $("#hidFinger" + finger).val(data.Message);
                } else {
                    bootbox.confirm("Dữ liệu vân tay với user = " + userid + " chưa lấy được? Bạn có muốn thử lại", function (result) {
                        if (result) {
                            CustomerController.GetFinger(finger);
                        }
                    });
                }
            }
        });
    }
};

function GetFinger(finger) {
    CustomerController.GetFinger(finger);
}