$(function () {
    $("body").on("click", ".btnPrint", function () {
        var id = $(this).attr("idata");
        var url = '/Parking/PRIDE_ActiveCard/PrintBill_v2?id=' + id + '&type=2';
        window.open(url, '_blank');

    })

    $('body').on('click', '#lpOrder tr.trItem', function () {
        var row = $(this);
        var id = row.attr('idata');
        //alert(id);
        $('tr.trHide:not(.showDetailBox-child-' + id + ')').hide();
        // nếu đã load rồi
        if (row.parent().find('.showDetailBox-child-' + id).length > 0) {

            var chk = row.parent().find('.showDetailBox-child-' + id).is(":hidden");
            if (chk) {
                row.parent().find('.showDetailBox-child-' + id).stop().fadeIn();
            } else {
                row.parent().find('.showDetailBox-child-' + id).stop().fadeOut();
            }
        } else { // nếu chưa load

            var model = {
                Id: id
            };

            $.ajax({
                url: _prefixParkingDomain + '/OrderActiveCard/DetailBox',
                type: 'GET',
                data: model,
                success: function (response) {
                    if (response !== '') {
                        if (row.parent().find('.showDetailBox-child-' + id).length <= 0) {
                            row.after(response);
                            row.parent().find('.showDetailBox-child-' + id).stop().fadeIn();
                        }
                    }
                }
            });
        }
    });

    $('body').on('click', '#lpOrderPRIDE tr.trItem', function () {
        var row = $(this);
        var id = row.attr('idata');
        //alert(id);
        $('tr.trHide:not(.showDetailBox-child-' + id + ')').hide();
        // nếu đã load rồi
        if (row.parent().find('.showDetailBox-child-' + id).length > 0) {

            var chk = row.parent().find('.showDetailBox-child-' + id).is(":hidden");
            if (chk) {
                row.parent().find('.showDetailBox-child-' + id).stop().fadeIn();
            } else {
                row.parent().find('.showDetailBox-child-' + id).stop().fadeOut();
            }
        } else { // nếu chưa load

            var model = {
                Id: id
            };

            $.ajax({
                url: _prefixParkingDomain + '/OrderActiveCard/PRIDEDetailBox',
                type: 'GET',
                data: model,
                success: function (response) {
                    if (response !== '') {
                        if (row.parent().find('.showDetailBox-child-' + id).length <= 0) {
                            row.after(response);
                            row.parent().find('.showDetailBox-child-' + id).stop().fadeIn();
                        }
                    }
                }
            });
        }
    });
});
