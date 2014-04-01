using edu.tamu.courses.imagesynth.core;
using edu.tamu.courses.imagesynth.core.imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth
{
    public class Irisdescence
    {
        public static Irisdescence Current = new Irisdescence();
        private UnmanagedImage Image;

        public Irisdescence()
        {
            System.Drawing.Bitmap fileImage = edu.tamu.courses.imagesynth.core.imaging.Image.FromFile("../../data/textures/iris-map-3.png");
            Image = UnmanagedImage.FromManagedImage(fileImage);
        }

        public Color ComputeColor(Vector3 v, Vector3 n)
        {
            int y = Image.Height / 2;
            float costheta = v % n;
            if (costheta <= 0)
            {
                costheta = 0f;
            }
            float d = 1f / (costheta + 1f);
            float u = 2f * d - 1f;
            int x = (int)(u * Image.Width) - 2;
            x = x <= 0 ? 0 : x;
            Color color = new Color(Image.GetPixel(x, y));
            color.PostLoad();
            return color;
        }
    }
}
