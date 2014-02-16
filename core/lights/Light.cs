using edu.tamu.courses.imagesynth.core;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.lights
{
    public abstract class Light
    {
        public Vector3 Position { get; set; }
        public Color Color { get; set; }

        public virtual void PreLoad() { }
        public virtual void PostLoad() { }

        public abstract Color ComputeFinalLightColor(Vector3 ph);
        public virtual Vector3 ComputeLightVector(Vector3 P)
        {
            return this.Position - P;
        }

        public static Light CreateFromJson(JsonData jsonLight)
        {
            Type lightType = Type.GetType("edu.tamu.courses.imagesynth.lights." + (string)jsonLight["Type"]);
            ConstructorInfo constructer = lightType.GetConstructor(new Type[] { });
            Light light = (Light)constructer.Invoke(null);
            light.PreLoad();
            foreach (PropertyInfo property in lightType.GetProperties())
            {
                if (jsonLight.ToJson().Contains(property.Name))
                {
                    JsonData jsonValue = jsonLight[property.Name];
                    if (jsonValue.IsDouble)
                    {
                        property.SetValue(light, float.Parse(jsonLight[property.Name].ToString()));
                    }
                    else if (jsonValue.IsInt)
                    {
                        property.SetValue(light, int.Parse(jsonLight[property.Name].ToString()));
                    }
                    else if (jsonValue.IsObject)
                    {
                        String otypeName = (String)jsonValue["Type"];
                        if (otypeName.ToLower() == "vector3" || otypeName.ToLower() == "vector4" || otypeName.ToLower() == "color")
                        {
                            Type vectorType = CoreAssembly.Current.GetType("edu.tamu.courses.imagesynth.core." + otypeName);
                            Vector vector = Vector.CreateFromJson(otypeName, jsonValue);
                            property.SetValue(light, vector);
                        }
                    }
                }
            }
            light.PostLoad();
            return light;
        }
    }
}
