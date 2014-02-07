using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;

namespace edu.tamu.courses.imagesynth.lights
{
    public class PointLight : Light
    {
        public PointLight() { }
        public float R0 { get; set; }
        public float Alpha { get; set; } //0 <= alpha <= 2

        public override Color ComputeFinalLightColor(Vector3 ph)
        {
            float R = (this.Position - ph).Norm;
            Alpha = Alpha > 2f ? 2f : Alpha;
            Alpha = Alpha <= 0 ? 0.1f : Alpha;

            float attenuation = (float)Math.Pow(R0 / R, Alpha);
            //Color = new Color(attenuation * Color);
            return Color;
        }
    }
}
