using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core
{
    public abstract class Vector
    {

        public virtual void PreLoad() { }
        public virtual void PostLoad() { }

        public static Vector CreateFromJson(String otypeName, JsonData jsonVector)
        {
            Type vectorType = Type.GetType("edu.tamu.courses.imagesynth.core." + otypeName);
            ConstructorInfo vectorConstructor = vectorType.GetConstructor(new Type[] { });
            Vector vector = vectorConstructor.Invoke(null) as Vector;
            vector.PreLoad();
            foreach (FieldInfo vProperty in vectorType.GetFields())
            {
                if (jsonVector.ToJson().Contains(vProperty.Name))
                {
                    JsonData coordValue = jsonVector[vProperty.Name];
                    if (coordValue != null && coordValue.IsDouble)
                    {
                        vProperty.SetValue(vector, float.Parse(coordValue.ToString()));
                    }
                }
            }
            vector.PostLoad();
            return vector;
        }
    }
}
