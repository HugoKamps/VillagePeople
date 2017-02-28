using System;

namespace VillagePeople.Util
{

    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double W { get; set; }

        public Vector2D() : this(0, 0, 0)
        {
        }

        public Vector2D(double x, double y, double w = 0)
        {
            X = x;
            Y = y;
            W = w;
        }

        public double Length() => Math.Sqrt(X * X + Y * Y);
        public double LengthSquared() => X * X + Y * Y;

        public Vector2D Add(Vector2D v)
        {
            X += v.X;
            Y += v.Y;
            W += v.W;
            return this;
        }

        public Vector2D Sub(Vector2D v)
        {
            X -= v.X;
            Y -= v.Y;
            W -= v.W;
            return this;
        }

        public Vector2D Multiply(Vector2D v)
        {
            X *= v.X;
            Y *= v.Y;
            W *= v.W;
            return this;
        }

        public Vector2D Multiply(double value)
        {
            X *= value;
            Y *= value;
            W *= value;
            return this;
        }

        public Vector2D Divide(double value) => Multiply(1.0 / value);

        public Vector2D Normalize()
        {
            double length = Length();
            if (length != 0)
            {
                X /= length;
                Y /= length;
            }
            return this;
        }

        public Vector2D Truncate(double maX)
        {
            if (Length() > maX)
            {
                Normalize();
                Multiply(maX);
            }
            return this;
        }

        public Vector2D Clone() => new Vector2D(X, Y);
        public override string ToString() => $"({X},{Y})";
    }


}
