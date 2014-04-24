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
            return Color;
        }
    }
}
