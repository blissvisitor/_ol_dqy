require.config({

    baseUrl: "",
    paths: {
        "jquery": "./js/jquery-3.2.1.min", //因为jquery2.0不支持IE8 此处使用1.X版本
        'css': './js/require/css.min',
        'text': './js/require/text',
        "ztreecore": "./js/z-tree/js/jquery.ztree.core",
        "ztreeexcheck": "./js/z-tree/js/jquery.ztree.excheck",
        "layer": "./js/layer/layer",
        "pdfmake": './js/pdfmake/pdfmake.min',
        "filesaver": './js/filesaver/filesaver.min',
        //引入地图模块
        "olmap": "./js/map",
        'ol': './js/map/modules/libs/ol/ol',
        "mapContent": './js/map/mapContent'
    },
    shim: { //用于加载不符合AMD规范的JS文件
        "ztreecore": {
            deps: ['jquery', 'css!./js/z-tree/css/zTreeStyle/zTreeStyle.css'],
            exports: "ztreecore"
        },
        "ztreeexcheck": {
            deps: ['jquery', "ztreecore", 'css!./js/z-tree/css/zTreeStyle/zTreeStyle.css'],
            exports: "ztreeexcheck"
        },
        'ol': {
            deps: [
                'css!olmap/modules/libs/ol/ol.css'
            ]
        },
        'layer': {
            deps: [
                'css!./js/layer/theme/default/layer.css'
            ]
        }
    }

});
// require(['mapContent', 'olmap/modules/themeticMap/simpleExport','./js/funcs/heatmap'], function(mapContent,simpleExport,heatmap) {
    require(['mapContent', 'olmap/modules/themeticMap/simpleExport','./js/funcs/heatmap'], function(mapContent,simpleExport,heat) {
    mapContent.init('mapDiv');
    //导出地图初始化
    simpleExport.init('thematic_export');
    //热力图初始化
    heat.init('heat_map');
    console.log(mapContent);
})