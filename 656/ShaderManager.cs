﻿using edu.tamu.courses.imagesynth.core;
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
        private static Dictionary<String, Shader> shaders = new Dictionary<string,Shader>();
        public static Shader CreateShaderFromJson(JsonData jsonShader)
        {
            //get the freshest of the binaries from the dll
            String currentPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Assembly shaderAssembly = Assembly.LoadFile(currentPath + "\\edu.tamu.courses.imagesynth.shaders.dll");
            Type shaderType = shaderAssembly.GetType("edu.tamu.courses.imagesynth.shaders." + (String)jsonShader["Type"]);
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
                    else if (jsonValue.IsBoolean)
                    {
                        property.SetValue(shader, (bool)jsonShader[property.Name]);
                    }
                    else if (jsonValue.IsArray)
                    {
                        float[] value = new float[jsonShader[property.Name].Count];
                        for (int i = 0; i < jsonShader[property.Name].Count; i++)
                        {
                            value[i] = float.Parse(jsonShader[property.Name][i].ToString());
                        }
                        Color color = new Color(value[0], value[1], value[2]);
                        color.PostLoad();
                        property.SetValue(shader, color);
                        //property.SetValue(shader, value);
                    }
                }
            }
            String name = (String)jsonShader["Name"];
            shaders[name] = shader;
            return shader;
        }

        public static void clearShaders()
        {
            if (shaders != null) shaders.Clear();
        }

        public static Shader GetShader(String name)
        {
            return shaders[name];
        }
    }
}
