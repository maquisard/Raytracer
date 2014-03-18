using System;
using System.Collections.Generic;
using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.lights;


namespace edu.tamu.courses.imagesynth.shaders
{
    public class Phong : Shader
    {
        public const int TRUNCATE = 0;
        public const int GOOCH = 1;

        public Phong() { }

        public Color Color0 { get; set; }
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }
        public float KsAlpha { get; set; }
        public float Ks { get; set; }
        public float Alpha { get; set; }
        public int SMethod { get; set; }

        public override Color ComputeColor(Light light, Vector3 Ph, Vector3 npe, Vector3 Nlh, Vector3 Nh)
        {
            Color color = Color.BLACK;
            float c = this.ComputeC(Nlh, Nh);

            float s = this.ComputeS(npe, Nh, Nlh);

            color = new Color(Color0 * (1f - c) + Color1 * c);
            color = new Color(color * (1f - s * Ks) + Color2 * s * Ks);
            return new Color(light.ComputeFinalLightColor(Ph) * color);
        }

        public override Color ComputeColor(Light light, Vector3 Ph, Vector3 npe, Vector3 Nlh, Vector3 Nh, float c)
        {
            Color color = Color.BLACK;
            //float c = this.ComputeC(Nlh, Nh);

            float s = this.ComputeS(npe, Nh, Nlh);

            color = new Color(Color0 * (1f - c) + Color1 * c);
            color = new Color(color * (1f - s * Ks) + Color2 * s * Ks);
            return new Color(light.ComputeFinalLightColor(Ph) * color);
        }

        public override Color ComputeColor(ShaderProperties properties)
        {
            if (properties.Texture != null)
            {
                Color1 = new Color(properties.Texture.ComputeColor(properties.UVCoordinates, properties.IPoint));
                Color0 = new Color(Color1 - (155f / 255f));
            }
            return base.ComputeColor(properties);
        }


        public virtual float ComputeS(Vector3 npe, Vector3 nh, Vector3 nlh)
        {
            Vector3 v = -1f * npe;
            Vector3 r = 2f * (nh % v) * nh - v;
            r.Normalize();
            float s = r % nlh;
            s = SMethod == TRUNCATE ? (s < 0 ? 0 : s) : (s + 1f) / 2f;
            s = (float)System.Math.Pow(s, KsAlpha);
            return s;
        }

        public override float ComputeC(Vector3 Nlh, Vector3 Nh)
        {
            float c = Nlh % Nh;
            c = SMethod == TRUNCATE ? (c < 0f ? 0f : c) : (1f + c) / 2f;
            c = (float)Math.Pow(c, Alpha);
            return c;
        }
    }
}
