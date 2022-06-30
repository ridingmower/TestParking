var AC_CardController = {
    init: function () {

    },
    loadDataCard: function (code) {
        $.ajax({
            url: _prefixAccessDomain + '/tblCard/GetListCardByKey',
            data: { key: code },
            type: 'json',
            async: true,
            success: function (data) {
                $("input[id=CardNumber]").autocomplete({
                    source: data
                });
            }
        });
    },
    loadDataCardNo: function (cardno) {
        $.ajax({
            url: _prefixAccessDomain + '/tblCard/GetListCardByCardNo',
            data: { key: cardno },
            type: 'json',
            async: true,
            success: function (data) {
                $("input[id=CardNo]").autocomplete({
                    source: data
                });
            }
        });
    },
    getCard: function (code) {
        $.ajax({
            url: _prefixAccessDomain + '/tblCard/GetCard',
            data: { code: code },
            success: function (data) {
                if (data !== null) {
                    $("#CardNo").val(data.CardNo);
                    $("#CardDescription").val(data.Description);
                } else {
                    $('#cardinfo').find("#boxCardInfo span").text("Thẻ không tồn tại trong hệ thống");
                }
            }
        });
    },
    loadDataCustomer: function (code) {
        $.ajax({
            url: _prefixAccessDomain + '/tblCard/GetListCustomerByKey',
            data: { key: code },
            type: 'json',
            async: true,
            success: function (data) {
                $("input[id=txtSearchCustomer]").autocomplete({
                    source: function (request, response) {
                        response(data);
                        return;
                    },
                    select: function (e, ui) {
                        AC_CardController.getCustomer(ui.item.id);
                    }
                });
            }
        });
    },
    getCustomer: function (code) {
        $.ajax({
            url: _prefixAccessDomain + '/tblCard/GetCustomer',
            data: { code: code },
            //type: 'json',
            //async: true,
            success: function (data) {
                $("#CustomerID").val(data.CustomerID);
                $("#CustomerCode").val(data.CustomerCode);
                $("#CustomerName").val(data.CustomerName);
                $("#CustomerIdentify").val(data.IDNumber);
                $("#CustomerMobile").val(data.Mobile);
                $('#CustomerGroupID').val(data.CustomerGroupID);
                $('#CustomerAddress').val(data.Address);

                //Vị trí upload avatar
                FileUploadController.init("BoxRenderFile", "FileUpload", "", data.CustomerID);

                $('#CustomerGroupID').trigger('chosen:updated');
                $('#txtSearchCustomer').val('');
            }
        });
    },
    RemoveAllCard: function (totalItem, url) {
        $.ajax({
            url: _prefixAccessDomain + '/tblCard/RemoveAllCardSeleted',
            data: {},
            type: 'json',
            async: true,
            success: function (data) {
                //AC_CardController.loadModalButton(totalItem);
                window.location.href = url;
            }
        });
    },
    AddRemoveCardChoice: function (choices, totalItem, isAdd) {
        $.ajax({
            url: _prefixAccessDomain + '/tblCard/AddOrRemoveOneAllCardSeleted',
            data: { CardNumbers: choices, isAdd: isAdd },
            type: 'json',
            async: true,
            success: function (data) {
                AC_CardController.loadModalButton(totalItem);
            }
        });
    },
    loadModalButton: function (totalItem, url) {
        $.ajax({
            url: _prefixAccessDomain + '/tblCard/ModalButtonControl',
            type: 'GET',
            data: {
                totalItem: totalItem,
                /*url: url*/
            },
            success: function (response) {
                $('#boxCardAction').html('');

                $('#boxCardAction').html(response);
            }
        });
    },
    ActionToCard: function (type, url) {
        var levelid = $("#boxCardAction").find("#accesslevel").val();

        $.ajax({
            url: _prefixAccessDomain + '/tblCard/ActionToCards',
            data: { type: type, value: levelid },
            type: 'json',
            async: true,
            success: function (data) {
                if (data.isSuccess) {
                    toastr.success(data.Message);

                    window.location.href = url;
                } else {
                    toastr.error(data.Message);
                }
            }
        });
    },
    UpdateCardType: function (cardtype, url) {
        //var levelid = $("#boxCardAction").find("#accesslevel").val();

        $.ajax({
            url: _prefixAccessDomain + '/tblCard/UpdateCardType',
            data: { cardtype: cardtype, url: url },
            type: 'json',
            async: true,
            success: function (data) {
                if (data.isSuccess) {
                    toastr.success(data.Message);

                    window.location.href = url;
                } else {
                    toastr.error(data.Message);
                }
            }
        });
    }
};


function ChangeAnotherCustomer() {
    AC_CardController.getCustomer("", "0");
    $("#isChangeCustomer").val("true");

    $("#CustomerCode").focus();
    toastr.info("Vui lòng nhập mã khách hàng mới. Nhấn 'Lưu và thoát' để lưu thay đổi.", { timeOut: 1500 });
}

function ReturnCardFromCustomer() {
    AC_CardController.getCustomer("", "0");
    $("#isReturnCard").val("true");

    toastr.info("Trả thẻ thành công. Nhấn 'Lưu và thoát' để lưu thay đổi.", { timeOut: 1500 });
}

function RemoveAllSelectedCard(total, url) {
    AC_CardController.RemoveAllCard(total, url);
}

function LockCardSelected(url) {
    bootbox.confirm("Bạn chắc chắn muốn khóa các thẻ này?", function (result) {
        if (result) {
            AC_CardController.ActionToCard("LOCK", url);
        }
    });
}

function UnlockCardSelected(url) {
    bootbox.confirm("Bạn chắc chắn muốn mở khóa các thẻ này?", function (result) {
        if (result) {
            AC_CardController.ActionToCard("UNLOCK", url);
        }
    });
}

function DeleteCardSelected(url) {
    bootbox.confirm("'Bạn chắc chắn muốn xóa các thẻ này?", function (result) {
        if (result) {
            AC_CardController.ActionToCard("DELETE", url);
        }
    });
}

function AuthorizeCardSelected(url) {
    bootbox.confirm("'Bạn chắc chắn muốn phân quyền các thẻ này?", function (result) {
        if (result) {
            AC_CardController.ActionToCard("AUTHORIZE", url);
        }
    });
}

function AuthorizeAllCard(url) {
    bootbox.confirm("'Bạn chắc chắn muốn phân quyền tất cả thẻ?", function (result) {
        if (result) {
            AC_CardController.ActionToCard("AUTHORIZEALL", url);
        }
    });
}

function UpdateCardType(url) {
    bootbox.confirm("Bạn chắc chuyển đổi loại thẻ không?", function (result) {
        if (result) {
            var cardtypes = $("#cardtype").val();            
            AC_CardController.UpdateCardType(cardtypes, url);
        }
    });
}
