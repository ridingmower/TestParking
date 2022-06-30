var UploadConfig = {
    TimeInteval: 300,
    TimeOut: 300000,
    ControllerChangePageSize: false,
    ControllerPageIndex: 1,
    CardChangePageSize: false,
    CardPageIndex: 1,
    CustomerChangePageSize: false,
    CustomerPageIndex: 1
};

$(function () {
    UploadController.GetLine('');

    $("body").on("click", ".btnSort", function () {
        var columnclick = $(this).attr("idata");
        var columndefalt = $("#column").val();
        var sortdefalt = $("#sort").val();

        if (columnclick === columndefalt) {
            if (sortdefalt === "asc") {
                $("#sort").val("desc");
                $(this).find("i").removeClass("fa fa-caret-up");
                $(this).find("i").addClass("fa fa-caret-down");
            } else {
                $("#sort").val("asc");
                $(this).find("i").removeClass("fa fa-sort");
                $(this).find("i").addClass("fa fa-caret-up");
            }
        } else {
            $("#sort").val("asc");
            $(this).find("i").removeClass("fa fa-sort");
            $(this).find("i").addClass("fa fa-caret-up");
        }

        $("#column").val(columnclick);
       

        UploadController.CardLoadData();
    })

    $("body").on("change", "#ddlcomputer", function () {
        var cmd = $(this);
        var str = '';
        cmd.parent().find('ul.multiselect-container li.active').each(function () {
            var _cmd = $(this);
            str += _cmd.find('input[type=checkbox]').val() + ',';
        });

        UploadController.GetLine(str);        
    })

    $("body").on("change", "#ddlline", function () {      
        UploadController.ControllerLoadData(1);
    })

    $("body").on("change", "#groupController", function () {
        UploadController.ControllerLoadData(1);
    })

    $("body").on("change", "#ddlPageSizeCard", function () {
        UploadController.CardLoadData(true);
      
    })

    $("body").on("change", "#ddlPageSizeCustomer", function () {
       
        UploadController.CustomerLoadData(true);
    })
})

var UploadController = {
    init: function () {
        $.ajax({
            url: _prefixAccessDomain + '/Upload/GetTimeSetting',
            type: 'GET',
            data: {},
            success: function (response) {
                UploadConfig.TimeInteval = response.u1;
                UploadConfig.TimeOut = response.u2;
            }
        });
    },

    ControllerLoadData: function (changePageSize) {
        var key = $("#txtControllerAnotherKey").val();
        var computers = $("#computers").val();
        var line = $("#ddlline").val();
        var grpContronler = $("#groupController").val();
        $.ajax({
            url: _prefixAccessDomain + '/Upload/ListController',
            type: 'GET',
            data: {
                key: key,
                computerids: computers,
                line: line,
                groupControllerId: grpContronler,
                page: UploadConfig.ControllerPageIndex
            },
            success: function (response) {
                $('#tblControllers > tbody').html('');

                $('#tblControllers > tbody').html(response);

                UploadController.SelfHostLoadData();

                //Tổng số trang
                var totalPage = $("#tblControllers > tbody #pageTotal").val();

                //Tổng số bản ghi
                var number = $("#tblControllers > tbody").find("#totalItem").val();

                //Tổng số đã chọn
                var selected = $("#tblControllers > tbody").find("#selectedItem").val();

                //
                $("#ControllerPageIndex").text(UploadConfig.ControllerPageIndex);

                $("#ControllerPageCount").text(totalPage);

                $(".ControllerNumber").text(number);

                $(".ControllerSelected").text(selected);

                //
                if (totalPage !== "0") {
                    UploadController.ControllerPaging("ControllerPagination", totalPage, changePageSize);
                } else {
                    $("#ControllerPageIndex").text("0");
                    UploadController.ControllerPagingEmpty("ControllerPagination");
                }

                $('.chkCheckController').click(function () {
                    if ($(this).is(":checked")) {
                        var choices = [];
                        var cn = $(this).val();
                        choices.push(cn);

                        UploadController.ControllerAddListExtend(choices, true);
                    } else {
                        var unchoices = [];
                        var cn1 = $(this).val();
                        unchoices.push(cn1);

                        UploadController.ControllerAddListExtend(unchoices, false);
                    }
                });
            }
        });
    },
    ControllerPagingEmpty: function (name) {
        $('#' + name).empty();
        $('#' + name).removeData("twbs-pagination");
        $('#' + name).unbind("page");
    },
    ControllerPaging: function (name, totalPage, changePageSize) {
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
                UploadConfig.ControllerPageIndex = page;
                UploadController.ControllerLoadData(false);
                $("#chkCheckALLController").prop("checked", false);
            }
        });
    },
    ControllerAddListExtend: function (listId, isAdd) {
        $.ajax({
            url: _prefixAccessDomain + '/Upload/AddNewListController',
            data: { listId: listId, isAdd: isAdd },
            type: 'json',
            async: true,
            success: function (data) {
                if (data.isSuccess) {
                    UploadController.ControllerLoadData(false);
                }
            }
        });
    },
    ControllerRemoveAllExtend: function () {
        $.ajax({
            url: _prefixAccessDomain + '/Upload/DeleteAllSelectedController',
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    UploadController.ControllerLoadData(false);
                }
            }
        });
    },
    ControllerRemoveExtend: function (id) {
        $.ajax({
            url: _prefixAccessDomain + '/Upload/DeleteOneSelectedCard',
            data: { id: id },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    UploadController.ControllerLoadData(false);
                }
            }
        });
    },

    SelfHostLoadData: function () {
        $.ajax({
            url: _prefixAccessDomain + '/Upload/ListSelfHost',
            type: 'GET',
            data: {},
            success: function (response) {
                $('#ListSelfHost').html('');

                $('#ListSelfHost').html(response);
            }
        });

        $.ajax({
            url: _prefixAccessDomain + '/Upload/ListSelfHostProcess',
            type: 'GET',
            data: {},
            success: function (response) {
                $('#BoxHostCardProgress').html('');

                $('#BoxHostCardProgress').html(response);

                $('#BoxHostCustomerProgress').html('');

                $('#BoxHostCustomerProgress').html(response);
            }
        });
    },

    CardLoadData: function (changePageSize) {
        var key = $("#txtCardAnotherKey").val();
        var cardgroups = $("#cardgroups").val();
        var customergroup = $("#ddlcustomergroup").val();
        var levels = $("#levels").val();
        var column = $("#column").val();
        var sort = $("#sort").val();
        var pagesize = $("#ddlPageSizeCard").val();

        $.ajax({
            url: _prefixAccessDomain + '/Upload/ListCard',
            type: 'GET',
            data: {
                key: key,
                cardgroupids: cardgroups,
                customergroupid: customergroup,
                accesslevelids: levels,
                page: UploadConfig.CardPageIndex,
                column: column,
                sort: sort,
                pageSize: pagesize
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
                $("#CardPageIndex").text(UploadConfig.CardPageIndex);

                $("#CardPageCount").text(totalPage);

                $(".CardCount").text(number);

                $(".CardSelected").text(selected);

                $("#hidTotalCardConfirm").val(number);

                //
                if (totalPage !== "0") {
                    UploadController.CardPaging("CardPagination", totalPage, changePageSize);
                } else {
                    $("#CardPageIndex").text("0");
                    UploadController.CardPagingEmpty("CardPagination");
                }

                $('.chkCheckCard').click(function () {
                    if ($(this).is(":checked")) {
                        var choices = [];
                        var cn = $(this).val();
                        choices.push(cn);

                        var check = UploadController.GetCountListSession("1");
                        if (check) {
                            UploadController.CardAddListExtend(choices, true);
                        } else {
                            toastr.error("Số thẻ được chọn tối đa là 20!");
                            $(this).prop("checked","");
                        }
                        
                    } else {
                        var unchoices = [];
                        var cn1 = $(this).val();
                        unchoices.push(cn1);

                        UploadController.CardAddListExtend(unchoices, false);
                    }
                });
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
                UploadConfig.CardPageIndex = page;
                UploadController.CardLoadData(false);
                $("#chkCheckALLCard").prop("checked", false);
            }
        });
    },
    CardAddListExtend: function (listId, isAdd) {
        $.ajax({
            url: _prefixAccessDomain + '/Upload/AddNewListCard',
            data: { listId: listId, isAdd: isAdd },
            type: 'json',
            async: true,
            success: function (data) {
                if (data.isSuccess) {
                    UploadController.CardLoadData(false);
                }
            }
        });
    },
    CardRemoveAllExtend: function () {
        $.ajax({
            url: _prefixAccessDomain + '/Upload/DeleteAllSelectedCard',
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    UploadController.CardLoadData(false);
                }
            }
        });
    },
    CardRemoveExtend: function (id) {
        $.ajax({
            url: _prefixAccessDomain + '/Upload/DeleteOneSelectedCard',
            data: { id: id },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    UploadController.CardLoadData(false);
                }
            }
        });
    },

    UploadCardsToDevice: function (isAll) {

    },
    DeleteCardsToDevice: function (isAll) {

    },

    GetListCardWantToUse: async function(isAll, action) {
        var key = $("#txtCardAnotherKey").val();
        var cardgroups = $("#cardgroups").val();
        var customergroup = $("#ddlcustomergroup").val();
        var levels = $("#levels").val();

        //Ngày gia hạn mới
        var newDate = $("#dtpNewExpireDate").val();
        var isUseNewDate = $("#cbUseNewDate").is(":checked");

        //Get gridmodel if isAll == true.

        var model = {
            key: key,
            cardgroupids: cardgroups,
            customergroupid: customergroup,
            accesslevelids: levels,
            isall: isAll,
            newdateexpire: newDate,
            isusenewdate: isUseNewDate,
            pageIndex: 1
        };

        var data = await $.ajax({
            url: _prefixAccessDomain + '/Upload/GetListCardWantToUse',
            type: 'POST',
            async: true,
            data: {
                obj: model
            },
            
        });

        console.log(data);

        if (data.length > 0) {
            if (isAll === "true") {
                console.log("paging")

                for (var i = 1; i <= data[0].totalPage; i++) {
                    //await $.ajax({
                    //    url: _prefixAccessDomain + '/Upload/GetListCardWantToUse',
                    //    type: 'POST',
                    //    async: false,
                    //    data: {
                    //        obj: modelP
                    //    },
                    //    success: function (data) {
                    //        if (data.length > 0) {
                    //            UploadController.SendDataCard(data, action);
                    //        }
                    //    }
                    //});
                    var modelP = {
                        key: key,
                        cardgroupids: cardgroups,
                        customergroupid: customergroup,
                        accesslevelids: levels,
                        isall: isAll,
                        newdateexpire: newDate,
                        isusenewdate: isUseNewDate,
                        pageIndex: i
                    };

                    var result = await $.ajax({
                        url: _prefixAccessDomain + '/Upload/GetListCardWantToUse',
                        type: 'POST',
                        async: true,
                        data: {
                            obj: modelP
                        },
                        //success: function (data) {
                        //    if (data.length > 0) {
                        //        UploadController.SendDataCard(data, action);
                        //    }
                        //}
                    });

                    console.log("Page: " + i + " / " + data[0].totalPage);

                    if (result.length > 0) {
                        await UploadController.SendDataCard(i, data[0].pageSize, result, action, isUseNewDate, data[0].totalItem, data[0].totalController);

                        console.log('time: ' + (UploadConfig.TimeInteval * result[0].DataSend.length * result.length));

                        //await UploadController.timeout(UploadConfig.TimeInteval * result[0].DataSend.length * result.length); //thừa
                    }
                }

            } else {
                var total = data[0].DataSend.length;
                var totalController = data[0].totalController;
                await UploadController.SendDataCard(1, 20, data, action, isUseNewDate, total, totalController);
            }
        }
    },

    SendDataCard: async function (pageindex, pagesize, data, action, isUseNewDate, total,totalController) { 
        if (action === "UPLOAD") {
 
            for (var j = 0; j < data.length; j++) {
                
                for (var i = 0; i < data[j].DataSend.length; i++) {

                    await UploadController.timeout(UploadConfig.TimeInteval);

                    var count1 = ((i + 1) + (pageindex - 1) * pagesize * totalController);

                    await UploadController.UploadEvent(data[j].DataSend[i], data[j].Address, "CARD", count1, total, isUseNewDate);

                    console.log(count1 + " / " + total);
                }

                //await UploadController.timeout(UploadConfig.TimeInteval * data[j].DataSend.length);
            }
      
            //data.map(async (item, idx) => {

                

            //    //item.DataSend.map(async (dt, index) => {
                    
            //    //});
            //});
            //await data.forEach(async function (item) {
            //    var total1 = item.DataSend.length;
            //    var count1 = 0;

                
            //    await item.DataSend.forEach(async function (dt) {

                    
            //    });
                
            //    //console.log(count1 + " / " + total1);

            //    //var time1 = setInterval(function () {
                   

                    

            //    //    if (count1 === total1) {
            //    //        clearInterval(time1);
            //    //        //alert("OK");
            //    //    }

            //    //}, UploadConfig.TimeInteval);
            //});

        } else {
            for (var j = 0; j < data.length; j++) {

                for (var i = 0; i < data[j].DataSend.length; i++) {
                    await UploadController.timeout(UploadConfig.TimeInteval);

                    var count = (i + 1) + (pageindex - 1) * pagesize * totalController;

                    await UploadController.DeleteEvent(data[j].DataSend[i], data[j].Address, "CARD", count, total, isUseNewDate);

                    console.log(count + " / " + total);
                }

                await UploadController.timeout(UploadConfig.TimeInteval * data[j].DataSend.length);
            }
            //data.map(async (item, idx) => {

            //    item.DataSend.map(async (dt, idx) => {

            //        var count = idx + 1;

            //        await UploadController.DeleteEvent(dt, item.Address, "CARD", count, total, isUseNewDate);

            //        console.log(count + " / " + total);

            //    });
            //});
        }
    },
    SendDataCustomer: async function (pageindex, pagesize, data, action, isUseNewDate, total, totalController) {


        if (action === "UPLOAD") {

            for (var j = 0; j < data.length; j++) {

                for (var i = 0; i < data[j].DataSend.length; i++) {
                    await UploadController.timeout(UploadConfig.TimeInteval);

                    var count1 = (i + 1) + (pageindex - 1) * pagesize * totalController;

                    await UploadController.UploadEvent(data[j].DataSend[i], data[j].Address, "FINGER", count1, total, isUseNewDate);

                    console.log(count1 + " / " + total);
                }

                await UploadController.timeout(UploadConfig.TimeInteval * data[j].DataSend.length);
            }
            //data.map(async (item, idx) => {



            //    //item.DataSend.map(async (dt, index) => {

            //    //});
            //});
            //await data.forEach(async function (item) {
            //    var total1 = item.DataSend.length;
            //    var count1 = 0;


            //    await item.DataSend.forEach(async function (dt) {


            //    });

            //    //console.log(count1 + " / " + total1);

            //    //var time1 = setInterval(function () {




            //    //    if (count1 === total1) {
            //    //        clearInterval(time1);
            //    //        //alert("OK");
            //    //    }

            //    //}, UploadConfig.TimeInteval);
            //});

        } else {
            for (var j = 0; j < data.length; j++) {

                for (var i = 0; i < data[j].DataSend.length; i++) {
                    await UploadController.timeout(UploadConfig.TimeInteval);

                    var count = (i + 1) + (pageindex - 1) * pagesize * totalController;

                    await UploadController.DeleteEvent(data[j].DataSend[i], data[j].Address, "FINGER", count, total, isUseNewDate);

                    console.log(count + " / " + total);
                }

                await UploadController.timeout(UploadConfig.TimeInteval * data[j].DataSend.length);
            }
            //data.map(async (item, idx) => {

            //    item.DataSend.map(async (dt, idx) => {

            //        var count = idx + 1;

            //        await UploadController.DeleteEvent(dt, item.Address, "CARD", count, total, isUseNewDate);

            //        console.log(count + " / " + total);

            //    });
            //});
        }
    },

    CustomerLoadData: function (changePageSize) {
        var key = $("#txtCustomerAnotherKey").val();
        var customergroup = $("#ddlcuscustomergroup").val();
        var levels = $("#customerlevels").val();
        var pagesize = $("#ddlPageSizeCustomer").val();

        $.ajax({
            url: _prefixAccessDomain + '/Upload/ListCustomer',
            type: 'GET',
            data: {
                key: key,
                customergroupid: customergroup,
                accesslevelids: levels,
                page: UploadConfig.CustomerPageIndex,
                pageSize: pagesize
            },
            success: function (response) {
                $('#tblCustomers > tbody').html('');

                $('#tblCustomers > tbody').html(response);

                //Tổng số trang
                var totalPage = $("#tblCustomers > tbody #pageTotal").val();

                //Tổng số bản ghi
                var number = $("#tblCustomers > tbody").find("#totalItem").val();

                //Tổng số đã chọn
                var selected = $("#tblCustomers > tbody").find("#selectedItem").val();

                //
                $("#CustomerPageIndex").text(UploadConfig.CustomerPageIndex);

                $("#CustomerPageCount").text(totalPage);

                $(".CustomerCount").text(number);

                $(".CustomerSelected").text(selected);

                $("#hidTotalCustomerConfirm").val(number);

                //
                if (totalPage !== "0") {
                    UploadController.CustomerPaging("CustomerPagination", totalPage, changePageSize);
                } else {
                    $("#CustomerPageIndex").text("0");
                    UploadController.CustomerPagingEmpty("CustomerPagination");
                }

                $('.chkCheckCustomer').click(function () {
                    if ($(this).is(":checked")) {
                        var choices = [];
                        var cn = $(this).val();
                        choices.push(cn);

                        var check = UploadController.GetCountListSession("2");
                        if (check) {
                            UploadController.CustomerAddListExtend(choices, true);
                        } else {
                            toastr.error("Số vân tay được chọn tối đa là 20!");
                            $(this).prop("checked", "");
                        }
                       
                    } else {
                        var unchoices = [];
                        var cn1 = $(this).val();
                        unchoices.push(cn1);

                        UploadController.CustomerAddListExtend(unchoices, false);
                    }
                });
            }
        });
    },
    CustomerPagingEmpty: function (name) {
        $('#' + name).empty();
        $('#' + name).removeData("twbs-pagination");
        $('#' + name).unbind("page");
    },
    CustomerPaging: function (name, totalPage, changePageSize) {
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
                UploadConfig.CustomerPageIndex = page;
                UploadController.CustomerLoadData(false);
                $("#chkCheckALLCustomer").prop("checked", false);
            }
        });
    },
    CustomerAddListExtend: function (listId, isAdd) {
        $.ajax({
            url: _prefixAccessDomain + '/Upload/AddNewListCustomer',
            data: { listId: listId, isAdd: isAdd },
            type: 'json',
            async: true,
            success: function (data) {
                if (data.isSuccess) {
                    UploadController.CustomerLoadData(false);
                }
            }
        });
    },
    CustomerRemoveAllExtend: function () {
        $.ajax({
            url: _prefixAccessDomain + '/Upload/DeleteAllSelectedCustomer',
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    UploadController.CustomerLoadData(false);
                }
            }
        });
    },
    CustomerRemoveExtend: function (id) {
        $.ajax({
            url: _prefixAccessDomain + '/Upload/DeleteOneSelectedCustomer',
            data: { id: id },
            type: 'json',
            //async: true,
            success: function (data) {
                if (data.isSuccess) {
                    UploadController.CustomerLoadData(false);
                }
            }
        });
    },

    UploadCustomersToDevice: function (isAll) {

    },
    DeleteCustomersToDevice: function (isAll) {

    },

    GetListCustomerWantToUse: async function (isAll, action) {
        var key = $("#txtCustomerAnotherKey").val();
        var customergroup = $("#ddlcuscustomergroup").val();
        var levels = $("#customerlevels").val();

        //Ngày gia hạn mới
        var newDate = $("#dtpCusNewExpireDate").val();
        var isUseNewDate = $("#cbCusUseNewDate").is(":checked");

        var model = {
            key: key,
            customergroupid: customergroup,
            accesslevelids: levels,
            isall: isAll,
            newdateexpire: newDate,
            isusenewdate: isUseNewDate,
            pageIndex: 1
        };

        var data = await $.ajax({
            url: _prefixAccessDomain + '/Upload/GetListCustomerWantToUse',
            type: 'POST',
            async: true,
            data: {
                obj: model
            },

        });

        console.log(data);

        if (data.length > 0) {
            if (isAll === "true") {
                console.log("paging")

                for (var i = 1; i <= data[0].totalPage; i++) {
                    //await $.ajax({
                    //    url: _prefixAccessDomain + '/Upload/GetListCardWantToUse',
                    //    type: 'POST',
                    //    async: false,
                    //    data: {
                    //        obj: modelP
                    //    },
                    //    success: function (data) {
                    //        if (data.length > 0) {
                    //            UploadController.SendDataCard(data, action);
                    //        }
                    //    }
                    //});
                    var modelP = {
                        key: key,
                        customergroupid: customergroup,
                        accesslevelids: levels,
                        isall: isAll,
                        newdateexpire: newDate,
                        isusenewdate: isUseNewDate,
                        pageIndex: i
                    };

                    var result = await $.ajax({
                        url: _prefixAccessDomain + '/Upload/GetListCustomerWantToUse',
                        type: 'POST',
                        async: true,
                        data: {
                            obj: modelP
                        },
                        //success: function (data) {
                        //    if (data.length > 0) {
                        //        UploadController.SendDataCard(data, action);
                        //    }
                        //}
                    });

                    console.log("Page: " + i + " / " + data[0].totalPage);

                    if (result.length > 0) {
                        await UploadController.SendDataCustomer(i, data[0].pageSize, result, action, isUseNewDate, data[0].totalItem, data[0].totalController);

                        console.log('time: ' + (UploadConfig.TimeInteval * result[0].DataSend.length * result.length));

                        await UploadController.timeout(UploadConfig.TimeInteval * result[0].DataSend.length * result.length);
                    }
                }

            } else {
                var total = data[0].DataSend.length;
                var totalController = data[0].totalController;
                await UploadController.SendDataCustomer(1, 20, data, action, isUseNewDate, total, totalController);
            }
        }
    },
    
    UploadEvent: async function (obj, objhost, type, count, total, isusenewdate) {

        //if (objhost.length > 0) {
        //    $.each(objhost, function (i, item) {
                
        //    });
        //}

        await UploadController.FormatSendUpload(obj, objhost, type, count, total, isusenewdate);

    },
    DeleteEvent: async function (obj, objhost, type, count, total, isusenewdate) {

        //if (objhost.length > 0) {
        //    $.each(objhost, function (i, item) {
                
        //    });
        //}

        await UploadController.FormatSendDelete(obj, objhost, type, count, total, isusenewdate);

    },

    FormatSendUpload: async function (obj, uri, typeA, count, total, isusenewdate) {
        var urlUpload = 'http://' + uri + ':8081/api/register/upload';
        var data = await $.ajax({
            type: "POST",
            url: urlUpload,
            data: obj,
            error: function (xhr, status) {
                if (status === "timeout") {
                    console.log("Time out: " + obj.CardNumber + " - " + obj.UserIDofFinger + " - " + obj.ControllerIDs);
                }

                console.log("UPLOAD error: " + xhr);
            }
            //,
            //timeout: UploadConfig.TimeOut
        });

        if (data.length > 0) {
            $.each(data, function (i, item) {

                UploadController.writeLog(item);

                UploadController.SaveEvent(obj, item, typeA, "UPLOAD", item.Message, isusenewdate);
            });

            if (typeA === "CARD") {
                UploadController.showProgress("BoxHostCardProgress", count, total, uri);
            } else {
                UploadController.showProgress("BoxHostCustomerProgress", count, total, uri);
            }

            //if (type === "CARD") {
            //    alert(type);

            //} else {
            //    alert(type);
            //}
        } else {
            //alert("ERROR");
        }
    },
    FormatSendDelete: async function (obj, uri, typeA, count, total, isusenewdate) {
        var url = "";
        var url1 = 'http://' + uri + ':8081/api/register/delete';
        var url2 = 'http://' + uri + ':8081/api/register/deletebycontroller';

        var isDeleteCardByController = $("#chkDeleteCardsByController").is(":checked"); // True - Xóa theo url2
        var isDeleteCustomerByController = $("#chkDeleteCustomersByController").is(":checked"); // True - Xóa theo url2

        if (typeA === "CARD") {
            if (isDeleteCardByController) {
                url = url2;
            } else {
                url = url1;
            }
        } else {
            if (isDeleteCustomerByController) {
                url = url2;
            } else {
                url = url1;
            }
        }

        var data = await $.ajax({
            type: "POST",
            url: url,
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

        if (data.length > 0) {
            $.each(data, function (i, item) {

                UploadController.writeLog(item);

                UploadController.SaveEvent(obj, item, typeA, "DELETE", item.Message, isusenewdate);
            });

            if (typeA === "CARD") {
                UploadController.showProgress("BoxHostCardProgress", count, total, uri);
            } else {
                UploadController.showProgress("BoxHostCustomerProgress", count, total, uri);
            }

            //if (type === "CARD") {
            //    alert(type);

            //} else {
            //    alert(type);
            //}
        } else {
            //alert("ERROR");
        }
    },

    SaveEvent: function (obj, item, type, action, mess, isusenewdate) {
        var mo = {
            objE: obj,
            actionV: action,
            controllerid: item.ControllerID,
            eventtype: type,
            desc: mess,
            isusenewdate: isusenewdate
        };

        $.ajax({
            type: "POST",
            url: _prefixAccessDomain + '/Upload/SaveEvent',
            data: { model: mo },
            success: function (data) {

            },
            error: function (er) {
                console.log("Process upload error: " + er);
            }
        });
    },
    showProgress: function (name, count, total, address) {
        address = address.split('.').join('').replace("https://", "").replace("http://", "");
        //$("#" + name).find("#HOST_" + address).find("#ProgressBar").text(count + " / " + total);

        $.ajax({
            url: _prefixAccessDomain + '/Upload/ProgressBar',
            type: 'GET',
            data: {
                address: address,
                curr: count,
                total: total
            },
            success: function (response) {
                $("#" + name).find("#HOST_" + address).find("#ProgressBar").html(response);

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
    writeLog: function (item) {
        


        if (item.Success) {
            var size1 = $("#listsuccess").find('p').length;

            $("#listsuccess").prepend('<p> ' + (size1 + 1) + ". " + GetDateDDMMYYYY() + " - " + item.Message +  '</p>');
        } else {
            var size2 = $("#listerror").find('p').length;

            $("#listerror").prepend('<p> ' + (size2 + 1) + ". " + GetDateDDMMYYYY() + " - " + item.Message + '</p>');
        }
    },
    LoadModalConfirm: function (isAll, action, name) {
        var model = {
            isAll: isAll,
            totalItem: $("#hidTotalCardConfirm").val(),
            totalItemCus: $("#hidTotalCustomerConfirm").val(),
            actionTake: action,
            name: name
        };

        var result = FunctionHelperController.LoadData(model, '/Access/Upload/ModalConfirm');
        result.success(function (data) {
            $("#boxModalConfirm").html(data);
            $("#ModalConfirm").modal('show');
        });
    },
    GetLine: function (pcid) {
        var model = {
            pcid: pcid        
        };
        var result = FunctionHelperController.LoadData(model, '/Access/Upload/PartialLine');
        result.success(function (data) {
            $("#boxLine").html('');
            $("#boxLine").html(data);
            $('.chosen-select').chosen({ allow_single_deselect: true, search_contains: true });          
        });
    },
    timeout: function (ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    },
    GetCountListSession: function (type) {
        var ischeck = false;
        $.ajax({
            url: _prefixAccessDomain + '/Upload/GetCountListSession',
            data: {type:type },
            type: 'json',
            async: false,
            success: function (data) {
                ischeck = data.isSuccess;               
            }
        });

        return ischeck;
    }
};

//Controller
function AddExtendController(id) {
    UploadController.ControllerAddListExtend(id);
}

function RemoveAllExtendController() {
    UploadController.ControllerRemoveAllExtend();
}

function RemoveExtendController(id) {
    UploadController.ControllerRemoveExtend(id);
}

function SearchInFromListController() {
    UploadController.ControllerLoadData(true);
}

function ReloadControllers() {
    UploadController.ControllerLoadData(true);
}

//Card
function AddExtendCard(id) {
    UploadController.CardAddListExtend(id);
}

function RemoveAllExtendCard() {
    UploadController.CardRemoveAllExtend();
}

function RemoveExtendCard(id) {
    UploadController.CardRemoveExtend(id);
}

function SearchInFromListCard() {
    UploadController.CardLoadData(true);
}

function UploadCards(isAll) {

    UploadController.LoadModalConfirm(isAll, 'nạp', 'thẻ');

    //UploadController.GetListCardWantToUse(isAll, "UPLOAD");
}

function DeleteCards(isAll) {
    UploadController.LoadModalConfirm(isAll, 'hủy', 'thẻ');

    //UploadController.GetListCardWantToUse(isAll, "DELETE");
}

//Customer
function AddExtendCustomer(id) {
    UploadController.CustomerAddListExtend(id);
}

function RemoveAllExtendCustomer() {
    UploadController.CustomerRemoveAllExtend();
}

function RemoveExtendCustomer(id) {
    UploadController.CustomerRemoveExtend(id);
}

function SearchInFromListCustomer() {
    UploadController.CustomerLoadData(true);
}

function UploadCustomers(isAll) {
    UploadController.LoadModalConfirm(isAll, 'nạp', 'vân tay khách hàng');
}

function DeleteCustomers(isAll) {
    UploadController.LoadModalConfirm(isAll, 'hủy', 'vân tay khách hàng');
}

function UploadConfirm(type, isAll, action) {
    $("#ModalConfirm").modal('hide');

    if (type === "CARD") {
        UploadController.GetListCardWantToUse(isAll, action);
    } else {
        UploadController.GetListCustomerWantToUse(isAll, action);
    }
}
