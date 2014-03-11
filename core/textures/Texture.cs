using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.textures
{
    public abstract class Texture
    {
        public abstract Color ComputeColor(Vector uvcoordinates, Vector3 iPoint);

        public virtual void PreLoad() { }
        public virtual void PostLoad() { }
    }
}
