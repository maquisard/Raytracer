using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class Lambert : Shader
    {
        public Lambert() { }

        public Color Color0 { get; set; }
        public Color Color1 { get; set; }
        public float Alpha { get; set; }

        public override Color ComputeColor(Light light, Vector3 Ph, Vector3 npe, Vector3 Nlh, Vector3 Nh)
        {
            float c = this.ComputeC(Nlh, Nh);
            Vector3 finalColor = (1f - c) * Color0 + c * Color1;
            return new Color(light.ComputeFinalLightColor(Ph) * finalColor);
        }

        public override Color ComputeColor(Light light, Vector3 Ph, Vector3 npe, Vector3 Nlh, Vector3 Nh, float c)
        {
            //float c = this.ComputeC(Nlh, Nh);
            Vector3 finalColor = (1f - c) * Color0 + c * Color1;
            return new Color(light.ComputeFinalLightColor(Ph) * finalColor);
        }

        public override float ComputeC(Vector3 Nlh, Vector3 Nh) //the diffuse coefficient
        {
            float c = Nlh % Nh;
            c = c < 0 ? 0 : c;
            c = (float)Math.Pow(c, Alpha);
            return c;
        }
    }
}
