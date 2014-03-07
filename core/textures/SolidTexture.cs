using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.textures
{
    public abstract class SolidTexture
    {
        public abstract Color ComputeColor(Vector3 uvw, Vector3 P);

        public virtual void PreLoad() { }
        public virtual void PostLoad() { }
    }
}
