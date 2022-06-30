$(function () {
    $("body").on("autocomplete", ".lockcard", function () {
        source: []
    })

    $("body").on("keyup", ".lockcard", function () {             
        PK_CardController.LoadNoteLock();
    })

    $("body").on("click", ".lockcard", function () {
        PK_CardController.LoadNoteLock();
        $(".lockcard").keydown();
    })
    

    $("body").on("autocomplete", ".unlockcard", function () {
        source: []
    })

    $("body").on("keyup", ".unlockcard", function () {   
        PK_CardController.LoadNoteUnLock();
    })  

    $("body").on("click", ".unlockcard", function () {
        PK_CardController.LoadNoteUnLock();
        $(".unlockcard").keydown();
    })  
})

var PK_CardController = {
    init: function () {

    },
    loadDataCard: function (code) {
        $.ajax({
            url: _prefixParkingDomain + '/tblCard/GetListCardByKey',
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
            url: _prefixParkingDomain + '/tblCard/GetListCardByCardNo',
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
            url: _prefixParkingDomain + '/tblCard/GetCard',
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
            url: _prefixParkingDomain + '/tblCard/GetListCustomerByKey',
            data: { key: code },
            type: 'json',
            async: true,
            success: function (data) {
                //$("input[id=txtSearchCustomer]").autocomplete({
                //    source: data
                //});

                $("input[id=txtSearchCustomer]").autocomplete({
                    source: function (request, response) {
                        response(data);
                        return;
                    },
                    select: function (e, ui) {
                        PK_CardController.getCustomer(ui.item.id);
                    }
                });
            }
        });
    },
    getCustomer: function (code) {
        $.ajax({
            url: _prefixParkingDomain + '/tblCard/GetCustomer',
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
                $('#CompartmentId').val(data.CompartmentId);
                //Vị trí upload avatar
                FileUploadController.init("BoxRenderFile", "FileUpload", "", data.CustomerID);

                //
                //$('.chosen-select').chosen({ allow_single_deselect: true });
                $('#CustomerGroupID').trigger('chosen:updated');
                $('#txtSearchCustomer').val('');
            }
        });
    },
    RemoveAllCard: function (totalItem, url) {
        $.ajax({
            url: _prefixParkingDomain + '/tblCard/RemoveAllCardSeleted',
            data: {},
            type: 'json',
            async: true,
            success: function (data) {
                //PK_CardController.loadModalButton(totalItem);
                window.location.href = url;
            }
        });
    },
    AddRemoveCardChoice: function (choices, totalItem, isAdd) {
        $.ajax({
            url: _prefixParkingDomain + '/tblCard/AddOrRemoveOneAllCardSeleted',
            data: { CardNumbers: choices, isAdd: isAdd },
            type: 'json',
            async: true,
            success: function (data) {
                PK_CardController.loadModalButton(totalItem);
            }
        });
    },
    loadModalButton: function (totalItem, url) {

        $.ajax({
            url: _prefixParkingDomain + '/tblCard/ModalButtonControl',
            type: 'POST',
            data: {
                totalItem: totalItem,
                url: url
            },
            success: function (response) {
                $('#boxCardAction').html('');

                $('#boxCardAction').html(response);
            }
        });
    },
    loadAllModalButton: function (totalItem, url) {
      
        $.ajax({
            url: _prefixParkingDomain + '/tblCard/AllModalButtonControl',
            type: 'POST',
            data: {
                totalItem: totalItem,
                url: url
            },
            success: function (response) {
                $('#boxCardAllAction').html('');

                $('#boxCardAllAction').html(response);
            }
        });
    },
    AllActionToCard: function (type, url, mess, key, cardgr ,cus) {
        debugger;
        $.ajax({
            url: _prefixParkingDomain + '/tblCard/AllActionToCards',
            data: { type: type, mess: mess, key: key, cardgr: cardgr , cus : cus},
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
    ActionToCard: function (type, url, mess) {
        $.ajax({
            url: _prefixParkingDomain + '/tblCard/ActionToCards',
            data: { type: type, mess: mess },
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

    LoadNoteLock: function () {
        var key = $(".lockcard").val();
        $.ajax({
            url: _prefixParkingDomain + '/tblCard/LoadNoteLock',
            data: { key: key },
            type: 'json',
            async: true,
            success: function (data) {
                //$("input[id=txtSearchCustomer]").autocomplete({
                //    source: data
                //});

                //$(".lockcard").autocomplete({
                //    source: function (request, response) {
                //        response(data);
                //        return;
                //    },
                //    minLength: 2,
                //    select: function (e, ui) {                        
                //        $(".lockcard").val(ui.item.text);
                //    }
                //});

                $(".lockcard").autocomplete({
                    source: function (request, response) {
                        response(data);
                        return;
                    },
                    minLength: 0,
                    select: function (event, ui) {
                        $(".lockcard").val(ui.item.text);        
                    }
                });
            }
        });
    },
    LoadNoteUnLock: function () {
        var key = $(".unlockcard").val();
        $.ajax({
            url: _prefixParkingDomain + '/tblCard/LoadNoteUnLock',
            data: { key: key },
            type: 'json',
            async: true,
            success: function (data) {
                //$(".unlockcard").autocomplete({
                //    source: data
                //});
              
                //$(".unlockcard").autocomplete({
                //    source: function (request, response) {
                //        response(data);
                //        return;
                //    },
                //    select: function (e, ui) {
                //        $(".unlockcard").val(ui.item.text);
                //    }
                //});
                $(".unlockcard").autocomplete({
                    source: function (request, response) {
                        response(data);
                        return;
                    },
                    minLength: 0,
                    select: function (event, ui) {
                        $(".unlockcard").val(ui.item.text);
                    }
                });
            }
        });
    }
};

function ChangeAnotherCustomer() {
    var _changeAnotherCustomer = $('input[name=_changeAnotherCustomer]').val();
    PK_CardController.getCustomer("", "0");
    $("#isChangeCustomer").val("true");

    $("#CustomerCode").focus();
    toastr.info(_changeAnotherCustomer, { timeOut: 1500 });
}

function ReturnCardFromCustomer() {
    var _returnCardFromCustomer = $('input[name=_returnCardFromCustomer]').val();
    PK_CardController.getCustomer("", "0");
    $("#isReturnCard").val("true");

    $("#Plate1").val('');
    $("#Plate2").val('');
    $("#Plate3").val('');
    $("#VehicleName1").val('');
    $("#VehicleName2").val('');
    $("#VehicleName3").val('');

    toastr.info(_returnCardFromCustomer, { timeOut: 1500 });
}

function RemoveAllSelectedCard(total, url) {
    PK_CardController.RemoveAllCard(total, url);
}

var _reason = $('input[name=_reason]').val();

function LockCardSelected(url) {
    debugger;
    var _lockCardConfirm = $('input[name=_lockCardConfirm]').val();
    bootbox.confirm(_lockCardConfirm, function (result) {
        if (result) {          
            bootbox.prompt(_reason, function (mess) {
                if (mess === null) {
                    PK_CardController.ActionToCard("LOCK", url, mess);
                } else {
                    PK_CardController.ActionToCard("LOCK", url, mess);
                }
            });
            
            $(".bootbox-input-text").addClass("lockcard");
        }
    });
}

var _reason = $('input[name=_reason]').val();

function LockCardAllSelected(url, key, cus) {
    debugger;
    var cards = $('#cardgroups').val();
  
    var _lockCardConfirm = $('input[name=_lockCardConfirm]').val();
    bootbox.confirm(_lockCardConfirm, function (result) {
        if (result) {
            bootbox.prompt(_reason, function (mess) {
                if (mess === null) {
                    PK_CardController.AllActionToCard("LOCK", url, mess, key, cards, cus);
                } else {
                    PK_CardController.AllActionToCard("LOCK", url, mess, key, cards ,cus);
                }
            });

            $(".bootbox-input-text").addClass("lockcard");
        }
    });
}

function UnLockCardAllSelected(url, key , cus) {

    var cards = $('#cardgroups').val();
    var _openCardConfirm = $('input[name=_openCardConfirm]').val();
    bootbox.confirm(_openCardConfirm, function (result) {
        if (result) {
            bootbox.prompt(_reason, function (mess) {
                if (mess === null) {
                    PK_CardController.AllActionToCard("UNLOCK", url, mess, key, cards , cus);
                } else {
                    PK_CardController.AllActionToCard("UNLOCK", url, mess, key, cards , cus);
                }
            });

            $(".bootbox-input-text").addClass("unlockcard");
        }
    });
}


function UnlockCardSelected(url) {
    var _openCardConfirm = $('input[name=_openCardConfirm]').val();

    bootbox.confirm(_openCardConfirm, function (result) {
        if (result) {
            bootbox.prompt(_reason, function (mess) {
                if (mess === null) {
                    PK_CardController.ActionToCard("UNLOCK", url, mess);
                } else {
                    PK_CardController.ActionToCard("UNLOCK", url, mess);
                }
            });
            $(".bootbox-input-text").addClass("unlockcard");
        }
    });
}

function DeleteCardSelected(url) {
    var _deleteConfirm = $('input[name=_deleteConfirm]').val();
    bootbox.confirm(_deleteConfirm, function (result) {
        if (result) {
            bootbox.prompt(_reason, function (mess) {
                if (mess === null) {
                    PK_CardController.ActionToCard("DELETE", url, mess);
                } else {
                    PK_CardController.ActionToCard("DELETE", url, mess);
                }
            });
        }
    });
}
