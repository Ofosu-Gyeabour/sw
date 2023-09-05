$('#logoff').bind('click', function (e) {

    e.preventDefault();

    var pfx = '..';

    Ext.MessageBox.confirm('Log off?', 'Are you sure you want to log off?', function (btn) {
        if (btn == 'yes') {
            $.post(pfx + '/User/Logout', {}, function (response) {
                if (response.status.toString() == "true") {
                    window.location = '/';
                }
                else {
                    window.location = '/';
                }
            });
        }
    });

});