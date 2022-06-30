var UserAdminController = {
    RemoveAllUser: function (totalItem) {
        $.ajax({
            url: '/UserAdmin/RemoveAllUserSeleted',
            data: {},
            type: 'json',
            async: true,
            success: function (data) {
                UserAdminController.loadModalButton(totalItem);

                $("#simple-table input[type=checkbox]").prop('checked', false);

                $("#simple-table tr").removeClass('info');

                $("#simple-table td").removeClass('info');
            }
        });
    },
    AddRemoveUserChoice: function (choices, totalItem) {
        $.ajax({
            url: '/UserAdmin/AddOrRemoveOneAllUserSeleted',
            data: { lUsers: choices },
            type: 'json',
            async: true,
            success: function (data) {
                UserAdminController.loadModalButton(totalItem);
            }
        });
    },
    loadModalButton: function (totalItem, url) {
        $.ajax({
            url: '/UserAdmin/ModalButtonControl',
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
            url: '/UserAdmin/ActionToUsers',
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
            UserAdminController.ActionToUser("Authorize", url);
        }
    });
}

function RemoveAllSelectedUser(total, url) {
    UserAdminController.RemoveAllUser(total, url);
}