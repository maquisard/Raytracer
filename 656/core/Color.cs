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
        public Color(float r, float g, float b) : base(r, g, b) { }

    }
}
