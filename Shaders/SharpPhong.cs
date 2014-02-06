using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class SharpPhong : Phong
    {
        public float CUTOFF { get; set; }

        public SharpPhong() { }
        public override float ComputeS(Vector3 npe, Vector3 nh, Vector3 nlh)
        {
            float s = base.ComputeS(npe, nh, nlh);
            return s > CUTOFF ? 1f : 0f;
        }

    }
}
