using System;
using System.Text.RegularExpressions;

namespace SWBF2
{
    public struct Vector3
    {
        public readonly double x, y, z;

        public static readonly Vector3 zero = new Vector3(0, 0, 0);

        /// <summary>
        /// Parses doubles
        /// </summary>
        private static readonly Regex r = new Regex(@"(-?)(0|([1-9][0-9]*))(\.[0-9]+)?");

        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 Parse(string s)
        {
            var values = r.Matches(s);
            if (values.Count != 3)
            {
                throw new FormatException("Vector3 must have 3 matches for regex: " + r.ToString());
            }

            var x = double.Parse(values[0].Value);
            var y = double.Parse(values[1].Value);
            var z = double.Parse(values[2].Value);

            return new Vector3(x, y, z);
        }

        public static bool TryParse(string s, out Vector3 vector3)
        {
            try
            {
                vector3 = Parse(s);
                return true;
            }
            catch
            {
                vector3 = default(Vector3);
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("{0:F6}, {1:F6}, {2:F6}", x, y, z);
        }
    }
}