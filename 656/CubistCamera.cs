using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.random;
using edu.tamu.courses.imagesynth.core.system;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    public class CubistCamera : Camera
    {
        private UniformGenerator thetaGenerator = new UniformGenerator(new Range(-(float)System.Math.PI, (float)System.Math.PI));
        private UniformGenerator alphaGenerator = new UniformGenerator(new Range(0f, (float)System.Math.PI));

        public Vector3 ComputePP(Vector3 npe)
        {
            float rndTheta = thetaGenerator.Next();
            float rndAlpha = alphaGenerator.Next();

            float x = (float)(Math.Cos(rndTheta) + Math.Sin(rndAlpha));
            float y = (float)(Math.Sin(rndTheta) * Math.Cos(rndAlpha));
            float z = (float)(Math.Cos(rndAlpha + rndTheta) * Math.Sin(rndAlpha - rndTheta)) / (float)(Math.Cos(rndAlpha - rndTheta) * Math.Sin(rndAlpha + rndTheta) + 1f);

            return npe + new Vector3(x, y, z);
        }
    }
}
