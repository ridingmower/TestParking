$(function () {
    $('#ddlExcelColumn').change(function () {
        var cmd = $(this);
        var a = cmd.parent().find('ul.multiselect-container li:nth-child(2)');
        $(a).change(function () {
            if (a.is('.active')) {
                cmd.multiselect("selectAll", true);
                cmd.multiselect("refresh");
            }
            else {
                cmd.multiselect("deselectAll", true);
                cmd.multiselect("refresh");
            }
        });

        var str = '';
        cmd.parent().find('ul.multiselect-container li.active').each(function () {
            var _cmd = $(this);
            str += _cmd.find('input[type=checkbox]').val() + ',';
        });
        $('#excelcol').val(str);
    });


});