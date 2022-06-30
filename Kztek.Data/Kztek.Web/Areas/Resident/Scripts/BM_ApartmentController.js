$(function () {
    //danh sách thành viên
    BM_ApartmentController.LoadChooseResident(1);

    //danh sách dịch vụ
    BM_ApartmentController.LoadChooseService(1);

    //1.mở modal thành viên
    $("body").on("click", "button[name=btnModalSearch]", function () {
        BM_ApartmentController.OpenModal("/Resident/BM_Apartment/ModalSearch", "", "Modal_Search", "boxModal");
    })

    //2.tìm kiếm
    $("body").on("click", "button[name=btnSearchResident]", function () {
        BM_ApartmentController.LoadSearchResident(1);

    })

    //3.click phân trang thành viên 
    $('body').on('click', '#AjaxPagingResident .pagination li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');
        var ps = $('.cssPagingSelect').val();

        if (ps === null || ps === '') {
            ps = '@paramHeader.pageSize';
        }

        BM_ApartmentController.LoadSearchResident(_page);

        return false;
    });

    //4.click phân trang thành viên
    $('body').on('click', '#AjaxPagingChoose .pagination li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');
        var ps = $('.cssPagingSelect').val();

        if (ps === null || ps === '') {
            ps = '@paramHeader.pageSize';
        }

        BM_ApartmentController.LoadChooseResident(_page);

        return false;
    });

    //5.check all thành viên
    $('body').on('click', '#chkAllHeader', function () {

        var str = '';

        $("#lpSearchResident tbody").find("tr").each(function () {
            var cmd = $(this);
            var chk = cmd.find(".chkItem");
            var id = chk.val();
            str += id + ",";

        });

        if (str !== '' && str !== null && str !== 'undefined') {
            BM_ApartmentController.CheckResident(str, $(this).is(":checked"), 1);
        }
    });

    //6.check từng cái thành viên
    $('body').on('click', '.chkItem', function () {
        var cmd = $(this);
        var str = cmd.val();
        if (str !== '' && str !== null && str !== 'undefined') {
            BM_ApartmentController.CheckResident(str, $(this).is(":checked"), 2);
        }
    });

    //7.lưu đã chọn thành viên
    $('body').on('click', '#btnCompletedSearch', function () {
        $("#Modal_Search").modal("hide");
        BM_ApartmentController.AddToListChoose();
    });

    //8.đóng modal tìm kiếm thành viên
    $('body').on('click', '#btnCloseSearch', function () {
        $("#Modal_Search").modal("hide");
        $("#strEmployee").val("");
    });

    //9.xóa thành viên
    $('body').on('click', '.btnDeleteChoose', function () {
        var _data = $(this).attr("idata");
        if (confirm('Bạn chắc chắn muốn xóa bản ghi này?')) {
            BM_ApartmentController.RemoveResident(_data);
        }

    });

    //10.mở modal search dv
    $("body").on("click", "button[name=btnModalSearchService]", function () {
        BM_ApartmentController.OpenModal("/Resident/BM_Apartment/ModalSearchService", "", "Modal_SearchService", "boxModal");
    })

    //11.tìm kiếm
    $("body").on("click", "button[name=btnSearchService]", function () {
        BM_ApartmentController.LoadSearchService(1);

    })

    //12.click phân trang tìm kiếm dv
    $('body').on('click', '#AjaxPagingService .pagination li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');
        var ps = $('.cssPagingSelect').val();

        if (ps === null || ps === '') {
            ps = '@paramHeader.pageSize';
        }

        BM_ApartmentController.LoadSearchService(_page);

        return false;
    });

    //13.click phân trang danh sách dv
    $('body').on('click', '#AjaxPagingChooseService .pagination li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');
        var ps = $('.cssPagingSelect').val();

        if (ps === null || ps === '') {
            ps = '@paramHeader.pageSize';
        }

        BM_ApartmentController.LoadChooseService(_page);

        return false;
    });

    //14.check all dịch vụ
    $('body').on('click', '#chkAllHeaderService', function () {

        var str = '';

        $("#lpSearchService tbody").find("tr").each(function () {
            var cmd = $(this);
            var chk = cmd.find(".chkItem");
            var id = chk.val();
            str += id + ",";

        });

        if (str !== '' && str !== null && str !== 'undefined') {
            BM_ApartmentController.CheckService(str, $(this).is(":checked"), 1);
        }
    });

    //15.check từng cái dịch vụ
    $('body').on('click', '.chkItemService', function () {
        var cmd = $(this);
        var str = cmd.val();
        if (str !== '' && str !== null && str !== 'undefined') {
            BM_ApartmentController.CheckService(str, $(this).is(":checked"), 2);
        }
    });

    //16.lưu dịch vụ đã chọn
    $('body').on('click', '#btnCompletedSearchService', function () {
        $("#Modal_SearchService").modal("hide");
        BM_ApartmentController.AddServiceToListChoose();
    });

    //17.đóng modal tìm kiếm dịch vụ
    $('body').on('click', '#btnCloseSearchService', function () {
        $("#Modal_SearchService").modal("hide");
        $("#strService").val("");
    });

    //18.xóa dịch vụ
    $('body').on('click', '.btnDeleteService', function () {
        var _data = $(this).attr("idata");
        if (confirm('Bạn chắc chắn muốn xóa bản ghi này?')) {
            BM_ApartmentController.RemoveService(_data);
        }

    });

    //19.thay đổi tòa thì thay đổi tầng
    $("body").on("change", "#BuildingId", function () {
        var buildingid = $(this).val();
        $.ajax({
            url: '/Resident/BM_Apartment/Partial_ListFloor',
            data: { buildingid: buildingid },
            type: 'POST',
            success: function (data) {
                $('#boxFloor').html('');
                $('#boxFloor').html(data);
                $('.chosen-select').chosen({ allow_single_deselect: true, width: '100%', search_contains: true });
            }
        });
    })

    //19.thay đổi tầng
    $("body").on("change", "#DDLFloor", function () {
        var id = $(this).val();
        if (id !== '0') {
            $("#strfloor").val(id);
        } else {
            $("#strfloor").val('');
        }
             
    })

})

var BM_ApartmentController = {
    OpenModal: function (url, _id, modalid, boxid) {
        $.ajax({
            type: "GET",
            data: { id: _id },
            url: url,
            async: false,
            success: function (response) {
                $("#" + boxid).html(response);
                $("#" + modalid).modal("show");
                ChosenSelect();
                //MultiSelect();
                //$("#contractor_chosen").css("width", "179px");

            }
        });
    },
    LoadSearchResident: function (page) {
        var frm = $("#frmSearch");
        var obj = {
            key: frm.find("input[name=txtKey]").val(),
            group: frm.find("#regroup").val(),
            page: page,
            employees: $("#strEmployee").val()
        };

        $.ajax({
            type: "GET",
            data: obj,
            url: '/Resident/BM_Apartment/Partial_SearchResident',
            async: false,
            success: function (response) {
                $("#divPadding").html("");
                $("#divPadding").html(response);

            }
        });
    },
    LoadChooseResident: function (page) {
        var strEmployeeChoose = $("#strEmployeeChoose").val();
        var status = $("#statusvalue").val();
        $.ajax({
            type: "GET",
            data: { strEmployeeChoose: strEmployeeChoose, status: status, page: page },
            url: '/Resident/BM_Apartment/Partial_ChooseResident',
            async: false,
            success: function (response) {
                $("#divPaddingChoose").html("");
                $("#divPaddingChoose").html(response);

            }
        });
    },
    CheckResident: function (strId, check, type) {
        var strEmployee = $("#strEmployee").val();
        $.ajax({
            url: '/Resident/BM_Apartment/ChooseResident',
            data: { strIds: strId, isCheck: check, strEmployee: strEmployee, type: type },
            type: 'json',
            async: true,
            success: function (data) {
                $("#strEmployee").val(data);
            }
        });
    },
    AddToListChoose: function () {
        var strEmployee = $("#strEmployee").val();
        var strEmployeeChoose = $("#strEmployeeChoose").val();

        $.ajax({
            url: '/Resident/BM_Apartment/AddToListChoose',
            data: { strEmployee: strEmployee, strEmployeeChoose: strEmployeeChoose, quantity: $("#Quantity").val() },
            type: 'json',
            async: true,
            success: function (data) {
                $("#strEmployeeChoose").val(data.strEmployeeChoose);
                $("#strEmployee").val("");
                BM_ApartmentController.LoadChooseResident(1);

                if (data.message.isSuccess) {
                    toastr.success(data.message.Message);
                } else {
                    toastr.error(data.message.Message);
                }
            }
        });
    },
    RemoveResident: function (id) {
        var strEmployeeChoose = $("#strEmployeeChoose").val();
        $.ajax({
            url: '/Resident/BM_Apartment/RemoveResident',
            data: { id: id, strEmployeeChoose: strEmployeeChoose },
            type: 'json',
            async: true,
            success: function (data) {
                $("#strEmployeeChoose").val(data);

                var page = $("#AjaxPagingChoose .pagination li.active a").text();
                BM_ApartmentController.LoadChooseResident(page);

                toastr.success("Xóa thành công");
            }
        });
    },
    LoadSearchService: function (page) {
        var frm = $("#frmSearchService");
        var obj = {
            key: frm.find("input[name=txtKey]").val(),        
            page: page,
            services: $("#strService").val()
        };

        $.ajax({
            type: "GET",
            data: obj,
            url: '/Resident/BM_Apartment/Partial_SearchService',
            async: false,
            success: function (response) {
                $("#divPaddingService").html("");
                $("#divPaddingService").html(response);

            }
        });
    },
    LoadChooseService: function (page) {
        var strServiceChoose = $("#strServiceChoose").val();
        var status = $("#statusvalue").val();
        $.ajax({
            type: "GET",
            data: { strServiceChoose: strServiceChoose, status: status, page: page },
            url: '/Resident/BM_Apartment/Partial_ChooseService',
            async: false,
            success: function (response) {
                $("#divPaddingChooseService").html("");
                $("#divPaddingChooseService").html(response);

            }
        });
    },
    CheckService: function (strId, check, type) {
        var strService = $("#strService").val();
        $.ajax({
            url: '/Resident/BM_Apartment/ChooseService',
            data: { strIds: strId, isCheck: check, strService: strService, type: type },
            type: 'json',
            async: true,
            success: function (data) {
                $("#strService").val(data);
            }
        });
    },
    AddServiceToListChoose: function () {
        var strService = $("#strService").val();
        var strServiceChoose = $("#strServiceChoose").val();

        $.ajax({
            url: '/Resident/BM_Apartment/AddServiceToListChoose',
            data: { strService: strService, strServiceChoose: strServiceChoose },
            type: 'json',
            async: true,
            success: function (data) {
                $("#strServiceChoose").val(data.strServiceChoose);
                $("#strService").val("");
                BM_ApartmentController.LoadChooseService(1);

                if (data.message.isSuccess) {
                    toastr.success(data.message.Message);
                } else {
                    toastr.error(data.message.Message);
                }
            }
        });
    },
    RemoveService: function (id) {
        var strServiceChoose = $("#strServiceChoose").val();
        $.ajax({
            url: '/Resident/BM_Apartment/RemoveService',
            data: { id: id, strServiceChoose: strServiceChoose },
            type: 'json',
            async: true,
            success: function (data) {
                $("#strServiceChoose").val(data);

                var page = $("#AjaxPagingChooseService .pagination li.active a").text();
                BM_ApartmentController.LoadChooseService(page);

                toastr.success("Xóa thành công");
            }
        });
    },
}
