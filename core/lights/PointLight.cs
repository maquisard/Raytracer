using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;

namespace edu.tamu.courses.imagesynth.lights
{
    public class PointLight : Light
    {
        public PointLight() { }
        public float r0 { get; set; }
        public float alpha { get; set; } //0 <= alpha <= 2

        public override Color ComputeFinalLightColor(Vector3 ph)
        {
            float r = (this.Position - ph).Norm;
            alpha = alpha > 2f ? 2f : alpha;
            alpha = alpha <= 0 ? 0.1f : alpha;

            float attenuation = (float)Math.Pow(r0 / r, alpha);
            Color = new Color(attenuation * Color);
            return Color;
        }
    }
}
