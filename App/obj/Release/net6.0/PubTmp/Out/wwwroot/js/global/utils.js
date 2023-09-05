var utils = utils || {};

utils.getActiveProfiles = function (_URLString, Knt) {
    var p = [];
    //'/User/getActiveModules
    $.getJSON(_URLString, {}, function (r) {
        if (r.status.toString() == "true") {
            $.each(r.data, function (i, d) {
                p[i] = [d.Id, d.module, d.describ];
            });
            Knt.getStore().loadData(p);
        }
    });
}

utils.getGeneralProfiles = function (pf) {
    var dta = [];
    $.getJSON('/User/getProfiles', {}, function (a) {
        
        if (a.status.toString() == "true") {
            $.each(a.msg, function (i, dt) {
                dta[i] = [dt.Id, dt.nameOfProfile];
            });
            
            pf.getStore().loadData(dta);
        }
    });
}

utils.getGeneralProfileStore = function (_urlString) {
    var profile_store = new Ext.data.Store({
        autoLoad: true, restful: false,
        url: _urlString,
        reader: new Ext.data.JsonReader({ type: 'json', root: 'msg' }, [
            { name: 'Id', type: 'int' },
            { name: 'nameOfProfile', type:'string'}
        ])
    })

    return profile_store;
}

utils.getDepartmentStore = function (urlString) {
    var department_store = new Ext.data.Store({
        autoLoad: true, restful: false,
        url: urlString,
        reader: new Ext.data.JsonReader({ type: 'json', root: 'data' }, [
            { name: 'id', type: 'int' },
            { name: 'name', type: 'string' }
        ])
    })

    return department_store;
}
utils.getUserList = function (urlString,k) {
    var ar = [];
    var adStr = ''; var actStr = ''; var logStr = '';
    $.getJSON(urlString, {}, function (rsp) {

        if (rsp.status.toString() == "true") {
            $.each(rsp.data, function (i, d) {
                ar[i] = [d.id, d.usrname, d.active, d.logged, d.isAdmin, d.profile];
            });
            
            k.getStore().removeAll();
            k.getStore().loadData(ar);
        }
    });
}

utils.getModulesForProfile = function (urlString,_profile, _widget) {
    var profile_list = [];

    $.getJSON(urlString, { _uProfile: _profile }, function (fs) {
        if (fs.status.toString() == "true") {
            $.each(fs.msg, function (i, d) {
                profile_list[i] = [d.id,d.module, d.describ];
            });

            _widget.getStore().removeAll();
            _widget.getStore().loadData(profile_list);
        }
    });
}
utils.returnGreeting = function () {
    console.log("hello there");
}