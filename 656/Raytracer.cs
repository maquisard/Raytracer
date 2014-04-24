using edu.tamu.courses.imagesynth.Animations;
using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.core;
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
        public Vector3 Pe { get; protected set; }

        protected UniformOneGenerator randomGenerator = new UniformOneGenerator();
        protected UniformGenerator randomGeneratorLight;



        public virtual void Raytrace()
        {
            float max = Scene.Camera.Xmax * Scene.Camera.Ymax;
            float iteration = 0f;
            ImageData image = new ImageData((int)Scene.Camera.Xmax, (int)Scene.Camera.Ymax);
            randomGeneratorLight = new UniformGenerator(new Range(0, Scene.MSamplePerPixels * Scene.NSamplePerPixels - 1));

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

                    float totalSamples = Scene.MSamplePerPixels * Scene.NSamplePerPixels;
                    //UniformGenerator randomTime = new UniformGenerator(new Range(tmin, tmax));
                    //float randt = randomTime.Next();
                    float time = 0.0f;
                    VerticalFall animation = new VerticalFall(0f, -9.8f);
                    //this.ResetAnimation();

                    //randomRefracteddVector = new RandomVector3(randomGenerator);

                    for (int i = 0; i < Scene.MSamplePerPixels; i++)
                    {
                        for (int j = 0; j < Scene.NSamplePerPixels; j++)
                        {
                            X = I + (i / (float)Scene.MSamplePerPixels) + randx;
                            Y = J + (j / (float)Scene.NSamplePerPixels) + randy;
                            float x = X / Scene.Camera.Xmax;
                            float y = Y / Scene.Camera.Ymax;
                            Pp = Scene.Camera.P0 + (x * Scene.Camera.Sx * Scene.Camera.N0) + (y * Scene.Camera.Sy * Scene.Camera.N1);
                            Pe = Scene.Camera.ComputePe();
                            Npe = Pp - Pe;
                            if (Scene.Camera is CubistCamera)
                            {
                                Npe = (Scene.Camera as CubistCamera).ComputePP(Npe);
                            }
                            Npe.Normalize();

                            if (Scene.Camera is OOFCamera)
                            {
                                ((OOFCamera)Scene.Camera).Ith = i;
                                ((OOFCamera)Scene.Camera).Jth = j;
                                ((OOFCamera)Scene.Camera).RndOffsetX = rndoffsetx;
                                ((OOFCamera)Scene.Camera).RndOffsetY = rndoffsety;
                            }
                            float t = -1f;
                            
                            //doing the animation thing right here.
                            //float ts = ((I * Scene.Camera.Xmax + J) / max) + randt;
                            float ts = 0;
                            //time = ((i * Scene.NSamplePerPixels + j) / totalSamples);
                            //if (i < Scene.MSamplePerPixels - 1 && j < Scene.NSamplePerPixels - 1)
                            //{
                            //    this.UpdateScene(time, animation);
                            //    time += 0.001f;
                            //}

                            Shape shape = Scene.GetIntersectedShape(Pe, Npe, ref t, ts); //Implement the Get Intersected Shape

                            if (shape != null)
                            {
                                Vector3 iPoint = Pe + Npe * t; //Intersection point
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

                                    Dictionary<float, Shape> intersectedShapes = Scene.GetIntersectedShapes(iPoint, lightVector, distanceToLight, ts);
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
                                    Color kraColor = TextureManager.GetTexture("marble").ComputeColor(properties.UVCoordinates, properties.IPoint);
                                    if (shape.Shader is Material)
                                    {
                                        Material material = shape.Shader as Material;
                                        Color reflectionColor = Color.BLACK;
                                        Color refractionColor = Color.BLACK;
                                        if (material.IsReflective)
                                        {
                                            int itr = 1;
                                            int maxRecursions = 4;
                                            float ki = material.Kr;
                                            Vector3 v = -1f * Npe;
                                            if (material.IsGlossy)
                                            {
                                                v += new RandomVector3(randomGenerator);
                                                v.Normalize();
                                            }
                                            Vector3 n = iNormal;
                                            reflectionColor = new Color((1f - ki) * diffuseColor + ki * RaytraceReflection(ki, maxRecursions, ref itr, Scene, iPoint, v, n, rnd, rndoffsetx, rndoffsety, i, j, ts));
                                            if (material.Kiris > 1) //has iris properties
                                            {
                                                reflectionColor = new Color(material.Kiris * reflectionColor * Irisdescence.Current.ComputeColor(v, n));
                                            }
                                        }
                                        if (material.IsRefractive)
                                        {
                                            int itr = 1;
                                            int maxRecursions = 4;
                                            Vector3 v = -1f * Npe;
                                            if (material.IsTranslucent)
                                            {
                                                v += new RandomVector3(randomGenerator);
                                                //v += randomRefracteddVector;
                                                v.Normalize();
                                            }
                                            Vector3 n = iNormal;
                                            float kra = material.Kra;
                                            //float kra = RefractIndex(kraColor);
                                            float kt = material.Kt;
                                            refractionColor = new Color(kt * RaytraceRefraction(kra, kt, ref itr, maxRecursions, Scene, iPoint, v, n, rnd, rndoffsetx, rndoffsety, i, j, ts));
                                            reflectionColor = new Color(refractionColor + (1f - kt) * reflectionColor);
                                        }

                                        color = new Color(color + reflectionColor);

                                    }
                                    else
                                    {
                                        color = new Color(color + diffuseColor);
                                    }

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

        protected Color RaytraceRefraction(float kra, float kt, ref int itr, int max, Scene Scene, Vector3 p, Vector3 v, Vector3 n, float rnd, float rndoffsetx, float rndoffsety, int i, int j, float time)
        {
            Color color = Color.BLACK;
            float t = -1f;
            float C = v % n;
            kra = C < 0 ? 1 / kra : kra; 
            
            float term = ((C * C - 1f) / (kra * kra)) + 1f;
            if (term < 0)
            {
                itr++;
                if (itr <= max)
                {
                    //return color;
                    //Color reflectionColor = Color.BLACK;
                    int _itr = 1;
                    int _maxRecursions = 4;
                    float ki = 1f - kt;
                    return new Color(ki * RaytraceReflection(ki, max, ref itr, Scene, p, v, n, rnd, rndoffsetx, rndoffsety, i, j, time));
                }
                return color;
            }

            Vector3 r = (-1f / kra) * v + ((C / kra) - (float)Math.Sqrt(term)) * n;
            r.Normalize();

            Shape shape = Scene.GetIntersectedShape(p, r, ref t, time); //Implement the Get Intersected Shape
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

                    Dictionary<float, Shape> intersectedShapes = Scene.GetIntersectedShapes(iPoint, lightVector, distanceToLight, time);
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
                    Color kraColor = TextureManager.GetTexture("marble").ComputeColor(properties.UVCoordinates, properties.IPoint);
                    if (shape.Shader is Material)
                    {
                        itr++;
                        Material material = shape.Shader as Material;
                        Color reflectiveColor = Color.BLACK;
                        Color refractiveColor = Color.BLACK;
                        if (material.IsReflective)
                        {
                            int _itr = 1;
                            int _maxRecursions = 4;
                            if (itr <= max)
                            {
                                float ks = material.Kr;
                                Vector3 _v = -1f * r;
                                if (material.IsGlossy)
                                {
                                    _v += new RandomVector3(randomGenerator);
                                    _v.Normalize();
                                }
                                Vector3 _n = iNormal;
                                reflectiveColor = new Color((1f - ks) * diffuseColor + ks * RaytraceReflection(ks, max, ref itr, Scene, iPoint, _v, _n, rnd, rndoffsetx, rndoffsety, i, j, time));
                            }
                        }
                        if (material.IsRefractive)
                        {
                            if (itr <= max)
                            {
                                Vector3 _v = -1f * r;
                                if (material.IsTranslucent)
                                {
                                    _v += new RandomVector3(randomGenerator);
                                    //_v += randomRefracteddVector;
                                    _v.Normalize();
                                }
                                Vector3 _n = iNormal;
                                float _kra = material.Kra;
                                //float _kra = RefractIndex(kraColor);
                                float _kt = material.Kt;
                                refractiveColor = new Color(_kt * RaytraceRefraction(_kra, _kt, ref itr, max, Scene, iPoint, v, n, rnd, rndoffsetx, rndoffsety, i, j, time));
                                reflectiveColor = new Color(refractiveColor + (1f - _kt) * reflectiveColor);
                            }
                        }
                        color = new Color(color + reflectiveColor);
                    }
                    else
                    {
                        color = new Color(color + diffuseColor);
                    }
                }
            }
            return color;
        }


        protected Color RaytraceReflection(float ki, float max, ref int itr, Scene Scene, Vector3 p, Vector3 v, Vector3 n, float rnd, float rndoffsetx, float rndoffsety, int i, int j, float time)
        {
            Color color = Color.BLACK;
            Vector3 r = (-1f * v) + 2f * (v % n) * n;
            r.Normalize();
            float t = -1f;
            Shape shape = Scene.GetIntersectedShape(p, r, ref t, time); //Implement the Get Intersected Shape
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

                    Dictionary<float, Shape> intersectedShapes = Scene.GetIntersectedShapes(iPoint, lightVector, distanceToLight, time);
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
                    Color kraColor = TextureManager.GetTexture("marble").ComputeColor(properties.UVCoordinates, properties.IPoint);
                    if (shape.Shader is Material)
                    {
                        Material material = shape.Shader as Material;
                        Color reflectiveColor = Color.BLACK;
                        Color refractiveColor = Color.BLACK;
                        itr++;
                        if (material.IsReflective)
                        {
                            float ks = material.Kr;
                            if (itr <= max)
                            {
                                Vector3 _v = -1f * r;
                                if (material.IsGlossy)
                                {
                                    _v += new RandomVector3(randomGenerator);
                                    _v.Normalize();
                                }
                                Vector3 _n = iNormal;
                                reflectiveColor = new Color((1f - ks) * diffuseColor + ks * RaytraceReflection(ks, max, ref itr, Scene, iPoint, _v, _n, rnd, rndoffsetx, rndoffsety, i, j, time));
                                if (material.Kiris > 1f) //has iris properties
                                {
                                    reflectiveColor = new Color(material.Kiris * reflectiveColor * Irisdescence.Current.ComputeColor(_v, _n));
                                }
                            }
                        }
                        if (material.IsRefractive)
                        {
                            if (itr <= max)
                            {
                                Vector3 _v = -1f * r;
                                if (material.IsTranslucent)
                                {
                                    //_v += randomRefracteddVector;
                                    _v += new RandomVector3(randomGenerator);
                                    _v.Normalize();
                                }
                                Vector3 _n = iNormal;
                                float _kra = material.Kra;
                                //float _kra = RefractIndex(kraColor);
                                float _kt = material.Kt;
                                refractiveColor = new Color(_kt * RaytraceRefraction(_kra, _kt, ref itr, (int)max, Scene, iPoint, v, n, rnd, rndoffsetx, rndoffsety, i, j, time));
                                reflectiveColor = new Color(refractiveColor + (1f - _kt) * reflectiveColor);
                            }
                        }
                        color = new Color(color + reflectiveColor);
                    }
                    else
                    {
                        color = new Color(color + diffuseColor);
                    }
                }
            }
            return color;
        }

        protected float RefractIndex(Color color)
        {
            return IsBlue(color) ? 0.85f : IsRed(color) ? 0.45f : IsYellow(color) ? 0.65f : 0.95f;
        }

        protected bool IsBlue(Color color) { return Proximate(color, new Color(0f, 0f, 1f)); }
        protected bool IsRed(Color color) { return Proximate(color, new Color(1f, 0f, 0f)); }
        protected bool IsYellow(Color color) { return Proximate(color, new Color(1f, 1f, 0f)); }


        protected bool Proximate(Color color1, Color color2)
        {
            return (color1 - color2).Norm <= 0.5f;
        }

        protected void UpdateScene(float t, Animation<float> animation)
        {
            Shape animated = null;
            foreach (Shape shape in Scene.Shapes)
            {
                if (shape.Id == 0)
                {
                    animated = shape;
                    break;
                }
            }
            animation.Update(animated, "Center.Y", t);
        }

        protected void ResetAnimation()
        {
            foreach (Shape shape in Scene.Shapes)
            {
                if (shape.Id == 0)
                {
                    (shape as Sphere).Center.Y = -4f;
                    break;
                }
            }
        }

    }
}
