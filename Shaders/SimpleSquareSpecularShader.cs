using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class SimpleSquareSpecularShader : SimpleSharpShader
    {
        public SimpleSquareSpecularShader() { }

        public override Color ComputeColor(Light light, Vector3 pIntersection, Vector3 npe, Vector3 Nlh, Vector3 Nh)
        {
            if(!(light is SquareLight)) throw new ArgumentException("Square Specular Light only support");
            SquareLight sLight = light as SquareLight;
            Color color = Color.BLACK;
            float c = this.ComputeC(Nlh, Nh);
            c = (float)System.Math.Pow(c, Alpha);
            c = c < 0 ? c = 0 : c;

            Vector3 v = -1f * npe;
            Vector3 r = 2f * (Nh % v) * Nh - v;
            r.Normalize();

            float s = 0f;
            float denom = r % sLight.Nl;
            if (denom != 0) //Ray and plane are parallel
            {
                float t = ((sLight.Position - pIntersection) % sLight.Nl) / denom;
                if (t > 0) //there is an intersection with the light
                {
                    Vector3 pLightIntersection = pIntersection + r * t;
                    float x = 2f * ((pLightIntersection - sLight.Position) % sLight.N0) / sLight.Sx;
                    float y = 2f * ((pLightIntersection - sLight.Position) % sLight.N1) / sLight.Sy;
                    if (Math.Abs(x) <= 1 && Math.Abs(y) <= 1)
                    {
                        s = 1f;
                    }
                }
            }
            s = SMethod == TRUNCTATE ? (s < 0 ? 0 : s) : (s + 1f) / 2f;
            s = (float)System.Math.Pow(s, KsAlpha);
            s = s > CUTOFF ? 1f : 0f;
            color = (Color0 * (1f - c) + Color1 * c) as Color;
            color = color * (1f - s * Ks) + Color2 * s * KsAlpha as Color;
            return light.Color * color as Color;
        }
    }
}
