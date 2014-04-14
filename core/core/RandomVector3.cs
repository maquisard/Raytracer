using edu.tamu.courses.imagesynth.core.random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.core
{
    public class RandomVector3 : Vector3
    {
        public RandomVector3(UniformOneGenerator generator)
        {
            this.X = generator.Next();
            this.Y = generator.Next();
            this.Z = generator.Next();
        }
    }
}
