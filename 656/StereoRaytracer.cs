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
    public class StereoRaytracer : Raytracer
    {
        public override void Raytrace()
        {
            StereoCamera stereoCamera = (StereoCamera)Scene.Camera;
            this.Raytrace(stereoCamera.LeftCamera, "left");
            this.Raytrace(stereoCamera.RightCamera, "right");
        }

        protected void Raytrace(Camera camera, String cameraName)
        {
            float max = camera.Xmax * camera.Ymax;
            float iteration = 0f;
            ImageData image = new ImageData((int)camera.Xmax, (int)camera.Ymax);
            randomGeneratorLight = new UniformGenerator(new Range(0, Scene.MSamplePerPixels * Scene.NSamplePerPixels - 1));

            for (int I = 0; I < camera.Xmax; I++)
            {
                for (int J = 0; J < camera.Ymax; J++)
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
                    //float time = 0.0f;
                    //VerticalFall animation = new VerticalFall(0f, -9.8f);
                    //this.ResetAnimation();

                    //randomRefracteddVector = new RandomVector3(randomGenerator);

                    for (int i = 0; i < Scene.MSamplePerPixels; i++)
                    {
                        for (int j = 0; j < Scene.NSamplePerPixels; j++)
                        {
                            X = I + (i / (float)Scene.MSamplePerPixels) + randx;
                            Y = J + (j / (float)Scene.NSamplePerPixels) + randy;
                            float x = X / camera.Xmax;
                            float y = Y / camera.Ymax;
                            Pp = camera.P0 + (x * camera.Sx * camera.N0) + (y * camera.Sy * camera.N1);
                            if (camera is OOFCamera)
                            {
                                ((OOFCamera)camera).Ith = i;
                                ((OOFCamera)camera).Jth = j;
                                ((OOFCamera)camera).RndOffsetX = rndoffsetx;
                                ((OOFCamera)camera).RndOffsetY = rndoffsety;
                                Npe = Pp - (camera as OOFCamera).ComputePe();
                            }
                            else
                            {
                                Npe = Pp - camera.Pe;
                            }
                            Npe.Normalize();
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

                            Shape shape = Scene.GetIntersectedShape(camera.Pe, Npe, ref t, ts); //Implement the Get Intersected Shape

                            if (shape != null)
                            {
                                Vector3 iPoint = camera.Pe + Npe * t; //Intersection point
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

            image.SaveToFile(Scene.Name + "_" + cameraName + ".png");
        }
    }
}
