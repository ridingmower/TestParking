


var CommonUseController = {
    initGate: function () {
        CommonUseController.loadDataGate();
        CommonUseController.registerEventGate();
    },
    registerEventGate: function () {
        $('#boxGate #frmGateQuick').validate({
            rules: {
                txtGateName: {
                    required: true,
                    minlength: 5,
                    maxlength: 150
                }
            },
            messages: {
                txtGateName: {
                    required: "Bạn phải nhập tên cổng",
                    minlength: "Tên phải lớn hơn 5 ký tự",
                    maxlength: "Tên phải quá 150 ký tự"
                }
            }
        });
    },
    openModalGate: function () {
        $('#boxGate #modalGateQuick').modal('show');
        CommonUseController.resetFormGate();
        $('#boxGate #modalGateQuick').find("#boxTitle").text("Thêm mới");
    },
    loadDataGate: function (ID) {
        $.ajax({
            url: '/CommonUse/ListGateActive',
            type: 'GET',
            data: { gateID: ID},
            success: function (response) {
                $('#boxGate').html('');
                $('#boxGate').html(response);
            }
        })
    },
    saveDataGate: function () {
        var name = $('#boxGate #txtGateName').val();
        var obj = {
            GateName: name,
        }

        $.ajax({
            url: '/CommonUse/Create',
            data: {
                obj: obj
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.isSuccess) {
                    CommonUseController.resetFormGate();
                    toastr.success(response.Message);
                    CommonUseController.loadDataGate();
                }
                else {
                    toastr.error(response.Message);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    resetFormGate: function () {
        $('#boxGate #txtGateName').val("");
    }
}

CommonUseController.initGate();

function OpenModalGate() {
    CommonUseController.openModalGate();
}

function SaveGate() {
    CommonUseController.saveDataGate();
    $('#boxGate #modalGateQuick').modal('hide');
    $(".modal-backdrop").css('display', 'none');
}
