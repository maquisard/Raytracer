using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public abstract class Shader
    {
        public const int TRUNCTATE = 0;
        public const int OFFSET = 1;

        public static Color LightColor { get; set; }
        public static float Alpha { get; set; }
        
        public float Ks { get; set; } //specular coefficient
        public float KsAlpha { get; set; }
        public int SMethod { get; set; }

        public abstract Color ComputeColor(Vector3 npe, Vector3 Nlh, Vector3 Nh);
    }
}
