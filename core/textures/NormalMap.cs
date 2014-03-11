using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.textures
{
    public class NormalMap : ImageTexture
    {
        public float Scale { get; set; }
        public float Amount { get; set; }

        public Vector3 GenerateNormal(Vector3 normal, Vector3 iPoint)
        {
            Vector3 noise = Vector3.Zero;
            float x = iPoint.X / Scale;
            float y = iPoint.Y / Scale;
            float z = iPoint.Z / Scale;
            noise.X = (float)NormalNoise.Current.Noise(x, y, z);
            noise.Y = (float)NormalNoise.Current.Noise(y, z, x);
            noise.Z = (float)NormalNoise.Current.Noise(z, x, y);

            Vector3 n = (normal + noise * Amount);
            n.Normalize();
            return n;
        }
    }
}
