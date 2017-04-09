using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillagePeople.Util
{
    public class LinearEquation
    {
        private float dX, dY, c, maxX, maxY, minX, minY;
        public LineType Type;

        public LinearEquation(Vector2D A, Vector2D B)
        {
            maxX = Math.Max(A.X, B.X);
            maxY = Math.Max(A.Y, B.Y);
            minX = Math.Min(A.X, B.X);
            minY = Math.Min(A.Y, B.Y);

            dX = B.X - A.X;
            dY = B.Y - A.Y;

            if (dX == 0)
            {
                Type = LineType.Vertical;
                return;
            }
            else if (dY == 0)
            {
                Type = LineType.Horizontal;
                return;
            }

            Type = LineType.Slanted;
            c = A.X - A.Y * (dY / dX);
        }

        public float F(float x)
        {
            if (Type == LineType.Horizontal && x >= minX && x <= maxX)  // Line is horizontal and x is within range
                return minY;                                            //  return y
            else if (Type == LineType.Vertical && x == minX)            // Line is vertical and line is on x
                return (minY + maxY) / 2;                               //  should actually return everything between minY and maxY
            else if (x >= minX && x <= maxX)                            // Line is slanted and x is within range
                return (dY / dX) * x + c;                               //  return y
            return float.MinValue;                                      // Error => returns min value of float
        }

        public static bool IntersectsVerticalLine(LinearEquation line, LinearEquation verticalLine)
        {
            var y = line.F(verticalLine.minX);
            return (y >= verticalLine.minY) && (y <= verticalLine.maxY);
        }

        public bool Intersects(LinearEquation f2)
        {
            if (Type == LineType.Horizontal && f2.Type == LineType.Horizontal)
                return (F(minX) == f2.F(minX));
            if (Type == LineType.Vertical && f2.Type == LineType.Vertical)
            {
                if (minX != f2.minX)
                    return false;
                if (minX > f2.maxX || maxX < f2.minX)
                    return false;
                return true;
            }
            if (Type == LineType.Slanted && f2.Type == LineType.Slanted ||
                Type == LineType.Slanted && f2.Type == LineType.Horizontal ||
                Type == LineType.Horizontal && f2.Type == LineType.Slanted)
            {
                float derivedC, derivedDYoverDX, derivedX;

                derivedC = f2.c - c;
                derivedDYoverDX = (dY / dX) / (f2.dY / f2.dX);
                derivedX = derivedC / derivedDYoverDX;

                return (F(derivedX) != f2.F(derivedX));
            }
            
            if (Type == LineType.Vertical)
            {
                if (f2.Type == LineType.Horizontal)
                {
                    if (f2.minY < minY || f2.minY > maxY)
                        return false;
                    if (f2.minX > minX || f2.maxX < maxX)
                        return false;
                    return true;
                }
                // f2 must be slanted
                return f2.F(minX) <= maxY && f2.F(minX) >= minY;
            }

            // f2 must be Vertical
            if (Type == LineType.Horizontal)
            {
                if (minY < f2.minY || minY > f2.maxY)
                    return false;
                if (minX > f2.minX || maxX < f2.maxX)
                    return false;
                return true;
            }
            // this must be slanted
            return F(f2.minX) <= f2.maxY && F(f2.minX) >= f2.minY;
        }

        public override string ToString()
        {
            if (Type == LineType.Horizontal)
                return " f( { " + minX + " .. " + maxX + " } ) = " + minY + " ";
            else if (Type == LineType.Vertical)
                return " f( " + minX + " ) = { " + minY + " .. " + maxY + " } ";
            return " f(x) = (" + dY + "/" + dX + ")x + " + c + " ";
        }
    }

    public enum LineType
    {
        Horizontal,
        Vertical,
        Slanted
    }
}
