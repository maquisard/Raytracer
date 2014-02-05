using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.lights
{
    public class SquareLight : Light
    {
        public Vector3 Vup { get; set; }
        public Vector3 Nl { get; set; }
        public Vector3 N0 { get; set; }
        public Vector3 N1 { get; set; }
        public float Sx { get; set; }
        public float Sy { get; set; }

        public SquareLight() { }

        public override void PostLoad()
        {
            this.Update();
        }

        public override Color ComputeFinalLightColor(Vector3 ph)
        {
            return this.Color;
        }

        public void Update()
        {
            N0 = Nl ^ Vup;
            N1 = N0 ^ Nl;
        }
    }
}
