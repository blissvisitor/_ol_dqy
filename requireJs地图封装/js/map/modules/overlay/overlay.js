define(['ol'], function(ol) {
    
    return　 {
    	/*
        *overlay
        *return overlay
        *弹出框 document.getElementById('popup-content').innerHTML = '<table class="table table-bordered"><tbody><tr><td><b>类型</b></td><td>' + data.CRKTYPE + '</td></tr><tr><td><b>所属乡镇</b></td><td>' + data.VILLNAME + '</td></tr></tbody></table>';
        *overlay.setPosition(coordinate);
        *隐藏弹出框 document.getElementById('popup-content').innerHTML='';
        *overlay.setPosition(undefined);
        */

        overlayFunction: function() {
            // 创建弹出框的div
            var popup = document.createElement('div');
            popup.className = 'ol-popup';
            // 创建弹出来框用来显示内容的div
            var popupContent = document.createElement('div');
            popupContent.id = 'popup-content';
            // 创建弹出框右上角的关闭按钮
            var closer = document.createElement('a');
            closer.id = 'popop-closer';
            closer.className = 'ol-popup-closer';
            closer.href = '#';

            // 把div和a标签放入弹出框的div中
            popup.appendChild(closer);
            popup.appendChild(popupContent);
            var overlay = new ol.Overlay({
                element: popup,
                autoPan: true,
                autoPanAnimation: undefined
            });

            closer.onclick = function() {
                popupContent.innerHTML='';
                overlay.setPosition(undefined);
                closer.blur();
                // initMap.themeSelect.getFeatures().clear();
                return false;
            };
            return overlay;
        }
    }

})