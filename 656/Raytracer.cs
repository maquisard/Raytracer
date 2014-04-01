using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.imaging;
using edu.tamu.courses.imagesynth.core.random;
using edu.tamu.courses.imagesynth.core.system;
using edu.tamu.courses.imagesynth.core.textures;
using edu.tamu.courses.imagesynth.lights;
using edu.tamu.courses.imagesynth.shaders;
using edu.tamu.courses.imagesynth.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    public class Raytracer
    {
        public Scene Scene;
        public float X { get; protected set; }
        public float Y { get; protected set; }
        public Vector3 Pp { get; protected set; }
        public Vector3 Npe { get; protected set; }

        protected UniformOneGenerator randomGenerator = new UniformOneGenerator();
        protected UniformGenerator randomGeneratorLight;

        public void Compute(int I, int J, int i, int j, int m, int n, float rx, float ry)
        {
            X = I + (i / (float)m) + (rx / (float)m);
            Y = J + (j / (float)n) + (ry / (float)n);
            float x = X / Scene.Camera.Xmax;
            float y = Y / Scene.Camera.Ymax;
            Pp = Scene.Camera.P0 + (x * Scene.Camera.Sx * Scene.Camera.N0) + (y * Scene.Camera.Sy * Scene.Camera.N1);
            Npe = Pp - Scene.Camera.Pe;
            Npe.Normalize();
            Console.WriteLine("\nX: {0}\nY: {1}\nPP: {2}\nNPE: {3}", X, Y, Pp, Npe);
        }

        public virtual void Raytrace()
        {
            randomGeneratorLight = new UniformGenerator(new Range(0, Scene.MSamplePerPixels * Scene.NSamplePerPixels - 1));
            float max = Scene.Camera.Xmax * Scene.Camera.Ymax;
            float iteration = 0f;
            ImageData image = new ImageData((int)Scene.Camera.Xmax, (int)Scene.Camera.Ymax);
            for (int I = 0; I < Scene.Camera.Xmax; I++)
            {
                for (int J = 0; J < Scene.Camera.Ymax; J++)
                {
                    Color color = Color.BLACK;
                    float randx = randomGenerator.Next() / (float)Scene.MSamplePerPixels;
                    float randy = randomGenerator.Next() / (float)Scene.NSamplePerPixels;

                    float rnd = randomGeneratorLight.Next();
                    float rndoffsetx = randomGenerator.Next();
                    float rndoffsety = randomGenerator.Next();

                    for (int i = 0; i < Scene.MSamplePerPixels; i++)
                    {
                        for (int j = 0; j < Scene.NSamplePerPixels; j++)
                        {
                            X = I + (i / (float)Scene.MSamplePerPixels) + randx;
                            Y = J + (j / (float)Scene.NSamplePerPixels) + randy;
                            float x = X / Scene.Camera.Xmax;
                            float y = Y / Scene.Camera.Ymax;
                            Pp = Scene.Camera.P0 + (x * Scene.Camera.Sx * Scene.Camera.N0) + (y * Scene.Camera.Sy * Scene.Camera.N1);
                            Npe = Pp - Scene.Camera.Pe;
                            Npe.Normalize();
                            float t = -1f;
                            Shape shape = Scene.GetIntersectedShape(Scene.Camera.Pe, Npe, ref t); //Implement the Get Intersected Shape
                            if (shape != null)
                            {
                                Vector3 iPoint = Scene.Camera.Pe + Npe * t; //Intersection point
                                Vector3 iNormal = shape.NormalAt(iPoint);   //Normal at the intersection

                                foreach (Light light in Scene.Lights)
                                {
                                    //Light light = _light;
                                    if (light is AreaLight)
                                    {
                                        ((AreaLight)light).Rnd = (int)rnd;
                                        ((AreaLight)light).Ith = i;
                                        ((AreaLight)light).Jth = j;
                                        ((AreaLight)light).RndOffsetX = rndoffsetx;
                                        ((AreaLight)light).RndOffsetY = rndoffsety;
                                    }

                                    Vector3 lightVector = light.ComputeLightVector(iPoint);
                                    float distanceToLight = (light.Position - iPoint).Norm;
                                    //float distanceToLight = lightVector.Norm;
                                    lightVector.Normalize();

                                    //color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal));
                                    ShaderProperties properties = new ShaderProperties();

                                    Dictionary<float, Shape> intersectedShapes = Scene.GetIntersectedShapes(iPoint, lightVector, distanceToLight);
                                    if (intersectedShapes.Count == 0 || light is LightProjection)
                                    {
                                        //ShaderProperties properties = new ShaderProperties();
                                        properties.IsCPrecomputed = false;
                                        properties.Light = light;
                                        properties.IPoint = iPoint;
                                        properties.EyeVector = Npe;
                                        properties.LightVector = lightVector;
                                        properties.NormalVector = iNormal;
                                        properties.Texture = shape.Texture;
                                        if (shape is UVInterface)
                                        {
                                            properties.UVCoordinates = (shape as UVInterface).UVCoordinates(iPoint);
                                        }
                                        //color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal));
                                        //color = new Color(color + shape.Shader.ComputeColor(properties));
                                    }
                                    else
                                    {
                                        float[] weights = new float[intersectedShapes.Count];
                                        float[] coefs = new float[intersectedShapes.Count];
                                        float weight_sum = 0;
                                        int g = 0;
                                        foreach (KeyValuePair<float, Shape> iShape in intersectedShapes)
                                        {
                                            Vector3 point = iPoint + iShape.Key * lightVector;
                                            if (shape != iShape.Value) // self intersection
                                            {
                                                Vector3 normal;
                                                if (iShape.Value.NormalMap != null)
                                                {
                                                    normal = iShape.Value.RealNormalAt(point);
                                                }
                                                else
                                                {
                                                    normal = iShape.Value.NormalAt(point);
                                                }
                                                weights[g] = (point - light.Position).Norm / distanceToLight;
                                                weight_sum += weights[g];
                                                Vector3 nlh = light.Position - point;
                                                nlh.Normalize();
                                                float cosTheta = nlh % normal;
                                                cosTheta = (cosTheta + 1f);
                                                coefs[g] = cosTheta < 0f ? 0f : cosTheta;
                                                g++;
                                            }
                                        }

                                        if (weight_sum == 0)
                                        {
                                            //ShaderProperties properties = new ShaderProperties();
                                            properties.IsCPrecomputed = false;
                                            properties.Light = light;
                                            properties.IPoint = iPoint;
                                            properties.EyeVector = Npe;
                                            properties.LightVector = lightVector;
                                            properties.NormalVector = iNormal;
                                            properties.Texture = shape.Texture;
                                            if (shape is UVInterface)
                                            {
                                                properties.UVCoordinates = (shape as UVInterface).UVCoordinates(iPoint);
                                            }
                                            //color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal));
                                            //color = new Color(color + shape.Shader.ComputeColor(properties));
                                        }
                                        else
                                        {
                                            float c = 1;
                                            for (int k = 0; k < weights.Length; k++)
                                            {
                                                c *= (float)Math.Pow(coefs[k], weights[k] / weight_sum);
                                                //c *= coefs[k];
                                            }
                                            //c = 2f * c;
                                            //ShaderProperties properties = new ShaderProperties();
                                            properties.IsCPrecomputed = true;
                                            properties.C = c;
                                            properties.Light = light;
                                            properties.IPoint = iPoint;
                                            properties.EyeVector = Npe;
                                            properties.LightVector = lightVector;
                                            properties.NormalVector = iNormal;
                                            properties.Texture = shape.Texture;
                                            if (shape is UVInterface)
                                            {
                                                properties.UVCoordinates = (shape as UVInterface).UVCoordinates(iPoint);
                                            }
                                            //color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal, c));
                                            //color = new Color(color + shape.Shader.ComputeColor(properties));
                                        }
                                    }
                                    //do the reflection here

                                    Color diffuseColor = shape.Shader.ComputeColor(properties);
                                    Color reflectionColor = Color.BLACK;
                                    if (shape.Shader is Material)
                                    {
                                        Material material = shape.Shader as Material;
                                        if (material.IsReflective)
                                        {
                                            int itr = 1;
                                            int maxRecursions = 4;
                                            float delta = 0.001f;
                                            float ki = material.Ks;
                                            Vector3 v = -1f * Npe;
                                            Vector3 n = iNormal;
                                            reflectionColor = new Color((1 - ki) * diffuseColor + ki * RaytraceReflection(ki, maxRecursions, delta, ref itr, Scene, iPoint, v, n, rnd, rndoffsetx, rndoffsety, i, j));
                                            if (material.Kiris > 1) //has iris properties
                                            {
                                                reflectionColor = new Color(material.Kiris * reflectionColor * Irisdescence.Current.ComputeColor(v, n)); 
                                            }
                                        }
                                    }
                                    color = new Color(color + diffuseColor + reflectionColor);

                                }
                            }
                        }
                    }

                    color = new Color(color / (float)(Scene.MSamplePerPixels * Scene.NSamplePerPixels));
                    image.SetPixel(I, J, color);
                    Console.WriteLine("{0:0.00}% Computed...", (iteration / max) * 100f);
                    iteration++;
                }
            }

            image.SaveToFile(Scene.Name + ".png");
        }

        private Color RaytraceReflection(float ki, float max, float delta, ref int itr, Scene Scene, Vector3 p, Vector3 v, Vector3 n, float rnd, float rndoffsetx, float rndoffsety, int i, int j)
        {
            Color color = Color.BLACK;
            Vector3 r = (-1f * v) + 2f * (v % n) * n;
            r.Normalize();
            float t = -1f;
            Shape shape = Scene.GetIntersectedShape(p, r, ref t); //Implement the Get Intersected Shape
            if (shape != null)
            {
                Vector3 iPoint = p + r * t; //Intersection point
                Vector3 iNormal = shape.NormalAt(iPoint);   //Normal at the intersection

                foreach (Light light in Scene.Lights)
                {
                    //Light light = _light;
                    if (light is AreaLight)
                    {
                        ((AreaLight)light).Rnd = (int)rnd;
                        ((AreaLight)light).Ith = i;
                        ((AreaLight)light).Jth = j;
                        ((AreaLight)light).RndOffsetX = rndoffsetx;
                        ((AreaLight)light).RndOffsetY = rndoffsety;
                    }

                    Vector3 lightVector = light.ComputeLightVector(iPoint);
                    float distanceToLight = (light.Position - iPoint).Norm;
                    //float distanceToLight = lightVector.Norm;
                    lightVector.Normalize();

                    //color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal));

                    Dictionary<float, Shape> intersectedShapes = Scene.GetIntersectedShapes(iPoint, lightVector, distanceToLight);
                    ShaderProperties properties = new ShaderProperties();

                    if (intersectedShapes.Count == 0 || light is LightProjection)
                    {
                        properties.IsCPrecomputed = false;
                        properties.Light = light;
                        properties.IPoint = iPoint;
                        properties.EyeVector = Npe;
                        properties.LightVector = lightVector;
                        properties.NormalVector = iNormal;
                        properties.Texture = shape.Texture;
                        if (shape is UVInterface)
                        {
                            properties.UVCoordinates = (shape as UVInterface).UVCoordinates(iPoint);
                        }
                        //color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal));
                        //color = new Color(color + shape.Shader.ComputeColor(properties));
                    }
                    else
                    {
                        float[] weights = new float[intersectedShapes.Count];
                        float[] coefs = new float[intersectedShapes.Count];
                        float weight_sum = 0;
                        int g = 0;
                        foreach (KeyValuePair<float, Shape> iShape in intersectedShapes)
                        {
                            Vector3 point = iPoint + iShape.Key * lightVector;
                            if (shape != iShape.Value) // self intersection
                            {
                                Vector3 normal;
                                if (iShape.Value.NormalMap != null)
                                {
                                    normal = iShape.Value.RealNormalAt(point);
                                }
                                else
                                {
                                    normal = iShape.Value.NormalAt(point);
                                }
                                weights[g] = (point - light.Position).Norm / distanceToLight;
                                weight_sum += weights[g];
                                Vector3 nlh = light.Position - point;
                                nlh.Normalize();
                                float cosTheta = nlh % normal;
                                cosTheta = (cosTheta + 1f);
                                coefs[g] = cosTheta < 0f ? 0f : cosTheta;
                                g++;
                            }
                        }

                        if (weight_sum == 0)
                        {
                            //ShaderProperties properties = new ShaderProperties();
                            properties.IsCPrecomputed = false;
                            properties.Light = light;
                            properties.IPoint = iPoint;
                            properties.EyeVector = Npe;
                            properties.LightVector = lightVector;
                            properties.NormalVector = iNormal;
                            properties.Texture = shape.Texture;
                            if (shape is UVInterface)
                            {
                                properties.UVCoordinates = (shape as UVInterface).UVCoordinates(iPoint);
                            }
                            //color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal));
                            //color = new Color(color + shape.Shader.ComputeColor(properties));
                        }
                        else
                        {
                            float c = 1;
                            for (int k = 0; k < weights.Length; k++)
                            {
                                c *= (float)Math.Pow(coefs[k], weights[k] / weight_sum);
                                //c *= coefs[k];
                            }
                            //c = 2f * c;
                            //ShaderProperties properties = new ShaderProperties();
                            properties.IsCPrecomputed = true;
                            properties.C = c;
                            properties.Light = light;
                            properties.IPoint = iPoint;
                            properties.EyeVector = Npe;
                            properties.LightVector = lightVector;
                            properties.NormalVector = iNormal;
                            properties.Texture = shape.Texture;
                            if (shape is UVInterface)
                            {
                                properties.UVCoordinates = (shape as UVInterface).UVCoordinates(iPoint);
                            }
                            //color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal, c));
                            //color = new Color(color + shape.Shader.ComputeColor(properties));
                        }
                    }

                    //do the reflection here
                    Color diffuseColor = shape.Shader.ComputeColor(properties);
                    Color reflectiveColor = Color.BLACK;
                    if (shape.Shader is Material)
                    {
                        Material material = shape.Shader as Material;
                        if (material.IsReflective)
                        {
                            itr++;
                            float ks = material.Ks;
                            if (itr <= max)
                            {
                                Vector3 _v = -1f * r;
                                Vector3 _n = iNormal;
                                reflectiveColor = new Color((1 - ks) * diffuseColor + ks * RaytraceReflection(ks, max, delta, ref itr, Scene, iPoint, _v, _n, rnd, rndoffsetx, rndoffsety, i, j));
                                if (material.Kiris > 1) //has iris properties
                                {
                                    reflectiveColor = new Color(material.Kiris * reflectiveColor * Irisdescence.Current.ComputeColor(_v, _n));
                                }
                            }
                        }
                    }
                    color = new Color(color + diffuseColor + reflectiveColor);
                }
            }
            return color;
        }
    }
}
