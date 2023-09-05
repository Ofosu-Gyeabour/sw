Ext.onReady(function () {

    var adm = Ext.get('adm');

    adm.on('click', function () {

        var msg = {
            title: 'UNDER DEVELOPMENT',
            msg: 'Admin module is currently under development',
            icon: Ext.MessageBox.INFO,
            buttons: Ext.Msg.OK
        };

        Ext.Msg.show(msg);

    });

});