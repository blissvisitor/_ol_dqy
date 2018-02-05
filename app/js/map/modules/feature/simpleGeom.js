define(["ol"], function(ol) {
    var emergencyPlot = emergencyPlot || {};

    emergencyPlot.utils = {};
    //求欧式距离
    emergencyPlot.utils.distance = function(point1, point2) {
        return Math.sqrt(Math.pow(point2[0] - point1[0], 2) + Math.pow(point2[1] - point1[1], 2));
    }

    //求出方位角
    emergencyPlot.utils.getAzimuth = function(startPnt, endPnt) {
        var azimuth;
        var angle = Math.asin(Math.abs(endPnt[1] - startPnt[1]) / emergencyPlot.utils.distance(startPnt, endPnt));
        if (endPnt[1] >= startPnt[1] && endPnt[0] >= startPnt[0])
            azimuth = angle + Math.PI;
        else if (endPnt[1] >= startPnt[1] && endPnt[0] < startPnt[0])
            azimuth = Math.PI * 2 - angle;
        else if (endPnt[1] < startPnt[1] && endPnt[0] < startPnt[0])
            azimuth = angle;
        else if (endPnt[1] < startPnt[1] && endPnt[0] >= startPnt[0])
            azimuth = Math.PI - angle;
        return azimuth;
    }

    //根据起点终点和方位角求出方位角
    emergencyPlot.utils.getThirdPoint = function(startPnt, endPnt, angle, distance, clockWise) {
        var azimuth = emergencyPlot.utils.getAzimuth(startPnt, endPnt);
        var alpha = clockWise ? azimuth + angle : azimuth - angle;
        var dx = distance * Math.cos(alpha);
        var dy = distance * Math.sin(alpha);
        return [endPnt[0] + dx, endPnt[1] + dy];
    };

    emergencyPlot.utils.getBaseLength = function(points) {
        var distance = 0;
        for (var i = 0; i < points.length - 1; i++)
            distance += emergencyPlot.utils.distance(points[i], points[i + 1]);
        return Math.pow(distance, 0.99);
    }

    //计算得出尾部节点
    emergencyPlot.utils.getTailPoints = function(points) {
        var allLen = emergencyPlot.utils.getBaseLength(points);
        var tailWidth = allLen * 0.1;
        var tailLeft = emergencyPlot.utils.getThirdPoint(points[1], points[0], Math.PI / 2, tailWidth, false);
        var tailRight = emergencyPlot.utils.getThirdPoint(points[1], points[0], Math.PI / 2, tailWidth, true);
        return [tailLeft, tailRight];
    }

    emergencyPlot.utils.getAngleOfThreePoints = function(pntA, pntB, pntC) {
        var angle = emergencyPlot.utils.getAzimuth(pntB, pntA) - emergencyPlot.utils.getAzimuth(pntB, pntC);
        return (angle < 0 ? angle + Math.PI * 2 : angle);
    };

    //获得长箭头中间的节点
    emergencyPlot.utils.getArrowBodyPoints = function(points, neckLeft, neckRight, tailWidthFactor) {
        var allLen = 0;
        for (var i = 0; i < points.length - 1; i++)
            allLen += emergencyPlot.utils.distance(points[i], points[i + 1]);
        var len = emergencyPlot.utils.getBaseLength(points);
        var tailWidth = len * tailWidthFactor;
        var neckWidth = emergencyPlot.utils.distance(neckLeft, neckRight);
        var widthDif = (tailWidth - neckWidth) / 2;
        var tempLen = 0,
            leftBodyPnts = [],
            rightBodyPnts = [];
        for (var i = 1; i < points.length - 1; i++) {
            var angle = emergencyPlot.utils.getAngleOfThreePoints(points[i - 1], points[i], points[i + 1]) / 2;
            tempLen += emergencyPlot.utils.distance(points[i - 1], points[i]);
            var w = (tailWidth / 2 - tempLen / allLen * widthDif) / Math.sin(angle);
            var left = emergencyPlot.utils.getThirdPoint(points[i - 1], points[i], Math.PI - angle, w, true);
            var right = emergencyPlot.utils.getThirdPoint(points[i - 1], points[i], angle, w, false);
            leftBodyPnts.push(left);
            rightBodyPnts.push(right);
        }
        return leftBodyPnts.concat(rightBodyPnts);
    };



    emergencyPlot.utils.getQuadricBSplineFactor = function(k, t) {
        if (k == 0)
            return Math.pow(t - 1, 2) / 2;
        if (k == 1)
            return (-2 * Math.pow(t, 2) + 2 * t + 1) / 2;
        if (k == 2)
            return Math.pow(t, 2) / 2;
        return 0;
    };


    emergencyPlot.utils.getQBSplinePoints = function(points) {
        if (points.length <= 2)
            return points;
        var n = 2;

        var bSplinePoints = [];
        var m = points.length - n - 1;
        bSplinePoints.push(points[0]);
        for (var i = 0; i <= m; i++) {
            for (var t = 0; t <= 1; t += 0.05) {
                var x = y = 0;
                for (var k = 0; k <= n; k++) {
                    var factor = emergencyPlot.utils.getQuadricBSplineFactor(k, t);
                    x += factor * points[i + k][0];
                    y += factor * points[i + k][1];
                }
                bSplinePoints.push([x, y]);
            }
        }
        bSplinePoints.push(points[points.length - 1]);
        return bSplinePoints;
    };

    //计算出头部节点
    emergencyPlot.utils.getArrowHeadPoints = function(points, tailLeft, tailRight) {
        var len = emergencyPlot.utils.getBaseLength(points);
        var headHeight = len * 0.3;
        var headPnt = points[points.length - 1];
        var tailWidth = emergencyPlot.utils.distance(tailLeft, tailRight);
        var headWidth = headHeight * 0.3;
        var neckWidth = headHeight * 0.15;
        var neckHeight = headHeight * 0.85;
        var headEndPnt = emergencyPlot.utils.getThirdPoint(points[points.length - 2], headPnt, 0, headHeight, true);
        var neckEndPnt = emergencyPlot.utils.getThirdPoint(points[points.length - 2], headPnt, 0, neckHeight, true);
        var headLeft = emergencyPlot.utils.getThirdPoint(headPnt, headEndPnt, Math.PI / 2, headWidth, false);
        var headRight = emergencyPlot.utils.getThirdPoint(headPnt, headEndPnt, Math.PI / 2, headWidth, true);
        var neckLeft = emergencyPlot.utils.getThirdPoint(headPnt, neckEndPnt, Math.PI / 2, neckWidth, false);
        var neckRight = emergencyPlot.utils.getThirdPoint(headPnt, neckEndPnt, Math.PI / 2, neckWidth, true);
        return [neckLeft, headLeft, headPnt, headRight, neckRight];
    };




    emergencyPlot.utils.mid = function(pnt1, pnt2) {
        return [(pnt1[0] + pnt2[0]) / 2, (pnt1[1] + pnt2[1]) / 2];
    };


    emergencyPlot.utils.getNormal = function(pnt1, pnt2, pnt3) {
        var dX1 = pnt1[0] - pnt2[0];
        var dY1 = pnt1[1] - pnt2[1];
        var d1 = Math.sqrt(dX1 * dX1 + dY1 * dY1);
        dX1 /= d1;
        dY1 /= d1;

        var dX2 = pnt3[0] - pnt2[0];
        var dY2 = pnt3[1] - pnt2[1];
        var d2 = Math.sqrt(dX2 * dX2 + dY2 * dY2);
        dX2 /= d2;
        dY2 /= d2;

        var uX = dX1 + dX2;
        var uY = dY1 + dY2;
        return [uX, uY];
    };

    emergencyPlot.utils.isClockWise = function(pnt1, pnt2, pnt3) {
        return ((pnt3[1] - pnt1[1]) * (pnt2[0] - pnt1[0]) > (pnt2[1] - pnt1[1]) * (pnt3[0] - pnt1[0]));
    };

    emergencyPlot.utils.getBisectorNormals = function(t, pnt1, pnt2, pnt3) {
        var normal = emergencyPlot.utils.getNormal(pnt1, pnt2, pnt3);
        var dist = Math.sqrt(normal[0] * normal[0] + normal[1] * normal[1]);
        var uX = normal[0] / dist;
        var uY = normal[1] / dist;
        var d1 = emergencyPlot.utils.distance(pnt1, pnt2);
        var d2 = emergencyPlot.utils.distance(pnt2, pnt3);
        if (dist > 0.0001) {
            if (emergencyPlot.utils.isClockWise(pnt1, pnt2, pnt3)) {
                var dt = t * d1;
                var x = pnt2[0] - dt * uY;
                var y = pnt2[1] + dt * uX;
                var bisectorNormalRight = [x, y];
                dt = t * d2;
                x = pnt2[0] + dt * uY;
                y = pnt2[1] - dt * uX;
                var bisectorNormalLeft = [x, y];
            } else {
                dt = t * d1;
                x = pnt2[0] + dt * uY;
                y = pnt2[1] - dt * uX;
                bisectorNormalRight = [x, y];
                dt = t * d2;
                x = pnt2[0] - dt * uY;
                y = pnt2[1] + dt * uX;
                bisectorNormalLeft = [x, y];
            }
        } else {
            x = pnt2[0] + t * (pnt1[0] - pnt2[0]);
            y = pnt2[1] + t * (pnt1[1] - pnt2[1]);
            bisectorNormalRight = [x, y];
            x = pnt2[0] + t * (pnt3[0] - pnt2[0]);
            y = pnt2[1] + t * (pnt3[1] - pnt2[1]);
            bisectorNormalLeft = [x, y];
        }
        return [bisectorNormalRight, bisectorNormalLeft];
    };


    emergencyPlot.utils.getCubicValue = function(t, startPnt, cPnt1, cPnt2, endPnt) {
        t = Math.max(Math.min(t, 1), 0);
        var tp = 1 - t;
        var t2 = t * t;
        var t3 = t2 * t;
        var tp2 = tp * tp;
        var tp3 = tp2 * tp;
        var x = (tp3 * startPnt[0]) + (3 * tp2 * t * cPnt1[0]) + (3 * tp * t2 * cPnt2[0]) + (t3 * endPnt[0]);
        var y = (tp3 * startPnt[1]) + (3 * tp2 * t * cPnt1[1]) + (3 * tp * t2 * cPnt2[1]) + (t3 * endPnt[1]);
        return [x, y];
    };
    //创建简单直箭头
    /**
     * @api {类型 function} 创建简单直箭头 createArrow
     * @apiParam {Array} coordinates 传入正在绘制的点的坐标点
     * @apiName createArrow 
     * @apiGroup plot
     */
    emergencyPlot.createArrow = function(coordinates) {
        var pnts = coordinates;
        var pnt1 = pnts[0];
        var pnt2 = pnts[1];
        var distance = emergencyPlot.utils.distance(pnt1, pnt2);
        var len = distance / 5;
        len = len > 300000 ? 300000 : len;
        var leftPnt = emergencyPlot.utils.getThirdPoint(pnt1, pnt2, Math.PI / 6, len, false);
        var rightPnt = emergencyPlot.utils.getThirdPoint(pnt1, pnt2, Math.PI / 6, len, true);
        return [pnt1, pnt2, leftPnt, pnt2, rightPnt];
    }

    //创建简单直箭头
    /**
     * @api {类型 function} 创建简单直箭头 createLongArrow
     * @apiParam {Array} coordinates 传入正在绘制的点的坐标点
     * @apiName createLongArrow 
     * @apiGroup plot
     */
    emergencyPlot.createFineArrow = function(coordinates) {
        if (coordinates.length < 2) {
            return;
        }
        var pnts = coordinates;
        var pnt1 = pnts[0];
        var pnt2 = pnts[1];
        var len = emergencyPlot.utils.getBaseLength(pnts);
        var tailWidth = len * 0.15;
        var neckWidth = len * 0.2;
        var headWidth = len * 0.25;
        var tailLeft = emergencyPlot.utils.getThirdPoint(pnt2, pnt1, Math.PI / 2, tailWidth, true);
        var tailRight = emergencyPlot.utils.getThirdPoint(pnt2, pnt1, Math.PI / 2, tailWidth, false);
        var headLeft = emergencyPlot.utils.getThirdPoint(pnt1, pnt2, Math.PI / 8.5, headWidth, false);
        var headRight = emergencyPlot.utils.getThirdPoint(pnt1, pnt2, Math.PI / 8.5, headWidth, true);
        var neckLeft = emergencyPlot.utils.getThirdPoint(pnt1, pnt2, Math.PI / 13, neckWidth, false);
        var neckRight = emergencyPlot.utils.getThirdPoint(pnt1, pnt2, Math.PI / 13, neckWidth, true);
        return [tailLeft, neckLeft, headLeft, pnt2, headRight, neckRight, tailRight, tailLeft];
    }

    //创建长箭头
    /**
     * @api {类型 function} 创建长箭头 createLongArrow
     * @apiParam {Array} coordinates 传入正在绘制的点的坐标点
     * @apiName createLongArrow 
     * @apiGroup plot
     */
    emergencyPlot.createLongArrow = function(coordinates) {
        var pnts = coordinates;
        if (coordinates.length < 2) {
            return;
        }
        var tailPnts = emergencyPlot.utils.getTailPoints(pnts);
        var headPnts = emergencyPlot.utils.getArrowHeadPoints(pnts, tailPnts[0], tailPnts[1]);
        var neckLeft = headPnts[0];
        var neckRight = headPnts[4];
        var bodyPnts = emergencyPlot.utils.getArrowBodyPoints(pnts, neckLeft, neckRight, 0.0001);
        var count = bodyPnts.length;
        var leftPnts = [tailPnts[0]].concat(bodyPnts.slice(0, count / 2));
        leftPnts.push(neckLeft);
        var rightPnts = [tailPnts[1]].concat(bodyPnts.slice(count / 2, count));
        rightPnts.push(neckRight);

        leftPnts = emergencyPlot.utils.getQBSplinePoints(leftPnts);
        rightPnts = emergencyPlot.utils.getQBSplinePoints(rightPnts);

        return [leftPnts.concat(headPnts, rightPnts.reverse())];
    }

    //创建聚集地
    /**
     * @api {类型 function} 创建聚集地 createHabitation
     * @apiParam {Array} coordinates 传入正在绘制的点的坐标点
     * @apiName createHabitation 
     * @apiGroup plot
     */
    emergencyPlot.createHabitation = function(coordinates) {
        var pnts = coordinates;
        if (pnts.length < 2) {
            return;
        }
        if (pnts.length == 2) {
            var mid = emergencyPlot.utils.mid(pnts[0], pnts[1]);
            var d = emergencyPlot.utils.distance(pnts[0], mid) / 0.9;
            var pnt = emergencyPlot.utils.getThirdPoint(pnts[0], mid, Math.PI / 2, d, true);
            pnts = [pnts[0], pnt, pnts[1]];
        }
        var mid = emergencyPlot.utils.mid(pnts[0], pnts[2]);
        pnts.push(mid, pnts[0], pnts[1]);

        var normals = [];
        for (var i = 0; i < pnts.length - 2; i++) {
            var pnt1 = pnts[i];
            var pnt2 = pnts[i + 1];
            var pnt3 = pnts[i + 2];
            var normalPoints = emergencyPlot.utils.getBisectorNormals(0.4, pnt1, pnt2, pnt3);
            normals = normals.concat(normalPoints);
        }
        var count = normals.length;
        normals = [normals[count - 1]].concat(normals.slice(0, count - 1));
        var pList = [];
        for (i = 0; i < pnts.length - 2; i++) {
            pnt1 = pnts[i];
            pnt2 = pnts[i + 1];
            pList.push(pnt1);
            for (var t = 0; t <= 100; t++) {
                var pnt = emergencyPlot.utils.getCubicValue(t / 100, pnt1, normals[i * 2], normals[i * 2 + 1], pnt2);
                pList.push(pnt);
            }
            pList.push(pnt2);
        }
        return [pList];
    }
    return emergencyPlot;
});