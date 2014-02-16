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
    public class Repeater
    {
        public int CountX { get; set; }
        public int CountY { get; set; }
        public int CountZ { get; set; }

        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public float OffsetZ { get; set; }

        public String ShapeType { get; set; }
        public Vector3 StartPoint { get; set; }
        public String ShapeProperty { get; set; }
        public List<StaticProperty> StaticProperties { get; set; }

        public Repeater()
        {
            this.StaticProperties = new List<StaticProperty>();
        }

        public List<Shape> CreateShapes()
        {
            List<Shape> shapes = new List<Shape>();
            for (int i = 0; i < CountX; i++)
            {
                for (int j = 0; j < CountY; j++)
                {
                    for (int k = 0; k < CountZ; k++)
                    {
                        Vector3 point = new Vector3();
                        point.X = StartPoint.X + (float)i * OffsetX;
                        point.Y = StartPoint.Y + (float)j * OffsetY;
                        point.Z = StartPoint.Z + (float)k * OffsetZ;
                        Type shapeType = Type.GetType("edu.tamu.courses.imagesynth.shapes." + ShapeType);
                        ConstructorInfo constructer = shapeType.GetConstructor(new Type[] { });
                        Shape shape = (Shape)constructer.Invoke(null);
                        shape.PreLoad();
                        PropertyInfo property = shapeType.GetProperty(ShapeProperty);
                        property.SetValue(shape, point);
                        foreach (StaticProperty staticProperty in this.StaticProperties)
                        {
                            PropertyInfo sProperty = shapeType.GetProperty(staticProperty.Name);
                            if (staticProperty.Name.Contains("Shader"))
                            {
                                Shader shader = ShaderManager.GetShader(staticProperty.Value.ToString());
                                sProperty.SetValue(shape, shader);
                            }
                            else
                            {
                                sProperty.SetValue(shape, staticProperty.Value);
                            }
                        }
                        shape.PostLoad();
                        shapes.Add(shape);
                    }
                }
            }
            return shapes;
        }

        public static Repeater CreateFromJson(JsonData jsonRepeater)
        {
            Repeater repeater = new Repeater();
            foreach (PropertyInfo property in typeof(Repeater).GetProperties())
            {
                //Console.WriteLine(property.Name);
                if (jsonRepeater.ToJson().Contains(property.Name))
                {
                    JsonData jsonValue = jsonRepeater[property.Name];
                    if (jsonValue.IsDouble)
                    {
                        property.SetValue(repeater, float.Parse(jsonRepeater[property.Name].ToString()));
                    }
                    else if (jsonValue.IsInt)
                    {
                        property.SetValue(repeater, int.Parse(jsonRepeater[property.Name].ToString()));
                    }
                    else if (jsonValue.IsString)
                    {
                        property.SetValue(repeater, jsonRepeater[property.Name].ToString());
                    }
                    else if (jsonValue.IsObject)
                    {
                        String otypeName = (String)jsonValue["Type"];
                        if (otypeName.ToLower() == "vector3" || otypeName.ToLower() == "vector4")
                        {
                            //Type vectorType = CoreAssembly.Current.GetType("edu.tamu.courses.imagesynth.core." + otypeName);
                            Vector vector = Vector.CreateFromJson(otypeName, jsonValue);
                            property.SetValue(repeater, vector);
                        }
                    }
                    else if (jsonValue.IsArray)
                    {
                        for (int i = 0; i < jsonValue.Count; i++)
                        {
                            repeater.StaticProperties.Add(StaticProperty.CreateFromJson(jsonValue[i]));
                        }
                    }
                }
            }
            return repeater;
        }

        public class StaticProperty
        {
            public String Name { get; set; }
            public Object Value { get; set; }
            public StaticProperty() { }

            public static StaticProperty CreateFromJson(JsonData jsonProperty)
            {
                StaticProperty sproperty = new StaticProperty();
                foreach (PropertyInfo property in typeof(StaticProperty).GetProperties())
                {
                    //Console.WriteLine(property.Name);
                    if (jsonProperty.ToJson().Contains(property.Name))
                    {
                        JsonData jsonValue = jsonProperty[property.Name];
                        if (jsonValue.IsDouble)
                        {
                            property.SetValue(sproperty, float.Parse(jsonProperty[property.Name].ToString()));
                        }
                        else if (jsonValue.IsInt)
                        {
                            property.SetValue(sproperty, int.Parse(jsonProperty[property.Name].ToString()));
                        }
                        else if (jsonValue.IsString)
                        {
                            property.SetValue(sproperty, jsonProperty[property.Name].ToString());
                        }
                        else if (jsonValue.IsObject)
                        {
                            String otypeName = (String)jsonValue["Type"];
                            if (otypeName.ToLower() == "vector3" || otypeName.ToLower() == "vector4")
                            {
                                //Type vectorType = CoreAssembly.Current.GetType("edu.tamu.courses.imagesynth.core." + otypeName);
                                Vector vector = Vector.CreateFromJson(otypeName, jsonValue);
                                property.SetValue(sproperty, vector);
                            }
                        }
                    }
                }
                return sproperty;
            }
        }
    }
}
