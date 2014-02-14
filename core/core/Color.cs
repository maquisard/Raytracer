using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core
{
    public class Color : Vector3
    {
        private static Color _black = new Color(0f, 0f, 0f);
        private static Color _white = new Color(1f, 1f, 1f);
        public static Color BLACK { get { return _black; } }
        public static Color WHITE { get { return _white; } }
 
        #region Channels
        public float R 
        {
            get { return X; }
            set { X = value; }
        }
        public float G
        {
            get { return Y; }
            set { Y = value; }
        }
        public float B
        {
            get { return Z; }
            set { Z = value; }
        }
        #endregion

        public Color() : this(Color.BLACK) { }
        public Color(Color color) : this(color.R, color.G, color.B) { }
        public Color(System.Drawing.Color color) : this(color.R, color.G, color.B) { }
        public Color(float r, float g, float b) : base(r, g, b) { }
        public Color(Vector3 v) : this(v.X, v.Y, v.Z) { }

        public static explicit operator System.Drawing.Color(Color color)
        {
            //color.R = float.IsNaN(color.R) ? 0f : color.R;
            //color.G = float.IsNaN(color.G) ? 0f : color.G;
            //color.B = float.IsNaN(color.B) ? 0f : color.B;

            int r = (int)(color.R > 1f ? 255 : color.R < 0f ? 0 : 255f * color.R);
            int g = (int)(color.G > 1f ? 255 : color.G < 0f ? 0 : 255f * color.G);
            int b = (int)(color.B > 1f ? 255 : color.B < 0f ? 0 : 255f * color.B);
            return System.Drawing.Color.FromArgb(r, g, b);
        }

        public override void PostLoad()
        {
            R = R / 255f;
            G = G / 255f;
            B = B / 255f;
        }
    }
}
