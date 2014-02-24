using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.textures;
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
        public abstract Color ComputeColor(Light light, Vector3 Ph, Vector3 npe, Vector3 Nlh, Vector3 Nh, float c); ///Shadow case
                                                                                                                    ///
        public virtual Color ComputeColor(ShaderProperties properties)
        {
            if (properties == null) throw new ArgumentNullException("The shader properties object must not be null");
            if (properties.IsCPrecomputed)
            {
                return this.ComputeColor(properties.Light, properties.IPoint, properties.EyeVector, properties.LightVector, properties.NormalVector, properties.C);
            }
            else
            {
                return this.ComputeColor(properties.Light, properties.IPoint, properties.EyeVector, properties.LightVector, properties.NormalVector);
            }
        }

        public virtual float ComputeC(Vector3 Nlh, Vector3 Nh) //the diffuse coefficient
        {
            return Nlh % Nh;
        }

    }
}
