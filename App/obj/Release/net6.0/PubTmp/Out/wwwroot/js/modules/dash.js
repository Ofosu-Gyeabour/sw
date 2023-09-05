Ext.onReady(function () {

    var dash = Ext.get('dash');

    dash.on('click', function () {

        var msg = {
            title: 'UNDER DEVELOPMENT',
            msg: 'Dashboard module is currently under development',
            icon: Ext.MessageBox.INFO,
            buttons: Ext.Msg.OK
        };

        Ext.Msg.show(msg);

    });

});