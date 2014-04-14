using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public class DynSphere : Sphere, IDynamicShape
    {
        public float Tmin { get; set; }
        public float Tmax { get; set; }

        public Vector3 NormalAt(Vector3 p, float time)
        {
            Vector3 normal = p - this.getCenter(time);
            normal.Normalize();
            if (NormalMap == null)
            {
                return normal;
            }
            else
            {
                return this.GetNormalVector(normal, p, this.UVCoordinates(p));
            }
        }

        public float Intersect(Vector3 pe, Vector3 npe, float time)
        {
            if (Math.Abs(npe.Norm - 1f) > 0.0001) throw new ArgumentException(String.Format("The ray vector must be a unit vector. Norm = {0}", npe.Norm));
            Vector3 pc = this.getCenter(time);
            float r = Radius;
            float b = npe % (pc - pe);
            float c = (pc - pe) % (pc - pe) - r * r;

            float t = -1f; //No Intersection
            if (c > 0.01f)
            {
                float delta = b * b - c;

                if (delta >= 0 && b >= 0)
                {
                    t = b - (float)Math.Sqrt(delta);
                }
            }
            return t;
        }

        protected Vector3 getCenter(float time)
        {
           // float realtime = time * Tmax + (1f - time) * Tmin;
            return new Vector3(this.Center.X, this.Center.Y + 0.001f, this.Center.Z);
        }
    }
}
