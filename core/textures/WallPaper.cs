using edu.tamu.courses.imagesynth.core.imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.textures
{
    public class WallPaper
    {
        private UnmanagedImage imageItem;
        private Color background;
        public int CountX { get; set; }
        public int CountY { get; set; }

        public WallPaper(String filename)
        {
            imageItem = UnmanagedImage.FromManagedImage(Image.FromFile(filename));
        }

        public void Generate(String filename)
        {
            UnmanagedImage data = UnmanagedImage.Create(CountX * imageItem.Width, CountY * imageItem.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for (int i = 0; i < CountX; i++)
            {
                for (int j = 0; j < CountY; j++)
                {
                    for (int x = 0; x < imageItem.Width; x++)
                    {
                        for (int y = 0; y < imageItem.Height; y++)
                        {

                            data.SetPixel(x + i * imageItem.Width, y + j * imageItem.Height, imageItem.GetPixel(x, y));
                        }
                    }
                }
            }
            data.ToManagedImage().Save(filename);
            data = null;
        }
    }
}
