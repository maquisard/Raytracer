using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.tests
{
    public class Quiz9
    {
        public void Run()
        {
            Vector4 p0 = new Vector4(1,8,-3,6);
            Vector4[] otherPs = 
            {
                new Vector4(13,104,-39,78),
                new Vector4(17,104,-39,42),
                new Vector4(7,56,-21,42),
                new Vector4(17,136,-51,102)
            };

            foreach (Vector4 p in otherPs)
            {
                Console.WriteLine("P0 Homogenous to {0}: {1}", p, this.Equivalent(p0, p));
            }


        }

        private bool Equivalent(Vector4 p0, Vector4 p1)
        {
            float r0x = p0.X / p0.W;
            float r0y = p0.Y / p0.W;
            float r0z = p0.Z / p0.W;
            float r1x = p1.X / p1.W;
            float r1y = p1.Y / p1.W;
            float r1z = p1.Z / p1.W;

            return r0x == r1x && r0y == r1y && r0z == r1z;
        }
    }
}
