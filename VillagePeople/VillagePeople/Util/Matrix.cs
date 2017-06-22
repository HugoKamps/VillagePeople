using System;

namespace VillagePeople.Util {
    internal class Matrix {
        public float[,] M;

        public Matrix(float m11, float m12, float m13,
            float m21, float m22, float m23,
            float m31, float m32, float m33) {
            M = new float[3, 3];
            M[0, 0] = m11;
            M[0, 1] = m12;
            M[0, 2] = m13;
            M[1, 0] = m21;
            M[1, 1] = m22;
            M[1, 2] = m23;
            M[2, 0] = m31;
            M[2, 1] = m32;
            M[2, 2] = m33;
        }

        public Matrix(float f = 1.0f) :
            this(f, 0, 0,
                0, f, 0,
                0, 0, f) { }

        public Matrix(Vector2D vector2D) :
            this((int) vector2D.X, 0, 0,
                (int) vector2D.Y, 0, 0,
                (int) vector2D.W, 0, 0) { }

        // Function that converts a vector2d converted from a matrix
        public static Vector2D ToVector2D(Matrix m1) {
            return new Vector2D(m1.M[0, 0], m1.M[1, 0], m1.M[2, 0]);
        }

        // Function that returns a new default identity matrix
        public static Matrix Identity() {
            return new Matrix();
        }

        // Method that returns a matrix scaled to a given value
        public static Matrix Scale(float s) {
            return new Matrix(s) {M = {[2, 2] = 1}};
        }

        // Method that returns this matrix rotated by a given amount of degrees
        public Matrix Rotate(float deg) {
            var rad = Math.PI * deg / 180.0;
            var rotationMatrix = new Matrix(
                (float) Math.Cos(rad), (float) -Math.Sin(rad), 0,
                (float) Math.Sin(rad), (float) Math.Cos(rad), 0,
                0, 0, 1
            );
            return this * rotationMatrix;
        }

        // Function that multiplies two matrixes and returns the result
        public static Matrix operator *(Matrix m1, Matrix m2) {
            var m = new Matrix();
            for (var r = 0; r < 3; r++)
            for (var c = 0; c < 3; c++) {
                var sum = 0f;
                for (var i = 0; i < 3; i++) {
                    sum += m1.M[r, i] * m2.M[i, c];
                    m.M[r, c] = sum;
                }
            }
            return m;
        }

        // Function that adds two matrixes and returns the result
        public static Matrix operator +(Matrix m1, Matrix m2) {
            var m = new Matrix();
            for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                m.M[i, j] = m1.M[i, j] + m2.M[i, j];
            return m;
        }

        // Function that subtracts one matrix from another and returns the result
        public static Matrix operator -(Matrix m1, Matrix m2) {
            var m = new Matrix();
            for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                m.M[i, j] = m1.M[i, j] - m2.M[i, j];
            return m;
        }

        // Function that returns a matrix multiplied with a given float value
        public static Matrix operator *(Matrix m1, float f) {
            var m = new Matrix();
            for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                m.M[i, j] = f * m1.M[i, j];
            return m;
        }

        // Function that returns a matrix multiplied with a given float value
        public static Matrix operator *(float f, Matrix m1) {
            var m = new Matrix();
            for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                m.M[i, j] = f * m1.M[i, j];
            return m;
        }

        // Function that returns a Vector2D multiplied with a matrix
        public static Vector2D operator *(Matrix m1, Vector2D v1) {
            return ToVector2D(m1 * new Matrix(v1));
        }

        public static Vector2D operator *(Vector2D v1, Matrix m1) {
            return ToVector2D(m1 * new Matrix(v1));
        }

        public Vector2D TransformToVector2D(Vector2D v) {
            var tempX = M[0, 1] * v.X + M[1, 1] * v.Y + M[2, 1];
            var tempY = M[0, 2] * v.X + M[1, 2] * v.Y + M[2, 2];

            return new Vector2D(tempX, tempY);
        }

        public override string ToString() {
            var s = "";
            for (var i = 0; i < 3; i++) {
                s += "|";
                for (var j = 0; j < 3; j++)
                    s += M[i, j] + " ";
                s += "|\n";
            }
            return s;
        }
    }
}