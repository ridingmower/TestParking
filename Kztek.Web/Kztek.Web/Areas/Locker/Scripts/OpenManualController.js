var OpenManualConfig = {
    TimeInteval: 500,
    TimeOut: 300000,
    LockerChangePageSize: false,
    LockerPageIndex: 1
};

$(function () {
    //Click chọn tất cả bộ đk
    $('body').on('click', '#chkCheckALLLocker', function () {
        var cmd = $(this);

        if (cmd.is(":checked")) {
            $('.chkCheckLocker').prop('checked', true);
            $('.chkCheckLocker').parents().addClass('info');

            var choices = [];

            $("#tblLocker tbody").find("tr").each(function () {
                var cmd = $(this);
                var chk = cmd.find(".chkCheckLocker");
                if (chk.is(":checked")) {
                    var id = chk.val();

                    choices.push(id);
                }
            });

            OpenManualController.LockerSelected(choices, "1");
        }
        else {
            $('.chkCheckLocker').prop('checked', false);
            $('.chkCheckLocker').parents().removeClass('info');

            var unchoices = [];

            $("#tblLocker tbody").find("tr").each(function () {
                var cmd = $(this);
                var chk = cmd.find(".chkCheckLocker");
                if (chk.is(":checked") === false) {
                    var id = chk.val();

                    unchoices.push(id);
                }
            });

            OpenManualController.LockerSelected(unchoices, "2");
        }
    });

    //
    $('body').on('click', '.chkCheckLocker', function () {
        var cmd = $(this);

        if (cmd.is(":checked")) {
            var choices = [];
            var cn = cmd.val();
            choices.push(cn);

            OpenManualController.LockerSelected(choices, "1");
        } else {
            var unchoices = [];
            var cn1 = cmd.val();
            unchoices.push(cn1);

            OpenManualController.LockerSelected(unchoices, "2");
        }
    });

    $('body').on('click', '#btnUnlockSelectedLocker', function () {
        bootbox.prompt('Lý do muốn mở tủ là gì ?', function (result) {
            if (result !== "") {
                OpenManualController.UnlockSelectedLocker(result);
            }
        });
    });

    $('body').on('click', '.btnUnlockLocker', function () {

    });
});

var OpenManualController = {
    GetCurrentTask: function () {
        return $("#TaskViewId").val();
    },

    LockerData: function (changePageSize) {
        //
        var key = $("#key").val();
        var controllerid = $("#controllerid").val();
        var taskid = OpenManualController.GetCurrentTask();

        var result = FunctionHelperController.LoadData({ taskid: taskid, key: key, controllerid: controllerid, page: OpenManualConfig.LockerPageIndex }, _prefixLockerDomain + '/Upload/PartialLocker');

        result.success(function (data) {
            $('#tblLocker tbody').html(data);

            //Tổng số trang
            var totalPage = $("#tblLocker > tbody #pageTotal").val();

            //Tổng số bản ghi
            var number = $("#tblLocker > tbody").find("#totalItem").val();

            //Tổng số đã chọn
            var selected = $("#tblLocker > tbody").find("#selectedItem").val();

            //
            $("#LockerPageIndex").text(OpenManualConfig.LockerPageIndex);

            $("#LockerPageCount").text(totalPage);

            $(".LockerCount").text(number);

            $(".LockerSelected").text(selected);

            //
            if (totalPage !== "0") {
                OpenManualController.LockerPaging("LockerPaging", totalPage, changePageSize);
            } else {
                $("#LockerPageIndex").text("0");
                OpenManualController.LockerPagingEmpty("LockerPaging");
            }
        });
    },

    LockerSelected: function (listId, isAdd) {
        var taskid = OpenManualController.GetCurrentTask();

        $.ajax({
            url: _prefixLockerDomain + '/Upload/LockerSelected',
            data: { listId: listId, isAdd: isAdd, taskid: taskid },
            type: 'json',
            async: true,
            success: function (data) {
                if (data.isSuccess) {
                    OpenManualController.LockerData(false);
                }
            }
        });
    },

    LockerPagingEmpty: function (name) {
        $('#' + name).empty();
        $('#' + name).removeData("twbs-pagination");
        $('#' + name).unbind("page");
    },

    LockerPaging: function (name, totalPage, changePageSize) {
        //Unbind pagination if it existed or click change pagesize
        if ($('#' + name + ' a').length === 0 || changePageSize === true) {
            $('#' + name).empty();
            $('#' + name).removeData("twbs-pagination");
            $('#' + name).unbind("page");
        }

        $('#' + name).twbsPagination({
            totalPages: totalPage,
            first: "<<",
            next: ">",
            last: ">>",
            prev: "<",
            visiblePages: 5,
            paginationClass: "pagination pagination-sm",
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                OpenManualConfig.LockerPageIndex = page;
                OpenManualController.LockerData(false);
                $("#chkCheckALLLocker").prop("checked", false);
            }
        });
    },

    UnlockSelectedLocker: function (message) {
        var formData = new FormData();

        var result = FunctionHelperController.Update(formData, _prefixLockerDomain + '/Upload/UnlockSelectedLocker');
        result.success(function (data) {
            var dataEmployee = data.dataSend;
            var dataAddress = data.dataAddress;

            var total = data.dataSend.length;
            var count = data.dataSend.length;

            var time = setInterval(function () {
                //debugger;
                count = OpenManualController.ModelSendFormat(dataEmployee[count], dataAddress, message, count);

                if (count === total || count === 0 || count === undefined) {
                    clearInterval(time);
                }

            }, UploadLockerConfig.TimeInteval);
        });
    },

    UnlockLocker: function (itemJson, message) {
        var formData = new FormData();
        formData.append('ModelJson', itemJson);

        var result = FunctionHelperController.Update(formData, _prefixLockerDomain + '/Upload/UnlockLocker');
        result.success(function (data) {

            var dataEmployee = data.dataSend;
            var dataAddress = data.dataAddress;

            var total = data.dataSend.length;
            var count = data.dataSend.length;

            var time = setInterval(function () {
                //debugger;
                count = OpenManualController.ModelSendFormat(dataEmployee[count], dataAddress, message, count);

                if (count === total || count === 0 || count === undefined) {
                    clearInterval(time);
                }

            }, UploadLockerConfig.TimeInteval);

        });
    },

    ModelSendFormat: function (itemEmployee, arrAddress, message, count) {
        cout = count + 1;

        $.each(arrAddress, function (i, item) {
            OpenManualController.FormatSendUplock(itemEmployee, item, message);
        });

        return count;
    },

    FormatSendUplock: function (obj, uri, message) {
        $.ajax({
            type: "POST",
            url: 'http://' + uri + ':8081/api/register/unlock',
            data: obj,
            success: function (data) {
                
            },
            error: function (xhr, status) {
                if (status === "timeout") {
                    console.log("Time out: " + obj.CardNumber + " - " + obj.UserIDofFinger + " - " + obj.ControllerIDs);
                }

                console.log("UPLOAD error: " + xhr);
            }
            //,
            //timeout: UploadConfig.TimeOut
        });
    },

    SaveProcess: function (obj, message, result) {

    }
};