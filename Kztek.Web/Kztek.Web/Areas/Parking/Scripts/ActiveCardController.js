var config = {
    changePageSize: false,
    pageIndex: 1
}

$(function () {

    $('body').on("change", "#datenewextendfrompicker", function () {
        ActiveCardController.loadBoxMonth();
    })

    $('body').on("change", "#datestart", function () {
        ActiveCardController.loadBoxMonth();
    })
    $('body').on("change", "#txtDa_1", function () {
        alert(1);
    })

    //$('body').on("change", ".chnsd", function () {
    //    alert(2);
    //    //var id = $(this).attr("idata");
    //    //var type = $(this).attr("idata1");
    //    //var money = $("#" + id).find("input[name=txtMoney]").val();
    //    //var date = $("#" + id).find("input[name=txtDateM]").val();
    //    //ActiveCardController.ChangeDateMoney(id, money, date, type);
    //})
})

var ActiveCardController = {
    init: function () {

    },
    loadData: function (changePageSize) {
        var key = $("input[name=key]").val();
        var anotherkey = $("input[name=txtAnotherKey]").val();
        var cardgroup = $("#cardgroups").val();
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var customergroup = $("#customergroup").val();

        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/boxCards',
            type: 'GET',
            data: {
                key: key,
                anotherkey: anotherkey,
                cardgroups: cardgroup,
                customergroup: customergroup,
                fromdate: fromdate,
                todate: todate,
                page: config.pageIndex
            },
            success: function (response) {
                $('#boxCards table > tbody').html('');

                $('#boxCards table > tbody').html(response);

                var totalPage = $("#boxCards table > tbody #pageTotal").val();

                var number = $("#boxCards table > tbody").find("#totalItem").val();

                $("#boxCards #pageIndex").text(config.pageIndex);

                $("#boxCards #pageCount").text(totalPage);

                $("#boxCards .numberCard").text(number);

                if (totalPage !== "0") {
                    ActiveCardController.paging(totalPage, changePageSize);
                    //toastr.success("Tìm kiếm thành công");
                } else {
                    $("#boxCards #pageIndex").text("0");
                    ActiveCardController.pagingEmpty();
                }
            }
        });
    },
    search: function () {

    },
    pagingEmpty: function () {
        $('#pagination').empty();
        $('#pagination').removeData("twbs-pagination");
        $('#pagination').unbind("page");
    },
    paging: function (totalPage, changePageSize) {
        //debugger;
        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "<<",
            next: ">",
            last: ">>",
            prev: "<",
            visiblePages: 5,
            paginationClass: "pagination pagination-sm",
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                config.pageIndex = page;
                ActiveCardController.loadData(false);
                $("#chkCheckALL").prop("checked", false);             
            }
        });
    },
    addListExtendCard: function (listId) {
        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/AddNewListCardSub',
            data: { listId: listId },
            type: 'json',
            async: true,
            success: function (data) {
                ActiveCardController.LoadListExtendCard();
            }
        });
    },
    LoadListExtendCard: function () {
        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/boxCardChoices',
            type: 'GET',
            success: function (response) {
                $("#boxExtendCards table > tbody").html('');
                $("#boxExtendCards table > tbody").html(response);

                var number = 0;
                $("#boxExtendCards table > tbody").find("tr").each(function () {
                    number += 1;
                })

                $("#boxExtendCards").find(".numberExtendCard").text(number);
            }
        })
    },
    removeAllExtend: function () {
        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/DeleteAllSelectedCard',
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    ActiveCardController.LoadListExtendCard();
                }
            }
        });
    },
    removeExtend: function (id) {
        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/DeleteOneSelectedCard',
            data: { id: id },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    ActiveCardController.LoadListExtendCard();
                }
            }
        });
    },
    ExtendAllCard: function () {
        //Query
        var key = $("input[name=key]").val();
        var anotherkey = $("input[name=txtAnotherKey]").val();
        var cardgroup = $("#cardgroups").val();
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var customergroup = $("#customergroup").val();
        var allownegative = $("#isAllowNegativeDays").prop("checked");

        //Cấu hình mới
        var newDate = $("#datenewextendfrompicker").val();
        var feeLevel = $("#newfeelevel").val();
        var newDateActive = $("#datenewactivefrompicker").val();

        var obj = {
            KeyWord: key,
            AnotherKey: anotherkey,
            CardGroup: cardgroup,
            CustomerGroup: customergroup,
            DateExtend: newDate,
            FeeLevel: feeLevel,
            isAllowNegativeDays: allownegative,
            DateActive: newDateActive,
            fromdate: fromdate,
            todate: todate
        }

        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/ExtendAllCard',
            data: { obj: obj },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                  //  toastr.success(data.Message);
                    ActiveCardController.loadData();

                    bootbox.alert({
                        message: "<span style='font-size:24px; color:green'> Gia hạn thành công!</span>"
                    });
                } else {
                    //toastr.error(data.Message);

                    bootbox.alert({
                        message: "<span style='font-size:24px; color:red'> Có lỗi xảy ra!</span>"
                    });
                }
            }
        });
    },
    ExtendCards: function () {
        //Cấu hình mới
        var newDate = $("#datenewextendfrompicker").val();
        var feeLevel = $("#newfeelevel").val();
        var allownegative = $("#isAllowNegativeDays").prop("checked");
        var newDateActive = $("#datenewactivefrompicker").val();

        var obj = {
            DateExtend: newDate,
            FeeLevel: feeLevel,
            isAllowNegativeDays: allownegative,
            DateActive: newDateActive
        }

        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/ExtendSelectedCard',
            data: { obj: obj },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    //toastr.success(data.Message);
                    ActiveCardController.loadData(true);
                  
                    //PK_ActiveCardController.LoadListExtendCard();

                    $("#boxExtendCards table > tbody").html('');

                    bootbox.alert({
                        message: "<span style='font-size:24px; color:green'> Gia hạn thành công!</span>"
                    });
                } else {
                    //toastr.error(data.Message);
                    bootbox.alert({
                        message: "<span style='font-size:24px; color:red'> Có lỗi xảy ra!</span>"
                    });
                }
            }
        });
    },
    ExtendAllOneMonth: function () {
        //Query
        var key = $("input[name=key]").val();
        var anotherkey = $("input[name=txtAnotherKey]").val();
        var cardgroup = $("#cardgroups").val();
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var customergroup = $("#customergroup").val();
        var allownegative = $("#isAllowNegativeDays").prop("checked");

        //Cấu hình mới
        var newDate = $("#datenewextendfrompicker").val();
        var feeLevel = $("#newfeelevel").val();
        var newDateActive = $("#datenewactivefrompicker").val();

        var obj = {
            KeyWord: key,
            AnotherKey: anotherkey,
            CardGroup: cardgroup,
            CustomerGroup: customergroup,
            DateExtend: newDate,
            FeeLevel: feeLevel,
            isAllowNegativeDays: allownegative,
            DateActive: newDateActive,
            fromdate: fromdate,
            todate: todate
        }

        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/ExtendAllOneMonth',
            data: { obj: obj },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    toastr.success(data.Message);
                    ActiveCardController.loadData();
                } else {
                    toastr.error(data.Message);
                }
            }
        });
    },
    ExtendCardsOneMonth: function () {
        //Cấu hình mới
        var newDate = $("#datenewextendfrompicker").val();
        var feeLevel = $("#newfeelevel").val();
        var allownegative = $("#isAllowNegativeDays").prop("checked");
        var newDateActive = $("#datenewactivefrompicker").val();

        var obj = {
            DateExtend: newDate,
            FeeLevel: feeLevel,
            isAllowNegativeDays: allownegative,
            DateActive: newDateActive
        }

        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/ExtendSelectedOneMonth',
            data: { obj: obj },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    toastr.success(data.Message);
                    ActiveCardController.loadData(true);
                    //PK_ActiveCardController.LoadListExtendCard();

                    $("#boxExtendCards table > tbody").html('');
                } else {
                    toastr.error(data.Message);
                }
            }
        });
    },
    loadBoxMonth: function () {
        var datenew = $("#datenewextendfrompicker").val();
        var datestart = $("#datestart").val();
        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/Partial_Month',
            type: 'GET',
            data: {
                datenew: datenew,
                datestart: datestart
            },
            success: function (response) {
                $('#boxMonth').html('');
                $('#boxMonth').html(response);
                LoadDateRangePicker();
              
            }
        });
    },
    ExtendCardsV2: function () {
        //Cấu hình mới
        var newDate = $("#datenewextendfrompicker").val();    
        var allownegative = $("#isAllowNegativeDays").prop("checked");  
        var json = $("#jsonMonth").val();

        var obj = {
            DateExtend: newDate,       
            isAllowNegativeDays: allownegative,
            Json: json
        }

        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/ExtendSelectedCardV2',
            data: { obj: obj },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    //toastr.success(data.Message);
                    ActiveCardController.loadData(true);
                    //PK_ActiveCardController.LoadListExtendCard();

                    $("#boxExtendCards table > tbody").html('');

                    bootbox.alert({
                        message: "<span style='font-size:24px; color:green'> Gia hạn thành công!</span>"
                    });
                } else {
                    //toastr.error(data.Message);

                    bootbox.alert({
                        message: "<span style='font-size:24px; color:red'> Có lỗi xảy ra!</span>"
                    });
                }
            }
        });
    },
    ExtendAllCardV2: function () {
        //Query
        var key = $("input[name=key]").val();
        var anotherkey = $("input[name=txtAnotherKey]").val();
        var cardgroup = $("#cardgroups").val();
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var customergroup = $("#customergroup").val();
        var allownegative = $("#isAllowNegativeDays").prop("checked");

        //Cấu hình mới
        var newDate = $("#datenewextendfrompicker").val();
        var feeLevel = $("#newfeelevel").val();
        var newDateActive = $("#datenewactivefrompicker").val();
        var json = $("#jsonMonth").val();

        var obj = {
            KeyWord: key,
            AnotherKey: anotherkey,
            CardGroup: cardgroup,
            CustomerGroup: customergroup,
            DateExtend: newDate,
            FeeLevel: feeLevel,
            isAllowNegativeDays: allownegative,
            DateActive: newDateActive,
            fromdate: fromdate,
            todate: todate,
            Json: json
        }

        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/ExtendAllCardV2',
            data: { obj: obj },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                   // toastr.success(data.Message);
                    ActiveCardController.loadData();

                    bootbox.alert({
                        message: "<span style='font-size:24px; color:green'> Gia hạn thành công!</span>"
                    });
                } else {
                    //toastr.error(data.Message);

                    bootbox.alert({
                        message: "<span style='font-size:24px; color:red'> Có lỗi xảy ra!</span>"
                    });
                }
            }
        });
    },
    ChangeDateMoney: function (id, type) {
        var money = $("#" + id).find("input[name=txtMoney]").val();
        var date = $("#" + id).find("input[name=txtDateM]").val();
        var obj = {
            Id: id,
            Money: money,
            Type: type,
            Date: date,
            Json: $("#jsonMonth").val()
        }
        $.ajax({
            url: _prefixParkingDomain + '/ActiveCard/ChangeDateMoney',
            data: { obj: obj },
            type: 'json',
            //async: true,
            success: function (data) {
                $("#jsonMonth").val(data);
            }
        });
    }
}

function SearchListCard() {
    ActiveCardController.loadData(true);
}

function addExtendCard(id) {
    ActiveCardController.addListExtendCard(id);
}

function RemoveAllExtendCard() {
    ActiveCardController.removeAllExtend();
}

function RemoveExtendCard(id) {
    ActiveCardController.removeExtend(id);
}

var confirmExtendCard = $('#confirmExtendCard').val();
var messErrExtendCard = $('#messErrExtendCard').val();

function ExtendAllCard() {
    var count = 0;
    $('#boxCards table > tbody').find("tr").each(function () {
        count++;
    });

    if (count > 0) {
        if (confirm(confirmExtendCard)) {
            ActiveCardController.ExtendAllCard();
        }
    } else {
        toastr.error(messErrExtendCard);
    }
}

function ExtendAllOneMonth() {
    var count = 0;
    $('#boxCards table > tbody').find("tr").each(function () {
        count++;
    });

    if (count > 0) {
        if (confirm(confirmExtendCard)) {
            ActiveCardController.ExtendAllOneMonth();
        }
    } else {
        toastr.error(messErrExtendCard);
    }
}

function ExtendCards() {

    var number = 0;
    $("#boxExtendCards table > tbody").find("tr").each(function () {
        number += 1;
    })

    if (number > 0) {
        if (confirm(confirmExtendCard)) {
            ActiveCardController.ExtendCards();
        }
    } else {
        toastr.error(messErrExtendCard);
    }
}

function ExtendCardsOneMonth() {

    var number = 0;
    $("#boxExtendCards table > tbody").find("tr").each(function () {
        number += 1;
    })

    if (number > 0) {
        if (confirm(confirmExtendCard)) {
            ActiveCardController.ExtendCardsOneMonth();
        }
    } else {
        toastr.error(messErrExtendCard);
    }
}

function SearchInFromList() {
    ActiveCardController.loadData(true);
}

function ExtendCardsV2() {

    var number = 0;
    $("#boxExtendCards table > tbody").find("tr").each(function () {
        number += 1;
    })

    if (number > 0) {
        if (confirm(confirmExtendCard)) {
            ActiveCardController.ExtendCardsV2();
        }
    } else {
        toastr.error(messErrExtendCard);
    }
}

function ExtendAllCardV2() {
    var count = 0;
    $('#boxCards table > tbody').find("tr").each(function () {
        count++;
    });

    if (count > 0) {
        if (confirm(confirmExtendCard)) {
            ActiveCardController.ExtendAllCardV2();
        }
    } else {
        toastr.error(messErrExtendCard);
    }
}

//function ChangeDateMoney(id, type) {
//    var money = $("#" + id).find("input[name=txtMoney]").val();
//    var date = $("#" + id).find("input[name=txtDateM]").val();
//    var obj = {
//        Id: id,
//        Money: money,
//        Type: type,
//        Date: date,
//        Json: $("#jsonMonth").val()
//    }
//    $.ajax({
//        url: _prefixParkingDomain + '/ActiveCard/ChangeDateMoney',
//        data: { obj: obj },
//        type: 'json',
//        //async: true,
//        success: function (data) {
//            $("#jsonMonth").val(data);
//        }
//    });
//}