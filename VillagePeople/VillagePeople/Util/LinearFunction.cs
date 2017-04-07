using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillagePeople.Util
{
    public class LinearFunction
    {
        private float dX, dY, c, maxX, maxY, minX, minY;
        public bool Horizontal;

        public LinearFunction(Vector2D A, Vector2D B)
        {
            maxX = Math.Max(A.X, B.X);
            maxY = Math.Max(A.Y, B.Y);
            minX = Math.Min(A.X, B.X);
            minY = Math.Min(A.Y, B.Y);

            dX = B.X - A.X;
            dY = B.Y - A.Y;

            if (dX != 0)
                Horizontal = true;
            
            if (dY == 0) // Prevent division by 0
            {
                dY = dX;
                dX = 0;
            }
            c = A.X - A.Y * (dX / dY);
        }

        public float F(float x)
        {
            if (x < minX || x > maxX)
                return float.MinValue;

            if ((dX / dY) == 0)
                return minY;

            if (x >= minX && x <= maxX)
            {
                var y = (dX / dY) * x + c;

                if (y >= minY && y <= maxY)
                {
                    return y;
                }
            }

            return float.MinValue;
        }

        public static bool IntersectsVerticalLine(LinearFunction line, LinearFunction verticalLine)
        {
            var y = line.F(verticalLine.minX);
            return (y >= verticalLine.minY) && (y <= verticalLine.maxY);
        }

        public bool Intersects(LinearFunction f2)
        {
            if (!Horizontal && !f2.Horizontal)
            {
                bool res= 
                    minX == f2.minX &&                          // on same line and
                    ((minY >= f2.minY && minY <= f2.maxY) ||    // minY in range (f2.minY, f2.maxY) or
                    (maxY >= f2.minY && maxY <= f2.maxY));      // maxY in range (f2.minY, f2.maxY)
                return res;
            }
            else if (Horizontal && !f2.Horizontal)
            {
                return IntersectsVerticalLine(this, f2);
            }
            else if (!Horizontal && f2.Horizontal)
            {
                return IntersectsVerticalLine(f2, this);
            }
            else
            {
                float newC, newdXOverdY, newX;

                newC = c - f2.c;
                newdXOverdY = (dX / dY) - (f2.dX / f2.dY);

                if (newdXOverdY == 0)
                    return false;

                newX = newC / newdXOverdY;

                if (F(newX) != float.MinValue && f2.F(newX) != float.MinValue)
                    return true;
                return false;
            }
        }

        public override string ToString()
        {
            if (!Horizontal)
                return " f(x) = {" + minY + " ... " + maxY + "} ";
            else if (dX / dY == 0)
                return " f(x) = " + c + " ";
            return " f(x) = (" + dX + "/" + dY + ")x + " + c + " ";
        }
    }
}
