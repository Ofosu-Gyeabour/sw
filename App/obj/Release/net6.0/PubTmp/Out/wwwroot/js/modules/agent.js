Ext.onReady(function () {

    var agent = Ext.get('agent');

    agent.on('click', function () {

        var msg = {
            title: 'UNDER DEVELOPMENT',
            msg: 'Agent module is currently under development',
            icon: Ext.MessageBox.INFO,
            buttons: Ext.Msg.OK
        };

        Ext.Msg.show(msg);

    });

});