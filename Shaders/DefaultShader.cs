using edu.tamu.courses.imagesynth.core;
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
            this.Color0 = Color.WHITE;
            this.Color1 = Color.BLACK;
            this.Color2 = (Color.WHITE) / 2f as Color;
        }
    }
}
