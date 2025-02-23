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
    sealed class Square : Shape
    {
        public Square(double x, double y) : base(x, y) { }

        public override bool IsInside(double X, double Y)
        {
            double r2 = r / (double)Math.Sqrt(2);
            return x - r2 <= X && X <= x + r2 && y - r2 <= Y && Y <= y + r2;
        }
        public override void Draw(DrawingContext dc)
        {
            Brush brush = new SolidColorBrush(color);
            Pen pen = new Pen(brush, 1, lineCap: PenLineCap.Square);
            double r2 = r / (double)Math.Sqrt(2);

            Point p1 = new Point(x - r2, y + r2);
            Point p2 = new Point(x + r2, y + r2);
            Point p3 = new Point(x + r2, y - r2);
            Point p4 = new Point(x - r2, y - r2);
            dc.DrawLine(pen, p1, p2);
            dc.DrawLine(pen, p2, p3);
            dc.DrawLine(pen, p3, p4);
            dc.DrawLine(pen, p4, p1);
        }
    }
}
