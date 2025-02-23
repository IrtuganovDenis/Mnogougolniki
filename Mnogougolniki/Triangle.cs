using Avalonia;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mnogougolniki
{
    sealed class Triangle : Shape
    {
        public Triangle(double x, double y) : base(x, y)
        {
        }

        public override bool IsInside(double X, double Y)
        {
            var point = new Point(X, Y);
            Point p1 = new Point(x, y - r);
            Point p2 = new Point(x - r * (double)Math.Sqrt(3) / 2, y + (double)r / 2);
            Point p3 = new Point(x + r * (double)Math.Sqrt(3) / 2, y + (double)r / 2);
            double S = Shape.r * Shape.r * Math.Sqrt(3) * 3 * 0.25;
            return Math.Abs(S - Heron(p1, p2, point) - Heron(p1, p3, point) - Heron(p2, p3, point)) <= 0.1;
        }

        public override void Draw(DrawingContext dc)
        {
            Brush brush = new SolidColorBrush(color);
            Pen pen = new(brush, 1, lineCap: PenLineCap.Square);
            Point p1 = new Point(x, y - r);
            Point p2 = new Point(x - r * (double)Math.Sqrt(3) / 2, y + (double)r / 2);
            Point p3 = new Point(x + r * (double)Math.Sqrt(3) / 2, y + (double)r / 2);

            dc.DrawLine(pen, p1, p2);
            dc.DrawLine(pen, p2, p3);
            dc.DrawLine(pen, p1, p3);
        }

        private static double Heron(Point p1, Point p2, Point p3)
        {
            var a = Point.Distance(p1, p2);
            var b = Point.Distance(p1, p3);
            var c = Point.Distance(p2, p3);
            var p = (a + b + c) / 2;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
    }
}