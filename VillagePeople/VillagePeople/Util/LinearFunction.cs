using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillagePeople.Util
{
    public class LinearFunction
    {
        private float dXY, c, maxX, maxY, minX, minY;

        public LinearFunction(Vector2D A, Vector2D B)
        {
            maxX = Math.Max(A.X, B.X);
            maxY = Math.Max(A.Y, B.Y);
            minX = Math.Min(A.X, B.X);
            minY = Math.Min(A.Y, B.Y);

            var dX = B.X - A.X;
            var dY = B.Y - A.Y;
            
            if (dY == 0) // Prevent division by 0
            {
                dX = 0;
                dY = 1;
            }

            dXY = (dX / dY);

            c = A.X - A.Y * dXY;
        }

        public float F(float x)
        {
            if (x >= minX && x <= maxX)
            {
                var y = dXY * x + c;

                if (y >= minY && y <= maxY)
                {
                    return y;
                }
            }

            return float.MinValue;
        }

        public bool Intersects(LinearFunction f2)
        {
            float newC, newdXOverdY, newX;

            newC = c - f2.c;
            newdXOverdY = dXY - f2.dXY;

            newX = newC / newdXOverdY;

            if (F(newX) != float.MinValue && f2.F(newX) != float.MinValue)
                return true;
            return false;
        }

        public override string ToString()
        {
            return "f(x) = " + dXY + "x + " + c;
        }
    }
}
