Ext.onReady(function () {

    var officeA = Ext.get('officeA');

    officeA.on('click', function () {

        var msg = {
            title: 'UNDER DEVELOPMENT',
            msg: 'Office Area module is currently under development',
            icon: Ext.MessageBox.INFO,
            buttons: Ext.Msg.OK
        };

        Ext.Msg.show(msg);

    });

});