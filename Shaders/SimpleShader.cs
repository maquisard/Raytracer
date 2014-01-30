using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class SimpleShader : Shader
    {
        public Color Color0 { get; set; }
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }

        public SimpleShader() { }
        public override Color ComputeColor(Vector3 npe, Vector3 Nlh, Vector3 Nh)
        {
            Color color = Color.BLACK;
            float c = this.ComputeC(Nlh, Nh);
            c = (float)System.Math.Pow(c, Alpha);
            c = c < 0 ? c = 0 : c;

            Vector3 v = -1f * npe;
            Vector3 r = 2f * (Nh % v) * Nh - v;
            float s = r % Nlh;
            s = SMethod == TRUNCTATE ? (s < 0 ? 0 : s) : (s + 1f) / 2f;
            s = (float)System.Math.Pow(s, KsAlpha);
            color = (Color0 * (1f - c) + Color1 * c) as Color;
            color = color * (1f - s * Ks) + Color2 * s * KsAlpha as Color;
            return LightColor * color as Color;
        }

        protected virtual float ComputeC(Vector3 Nlh, Vector3 Nh) //the diffuse coefficient
        {
            return Nlh % Nh;
        }
    }
}
