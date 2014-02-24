using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.textures;
using edu.tamu.courses.imagesynth.lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.shaders
{
    public class ShaderProperties
    {
        public Light Light { get; set; }
        public Vector3 IPoint { get; set; }
        public Vector3 EyeVector { get; set; } //Npe
        public Vector3 LightVector { get; set; } //Nlh from intersection to light
        public Vector3 NormalVector { get; set; } //Nh, the normal at the intersection point
        public float C { get; set; } //cosine
        public bool IsCPrecomputed { get; set; } //
        public Texture Texture { get; set; }
        public Vector2 UVCoordinates { get; set; }
    }
}
