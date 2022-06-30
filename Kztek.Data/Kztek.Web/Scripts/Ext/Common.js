var _prefixAccessDomain = '/Access';//Vào ra
var _prefixBMSDomain = '/BMS';//Tòa nhà
var _prefixParkingDomain = '/Parking';//Bãi xe
var _prefixLockerDomain = '/Locker';//Tủ đồ
var _prefixResidentDomain = '/Resident';//Tủ đồ

$(function () {
    $(".numbericMoney").mask("000,000,000,000,000", { reverse: true });
    $('.table thead tr th').each(function () {
        var _text = $(this).text();
        $(this).attr('title', _text.trim());
    });


    //
    var $overflow = '';
    var colorbox_params = {
        rel: 'colorbox',
        reposition: true,
        scalePhotos: true,
        scrolling: false,
        previous: '<i class="ace-icon fa fa-arrow-left"></i>',
        next: '<i class="ace-icon fa fa-arrow-right"></i>',
        close: '&times;',
        current: '{current} of {total}',
        maxWidth: '100%',
        maxHeight: '100%',
        onOpen: function () {
            $overflow = document.body.style.overflow;
            document.body.style.overflow = 'hidden';
        },
        onClosed: function () {
            document.body.style.overflow = $overflow;
        },
        onComplete: function () {
            $.colorbox.resize();
        }
    };

    $('a.img_box').colorbox({ 'photo': true });

    $('.ace-thumbnails [data-rel="colorbox"]').colorbox(colorbox_params);
    $("#cboxLoadingGraphic").html("<i class='ace-icon fa fa-spinner orange fa-spin'></i>");//let's add a custom loading icon

    //
    $(document).one('ajaxloadstart.page', function (e) {
        $('#colorbox, #cboxOverlay').remove();
    });


    $('.daterangpicker').daterangepicker({
        'applyClass': 'btn-sm btn-success',
        'cancelClass': 'btn-sm btn-default',
        autoUpdateInput: true,
        locale: {
            applyLabel: 'Apply',
            cancelLabel: 'Cancel',
            format: 'DD/MM/YYYY'
        },
        singleDatePicker: true,
        showDropdowns: true
    });


    $('.daterangtimepicker').daterangepicker({
        'applyClass': 'btn-sm btn-success',
        'cancelClass': 'btn-sm btn-default',
        autoUpdateInput: true,
        timePicker: true,
        timePickerIncrement: 1,
        timePicker24Hour: true,
        locale: {
            applyLabel: 'Apply',
            cancelLabel: 'Cancel',
            format: 'DD/MM/YYYY HH:mm'
        },
        singleDatePicker: true,
        showDropdowns: true
    });

    $('.daterangtimepicker2').daterangepicker({
        'applyClass': 'btn-sm btn-success',
        'cancelClass': 'btn-sm btn-default',
        autoUpdateInput: true,
        timePicker: true,
        timePickerIncrement: 1,
        timePicker24Hour: true,
        locale: {
            applyLabel: 'Apply',
            cancelLabel: 'Cancel',
            format: 'DD/MM/YYYY HH:mm'
        },
        singleDatePicker: false,
        showDropdowns: true
    });

    $('.daterangnoautopicker').daterangepicker({
        'applyClass': 'btn-sm btn-success',
        'cancelClass': 'btn-sm btn-default',
        autoUpdateInput: false,
        locale: {
            applyLabel: 'Apply',
            cancelLabel: 'Cancel',
            format: 'DD/MM/YYYY'
        },
        singleDatePicker: true,
        showDropdowns: true
    });


    $('.datepicker').datetimepicker({
        format: 'DD/MM/YYYY HH:mm:ss',//use this option to display seconds
        icons: {
            time: 'fa fa-clock-o',
            date: 'fa fa-calendar',
            up: 'fa fa-chevron-up',
            down: 'fa fa-chevron-down',
            previous: 'fa fa-chevron-left',
            next: 'fa fa-chevron-right',
            today: 'fa fa-arrows ',
            clear: 'fa fa-trash',
            close: 'fa fa-times'
        }
    });

    if (!ace.vars['touch']) {
        $('.chosen-select').chosen({ allow_single_deselect: true });
        //resize the chosen on window resize

        $(window)
            .off('resize.chosen')
            .on('resize.chosen', function () {
                $('.chosen-select').each(function () {
                    var $this = $(this);
                    $this.next().css({ 'width': $this.parent().width() });
                })
            }).trigger('resize.chosen');
        //resize chosen on sidebar collapse/expand
        $(document).on('settings.ace.chosen', function (e, event_name, event_val) {
            if (event_name != 'sidebar_collapsed') return;
            $('.chosen-select').each(function () {
                var $this = $(this);
                $this.next().css({ 'width': $this.parent().width() });
            })
        });
    }

    var _nonSelectedText = $('input[name=_nonSelectedText]').val();
    var _allSelectedText = $('input[name=_allSelectedText]').val();
    var _nSelectedText = $('input[name=_nSelectedText]').val();

    $('.multiselect').multiselect({
        enableFiltering: true,
        enableHTML: true,
        nonSelectedText: _nonSelectedText,
        allSelectedText: _allSelectedText,
        nSelectedText: _nSelectedText,
        numberDisplayed: 1,
        enableCaseInsensitiveFiltering: true,
        buttonClass: 'btn btn-white btn-primary',
        templates: {
            button: '<button type="button" class="multiselect dropdown-toggle" data-toggle="dropdown"><span class="multiselect-selected-text"></span> &nbsp;<b class="fa fa-caret-down"></b></button>',
            ul: '<ul class="multiselect-container dropdown-menu"></ul>',
            filter: '<li class="multiselect-item filter"><div class="input-group"><span class="input-group-addon"><i class="fa fa-search"></i></span><input class="form-control multiselect-search" type="text"></div></li>',
            filterClearBtn: '<span class="input-group-btn"><button class="btn btn-default btn-white btn-grey multiselect-clear-filter" type="button"><i class="fa fa-times-circle red2"></i></button></span>',
            li: '<li><a tabindex="0"><label></label></a></li>',
            divider: '<li class="multiselect-item divider"></li>',
            liGroup: '<li class="multiselect-item multiselect-group"><label></label></li>'
        }
    });
});

function ChosenSelect() {
    if (!ace.vars['touch']) {
        $('.chosen-select').chosen({ allow_single_deselect: true });
        //resize the chosen on window resize

        $(window)
            .off('resize.chosen')
            .on('resize.chosen', function () {
                $('.chosen-select').each(function () {
                    var $this = $(this);
                    $this.next().css({ 'width': $this.parent().width() });
                })
            }).trigger('resize.chosen');
        //resize chosen on sidebar collapse/expand
        $(document).on('settings.ace.chosen', function (e, event_name, event_val) {
            if (event_name != 'sidebar_collapsed') return;
            $('.chosen-select').each(function () {
                var $this = $(this);
                $this.next().css({ 'width': $this.parent().width() });
            })
        });
    }
}

function ConvertCharacter(str) {
    str = str.replace('&#244;', 'ô').replace('&#244;', 'ô').replace('&#244;', 'ô').replace('&#244;', 'ô').replace('&#234;', 'ê').replace('&#234;', 'ê').replace('&#234;', 'ê').replace('&#234;', 'ê').replace('&#225;', 'á').replace('&#225;', 'á').replace('&#225;', 'á').replace('&#225;', 'á').replace('&#226;', 'â').replace('&#226;', 'â').replace('&#226;', 'â').replace('&#226;', 'â').replace('&#233;', 'é').replace('&#233;', 'é').replace('&#233;', 'é').replace('&#233;', 'é').replace('&#233;', 'é').replace('&#212;', 'Ô').replace('&#212;', 'Ô').replace('&#212;', 'Ô').replace('&#212;', 'Ô').replace('&#212;', 'Ô').replace('&#224;', 'à').replace('&#224;', 'à').replace('&#224;', 'à').replace('&#224;', 'à').replace('&#224;', 'à');

    return str;
}

function JSFileUpload() {
    $('input[idata=FileUploadUserController]').ace_file_input({
        no_file: 'No File ...',
        btn_choose: 'Choose',
        btn_change: 'Change',
        droppable: false,
        onchange: null,
        thumbnail: false //| true | large
        //whitelist:'gif|png|jpg|jpeg'
        //blacklist:'exe|php'
        //onchange:''
        //
    });
}

function FormatMoney(money) {
    if (money !== "0") {
        return money.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.")
    } else {
        return "0"
    }
   
}

function formatJSONDate(jsonTime) {
    var MyDate_String_Value = jsonTime
    var value = new Date
        (
        parseInt(MyDate_String_Value.replace(/(^.*\()|([+-].*$)/g, ''))
        );
    return value;
}

function formatDateFromJson(jsonTime) {
    var dateString = jsonTime.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = ('0' + (currentTime.getMonth() + 1)).slice(-2);
    var day = ('0' + currentTime.getDate()).slice(-2);
    var year = currentTime.getFullYear();
    //var hour = currentTime.getHours();
    //var minute = currentTime.getMinutes();
    var date = day + "/" + month + "/" + year;

    return date;
}

function formatDateFromJson1(jsonTime) {
    var dateString = jsonTime.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = ('0' + (currentTime.getMonth() + 1)).slice(-2);
    var day = ('0' + currentTime.getDate()).slice(-2);
    var year = currentTime.getFullYear();
    var hour = currentTime.getHours();
    var minute = currentTime.getMinutes();
    var date = day + "/" + month + "/" + year + " " + hour + ":" + minute;

    return date;
}

function SearchSubmit(formName) {
    $('button[name=btnFilter]').click(function () {
        $('#chkExport').val('0');
        $('#' + formName).submit();
    });
}

function ExcelSubmit(formName) {
    $('button[name=btnExport]').click(function () {
        $('#chkExport').val('1');
        $('#' + formName).submit();
    });
}

function DeleteSubmit(url) {
    var deleteConfirm = $('input[name=_deleteConfirm]').val();
    var _noti = $('input[name=_noti]').val();
    $('.btnDelete').click(function () {
        var cmd = $(this);
        var _id = cmd.attr('idata');

        bootbox.confirm( deleteConfirm, function (result) {
            if (result) {
                $.ajax({
                    url: url,
                    data: { id: _id },
                    type: 'json',
                    //async:false,
                    success: function (data) {
                        if (data.isSuccess) {
                            cmd.parent().parent().parent().fadeOut();
                            toastr.success(data.Message, _noti);
                        } else {
                            toastr.error(data.Message, _noti);
                        }
                    }
                });
            }
        });
    });
}

function GetDateDDMMYYYY() {
    var d = new Date();

    var curr_hours = d.getHours();

    var curr_minutes = d.getMinutes();

    var curr_seconds = d.getSeconds();

    var curr_date = d.getDate();

    var curr_month = d.getMonth();

    var curr_year = d.getFullYear();

    return curr_date + "/" + curr_month + "/" + curr_year + ":" + curr_hours + ":" + curr_minutes + ":" + curr_seconds;
}
