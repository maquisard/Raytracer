using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public interface IDynamicShape
    {
        Vector3 NormalAt(Vector3 p, float time);
        float Intersect(Vector3 pe, Vector3 npe, float time);
    }
}
