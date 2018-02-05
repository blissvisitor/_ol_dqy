//ol 样式
define(['ol', 'map/modules/style/chartstyle'], function(ol,chartstyle) {
    var module = {

    };
    /*marker
     *param
     *{'anchor':[],'src':''}
     *src 图标地址
     * anchor icon center 图标中心点[0.5,0.5]
     *rotateWithView 是否根据地图进行旋转
     * rotation 旋转弧度
     *自定义图标
     */
    function Marker(param) {
        var style = new ol.style.Style({
            image: new ol.style.Icon({
                src: param.src ? param.src : '../img/marker/flag3_24x24.png',
                anchor: param.anchor !== undefined ? param.anchor : [0.5, 1],
                crossOrigin: 'Anonymous',
                rotateWithView:param.rotateWithView!==undefined?param.rotateWithView:false,
                rotation:param.rotation!==undefined?param.rotation:0
            }),
            zIndex: param.zIndex ? param.zIndex : 100
        });
        return style;
    };
    /*Circle圆
     *param
     *{'strokeColor':'','strokeWidth':2,'fillColor':'','radius':10}
     *zIndex
     */
    function Circle(param) {
        return new ol.style.Circle({
            fill: new ol.style.Fill({ color: param.fillColor ? param.fillColor : '#ffcc33' }),
            radius: param.radius ? param.radius : 10,
            stroke: new ol.style.Stroke({ color: param.strokeColor ? param.strokeColor : '#ffcc33', width: param.strokeWidth ? param.strokeWidth : 1 }),
            snapToPixel: param.snapToPixel !== undefined ? param.snapToPixel : true
        })

    }
    /*regularshape
     *param
     *type 1:square 2:triangle 3:star 4:cross 5:x
     *radius 半径
     *fillColor,strokeColor,strokeWidth
     *{'strokeColor':'','strokeWidth':2,'fillColor':'','type':1,'radius':10}
     *自定义图标
     */
    function RegularShape(param) {
        var fill = new ol.style.Fill({ color: param.fillColor });
        var stroke = new ol.style.Stroke({ color: param.strokeColor, width: param.strokeWidth });
        var image;
        switch (param.type) {
            case '1':
                image = new ol.style.RegularShape({
                    fill: fill,
                    stroke: stroke,
                    points: 4,
                    radius: param.radius ? param.radius : 10,
                    angle: Math.PI / 4
                });
                break;
            case '2':
                image = new ol.style.RegularShape({
                    fill: fill,
                    stroke: stroke,
                    points: 3,
                    radius: param.radius ? param.radius : 10,
                    rotation: Math.PI / 4,
                    angle: 0
                });
                break;
            case '3':
                image = new ol.style.RegularShape({
                    fill: fill,
                    stroke: stroke,
                    points: 5,
                    radius: param.radius ? param.radius : 10,
                    radius2: 4,
                    angle: 0
                });
                break;
            case '4':
                image = new ol.style.RegularShape({
                    fill: fill,
                    stroke: stroke,
                    points: 4,
                    radius: param.radius ? param.radius : 10,
                    radius2: 0,
                    angle: 0
                });
                break;
            case '5':
                image = new ol.style.RegularShape({
                    fill: fill,
                    stroke: stroke,
                    points: 4,
                    radius: param.radius ? param.radius : 10,
                    radius2: 0,
                    angle: Math.PI / 4
                });
                break;

        }
        return new ol.style.Style({
            image: image
        })
    }

    /*linestring
     *param 线
     *{'strokeColor':'','strokeWidth':2}
     */
    function LineString(param) {
        var style = new ol.style.Style({
            stroke: new ol.style.Stroke({
                color: param.strokeColor ? param.strokeColor : '#2E87DF',
                width: param.strokeWidth ? param.strokeWidth : 2,
                lineCap: param.lineCap ? param.lineCap : 'round', //Line cap style: butt, round, or square
                lineJoin: param.lineJoin ? param.lineJoin : 'round', //Line join style: bevel, round, or miter
                lineDash: param.lineDash ? param.lineDash : undefined,
                lineDashOffset: param.lineDashOffset ? param.lineDashOffset : 'O',
                miterLimit: param.miterLimit ? param.miterLimit : 10
            }),
            zIndex: param.zIndex ? param.zIndex : 100
        });
        return style;
    };
    /*polygon
     *param 面
     *{'strokeColor':'','strokeWidth':2,'fillColor':''}
     */
    function Polygon(param) {
        var style = new ol.style.Style({
            stroke: new ol.style.Stroke({
                color: param.strokeColor ? param.strokeColor : '#2E87DF',
                width: param.strokeWidth ? param.strokeWidth : 2,
                lineCap: param.lineCap ? param.lineCap : 'round', //Line cap style: butt, round, or square
                lineJoin: param.lineJoin ? param.lineJoin : 'round', //Line join style: bevel, round, or miter
                lineDash: param.lineDash ? param.lineDash : undefined,
                lineDashOffset: param.lineDashOffset ? param.lineDashOffset : 'O',
                miterLimit: param.miterLimit ? param.miterLimit : 10
            }),
            fill: new ol.style.Fill({
                color: param.fillColor ? param.fillColor : '#1CBE6D'
            }),
            zIndex: param.zIndex ? param.zIndex : 100
        });
        return style;
    };
    /*Text 样式设置文字标注
     *param 
     *style 样式
     *text样式参数 text文字
     */
    function Text(param) {
        var text = new ol.style.Text({
            font: param.font ? param.font : '10px sans-serif',
            maxAngle: param.maxAngle ? param.maxAngle : Math.PI / 4,
            offsetX: param.offsetX ? param.offsetX : 0,
            offsetY: param.offsetY ? param.offsetY : 0,
            overflow: param.overflow ? param.overflow : false,
            placement: param.placement ? param.placement : 'point', //line point
            rotateWithView: param.rotateWithView ? param.rotateWithView : false,
            rotation: param.rotation ? param.rotation : 0,
            text: param.text ? param.text : '',
            textAlign: param.textAlign ? param.textAlign : 'center', //. Possible values: 'left', 'right', 'center', 'end' or 'start'. Default is 'center' for placement: 'point'. For placement: 'line', the default is to let the renderer choose a placement where maxAngle is not exceeded.
            textBaseline: param.textBaseline ? param.textBaseline : 'middle', // 'bottom', 'top', 'middle', 'alphabetic', 'hanging', 'ideographic'.
            fill: new ol.style.Fill({ color: param.fillColor ? param.fillColor : '#333' }),
            stroke: new ol.style.Stroke({ color: param.strokeColor ? param.strokeColor : '#333', width: param.strokeWidth ? param.strokeWidth : 1 }),
            padding: param.padding ? param.padding : [0, 0, 0, 0]
        });
        param.style.setText(text);
        return param.style;
    };
 /*Chart 统计图 style
     *param 
     *type 统计图类别donut pie3D pie bar  
     *colors 颜色方案"classic","dark","pale","pastel","neon"
     * feature属性需要设置例如：sum:98总数,data:[12,32,42,12]
     */

    // Style function
    var styleCache = {};

    function getFeatureStyle(feature, sel,param) {
        if(!param){
            param={};
        }
        var k = $("#graph").val() + "-" + $("#color").val() + "-" + (sel ? "1-" : "") + feature.get("data");
        var style = styleCache[k];
        if (!style) {
            var radius = 5;
            // area proportional to data size: s=PI*r^2
            radius = radius * Math.sqrt(feature.get("sum") / Math.PI);
            var data = feature.get("data");
            radius *= (sel ? 1.2 : 1);
            // Create chart style
            style = [new ol.style.Style({
                image: new ol.style.Chart({
                    type: param.type||"pie",
                    radius: radius,
                    data: data,
                    rotateWithView: true,
                    stroke: new ol.style.Stroke({
                        color: "#fff",
                        width: 2
                    }),
                    colors:param.colors||''
                })
            })];

            // Show values on select


            if (sel) {
                /*
                            var sum = 0;
                            for (var i=0; i<data.length; i++)
                            {   sum += data[i];
                            }
                            */
                var sum = feature.get("sum");

                var s = 0;
                for (var i = 0; i < data.length; i++) {
                    var d = data[i];
                    var a = (2 * s + d) / sum * Math.PI - Math.PI / 2;
                    var v = Math.round(d / sum * 1000);
                    if (v > 0) {
                        style.push(new ol.style.Style({
                            text: new ol.style.Text({
                                text: d+' ('+(v / 10) + "%)",
                                /* d.toString() */
                                offsetX: Math.cos(a) * (radius + 3),
                                offsetY: Math.sin(a) * (radius + 3),
                                textAlign: (a < Math.PI / 2 ? "left" : "right"),
                                textBaseline: "middle",
                                stroke: new ol.style.Stroke({ color: "#fff", width: 2.5 }),
                                fill: new ol.style.Fill({ color: "#333" })
                            })
                        }));
                    }
                    s += d;
                }
            }
        }
        styleCache[k] = style;
        return style;
    }
    var module = {
        'Marker': Marker,
        'Circle': Circle,
        'RugularShape': RegularShape,
        'LineString': LineString,
        'Polygon': Polygon,
        'Text': Text,
        'getFeatureStyle':getFeatureStyle
    };
    return module;

})