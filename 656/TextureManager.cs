using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.textures;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    public class TextureManager
    {
        private static Dictionary<String, Texture> textures = new Dictionary<string, Texture>();
        public static Texture CreateShaderFromJson(JsonData jsonShader)
        {
            //get the freshest of the binaries from the dll
            String currentPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Assembly shaderAssembly = Assembly.LoadFile(currentPath + "\\edu.tamu.courses.imagesynth.core.dll");
            Type TextureType = shaderAssembly.GetType("edu.tamu.courses.imagesynth.core.texture." + (String)jsonShader["Type"]);
            Texture texture = (Texture)Activator.CreateInstance(TextureType);
            texture.PreLoad();
            foreach (PropertyInfo property in TextureType.GetProperties())
            {
                if (jsonShader.ToJson().Contains(property.Name))
                {
                    JsonData jsonValue = jsonShader[property.Name];
                    if (jsonValue.IsDouble)
                    {
                        property.SetValue(texture, float.Parse(jsonShader[property.Name].ToString()));
                    }
                    else if (jsonValue.IsInt)
                    {
                        property.SetValue(texture, int.Parse(jsonShader[property.Name].ToString()));
                    }
                    else if (jsonValue.IsString)
                    {
                        property.SetValue(texture, jsonValue[property.Name].ToString());
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
                        property.SetValue(texture, color);
                    }
                }
            }
            texture.PostLoad();
            String name = (String)jsonShader["Name"];
            textures[name] = texture;
            return texture;
        }

        public static void clearTextures()
        {
            if (textures != null) textures.Clear();
        }

        public static Texture GetTexture(String name)
        {
            return textures[name];
        }
    }
}
