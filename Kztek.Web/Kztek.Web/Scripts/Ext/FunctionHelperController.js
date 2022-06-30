var FunctionHelperController = {
    //idrecord - Id bản ghi
    //isUpdate - phân biệt là update / create
    //classname - tên class
    //url - địa chỉ controller thực hiện lệnh
    Interface: function (idrecord, isUpdate, classname, url, type) {
        var box = $("#boxModal");
        //debugger;
        $.ajax({
            type: "GET",
            data: { id: idrecord, isUpdate: isUpdate, type: type },
            url: url,
            async: false,
            success: function (response) {
                box.html(response);
                $("#Modal_" + classname).modal("show");

                //Load lại chosen-select
                ActiveChosenSelect();

                //Load lại hàm date time range ko tự động nhập ngày
                ReloadDateRangeNoAuto();

                //Load lại hàm date time range tự động nhập ngày
                ReloadDateRangeAuto();

                ReloadDateRangeTimeAuto();

                //Load lại hàm date time range với 1 class cụ thể
                ReloadDateRangeAutoForSpecific('daterangpicker');

                //Load lại hàm scroll với 1 class cụ thể
                ReloadScrollForSpecific('tblScroll');

                //Dành cho css file Upload
                FormatInputUpload();

                //Với sự kiện UnitCode
                AutoComplete("ListUnitDefault", "hidNewUnitId", "/Product/GetListUnitDefault");

                //Với sự kiện tìm khách hàng
                AutoComplete("customer", "hidCustomerId", "/OrderProduct/GetListCustomer");

                //Với sự kiện tìm khách hàng
                AutoCompleteProduct("product", "hidInvoiceOut", "/OrderProduct/GetListProduct");

                //Datetime input mask 99/99/9999
                DateTimeInputMask('datetimeinputmask');

                //
                ReloadNumbericMoney();

                //Với sự kiện tìm khách hàng
                AutoComplete("person", "hidPersonId", "/CashBook/GetListPerson");

                //
                AutoCompleteRow('hidUnitId', "/Product/GetListUnitDefault", 'hidUnitCode');
            }
        });

    },
    //idboxmodal - Id div chứa cái modal bên trong 1 modal khác
    //idrecord - Id bản ghi
    //isUpdate - phân biệt là update / create
    //classname - tên class
    //url - địa chỉ controller thực hiện lệnh
    InterfaceSub: function (idboxmodal, idrecord, isUpdate, classname, url, type) {
        var box = $("#" + idboxmodal);

        $.ajax({
            type: "GET",
            data: { id: idrecord, isUpdate: isUpdate, type: type },
            url: url,
            success: function (response) {
                box.html(response);
                $("#Modal_" + classname).modal("show");

                //Load lại chosen-select
                $('.chosen-select').chosen({ allow_single_deselect: true, width: '100%' });

                //Datetime input mask 99/99/9999
                DateTimeInputMask('datetimeinputmask');
            }
        });

    },
    //modal giao hàng (đặt hàng controller)
    InterfaceDelivery: function (idboxmodal, idrecord,storeid, url) {
        var box = $("#" + idboxmodal);
        $.ajax({
            type: "GET",
            data: { invoiceid: idrecord, storeid: storeid},
            url: url,
            success: function (response) {
                box.html(response);
                $("#Modalship").modal("show");

                //Load lại chosen-select
                $('.chosen-select').chosen({ allow_single_deselect: true, width: '100%' });

                //Datetime input mask 99/99/9999
                DateTimeInputMask('datetimeinputmask');

                //Load lại hàm date time range với 1 class cụ thể
                ReloadDateRangeAutoForSpecific('daterangpicker');

                //
                ReloadNumbericMoney();
            }
        });

    },
    //idmodal - id của modal đó
    CloseModal: function (idmodal, isCountinue) {
        $("#" + idmodal).modal("hide");

        if (isCountinue) {
            $('.modal-backdrop').remove();
        }
    },
    //obj - Model submit lên
    //url - Địa chỉ controller
    Create: function (obj, url) {
        return $.ajax({
            type: "POST",
            datatype: "json",
            data: obj,
            url: url,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false
        });
    },
    //obj - Model submit lên
    //url - Địa chỉ controller
    Update: function (obj, url) {
        return $.ajax({
            type: "POST",
            datatype: "json",
            data: obj,
            url: url,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false
        });
    },
    //idrecord - id bản ghi muốn xóa
    //url - Địa chỉ controller
    Delete: function (idrecord, url) {
        return $.ajax({
            type: "POST",
            datatype: "json",
            data: { id: idrecord },
            url: url
        });
    },
    DeleteMulti: function (idrecord, url) {
        return $.ajax({
            type: "POST",
            datatype: "json",
            data: { lstId: idrecord },
            url: url
        });
    },
    DeleteByObj: function (obj, url) {
        return $.ajax({
            type: "POST",
            datatype: "json",
            data: obj,
            url: url
        });
    },
    LoadData: function (model, url) {
        return $.ajax({
            url: url,
            type: 'GET',
            data: model
        });
    },
    LoadDataJson: function (model, url) {
        return $.ajax({
            type: "POST",
            datatype: "json",
            contetType: 'json',
            data: model,
            url: url
        });
    },

    //table - Id table đó
    //trclass - class tr
    //id - Id bản ghi
    JsSelectedRow: function (table, trclass, id) {
        $("#" + table).find("." + trclass).removeClass('info');
        $("#" + table).find("tr[class='" + trclass + "'][idata='" + id + "']").addClass('info');
    }
};

