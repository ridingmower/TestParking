var config = {
    changePageSize: false,
    pageIndex: 1
}

var ActiveCardDateActiveController = {
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
            url: _prefixParkingDomain + '/ActiveCardDateActive/boxCards',
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
                    ActiveCardDateActiveController.paging(totalPage, changePageSize);
                    //toastr.success("Tìm kiếm thành công");
                } else {
                    $("#boxCards #pageIndex").text("0");
                    ActiveCardDateActiveController.pagingEmpty();
                }
            }
        })
    },
    search: function () {

    },
    pagingEmpty: function () {
        $('#pagination').empty();
        $('#pagination').removeData("twbs-pagination");
        $('#pagination').unbind("page");
    },
    paging: function (totalPage, changePageSize) {
        debugger;
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
                ActiveCardDateActiveController.loadData(false);
                $("#chkCheckALL").prop("checked", false);
            }
        });
    },
    addListExtendCard: function (listId) {
        $.ajax({
            url: _prefixParkingDomain + '/ActiveCardDateActive/AddNewListCardSub',
            data: { listId: listId },
            type: 'json',
            async: true,
            success: function (data) {
                ActiveCardDateActiveController.LoadListExtendCard();
            }
        });
    },
    LoadListExtendCard: function () {
        $.ajax({
            url: _prefixParkingDomain + '/ActiveCardDateActive/boxCardChoices',
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
            url: _prefixParkingDomain + '/ActiveCardDateActive/DeleteAllSelectedCard',
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    ActiveCardDateActiveController.LoadListExtendCard();
                }
            }
        });
    },
    removeExtend: function (id) {
        $.ajax({
            url: _prefixParkingDomain + '/ActiveCardDateActive/DeleteOneSelectedCard',
            data: { id: id },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    ActiveCardDateActiveController.LoadListExtendCard();
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
            url: _prefixParkingDomain + '/ActiveCardDateActive/ExtendAllCard',
            data: { obj: obj },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    toastr.success(data.Message);
                    ActiveCardDateActiveController.loadData();
                } else {
                    toastr.error(data.Message);
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
            url: _prefixParkingDomain + '/ActiveCardDateActive/ExtendSelectedCard',
            data: { obj: obj },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    toastr.success(data.Message);
                    ActiveCardDateActiveController.loadData(true);
                    //PK_ActiveCardController.LoadListExtendCard();

                    $("#boxExtendCards table > tbody").html('');
                } else {
                    toastr.error(data.Message);
                }
            }
        });
    }
}

function SearchListCard() {
    ActiveCardDateActiveController.loadData(true);
}

function addExtendCard(id) {
    ActiveCardDateActiveController.addListExtendCard(id);
}

function RemoveAllExtendCard() {
    ActiveCardDateActiveController.removeAllExtend();
}

function RemoveExtendCard(id) {
    ActiveCardDateActiveController.removeExtend(id);
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
            ActiveCardDateActiveController.ExtendAllCard();
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
            ActiveCardDateActiveController.ExtendCards();
        }
    } else {
    toastr.error(messErrExtendCard);
    }
}

function SearchInFromList() {
   ActiveCardDateActiveController.loadData(true);
}