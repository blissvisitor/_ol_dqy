map.on('pointermove', function(evt) {
    if (evt.dragging) {
        return;
    }
    displayFeatureInfo(evt);
});
var displayFeatureInfo = function(evt) {
    var url = fdLayer.getSource().getGetFeatureInfoUrl(evt.coordinate, map.getView().getResolution(),
        'EPSG:4326', {
            'INFO_FORMAT': 'application/json',
            'FEATURE_COUNT': 1
        });
    $.ajax({
        url: url,
        async: false,
        dataType: 'json',
        success: function(data) {
            if (data.features.length > 0) {
                var feature = data.features[0];
                var fdid = feature.properties.fdid;
                var fdno = feature.properties.fdno;
                if (!fdno) { fdno = ''; };
                //合并坟地死者姓名
                $.ajax({
                    url: "/home/getfdszname",
                    type: 'post',
                    data: { fdid: fdid },
                    dataType: 'text',
                    async: false,
                    success: function(result) {
                        content.innerHTML = '<div>坟地编号：' + fdno + '</div><br/><div>死者姓名：' + result + '</div>';
                        overlay.setPosition(feature.geometry.coordinates);
                    }
                })

            } else {
                content.innerHTML = '';
                overlay.setPosition(undefined);
            }
        }
    })
};