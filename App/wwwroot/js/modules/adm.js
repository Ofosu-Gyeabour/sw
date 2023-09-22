Ext.onReady(function () {

    var adm = Ext.get('adm');

    adm.on('click', function () {

        var admWindow = Ext.getCmp('adminWindow');

        if (!admWindow) {

            admWindow = new Ext.Window({
                id: 'adminWindow',
                title: 'SYSTEM ADMINISTRATION',
                height: 700,
                width: 1200,
                resizable: true,
                closable: true,
                collapsible: true,
                defaults: { xtype: 'panel', frame: true, border: true }, layout: 'column',
                items: [
                    {
                        title: 'Admin Parameters', columnWidth: .20, defaults: { xtype: 'treepanel', autoScroll: true, border: true, height:625 },
                        items: [
                            {
                                root: {
                                    text: 'Configuration Settings', expanded: true,
                                    children: [
                                        {
                                            text: 'Shipping Lookups', leaf: false, expanded: true,
                                            children: [
                                                {
                                                    text: 'City Lookup', leaf: true,visible: false,
                                                    listeners: {
                                                        'click': function (btn) {
                                                            Ext.getCmp('admPanel').layout.setActiveItem(0);
                                                        }
                                                    }
                                                },
                                                {
                                                    text: 'Country Lookup', leaf: true,
                                                    listeners: {
                                                        'click': function (btn) {
                                                            Ext.getCmp('admPanel').layout.setActiveItem(1);
                                                        }
                                                    }
                                                },
                                                {
                                                    text: 'Departments', leaf: true,
                                                    listeners: {
                                                        'click': function (btn) {
                                                            Ext.getCmp('admPanel').layout.setActiveItem(2);
                                                        }
                                                    }
                                                },
                                                {
                                                    text: 'Company Types', leaf: true,
                                                    listeners: {
                                                        'click': function (btn) {
                                                            Ext.getCmp('admPanel').layout.setActiveItem(3);
                                                        }
                                                    }
                                                },
                                                {
                                                    text: 'Vehicle Pool', leaf: true,
                                                    listeners: {
                                                        'click': function (btn) {
                                                            Ext.getCmp('admPanel').layout.setActiveItem(4);
                                                        }
                                                    }
                                                },
                                                /*{
                                                    text: 'Cities Lookup', leaf: true,
                                                    listeners: {
                                                        'click': function (btn) {
                                                            Ext.getCmp('admPanel').layout.setActiveItem(4);
                                                        }
                                                    }
                                                },
                                                {
                                                    text: 'Container Types', leaf: true,
                                                    listeners: {
                                                        'click': function (btn) {
                                                            Ext.getCmp('admPanel').layout.setActiveItem(5);
                                                        }
                                                    }
                                                }*/
                                            ]
                                        },
                                        
                                        /*{
                                            text: 'Company Types', leaf: true,
                                            listeners: {
                                                'click': function (btn) {
                                                    Ext.getCmp('admPanel').layout.setActiveItem(7);
                                                }
                                            }
                                        },
                                        {
                                            text: 'Operations', leaf: false,
                                            children: [
                                                {
                                                    text: 'Vehicle Pool', leaf: true,
                                                    listeners: {
                                                        'click': function (btn) {
                                                            Ext.getCmp('admPanel').layout.setActiveItem(8);
                                                        }
                                                    }
                                                }
                                            ]
                                        }*/
                                    ]
                                }
                            }
                        ]
                    },
                    {
                        columnWidth: .80, id: 'admPanel', layout: 'card', autoScroll: true, setActiveItem:0,
                        defaults: { xtype: 'panel' },
                        items: [
                            {
                                defaults: { xtype: 'panel', frame: true, border: true }, layout: 'form',
                                items: [
                                    {
                                        defaults: { xtype: 'panel', frame: true, border: true }, layout: 'column',
                                        items: [
                                            {
                                                title: 'Add City',columnWidth:.4, defaults: { xtype: 'form' },
                                                items: [
                                                    {
                                                        id: '', defaults: { xtype:'combo',forceSelection: true, allowBlank: false, mode:'local', anchor: '100%', allowBlank: false },
                                                        items: [
                                                            {
                                                                id: '', fieldLabel: 'Country',
                                                                store: '',
                                                                valueField: '', displayField:''
                                                            },
                                                            { id:'', xtype:'textfield', fieldLabel:'Add City'}
                                                        ],
                                                        buttons: [
                                                            {
                                                                id: '', text: 'Save',
                                                                handler: function () {

                                                                }
                                                            },
                                                            {
                                                                id: '', text: 'Clear City',
                                                                handler: function (btn) {

                                                                }
                                                            }
                                                        ]
                                                    }
                                                ]
                                            },
                                            {
                                                title: 'Upload',columnWidth:.6, defaults: { xtype: 'form' },
                                                items: [
                                                    {
                                                        id: 'city-upl-form', fileUpload: true, width: 500, defaults: { anchor: '85%', allowBlank: false, msgTarget: 'up' }, //bodyStyle: 'padding: 10px 10px 0 10px;', 
                                                        labelWidth: 50,
                                                        items: [
                                                            { id: 'city_file_name', xtype: 'textfield', fieldLabel: 'Name' },
                                                            {
                                                                id: '', xtype: 'fileuploadfield', fieldLabel: 'File', name: 'specie-name', buttonText: '...', buttonCfg: {
                                                                    iconCls: 'upload-icon'
                                                                }
                                                            }
                                                        ],
                                                        buttons: [
                                                            {
                                                                id: '', text: 'Save City',
                                                                handler: function () {
                                                                    var fp = Ext.getCmp('city-upl-form');
                                                                    if (fp.getForm().isValid()) {
                                                                        fp.getForm().submit({
                                                                            url: '',
                                                                            waitMsg: 'Uploading city file...',
                                                                            standardSubmit: true,
                                                                            params: { caption: Ext.fly('city_file_name').getValue() },
                                                                            success: function (form, action) {
                                                                                var data = JSON.parse(action.response.responseText);
                                                                                if (data.success.toString() == "true") {
                                                                                    //loadSPECIEPreviewData(data.good, Ext.getCmp('grdSpecieDtaPreview'));
                                                                                }
                                                                            },
                                                                            failure: function (form, action) {
                                                                                var data = JSON.parse(action.response.responseText);
                                                                                Ext.Msg.alert('SAVE SPECIE STATUS', 'An error occured', this);
                                                                            }
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                        ]
                                                    }
                                                ]
                                            }
                                        ]
                                    },
                                    { title: 'City List', height:450 }
                                ]
                            },
                            { title: 'Country settings' },
                            { title: 'Departments' },
                            { title: 'Company Types' },
                            { title: 'Vehicle Pool' },
                            { title: 'Active Item Five' },
                            { title: 'Active Item Six' },
                            { title: 'Active Item Seven' },
                            { title: 'Active Item Eight' }
                        ]                          
                    }
                ]
            }).show();
        }

    });

});