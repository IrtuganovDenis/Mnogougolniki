using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using System.Security.Cryptography;

namespace Mnogougolniki
{
    public class CustomControl : UserControl
    {
        private List<Shape> shapes = [
            new Circle(400, 400),
            new Square(300, 700),
            new Triangle(200, 450),
            new Square(600, 500)
            ];

        double prx, pry;

        public void Delete(double X, double Y)
        {
            int i = shapes.Count() - 1;
            while (i >= 0)
            {
                if (shapes[i].IsInside(X, Y))
                {
                    shapes.Remove(shapes[i]);
                    break;
                }
                --i;
            }
            InvalidateVisual();
        }

        public void Click(double X, double Y)
        {
            //Console.WriteLine("click");
            bool in_shapes = false;
            foreach (var shape in shapes)
            {
                if (shape.IsInside(X, Y))
                {
                    in_shapes = true;
                    prx = X;
                    pry = Y;
                    shape.Move = true;
                }
            }

            if (!in_shapes)
            {
                shapes.Add(new Circle(X, Y));
                shapes.Last().InHull = false;
                //CreateHullZavriev();
                CreateHullJarvis();
                Console.WriteLine(1);
                
                if (shapes.Last().InHull != true)
                {
                    shapes.Remove(shapes.Last());
                    foreach (Shape s in shapes)
                    {
                        s.Move = true;
                    }
                    prx = X;
                    pry = Y;
                }
            }
            InvalidateVisual();
        }

        public void Move(double X, double Y)
        {
            //Console.WriteLine("move");
            foreach (var shape in shapes)
            {
                if (shape.Move)
                {
                    shape.X += X - prx;
                    shape.Y += Y - pry;
                }

            }
            prx = X;
            pry = Y;
            InvalidateVisual();
        }

        public void Release(double X, double Y)
        {
            //Console.WriteLine("realise");
            foreach (var shape in shapes)
            {
                if (shape.Move)
                {
                    shape.Move = false;
                    
                }
                //shape.InHull = false;
            }
            //CreateHullZavriev();
            CreateHullJarvis();
            foreach (Shape s in shapes.ToList())
            {
                if (!s.InHull)
                {
                    shapes.Remove(s);
                }
            }
            InvalidateVisual();
        }

        public void CreateHullZavriev()
        {
            foreach (Shape s in shapes)
            {
                s.InHull = false;
            }
            int i = 0;
            foreach (Shape s1 in shapes)
            {
                int j = 0;
                foreach (Shape s2 in shapes)
                {
                    if (i == j)
                    {
                        ++j;
                        continue;
                    }
                    bool up = false, down = false;
                    int z = 0;
                    if (s1.X == s2.X)
                    {
                        foreach (Shape s3 in shapes)
                        {
                            if (i == z || j == z)
                            {
                                ++z;
                                continue;
                            }
                            if (s3.X > s1.X)
                            {
                                up = true;
                            }
                            else if (s3.X < s1.X)
                            {
                                down = true;
                            }
                            ++z;
                        }
                    }
                    else
                    {
                        double k = (s1.Y - s2.Y) / (s1.X - s2.X), b = s1.Y - k * s1.X;
                        z = 0;
                        foreach (Shape s3 in shapes)
                        {
                            if (i == z || j == z)
                            {
                                ++z;
                                continue;
                            }
                            double y = k * s3.X + b;
                            if (s3.Y > y)
                            {
                                up = true;
                            }
                            else if (s3.Y < y)
                            {
                                down = true;
                            }
                            ++z;
                        }
                    }
                    if (!up || !down)
                    {
                        s1.InHull = true;
                        s2.InHull = true;
                    }
                    
                    ++j;
                }
                ++i;
            }
        }
        private double getCos(Shape s1, Shape s2, Shape s3)
        {
            Point p1 = new Point(s1.X, s1.Y);
            Point p2 = new Point(s2.X, s2.Y);
            Point p3 = new Point(s3.X, s3.Y);
            double a = Point.Distance(p1, p2);
            double b = Point.Distance(p1, p3);
            double c = Point.Distance(p2, p3);

            return (a * a + c * c - b * b) / (2 * a * c);
        }

        private void toHull(Shape sh)
        {
            foreach (Shape s in shapes)
            {
                if (s == sh)
                {
                    s.InHull = true;
                }
            }
        }

        public void CreateHullJarvis()
        {
            foreach (Shape s in shapes)
            {
                s.InHull = false;
            }

            Pen pen = new Pen(new SolidColorBrush(Colors.Black), 1, lineCap: PenLineCap.Square);

            Shape frst = shapes[0];

            foreach (Shape s in shapes)
            {
                if (s.X < frst.X || (frst.X == s.X && s.Y < frst.Y))
                {
                    frst = s;
                }
            }

            toHull(frst);
            Shape sec = new Circle(frst.X, frst.Y - 1);
            Shape last = null;
            double maxcos = -2;

            foreach (Shape s in shapes)
            {
                if (s == frst || s == sec) continue;
                var a = getCos(frst, sec, s);
                if (a > maxcos)
                {
                    last = s;
                    maxcos = a;
                }
            }

            sec = last;
            toHull(last);
            Shape start = frst;

            while (true)
            {
                double mincos = 2;
                foreach (Shape s in shapes)
                {
                    if (s == start || s == sec) continue;
                    var a = getCos(start, sec, s);
                    if (mincos > a)
                    {
                        last = s;
                        mincos = a;
                    }
                }

                start = sec;
                sec = last;
                toHull(last);

                if (last == frst) break;
            }
        }

        public void DrawHullJarvis(DrawingContext drawingContext)
        {
            foreach (Shape s in shapes)
            {
                s.InHull = false;
            }

            Pen pen = new Pen(new SolidColorBrush(Colors.Black), 1, lineCap: PenLineCap.Square);

            Shape frst = shapes[0];

            foreach (Shape s in shapes)
            {
                if (s.X < frst.X || (frst.X == s.X && s.Y < frst.Y))
                {
                    frst = s;
                }
            }

            toHull(frst);
            Shape sec = new Circle(frst.X, frst.Y - 1);
            Shape last = null;
            double maxcos = -2;

            foreach (Shape s in shapes)
            {
                if (s == frst || s == sec) continue;
                var a = getCos(frst, sec, s);
                if (a > maxcos)
                {
                    last = s;
                    maxcos = a;
                }
            }

            sec = last;
            toHull(last);
            drawingContext.DrawLine(pen, new Point(frst.X, frst.Y), new Point(sec.X, sec.Y));
            Shape start = frst;

            while (true)
            {
                double mincos = 2;
                foreach (Shape s in shapes)
                {
                    if (s == start || s == sec) continue;
                    var a = getCos(start, sec, s);
                    if (mincos > a)
                    {
                        last = s;
                        mincos = a;
                    }
                }

                start = sec;
                sec = last;
                toHull(last);
                drawingContext.DrawLine(pen, new Point(start.X, start.Y), new Point(sec.X, sec.Y));
                
                if (last == frst) break;
            }
        }
        
        
        
        public void DrawHullZavriev(DrawingContext drawingContext)
        {
            int i = 0;
            foreach (Shape s1 in shapes)
            {
                int j = 0;
                foreach (Shape s2 in shapes)
                {
                    if (i == j)
                    {
                        ++j;
                        continue;
                    }
                    bool up = false, down = false;
                    int z = 0;
                    if (s1.X == s2.X)
                    {
                        foreach (Shape s3 in shapes)
                        {
                            if (i == z || j == z)
                            {
                                ++z;
                                continue;
                            }
                            if (s3.X > s1.X)
                            {
                                up = true;
                            }
                            else if (s3.X < s1.X)
                            {
                                down = true;
                            }
                            ++z;
                        }
                    }
                    else
                    {
                        double k = (s1.Y - s2.Y) / (s1.X - s2.X), b = s1.Y - k * s1.X;
                        z = 0;
                        foreach (Shape s3 in shapes)
                        {
                            if (i == z || j == z)
                            {
                                ++z;
                                continue;
                            }
                            double y = k * s3.X + b;
                            if (s3.Y > y)
                            {
                                up = true;
                            }
                            else if (s3.Y < y)
                            {
                                down = true;
                            }
                            ++z;
                        }
                    }
                    if (!up || !down)
                    {
                        s1.InHull = true;
                        s2.InHull = true;
                        Pen p = new Pen(new SolidColorBrush(Colors.Black), 1, lineCap: PenLineCap.Square);
                        drawingContext.DrawLine(p, new Point(s1.X, s1.Y), new Point(s2.X, s2.Y));
                    }
                    ++j;
                }
                ++i;
            }
        }


        public override void Render(DrawingContext drawingContext)
        {
            /*
            foreach (var s in shapes)
            {
                if (!s.InHull)
                {
                    shapes.Remove(s);
                }
            }
            */

            foreach (var s in shapes)
            {
                s.Draw(drawingContext);
            }

            if (shapes.Count() >= 3)
            {
                //DrawHullZavriev(drawingContext);
                DrawHullJarvis(drawingContext);
            }
        }
    }
}

