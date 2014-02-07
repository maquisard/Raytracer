using edu.tamu.courses.imagesynth.core;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    public class Camera
    {
        #region Given Values
        public Vector3 Pe { get; private set; }
        public Vector3 View { get; private set; }
        public Vector3 Up { get; private set; }

        public float D { get; private set; }
        public float Xmax { get; private set; }
        public float Ymax { get; private set; }
        public float Sx { get; private set; }
        #endregion

        #region Computed Values
        public float Sy { get; private set; }

        public Vector3 N2 { get; private set; }
        public Vector3 N0 { get; private set; }
        public Vector3 N1 { get; private set; }
        public Vector3 Pc { get; private set; }
        public Vector3 P0 { get; private set; }
        #endregion

        public Camera() { }

        private void updateValues()
        {
            Up.Normalize();
            Sy = (Ymax / Xmax) * Sx;
            N2 = -1f * (View / View.Norm);
            N0 = (View ^ Up);
            N0.Normalize();
            N1 = (N0 ^ View) / View.Norm;
            Pc = Pe - D * N2;
            P0 = Pc - 0.5f * ((N0 * Sx) + (N1 * Sy));
        }

        public static Camera CreateFromJson(JsonData jsonCamera)
        {
            Camera camera = new Camera();
            Type cameraType = typeof(Camera);
            foreach(PropertyInfo property in cameraType.GetProperties())
            {
                if (jsonCamera.ToJson().Contains(property.Name))
                {
                    JsonData  jsonValue = jsonCamera[property.Name];
                    if (jsonValue.IsDouble)
                    {
                        property.SetValue(camera, float.Parse(jsonCamera[property.Name].ToString()));
                    }
                    else if(jsonValue.IsObject)
                    {
                        String otypeName = (String)jsonValue["Type"];
                        if (otypeName.ToLower() == "vector3" || otypeName.ToLower() == "vector4")
                        {
                            Vector vector = Vector.CreateFromJson(otypeName, jsonValue);
                            property.SetValue(camera, vector);
                        }
                    }
                }
            }
            camera.updateValues();
            return camera;
        }

        public override string ToString()
        {
            return String.Format("Pe: {0}\nView: {1}\nUp: {2}\nD: {3}\nXmax: {4}\nYMax: {5}\nSx: {6}\nSy: {7}\nN2: {8}\nN0: {9}\nN1: {10}\nPc: {11}\nP0: {12}",
                                    Pe, View, Up, D, Xmax, Ymax, Sx, Sy, N2, N0, N1, Pc, P0);
        }
    }
}
