Ext.onReady(function () {

    var drv = Ext.get('drv');

    drv.on('click', function () {

        var msg = {
            title: 'UNDER DEVELOPMENT',
            msg: 'Driver module is currently under development',
            icon: Ext.MessageBox.INFO,
            buttons: Ext.Msg.OK
        };

        Ext.Msg.show(msg);

    });

});