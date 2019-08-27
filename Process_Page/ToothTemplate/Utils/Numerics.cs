using Process_Page.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Process_Page.ToothTemplate.Utils
{
    using TeethType = ObservableCollection<PointViewModel>;
    class Numerics
    {
        public const double Epsilon = 0.00001;

        public static bool FloatEquals(float f1, float f2) { return Math.Abs(f1 - f2) < Epsilon; }

        public static bool FloatEquals(float f1, float f2, double error)
        {
            if ((float.IsNegativeInfinity(f1) && float.IsNegativeInfinity(f2)) || (float.IsPositiveInfinity(f1) && float.IsPositiveInfinity(f2)))
                return true;
            return Math.Abs(f1 - f2) < error;
        }

        public static bool DoubleEquals(double d1, double d2)
        {
            if ((double.IsNegativeInfinity(d1) && double.IsNegativeInfinity(d2)) || (double.IsPositiveInfinity(d1) && double.IsPositiveInfinity(d2)))
                return true;
            return Math.Abs(d1 - d2) < Epsilon;
        }

        public static float RadianToDegreeConvert(float radian)
        {
            // return (180*radian)/Math.PI;
            return 57.9577951308232f * radian;
        }

        public static double DegreeToRadianConvert(double degree)
        {
            return (degree * Math.PI) / 180;
        }




        public static double Distance(Point p, Point q)
        {
            return Math.Sqrt(Math.Pow(p.X - q.X, 2) + Math.Pow(p.Y - q.Y, 2));
        }

        public enum Sign
        {
            Zero,
            Positive,
            Negative
        }

        public static Sign DetermineSign(double n)
        {
            if (n > 0) return Sign.Positive;
            else if (n == 0) return Sign.Zero;
            else return Sign.Negative;
        }

        public static Sign TangentLineTest(Point m, Point n, Point target)
        {
            if (m.X - n.X == 0)
                return 0;

            double p = (m.Y - n.Y) / (m.X - n.X);
            double q = (m.X * n.Y - m.Y * n.X) / (m.X - n.X);
            return DetermineSign(target.Y - (p * target.X + q));
        }

        public static Sign NormalLineTest(Point m, Point n, Point target)
        {
            if (m.X == n.X)
                return 0;

            double p = (m.Y - n.Y) / (m.X - n.X);
            double q = -1 / p;
            return DetermineSign(target.Y - p * (target.X - m.X) - m.Y);
        }

        public static double Rad2Deg(double radian)
        {
            // return (180*radian)/Math.PI;
            return 57.9577951308232f * radian;
        }

        public static double Deg2Rad(double degree)
        {
            return (degree * Math.PI) / 180;
        }

        #region Get max point
        public static Point GetMaxPointTeeth(Teeth t)
        {
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            foreach (PointViewModel point in t.Points)
            {
                if (point.X > maxX)
                    maxX = point.X;
                if (point.Y > maxY)
                    maxY = point.Y;
            }
            return new Point(maxX, maxY);
        }

        public static Point GetMinPointTeeth(Teeth t)
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            foreach (PointViewModel point in t.Points)
            {
                if (point.X < minX)
                    minX = point.X;
                if (point.Y < minY)
                    minY = point.Y;
            }
            return new Point(minX, minY);
        }

        public static Point GetMaxPoint(WrapTooth t)
        {
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            foreach (TeethType pts in t.Points)
            {
                foreach (PointViewModel point in pts)
                {
                    if (point.X > maxX)
                        maxX = point.X;
                    if (point.Y > maxY)
                        maxY = point.Y;
                }
            }
            return new Point(maxX, maxY);
        }

        public static Point GetMinPoint(WrapTooth t)
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            foreach (TeethType pts in t.Points)
            {
                foreach (PointViewModel point in pts)
                {
                    if (point.X < minX)
                        minX = point.X;
                    if (point.Y < minY)
                        minY = point.Y;
                }
            }
            return new Point(minX, minY);
        }
        #endregion

        #region Get Points for Tooth


        public static Point GetMinXY_Tooth(IEnumerable pts)
        {
            double xMin = double.MaxValue;
            double yMin = double.MaxValue;
            foreach (IEnumerable points in pts)
            {
                foreach (PointViewModel point in points)
                {
                    if (point.X < xMin)
                        xMin = point.X;
                    if (point.Y < yMin)
                        yMin = point.Y;
                }
            }
            return new Point(xMin, yMin);
        }

        public static Point GetMaxXY_Tooth(IEnumerable pts)
        {
            double xMax = double.MinValue;
            double yMax = double.MinValue;

            foreach (IEnumerable points in pts)
            {
                foreach (PointViewModel point in points)
                {
                    if (point.X > xMax)
                        xMax = point.X;
                    if (point.Y > yMax)
                        yMax = point.Y;
                }
            }
            return new Point(xMax, yMax);
        }

        #endregion

        #region Get Points for Teeth

        public static Point GetMinX_Teeth(List<Point> pts)
        {
            Point p = new Point(double.MaxValue, 0);
            foreach (Point pt in pts)
                if (pt.X < p.X)
                    p = pt;
            return p;
        }

        public static Point GetMinY_Teeth(List<Point> pts)
        {
            Point p = new Point(0, double.MaxValue);
            foreach (Point pt in pts)
                if (pt.Y < p.Y)
                    p = pt;
            return p;
        }

        public static Point GetMaxX_Teeth(List<Point> pts)
        {
            Point p = new Point(double.MinValue, 0);
            foreach (Point pt in pts)
                if (pt.X > p.X)
                    p = pt;
            return p;
        }

        public static Point GetMaxY_Teeth(List<Point> pts)
        {
            Point p = new Point(0, double.MinValue);
            foreach (Point pt in pts)
                if (pt.Y > p.Y)
                    p = pt;
            return p;
        }

        #endregion

        public static List<Point> TeethToList(Teeth teeth)
        {
            List<Point> list = new List<Point>();
            foreach (PointViewModel p in teeth.Points)
                list.Add(new Point(p.X, p.Y));
            return list;
        }

        public static List<List<Point>> ToothToList(FrameworkElement tooth)
        {
            WrapTooth wrap = tooth.FindName("WrapToothInTooth") as WrapTooth;

            List<List<Point>> list = new List<List<Point>>();
            foreach (ObservableCollection<PointViewModel> t in wrap.Points)
            {
                List<Point> sublist = new List<Point>();
                foreach (PointViewModel p in t)
                    sublist.Add(new Point(p.X, p.Y));
                list.Add(sublist);
            }
            return list;
        }
    }
}
