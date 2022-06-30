$(function () {
    DemoFilter.PartialDemo(1);

    $('body').on('click', '#pagDemo li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');

        DemoFilter.PartialDemo(_page);

        return false;
    })

    var timer;

    $('body').on('keyup change', ".filter", function () {
        var cmd = $(this);

        var id = cmd.attr("idata");

        var data = cmd.val();

        if (id.indexOf("Lane") >= 0 || id.indexOf("CardGroup") >= 0 || id.indexOf("Gate") >= 0 || id.indexOf("User") >= 0) {
            DemoFilter.GetIdByKey(data, id);

        } else {
            $("#" + id).val(data.trim());
        }

        if (timer) {
            clearTimeout(timer);
        }

        timer = setTimeout(function () {

            DemoFilter.PartialDemo(1);

        }, 500);
    });
})

var DemoFilter = {
    PartialDemo: function (page) {
        var key = "CardNo$" + $("#CardNo").val() + "|" +
            "CardNumber$" + $("#CardNumber").val() + "|" +
            "Plate$" + $("#Plate").val() + "|" +
            "CardGroup$" + $("#CardGroup").val() + "|" +
            "LaneIn$" + $("#LaneIn").val() + "|" +
            "DateTimeIn$" + $("#DateTimeIn").val() + "|" +
            "DateTimeOut$" + $("#DateTimeOut").val() + "|" +
            "DCTypeCode$" + $("#DCTypeCode").val() ;

        var obj = {
            page: page,
            key: key
        };
        $.ajax({
            url: _prefixParkingDomain + '/Report/Partial_DemoFilter',
            type: 'POST',
            data: obj,
            success: function (data) {
                $('#tblEvent tbody').html('');
                $('#tblEvent tbody').html(data);

                $("#spCount").text($("#totalCount").val());

                DemoFilter.PartialPaging();
            }
        });
       
    },
    PartialPaging: function () {
        
        var obj = {
            Id: "pagDemo",
            Controller: "Report",
            Action: "Partial_DemoFilter",
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