﻿using edu.tamu.courses.imagesynth.Animations;
using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.textures;
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
    public abstract class Shape : Animable
    {
        public Shader Shader { get; protected set; }
        public Texture Texture { get; protected set; }
        public NormalMap NormalMap { get; set; }
        public bool IsTransparent { get; set; }
        public int Id { get; set; }


        public virtual void PreLoad() { }
        public virtual void PostLoad() { }

        public Shape() { this.IsTransparent = false; }

        public abstract Vector3 NormalAt(Vector3 p);

        public virtual Vector3 RealNormalAt(Vector3 p)
        {
            return NormalAt(p);
        }

        public abstract float Intersect(Vector3 pe, Vector3 npe);


        public static Shape CreateFromJson(JsonData jsonShape)
        {
            Type shapeType = Type.GetType("edu.tamu.courses.imagesynth.shapes." + (String)jsonShape["Type"]);
            ConstructorInfo constructer = shapeType.GetConstructor(new Type[] { });
            Shape shape = (Shape)constructer.Invoke(null);
            shape.PreLoad();
            String shaderName = (String)jsonShape["Shader"];
            if (jsonShape.ToJson().Contains("Texture"))
            {
                String textureName = (String)jsonShape["Texture"];
                shape.Texture = TextureManager.GetTexture(textureName);
            }
            if (jsonShape.ToJson().Contains("NormalMap"))
            {
                String mapName = (String)jsonShape["NormalMap"];
                shape.NormalMap = (NormalMap)TextureManager.GetTexture(mapName);
            }

            shape.Shader = ShaderManager.GetShader(shaderName);

            foreach(PropertyInfo property in shapeType.GetProperties())
            {
                //Console.WriteLine(property.Name);
                if (jsonShape.ToJson().Contains(property.Name) && property.Name != "Shader" && property.Name != "Texture" && property.Name != "NormalMap")
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
                    else if (jsonValue.IsBoolean)
                    {
                        property.SetValue(shape, bool.Parse(jsonShape[property.Name].ToString()));
                    }
                    else if (jsonValue.IsString && !((String)jsonValue).Contains("Shader"))
                    {
                        property.SetValue(shape, jsonShape[property.Name].ToString());
                    }
                    else if (jsonValue.IsObject)
                    {
                        String otypeName = (String)jsonValue["Type"];
                        if (otypeName.ToLower() == "vector3" || otypeName.ToLower() == "vector4")
                        {
                            Type vectorType = CoreAssembly.Current.GetType("edu.tamu.courses.imagesynth.core." + otypeName);
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
            shape.PostLoad();
            return shape;
        }

    }
}
