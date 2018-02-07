define(['ol'], function(ol) {
    /*
     *聚合图层
     *param
     *source 聚合图层数据源
     *distance 聚合距离
     *returns 聚合图层，添加到map中即可
     */
    function Cluster(param) {
        var distance = param.distance;
        distance = parseInt(param.distance, 10);
        if (isNaN(distance)) {
            console.log('聚合距离参数错误！');
            return null;
        }
        var clusterSource = new ol.source.Cluster({
            distance: distance,
            source: source
        });
        var styleCache = {};
        var clusters = new ol.layer.Vector({
            source: clusterSource,
            style: function(feature) {
                var size = feature.get('features').length;
                var style = styleCache[size];
                if (!style) {
                    style = new ol.style.Style({
                        image: new ol.style.Circle({
                            radius: 10,
                            stroke: new ol.style.Stroke({
                                color: '#fff'
                            }),
                            fill: new ol.style.Fill({
                                color: '#3399CC'
                            })
                        }),
                        text: new ol.style.Text({
                            text: size.toString(),
                            fill: new ol.style.Fill({
                                color: '#fff'
                            })
                        })
                    });
                    styleCache[size] = style;
                }
                return style;
            }
        });
    };
    /*
     *热力图层
     *param
     *source 热力图数据源
     *gradient 色带数组
     *radius 半径
     *blur 模糊度
     *returns 热力图层，添加到map中即可
     */
    function Heatmap(param) {
        var blur = param.blur !=undefined ? param.blur : 15;
        var radius = param.radius != undefined ? param.radius : 8;
        var vector_heatmap = new ol.layer.Heatmap({
            source: param.source,
            gradient:param.gradient!=undefined?param.gradient:['#00f', '#0ff', '#0f0', '#ff0', '#f00'],
            blur: parseInt(blur, 10),
            radius: parseInt(radius, 10),
            weight:param.weight!=undefined?param.weight:'weight',
            minResolution:param.minResolution!=undefined?parseFloat(param.minResolution):undefined,
            zIndex:param.zIndex!=undefined?parseInt(param.zIndex,10):100
        });
        return vector_heatmap;
    };
    var module = {
        cluster: Cluster,
        heatmap: Heatmap
    };
    return module;
})