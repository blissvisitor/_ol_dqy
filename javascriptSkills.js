 /**
 * 下载图片
 <a id="export_img" style='display: none;'></a>
 */
function downImage(url,mapName) {
    //以下代码为下载此图片功能
    var triggerDownload = $("#export_img").attr("href", url).attr("download", mapName + '.jpg');
    triggerDownload[0].click();
//    triggerDownload.remove();
}
//产生随机色
 function createColor(index) {
                var r = 0, g = 100,b = 180,color="";
                r = r + index*5;
                g = g + index * 6;
                b = b + index * 7;
             
                while(r>255){
                    r=r-255;
                }
           
                while(g>255){
                    g=g-255;
                }

               while(b>255){
                    b=b-255;
                }

                if (r.toString(16).length == 1) {
                    color = '0' + r.toString(16);
                } else {
                    color = r.toString(16);
                }
                if (g.toString(16).length == 1) {
                    color = color+'0' + g.toString(16);
                } else {
                    color = color+g.toString(16);
                }
                if (b.toString(16).length == 1) {
                    color = color+'0' + b.toString(16);
                } else {
                    color = color+b.toString(16);
                }
                return color;
            }
            for(var i=0;i<100;i++)
            {
               console.dir(createColor(i));
            }