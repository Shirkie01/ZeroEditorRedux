using System;
using System.Text.RegularExpressions;

namespace SWBF2
{
    public struct Quaternion
    {
        public readonly double w, x, y, z;

        public static readonly Quaternion identity = new Quaternion(0, 0, 0, 1);

        /// <summary>
        /// Parses doubles
        /// </summary>
        private static readonly Regex r = new Regex(@"(-?)(0|([1-9][0-9]*))(\.[0-9]+)?");

        public Quaternion(double w, double x, double y, double z)
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
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

            return new Quaternion(w, x, y, z);
        }

        public override string ToString()
        {
            return string.Format("{0:F6}, {1:F6}, {2:F6}, {3:F6}", w, x, y, z);
        }

        public Vector3 ToEulerAngles()
        {
            var eulerX = Math.Atan2(2 * (w * x + y * z), 1 - 2 * (x * x + y * y));
            var eulerY = Math.Asin(2 * (w * y - z * x));
            var eulerZ = Math.Atan2(2 * (w * z + x * y), 1 - 2 * (y * y + z * z));

            return new Vector3(eulerX, eulerY, eulerZ);
        }
    }
}