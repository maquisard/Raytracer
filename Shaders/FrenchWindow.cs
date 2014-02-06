using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class FrenchWindow : SquarePhong
    {
        public float WindowFrameThickness { get; set; }

        public FrenchWindow() { }

        public override float Evaluate(float x, float y)
        {
            float s = 0f;
            if (Math.Abs(x) <= 1 && Math.Abs(y) <= 1)
            {
                if (!(Math.Abs(x) <= WindowFrameThickness || Math.Abs(y) <= WindowFrameThickness))
                {
                    s = 1f;
                }
            }
            return s;
        }
    }
}
