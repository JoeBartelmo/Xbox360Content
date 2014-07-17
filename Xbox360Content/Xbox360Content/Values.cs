using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360Content.Values
{
    public struct int24
    {
        static int24 max = new int24(8388607),
            min = new int24(-8388608);
        public static int24 MaxValue { get { return max; } }
        public static int24 MinValue { get { return min; } }
        int _val;
        public int Value
        {
            get { return _val; }
            set
            {
                if (value > 8388607 || value < -8388608)
                    throw new OverflowException();
                _val = value;
            }
        }
        public int24(int val)
        {
            if (val > 8388607 || val < -8388608)
                throw new OverflowException();
            _val = val;
        }

        #region conv
        public static implicit operator int(int24 i)
        {
            return i._val;
        }
        public static explicit operator int24(int i)
        {
            return (i > MaxValue) ? MaxValue :
                ((i < MinValue) ? MinValue : new int24(i));
        }
        public static int24 operator +(int24 i1, int24 i2) { return new int24(i1 + i2); }
        public static int24 operator *(int24 i1, int24 i2) { return new int24(i1 * i2); }
        public static int24 operator /(int24 i1, int24 i2) { return new int24(i1 / i2); }
        public static int24 operator -(int24 i1, int24 i2) { return new int24(i1 - i2); }
        public static int24 operator |(int24 i1, int24 i2) { return new int24((int)i1 | i2); }
        public static int24 operator &(int24 i1, int24 i2) { return new int24((int)i1 & i2); }
        public static int24 operator +(int i1, int24 i2) { return new int24(i1 + i2); }
        public static int24 operator *(int i1, int24 i2) { return new int24(i1 * i2); }
        public static int24 operator /(int i1, int24 i2) { return new int24(i1 / i2); }
        public static int24 operator -(int i1, int24 i2) { return new int24(i1 - i2); }
        public static int24 operator |(int i1, int24 i2) { return new int24(i1 | i2); }
        public static int24 operator &(int i1, int24 i2) { return new int24(i1 & i2); }
        #endregion
    }
}
