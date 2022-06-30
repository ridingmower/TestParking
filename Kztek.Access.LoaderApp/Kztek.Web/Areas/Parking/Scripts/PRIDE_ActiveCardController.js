$(function () {
    $("body").on("change", "input[name=datenewextendfrompicker]", function () {
        $("#strIDCards").val("");
        $("#priceUnCheck").val(0);
        $("#checkExtend").val("0");
        PRIDE_ActiveCardController_v2.loadData(false);
        PRIDE_ActiveCardController_v2.LoadListExtendCard();
    })

    $("body").on("click", "button[name=btnPrintBill]", function () {
        var count = $("#checkExtend").val();
        if (count === "0") {
            toastr.error("Vui lòng gia hạn thẻ");
        } else {
             if (confirm("Bạn có muốn in hóa đơn không?")) {
            PRIDE_ActiveCardController_v2.GetListCardNumberExtendAll();
        }
        }
       
    })
})

var config = {
    changePageSize: false,
    pageIndex: 1
}

var PRIDE_ActiveCardController_v2 = {
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
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/PRIDE_boxCards',
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
              

                //load tiền theo nhóm thẻ
                PRIDE_ActiveCardController_v2.BoxMoneyByCardGroup();

                //tổng tiền
                GetTotalMoney();

                if (totalPage !== "0") {
                    PRIDE_ActiveCardController_v2.paging(totalPage, changePageSize);
                    //toastr.success("Tìm kiếm thành công");
                } else {
                    $("#boxCards #pageIndex").text("0");
                    PRIDE_ActiveCardController_v2.pagingEmpty();
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
                PRIDE_ActiveCardController_v2.loadData(false);
                //$("#chkCheckALL").prop("checked", false);
            }
        });
    },
    addListExtendCard: function (listId,price) {
        $.ajax({
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/AddNewListCardSub',
            data: { listId: listId,price:price },
            type: 'json',
            async: true,
            success: function (data) {            
                PRIDE_ActiveCardController_v2.LoadListExtendCard();               
            }
        });
    },
    LoadListExtendCard: function () {
        $.ajax({
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/boxCardChoices',
            type: 'GET',
            success: function (response) {
                $("#boxExtendCards table > tbody").html('');
                $("#boxExtendCards table > tbody").html(response);

                var number = 0;
                $("#boxExtendCards table > tbody").find("tr").each(function () {
                    number += 1;
                })

                $("#boxExtendCards").find(".numberExtendCard").text(number);
                PRIDE_ActiveCardController_v2.GetTotalPriceBoxCardChoice();
            }
        })
    },
    removeAllExtend: function () {
        $.ajax({
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/DeleteAllSelectedCard',
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    PRIDE_ActiveCardController_v2.LoadListExtendCard();
                }
            }
        });
    },
    removeExtend: function (id) {
        $.ajax({
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/DeleteOneSelectedCard',
            data: { id: id },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    PRIDE_ActiveCardController_v2.LoadListExtendCard();
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
        var totalMoney = $("#totalPrice1").text();
        if (feeLevel === '' || feeLevel === null || feeLevel === 'undefined') {
            feeLevel = "0";
        }

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
            todate: todate,
            TotalMoney: totalMoney
        }

        $.ajax({
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/ExtendAllCard_v2',
            data: { obj: obj },
            type: 'json',
            async: false,
            success: function (data) {
                if (data.isSuccess) {
                    //if (confirm("Bạn có muốn in hóa đơn không?")) {
                    //    PRIDE_ActiveCardController_v2.GetListCardNumberExtendAll();
                    //}
                    toastr.success(data.Message);
                    PRIDE_ActiveCardController_v2.loadData();  
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
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/ExtendSelectedCard',
            data: { obj: obj },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.result.isSuccess) {
                    if (confirm("Bạn có muốn in hóa đơn không?")) {
                        var url = '/Parking/PRIDE_ActiveCard/PrintBill_v2?cardnumbers=' + data.cardnumbers;
                        window.open(url, '_blank');
                    }
                   
                    toastr.success(data.result.Message);
                    PRIDE_ActiveCardController_v2.loadData(true);
                    //PK_PRIDE_ActiveCardController_v2.LoadListExtendCard();
                  
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
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/UnCheckExtendCard',
            data: { Id: Id, price: price, priceUnCheck: priceUnCheck, strIDCards: strIDCards },
            type: 'json',
            async: true,
            success: function (data) {
                $("#strIDCards").val(data.strIDCards);
                $("#priceUnCheck").val(data.priceUnCheck);               
                PRIDE_ActiveCardController_v2.loadData();
            }
        });
    },
    UnCheckALL: function (strId, check) {
        var strIDCards = $("#strIDCards").val();
        var priceUnCheck = $("#priceUnCheck").val();
        $.ajax({
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/UnCheckALL',
            data: { strIds: strId, isCheck: check, priceUnCheck: priceUnCheck, strIDCards: strIDCards },
            type: 'json',
            async: true,
            success: function (data) {
                $("#strIDCards").val(data.strIDCards);
                $("#priceUnCheck").val(data.priceUnCheck);
                PRIDE_ActiveCardController_v2.loadData();
            }
        });
    },
    CheckFeeCardGroup: function () {
        var cardgroup = $("#cardgroups").val();
        var check = true;
        $.ajax({
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/CheckFeeCardGroup',
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
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/TotalPriceBoxCardChoice',
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
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/GetListCardNumberExtendAll',
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
                var url = '/Parking/PRIDE_ActiveCard/PrintBill_v2?cardnumbers=' + data + '&type=' + '1';
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
            url: _prefixParkingDomain + '/PRIDE_ActiveCard/BoxMoneyByCardGroup',
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
    PRIDE_ActiveCardController_v2.loadData(true);
}

function addExtendCard(id, price) {
    PRIDE_ActiveCardController_v2.addListExtendCard(id, price);
}

function UnCheckExtendCard(id,price) {
    PRIDE_ActiveCardController_v2.UnCheckExtendCard(id,price);
}

function RemoveAllExtendCard() {
    PRIDE_ActiveCardController_v2.removeAllExtend();
}

function RemoveExtendCard(id) {
    PRIDE_ActiveCardController_v2.removeExtend(id);
}

var confirmExtendCard = $('#confirmExtendCard').val();
var messErrExtendCard = $('#messErrExtendCard').val();

function ExtendAllCard() {

    var check = PRIDE_ActiveCardController_v2.CheckFeeCardGroup();
    if (check) {
        var count = 0;
        $('#boxCards table > tbody').find("tr").each(function () {
            count++;
        });

        if (count > 0) {
            if (confirm(confirmExtendCard)) {
                PRIDE_ActiveCardController_v2.ExtendAllCard();               
            }
        } else {
            toastr.error(messErrExtendCard);
        }
    }
}

function ExtendCards() {

    var check = PRIDE_ActiveCardController_v2.CheckFeeCardGroup();
    if (check) {
        var number = 0;
        $("#boxExtendCards table > tbody").find("tr").each(function () {
            number += 1;
        })

        if (number > 0) {
            if (confirm(confirmExtendCard)) {
                PRIDE_ActiveCardController_v2.ExtendCards();
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
    PRIDE_ActiveCardController_v2.loadData(true);
    
}

function GetTotalMoney() {
    var money = $("#newfeelevel").val();
    var strIDCards = $("#strIDCards").val();
    var countuncheck = 0;

    if (strIDCards !== '' && strIDCards !== 'null' && strIDCards !== 'undefined') {
        countuncheck = strIDCards.split(',').length;
    }

    if (money !== "0") {
        money = money.replace(".", "").replace(".", "");
    }

    var rowCount = $('#totalRow').text() - countuncheck;
    $("#totalPrice1").text(FormatMoney(money * rowCount));
}
