using System;

namespace GPSserver
{
    public class Point
    {
        public double X;
        public double Y;

        public Point()
        {
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class Transformer
    {
        private static double pi = 3.14159265358979324D;    // 圆周率
        private static double a = 6378245.0D;   // WGS 长轴半径
        private static double ee = 0.00669342162296594323D; // WGS 偏心率的平方

        public static bool outofChina(Point pt)
        {
            if (pt.X < 72.004 || pt.X > 137.8347)
                return true;
            if (pt.Y < 0.8293 || pt.Y > 55.8271)
                return true;
            return false;
        }

        public static double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        public static double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0 * pi)) * 2.0 / 3.0;
            return ret;
        }

        // 84->gcj02
        public static Point WGS84ToGCJ02(Point pt)
        {
            if (outofChina(pt))
            {
                return pt;
            }
            double dLat = transformLat(pt.X - 105.0, pt.Y - 35.0);
            double dLon = transformLon(pt.X - 105.0, pt.Y - 35.0);
            double radLat = pt.Y / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            Point retPt = new Point();
            retPt.Y = pt.Y + dLat;
            retPt.X = pt.X + dLon;
            return retPt;
        }

        // gcj02-84
        public static Point GCJ02ToWGS84(Point pt)
        {
            if (outofChina(pt))
            {
                return pt;
            }
            Point retPt = new Point();
            retPt.Y = pt.Y * 2.0 - WGS84ToGCJ02(pt).Y;
            retPt.X = pt.X * 2.0 - WGS84ToGCJ02(pt).X;
            return retPt;
        }

        public static Point WGS84ToWGSWeb(Point pt)
        {
            if (outofChina(pt))
            {
                return pt;
            }
            Point retPt = new Point();
            retPt.X = pt.X * 20037508.34 / 180.0;
            double y = Math.Log(Math.Tan((90 + pt.Y) * Math.PI / 360.0)) / (Math.PI / 180.0);
            retPt.Y = y * 20037508.34 / 180.0;
            return retPt;
        }

        public static Point WGSWebToWGS84(Point pt)
        {
            Point retPt = new Point();
            retPt.X = pt.X / 20037508.34 * 180.0;
            double y = pt.Y / 20037508.34 * 180.0;
            retPt.Y = 180.0 / Math.PI * (2 * Math.Atan(Math.Exp(y * Math.PI / 180.0)) - Math.PI / 2);
            return retPt;
        }

        private static double x_pi = Math.PI * 3000.0 / 180.0;

        public static Point GCJ02ToBD09(Point pt)
        {
            Point retPt = new Point();
            double z = Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y) + 0.00002 * Math.Sin(pt.Y * x_pi);
            double theta = Math.Atan2(pt.Y, pt.X) + 0.000003 * Math.Cos(pt.X * x_pi);
            retPt.X = z * Math.Cos(theta) + 0.0065;
            retPt.Y = z * Math.Sin(theta) + 0.006;
            return retPt;
        }

        public static Point BD09ToGCJ02(Point pt)
        {
            Point retPt = new Point();
            double x = pt.X - 0.0065, y = pt.Y - 0.006;
            double z = Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            retPt.X = z * Math.Cos(theta);
            retPt.Y = z * Math.Sin(theta);
            return retPt;
        }

        private static double[] MCBAND = { 12890594.86, 8362377.87, 5591021, 3481989.83, 1678043.12, 0 };
        private static int[] LLBAND = { 75, 60, 45, 30, 15, 0 };

        private static double[,] MC2LL = {
                           {1.410526172116255e-8, 0.00000898305509648872, -1.9939833816331, 200.9824383106796, -187.2403703815547, 91.6087516669843, -23.38765649603339, 2.57121317296198, -0.03801003308653, 17337981.2},
                           {-7.435856389565537e-9, 0.000008983055097726239, -0.78625201886289, 96.32687599759846, -1.85204757529826, -59.36935905485877, 47.40033549296737, -16.50741931063887, 2.28786674699375, 10260144.86},
                           {-3.030883460898826e-8, 0.00000898305509983578, 0.30071316287616, 59.74293618442277, 7.357984074871, -25.38371002664745, 13.45380521110908, -3.29883767235584, 0.32710905363475, 6856817.37},
                           {-1.981981304930552e-8, 0.000008983055099779535, 0.03278182852591, 40.31678527705744, 0.65659298677277, -4.44255534477492, 0.85341911805263, 0.12923347998204, -0.04625736007561, 4482777.06},
                           {3.09191371068437e-9, 0.000008983055096812155, 0.00006995724062, 23.10934304144901, -0.00023663490511, -0.6321817810242, -0.00663494467273, 0.03430082397953, -0.00466043876332, 2555164.4},
                           {2.890871144776878e-9, 0.000008983055095805407, -3.068298e-8, 7.47137025468032, -0.00000353937994, -0.02145144861037, -0.00001234426596, 0.00010322952773, -0.00000323890364, 826088.5}
                           };

        private static double[,] LL2MC = {
                             {-0.0015702102444, 111320.7020616939, 1704480524535203, -10338987376042340, 26112667856603880, -35149669176653700, 26595700718403920, -10725012454188240, 1800819912950474, 82.5},
                             {0.0008277824516172526, 111320.7020463578, 647795574.6671607, -4082003173.641316, 10774905663.51142, -15171875531.51559, 12053065338.62167, -5124939663.577472, 913311935.9512032, 67.5},
                             {0.00337398766765, 111320.7020202162, 4481351.045890365, -23393751.19931662, 79682215.47186455, -115964993.2797253, 97236711.15602145, -43661946.33752821, 8477230.501135234, 52.5},
                             {0.00220636496208, 111320.7020209128, 51751.86112841131, 3796837.749470245, 992013.7397791013, -1221952.21711287, 1340652.697009075, -620943.6990984312, 144416.9293806241, 37.5},
                             {-0.0003441963504368392, 111320.7020576856, 278.2353980772752, 2485758.690035394, 6070.750963243378, 54821.18345352118, 9540.606633304236, -2710.55326746645, 1405.483844121726, 22.5},
                             {-0.0003218135878613132, 111320.7020701615, 0.00369383431289, 823725.6402795718, 0.46104986909093, 2351.343141331292, 1.58060784298199, 8.77738589078284, 0.37238884252424, 7.45}
                         };

        public static Point BDWebToBD09(Point pt)
        {
            Point retPt = new Point();
            double x = Math.Abs(pt.X), y = Math.Abs(pt.Y);
            double[] cF = new double[MC2LL.GetLength(1)];
            for (int i = 0; i < MCBAND.Length; i++)
            {
                if (y >= MCBAND[i])
                {
                    for (int j = 0; j < MC2LL.GetLength(1); j++)
                    {
                        cF[j] = MC2LL[i, j];
                    }
                    break;
                }
            }
            x = cF[0] + cF[1] * Math.Abs(x);
            double cC = y / cF[9];
            y = cF[2] + cF[3] * cC + cF[4] * cC * cC + cF[5] * cC * cC * cC + cF[6] * cC * cC * cC * cC + cF[7] * cC * cC * cC * cC * cC + cF[8] * cC * cC * cC * cC * cC * cC;
            x *= (x < 0 ? -1 : 1);
            y *= (y < 0 ? -1 : 1);
            retPt.X = x;
            retPt.Y = y;
            return retPt;
        }

        public static Point BD09ToBDWeb(Point pt)
        {
            Point retPt = new Point();
            bool bt = false;
            double[] cF = new double[LL2MC.GetLength(1)];
            double x = _getLoop(pt.X, -180, 180);
            double y = _getRange(pt.Y, -74, 74);
            for (int i = 0; i < LLBAND.Length; i++)
            {
                if (y >= LLBAND[i])
                {
                    for (int j = 0; j < LL2MC.GetLength(1); j++)
                    {
                        cF[j] = LL2MC[i, j];
                    }
                    bt = true;
                    break;
                }
            }
            if (bt)
            {
                for (int i = LLBAND.Length - 1; i >= 0; i--)
                {
                    if (y <= -LLBAND[i])
                    {
                        for (int j = 0; j < LL2MC.GetLength(1); j++)
                        {
                            cF[j] = LL2MC[i, j];
                        }
                        break;
                    }
                }
            }
            x = cF[0] + cF[1] * Math.Abs(x);
            double cC = Math.Abs(y) / cF[9];
            y = cF[2] + cF[3] * cC + cF[4] * cC * cC + cF[5] * cC * cC * cC + cF[6] * cC * cC * cC * cC + cF[7] * cC * cC * cC * cC * cC + cF[8] * cC * cC * cC * cC * cC * cC;
            x *= (x < 0 ? -1 : 1);
            y *= (y < 0 ? -1 : 1);
            retPt.X = x;
            retPt.Y = y;
            return retPt;
        }

        public static double _getLoop(double lng, double min, double max)
        {
            while (lng > max)
            {
                lng -= max - min;
            }
            while (lng < min)
            {
                lng += max - min;
            }
            return lng;
        }

        public static double _getRange(double lat, double min, double max)
        {
            if (min != null)
            {
                lat = Math.Max(lat, min);
            }
            if (max != null)
            {
                lat = Math.Min(lat, max);
            }
            return lat;
        }
    }
}