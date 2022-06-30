$(function () {
    ReportInOutByFilterController.load_ViewImage();

    $('body').on('click', '#pagging li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');


        var DCTypeName = $('#DCTypeName').val();
        $("#DCTypeNames").val(DCTypeName);

        var laneIns = $('#LaneIn').val();
        $("#LaneIns").val(laneIns);

        var laneOuts = $('#LaneOut').val();
        $("#LaneOuts").val(laneOuts);

        var cardGrs = $('#CardGroup').val();
        $("#CardGr").val(cardGrs);

       
        ReportInOutByFilterController.PartialInOutFilter(_page, DCTypeName, laneIns, laneOuts, cardGrs);

        return false;
    })

    var timer;

    $('body').on('keyup change', ".filter", function () {
        var cmd = $(this);

        var id = cmd.attr("idata");

        var data = cmd.val();

        if (id.indexOf("Lane") >= 0 || id.indexOf("CardGroup") >= 0 || id.indexOf("Gate") >= 0 || id.indexOf("User") >= 0) {
            ReportInOutByFilterController.GetIdByKey(data, id);

        } else {
            $("#" + id).val(data.trim());
        }

        if (timer) {
            clearTimeout(timer);
        }

        timer = setTimeout(function () {

            ReportInOutByFilterController.PartialInOutFilter(1,"");

        }, 5000);
    });
})

var ReportInOutByFilterController = {

    load_ViewImage: function () {
        var $overflow = '';
        var colorbox_params = {
            rel: 'colorbox',
            reposition: true,
            scalePhotos: true,
            scrolling: false,
            previous: '<i class="ace-icon fa fa-arrow-left"></i>',
            next: '<i class="ace-icon fa fa-arrow-right"></i>',
            close: '&times;',
            current: '{current} of {total}',
            maxWidth: '100%',
            maxHeight: '100%',
            onOpen: function () {
                $overflow = document.body.style.overflow;
                document.body.style.overflow = 'hidden';
            },
            onClosed: function () {
                document.body.style.overflow = $overflow;
            },
            onComplete: function () {
                $.colorbox.resize();
            }
        };

        $('.ace-thumbnails [data-rel="colorbox"]').colorbox(colorbox_params);
        $("#cboxLoadingGraphic").html("<i class='ace-icon fa fa-spinner orange fa-spin'></i>");//let's add a custom loading icon


        $(document).one('ajaxloadstart.page', function (e) {
            $('#colorbox, #cboxOverlay').remove();
        });

    },
    PartialInOutFilter: function (page,Dctype , LanIns, LaneOuts, Cardgrs) {
        var key = "CardNo$" + $("#CardNo").val() + "|" +
            "CardNumber$" + $("#CardNumber").val() + "|" +
            "Plate$" + $("#Plate").val() + "|" +         
            "Customer$" + $("#Customer").val() + "|" +
            "DateTimeIn$" + $("#DateTimeIn").val() + "|" +
            "DateTimeOut$" + $("#DateTimeOut").val() + "|" +
            "UserIn$" + $("#UserIn").val() + "|" +
            "Moneys$" + $("#Moneys").val() + "|" +
            "UserOut$" + $("#UserOut").val();

        if (Dctype == '' || Dctype == undefined) {
            var st1 = $('#DCTypeName').val();
            Dctype = st1;
        } else {
            Dctype = Dctype;
        }

        if (LanIns == '' || LanIns == undefined) {
            var Ins = $('#LaneIn').val();
            LanIns = Ins;
        } else {
            LanIns = LanIns;
        }

        if (LaneOuts == '' || LaneOuts == undefined) {
            var Outs = $('#LaneOut').val();
            LaneOuts = Outs;
        } else {
            LaneOuts = LaneOuts;
        }

        if (Cardgrs == '' || Cardgrs == undefined) {
            var cardGr = $('#CardGroup').val();
            Cardgrs = cardGr;
        } else {
            Cardgrs = Cardgrs;
        }
        var obj = {
            page: page,
            key: key,
            discount: Dctype,
            LanIns: LanIns,
            LaneOuts: LaneOuts,
            Cardgrs: Cardgrs
        };
        $.ajax({
            url: _prefixParkingDomain + '/Report/Partial_InOutFilter',
            type: 'POST',
            data: obj,
            success: function (data) {
                $('#tblEvent tbody').html('');
                $('#tblEvent tbody').html(data);
               
                ReportInOutByFilterController.load_ViewImage();
                $('[data-spzoom]').spzoom({
                    width: 350,
                    height: 350,
                    position: 'right',
                    margin: 25,
                    showTitle: true,
                    titlePosition: 'bottom'
                });
                //window.addEventListener('DOMContentLoaded', function () {
                //    var image = document.querySelector('#image');

                //    if (image !== null) {
                //        var data = document.querySelector('#data');
                //        var cropBoxData = document.querySelector('#cropBoxData');
                //        var button = document.getElementById('button');
                //        var result = document.getElementById('result');
                //        var cropper = new Cropper(image, {
                //            ready: function (event) {
                //                // Zoom the image to its natural size
                //                cropper.zoomTo(1);
                //            },

                //            crop: function (event) {
                //                data.textContent = JSON.stringify(cropper.getData());
                //                cropBoxData.textContent = JSON.stringify(cropper.getCropBoxData());
                //            },

                //            zoom: function (event) {
                //                // Keep the image in its natural size
                //                if (event.detail.oldRatio === 1) {
                //                    event.preventDefault();
                //                }
                //            },
                //        });

                //        button.onclick = function () {
                //            result.innerHTML = '';
                //            result.appendChild(cropper.getCroppedCanvas());
                //        };
                //    }

                //});
                $("#spCount").text($("#totalCount").val());

                ReportInOutByFilterController.PartialPaging();
            }
        });

    },
    PartialPaging: function () {

        var obj = {
            Id: "pagging",
            Controller: "Report",
            Action: "Partial_InOutFilter",
            TotalPage: $("#totalPage").val(),
            TotalItem: $("#totalCount").val(),
            PageSize: $("#pageSize").val(),
            PageIndex: $("#pageIndex").val()
        };

        $.ajax({
            url: _prefixParkingDomain + '/Report/Partial_PagingAjax',
            type: 'POST',
            data: obj,
            success: function (data) {
                $('#boxPaging').html('');
                $('#boxPaging').html(data);
            }
        });

    },
    GetIdByKey: function (key, id) {
        var obj = {
            key: key,
            action: id
        };
        $.ajax({
            url: _prefixParkingDomain + '/Report/GetIdByKey',
            type: 'POST',
            data: obj,
            success: function (data) {
                $("#" + id).val(data);
            }
        });
    }
}