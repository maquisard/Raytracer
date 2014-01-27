using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.shaders;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shapes
{
    public abstract class Shape
    {
        public Shader Shader { get; private set; }
        public Shape() { }

        public abstract float Intersect(Vector3 pe, Vector3 npe);

        public static Shape CreateFromJson(JsonData jsonShape)
        {
            Type shapeType = Type.GetType("edu.tamu.courses.imagesynth.shapes." + (String)jsonShape["Type"]);
            ConstructorInfo constructer = shapeType.GetConstructor(new Type[] { });
            Shape shape = (Shape)constructer.Invoke(null);
            String shaderName = (String)jsonShape["Shader"];
            Shader shader = ShaderManager.GetShader(shaderName);
            shape.Shader = shader == null ? ShaderManager.DEFAULTSHADER : shader;

            foreach(PropertyInfo property in shapeType.GetProperties())
            {
                //Console.WriteLine(property.Name);
                if (jsonShape.ToJson().Contains(property.Name))
                {
                    JsonData  jsonValue = jsonShape[property.Name];
                    if (jsonValue.IsDouble)
                    {
                        property.SetValue(shape, float.Parse(jsonShape[property.Name].ToString()));
                    }
                    else if (jsonValue.IsInt)
                    {
                        property.SetValue(shape, int.Parse(jsonShape[property.Name].ToString()));
                    }
                    else if(jsonValue.IsObject)
                    {
                        String otypeName = (String)jsonValue["Type"];
                        if (otypeName.ToLower() == "vector3" || otypeName.ToLower() == "vector4")
                        {
                            Type vectorType = Type.GetType("edu.tamu.courses.imagesynth.core." + otypeName);
                            ConstructorInfo vectorConstructor = vectorType.GetConstructor(new Type[] { });
                            Object vector = vectorConstructor.Invoke(null);
                            foreach (FieldInfo vProperty in vectorType.GetFields())
                            {
                                if (jsonValue.ToJson().Contains(vProperty.Name))
                                {
                                    JsonData coordValue = jsonValue[vProperty.Name];
                                    if (coordValue != null && coordValue.IsDouble)
                                    {
                                        vProperty.SetValue(vector, float.Parse(coordValue.ToString()));
                                    }
                                }
                            }
                            property.SetValue(shape, vector);
                        }
                    }
                }
            }
            return shape;
        }
    }
}
