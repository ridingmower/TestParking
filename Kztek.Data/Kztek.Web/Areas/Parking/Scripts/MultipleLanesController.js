$(function () {



    //$("body").on("click", "#CurrentDirection ", function () {
    //table_1
    $("body").on("click", "#tableid_1", function () {

        var ViewOrder = 1;
      
        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/ModalCreateAndUpdatePC',
            type: 'POST',
            data: {
                ViewOrder: ViewOrder
            },
            success: function (response) {
                $('#boxModal').html('');
                $('#boxModal').html(response);
                $("#modalImportCard_1").modal("show");
            }
        });
    });

    //table_2

    $("body").on("click", "#tableid_2", function () {

        var ViewOrder = 2;

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/ModalCreateAndUpdatePC',
            type: 'POST',
            data: {
                ViewOrder: ViewOrder
            },
            success: function (response) {
                $('#boxModal').html('');
                $('#boxModal').html(response);
                $("#modalImportCard_2").modal("show");
            }
        });
    });

    //table_3

    $("body").on("click", "#tableid_3", function () {

        var ViewOrder = 3;

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/ModalCreateAndUpdatePC',
            type: 'POST',
            data: {
                ViewOrder: ViewOrder
            },
            success: function (response) {
                $('#boxModal').html('');
                $('#boxModal').html(response);
                $("#modalImportCard_3").modal("show");
            }
        });
    });


    //table_4

    $("body").on("click", "#tableid_4", function () {

        var ViewOrder = 4;

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/ModalCreateAndUpdatePC',
            type: 'POST',
            data: {
                ViewOrder: ViewOrder
            },
            success: function (response) {
                $('#boxModal').html('');
                $('#boxModal').html(response);
                $("#modalImportCard_4").modal("show");
            }
        });
    });


    //table_5

    $("body").on("click", "#tableid_5", function () {

        var ViewOrder = 5;

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/ModalCreateAndUpdatePC',
            type: 'POST',
            data: {
                ViewOrder: ViewOrder
            },
            success: function (response) {
                $('#boxModal').html('');
                $('#boxModal').html(response);
                $("#modalImportCard_5").modal("show");
            }
        });
    });


    //table_6

    $("body").on("click", "#tableid_6", function () {

        var ViewOrder = 6;

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/ModalCreateAndUpdatePC',
            type: 'POST',
            data: {
                ViewOrder: ViewOrder
            },
            success: function (response) {
                $('#boxModal').html('');
                $('#boxModal').html(response);
                $("#modalImportCard_6").modal("show");
            }
        });
    });


    //table_7

    $("body").on("click", "#tableid_7", function () {

        var ViewOrder = 7;

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/ModalCreateAndUpdatePC',
            type: 'POST',
            data: {
                ViewOrder: ViewOrder
            },
            success: function (response) {
                $('#boxModal').html('');
                $('#boxModal').html(response);
                $("#modalImportCard_7").modal("show");
            }
        });
    });


    //table_8

    $("body").on("click", "#tableid_8", function () {

        var ViewOrder = 8;

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/ModalCreateAndUpdatePC',
            type: 'POST',
            data: {
                ViewOrder: ViewOrder
            },
            success: function (response) {
                $('#boxModal').html('');
                $('#boxModal').html(response);
                $("#modalImportCard_8").modal("show");
            }
        });
    });


    //table_9

    $("body").on("click", "#tableid_9", function () {

        var ViewOrder = 9;

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/ModalCreateAndUpdatePC',
            type: 'POST',
            data: {
                ViewOrder: ViewOrder
            },
            success: function (response) {
                $('#boxModal').html('');
                $('#boxModal').html(response);
                $("#modalImportCard_9").modal("show");
            }
        });
    });
//    $("body").on("click", "#btnSave", function () {


//      var  items = [];

//        var lengt = $('#Count').val();
//        for (var i = 0; i < lengt; i++) {
//            var c = i + 1;       
//            var obj = {
//                PCid: $(' #tableid_' + c).find('#PCid').val(),
//                ViewOrder: c,
//                CurrentDirection: $(' #tableid_' + c).find('#CurrentDirection').val(),
//                SideIndex: $(' #tableid_' + c).find("#SideIndex").val()
//            //     Name: $(' #modalImportCard').find('#Name').val(),
//            //PCid: $(' #modalImportCard').find('#PCid').val(),
//            //ViewOrder: 1,
//            //CurrentDirection: $(' #modalImportCard').find('#CurrentDirection').val(),
//            //SideIndex: $(' #modalImportCard').find("#SideIndex").val()
//            }
//            items.push(obj);



//        }
//        items = JSON.stringify({ 'list': items });
//        $.ajax({
//            url: _prefixParkingDomain + '/MultipleLanesMap/SavePcId',
//            type: 'POST',
//            data: items,
//            contentType: 'application/json; charset=utf-8',
//            dataType: 'json',


//            success: function (data) {
//                if (data.isSuccess) {

//                    toastr.success(data.Message, "");
//                } else {
//                    toastr.error(data.Message, "");
//                }
//            }
//        });



  //      });
    //table_1
    $("body").on("click", "#btnSave_1", function () {

    /*    var viewOder = ('#tableid_1').find("#ViewOrder").val();*/
        var obj = {
            Name: $(' #modalImportCard_1').find('#Name').val(),
            PCid: $(' #modalImportCard_1').find('#PCid').val(),
            ViewOrder: 1,
            CurrentDirection: $(' #modalImportCard_1').find('#CurrentDirection').val(),
            SideIndex: $(' #modalImportCard_1').find("#SideIndex").val()

        }

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/SaveOnlyPc',   
            type: 'POST',
            data: {
                obj: obj
            },

            success: function (data) {
                if (data.isSuccess) {
                    
                    $("#modalImportCard_1").modal("hide");
                    var vlue = $('#countpcid').val();
                    MultipleLanesController.showPc(vlue)
                    toastr.success(data.Message, "");
                } else {
                    toastr.error(data.Message, "");
                }
            }
        });



    });
    //table_2
    $("body").on("click", "#btnSave_2", function () {

        /*    var viewOder = ('#tableid_1').find("#ViewOrder").val();*/
        var obj = {
            Name: $(' #modalImportCard_2').find('#Name').val(),
            PCid: $(' #modalImportCard_2').find('#PCid').val(),
            ViewOrder: 2,
            CurrentDirection: $(' #modalImportCard_2').find('#CurrentDirection').val(),
            SideIndex: $(' #modalImportCard_2').find("#SideIndex").val()

        }

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/SaveOnlyPc',
            type: 'POST',
            data: {
                obj: obj
            },

            success: function (data) {
                if (data.isSuccess) {

                    $("#modalImportCard_2").modal("hide");
                    var vlue = $('#countpcid').val();
                    MultipleLanesController.showPc(vlue)
                    toastr.success(data.Message, "");
                } else {
                    toastr.error(data.Message, "");
                }
            }
        });



    });

    //table_3
    $("body").on("click", "#btnSave_3", function () {

        /*    var viewOder = ('#tableid_1').find("#ViewOrder").val();*/
        var obj = {
            Name: $(' #modalImportCard_3').find('#Name').val(),
            PCid: $(' #modalImportCard_3').find('#PCid').val(),
            ViewOrder: 3,
            CurrentDirection: $(' #modalImportCard_3').find('#CurrentDirection').val(),
            SideIndex: $(' #modalImportCard_3').find("#SideIndex").val()

        }

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/SaveOnlyPc',
            type: 'POST',
            data: {
                obj: obj
            },

            success: function (data) {
                if (data.isSuccess) {

                    $("#modalImportCard_3").modal("hide");
                    var vlue = $('#countpcid').val();
                    MultipleLanesController.showPc(vlue)
                    toastr.success(data.Message, "");
                } else {
                    toastr.error(data.Message, "");
                }
            }
        });



    });

    //table_4
    $("body").on("click", "#btnSave_4", function () {

        /*    var viewOder = ('#tableid_1').find("#ViewOrder").val();*/
        var obj = {
            Name: $(' #modalImportCard_4').find('#Name').val(),
            PCid: $(' #modalImportCard_4').find('#PCid').val(),
            ViewOrder: 4,
            CurrentDirection: $(' #modalImportCard_4').find('#CurrentDirection').val(),
            SideIndex: $(' #modalImportCard_4').find("#SideIndex").val()

        }

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/SaveOnlyPc',
            type: 'POST',
            data: {
                obj: obj
            },

            success: function (data) {
                if (data.isSuccess) {

                    $("#modalImportCard_4").modal("hide");
                    var vlue = $('#countpcid').val();
                    MultipleLanesController.showPc(vlue)
                    toastr.success(data.Message, "");
                } else {
                    toastr.error(data.Message, "");
                }
            }
        });



    });

    //table_5
    $("body").on("click", "#btnSave_5", function () {

        /*    var viewOder = ('#tableid_1').find("#ViewOrder").val();*/
        var obj = {
            Name: $(' #modalImportCard_5').find('#Name').val(),
            PCid: $(' #modalImportCard_5').find('#PCid').val(),
            ViewOrder: 5,
            CurrentDirection: $(' #modalImportCard_5').find('#CurrentDirection').val(),
            SideIndex: $(' #modalImportCard_5').find("#SideIndex").val()

        }

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/SaveOnlyPc',
            type: 'POST',
            data: {
                obj: obj
            },

            success: function (data) {
                if (data.isSuccess) {

                    $("#modalImportCard_5").modal("hide");
                    var vlue = $('#countpcid').val();
                    MultipleLanesController.showPc(vlue)
                    toastr.success(data.Message, "");
                } else {
                    toastr.error(data.Message, "");
                }
            }
        });



    });

    //table_6
    $("body").on("click", "#btnSave_6", function () {

        /*    var viewOder = ('#tableid_1').find("#ViewOrder").val();*/
        var obj = {
            Name: $(' #modalImportCard_6').find('#Name').val(),
            PCid: $(' #modalImportCard_6').find('#PCid').val(),
            ViewOrder: 6,
            CurrentDirection: $(' #modalImportCard_6').find('#CurrentDirection').val(),
            SideIndex: $(' #modalImportCard_6').find("#SideIndex").val()

        }

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/SaveOnlyPc',
            type: 'POST',
            data: {
                obj: obj
            },

            success: function (data) {
                if (data.isSuccess) {

                    $("#modalImportCard_6").modal("hide");
                    var vlue = $('#countpcid').val();
                    MultipleLanesController.showPc(vlue)
                    toastr.success(data.Message, "");
                } else {
                    toastr.error(data.Message, "");
                }
            }
        });



    });

    //table_7
    $("body").on("click", "#btnSave_7", function () {

        /*    var viewOder = ('#tableid_1').find("#ViewOrder").val();*/
        var obj = {
            Name: $(' #modalImportCard_7').find('#Name').val(),
            PCid: $(' #modalImportCard_7').find('#PCid').val(),
            ViewOrder: 7,
            CurrentDirection: $(' #modalImportCard_7').find('#CurrentDirection').val(),
            SideIndex: $(' #modalImportCard_7').find("#SideIndex").val()

        }

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/SaveOnlyPc',
            type: 'POST',
            data: {
                obj: obj
            },

            success: function (data) {
                if (data.isSuccess) {

                    $("#modalImportCard_7").modal("hide");
                    var vlue = $('#countpcid').val();
                    MultipleLanesController.showPc(vlue)
                    toastr.success(data.Message, "");
                } else {
                    toastr.error(data.Message, "");
                }
            }
        });



    });

    //table_8
    $("body").on("click", "#btnSave_8", function () {

        /*    var viewOder = ('#tableid_1').find("#ViewOrder").val();*/
        var obj = {
            Name: $(' #modalImportCard_8').find('#Name').val(),
            PCid: $(' #modalImportCard_8').find('#PCid').val(),
            ViewOrder: 8,
            CurrentDirection: $(' #modalImportCard_8').find('#CurrentDirection').val(),
            SideIndex: $(' #modalImportCard_8').find("#SideIndex").val()

        }

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/SaveOnlyPc',
            type: 'POST',
            data: {
                obj: obj
            },

            success: function (data) {
                if (data.isSuccess) {

                    $("#modalImportCard_8").modal("hide");
                    var vlue = $('#countpcid').val();
                    MultipleLanesController.showPc(vlue)
                    toastr.success(data.Message, "");
                } else {
                    toastr.error(data.Message, "");
                }
            }
        });



    });

    //table_9
    $("body").on("click", "#btnSave_9", function () {

        /*    var viewOder = ('#tableid_1').find("#ViewOrder").val();*/
        var obj = {
            Name: $(' #modalImportCard_9').find('#Name').val(),
            PCid: $(' #modalImportCard_9').find('#PCid').val(),
            ViewOrder: 9,
            CurrentDirection: $(' #modalImportCard_9').find('#CurrentDirection').val(),
            SideIndex: $(' #modalImportCard_9').find("#SideIndex").val()

        }

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/SaveOnlyPc',
            type: 'POST',
            data: {
                obj: obj
            },

            success: function (data) {
                if (data.isSuccess) {

                    $("#modalImportCard_9").modal("hide");
                    var vlue = $('#countpcid').val();
                    MultipleLanesController.showPc(vlue)
                    toastr.success(data.Message, "");
                } else {
                    toastr.error(data.Message, "");
                }
            }
        });



    });
});








//$("body").find("table #CurrentDirection ").each(function (index) {
//    $(this).on("click", function () {

//    });
//});


var MultipleLanesController = {
    showPc: function (vlue) {

        $.ajax({
            url: _prefixParkingDomain + '/MultipleLanesMap/ShowView',
            data: { vlue: vlue },
            type: 'json',
            //async:false,
            success: function (data) {
                $('#showpc').html('');
                $('#showpc').html(data);
            }
        });
    }
}