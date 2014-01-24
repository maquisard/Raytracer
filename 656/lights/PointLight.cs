using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.lights
{
    public class PointLight : Light
    {
        public PointLight() { }
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
    }
}
