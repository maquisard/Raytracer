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
    public class GIRaytracer : Raytracer
    {


        public override void Raytrace()
        {
            GeodosicDome dome = new GeodosicDome(Scene.MSamplePerPixels, Scene.NSamplePerPixels);
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
                            Shape shape = Scene.GetIntersectedShape(Pe, Npe, ref t, 0f); //Implement the Get Intersected Shape
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
                                    lightVector.Normalize();

                                    //doing occlusion right here
                                    Color oColor = Color.BLACK;
                                    float occlusion = 0;
                                    List<Vector3> offShoots = dome.GetDirections(iNormal);
                                    foreach (Vector3 direction in offShoots)
                                    {
                                        float to = -1f;
                                        if (iNormal % direction < 0)
                                        {
                                            throw new Exception("Fuck You");
                                        }
                                        Shape oShape = Scene.GetIntersectedShape(iPoint, direction, ref to, 0f);
                                        if (oShape == null)
                                        {
                                            //oColor = new Color(oColor + Color.WHITE); Acting like a sky dome exists and treating this as an intersection with the sky dome
                                            float c = direction % iNormal;
                                            c = c < 0 ? 0 : c;
                                            oColor = new Color(oColor + c * Color.WHITE);

                                        }
                                        else
                                        {
                                            if (shape.Id != oShape.Id)
                                            {
                                                //throw new Exception("Fuck You");
                                                occlusion++;
                                            }
                                        }
                                    }
                                    occlusion /= (float)(offShoots.Count); //average occlusion
                                    oColor = new Color(oColor / (float)(offShoots.Count));

                                    ShaderProperties properties = new ShaderProperties();
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
                                    

                                    Color diffuseColor = shape.Shader.ComputeColor(properties);
                                    diffuseColor = new Color(diffuseColor * (1f - occlusion));
                                    color = new Color(color + diffuseColor);

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

        //public override void Raytrace()
        //{
        //    GeodosicDome dome = new GeodosicDome(Scene.MSamplePerPixels, Scene.NSamplePerPixels);
        //    float max = Scene.Camera.Xmax * Scene.Camera.Ymax;
        //    float iteration = 0f;
        //    ImageData image = new ImageData((int)Scene.Camera.Xmax, (int)Scene.Camera.Ymax);
        //    randomGeneratorLight = new UniformGenerator(new Range(0, Scene.MSamplePerPixels * Scene.NSamplePerPixels - 1));

        //    for (int I = 0; I < Scene.Camera.Xmax; I++)
        //    {
        //        for (int J = 0; J < Scene.Camera.Ymax; J++)
        //        {
        //            Color color = Color.BLACK;
        //            float randx = randomGenerator.Next() / (float)Scene.MSamplePerPixels;
        //            float randy = randomGenerator.Next() / (float)Scene.NSamplePerPixels;

        //            float rnd = randomGeneratorLight.Next();
        //            float rndoffsetx = randomGenerator.Next();
        //            float rndoffsety = randomGenerator.Next();

        //            for (int i = 0; i < Scene.MSamplePerPixels; i++)
        //            {
        //                for (int j = 0; j < Scene.NSamplePerPixels; j++)
        //                {
        //                    X = I + (i / (float)Scene.MSamplePerPixels) + randx;
        //                    Y = J + (j / (float)Scene.NSamplePerPixels) + randy;
        //                    float x = X / Scene.Camera.Xmax;
        //                    float y = Y / Scene.Camera.Ymax;
        //                    Pp = Scene.Camera.P0 + (x * Scene.Camera.Sx * Scene.Camera.N0) + (y * Scene.Camera.Sy * Scene.Camera.N1);
        //                    Pe = Scene.Camera.ComputePe();
        //                    Npe = Pp - Pe;
        //                    if (Scene.Camera is CubistCamera)
        //                    {
        //                        Npe = (Scene.Camera as CubistCamera).ComputePP(Npe);
        //                    }
        //                    Npe.Normalize();

        //                    if (Scene.Camera is OOFCamera)
        //                    {
        //                        ((OOFCamera)Scene.Camera).Ith = i;
        //                        ((OOFCamera)Scene.Camera).Jth = j;
        //                        ((OOFCamera)Scene.Camera).RndOffsetX = rndoffsetx;
        //                        ((OOFCamera)Scene.Camera).RndOffsetY = rndoffsety;
        //                    }

        //                    float t = -1f;
        //                    Shape shape = Scene.GetIntersectedShape(Pe, Npe, ref t, 0f); //Implement the Get Intersected Shape
        //                    inShadow = false;
        //                    if (shape != null)
        //                    {
        //                        Vector3 iPoint = Pe + Npe * t; //Intersection point
        //                        Vector3 iNormal = shape.NormalAt(iPoint);   //Normal at the intersection


        //                        foreach (Light light in Scene.Lights)
        //                        {
        //                            //Light light = _light;
        //                            if (light is AreaLight)
        //                            {
        //                                ((AreaLight)light).Rnd = (int)rnd;
        //                                ((AreaLight)light).Ith = i;
        //                                ((AreaLight)light).Jth = j;
        //                                ((AreaLight)light).RndOffsetX = rndoffsetx;
        //                                ((AreaLight)light).RndOffsetY = rndoffsety;
        //                            }

        //                            Vector3 lightVector = light.ComputeLightVector(iPoint);
        //                            float distanceToLight = (light.Position - iPoint).Norm;
        //                            //float distanceToLight = lightVector.Norm;
        //                            lightVector.Normalize();

                                    
        //                            //doing occlusion right here
        //                            Color oColor = Color.BLACK;
        //                            float occlusion = 0;
        //                            foreach (Vector3 direction in dome.GetDirections(iNormal))
        //                            {
        //                                float to = -1f;
        //                                Shape oShape = Scene.GetIntersectedShape(iPoint, direction, ref to, 0f);
        //                                if (oShape == null)
        //                                {
        //                                    //oColor = new Color(oColor + Color.WHITE);
        //                                    //occlusion--;

        //                                    ShaderProperties props = new ShaderProperties();
        //                                    props.IsCPrecomputed = true;
        //                                    props.C = iNormal % direction;
        //                                    PointLight oLight = new PointLight();
        //                                    oLight.Position = iPoint + direction;
        //                                    oLight.Color = Color.WHITE;
        //                                    props.Light = oLight;
        //                                    props.IPoint = iPoint;
        //                                    props.EyeVector = Npe;
        //                                    props.LightVector = direction;
        //                                    props.NormalVector = iNormal;
        //                                    oColor = new Color(oColor + shape.Shader.ComputeColor(props));

        //                                }
        //                                else
        //                                {
        //                                    if (shape.Id != oShape.Id)
        //                                    {
        //                                        occlusion++;
        //                                        Vector3 oPoint = iPoint + direction * to;
        //                                        Vector3 oNormal = oShape.NormalAt(oPoint);

        //                                        Vector3 oLightVector = light.ComputeLightVector(oPoint);
        //                                        oLightVector.Normalize();

        //                                        ShaderProperties oProps = new ShaderProperties();
        //                                        oProps.IsCPrecomputed = false;
        //                                        oProps.Light = light;
        //                                        oProps.IPoint = oPoint;
        //                                        oProps.EyeVector = direction;
        //                                        oProps.LightVector = oLightVector;
        //                                        oProps.NormalVector = oNormal;
        //                                        oProps.Texture = oShape.Texture;
        //                                        if (oShape is UVInterface)
        //                                        {
        //                                            oProps.UVCoordinates = (oShape as UVInterface).UVCoordinates(oPoint);
        //                                        }
        //                                        //oColor = new Color(oColor + oShape.Shader.ComputeColor(oProps));
        //                                        //float oDiffuse = oNormal % oLightVector;
        //                                    }
        //                                }
        //                            }

        //                            occlusion /= (float)(Scene.MSamplePerPixels * Scene.NSamplePerPixels);
        //                            oColor = new Color(oColor/ (float)(Scene.MSamplePerPixels * Scene.NSamplePerPixels));

        //                            ShaderProperties properties = new ShaderProperties();
        //                            Dictionary<float, Shape> intersectedShapes = Scene.GetIntersectedShapes(iPoint, lightVector, distanceToLight, 0f);
        //                            //if (intersectedShapes.Count == 0 || light is LightProjection)
        //                           // {
        //                                //ShaderProperties properties = new ShaderProperties();
        //                                properties.IsCPrecomputed = false;
        //                                properties.Light = light;
        //                                properties.IPoint = iPoint;
        //                                properties.EyeVector = Npe;
        //                                properties.LightVector = lightVector;
        //                                properties.NormalVector = iNormal;
        //                                properties.Texture = shape.Texture;
        //                                if (shape is UVInterface)
        //                                {
        //                                    properties.UVCoordinates = (shape as UVInterface).UVCoordinates(iPoint);
        //                                }
        //                                //color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal));
        //                                //color = new Color(color + shape.Shader.ComputeColor(properties));
        //                            /*}
        //                            else
        //                            {
        //                                float[] weights = new float[intersectedShapes.Count];
        //                                float[] coefs = new float[intersectedShapes.Count];
        //                                float weight_sum = 0;
        //                                int g = 0;
        //                                foreach (KeyValuePair<float, Shape> iShape in intersectedShapes)
        //                                {
        //                                    Vector3 point = iPoint + iShape.Key * lightVector;
        //                                    if (shape != iShape.Value) // self intersection
        //                                    {
        //                                        Vector3 normal;
        //                                        if (iShape.Value.NormalMap != null)
        //                                        {
        //                                            normal = iShape.Value.RealNormalAt(point);
        //                                        }
        //                                        else
        //                                        {
        //                                            normal = iShape.Value.NormalAt(point);
        //                                        }
        //                                        weights[g] = (point - light.Position).Norm / distanceToLight;
        //                                        weight_sum += weights[g];
        //                                        Vector3 nlh = light.Position - point;
        //                                        nlh.Normalize();
        //                                        float cosTheta = nlh % normal;
        //                                        //cosTheta = (cosTheta + 1f);
        //                                        coefs[g] = cosTheta < 0f ? 0f : cosTheta;
        //                                        g++;
        //                                    }
        //                                }

        //                                if (weight_sum == 0)
        //                                {
        //                                    //ShaderProperties properties = new ShaderProperties();
        //                                    properties.IsCPrecomputed = false;
        //                                    properties.Light = light;
        //                                    properties.IPoint = iPoint;
        //                                    properties.EyeVector = Npe;
        //                                    properties.LightVector = lightVector;
        //                                    properties.NormalVector = iNormal;
        //                                    properties.Texture = shape.Texture;
        //                                    if (shape is UVInterface)
        //                                    {
        //                                        properties.UVCoordinates = (shape as UVInterface).UVCoordinates(iPoint);
        //                                    }
        //                                    //color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal));
        //                                    //color = new Color(color + shape.Shader.ComputeColor(properties));
        //                                }
        //                                else
        //                                {
        //                                    float c = 1;
        //                                    for (int k = 0; k < weights.Length; k++)
        //                                    {
        //                                        //c *= (float)Math.Pow(coefs[k], weights[k] / weight_sum);
        //                                        c *= coefs[k];
        //                                    }
        //                                    //c = 2f * c;
        //                                    //ShaderProperties properties = new ShaderProperties();
        //                                    properties.IsCPrecomputed = true;
        //                                    properties.C = c;
        //                                    properties.Light = light;
        //                                    properties.IPoint = iPoint;
        //                                    properties.EyeVector = Npe;
        //                                    properties.LightVector = lightVector;
        //                                    properties.NormalVector = iNormal;
        //                                    properties.Texture = shape.Texture;
        //                                    if (shape is UVInterface)
        //                                    {
        //                                        properties.UVCoordinates = (shape as UVInterface).UVCoordinates(iPoint);
        //                                    }
        //                                    //color = new Color(color + shape.Shader.ComputeColor(light, iPoint, Npe, lightVector, iNormal, c));
        //                                    //color = new Color(color + shape.Shader.ComputeColor(properties));
        //                                }
        //                            }*/
        //                            //do the reflection here

        //                            Color diffuseColor = shape.Shader.ComputeColor(properties);
        //                            diffuseColor = new Color(diffuseColor * (1f - occlusion) * Scene.OcclusionAmount + diffuseColor * (1f - Scene.OcclusionAmount));
        //                            //diffuseColor = new Color(diffuseColor * (1f - occlusion) + occlusion * oColor);
        //                            //diffuseColor = new Color((1f - Scene.OcclusionAmount) * diffuseColor + (occlusion * Scene.OcclusionAmount / (float)(Scene.MSamplePerPixels * Scene.NSamplePerPixels)) * diffuseColor);

        //                            Color kraColor = TextureManager.GetTexture("marble").ComputeColor(properties.UVCoordinates, properties.IPoint);
        //                            if (shape.Shader is Material)
        //                            {
        //                                Material material = shape.Shader as Material;
        //                                Color reflectionColor = Color.BLACK;
        //                                Color refractionColor = Color.BLACK;
        //                                if (material.IsReflective)
        //                                {
        //                                    int itr = 1;
        //                                    int maxRecursions = 4;
        //                                    float ki = material.Kr;
        //                                    Vector3 v = -1f * Npe;
        //                                    if (material.IsGlossy)
        //                                    {
        //                                        v += new RandomVector3(randomGenerator);
        //                                        v.Normalize();
        //                                    }
        //                                    Vector3 n = iNormal;
        //                                    reflectionColor = new Color((1f - ki) * diffuseColor + ki * RaytraceReflection(ki, maxRecursions, ref itr, Scene, iPoint, v, n, rnd, rndoffsetx, rndoffsety, i, j, 0f));
        //                                    if (material.Kiris > 1) //has iris properties
        //                                    {
        //                                        reflectionColor = new Color(material.Kiris * reflectionColor * Irisdescence.Current.ComputeColor(v, n));
        //                                    }
        //                                }
        //                                if (material.IsRefractive)
        //                                {
        //                                    int itr = 1;
        //                                    int maxRecursions = 4;
        //                                    Vector3 v = -1f * Npe;
        //                                    if (material.IsTranslucent)
        //                                    {
        //                                        v += new RandomVector3(randomGenerator);
        //                                        //v += randomRefracteddVector;
        //                                        v.Normalize();
        //                                    }
        //                                    Vector3 n = iNormal;
        //                                    float kra = material.Kra;
        //                                    //float kra = RefractIndex(kraColor);
        //                                    float kt = material.Kt;
        //                                    refractionColor = new Color(kt * RaytraceRefraction(kra, kt, ref itr, maxRecursions, Scene, iPoint, v, n, rnd, rndoffsetx, rndoffsety, i, j, 0f));
        //                                    reflectionColor = new Color(refractionColor + (1f - kt) * reflectionColor);
        //                                }

        //                                color = new Color(color + reflectionColor);

        //                            }
        //                            else
        //                            {
        //                                color = new Color(color + diffuseColor);
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //            color = new Color(color / (float)(Scene.MSamplePerPixels * Scene.NSamplePerPixels));
        //            image.SetPixel(I, J, color);
        //            Console.WriteLine("{0:0.00}% Computed...", (iteration / max) * 100f);
        //            iteration++;
        //        }
        //    }

        //    image.SaveToFile(Scene.Name + ".png");
        //}
    }
}
