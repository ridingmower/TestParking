var UserController = {
    RemoveAllUser: function (totalItem) {
        $.ajax({
            url: '/User/RemoveAllUserSeleted',
            data: {},
            type: 'json',
            async: true,
            success: function (data) {
                UserController.loadModalButton(totalItem);

                $("#simple-table input[type=checkbox]").prop('checked', false);

                $("#simple-table tr").removeClass('info');

                $("#simple-table td").removeClass('info');
            }
        });
    },
    AddRemoveUserChoice: function (choices, totalItem) {
        $.ajax({
            url: '/User/AddOrRemoveOneAllUserSeleted',
            data: { lUsers: choices },
            type: 'json',
            async: true,
            success: function (data) {
                UserController.loadModalButton(totalItem);
            }
        });
    },
    loadModalButton: function (totalItem, url) {
        $.ajax({
            url: '/User/ModalButtonControl',
            type: 'GET',
            data: {
                totalItem: totalItem,
                url: url
            },
            success: function (response) {
                $('#boxUserAction').html('');

                $('#boxUserAction').html(response);
            }
        });
    },
    ActionToUser: function (type, url) {
        var roles = $("#boxUserAction").find("#roles").val();

        $.ajax({
            url: '/User/ActionToUsers',
            data: { type: type, roles: roles },
            type: 'json',
            async: true,
            success: function (data) {
                if (data.isSuccess) {
                    toastr.success(data.Message);

                    window.location.href = url;
                } else {
                    toastr.error(data.Message);
                }
            }
        });
    }
};

function AuthorizeUserSelected(total, url) {
    bootbox.confirm($('input[name=_Role_User]').val(), function (result) {
        if (result) {
            UserController.ActionToUser("Authorize", url);
        }
    });
}

function RemoveAllSelectedUser(total, url) {
    UserController.RemoveAllUser(total, url);
}