//轨迹回放
define(['ol'

], function(ol) {
    var step = 0.01,
        requestID;
    /*
    轨迹模拟
    point 点feature
    line 线feature
     */
    function animation(point, line) {
        if (requestAnimationFrame) {
            // if (requestID) {
            //     cancelAnimationFrame(requestID);
            //     step = 0.01;
            //     requestID = null;
            // }
            requestID = requestAnimationFrame(function() {
                if (step <= 1) {
                    var second = line.getGeometry().getCoordinateAt(step);
                    var first = point.getGeometry().getCoordinates();
                    var angle = -Math.atan((second[1] - first[1]) / (second[0] - first[0]));
                    point.getGeometry().setCoordinates(line.getGeometry().getCoordinateAt(step));
                    point.getStyle().getImage().setRotation(angle);
                    // point2.getGeometry().setCoordinates(slowLine.getGeometry().getCoordinateAt(step));
                    step = step + 0.003;
                    animation();
                } else {
                    cancelAnimationFrame(requestID);
                    step = 0.01;
                    requestID = null;
                }
            });
        } else {
            requestID = setInterval(function() {
                if (step <= 1) {
                    var second = line.getGeometry().getCoordinateAt(step);
                    var first = point.getGeometry().getCoordinates();
                    var angle = -Math.atan((second[1] - first[1]) / (second[0] - first[0]));
                    point.getGeometry().setCoordinates(line.getGeometry().getCoordinateAt(step));
                    point.getStyle().getImage().setRotation(angle);
                    // point2.getGeometry().setCoordinates(slowLine.getGeometry().getCoordinateAt(step));
                    step = step + 0.003;
                    animation();
                } else {
                    clearInterval(requestID);
                    step = 0.01;
                    requestID = null;
                }
            }, 16);

        }
    };
    var module = {
        animation: animation
    };
    return module;

}
})