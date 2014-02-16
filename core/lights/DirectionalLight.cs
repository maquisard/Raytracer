using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.lights
{
    public class DirectionalLight : Light
    {
        public Vector3 Normal { get; set; }

        public override Color ComputeFinalLightColor(Vector3 ph)
        {
            return this.Color;
        }

        public override Vector3 ComputeLightVector(Vector3 P)
        {
            return -1f * Normal;
        }

        public override void PostLoad()
        {
            Normal.Normalize();
        }
    }
}
