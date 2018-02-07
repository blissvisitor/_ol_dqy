/*导出地图
依赖filesaver、mapContent、浏览器支持canvas
ID 导出按钮id
*/
define(['jquery', 'mapContent', 'ol', 'filesaver'], function($, mapContent, ol, filesaver) {
    function init(ID) {
        $("#" + ID).click(function() {
            mapContent.map.once('postcompose', function(event) {
                var canvas = event.context.canvas;
                if (navigator.msSaveBlob) {
                    navigator.msSaveBlob(canvas.msToBlob(), 'map.png');
                } else {
                    canvas.toBlob(function(blob) {
                        saveAs(blob, 'map.png');
                    });
                }
            });
            mapContent.map.renderSync();
        });
    };
    var module = {
        init: init
    };
    return module;
})