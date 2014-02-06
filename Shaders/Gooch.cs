using System;
using System.Collections.Generic;
using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.lights;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class Gooch : Lambert
    {
        public Gooch() { }

        public override float ComputeC(Vector3 Nlh, Vector3 Nh)
        {
            float c = Nlh % Nh;
            c = (1f + c) / 2f;
            c = (float)Math.Pow(c, Alpha);
            return c;
        }
    }
}
