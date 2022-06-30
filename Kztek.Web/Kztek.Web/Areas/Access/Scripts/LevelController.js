var LevelController = {
    init: function () {

    },
    registerEvent: function () {

    },
    loadDataDoor: function (selected, objId, isUpdateAble) {
        $.ajax({
            url: _prefixAccessDomain + '/tblLevel/GetListDoor',
            type: 'POST',
            data: {
                selected: selected,
                objId: objId,
                isUpdateable: isUpdateAble
            },
            success: function (response) {
                $('#boxDoor').html('');
                $('#boxDoor').html(response);

                var demo = $('#boxDoor #cbDoorList').bootstrapDualListbox({
                    nonselectedlistlabel: 'Non-selected',
                    selectedlistlabel: 'Selected',
                    preserveselectiononmove: 'moved',
                    moveonselect: false
                });

                var container = demo.bootstrapDualListbox('getContainer');
                container.find('.btn').addClass('btn-white btn-info btn-bold');
            }
        });
    },
    loadDataControllerTime: function (list, isUpdateable, objId) {
        if (isUpdateable) {
            $.ajax({
                url: _prefixAccessDomain + '/tblLevel/ControllerInDoorOnUpdate',
                type: 'POST',
                data: {
                    controllerList: list,
                    id: objId
                },
                success: function (response) {
                    $('#boxControlerTime').html('');
                    $('#boxControlerTime').html(response);

                    $('.chosen-select').chosen({ allow_single_deselect: true });
                    //resize the chosen on window resize

                    $(window)
                        .off('resize.chosen')
                        .on('resize.chosen', function () {
                            $('.chosen-select').each(function () {
                                var $this = $(this);
                                $this.next().css({ 'width': $this.parent().width() });
                            });
                        }).trigger('resize.chosen');
                    //resize chosen on sidebar collapse/expand
                    $(document).on('settings.ace.chosen', function (e, event_name, event_val) {
                        if (event_name !== 'sidebar_collapsed') return;
                        $('.chosen-select').each(function () {
                            var $this = $(this);
                            $this.next().css({ 'width': $this.parent().width() });
                        });
                    });
                }
            });
        } else {
            $.ajax({
                url: _prefixAccessDomain + '/tblLevel/ControllerInDoor',
                type: 'POST',
                data: {
                    controllerList: list
                },
                success: function (response) {
                    $('#boxControlerTime').html('');
                    $('#boxControlerTime').html(response);

                    $('.chosen-select').chosen({ allow_single_deselect: true });
                    //resize the chosen on window resize

                    $(window)
                        .off('resize.chosen')
                        .on('resize.chosen', function () {
                            $('.chosen-select').each(function () {
                                var $this = $(this);
                                $this.next().css({ 'width': $this.parent().width() });
                            });
                        }).trigger('resize.chosen');
                    //resize chosen on sidebar collapse/expand
                    $(document).on('settings.ace.chosen', function (e, event_name, event_val) {
                        if (event_name !== 'sidebar_collapsed') return;
                        $('.chosen-select').each(function () {
                            var $this = $(this);
                            $this.next().css({ 'width': $this.parent().width() });
                        });
                    });
                }
            });
        }
    },
    openModalChoiceAuth: function () {
        $("#modalChoiceAuth").modal("show");
    },
    updateAuth: function () {
        var key = $("#frmCard").find("input[name=key]").val();
        var fdate = $("#frmCard").find("#fromdate").val();
        var tdate = $("#frmCard").find("#todate").val();
        var cardstatus = $("#frmCard").find("#cardstatus").val();
        var customergroup = $("#frmCard").find("#customergroup").val();
        var cardgroup = $("#frmCard").find("#cardgroup").val();
        var columnquery = $("#frmCard").find("input[name=columnQuery]").val();

        var level = $("#modalChoiceAuth").find("#levelid").val();

        var obj = {
            key: key,
            fromdate: fdate,
            todate: tdate,
            cardstatus: cardstatus,
            customergroup: customergroup,
            cardgroup: cardgroup,
            columnQuery: columnquery
        };

        $("#modalChoiceAuth").find('.loading1').css('display', 'block');
        $("#modalChoiceAuth").find('#proccess').css('display', 'block');
        $("#modalChoiceAuth").find("#messs").css('display', 'none');

        $.ajax({
            url: _prefixAccessDomain + '/tblCard/UpdateAuthMulti',
            data: { objSer: obj, level: level },
            type: 'json',
            //async:false,
            success: function (data) {
                if (data.Success) {
                    $("#modalChoiceAuth").find('.loading1').css('display', 'none');

                    toastr.success(data.Message);

                    $("#modalChoiceAuth").modal("hide");
                } else {
                    toastr.error(data.Message);
                }
            }
        });
    },
    updateAuthCustomer: function () {
        var key = $("#frmCustomerMap").find("input[name=key]").val();
        var customerstatus = $("#frmCustomerMap").find("#customerstatus").val();
        var customergroup = $("#frmCustomerMap").find("#customergroup").val();

        var level = $("#modalChoiceAuth").find("#levelid").val();

        var obj = {
            key: key,
            cardstatus: customerstatus,
            customergroup: customergroup
        };

        $("#modalChoiceAuth").find('.loading1').css('display', 'block');
        $("#modalChoiceAuth").find('#proccess').css('display', 'block');
        $("#modalChoiceAuth").find("#messs").css('display', 'none');

        $.ajax({
            url: _prefixAccessDomain + '/AC_CustomerMap/UpdateAuthMulti',
            data: { objSer: obj, level: level },
            type: 'json',
            //async:false,
            success: function (data) {
                if (data.Success) {
                    $("#modalChoiceAuth").find('.loading1').css('display', 'none');

                    toastr.success(data.Message);

                    $("#modalChoiceAuth").modal("hide");
                } else {
                    toastr.error(data.Message);
                }
            }
        });
    }
};

function SelectedDoor(selected, objId, isUpdateAble) {
    var _val = [];

    $('#boxDoor #cbDoorList').find('option:selected').each(function () {
        _val.push($(this).attr("idata"));
    });

    LevelController.loadDataControllerTime(_val, isUpdateAble, objId);
}

//
function openModalAuth() {
    LevelController.openModalChoiceAuth();
}

function updateAuthCard() {
    LevelController.updateAuth();
}

function updateAuthCustomer() {
    LevelController.updateAuthCustomer();
}