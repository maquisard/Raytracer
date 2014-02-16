using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public class Ellipsoid : Quadrics
    {
        protected override float A02 { get { return 1f; } }
        protected override float A12 { get { return 1f; } }
        protected override float A22 { get { return 1f; } }
        protected override float A21 { get { return 0f; } }
        protected override float A00 { get { return -1f; } }

    }
}
