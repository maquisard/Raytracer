using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class SimpleShader1 : SimpleShader
    {
        public SimpleShader1() { }
        protected override float ComputeC()
        {
            return (1f + Math.Dot(Nlh, Nh)) / 2f;        
        }
    }
}
