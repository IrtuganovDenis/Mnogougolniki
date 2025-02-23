using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mnogougolniki
{
    abstract class Shape
    {
        static protected int r;
        protected double x, y;
        protected bool IsDragged, IsMoved;
        protected bool IsInHull;
        static protected Color color;
        protected Shape(double _x, double _y)
        {
            x = _x;
            y = _y;
            IsMoved = false;
        }

        static Shape()
        {
            color = Colors.Blue;
            r = 50;
        }

        public bool InHull
        {
            get
            {
                return IsInHull;
            }
            set
            {
                IsInHull = value;
            }
        }
        public bool Move
        {
            get
            {
                return IsMoved;
            }
            set
            {
                IsMoved = value;
            }
        }
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public abstract void Draw(DrawingContext dc);
        public abstract bool IsInside(double X, double Y);
    }
}