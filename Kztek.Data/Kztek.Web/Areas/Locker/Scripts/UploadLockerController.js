var UploadLockerConfig = {
    TimeInteval: 500,
    TimeOut: 300000,
    CardChangePageSize: false,
    CardPageIndex: 1
};

$(function () {
    //Click chọn tất cả bộ đk
    $('body').on('click', '#chkCheckALLController', function () {
        var cmd = $(this);

        if (cmd.is(":checked")) {
            $('.chkCheckController').prop('checked', true);
            $('.chkCheckController').parents().addClass('info');

            var choices = [];

            $("#boxController tbody").find("tr").each(function () {
                var cmd = $(this);
                var chk = cmd.find(".chkCheckController");
                if (chk.is(":checked")) {
                    var id = chk.val();

                    choices.push(id);
                }
            });

            UploadLockerController.LockerControllerSelected(choices, true);
        }
        else {
            $('.chkCheckController').prop('checked', false);
            $('.chkCheckController').parents().removeClass('info');

            var unchoices = [];

            $("#boxController tbody").find("tr").each(function () {
                var cmd = $(this);
                var chk = cmd.find(".chkCheckController");
                if (chk.is(":checked") === false) {
                    var id = chk.val();

                    unchoices.push(id);
                }
            });

            UploadLockerController.LockerControllerSelected(unchoices, false);
        }
    });

    //
    $('body').on('click', '.chkCheckController', function () {
        var cmd = $(this);

        if (cmd.is(":checked")) {
            var choices = [];
            var cn = cmd.val();
            choices.push(cn);

            UploadLockerController.LockerControllerSelected(choices, true);
        } else {
            var unchoices = [];
            var cn1 = cmd.val();
            unchoices.push(cn1);

            UploadLockerController.LockerControllerSelected(unchoices, false);
        }
    });

    //Load dữ liệu danh sách thẻ.
    $('body').on('click', '#LoadDataListCard', function () {
        UploadLockerController.CardData();
    });

    //Click chọn tất cả
    $('body').on('click', '#chkCheckALLCard', function () {
        var cmd = $(this);

        if (cmd.is(":checked")) {
            $('.chkCheckCard').prop('checked', true);
            $('.chkCheckCard').parents().addClass('info');

            var choices = [];

            $("#tblCards tbody").find("tr").each(function () {
                var cmd = $(this);
                var chk = cmd.find(".chkCheckCard");
                if (chk.is(":checked")) {
                    var id = chk.val();

                    choices.push(id);
                }
            });

            UploadLockerController.CardSelected(choices, "1");
        }
        else {
            $('.chkCheckCard').prop('checked', false);
            $('.chkCheckCard').parents().removeClass('info');

            var unchoices = [];

            $("#tblCards tbody").find("tr").each(function () {
                var cmd = $(this);
                var chk = cmd.find(".chkCheckCard");
                if (chk.is(":checked") === false) {
                    var id = chk.val();

                    unchoices.push(id);
                }
            });

            UploadLockerController.CardSelected(unchoices, "2");
        }
    });

    $('body').on('click', '.chkCheckCard', function () {
        var cmd = $(this);

        if (cmd.is(":checked")) {
            var choices = [];
            var cn = cmd.val();
            choices.push(cn);

            UploadLockerController.CardSelected(choices, "1");
        } else {
            var unchoices = [];
            var cn1 = cmd.val();
            unchoices.push(cn1);

            UploadLockerController.CardSelected(unchoices, "2");
        }
    });

    $('body').on('click', '#btnConfirmLocker', function () {
        //debugger;
        var taskid = $("#TaskViewId").val();
        var action = $("#LockerAction").val() === 'nạp' ? 'UPLOAD' : 'DELETE';
        UploadLockerController.LockerSelfHostDataApi(taskid, action);
    });

    //Click
    $('body').on('click', '.btnEditLocker', function () {
        var cmd = $(this);
        var objCard = cmd.attr('idata');

        UploadLockerController.LockerRegister(objCard);
    });

    //Add new locker to card
    $('body').on('click', '#boxModalLockerDetail #btnAddNewLocker', function () {
        var cmd = $(this);
        var cardnumber = cmd.attr('idata');

        UploadLockerController.LockerCardAddNew(cardnumber);
    });

    //Save LOcker + card
    $('body').on('click', '#boxModalLockerDetail .btnSaveLockerCard', function () {
        var cmd = $(this);
        var lockerid = cmd.parent().parent().find("#LockerSelect").val();
        var cardnumber = cmd.attr('idata');

        UploadLockerController.LockerCardRegisterConfirm(cardnumber, lockerid);

        //bootbox.confirm('Bạn chắc chắn muốn gắn thẻ với tủ này?', function (result) {
        //    if (result) {
               
        //    }
        //});
    });

    //Xóa thẻ với thêm nhưng chưa gắn lưu
    $('body').on('click', '#boxModalLockerDetail .btnRemoveLockerCardNew', function () {
        var cmd = $(this);

        cmd.parent().parent().remove();
    });

    //Xóa thẻ đã đăng ký cần cảnh báo: Vui lòng lấy đồ ra trước khi xóa
    $('body').on('click', '#boxModalLockerDetail .btnRemoveLockerCard', function () {
        var cmd = $(this);
        var lockerid = cmd.attr('idata');
        var cardnumber = cmd.attr('idata1');

        UploadLockerController.RemoveLockerCardRegisterConfirm(cardnumber, lockerid);

    });

    //Đóng form detail locker
    $('body').on('click', '#btnCloseLockerDetail', function () {
        var cmd = $(this);
        var cardnumber = cmd.attr('idata');
        $("#ModalLockerCard_" + cardnumber).modal('hide');
        UploadLockerController.CardData(false);

        var taskid = UploadLockerController.GetCurrentTask();
        UploadLockerController.LockerData(taskid);
    });

    //Relaod lại controller
    $('body').on('click', '#btnReloadDataController', function () {
        var taskid = UploadLockerController.GetCurrentTask();

        UploadLockerController.LockerData(taskid);
    });

    //Xóa danh sách thẻ đã chọn
    $('body').on('click', '#btnRemoveAllCardLockerSelected', function () {
        UploadLockerController.CardSelected([], "3");
    });
});

var UploadLockerController = {
    init: function () {
        $.ajax({
            url: _prefixLockerDomain + '/Upload/GetTimeSetting',
            type: 'GET',
            data: {},
            success: function (response) {
                UploadLockerConfig.TimeInteval = response.u1;
                UploadLockerConfig.TimeOut = response.u2;
            }
        });
    },

    GetCurrentTask: function () {
        return $("#TaskViewId").val();
    },

    LockerControllerData: function () {
        var taskid = UploadLockerController.GetCurrentTask();

        var lineid = $("#LineID").val();

        var result = FunctionHelperController.LoadData({ taskid: taskid, lineid: lineid }, _prefixLockerDomain + '/Upload/PartialListController');

        result.success(function (data) {
            $('#boxController').html(data);

            UploadLockerController.LockerSelfHostData(taskid);
            UploadLockerController.LockerData(taskid);
        });
    },
    LockerControllerSelected: function (listId, isAdd) {
        var taskid = UploadLockerController.GetCurrentTask();

        $.ajax({
            url: _prefixLockerDomain + '/Upload/LockerControllerSelected',
            data: { listId: listId, isAdd: isAdd, taskid: taskid },
            type: 'json',
            async: true,
            success: function (data) {
                if (data.isSuccess) {
                    UploadLockerController.LockerControllerData();
                }
            }
        });
    },

    LockerSelfHostData: function (taskid) {
        var result1 = FunctionHelperController.LoadData({ taskid: taskid }, _prefixLockerDomain + '/Upload/PartialListSelfHost');

        result1.success(function (data) {
            $('#boxSelfHost').html(data);
        });

        var result2 = FunctionHelperController.LoadData({ taskid: taskid }, _prefixLockerDomain + '/Upload/ListSelfHostProcess');

        result2.success(function (data) {
            $('#BoxHostLockerProgress').html(data);
        });
    },

    LockerSelfHostDataApi: function (taskid, action) {

        var formData = new FormData();
        formData.append('taskid', taskid);

        var result = FunctionHelperController.Update(formData, _prefixLockerDomain + '/Upload/DataSelfHost');
        result.success(function (data) {
            UploadLockerController.DataSend(data, action);
        });
    },

    LockerData: function (taskid) {
        var result = FunctionHelperController.LoadData({ taskid: taskid }, _prefixLockerDomain + '/Upload/PartialListLocker');

        result.success(function (data) {
            $('#boxLockerCount').html(data);
        });
    },
    LockerRegister: function (objCard) {
        //debugger;
        var taskid = UploadLockerController.GetCurrentTask();

        var dt = JSON.parse(objCard);

        var result = FunctionHelperController.LoadData({ dt: objCard, taskid: taskid }, _prefixLockerDomain + '/Upload/PartialLockerRegister');
        result.success(function (data) {
            $("#boxModalLockerDetail").html(data);

            $("#ModalLockerCard_" + dt.CardNumber).modal('show');
        });
    },

    CardData: function (changePageSize) {
        //
        var key = $("#txtCardAnotherKey").val();
        var cardgroups = $("#cardgroups").val();
        var customergroups = $("#ddlcustomergroup").val();
        var taskid = UploadLockerController.GetCurrentTask();

        $.ajax({
            url: _prefixLockerDomain + '/Upload/PartialListCard',
            type: 'GET',
            data: {
                key: key,
                cardgroupids: cardgroups,
                customergroupid: customergroups,
                taskid: taskid,
                page: UploadLockerConfig.CardPageIndex
            },
            success: function (response) {
                $('#tblCards > tbody').html('');

                $('#tblCards > tbody').html(response);

                //Tổng số trang
                var totalPage = $("#tblCards > tbody #pageTotal").val();

                //Tổng số bản ghi
                var number = $("#tblCards > tbody").find("#totalItem").val();

                //Tổng số đã chọn
                var selected = $("#tblCards > tbody").find("#selectedItem").val();

                //
                $("#CardPageIndex").text(UploadLockerConfig.CardPageIndex);

                $("#CardPageCount").text(totalPage);

                $(".CardCount").text(number);

                $(".CardSelected").text(selected);

                $("#hidTotalLockerConfirm").val(number);

                //
                if (totalPage !== "0") {
                    UploadLockerController.CardPaging("CardPagination", totalPage, changePageSize);
                } else {
                    $("#CardPageIndex").text("0");
                    UploadLockerController.CardPagingEmpty("CardPagination");
                }
            }
        });
    },
    CardSelected: function (listId, isAdd) {
        var taskid = UploadLockerController.GetCurrentTask();

        $.ajax({
            url: _prefixLockerDomain + '/Upload/CardSelected',
            data: { listId: listId, isAdd: isAdd, taskid: taskid },
            type: 'json',
            async: true,
            success: function (data) {
                if (data.isSuccess) {
                    UploadLockerController.CardData(false);
                }
            }
        });
    },
    CardPagingEmpty: function (name) {
        $('#' + name).empty();
        $('#' + name).removeData("twbs-pagination");
        $('#' + name).unbind("page");
    },
    CardPaging: function (name, totalPage, changePageSize) {
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
                UploadLockerConfig.CardPageIndex = page;
                UploadLockerController.CardData(false);
                $("#chkCheckALLCard").prop("checked", false);
            }
        });
    },

    LoadModalConfirm: function (isAll, action, name) {
        //
        var key = $("#txtCardAnotherKey").val();
        var cardgroups = $("#cardgroups").val();
        var customergroups = $("#ddlcustomergroup").val();
        var taskid = UploadLockerController.GetCurrentTask();

        var model = {
            isAll: isAll,
            actionTake: action,
            name: name,

            totalItem: $("#hidTotalLockerConfirm").val(),

            key: key,
            cardgroupids: cardgroups,
            customergroupid: customergroups,
            taskid: taskid
        };

        var result = FunctionHelperController.LoadData(model, _prefixLockerDomain + '/Upload/ModalConfirm');
        result.success(function (data) {
            $("#boxModalConfirm").html(data);
            $("#ModalConfirm").modal('show');
        });
    },

    DataSend: function (dataAddress, action) {
        var dt = $("#ModalConfirm #LockerDataJSON").val();
        var dataLocker = JSON.parse(dt);

        total = dataLocker.length;

        var count1 = 0;

        var time1 = setInterval(function () {
            //debugger;
            count1 = UploadLockerController.FormatDataSend(dataLocker[count1], dataAddress, count1, total, action);

            console.log(count1);

            if (count1 === total || count1 === 0 || count1 === undefined) {
                clearInterval(time1);
            }

        }, UploadLockerConfig.TimeInteval);
    },

    //obj - tblLocker
    FormatDataSend: function (obj, address, count, total, action) {
        count = count + 1;

        var result = FunctionHelperController.LoadDataJson({ model: obj }, _prefixLockerDomain + '/Upload/DataSendToApp');
        result.success(function (data) {
            if (action === "UPLOAD") {
                UploadLockerController.UploadEvent(data, address, 'LOCKER', count, total, obj);
            } else {
                UploadLockerController.DeleteEvent(data, address, 'LOCKER', count, total, obj);
            }
        });

        return count;
    },

    UploadEvent: function (obj, objhost, type, count, total, modelLocker) {

        if (objhost.length > 0) {
            $.each(objhost, function (i, item) {
                UploadLockerController.FormatSendUpload(obj, item.Address, type, count, total, modelLocker);
            });
        }

    },
    DeleteEvent: function (obj, objhost, type, count, total, modelLocker) {
        

        if (objhost.length > 0) {
            $.each(objhost, function (i, item) {
                UploadLockerController.FormatSendDelete(obj, item.Address, type, count, total, modelLocker);
            });
        }

        
    },

    FormatSendUpload: function (obj, uri, typeA, count, total, modelLocker) {
        $.ajax({
            type: "POST",
            url: 'http://' + uri + ':8081/api/register/upload',
            data: obj,
            success: function (data) {
                if (data.length > 0) {
                    $.each(data, function (i, item) {

                        UploadLockerController.writeLog(item);

                        UploadLockerController.SaveEvent(modelLocker, "UPLOAD", item.Message);
                    });

                    UploadLockerController.showProgress("BoxHostLockerProgress", count, total, uri);
                    
                } else {
                    //alert("ERROR");
                }
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
    FormatSendDelete: function (obj, uri, typeA, count, total, modelLocker) {
        var url = "";
        var url1 = 'http://' + uri + ':8081/api/register/delete';
        var url2 = 'http://' + uri + ':8081/api/register/deletebycontroller';

        var isDeleteLockerByController = $("#chkDeleteLockersByController").is(":checked"); // True - Xóa theo url2

        if (isDeleteLockerByController) {
            url = url2;
        } else {
            url = url1;
        }

        $.ajax({
            type: "POST",
            url: url,
            data: obj,
            success: function (data) {
                if (data.length > 0) {
                    $.each(data, function (i, item) {

                        UploadLockerController.writeLog(item);

                        UploadLockerController.SaveEvent(modelLocker, "DELETE", item.Message);
                    });

                    UploadLockerController.showProgress("BoxHostLockerProgress", count, total, uri);
                } else {
                    //alert("ERROR");
                }
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

    writeLog: function (item) {

        if (item.Success) {
            var size1 = $("#listsuccess").find('p').length;

            $("#listsuccess").prepend('<p> ' + (size1 + 1) + ". " + GetDateDDMMYYYY() + " - " + item.Message + '</p>');
        } else {
            var size2 = $("#listerror").find('p').length;

            $("#listerror").prepend('<p> ' + (size2 + 1) + ". " + GetDateDDMMYYYY() + " - " + item.Message + '</p>');
        }
    },

    SaveEvent: function (objLocker, action, message) {

        $.ajax({
            type: "POST",
            url: _prefixLockerDomain + '/Upload/SaveEvent',
            data: { model: objLocker, actionV: action, message: message },
            success: function (data) {

            },
            error: function (er) {
                console.log("Process upload error: " + er);
            }
        });
    },

    showProgress: function (name, count, total, address) {
        address = address.split('.').join('');

        $.ajax({
            url: _prefixLockerDomain + '/Upload/ProgressBar',
            type: 'GET',
            data: {
                address: address,
                curr: count,
                total: total
            },
            success: function (response) {
                $("#" + name).find("#HOST_" + address).find("#ProgressBar").html(response);

                if (count === total) {
                    bootbox.alert('Hoàn thành');
                    $("#ModalConfirm").modal('hide');
                }

                $('.easy-pie-chart.percentage').each(function () {
                    $(this).easyPieChart({
                        barColor: $(this).data('color'),
                        trackColor: '#EEEEEE',
                        scaleColor: false,
                        lineCap: 'butt',
                        lineWidth: 8,
                        animate: false,
                        size: 75
                    }).css('color', $(this).data('color'));
                });
            }
        });
    },

    LockerCardAddNew: function (cardnumber) {
        var taskid = UploadLockerController.GetCurrentTask();

        var result = FunctionHelperController.LoadData({ cardnumber: cardnumber, taskid: taskid }, _prefixLockerDomain + '/Upload/PartialLockerModel');

        result.success(function (data) {
            $("#ModalLockerCard_" + cardnumber).find(".divOverBoxLocker").animate({ scrollTop: 0 }, "fast");

            $("#ModalLockerCard_" + cardnumber).find("#tblLockerCard tbody").prepend(data);

            $('.chosen-select').chosen({ allow_single_deselect: true });
        });
    },

    LockerCardRegisterConfirm: function (cardnumber, lockerid) {
        var formData = new FormData();
        formData.append('lockerid', lockerid);
        formData.append('cardnumber', cardnumber);

        var result = FunctionHelperController.Create(formData, _prefixLockerDomain + '/Upload/RegisterLockerWithCardNumber');
        result.success(function (data) {
            if (data.isSuccess) {
                toastr.success(data.Message);
                UploadLockerController.LockerCardData(cardnumber);
                $("#ModalConfirm").modal('hide');
            } else {
                toastr.error(data.Message);
            }
        });
    },

    RemoveLockerCardRegisterConfirm: function (cardnumber, lockerid) {


        var formData = new FormData();
        formData.append('lockerid', lockerid);

        var result = FunctionHelperController.Create(formData, _prefixLockerDomain + '/Upload/RemoveRegisterLockerWithCardNumber');
        result.success(function (data) {
            if (data.isSuccess) {
                toastr.success(data.Message);
                UploadLockerController.LockerCardData(cardnumber);
            } else {
                toastr.error(data.Message);
            }
        });
    },

    LockerCardData: function (cardnumber) {
        var taskid = UploadLockerController.GetCurrentTask();

        var formData = new FormData();
        formData.append('cardnumber', cardnumber);

        var result = FunctionHelperController.Update(formData, _prefixLockerDomain + '/Upload/LoadDataLockerCard');
        result.success(function (data) {

            $("#ModalLockerCard_" + cardnumber).find("#tblLockerCard tbody").html('');

            var count1 = 0;

            var time1 = setInterval(function () {
                count1 = UploadLockerController.LoadDataLockerCard(data[count1], count1, cardnumber, taskid);

                if (count1 === data.length) {
                    clearInterval(time1);
                }

            }, 100);

            //$.each(data, function (i, item) {

                
              
            //});

            
        });
    },

    LoadDataLockerCard: function (item, count, cardnumber, taskid) {
        count = count + 1;

        var result1 = FunctionHelperController.LoadData({ dataObj: JSON.stringify(item), cardnumber: cardnumber, taskid: taskid }, _prefixLockerDomain + '/Upload/PartialLockerModel');

        result1.success(function (data) {

            $("#ModalLockerCard_" + cardnumber).find("#tblLockerCard tbody").append(data);

            $('.chosen-select').chosen({ allow_single_deselect: true });
        });

        return count;
    }
};

function UploadLockers(isAll) {

    UploadLockerController.LoadModalConfirm(isAll, 'nạp', 'tủ');

    //UploadController.GetListCardWantToUse(isAll, "UPLOAD");
}

function DeleteLockers(isAll) {
    UploadLockerController.LoadModalConfirm(isAll, 'hủy', 'tủ');

    //UploadController.GetListCardWantToUse(isAll, "DELETE");
}
