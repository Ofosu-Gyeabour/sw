var access = access || {};

access.GetAssignedModules = function (URL, icons) {
    //alert('loading...');
    var _data;
	var pfx = '~';
    var icons_count = icons.length;
    $.getJSON(URL, {}, function (res) {
        $.each(res, function (idx, _value) {
            //iterating over the controls
            $.each(icons, function (i, ctrl) {
                var control = $(this);
                if (i <= icons_count - 1) {
                    if (control.hasClass(_value)) {
                        $.getScript('../js/modules/' + _value + '.js');
                        control.show();
                    }
                }

            });
        });
    });

    return _data;
}