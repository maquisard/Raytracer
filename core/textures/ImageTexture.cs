using edu.tamu.courses.imagesynth.core.imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.textures
{
    public class ImageTexture : Texture
    {
        protected UnmanagedImage Image { get; private set; }
        public String FileName { get; set; }
        public override Color ComputeColor(Vector2 uvcoordinates)
        {
            float X = uvcoordinates.X * (float)(Image.Width - 3);
            float Y = uvcoordinates.Y * (float)(Image.Height - 3);
            int Is = (int)Math.Ceiling(X + 0.5f);
            int Js = (int)Math.Ceiling(Y + 0.5f);

            if (Is < 0 || Is > Image.Width || Js <0 || Js > Image.Height)
            {
                Is = Is;
                Js = Js;
            }

            float a11 = (X - Is + 0.5f) * (Y - Js + 0.5f);
            float a10 = (X - Is + 0.5f) * (Js - Y + 0.5f);
            float a01 = (Is - X + 0.5f) * (Y - Js + 0.5f);
            float a00 = (Is - X + 0.5f) * (Js - Y + 0.5f);
            Color color = new Color(
                a00 * GetLocalPixel(Is - 1, Js - 1) +
                a10 * GetLocalPixel(Is, Js - 1) +
                a11 * GetLocalPixel(Is, Js) +
                a01 * GetLocalPixel(Is - 1, Js));

            return color;
        }

        private void LoadFromFile()
        {
            Bitmap fileImage = edu.tamu.courses.imagesynth.core.imaging.Image.FromFile(FileName);
            Image = UnmanagedImage.FromManagedImage(fileImage);
        }

        private Color GetLocalPixel(int i, int j)
        {
            Color color = new Color(Image.GetPixel(i, j));
            color.PostLoad();
            return color;
        }

        public override void PostLoad()
        {
            this.LoadFromFile();
        }
    }
}
