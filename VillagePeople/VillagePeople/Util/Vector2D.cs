using System;

namespace VillagePeople.Util
{
    public class Vector2D
    {
        public float X;
        public float Y;
        public float W;

        public Vector2D() : this(0, 0) { }
        public Vector2D(float x, float y, float w = 1)
        {
            X = x;
            Y = y;
            W = w;
        }

        public static Vector2D Scale(Vector2D v1, double target)
        {
            return new Vector2D(v1.X, v1.Y, v1.W).Scale(target);
        }

        public Vector2D Scale(double target)
        {
            double diff;
            var length = Length();

            if (length < target)
            {
                diff = 1 + Math.Abs(length / target - 1);
            }
            else
            {
                diff = target / length;
            }

            return this * Matrix.Scale((float)diff);
        }

        public Vector2D Normalize()
        {
            float length = Length();
            if (length != 0.0f)
            {
                X /= length;
                Y /= length;
                W /= length;
            }
            return this;
        }

        public Vector2D Truncate(float maX)
        {
            if (Length() > maX)
            {
                Normalize();
                return this * maX;
            }
            return this;
        }
        public float Length() => (float)Math.Sqrt(X * X + Y * Y);
        public float LengthSquared() => X * X + Y * Y;
        public Vector2D Clone() => new Vector2D(X, Y, W);

        public static Vector2D operator +(Vector2D v1, Vector2D v2) => new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        public static Vector2D operator -(Vector2D v1, Vector2D v2) => new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        public static Vector2D operator *(Vector2D v1, Vector2D v2) => new Vector2D(v1.X * v2.X, v1.Y * v2.Y);
        public static Vector2D operator /(Vector2D v1, Vector2D v2) => new Vector2D(v1.X / v2.X, v1.Y / v2.Y);

        public static Vector2D operator *(Vector2D v, float f) => new Vector2D(v.X * f, v.Y * f);
        public static Vector2D operator /(Vector2D v, float f) => new Vector2D(v.X / f, v.Y / f);
        public static Vector2D operator *(float f, Vector2D v) => v * f;
        public static Vector2D operator /(float f, Vector2D v) => v / f;

        public override string ToString() => $"({X}, {Y})";
    }
}
