Ext.onReady(function () {

    var pwd_reset = Ext.get('pwd_reset');

    pwd_reset.on('click', function () {

        var pwd_reset_window = Ext.getCmp('pwdResetWindow');

        if (!pwd_reset_window) {

            pwd_reset_window = new Ext.Window({
                id: 'pwdResetWindow',
                title: 'PASSWORD RESET FORM',
                height: 160,
                width: 350,
                collapsible: false,
                resizable: false,
                closable: true,
                defaults: { xtype: 'panel', frame: true },layout:'form',
                items: [
                    {
                        defaults: { xtype: 'form' }, layout: 'form',
                        items: [
                            {
                                id: 'pwd_r_form', defaults: { xtype: 'textfield', allowBlank: false, anchor: '95%' }, layout: 'form',
                                items: [
                                    { id: 'pwd_usrname', fieldLabel: 'User name', disabled: true },
                                    { id: 'pwd_new', fieldLabel: 'New Password', inputType: 'password' },
                                    { id: 'pwd_confirm', fieldLabel: 'Confirm', inputType: 'password' }
                                ],
                                buttons: [
                                    {
                                        id: 'btn_pwd_save', text: 'Save',
                                        handler: function (btn) {
                                            var frm = Ext.getCmp('pwd_r_form').getForm();
                                            if (frm.isValid()) {
                                                if (Ext.fly('pwd_new').getValue() == Ext.fly('pwd_confirm').getValue()) {
                                                    $.post('/User/changePassword',
                                                        { usrname: Ext.fly('pwd_usrname').getValue(), pwd: Ext.fly('pwd_new').getValue(),flag:'self' }, function (fs) {
                                                            if (fs.status.toString() == "true") {
                                                                var goodmsg = {
                                                                    title: 'PASSWORD CHANGE',
                                                                    msg: fs.msg,
                                                                    icon: Ext.MessageBox.INFO,
                                                                    buttons: Ext.Msg.OK
                                                                };

                                                                Ext.Msg.show(goodmsg);
                                                            }
                                                    });
                                                }
                                                else {
                                                    var msg = {
                                                        title: 'MISMATCHING PASSWORDS',
                                                        msg: 'Passwords inputted do NOT match',
                                                        icon: Ext.MessageBox.WARNING,
                                                        buttons: Ext.Msg.OK
                                                    };
                                                    $('#btn_pwd_clr').trigger('click');
                                                    Ext.Msg.show(msg);
                                                    
                                                }
                                            }
                                        }
                                    },
                                    {
                                        id: 'btn_pwd_clr', text: 'Clear',
                                        handler: function (btn) {
                                            $('#pwd_confirm').val('');
                                            $('#pwd_new').val('').focus();
                                        }
                                    }
                                ],
                                listeners: {
                                    'render': function () {
                                        $.getJSON('/Utility/GetCurrentUser', {}, function (fs) {
                                            if (fs.status.toString() == "true") {
                                                Ext.getCmp('pwd_usrname').setValue(fs.dta.usrname);
                                            }

                                            $('#pwd_new').focus();
                                        })
                                    }
                                }
                            }
                            
                        ]
                        
                    }
                ]
            }).show();

        }

    });

});