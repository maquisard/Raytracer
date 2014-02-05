using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.lights;
using edu.tamu.courses.imagesynth.shaders;
using edu.tamu.courses.imagesynth.shapes;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    public class Scene
    {
        public List<Shape> Shapes { get; private set; }
        public List<Light> Lights { get; private set; }
        public Camera Camera { get; private set; }
        public int MSamplePerPixels { get; private set; }
        public int NSamplePerPixels { get; private set; }
        public String Name { get; private set; }

        public Scene() 
        { 
            this.Shapes = new List<Shape>();
            this.Lights = new List<Light>();
        }

        public void Clear()
        {
            this.Shapes.Clear();
            this.Lights.Clear();
            this.Camera = null;
            this.MSamplePerPixels = -1;
            this.NSamplePerPixels = -1;
        }

        public Shape GetIntersectedShape(Vector3 pe, Vector3 npe, ref float t)
        {
            SortedList<float, int> ts = new SortedList<float, int>();
            for (int i = 0; i < Shapes.Count; i++)
            {
                float _t = Shapes[i].Intersect(pe, npe);
                if (_t >= 0f)
                {
                    ts.Add(_t, i);
                }
            }
            if (ts.Count == 0) return null;
            t = ts.Keys[0];
            return this.Shapes[ts[t]];
        }

        public static Scene LoadFromFile(String filename)
        {
            Scene scene = null;
            using(StreamReader filereader = new StreamReader(filename))
            {
                String rawScene = filereader.ReadToEnd();
                JsonData jsonScene = JsonMapper.ToObject(rawScene);
                scene = new Scene();
                scene.Camera = Camera.CreateFromJson(jsonScene["camera"]);

                //loading all the referenced shaders
                for (int i = 0; i < jsonScene["shaders"].Count; i++)
                {
                    JsonData jsonShader = jsonScene["shaders"][i];
                    ShaderManager.CreateShaderFromJson(jsonShader);
                }

                //locading the shapes -- which will also call the shaders
                for (int i = 0; i < jsonScene["shapes"].Count; i++)
                {
                    JsonData jsonShape = jsonScene["shapes"][i];
                    Shape shape = Shape.CreateFromJson(jsonShape);
                    if (shape is SkySphere)
                    {
                        SkySphere skySphere = shape as SkySphere;
                        float value = skySphere.Evaluate(scene.Camera.Pe);
                        if (value > 0f)
                        {
                            throw new Exception("Blasphemy!!!! - You are out of the world. Please readjust you camera.");
                        }
                    }
                    else if (shape is Sphere)
                    {
                        Sphere sphere = shape as Sphere;
                        float value = sphere.Evaluate(scene.Camera.Pe);
                        if (value < 0f)
                        {
                            throw new Exception("Warning!!!! - You are inside an object. Please readjust you camera.");
                        }
                    }
                    scene.Shapes.Add(shape);
                }

                //loading the lights
                for (int i = 0; i < jsonScene["lights"].Count; i++)
                {
                    JsonData jsonLight = jsonScene["lights"][i];
                    scene.Lights.Add(Light.CreateFromJson(jsonLight));
                }

                //Console.WriteLine(scene.Camera.ToString());
                scene.MSamplePerPixels = int.Parse(jsonScene["sampleperpixel"]["m"].ToString());
                scene.NSamplePerPixels = int.Parse(jsonScene["sampleperpixel"]["n"].ToString());
                scene.Name = jsonScene["name"].ToString();

                Shader.Alpha = float.Parse(jsonScene["alpha"].ToString());

                //handling the sky
            }
            return scene;
        }
    }
}
