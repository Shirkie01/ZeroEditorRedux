using System;
using System.Text.RegularExpressions;

namespace SWBF2
{
    public struct Quaternion
    {
        public readonly double x, y, z, w;

        public static readonly Quaternion identity = new Quaternion(0, 0, 0, 1);

        /// <summary>
        /// Parses doubles
        /// </summary>
        private static readonly Regex r = new Regex(@"(-?)(0|([1-9][0-9]*))(\.[0-9]+)?");

        public Quaternion(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Quaternion Parse(string s)
        {
            var values = r.Matches(s);

            if (values.Count != 4)
            {
                throw new FormatException("Quaternion must have 4 matches for regex: " + r.ToString());
            }

            var x = double.Parse(values[0].Value);
            var y = double.Parse(values[1].Value);
            var z = double.Parse(values[2].Value);
            var w = double.Parse(values[3].Value);

            return new Quaternion(x, y, z, w);
        }

        public override string ToString()
        {
            return string.Format("{0:F6}, {1:F6}, {2:F6}, {3:F6}", x, y, z, w);
        }
    }
}