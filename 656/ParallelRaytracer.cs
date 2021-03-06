﻿//using edu.tamu.courses.imagesynth.core;
//using edu.tamu.courses.imagesynth.core.imaging;
//using edu.tamu.courses.imagesynth.core.random;
//using edu.tamu.courses.imagesynth.core.system;
//using edu.tamu.courses.imagesynth.lights;
//using edu.tamu.courses.imagesynth.shapes;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace edu.tamu.courses.imagesynth
//{
//    public class ParallelRaytracer : Raytracer
//    {
//        public override void Raytrace()
//        {
//            randomGeneratorLight = new UniformGenerator(new Range(0, Scene.MSamplePerPixels * Scene.NSamplePerPixels - 1));
//            float max = Scene.Camera.Xmax * Scene.Camera.Ymax;
//            float iteration = 0f;
//            ImageData image = new ImageData((int)Scene.Camera.Xmax, (int)Scene.Camera.Ymax);
//            //for (int I = 0; I < Scene.Camera.Xmax; I++)
//            Parallel.For(0, (int)Scene.Camera.Xmax, I =>
//            {
//                for (int J = 0; J < Scene.Camera.Ymax; J++)
//                {
//                    Color color = Color.BLACK;
//                    float randx = randomGenerator.Next() / (float)Scene.MSamplePerPixels;
//                    float randy = randomGenerator.Next() / (float)Scene.NSamplePerPixels;

//                    float rnd = randomGeneratorLight.Next();
//                    float rndoffsetx = randomGenerator.Next();
//                    float rndoffsety = randomGenerator.Next();

//                    for (int i = 0; i < Scene.MSamplePerPixels; i++)
//                    {
//                        for (int j = 0; j < Scene.NSamplePerPixels; j++)
//                        {
//                            X = I + (i / (float)Scene.MSamplePerPixels) + randx;
//                            Y = J + (j / (float)Scene.NSamplePerPixels) + randy;
//                            float x = X / Scene.Camera.Xmax;
//                            float y = Y / Scene.Camera.Ymax;
//                            Pp = Scene.Camera.P0 + (x * Scene.Camera.Sx * Scene.Camera.N0) + (y * Scene.Camera.Sy * Scene.Camera.N1);
//                            Npe = Pp - Scene.Camera.Pe;
//                            Npe.Normalize();
//                            float t = -1f;
//                            Shape shape = Scene.GetIntersectedShape(Scene.Camera.Pe, Npe, ref t); //Implement the Get Intersected Shape
//                            if (shape != null)
//                            {
//                                Vector3 iPoint = Scene.Camera.Pe + Npe * t; //Intersection point
//                                Vector3 iNormal = shape.NormalAt(iPoint);   //Normal at the intersection

//                                foreach (Light light in Scene.Lights)
//                                {
//                                    //Light light = _light;
//                                    if (light is AreaLight)
//                                    {
//                                        ((AreaLight)light).Rnd = (int)rnd;
//                                        ((AreaLight)light).Ith = i;
//                                        ((AreaLight)light).Jth = j;
//                                        ((AreaLight)light).RndOffsetX = rndoffsetx;
//                                        ((AreaLight)light).RndOffsetY = rndoffsety;
//                                    }

//                                    Vector3 lightVector = light.ComputeLightVector(iPoint);
//                                    float distanceToLight = (light.Position - iPoint).Norm;
//                                    //float distanceToLight = lightVector.Norm;
//                                    lightVector.Normalize();

//                                    //color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal));

//                                    Dictionary<float, Shape> intersectedShapes = Scene.GetIntersectedShapes(iPoint, lightVector, distanceToLight);
//                                    if (intersectedShapes.Count == 0)
//                                    {
//                                        color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal));
//                                    }
//                                    else
//                                    {
//                                        float[] weights = new float[intersectedShapes.Count];
//                                        float[] coefs = new float[intersectedShapes.Count];
//                                        float weight_sum = 0;
//                                        int g = 0;
//                                        foreach (KeyValuePair<float, Shape> iShape in intersectedShapes)
//                                        {
//                                            Vector3 point = iPoint + iShape.Key * lightVector;
//                                            if (shape != iShape.Value) // self intersection
//                                            {
//                                                Vector3 normal = iShape.Value.NormalAt(point);
//                                                weights[g] = (point - light.Position).Norm / distanceToLight;
//                                                weight_sum += weights[g];
//                                                Vector3 nlh = light.Position - point;
//                                                nlh.Normalize();
//                                                float cosTheta = nlh % normal;
//                                                //cosTheta = (cosTheta + 1f) / 2f;
//                                                coefs[g] = cosTheta < 0f ? 0f : cosTheta;
//                                                g++;
//                                            }
//                                        }

//                                        if (weight_sum == 0)
//                                        {
//                                            color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal));
//                                        }
//                                        else
//                                        {
//                                            float c = 1;
//                                            for (int k = 0; k < weights.Length; k++)
//                                            {
//                                                //c *= (float)Math.Pow(coefs[k], weights[k] / weight_sum);
//                                                c *= coefs[k];
//                                            }
//                                            //c = 2f * c;
//                                            color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal, c));
//                                        }
//                                    }

//                                    //get the shader, get the scene lights and compute the color at X, Y
//                                    //if (shape is Plane) color = new Color(0, 1, 0);
//                                    //else color = new Color(1, 1, 0);

//                                }
//                            }
//                        }
//                    }

//                    color = new Color(color / (float)(Scene.MSamplePerPixels * Scene.NSamplePerPixels));
//                    image.SetPixel(I, J, color);
//                    Console.WriteLine("{0:0.00}% Computed...", (iteration / max) * 100f);
//                    iteration++;
//                }
//            });

//            image.SaveToFile(Scene.Name + ".png");
//        }
//    }
//}
