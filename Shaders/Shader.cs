using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public abstract class Shader
    {        

        public abstract Color ComputeColor(Light light, Vector3 Ph, Vector3 npe, Vector3 Nlh, Vector3 Nh);

        public virtual float ComputeC(Vector3 Nlh, Vector3 Nh) //the diffuse coefficient
        {
            return Nlh % Nh;
        }

    }
}
