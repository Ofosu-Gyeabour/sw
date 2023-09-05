Ext.onReady(function () {

    var pwd_reset = Ext.get('pwd_reset');

    pwd_reset.on('click', function () {

        var msg = {
            title: 'UNDER DEVELOPMENT',
            msg: 'Password reset module is currently under development',
            icon: Ext.MessageBox.INFO,
            buttons: Ext.Msg.OK
        };

        Ext.Msg.show(msg);

    });

});