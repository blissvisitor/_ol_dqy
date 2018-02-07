/*
 *地图导出功能 支持jpg pdf 功能复杂
 *thematic_export id添加按钮，引入export.js模块 依赖map/module/libs/pdfmake 
 */
define([
    'jquery',
    'ol',
    'mapContent',
    'layer',
    'text!olmap/html/export.html',
    'pdfmake',
    'filesaver'
], function($, ol, mapContent, layer, htm,pdfmake,filesaver) {


    var mapHeight, mapWidth, exportIndex;

    var exportW, exportH, mapW, mapH, dim0, dim1;
    var titleHeight = 60;
    var titleH = 0;
    var direction = $("#paperDir :selected").val();
    //设置图例大小
    var legendW = 145;
    var legendH = 221;
    var arrowWH = 128;
    var realH, realW;

    function initExport() {
        //初始化
        $(document).ready(function() {
            realH = mapHeight;
            realW = mapWidth;
            $('#aHeight').blur(function() {
                var val = $('#aHeight').val();
                $('#aHeight').val(val.replace(/\D/g, ''));
                if (val > 3000) {
                    $('#aHeight').val(3000);
                }
                if (val < 200) {
                    $('#aHeight').val(200);
                }
            });
            $('#aWidth').blur(function() {
                var val = $('#aWidth').val();
                $('#aWidth').val(val.replace(/\D/g, ''));
                if (val > 3000) {
                    $('#aWidth').val(3000);
                }
                if (val < 200) {
                    $('#aWidth').val(200);
                }
            });
            //纸张选择事件
            $("#paperType").change(function() {
            	direction = $("#paperDir :selected").val();
                if (isPreview) {
                    $("#cancelPreviewMap").click();
                    isPreview = true;
                }
                var type = $("#paperType :selected").val();
                if (type == 'selfSet') {
                    $("#selfSetDiv").css("display", "block");
                    // parent.$('.range-bar').show();
                    return;
                } else {
                    $("#selfSetDiv").css("display", "none");
                }

                //设置图例大小
                // parent.$("#setThematic").width(legendW * 0.6);
                // parent.$("#setThematic").height(legendH * 0.6);
                // //指北针大小
                // parent.$("#arrow").css('display', 'block');
                // parent.$("#arrow").height(arrowWH * 0.6);
                // parent.$("#arrow").width(arrowWH * 0.6);

                dim0 = '';
                dim1 = '';

                var size = $("#paperType :selected").val();
                var resolution = $("#resolution :selected").val();
                //设置标题高度
                if (resolution == '100') {
                    titleH = titleHeight;
                } else if (resolution == '200') {
                    titleH = titleHeight * 2;
                } else if (resolution == '300') {
                    titleH = titleHeight * 5;
                };
                //横向
                if (direction == 0) {
                    if (size == 'selfSet') { //自定义


                    } else {
                        $("#selfSetDiv").css("display", "none");
                        var dims = {
                            a0: [1189, 841],
                            a1: [841, 594],
                            a2: [594, 420],
                            a3: [420, 297],
                            a4: [297, 210],
                            a5: [210, 148]
                        };
                        var dim = dims[size];
                        dim0 = dim[0];
                        dim1 = dim[1];
                        if (dim0 / dim1 > realW / realH) {
                            exportW = realW;
                            exportH = realW * dim1 / dim0;
                        } else {
                            exportH = realH;
                            exportW = realH * dim0 / dim1;
                        }
                        //初始化地图范围
                        $("#map").width(realW);
                        $("#map").height(realH);
                        mapContent.map.updateSize();
                        //设置新的范围
                        $("#map").width(exportW);
                        $("#map").height(exportH);
                        mapContent.map.updateSize();
                    }
                } else if (direction == 1) { //纵向
                    if (size == 'selfSet') { //自定义
                    } else {
                        $("#selfSetDiv").css("display", "none");
                        var dims = {
                            a0: [841, 1189],
                            a1: [594, 841],
                            a2: [420, 594],
                            a3: [297, 420],
                            a4: [210, 297],
                            a5: [148, 210]
                        };
                        var dim = dims[size];
                        dim0 = dim[0];
                        dim1 = dim[1];
                        if (dim0 / dim1 > realW / realH) {
                            exportW = realW;
                            exportH = realW * dim1 / dim0;
                        } else {
                            exportH = realH;
                            exportW = realH * dim0 / dim1;
                        }
                        //初始化地图范围
                        $("#map").width(realW);
                        $("#map").height(realH);
                        mapContent.map.updateSize();
                        //设置新的范围
                        $("#map").width(exportW);
                        $("#map").height(exportH);
                        mapContent.map.updateSize();

                    }
                }
                if (isPreview) {
                    isPreview = false;
                    $("#previewMap").click();
                }

            });
            //自定义纸张
            $("#selfEnsure").click(function() {
                var w = parseFloat($("#aWidth").val());
                var h = parseFloat($("#aHeight").val());
                if (w != null && h != null && w != 'undefined' && h != 'undefined' && w != '' && h != '') {
                    if (direction == 0) { //横
                        dim0 = h;
                        dim1 = w;
                        if (dim1 < 298 && dim0 < 421) {
                            paperType = 'a4';
                        } else if (dim1 > 296 && dim1 < 595 && dim0 > 419 && dim0 < 842) {
                            paperType = 'a2';
                        } else if (dim1 > 594 && dim0 > 841) {
                            paperType = 'a0';
                        }

                    } else if (direction == 1) { //纵
                        dim0 = w;
                        dim1 = h;
                        if (dim0 < 298 && dim1 < 421) {
                            paperType = 'a4';
                        } else if (dim0 > 296 && dim0 < 595 && dim1 > 419 && dim1 < 842) {
                            paperType = 'a2';
                        } else if (dim0 > 594 && dim1 > 841) {
                            paperType = 'a0';
                        }
                    }



                } else {
                    layer.msg("请设置纸张宽高！");
                    return;
                }
                if (dim0 / dim1 > realW / realH) {
                    exportW = realW;
                    exportH = realW * dim1 / dim0;
                } else {
                    exportH = realH;
                    exportW = realH * dim0 / dim1;
                }
                //初始化地图范围
                $("#map").width(realW);
                $("#map").height(realH);
                mapContent.map.updateSize();
                //设置新的范围
                $("#map").width(exportW);
                $("#map").height(exportH);
                mapContent.map.updateSize();

                if (isPreview) {
                    isPreview = false;
                    $("#previewMap").click();
                }


            });
            var paperType = '';
            //导出专题图
            $("#exportMap").click(function() {
                var oper = 1;
                var mapName = $("#mapName").val();
                if (!mapName) {
                    layer.msg('请设置专题图名称！');
                    return;
                }
                var paperSize = $("#paperType :selected").val();
                if (paperSize == 'selfSet') {
                    paperSize = paperType;
                    if (!$('#aWidth').val() || !$('#aWidth').val()) {
                        layer.msg('请设置自定义宽高！');
                        return;
                    }
                }
                var resolution = $("#resolution :selected").val();
                var fileType = $("#fileType :selected").val();
                $("#cancelPreviewMap").attr('disabled', true);
                $("#previewMap").attr('disabled', false);
                $("#cancelPreviewMap").css('background-color', '#777');
                $("#previewMap").css('background-color', '#e24141');
                //自定义确定
                $("#selfEnsure").attr('disabled', false);
                $("#selfEnsure").css('background-color', '#e24141');
                if (fileType == ".jpg") {
                    exportThematicJpg(titleH, 'jpg', dim0, dim1, exportW, exportH, resolution, mapName, paperSize, oper);
                } else {
                    exportThematicJpg(titleH, 'pdf', dim0, dim1, exportW, exportH, resolution, mapName, paperSize, oper);
                }
                //隐藏控制条
                // parent.$('.range-bar').show();
            });
            //预览专题图
            $("#previewMap").click(function() {
                var oper = 2;
                var mapName = $("#mapName").val();
                if (!mapName) {
                    layer.msg('请设置专题图名称！');
                    return;
                }
                var paperSize = $("#paperType :selected").val();
                if (paperSize == 'selfSet') {
                    paperSize = paperType;
                    if (!$('#aWidth').val() || !$('#aWidth').val()) {
                        layer.msg('请设置自定义宽高！');
                        return;
                    }
                }
                var resolution = $("#resolution :selected").val();
                var fileType = $("#fileType :selected").val();
                if (fileType == ".jpg") {
                    exportThematicJpg(titleH, 'jpg', dim0, dim1, exportW, exportH, resolution, mapName, paperSize, oper);
                } else {
                    exportThematicJpg(titleH, 'pdf', dim0, dim1, exportW, exportH, resolution, mapName, paperSize, oper);
                }
                $("#cancelPreviewMap").attr('disabled', false);
                $("#previewMap").attr('disabled', true);
                $("#previewMap").css('background-color', '#777');
                $("#cancelPreviewMap").css('background-color', '#e24141');
                //自定义确定
                $("#selfEnsure").attr('disabled', true);
                $("#selfEnsure").css('background-color', '#777');
                //隐藏控制条
                // parent.$('.range-bar').hide();
            });
            //取消预览专题图
            $("#cancelPreviewMap").click(function() {
                //显示控制条
                // parent.$('.range-bar').show();
                $("#cancelPreviewMap").attr('disabled', true);
                $("#previewMap").attr('disabled', false);
                $("#cancelPreviewMap").css('background-color', '#777');
                $("#previewMap").css('background-color', '#e24141');

                //自定义确定
                $("#selfEnsure").attr('disabled', false);
                $("#selfEnsure").css('background-color', '#e24141');
                var oper = 3;
                var mapName = $("#mapName").val();
                var paperSize = $("#paperType :selected").val();
                var resolution = $("#resolution :selected").val();
                var fileType = $("#fileType :selected").val();
                if (fileType == ".jpg") {
                    exportThematicJpg(titleH, 'jpg', dim0, dim1, exportW, exportH, resolution, mapName, paperSize, oper);
                } else {
                    exportThematicJpg(titleH, 'pdf', dim0, dim1, exportW, exportH, resolution, mapName, paperSize, oper);
                }

            });
            //清晰度
            $("#resolution").change(function() {
                $("#paperType").change();
            });
            //纸张方向
            $("#paperDir").change(function() {
                direction = $("#paperDir :selected").val();
                $("#paperType").change();
            });
            $("#paperType").change();
        });
    }

    //屏蔽地图事件
    function operTools(bl) {
        mapContent.map.getInteractions().forEach(function(interaction) {
            if (interaction instanceof ol.interaction.MouseWheelZoom) {
                interaction.setActive(bl);
            }
        }, this);
        mapContent.map.getInteractions().forEach(function(interaction) {
            if (interaction instanceof ol.interaction.DragPan) {
                interaction.setActive(bl);
            }
        }, this);
    }
    //导出图片,pdf
    var exportImgIndex = null;
    var currZoom = null;
    var currCenter = null;
    var canWidth, canHeight;
    var isPreview = false; //是否预览
    var mapResolution;
    // var tl1 = document.getElementById('tl1').src,
    //     tl_1_a = document.getElementById('tl_1_a').src,
    //     tl_1_b = document.getElementById('tl_1_b').src,
    //     tl2 = document.getElementById('tl2').src,
    //     tl_2_a = document.getElementById('tl_2_a').src,
    //     tl_2_b = document.getElementById('tl_2_b').src,
    //     tl3 = document.getElementById('tl3').src,
    //     tl_3_a = document.getElementById('tl_3_a').src,
    //     tl_3_b = document.getElementById('tl_3_b').src,
    //     tl4 = document.getElementById('tl4').src,
    //     tl_4_a = document.getElementById('tl_4_a').src,
    //     tl_4_b = document.getElementById('tl_4_b').src,
    //     tl5 = document.getElementById('tl5').src,
    //     tl_5_a = document.getElementById('tl_5_a').src,
    //     tl_5_b = document.getElementById('tl_5_b').src,
    //     compass = document.getElementById('compass').src,
    //     compass1 = document.getElementById('compass1').src,
    //     compass2 = document.getElementById('compass2').src;
    function exportThematicJpg(titleHeight, exportType, dim0, dim1, mapW, mapH, resolution, mapName, paperSize, oper) {

        if (!isPreview) {
            currZoom = mapContent.view.getZoom();
            currCenter = mapContent.view.getCenter();
            mapResolution = mapContent.view.getResolution();
        }
        var legendSource, compassSource;
        var legendWidth = '';
        var legendHeight = '';
        var arrowWithHeight = '';
        resolution = '100';
        var width = Math.round(dim0 * resolution / 25.4);
        var height = Math.round(dim1 * resolution / 25.4);
        canWidth = width;
        canHeight = height;
        //titleHeight = titleHeight * resolution / 25.4;
        //设置图例大小
        var vl = $('#symbolic').val();
        // if (paperSize == 'a4' || paperSize == 'a3') {
        //     $('#compass').attr('src', compass);
        //     arrowWithHeight = $('#compass').height();
        //     compassSource = document.getElementById('compass');
        //     switch (vl) {
        //         case 'isting':
        //             $('#tl5').attr('src', tl5);
        //             legendSource = document.getElementById('tl5');

        //             legendWidth = $('#tl5').width();
        //             legendHeight = $('#tl5').height();
        //             break;
        //         case 'ispai':
        //             $('#tl4').attr('src', tl4);
        //             legendWidth = $('#tl4').width();
        //             legendHeight = $('#tl4').height();
        //             legendSource = document.getElementById('tl4');
        //             break;
        //         case 'isjia':
        //             $('#tl3').attr('src', tl3);
        //             legendWidth = $('#tl3').width();
        //             legendHeight = $('#tl3').height();
        //             legendSource = document.getElementById('tl3');
        //             break;
        //         case 'hasgw':
        //             $('#tl2').attr('src', tl2);
        //             legendWidth = $('#tl2').width();
        //             legendHeight = $('#tl2').height();
        //             legendSource = document.getElementById('tl2');
        //             break;
        //         case 'owner':
        //             $('#tl1').attr('src', tl1);
        //             legendWidth = $('#tl1').width();
        //             legendHeight = $('#tl1').height();
        //             legendSource = document.getElementById('tl1');
        //             break;
        //         default:
        //             $('.maptl').hide();
        //             legendWidth = 0;
        //             legendHeight = 0;
        //             legendSource = null;
        //             break;
        //     }
        // } else if (paperSize == 'a2' || paperSize == 'a1') {
        //     $('#compass').attr('src', compass1);
        //     arrowWithHeight = $('#compass').height();
        //     compassSource = document.getElementById('compass');
        //     switch (vl) {
        //         case 'isting':
        //             $('#tl5').attr('src', tl_5_a);
        //             legendWidth = $('#tl5').width();
        //             legendHeight = $('#tl5').height();
        //             legendSource = document.getElementById('tl5');
        //             break;
        //         case 'ispai':
        //             $('#tl4').attr('src', tl_4_a);
        //             legendWidth = $('#tl4').width();
        //             legendHeight = $('#tl4').height();
        //             legendSource = document.getElementById('tl4');
        //             break;
        //         case 'isjia':
        //             $('#tl3').attr('src', tl_3_a);
        //             legendWidth = $('#tl3').width();
        //             legendHeight = $('#tl3').height();
        //             legendSource = document.getElementById('tl3');
        //             break;
        //         case 'hasgw':
        //             $('#tl2').attr('src', tl_2_a);
        //             legendWidth = $('#tl2').width();
        //             legendHeight = $('#tl2').height();
        //             legendSource = document.getElementById('tl2');
        //             break;
        //         case 'owner':
        //             $('#tl1').attr('src', tl_1_a);
        //             legendWidth = $('#tl1').width();
        //             legendHeight = $('#tl1').height();
        //             legendSource = document.getElementById('tl1');
        //             break;
        //         default:
        //             $('.maptl').hide();
        //             legendWidth = 0;
        //             legendHeight = 0;
        //             legendSource = null;
        //             break;
        //     }
        // } else if (paperSize == 'a0') {
        //     $('#compass').attr('src', compass2);
        //     arrowWithHeight = $('#compass').height();
        //     compassSource = document.getElementById('compass');
        //     switch (vl) {
        //         case 'isting':
        //             $('#tl5').attr('src', tl_5_b);
        //             legendWidth = $('#tl5').width();
        //             legendHeight = $('#tl5').height();
        //             legendSource = document.getElementById('tl5');
        //             break;
        //         case 'ispai':
        //             $('#tl4').attr('src', tl_4_b);
        //             legendWidth = $('#tl4').width();
        //             legendHeight = $('#tl4').height();
        //             legendSource = document.getElementById('tl4');
        //             break;
        //         case 'isjia':
        //             $('#tl3').attr('src', tl_3_b);
        //             legendWidth = $('#tl3').width();
        //             legendHeight = $('#tl3').height();
        //             legendSource = document.getElementById('tl3');
        //             break;
        //         case 'hasgw':
        //             $('#tl2').attr('src', tl_2_b);
        //             legendWidth = $('#tl2').width();
        //             legendHeight = $('#tl2').height();
        //             legendSource = document.getElementById('tl2');
        //             break;
        //         case 'owner':
        //             $('#tl1').attr('src', tl_1_b);
        //             legendWidth = $('#tl1').width();
        //             legendHeight = $('#tl1').height();
        //             legendSource = document.getElementById('tl1');
        //             break;
        //         default:
        //             $('.maptl').hide();
        //             legendWidth = 0;
        //             legendHeight = 0;
        //             legendSource = null;
        //             break;
        //     }
        // };
        //导出
        if (oper == '1') {
            $("#cancelPreviewMap").attr('disabled', true);
            $("#previewMap").attr('disabled', false);
            $("#cancelPreviewMap").css('background-color', '#777');
            $("#previewMap").css('background-color', '#e24141');
            exportImgIndex = layer.load("2", { shade: [0.8, '#393D49'] });
            if (!isPreview) {
                var extent1 = mapContent.view.calculateExtent([mapW, mapH]);
                $("#map").height(canHeight);
                $("#map").width(canWidth);
                mapContent.map.setSize([canWidth, canHeight]);
                mapContent.map.updateSize();
                mapContent.view.fit(extent1, [canWidth, canHeight]);
                // if (mapW / canWidth * mapResolution < bdResolution[18]) {
                //     mapContent.view.setResolution(bdResolution[18]);
                // } else if (mapW / canWidth * mapResolution > bdResolution[0]) {
                //     mapContent.view.setResolution(bdResolution[0]);
                // } else {
                //     mapContent.view.setResolution(mapW / canWidth * mapResolution);
                // }
                // mapContent.view.setResolution(mapW / canWidth * mapResolution);
                mapContent.view.setCenter(currCenter);
                mapContent.map.updateSize();
            }
            mapContent.map.once('postcompose', function(event) {
                setTimeout(function() {
                    var type = 'image/jpeg';
                    var canvas = event.context.canvas;
                    var mapCanvas = canvas;
                    var myCanvas = document.createElement("canvas");
                    var canvasWidth = width;
                    var canvasHeight = height;
                    myCanvas.width = canvasWidth;
                    myCanvas.height = canvasHeight;
                    var ctx2 = myCanvas.getContext("2d");
                    //绘制边框
                    ctx2.beginPath();
                    ctx2.rect(0, 0, canvasWidth, canvasHeight);
                    ctx2.fillStyle = "white";
                    //ctx2.fillRect(0, 0, myCanvas.width, myCanvas.height);
                    ctx2.fill();
                    //绘制标题
                    //设置字体样式
                    var fontsize = 40;
                    var fontS = 40;
                    if (titleHeight == 60) {
                        ctx2.font = fontsize * 1.2 + "px Courier New";
                        fontS = fontsize * 1.2;

                    } else if (titleHeight == 120) {
                        ctx2.font = fontsize * 3 + "px Courier New";
                        fontS = fontsize * 3;
                    } else if (titleHeight == 300) {
                        ctx2.font = fontsize * 7 + "px Courier New";
                        fontS = fontsize * 7;
                    }
                    //设置字体填充颜色
                    ctx2.fillStyle = "black";
                    //从坐标点(50,50)开始绘制文字
                    ctx2.textAlign = "center";
                    ctx2.fillText(mapName, canvasWidth / 2, titleHeight * 5 / 6);
                    //绘制地图
                    ctx2.drawImage(mapCanvas, 0, 0, mapCanvas.width, mapCanvas.height, 0, titleHeight, canvasWidth, canvasHeight);
                    //外框
                    ctx2.strokeStyle = "black"; /*设置边框*/
                    ctx2.lineWidth = 4; /*边框的宽度*/
                    //ctx2.strokeRect(0, titleHeight, mapCanvas.width, mapCanvas.height);
                    ctx2.strokeRect(0, titleHeight, canvasWidth, canvasHeight - titleHeight);
                    //内框
                    ctx2.strokeStyle = "black"; /*设置边框*/
                    ctx2.lineWidth = 2; /*边框的宽度*/
                    ctx2.strokeRect(6, titleHeight + 6, canvasWidth - 12, canvasHeight - titleHeight - 12);
                    //空白
                    ctx2.lineWidth = 2
                    ctx2.strokeStyle = "white";
                    ctx2.strokeRect(4, titleHeight + 4, canvasWidth - 8, canvasHeight - titleHeight - 8);
                    ////绘制图例
                    if (legendSource) {
                        ctx2.drawImage(legendSource, parseFloat(canvasWidth - legendWidth - 10), parseFloat(canvasHeight - legendHeight - 10));
                    }
                    //绘制指北针
                    if(compassSource){
                    ctx2.drawImage(compassSource, parseFloat(canvasWidth - arrowWithHeight - 30), titleHeight + 30);
                	}
                    //将canvas元素中的图像转变为DataURL
                    var dataurl = myCanvas.toDataURL('image/jpeg');
                    if (exportType == "pdf") {
                        var dd = {
                            content: [{
                                image: dataurl,
                                alignment: "center",
                                width: 508
                            }]
                        };
                        pdfMake.createPdf(dd).download(mapName + ".pdf");
                    } else if (exportType == 'jpg') {
                         myCanvas.toBlob(function(blob) {
                        saveAs(blob, 'map.png');
                    });
                    	//downImage(dataurl,mapName);
                        //window.open(dataurl);
                        //document.write('<img src="' + dataurl + '"/>');
                        //saveFile(dataurl, mapName + '.jpg');
                        // mapName += ".jpg";
                        // window.location.href = "/Attach/DownMapImage?base64=" + dataurl + "&mapName=" + mapName;
               
                        // DownLoadFile({
                        //     url: "/Gis/OutputBase64", //请求的url
                        //     data: { imgByte: dataurl, name: mapName + '.jpg' } //要发送的数据
                        // });
                    }
                    //关闭加载框
                    layer.close(exportImgIndex);
                    exportImgIndex = null;


                    //还原
                    $("#map").height(mapH);
                    $("#map").width(mapW);
                    mapContent.map.setSize([mapW, mapH]);
                    mapContent.map.updateSize();
                    mapContent.view.setZoom(currZoom);
                    // mapContent.view.setResolution(mapResolution);
                    mapContent.view.setCenter(currCenter);
                    isPreview = false;
                    operTools(true);
                    // $('#compass').attr('src', compass);
                    // switch (vl) {
                    //     case 'isting':
                    //         $('#tl5').attr('src', tl5);
                    //         break;
                    //     case 'ispai':
                    //         $('#tl4').attr('src', tl4);
                    //         break;
                    //     case 'isjia':
                    //         $('#tl3').attr('src', tl3);
                    //         break;
                    //     case 'hasgw':
                    //         $('#tl2').attr('src', tl2);
                    //         break;
                    //     case 'owner':
                    //         $('#tl1').attr('src', tl1);
                    //         break;
                    //     default:
                    //         $('.maptl').hide();
                    //         break;
                    // }

                }, 5000);
            });
            mapContent.map.renderSync();
        } else if (oper == '2') { //预览
            isPreview = true;
            operTools(false);
            var extent1 = mapContent.view.calculateExtent([mapW, mapH]);
            $("#map").height(canHeight);
            $("#map").width(canWidth);
            mapContent.map.setSize([canWidth, canHeight]);
            mapContent.map.updateSize();
            mapContent.view.fit(extent1, [canWidth, canHeight]);
            // if (mapW / canWidth * mapResolution < bdResolution[18]) {
            //     mapContent.view.setResolution(bdResolution[18]);
            // } else if (mapW / canWidth * mapResolution > bdResolution[0]) {
            //     mapContent.view.setResolution(bdResolution[0]);
            // } else {
            //     mapContent.view.setResolution(mapW / canWidth * mapResolution);
            // }
            mapContent.view.setResolution(mapW / canWidth * mapResolution);
            mapContent.view.setCenter(currCenter);
            mapContent.map.updateSize();

        } else if (oper == '3') { //取消预览
            if (isPreview) {
                $("#map").height(mapH);
                $("#map").width(mapW);
                mapContent.map.setSize([mapW, mapH]);
                mapContent.map.updateSize();
                mapContent.view.setZoom(currZoom);
                // mapContent.view.setResolution(mapResolution);
                mapContent.view.setCenter(currCenter);
                isPreview = false;
                operTools(true);
                // $('#compass').attr('src', compass);
                // switch (vl) {
                //     case 'isting':
                //         $('#tl5').attr('src', tl5);
                //         break;
                //     case 'ispai':
                //         $('#tl4').attr('src', tl4);
                //         break;
                //     case 'isjia':
                //         $('#tl3').attr('src', tl3);
                //         break;
                //     case 'hasgw':
                //         $('#tl2').attr('src', tl2);
                //         break;
                //     case 'owner':
                //         $('#tl1').attr('src', tl1);
                //         break;
                //     default:
                //         $('.maptl').hide();
                //         break;
                // }
            }

        }
    };

    function init() {
        mapWidth = $('#map').width();
        mapHeight = $('#map').height();
        //导出专题图
        $('#thematic_export').click(function() {
            var winWidth = '';
            // 获取窗口宽度
            if (window.innerWidth)
                winWidth = window.innerWidth;
            else if ((document.body) && (document.body.clientWidth))
                winWidth = document.body.clientWidth;
            var leftDis = winWidth - 420 + 'px';
            if (exportIndex) {
                layer.close(exportIndex);
                exportIndex = null;
            }
            exportIndex = layer.open({
                type: 1,
                title: "导出参数设置",
                area: ['25%', '300px'],
                offset: ['10px', '10px'],
                border: [0, 0, false],
                shade: 0,
                closeBtn: 2,
                content: htm,
                success: function(layero, index) {
                    initExport();
                },
                end: function() {
                    //设置图例，指北针宽高
                    // var vl = $('#symbolic').val();
                    // $('#compass').attr('src', compass);
                    // switch (vl) {
                    //     case 'isting':
                    //         $('#tl5').attr('src', tl5);
                    //         break;
                    //     case 'ispai':
                    //         $('#tl4').attr('src', tl4);
                    //         break;
                    //     case 'isjia':
                    //         $('#tl3').attr('src', tl3);
                    //         break;
                    //     case 'hasgw':
                    //         $('#tl2').attr('src', tl2);
                    //         break;
                    //     case 'owner':
                    //         $('#tl1').attr('src', tl1);
                    //         break;
                    //     default:
                    //         $('.maptl').hide();
                    //         break;
                    // }

                    $("#map").height(mapHeight);
                    $("#map").width(mapWidth);
                    mapContent.map.updateSize();
                }
            });

        });
    }
    var module = {
        init: init
    };
    return module;
})