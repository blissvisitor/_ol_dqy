define(['ol'],function(ol){
	var content={
		serverUrl:'http://10.0.0.4:8080/geoserver/sf/wms',//地图服务地址
		center:[11196088.239061767, 4227957.910490982],//地图中心
		zoom:4,//初始化级别
		minZoom:0,//最小级别
		maxZoom:18,//最大级别
		projection:'EPSG:4326'//坐标系统
	}
	return content;
})