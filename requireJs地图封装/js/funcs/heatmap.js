define(['jquery', 'ol', 'mapContent', 'olmap/modules/themeticMap/theme'], function($, ol, mapContent, theme) {
    var on = 0;

    function init(ID) {
        $('#' + ID).click(function() {
            if (on == 0) {
                on = 1;
                //显示热力图
                addHeatMap();
            } else {
                on = 0;
                //关闭热力图
                removeHeatMap();
            }
        });
    };
    var vector_province = theme.heatmap({
        source: new ol.source.Vector({
            url: 'http://10.0.0.4:8080/geoserver/sf/ows?service=WFS&version=1.0.0&request=GetFeature&typeName=sf:province_p&maxFeatures=1000&outputFormat=application%2Fjson',
            format: new ol.format.GeoJSON({})
        }),
        blur: parseInt(30, 10),
        radius: parseInt(15, 10),
        zIndex: 101
    });
    var vector_city = theme.heatmap({
        source: new ol.source.Vector({
            url: 'http://10.0.0.4:8080/geoserver/sf/ows?service=WFS&version=1.0.0&request=GetFeature&typeName=sf:city_p&maxFeatures=5000&outputFormat=application%2Fjson',
            format: new ol.format.GeoJSON({})
        }),
        blur: parseInt(15, 10),
        radius: parseInt(8, 10),
        zIndex: 110
    });
    //添加
    function addHeatMap() {
        mapContent.map.addLayer(vector_city);
    };
    //删除
    function removeHeatMap() {
        mapContent.map.removeLayer(vector_city);
    };
    var module = {
        init: init
    }
    return module;

})