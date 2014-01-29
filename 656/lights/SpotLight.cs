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
        public Vector3 Nl { get; set; }
        public SpotLight() { }
    }
}
