using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class SimpleSharpShader : SimpleShader
    {
        public float CUTOFF { get; set; }

        public SimpleSharpShader() { }
        public override Color ComputeColor(Light light, Vector3 Ph, Vector3 npe, Vector3 Nlh, Vector3 Nh)
        {
            Color color = Color.BLACK;
            float c = this.ComputeC(Nlh, Nh);
            c = (float)System.Math.Pow(c, Alpha);
            c = c < 0 ? c = 0 : c;

            Vector3 v = -1f * npe;
            Vector3 r = 2f * (Nh % v) * Nh - v;
            r.Normalize();
            float s = r % Nlh;
            s = SMethod == TRUNCTATE ? (s < 0 ? 0 : s) : (s + 1f) / 2f;
            s = (float)System.Math.Pow(s, KsAlpha);
            s = s > CUTOFF ? 1f : 0f;
            color = (Color0 * (1f - c) + Color1 * c) as Color;
            color = color * (1f - s * Ks) + Color2 * s * KsAlpha as Color;
            return light.Color * color as Color;
        }

    }
}
