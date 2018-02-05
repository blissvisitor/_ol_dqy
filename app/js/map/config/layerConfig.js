//图层配置  
//level图层控制级别 2两级 1一级
define(['ol'], function(ol) {
    var layers = {
        basicLayers: [{
                'type': 'tile', //图层类型
                'label': '行政区划', //图层名称(标注)
                'layerName': 'xzqhLayer', //图层名称(名称)不需要改
                'visible': true, //是否显示 
                'zIndex': 1, //层级
                'source': {
                    'type': 'XYZ',
                    'url': 'http://www.google.cn/maps/vt/pb=!1m4!1m3!1i{z}!2i{x}!3i{y}!2m3!1e0!2sm!3i345013117!3m8!2szh-CN!3scn!5e1105!12m4!1e68!2m2!1sset!2sRoadmap!4e0', //地址
                    'crossOrigin': 'Anonymous', //跨域
                    'logo': false
                }
            },
            {
                'type': 'tile', //图层类型
                'label': '影像图', //图层名称(标注)
                'layerName': 'rasterLayer', //图层名称(名称) 不需要改
                'visible': false, //是否显示 
                'zIndex': 2, //层级
                'source': {
                    'type': 'XYZ', //类型
                    'url': 'http://mt2.google.cn/vt/lyrs=y&hl=zh-CN&gl=CN&src=app&x={x}&y={y}&z={z}&s=G', //地址
                    'crossOrigin': 'Anonymous', //跨域
                    'logo': false
                }
            }
        ],
        Layers: [
        // {
        //         'level': 2,
        //         'label': '中国行政区',
        //         'layerName': 'chinaLayer',
        //         'layers': [{
        //                 'type': 'image', //图层类型
        //                 'label': '省', //图层名称(标注)
        //                 'layerName': 'provinceLayer', //图层名称(名称)
        //                 'visible': true, //是否显示 
        //                 'zIndex': 3, //层级
        //                 'source': {
        //                     'type': 'ImageWMS', //类型
        //                     'format': 'image/png', //image请求类型
        //                     'version': '1.1.1',
        //                     'styles': '', //样式
        //                     'url': '', //地址
        //                     'layers': 'sf:province', //图层
        //                     'crossOrigin': 'Anonymous', //跨域
        //                     'logo': false
        //                 }
        //             },
        //             {
        //                 'type': 'image', //图层类型
        //                 'label': '市', //图层名称(标注)
        //                 'layerName': 'cityLayer', //图层名称(名称)
        //                 'visible': true, //是否显示 
        //                 'zIndex': 4, //层级
        //                 'source': {
        //                     'type': 'ImageWMS', //类型
        //                     'format': 'image/png', //image请求类型
        //                     'version': '1.1.1',
        //                     'styles': '', //样式
        //                     'url': '', //地址
        //                     'layers': 'dx:city', //图层
        //                     'crossOrigin': 'Anonymous', //跨域
        //                     'logo': false
        //                 }
        //             }
        //         ]
        //     },
            {
                'level': 1,
                'type': 'image', //图层类型
                'label': '市', //图层名称(标注)
                'layerName': 'cityLayer', //图层名称(名称)
                'visible': true, //是否显示 
                'zIndex': 4, //层级
                'source': {
                    'type': 'ImageWMS', //类型
                    'format': 'image/png', //image请求类型
                    'version': '1.1.1',
                    'styles': '', //样式
                    'url': '', //地址
                    'layers': 'dx:city', //图层
                    'crossOrigin': 'Anonymous', //跨域
                    'logo': false
                }

            }
        ]

        // //叠加图层
        // Layers: [{ //矢量图层
        //         'type': 'vector', //图层类型
        //         'style': undefined, //style样式
        //         'label': '' ,//图层名称(标注)
        //         'layerName': '', //图层名称(名称)
        //         'visible': true, //是否显示 
        //         'zIndex': '', //层级
        //         'source': {
        //             'format': undefined, //feature format used by the XHR feature loader when url is set. 
        //             'url': undefined, //地址
        //             'features': [],
        //             'crossOrigin': 'Anonymous' //跨域
        //         }
        //     },
        //     //栅格图层
        //     {
        //         'type': 'image', //图层类型
        //         'label': '' ,//图层名称(标注)
        //         'layerName': '', //图层名称(名称)
        //         'visible': true, //是否显示 
        //         'zIndex': '', //层级
        //         'source': {
        //             'type': 'ImageWMS', //类型
        //             'format': 'image/png', //image请求类型
        //             'version': '1.1.1',
        //             'styles': '', //样式
        //             'url': '', //地址
        //             'layers': '', //图层
        //             'crossOrigin': 'Anonymous', //跨域
        //             'logo': false
        //         }
        //     },
        //     //切片 sourceXYZ
        //     {
        //         'type': 'tile', //图层类型
        //         'visible': true, //是否显示 
        //         'label': '', //图层名称(标注)
        //         'layerName': '', //图层名称(名称)
        //         'zIndex': '', //层级
        //         'source': {
        //             'type': 'XYZ', //类型
        //             'url': '', //地址
        //             'projection': 'epsg:3857', //坐标系统
        //             'maxZoom': 18, //最大缩放级别
        //             'minZoom': 0, //最小缩放级别
        //             'tileGrid': undefined, //ol.tilegrid.TileGrid
        //             'crossOrigin': 'Anonymous' //跨域
        //         }
        //     },
        //     //切片 TileImage
        //     {
        //         'type': 'tile', //图层类型
        //         'visible': true, //是否显示 
        //         'label': '' ,//图层名称(标注)
        //         'layerName': '', //图层名称(名称)
        //         'zIndex': '', //层级
        //         'source': {
        //             'type': 'TileImage', //类型
        //             'url': undefined, //地址             
        //             'tileGrid': undefined, //ol.tilegrid.TileGrid
        //             'tileUrlFunction': undefined, //   ol.TileLoadFunctionType Optional function to get tile URL given a tile coordinate and the projection.
        //             'crossOrigin': 'Anonymous' //跨域
        //         }
        //     },
        //     //切片 TileWMS
        //     {
        //         'type': 'tile', //图层类型
        //         'visible': true, //是否显示 
        //         'label': '' ,//图层名称(标注)
        //         'layerName': '', //图层名称(名称)
        //         'zIndex': '', //层级
        //         'source': {
        //             'type': 'TileWMS', //类型
        //             'projection': 'epsg:3857', //坐标系统
        //             'format': 'image/png', //image请求类型
        //             'version': '1.1.1',
        //             'styles': '', //样式
        //             'url': undefined, //地址wms 服务地址
        //             'layers': '', //图层
        //             'tileGrid': undefined, //ol.tilegrid.TileGrid
        //             'tileLoadFunction': undefined, //ol.TileLoadFunctionType |
        //             'crossOrigin': 'Anonymous' //跨域
        //         }
        //     }
        // ]
    };
    return layers;
})