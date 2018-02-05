//绘制 编辑
define(['ol'],function(ol){
	/*绘制工具
	*param
	*source 矢量图层source
	*map map对象
	*style 绘制样式
	*type 绘制类型 'Point', 'LineString', 'LinearRing', 'Polygon', 'MultiPoint', 'MultiLineString', 'MultiPolygon', 'GeometryCollection', 'Circle'
	*freehand true false
	*return draw绘制对象
	*/
	function Draw(param){
		if(!param.map){
			console.log('缺少map参数');
			return;
		}
		if(!param.source){
			console.log('缺少source参数');
			return;	
		}
		var draw=new ol.interaction.Draw({
			source:param.source,
			type:param.type,
			style:param.style!=undefined?param.style:undefined,
			freehand:param.freehand!=undefined?param.freehand:false
		});
		param.map.addInteraction(draw);
		return draw;
	}

	/*编辑工具
	*param
	*source 编辑的图形所在矢量图层source
	*features 编辑的图形数组
	*map map对象
	*style 编辑样式
	*return modify绘制对象
	*/
	function Modify(param){
		var modify=new ol.interaction.Modify({
			source:param.source?param.source:undefined,
			features:param.features!=undefined?param.features:undefined,
			style:param.style!=undefined?param.style:undefined
		});
		param.map.addInteraction(modify);
		return modify;
	};
	/*拉框
	*param
	*source 编辑的图形所在矢量图层source
	*features 编辑的图形数组
	*map map对象
	*style 编辑样式
	*return modify绘制对象
	*/
	function DragBox(param){
		var dragbox=new ol.interaction.DragBox({
			
		});
		param.map.addInteraction(dragbox);
		return dragbox;
	};
	var module={
		'draw':Draw,
		'modify':Modify,
		'dragbox':DragBox
	};
	return module;
})