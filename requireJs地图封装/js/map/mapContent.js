//地图基本功能初始化
define([
    'jquery',
    'ol',
    'olmap/config/mapConfig',
    'olmap/config/layerConfig',
    'olmap/modules/measure/measuretool',
    'ztreecore',
    'ztreeexcheck',
    'text!olmap/html/map.html',
    'css!olmap/css/map.css'
], function(
    $,
    ol,
    mapConfig,
    layerConfig,
    measure,
    ztreecore,
    ztreeexcheck,
    htm,
    css
) {
    var content = {
        map: '',
        view: '',
        layers: [],
        center: mapConfig.center,
        projection: mapConfig.projection,
        zoom: mapConfig.zoom,
        minZoom: mapConfig.minZoom,
        maxZoom: mapConfig.maxZoom,
        init: init,
    };

    var t = content;
    var layerTree;

    function init(node) {
        $('#' + node).html(htm);
        $(function() {

            initMap();
            initTree();
        })
    }
    //初始化地图
    function initMap() {
        t.view = new ol.View({
            center: t.center,
            zoom: t.zoom,
            minZoom: t.minZoom ? t.minZoom : 0,
            maxZoom: t.maxZoom ? t.maxZoom : 18,
        });
        //初始化图层
        initLayersControls();
        //地图初始化
        t.map = new ol.Map({
            layers: t.layers,
            controls: ol.control.defaults({
                zoom: false,
                attribution: false
            }).extend([
                new ol.control.ScaleLine()
                // new ol.control.MeasureTool({ //测量
                //     sphereradius: 6378137, //sphereradius
                // })
            ]),
            interactions: ol.interaction.defaults({
                doubleClickZoom: false
            }).extend([
                //
            ]),
            target: document.getElementById('map'),
            view: t.view,
            logo: false
        });
        var MeasureTool = new ol.control.MeasureTool({
            sphereradius: 6378137, //sphereradius
        });
        t.map.addControl(MeasureTool);
        //初始化工具栏
        initTools();
    }
    var layerControls = [];
    var xzqhLayer, rasterLayer;
    //初始化底图
    function initBasicLayers() {
        var basicLayers = layerConfig.basicLayers;
        if (basicLayers.length > 0) {
            if (basicLayers.length > 1) {
                $('#nowLayer').show();
                xzqhLayer = initLayer(basicLayers[0]);
                rasterLayer = initLayer(basicLayers[1]);
                t.layers.push(xzqhLayer);
                t.layers.push(rasterLayer);

                //底图切换
                $("#nowLayer").on("mouseover", function() {
                    $("#hideLayer").animate({ "right": "103px" }, 'fast');
                });
                $(".rastLayerChange").on("mouseleave", function() {
                    $("#hideLayer").animate({ "right": "20px" }, 'fast');
                });
                $('#hideLayer').click(function() {
                    $("#hideLayer").animate({ "right": "20px" }, 'fast');
                    var imgBack = $("#hideLayer").css('background');
                    $("#hideLayer").css("background", $("#nowLayer").css('background'));
                    $("#nowLayer").css("background", imgBack);
                    if (rasterLayer.getVisible()) {
                        rasterLayer.setVisible(false);
                        xzqhLayer.setVisible(true);
                    } else {
                        rasterLayer.setVisible(true);
                        xzqhLayer.setVisible(false);
                    }
                });
            } else {
                $('#nowLayer').hide();
                $('#hideLayer').hide();
                if (basicLayers[0].layername == 'xzqhLayer') {
                    xzqhLayer = initLayer(basicLayers[0]);
                    t.layers.push(xzqhLayer);
                } else {
                    rasterLayer = initLayer(basicLayers[0]);
                    t.layers.push(rasterLayer);
                }
            }
        } else {
            console.log('请配置底图！')
        }
    }
    //图层控制
    function initLayersControls() {
        t.layers = [];
        initBasicLayers();
        var _layers = layerConfig.Layers;
        if (_layers.length > 0) {
            _layers.forEach(function(item, index) {
                var level = item.level;
                if (level == 2) {
                    var a = {
                        name: item.label,
                        layername: item.layerName,
                        layers: []
                    }
                    _cLayers = item.layers;
                    _cLayers.forEach(function(item_1, index_1) {
                        a.layers.push({
                            name: item_1.label,
                            layername: item_1.layerName
                        });
                        t.layers.push(initLayer(item_1));
                    });
                    layerControls.push(a);
                } else if (level == 1) {


                    var a = {
                        name: item.label,
                        layername: item.layerName,
                        layers: []
                    }
                    layerControls.push(a);
                    t.layers.push(initLayer(item));
                }
            });


        } else {
            console.error('请配置图层数据');
            return;
        }
    }
    //初始化图层
    function initLayer(params) {
        var type = params.type;
        switch (type) {
            case 'vector':
                return createVectorLayer(params);
                break;
            case 'image':
                return createImageLayer(params);
                break;
            case 'tile':
                return createTileLayer(params)
                break;
        }
    }
    //初始化矢量图层
    function createVectorLayer(params) {
        var layer = new ol.layer.Vector({
            source: new ol.source.Vector({
                features: params.source.features,
                format: params.source.format,
                url: params.source.url,
                crossOrigin: params.source.crossOrigin

            }),
            style: params.style,
            visible: params.visible,
            logo: params.logo,
            zIndex: params.zIndex,
            name: params.layerName
        });

        return layer;
    }
    //初始化动态图层
    function createImageLayer(params) {
        var sourceType = params.source.type;
        var source = '';
        switch (sourceType.toLowerCase()) {
            case 'imagewms':
                source = new ol.source.ImageWMS({
                    crossOrigin: params.source.crossOrigin,
                    url: params.source.url ? params.source.url : mapConfig.serverUrl,
                    logo: params.source.logo,
                    params: {
                        'FORMAT': params.source.format,
                        'VERSION': params.source.version,
                        'LAYERS': params.source.layers,
                        'STYLES': params.source.styles
                    }
                })
                break;
            case '':

                break;
        }
        var layer = new ol.layer.Image({
            source: source,
            visible: params.visible,
            zIndex: params.zIndex,
            name: params.layerName
        });
        return layer;
    }
    //初始化切片图层
    function createTileLayer(params) {
        var sourceType = params.source.type;
        var source = '';
        switch (sourceType.toLowerCase()) {
            case 'xyz':
                source = new ol.source.XYZ({
                    crossOrigin: params.source.crossOrigin,
                    url: params.source.url
                    // projection: params.source.projection,
                    // maxZoom: params.source.maxZoom,
                    // minZoom: params.source.minZoom,
                    // tileGrid: params.source.tileGrid
                })
                break;
            case 'tileimage':
                source = new ol.source.TileImage({
                    crossOrigin: params.source.crossOrigin,
                    // url: params.source.url,
                    tileUrlFunction: params.source.tileUrlFunction,
                    tileGrid: params.source.tileGrid,
                    projection: params.source.projection
                })
                break;
            case 'tilewms':
                source = new ol.source.TileWMS({
                    crossOrigin: params.source.crossOrigin,
                    url: params.source.url ? params.source.url : mapConfig.serverUrl,
                    logo: params.source.logo,
                    projection: params.source.projection,
                    tileGrid: params.source.tileGrid,
                    tileLoadFunction: params.source.tileLoadFunction,
                    params: {
                        'FORMAT': params.source.format,
                        'VERSION': params.source.version,
                        'LAYERS': params.source.layers,
                        'STYLES': params.source.styles
                    }
                })
                break;
            case 'bingmaps':
                source = new ol.source.BingMaps({

                })
                break;
        }
        var layer = new ol.layer.Tile({
            source: source,
            visible: params.visible,
            zIndex: params.zIndex,
            name: params.layerName
        });
        return layer;
    }
    //浏览工具
    function initTools() {
        $("#commonTools ul li div").click(function() {
            var attr = $(this).attr("class");
            var currentZoom = t.view.getZoom();
            switch (attr) {
                case "zoomToExtent":
                    t.view.setCenter(t.center);
                    t.view.setZoom(t.zoom);
                    break;
                case "zoomIn":
                    currentZoom = currentZoom + 1;
                    t.view.setZoom(currentZoom);
                    break;
                case "zoomOut":
                    currentZoom = currentZoom - 1;
                    t.view.setZoom(currentZoom);
                    break;
                case "layerManage":
                    layerManage();
                    break;
            }
        });
    }

    //初始化图层管理tree
    function initTree() {
        var setting = {
            view: {
                showLine: true,
                dblClickExpand: true,
                selectedMulti: false,
                showIcon: false
                //fontCss: function(treeId, treeNode) { return { color: "#333", "font-weight": "bold", "font-size": "18px" } }
                //function(treeId, treeNode) {
                //    return treeNode.level != 1;
                //    }
            },
            check: {
                enable: true,
                //nocheckInherit: false,
                //autoCheckTrigger: false,
                chkboxType: { "Y": "ps", "N": "ps" },
                checkType: "checkbox"

            },
            callback: { //回调函数
                onCheck: zTreeOnCheck //check操作
                //onClick: zTreeonClick
                //beforeClick: beforeClick
            },
            data: {

                simpleData: {
                    enable: true
                }
            }
        };
        var idnum = null; //节点id不重复
        var zNodes = []; //z-tree json格式数据数组
        var str = {}; //存储当前目录结构{"根目录":{"子目录"：[子目录数组]}}
        for (var i = 0; i < layerControls.length; i++) { //读取监控，转换成z-tree对应格式数据
            var name = layerControls[i].name.toString(); //名称
            var layername = layerControls[i].layername.toString(); //代码
            var childLayers = layerControls[i].layers;
            //var id = data[i].gid;//唯一id
            if (!str.hasOwnProperty(name)) //如果不存在此根目录，则直接添加此类根目录及子目录
            {
                str[name] = { 'groupname': [] };
                var a = { "id": idnum + 1, "name": name, "layername": layername, checked: true, "nocheck": false, open: false, children: [] };
                for (var m = 0; m < childLayers.length; m++) {
                    var bm = childLayers[m].name;
                    var mc = childLayers[m].layername;
                    a.children.push({ "id": idnum + 2, "name": bm, "layername": mc, checked: true, open: false });
                    idnum += 2;
                    str[name].groupname.push(bm);
                }
                zNodes.push(a);

            } else {
                continue;
            }

        };
        $(document).ready(function() {

            if (layerTree) {
                $.fn.zTree.destroy("treelist");
                layerTree = '';
            }
            $.fn.zTree.init($("#treelist"), setting, zNodes);
            layerTree = $.fn.zTree.getZTreeObj("treelist");

            /*curMenu = zTree_Menu.getNodes()[0].children[0].children[0];
            zTree_Menu.selectNode(curMenu);
            treeObj.hover(function () {
                if (!treeObj.hasClass("showIcon")) {
                    treeObj.addClass("showIcon");
                }
            }, function() {
                treeObj.removeClass("showIcon");
            });*/

        });
    };

    function beforeClick(treeId, treeNode) {
        if (treeNode.level == 0) {
            var zTree = $.fn.zTree.getZTreeObj("treeDemo");
            zTree.expandNode(treeNode);
            return false;
        }
        return true;
    }

    function addDiyDom(treeId, treeNode) {
        var spaceWidth = 5;
        var switchObj = $("#" + treeNode.tId + "_switch"),
            icoObj = $("#" + treeNode.tId + "_ico");
        switchObj.remove();
        icoObj.before(switchObj);

        if (treeNode.level > 1) {
            var spaceStr = "<span style='display: inline-block;width:" + (spaceWidth * treeNode.level) + "px'></span>";
            switchObj.before(spaceStr);
        }
    }

    function zTreeOnCheck(event, treeId, treeNode) { //绑定节点check事件
        var layers = t.map.getLayers().getArray();
        if (treeNode.level == 0) {
            var children = treeNode.children;
            if (children.length > 0) {
                for (var item in children) {
                    for (var ly in layers) {
                        if (layers[ly].getProperties().name == children[item].layername) {
                            layers[ly].setVisible(treeNode.checked);
                            break;
                        }
                    }

                }
            } else {
                for (var ly in layers) {
                    if (layers[ly].getProperties().name == treeNode.layername) {
                        layers[ly].setVisible(treeNode.checked);
                        break;
                    }
                }
            }
        } else if (treeNode.level == 1) {
            for (var ly in layers) {
                if (layers[ly].getProperties().name == treeNode.layername) {
                    layers[ly].setVisible(treeNode.checked);
                    break;
                }
            }
        }
    };
    var layername, level;

    function filterNode(node) {
        return (node.level == level && node.layername == layername);
    };
    return content;
})