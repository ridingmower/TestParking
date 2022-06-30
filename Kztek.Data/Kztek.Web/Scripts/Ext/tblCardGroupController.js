var tblCardGroupController = {
    init: function () {
    },
    registerEvent: function () {
    },
    loadData: function (type, value1, value2, value3, value4) {
        if (type === "0") {
            $.ajax({
                url: _prefixParkingDomain + '/tblCardGroup/FormulationEachTurn',
                type: 'GET',
                data: {
                    value: value1,
                },
                success: function (response) {
                    $('#boxFormulation').html('');
                    $('#boxFormulation').html(response);

                    $(".formatMoney").mask("000.000.000.000.000", { reverse: true });
                }
            })
        }
        else if (type === "1") {
            $.ajax({
                url: _prefixParkingDomain +'/tblCardGroup/FormulationBlock',
                type: 'GET',
                data: {
                    value: value4,
                },
                success: function (response) {
                    $('#boxFormulation').html('');
                    $('#boxFormulation').html(response);

                    $(".formatMoney").mask("000.000.000.000.000", { reverse: true });
                }
            })
        } else if (type === "2") {
            $.ajax({
                url: _prefixParkingDomain +'/tblCardGroup/FormulationTimePeriod',
                type: 'GET',
                data: {
                    period: value2, cost: value3
                },
                success: function (response) {
                    $('#boxFormulation').html('');
                    $('#boxFormulation').html(response);
                }
            })
        }
    },
    addNewBlockTime: function (numberRow) {
        $.ajax({
            url: _prefixParkingDomain + '/tblCardGroup/FormulationNewBlock',
            type: 'GET',
            data: {
                numberIndex: numberRow
            },
            success: function (response) {
                $('#boxFormulation').append(response);

                $(".formatMoney").mask("000.000.000.000.000", { reverse: true });
            }
        })
    }
}

function AddNewBlockTime() {
    var rows = 0;
    $("#boxFormulation").find(".form-group").each(function () {
        rows += 1;
    })

    tblCardGroupController.addNewBlockTime(rows);
}