using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class SimpleShader : Shader
    {
        public float[] Color0 { get; set; }
        public float[] Color1 { get; set; }

        public SimpleShader() { }
        public override float[] ComputeColor()
        {
            float[] color = Math.Zero3;
            float c = this.ComputeC();
            c = (float)System.Math.Pow(c, Alpha);
            c = c < 0 ? c = 0 : c;
            color = Math.Add(
                                Math.Multiply((1f - c), Color0),
                                Math.Multiply(c, Color1)
                            );
            color = Math.Multiply(color, LightColor);
            return color;
        }

        protected virtual float ComputeC()
        {
            return Math.Dot(Nlh, Nh);
        }
    }
}
