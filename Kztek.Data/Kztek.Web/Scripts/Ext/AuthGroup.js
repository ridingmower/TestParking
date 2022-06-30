$(function () {
    $("body").on("change", "#ddlCardGroup", function () {
        var str = '';
        var cmd = $(this);
        cmd.parent().find('ul.multiselect-container li.active').each(function () {
            var _cmd = $(this);
            str += _cmd.find('input[type=checkbox]').val() + ';';
        });
        $('#authcardgroupid').val(str);      
    })

})