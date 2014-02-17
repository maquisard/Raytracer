using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.lights
{
    public class SpotLight : Light
    {
        public const int TRUNCATE = 0;
        public const int OFFSET = 1;

        public Vector3 Nl { get; set; }
        public SpotLight() { }
        public float Alpha { get; set; } //0 <= alpha <= 2
        public int ShadingMethod { get; set; }
        public Color Color0 { get; set; }
        public Color Color1 { get; set; }
        public float K { get; set; }

        public override void PostLoad()
        {
            base.PostLoad();
            this.Nl.Normalize();
        }

        public override Color ComputeFinalLightColor(Vector3 ph) //ph is the intersection point
        {
            Vector3 pl = this.Position;
            Vector3 v = ph - pl; //vector from the light to the intersection point
            v.Normalize();

            float costheta1 = this.Nl % v;
            float sintheta1 = (this.Nl ^ v).Norm;
            float sintheta2 = (float)Math.Sin(Alpha);
            float costheta2 = (float)Math.Cos(Alpha);

            //float s = this.Nl % v; //try truncation also later and see what you get
            float s = costheta1 * costheta2 + sintheta1 * sintheta2;
            s = ShadingMethod == TRUNCATE ? (s < 0 ? 0 : s) : (s + 1f) / 2f;
            s = (float)Math.Pow(s, K);
            Color = new Color(Color0 * (1f - s) + Color1 * s);
            return Color;
        }
    }
}
