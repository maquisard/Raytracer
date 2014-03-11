using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.textures
{
    public class MarbleTexture : Texture
    {
        public float Frequency { get; set; }
        public float Scale { get; set; }
        public int Octaves { get; set; }
        public float StripesPerUnit { get; set; }

        public Color Color0 { get; set; }
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }

        private SolidNoise Noise;

        public MarbleTexture()
        {
            Noise = new SolidNoise();
            Color0 = new Color(0.8f, 0.0f, 0.0f);
            Color1 = new Color(0.0f, 0.0f, 0.8f);
            Color2 = new Color(0.8f, 0.8f, 0.00f);
        }

        public override void PostLoad()
        {
            Frequency = (float)Math.PI * StripesPerUnit;
        }

        public override Color ComputeColor(Vector uvcoordinates, Vector3 iPoint)
        {
            Vector2 uv = uvcoordinates as Vector2;
            float temp = Scale * Noise.Turbulence(Frequency * iPoint, Octaves);
            float t = 2.0f * (float)Math.Abs(Math.Sin(Frequency * iPoint.X + temp));

            if (t < 1.0f)
            {
                return new Color(Color1 * t + (1f - t) * Color2);
            }
            else
            {
                t -= 1f;
                return new Color(Color0 * t + (1f - t) * Color1);
            }
        }
    }
}
