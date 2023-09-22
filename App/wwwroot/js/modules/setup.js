Ext.onReady(function () {
    
    var ubt = Ext.get('setup');


    ubt.on('click', function () {
        var xWindow = Ext.getCmp('usrmgmtWindow');
        if (!xWindow) {

            xWindow = new Ext.Window({
                id: 'usrmgmtWindow',
                title: 'USER MANAGEMENT',
                height: 700,
                width: 1200,
                collapsible: false,
                resizable: false,
                closable: true,
                defaults: { xtype: 'panel', frame: true, border: true }, layout: 'form',
                items: [
                    {
                        title: '', height: 500, defaults: { xtype: 'panel' }, layout: 'column',
                        items: [
                            {
                                title: 'User details', width: '50%', height: 480, defaults: { xtype: 'form', frame: true, border: true }, layout: 'accordion',
                                items: [
                                    {
                                        id: 'frmUsrD', title: 'Create User Account', defaults: { xtype: 'textfield', anchor: '90%', allowBlank: false },
                                        items: [
                                            { id: 'xsname', fieldLabel: 'Surname' },
                                            { id: 'xfname', fieldLabel: 'First name' },
                                            { id: 'xothernames', fieldLabel: 'Other names', allowBlank: true },
                                            { id: 'xUsr', fieldLabel: 'Username' },
                                            { id: 'xPwd', fieldLabel: 'Password', inputType: 'password' },
                                            { id: 'xPwdc', fieldLabel: 'Confirm', inputType: 'password' },
                                            {
                                                xtype: 'combo', id: 'xDpt', fieldLabel: 'Department', forceSelection: true, typeAhead: true, allowBlank: true, mode: 'local',
                                                store: utils.getDepartmentStore('/Utility/GetDepartments'),
                                                valueField: 'id', displayField: 'name'
                                            },
                                            {
                                                xtype: 'combo', id: 'xUstat', fieldLabel: 'User status', forceSelection: true, typeAhead: true, mode: 'local', store: ['ACTIVE', 'INACTIVE']
                                            },
                                            { xtype: 'combo', id: 'xAd', fieldLabel: 'Admin status', forceSelection: true, typeAhead: true, mode: 'local', store: ['YES', 'NO'] },
                                            {
                                                xtype: 'combo', id: 'uprf', fieldLabel: 'Profile', forceSelection: true, typeAhead: true, mode: 'local',
                                                store: utils.getGeneralProfileStore('/User/getProfiles'),
                                                valueField: 'id', displayField: 'nameOfProfile'
                                            },
                                        ],
                                        buttons: [
                                            {
                                                id: 'btUsrD', text: 'Save User Account',
                                                listeners: {
                                                    'click': function (btn) {
                                                        var _s = $('#xPwd').val(); var _t = $('#xPwdc').val();
                                                        if (_s.toString() == _t.toString()) {
                                                            $.post('/User/saveUserCredentials',
                                                                {
                                                                    sname: Ext.fly('xsname').getValue(), fname: Ext.fly('xfname').getValue(), onames: Ext.fly('xothernames').getValue(),
                                                                    usr: Ext.fly('xUsr').getValue(), pwd: Ext.fly('xPwd').getValue(), stat: Ext.fly('xUstat').getValue(),
                                                                    isAdm: Ext.fly('xAd').getValue(), prof: Ext.fly('uprf').getValue(), dept: Ext.getCmp('xDpt').getValue()
                                                                }, function (r) {
                                                                    if (r.status.toString()) {
                                                                        Ext.Msg.alert('USER ACCOUNT', r.message, this);
                                                                    }
                                                                }, "json");
                                                        }
                                                        else {
                                                            Ext.Msg.alert('INCORRECT PASSWORD', 'The Passwords entered are not the same', this);
                                                        }
                                                    }
                                                }
                                            },
                                            {
                                                id: 'btUsrClr', text: 'Clear User Account',
                                                listeners: {
                                                    'click': function (btn) {
                                                        Ext.getCmp('frmUsrD').getForm().reset();
                                                        $('#uprf').val('');
                                                        $('#xsname').focus();
                                                    }
                                                }
                                            }
                                        ]
                                    },
                                    {
                                        id: 'frm_pwd_reset', title: 'Reset Password', defaults: { xtype: 'textfield', anchor: '100%' }, layout: 'form',
                                        items: [
                                            { id: 'frm_p_user', fieldLabel: 'User name' },
                                            { id:'frm_p_pwd',inputType:'password', fieldLabel:'Password'}
                                        ],
                                        buttons: [
                                            {
                                                id: 'btn_usr_change',
                                                text:'Change Password',
                                                handler: function (btn) {
                                                    var _form = Ext.getCmp('frm_pwd_reset').getForm();
                                                    if (_form.isValid()) {
                                                        $.post('/User/changePassword',
                                                            { usrname: Ext.fly('frm_p_user').getValue(), pwd: Ext.fly('frm_p_pwd').getValue(),flag:'*' },
                                                            function (fs) {
                                                                if (fs.status.toString() == "true") {
                                                                    var gmsg = {
                                                                        title: 'PASSWORD CHANGE',
                                                                        msg: fs.msg,
                                                                        icon: Ext.MessageBox.INFO,
                                                                        buttons: Ext.Msg.OK
                                                                    };

                                                                    Ext.Msg.show(gmsg);
                                                                }
                                                        });
                                                    }
                                                }
                                            },
                                            {
                                                id: 'btn_usr_clr',
                                                text:'Clear',
                                                handler: function (btn) {
                                                    Ext.getCmp('frm_pwd_reset').getForm();
                                                    $('#frm_p_user').focus();
                                                }
                                            }
                                        ]
                                    },
                                    {
                                        id: 'frmUsrAmend', title: 'Amend User Profiles', defaults: { xtype: 'textfield', anchor: '100%' },
                                        items: [
                                            {
                                                id: 'amusr', fieldLabel: 'User name',
                                                listeners: {
                                                    'blur': function () {
                                                        $.getJSON('/Utility/getCurrentProfileOfUser', { u: Ext.fly('amusr').getValue() }, function (rs) {
                                                            if (rs.status.toString() == "true") {
                                                                $('#amprof').val(rs.msg.toString()).attr('readonly', 'readonly');
                                                                $('#amnprof').val('').focus();
                                                            }
                                                        });
                                                    }
                                                }
                                            },
                                            {
                                                id: 'amprof', fieldLabel: 'Current profile'
                                            },
                                            {
                                                id: 'amnprof', xtype: 'combo', fieldLabel: 'New Profile', forceSelection: true, typeAhead: true, mode: 'local',
                                                store: utils.getGeneralProfileStore('/User/getProfiles'),
                                                valueField: 'id', displayField: 'nameOfProfile',
                                                listeners: {
                                                    'select': function () {
                                                        utils.getModulesForProfile('/Utility/GetProfileModules', $('#amnprof').val(), Ext.getCmp('grdamn'));
                                                    }
                                                }
                                            },
                                            new Ext.grid.GridPanel({
                                                id: 'grdamn', title: 'MODULES IN SELECTED PROFILE', height: 200, autoScroll: true,
                                                store: new Ext.data.GroupingStore({
                                                    reader: new Ext.data.ArrayReader({}, [
                                                        { name: 'id', type: 'int' },
                                                        { name: 'module', type: 'string' },
                                                        { name: 'describ', type: 'string' }
                                                    ]),
                                                    sortInfo: {
                                                        field: "id",
                                                        direction: "ASC"
                                                    },
                                                    groupField: "describ"
                                                }),
                                                columns: [
                                                    { id: 'id', header: 'ID', width: 60, hidden: true, sortable: true, dataIndex: 'Id' },
                                                    { id: 'module', header: '', width: 100, hidden: true, sortable: true, dataIndex: 'module' },
                                                    { id: 'describ', header: '', width: 600, hidden: false, sortable: true, dataIndex: 'describ' }
                                                ], stripeRows: true
                                            })
                                        ],
                                        buttons: [
                                            {
                                                id: 'amnBtnSv', text: 'Save Amended Profile',
                                                listeners: {
                                                    'click': function (btn) {
                                                        var fr = Ext.getCmp('frmUsrAmend').getForm();
                                                        if (fr.isValid()) {
                                                            $.post('/User/AmendUserProfile',
                                                                { u: Ext.fly('amusr').getValue(), pro: Ext.fly('amnprof').getValue() },
                                                                function (rss) {
                                                                    if (rss.status.toString() == "true") {
                                                                        Ext.Msg.alert('AMEND USER PROFILE', rss.message.toString(), this);
                                                                        $('#amnBtnClr').trigger('click');
                                                                    }
                                                                });
                                                        }
                                                    }
                                                }
                                            },
                                            {
                                                id: 'amnBtnClr', text: 'Clear',
                                                listeners: {
                                                    'click': function (btn) {
                                                        Ext.getCmp('frmUsrAmend').getForm().reset();
                                                        Ext.getCmp('grdamn').getStore().removeAll();
                                                        $('#amusr').val('').focus();
                                                    }
                                                }
                                            }
                                        ]
                                    },
                                    /*
                                    {
                                        id: 'frmChngPwd', title: 'Change User Password', defaults: { xtype: 'textfield', anchor: '90%' },
                                        items: [
                                            { id: 'cusr', fieldLabel: 'User Account' },
                                            { id: 'cpwd', fieldLabel: 'New Password', inputType: 'password' },
                                            { id: 'cpwdconfirm', fieldLabel: 'Confirmation', inputType: 'password' }
                                        ],
                                        buttons: [
                                            {
                                                id: 'cbtnSv', text: 'Save',
                                                listeners: {
                                                    'click': function (btn) {
                                                        var pwd = Ext.fly('cpwd').getValue();
                                                        var pwdC = Ext.fly('cpwdconfirm').getValue();
                                                        if (pwd != pwdC) {
                                                            Ext.Msg.alert('PASSWORD ERROR', 'Passwords entered do NOT match', this);
                                                            return false;
                                                        }

                                                        var f = Ext.getCmp('frmChngPwd').getForm();
                                                        if (f.isValid()) {
                                                            $.post('/User/amendUserCredentials',
                                                                { usr: Ext.fly('cusr').getValue(), pw: Ext.fly('cpwd').getValue() },
                                                                function (r) {
                                                                    if (r.status.toString() == "true") {
                                                                        Ext.Msg.alert('PASSWORD RESET', 'Password has been reset for user', this);
                                                                        $('#cbtnClr').trigger('click');
                                                                    }
                                                                }, "json");
                                                        }
                                                    }
                                                }
                                            },
                                            {
                                                id: 'cbtnClr', text: 'Clear',
                                                listeners: {
                                                    'click': function (btn) {
                                                        Ext.getCmp('frmChngPwd').getForm().reset();
                                                        $('#cusr').focus();
                                                    }
                                                }
                                            }
                                        ]
                                    },

                                    {
                                        id: 'frmChgSwitchUsr', title: 'Switch User Access', defaults: { xtype: 'textfield', allowBlank: false, anchor: '90%' },
                                        items: [
                                            { id: 'swUsr', fieldLabel: 'User' },
                                            { xtype: 'combo', id: 'swMeans', fieldLabel: 'Means of Access', mode: 'local', store: ['Active Directory', 'Normal Authentication'] }
                                        ],
                                        buttons: [
                                            {
                                                id: 'switchBtnSv', text: 'Save',
                                                listeners: {
                                                    'click': function (btn) {
                                                        var f = Ext.getCmp('frmChgSwitchUsr').getForm();
                                                        if (f.isValid()) {
                                                            $.post('/User/switchAccessMode',
                                                                { u: Ext.fly('swUsr').getValue(), access: Ext.fly('swMeans').getValue() },
                                                                function (rsp) {
                                                                    if (rsp.status.toString() == "true") {
                                                                        Ext.Msg.alert('SWITCH USER ACCESS', 'Access to system switched for user', this);
                                                                        $('#switchBtnClr').trigger('click');
                                                                    }
                                                                }, "json");
                                                        }
                                                    }
                                                }
                                            },
                                            {
                                                id: 'switchBtnClr', text: 'Clear',
                                                listeners: {
                                                    'click': function (btn) {
                                                        Ext.getCmp('frmChgSwitchUsr').getForm().reset();
                                                        $('#swUsr').focus();
                                                    }
                                                }
                                            }
                                        ]
                                    }
                                    */
                                ]
                            },
                            {
                                title: 'Profile Configuration', width: '50%', defaults: { xtype: 'tabpanel', tabPosition: 'top', enableScroll: true, frame: true, height: 500 },
                                items: [
                                    {
                                        activeTab: 0,
                                        items: [
                                            {
                                                id: '', title: 'User Profiles', defaults: { xtype: 'form', frame: true }, layout: 'form',
                                                items: [
                                                    {
                                                        title: '', defaults: { xtype: 'form', frame: true, border: true },
                                                        items: [
                                                            new Ext.grid.GridPanel({
                                                                id: 'grdPr', title: '', height: 400, autoScroll: true,
                                                                store: new Ext.data.GroupingStore({
                                                                    reader: new Ext.data.ArrayReader({}, [
                                                                        { name: 'Id', type: 'int' },
                                                                        { name: 'nameOfProfile', type: 'string' }
                                                                    ]),
                                                                    sortInfo: {
                                                                        field: "Id",
                                                                        direction: "ASC"
                                                                    },
                                                                    groupField: "nameOfProfile"
                                                                }),
                                                                columns: [
                                                                    { id: 'Id', header: 'ID', width: 60, hidden: true, sortable: true, dataIndex: 'Id' },
                                                                    { id: 'nameOfProfile', header: '', width: 600, hidden: false, sortable: true, dataIndex: 'nameOfProfile' },
                                                                ], listeners: {
                                                                    'render': function () {
                                                                        utils.getGeneralProfiles(Ext.getCmp('grdPr'));
                                                                    },
                                                                    'afterrender': function () {
                                                                        setInterval(function () {
                                                                            utils.getGeneralProfiles(Ext.getCmp('grdPr'));
                                                                        }, 10000)
                                                                    }
                                                                }
                                                            })
                                                        ]
                                                    }
                                                ]
                                            },
                                            {
                                                id: '', title: 'New User Profile', defaults: { xtype: 'panel', frame: true, border: true, height: 230 }, layout: 'form',
                                                items: [
                                                    {
                                                        id: '', title: '', defaults: { xtype: 'form', frame: true, border: true }, layout: 'form',
                                                        items: [
                                                            {
                                                                id: 'frmSUsrProf', title: '', width: '50%', height: 420,
                                                                items: [
                                                                    new Ext.grid.GridPanel({
                                                                        id: 'grdNUsrProf', title: '', height: 200, autoScroll: true,
                                                                        store: new Ext.data.GroupingStore({
                                                                            restful: false,
                                                                            reader: new Ext.data.ArrayReader({}, [
                                                                                { name: 'Id', type: 'int' },
                                                                                { name: 'module', type: 'string' },
                                                                                { name: 'describ', type: 'string' }
                                                                            ]),
                                                                            sortInfo: {
                                                                                field: "Id",
                                                                                direction: "ASC"
                                                                            },
                                                                            groupField: "Id"
                                                                        }),
                                                                        columns: [
                                                                            { id: 'id', header: 'ID', width: 60, hidden: true, sortable: true, dataIndex: 'id' },
                                                                            { id: 'module', header: 'MODULE', width: 450, hidden: false, sortable: true, dataIndex: 'module' },
                                                                            { id: 'describ', header: 'MODULES', width: 60, hidden: true, sortable: true, dataIndex: 'describ' }
                                                                        ], stripeRows: true,
                                                                        listeners: {
                                                                            'afterrender': function () {
                                                                                utils.getActiveProfiles('/Utility/getActiveModules', Ext.getCmp('grdNUsrProf'))

                                                                                utils.returnGreeting();
                                                                            },
                                                                            'rowdblclick': function (e, t) {


                                                                                var pro = e.getStore().getAt(t);
                                                                                var _st = pro.get('describ').toString();
                                                                                if (Ext.getCmp('tUsrMod').getValue().length < 1) {
                                                                                    Ext.getCmp('tUsrMod').setValue(_st);
                                                                                }
                                                                                else {
                                                                                    Ext.getCmp('tUsrMod').setValue(Ext.fly('tUsrMod').getValue() + '|' + _st);
                                                                                }
                                                                                /*
                                                                                $.post('/User/storeUserProfile',
                                                                                    { id: pro.get('Id'), mod: pro.get('module'), modDescription: pro.get('describ') }, function (usr) {
                                                                                        if (usr.status.toString() == "true") {
                                                                                            var x = [];
                                                                                            $.each(usr.msg, function (i, dt) {
                                                                                                x[i] = [dt.Id, dt.module, dt.describ];
                                                                                            });
                                                                                            //var _st = Ext.getCmp('tUsrMod').getValue();
                                                                                            var _st = pro.get('module').toString();
                                                                                            if (Ext.getCmp('tUsrMod').getValue().length < 1) {
                                                                                                Ext.getCmp('tUsrMod').setValue(_st);
                                                                                            }
                                                                                            else {
                                                                                                Ext.getCmp('tUsrMod').setValue(Ext.fly('tUsrMod').getValue() + ',' + _st);
                                                                                            }
                                                                                            Ext.getCmp('grdSUsrProf').getStore().loadData();

                                                                                        }
                                                                                    }, "json");
                                                                                    */
                                                                            }
                                                                        }
                                                                    })
                                                                ]
                                                            }
                                                        ]
                                                    },
                                                    {
                                                        title: '', defaults: { xtype: 'form', frame: true, border: true }, layout: 'column', height: 150,
                                                        items: [
                                                            {
                                                                id: 'fUsrMod', width: '100%',
                                                                items: [
                                                                    { xtype: 'textfield', id: 'tUsrMod', anchor: '100%', fieldLabel: 'Module(s)', disabled: true },
                                                                    { xtype: 'textfield', id: 'tUsrProf', anchor: '100%', fieldLabel: 'Profile Name' }
                                                                ],
                                                                buttons: [
                                                                    {
                                                                        id: 'btUsrModClr',
                                                                        text: 'Clear Profile',
                                                                        listeners: {
                                                                            'click': function (btn) {
                                                                                Ext.getCmp('fUsrMod').getForm().reset();
                                                                            }
                                                                        }
                                                                    },
                                                                    {
                                                                        id: 'btUsrModSv', text: 'Save Profile',
                                                                        listeners: {
                                                                            'click': function (btn) {
                                                                                $.post('/User/saveUserProfile', { _profile: Ext.fly('tUsrProf').getValue(), _profContent: Ext.fly('tUsrMod').getValue() }, function (p) {
                                                                                    if (p.status.toString() == "true") {
                                                                                        //update the user profile grid
                                                                                        //update the profile combo box for user creation
                                                                                        Ext.Msg.alert('USER PROFILE', 'User Profile created successfully', this);
                                                                                    }
                                                                                }, "json");
                                                                            }
                                                                        }
                                                                    }
                                                                ]
                                                            }
                                                        ]
                                                    },
                                                    {
                                                        id: '', defaults: { xtype: 'button' }, layout: 'hbox',
                                                        items: [
                                                            //{ text: 'Save User Configuration' }, { text: 'Clear Fields' }, { text: 'Close Window' }
                                                        ]
                                                    }
                                                ]
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    },
                    {
                        title: 'User List', height: 200, defaults: { xtype: 'form', frame: true, border: true },
                        items: [
                            new Ext.grid.GridPanel({
                                id: 'grdUList', title: '', height: 130, autoScroll: true,
                                store: new Ext.data.GroupingStore({
                                    reader: new Ext.data.ArrayReader({}, [
                                        { name: 'id', type: 'int' },
                                        { name: 'usrname', type: 'string' },
                                        { name: 'active', type: 'string' },
                                        { name: 'logged', type: 'string' },
                                        { name: 'isAdmin', type: 'string' },
                                        { name: 'profile', type: 'string' }
                                    ]),
                                    sortInfo: {
                                        field: "id",
                                        direction: "ASC"
                                    },
                                    groupField: "usrname"
                                }),
                                columns: [
                                    { id: 'id', header: 'ID', width: 60, hidden: true, sortable: true, dataIndex: 'id' },
                                    { id: 'usrname', header: 'USERNAME', width: 250, hidden: false, sortable: true, dataIndex: 'usrname' },
                                    { id: 'active', header: 'ACTIVE', width: 250, hidden: false, sortable: false, dataIndex: 'active' },
                                    { id: 'logged', header: 'LOGGED', width: 200, hidden: false, sortable: false, dataIndex: 'logged' },
                                    { id: 'isAdmin', header: 'ADMIN', width: 200, hidden: false, sortable: false, dataIndex: 'isAdmin' },
                                    { id: 'profile', header: 'USER PROFILE', width: 250, hidden: false, sortable: false, dataIndex: 'profile' }
                                ], stripeRows: true,
                                listeners: {
                                    'render': function () {
                                        utils.getUserList('/User/GetUserList',Ext.getCmp('grdUList'));
                                    },
                                    'afterrender': function () {
                                        setInterval(utils.getUserList('/User/GetUserList',Ext.getCmp('grdUList')), 30000);
                                    }
                                }
                            })
                        ]
                    }
                ]
            }).show();
        }
        //}); //REMOVE HERE

    });
});