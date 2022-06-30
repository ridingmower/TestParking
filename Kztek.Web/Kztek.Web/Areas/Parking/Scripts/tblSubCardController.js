$(function () {

    //$("body").on("keydown", "#MainCard", function (event) {

    //    tblSubCardController.AutoComplete();
    //})

    $("body").on("autocomplete", "#MainCard", function () {
        source: []
    })

    $("body").on("keyup", "#MainCard", function () {
        tblSubCardController.AutoComplete();
    })

    $("body").on("click", "#MainCard", function () {
        tblSubCardController.AutoComplete();
        $("#MainCard").keydown();
    })  

    $("body").on("click", "button[name=btnImport]", function () {
        tblSubCardController.LoadModal();
    })  
    $("body").on("click", "#btnSave", function () {
        tblSubCardController.Import();
    })

})

var tblSubCardController = {
    AutoComplete: function () {
        var key = $("#MainCard").val();
        $.ajax({
            url: _prefixParkingDomain + '/tblSubCard/AutoComplete',
            data: { key: key },
            type: 'json',
            async: true,
            success: function (data) {

                $("#MainCard").autocomplete({
                    source: function (request, response) {
                        response(data);
                        return;
                    },
                    minLength: 0,
                    select: function (event, ui) {
                      
                        $("#MainCard").val(ui.item.id);                      
                    }
                });
            }
        });
    },
    Import: function () {
        $('.loading1').css('display', 'block');
        $('#proccess').css('display', 'block');
        $("#messs").css('display', 'none');

        var file = $("#modalImportCard").find("input[name=FileUpload]").prop("files");

        var formdata = new FormData();

        for (i = 0; i < file.length; i++) {
            //Appending each file to FormData object
            formdata.append(file[i].name, file[i]);
        }

        $.ajax({
            type: "POST",
            url: _prefixParkingDomain + '/tblSubCard/ImportFile',
            data: formdata,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.isSuccess) {
                    $('.loading1').css('display', 'none');

                    toastr.success(response.Message);

                    window.location.href = "/Parking/tblSubCard";
                }
                else {
                    toastr.error(response.Message);
                }

            },
            error: function (error) {
                alert("errror");
                console.log(error);
            }
        });
    },
    LoadModal: function () {
        $.ajax({
            url: _prefixParkingDomain + '/tblSubCard/ModalImport',
            type: 'POST',
            data: {
                
            },
            success: function (response) {
                $('#boxModal').html('');
                $('#boxModal').html(response);
                $("#modalImportCard").modal("show");
            }
        });
    },
}