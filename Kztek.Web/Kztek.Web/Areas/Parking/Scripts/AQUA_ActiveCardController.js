$(function () {
    $("body").on("change", "input[name=datenewextendfrompicker]", function () {
        $("#strIDCards").val("");
        $("#priceUnCheck").val(0);
        $("#checkExtend").val("0");
        AQUA_ActiveCardController.loadData(false);
        AQUA_ActiveCardController.LoadListExtendCard();
    })

    $("body").on("click", "button[name=btnPrintBill]", function () {
        var count = $("#checkExtend").val();
        if (count === "0") {
            toastr.error("Vui lòng gia hạn thẻ");
        } else {
             if (confirm("Bạn có muốn in hóa đơn không?")) {
            AQUA_ActiveCardController.GetListCardNumberExtendAll();
        }
        }
       
    })
})

var config = {
    changePageSize: false,
    pageIndex: 1
}

var AQUA_ActiveCardController = {
    init: function () {

    },
    loadData: function (changePageSize) {
        var key = $("input[name=key]").val();
        var strIDCards = $("#strIDCards").val();
        var address = $("input[name=address]").val();
        var anotherkey = $("input[name=txtAnotherKey]").val();
        var cardgroup = $("#cardgroups").val();
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var customergroup = $("#customergroup").val();
        var newDateActive = $("#datenewextendfrompicker").val();

        $.ajax({
            url: _prefixParkingDomain + '/AQUA_ActiveCard/boxCards',
            type: 'GET',
            data: {
                key: key,
                newDateActive: newDateActive,
                strIDCards: strIDCards,
                address: address,
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

                $("#boxCards .numberCardTotal").text(number);

                var count = 0;
                if (strIDCards !== '' && strIDCards !== null && strIDCards !== 'undefined') {
                    count = parseInt(number) - parseInt(strIDCards.split(',').length);
                    if (count < 0) {
                        number = "0";
                    } else {
                        number = count.toString();
                    }
                   
                }

                $("#boxCards #pageIndex").text(config.pageIndex);

                $("#boxCards #pageCount").text(totalPage);

                //số thẻ đã chọn
                $("#boxCards .numberCard").text(number);
             
                var money = parseInt($("#Total").val()) - parseInt($("#priceUnCheck").val());
                if (isNaN(money) || money < 0) {
                    money = 0;
                }
                //tổng tiền
                $("#totalPrice1").text(FormatMoney(money));

                //load tiền theo nhóm thẻ
                AQUA_ActiveCardController.BoxMoneyByCardGroup();

                if (totalPage !== "0") {
                    AQUA_ActiveCardController.paging(totalPage, changePageSize);
                    //toastr.success("Tìm kiếm thành công");
                } else {
                    $("#boxCards #pageIndex").text("0");
                    AQUA_ActiveCardController.pagingEmpty();
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
                AQUA_ActiveCardController.loadData(false);
                //$("#chkCheckALL").prop("checked", false);
            }
        });
    },
    addListExtendCard: function (listId,price) {
        $.ajax({
            url: _prefixParkingDomain + '/AQUA_ActiveCard/AddNewListCardSub',
            data: { listId: listId,price:price },
            type: 'json',
            async: true,
            success: function (data) {            
                AQUA_ActiveCardController.LoadListExtendCard();               
            }
        });
    },
    LoadListExtendCard: function () {
        $.ajax({
            url: _prefixParkingDomain + '/AQUA_ActiveCard/boxCardChoices',
            type: 'GET',
            success: function (response) {
                $("#boxExtendCards table > tbody").html('');
                $("#boxExtendCards table > tbody").html(response);

                var number = 0;
                $("#boxExtendCards table > tbody").find("tr").each(function () {
                    number += 1;
                })

                $("#boxExtendCards").find(".numberExtendCard").text(number);
                AQUA_ActiveCardController.GetTotalPriceBoxCardChoice();
            }
        })
    },
    removeAllExtend: function () {
        $.ajax({
            url: _prefixParkingDomain + '/AQUA_ActiveCard/DeleteAllSelectedCard',
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    AQUA_ActiveCardController.LoadListExtendCard();
                }
            }
        });
    },
    removeExtend: function (id) {
        $.ajax({
            url: _prefixParkingDomain + '/AQUA_ActiveCard/DeleteOneSelectedCard',
            data: { id: id },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    AQUA_ActiveCardController.LoadListExtendCard();
                }
            }
        });
    },
    ExtendAllCard: function () {
        //Query
        var key = $("input[name=key]").val();
        var strIDCards = $("#strIDCards").val();
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
            strIDCards: strIDCards,
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
            url: _prefixParkingDomain + '/AQUA_ActiveCard/ExtendAllCard',
            data: { obj: obj },
            type: 'json',
            async: false,
            success: function (data) {
                if (data.isSuccess) {
                    //if (confirm("Bạn có muốn in hóa đơn không?")) {
                    //    AQUA_ActiveCardController.GetListCardNumberExtendAll();
                    //}
                    toastr.success(data.Message);
                    AQUA_ActiveCardController.loadData();  
                    $("#checkExtend").val("1");
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
            url: _prefixParkingDomain + '/AQUA_ActiveCard/ExtendSelectedCard',
            data: { obj: obj },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.result.isSuccess) {
                    if (confirm("Bạn có muốn in hóa đơn không?")) {
                        var url = '/Parking/AQUA_ActiveCard/PrintBill?cardnumbers=' + data.cardnumbers;
                        window.open(url, '_blank');
                    }
                   
                    toastr.success(data.result.Message);
                    AQUA_ActiveCardController.loadData(true);
                    //PK_AQUA_ActiveCardController.LoadListExtendCard();
                  
                    $("#boxExtendCards table > tbody").html('');
                } else {
                    toastr.error(data.result.Message);
                }
            }
        });
    },
    UnCheckExtendCard: function (Id,price) {
        var strIDCards = $("#strIDCards").val();
        var priceUnCheck = $("#priceUnCheck").val();
        $.ajax({
            url: _prefixParkingDomain + '/AQUA_ActiveCard/UnCheckExtendCard',
            data: { Id: Id, price: price, priceUnCheck: priceUnCheck, strIDCards: strIDCards },
            type: 'json',
            async: true,
            success: function (data) {
                $("#strIDCards").val(data.strIDCards);
                $("#priceUnCheck").val(data.priceUnCheck);               
                AQUA_ActiveCardController.loadData();
            }
        });
    },
    UnCheckALL: function (strId, check) {
        var strIDCards = $("#strIDCards").val();
        var priceUnCheck = $("#priceUnCheck").val();
        $.ajax({
            url: _prefixParkingDomain + '/AQUA_ActiveCard/UnCheckALL',
            data: { strIds: strId, isCheck: check, priceUnCheck: priceUnCheck, strIDCards: strIDCards },
            type: 'json',
            async: true,
            success: function (data) {
                $("#strIDCards").val(data.strIDCards);
                $("#priceUnCheck").val(data.priceUnCheck);
                AQUA_ActiveCardController.loadData();
            }
        });
    },
    CheckFeeCardGroup: function () {
        debugger;
        var cardgroup = $("#cardgroups").val();
        var check = true;
        $.ajax({
            url: _prefixParkingDomain + '/AQUA_ActiveCard/CheckFeeCardGroup',
            data: { CardGroupID: cardgroup },
            type: 'json',
            async: false,
            success: function (data) {
                //if (!data.isSuccess) {
                //    toastr.error(data.Message);
                //    check = false;
                //} else {
                //    check = true;
                //}
            }
        });       
        return check;

    },
    GetTotalPriceBoxCardChoice: function () {
        $.ajax({
            url: _prefixParkingDomain + '/AQUA_ActiveCard/TotalPriceBoxCardChoice',
            data: {},
            type: 'json',
            async: false,
            success: function (data) {
                $("#totalPrice2").text(data);
            }
        });       
    },
    GetListCardNumberExtendAll: function () {
        var key = $("input[name=key]").val();
        var strIDCards = $("#strIDCards").val();
        var address = $("input[name=address]").val();
        var anotherkey = $("input[name=txtAnotherKey]").val();
        var cardgroup = $("#cardgroups").val();
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var customergroup = $("#customergroup").val();
        var newDateActive = $("#datenewextendfrompicker").val();
        $.ajax({
            url: _prefixParkingDomain + '/AQUA_ActiveCard/GetListCardNumberExtendAll',
            type: 'GET',
            data: {
                key: key,
                newDateActive: newDateActive,
                strIDCards: strIDCards,
                address: address,
                anotherkey: anotherkey,
                cardgroups: cardgroup,
                customergroup: customergroup,
                fromdate: fromdate,
                todate: todate,
                page: config.pageIndex
            },
            //async: true,
            success: function (data) {
                var url = '/Parking/AQUA_ActiveCard/PrintBill?cardnumbers=' + data + '&type=' + '1';
                window.open(url, '_blank');
            }
        });
    },
    BoxMoneyByCardGroup: function () {
        var key = $("input[name=key]").val();
        var strIDCards = $("#strIDCards").val();
        var address = $("input[name=address]").val();
        var anotherkey = $("input[name=txtAnotherKey]").val();
        var cardgroup = $("#cardgroups").val();
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var customergroup = $("#customergroup").val();
        var newDateActive = $("#datenewextendfrompicker").val();

        $.ajax({
            url: _prefixParkingDomain + '/AQUA_ActiveCard/BoxMoneyByCardGroup',
            type: 'GET',
            data: {
                key: key,
                newDateActive: newDateActive,
                strIDCards: strIDCards,
                address: address,
                anotherkey: anotherkey,
                cardgroups: cardgroup,
                customergroup: customergroup,
                fromdate: fromdate,
                todate: todate           
            },
            success: function (response) {
                $('#boxMoneyCardGroup').html('');
                $('#boxMoneyCardGroup').html(response);                
            }
        });
    }
}

function SearchListCard() {
    $("#strIDCards").val("");
    $("#priceUnCheck").val(0);
    $("#checkExtend").val("0");
    AQUA_ActiveCardController.loadData(true);
}

function addExtendCard(id, price) {
    AQUA_ActiveCardController.addListExtendCard(id, price);
}

function UnCheckExtendCard(id,price) {
    AQUA_ActiveCardController.UnCheckExtendCard(id,price);
}

function RemoveAllExtendCard() {
    AQUA_ActiveCardController.removeAllExtend();
}

function RemoveExtendCard(id) {
    AQUA_ActiveCardController.removeExtend(id);
}

var confirmExtendCard = $('#confirmExtendCard').val();
var messErrExtendCard = $('#messErrExtendCard').val();

function ExtendAllCard() {
    debugger;
    var check = AQUA_ActiveCardController.CheckFeeCardGroup();
    if (check) {
        debugger;
        var count = 0;
        $('#boxCards table > tbody').find("tr").each(function () {
            count++;
        });

        if (count > 0) {
            if (confirm(confirmExtendCard)) {
                AQUA_ActiveCardController.ExtendAllCard();               
            }
        } else {
            toastr.error(messErrExtendCard);
        }
    }
}

function ExtendCards() {

    var check = AQUA_ActiveCardController.CheckFeeCardGroup();
    if (check) {
        var number = 0;
        $("#boxExtendCards table > tbody").find("tr").each(function () {
            number += 1;
        })

        if (number > 0) {
            if (confirm(confirmExtendCard)) {
                AQUA_ActiveCardController.ExtendCards();
            }
        } else {
            toastr.error(messErrExtendCard);
        }
    }
  
}

function SearchInFromList() {
    $("#strIDCards").val("");
    $("#priceUnCheck").val(0);
    $("#checkExtend").val("0");
    AQUA_ActiveCardController.loadData(true);
}