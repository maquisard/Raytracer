using edu.tamu.courses.imagesynth.shaders;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    public class ShaderManager
    {
        public static Shader DEFAULTSHADER = new DefaultShader();
        private static Dictionary<String, Shader> shaders = new Dictionary<string, Shader>();
        public static Shader CreateShaderFromJson(JsonData jsonShader)
        {
            shaders.Clear();

            //get the freshest of the binaries from the dll
            Assembly shaderAssembly = Assembly.LoadFile("edu.tamu.courses.imagesynth.shaders.dll");
            Type shaderType = Type.GetType("edu.tamu.courses.imagesynth.shaders." + (String)jsonShader["Type"]);
            Shader shader = (Shader)Activator.CreateInstance(shaderType);
            foreach (PropertyInfo property in shaderType.GetProperties())
            {
                if (jsonShader.ToJson().Contains(property.Name))
                {
                    JsonData jsonValue = jsonShader[property.Name];
                    if (jsonValue.IsDouble)
                    {
                        property.SetValue(shader, float.Parse(jsonShader[property.Name].ToString()));
                    }
                    else if (jsonValue.IsInt)
                    {
                        property.SetValue(shader, int.Parse(jsonShader[property.Name].ToString()));
                    }
                    else if (jsonValue.IsArray)
                    {
                        float[] value = new float[jsonShader[property.Name].Count];
                        for (int i = 0; i < jsonShader[property.Name].Count; i++)
                        {
                            value[i] = float.Parse(jsonShader[property.Name][i].ToString());
                        }
                        property.SetValue(shader, value);
                    }
                }
            }
            String name = (String)jsonShader["Name"];
            shaders[name] = shader;
            return shader;
        }

        public static Shader GetShader(String name)
        {
            return shaders[name];
        }
    }
}
