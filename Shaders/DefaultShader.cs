using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class DefaultShader : SimpleShader
    {
        public DefaultShader()
        {
            this.Color0 = new float[3] { 1f, 1f, 1f };
            this.Color1 = new float[3] { 0f, 0f, 0f };
        }
    }
}
