var HomeController = {
    init: function () {

    },
    loadViewVehicleInOut: function (dateFill) {
        $.ajax({
            url: '/Home/ReportVehicleInOut',
            type: 'GET',
            data: {
                dateFill: dateFill,
            },
            success: function (response) {
                $('#boxVehicleInOut').html('');
                $('#boxVehicleInOut').html(response);

                var totalVehicleContractIn = $("#boxVehicleInOut").find("#TotalVehicleContractIn").val();
                var totalVehicleNoContractIn = $("#boxVehicleInOut").find("#TotalVehicleNoContractIn").val();
                var totalVehicleCommand = $("#boxVehicleInOut").find("#TotalVehicleCommand").val();
                var totalVehicleNoCommand = $("#boxVehicleInOut").find("#TotalVehicleNoCommand").val();
                var totalVehicleNoContractOut = $("#boxVehicleInOut").find("#TotalVehicleNoContractOut").val();
                var total = $("#boxVehicleInOut").find("#Total").val();

                HomeController.loadDataVehicleInOut(totalVehicleContractIn, totalVehicleNoContractIn, totalVehicleCommand, totalVehicleNoCommand, totalVehicleNoContractOut, total);

                $('#boxVehicleInOut input[id="dateFill"]').daterangepicker({
                    'applyClass': 'btn-sm btn-success',
                    'cancelClass': 'btn-sm btn-default',
                    autoUpdateInput: true,
                    locale: {
                        applyLabel: 'Apply',
                        cancelLabel: 'Cancel',
                        format: 'DD/MM/YYYY'
                    },
                    showDropdowns: true,
                    ranges: {
                        'Hôm nay': [moment(), moment()],
                        'Hôm trước': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                        '7 ngày trước': [moment().subtract(6, 'days'), moment()],
                        '30 ngày trước': [moment().subtract(29, 'days'), moment()],
                        'Tháng này': [moment().startOf('month'), moment().endOf('month')],
                        'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                    }
                });
                $('input[id="dateFill"]').on('apply.daterangepicker', function (ev, picker) {
                    $(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));
                });

                $('input[id="dateFill"]').on('cancel.daterangepicker', function (ev, picker) {
                    $(this).val('');
                });
            }
        })
    },
    loadDataVehicleInOut: function (totalVehicleContractIn, totalVehicleNoContractIn, totalVehicleCommand, totalVehicleNoCommand, totalVehicleNoContractOut, total) {
        $.ajax({
            url: '/Home/CountingVehicleInOut',
            data: {
                totalVehicleContractIn: totalVehicleContractIn,
                totalVehicleNoContractIn: totalVehicleNoContractIn,
                totalVehicleCommand: totalVehicleCommand,
                totalVehicleNoCommand: totalVehicleNoCommand,
                totalVehicleNoContractOut, totalVehicleNoContractOut,
                total: total
            },
            type: 'POST',
            dataType: 'json',
            success: function (data) {
                var placeholder = $('#boxVehicleInOut #piechart-placeholder').css({ 'width': '90%', 'min-height': '150px' });
                HomeController.configChart(placeholder, data);
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadViewVehicleOnTotal: function (dateFill) {
        $.ajax({
            url: '/Home/TotalVehiclePrice',
            type: 'GET',
            data: {
                dateFill: dateFill,
            },
            success: function (response) {
                $('#boxTotalVehiclePrice').html('');
                $('#boxTotalVehiclePrice').html(response);

                var totalVehicleContract = $("#boxTotalVehiclePrice").find("#TotalVehicleContract").val();
                var totalVehicleNoContract = $("#boxTotalVehiclePrice").find("#TotalVehicleNoContract").val();
                var total = $("#boxTotalVehiclePrice").find("#Total").val();

                HomeController.loadDataVehicleOnTotal(totalVehicleContract, totalVehicleNoContract, total);

                $('#boxTotalVehiclePrice input[id="dateFill"]').daterangepicker({
                    'applyClass': 'btn-sm btn-success',
                    'cancelClass': 'btn-sm btn-default',
                    autoUpdateInput: true,
                    locale: {
                        applyLabel: 'Apply',
                        cancelLabel: 'Cancel',
                        format: 'DD/MM/YYYY'
                    },
                    showDropdowns: true,
                    ranges: {
                        'Hôm nay': [moment(), moment()],
                        'Hôm trước': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                        '7 ngày trước': [moment().subtract(6, 'days'), moment()],
                        '30 ngày trước': [moment().subtract(29, 'days'), moment()],
                        'Tháng này': [moment().startOf('month'), moment().endOf('month')],
                        'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                    }
                });
            }
        })
    },
    loadDataVehicleOnTotal: function (totalVehicleContract, totalVehicleNoContract, total) {
        $.ajax({
            url: '/Home/CountVehiclePrice',
            data: {
                moneyVehicleContract: totalVehicleContract,
                moneyVehicleNoContract: totalVehicleNoContract,
                total: total
            },
            type: 'POST',
            dataType: 'json',
            success: function (data) {
                var placeholder = $('#boxTotalVehiclePrice #piechart-placeholder').css({ 'width': '90%', 'min-height': '150px' });
                HomeController.configChart(placeholder, data);
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    configChart: function (placeholder, data, position) {
        $.plot(placeholder, data, {
            series: {
                pie: {
                    show: true,
                    tilt: 0.8,
                    highlight: {
                        opacity: 0.25
                    },
                    stroke: {
                        color: '#fff',
                        width: 2
                    },
                    startAngle: 2
                }
            },
            legend: {
                show: true,
                position: position || "ne",
                labelBoxBorderColor: null,
                margin: [-30, 15]
            }
            ,
            grid: {
                hoverable: true,
                clickable: true
            }
        });

        //pie chart tooltip example
        var $tooltip = $("<div class='tooltip top in'><div class='tooltip-inner'></div></div>").hide().appendTo('body');
        var previousPoint = null;

        placeholder.on('plothover', function (event, pos, item) {
            if (item) {
                if (previousPoint != item.seriesIndex) {
                    previousPoint = item.seriesIndex;
                    var tip = item.series['label'] + " : " + item.series['percent'] + '%';
                    $tooltip.show().children(0).text(tip);
                }
                $tooltip.css({ top: pos.pageY + 10, left: pos.pageX + 10 });
            } else {
                $tooltip.hide();
                previousPoint = null;
            }

        });

        /////////////////////////////////////
        $(document).one('ajaxloadstart.page', function (e) {
            $tooltip.remove();
        });
    },
}

function SearchDate() {
    var date = $('#boxVehicleInOut #dateFill').val();
    HomeController.loadViewVehicleInOut(date);
}

function SearchDateOnTotal() {
    var date = $('#boxTotalVehiclePrice #dateFill').val();
    HomeController.loadViewVehicleOnTotal(date);
}