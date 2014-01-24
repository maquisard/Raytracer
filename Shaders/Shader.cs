using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public abstract class Shader
    {
        public float[] Nlh { get; set; } //vector toward the light Unit Vector
        public float[] Nh { get; set; }  //Normal vector from the intersection point
        public static float[] LightColor { get; set; }
        public static float Alpha { get; set; }

        public abstract float[] ComputeColor();
    }
}
