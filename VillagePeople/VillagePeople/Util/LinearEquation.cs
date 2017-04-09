using System;

namespace VillagePeople.Util
{
    public class LinearEquation
    {
        private float _dX, _dY, _c, _maxX, _maxY, _minX, _minY;
        public LineType Type;

        public LinearEquation(Vector2D a, Vector2D b)
        {
            _maxX = Math.Max(a.X, b.X);
            _maxY = Math.Max(a.Y, b.Y);
            _minX = Math.Min(a.X, b.X);
            _minY = Math.Min(a.Y, b.Y);

            _dX = b.X - a.X;
            _dY = b.Y - a.Y;

            if (_dX == 0)
            {
                Type = LineType.Vertical;
                return;
            }
            if (_dY == 0)
            {
                Type = LineType.Horizontal;
                return;
            }

            Type = LineType.Slanted;
            _c = a.Y - a.X * (_dY / _dX);
        }

        public float F(float x)
        {
            if (Type == LineType.Horizontal && x >= _minX && x <= _maxX) // Line is horizontal and x is within range
                return _minY; //  return y
            if (Type == LineType.Vertical && x == _minX) // Line is vertical and line is on x
                return (_minY + _maxY) / 2; //  should actually return everything between minY and maxY
            if (x >= _minX && x <= _maxX) // Line is slanted and x is within range
                return _dY / _dX * x + _c; //  return y
            return float.MinValue; // Error => returns min value of float
        }

        public bool Intersects(LinearEquation f2)
        {
            if (Type == LineType.Horizontal && f2.Type == LineType.Horizontal)
                return F(_minX) == f2.F(_minX);
            if (Type == LineType.Vertical && f2.Type == LineType.Vertical)
            {
                if (_minX != f2._minX)
                    return false;
                if (_minX > f2._maxX || _maxX < f2._minX)
                    return false;
                return true;
            }
            if (Type == LineType.Slanted && f2.Type == LineType.Slanted)
            {
                float derivedC, derivedDYoverDx, derivedX;

                derivedC = f2._c - _c;
                derivedDYoverDx = _dY / _dX / (f2._dY / f2._dX);
                derivedX = derivedC / derivedDYoverDx;

                return F(derivedX) != f2.F(derivedX);
            }

            if (Type == LineType.Slanted && f2.Type == LineType.Horizontal)
            {
                float derivedC, derivedX;

                derivedC = f2._minY - _c;
                derivedX = derivedC / (_dY / _dX);

                return derivedX >= f2._minX && derivedX <= f2._maxX && F(derivedX) != float.MinValue;
            }

            if (Type == LineType.Horizontal && f2.Type == LineType.Slanted)
            {
                float derivedC, derivedX;

                derivedC = _minY - f2._c;
                derivedX = derivedC / (f2._dY / f2._dX);

                return derivedX >= _minX && derivedX <= _maxX && f2.F(derivedX) != float.MinValue;
            }

            if (Type == LineType.Vertical)
            {
                if (f2.Type == LineType.Horizontal)
                {
                    if (f2._minY < _minY || f2._minY > _maxY)
                        return false;
                    if (f2._minX > _minX || f2._maxX < _maxX)
                        return false;
                    return true;
                }
                // f2 must be slanted
                return f2.F(_minX) <= _maxY && f2.F(_minX) >= _minY;
            }

            // f2 must be Vertical
            if (Type == LineType.Horizontal)
            {
                if (_minY < f2._minY || _minY > f2._maxY)
                    return false;
                if (_minX > f2._minX || _maxX < f2._maxX)
                    return false;
                return true;
            }
            // this must be slanted
            return F(f2._minX) <= f2._maxY && F(f2._minX) >= f2._minY;
        }

        public override string ToString()
        {
            if (Type == LineType.Horizontal)
                return " f( { " + _minX + " .. " + _maxX + " } ) = " + _minY + " ";
            if (Type == LineType.Vertical)
                return " f( " + _minX + " ) = { " + _minY + " .. " + _maxY + " } ";
            return " f(x) = (" + _dY + "/" + _dX + ")x + " + _c + " ";
        }
    }

    public enum LineType
    {
        Horizontal,
        Vertical,
        Slanted
    }
}