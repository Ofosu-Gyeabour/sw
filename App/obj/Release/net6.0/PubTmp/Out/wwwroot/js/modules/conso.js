Ext.onReady(function () {

    var conso = Ext.get('conso');

    conso.on('click', function () {

        var msg = {
            title: 'UNDER DEVELOPMENT',
            msg: 'Consolidator module is currently under development',
            icon: Ext.MessageBox.INFO,
            buttons: Ext.Msg.OK
        };

        Ext.Msg.show(msg);

    });

});