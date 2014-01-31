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
                        if (!skySphere.Contains(scene.Camera.Pe))
                        {
                            throw new Exception("Blasphemy!!!! - You are out of the world. Please readjust you camera.");
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

                Console.WriteLine(scene.Camera.ToString());
                scene.MSamplePerPixels = int.Parse(jsonScene["sampleperpixel"]["m"].ToString());
                scene.NSamplePerPixels = int.Parse(jsonScene["sampleperpixel"]["n"].ToString());

                Shader.Alpha = float.Parse(jsonScene["alpha"].ToString());

                //handling the sky
            }
            return scene;
        }
    }
}
