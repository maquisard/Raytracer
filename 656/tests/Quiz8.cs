using edu.tamu.courses.imagesynth.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.tests
{
    public class Quiz8
    {
        public void Run()
        {
            Vector3 v = new Vector3(-34.92f, -22.87f, 25.83f);
            Vector3 n = new Vector3(-0.3f, 0.3f, 0.9f);
            //v.Normalize();
            n.Normalize();

            float theta = 0.86f;
            float costheta = (float)Math.Cos(theta);
            float sintheta = (float)Math.Sin(theta);
            Vector3 r = costheta * v + ((1f - costheta) * (n % v) * n) + sintheta * (n ^ v);
            //r.Normalize();
            Console.WriteLine("R: {0}", r);

            v = new Vector3(-2.82f, 30.00f, 33.88f);
            //v.Normalize();
            theta = 0.58f;
            costheta = (float)Math.Cos(theta);
            sintheta = (float)Math.Sin(theta);
            Matrix3x3 yrotation = new Matrix3x3();
            yrotation.V00 = costheta;
            yrotation.V01 = 0;
            yrotation.V02 = sintheta;
            yrotation.V10 = 0;
            yrotation.V11 = 1;
            yrotation.V12 = 0;
            yrotation.V20 = -sintheta;
            yrotation.V21 = 0;
            yrotation.V22 = costheta;
            r = yrotation * v;
            Console.WriteLine("Y Rotation R: {0}\n", r);

            v = new Vector3(-30.79f, 20.52f, 39.00f);
            //v.Normalize();
            theta = 0.30f;
            costheta = (float)Math.Cos(theta);
            sintheta = (float)Math.Sin(theta);
            Matrix3x3 zrotation = new Matrix3x3();
            zrotation.V00 = costheta;
            zrotation.V01 = -sintheta;
            zrotation.V02 = 0;
            zrotation.V10 = sintheta;
            zrotation.V11 = costheta;
            zrotation.V12 = 0;
            zrotation.V20 = 0;
            zrotation.V21 = 0;
            zrotation.V22 = 1;
            r = zrotation * v;
            Console.WriteLine("Z Rotation R: {0}\n", r);

            v = new Vector3(22.00f, 8.53f, -19.19f);
            //v.Normalize();
            theta = 1.48f;
            costheta = (float)Math.Cos(theta);
            sintheta = (float)Math.Sin(theta);
            Matrix3x3 xrotation = new Matrix3x3();
            xrotation.V00 = 1;
            xrotation.V01 = 0;
            xrotation.V02 = 0;
            xrotation.V10 = 0;
            xrotation.V11 = costheta;
            xrotation.V12 = -sintheta;
            xrotation.V20 = 0;
            xrotation.V21 = sintheta;
            xrotation.V22 = costheta;
            r = xrotation * v;
            Console.WriteLine("X Rotation R: {0}\n", r);

        }
    }
}
