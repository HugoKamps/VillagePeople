using System;

namespace VillagePeople.Util
{
    public class Vector2D
    {
        public float W;
        public float X;
        public float Y;

        public Vector2D() : this(0, 0)
        {
        }

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
                diff = 1 + Math.Abs(length / target - 1);
            else
                diff = target / length;

            return this * Matrix.Scale((float)diff);
        }

        public Vector2D Normalize()
        {
            var length = Length();
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

        public static float Distance(Vector2D v1, Vector2D v2)
        {
            float dx = v2.X - v1.X;
            float dy = v2.Y - v1.Y;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public float Length() => (float)Math.Sqrt(X * X + Y * Y);
        public float LengthSquared() => X * X + Y * Y;
        public Vector2D Clone() => new Vector2D(X, Y, W);

        public static bool operator ==(Vector2D v1, Vector2D v2) => (v1.X == v2.X && v1.Y == v2.Y);
        public static bool operator !=(Vector2D v1, Vector2D v2) => (v1.X != v2.X || v1.Y != v2.Y);

        public static Vector2D operator +(Vector2D v1, Vector2D v2) => new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        public static Vector2D operator -(Vector2D v1, Vector2D v2) => new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        public static Vector2D operator *(Vector2D v1, Vector2D v2) => new Vector2D(v1.X * v2.X, v1.Y * v2.Y);
        public static Vector2D operator /(Vector2D v1, Vector2D v2) => new Vector2D(v1.X / v2.X, v1.Y / v2.Y);

        public static Vector2D operator *(Vector2D v, float f) => new Vector2D(v.X * f, v.Y * f);
        public static Vector2D operator /(Vector2D v, float f) => new Vector2D(v.X / f, v.Y / f);
        public static Vector2D operator *(float f, Vector2D v) => v * f;
        public static Vector2D operator /(float f, Vector2D v) => v / f;

        public override string ToString() => $"({X}, {Y})";
        //public static bool operator ==(Vector2D v1, Vector2D v2) => v1.X == v2.X && v1.Y == v2.Y && v1.W == v2.W;
        //public static bool operator !=(Vector2D v1, Vector2D v2) => v1.X != v2.X || v1.Y != v2.Y || v1.W != v2.W;

        public static Vector2D Vec2DRotateAroundOrigin(Vector2D v, float angle)
        {
            var m = new Matrix();

            m.Rotate(angle);

            return m.TransformToVector2D(v);
        }

        public static bool LineIntersection2D(Vector2D v1, Vector2D v2, Vector2D v3, Vector2D v4)
        {
            double rTop = (v1.Y - v3.Y) * (v4.X - v3.X) - (v1.X - v3.X) * (v4.Y - v3.Y);
            double sTop = (v1.Y - v3.Y) * (v2.X - v1.X) - (v1.X - v3.X) * (v2.Y - v1.Y);

            double bot
                = (v2.X - v1.X) * (v4.Y - v3.Y) - (v2.Y - v1.Y) * (v4.X - v3.X);

            if (bot == 0)
            {
                return false;
            }

            var invBot = 1.0 / bot;
            var r = rTop * invBot;
            var s = sTop * invBot;

            return r > 0 && r < 1 && s > 0 && s < 1;
        }

        public static bool LineIntersection2D(Vector2D v1, Vector2D v2, Vector2D v3, Vector2D v4, ref Vector2D point)
        {

            var rTop = (v1.Y - v3.Y) * (v4.X - v3.X) - (v1.X - v3.X) * (v4.Y - v3.Y);
            var rBot = (v2.X - v1.X) * (v4.Y - v3.Y) - (v2.Y - v1.Y) * (v4.X - v3.X);

            var sTop = (v1.Y - v3.Y) * (v2.X - v1.X) - (v1.X - v3.X) * (v2.Y - v1.Y);
            var sBot = (v2.X - v1.X) * (v4.Y - v3.Y) - (v2.Y - v1.Y) * (v4.X - v3.X);

            if (rBot == 0 || sBot == 0)
            {
                return false;
            }

            var r = rTop / rBot;
            var s = sTop / sBot;

            if (!(r > 0) || !(r < 1) || !(s > 0) || !(s < 1)) return false;
            point = v1 + r * (v2 - v1);

            return true;
        }
    }
}