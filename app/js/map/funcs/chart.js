//统计专题图demo
define(['jquery',
    'ol',
    'mapContent',
    'map/modules/style/style'
], function($, ol, mapContent, style) {
    
    var map = mapContent.map;    
    return {
        init: function() {
            $(function() {
                var map = mapContent.map;
                //ready
                // 30 random features with data: array of 4 values
                var ext = map.getView().calculateExtent(map.getSize());
                var features = [];
                for (var i = 0; i < 30; ++i) {
                    var n, nb = 0,
                        data = [];
                    for (var k = 0; k < 6; k++) {
                        n = Math.round(8 * Math.random());
                        data.push(n);
                        nb += n;
                    }
                    features[i] = new ol.Feature({
                        geometry: new ol.geom.Point([ext[0] + (ext[2] - ext[0]) * Math.random(), ext[1] + (ext[3] - ext[1]) * Math.random()]),
                        data: data,
                        sum: nb
                    });
                }
                var vector = new ol.layer.Vector({
                    name: 'Vecteur',
                    source: new ol.source.Vector({ features: features }),
                    // y ordering
                    // renderOrder: ol.ordering.yOrdering(),
                    style: function(f) { return style.getFeatureStyle(f,false); },
                    zIndex:200
                })

                map.addLayer(vector);

                // Control Select 
                var select = new ol.interaction.Select({
                    style: function(f) { return style.getFeatureStyle(f, true); }
                });
                map.addInteraction(select);
            })


        },
        destroy: function() {

        }
    }
})