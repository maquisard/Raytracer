﻿using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;

namespace edu.tamu.courses.imagesynth.lights
{
    public class PointLight : Light
    {
        public PointLight() { }
        public float r0 { get; set; }
        public float alpha { get; set; } //0 <= alpha <= 2

        public Color ComputeFinalLightColor(float r)
        {
            alpha = alpha > 2f ? 2f : alpha;
            alpha = alpha <= 0 ? 0.1f : alpha;

            float attenuation = (float)Math.Pow(r0 / r, alpha);
            return (attenuation * Color) as Color;
        }
    }
}
