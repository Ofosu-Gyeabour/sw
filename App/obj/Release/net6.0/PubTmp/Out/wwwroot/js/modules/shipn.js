Ext.onReady(function () {

    var shipn = Ext.get('shipn');

    shipn.on('click', function () {

        var msg = {
            title: 'UNDER DEVELOPMENT',
            msg: 'Shipping module is currently under development',
            icon: Ext.MessageBox.INFO,
            buttons: Ext.Msg.OK
        };

        Ext.Msg.show(msg);

    });

});